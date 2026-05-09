# System Name: PvP Consent System

## Overview

The PvP Consent System stores each player's PvP preference on `PlayerMobile.NONPK` and uses that value in gumps, NPC speech, event gates, notoriety, harmful-action checks, beneficial-action checks, and several area-damage items.

Code-Verified: 2026-05-07

The system's public choices are Neutral, PvP, and PvE. The enum also contains `NONPKinEvent`, which is used by the custom event moongate to temporarily move PvE players into an event-capable state.

## Source Trace

| Area | Script | Role |
| :--- | :--- | :--- |
| Player flag | `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Defines the `NONPK` enum, exposes the `PlayerMobile.NONPK` command property, and serializes the flag with the player. |
| Choice gump and command | `Data/Scripts/Custom/PvPConsent/Gumps/PKNONPKGUMP.cs` | Builds the choice gump and registers `[ChoosePvP`. |
| Choice NPC | `Data/Scripts/Custom/PvPConsent/GoddessOfProtection.cs` | Defines Thuvia, the blessed stationary Mobile that speaks to nearby players and opens the choice gump on speech. |
| Event gates | `Data/Scripts/Custom/PvPConsent/NONPKEventMoongate.cs` | Defines event moongates that toggle PvE players into and out of `NONPKinEvent`. |
| Example items | `Data/Scripts/Custom/PvPConsent/PKSword.cs`, `Data/Scripts/Custom/PvPConsent/NONPKSword.cs` | Demonstrate flag-gated equipment. |
| Harmful and beneficial checks | `Data/Scripts/System/Misc/Notoriety.cs` | Installs global `Mobile.AllowBeneficialHandler` and `Mobile.AllowHarmfulHandler` logic and returns notoriety hues. |
| Region harmful check | `Data/Scripts/System/Regions/BaseRegion.cs` | Adds a region-level direct player-vs-player block for PvE players and PvE targets. |
| Field and splatter damage | Potion and splatter scripts | Adds parallel flag checks for conflagration fields, frostbite fields, and `MonsterSplatter`. |

## Player Flag States

| Enum value | Meaning in this system | Set by |
| :--- | :--- | :--- |
| `Null` | Neutral. The player has not chosen PvP or PvE through `[ChoosePvP`, or chose "Remain Neutral" in the gump. | Default enum value, gump button `0`, or admin property edit. |
| `NONPK` | PvE. The player is protected from normal player PvP and cannot initiate normal player PvP. | Gump button `2`, or admin property edit. |
| `NONPKinEvent` | Temporary event state for PvE players. The core harmful/notoriety checks do not treat this value as `NONPK`, `PK`, or `Null`. | `NONPKEventMoongate.OnGateUsed`. |
| `PK` | PvP. The player may attack PvP and Neutral players, but not PvE players. | Gump button `1`, or admin property edit. |

## Player Command

| Command | Access | Usage attribute | Description attribute | Behavior |
| :--- | :--- | :--- | :--- | :--- |
| `[ChoosePvP` | `AccessLevel.Player` | None in source | None in source | If the caller is a `PlayerMobile` whose `NONPK` flag is `Null`, sends the `PKNONPK` gump. Otherwise sends `You have already chosen a path`. |

The command does not accept parameters. The command does not let a player reopen the gump after choosing `PK` or `NONPK`; staff can still alter the flag directly through the `PlayerMobile.NONPK` command property.

## Choice Gump

`PKNONPK` is a `Server.Gumps.Gump` built at `base(0, 0)`. It is non-closable, disposable, draggable, and not resizable. The UI presents three reply buttons:

| Button ID | Label | State written | Title written | Messages |
| :--- | :--- | :--- | :--- | :--- |
| `0` | Remain Neutral | `NONPK.Null` | `null` | Sends `You have chosen neutrality`, closes the gump, then explains that neutral players can attack PvP and other neutral players, but not PvE players. |
| `1` | Mark me for PvP | `NONPK.PK` | `[PvP]` | Sends `You have chosen [PvP]`, closes the gump, then explains that PvP players can attack any player except PvE players. |
| `2` | Mark me for PvE | `NONPK.NONPK` | `[PvE]` | Sends `You have chosen [PvE]`, closes the gump, then explains that PvE players cannot attack any player and cannot perform beneficial actions on PvP players. |

The gump response handler casts `sender.Mobile` to `PlayerMobile` and writes the flag directly. It does not perform an internal null check before writing the flag.

## Goddess Of Protection Mobile

`GoddessOfProtection` is a blessed, stationary `BaseCreature` named `Thuvia` with the title `the Goddess Of Protection`. The constructable Mobile uses `AIType.AI_Melee`, `FightMode.None`, body `0x191`, female body settings, blessed gear, and `CantWalk = true`.

### Speech

The Mobile handles speech from mobiles within range 3 and processes speech while the speaker is within range 6. The speech is lower-cased before comparison.

| Speech | Response |
| :--- | :--- |
| `hail goddess`, `hail`, `pvp`, `pve`, `pk`, `nonpk` | Says a greeting asking whether the player wants to choose between PvE and PvP. |
| `help` | Explains that PvE fights monsters and PvP fights other players, then tells the player to say `choose`. |
| `status` | Reads the speaker's `PlayerMobile.NONPK` flag and sends PvE, PvP, or not-yet-chosen status text. |
| `choose` | Sends a new `PKNONPK` gump. |

### Movement Prompt

On nearby player movement within range 8, Thuvia says one random line from her configured speech list, turns toward the moving player, and sends two direct messages: one asking whether the player wishes to choose a path and one telling them to say `help` or `choose`. A static spam flag suppresses repeated movement prompts for 8 seconds.

## Harmful Action Rules

The main PvP restrictions are installed in `NotorietyHandlers.Initialize`, which assigns `Mobile.AllowHarmfulHandler = Mobile_AllowHarmful`.

| Attacker state | Target state | Result |
| :--- | :--- | :--- |
| Staff attacker above `AccessLevel.Player` | Any target | Allowed. |
| Any attacker | Staff target above `AccessLevel.Player` | Blocked. |
| Government `CanAttackBanned` case | Banned target inside that helper's rules | Allowed before PvP-consent flag checks run. |
| XMLPoints challengers | XMLPoints opponent | Allowed. |
| Bard-provoked creature | Bard target | Allowed unless the provoker is a player-controlled owner and the target is PvE. |
| PvE player | Uncontrolled non-player `BaseCreature` | Allowed. |
| Wild or non-player-owned `BaseCreature` | PlayerMobile or creature target | Allowed. |
| PvE player | PlayerMobile or player-owned pet/summon | Blocked with the PvE cannot attack players or pets message. |
| PlayerMobile | PvE PlayerMobile or PvE-owned pet/summon | Blocked with the cannot attack PvE players or pets message. |
| Controlled or summoned creature | Its master or another creature owned by the same master | Blocked. |
| Government banned, city-war, or allied city case | Target accepted by the helper | Allowed only if no earlier PvP-consent block returned false. |
| Guild allies or same guild | Guild target | Allowed. |
| Either guild status is `Waring` | Guild target | Blocked by `CheckBeneficialStatus`. |
| No earlier rule matched | Any target | Allowed. |

The region-level `BaseRegion.AllowHarmful` adds a direct `PlayerMobile` versus `PlayerMobile` block: a PvE attacker cannot harm PvP or Neutral players, and no direct player attacker can harm a PvE target.

## Beneficial Action Rules

The global beneficial-action hook blocks one PvP-consent case: a PvE player cannot perform beneficial actions on a PvP player. Other major checks still apply first or later, including null/map validation, staff rules, XMLPoints team membership, player-government restrictions, guild ally checks, and map beneficial restrictions.

| Source state | Target state | Result from PvP-consent check |
| :--- | :--- | :--- |
| PvE player | PvP player | Blocked. |
| PvE player | PvE or Neutral player | Not blocked by this check. |
| PvP player | PvE, PvP, or Neutral player | Not blocked by this check. |
| Neutral player | PvE, PvP, or Neutral player | Not blocked by this check. |

## Notoriety Rules

`MobileNotoriety` returns `Notoriety.Invulnerable` when both source and target are `PlayerMobile` instances and either player's `NONPK` flag is `NONPK`. This happens before murderer, criminal, XMLPoints, faction, guild, and government notoriety handling.

`NONPKinEvent` is not included in that explicit invulnerable check.

## Event Moongates

`NONPKEventMoongate` is an `Item` with `Target`, `TargetMap`, and `Dispellable` command properties at `AccessLevel.GameMaster`.

### Activation

| Trigger | Range behavior |
| :--- | :--- |
| `OnDoubleClick` | Player-only. Uses range 1 and sends localized "That is too far away" if outside range. |
| `OnMoveOver` | Player-only. Starts the gate check with range 0. |

Both paths start a 1-second `DelayTimer`, then validate range and either open a confirmation gump or move the player.

### Travel Restrictions

| Restriction | Result |
| :--- | :--- |
| Player carries a faction sigil | Sends localized message `1061632`; travel blocked. |
| Young player targets `Map.Lodor` | Sends localized message `1049543`; travel blocked. |
| Player has at least 1 kill and target map is not `Map.Lodor` | Sends localized message `1019004`; travel blocked. |
| Target map requires an expansion flag not present in `NetState.Flags` | Sends localized message `1019004`; travel blocked. |
| Player is casting a spell | Sends localized message `1049616`; travel blocked. |
| `TargetMap` is null or `Map.Internal` | Sends `This moongate does not seem to go anywhere.` |
| Valid target map | Teleports pets, moves the player, plays sound `0x1FE` for visible/player users, then runs `OnGateUsed`. |

### Flag Toggle

| Current player flag | New flag after use | Extra effect |
| :--- | :--- | :--- |
| `NONPK` | `NONPKinEvent` | None. |
| `NONPKinEvent` | `NONPK` | Clears `m.Combatant`. |
| `Null` | Unchanged | None. |
| `PK` | Unchanged | None. |

`NONPKEventConfirmationMoongate` is separate from `NONPKEventMoongate`. Despite its name, it subclasses `Moongate` and only customizes warning-gump behavior; it does not contain the `NONPK`/`NONPKinEvent` toggle.

## Example Flag-Gated Items

| Item | Class | Equip rule | Weapon properties |
| :--- | :--- | :--- | :--- |
| `PK sword` | `PKSword` | `OnEquip` succeeds only when the wearer is a `PlayerMobile` with `NONPK == NONPK.PK`. | Base sword art `0xF61`, Armor Ignore, Concussion Blow, Silver slayer, Force damage, Surpassingly accuracy, Massive durability. |
| `NONPK sword` | `NONPKSword` | `OnEquip` succeeds only when the wearer is a `PlayerMobile` with `NONPK == NONPK.NONPK`. | Same base weapon stats and magic levels as `PKSword`. |

Both item classes call `base.OnEquip` only after the flag check succeeds, then return `true`. Both cast `from` to `PlayerMobile` without a null guard.

## Area Damage Rules

| Source | PvP-consent behavior |
| :--- | :--- |
| Conflagration potion field | Non-player targets are always valid. Against `PlayerMobile` targets, PvE throwers can only damage themselves; PvP and Neutral throwers can damage PvP and Neutral players. |
| Frostbite potion field | Uses the same player-target validity rules as conflagration fields. |
| `MonsterSplatter` | Allows PvE player attackers to damage uncontrolled non-player `BaseCreature` targets, blocks player attackers from damaging PvE players or PvE-owned pets, allows XMLPoints challengers, then falls through to government and guild checks. |

The potion field checks do not handle `NONPKinEvent` as PvP, PvE, or Neutral for player targets; if the thrower has `NONPKinEvent`, the explicit player-target cases do not match.

## Persistence

### PlayerMobile

`PlayerMobile.Serialize` currently writes version `37`. The PvP-consent state is part of the version 33 data block:

| Write order in current serializer | Data |
| :--- | :--- |
| After city, city title, city title display, owes-back-taxes, and back-taxes amount | `(int)m_NONPK` |
| Immediately after the flag | `Title` |

`PlayerMobile.Deserialize` reads the flag and title in `case 33`, then falls through to older data blocks. Characters saved before version 33 do not contain this field and therefore keep the enum default unless later code changes it.

### GoddessOfProtection

`GoddessOfProtection` writes version `0` after `base.Serialize(writer)` and reads that version after `base.Deserialize(reader)`. It does not persist custom fields.

### NONPKEventMoongate

`NONPKEventMoongate` writes version `1`, then writes `Target`, `TargetMap`, and version 1 field `Dispellable`. Deserialization reads `Target` and `TargetMap` for all versions and only reads `Dispellable` when `version >= 1`.

### NONPKEventConfirmationMoongate

`NONPKEventConfirmationMoongate` writes version `0` and persists warning-gump dimensions, colors, title/message clilocs, and optional message string using encoded integers plus `ReadString`.

### PKSword And NONPKSword

Both sword classes write and read version `0` and persist no custom fields beyond their base weapon state.

## Admin Notes

* `PlayerMobile.NONPK` is exposed as `[CommandProperty(AccessLevel.Administrator)]`, so administrators can inspect and edit it through normal property tooling.
* `NONPKEventMoongate.Target`, `TargetMap`, and `Dispellable` are exposed as `[CommandProperty(AccessLevel.GameMaster)]`.
* `Scripts.csproj` currently lists `Custom\PvPConsent\PKNONPKGUMP.cs`, while the traced gump file lives at `Custom\PvPConsent\Gumps\PKNONPKGUMP.cs`. Verify project inclusion before relying on MSBuild-only script compilation.

## Technical Trace

* Target acquisition -> audit row named `PvP Consent System` pointed at the chooser gump and was marked `Missing`.
* Entry points -> traced `[ChoosePvP`, `PKNONPK.OnResponse`, Thuvia speech and movement hooks, event moongate movement/double-click hooks, `PlayerMobile.NONPK`, and the global notoriety handlers.
* Combat enforcement -> traced `Mobile_AllowHarmful`, `Mobile_AllowBeneficial`, `MobileNotoriety`, `BaseRegion.AllowHarmful`, potion field validity checks, and `MonsterSplatter.CanDealDamageTo`.
* Persistence -> traced `PlayerMobile` version 33 flag/title reads inside the current version 37 serializer, plus version blocks for the event gate, warning moongate, NPC, and sample swords.
