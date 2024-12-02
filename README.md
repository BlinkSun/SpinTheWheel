
# 🎡 SpinTheWheel

**SpinTheWheel** est une application WPF interactive et amusante qui vous permet de sélectionner aléatoirement des participants grâce à une magnifique roue animée. Parfaite pour des jeux, des activités de groupe ou simplement pour éviter de prendre une décision difficile vous-même. 😉

---

## 🚀 Fonctionnalités Principales

- **Roue Animée** : Cliquez sur le centre de la roue pour la faire tourner, et laissez-la ralentir pour la sélection d'un participant. 🎯
- **Gestion des Participants** :
  - Ajoutez, modifiez ou supprimez des participants dans une interface conviviale. ✏️
  - Marquez les participants comme "déjà choisis" pour qu'ils ne soient plus sélectionnés.
- **Base de Données Locale** : Utilise SQLite pour stocker les participants dans un fichier `database.db` situé directement à côté de l'exécutable. 🗂️
- **Design Moderne et Minimaliste** : Une interface intuitive et épurée, parfaite pour tous les utilisateurs. 🖼️

---

## 🛠️ Architecture et Technologies

Ce projet est construit avec une approche **MVVM (Model-View-ViewModel)** et les meilleures pratiques WPF, combinant robustesse et modularité.

### Technologies Utilisées
- **WPF (.NET 9.0)** : Pour une interface graphique élégante et réactive.
- **SQLite** : Gestion locale des données, rapide et efficace.
- **MVVM** : Séparation propre des responsabilités et extensibilité.
- **Visual Studio** : Environnement de développement principal.

---

## 📂 Structure du Projet

Voici un aperçu de la structure principale du projet :

```
📂 SpinTheWheel
├── 📁 Models                        # Définit les classes de données (ex. Participant)
├── 📁 ViewModels                    # Logique d'application (MVVM)
├── 📁 Views                         # Interfaces utilisateur (XAML)
|   ├── MainWindow.xaml              # Interface principale avec la roue animée
|   └── ParticipantsWindow.xaml      # Interface pour la gestion des participants
├── 📁 Services                      # Gestion des interactions avec la base de données SQLite
├── 📁 Converters                    # Convertisseurs pour simplifier les bindings WPF
├── Application.xaml                 # Configuration de l'application
```

---

## 📖 Fonctionnement

### 1. **Lancer l'Application**
   - À l'ouverture, l'application initialise automatiquement la base de données (si elle n'existe pas déjà).

### 2. **Roue Principale**
   - Cliquez au centre de la roue pour la faire tourner. Elle ralentira progressivement et affichera le nom du participant sélectionné.

### 3. **Gestion des Participants**
   - Accédez au gestionnaire via un bouton subtil dans le coin supérieur droit ⚙️.
   - Ajoutez, supprimez ou modifiez les participants.
   - Les participants "déjà choisis" ne seront plus sélectionnés (grâce à un flag `Done`).

---

## 🎨 Interface Utilisateur

### Fenêtre Principale
- **Roue Animée** : Une belle roue avec un bouton central interactif.
- **Nom Affiché** : Le participant sélectionné s'affiche en grand après la rotation.
- **Bouton Subtil** : Accès discret à la gestion des participants.

### Gestion des Participants
- **Liste des Participants** : Affiche les noms avec un checkbox indiquant s'ils ont été choisis.
- **Boutons CRUD** :
  - Ajouter un participant via un prompt.
  - Supprimer un participant sélectionné.
  - Sauvegarder les modifications des flags directement dans la base.

---

## ⚡ Fonctionnalités Techniques

- **Base de Données SQLite** :
  - Créée automatiquement si elle n’existe pas.
  - Contient une table `Participant` avec les colonnes suivantes :
    - `Id` : Identifiant unique.
    - `Name` : Nom du participant.
    - `Done` : Booléen indiquant si le participant a été choisi.

- **Animation de la Roue** :
  - Rotation simulée avec un ralentissement progressif.
  - Sélection aléatoire des participants non flagués (`Done = False`).

- **Extensions WPF** :
  - Convertisseurs personnalisés (`NullToVisibility`, `InverseBoolean`) pour simplifier les bindings.

---

## 🌟 À Venir (Roadmap)

Quelques idées pour améliorer encore le projet :
- 🖌️ Ajouter des thèmes personnalisables (clair/sombre).
- 🔊 Effets sonores pour accompagner l'animation de la roue.
- 📊 Statistiques des sélections (nombre de fois choisi par participant).
- 🔄 Mode de réinitialisation pour remettre tous les participants à `Done = False`.

---

## 🛑 Prérequis

- **Windows** : Compatible avec Windows 10 et versions ultérieures.
- **.NET 9.0** : Nécessaire pour exécuter l'application.
- **SQLite** : Bibliothèque incluse automatiquement avec le projet.

---

## 🚴 Instructions pour Exécuter

1. Clonez ce dépôt :
   ```bash
   git clone https://github.com/BlinkSun/SpinTheWheel.git
   ```
2. Ouvrez le projet dans **Visual Studio**.
3. Compilez et exécutez l'application (`Ctrl+F5`).

---

## 🤝 Contribuer

Les contributions sont toujours les bienvenues ! Voici comment vous pouvez aider :
1. Forkez ce dépôt.
2. Créez une branche pour vos modifications :
   ```bash
   git checkout -b feature/amélioration
   ```
3. Effectuez vos modifications et commitez-les :
   ```bash
   git commit -m "Ajout d'une nouvelle fonctionnalité"
   ```
4. Poussez vos modifications :
   ```bash
   git push origin feature/amélioration
   ```
5. Ouvrez une Pull Request. 🛠️

---

## 💌 Remerciements

Un grand merci à tous ceux qui testeront ou contribueront à ce projet. N’hésitez pas à partager vos idées ou vos retours, ils sont toujours les bienvenus !

---

## 🧑‍💻 Auteur

- **Damien Villeneuve (BlinkSun)**  
  Développeur passionné par la programmation, l'astronomie et les sciences en général.  
  [GitHub](https://github.com/BlinkSun)  
