# Taxidermy

Taxidermy is a collection of Item-driven trophy systems. It has no dedicated command registration. Players and staff interact with constructable Items, corpse state, context-menu targets, house addon placement, and passive death-drop hooks.

## Script Inventory

| Script | Runtime role |
| --- | --- |
| `Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs` | Legacy taxidermy kit, `TrophyDeed`, and wall-mounted `TrophyAddon` placement. |
| `Data/Scripts/Trades/Taxidermy/TrophyBase.cs` | Custom trophy base that turns fish or supported corpses into `MountedTrophyHead` Items. |
| `Data/Scripts/Trades/Taxidermy/MountingBase.cs` | Custom mounting base that turns supported corpses into large mounted `StuffedBear` display Items. |
| `Data/Scripts/Trades/Taxidermy/StuffingBasket.cs` | Custom stuffing basket that turns supported corpses into stuffed displays or tiger rug deeds. |
| `Data/Scripts/Trades/Taxidermy/MountedTrophyHead.cs` | Flippable wall-head trophy Item with killer/location properties. |
| `Data/Scripts/Trades/Taxidermy/StuffedBear.cs` | Generic stuffed or mounted trophy display Item with killer/location properties. |
| `Data/Scripts/Trades/Taxidermy/MagicSkulls.cs` | Passive skull, demon-head, vampire-head, and elemental-heart trophy drops. |
| `Data/Scripts/Mobiles/Base/DropRelic.cs` | Death-drop integration point for stuffed trophies, rugs, skulls, and hearts. |
| `Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs` | Persists `VisitedByTaxidermist`, the corpse reuse guard. |

## Item Entry Points

| Item | Use path | Target | Requirements | Consumed on success | Output |
| --- | --- | --- | --- | --- | --- |
| `TaxidermyKit` | Double-click while in backpack | `Corpse` or `BigFish`, target range 3 | Kit in backpack, `Carpentry.Base >= 90.0`, and 10 `Board` in backpack | 10 boards and the kit | `TrophyDeed` |
| `TrophyDeed` | Double-click while in backpack | No target; uses caller position and facing | Caller must be in a house where they are co-owner; north or west adjacent wall required | The deed | Immovable `TrophyAddon` added to the house addon list |
| `TrophyAddon` | Double-click in world | No target | Caller must be house co-owner and within 1 tile | The addon | Matching `TrophyDeed` returned to backpack |
| `TrophyBase` | Single-click context entry | `Fish`, `BigFish`, `NewFish`, or supported `Corpse`, target range 3 | Context caller must be `PlayerMobile`; corpse must not be `VisitedByTaxidermist` | The base, and fish Items if a fish was targeted | `MountedTrophyHead` in backpack |
| `MountingBase` | Single-click context entry | Supported `Corpse`, target range 3 | Context caller must be `PlayerMobile`; corpse must not be `VisitedByTaxidermist` | The base | `StuffedBear` named `mounted trophy of ...` in backpack |
| `StuffingBasket` | Single-click context entry | Supported `Corpse`, target range 3 | Context caller must be `PlayerMobile`; corpse must not be `VisitedByTaxidermist` | The basket | `StuffedBear` named `stuffed trophy of ...` or a tiger rug deed in backpack |

Double-clicking `TrophyBase`, `MountingBase`, or `StuffingBasket` opens a `SpeechGump` help page instead of starting the target cursor.

## Corpse Reuse Guard

Taxidermy uses `Corpse.VisitedByTaxidermist` as the shared one-use flag. `TaxidermyKit`, `TrophyBase`, `MountingBase`, `StuffingBasket`, and some other corpse systems check the flag before extracting a trophy. Successful corpse trophy creation sets the flag to `true`.

Fish targets are not corpses. `TrophyBase` deletes targeted fish Items and its base Item, but it does not use `VisitedByTaxidermist`.

## Legacy Taxidermy Kit

`TaxidermyKit` supports only the built-in table below. It compares `m_Table[i].CreatureType == obj.GetType()`, so subclasses are not accepted unless listed exactly.

