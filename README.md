# CityWheel

> Jeu sérieux 3D de **sensibilisation à l'accessibilité urbaine** : le joueur, en fauteuil roulant, doit rejoindre l'hôpital par un chemin réellement accessible.

Projet réalisé dans le cadre du cours **« Services et technologies multimédia »**  
 Université de Genève (UNIGE), printemps 2026.

---

## Le concept

Le but n'est pas la difficulté « arcade », mais de **faire vivre les barrières d'accessibilité** du quotidien d'une personne à mobilité réduite : trottoirs sans rampe, passages fermés, détours imposés, obstacles à contourner.

Le jeu distingue deux familles de difficultés, porteuses de deux messages :

- **Les barrières d'infrastructure** (passages fermés, bordures) → faire demi-tour et chercher un chemin accessible.

- **Les obstacles / incivilités** (trottinettes, véhicules mal garés) → contourner.

> Idée centrale : l'adaptation devrait venir de **l'environnement**, pas de la personne handicapée. L'aide adaptative du jeu agit sur la difficulté de manipulation, jamais sur le message.

---
 
## Démonstration (Vidéo)
 
▶️ **[Vidéo de démonstration du gameplay](https://youtu.be/U3Xblcu_kU4)**

---

## Comment jouer

| Action | Touches |
|---|---|
| Avancer / Reculer | `W` / `S` (ou ↑ / ↓) |
| Tourner | `A` / `D` (ou ← / →) |

**Objectif :** atteindre l'**hôpital** en suivant un chemin accessible. Une **flèche directrice** pointe en permanence vers la destination.

---

## Fonctionnalités

- **Parcours accessible** sous forme de labyrinthe de colliders (trottoirs roulables, bordures et passages fermés bloquants, rampes).

- **Messages pédagogiques colorés** selon la situation (mission, passage fermé, collision, adaptation).

- **Adaptativité bidirectionnelle** (voir ci-dessous).

- **Pop-up « COMPRIS »** qui met le jeu en pause quand la ville s'adapte, pour rendre l'adaptation impossible à manquer.

- **Deux chemins accessibles** vers l'hôpital + écran de fin qui indique « chemin 1 sur 2 » pour encourager à **rejouer**.

- **Écran d'introduction** (contexte + tutoriel) et **écran de fin / débrief** (temps, obstacles rencontrés, passages fermés, message de sensibilisation, bouton Rejouer).

- **Chronomètre** dont la couleur évolue avec le seuil d'adaptativité.

- **Compagnon** : un chat (modèle OpenSCAD) à retrouver sur le parcours.

---

## L'adaptativité (cœur du jeu)

Le jeu ajuste la difficulté **dans les deux sens** pour maintenir l'équilibre :

- **Joueur trop à l'aise** (à 12 s sans collision) → des obstacles cachés apparaissent un par un, **devant** le joueur, jusqu'à 9 maximum. *La ville se complexifie.*

- **Joueur en difficulté** (3 collisions) → des obstacles secondaires sont retirés et l'ajout de difficulté est désactivé définitivement. *La ville facilite le parcours.*

Un **anti-rebond** garantit qu'un contact ne compte que pour une seule collision. **Choix pédagogique fort :** les passages fermés (barrières d'accessibilité) ne comptent **jamais** comme une collision — ils sont le propos du jeu, pas une faute.

---

## Technique

- **Moteur :** Unity 6.3 LTS (6000.3.11f1) — pipeline de rendu **URP**.

- **Scène principale :** `Assets/Scenes/Scene_Principale.unity`.

- **Scripts :** `Assets/MatosImportee/Scripts/` — déplacement (`WheelchairController`), caméra (`SimpleCameraFollow`), adaptativité (`AdaptiveManager`, `AdaptPopupManager`), messages (`ObstacleMessage`, `PassageMessage`, `ApparitionProximite`), objectif & fin (`GoalTrigger`, `EndManager`, `ChronoDisplay`), chemins (`CheminTrigger`), orientation (`FlecheDirectrice`), compagnon (`CatTrigger`), interface (`IntroManager`).

- **Modèles OpenSCAD :** hôpital et chat modélisés en OpenSCAD, exportés en STL puis convertis en OBJ (via Blender, rotation −90° en X) avant import dans Unity.

---

## Limites & pistes d'amélioration
 
Choix assumés dans la version actuelle (cohérents avec le cadre et la deadline) :

- **Un seul niveau / une seule ville** → les deux chemins offrent déjà de la rejouabilité ; l'étape suivante serait plusieurs quartiers.

- **Physique du fauteuil volontairement simple** (déplacement à plat, colliders plutôt que physique de pente) → privilégie la robustesse et le propos.

- **Génération procédurale simulée** (obstacles pré-placés puis révélés) plutôt que réellement aléatoire.

- **Interface volontairement sobre** → la prochaine étape, par cohérence avec le propos, serait une **interface elle-même accessible** : réglage de la taille des textes, du contraste, et **contrôles reconfigurables**.
Autres pistes : ajout d'une **mini-carte**, d'un **menu principal**, et nettoyage de quelques chevauchements de tuiles de route.
 
---

## Ouvrir le projet

1. Installer **Unity Hub** et la version **6.3 LTS (6000.3.11f1)**.
2. Cloner le dépôt :
   ```bash
   git clone https://gitlab.unige.ch/citywheel/citywheel.git
   ```
3. Ouvrir le dossier du projet via Unity Hub.
4. Ouvrir la scène `Assets/Scenes/Scene_Principale.unity` et appuyer sur **Play**.

---

## Auteurs

- **Ramine Yunus**
- **Menna Mahmoud**

Cours « Services et technologies multimédia »  Université de Genève (UNIGE), 2026.

---

## Crédits & licences des assets

**Kenney** - packs sous licence **CC0** (domaine public) :
- City Kit (Commercial) - https://kenney.nl/assets/city-kit-commercial
- City Kit (Roads) - https://kenney.nl/assets/city-kit-roads
- City Kit (Suburban) - https://kenney.nl/assets/city-kit-suburban
- City Kit (Industrial) - https://kenney.nl/assets/city-kit-industrial
- Coaster Kit - https://kenney.nl/assets/coaster-kit

**Unity Asset Store**
- Fauteuil roulant : **« WhellChair »** par **AK STUDIO ART** - gratuit, *Standard Unity Asset Store EULA* - https://assetstore.unity.com/packages/3d/props/whellchair-260547

**Sketchfab**
- Trottinette électrique (obstacles) : **« Scooter Low-Poly »** par **RayznGames** - licence **CC BY 4.0** *(attribution obligatoire)* - https://sketchfab.com/3d-models/scooter-low-poly-3a02fbfa5bf142c9bdace5f8bb5bd1d9

**Modèles réalisés par les auteurs**
- Hôpital et chat - modélisés en **OpenSCAD**.

---

*Prototype développé dans un cadre pédagogique. Objectif : sensibiliser à l'accessibilité urbaine et au handicap moteur.*