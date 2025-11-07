using System.Collections.ObjectModel;
using System.Windows.Input;
using WineDegust.Models;
using WineDegust.Services;

namespace WineDegust.ViewModels;

public class WineListViewModel : BaseViewModel
{
    private readonly WineService _wineService;
    
    public ObservableCollection<Wine> Wines { get; }
    
    public ICommand AddWineCommand { get; }
    public ICommand SelectWineCommand { get; }
    public ICommand DeleteWineCommand { get; }
    
    public WineListViewModel(WineService wineService)
    {
        _wineService = wineService;
        Title = "Mes Vins";
        
        Wines = _wineService.GetAllWines();
        
        AddWineCommand = new Command(async () => await OnAddWine());
        SelectWineCommand = new Command<Wine>(async (wine) => await OnSelectWine(wine));
        DeleteWineCommand = new Command<Wine>(async (wine) => await OnDeleteWine(wine));
    }
    
    private async Task OnAddWine()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddEditWinePage));
    }
    
    private async Task OnSelectWine(Wine wine)
    {
        if (wine == null)
            return;
            
        await Shell.Current.GoToAsync($"{nameof(Views.WineDetailPage)}?WineId={wine.Id}");
    }
    
    private async Task OnDeleteWine(Wine wine)
    {
        if (wine == null)
            return;
            
        bool answer = await Shell.Current.DisplayAlert(
            "Supprimer",
            $"Êtes-vous sûr de vouloir supprimer {wine.Name}?",
            "Oui",
            "Non");
            
        if (answer)
        {
            await _wineService.DeleteWineAsync(wine.Id);
        }
    }
}
