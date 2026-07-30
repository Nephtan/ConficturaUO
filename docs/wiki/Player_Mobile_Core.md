# Player Mobile Core

## Overview

`PlayerMobile` is the shard's concrete player character `Mobile`. It extends the RunUO `Mobile` base class, implements `IHonorTarget`, and centralizes player-only lifecycle hooks, persistent shard settings, title display, death handling, item insurance, follower tracking, young-player behavior, spellbar state, buff icons, quest bridge state, and MyRunUO invalidation.

This file is not a command package. Most player-facing commands live under `Data/Scripts/System/Commands/Player/` and read or mutate the public `PlayerMobile` fields documented here. The core class itself is reached through engine lifecycle events, `Mobile` overrides, context menu entries, packet throttling, serialization, and other systems casting a `Mobile` to `PlayerMobile`.

## Source Trace

| File | Type | Purpose |
| --- | --- | --- |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | `Mobile` subclass | Core player character implementation and persistence. |
| `Data/Scripts/Mobiles/Base/PlayerSettings.cs` | helper class | String-slot helpers for player settings, quest flags, discovered worlds, keys, gump hue, loot choices, and related UI state. |
| `Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs` | command and helper script | Player command entry point for `PlayerMobile.CharacterSheath`. |
| `Data/Scripts/System/Commands/Player/ClassicPoisoning.cs` | command script | Player command entry point for `PlayerMobile.ClassicPoisoning`. |
| `Data/Scripts/System/Commands/Player/SkillListing.cs` | command and gump script | Player command entry point for compact skill display behavior. |
| `Data/Scripts/System/Commands/Player/SpellBarsFunctions.cs` | player settings helper script | Reads and writes string-backed `PlayerMobile` spellbar settings. |
| `Data/Scripts/System/Commands/Player/QuickBar.cs` | command and gump script | Player quickbar entry point using `PlayerMobile.QuickBar` state. |
| `Data/Scripts/Mobiles/Base/BaseCreature.cs` | shared creature base | Adds and removes controlled and summoned creatures from `PlayerMobile.AllFollowers`. |

## Core Entry Points

| Entry point | Trigger | Compiled behavior |
| --- | --- | --- |
| `PlayerMobile.Initialize()` | Script load | Registers movement throttling for packet `0x02`, hooks `EventSink.Login`, `Logout`, `Connected`, and `Disconnected`, and schedules `CheckPets()` under `Core.SE`. |
| `OnLogin(LoginEventArgs)` | Login event | Runs virtue/champion title atrophy, applies account lockdown handling, and claims auto-stabled pets for `PlayerMobile` callers. |
| `OnLogout(LogoutEventArgs)` | Logout event | Auto-stables eligible pets. |
| `EventSink_Connected` | Network connection | Starts session time, starts the active quest timer, clears `BedrollLogout`, updates `LastOnline`, starts disguise timers, and clears special moves on a zero-delay timer. |
| `EventSink_Disconnected` | Network disconnect | Removes active house design context, ejects entities from the customized foundation, accumulates game time, stops quest timers, clears speech log, updates `LastOnline`, and stops disguise timers. |
| `Deserialize(GenericReader)` | World load | Reads save version `37`, migrates older versions through fall-through cases, rebuilds stabled flags, restores hiding buff icon state, and clears monster race state when monster characters are disabled. |
| `Serialize(GenericWriter)` | World save | Cleans expired anti-macro entries, decays kills/titles, writes base `Mobile` state, then writes `PlayerMobile` version `37` data positionally. |

## Player Flags And Enums

### `PlayerFlag`

`PlayerFlag` is an integer bit field. The first sixteen bits are marked as default-distribution reserved in the comment, and current flags occupy those low bits.

