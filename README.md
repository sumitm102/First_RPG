# 2D RPG Game - Unity Showcase

![Unity](https://img.shields.io/badge/Unity-6.0-blue?logo=unity&logoColor=white) ![C#](https://img.shields.io/badge/C%23-Visual_Studio-blue?logo=c-sharp&logoColor=white)

A 2D action RPG built in Unity, showcasing dynamic player abilities, elemental effects, and a flexible stat-based system. This project demonstrates gameplay mechanics, AI state machines, and an extensible skill system.

---

## Table of Contents
- [Gameplay](#gameplay)
  - [Player Mechanics](#player-mechanics)
  - [Enemy Mechanics](#enemy-mechanics)
  - [Buffs and Modifiers](#buffs-and-modifiers)
- [Technical Details](#technical-details)
- [Planned Features](#planned-features)
- [Contributing](#contributing)

---

## Gameplay

### Player Mechanics
- **Movement:** Walk, run, jump, dash.
- **Combat Skills:**
  - Basic attack
  - Jump attack
  - Advanced skills: create shards, summon clones, throw sword at enemies
- **Stats System:** Upgradeable stats affecting attack, defense, speed, and other attributes.
- **Elemental Effects:**
  - **Fire:** Burn damage over time
  - **Ice:** Slows enemies temporarily
  - **Lightning:** Builds charge with small hits; final hit triggers extra lightning damage
- **Resources:** Player has health and other resource systems for skill usage.

### Enemy Mechanics
- **AI:** Enemies use state machines for movement and combat decisions.
- **Resources:** Health and other stats similar to the player.
- **Future Plans:** Different enemy types with unique behaviors.

### Buffs and Modifiers
- Stats can be temporarily modified through buffs.
- Adds strategic depth and allows dynamic gameplay adjustments.

---

## Technical Details
- **Language:** C#  
- **Unity Version:** 6  
- **Design Patterns:**
  - State Machines for AI and player behavior
  - **ScriptableObjects** for secure and reusable data
  - **Singleton Pattern** for global managers
  - **Observer Pattern** for event-based input handling
- **Assets:** Third-party visual and audio assets for sprites, animations, and sounds.
- **Purpose:** Showcase of gameplay mechanics and architecture for future RPG projects.

---

## Planned Features
- Multiple dungeon levels
- Quest system
- Save/load game system
- Dialogue system
- Additional enemies
- More advanced player skills

