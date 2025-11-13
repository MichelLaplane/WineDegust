using WineDegust.ViewModels;

namespace WineDegust.Views;

public partial class AddEditWinePage : ContentPage
{
    private readonly AddEditWineViewModel _viewModel;
    
    public AddEditWinePage(AddEditWineViewModel viewModel)
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
