# Safe pet logout and weekly stable billing verification

Date: 2026-11-05. Base: main at 0d9d5d7d. This is isolated verification, not deployment or native-client acceptance.

## Delivered behavior

Instant disconnection protects owned, eligible pets and loaded cargo before disconnect subscribers can save the world, even above normal stable capacity. Delayed logout retains capacity and loaded-carrier exclusions. Startup recovery, pending automatic returns, manual interfaces, temporary mount shelter and summoning use the shared stable service.

Every stay receives seven free days. Thereafter, each pet costs 30 gold per real week plus recursive StrongBackpack cargo weight, fixed at intake. All elapsed weekly boundaries accrue in UTC; the five-minute callback collects bank gold/checks. Explicit claims can also use backpack gold. Insufficient funds are not partially withdrawn. Unpaid pets remain safe and pending. City-origin fees retain the originating city beneficiary.

## Results and evidence

Final operational run:

`output/stabling-verification/ee4f93b0cabe4124884a2e78cc5cd9fa/`

| Evidence class | Command or artifact | Result |
| --- | --- | --- |
| Operational executable build | scripts/Test-Stabling.ps1; server-build.log | Release/x86 Server.csproj built in isolated output; tracked root executable untouched. |
| Runtime script compilation | runtime-compile.log | Uninstrumented production snapshot passed -compileonly -nocache. |
| Production components | tests/StableRegression.cs; regression.log | 90 assertions passed against the compiled production assembly, without loading saves or starting listeners/timer threads. |
| Copied historical world | tests/StableWorldRoundtrip.cs; world-write.log | Adopted 863 existing pre-billing stable records with zero debt and seven-day grace; saved version 38 in a disposable copy. |
| Independent copied-world reload | world-read.log | Preserved owner/stable/pending references, loaded nested cargo, rate, UTC deadline and outstanding balance; no duplicate accrual. |
| Original save protection | Test-Stabling.ps1 before/after SHA-256 comparison | All original save-file hashes unchanged. Copies and account data remain local and ignored. |
| Startup initialization smoke | Data/Logs/Console.log; startup-smoke.stderr.log | -service -nocache reached the post-Initialize fixture milestone and exited before game listeners. The unrelated Orb startup call was disabled only in the fixture; the runner asserts absence of its port-2594 listener message. |
| IDE/project hygiene | output/stabling-verification/ide-build.log | ConficturaUO.sln Release/Any CPU built with isolated output/intermediates, including Scripts.csproj. Existing unrelated warnings remain. |
| Project/source truth | output/stabling-verification/project-truth-equivalent.json | 6,599 source files and compile entries; zero missing, unlisted or duplicate entries. Literal paths and URI decoding applied. |
| Documentation and diff checks | Local link/path checks; git diff --check | Passed. |

The phase inventory parser required an absent intermediate source inventory, so an equivalent XML/filesystem comparison was used and retained instead.

Component scenarios include full/over-capacity stables, loaded nested cargo and custom carriers, public regions, qualifying homes and visitors, bedroll protection, delayed logout, reconnect reset, ghost/follower limits, null/foreign references, interrupted internalization and return callbacks, every intake/retrieval interface, first-week boundaries, repeated callbacks, elapsed weeks, bank checks, insufficient combined funds, city revenue and deletion, and positional version-37/38 reads.

## Retained failed observations

Earlier output directories remain local:

- The first runtime compile rejected HashSet because the runtime script references omit System.Core. The production implementation now uses Dictionary.
- Regression caught automatic-return registration being removed by the control-master callback. Registration now follows master clearing.
- A nonvirtual serializer fixture failed verification; it was replaced with unique version-prefix extraction and an independently loaded historical-world test.
- The first startup fixture lacked turn-based combat data and failed initialization. Runtime data is now copied into the fixture.
- An intermediate smoke still started Orb through its static constructor despite renaming its initializer. That run is not accepted as listener-isolation evidence. The final fixture disables the actual StartServer call, preserves the production source unchanged, and checks the service log for absence of the Orb listener.

Injected callback exceptions in passing regression logs are intentional failure cases and remain visible as evidence.

## Save compatibility and rollout

PlayerMobile version 38 writes, after the base payload: captured protection bool, billing count, and version-0 inline records (pet mobile, int rate, DateTime UTC ticks, long debt, optional city item), followed by the unchanged version-37 payload. Reader case 38 consumes that prefix and falls through to 37. Creature serialization and type identities are unchanged.

Back up complete saves immediately before deployment. Existing pets acquire seven-day grace on adoption, with no retroactive debt. Rollback requires restoring compatible code and the pre-update saves together; version-37-only code must not load version-38 player saves. No deployment, live player recovery or native client test was performed.
