# Base Spell Framework

## Scope

This page documents the shared `Server.Spells.Spell` infrastructure used by Confictura spell lines.
It covers the base cast lifecycle, spell metadata, registry lookup, resource checks, disturbance handling, delayed damage context, and helper APIs that derived spell classes rely on.

This is not a player spell list. For player-facing spell-bar usage, see [Magic Toolbars Guide](Magic_Toolbars_Guide.md). For standard Magery spell contents, see the dedicated Magery spell documentation when it is available.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Magic/Base/Spell.cs` | Abstract `Spell` base class, cast state machine, resource checks, fizzle checks, damage scaling, disturbance handling, cast timers, and target sequence helpers. |
| `Data/Scripts/Magic/Base/SpellInfo.cs` | Metadata container for spell name, mantra, reagents, reagent amounts, animation action, town permission, and hand particle effects. |
| `Data/Scripts/Magic/Base/SpellRegistry.cs` | Static spell and special-move registry used by numeric spell IDs and name-based spell construction. |
| `Data/Scripts/Magic/Base/Initializer.cs` | Registers spell IDs for Magery, Necromancy, Chivalry, custom schools, special moves, research spells, and item spells. |
| `Data/Scripts/Magic/Base/SpellHelper.cs` | Shared helpers for damage timing, travel checks, town checks, fields, healing, stat mods, transformations, and delayed spell damage. |
| `Data/Scripts/Magic/Base/SpellState.cs` | Defines `None`, `Casting`, and `Sequencing` cast states. |
| `Data/Scripts/Magic/Base/DisturbType.cs` | Defines disturbance causes such as equip requests, use requests, damage, death, and new casts. |
| `Data/Scripts/Magic/Base/SpellCircle.cs` | Defines the eight standard Magery circles. |
| `Data/Scripts/Magic/Base/Reagent.cs` | Maps named reagent properties to item `Type` entries used by `SpellInfo`. |
| `Data/Scripts/Magic/Base/SpecialMove.cs` | Shared Samurai/Ninja special-move framework registered through `SpellRegistry.SpecialMoves`. |
| `Data/Scripts/Magic/Base/UnsummonTimer.cs` | Utility timer that deletes temporary summoned creatures. |
| `Data/Scripts/Magic/Base/SpellBooks.cs` | Randomized spellbook item variants and serialization wrappers for several spellbook families. |

## Spell Object Model

Each concrete spell is an ephemeral object created with a caster, an optional scroll or casting item, and a `SpellInfo` instance.
The base object exposes the caster, scroll, metadata, start time, current `SpellState`, mantra, reagent list, and default skill hooks.

Derived spell classes must provide:

| Member | Purpose |
| --- | --- |
| `OnCast()` | Performs the spell-specific effect or assigns a target after the cast timer completes. |
| `GetMana()` | Returns the base mana cost before `LowerManaCost` and Mind Rot scaling. |
| `CastDelayBase` | Returns the base cast delay before faster-casting adjustment. |

Most spell families also override `GetCastSkills(out double min, out double max)`, `CastSkill`, `DamageSkill`, `ConsumeReagents()`, `GetMana()`, or `CastDelayBase` to provide school-specific behavior.
For example, `MagerySpell` supplies circle-based skill ranges, mana costs, resist checks, and cast delay.

## Cast Lifecycle

| Step | Framework behavior |
| --- | --- |
| `Cast()` entry | Records `StartCastTime`, disturbs a previous sequencing spell under AOS rules, then checks blessed state, world restrictions, school conflicts, liveness, active casts, transformation locks, frozen/paralyzed state, recovery time, calm state, and current mana. |
| Begin-cast approval | Requires `Mobile.CheckSpellCast(this)`, `CheckCast()`, and `Region.OnBeginSpellCast(caster, this)` to succeed before the spell state changes. |
| State transition | Sets `State` to `Casting`, assigns `Caster.Spell = this`, reveals the caster when `RevealOnCast` is true, and says the mantra for player casters. |
| Visual setup | Starts an animation timer for human bodies when `ShowHandMovement` is true and applies configured left/right hand particle effects. |
| Hand and ability cleanup | Clears hands unless the scroll/casting item is exempt, and clears current weapon ability under ML rules. |
| Cast timer | Starts a one-shot `CastTimer` for `GetCastDelay()`, then calls `OnBeginCast()`. |
| Sequencing | When the timer fires, state becomes `Sequencing`, `Mobile.OnSpellCast()` and `Region.OnSpellCast()` run, `NextSpellTime` is set from `GetCastRecovery()`, and the derived `OnCast()` method is called. |
| Target timeout | If `OnCast()` assigns a new player target, the framework applies a 30-second target timeout. |

Derived spells are expected to call `CheckSequence()`, `CheckBSequence(target)`, or `CheckHSequence(target)` before applying final effects, then call `FinishSequence()` when the effect path is done.

## Final Sequence Checks

`CheckSequence()` is the main final gate before a spell effect should take resources or apply results.
It verifies that the caster is alive, the active mobile spell still points to this spell object, state is `Sequencing`, and any scroll-like source is still valid and still rooted on the caster unless it is an exempt casting item.

If the spell remains valid, the method checks reagents, mana, AOS frozen/paralyzed state, calm state, and `CheckFizzle()`.
On success it consumes mana, drains Elementalism stamina, consumes eligible scroll/potion items or magic-staff charges, clears hands when appropriate, awards karma, applies Vampiric Embrace garlic backlash, and returns `true`.
On failure it sends the matching fizzle or resource message and returns `false`.

Use the target helpers for common target polarity:

| Helper | Behavior |
| --- | --- |
| `CheckBSequence(target)` | Requires a living target unless `allowDead` is true, verifies `CanBeBeneficial`, runs `CheckSequence()`, then calls `DoBeneficial(target)`. |
| `CheckHSequence(target)` | Requires a living target, verifies `CanBeHarmful`, runs `CheckSequence()`, then calls `DoHarmful(target)`. |

## Disturbance And Recovery

`Spell` implements the core `ISpell` callbacks used by `Mobile` and `Region`.

| Event | Base response |
| --- | --- |
| Caster hurt | Player casters are disturbed while casting unless Protection or Elemental Protection prevents it. |
| Caster killed | Disturbs with `DisturbType.Kill`. |
| Connection changed | Calls `FinishSequence()`. |
| Movement | Blocks movement while casting when `BlocksMovement` is true. |
| Equip request | Disturbs a spell in `Casting`. |
| Use-object request | Disturbs a spell in `Sequencing`. |
| Town cast check | Returns `SpellInfo.AllowTown`. |

`Disturb()` clears the state, clears `Caster.Spell`, stops active cast/animation timers, calls `OnDisturb()`, cancels targeting when sequencing, and applies recovery timing where appropriate.

## Damage And Resource Scaling

The base damage path combines dice damage, Inscribe bonus, Intelligence bonus, spell-damage-increase item attributes, skill scaling, target scalar, slayer scalar, region scalar, creature damage hooks, and Evasion.
AoS spell damage caps are pulled from server settings for monster and player targets.

Mana is scaled through Mind Rot and `LowerManaCost`, with the lower-mana-cost cap clamped to 40 percent.
Cast recovery uses `CastRecovery` under AOS and defaults to a 0.75-second delay outside AOS.
Cast delay applies faster-casting caps by school, applies the Protection penalty, and never drops below `CastDelayMinimum`.

Delayed damage spells can opt out of stacking by returning `false` from `DelayedDamageStacking`.
`SpellHelper` then stores the active delayed-damage timer per spell type and target so a newer timer can replace the older one.

## Registry And Construction

`Initializer.Initialize()` calls `SpellRegistry.Register(id, type)` for standard spells, custom spell schools, special moves, research spells, and miscellaneous item spells.
`SpellRegistry.NewSpell(int id, Mobile caster, Item scroll)` creates a concrete `Spell` from the registered numeric ID using a two-argument constructor.
`SpellRegistry.NewSpell(string name, Mobile caster, Item scroll)` searches a fixed list of spell namespace segments and attempts the same constructor path by type name.

Special moves are registered through the same registry.
When a registered type subclasses `SpecialMove`, the registry creates one move instance and stores it in `SpellRegistry.SpecialMoves` for later lookup.

## Serialization Notes

`Spell` instances are transient cast objects and do not serialize world state.
Persistent data lives on items, mobiles, spellbooks, scrolls, toolbar fields, transformation contexts, or spell-specific helper items and timers.

When adding a spell-related `Item` or `Mobile`, follow the normal RunUO positional serialization rules for that object.
The base spell framework does not provide a serializer to inherit for individual cast instances.

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpellRegistry` allocates `m_Types` with 700 slots, but `Initializer` attempts to register IDs `700` through `813`. `SpellRegistry.Register()` silently ignores IDs greater than or equal to `m_Types.Length`. | Misc item spells, Death Knight spells, Holy Man spells, and Enchantment spells registered at those IDs are not available through numeric `SpellRegistry` lookup or registry-number lookup. |
| `CheckSequence()`, `CheckBSequence()`, and `CheckHSequence()` already run `CheckFizzle()` through the final sequence path, but several derived Enchantment spells also chain an extra `CheckFizzle()` after those helpers. | Those casts can roll success and skill checks twice, making successful effects less likely and causing behavior to diverge from the base framework contract. |
| `SpellRegistry.NewSpell()` catches all constructor exceptions and returns `null` without logging. | Broken spell constructors or wrong constructor signatures can fail silently, making registry-backed cast failures hard to diagnose. |
| `Mobile.Spell` only writes a console warning when a live spell reference is overwritten. | Overlapping direct-cast paths can replace the active spell object without structured recovery or caller-facing diagnostics. |