| Flag | Public property or purpose |
| --- | --- |
| `Glassblowing` | `Glassblowing` command property. |
| `Masonry` | `Masonry` command property. |
| `SandMining` | `SandMining` command property. |
| `StoneMining` | `StoneMining` command property. |
| `ToggleMiningStone` | `ToggleMiningStone` command property. |
| `KarmaLocked` | Prevents normal positive karma behavior in systems that check it. |
| `AutoRenewInsurance` | Reinsures insured items on death when bank withdrawal succeeds. |
| `UseOwnFilter` | Per-player chat/filter flag. |
| `PublicMyRunUO` | Public MyRunUO display toggle; invalidates MyRunUO on change. |
| `PagingSquelched` | Help/page squelch flag. |
| `Young` | Young-player protection flag and name suffix. |
| `AcceptGuildInvites` | Guild invitation toggle. |
| `DisplayChampionTitle` | Champion title display toggle. |
| `HasStatReward` | Persistent stat reward flag. |

### Other Player Enums

| Enum | Values | Purpose |
| --- | --- | --- |
| `NONPK` | `Null`, `NONPK`, `NONPKinEvent`, `PK` | PvP consent state stored directly on `PlayerMobile`. |
| `NpcGuild` | `None`, standard trade guilds, `ElementalGuild`, `JestersGuild` | NPC guild membership and fee logic. |
| `SolenFriendship` | `None`, `Red`, `Black` | Solen quest/faction friendship state. |

## Persistent Player Settings

`PlayerMobile` contains many public fields that are persisted directly rather than wrapped in dedicated value objects. These are world-save sensitive because they are written and read positionally.

| Category | Fields |
| --- | --- |
| Account/session | `LastOnline`, `SessionStart`, `GameTime`, `m_GameTime`, murder decay timers. |
| Guild and NPC guild | `GuildRank`, `GuildMessageHue`, `AllianceMessageHue`, `NpcGuild`, `NpcGuildJoinTime`, `NpcGuildGameTime`, `NextBODTurnInTime`. |
| Government | `City`, `CityTitle`, `ShowCityTitle`, `OwesBackTaxes`, `BackTaxesAmount`. |
| PvP consent | `NONPK`, plus inherited title storage. |
| Travel/camping | `Camp`, `Bedroll`, `BedrollLogout`, `LastPetBallTime`, `LastEscortTime`. |
| UI preferences | `GumpHue`, `WeaponBarOpen`, `QuickBar`, `RegBar`, `MyLibrary`, `MyChat`, `MusicPlaylist`, `CharMusical`, `SkillDisplay`. |
| Play style | `CharacterEvil`, `CharacterOriental`, `CharacterBarbaric`, `CharacterElement`, `CharacterGuilds`, `CharacterBegging`, `CharacterWepAbNames`, `CharacterSheath`, `ClassicPoisoning`. |
| Quest and achievement strings | `CharacterKeys`, `CharacterDiscovered`, `StandardQuest`, `FishingQuest`, `AssassinQuest`, `MessageQuest`, `BardsTaleQuest`, `ThiefQuest`, `KilledSpecialMonsters`, `EpicQuestName`, `EpicQuestNumber`, `CharacterWanted`. |
| Spellbar strings | `SpellBarsMage1-4`, `SpellBarsNecro1-2`, `SpellBarsKnight1-2`, `SpellBarsDeath1-2`, `SpellBarsBard1-2`, `SpellBarsPriest1-2`, `SpellBarsMonk1-2`, `SpellBarsArch1-4`, `SpellBarsElly1-2`. |
| Character build caps | `SkillStart`, `SkillBoost`, `SkillEther`; load logic sets `Skills.Cap = SkillStart + SkillBoost + SkillEther`. |
| Followers and pets | `AllFollowers`, `AutoStabled`, inherited `Stabled`, `FollowersMax`, `Learning`. |
| Combat and magic runtime | `NinjaWepCooldown`, `ExecutesLightningStrike`, `PeacedUntil`, `AnkhNextUse`, `AcceleratedStart`, `AcceleratedSkill`, `EnemyOfOneType`, `WaitingForEnemy`. |
| Visibility and staff tools | `VisibilityList`, `PermaFlags`, `IgnoreMobiles`, `IsStealthing`, staff disguise template and temporary stat maxima. |
| Rewards and crafting | `ToTItemsTurnedIn`, `ToTTotalMonsterFame`, `BOBFilter`, acquired recipe dictionary. |

