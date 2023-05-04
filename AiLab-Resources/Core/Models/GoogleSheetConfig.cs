using System;
namespace AiLab_Resources.Core.Models
{
    public class GoogleSheetConfig
    {
        public string ApplicationName { get; set; }
        public string GoogleSheetApiKey { get; set; }
        public GoogleSheetInfo GoogleSheetInfo { get; set; }
    }

    public class GoogleSheetInfo
    {
        public string SheetId { get; set; }
        public List<string> MemberSites { get; set; }
        public List<string> BackendSites { get; set; }
        public List<string> CommonServices { get; set; }
    }
}
