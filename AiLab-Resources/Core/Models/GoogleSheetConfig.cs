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
        public List<string> MemberSiteSheetNames { get; set; }
        public List<string> BackEndSiteSheetNames { get; set; }
        public List<string> BonusServices { get; set; }
    }
}