| Target type | North item ID | West item ID | Deed label | Addon label |
| --- | ---: | ---: | ---: | ---: |
| `BrownBear` | `0x1E60` | `0x1E67` | `1041093` | `1041107` |
| `GreatHart` | `0x1E61` | `0x1E68` | `1041095` | `1041109` |
| `BigFish` | `0x1E62` | `0x1E69` | `1041096` | `1041110` |
| `Gorilla` | `0x1E63` | `0x1E6A` | `1041091` | `1041105` |
| `Orc` | `0x1E64` | `0x1E6B` | `1041090` | `1041104` |
| `PolarBear` | `0x1E65` | `0x1E6C` | `1041094` | `1041108` |
| `Troll` | `0x1E66` | `0x1E6D` | `1041092` | `1041106` |

When a `BigFish` is targeted, the kit reads `BigFish.Fisher` and casts `BigFish.Weight` to `int`, then consumes the fish. Those values are intended for `TrophyDeed` and `TrophyAddon` property display when weight is at least 20 stones.

`TrophyDeed` placement chooses north or west art by checking adjacent walls. If both wall checks pass, the caller must face north/south for a north-wall trophy or east/west for a west-wall trophy. Diagonal facing displays "Turn to face the wall on which to hang this trophy."

## TrophyBase Fish Outputs

| Target | Output Item | Name | ItemID | Hue |
| --- | --- | --- | ---: | --- |
| `Fish` or `BigFish` | `MountedTrophyHead` | `mounted fish` | `0x1E69` | Default |
| `NewFish` | `MountedTrophyHead` | `mounted ` + fish name | `0x44E8` | Target fish hue |

## TrophyBase Corpse Head Outputs

`TrophyBase` creates one `MountedTrophyHead`, sets its hue and item ID, names it `mounted head of ` plus the corpse name and owner title, adds region provenance with `Worlds.GetRegionName(from.Map, from.Location)`, and adds killer text only when `Corpse.m_Killer` is a `PlayerMobile`.

