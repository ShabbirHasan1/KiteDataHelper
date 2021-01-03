using KiteDataHelper.Common;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KiteDataHelper.Service
{
    public class KiteAuthenticationService : IKiteAuthenticationService
    {
        private readonly ILogger<KiteAuthenticationService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;

        public KiteAuthenticationService(ILogger<KiteAuthenticationService> logger, HttpClient httpClient, IConfiguration configuration, IMemoryCache cache)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task Login(string request_token)
        {
            string tokenExchangeEndpoint = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteAccessTokenUrl")}";
            KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot = null;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);

            if (!cacheFetchResult)
            {
                try
                {
                    string accessTokenRequest = $"{this._configuration.GetValue<string>("kiteApiKey")}{request_token}{this._configuration.GetValue<string>("kiteApiSecret")}";
                    string accessTokenRequestHash = CommonFunctions.ComputeSha256Hash(accessTokenRequest);
                    var param = new Dictionary<string, dynamic>
                    {
                        {"api_key", this._configuration.GetValue<string>("kiteApiKey")},
                        {"request_token", request_token},
                        {"checksum", accessTokenRequestHash}
                    };

                    string paramString = String.Join("&", param.Select(x => CommonFunctions.BuildParam(x.Key, x.Value)));
                    HttpContent accessTokenRequestContent = new StringContent(paramString, Encoding.UTF8, "application/x-www-form-urlencoded");
                    accessTokenRequestContent.Headers.Add("X-Kite-Version", "3");
                    HttpResponseMessage access_token_response = await this._httpClient.PostAsync(tokenExchangeEndpoint, accessTokenRequestContent);

                    if (access_token_response.IsSuccessStatusCode)
                    {
                        Stream responseStream = await access_token_response.Content.ReadAsStreamAsync();
                        var respose = new StreamReader(responseStream).ReadToEnd();
                        kiteAccessTokenResponseRoot = JsonSerializer.Deserialize<KiteAccessTokenResponseRoot>(respose);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
                        cacheOptions.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(8));
                        cacheOptions.Priority = CacheItemPriority.NeverRemove;
                        cacheOptions.RegisterPostEvictionCallback(new PostEvictionDelegate(this.CacheEvictionCallback));
                        cacheOptions.SetSize(CommonFunctions.GetObjectSize(kiteAccessTokenResponseRoot));
                        
                        using (ICacheEntry cacheEntry = this._cache.CreateEntry((object)"kite_access_token"))
                        {
                            cacheEntry.SetSize(CommonFunctions.GetObjectSize(kiteAccessTokenResponseRoot));
                            cacheEntry.SetOptions(cacheOptions);
                            cacheEntry.SetValue(kiteAccessTokenResponseRoot);
                        }
                    }
                    else
                    {
                        this._logger.LogError(access_token_response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex.Message);
                    throw new IntelliTradeException("An error occurred while trying to get access token from Kite.", ex);
                }
            }
        }

        public async Task Logout()
        {
            KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot = null;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);
            if ((kiteAccessTokenResponseRoot != null) && cacheFetchResult)
            {
                this._cache.Remove("kite_access_token");
                var param = new Dictionary<string, dynamic>();
                CommonFunctions.AddIfNotNull(param, "api_key", this._configuration.GetValue<string>("kiteApiKey"));
                CommonFunctions.AddIfNotNull(param, "access_token", (string)kiteAccessTokenResponseRoot.data.access_token);
                string encodedParmas = string.Empty;
                foreach (KeyValuePair<string, dynamic> keyValuePair in param)
                {
                    if(string.IsNullOrEmpty(encodedParmas))
                        encodedParmas = CommonFunctions.BuildParam(keyValuePair.Key, keyValuePair.Value);
                    else
                        encodedParmas = encodedParmas + "&" + CommonFunctions.BuildParam(keyValuePair.Key, keyValuePair.Value);
                }
                
                string kiteLogoutUrl = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteAccessTokenUrl")}?{encodedParmas}";
                this._httpClient.DefaultRequestHeaders.Add("X-Kite-Version", "3");
                HttpResponseMessage httpResponseMessage = await this._httpClient.DeleteAsync(kiteLogoutUrl);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Logout from Kite API Successful.");
                }
                else
                {
                    this._logger.LogError("Logout from Kite API Unsuccessful.");
                }
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot = null;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);
            if ((kiteAccessTokenResponseRoot != null) && cacheFetchResult)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task RefreshKiteAccessToken()
        {
            /*var param = new Dictionary<string, dynamic>();
            KiteAccessTokemResponseRoot kiteAccessTokemResponseRoot = null;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokemResponseRoot>("kite_access_token", out kiteAccessTokemResponseRoot);

            if ((kiteAccessTokemResponseRoot != null) && cacheFetchResult)
            {
                string checksum = CommonFunctions.ComputeSha256Hash($"{this._configuration.GetValue<string>("kiteApiKey")}{(string)kiteAccessTokemResponseRoot.data.refresh_token}{this._configuration.GetValue<string>("kiteApiSecret")}");
                CommonFunctions.AddIfNotNull(param, "api_key", this._configuration.GetValue<string>("kiteApiKey"));
                CommonFunctions.AddIfNotNull(param, "refresh_token", (string)kiteAccessTokemResponseRoot.data.refresh_token);
                CommonFunctions.AddIfNotNull(param, "checksum", checksum);
                string tokenRefreshEndpoint = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteAccessTokenRefreshUrl")}";
                string paramString = String.Join("&", param.Select(x => CommonFunctions.BuildParam(x.Key, x.Value)));
                HttpContent accessTokenRefreshContent = new StringContent(paramString, Encoding.UTF8, "application/x-www-form-urlencoded");
                accessTokenRefreshContent.Headers.Add("X-Kite-Version", "3");

                HttpResponseMessage refresh_token_response = await this._httpClient.PostAsync(tokenRefreshEndpoint, accessTokenRefreshContent);
                TokenSet tokenSet;

                if (refresh_token_response.IsSuccessStatusCode)
                {
                    Stream responseStream = await refresh_token_response.Content.ReadAsStreamAsync();
                    var response = new StreamReader(responseStream).ReadToEnd();
                    tokenSet = JsonSerializer.Deserialize<TokenSet>(response);
                    kiteAccessTokemResponseRoot.data.access_token = tokenSet.AccessToken;
                    kiteAccessTokemResponseRoot.data.refresh_token = tokenSet.RefreshToken;
                    this._cache.Set<KiteAccessTokemResponseRoot>("kite_access_token", kiteAccessTokemResponseRoot);
                }
                else
                {
                    throw new IntelliTradeException("kite_access_token object of type KiteAccessTokemResponseRoot not found in cache.");
                }
            }*/

            throw new NotImplementedException();

        }

        private void CacheEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            if (EvictionReason.Expired == reason)
            {
                KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot = (KiteAccessTokenResponseRoot)value;
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
                cacheOptions.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(8));
                cacheOptions.Priority = CacheItemPriority.NeverRemove;
                cacheOptions.RegisterPostEvictionCallback(new PostEvictionDelegate(this.CacheEvictionCallback));
                cacheOptions.SetSize(CommonFunctions.GetObjectSize(kiteAccessTokenResponseRoot));

                using (ICacheEntry cacheEntry = this._cache.CreateEntry((object)"kite_access_token"))
                {
                    cacheEntry.SetSize(CommonFunctions.GetObjectSize(kiteAccessTokenResponseRoot));
                    cacheEntry.SetOptions(cacheOptions);
                    cacheEntry.SetValue(kiteAccessTokenResponseRoot);
                }
            }
        }
    }
}
