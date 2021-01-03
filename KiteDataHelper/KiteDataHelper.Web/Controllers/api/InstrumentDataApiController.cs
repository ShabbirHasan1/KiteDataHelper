using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Common.KiteStructures;
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

        public InstrumentDataApiController(ILogger<CandleDataApiController> logger, IConfiguration configuration, IMarketDataAccessService marketDataAccessService,
            IMemoryCache cache)
        {
            _logger = logger;
            _configuration = configuration;
            _marketDataAccessService = marketDataAccessService;
            _cache = cache;
        }

        // GET: api/<InstrumentDataApiController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Instrument> instruments = null;

            try
            {
                instruments = await _marketDataAccessService.GetInstrumentsList();
            }
            catch (IntelliTradeException ex)
            {
                _logger.LogError($"An error occurred while getting instruments list. {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting instruments list. {ex.Message}");
            }

            if (instruments == null)
                return new StatusCodeResult(500);
            else
                return Ok(instruments);
        }

        // GET api/<InstrumentDataApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
