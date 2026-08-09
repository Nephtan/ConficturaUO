# OmniAI

## Overview
OmniAI is a `BaseAI` replacement in `Server.Mobiles` that lets a creature mix standard movement and targeting with optional Bushido, Knightship, Magery, Necromancy, and Ninjitsu actions. It is not a standalone engine with spawners or persistence hooks; it is attached per creature by returning `new OmniAI(this)` from that creature's `ForcedAI` override.

## Core Scripts
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Core.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Bushido.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Knightship.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Necromancy.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Ninjitsu.cs`
- `Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs`

## How A Creature Uses It
Override `ForcedAI` on a `BaseCreature` subclass:

```csharp
protected override BaseAI ForcedAI
{
    get { return new OmniAI(this); }
}
```

`AITester` is the included sample mobile. It assigns `ForcedAI` to `new OmniAI(this)` and uses its constructor argument to enable one primary module at `120.0`: Magery (`0`), Necromancy (`1`), Bushido (`2`), or Ninjitsu (`3`).

## Skill Gates
OmniAI only checks a small set of skills, and every gate is `Base > 10.0`:

- `Musicianship` marks Bard support as available, but there is no active bard power implementation in combat.
- `Bushido` enables Bushido stance and strike logic.
- `Knightship` enables chivalry-style spell picks and healing support.
- `Magery` enables spell damage, buffs, curses, mana drain, and healing support.
- `Necromancy` enables necromancy attacks, familiars, curses, and healing support.
- `Ninjitsu` enables stealth movement, smoke bombs, ninja moves, and shuriken throws.

`m_SwapWeapons` is true only when Bushido or Ninjitsu is enabled. `m_Melees` stays true for ranged-free creatures that do not rely only on Magery or Necromancy, and false for pure ranged/caster behavior.

## Main Loop
`Think()` does three OmniAI-specific checks before falling back to `BaseAI`:

1. Every 2 seconds it calls `CheckForFieldSpells()`.
2. Every 3 seconds it calls `CheckArmed(m_SwapWeapons)`.
3. If the mobile currently has a targeting cursor from a spell or ability, it resolves that cursor through `ProcessTarget()`.

`DoActionWander()` and `DoActionGuard()` both try to hide first when Ninjitsu is available and the creature is not poisoned. If no target is found while wandering, the AI meditates when mana is missing and Meditation is above `60.0`; otherwise it runs base wandering and then attempts self-healing.

`DoActionCombat()` keeps the current combatant only while the target is alive, on the same map, and within practical pursuit range. If health drops below 20% and the creature is not controlled, summoned, or paragon, OmniAI may switch to `Flee` with a flat 10% chance or a higher chance based on how far behind it is in hit points.

## Healing Logic
`TryToHeal()` is skipped for summoned creatures and respects a cooldown through `m_NextHealTime`.

- If health is above 75% and poison is absent or below Lesser, no heal is attempted.
- Healing choices are sorted by the creature's current skill values among `Magery`, `Necromancy`, `Knightship`, and `Healing`.
- Magery uses `Cure`, `Greater Heal`, or `Heal` depending on poison and mana.
- Necromancy does not cast a necromancer spell here; it simply uses `Spiritualism`.
- Knightship uses `Cleanse By Fire` for poison or `Close Wounds` when mana is at least 10.
- `Healing` starts a self-bandage with a dex-based delay.

## Combat Modules

### Bushido
Bushido alternates between stances and attacks.

- Stances: `Confidence` at `25.0+`, `CounterAttack` at `40.0+`, and `Evasion` at `60.0+`.
- Attack moves: `HonorableExecution` when the target is low enough to finish, `LightningStrike` at `50.0+`, `MomentumStrike` at `70.0+`, or weapon primary/secondary abilities from Tactics skill.

### Knightship
`KnightshipPower()` usually casts a spell and only sometimes falls back to `UseWeaponStrike()`.

- `RemoveCurse` is preferred when OmniAI detects a negative stat mod or suspects necromantic pressure from the opponent.
- `HolyLight` requires `Knightship >= 55.0` and at least 10 mana.
- `DispelEvil` is only used when nearby enemies include summoned creatures, uncontrolled evil creatures, or transformed targets.
- `DivineFury` requires `Knightship >= 35.0` and is skipped if already active.
- `ConsecrateWeapon` is only chosen when the current weapon exists and is not already consecrated.

### Magery
Magery always tries support spells before raw damage.

- `CheckBless()` is evaluated first.
- `CheckCurse()` is evaluated second.
- There is then a 25% poison attempt against a non-poisoned combatant.
- Mana-drain logic becomes more likely against opponents with caster-related skills.
- The summon branch is effectively disabled because `m_CanUseMagerySummon` always returns `false`.
- If none of the above returns a spell, OmniAI picks a damage spell from `AttackSpells`, `Harm`, `Fireball`, `Lightning`, `MindBlast`, `Explosion`, `EnergyBolt`, or `FlameStrike`, weighted by Magery skill and current mana.

### Necromancy
Necromancy rotates among army-building, curses, Blood Oath, shapeshifting, and damage.

- It can summon a familiar or cast `Animate Dead`.
- Curse picks include `CorpseSkin`, `EvilOmen`, `MindRot`, and `Strangle`, based on skill and roll.
- Shapeshifts are throttled by a 130-second cooldown and can choose `WraithForm`, `HorrificBeast`, `LichForm`, or `VampiricEmbrace`.
- Damage picks include `CurseWeapon`, `PainSpike`, `PoisonStrike`, `Wither`, `Strangle`, and `VengefulSpirit`.

Necromancer shapeshifting is only allowed when the mobile is a `BaseVendor`, because `m_CanShapeShift` returns true only for that type.

### Ninjitsu
Ninjitsu supports stealth-first movement and burst tools.

- The first ninjitsu action grants the AI 3 to 5 smoke bombs.
- Hidden attackers choose `Backstab`, `SurpriseAttack`, or `KiAttack` based on skill.
- Visible attackers have a 20% chance to cast `MirrorImage`; otherwise they choose `FocusAttack`, `DeathStrike`, or a normal weapon move.
- While fleeing, a ninja can spend 10 mana and one smoke bomb to hide immediately.
- Shuriken throws can occur every 5 to 15 seconds within 12 tiles, deal 3 to 5 physical damage, and apply poison tiers from Lesser up to Lethal based on Ninjitsu skill.

## Target Resolution
`ProcessTarget()` handles the spell cursor after OmniAI selects an action.

- Harmful targets resolve against the current combatant if the target is visible, in LOS, and within range.
- Beneficial targets resolve on the AI's own mobile.
- Teleport and Shadowjump search nearby valid landing tiles around the combatant, then fall back to random valid points.
- `AnimateDead` scans nearby corpses and rejects player corpses, low-fame corpses, bonded pets, summons, and channeled corpses.
- `FindRandomTarget()` and `CanTarget()` are helper methods used to screen enemies and allies based on criminal state, murder counts, guilds, parties, summon/control ownership, blessed status, and hidden staff.

## Known Code Defects
- Weapon swapping is functionally broken. `Think()` calls `CheckArmed()` every 3 seconds, but `CheckArmed()` immediately returns when `DateTime.Now > m_NextWeaponSwap`, which is true on first use and prevents the equip/swap block from running.
- Magery summons are dead code in the shipped implementation because `m_CanUseMagerySummon` is hard-coded to `false`, so the summon-selection logic is never reached.
- Necromancer shapeshifting is practically restricted to vendor mobiles because `m_CanShapeShift` only returns true for `BaseVendor`.

## Serialization
`OmniAI` itself does not define custom RunUO serialization in this package. The included `AITester` mobile does serialize correctly with version `0`.

## Audience
Staff and developers

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0052.
- Backlog rows: RB-06739.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Core.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Bushido.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Knightship.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Necromancy.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Ninjitsu.cs (CurrentFile)
- Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Event=1; Timer=1.
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Ninjitsu.cs:L90 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs:L227 Event EventSink access=GlobalOrInternal

### Serialization Evidence

- Serialized rows matched: 1.
- Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs:Server.Mobiles.AITester version=0 serialize=L123 deserialize=L129 alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/AITester.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Bushido.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Bushido.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Core.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Core.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Knightship.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Knightship.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Magery.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Necromancy.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Necromancy.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Ninjitsu.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Ninjitsu.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs=Keep
- Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
