using AiLab_Resources.Core.Models;

namespace AiLab_Resources.Core.Interfaces
{
    public interface IResourcesGenerator
    {
        void ResourcesGeneratorResx(GoogleSheetConfig config, List<string> sheetNames, string prefix, string outPutFolder);
    }
}
