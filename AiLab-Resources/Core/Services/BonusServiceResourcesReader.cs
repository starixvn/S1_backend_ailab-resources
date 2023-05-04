using ClosedXML.Excel;
using System.Collections;
using System.Reflection;
using System.Resources;

namespace AiLab_Resources.Core.Services
{
    public class BonusServiceResourcesReader
    {
        //public List<string> BonusServiceMsgResx = new() { "BonusService_lan_ch", "BonusService_lan_cs", "BonusService_lan_en", "BonusService_lan_hi",
        //    "BonusService_lan_id", "BonusService_lan_kh", "BonusService_lan_my", "BonusService_lan_th", "BonusService_lan_vn" };
        //public List<string> BonusServiceMsgResx = new() { "msg_bd", "msg_cs", "msg_en", "msg_hi",
        //    "msg_id", "msg_my", "msg_th", "msg_vn" };
        //public List<string> BonusServiceMsgResx = new() { "Bwing_msg_en", "Bwing_msg_vn" };
        public List<string> BonusServiceMsgResx = new() { "Goal123_msg_en", "Goal123_msg_vn" };


        public void ReadOldResourcesExportExcel()
        {
            var dictResourceKey = new Dictionary<string, string>();
            var assembly = Assembly.GetExecutingAssembly();

            // 取 Key
            foreach (var resourceName in BonusServiceMsgResx)
            {
                var resourceManager = new ResourceManager($"AiLab_Resources.Resources.Resx.BonusServices.{resourceName}", assembly);
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

            foreach (var resourceName in BonusServiceMsgResx)
            {
                var resourceManager = new ResourceManager($"AiLab_Resources.Resources.Resx.BonusServices.{resourceName}", assembly);

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
