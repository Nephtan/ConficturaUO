# SOURCE-BATCH-313 WeaponEngravingTool Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-313`
- Candidate: `SB313-CAND-001`
- Behavior: add stale/null/mobile/backpack/target/gump response guards to WeaponEngravingTool interaction, target, helper, and gump paths.
- System: `Items:Special:Veteran Rewards / WeaponEngravingTool`
- File: `Data/Scripts/Items/Special/Veteran Rewards/WeaponEngravingTool.cs`

## Allowed Source Change

Add guard-only checks to:

- `WeaponEngravingTool.OnDoubleClick(Mobile from)`
- `WeaponEngravingTool.Recharge(Mobile from, Mobile guildmaster)`
- `WeaponEngravingTool.Find(Mobile from)`
- `TargetWeapon.OnTarget(Mobile from, object targeted)`
- `InternalGump.OnResponse(NetState state, RelayInfo info)`
- `ConfirmGump.OnResponse(NetState state, RelayInfo info)`

The guards may return early for null/deleted mobiles, deleted source tools, missing backpacks, null/deleted target weapons, null `NetState`, null `RelayInfo`, or stale gump state before dereferences, gump creation, engraving mutation, charge decrement, recharge, or response messaging.

## Must Stay Unchanged

- `RewardSystem.CheckIsUsableBy` behavior
- Target prompt
- `BaseWeapon` eligibility
- `InternalGump` layout/text flow
- 64-character truncation
- Blank engraving removal
- Localized messages
- `UsesRemaining` decrement
- Blue diamond and 100000 gold recharge rules
- Guildmaster recharge flow
- `IsRewardItem` persistence
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-313 WeaponEngravingTool Guard Repair
```
