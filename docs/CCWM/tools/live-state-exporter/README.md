# CCWMLiveStateExporter

`CCWMLiveStateExporter` is offline CCWM tooling. It must stay under
`docs/CCWM/tools/live-state-exporter/` and must not be placed in `Data/Scripts`
or added to `Data/Scripts/Scripts.csproj`.

The tool is a standalone console app that references the server project and
compiles `Data/Scripts` in memory at runtime so it can load committed saves
through the normal RunUO serialization constructors. It does not register RunUO
commands, does not define an `Initialize` method, and does not start the server
network loop.

Expected command shape:

```powershell
CCWMLiveStateExporter --saves Saves --out docs/CCWM/live-state --scan Sosaria,1186,1321,0,18 --run 396
```

The exporter intentionally assumes the saves directory is named `Saves` because
the RunUO `World` loader uses fixed relative `Saves/...` paths. To export from a
copy, run the tool from a directory that contains the copied `Saves` folder.

Build it through the CCWM-only solution, not `ConficturaUO.sln`:

```powershell
msbuild docs/CCWM/tools/CCWM.Tools.sln /p:Configuration=Release /p:Platform="Any CPU"
```

Outputs:

- `manifest.yaml`
- `mobiles.jsonl`
- `items.jsonl`
- `spawners.jsonl`
- optional `scans/run-<run>-<x>-<y>.yaml`

After every refresh, run:

```powershell
python docs/CCWM/tools/ccwm_verify.py
```
