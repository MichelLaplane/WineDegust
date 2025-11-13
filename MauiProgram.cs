using Microsoft.Extensions.Logging;

namespace WineDegust;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register services
        builder.Services.AddSingleton<Services.WineService>();
        
        // Register pages
        builder.Services.AddSingleton<Views.WineListPage>();
        builder.Services.AddTransient<Views.WineDetailPage>();
        builder.Services.AddTransient<Views.AddEditWinePage>();
        
        // Register view models
        builder.Services.AddSingleton<ViewModels.WineListViewModel>();
        builder.Services.AddTransient<ViewModels.WineDetailViewModel>();
        builder.Services.AddTransient<ViewModels.AddEditWineViewModel>();

        return builder.Build();
    }
}
