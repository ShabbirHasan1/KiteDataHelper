using KiteDataHelper.Common.Exceptions;
using KiteDataHelper.Common.Models;
using NotVisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace KiteDataHelper.Common
{
    public static class CommonFunctions
    {
        public static long GetObjectSize(object TestObject)
        {
            byte[] arr;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, TestObject);
                arr = ms.ToArray();
            }

            return arr.Length;
        }

        public static string ComputeSha256Hash(string Data)
        {
            SHA256Managed sha256 = new SHA256Managed();
            StringBuilder hexhash = new StringBuilder();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data), 0, Encoding.UTF8.GetByteCount(Data));
            foreach (byte b in hash)
            {
                hexhash.Append(b.ToString("x2"));
            }
            return hexhash.ToString();
        }

        public static DateTime? StringToDate(string DateString)
        {
            if (String.IsNullOrEmpty(DateString))
                return null;

            try
            {
                if (DateString.Length == 10)
                {
                    return DateTime.ParseExact(DateString, "yyyy-MM-dd", null);
                }
                else
                {
                    return DateTime.ParseExact(DateString, "yyyy-MM-dd HH:mm:ss", null);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Serialize C# object to JSON string.
        /// </summary>
        /// <param name="obj">C# object to serialize.</param>
        /// <returns>JSON string/</returns>
        public static string JsonSerialize(object obj)
        {
            string json = JsonSerializer.Serialize(obj);
            MatchCollection mc = Regex.Matches(json, @"\\/Date\((\d*?)\)\\/");
            foreach (Match m in mc)
            {
                UInt64 unix = UInt64.Parse(m.Groups[1].Value) / 1000;
                json = json.Replace(m.Groups[0].Value, UnixToDateTime(unix).ToString());
            }
            return json;
        }

        /// <summary>
        /// Deserialize Json string to nested string dictionary.
        /// </summary>
        /// <param name="Json">Json string to deserialize.</param>
        /// <returns>Json in the form of nested string dictionary.</returns>
        public static Dictionary<string, dynamic> JsonDeserialize(string Json)
        {
            JsonElement elm = JsonSerializer.Deserialize<JsonElement>(Json);
            // Replace double with decimal in the map
            Dictionary<string, dynamic> dict = CommonFunctions.ElementToDict(elm);
            return dict;
        }

        /// <summary>
        /// Recursively traverses an object and converts JsonElement objects to corresponding primitives.
        /// </summary>
        /// <param name="obj">Input JsonElement object.</param>
        /// <returns>Object with primitives</returns>
        public static dynamic ElementToDict(JsonElement obj)
        {
            if (obj.ValueKind == JsonValueKind.Number)
            {
                return StringToDecimal(obj.GetRawText());
            }
            else if (obj.ValueKind == JsonValueKind.String)
            {
                return obj.GetString();
            }
            else if (obj.ValueKind == JsonValueKind.True || obj.ValueKind == JsonValueKind.False)
            {
                return obj.GetBoolean();
            }
            else if (obj.ValueKind == JsonValueKind.Object)
            {
                var map = obj.EnumerateObject().ToList();
                var newMap = new Dictionary<String, dynamic>();
                for (int i = 0; i < map.Count; i++)
                {
                    newMap.Add(map[i].Name, ElementToDict(map[i].Value));
                }
                return newMap;
            }
            else if (obj.ValueKind == JsonValueKind.Array)
            {
                var items = obj.EnumerateArray().ToList();
                var newItems = new ArrayList();
                for (int i = 0; i < items.Count; i++)
                {
                    newItems.Add(ElementToDict(obj[i]));
                }
                return newItems;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts string to decimal. Handles culture and scientific notations.
        /// </summary>
        /// <param name="value">Input string.</param>
        /// <returns>Decimal value</returns>
        public static decimal StringToDecimal(String value)
        {
            return decimal.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parse instruments API's CSV response.
        /// </summary>
        /// <param name="Data">Response of instruments API.</param>
        /// <returns>CSV data as array of nested string dictionary.</returns>
        public static List<Dictionary<string, dynamic>> ParseCSV(string Data)
        {
            string[] lines = Data.Split('\n');

            List<Dictionary<string, dynamic>> instruments = new List<Dictionary<string, dynamic>>();

            using (var parser = new CsvTextFieldParser(StreamFromString(Data)))
            {
                // parser.CommentTokens = new string[] { "#" };
                // parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;

                // Skip over header line.
                string[] headers = parser.ReadFields();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                    for (var i = 0; i < headers.Length; i++)
                        item.Add(headers[i], fields[i]);

                    instruments.Add(item);
                }
            }

            return instruments;
        }

        /// <summary>
        /// Wraps a string inside a stream
        /// </summary>
        /// <param name="value">string data</param>
        /// <returns>Stream that reads input string</returns>
        public static MemoryStream StreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        /// <summary>
        /// Helper function to add parameter to the request only if it is not null or empty
        /// </summary>
        /// <param name="Params">Dictionary to add the key-value pair</param>
        /// <param name="Key">Key of the parameter</param>
        /// <param name="Value">Value of the parameter</param>
        public static void AddIfNotNull(Dictionary<string, dynamic> Params, string Key, string Value)
        {
            if (!String.IsNullOrEmpty(Value))
                Params.Add(Key, Value);
        }

        /// <summary>
        /// Creates key=value with url encoded value
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        /// <returns>Combined string</returns>
        public static string BuildParam(string Key, dynamic Value)
        {
            if (Value is string)
            {
                return HttpUtility.UrlEncode(Key) + "=" + HttpUtility.UrlEncode((string)Value);
            }
            else
            {
                string[] values = (string[])Value;
                return String.Join("&", values.Select(x => HttpUtility.UrlEncode(Key) + "=" + HttpUtility.UrlEncode(x)));
            }
        }

        /// <summary>
        /// Converts Unix timestamp to DateTime
        /// </summary>
        /// <param name="unixTimeStamp">Unix timestamp in seconds.</param>
        /// <returns>DateTime object.</returns>
        public static DateTime UnixToDateTime(UInt64 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 5, 30, 0, 0, DateTimeKind.Unspecified);
            dateTime = dateTime.AddSeconds(unixTimeStamp);
            return dateTime;
        }

        public static string GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public static T GetValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }

        public static decimal FindNearestStrike(decimal currentLevel)
        {
            decimal decimalPart;
            decimal upperLevel;
            decimal lowerLevel;
            try
            {
                decimalPart = currentLevel - Math.Truncate(currentLevel);
                decimalPart = 1 - decimalPart;
                currentLevel = currentLevel + decimalPart;
                upperLevel = currentLevel;
                lowerLevel = currentLevel;

                while (!((upperLevel % 100) == 0))
                {
                    upperLevel += Convert.ToDecimal(1);
                }

                while (!((lowerLevel % 100) == 0))
                {
                    lowerLevel -= Convert.ToDecimal(1);
                }
            }
            catch (Exception ex)
            {
                throw new IntelliTradeException("An error occurred while calculating strike price.", ex);
            }

            decimal upperDiff = upperLevel - currentLevel;
            decimal lowerDiff = currentLevel - lowerLevel;
            if (upperDiff > lowerDiff)
                return lowerLevel;
            else
                return upperLevel;
        }

        public static double ToUnixEpoch(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static string ToCsvString(this List<KiteCandleUnit> dataFrame)
        {
            string retVal = string.Empty;
            using (Stream stream = new MemoryStream())
            {
                StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.WriteLine($"time,open,high,low,close,volume");
                try
                {
                    foreach (KiteCandleUnit row in dataFrame)
                    {
                        try
                        {
                            string time = row.time.ToString();
                            string open = row.open.ToString();
                            string high = row.high.ToString();
                            string low = row.low.ToString();
                            string close = row.close.ToString();
                            string volume = row.volume.ToString();
                            streamWriter.WriteLine($"{time},{open},{high},{low},{close},{volume}");
                        }
                        catch (Exception ex)
                        {
                            throw new IntelliTradeException("An error occurred while parsing columns for CSV.", ex);
                        }
                    }
                    streamWriter.Flush();
                    stream.Position = 0;
                    StreamReader reader = new StreamReader(stream);
                    retVal = reader.ReadToEnd();
                    streamWriter.Close();
                    reader.Close();
                }
                catch (IntelliTradeException ex)
                {
                    throw ex;
                }
            }

            return retVal;
        }
    }
}
