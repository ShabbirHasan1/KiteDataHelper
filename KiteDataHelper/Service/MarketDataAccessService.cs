using KiteDataHelper.Common;
using KiteDataHelper.Common.Enums;
using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Interfaces.Services;
using KiteDataHelper.Common.KiteStructures;
using KiteDataHelper.Common.Models;
using KiteDataHelper.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NotVisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KiteDataHelper.Service
{
    public class MarketDataAccessService : IMarketDataAccessService
    {
        private readonly ILogger<MarketDataAccessService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;
        private readonly KiteInstruments _kiteInstruments;

        public MarketDataAccessService(ILogger<MarketDataAccessService> logger, HttpClient httpClient, IConfiguration configuration, IMemoryCache cache, KiteInstruments kiteInstruments)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
            _kiteInstruments = kiteInstruments;
        }

        public async Task<List<Instrument>> GetInstrumentsList()
        {
            List<Instrument> instruments = null;
            Stream cashResponseStream = null;
            Stream nfoResponseStream = null;

            if (!_kiteInstruments.IsSet)
            {
                KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot;
                bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);

                if (cacheFetchResult)
                {
                    try
                    {
                        string cashInstrumentFetchUrl = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteInstrumentDataBaseUrl")}{this._configuration.GetValue<string>("kiteCashInstrumentDataUrl")}";
                        string nfoInstrumentFetchUrl = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteInstrumentDataBaseUrl")}{this._configuration.GetValue<string>("kiteNfoInstrumentDataUrl")}";
                        string kiteApiKey = this._configuration.GetValue<string>("kiteApiKey");
                        this._httpClient.DefaultRequestHeaders.Add("X-Kite-Version", "3");
                        this._httpClient.DefaultRequestHeaders.Add("Authorization", $"token {kiteApiKey}:{kiteAccessTokenResponseRoot.data.access_token}");
                        HttpResponseMessage cash_instrument_response = await this._httpClient.GetAsync(cashInstrumentFetchUrl);
                        HttpResponseMessage nfo_instrument_response = await this._httpClient.GetAsync(nfoInstrumentFetchUrl);

                        if (cash_instrument_response.IsSuccessStatusCode && nfo_instrument_response.IsSuccessStatusCode)
                        {
                            cashResponseStream = await cash_instrument_response.Content.ReadAsStreamAsync();
                            nfoResponseStream = await nfo_instrument_response.Content.ReadAsStreamAsync();

                            CsvTextFieldParser csvTextFieldParser = new CsvTextFieldParser(cashResponseStream);
                            CsvTextFieldParser nfoTextFieldParser = new CsvTextFieldParser(nfoResponseStream);
                            instruments = new List<Instrument>();
                            string[] headers = csvTextFieldParser.ReadFields();
                            while (!csvTextFieldParser.EndOfData)
                            {
                                try
                                {
                                    Dictionary<string, dynamic> rowData = new Dictionary<string, dynamic>();
                                    string[] allFields = csvTextFieldParser.ReadFields();
                                    rowData.Add(headers[0], allFields[0]);
                                    rowData.Add(headers[1], allFields[1]);
                                    rowData.Add(headers[2], allFields[2]);
                                    rowData.Add(headers[3], allFields[3]);
                                    rowData.Add(headers[4], allFields[4]);
                                    rowData.Add(headers[5], allFields[5]);
                                    rowData.Add(headers[6], allFields[6]);
                                    rowData.Add(headers[7], allFields[7]);
                                    rowData.Add(headers[8], allFields[8]);
                                    rowData.Add(headers[9], allFields[9]);
                                    rowData.Add(headers[10], allFields[10]);
                                    rowData.Add(headers[11], allFields[11]);
                                    instruments.Add(new Instrument(rowData));
                                }
                                catch (CsvMalformedLineException ex)
                                {
                                    this._logger.LogError(ex.Message);
                                    this._logger.LogCritical(ex.StackTrace);
                                }
                                catch (Exception ex)
                                {
                                    this._logger.LogError(ex.Message);
                                    this._logger.LogCritical(ex.StackTrace);
                                }
                            }

                            headers = nfoTextFieldParser.ReadFields();
                            while (!nfoTextFieldParser.EndOfData)
                            {
                                try
                                {
                                    Dictionary<string, dynamic> rowData = new Dictionary<string, dynamic>();
                                    string[] allFields = nfoTextFieldParser.ReadFields();
                                    rowData.Add(headers[0], allFields[0]);
                                    rowData.Add(headers[1], allFields[1]);
                                    rowData.Add(headers[2], allFields[2]);
                                    rowData.Add(headers[3], allFields[3]);
                                    rowData.Add(headers[4], allFields[4]);
                                    rowData.Add(headers[5], allFields[5]);
                                    rowData.Add(headers[6], allFields[6]);
                                    rowData.Add(headers[7], allFields[7]);
                                    rowData.Add(headers[8], allFields[8]);
                                    rowData.Add(headers[9], allFields[9]);
                                    rowData.Add(headers[10], allFields[10]);
                                    rowData.Add(headers[11], allFields[11]);
                                    instruments.Add(new Instrument(rowData));
                                }
                                catch (CsvMalformedLineException ex)
                                {
                                    this._logger.LogError(ex.Message);
                                    this._logger.LogCritical(ex.StackTrace);
                                }
                                catch (Exception ex)
                                {
                                    this._logger.LogError(ex.Message);
                                    this._logger.LogCritical(ex.StackTrace);
                                }
                            }

                            _kiteInstruments.Instruments = instruments;
                            _kiteInstruments.IsSet = true;

                        }
                        else
                        {
                            this._logger.LogError(cash_instrument_response.ReasonPhrase);
                            this._logger.LogError(nfo_instrument_response.ReasonPhrase);
                        }
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex.Message);
                        throw new IntelliTradeException("An error occurred while trying to get instruments for the day.", ex);
                    }
                    finally
                    {
                        cashResponseStream.Close();
                        nfoResponseStream.Close();
                    }
                }
            }
            else
            {
                instruments = _kiteInstruments.Instruments;
            }
            
            return instruments;
        }
        /*
        public async Task<DataFrame> GetData(uint instrumentToken, Interval granularity, DateTime startDate, DateTime endDate)
        {
            KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot;
            KiteCandleResponse kiteCandleResponse;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);
            
            PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("timestamp");
            DecimalDataFrameColumn open = new DecimalDataFrameColumn("open");
            DecimalDataFrameColumn high = new DecimalDataFrameColumn("high");
            DecimalDataFrameColumn low = new DecimalDataFrameColumn("low");
            DecimalDataFrameColumn close = new DecimalDataFrameColumn("close");
            DecimalDataFrameColumn volume = new DecimalDataFrameColumn("volume");
            DataFrame df = null;
            Type[] frameTypes = new Type[] { typeof(string), typeof(decimal), typeof(decimal), typeof(decimal), typeof(decimal), typeof(decimal) };
            string[] colNames = new string[] { "timestamp","open","high","low","close","volume" };
            if (cacheFetchResult)
            {
                try
                {
                    string effectiveStartDay = startDate.Day < 10 ? $"0{startDate.Day.ToString()}" : startDate.Day.ToString();
                    string effectiveEndDay = endDate.Day < 10 ? $"0{endDate.Day.ToString()}" : endDate.Day.ToString();
                    string effectiveStartMonth = startDate.Month < 10 ? $"0{startDate.Month.ToString()}" : startDate.Month.ToString();
                    string effectiveEndMonth = endDate.Month < 10 ? $"0{endDate.Month.ToString()}" : endDate.Month.ToString();

                    string from = $"from={startDate.Year.ToString()}-{effectiveStartMonth}-{effectiveStartDay}+09:15:00";
                    string to = $"to={endDate.Year.ToString()}-{effectiveEndMonth}-{effectiveEndDay}+15:30:00";
                    string instrumentFetchUrl = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteHistoricalDataUrl")}/{instrumentToken.ToString()}/{granularity.GetDescription()}?{from}&{to}";
                    string kiteApiKey = this._configuration.GetValue<string>("kiteApiKey");

                    if(!this._httpClient.DefaultRequestHeaders.Contains("X-Kite-Version"))
                        this._httpClient.DefaultRequestHeaders.Add("X-Kite-Version", "3");

                    if (!this._httpClient.DefaultRequestHeaders.Contains("Authorization"))
                        this._httpClient.DefaultRequestHeaders.Add("Authorization", $"token {kiteApiKey}:{kiteAccessTokenResponseRoot.data.access_token}");

                    HttpResponseMessage historical_data_response = await this._httpClient.GetAsync(instrumentFetchUrl);

                    if (historical_data_response.IsSuccessStatusCode)
                    {
                        Stream responseStream = await historical_data_response.Content.ReadAsStreamAsync();
                        var respose = new StreamReader(responseStream).ReadToEnd();
                        kiteCandleResponse = JsonSerializer.Deserialize<KiteCandleResponse>(respose);
                        string fileName = $".\\{Guid.NewGuid()}.csv";
                        if (string.Compare(kiteCandleResponse.status, "success") == 0)
                        {
                            using (Stream stream = new FileStream(fileName, FileMode.CreateNew))
                            {
                                StreamWriter streamWriter = new StreamWriter(stream);
                                streamWriter.WriteLine($"timestamp,open,high,low,close,volume");
                                foreach (ArrayList arrayList in kiteCandleResponse.data.candles)
                                {
                                    streamWriter.WriteLine($"{arrayList[0]},{arrayList[1]},{arrayList[2]},{arrayList[3]},{arrayList[4]},{arrayList[5]}");
                                }
                                streamWriter.Close();
                            }

                            using (Stream readStream = new FileStream(fileName, FileMode.Open))
                            {
                                df = DataFrame.LoadCsv(readStream, separator: ',', header: true, columnNames: colNames, dataTypes: new Type[] { typeof(string), typeof(decimal), typeof(decimal), typeof(decimal), typeof(decimal), typeof(decimal) });
                            }

                            File.Delete(fileName);
                        }
                    }
                    else
                    {
                        this._logger.LogError(historical_data_response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogCritical(ex.Message);
                }
            }

            return df;
        }*/

        public async Task<List<KiteCandleUnit>> GetData(uint instrumentToken, Interval granularity, DateTime startDate, DateTime endDate)
        {
            KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot;
            KiteCandleResponse kiteCandleResponse;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);
            List<KiteCandleUnit> kiteCandles = null;
            
            if (cacheFetchResult)
            {
                try
                {
                    string effectiveStartDay = startDate.Day < 10 ? $"0{startDate.Day.ToString()}" : startDate.Day.ToString();
                    string effectiveEndDay = endDate.Day < 10 ? $"0{endDate.Day.ToString()}" : endDate.Day.ToString();
                    string effectiveStartMonth = startDate.Month < 10 ? $"0{startDate.Month.ToString()}" : startDate.Month.ToString();
                    string effectiveEndMonth = endDate.Month < 10 ? $"0{endDate.Month.ToString()}" : endDate.Month.ToString();

                    string from = $"from={startDate.Year.ToString()}-{effectiveStartMonth}-{effectiveStartDay}+09:15:00";
                    string to = $"to={endDate.Year.ToString()}-{effectiveEndMonth}-{effectiveEndDay}+15:30:00";
                    string instrumentFetchUrl = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteHistoricalDataUrl")}/{instrumentToken.ToString()}/{granularity.GetDescription()}?{from}&{to}";
                    string kiteApiKey = this._configuration.GetValue<string>("kiteApiKey");

                    if (!this._httpClient.DefaultRequestHeaders.Contains("X-Kite-Version"))
                        this._httpClient.DefaultRequestHeaders.Add("X-Kite-Version", "3");

                    if (!this._httpClient.DefaultRequestHeaders.Contains("Authorization"))
                        this._httpClient.DefaultRequestHeaders.Add("Authorization", $"token {kiteApiKey}:{kiteAccessTokenResponseRoot.data.access_token}");

                    HttpResponseMessage historical_data_response = await this._httpClient.GetAsync(instrumentFetchUrl);

                    if (historical_data_response.IsSuccessStatusCode)
                    {
                        Stream responseStream = await historical_data_response.Content.ReadAsStreamAsync();
                        var response = new StreamReader(responseStream).ReadToEnd();
                        kiteCandleResponse = JsonSerializer.Deserialize<KiteCandleResponse>(response);
                        string fileName = $".\\{Guid.NewGuid()}.csv";
                        if (string.Compare(kiteCandleResponse.status, "success") == 0)
                        {
                            kiteCandles = new List<KiteCandleUnit>();
                            foreach (ArrayList arrayList in kiteCandleResponse.data.candles)
                            {
                                KiteCandleUnit kiteCandleUnit = new KiteCandleUnit();
                                DateTime timestamp;
                                decimal open;
                                decimal high;
                                decimal low;
                                decimal close;
                                long volume;
                                bool timestampresult = DateTime.TryParse(arrayList[0].ToString(), out timestamp);
                                bool openresult = Decimal.TryParse(arrayList[1].ToString(), out open);
                                bool highresult = Decimal.TryParse(arrayList[2].ToString(), out high);
                                bool lowresult = Decimal.TryParse(arrayList[3].ToString(), out low);
                                bool closeresult = Decimal.TryParse(arrayList[4].ToString(), out close);
                                bool volumeresult = long.TryParse(arrayList[5].ToString(), out volume);
                                kiteCandleUnit.time = timestamp;
                                kiteCandleUnit.open = open;
                                kiteCandleUnit.high = high;
                                kiteCandleUnit.low = low;
                                kiteCandleUnit.close = close;
                                kiteCandleUnit.volume = volume;
                                kiteCandles.Add(kiteCandleUnit);
                            }
                            responseStream.Close();
                        }
                    }
                    else
                    {
                        this._logger.LogError(historical_data_response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogCritical(ex.Message);
                    this._logger.LogCritical(ex.StackTrace);
                }
            }

            return kiteCandles;
        }

        public async Task<decimal> GetLtp(string query)
        {
            decimal ltp = decimal.MinValue;
            bool parsed = false;
            KiteAccessTokenResponseRoot kiteAccessTokenResponseRoot;
            bool cacheFetchResult = this._cache.TryGetValue<KiteAccessTokenResponseRoot>("kite_access_token", out kiteAccessTokenResponseRoot);
            if (cacheFetchResult)
            {
                try
                {
                    string ltpFetchUrl = $"{this._configuration.GetValue<string>("kiteApiBaseUrl")}{this._configuration.GetValue<string>("kiteLtpUrl")}?i={query}";
                    string kiteApiKey = this._configuration.GetValue<string>("kiteApiKey");

                    if (!this._httpClient.DefaultRequestHeaders.Contains("X-Kite-Version"))
                        this._httpClient.DefaultRequestHeaders.Add("X-Kite-Version", "3");

                    if (!this._httpClient.DefaultRequestHeaders.Contains("Authorization"))
                        this._httpClient.DefaultRequestHeaders.Add("Authorization", $"token {kiteApiKey}:{kiteAccessTokenResponseRoot.data.access_token}");

                    HttpResponseMessage ltp_response = await this._httpClient.GetAsync(ltpFetchUrl);
                    string response = string.Empty;
                    if (ltp_response.IsSuccessStatusCode)
                    {
                        using (Stream responseStream = await ltp_response.Content.ReadAsStreamAsync())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                response = reader.ReadToEnd();
                            }
                        }

                        JObject lastPriceResponse = JObject.Parse(response);
                        var lastprice = from p in lastPriceResponse["data"][query] select (string)p;
                        
                        if (lastprice != null)
                            parsed = decimal.TryParse(lastprice.LastOrDefault(), out ltp);

                        if(!parsed)
                            throw new IntelliTradeException("An error occurred while parsing last traded price.");
                    }
                    else
                    {
                        throw new IntelliTradeException("An error occurred while getting last traded price.");
                    }
                }
                catch (IntelliTradeException ex)
                {
                    this._logger.LogCritical(ex.Message);
                    this._logger.LogCritical(ex.StackTrace);
                    throw ex;
                }
                catch (Exception ex)
                {
                    this._logger.LogCritical(ex.Message);
                    this._logger.LogCritical(ex.StackTrace);
                }
            }

            return ltp;
        }
    }
}
