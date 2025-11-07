# Guide de développement - WineDegust

## Configuration de l'environnement de développement

### Prérequis

1. **Visual Studio 2022** (Windows/Mac) ou **Visual Studio Code** avec extensions C#/.NET MAUI
2. **.NET 8 SDK** : [Télécharger](https://dotnet.microsoft.com/download/dotnet/8.0)
3. **Workload .NET MAUI** :
   ```bash
   dotnet workload install maui
   ```

### Configuration par plateforme

#### Android
- **Android SDK** (API 21 ou supérieure)
- **Java Development Kit (JDK)** 11 ou supérieur
- Configuration dans Visual Studio : Outils → Android → Android SDK Manager

#### iOS (Mac uniquement)
- **Xcode** (dernière version stable)
- **Compte développeur Apple** pour le déploiement sur appareil
- Configurer le provisioning profile dans Xcode

#### Windows
- **Windows 10/11** avec SDK 19041 ou supérieur
- Configuration automatique avec Visual Studio 2022

## Structure du projet

```
WineDegust/
├── Models/                     # Modèles de données
│   └── Wine.cs                 # Modèle Wine avec toutes les propriétés
├── Views/                      # Pages XAML
│   ├── WineListPage.xaml       # Liste des vins
│   ├── WineDetailPage.xaml     # Détails d'un vin
│   └── AddEditWinePage.xaml    # Formulaire d'ajout/édition
├── ViewModels/                 # ViewModels MVVM
│   ├── BaseViewModel.cs        # ViewModel de base
│   ├── WineListViewModel.cs
│   ├── WineDetailViewModel.cs
│   └── AddEditWineViewModel.cs
├── Services/                   # Services métier
│   └── WineService.cs          # Gestion des vins
├── Resources/                  # Ressources de l'application
│   ├── AppIcon/                # Icône de l'application
│   ├── Splash/                 # Écran de démarrage
│   ├── Images/                 # Images
│   └── Styles/                 # Styles XAML
│       ├── Colors.xaml
│       └── Styles.xaml
├── Platforms/                  # Code spécifique aux plateformes
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
├── App.xaml                    # Application XAML
├── AppShell.xaml               # Shell de navigation
└── MauiProgram.cs              # Point d'entrée et DI
```

## Architecture

Le projet utilise le pattern **MVVM (Model-View-ViewModel)** :

### Models
Les modèles représentent les données de l'application. `Wine.cs` contient :
- Propriétés de base (nom, producteur, région, année)
- Chemin de la photo
- Notes de dégustation (visuel, olfactif, gustatif, global)
- Note de dégustation calculée (propriété computed)
- Note de goût personnel

### Views
Les vues sont des pages XAML qui définissent l'interface utilisateur :
- Liaison de données avec `{Binding}`
- Styles centralisés dans `Resources/Styles/`
- Navigation via Shell

### ViewModels
Les ViewModels contiennent la logique de présentation :
- Implémentent `INotifyPropertyChanged` via `BaseViewModel`
- Exposent des `ICommand` pour les actions utilisateur
- Communiquent avec les Services
- Pas de référence directe aux Views

### Services
Les services contiennent la logique métier et l'accès aux données :
- `WineService` : CRUD des vins
- Peut être étendu pour ajouter la persistance

## Compilation et exécution

### Ligne de commande

```bash
# Restaurer les packages
dotnet restore

# Compiler pour Android
dotnet build -f net8.0-android

# Compiler pour iOS (Mac uniquement)
dotnet build -f net8.0-ios

# Compiler pour Windows
dotnet build -f net8.0-windows10.0.19041.0

# Exécuter sur Android
dotnet run -f net8.0-android

# Exécuter sur iOS
dotnet run -f net8.0-ios
```

### Visual Studio

1. Ouvrir `WineDegust.sln`
2. Sélectionner la plateforme cible dans la barre d'outils
3. Sélectionner l'émulateur ou l'appareil
4. Appuyer sur F5 pour compiler et exécuter

## Injection de dépendances

Le projet utilise l'injection de dépendances native de .NET MAUI.

Configuration dans `MauiProgram.cs` :
```csharp
// Services
builder.Services.AddSingleton<Services.WineService>();

// Pages
builder.Services.AddSingleton<Views.WineListPage>();
builder.Services.AddTransient<Views.WineDetailPage>();
builder.Services.AddTransient<Views.AddEditWinePage>();

// ViewModels
builder.Services.AddSingleton<ViewModels.WineListViewModel>();
builder.Services.AddTransient<ViewModels.WineDetailViewModel>();
builder.Services.AddTransient<ViewModels.AddEditWineViewModel>();
```

## Navigation

La navigation utilise **Shell Navigation** :

```csharp
// Navigation vers une page enregistrée
await Shell.Current.GoToAsync(nameof(WineDetailPage));

// Navigation avec paramètres
await Shell.Current.GoToAsync($"{nameof(WineDetailPage)}?WineId={wine.Id}");

// Retour arrière
await Shell.Current.GoToAsync("..");
```

Les paramètres sont reçus via l'attribut `[QueryProperty]` :
```csharp
[QueryProperty(nameof(WineId), "WineId")]
public class WineDetailViewModel : BaseViewModel
{
    public string WineId { get; set; }
}
```

## Capture de photos

La capture de photos utilise `MediaPicker` :

```csharp
if (MediaPicker.Default.IsCaptureSupported)
{
    var photo = await MediaPicker.Default.CapturePhotoAsync();
    if (photo != null)
    {
        var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
        using var stream = await photo.OpenReadAsync();
        using var newStream = File.OpenWrite(localFilePath);
        await stream.CopyToAsync(newStream);
    }
}
```

### Permissions requises

**Android** (`Platforms/Android/AndroidManifest.xml`) :
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
```

**iOS** (`Platforms/iOS/Info.plist`) :
```xml
<key>NSCameraUsageDescription</key>
<string>This app needs access to the camera to take photos of wine bottles.</string>
```

## Styles et thèmes

Les styles sont centralisés dans `Resources/Styles/` :

- **Colors.xaml** : Définition des couleurs de l'application
- **Styles.xaml** : Styles des contrôles (Button, Entry, Label, etc.)

Application des styles :
```xml
<!-- Utilisation d'une couleur statique -->
<Button BackgroundColor="{StaticResource Primary}" />

<!-- Le style Button est appliqué automatiquement -->
<Button Text="Enregistrer" />
```

## Tests

### Tests unitaires recommandés

1. **Tests des ViewModels** :
   - Vérifier les commandes
   - Vérifier les propriétés calculées
   - Vérifier la logique métier

2. **Tests des Models** :
   - Vérifier les propriétés calculées (ex: `TastingNote`)
   - Vérifier les validations

3. **Tests des Services** :
   - Vérifier les opérations CRUD
   - Vérifier la persistance des données

### Exemple de test

```csharp
[Test]
public void Wine_TastingNote_IsCalculatedCorrectly()
{
    var wine = new Wine
    {
        VisualScore = 2,
        OlfactiveScore = 5,
        GustativeScore = 7,
        GlobalScore = 3
    };
    
    Assert.AreEqual(17, wine.TastingNote);
}
```

## Améliorations futures

### Persistance des données

Ajouter SQLite pour sauvegarder les données :

```bash
dotnet add package sqlite-net-pcl
```

Modifier `WineService` pour utiliser une base de données au lieu d'une collection en mémoire.

### Synchronisation cloud

Intégrer Azure Mobile Services ou Firebase pour :
- Sauvegarde cloud
- Synchronisation multi-appareils
- Partage de fiches

### Export PDF

Utiliser une bibliothèque comme QuestPDF pour générer des fiches de dégustation en PDF.

### Recherche et filtrage

Ajouter des fonctionnalités de :
- Recherche par nom, producteur, région
- Filtres par note, année
- Tri personnalisé

## Contribution

1. Fork le projet
2. Créer une branche feature (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## Licence

Ce projet est sous licence MIT. Voir le fichier LICENSE pour plus de détails.