## Admin Notes

The base spell framework has no dedicated staff command.
For debugging, inspect the caller's active `Mobile.Spell`, `NextSpellTime`, mana, transformation state, equipped spellbook/casting item, and any spell-specific item or timer state involved in the spell family being tested.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0071.
- Backlog rows: RB-06665.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Base/Spell.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpellInfo.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpellRegistry.cs (CurrentFile)
- Data/Scripts/Magic/Base/Initializer.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpellHelper.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpellState.cs (CurrentFile)
- Data/Scripts/Magic/Base/DisturbType.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpellCircle.cs (CurrentFile)
- Data/Scripts/Magic/Base/Reagent.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpecialMove.cs (CurrentFile)
- Data/Scripts/Magic/Base/UnsummonTimer.cs (CurrentFile)
- Data/Scripts/Magic/Base/SpellBooks.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Initialize=1; Timer=8.
- Data/Scripts/Magic/Base/Initializer.cs:L8 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Magic/Base/SpecialMove.cs:L325 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/Spell.cs:L1219 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/Spell.cs:L1251 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/SpellHelper.cs:L30 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/SpellHelper.cs:L1357 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/SpellHelper.cs:L1392 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/SpellHelper.cs:L1755 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Base/UnsummonTimer.cs:L6 Timer CustomTimerSubclass access=GlobalOrInternal

