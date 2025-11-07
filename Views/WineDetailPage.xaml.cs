using WineDegust.ViewModels;

namespace WineDegust.Views;

public partial class WineDetailPage : ContentPage
{
    private readonly WineDetailViewModel _viewModel;
    
    public WineDetailPage(WineDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadWine();
    }
}