`PlayerSettings` initializes and reads many string slots lazily. Examples include quest payload slots, discovered-world flags, character keys, gump hue, spell hue, loot choices, and play-style setup helpers.

## Stat And Resistance Rules

| Mechanic | Formula or behavior |
| --- | --- |
| Hits maximum | Uses `PlayerLevelMod(Str, this) + BonusHits`. Under `Core.ML`, player `BonusHits` is capped at `25`. `MysticalFox` and `GreyWolf` animal forms add `20`. Staff disguise can override this with the template's `HitsMax`. |
| Stamina maximum | Uses `PlayerLevelMod(base.StamMax, this) + BonusStam`, unless staff disguise is active. |
| Mana maximum | Uses `PlayerLevelMod(base.ManaMax, this) + BonusMana`, unless staff disguise is active. |
| Raw stat reads | Under `Core.ML`, player `Str`, `Int`, and `Dex` getters return at most `150`. Staff and higher access bypass this getter cap. |
| Carry weight | `(Core.ML && Race == Human ? 100 : 40) + (int)(3.5 * Str)`. |
| Maximum resistance | Starts from base. Non-physical maximum resistance above `60` is capped to `60` while under `CurseSpell`. ML elves gain `+5` maximum Energy resistance after the curse cap. `ResearchRockFlesh` forces Physical maximum to `90`. |
| Minimum resistance | `magicResist = (int)(MagicResist.Value * 10)`. At `1000+`, `min = 40 + ((magicResist - 1000) / 50)`; at `400+`, `min = (magicResist - 400) / 15`. The result is capped by `MaxPlayerResistance` and cannot go below the base minimum. |
| Equipment validation | Stat, gender, race, ethics, and faction checks can move weapons, armor, clothing, and faction items into the backpack. Validation is delayed through `Timer.DelayCall(TimeSpan.Zero, ValidateEquipment_Sandbox)`. |
| Skill-dependent follower cap | If Herding, Veterinary, Druidism, and Taming are all `120.0+`, `FollowersMax = 8`; all `90.0+` gives `7`; all `60.0+` gives `6`; otherwise `5`. |

## Movement And Visibility

| Mechanic | Compiled behavior |
| --- | --- |
| Movement speed | Direction-only turns use mounted run speed. Mounted characters and animal forms with `SpeedBoost` use mount walk/run speeds; others use foot walk/run speeds. |
| Fastwalk prevention | Packet `0x02` throttling compares `m_NextMovementTime` to `DateTime.Now`, using a `0.4` second threshold. It is bypassed for counselor+ access, speed shoes, and active `WindRunner`, `SythSpeed`, `Celerity`, or `CheetahPaws`. |
| Fall damage | Non-teleport player movement with a Z drop over `20` subtracts `((zDrop / 20) * 10) - 5` Hits. |
| Hidden movement | Under `Core.SE`, hidden player movement consumes stealth steps. Running consumes two steps. If steps expire, the player reveals or rechecks Stealth. |
| Collision | Dead players ignore movable impassables while moving. Players can shove their own controlled followers, `MoonCritter`, ignored mobiles, and targets under `WraithFormSpell`. |
| Visibility list | A `PlayerMobile` can see another player if the target's `m_VisList` contains the viewer. |
| Staff disguise | `SetStaffDisguise(template)` temporarily replaces displayed stat maxima, current hits/stam/mana, hunger, and thirst. Passing `null` clears the disguise and clamps current stats. This state is not serialized by `PlayerMobile`. |

## Context Menus And Gumps

`GetContextMenuEntries()` adds player-specific context entries.

