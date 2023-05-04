using AiLab_Resources.Core.Interfaces;
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
        var serviceProvider = services.BuildServiceProvider();

        var resourcesProvider = serviceProvider.GetRequiredService<MemberSiteResourcesReader>();
        resourcesProvider.ReadOldResourcesExportExcel();

        //var resourcesProvider = serviceProvider.GetRequiredService<IResourcesGenerator>();
        //resourcesProvider.ResourcesGenerator();
    }
}