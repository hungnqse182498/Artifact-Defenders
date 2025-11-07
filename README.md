# Artifact

Simple 2D game focused in resource and time management!

Originally made in **72 hours** for both **Ludum Dare 46** and **Torneo GJA Round 1**  
Theme: **"Keep It Alive"**

| ✨ Features | 🎮 Play! | 📸 Screenshots |
|------------|---------:|---------------:|

![Banner](https://svartskogen.com/images/artifact2x1.png)

---

## Code & Features

- **Engine:** Unity **2019.4.9f1** (LTS) — recommended to open the project.
- **Genre:** 2D, resource & time management with light combat mechanics.
- All scripts are documented with `<summary>` tags for quick understanding.
- **ScriptableObject**-based systems for:
  - Localization
  - Item / artifact definitions
  - Balancing parameters
- Simple but functional systems:
  - Player controller and skills (dash, basic attack, etc.)
  - Enemy AI & health/damage system
  - Resource management and time-based events
  - UI with skill buttons and cooldown visuals
- Target platform: **WebGL** (designed for **960×600**), portable to Desktop / Mobile.
- Project follows a lightweight structure — easy to fork & modify.

---

## Demo / Play

You can play the original Ludum Dare build on itch.io:  
➡ https://svartskogen.itch.io/artifact

*(For your fork / Artifact-Defenders, build WebGL or run in the Editor — see instructions below.)*

---

## Screenshots

![Screen 1](http://media.svartskogen.com/artifact/screen1.jpg)  
![Screen 2](http://media.svartskogen.com/artifact/screen2.jpg)

---

## Getting Started (for contributors)

### Requirements
- Unity **2019.4.9f1** (LTS) — project tested on this version.
- Git
- Recommended: Visual Studio or Rider for script editing.

### Clone
```bash
git clone https://github.com/Svartskogen/Artifact.git
# OR if you're using the fork:
git clone https://github.com/hungnqse182498/Artifact-Defenders.git
