namespace WineDegust;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute(nameof(Views.WineDetailPage), typeof(Views.WineDetailPage));
        Routing.RegisterRoute(nameof(Views.AddEditWinePage), typeof(Views.AddEditWinePage));
    }
}
