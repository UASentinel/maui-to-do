﻿using Microsoft.Extensions.Logging;
using ToDoMauiClient.DataServices;
using ToDoMauiClient.Pages;

namespace ToDoMauiClient
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient<IRestDataService, RestDataService>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<ManageToDoPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