| Context | Entry behavior |
| --- | --- |
| Self with active quest | Adds quest context menu entries from `QuestSystem`. |
| Self, alive, insurance enabled | Adds item insurance toggle and auto-renew/cancel entries. |
| Self in owned house | Adds reclaim-vendor entry when internalized vendors exist; adds leave-house entry under AOS house rules. |
| Self with justice protectors | Adds cancel-protection entry. |
| Other player | Adds party invite/remove entries under AOS rules. |
| Other player in house | Friends can add an eject-player entry under AOS house rules. |

`CancelRenewInventoryInsuranceGump` is the local confirmation `Gump` used on `Core.SE` to disable inventory insurance auto-renewal.

## Item Insurance

Insurance is enabled when `Core.AOS` is true.

| Rule | Compiled behavior |
| --- | --- |
| Toggle command path | Self context menu target flow only; no standalone command is registered in `PlayerMobile`. |
| Initial insurance fee | `900` gold withdrawn from the bank if `item.PayedInsurance` is false. |
| Eligible items | Must be equipped or in the backpack, non-stackable, not cursed, not most containers, not `BagOfSending`, `KeyRing`, blessed spellbooks, `PotionKeg`, or `Sigil`. |
| Auto-renew | If `AutoRenewInsurance` is true, death withdraws `900` gold per insured item. Success keeps `PayedInsurance = true`; failure clears insurance and increments `m_NonAutoreinsuredItems`. |
| Challenge override | `XmlPoints.InsuranceIsFree(this, m_InsuranceAward)` makes insurance free and paid for that death. |
| Death move result | Insured items move to the backpack. Young players also keep movable items that would otherwise move to a corpse. |
| PvP award tracking | The most recent player damager, including a controlled creature's master, becomes `m_InsuranceAward`. |

## Death And Resurrection

| Stage | Compiled behavior |
| --- | --- |
| Guard sentencing | When `MyServerSettings.GuardsSentenceDeath()` is false and the last killer is a town guard or qualifying vendor, death is cancelled. The player is moved to a prison point, pets are teleported, hits/stam/mana refill, and many backpack consumables/tools are deleted. |
| Immortality items | `SeeIfJewelInBag.IHaveAJewel(this)` and `SeeIfGemInBag.IHaveAGem(this)` can cancel death. |
| Clone control | `CloneCommands.UncontrolDeath(this)` can cancel death when player control/possession is being unwound. |
| Trade cleanup | Active secure trades are cancelled through `NetState.CancelAllTrades()`, and held items are dropped. |
| Ammo recovery | Recoverable ammo is restored before death, and attackers can recover ammo after a delayed kill callback. |
| Crime/perma flags | `m_PermaFlags` are cleared on death; the corpse is marked criminal and Classic Stealing can make the player criminal. |
| Buff cleanup | Non-`RetainThroughDeath` buff icons are removed after death. |
| Young-player death | Young players can be teleported and receive `YoungDeathNotice` after a `2.5` second delay. |
| Resurrection | Restores hunger/thirst to `20`, refills hits/stam/mana, plays a random resurrection music entry, logs one of seven resurrection messages, stops any quest arrow, gives non-monster characters a `DeathRobe`, and syncs race state. |

## Auto-Stabling And Followers

`AllFollowers` is a player-owned list maintained by `BaseCreature` when control masters or summon masters change. `PlayerMobile` uses it for follower count display, auto-stabling, and guard-property display.

| Operation | Compiled behavior |
| --- | --- |
| Startup `CheckPets()` | Under `Core.SE`, scans all world mobiles. If a player has more followers than auto-stabled pets can account for, it calls `AutoStablePets()`. |
| `AutoStablePets()` | Runs on logout. It skips summoned pets on the same map, ridden mounts, pack animals with backpack contents, and cases where stable capacity is full. Eligible pets are ordered to stay, internalized, unmastered, marked stabled, restored to max loyalty, added to `Stabled`, and added to `m_AutoStabled`. |
| Summoned pet cleanup | Summoned pets off the player's map play anger sound and are deleted on a zero-delay timer. |
| `ClaimAutoStabledPets()` | Runs on login. Alive players reclaim pets if `Followers + pet.ControlSlots <= FollowersMax`. The pet is remastered, moved to the player's location/map, ordered to follow, unstabled, and removed from `Stabled`. |
| Over-cap claim | Pets that cannot fit stay in the stable and send localized message `1049612`. |

