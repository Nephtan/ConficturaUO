# Inherited spider ability decision

Status: Proposed design revision; no production source change or tuning applied.

The normal-AI reference-v1 run died after 130.215 seconds, after killing one guardian and leaving its ten defenders alive. This demonstrates a failed run and a poor add-management policy. It does not isolate the cause of death or establish that the encounter is overtuned. Preserve that distinction when interpreting the following source finding.

## Source finding

`ArachnidOutbreakSpider` inherits `DreadSpider` without overriding its breath. The parent is a mage with a separate spider breath attack. That attack deals damage, then calls ordinary `Mobile.Paralyze`; it does not create the new warning/restraint items.

- `Data/Scripts/Mobiles/Insects/Spiders/DreadSpider.cs`: `HasBreath` returns true; `BreathDealDamage` selects base form 6; 118–132 ordinary health; Fame 5,000.
- `Data/Scripts/Mobiles/Base/BaseCreature.cs:847`: default breath scalar is 0.20 of current health. `BreathComputeDamage` at line 2222 caps the initial result at 100, then enforces at least `DamageMax`.
- `BaseCreature.BreathDealDamage` at line 1046 applies that damage. Form 6 at line 1201 requests paralysis for Fame/200 seconds, capped at 15. Incoming damage may break ordinary paralysis; this is not a guarantee of fifteen uninterrupted seconds.
- `Data/Scripts/Magic/Base/Spell.cs:665`: ordinary paralysis/freeze blocks casting. This differs from the outbreak's movement-only restraint.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakCreatures.cs`: guardians receive 3,000 health while retaining the parent's breath calculation. Healthy ordinary spiders calculate 23–26 raw breath damage; a healthy guardian reaches the 100 raw cap before resistances. Health and breath damage are therefore coupled, particularly as health falls below the cap threshold.

The new boss's three-second warning/four-second movement restraint is not the only web/control mechanic present. The inherited breath can obscure that intended counterplay. Treating a health adjustment as an independent pacing change would also overlook this dependency. These are source facts; attribution of the failed run needs further observation.

## Recommended revision for review

Disable only the inherited breath on `ArachnidOutbreakSpider`:

```csharp
public override bool HasBreath
{
    get { return false; }
}
```

Keep its ordinary mage AI, melee, poison, skills, drops and finite reinforcement budgets. Keep the boss's explicit telegraphed web. Ordinary mage spells can still apply their usual control effects; this proposal does not promise immunity to all paralysis. Normal world `DreadSpider` remains unchanged.

This narrows the event's special web behavior and removes the guardian health/breath coupling. It is a real enemy-profile change, beyond the agreed automatic tuning limits, so it needs a design decision before implementation. It would require forced runtime compilation, targeted inherited/owned behavior checks, native combat observation and new complete reference runs. No serialized type/field/order change is proposed.

The alternative is to retain the inherited breath deliberately and test/design around its mage, poison and paralysis pressure. In either case, strengthen the scripted policy to prioritize nearby low-health defenders and freeze that policy before further tuning. Do not count the reference-v1 death as an unsuccessful tuning pass: no encounter values were changed.
