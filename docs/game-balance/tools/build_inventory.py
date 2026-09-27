"""Rebuild the source census for the gameplay study; never changes game files.

Uses only the Python standard library and rg. This is a lexical inventory, not a
C# parser, a project-inclusion audit, or proof that a path executes in play.
"""

import argparse
import csv
import hashlib
import json
import re
import subprocess
from collections import Counter, defaultdict
from pathlib import Path
import xml.etree.ElementTree as ET


ROOT = Path(__file__).resolve().parents[3]
OUT = ROOT / "docs/game-balance/data"
ROOTS = ("Data/Scripts", "Data/System/Source")
DOMAIN = {
    "Custom": "Cross-system custom packages",
    "Items": "Equipment, rewards, objects and property",
    "Magic": "Magic and abilities",
    "Mobiles": "Characters, creatures and NPCs",
    "Quests": "Adventure and rewards",
    "System": "Shared rules, skills and world services",
    "Trades": "Production and economy",
    "ServerCore": "Engine and shared primitives",
}


def source_paths():
    result = subprocess.run(
        ["rg", "--files", *ROOTS, "-g", "*.cs", "-g", "!**/bin/**", "-g", "!**/obj/**"],
        cwd=ROOT, capture_output=True, text=True, check=True,
    )
    return sorted(p.replace("\\", "/") for p in result.stdout.splitlines())


def classify(path):
    parts = path.split("/")
    if path.startswith("Data/System/Source/"):
        return "ServerCore", "ServerCore/" + (parts[3] if len(parts) > 4 else "Core")
    root = parts[2]
    family = "/".join(parts[2:4]) if len(parts) > 4 else root + "/Root"
    if root == "Custom" and parts[3] in ("Combat", "PvE", "Progression", "Integrations", "ThirdParty", "StaffTools") and len(parts) > 5:
        family = "/".join(parts[2:5])
    return root, family


def csv_text(rows, fields):
    import io
    stream = io.StringIO(newline="")
    writer = csv.DictWriter(stream, fieldnames=fields, lineterminator="\n")
    writer.writeheader()
    writer.writerows(rows)
    return stream.getvalue()


