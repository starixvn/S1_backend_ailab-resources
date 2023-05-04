using AiLab_Resources.Core.Interfaces;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using ICSharpCode.Decompiler.Util;
using Microsoft.Extensions.Configuration;
using AiLab_Resources.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AiLab_Resources.Core.Services
{
    public class GoogleSheetResourcesGenerator : IResourcesGenerator
    {
        private string outputFilePath = Directory.GetCurrentDirectory();
        private readonly GoogleSheetConfig _config;

        public GoogleSheetResourcesGenerator(IServiceProvider provider)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            _config = configuration.GetSection("GoogleSheetConfig").Get<GoogleSheetConfig>();
        }

        public void ResourcesGenerator()
        {
            var service = new SheetsService(new BaseClientService.Initializer
            {
                ApplicationName = _config.ApplicationName,
                ApiKey = _config.GoogleSheetApiKey,
            });

            var sheetId = _config.GoogleSheetInfo.SheetId;
            var sheetsRequest = service.Spreadsheets.Get(sheetId);
            var spreadsheet = sheetsRequest.Execute();
            var sheetList = spreadsheet.Sheets
                .OrderByDescending(s => s.Properties.Title);
            foreach (var s in sheetList)
            {
                Console.WriteLine(s.Properties.Title);
            }

            var sheetName = _config.GoogleSheetInfo.MemberSiteSheetNames.FirstOrDefault();
            var sheetRequest = service.Spreadsheets.Values.Get(sheetId, sheetName);
            var sheet = sheetRequest.Execute();

            var values = sheet.Values;


            if (values != null && values.Count > 0)
            {
                var titleRow = values.First();
                var langIdx = 1;
                
                foreach (var lang in titleRow.Skip(1))
                {
                    var langId = lang.ToString()!.ToLower();
                    using ResXResourceWriter resxWriter = new ResXResourceWriter(outputFilePath + $"MemberSite_{langId}.resx");
                    foreach (var row in values.Skip(1))
                    {
                        if (row.Count == 0) continue; // 避免 key沒填寫
                        if(langIdx >= row.Count) continue; //避免除了 Key 以外都沒有 value
                        
                        var key = row[0].ToString();
                        if(string.IsNullOrEmpty(key)) continue;

                        var value = row[langIdx].ToString().Trim();
                        if (string.IsNullOrEmpty(value)) continue;

                        resxWriter.AddResource(key, value);
                    }
                    langIdx++;
                }
            }
            else
            {
                Console.WriteLine(@"No data found.");
            }

            Console.WriteLine(@"resx generator done");
        }
    }
}
