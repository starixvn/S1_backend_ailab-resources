using AiLab_Resources.Core.Models;

namespace AiLab_Resources.Core.Interfaces
{
    public interface IResourcesGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sheetNames"></param>
        /// <param name="prefix">專案名稱_</param>
        /// <param name="outPutFolder">輸出資料夾名稱</param>
        void ResourcesGeneratorResx(GoogleSheetConfig config, List<string> sheetNames, string prefix, string outPutFolder);
    }
}