## Young Player System

| Mechanic | Compiled behavior |
| --- | --- |
| Flag | `Young` is stored in `PlayerFlag.Young` and appends `(Young)` to the name suffix. |
| Loss on kills | If a young player gains kills, the account removes young status with message id `0`. |
| Loss on skill total | If a young player reaches `SkillsTotal >= 4500`, the account removes young status with localized message `1019036`. |
| Attack protection | `CheckYoungProtection(from)` blocks attacks in young-protected regions unless the attacker is an ignoring creature or the active quest ignores young protection. The warning message is rate-limited to once per minute. |
| Young healing | `CheckYoungHealTime()` allows a young-heal event once every five minutes. |
| Logout | `BedrollLogout` makes `GetLogoutDelay()` return zero. |

## Speech, Chat, And Logs

| Mechanic | Compiled behavior |
| --- | --- |
| Dead speech | Dead players normally use base ghost speech mutation. ML players with `Spiritualism >= 100.0`, or AOS listeners with `Spiritualism >= 100.0`, bypass mutation. |
| Guild/alliance chat | New guild-system messages are routed through guild or alliance chat methods. Staff in range with higher access receive a generated `[Guild]` or `[Alliance]` message packet. |
| Speech log | When `SpeechLog.Enabled` and the player has `NetState`, `OnSpeech()` lazily creates a `SpeechLog` and records speech. The log is cleared on disconnect. |

## Commands And External Player UI

`PlayerMobile` itself does not call `CommandSystem.Register()`. Related command files register player commands that alter persistent `PlayerMobile` fields.

| Command | Access | Script | PlayerMobile state |
| --- | --- | --- | --- |
| `[sheathe` | `Player` | `AutoSheatheWeapon.cs` | Toggles `CharacterSheath`; `OnWarmodeChanged()` calls `AutoSheatheWeapon.From(this)`. |
| `[poisons` | `Player` | `ClassicPoisoning.cs` | Toggles `ClassicPoisoning` between special-infectious-strike mode and one-handed slashing/piercing hit mode. |
| `[skilllist` | `Player` | `SkillListing.cs` | Opens `SkillListingGump`; `SkillDisplay` controls whether locked skills display. |
| `[spellhue [<name>]` | `Player` | `SpellHue.cs` | Sets `MagerySpellHue` from the first integer argument. |
| `[quickbar` | `Player` | `QuickBar.cs` | Opens/configures the persistent `QuickBar` string. |
| `[regbar` / `[regclose` | `Player` | `RegBar.cs` | Opens/closes reagent bar UI backed by `RegBar`. |
| `[loot` | `Player` | `Loot.cs` | Opens loot-selection UI backed by `CharacterLoot` and a selected auto-loot container. |
| `[music` / `[musical` | `Player` | `MusicPlaylist.cs`, `MusicPlayer.cs` | Configures `MusicPlaylist` and `CharMusical`. |
| `[evil`, `[oriental`, `[barbaric` | `Player` | play-style command files | Toggle `CharacterEvil`, `CharacterOriental`, and `CharacterBarbaric` mutually. |
| Spellbar commands | `Player` | `SpellBarsCommands.cs`, `SpellBarsManage.cs`, `SpellBarsDisplay.cs` | Open, close, display, and edit spellbar strings for magic families. |

## Serialization

`PlayerMobile` writes version `37`. Every block is positional and falls through on read. New fields must be added with a version bump and read before falling through to older cases.

