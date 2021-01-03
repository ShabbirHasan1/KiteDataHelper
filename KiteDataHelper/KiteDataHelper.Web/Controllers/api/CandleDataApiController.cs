using KiteDataHelper.Common.Enums;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Common.Models;
using KiteDataHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntelliTrade.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandleDataApiController : ControllerBase
    {
        private readonly ILogger<CandleDataApiController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMarketDataAccessService _marketDataAccessService;
        private readonly IMemoryCache _cache;

        public CandleDataApiController(ILogger<CandleDataApiController> logger, IConfiguration configuration, IMarketDataAccessService marketDataAccessService,
            IMemoryCache cache)
        {
            _logger = logger;
            _configuration = configuration;
            _marketDataAccessService = marketDataAccessService;
            _cache = cache;
        }

        // GET: api/<CandleDataApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CandleDataApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] uint instrumentToken, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            List<CandleUnit> candleUnits = null;
            List<KiteCandleUnit> kiteCandleUnits = null;
            if (endDate == null)
            {
                endDate = DateTime.Now;
                endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 15, 30, 00);
                startDate = endDate.Value.AddMonths(-1);
                startDate = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 09, 15, 00);
            }

            try
            {
                switch (id)
                {
                    case (int)Interval.FifteenMinute:
                        kiteCandleUnits = await _marketDataAccessService.GetData(instrumentToken, Interval.FifteenMinute, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                        break;
                    default:
                        kiteCandleUnits = null;
                        break;
                }
            }
            catch (IntelliTradeException ex)
            {
                _logger.LogError($"An error occurred while getting OHLC data. {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting OHLC data. {ex.Message}");
            }

            if (kiteCandleUnits.Count > 0)
            {
                candleUnits = new List<CandleUnit>();
                foreach (KiteCandleUnit row in kiteCandleUnits)
                {
                    try
                    {
                        CandleUnit candleUnit = new CandleUnit();
                        candleUnit.time = (row.time.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        candleUnit.open = row.open.Value;
                        candleUnit.high = row.high.Value;
                        candleUnit.low = row.low.Value;
                        candleUnit.close = row.close.Value;
                        candleUnit.volume = row.volume.Value;
                        candleUnits.Add(candleUnit);
                    }
                    catch (IntelliTradeException ex)
                    {
                        _logger.LogError($"An error occurred while parsing OHLC data. {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"An error occurred while parsing OHLC data. {ex.Message}");
                    }
                }
            }

            if (candleUnits == null)
            {
                return new StatusCodeResult(500);
            }
            else
                return Ok(candleUnits);
        }

        // POST api/<CandleDataApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CandleDataApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CandleDataApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