### Serialization Evidence

- Serialized rows matched: 7.
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MyElementalSpellbook version=0 serialize=L186 deserialize=L192 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MyNecromancerSpellbook version=0 serialize=L99 deserialize=L105 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MyNinjabook version=0 serialize=L359 deserialize=L365 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MyPaladinbook version=0 serialize=L502 deserialize=L508 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MySamuraibook version=0 serialize=L425 deserialize=L431 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MySongbook version=0 serialize=L567 deserialize=L573 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Base/SpellBooks.cs:Server.Items.MySpellbook version=0 serialize=L297 deserialize=L303 alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/Magic/Base/DisturbType.cs=Keep
- Data/Scripts/Magic/Base/DisturbType.cs=Keep
- Data/Scripts/Magic/Base/Initializer.cs=Keep
- Data/Scripts/Magic/Base/Initializer.cs=Keep
- Data/Scripts/Magic/Base/Reagent.cs=Keep
- Data/Scripts/Magic/Base/Reagent.cs=Keep
- Data/Scripts/Magic/Base/SpecialMove.cs=Keep
- Data/Scripts/Magic/Base/SpecialMove.cs=Keep
- Data/Scripts/Magic/Base/Spell.cs=Keep
- Data/Scripts/Magic/Base/Spell.cs=Keep
- Data/Scripts/Magic/Base/SpellBooks.cs=Keep
- Data/Scripts/Magic/Base/SpellBooks.cs=Keep
- Data/Scripts/Magic/Base/SpellCircle.cs=Keep
- Data/Scripts/Magic/Base/SpellCircle.cs=Keep
- Data/Scripts/Magic/Base/SpellHelper.cs=Keep
- Data/Scripts/Magic/Base/SpellHelper.cs=Keep
- Data/Scripts/Magic/Base/SpellInfo.cs=Keep
- Data/Scripts/Magic/Base/SpellInfo.cs=Keep
- Data/Scripts/Magic/Base/SpellRegistry.cs=Keep
- Data/Scripts/Magic/Base/SpellRegistry.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
