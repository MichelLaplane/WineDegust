using System.Windows.Input;
using WineDegust.Models;
using WineDegust.Services;

namespace WineDegust.ViewModels;

[QueryProperty(nameof(WineId), "WineId")]
public class WineDetailViewModel : BaseViewModel
{
    private readonly WineService _wineService;
    private Wine? _wine;
    
    public string WineId { get; set; } = string.Empty;
    
    public Wine? Wine
    {
        get => _wine;
        set
        {
            _wine = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand EditWineCommand { get; }
    public ICommand DeleteWineCommand { get; }
    
    public WineDetailViewModel(WineService wineService)
    {
        _wineService = wineService;
        Title = "Détails du Vin";
        
        EditWineCommand = new Command(async () => await OnEditWine());
        DeleteWineCommand = new Command(async () => await OnDeleteWine());
    }
    
    public void LoadWine()
    {
        if (Guid.TryParse(WineId, out var id))
        {
            Wine = _wineService.GetWine(id);
            if (Wine != null)
            {
                Title = Wine.Name;
            }
        }
    }
    
    private async Task OnEditWine()
    {
        if (Wine == null)
            return;
            
        await Shell.Current.GoToAsync($"{nameof(Views.AddEditWinePage)}?WineId={Wine.Id}");
    }
    
    private async Task OnDeleteWine()
    {
        if (Wine == null)
            return;
            
        bool answer = await Shell.Current.DisplayAlert(
            "Supprimer",
            $"Êtes-vous sûr de vouloir supprimer {Wine.Name}?",
            "Oui",
            "Non");
            
        if (answer)
        {
            await _wineService.DeleteWineAsync(Wine.Id);
            await Shell.Current.GoToAsync("..");
        }
    }
}
