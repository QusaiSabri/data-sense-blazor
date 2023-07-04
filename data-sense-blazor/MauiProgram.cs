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

        builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
        builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        // Provide ILogger when creating SQLServerDatabaseService singleton
        builder.Services.AddSingleton(s => new SQLServerDatabaseService(connectionString, s.GetRequiredService<ILogger<SQLServerDatabaseService>>()));
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
}

