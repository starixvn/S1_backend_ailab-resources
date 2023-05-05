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
        public List<string> BonusServices { get; set; }
        public List<string> ThirdPartyServices { get; set; }
        public List<string> ThirdPartyWagers { get; set; }
        public List<string> ThirdPartySeamlessWallets { get; set; }
        public List<string> ThirdPartyBetHistorys { get; set; }
        public List<string> Storeservices { get; set; }
        public List<string> Paymentservices { get; set; }
        public List<string> HangfireServices { get; set; }
        public List<string> StatisticalWorkers { get; set; }
    }
}
