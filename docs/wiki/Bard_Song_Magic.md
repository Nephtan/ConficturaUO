# Bard Song Magic

## Overview
Bard song magic is implemented as a dedicated `SongBook` spellbook type with 16 spell IDs (`351` through `366`), paired sheet-music scrolls, a custom gump, and one command per song. Casting always resolves through the bard spell classes in `Data/Scripts/Magic/Bard/Spells/`.

## Core Components
- `SongBook` is the bard spellbook. It opens `SongBookGump`, stores a single assigned `BaseInstrument`, and serializes that instrument reference at version `0`.
- `SongBookGump` lists only the songs already present in the player's songbook, provides the `Assign Instrument` targeting flow, and can cast songs directly from button IDs `351` through `366`.
- `CastSongSpells.Initialize()` registers player commands for every song. Most command names match the spell name, but the Dexterity song is registered as `[ShephardsDance]`, not `[SheepfoeMambo]`.
- Each bard scroll is only sheet music. Double-clicking a scroll does not cast anything; it only tells the player that the sheet music must be in the music book.

## Shared Casting Rules
- All songs use `Musicianship` as both `CastSkill` and `DamageSkill`.
- Songs do not clear the caster's hands on cast.
- `GetCastSkills` sets the required range to `RequiredSkill` through `RequiredSkill + 30.0`.
- Every successful or failed song attempt consumes one use from the assigned instrument through `BardFunctions.UseBardInstrument`. The instrument plays its success or failure sound, loses one use, and is deleted when its final use is spent.
- Most songs require the assigned instrument to be within range `1` of the caster. If the stored instrument is missing or out of range, the cast aborts with the "Your instrument is missing!" message.
- Shared bard scaling uses `MusicSkill(Caster)`, which is the integer sum of `Musicianship`, `Provocation`, `Discordance`, and `Peacemaking`.

## Learning And Activation
1. Put sheet-music scrolls into the `SongBook` so the book contains spell IDs `351` to `366`.
2. Double-click the `SongBook` while within range `1` to open the bard gump.
3. Use `Assign Instrument` to target a `BaseInstrument`.
4. Cast from the gump or with the matching player command.