| Target mobile classes or condition | ItemID | Hue rule |
| --- | ---: | --- |
| `AncientWyrm`, `CaddelliteDragon`, `DragonKing`, `SlasherOfVoid`, `VolcanicDragon`, `AshDragon`, `BottleDragon`, `RadiationDragon`, `CrystalDragon`, `VoidDragon`, `ElderDragon`, `DeepSeaDragon`, `ShadowWyrm`, `ZombieDragon` | `0x21FB` | Corpse hue, except `ShadowWyrm` `0x966`, `DragonKing` `0xA65` |
| `DrakkhenRed`, or `Dragoon` with `Corpse.Amount == 604` | `0x65AD` | `0xB01` |
| `DrakkhenBlack`, or other `Dragoon` | `0x6553` | `0` |
| `Dragons`, `Wyrm`, `RidingDragon`, `GemDragon`, `GhostDragyn`, `GrayDragon`, `BlueDragon`, `MetalDragon`, `Dragon`, `StoneDragon`, `WhiteDragon`, `BlackDragon`, `AsianDragon`, `GreenDragon`, `DragonGolem` | `0x270D` | Corpse hue, except `Dragon` `0x9A2` |
| `Wyrms` with `Corpse.Amount == 12` or `46` | `0x270D` | Corpse hue |
| Other `Wyrms` | `0x33FD` | Corpse hue |
| `NightWyrm`, `OnyxWyrm`, `EmeraldWyrm`, `AmethystWyrm`, `SapphireWyrm`, `GarnetWyrm`, `TopazWyrm`, `RubyWyrm`, `SpinelWyrm`, `QuartzWyrm`, `JungleWyrm`, `DesertWyrm`, `MountainWyrm`, `IceDragon`, `LavaDragon`, `WhiteWyrm` | `0x393B` | Corpse hue |
| `Lizardman`, `Reptaur`, `LizardmanArcher` | `0x393F` | Corpse hue |
| `Sakleth`, `MutantLizardman`, `Grathek`, `Sleestax`, `SaklethArcher`, `SaklethMage`, `Reptalar`, `ReptalarShaman`, `ReptalarChieftain` | `0x33AB` | Corpse hue |
| `Goblin`, `GoblinArcher` | `0x3937` | Corpse hue |
| `Ratman`, `RatmanMage`, `RatmanArcher` | `0x392B` | Corpse hue |
| `Bugbear` | `0x3935` | Corpse hue |
| `MinotaurCaptain`, `RottingMinotaur`, `MutantMinotaur`, `MinotaurSmall`, `MinotaurScout`, `Minotaur` | `0x3944` | Corpse hue |
| `Cyclops`, `ZornTheBlacksmith`, `ShamanicCyclops` | `0x3931` | Corpse hue |
| `StoneGiant`, `IceGiant`, `LavaGiant`, `MountainGiant` | `0x3912` | Corpse hue |
| `UndeadGiant` | `0x21F9` | Corpse hue |
| `Titan`, `ElderTitan` | `0x21F7` | Corpse hue |
| `TundraOgre`, `OgreMagi`, `Ogre` | `0x33E1` | Corpse hue |
| `Ettin`, `ArcticEttin`, `AncientEttin` | `0x393D` | Corpse hue |
| `EttinShaman` | `0x33A7` | Corpse hue |
| `HillGiant`, `HillGiantShaman` | `0x33A9` | Corpse hue |
| Gargoyle group: `FireGargoyle`, `Gargoyle`, `AncientGargoyle`, `GhostGargoyle`, `MutantGargoyle`, `CosmicGargoyle`, `SpectralGargoyle`, `ZombieGargoyle`, `SkeletalGargoyle`, `GargoyleBones`, `GargoyleMarble`, `StygianGargoyle`, `StygianGargoyleLord`, `CodexGargoyleA`, `CodexGargoyleB`, `GargoyleWarrior`, `StoneGargoyle`, `ShadowDemon` | `0x3933` | Corpse hue |
| Demon/gem gargoyle group: `GargoyleRuby`, `GargoyleEmerald`, `GargoyleAmethyst`, `GargoyleSapphire`, `Tarjan`, `BloodDemigod`, `Xurtzar`, `AbysmalDaemon`, `DeepSeaDevil`, `Devil`, `BloodDemon`, `Demon`, `FireDemon`, `Daemonic`, `DaemonTemplate`, `Daemon` | `0x567F` | Corpse hue, except `DeepSeaDevil` `1365` |
| `Balron` | `0x5681` | Corpse hue |
| `Archfiend` | `0x5681` | `0xB1E` |
| `Fiend` | `0x567F` | `0xB1E` |
| `GargoyleOnyx`, `BlackGateDemon` | `0x392C` | `0` |
| `BrownBear`, `GrizzlyBearRiding`, `GrizzlyBear` | `0x1E67` | Corpse hue |
| `SabretoothBear`, `SabretoothBearRiding`, `DeathBear`, `DireBear`, `ElderBrownBear`, `ElderBrownBearRiding`, `GreatBear` | `0x339B` | Corpse hue |
| `CaveBear`, `CaveBearRiding` | `0x339D` | Corpse hue |
| `ElderBlackBear`, `ElderBlackBearRiding`, `BlackBear`, `SabreclawCub`, `KodiakBear` | `0x3399` | `0` |
| `PolarBear` | `0x1E6C` | `0` |
| `ElderPolarBear`, `ElderPolarBearRiding` | `0x339F` | Corpse hue |
| `OgreLord`, `ArcticOgreLord` | `0x3378` | Corpse hue |
| `Cerberus` | `0x335A` | Corpse hue |
| `Drake` | `0x3368` | Corpse hue |
| `SwampDrake`, `SwampDrakeRiding` | `0x3385` | Corpse hue |
| `AncientDrake` | `0x3358` | Corpse hue |
| `Owlbear` | `0x337B` | Corpse hue |
| `AbysmalOgre` | `0x3354` | Corpse hue |
| `SeaDrake` | `0x3381` | Corpse hue |
| `AncientCyclops` | `0x3356` | Corpse hue |
| `StormGiant` | `0x335C` | Corpse hue |
| `CloudGiant` | `0x335C` | `0xB70` |
| `StarGiant` with `Corpse.Amount == 770` | `0x3352` | `0xB73` |
| Other `StarGiant` | `0x3374` | `0xB73` |
| `DemonOfTheSea` | `0x337D` | Corpse hue |
| `DragonGhost` | `0x337F` | Corpse hue |
| `Tiger`, `SabretoothTiger`, `SabretoothTigerRiding`, `TigerRiding` | Random `0x3389` or `0x6549` | `0` |
| `WhiteTiger`, `WhiteTigerRiding` | `0x654F` | `0` |
| `PredatorHellCat`, `PredatorHellCatRiding` | `0x3389` | Corpse hue |
| `Lion`, `LionRiding` | `0x654D` | `0` |
| `Panther` | `0x6551` | `0` |
| `SnowLion`, `Manticore`, `Chimera` | `0x3376` | Corpse hue |
| `Exodus` | `0x5681` | Corpse hue |
| `Wyvra`, `Hydra`, `EnergyHydra` | `0x3372` | Corpse hue, except `Hydra` `0xA5D` |
| `SkeletalDragon` | `0x33B3` | Corpse hue |
| `Dracolich` | `0x3364` | Corpse hue |
| `SwampThing` | `0x3387` | Corpse hue |
| `Griffon`, `GriffonRiding` | `0x3370` | Corpse hue |
| `Walrus` | `0x33DF` | Corpse hue |
| `Meglasaur` | `0x3362` | Corpse hue |
| `Stegosaurus` | `0x3393` | Corpse hue |
| `Tyranasaur` | `0x3391` | Corpse hue |
| `DragonTurtle` | `0x3366` | Corpse hue |
| `DeepSeaGiant` | `0x3360` | Corpse hue |
| `Trollbear` | `0x33E3` | Corpse hue |
| `Satan` | `0x33AF` | Corpse hue |
| `SeaGiant` | `0x3383` | Corpse hue |
| `SandGiant`, `AbyssGiant` | `0x3352` | Corpse hue, except `SandGiant` `0x96D` |
| `JungleGiant` | `0x3374` | Corpse hue |
| `ForestGiant`, `FireGiant` | `0x336C` | Corpse hue, except `FireGiant` `0xA93` |
| `ZombieGiant`, `FleshGolem`, `AncientFleshGolem` | `0x336A` | Corpse hue |
| `FrostGiant` | `0x336E` | Corpse hue |
| `AntaurKing`, `AntaurProgenitor`, `AntaurSoldier`, `AntaurWorker` | `0x654B` | `0` |
| `SeaTroll`, `FrostTroll`, `FrostTrollShaman`, `SwampTroll`, `TrollWitchDoctor`, `Troll` | `0x1E6D` | Corpse hue |
| `Orc`, `OrcBomber`, `OrcCaptain`, `OrcishLord`, `OrcishMage`, `Urk`, `UrkShaman`, `Urc`, `UrcShaman`, `UrcBowman`, `OrkMage`, `OrkMonks`, `OrkRogue`, `OrkWarrior` | `0x1E6B` | `0` |
| `GreatHart`, `Antelope` | `0x1E68` | Corpse hue |
| `Gorilla`, `Infected`, `Gorakong`, `Ape` | `0x1E6A` | `Infected` keeps corpse hue; others use `0` |
| `Yeti` | `0x1E6A` | `0x47E` |
| `Pixie`, `Sprite`, `Faerie` | Random one of `0x2A79`, `0x2A75`, `0x2A71`, `0x2A77`, `0x2A73` | Corpse hue |
| `Unicorn`, `Dreadhorn` | `0x33B1` | Corpse hue |
| `DarkUnicorn`, `DarkUnicornRiding` | `0x335E` | Corpse hue |
| `Nightmare`, `AncientNightmareRiding`, `AncientNightmare`, `Placeron` | `0x33AD` | Corpse hue |
| `Wyvern`, `Wyverns`, `Teradactyl` | `0x33B5` | Corpse hue |
| `AncientWyvern` | `0x3397` | Corpse hue |
| `IceDevil` | `0x338F` | Corpse hue |
| `Xenomorph` | `0x33B9` | `0` |
| `Hippogriff`, `HippogriffRiding` | `0x33DD` | Corpse hue |
| `HellBeast` | `0x33DB` | Corpse hue |
| `Styguana` | `0x33B7` | Corpse hue |
| `Watcher` | `0x33BB` | Corpse hue |
| `CragCat` | `0x33BD` | Corpse hue |
| Primeval dragon variants: `PrimevalGreenDragon`, `PrimevalNightDragon`, `PrimevalRedDragon`, `PrimevalRoyalDragon`, `PrimevalRunicDragon`, `PrimevalSeaDragon`, `ReanimatedDragon`, `VampiricDragon`, `PrimevalAbysmalDragon`, `PrimevalAmberDragon`, `PrimevalBlackDragon`, `PrimevalDragon`, `PrimevalSilverDragon`, `PrimevalVolcanicDragon`, `PrimevalFireDragon`, `PrimevalStygianDragon` | Class-specific IDs `0x33C1`, `0x33D3`, `0x33C5`, `0x33C3`, `0x33BF`, `0x33C7`, `0x33D5`, `0x33CD`, `0x33CB`, `0x33D1`, `0x33A3`, `0x33D9`, `0x33A5`, `0x33CF`, `0x33D7`, `0x33C9` | Corpse hue |
| `TitanLithos`, `TitanPyros`, `TitanHydros` | `0x338B`, `0x338D`, `0x338F` respectively | Corpse hue |

