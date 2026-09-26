# Champion Spawns

## Overview
The champion spawn system is implemented by `Server.Engines.CannedEvil.ChampionSpawn` and the companion `BaseChampion` boss framework. A placed spawn creates its own hidden controller item plus a `ChampionPlatform`, `ChampionAltar`, and `IdolOfTheChampion`, then cycles through escalating monster waves until it summons a boss. The stock configuration uses a 24-tile spawn radius, a 10-minute inactivity expiry window, and a 10-minute restart delay.

## Core components
- `ChampionSpawn`: hidden controller item that tracks theme, progress, timers, creatures, boss state, and damage entries.
- `ChampionSpawnInfo`: static table mapping each `ChampionSpawnType` to a boss class, three title strings, and four wave rosters.
- `BaseChampion`: shared boss reward logic for power scrolls, artifacts, skull drops, and the post-kill gold "goodies" scatter.
- `ChampionPlatform`, `ChampionAltar`, `IdolOfTheChampion`: world objects automatically created and kept in sync with the spawn's position and map.
- `ChampionSkull`: cursed skull item awarded from boss deaths on every playable map.

## Spawn lifecycle
1. Staff place the controller with `[add ChampionSpawn]`.
2. Setting `Active` to `true` calls `Start()`, starts a one-second `SliceTimer`, clears restart timers, and updates altar/platform hues.
3. `Respawn()` keeps the arena populated until a boss is active. The live creature cap is `200 - (GetSubLevel() * 40)`, so the four wave bands cap at 200, 160, 120, and 80 creatures.
4. Every deleted tracked creature increments `Kills`, contributes damage credit, can award Valor/title progress, and can rarely award a Scroll of Transcendence or a Power Scroll with a 105 skill cap.
5. Progress advances when kills reach 90% of `MaxKills`, where `MaxKills = 250 - (Level * 12)`.
6. Levels 0 through 15 add red skulls around the altar. White skulls show partial progress at roughly 20%, 40%, 60%, and 80%.
7. After level 16 would be reached, `SpawnChampion()` clears the skull counters and summons the boss at the altar.
8. When the boss is deleted, the spawn awards one random artifact if the boss rolled one, stops, optionally opens a `StarRoomGate`, and starts its restart timer.

## Expiry and reset behavior
- `ExpireDelay` defaults to 10 minutes and refreshes every time the spawn advances a level.
- If players do not make progress before `ExpireTime`, `Expire()` resets `Kills`.
- If no white skulls were earned before expiry, the spawn drops one red-skull level.
- If some white skull progress existed, only the white skulls are cleared.
- `RestartDelay` defaults to 10 minutes after a boss dies.

## Theme table
| `ChampionSpawnType` | Display name | Boss class | Wave 1 | Wave 2 | Wave 3 | Wave 4 |
| --- | --- | --- | --- | --- | --- | --- |
| `Abyss` | Abyss | `Semidar` | `GreaterMongbat`, `Imp` | `Gargoyle`, `Harpy` | `FireGargoyle`, `StoneGargoyle` | `Daemon`, `Succubus` |
| `Arachnid` | Arachnid | `Mephitis` | `Scorpion`, `GiantSpider` | `TerathanDrone`, `TerathanWarrior` | `DreadSpider`, `TerathanMatriarch` | `PoisonElemental`, `TerathanAvenger` |
| `ColdBlood` | Cold Blood | `Rikktor` | `Lizardman`, `Snake` | `LavaLizard`, `OphidianWarrior` | `Drake`, `OphidianArchmage` | `Dragon`, `OphidianKnight` |
| `ForestLord` | Forest Lord | `LordOaks` | `Pixie`, `Wisp` | `Kirin`, `Wisp` | `Centaur`, `Unicorn` | `EtherealWarrior`, `SerpentineDragon` |
| `VerminHorde` | Vermin Horde | `Barracoon` | `GiantRat`, `Slime` | `DireWolf`, `Ratman` | `HellHound`, `RatmanMage` | `RatmanArcher`, `SilverSerpent` |
| `UnholyTerror` | Unholy Terror | `Neira` | `Ghoul`, `Shade`, `Spectre`, `Wraith`, plus `Undead` when `Core.AOS` is enabled | `BoneMagi`, `Mummy`, `SkeletalMage` | `BoneKnight`, `Lich`, `SkeletalKnight` | `LichLord`, `RottingCorpse`, `SkeletonDragon` |
| `SleepingDragon` | Sleeping Dragon | `Serado` | `DeathwatchBeetleHatchling`, `Lizardman` | `DeathwatchBeetle`, `Kappa` | `LesserHiryu`, `RevenantLion` | `Hiryu`, `Oni` |
| `Glade` | Glade | `Twaulo` | `Pixie`, `ShadowWisp` | `Centaur`, `MLDryad` | `Satyr`, `CuSidhe` | `FerelTreefellow`, `RagingGrizzlyBear` |
| `Pestilence` | The Corrupt | `Ilhenir` | `PlagueSpawn`, `Bogling` | `PlagueBeast`, `BogThing` | `PlagueBeastLord`, `InterredGrizzle` | `FetidEssence`, `PestilentBandage` |

