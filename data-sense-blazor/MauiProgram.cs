using data_sense_blazor.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace data_sense_blazor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = configurationBuilder.Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddSingleton(s => new SQLServerDatabaseService(connectionString));
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
        //builder.Services.AddScoped<SQLServerDatabaseService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();

        return builder.Build();
    }

    private static async Task<IConfiguration> GetConfigurationAsync()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("appsettings.json");
        var stream = await response.Content.ReadAsStreamAsync();
        return new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
    }
}

