# WineDegust

Application MAUI (iOS, Android, Windows) d'aide à la dégustation de vins.

## Fonctionnalités

- **Prise de photo des bouteilles** : Capturez directement depuis l'application
- **Notation avec listes** :
  - **Visuel (aspect visuel)** : Note de 1 à 2 avec description
  - **Olfactif (nez)** : Note de 1 à 6 avec description
  - **Gustatif** : Note de 1 à 8 avec description
  - **Global** : Note de 1 à 4 avec description
  - **Note de dégustation** : Somme automatique des notes Visuel + Olfactif + Gustatif + Global (sur 20)
  - **Note de goût personnel** : Note subjective de 1 à 20

## Structure du projet

Le projet est organisé selon l'architecture MVVM :

- **Models/** : Modèles de données (Wine)
- **Views/** : Pages XAML de l'interface utilisateur
  - WineListPage : Liste des vins
  - WineDetailPage : Détails d'un vin
  - AddEditWinePage : Ajouter/Modifier un vin
- **ViewModels/** : Logique de présentation
- **Services/** : Services métier (WineService)
- **Platforms/** : Code spécifique à chaque plateforme (Android, iOS, Windows, MacCatalyst)

## Prérequis

Pour compiler et exécuter ce projet, vous devez installer :

1. **.NET 8 SDK ou plus récent**
2. **Workload .NET MAUI** : 
   ```bash
   dotnet workload install maui
   ```

## Compilation

```bash
# Restaurer les packages NuGet
dotnet restore

# Compiler pour Android
dotnet build -f net8.0-android

# Compiler pour iOS (Mac uniquement)
dotnet build -f net8.0-ios

# Compiler pour Windows
dotnet build -f net8.0-windows10.0.19041.0
```

## Exécution

```bash
# Android
dotnet run -f net8.0-android

# iOS (Mac uniquement)
dotnet run -f net8.0-ios

# Windows
dotnet run -f net8.0-windows10.0.19041.0
```

## Modèle de données Wine

Le modèle `Wine` contient les propriétés suivantes :

- Informations générales : Nom, Producteur, Année, Région
- Photo de la bouteille
- Notes de dégustation :
  - VisualScore (1-2) + VisualDescription
  - OlfactiveScore (1-6) + OlfactiveDescription
  - GustativeScore (1-8) + GustativeDescription
  - GlobalScore (1-4) + GlobalDescription
  - TastingNote (calculé automatiquement, sur 20)
  - TasteRating (1-20, note personnelle)
- Notes additionnelles

## Permissions requises

### Android (AndroidManifest.xml)
- `CAMERA` : Pour prendre des photos
- `WRITE_EXTERNAL_STORAGE` / `READ_EXTERNAL_STORAGE` : Pour sauvegarder les photos

### iOS (Info.plist)
- `NSCameraUsageDescription` : Description pour l'accès à la caméra
- `NSPhotoLibraryUsageDescription` : Description pour l'accès à la photothèque