## MountingBase Outputs

`MountingBase` creates a `StuffedBear` display named `mounted trophy of ...`. These outputs are Items in the player's backpack, not `BaseAddon` house components.

| Target mobile classes or condition | ItemID | Hue rule |
| --- | ---: | --- |
| `YoungRoc`, `Roc` | `0x6566` | Default |
| `GiantHawk`, `GiantRaven`, `Phoenix` | Random `0x65B3` or `0x65B4` | Corpse hue |
| `Jormungandr` | Random `0x4D0A` or `0x4D0B` | Default |
| `Basilosaurus` | Random `0x655C` or `0x655D` | `0xB70` |
| `GiantEel`, `SeaSerpent`, `DeepSeaSerpent` | Random `0x655C` or `0x655D` | Corpse hue |
| `GiantToad` | Random `0x6556` or `0x6557` | Default |
| `BullFrog`, `Frog`, `Toad`, `IceToad`, `PoisonFrog`, `FireToad` | Random `0x65AF` or `0x65B0` | Corpse hue |
| `Kraken` | `0x6555` | Default |
| `GiantSquid`, `Leviathan`, `RottingSquid` | `0x65AC` | Corpse hue |
| `AbyssCrawler`, `Arachnar`, `DreadSpider`, `FrostSpider`, `GiantSpider`, `LargeSpider`, `PhaseSpider`, `SandSpider`, `Tarantula`, `WaterStrider`, `ZombieSpider` | `0x65AB` | Class-specific hue or corpse hue |
| `GiantBlackWidow`, `MonstrousSpider`, `ShadowRecluse` | `0x6544` | Default |
| Brown, black, sabretooth, elder, dire, kodiak, and cave bear variants | Random `0x656B` or `0x656C` | Class-specific hue |
| Polar bear and elder polar bear variants | Random `0x655A` or `0x655B` | Default |
| `GrizzlyBearRiding` | `0x569D` | Default |
| `Scorpion`, `DeadlyScorpion` | Random `0x4FBB` or `0x4FBC` | `0xB2F` or `0xB4F` |
| `Gorilla`, `Ape`, `Gorakong` | Random `0x6558` or `0x6559` | Default |
| `Yeti`, `Infected` | Random `0x65B1` or `0x65B2` | `0xB4D` or `0xB01` |
| `LesserSeaSnake` | Random `0x65B6` or `0x65B7` | `0xB00` |
| `SeaSnake` | Random `0x5391` or `0x5392` | Default |
| `RavenousRiding` | Random `0x4FA4` or `0x4FA5` | `0x851` |
| `RaptorRiding` | Random `0x4FA4` or `0x4FA5` | `Corpse.Amount == 116` gives `0x796`; `117` gives `0xB01`; otherwise `0xB51` |

