# Goliath Monsters

## Scope

This page documents the large-humanoid mobile package under `Data/Scripts/Mobiles/Goliaths/`.
It covers the troll, ogre, ettin, cyclops, giant, titan, Zorn, and OrkDemigod creature scripts that can be placed or spawned through the normal RunUO mobile surfaces.

The package defines monsters only. It does not register commands, gumps, packet handlers, or standalone world systems.

## Source Scripts

| Family | Scripts | Role |
| --- | --- | --- |
| Trolls | `Troll.cs`, `SeaTroll.cs`, `SwampTroll.cs`, `FrostTroll.cs`, `FrostTrollShaman.cs`, `TrollWitchDoctor.cs` | Low-to-mid tier troll melee and caster variants with corpse-rummaging behavior on most entries. |
| Ogres | `Ogre.cs`, `TundraOgre.cs`, `OgreLord.cs`, `ArcticOgreLord.cs`, `AbysmalOgre.cs`, `OgreMagi.cs` | Ogre bruisers, lords, regional variants, and the mage AI ogre magi. |
| Ettins | `Ettin.cs`, `ArcticEttin.cs`, `EttinShaman.cs`, `AncientEttin.cs` | Two-headed melee variants plus higher-tier shaman and ancient caster variants. |
| Cyclopes | `Cyclops.cs`, `AncientCyclops.cs`, `ShamanicCyclops.cs` | Cyclops melee, ancient, and shaman variants; the ancient and shaman classes add breath attacks. |
| Giants | `Giant.cs`, `HillGiant.cs`, `HillGiantShaman.cs`, `MountainGiant.cs`, `FireGiant.cs`, `FrostGiant.cs`, `ForestGiant.cs`, `JungleGiant.cs`, `SandGiant.cs`, `AbyssGiant.cs`, `CloudGiant.cs`, `DeepSeaGiant.cs`, `IceGiant.cs`, `LavaGiant.cs`, `SeaGiant.cs`, `StarGiant.cs`, `StoneGiant.cs`, `StormGiant.cs` | Broad elemental and terrain-themed giant roster with boulder or elemental breath attacks, treasure-map levels, and frequent luck-gated chest drops. |
| Titans | `Titan.cs`, `ElderTitan.cs` | Mage AI titan variants with energy breath, rich loot packs, Goliath hides, and titan chest drops. |
| Named or boss mobiles | `ZornTheBlacksmith.cs`, `OrkDemigod.cs` | Named high-end encounters with special loot hooks, including Zorn's blacksmith rewards and the OrkDemigod relic-manual drop. |

## Combat Profile

| Tier | Representative classes | Main behavior |
| --- | --- | --- |
| Common bruisers | `Troll`, `SeaTroll`, `Ogre`, `TundraOgre`, `Ettin`, `ArcticEttin` | Melee AI, closest-target fight mode, modest hit point pools, and average or potion loot. |
| Mid-tier goliaths | `Cyclops`, `Giant`, `HillGiant`, `MountainGiant`, `StoneGiant`, `Titan`, `EttinShaman`, `OgreMagi` | Higher stats, treasure-map levels, richer loot packs, and either Goliath hides or caster support. |
| Elemental and regional giants | `FireGiant`, `FrostGiant`, `ForestGiant`, `JungleGiant`, `SandGiant`, `AbyssGiant`, `CloudGiant`, `SeaGiant`, `StarGiant`, `StormGiant` | Terrain-themed visuals and breath effects with mostly rich or filthy-rich loot profiles. |
| Elite encounters | `AncientCyclops`, `AncientEttin`, `DeepSeaGiant`, `IceGiant`, `LavaGiant`, `ElderTitan`, `ZornTheBlacksmith`, `OrkDemigod` | Large hit point pools, stronger resistances, boss-level fame, special chest rolls, and higher treasure-map levels. |

Most classes inherit `BaseCreature` with `FightMode.Closest`, perception range `10`, fight range `1`, active speed `0.2`, and passive speed `0.4`.
Melee AI is used for direct bruisers, while shamans, mage variants, titans, and many themed giants use mage AI.

## Reward And Resource Hooks

