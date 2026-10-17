# Voxel Survival 32 — Unity Prototype

This repository contains a playable Unity prototype scaffold for a 3D voxel survival game with:
- procedural chunk-based terrain
- survival loop: health + hunger + crafting
- host-client multiplayer skeleton using Mirror
- boss encounter prototype
- simple combat and world generation

Important: This is a prototype foundation, not a polished AAA game. It is meant to be a solid starting point for further iteration.

## Features included in the prototype
- Voxel world generator with chunk iteration around the player
- Player movement, sprinting, jumping, camera follow
- Health and hunger system
- Basic crafting system with simple recipes
- Melee combat
- Boss controller and basic boss AI
- Mirror networking bootstrap for host-client play
- Documentation for setup and next-steps

## Unity version
- Unity 2022.3 LTS

## How to open the project
1. Install Unity Hub and Unity 2022.3 LTS.
2. Clone or download this repo.
3. Open the folder as a Unity project.
4. In Unity Package Manager, install Mirror from the official package source or Git package URL.
5. Add a new scene called `Game`.
6. Create a default `NetworkManager` and assign the `Player` prefab.
7. Drag `VoxelWorldGenerator` and `MirrorBootstrap` into a scene object.
8. Press Play to test.

## Recommended Mirror setup
- Install Mirror via UPM.
- Create a `NetworkManager` GameObject.
- Use the included `MirrorBootstrap` script to start a host automatically in editor testing.
- Assign a `Player` prefab with `PlayerController` + `SurvivalSystem` + `PlayerCombat`.
- Spawn the `Boss` via a GameObject with `BossController`.

## Main scripts
- `Assets/Scripts/World/VoxelWorldGenerator.cs`
- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Player/SurvivalSystem.cs`
- `Assets/Scripts/Combat/PlayerCombat.cs`
- `Assets/Scripts/Combat/BossController.cs`
- `Assets/Scripts/Systems/CraftingSystem.cs`
- `Assets/Scripts/Network/MirrorBootstrap.cs`

## Notes
- The world generator is chunk-based and procedural, but intentionally lightweight for prototype use.
- Boss logic is intentionally simple so it is easy to expand.
- Since this is a prototype, you will need to assign prefabs and scene objects manually in the editor to fully test gameplay.

## Suggested next milestones
1. Add proper voxel block pooling and chunk unload management.
2. Add crafting UI and inventory UI.
3. Add fish/food gathering and resource nodes.
4. Replace fake boss AI with attack patterns and arena logic.
5. Add a lobby system and room management.
6. Introduce world streaming for larger scale open world.

## License
MIT