## StuffingBasket Outputs

`StuffingBasket` creates a `StuffedBear` display named `stuffed trophy of ...` unless the target is a tiger family creature, in which case it creates a rug deed and does not create a `StuffedBear`.

| Target mobile classes or condition | Output | Hue rule |
| --- | --- | --- |
| `SabretoothBearRiding`, `ElderBrownBear`, `ElderBrownBearRiding` | `StuffedBear` item `0x56A3` | Default |
| `BrownBear`, `SabreclawCub`, `BlackBear` | `StuffedBear` item `0x569B` | Default, `443`, or `0xB3A` |
| `GreatBear`, `KodiakBear`, `PolarBear`, `DireBear` | `StuffedBear` item `0x569F` | Default or class-specific hue |
| `GrizzlyBearRiding` | `StuffedBear` item `0x569D` | Default |
| `SabretoothBear`, duplicate later `SabretoothBearRiding` branch | `StuffedBear` item `0x840` | Default |
| `ElderPolarBear`, `ElderPolarBearRiding` | `StuffedBear` item `0x56A7` | Default |
| `ElderBlackBear`, `ElderBlackBearRiding` | `StuffedBear` item `0x56A5` | Default |
| `CaveBear` | `StuffedBear` item `0x6DE` | Default |
| `CaveBearRiding` | `StuffedBear` item `0x56A9` | Default |
| `LionRiding`, `SnowLion` | `StuffedBear` item `0x56A1` | Default or `0xB4D` |
| `Grathek`, `Lizardman`, `LizardmanArcher`, `Reptalar`, `ReptalarChieftain`, `ReptalarShaman`, `Reptaur`, `Sakleth`, `SaklethArcher`, `SaklethMage`, `Sleestax` | `StuffedBear` item `0x4D0C` | `0x77E` |
| `DeathAdder`, `JadeSerpent`, `GiantAdder`, `Titanoboa`, `RandomSerpent`, `LavaSnake`, `LavaSerpent`, `LargeSnake`, `JungleViper`, `IceSerpent`, `GiantSnake`, `GiantSerpent`, `GiantIceWorm`, `BloodSnake`, `GiantLamprey` | `StuffedBear` item `0x1A7F` | Corpse hue |
| `TigerRiding`, `SabretoothTigerRiding`, `Tiger`, `SabretoothTiger` | `TigerRugEastDeed` or `TigerRugSouthDeed` | Random orientation |
| `WhiteTigerRiding`, `WhiteTiger` | `WhiteTigerRugEastDeed` or `WhiteTigerRugSouthDeed` | Random orientation |