| Version | Data introduced or handled |
| --- | --- |
| `37` | `m_LastDecoConfirmTime`. |
| `36` | `UsingAncientBook`, `SpellBarsArch1-4`. |
| `35` | `SkillStart`, `SkillBoost`, `SkillEther`, plus cap migration from old `Skills.Cap` values. |
| `34` | Government city pointer/title/display/back-tax fields. |
| `33` | `NONPK` and `Title`. |
| `32` | `Camp`, `Bedroll`. |
| `31` | `MyChat`. |
| `30` | `RegBar`, `MyLibrary`. |
| `29` | Main custom settings block: MOTD, skill title selection, key/discovery strings, door settings, begging, weapon ability names, element, quest strings, spellbar strings, quickbar, thief/special kill/music/wanted/loot/music-style fields, epic quest fields, play-style flags, gump hue, weapon bar toggle, skill display, spell hue, and classic poisoning. |
| `28` | `PeacedUntil`. |
| `27` | `AnkhNextUse`. |
| `26` | `AutoStabled` mobile list. Older saves create a new empty list. |
| `25` | Acquired recipe dictionary. |
| `24` | Last honor loss. |
| `23` | Champion title info. |
| `22` | Last valor loss. |
| `21` | Treasures of Tokuno item/fame counters. |
| `20` | Alliance and guild message hues. |
| `19` | Guild rank and `LastOnline`. |
| `18` | `SolenFriendship`. |
| `17` / `16` | Active quest, done quest restart info, and `Profession`. Version 17 adds restart times. |
| `15` | Last compassion loss. |
| `14` | Compassion gains and next compassion day when gains are positive. |
| `13` / `12` | `BOBFilter`; version 13 removed the old paid-insurance list. |
| `11` | Migration for old paid-insurance item list. |
| `10` | Hair and beard mods. |
| `9` | Savage paint expiration. |
| `8` | NPC guild, join time, and guild game time. |
| `7` | Perma flags. |
| `6` | Tailor bulk-order timer. |
| `5` | Smith bulk-order timer. |
| `4` | Justice loss and justice protectors. |
| `3` | Sacrifice gain/loss and available resurrects. |
| `2` | `PlayerFlag` bit field. |
| `1` | Long-term murder decay, short-term murder decay, and game time. |

Notable non-serialized runtime state includes anti-macro tables, staff disguise template/stat override, active buff table, speech log, active design context, auto-loot bag cache, current session start, special movement throttle time, and many spell/combat runtime contexts.

## Known Issues

| Issue | Evidence from code trace |
| --- | --- |
| Movement throttling dereferences `pm` before checking it for null. | `MovementThrottle_Callback(NetState ns)` assigns `PlayerMobile pm = ns.Mobile as PlayerMobile;` and immediately calculates `pm.m_NextMovementTime - DateTime.Now` before the `pm != null` guard. A non-player `NetState.Mobile` would throw. |
| Auto-stabled pet claim has a broken null/deleted guard. | `ClaimAutoStabledPets()` checks `if (pet == null || pet.Deleted)`, but the branch then sets `pet.IsStabled = false` and checks `Stabled.Contains(pet)`. The null case still dereferences `pet`. |
| Some pooled range/client enumerables are not freed. | `SendToStaffMessage()` iterates `from.GetClientsInRange(8)` directly, and `DeltaEnemies()` iterates `this.GetMobilesInRange(18)` directly. Nearby code in `OnDroppedItemToWorld()` shows the expected `IPooledEnumerable` plus `Free()` pattern. |
| Guard-prison death cleanup assumes every player has a backpack. | The guard sentencing branch in `OnBeforeDeath()` loops through `this.Backpack.Items` without first checking `Backpack != null`, unlike the later insurance backpack scan. |
| Young death teleport defines non-Sosaria destination arrays but never selects them. | `YoungDeathTeleport()` declares Underworld, Serpent Island, and Isles of Dread destination arrays, but always assigns `list = m_SosariaDeathDestinations` before choosing the nearest point. |
| Auto-loot bag selection is static across the shard. | `LootChoiceUpdates` stores the selected loot container in one static `lootbag`, while `PlayerMobile.AutoLootBag` reads that static container and caches it per player. One player's selection can affect another player's auto-loot target. |
