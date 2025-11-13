using WineDegust.ViewModels;

namespace WineDegust.Views;

public partial class WineListPage : ContentPage
{
    public WineListPage(WineListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
