# Architecture WineDegust

## Vue d'ensemble de l'application

```
┌─────────────────────────────────────────────────────────────────┐
│                         WineDegust App                           │
│                   (iOS, Android, Windows)                        │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                         AppShell                                 │
│                    (Navigation Shell)                            │
└─────────────────────────────────────────────────────────────────┘
                                │
                ┌───────────────┼───────────────┐
                ▼               ▼               ▼
         ┌──────────┐    ┌──────────┐    ┌──────────┐
         │ WineList │    │  Detail  │    │ AddEdit  │
         │   Page   │    │   Page   │    │   Page   │
         └──────────┘    └──────────┘    └──────────┘
                │               │               │
                ▼               ▼               ▼
         ┌──────────┐    ┌──────────┐    ┌──────────┐
         │ WineList │    │  Detail  │    │ AddEdit  │
         │ViewModel │    │ViewModel │    │ ViewModel│
         └──────────┘    └──────────┘    └──────────┘
                │               │               │
                └───────────────┼───────────────┘
                                ▼
                        ┌──────────────┐
                        │ WineService  │
                        │   (CRUD)     │
                        └──────────────┘
                                │
                                ▼
                        ┌──────────────┐
                        │ Wine Model   │
                        │   (Data)     │
                        └──────────────┘
```

## Flux de données - Ajout d'un vin

```
1. Utilisateur                    2. UI                       3. ViewModel                4. Service

   Appuie sur                   AddEditWine                AddEditWine                WineService
   "Ajouter"  ────────────▶     Page.xaml    ───────────▶  ViewModel     ─────────▶   AddWineAsync()
                                                                                             │
                                                                                             ▼
                                                                                    Collection<Wine>
   Photo prise ──────────▶      Camera       ───────────▶  TakePhotoCommand ──────▶ Wine.PhotoPath
   (MediaPicker)               Integration                  OnTakePhoto()           
                                                                                    
   Sliders                     XAML Sliders  ───────────▶  Wine.VisualScore        
   ajustés    ────────────▶    (1-2, 1-6,                   Wine.OlfactiveScore     
                               1-8, 1-4)                    Wine.GustativeScore     
                                                            Wine.GlobalScore        
                                                                  │
                                                                  ▼
                                                         TastingNote (calculé)
                                                         = Sum(V+O+G+G)
   
   Description               Entry/Editor   ───────────▶  Wine.VisualDescription
   saisie     ────────────▶  Controls                     Wine.OlfactiveDesc...
                                                          etc.

   Note de goût             Slider         ───────────▶  Wine.TasteRating
   (1-20)     ────────────▶  (1-20)                      

   Appuie sur              Save Button    ───────────▶  SaveCommand.Execute() ─────▶ WineService
   "Enregistrer"                                         Validation + Save              .AddWineAsync()
                                                                                             │
                                                                                             ▼
   Retour liste ◀──────────  Navigation  ◀────────────  GoToAsync("..")           Collection
                             Shell                                                 mise à jour
```

## Modèle de données Wine

```
Wine
├── Id: Guid (généré automatiquement)
│
├── Informations générales
│   ├── Name: string (obligatoire)
│   ├── Producer: string
│   ├── Region: string
│   ├── Year: int?
│   ├── PhotoPath: string
│   └── TastingDate: DateTime
│
├── Notes de dégustation
│   ├── VisualScore: int (1-2, défaut: 1)
│   ├── VisualDescription: string
│   ├── OlfactiveScore: int (1-6, défaut: 1)
│   ├── OlfactiveDescription: string
│   ├── GustativeScore: int (1-8, défaut: 1)
│   ├── GustativeDescription: string
│   ├── GlobalScore: int (1-4, défaut: 1)
│   └── GlobalDescription: string
│
├── Notes calculées et personnelles
│   ├── TastingNote: int (0-20, calculé)
│   │   = VisualScore + OlfactiveScore 
│   │     + GustativeScore + GlobalScore
│   │
│   └── TasteRating: int (1-20, défaut: 10)
│
└── Notes: string (notes additionnelles)
```

## Pattern MVVM

