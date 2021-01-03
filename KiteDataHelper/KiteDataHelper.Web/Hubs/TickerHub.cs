using KiteDataHelper.Common.KiteStructures;
using KiteDataHelper.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KiteDataHelper.Web.Hubs
{
    public class TickerHub : Hub
    {
        private readonly ILogger<TickerHub> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly Ticker _ticker;
        private readonly TickCache _tickCache;

        public TickerHub(ILogger<TickerHub> logger,
            IConfiguration configuration,
            IMemoryCache cache, Ticker ticker, TickCache tickCache)
        {
            _logger = logger;
            _configuration = configuration;
            _ticker = ticker;
            _ticker.OnConnect += this.OnTickerConnect;
            _ticker.OnClose += this.OnTickerClose;
            _ticker.OnError += this.OnTickerError;
            _ticker.OnTick += this.OnTick;
            this._cache = cache;
            this._tickCache = tickCache;
        }

        public void CloseTicks()
        {
            _ticker.OnConnect -= this.OnTickerConnect;
            _ticker.OnError -= this.OnTickerError;
            _ticker.OnTick -= this.OnTick;
            _ticker.Close();
        }

        public void StartTicks(string instrumentToken)
        {
            _ticker.Connect();
            uint[] tokens = new uint[] { 260105 };
            _ticker.Subscribe(tokens);
            _ticker.SetMode(tokens, "full");
        }

        private void OnTickerConnect()
        {
            this._logger.LogInformation("Connection success!");
        }

        private void OnTickerClose()
        {
            this._logger.LogInformation("Connection successfully closed.");
            _ticker.OnClose -= this.OnTickerClose;
        }

        private void OnTick(Tick tick, IHubContext<TickerHub> hubContext)
        {
            
        }

        private void OnTickerError(string message)
        {
            this._logger.LogError(message);
        }
    }
}
