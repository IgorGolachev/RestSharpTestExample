using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace core.Util
{
    public sealed class FileHelper
    {
        public string ReadJsonStringFromFile(string filePath)
        {
            try
            {
                return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), filePath));
            }
            catch (FileNotFoundException e)
            {
                Logger.Log.Info(e.Message);
                return string.Empty;
            }
        }

        public Dictionary<string, string> GetTestDataFromPlainJsonFile(string filePath)
        {
            string data = ReadJsonStringFromFile(filePath);
            Dictionary<string, string> locationData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            return locationData;
        }

        public Dictionary<string, string> GetJsonSectionFromFile(string filePath, string jsonPath)
        {
            string data = ReadJsonStringFromFile(filePath);
            JObject j = JObject.Parse(data);
            data = j.SelectToken(jsonPath).ToString(Formatting.None);
            Dictionary<string, string> locationData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            return locationData;
        }
    }
}