```
┌─────────────────────────────────────────────────────────────────┐
│                            VIEW (XAML)                           │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  <Entry Text="{Binding Wine.Name}" />                      │  │
│  │  <Slider Value="{Binding Wine.VisualScore}" Min="1" Max="2"/>│ │
│  │  <Button Command="{Binding SaveCommand}" />                 │  │
│  └───────────────────────────────────────────────────────────┘  │
└────────────────────────────┬────────────────────────────────────┘
                             │ Data Binding
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                          VIEWMODEL                               │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  public Wine Wine { get; set; }                            │  │
│  │  public ICommand SaveCommand { get; }                      │  │
│  │  private async Task OnSave() {                             │  │
│  │      await _wineService.AddWineAsync(Wine);                │  │
│  │  }                                                          │  │
│  └───────────────────────────────────────────────────────────┘  │
└────────────────────────────┬────────────────────────────────────┘
                             │ Appel service
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                           SERVICE                                │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  public async Task<Wine> AddWineAsync(Wine wine) {         │  │
│  │      _wines.Add(wine);                                     │  │
│  │      await SaveWinesAsync();                               │  │
│  │      return wine;                                          │  │
│  │  }                                                          │  │
│  └───────────────────────────────────────────────────────────┘  │
└────────────────────────────┬────────────────────────────────────┘
                             │ Manipulation
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                           MODEL                                  │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  public class Wine {                                        │  │
│  │      public string Name { get; set; }                       │  │
│  │      public int VisualScore { get; set; } = 1;             │  │
│  │      public int TastingNote =>                             │  │
│  │          VisualScore + OlfactiveScore +                    │  │
│  │          GustativeScore + GlobalScore;                     │  │
│  │  }                                                          │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## Calcul automatique de la note de dégustation

```
Note de Dégustation = Somme de toutes les notes

┌──────────────────┐
│ Visuel (1-2)     │ ─┐
└──────────────────┘  │
                      │
┌──────────────────┐  │
│ Olfactif (1-6)   │ ─┤
└──────────────────┘  │  Somme automatique
                      ├──────────────────▶  Note de Dégustation
┌──────────────────┐  │                    (min: 4, max: 20)
│ Gustatif (1-8)   │ ─┤
└──────────────────┘  │
                      │
┌──────────────────┐  │
│ Global (1-4)     │ ─┘
└──────────────────┘

Exemple:
  Visuel:    2/2
  Olfactif:  5/6
  Gustatif:  7/8
  Global:    3/4
  ─────────────────
  Total:    17/20  ✓
```

## Plateformes et permissions

```
┌─────────────────────────────────────────────────────────────────┐
│                        Platforms/                                │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Android/                     iOS/                               │
│  ├── MainActivity.cs          ├── AppDelegate.cs                │
│  ├── MainApplication.cs       ├── Program.cs                    │
│  └── AndroidManifest.xml      └── Info.plist                    │
│      ├── CAMERA                   ├── NSCameraUsageDescription  │
│      ├── WRITE_EXTERNAL           └── NSPhotoLibraryUsage...    │
│      └── READ_EXTERNAL                                          │
│                                                                  │
│  MacCatalyst/                 Windows/                           │
│  ├── AppDelegate.cs           ├── App.xaml                      │
│  ├── Program.cs               └── App.xaml.cs                   │
│  └── Info.plist                                                 │
│      ├── NSCameraUsageDescription                               │
│      └── NSPhotoLibraryUsage...                                 │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Injection de dépendances

```
MauiProgram.cs
    │
    ├─ Services (Singleton)
    │   └─ WineService ──────────────────┐
    │                                     │
    ├─ Pages                              │
    │   ├─ WineListPage (Singleton) ──┐  │
    │   ├─ WineDetailPage (Transient) │  │
    │   └─ AddEditWinePage (Transient)│  │
    │                                  │  │
    └─ ViewModels                     │  │
        ├─ WineListViewModel ◀────────┼──┘
        │   (Singleton)               │
        ├─ WineDetailViewModel ◀──────┤
        │   (Transient)               │
        └─ AddEditWineViewModel ◀─────┘
            (Transient)

Constructor injection automatique par le conteneur DI
```
