# SOURCE-BATCH-446 Candidate Discovery Closeout

`SOURCE-BATCH-446` ran fresh non-gated candidate discovery after `SOURCE-BATCH-445`.

## Result

Recommended implementation target: `SB446-CAND-001` / `CustomHuePickerGump` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Misc/Dyes/CustomHuePicker.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `CustomHuePickerGump.OnResponse` dereferenced response, definition/group/hue data, and callback state before stale/null guards.
- Caller evidence: the gump is used by dye tub custom hue selection callbacks; valid callback behavior must remain unchanged.

## Skips

- `DisguiseKit.cs` was skipped as thieving identity/appearance/timer behavior.
- `BaseMagicObject.cs` was skipped as a broad serialized magic-object framework base class.
- spell target handler files were skipped as combat/travel/beneficial/harmful/magic policy behavior.
- remaining Government, Invasion, Homestead, StaffTools, economy/reward, housing/addon, pet, region/map, serializer, project/config/data, XML/config/data, and reorganization surfaces remain outside this runner.

## Decision

Proceed with `SOURCE-BATCH-446 CustomHuePickerGump Guard Repair`. Keep `SOURCE-BATCH-447+` pending fresh discovery after the source batch commits.