## Rewards
### Wave kills
- With `Core.ML` enabled, every playable map uses the same 0.1% chance per deleted tracked creature credited to a `PlayerMobile`. The most recent damager receives credit; a creature damager resolves to its master.
- A successful roll selects a Scroll of Transcendence with probability 25/49, or a Power Scroll with probability 24/49. The latter sets a skill cap of 105.
- The Scroll of Transcendence grants **0.6-1.0 skill points**, subject to its use rules. `CreateRandomSoT(true)` passes integers 6-10 to `ScrollofTranscendence.CreateRandom`, which multiplies them by 0.1; these are not 6-10 whole skill points.
- Underworld, Isles of Dread, and Serpent Island now use this same single roll. Their former separate 0.15% chance of a 0.1-0.5 Scroll of Transcendence has been removed.
- Players also receive Valor progress and champion title progress based on the killed creature's wave band.

### Boss death
- `BaseChampion.GivePowerScrolls()` runs on every playable map. Null maps and `Map.Internal` are excluded.
- With kill awards enabled and at least one eligible looter, **six Power Scrolls total** are distributed round-robin among shuffled players with boss damage rights. These set skill caps of **110 / 115 / 120**, with approximately **60% / 35% / 5%** weighting. Blacksmithing and Tailoring are excluded from this pool.
- Luck and alien/ET origin do not alter these scroll or skull rewards. Party membership and wave participation alone do not grant boss damage rights. Expired damage entries do not count; pet damage can credit the responsible player master.
- The boss then rolls one optional artifact: 5% unique, 10% shared, 15% decorative, otherwise no artifact.
- Artifact eligibility requires the player to be alive, still inside the champion region, and able to hold the item in their backpack.
- On every playable map, one random eligible looter also receives the boss's cursed `ChampionSkullType` skull. If there are no eligible players, the skull drops into the boss corpse. This remains a separate branch from the `NoKillAwards` scroll/gold gate.
- With kill awards enabled, unless a boss overrides `NoGoodies`, boss death also triggers delayed gold drops in a 12-tile radius. Gold amounts and the existing artifact roll were not changed by the map parity repair.
- Scroll delivery uses the backpack for living recipients. Dead recipients use their existing corpse, falling back to the backpack when it is missing. `Mobile.AddToBackpack` can place overflow at the player's world/logout location. Skull delivery still uses `AddToBackpack`.
- Existing Justice protection duplication rules remain in effect. They can add scrolls beyond the six base boss rewards; they are not a guaranteed extra award.

### Configured overworld spawns