| Reward hook | Behavior |
| --- | --- |
| Loot packs | Common entries use `Average`, `Meager`, `Rich`, or potion packs; caster and elite entries add scroll packs, gems, or multiple `FilthyRich` rolls. |
| Treasure maps | Most goliath scripts override `TreasureMapLevel`, commonly from level `3` through level `6` for the OrkDemigod. |
| Goliath hides | Titans, cyclopes, ettins, and most giant variants expose `Hides = 18` with `HideType.Goliath`. |
| Luck-gated chests | Many giants and titans roll `GetPlayerInfo.LuckyKiller()` on death before dropping themed `LootChest` rewards. |
| Stone giant granite | `StoneGiant` chooses a material hue after spawn and drops the matching granite resource on death. |
| OrkDemigod manual | `OrkDemigod` can drop a `ManualOfItems` named `Chest of Orcish Relics` once the killer passes the luck and special-kill gates. |
| Zorn rewards | `ZornTheBlacksmith` packs a `RubyPickaxe` and can carry a Caddellite-themed loot chest. |

## Admin Notes

These mobiles are normal `[Constructable]` creature classes, so staff can place them with the standard add command for each class name.
The package does not include a dedicated spawner, region controller, command, or configuration gump.

All inspected classes use RunUO mobile serialization with version `0` and no class-specific serialized fields beyond base mobile state.
Custom runtime state such as loot chest rolls, StoneGiant hue material selection, and one-time boss reward checks is handled during construction, spawn, or death rather than through additional saved fields.

## Known Issues

| Issue | Impact |
| --- | --- |
| `ShamanicCyclops` sets strength, dexterity, intelligence, damage, resistances, and skills, but does not call `SetHits()`. | Its live hit point pool may fall back to base mobile behavior instead of matching its intended cyclops-shaman tier. |
| Several mage-AI giant variants call `SetMana(0)` after configuring intelligence. | If mage AI depends on available mana in this shard build, those variants may behave more like melee breath users than full casters. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0092.
- Backlog rows: RB-06696.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Mobiles/Goliaths/ (CurrentDirectory)

### Runtime Evidence

- No runtime hook rows matched the reviewed source set in runtime-hook-map.csv.

### Serialization Evidence

- Serialized rows matched: 41.
- Data/Scripts/Mobiles/Goliaths/Cyclops/AncientCyclops.cs:Server.Mobiles.AncientCyclops version=0 serialize=L153 deserialize=L159 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Cyclops/Cyclops.cs:Server.Mobiles.Cyclops version=0 serialize=L267 deserialize=L273 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Cyclops/ShamanicCyclops.cs:Server.Mobiles.ShamanicCyclops version=0 serialize=L125 deserialize=L131 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Ettins/AncientEttin.cs:Server.Mobiles.AncientEttin version=0 serialize=L85 deserialize=L91 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Ettins/ArcticEttin.cs:Server.Mobiles.ArcticEttin version=0 serialize=L127 deserialize=L133 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Ettins/Ettin.cs:Server.Mobiles.Ettin version=0 serialize=L127 deserialize=L133 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Ettins/EttinShaman.cs:Server.Mobiles.EttinShaman version=0 serialize=L84 deserialize=L90 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Giants/AbyssGiant.cs:Server.Mobiles.AbyssGiant version=0 serialize=L171 deserialize=L177 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Giants/CloudGiant.cs:Server.Mobiles.CloudGiant version=0 serialize=L175 deserialize=L181 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Giants/DeepSeaGiant.cs:Server.Mobiles.DeepSeaGiant version=0 serialize=L173 deserialize=L179 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Giants/FireGiant.cs:Server.Mobiles.FireGiant version=0 serialize=L205 deserialize=L211 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Goliaths/Giants/ForestGiant.cs:Server.Mobiles.ForestGiant version=0 serialize=L181 deserialize=L187 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Mobiles/Goliaths/Cyclops/AncientCyclops.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Cyclops/AncientCyclops.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Cyclops/Cyclops.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Cyclops/Cyclops.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Cyclops/ShamanicCyclops.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Cyclops/ShamanicCyclops.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/AncientEttin.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/AncientEttin.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/ArcticEttin.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/ArcticEttin.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/Ettin.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/Ettin.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/EttinShaman.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Ettins/EttinShaman.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Giants/AbyssGiant.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Giants/AbyssGiant.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Giants/CloudGiant.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Giants/CloudGiant.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Giants/DeepSeaGiant.cs=Keep
- Data/Scripts/Mobiles/Goliaths/Giants/DeepSeaGiant.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
