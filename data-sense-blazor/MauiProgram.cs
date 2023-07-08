﻿using data_sense_blazor.Data;
using data_sense_blazor.Shared;
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

        builder.Services.AddSingleton(s => new SQLServerDatabaseService(connectionString, s.GetRequiredService<ILogger<SQLServerDatabaseService>>()));
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
        //builder.Services.AddScoped<SQLServerDatabaseService>();
        builder.Services.AddSingleton<AppState>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();

        return builder.Build();
    }
}

