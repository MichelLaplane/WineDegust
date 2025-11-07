using System.Collections.ObjectModel;
using WineDegust.Models;

namespace WineDegust.Services;

/// <summary>
/// Service to manage wine data storage and retrieval
/// </summary>
public class WineService
{
    private readonly ObservableCollection<Wine> _wines = new();
    
    public WineService()
    {
        // Load wines from storage (in a real app, this would load from database or file)
        LoadWines();
    }
    
    public ObservableCollection<Wine> GetAllWines()
    {
        return _wines;
    }
    
    public Wine? GetWine(Guid id)
    {
        return _wines.FirstOrDefault(w => w.Id == id);
    }
    
    public async Task<Wine> AddWineAsync(Wine wine)
    {
        _wines.Add(wine);
        await SaveWinesAsync();
        return wine;
    }
    
    public async Task UpdateWineAsync(Wine wine)
    {
        var existingWine = _wines.FirstOrDefault(w => w.Id == wine.Id);
        if (existingWine != null)
        {
            var index = _wines.IndexOf(existingWine);
            _wines[index] = wine;
            await SaveWinesAsync();
        }
    }
    
    public async Task DeleteWineAsync(Guid id)
    {
        var wine = _wines.FirstOrDefault(w => w.Id == id);
        if (wine != null)
        {
            _wines.Remove(wine);
            await SaveWinesAsync();
        }
    }
    
    private void LoadWines()
    {
        // In a real application, load from preferences, database, or file
        // For now, we start with an empty collection
    }
    
    private async Task SaveWinesAsync()
    {
        // In a real application, save to preferences, database, or file
        await Task.CompletedTask;
    }
}
