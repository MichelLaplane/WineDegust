namespace WineDegust.Models;

/// <summary>
/// Represents a wine with its tasting notes and photos
/// </summary>
public class Wine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = string.Empty;
    
    public string Producer { get; set; } = string.Empty;
    
    public int? Year { get; set; }
    
    public string Region { get; set; } = string.Empty;
    
    public string PhotoPath { get; set; } = string.Empty;
    
    public DateTime TastingDate { get; set; } = DateTime.Now;
    
    // Visual aspect (1-2)
    public int VisualScore { get; set; }
    public string VisualDescription { get; set; } = string.Empty;
    
    // Olfactive/Nose (1-6)
    public int OlfactiveScore { get; set; }
    public string OlfactiveDescription { get; set; } = string.Empty;
    
    // Gustative/Taste (1-8)
    public int GustativeScore { get; set; }
    public string GustativeDescription { get; set; } = string.Empty;
    
    // Global (1-4)
    public int GlobalScore { get; set; }
    public string GlobalDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// Calculated tasting note: sum of Visual + Olfactive + Gustative + Global
    /// Maximum: 2 + 6 + 8 + 4 = 20
    /// </summary>
    public int TastingNote => VisualScore + OlfactiveScore + GustativeScore + GlobalScore;
    
    // Personal taste rating (1-20)
    public int TasteRating { get; set; }
    
    public string Notes { get; set; } = string.Empty;
}
