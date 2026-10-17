# Voxel Survival 32

This repository contains a playable Unity prototype foundation for a 3D voxel survival game with:
- procedural chunk-based terrain
- survival loop: health + hunger + crafting
- host-client multiplayer prototype scaffold using Mirror
- boss encounter prototype
- simple combat and resource gathering

This is a prototype foundation, not a polished AAA game. It is a solid starting point for further iteration.

## Included prototype systems
- Voxel world generation with chunk iteration around the player
- Player movement, jumping, sprinting, and follow camera
- Health and hunger system
- Basic crafting system with item recipes
- Harvestable resource nodes for wood, stone, and fiber
- Melee combat and boss AI prototype
- Mirror bootstrap script for host-client development
- Game bootstrap that creates world/player/boss at runtime

## Unity version
- Unity 2022.3 LTS

## Install Mirror
Before testing multiplayer, install Mirror in the Unity project through Package Manager or through the official Mirror Git package, depending on your setup.

## How to open the project
1. Install Unity Hub and Unity 2022.3 LTS.
2. Clone or download this repository.
3. Open the folder as a Unity project in Unity Hub.
4. Install Mirror and ensure the project compiles.
5. Open or create a scene named `Game`.
6. Press Play to run the bootstrap.

## Runtime bootstrap
The `GameBootstrap` script automatically creates:
- a VoxelWorldGenerator
- a Player object with CharacterController, PlayerController, SurvivalSystem, and PlayerCombat
- a Boss object with BossController
- a CraftingSystem object

This means the prototype can begin without a manually assembled scene for early testing.

## Important notes
- The world generator is intentionally simple and is designed for prototype iteration, not large-scale optimization.
- The boss is a combat prototype with melee and ranged attack logic.
- Multiplayer logic is intentionally kept lightweight for the prototype stage. Mirror is configured as the foundation for extension.
- For large online worlds or 32-player sessions, you should upgrade to proper chunk streaming, server-authoritative logic, object pooling, and a real room/lobby system.

## Recommended next upgrades
1. Replace cube-per-block generation with mesh chunk generation.
2. Add a visible HUD for health, hunger, inventory, and crafting.
3. Implement server-authoritative Mirror networking for the player and boss state.
4. Add a lobby room manager and matchmaking.
5. Add more world biomes, resources, and bosses.

## License
MIT
