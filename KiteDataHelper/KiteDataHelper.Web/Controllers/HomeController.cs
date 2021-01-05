using KiteDataHelper.Common.Enums;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Common.KiteStructures;
using KiteDataHelper.Common.Models;
using KiteDataHelper.Models;
using KiteDataHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using KiteDataHelper.Common;
namespace KiteDataHelper.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IKiteAuthenticationService _kiteAuthenticationService;
        private readonly IConfiguration _configuration;
        private readonly IMarketDataAccessService _marketDataAccessService;
        private readonly IMemoryCache _cache;
        private readonly KiteInstruments _kiteInstruments;
        private readonly FifteenMinuteTimer _fifteenMinuteTimer;

        public HomeController(ILogger<HomeController> logger, IKiteAuthenticationService kiteAuthenticationService,
            IConfiguration configuration, IMarketDataAccessService marketDataAccessService,
            IMemoryCache cache, KiteInstruments kiteInstruments, FifteenMinuteTimer fifteenMinuteTimer)
        {
            _logger = logger;
            _kiteAuthenticationService = kiteAuthenticationService;
            _configuration = configuration;
            _marketDataAccessService = marketDataAccessService;
            this._cache = cache;
            _fifteenMinuteTimer = fifteenMinuteTimer;
            _kiteInstruments = kiteInstruments;
        }
        public async Task<IActionResult> KiteLoginRedirect(string request_token, string status)
        {
            try
            {
                await this._kiteAuthenticationService.Login(request_token);
            }
            catch (IntelliTradeException ex)
            {
                this._logger.LogError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (await this._kiteAuthenticationService.IsAuthenticated())
                {
                    ViewBag.IsAuthenticated = true;
                    await this._marketDataAccessService.GetInstrumentsList();
                }
                else
                {
                    ViewBag.IsAuthenticated = false;
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex.Message);
            }

            return View();
        }

        public async Task<IActionResult> Login()
        {
            if (!await this._kiteAuthenticationService.IsAuthenticated())
            {
                return Redirect(String.Format(this._configuration.GetValue<string>("kiteLoginUrl"), this._configuration.GetValue<string>("kiteApiKey")));
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (await this._kiteAuthenticationService.IsAuthenticated())
            {
                //_ticker.Close();
                ViewBag.IsAuthenticated = false;
                await this._kiteAuthenticationService.Logout();
            }

            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<Instrument>> GetInstruments()
        {
            List<Instrument> instruments = null;
            instruments = await _marketDataAccessService.GetInstrumentsList();
            return instruments;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> StartDataTimers()
        {
            _fifteenMinuteTimer.FifteenMinTimer.Start();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadData(string tradingSymbol, string interval, string from, string to)
        {
            List<KiteCandleUnit> kiteCandleUnits = null;
            Instrument? instrument = _kiteInstruments.Instruments.Where(obj => string.Compare(obj.TradingSymbol, tradingSymbol) == 0).FirstOrDefault();
            if (instrument == null)
            {
                return NotFound();
            }

            uint instrumentToken = instrument.Value.InstrumentToken;
            Interval lag = interval.GetValueFromDescription<Interval>();
            string[] fromParts = from.Split('/');
            string[] toParts = to.Split('/');
            DateTime dateFrom = new DateTime(Convert.ToInt32(fromParts[2]), Convert.ToInt32(fromParts[0]), Convert.ToInt32(fromParts[1]));
            DateTime dateTo = new DateTime(Convert.ToInt32(toParts[2]), Convert.ToInt32(toParts[0]), Convert.ToInt32(toParts[1]));
            TimeSpan totalDays = dateTo.Subtract(dateFrom);
            if (totalDays.Days > 30)
            {
                float loopCount = (float)totalDays.Days / 30;
                int loopTimes = totalDays.Days / 30;
                if (Convert.ToDouble(loopTimes) == loopCount)
                {
                    DateTime newTo = DateTime.MinValue;
                    for (int i = 0; i < loopTimes; i++)
                    {
                        if (i == 0)
                        {
                            newTo = dateFrom.AddDays(30);
                        }
                        else
                        {
                            dateFrom = newTo;
                            newTo = dateFrom.AddDays(30);
                        }

                        if (kiteCandleUnits == null)
                            kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, lag, dateFrom, newTo);
                        else
                        {
                            List<KiteCandleUnit> kites = await _marketDataAccessService.GetData(instrumentToken, lag, dateFrom, newTo);
                            kiteCandleUnits.AddRange(kites);
                        }
                    }
                }
                else
                {
                    DateTime newTo = DateTime.MinValue;
                    for (int i = 0; i < loopTimes; i++)
                    {
                        if (i == 0)
                        {
                            newTo = dateFrom.AddDays(30);
                        }
                        else
                        {
                            dateFrom = newTo;
                            newTo = dateFrom.AddDays(30);
                        }

                        if (kiteCandleUnits == null)
                            kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, lag, dateFrom, newTo);
                        else
                        {
                            List<KiteCandleUnit> kites = await _marketDataAccessService.GetData(instrumentToken, lag, dateFrom, newTo);
                            kiteCandleUnits.AddRange(kites);
                        }
                    }

                    dateFrom = newTo;
                    newTo = dateFrom.AddDays(dateTo.Subtract(dateFrom).Days);
                    List<KiteCandleUnit> lastKites = await _marketDataAccessService.GetData(instrumentToken, lag, dateFrom, newTo);
                    kiteCandleUnits.AddRange(lastKites);
                }
            }
            else
            {
                kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, lag, dateFrom, dateTo);
            }


            string csv = kiteCandleUnits.ToCsvString();
            Encoding ascii = Encoding.ASCII;
            byte[] bytes = ascii.GetBytes(csv);
            return File(bytes, "text/csv", $"{tradingSymbol}.csv");
        }
    }
}
