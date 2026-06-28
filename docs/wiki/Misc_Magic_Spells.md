# Misc Magic Spells

## Scope

This page documents the miscellaneous magic package implemented under `Data/Scripts/Magic/Misc/`.
It covers the `MagicalSpell` base class, item-triggered special spells, AI attack spell behavior, rune-bag casting, and the magic-object helper classes.

This is not the standard 64-spell Magery reference. For regular Magery spells and scrolls, see [Magery Spell System](Magery_Spell_System.md). For the shared casting lifecycle and registry behavior, see [Base Spell Framework](Base_Spell_Framework.md).

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Magic/Misc/MagicalSpell.cs` | Abstract `Spell` subclass for miscellaneous spells. It defaults cast and damage skill to Focus, requires each subclass to define skill, mana, and circle, and does not consume reagents. |
| `Data/Scripts/Magic/Misc/AttackSpells.cs` | AI-facing harmful spell that chooses a themed visual/damage package from caster skills, mana, creature type, and dungeon context. |
| `Data/Scripts/Magic/Misc/TravelSpell.cs` | Recall-like travel spell used by runebook charges and marked travel objects. |
| `Data/Scripts/Magic/Misc/IdentifySpell.cs` | Item target spell that delegates identification to `ArtifactManual.LookupTheItem()`. |
| `Data/Scripts/Magic/Misc/ThorLightningSpell.cs` | Item spell that applies an energy lightning strike to a harmful mobile target. |
| `Data/Scripts/Magic/Misc/SummonDragonSpell.cs` | Targeted item spell that summons a temporary `SummonDragon` mobile. |
| `Data/Scripts/Magic/Misc/SummonSnakesSpell.cs` | Targeted item spell that summons a temporary `SummonSnakes` mobile. |
| `Data/Scripts/Magic/Misc/SummonSkeleton.cs` | Direct item spell that summons a temporary `SummonSkeleton` mobile. |
| `Data/Scripts/Magic/Misc/BaseMagicObject.cs` | Charge-based magical weapon base for obsolete artifact-style spellcasting items. |
| `Data/Scripts/Magic/Misc/MagicCastingItems.cs` | Helper used by the base spell framework to treat magic staffs, teleport robes, and `BaseMagicObject` items as no-skill casting sources. |
| `Data/Scripts/Magic/Misc/MagicObjectTarget.cs` | Target cursor that delegates final targeting back to a `BaseMagicObject`. |
| `Data/Scripts/Magic/Misc/RuneMagic.cs` | Rune bag, full rune bag, rune stone classes, and an in-world rune magic book. |

## Registry And Entry Points

`Initializer` attempts to register this package as spell IDs `700` through `706`:

| ID | Spell |
| --- | --- |
| `700` | `SummonSnakesSpell` |
| `701` | `SummonDragonSpell` |
| `702` | `ThorLightningSpell` |
| `703` | `AttackSpells` |
| `704` | `SummonSkeletonSpell` |
| `705` | `IdentifySpell` |
| `706` | `TravelSpell` |

Several runtime paths bypass numeric registry lookup and construct these spell classes directly:

| Entry point | Behavior |
| --- | --- |
| `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs` and `Data/Scripts/Mobiles/Base/Behavior.cs` | Return `new AttackSpells(m_Mobile, null)` for some AI spell choices. |
| `Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs` | Casts `SummonDragonSpell` from Merlin's Mystical Staff after the staff cooldown check. |
| `Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs` | Casts `SummonSnakesSpell` from Staff of the Serpent after the staff cooldown check. |
| `Data/Scripts/Items/Magical/HydraTooth.cs` | Casts `SummonSkeletonSpell` directly from the hydra tooth scroll item. |
| `Data/Scripts/System/Gumps/RunebookGump.cs` | Casts `TravelSpell` from runebook entries when using runebook recall charges. |
| Obsolete `BaseMagicObject` artifacts | `GandalfsStaff`, `StaffofSnakes`, and `HammerofThor` cast the dragon, snake, and lightning spells through the charge-based object base. |

`MagicCastingItem.CastNoSkill()` returns true for `BaseMagicStaff`, `RobeOfTeleportation`, `Artifact_RobeOfTeleportation`, and `BaseMagicObject`.
The base `Spell` class uses that result to avoid normal skill fizzle, hand clearing, and some scroll-source handling for those item casts.

## Spell Catalog

| Spell | Circle | Skill/Mana | Main behavior |
| --- | --- | --- | --- |
| `TravelSpell` | First | Focus, `1` mana | Recalls to marked runes, default runebook entries, valid boat keys, house raffle deeds, or a runebook-supplied entry. It checks overload, source and destination travel rules, blocked locations, multis, and runebook charges. |
| `IdentifySpell` | First | Focus, `1` mana | Targets visible items and calls `ArtifactManual.LookupTheItem()`; on success it runs the sequence check and plays a sound. |
| `ThorLightningSpell` | Eighth | Focus, `1` mana | Targets a harmful mobile, runs `CheckHSequence()`, applies reflect handling, then deals pure energy lightning damage. |
| `AttackSpells` | Eighth | Magery damage/cast skill, `4` base mana | AI-oriented target spell that uses the larger of caster Magery or Necromancy, current mana, and a `Wizardry()` theme to pick damage type, visual, poison, and resist behavior. |
| `SummonSkeletonSpell` | First | Focus, `1` mana | Checks follower capacity for one slot, then summons a `SummonSkeleton` for 60 minutes. |
| `SummonSnakesSpell` | Eighth | Focus, `20` mana | Checks follower capacity for two slots, targets a spawnable/town-legal location, and summons a `SummonSnakes` mobile for 120 seconds. |
| `SummonDragonSpell` | Eighth | Focus, `30` mana | Checks follower capacity for three slots, targets a spawnable/town-legal location, and summons a `SummonDragon` mobile for 120 seconds. |

## Travel Behavior

`TravelSpell` is a lower-cost, Focus-based recall variant rather than a regular Magery spell.
It can be created without a preset destination, in which case the caster receives a target cursor, or with a runebook entry and runebook supplied by the runebook UI.

Supported targeted travel objects are:

| Target | Destination |
| --- | --- |
| Marked `RecallRune` | Rune target location and map. |
| `Runebook` with a default entry | The default runebook entry. |
| Boat `Key` with a valid linked `BaseBoat` | The boat's marked location, without the multi-block check. |
| Valid `HouseRaffleDeed` | The deed's plot location and facet. |

Before moving the caster, the spell checks `SpellHelper.CheckTravel()` from the source and to the target, `Worlds.AllowEscape()`, `Worlds.RegionAllowedRecall()`, `Worlds.RegionAllowedTeleport()`, overload state, blocked spawn locations, and optional multi blocking.
When a runebook is supplied, the spell also requires at least one charge and decrements the charge only after the final sequence check succeeds.

## AI Attack Spell

`AttackSpells` is used by custom AI spell selection as a broad themed attack instead of a single player-facing spell.
The target path:

| Step | Behavior |
| --- | --- |
| Power selection | Uses the larger of caster Magery and Necromancy, then caps a random circle by current mana. |
| Mana spend | Subtracts a manual mana amount from `2` through `46` based on the selected circle. |
| Damage cap | Starts from half the selected skill and caps damage by circle before AOS damage scaling. |
| Theme selection | `Wizardry()` returns air, any, cold, fire, main, nature, necro, storm, or water based on dungeon type and many creature classes. |
| Effect selection | The theme chooses one of more than 60 visual/damage packages, including physical, fire, cold, poison, energy, paralyze, mana/stamina drain, and poison application. |
| Final damage | Applies `SpellHelper.CheckReflect()`, Magic Resist mitigation, controlled-creature bonus damage, and `SpellHelper.Damage()`. |

## Rune Magic

`RuneBag` is an alternate casting interface built from physical rune stones.
`RuneBagCast()` scans the bag contents for 26 named rune classes such as `An`, `Bet`, `Corp`, `Flam`, `Kal`, `Vas`, and `Ylem`, then maps exact rune combinations and item counts to numeric spell IDs.

The implemented formula table covers:

| Spell family | IDs |
| --- | --- |
| Standard Magery | `0` through `63` |
| Necromancy | `100` through `116` |

Before casting, the rune bag requires `DesignContext.Check()` to pass and the rune bag to be inside the caster's backpack.
If a formula is recognized, it calls `SpellRegistry.NewSpell(m_SpellID, m, this)` and casts the returned spell.
If no formula matches, the caster receives "Not a known spell"; if registry construction fails, the caster receives the standard temporarily-disabled message.

The package also defines `FullRuneBag`, which creates a starter bag containing one of each rune stone, a `RuneJournal`, and a `RuneBag`.
Each rune stone class serializes only version `0`; the rune identity is represented by the concrete class and item name/item ID.

## Magic Objects

`BaseMagicObject` is an older charge-based item base built on `BaseBashing`.
It stores a `MagicObjectEffect` and charge count, displays remaining charges when identified, and applies a 4-second use delay through `BeginAction(typeof(BaseMagicObject))`.

The base double-click flow requires the item to be equipped by the user, calls `OnMagicObjectUse(from)` when charges remain, consumes a charge, and removes `SpellChanneling` when charges reach zero.
The default use path opens a `MagicObjectTarget`; obsolete staff subclasses instead override `OnMagicObjectUse()` and directly cast a special spell while temporarily setting the object immovable.

## Serialization Notes

| Class | Serialized data |
| --- | --- |
| `BaseMagicObject` | Version `0`; `MagicObjectEffect` as an integer and remaining charges. |
| `RuneBag`, `FullRuneBag`, `RuneStone`, concrete rune stones, `RuneMagicBook`, and `RuneBagBook` | Version `0`; no subclass payload. |
| `SummonDragon`, `SummonSnakes`, and `SummonSkeleton` | Version `0`; no subclass payload beyond `BaseCreature`. |
| `TravelSpell`, `IdentifySpell`, `ThorLightningSpell`, `AttackSpells`, and summon spell objects | Transient cast objects; they do not serialize world state. |

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpellRegistry` allocates `m_Types` with 700 slots, but `Initializer` registers this package at IDs `700` through `706`. | Numeric registry lookup cannot create these miscellaneous spells; direct constructors from AI, runebook, or item code still work where used. |
| `AttackSpells.Target()` applies manual mana spend and final damage without calling `CheckSequence()`, `CheckHSequence()`, or `CheckBSequence()`. | The AI attack path can bypass shared final sequence validation, including the standard resource, source-item, fizzle, and recovery contracts. |
| `SummonDragon.OnThink()` and `SummonSnakes.OnThink()` iterate `GetMobilesInRange(5)` without freeing the pooled enumerable. | Frequent summoned-creature thinking can leak pooled range enumerables. |
| The rune formula for `Vengeful Spirit` checks `Kal`, `Xen`, one `Bet`, and total item count `4`; the source comment notes that the formula should require two `Bet` runes. | A rune bag can cast Vengeful Spirit with one `Bet` plus any fourth rune instead of the intended duplicate-rune recipe. |

