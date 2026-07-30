# SOURCE-BATCH-219 ValentinesCard Guard Repair Closeout

## Summary

`SOURCE-BATCH-219` implemented `SB217-CAND-003` in `Data/Scripts/Items/Special/ValentinesCard.cs`.

`ValentinesCard.OnDoubleClick(Mobile from)` and `ValentinesCard.OnTarget(Mobile from, object targeted)` now guard stale/null/deleted interaction state before backpack checks, target assignment, signer reads, or property mutation.

## Preserved Behavior

- Unsigned-card gating, backpack-use message `1080063`, target prompt `1077497`, player/self/NPC/invalid-target messages `1077498`, `1077495`, `1077496`, and `1077488`, `To`/`From` assignment, property invalidation, label, blessed loot behavior, hue, randomized label metadata, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/ValentinesCard.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; confirmed the new mobile/source and deleted-target guards and preserved card prompt, signing messages, `To`/`From` assignment, property invalidation, and serialization methods.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache`: passed; runtime script compile completed successfully.
- `git diff --check`: passed with only expected CRLF working-copy warnings.
- Generated root build artifacts restoration: completed before staging.

## Result

Verified and ready for commit.