def generate():
    sources = []
    texts = {}
    for path in source_paths():
        raw = (ROOT / path).read_bytes()
        text = raw.decode("utf-8-sig", errors="replace")
        texts[path] = text
        root, family = classify(path)
        sources.append(dict(path=path, root=root, family=family,
                            lines=len(text.splitlines()), sha256=hashlib.sha256(raw).hexdigest(),
                            coverage="LexicalInventory", runtime_boundary="StartupScriptCandidate" if root != "ServerCore" else "EngineSource"))
    families = defaultdict(list)
    for source in sources:
        families[source["family"]].append(source)
    family_rows = [dict(family=family, domain=DOMAIN[items[0]["root"]], files=len(items),
                        lines=sum(i["lines"] for i in items),
                        coverage="LexicalInventory; see docs/game-balance/coverage-and-evidence.md for review depth")
                   for family, items in sorted(families.items())]

    skills_source = texts["Data/System/Source/Skills.cs"]
    enum = re.search(r"public enum SkillName\s*\{(.*?)\n\s*\}", skills_source, re.S).group(1)
    names = re.findall(r"(\w+)\s*=\s*(\d+)", enum)
    labels = dict((int(i), label) for i, label in re.findall(r'new SkillInfo\(\s*(\d+),\s*"([^"]+)"', skills_source))
    skill_uses = defaultdict(list)
    for path, text in texts.items():
        for name, count in Counter(re.findall(r"\bSkillName\.(\w+)\b", text)).items():
            skill_uses[name].append((path, count))
    skill_rows = []
    for name, index in names:
        hits = skill_uses[name]
        skill_rows.append(dict(id=index, symbol=name, label=labels[int(index)],
                               reference_files=len(hits), lexical_references=sum(n for _, n in hits),
                               source="Data/System/Source/Skills.cs",
                               usage_paths=";".join(p for p, _ in hits),
                               coverage="LexicalReferences; numeric indexes and dynamic references are not captured"))

    # The XML settings are positional. Export gameplay values only, not network,
    # account or server-identifying settings. Values are checked-in, not live.
    settings_path = "Data/Scripts/System/Misc/Settings.cs"
    settings = texts[settings_path]
    mappings = re.findall(r"(?:if|else if)\s*\(setting == (\d+)\)\s*\{\s*(S_\w+)\s*=", settings)
    defaults = dict(re.findall(r"private static \w+ (S_\w+) = ([^;]+);", settings))
    allowed = set("FloorTrapTrigger GetUnidentifiedChance NoMacroing StatGain StatGainDelay PetStatGainDelay GetTimeBetweenQuests GetTimeBetweenArtifactQuests GetGoldCutRate AllowMacroResources CreaturesSearching GuardsSentenceDeath GuardsPatrolOutside GuardsSprint NoMountsInCertainRegions AllowAlienChoice DamageToPets CriticalToPets SpellDamageIncreaseVsMonsters SpellDamageIncreaseVsPlayers QuestRewardModifier PlayerLevelMod Quest FastFriends FriendsAvoidHeels FriendsGuardFriends BoatDecay HomeDecay HousesDecay HousesPerAccount SellChance SellCommonChance SellRareChance SellVeryRareChance BuyChance BuyCommonChance BuyRareChance AllowCraftMagic Bribery Resources SoldResource HouseStorage CorpseDecay BoneDecay HouseOwners LawnsAllowed ShantysAllowed MonsterCharacters SpecialWeaponAbilSkill Stables HPModifier TrainDummies PickDips TrainMulti Scary Belly SkillBoost SkillGain FoodCheck BondDays BuyCloth".split())
    nodes = ET.parse(ROOT / "Info/settings.xml").getroot().findall("setting")
    settings_rows = []
    for position, key in mappings:
        if key[2:] in allowed:
            settings_rows.append(dict(position=position, setting=key[2:], source_default=defaults.get(key, ""),
                                      checked_in_value=nodes[int(position)-1].text.strip(),
                                      evidence=f"{settings_path}:Configure; Info/settings.xml positional entry {position}",
                                      caveat="Raw config input; getter clamps/logic and callers may change its effect; live deployment unverified"))

    regions = ET.parse(ROOT / "Data/System/XML/Regions.xml").getroot()
    facets = regions.findall("Facet")
    region_rows = []
    for facet in facets:
        for region in facet.iter("region"):
            region_rows.append(dict(facet=facet.get("name", ""), name=region.get("name", ""),
                                    type=region.get("type", "BaseRegion"), priority=region.get("priority", "default"),
                                    rectangles=len(region.findall("rect")), spawn_objects=len(region.findall(".//object")),
                                    source="Data/System/XML/Regions.xml", coverage="CheckedInDefinition; current world occupancy unverified"))
    summary = {
        "method": "Lexical census of rg-visible C# under Data/Scripts and Data/System/Source, excluding bin/obj; no execution or item-by-item balance certification",
        "source_baseline": subprocess.run(
            ["git", "log", "-1", "--format=%h", "--", *ROOTS, "Info/settings.xml", "Data/System/XML/Regions.xml"],
            cwd=ROOT, capture_output=True, text=True, check=True,
        ).stdout.strip(),
        "source_files": len(sources),
        "source_lines": sum(s["lines"] for s in sources),
        "files_by_root": dict(sorted(Counter(s["root"] for s in sources).items())),
        "families": len(family_rows), "skills": len(skill_rows),
        "region_facets": len(facets), "region_definitions": len(region_rows),
        "region_spawn_objects": sum(int(r["spawn_objects"]) for r in region_rows),
        "settings_exported": len(settings_rows),
        "exclusions": ["Generated bin/obj", "Live save contents and deployed state", "Private settings not whitelisted", "Client assets", "Non-C# code outside the stated source roots"],
    }
    tables = {
        "source-inventory.csv": sources,
        "source-families.csv": family_rows,
        "skills.csv": skill_rows,
        "gameplay-settings.csv": settings_rows,
        "regions.csv": region_rows,
    }
    files = {name: csv_text(rows, list(rows[0])) for name, rows in tables.items()}
    files["inventory-summary.json"] = json.dumps(summary, indent=2) + "\n"
    return files, summary


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true", help="Compare outputs without writing")
    args = parser.parse_args()
    files, summary = generate()
    if args.check:
        stale = [name for name, text in files.items() if not (OUT / name).exists() or (OUT / name).read_text(encoding="utf-8") != text]
        if stale:
            raise SystemExit("Stale inventory outputs: " + ", ".join(stale))
        print("PASS: all six inventory outputs reproduce exactly")
    else:
        OUT.mkdir(parents=True, exist_ok=True)
        for name, text in files.items():
            (OUT / name).write_text(text, encoding="utf-8", newline="\n")
        print(json.dumps(summary, indent=2))


if __name__ == "__main__":
    main()