## Admin Notes

When testing these spells, check the entry point first.
Some are registry IDs, some are direct item constructors, and `AttackSpells` is normally reached through AI spell selection rather than through player spellbooks.
For rune-bag failures, inspect the exact contained rune classes and item count before debugging the target spell.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0051.
- Backlog rows: RB-06725.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Misc/ (CurrentDirectory)
- Data/Scripts/Magic/Misc/MagicalSpell.cs (CurrentFile)
- Data/Scripts/Magic/Misc/AttackSpells.cs (CurrentFile)
- Data/Scripts/Magic/Misc/TravelSpell.cs (CurrentFile)
- Data/Scripts/Magic/Misc/IdentifySpell.cs (CurrentFile)
- Data/Scripts/Magic/Misc/ThorLightningSpell.cs (CurrentFile)
- Data/Scripts/Magic/Misc/SummonDragonSpell.cs (CurrentFile)
- Data/Scripts/Magic/Misc/SummonSnakesSpell.cs (CurrentFile)
- Data/Scripts/Magic/Misc/SummonSkeleton.cs (CurrentFile)
- Data/Scripts/Magic/Misc/BaseMagicObject.cs (CurrentFile)
- Data/Scripts/Magic/Misc/MagicCastingItems.cs (CurrentFile)
- Data/Scripts/Magic/Misc/MagicObjectTarget.cs (CurrentFile)
- Data/Scripts/Magic/Misc/RuneMagic.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/Behavior.cs (CurrentFile)
- Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs (CurrentFile)
- Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs (CurrentFile)
- Data/Scripts/Items/Magical/HydraTooth.cs (CurrentFile)
- Data/Scripts/System/Gumps/RunebookGump.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Event=2; Gump=9; Speech=3; Timer=4; WorldLoad=1.
- Data/Scripts/Magic/Misc/BaseMagicObject.cs:L115 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L9665 WorldLoad WorldLoad access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L9779 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Base/Behavior.cs:L10029 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L10397 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Base/Behavior.cs:L13063 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L13068 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L13120 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L14200 Event EventSink access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L15092 Event EventSink access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L15624 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/Behavior.cs:L15626 Speech OnSpeech access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 40.
- Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs:Server.Items.Artifact_GandalfsStaff version=1 serialize=L78 deserialize=L85 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs:Server.Items.Artifact_StaffofSnakes version=1 serialize=L73 deserialize=L80 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Items/Magical/HydraTooth.cs:Server.Items.HydraTooth version=0 serialize=L43 deserialize=L49 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/BaseMagicObject.cs:Server.Items.BaseMagicObject version=0 serialize=L146 deserialize=L154 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.An version=0 serialize=L678 deserialize=L684 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.Bet version=0 serialize=L704 deserialize=L710 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.Corp version=0 serialize=L730 deserialize=L736 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.Des version=0 serialize=L756 deserialize=L762 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.Ex version=0 serialize=L782 deserialize=L788 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.Flam version=0 serialize=L808 deserialize=L814 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.FullRuneBag version=0 serialize=L612 deserialize=L618 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Misc/RuneMagic.cs:Server.Items.Grav version=0 serialize=L834 deserialize=L840 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs=Keep
- Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs=Keep
- Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs=Keep
- Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs=Keep
- Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs=Keep
- Data/Scripts/Items/Magical/HydraTooth.cs=Keep
- Data/Scripts/Items/Magical/HydraTooth.cs=Keep
- Data/Scripts/Magic/Misc/AttackSpells.cs=Keep
- Data/Scripts/Magic/Misc/AttackSpells.cs=Keep
- Data/Scripts/Magic/Misc/BaseMagicObject.cs=Keep
- Data/Scripts/Magic/Misc/BaseMagicObject.cs=Keep
- Data/Scripts/Magic/Misc/IdentifySpell.cs=Keep
- Data/Scripts/Magic/Misc/IdentifySpell.cs=Keep
- Data/Scripts/Magic/Misc/MagicalSpell.cs=Keep
- Data/Scripts/Magic/Misc/MagicalSpell.cs=Keep
- Data/Scripts/Magic/Misc/MagicCastingItems.cs=Keep
- Data/Scripts/Magic/Misc/MagicCastingItems.cs=Keep
- Data/Scripts/Magic/Misc/MagicObjectTarget.cs=Keep
- Data/Scripts/Magic/Misc/MagicObjectTarget.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
