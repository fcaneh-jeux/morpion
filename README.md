# 🎮 Morpion (Tic-Tac-Toe) – Godot C#

## 📌 Présentation

Ce projet est une implémentation du jeu du morpion (tic-tac-toe) développée avec **Godot Engine (C#)**.

Il propose plusieurs modes de jeu ainsi qu’une intelligence artificielle à deux niveaux de difficulté.

---

## 🚀 Fonctionnalités

* ✔ Mode **Joueur vs Joueur**
* ✔ Mode **Joueur vs IA**
* ✔ IA niveau **Dur** (heuristique : blocage, victoire, fourchettes)
* ✔ IA niveau **Très dur** (algorithme **Minimax** – imbattable)
* ✔ Interface utilisateur avec menu de sélection
* ✔ Bouton de retour au menu
* ✔ Export Windows (.exe)

---

## 🧠 Intelligence artificielle

Deux approches sont implémentées :

### 🔹 IA "Dur" (heuristique)

* Cherche un coup gagnant
* Bloque l’adversaire
* Gère les situations de fourchette
* Utilise des positions stratégiques (centre, coins)

### 🔹 IA "Très dur" (Minimax)

* Explore récursivement tous les coups possibles
* Évalue chaque état de jeu
* Garantit un résultat optimal (ne peut pas perdre)

---

## 🛠️ Technologies utilisées

* **Godot Engine 4 (Mono)**
* **C# (.NET)**
* Architecture orientée objet simple

---

## 🧩 Architecture du projet

* `Game.cs` → logique du jeu (plateau, règles, IA)
* `Grid.cs` → gestion de la grille et interactions
* `Menu.cs` → interface de sélection (mode + difficulté)
* `scenes/` → scènes Godot (Menu, Main, Cell)

Le projet sépare clairement :

* la **logique métier** (Game)
* la **présentation / UI** (Grid, Menu)

---

## 🎮 Télécharger le jeu

👉 [Télécharger Morpion v1.0](https://github.com/fcaneh-jeux/morpion/releases/download/v1.0.0/morpion.exe)

---

## ⚠️ Note importante sur la réalisation

Ce projet a été développé **avec l’accompagnement de ChatGPT (OpenAI)**.

### Ce que cela signifie concrètement :

* 🧠 **Conception et logique** :

  * Les idées de gameplay, les choix d’architecture et la progression du projet viennent de moi
  * J’ai défini les besoins, les fonctionnalités et les évolutions

* 💬 **Support technique et pédagogique** :

  * ChatGPT m’a aidé à :

    * comprendre certains concepts (callbacks, IA, minimax…)
    * corriger des erreurs
    * améliorer certaines implémentations
    * proposer des pistes de refactorisation

* 🧱 **Code** :

  * Le code est le résultat :

    * de mes implémentations
    * * des ajustements, corrections et suggestions proposées par ChatGPT

👉 En résumé :

> Ce projet est une **collaboration humain + IA**, où j’ai piloté la logique, les choix et l’intégration, avec un support technique pour accélérer l’apprentissage et la résolution de problèmes.

---

## 📈 Objectif du projet

* Approfondir **C# avec Godot**
* Comprendre et implémenter une **IA (heuristique + minimax)**
* Structurer un projet complet (UI + logique + export)
* Produire un projet présentable dans un **portfolio développeur**

---

## 📸 Aperçu

*(Tu peux ajouter ici des screenshots du jeu)*

---

## 📬 Auteur

Projet réalisé par **Fabien Canehan**

---

## 🔄 Améliorations possibles

* Ajout d’animations
* Interface plus avancée (UI/UX)
* Score / historique des parties
* Version web ou mobile
