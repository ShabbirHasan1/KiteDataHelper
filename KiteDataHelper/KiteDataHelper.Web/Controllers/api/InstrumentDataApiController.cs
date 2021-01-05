using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KiteDataHelper.Common;
using KiteDataHelper.Common.Enums;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Common.KiteStructures;
using KiteDataHelper.Common.Models;
using KiteDataHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntelliTrade.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentDataApiController : ControllerBase
    {
        private readonly ILogger<CandleDataApiController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMarketDataAccessService _marketDataAccessService;
        private readonly IMemoryCache _cache;
        private readonly KiteInstruments _kiteInstruments;

        public InstrumentDataApiController(ILogger<CandleDataApiController> logger, IConfiguration configuration, IMarketDataAccessService marketDataAccessService,
            IMemoryCache cache, KiteInstruments kiteInstruments)
        {
            _logger = logger;
            _configuration = configuration;
            _marketDataAccessService = marketDataAccessService;
            _cache = cache;
            _kiteInstruments = kiteInstruments;
        }

        // GET api/<InstrumentDataApiController>/5
        [HttpGet]
        public IActionResult Get([FromQuery]string id)
        {
            IEnumerable<Instrument> instruments = null;
            List<string> tradingSymbols = null;

            if (_kiteInstruments.IsSet)
            {
                instruments = _kiteInstruments.Instruments.Where(obj => obj.TradingSymbol.ToLower().Contains(id) || obj.Name.ToLower().Contains(id));
            }

            if (instruments == null)
                return new StatusCodeResult(404);
            else
            {
                tradingSymbols = new List<string>();
                foreach (Instrument instrument in instruments)
                {
                    tradingSymbols.Add(instrument.TradingSymbol);
                }

                return Ok(tradingSymbols);
            }
        }

        // POST api/<InstrumentDataApiController>
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] KiteDataRequest dataRequest)
        {
            HttpResponseMessage result = null;
            List<KiteCandleUnit> kiteCandleUnits = null;
            Instrument ? instrument = _kiteInstruments.Instruments.Where(obj => string.Compare(obj.TradingSymbol, dataRequest.TradingSymbol) == 0).FirstOrDefault();
            if (instrument == null)
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
                result.Content = new StringContent("Script not found.");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/text");
                return result;
            }

            uint instrumentToken = instrument.Value.InstrumentToken;
            Interval interval = dataRequest.Interval.GetValueFromDescription<Interval>();
            string[] fromParts = dataRequest.From.Split('-');
            string[] toParts = dataRequest.To.Split('-');
            DateTime from = new DateTime(Convert.ToInt32(fromParts[2]), Convert.ToInt32(fromParts[0]), Convert.ToInt32(fromParts[1]));
            DateTime to = new DateTime(Convert.ToInt32(toParts[2]), Convert.ToInt32(toParts[0]), Convert.ToInt32(toParts[1]));
            TimeSpan totalDays = to.Subtract(from);
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
                            newTo = from.AddDays(30);
                        }
                        else
                        {
                            from = newTo;
                            newTo = from.AddDays(30);
                        }

                        if (kiteCandleUnits == null)
                            kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, interval, from, newTo);
                        else
                        {
                            List<KiteCandleUnit> kites = await _marketDataAccessService.GetData(instrumentToken, interval, from, newTo);
                            kiteCandleUnits.AddRange(kites);
                        }
                    }
                }
                else
                {
                    if (loopCount < 1)
                    {
                        kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, interval, from, to);
                    }
                    else
                    {
                        DateTime newTo = DateTime.MinValue;
                        for (int i = 0; i < loopTimes; i++)
                        {
                            if (i == 0)
                            {
                                newTo = from.AddDays(30);
                            }
                            else
                            {
                                from = newTo;
                                newTo = from.AddDays(30);
                            }

                            if (kiteCandleUnits == null)
                                kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, interval, from, newTo);
                            else
                            {
                                List<KiteCandleUnit> kites = await _marketDataAccessService.GetData(instrumentToken, interval, from, newTo);
                                kiteCandleUnits.AddRange(kites);
                            }
                        }

                        from = newTo;
                        newTo = from.AddDays(to.Subtract(from).Days);
                        List<KiteCandleUnit> lastKites = await _marketDataAccessService.GetData(instrumentToken, interval, from, newTo);
                        kiteCandleUnits.AddRange(lastKites);
                    }
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(kiteCandleUnits.ToCsvString());
                    writer.Flush();
                    stream.Position = 0;

                    result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = $"{dataRequest.TradingSymbol}.csv" };
                }
            }

            return result;
        }

        // PUT api/<InstrumentDataApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InstrumentDataApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
