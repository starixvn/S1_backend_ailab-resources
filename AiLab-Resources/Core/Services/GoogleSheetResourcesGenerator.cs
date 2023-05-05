using AiLab_Resources.Core.Interfaces;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using ICSharpCode.Decompiler.Util;
using AiLab_Resources.Core.Models;

namespace AiLab_Resources.Core.Services
{
    public class GoogleSheetResourcesGenerator : IResourcesGenerator
    {
        /// <summary>
        /// 產生語系檔
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sheetNames"></param>
        /// <param name="prefix">專案名稱_</param>
        /// <param name="outPutFolder">輸出資料夾名稱</param>
        public void ResourcesGeneratorResx(GoogleSheetConfig config, List<string> sheetNames,string prefix,string outPutFolder)
        {
            Console.WriteLine($"{prefix.Replace("_","")} ResourcesGenerator Start");

            var service = new SheetsService(new BaseClientService.Initializer
            {
                ApplicationName = config.ApplicationName,
                ApiKey = config.GoogleSheetApiKey,
            });

            var sheetId = config.GoogleSheetInfo.SheetId;

            foreach (var sheetName in sheetNames)
            {
                var sheetRequest = service.Spreadsheets.Values.Get(sheetId, sheetName);
                var sheet = sheetRequest.Execute();
                var sheetObjectValue = sheet.Values;
                
                if (sheetObjectValue is { Count: > 0 })
                {
                    var titleRow = sheetObjectValue.First();
                    var langIdx = 1;

                    foreach (var lang in titleRow.Skip(1))
                    {
                        var resourceName = GetResourceFileName(lang, sheetName, prefix);
                        var resourcePath = GetResourcesPath(outPutFolder);

                        using var resxWriter = new ResXResourceWriter($"{resourcePath}{resourceName}");
                        foreach (var row in sheetObjectValue.Skip(1))
                        {
                            if (row.Count == 0) continue; // 避免 key沒填寫
                            if (langIdx >= row.Count) continue; //避免除了 Key 以外都沒有 value

                            var key = row[0].ToString();
                            if (string.IsNullOrEmpty(key)) continue;

                            var value = row[langIdx].ToString()?.Trim();
                            if (string.IsNullOrEmpty(value)) continue;

                            resxWriter.AddResource(key, value);
                        }
                        langIdx++;
                        Console.WriteLine($"{resourceName} Generator Done");
                    }
                }
                else
                {
                    Console.WriteLine($"{sheetName} data found.");
                }
            }

            Console.WriteLine($"{prefix.Replace("_", "")} ResourcesGenerator End");
        }

        private string GetResourceFileName(object lang, string sheetName,string replacePrefix)
        {
            var langId = lang.ToString()!.ToLower();
            var resourceName = langId == "value"
                ? $"{sheetName.Substring(replacePrefix.Length)}.resx"
                : $"{sheetName.Substring(replacePrefix.Length)}_{langId}.resx";
            return resourceName;
        }
        
        private string GetResourcesPath(string projectName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Resources", "Resx", projectName);
        }
    }
}