`Spawns/RWChamp.xml` defines nine named areas across six physical maps: Sosaria, Lodoria, Underworld, Serpent Island, Isles of Dread, the Savaged Empire, Ambrosia, Umber Veil, and the Bottle World of Kuldar. The last three share `Map.Sosaria`. Atlantis is also covered by the reward fix if a champion is placed there, but has no entry in that XML.

The XML applies `SpawnRadius = 50` and `RestartDelay = 24:00:00`, overriding constructor defaults. Its outer spawners use 1200-1440 minute delays and companion monitor spawners, so the checked-in configuration is not proof of nine successful live spawns every calendar day. The `StarRoomGate` travel condition remains unchanged; it is separate from loot.

The staff planning document [Champion spawn ideas](../CHAMPION_SPAWN_IDEAS.md) separates the implemented parity fix from proposed reward and encounter additions.

## Boss-specific mechanics present in code
- `Barracoon`: can polymorph targets into rats and summon additional ratmen when local ratman counts are below 16.
- `LordOaks`: can spawn `Silvani`, reduces incoming damage while the queen is alive, and can summon extra pixies.
- `Rikktor`: has a 20% chance on melee hits to fire an area earthquake that damages nearby players and hostile controlled creatures for 60% of current health, clamped to 10-75.
- `Semidar`: always reflects male-body casters and sets incoming spell damage from male bodies to a `20` scalar, plus a 30% life-drain pulse on melee interactions.
- `Neira`: uses a virtual mount item, gains a speed boost below 25% health, and can create `UnholyBone` hazards on melee interactions.
- `Serado`: scales its resistances upward as it loses health and has a 20% poison-area counterattack on spell or melee triggers.
- `Twaulo`: drains damage, stamina, and mana on melee hits and can spawn pixies when attacked or damaged by spells.
- `Ilhenir`: packs Mondain's Legacy resources, can drop ooze on damage, and uses a 25% melee-triggered special attack.

## Staff configuration
- Place with `[add ChampionSpawn]`.
- Open its properties gump by double-clicking the hidden controller item.
- Relevant command properties are `Active`, `Type`, `RandomizeType`, `SpawnArea`, `SpawnRadius`, `ConfinedRoaming`, `ExpireDelay`, `RestartDelay`, `Kills`, `Champion`, and `HasBeenAdvanced`.
- `ConfinedRoaming = false` sets each spawned creature's `Home` to the spawn center and computes `RangeHome` from the spawn rectangle diagonal.
- `ConfinedRoaming = true` anchors each creature to its own spawn point and clamps `RangeHome` to the nearest wall distance inside the configured rectangle.
- Moving or remapping the controller also moves the altar, idol, platform, red skulls, and white skulls.

## Persistence
- `ChampionSpawn` serializes at version `5`.
- `BaseChampion`, `ChampionAltar`, `IdolOfTheChampion`, and most boss classes currently serialize at version `0`.
- `ChampionSkull` serializes at version `1` and converts older saves to cursed, uninsured skulls on load.

## Known code issues
- `RandomizeType` is incomplete. `ChampionSpawn.EndRestart()` only randomizes across five legacy themes and never selects `ForestLord`, `SleepingDragon`, `Glade`, or `Pestilence`.
- `Mephitis.OnGotMeleeAttack()` still contains a `// TODO: Web ability` stub, so that boss is missing at least one intended special attack in the compiled code.
- Plain champion-generated `PowerScroll` items store their cap in `Value`, but `GetPower()` recognizes named subclasses and otherwise returns zero. Their tooltip can omit the cap; fixing that display is a separate proposed change.
- `ChampionSkullPlatform.Validate()` clears a complete six-skull set without summoning a boss or issuing a reward. A useful skull ritual needs an explicit implementation.

## Source Trace

### Reward parity repair — 2026-09-26

