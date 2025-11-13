# Guide d'utilisation - WineDegust

## Introduction

WineDegust est une application mobile multi-plateforme (Android, iOS, Windows) conçue pour vous aider à enregistrer et suivre vos dégustations de vins.

## Fonctionnalités principales

### 1. Liste des vins
La page principale affiche tous vos vins enregistrés avec :
- Une photo de la bouteille
- Le nom du vin
- Le producteur
- La note de dégustation (sur 20)
- La note de goût personnel (sur 20)

**Actions disponibles :**
- Appuyez sur un vin pour voir ses détails
- Appuyez sur le bouton "Ajouter" pour enregistrer un nouveau vin
- Appuyez sur "✕" pour supprimer un vin

### 2. Ajouter/Modifier un vin

#### Informations de base
- **Nom du vin** * (obligatoire)
- **Producteur**
- **Région**
- **Année**

#### Photo de la bouteille
- Appuyez sur la zone de photo pour capturer une image avec l'appareil photo
- La photo est sauvegardée localement avec le vin

#### Notes de dégustation

##### Visuel (1-2 points)
Évaluez l'aspect visuel du vin :
- Clarté et limpidité
- Couleur et intensité
- Brillance

Utilisez le curseur pour sélectionner une note de 1 à 2 et ajoutez une description.

##### Olfactif / Nez (1-6 points)
Évaluez les arômes du vin :
- Intensité aromatique
- Complexité
- Nature des arômes (fruits, fleurs, épices, etc.)

Utilisez le curseur pour sélectionner une note de 1 à 6 et ajoutez une description.

##### Gustatif (1-8 points)
Évaluez le goût du vin :
- Équilibre (acidité, tanins, alcool, sucrosité)
- Intensité et persistance des saveurs
- Longueur en bouche
- Structure

Utilisez le curseur pour sélectionner une note de 1 à 8 et ajoutez une description.

##### Global (1-4 points)
Évaluez l'impression d'ensemble :
- Harmonie générale
- Potentiel de garde
- Rapport qualité/prix

Utilisez le curseur pour sélectionner une note de 1 à 4 et ajoutez une description.

#### Note de dégustation calculée
La note de dégustation est calculée automatiquement en additionnant les quatre notes précédentes :
- Visuel (max 2) + Olfactif (max 6) + Gustatif (max 8) + Global (max 4) = **Total sur 20**

Cette note objective reflète la qualité technique du vin.

#### Note de goût personnel (1-20)
Indiquez votre appréciation personnelle du vin, indépendamment de sa qualité technique.
- 1-5 : Vous n'avez pas aimé
- 6-10 : Acceptable mais pas mémorable
- 11-15 : Bon vin, vous l'appréciez
- 16-18 : Excellent vin, vous l'adorez
- 19-20 : Exceptionnel, un coup de cœur

#### Notes additionnelles
Espace libre pour noter :
- Accords mets-vins
- Occasion de dégustation
- Prix
- Lieu d'achat
- Toute autre information pertinente

### 3. Détails d'un vin

La page de détails affiche toutes les informations du vin :
- Photo en grand format
- Informations générales
- Toutes les notes de dégustation avec descriptions
- Note de dégustation totale
- Note de goût personnel
- Notes additionnelles

**Actions disponibles :**
- **Modifier** : Permet de modifier toutes les informations
- **Supprimer** : Supprime le vin après confirmation

## Conseils d'utilisation

### Pour une dégustation optimale

1. **Avant de déguster** :
   - Servez le vin à la bonne température
   - Utilisez un verre adapté
   - Assurez-vous d'avoir un bon éclairage pour évaluer la couleur

2. **Pendant la dégustation** :
   - Prenez d'abord la photo de la bouteille
   - Remplissez les notes dans l'ordre : Visuel → Olfactif → Gustatif → Global
   - Notez vos impressions immédiatement, elles sont plus précises

3. **Grille de notation suggérée** :

   **Visuel (1-2)** :
   - 1 : Aspect correct mais simple
   - 2 : Aspect brillant, couleur éclatante

   **Olfactif (1-6)** :
   - 1-2 : Arômes discrets ou défauts
   - 3-4 : Arômes présents et plaisants
   - 5-6 : Arômes complexes et intenses

   **Gustatif (1-8)** :
   - 1-3 : Déséquilibré ou défauts
   - 4-5 : Équilibré, saveurs correctes
   - 6-7 : Très bon équilibre, belles saveurs
   - 8 : Perfection, complexité exceptionnelle

   **Global (1-4)** :
   - 1-2 : Vin simple ou avec défauts
   - 3 : Bon vin, bien fait
   - 4 : Excellent vin, tout est harmonieux

## Stockage des données

Actuellement, les données sont stockées en mémoire pendant l'exécution de l'application. 

Pour une utilisation en production, il est recommandé d'implémenter :
- Sauvegarde dans une base de données locale (SQLite)
- Synchronisation cloud optionnelle
- Export des données en CSV ou PDF

## Support technique

Pour toute question ou suggestion d'amélioration, n'hésitez pas à ouvrir une issue sur le dépôt GitHub du projet.

## Développement futur

Fonctionnalités envisagées :
- Base de données persistante
- Export des dégustations en PDF
- Partage de fiches de dégustation
- Statistiques et graphiques
- Bibliothèque de vins avec API externe
- Reconnaissance des étiquettes par photo
- Rappels pour déguster les vins en cave