## Passive Death Drops

`BaseCreature.OnDeath(Container c)` calls `DropRelic.DropSpecialItem(this, killer, c)`. The taxidermy-relevant branch only runs when the corpse container and killer are not null, the killed `BaseCreature` is not stabled, controlled, or bonded, and the killer resolves to a `PlayerMobile`.

| Drop family | Gate | Output |
| --- | --- | --- |
| Stuffed bear/lion displays and tiger rugs | `GetPlayerInfo.LuckyKiller(killer.Luck)` and `Utility.RandomMinMax(1, 20) == 1` | The same bear/lion/tiger family trophies used by `StuffingBasket`, dropped directly into the corpse container |
| Skulls and heads | `90 < Utility.Random(100)` and player killer | Calls `Skulls.MakeSkull`; because `Utility.Random(100)` returns `0..99`, this is a 9 percent gate |
| Demon skull/head variant | Inside `Skulls.MakeSkull`, bone group 2 uses `Utility.RandomMinMax(1, 10) == 1` | 10 percent of demon-group skull rolls become one of `DeamonHeadA/B/C`; otherwise `SkullDemon` |
| Lava or ice giant hearts | Inside `Skulls.MakeSkull`, `Utility.RandomMinMax(1, 2) == 1` | `HeartOfFire` for `LavaGiant`, `HeartOfIce` for `IceGiant`; this is 50 percent after the 9 percent skull-call gate |

`GetPlayerInfo.LuckyKiller(int luck)` returns false at `luck <= 0`, caps luck at `2000`, then checks `(int)(luck * 0.02) + 10` against `Utility.RandomMinMax(1, 100)`. Positive luck therefore gives 10 to 50 percent lucky-killer success before the additional per-drop roll.

## Skull And Heart Mapping

`Skulls.MakeSkull` assigns a `bone` group and creates one trophy Item when `bone > 0`.

