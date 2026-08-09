# POST-BATCH-Q Staff Command Metadata Source Review Closeout

Reviewed at: 2026-06-14T16:17:42.2744356-05:00

## Scope

POST-BATCH-Q processed the 92 active `ReadyForSourceBatch` rows from `POST-BATCH-N-STAFF-COMMAND-METADATA` in `post-batch-n-source-readiness-queue.csv`. The review covered command registration, access metadata, Usage/Description-adjacent command conventions, parser/argument guards, deleted-mobile guards, staff/player trigger scope, and nearby source behavior.

## Results

- Review rows: 92
- System summary: XMLSpawner=68; Static Gump Tool=22; Custom:ChangeSeason=1; Custom:Character Swap=1
- Decision summary: Fixed=86; SafeNoChange=5; ReviewedNoChange=1
- Remaining active `POST-BATCH-N-STAFF-COMMAND-METADATA` rows: 0
- Remaining active `POST-BATCH-N` `ReadyForSourceBatch` rows: 23
- Active `QueuedSourceFollowUp` rows: 0

## Source Changes

Confirmed command handler defects were fixed with narrow guard changes only. Static Gump Tool command handlers now use a shared command-mobile guard before closing or sending gumps. XMLSpawner and custom command handlers now guard null or deleted mobile state and argument arrays where the existing command parser dereferences them. XMLPoints command handlers now use a shared command-mobile helper.

The config-driven XMLSpawner command rehash and global show/hide/trace commands were source-reviewed and left unchanged where they do not consume command-event mobile state.

No command names, access levels, public APIs, namespaces, type names, file locations, project files, XML/config files, serialization layout, or staff/player workflow semantics were intentionally changed.

## Verification

Verification passed:

- targeted command/access/metadata scans passed
- `git diff --check` passed with expected LF-to-CRLF warnings only
- initial sandboxed Visual Studio MSBuild hit user-profile SDK access denial
- escalated `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors
- `.\ConficturaServer.exe -compileonly -nocache` passed with compile-only verification completed successfully

## Outputs

- `docs/codebase-audit/outputs/post-batch-q-staff-command-metadata-source-review.csv`
- `docs/codebase-audit/outputs/post-batch-q-staff-command-metadata-closeout.md`
- `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
