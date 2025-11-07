using System.Windows.Input;
using WineDegust.Models;
using WineDegust.Services;

namespace WineDegust.ViewModels;

[QueryProperty(nameof(WineId), "WineId")]
public class AddEditWineViewModel : BaseViewModel
{
    private readonly WineService _wineService;
    private Wine _wine;
    private bool _isEdit;
    
    public string WineId { get; set; } = string.Empty;
    
    public Wine Wine
    {
        get => _wine;
        set
        {
            _wine = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsEdit
    {
        get => _isEdit;
        set
        {
            _isEdit = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand TakePhotoCommand { get; }
    
    public AddEditWineViewModel(WineService wineService)
    {
        _wineService = wineService;
        _wine = new Wine();
        
        SaveCommand = new Command(async () => await OnSave());
        CancelCommand = new Command(async () => await OnCancel());
        TakePhotoCommand = new Command(async () => await OnTakePhoto());
    }
    
    public void LoadWine()
    {
        if (!string.IsNullOrEmpty(WineId) && Guid.TryParse(WineId, out var id))
        {
            var wine = _wineService.GetWine(id);
            if (wine != null)
            {
                Wine = wine;
                IsEdit = true;
                Title = "Modifier le Vin";
                return;
            }
        }
        
        Wine = new Wine();
        IsEdit = false;
        Title = "Ajouter un Vin";
    }
    
    private async Task OnSave()
    {
        if (string.IsNullOrWhiteSpace(Wine.Name))
        {
            await Shell.Current.DisplayAlert("Erreur", "Le nom du vin est requis", "OK");
            return;
        }
        
        // Validate scores
        if (Wine.VisualScore < 1 || Wine.VisualScore > 2)
        {
            await Shell.Current.DisplayAlert("Erreur", "La note visuelle doit être entre 1 et 2", "OK");
            return;
        }
        
        if (Wine.OlfactiveScore < 1 || Wine.OlfactiveScore > 6)
        {
            await Shell.Current.DisplayAlert("Erreur", "La note olfactive doit être entre 1 et 6", "OK");
            return;
        }
        
        if (Wine.GustativeScore < 1 || Wine.GustativeScore > 8)
        {
            await Shell.Current.DisplayAlert("Erreur", "La note gustative doit être entre 1 et 8", "OK");
            return;
        }
        
        if (Wine.GlobalScore < 1 || Wine.GlobalScore > 4)
        {
            await Shell.Current.DisplayAlert("Erreur", "La note globale doit être entre 1 et 4", "OK");
            return;
        }
        
        if (Wine.TasteRating < 1 || Wine.TasteRating > 20)
        {
            await Shell.Current.DisplayAlert("Erreur", "La note de goût doit être entre 1 et 20", "OK");
            return;
        }
        
        if (IsEdit)
        {
            await _wineService.UpdateWineAsync(Wine);
        }
        else
        {
            await _wineService.AddWineAsync(Wine);
        }
        
        await Shell.Current.GoToAsync("..");
    }
    
    private async Task OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
    
    private async Task OnTakePhoto()
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                
                if (photo != null)
                {
                    // Save the photo to local storage
                    var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    
                    using var stream = await photo.OpenReadAsync();
                    using var newStream = File.OpenWrite(localFilePath);
                    await stream.CopyToAsync(newStream);
                    
                    Wine.PhotoPath = localFilePath;
                    OnPropertyChanged(nameof(Wine));
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", $"Impossible de prendre la photo: {ex.Message}", "OK");
        }
    }
}