| Bone group | Output Item | Target examples |
| ---: | --- | --- |
| 1 | `SkullDragon` | Standard dragon and wyvern classes such as `Dragon`, `Dragons`, `RidingDragon`, `Wyvern`, `Wyverns`, `Dragoon`, `SeaDragon`, `DragonGolem` |
| 2 | `SkullDemon` or `DeamonHeadA/B/C` | Demon/devil classes such as `DemonOfTheSea`, `BloodDemon`, `Devil`, `Balron`, `Archfiend`, `Satan`, `Daemon`, `Fiend`, `BlackGateDemon` |
| 3 | `SkullGiant` | Giants, titans, ettins, cyclops, `OrkDemigod`, undead giants, `SeaGiant`, `StarGiant`, `ZornTheBlacksmith` |
| 4 | `SkullGreatDragon` | Great dragon classes and primeval dragon variants such as `AncientWyrm`, `SkeletalDragon`, `Dracolich`, `CrystalDragon`, `ShadowWyrm`, `PrimevalFireDragon`, `DragonKing`, `ElderDragon` |
| 5 | `SkullMinotaur` | `RottingMinotaur`, `MinotaurCaptain`, `MinotaurScout`, `Minotaur` |
| 6 | `SkullWyrm` | Jewel/elemental wyrms and `DeepSeaDragon`, `WhiteWyrm`, `IceDragon`, `LavaDragon` |
| 7 | `SkullDinosaur` | `Tyranasaur`, `Stegosaurus`, `PackStegosaurus`, `Brontosaur` |
| 8 | `VampireHead` | `VampirePrince`, `VampireLord`, `Dracula` |

## Trophy Properties

| Item family | Stored strings | Property display |
| --- | --- | --- |
| `MountedTrophyHead` | `AnimalKiller`, `AnimalWhere` | Adds `AnimalWhere` and `AnimalKiller` only when non-empty and non-null |
| `StuffedBear` | `AnimalKiller`, `AnimalWhere` | Adds `AnimalWhere` and `AnimalKiller` only when non-empty and non-null |
| Skull Items | `SkullKiller`, `SkullWhere` | Displays `From ` + `SkullWhere` and `Slain by ` + `SkullKiller` |
| Heart Items | `HeartKiller`, `HeartWhere` | Displays `From ` + `HeartWhere` and `Slain by ` + `HeartKiller` |

## Serialization Notes

| Class | Version | Serialized after version |
| --- | ---: | --- |
| `TaxidermyKit` | 0 | Nothing |
| `TrophyAddon` | 1 | `Mobile m_Hunter`, `int m_AnimalWeight`, `int m_WestID`, `int m_NorthID`, `int m_DeedNumber`, `int m_AddonNumber` |
| `TrophyDeed` | 1 | `Mobile m_Hunter`, `int m_AnimalWeight`, `int m_WestID`, `int m_NorthID`, `int m_DeedNumber`, `int m_AddonNumber` |
| `TrophyBase` | 0 | Nothing |
| `MountingBase` | 0 | Nothing |
| `StuffingBasket` | 0 | Nothing |
| `MountedTrophyHead` | 0 | `string AnimalKiller`, `string AnimalWhere` |
| `StuffedBear` | 0 | `string AnimalKiller`, `string AnimalWhere` |
| Skull Items | 0 | `string SkullKiller`, `string SkullWhere` |
| Heart Items | 0 | `string HeartKiller`, `string HeartWhere` |

`Corpse.VisitedByTaxidermist` is part of `CorpseFlag` and is deserialized from the corpse save stream in version case 8.

## Admin Notes

No taxidermy-specific `[Command]`, `[Usage]`, or `[Description]` registration exists in the traced scripts. The entry Items are `[Constructable]`, so staff can instantiate them through the shard's standard item-add command path if available:

| Item type | Notes |
| --- | --- |
| `TaxidermyKit` | Legacy kit requiring 90.0 Carpentry and 10 boards. |
| `TrophyBase` | Dungeon rare type; broad head mounting. |
| `MountingBase` | Dungeon rare type; broad body mounting. |
| `StuffingBasket` | Dungeon rare type; stuffed trophies and rugs. |

`TrophyBase`, `MountingBase`, and `StuffingBasket` are listed in `DungeonLoot.DungeonRareTypes`. `TaxidermyKit` was not found in a direct vendor stocking path during this trace, although NPC dialogue says fur traders occasionally sell taxidermy kits.

## Known Issues

