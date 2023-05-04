using ClosedXML.Excel;
using System.Collections;
using System.Reflection;
using System.Resources;

namespace AiLab_Resources.Core.Services
{
    public class BackendSiteResourcesReader
    {
        //public List<string> BackEndSiteMsgResx = new() { "lan_ch", "lan_cs", "lan_en", "lan_hi", "lan_id", "lan_kh", "lan_my", "lan_th", "lan_vn" };
        //public List<string> BackEndSiteMsgResx = new() { "GameMsg_lan_en" };
        public List<string> BackEndSiteMsgResx = new() { "msg_en" };



        public void ReadOldResourcesExportExcel()
        {
            var dictResourceKey = new Dictionary<string, string>();
            var assembly = Assembly.GetExecutingAssembly();

            // 取 Key
            foreach (var resourceName in BackEndSiteMsgResx)
            {
                var resourceManager = new ResourceManager($"AiLab_Resources.Resources.Resx.BackendSites.{resourceName}", assembly);
                var resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
                foreach (DictionaryEntry entry in resourceSet)
                {
                    var key = entry.Key.ToString();
                    if (!dictResourceKey.ContainsKey(key))
                    {
                        dictResourceKey.Add(key, "");
                    }
                }
                resourceSet.Dispose();
            }
            var resourceKeys = dictResourceKey.Keys.ToList();

            var dictLang = new Dictionary<string, List<ResourceKeyValuePair>>();

            foreach (var resourceName in BackEndSiteMsgResx)
            {
                var resourceManager = new ResourceManager($"AiLab_Resources.Resources.Resx.BackendSites.{resourceName}", assembly);

                foreach (var key in resourceKeys)
                {
                    var langValue = resourceManager.GetString(key);
                    var langId = resourceName.Substring(resourceName.Length - 2);

                    if (dictLang.ContainsKey(key))
                    {
                        var langValues = dictLang[key];
                        langValues.Add(new ResourceKeyValuePair(langId, langValue));
                    }
                    else
                    {
                        var values = new ResourceKeyValuePair(langId, langValue);
                        var _ = new List<ResourceKeyValuePair>() { values };
                        dictLang.Add(key, _);
                    }
                }
            }

            var result = new List<ExportData>();

            foreach (var pair in dictLang)
            {
                var key = pair.Key;
                var csValue = GetValueForKey(pair.Value, "cs");
                var enValue = GetValueForKey(pair.Value, "en");
                var bdValue = GetValueForKey(pair.Value, "bd");
                var hiValue = GetValueForKey(pair.Value, "hi");
                var idValue = GetValueForKey(pair.Value, "id");
                var myValue = GetValueForKey(pair.Value, "my");
                var thValue = GetValueForKey(pair.Value, "th");
                var khValue = GetValueForKey(pair.Value, "kh");
                var vnValue = GetValueForKey(pair.Value, "vn");
                result.Add(new ExportData(key, bdValue, csValue, enValue, hiValue, idValue, myValue, thValue, vnValue, khValue));
            }

            using (XLWorkbook workbook = new XLWorkbook())
            {
                // 建立一個工作表
                IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

                // 設定標題列
                worksheet.Cell(1, 1).Value = "key";
                worksheet.Cell(1, 2).Value = "cs";
                worksheet.Cell(1, 3).Value = "en";
                worksheet.Cell(1, 4).Value = "bd";
                worksheet.Cell(1, 5).Value = "hi";
                worksheet.Cell(1, 6).Value = "id";
                worksheet.Cell(1, 7).Value = "my";
                worksheet.Cell(1, 8).Value = "th";
                worksheet.Cell(1, 9).Value = "kh";
                worksheet.Cell(1, 10).Value = "vn";


                // 寫入資料
                for (int i = 0; i < result.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = result[i].key;
                    worksheet.Cell(i + 2, 2).Value = result[i].cs;
                    worksheet.Cell(i + 2, 3).Value = result[i].en;
                    worksheet.Cell(i + 2, 4).Value = result[i].bd;
                    worksheet.Cell(i + 2, 5).Value = result[i].hi;
                    worksheet.Cell(i + 2, 6).Value = result[i].id;
                    worksheet.Cell(i + 2, 7).Value = result[i].my;
                    worksheet.Cell(i + 2, 8).Value = result[i].th;
                    worksheet.Cell(i + 2, 9).Value = result[i].kh;
                    worksheet.Cell(i + 2, 10).Value = result[i].vn;
                }

                // 自動調整欄寬
                worksheet.Columns().AdjustToContents();

                // 儲存Excel檔案
                string excelFileName = "output.xlsx";
                workbook.SaveAs(excelFileName);
            }
        }

        public string GetValueForKey(List<ResourceKeyValuePair> keyValuePairs, string langId)
        {
            foreach (var pair in keyValuePairs)
            {
                if (pair.LangId == langId)
                {
                    return pair.LangValue;
                }
            }

            return string.Empty;
        }

    }
}
