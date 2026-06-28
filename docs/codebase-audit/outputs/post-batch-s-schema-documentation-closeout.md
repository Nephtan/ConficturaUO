# POST-BATCH-S Schema Documentation Queue Closeout

Reviewed at: 2026-06-14T20:33:44.4399529-05:00

## Scope

POST-BATCH-S processed the 232 active `SchemaDocsOnly` rows from `POST-BATCH-N-SCHEMA-DOCUMENTATION` in `post-batch-n-source-readiness-queue.csv`. The active overlay confirmed the same 232 rows were still active before this batch.

This was a documentation/audit-only batch. It did not edit C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy.

## Results

- Review rows: 232
- Decision summary: Documented=226; FalsePositive=6
- Review class summary: PathConstantOrSchemaReference=130; XmlDocumentSchemaSurface=40; GeneratedOrWrittenDataSurface=38; DirectoryGlobSchemaSurface=10; DiagnosticStringNotSchemaPath=6; TextConfigReadSurface=6; ExistenceGateSchemaSurface=2
- Top systems: System:Misc=112; System:Commands=33; ServerCore=12; Housing=10; Items:Houses=10; Regions=10; Bulk Orders=4; Custom:Voting=4; Custom:XMLSpawner=4; System:Gumps=4
- Top files: Data/Scripts/System/Misc/Logs.cs=68; Data/Scripts/Items/Houses/ComponentVerification.cs=16; Data/Scripts/System/Misc/Build.cs=16; Data/Scripts/System/Misc/Accounts.cs=8; Data/Scripts/System/Misc/Reporting.cs=8; Data/Scripts/System/Obsolete/Obsolete.cs=8; Data/System/Source/Region.cs=8; Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs=6
- Remaining active `POST-BATCH-N-SCHEMA-DOCUMENTATION` rows: 0
- Remaining active `ReadyForSourceBatch` rows: 0
- Active `QueuedSourceFollowUp` rows: 0
- Remaining active `POST-BATCH-N` rows in other lanes: 214

## Schema Evidence Review

Each row was resolved against current source before disposition. The review CSV records the readiness row id, backlog/source ids, system, source file, schema/data reference or pattern, original queue line, current source line, parser/read/write evidence, documented contract, missing/default behavior note, reload assumption note, invalid-input note, decision, and reason.

The batch documented 226 real schema/data-contract references. These include XML/config paths, text config readers, generated/exported text or cfg outputs, directory glob surfaces, path constants, and existence gates. No schema normalization, parser change, migration policy, data-file edit, or runtime behavior change was made.

The six false-positive rows were diagnostic string literals captured by the queue extractor, not schema/data paths:

- PBS-0049/PBN-0274: Data/System/Source/Region.cs:L938 -> Could not find root element 'ServerRegions' in Regions.xml
- PBS-0052/PBN-0277: Data/System/Source/Region.cs:L968 -> Invalid region type '{0}' in regions.xml
- PBS-0061/PBN-0286: Data/System/Source/Items/Container.cs:L1918 -> Warning: double ItemID entry in containers.cfg
- PBS-0066/PBN-0291: Data/System/Source/Region.cs:L938 -> Could not find root element 'ServerRegions' in Regions.xml
- PBS-0067/PBN-0292: Data/System/Source/Region.cs:L968 -> Invalid region type '{0}' in regions.xml
- PBS-0070/PBN-0295: Data/System/Source/Items/Container.cs:L1918 -> Warning: double ItemID entry in containers.cfg

## Verification

Verification passed for this docs-only batch:

- confirm review CSV contains all 232 scoped rows
- confirm active overlay contains no remaining active `POST-BATCH-N-SCHEMA-DOCUMENTATION` rows
- confirm every review row resolved to a current source file and line
- confirm only docs/codebase-audit artifacts changed
- `git diff --check` passed with expected LF-to-CRLF working-copy warnings only

Source build and compile-only runtime verification are not required because this batch did not change C# source, project files, XML/config/data, or runtime behavior.

## Outputs

- `docs/codebase-audit/outputs/post-batch-s-schema-documentation-review.csv`
- `docs/codebase-audit/outputs/post-batch-s-schema-documentation-closeout.md`
- `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
