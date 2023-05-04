using AiLab_Resources.Core.Interfaces;
using AiLab_Resources.Core.Models;
using AiLab_Resources.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(config);
        services.AddTransient<IResourcesGenerator, GoogleSheetResourcesGenerator>();
        services.AddTransient<MemberSiteResourcesReader>();
        services.AddTransient<BackendSiteResourcesReader>();
        var serviceProvider = services.BuildServiceProvider();

        //var resourcesProvider = serviceProvider.GetRequiredService<BackendSiteResourcesReader>();
        //resourcesProvider.ReadOldResourcesExportExcel();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var googleSheetConfig = configuration.GetSection("GoogleSheetConfig").Get<GoogleSheetConfig>();


        var resourcesProvider = serviceProvider.GetRequiredService<IResourcesGenerator>();
        //resourcesProvider.ResourcesGeneratorResx(googleSheetConfig, googleSheetConfig.GoogleSheetInfo.MemberSites, "MemberSite_", nameof(googleSheetConfig.GoogleSheetInfo.MemberSites));
        resourcesProvider.ResourcesGeneratorResx(googleSheetConfig, googleSheetConfig.GoogleSheetInfo.BackendSites, "BackendSite_", nameof(googleSheetConfig.GoogleSheetInfo.BackendSites));
    }
}