## Song Reference
| Song | Spell ID | Gump / Command Name | Cast Delay | Required Skill | Mana | Actual Effect |
| --- | --- | --- | --- | --- | --- | --- |
| Army's Paeon | 351 | `Army's Paeon` / `[ArmysPaeon]` | 5s | 55.0 | 15 | Beneficial area effect in range `3`. Applies a buff icon and starts a heal-over-time timer on each valid target. Tick count is `floor(Musicianship * 0.16)` at 2-second intervals. Per-tick healing uses `MyServerSettings.PlayerLevelMod(6..10, target)` based on total `MusicSkill`: `<120`, `<240`, `<360`, `<480`, `>=480`. |
| Enchanting Etude | 352 | `Enchanting Etude` / `[EnchantingEtude]` | 2s | 60.0 | 20 | Beneficial area effect in range `3`. Adds an `Int` stat mod equal to `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |
| Energy Carol | 353 | `Energy Carol` / `[EnergyCarol]` | 5s | 50.0 | 12 | Beneficial area effect in range `3`. Adds `Energy` resistance by `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |
| Energy Threnody | 354 | `Energy Threnody` / `[EnergyThrenody]` | 5s | 70.0 | 25 | Harmful targeted song with target range `12`. Lowers `Energy` resistance by `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill` seconds. Against a `BaseCreature`, a matching `Slayer` or `Slayer2` property on the assigned instrument doubles both the reduction and duration. |
| Fire Carol | 355 | `Fire Carol` / `[FireCarol]` | 5s | 50.0 | 12 | Beneficial area effect in range `3`. Adds `Fire` resistance by `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |
| Fire Threnody | 356 | `Fire Threnody` / `[FireThrenody]` | 5s | 70.0 | 25 | Harmful targeted song with target range `12`. Lowers `Fire` resistance by `floor(MusicSkill / 16)` for `MusicSkill` seconds. Slayer-matched instruments double the reduction and duration against `BaseCreature` targets. |
| Foe Requiem | 357 | `Foe Requiem` / `[FoeRequiem]` | 2s | 80.0 | 30 | Harmful targeted song with target range `12`. Deals `MusicSkill / 15` damage split evenly across all five resist channels (`20/20/20/20/20`). Slayer-matched instruments double damage against `BaseCreature` targets. |
| Ice Carol | 358 | `Ice Carol` / `[IceCarol]` | 5s | 50.0 | 12 | Beneficial area effect in range `3`. Adds `Cold` resistance by `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |
| Ice Threnody | 359 | `Ice Threnody` / `[IceThrenody]` | 5s | 70.0 | 25 | Harmful targeted song with target range `12`. Lowers `Cold` resistance by `floor(MusicSkill / 16)` for `MusicSkill` seconds. Slayer-matched instruments double the reduction and duration against `BaseCreature` targets. |
| Knight's Minne | 360 | `Knight's Minne` / `[KnightsMinne]` | 5s | 50.0 | 12 | Beneficial area effect in range `3`. Adds `Physical` resistance by `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |
| Mage's Ballad | 361 | `Mage's Ballad` / `[MagesBallad]` | 6s | 55.0 | 15 | Beneficial area effect in range `3`, but unlike the other party buffs it excludes all `BaseCreature` targets. Tick count is `floor(Musicianship * 0.16)` at 2-second intervals. Per-tick mana gain uses `MyServerSettings.PlayerLevelMod(6..10, target)` based on total `MusicSkill`: `<120`, `<240`, `<360`, `<480`, `>=480`. |
| Magic Finale | 362 | `Magic Finale` / `[MagicFinale]` | 5s | 90.0 | 35 | Area purge in range `4`. It does not deal damage. It deletes nearby `BaseCreature` mobiles that are either `Summoned` or have `ControlSlots == 666`. |
| Poison Carol | 363 | `Poison Carol` / `[PoisonCarol]` | 5s | 50.0 | 12 | Beneficial area effect in range `3`. Adds `Poison` resistance by `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |
| Poison Threnody | 364 | `Poison Threnody` / `[PoisonThrenody]` | 5s | 70.0 | 25 | Harmful targeted song with target range `12`. Lowers `Poison` resistance by `floor(MusicSkill / 16)` for `MusicSkill` seconds. Slayer-matched instruments double the reduction and duration against `BaseCreature` targets. |
| Shepherd's Dance | 365 | `Shepherd's Dance` / `[ShephardsDance]` | 2s | 60.0 | 20 | Beneficial area effect in range `3`. Despite older `SheepfoeMambo` naming in filenames and class names, the compiled spell adds a `Dex` stat mod equal to `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. It does not modify swing speed directly. |
| Sinewy Etude | 366 | `Sinewy Etude` / `[SinewyEtude]` | 2s | 60.0 | 20 | Beneficial area effect in range `3`. Adds a `Str` stat mod equal to `MyServerSettings.PlayerLevelMod(MusicSkill / 16, caster)` for `MusicSkill * 2` seconds. |

## Targeting Rules
- Beneficial area songs collect mobiles in range `3` where `Caster.CanBeBeneficial(m, false, true)` returns true, and they exclude `Golem`.
- `Mage's Ballad` also excludes all `BaseCreature` mobiles, so it only restores mana to players and other non-creature beneficial targets.
- Harmful songs use `TargetFlags.Harmful` with range `12`.

## Serialization
- `SongBook` writes version `0` and then writes the assigned `Instrument` item reference.
- Every sheet-music scroll also uses version `0` serialization and stores no custom fields beyond its base `SpellScroll` state.

## Audit Notes
- Older documentation that described `SheepfoeMambo` as an attack-speed buff was stale. The actual compiled spell is `Shepherd's Dance` and applies a Dexterity stat bonus.
- Older documentation that described `Magic Finale` as a damage spell was stale. The compiled implementation only deletes summoned or special `ControlSlots == 666` creatures in range.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0033.
- Backlog rows: RB-06664.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Bard/Spells/ (CurrentDirectory)

### Runtime Evidence

- Hook summary: Timer=19.
- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs:L154 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs:L194 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs:L234 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs:L274 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs:L314 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/EnergyCarolSong.cs:L126 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/EnergyThrenodySong.cs:L138 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/FireCarolSong.cs:L133 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/FireThrenodySong.cs:L136 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/IceCarolSong.cs:L124 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/IceThrenodySong.cs:L137 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Bard/Spells/KnightsMinneSong.cs:L126 Timer CustomTimerSubclass access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/EnchantingEtudeSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/EnchantingEtudeSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/EnergyCarolSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/EnergyCarolSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/EnergyThrenodySong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/EnergyThrenodySong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/FireCarolSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/FireCarolSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/FireThrenodySong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/FireThrenodySong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/FoeRequiemSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/FoeRequiemSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/IceCarolSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/IceCarolSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/IceThrenodySong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/IceThrenodySong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/KnightsMinneSong.cs=Keep
- Data/Scripts/Magic/Bard/Spells/KnightsMinneSong.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