- [BaseChampion.cs](../../Data/Scripts/Custom/Champions/System/Mobiles/BaseChampion.cs): `OnBeforeDeath`, `GivePowerScrolls`, `GivePowerScrollTo`, `OnDeath`, `GoodiesTimer`.
- [ChampionSpawn.cs](../../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs): `OnSlice`, `CreateRandomSoT`, `GiveScrollTo`, `AwardArtifact`, `IsEligible`.
- [PowerScroll.cs](../../Data/Scripts/Items/Books/PowerScrolls/PowerScroll.cs): `CreateRandomNoCraft`, `AddNameProperties`, `GetPower`.
- [ScrollofTranscendence.cs](<../../Data/Scripts/Items/Special/Special Scrolls/ScrollofTranscendence.cs>): `CreateRandom`, `Use`.
- [BaseCreature.cs](../../Data/Scripts/Mobiles/Base/BaseCreature.cs): `GetLootingRights`; [Mobile.cs](../../Data/System/Source/Mobile.cs): `RegisterDamage`, `AddToBackpack`.
- [MapDefinitions.cs](../../Data/Scripts/System/Misc/MapDefinitions.cs): seven playable map registrations; [RWChamp.xml](../../Spawns/RWChamp.xml) and [RWChamp_Monitors.xml](../../Spawns/RWChamp_Monitors.xml): configured overworld areas and cleanup.
- [ChampionSkullPlatform.cs](../../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSkullPlatform.cs): `Validate`, `Clear`.
- Reproduce isolated verification with `powershell -NoProfile -File scripts/Test-ChampionRewards.ps1`. The runner builds `Server.csproj` Release/x86 into ignored scratch output, runs the real script compiler with `-compileonly -nocache`, then runs [ChampionRewardRegression.cs](../../tests/ChampionRewardRegression.cs).
- Verified: server build, runtime script compile, and 616 fixture assertions covering all seven maps, normal/alien players with 0/2000 equipment Luck, boss sharing/rights, pet credit, expired credit, dead/full-pack delivery, wave chance/type/quantity, internal maps, and the ML expansion gate. Alien characters retain their actual zero-Luck rule even with Luck equipment.
- No serialized fields, versions, class names, project includes, or spawn XML were changed. This is local compile and fixture evidence; live deployment, world initialization, and client gameplay acceptance remain to be verified after restart.

### Historical audit snapshot

The entries below preserve the June review; its numeric source locators refer to that historical snapshot.

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0009.
- Backlog rows: RB-06672.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnInfo.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/SliceTimer.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/RestartTimer.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionAltar.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionPlatform.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/IdolOfTheChampion.cs (CurrentFile)
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnRegion.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Gump=1; Timer=3.
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs:L938 Gump SendGump access=Internal
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs:L1284 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Custom/Champions/System/CannedEvil/RestartTimer.cs:L7 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Custom/Champions/System/CannedEvil/SliceTimer.cs:L7 Timer CustomTimerSubclass access=GlobalOrInternal

### Serialization Evidence

- Serialized rows matched: 4.
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionAltar.cs:Server.Engines.CannedEvil.ChampionAltar version=0 serialize=L27 deserialize=L36 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionPlatform.cs:Server.Engines.CannedEvil.ChampionPlatform version=0 serialize=L58 deserialize=L67 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs:Server.Engines.CannedEvil.ChampionSpawn version=5 serialize=L1144 deserialize=L1185 alignment=CountMismatch:Writes=19;Reads=20
- Data/Scripts/Custom/Champions/System/CannedEvil/IdolOfTheChampion.cs:Server.Engines.CannedEvil.IdolOfTheChampion version=0 serialize=L39 deserialize=L48 alignment=CountMatchNeedsTypeReview:UnknownWrites=1

### Project And Runtime Coverage

- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionAltar.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionAltar.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionPlatform.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionPlatform.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnInfo.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnInfo.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnRegion.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnRegion.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/IdolOfTheChampion.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/IdolOfTheChampion.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/RestartTimer.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/RestartTimer.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/SliceTimer.cs=Keep
- Data/Scripts/Custom/Champions/System/CannedEvil/SliceTimer.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
