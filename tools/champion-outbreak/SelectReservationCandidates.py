"""Rank terrain leads against private copied-world geometry; never approve sites."""
import argparse
import collections
import csv
import json
from pathlib import Path


def select(options):
    services = set(options.service_types.read_text(encoding="utf-8").splitlines())
    constraints = []
    with options.geometry.open(encoding="utf-8", newline="") as stream:
        for kind, x, y, width, height in csv.reader(stream):
            if kind in ("house", "city", "portal"):
                constraints.append((kind, int(x), int(y), int(width), int(height)))
    for filename, kind in ((options.mobiles, "service"), (options.items, "legacy_champion")):
        with filename.open(encoding="utf-8") as stream:
            for line in stream:
                row = json.loads(line)
                if row["map"] != "Sosaria" or not (0 <= row["x"] < 5120 and 0 <= row["y"] < 3128):
                    continue
                if kind == "service" and row["type"] in services:
                    constraints.append((kind, row["x"], row["y"], 1, 1))
                elif kind == "legacy_champion" and "ChampionSpawn" in row["type"]:
                    # Conservative envelope; raw export does not expose SpawnArea.
                    constraints.append((kind, row["x"] - 80, row["y"] - 80, 161, 161))
    groups = collections.defaultdict(dict)
    rejected = collections.Counter()
    for filename in options.terrain:
        for line in filename.read_text(encoding="utf-8").splitlines():
            if not line.startswith("TERRAIN_PASS\t"):
                continue
            fields = line.split("\t")
            x, y, connected = map(int, fields[2:5])
            if len(fields) < 10:
                raise ValueError("Terrain log lacks static architecture screening; rerun the current probe")
            if int(fields[9]) != 0:
                rejected.update(["static_roof"])
                continue
            # A 161-square proposed reservation plus another 24 tiles of surveyed
            # clearance. These observations do not forbid future outside housing.
            conflicts = {kind for kind, xx, yy, width, height in constraints
                         if xx < x + 105 and xx + width > x - 104
                         and yy < y + 105 and yy + height > y - 104}
            if conflicts:
                rejected.update(conflicts)
                continue
            nearest = min(max(abs(x - (xx + width / 2)) - width / 2,
                              abs(y - (yy + height / 2)) - height / 2)
                          for _, xx, yy, width, height in constraints)
            groups[fields[1]][x, y] = dict(id=fields[1], x=x, y=y, connected=connected,
                                          nearest_copied_constraint=nearest,
                                          boss=fields[5], broods=fields[6:9])
    result = []
    for name, candidates in sorted(groups.items()):
        best = max(candidates.values(), key=lambda row: (row["connected"], row["nearest_copied_constraint"]))
        best["clear_alternatives"] = len(candidates)
        result.append(best)
        print(f"LEAD {name}: ({best['x']},{best['y']}), {best['connected']} connected tiles; "
              f"nearest copied constraint {best['nearest_copied_constraint']}; {len(candidates)} alternatives")
    options.output.write_text(json.dumps(dict(status="proposed_not_approved", sites=result,
                                             rejected_by_constraint=rejected), indent=2) + "\n", encoding="utf-8")
    print(f"Selected {len(result)} regional leads. Current live occupancy, roads/access and client review remain unverified.")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--terrain", type=Path, action="append", required=True)
    parser.add_argument("--service-types", type=Path, required=True)
    parser.add_argument("--geometry", type=Path, required=True)
    parser.add_argument("--mobiles", type=Path, required=True)
    parser.add_argument("--items", type=Path, required=True)
    parser.add_argument("--output", type=Path, required=True)
    select(parser.parse_args())
