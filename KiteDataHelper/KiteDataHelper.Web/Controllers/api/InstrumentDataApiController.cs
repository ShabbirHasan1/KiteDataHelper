using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Common.KiteStructures;
using KiteDataHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        public void Post([FromBody] string value)
        {
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