* `TaxidermyKit` gathers `BigFish.Fisher` and `BigFish.Weight`, but `TrophyDeed(TrophyInfo info, Mobile hunter, int animalWeight)` calls the overload that drops `hunter` and `animalWeight`. Big fish trophies created through the legacy kit therefore lose their intended caught-by and weight properties.
* `TrophyBase`, `MountingBase`, and `StuffingBasket` start their target cursors from context-menu entries but never verify that the consumable Item is in the caller's backpack, locked down by the caller, or otherwise owned by the caller before deleting it on success.
* `StuffingBasket` has two `SabretoothBearRiding` branches. The first one assigns `0x56A3`, so the later `0x840` branch is unreachable.
* `MagicSkulls.MakeSkull` has an `m is Wyrms` branch that sets `color = 1150` but never assigns `bone = 6`; those `Wyrms` kills do not create a skull through this path.
* `MagicSkulls.MakeSkull` checks `Dracolich` and `SkeletalDragon` in the bone 4 great-dragon group before later bone 6 branches for the same types, so the bone 6 branches are unreachable.
* The passive skull gate is written as `90 < Utility.Random(100)`, which yields 9 percent, not the more typical 10 percent threshold.
* `TrophyBase`, `MountingBase`, and `StuffingBasket` allocate `Region reg = Region.Find(...)` but do not use `reg`.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0008.
- Backlog rows: RB-06784.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs (CurrentFile)
- Data/Scripts/Trades/Taxidermy/TrophyBase.cs (CurrentFile)
- Data/Scripts/Trades/Taxidermy/MountingBase.cs (CurrentFile)
- Data/Scripts/Trades/Taxidermy/StuffingBasket.cs (CurrentFile)
- Data/Scripts/Trades/Taxidermy/MountedTrophyHead.cs (CurrentFile)
- Data/Scripts/Trades/Taxidermy/StuffedBear.cs (CurrentFile)
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/DropRelic.cs (CurrentFile)
- Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Gump=3; Initialize=1; Timer=2.
- Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs:L436 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs:L520 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs:L343 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Trades/Taxidermy/MountingBase.cs:L68 Gump SendGump access=Internal
- Data/Scripts/Trades/Taxidermy/StuffingBasket.cs:L68 Gump SendGump access=Internal
- Data/Scripts/Trades/Taxidermy/TrophyBase.cs:L69 Gump SendGump access=Internal

### Serialization Evidence

- Serialized rows matched: 22.
- Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs:Server.Items.Corpse version=11 serialize=L700 deserialize=L759 alignment=CountMismatch:Writes=18;Reads=37
- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs:Server.Items.TaxidermyKit version=0 serialize=L27 deserialize=L34 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs:Server.Items.TrophyAddon version=1 serialize=L304 deserialize=L319 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs:Server.Items.TrophyDeed version=1 serialize=L514 deserialize=L529 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.DeamonHeadA version=0 serialize=L1385 deserialize=L1393 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.DeamonHeadB version=0 serialize=L1449 deserialize=L1457 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.DeamonHeadC version=0 serialize=L1513 deserialize=L1521 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.HeartOfFire version=0 serialize=L873 deserialize=L881 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.HeartOfIce version=0 serialize=L809 deserialize=L817 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.SkullDemon version=0 serialize=L1257 deserialize=L1265 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.SkullDinosaur version=0 serialize=L1001 deserialize=L1009 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs:Server.Misc.SkullDragon version=0 serialize=L1193 deserialize=L1201 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs=Keep
- Data/Scripts/Items/Misc/Bodies/Corpses/Corpse.cs=Keep
- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs=Keep
- Data/Scripts/Items/Trades/Carpenter Items/TaxidermyKit.cs=Keep
- Data/Scripts/Mobiles/Base/DropRelic.cs=Keep
- Data/Scripts/Mobiles/Base/DropRelic.cs=Keep
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs=Keep
- Data/Scripts/Trades/Taxidermy/MagicSkulls.cs=Keep
- Data/Scripts/Trades/Taxidermy/MountedTrophyHead.cs=Keep
- Data/Scripts/Trades/Taxidermy/MountedTrophyHead.cs=Keep
- Data/Scripts/Trades/Taxidermy/MountingBase.cs=Keep
- Data/Scripts/Trades/Taxidermy/MountingBase.cs=Keep
- Data/Scripts/Trades/Taxidermy/StuffedBear.cs=Keep
- Data/Scripts/Trades/Taxidermy/StuffedBear.cs=Keep
- Data/Scripts/Trades/Taxidermy/StuffingBasket.cs=Keep
- Data/Scripts/Trades/Taxidermy/StuffingBasket.cs=Keep
- Data/Scripts/Trades/Taxidermy/TrophyBase.cs=Keep
- Data/Scripts/Trades/Taxidermy/TrophyBase.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
