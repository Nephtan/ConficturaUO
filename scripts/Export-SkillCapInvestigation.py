"""Join compiled mutation inventories to reviewed families and source locations.

Run Test-SkillCapInvestigation.ps1 first. This script never loads world saves.
The C# lexer is only a source locator; mutation discovery comes from compiled IL.
Unmapped callers, ambiguous type locations, and unused review rules fail export.
"""

import argparse
import csv
import hashlib
import json
from pathlib import Path
import re
import subprocess
import urllib.parse
import xml.etree.ElementTree as ET


def rg_files(root, *args):
    result = subprocess.run(["rg", "--files", *args], cwd=root,
                            check=True, capture_output=True, text=True)
    return sorted(set(p.replace("\\", "/") for p in result.stdout.splitlines()))


def write_csv(path, rows, fields):
    with path.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=fields, lineterminator="\n")
        writer.writeheader()
        writer.writerows(rows)


def read_csv(path):
    with path.open(encoding="utf-8-sig", newline="") as stream:
        return list(csv.DictReader(stream))


def strip_literals(text):
    # Preserve offsets/newlines for exact source declaration locations.
    pattern = r'//[^\n]*|/\*[\s\S]*?\*/|@"(?:""|[^"])*"|"(?:\\.|[^"\\])*"|\x27(?:\\.|[^\x27\\])*\x27'
    return re.sub(pattern, lambda m: re.sub(r"[^\n]", " ", m[0]), text)


def definitions(text, path):
    clean = strip_literals(text)
    token = re.compile(r"\b(namespace|class|struct|interface|enum)\s+([\w.]+)|[{}]")
    stack = []
    pending = None
    result = {}
    for match in token.finditer(clean):
        if match.group(1):
            pending = (match.group(1), match.group(2), match.start())
        elif match[0] == "{":
            stack.append(pending)
            if pending and pending[0] != "namespace":
                namespaces = [e[1] for e in stack if e and e[0] == "namespace"]
                types = [e[1] for e in stack if e and e[0] != "namespace"]
                fullname = ".".join(namespaces) + "." + "+".join(types)
                result[fullname] = {"path": path, "start": pending[2], "end": len(text),
                                    "line": text.count("\n", 0, pending[2]) + 1}
            pending = None
        elif stack:
            entry = stack.pop()
            if entry and entry[0] != "namespace":
                for definition in result.values():
                    if definition["start"] == entry[2]:
                        definition["end"] = match.end()
    return clean, result


def project_entries(root, relative):
    project = root / relative
    tree = ET.parse(project)
    entries = set()
    for item in tree.iter():
        if item.tag.split("}")[-1] != "Compile" or "Include" not in item.attrib:
            continue
        target = project.parent / urllib.parse.unquote(item.attrib["Include"]).replace("\\", "/")
        entries.add(target.resolve().relative_to(root).as_posix())
    return entries


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("run_directory", type=Path)
    parser.add_argument("--repository", type=Path, default=Path(__file__).resolve().parent.parent)
    parser.add_argument("--output", type=Path, default=Path("docs/skill-cap-investigation"))
    args = parser.parse_args()
    root = args.repository.resolve()
    output = (root / args.output).resolve()
    output.mkdir(parents=True, exist_ok=True)
    run = args.run_directory.resolve()
    rules = json.loads((output / "review-rules.json").read_text(encoding="utf-8"))
    used_rules = set()
    files = rg_files(root, "Data/Scripts", "Data/System/Source", "-g", "*.cs",
                     "-g", "!**/bin/**", "-g", "!**/obj/**")
    ide = project_entries(root, "Data/Scripts/Scripts.csproj")
    core = project_entries(root, "Data/System/Source/Server.csproj")
    locations = {}
    sources = {}
    manifest = []
    for path in files:
        raw = (root / path).read_bytes()
        text = raw.decode("utf-8-sig", errors="replace")
        clean, found = definitions(text, path)
        sources[path] = (text, clean)
        for name, location in found.items():
            locations.setdefault(name, []).append(location)
        manifest.append({"Path": path, "SHA256": hashlib.sha256(raw).hexdigest(),
                         "Bytes": len(raw), "RuntimeScript": path.startswith("Data/Scripts/"),
                         "ProjectIncluded": path in (ide if path.startswith("Data/Scripts/") else core)})

    def locate(caller):
        typename, signature = caller.split("::", 1)
        typename = re.sub(r"`\d+", "", typename)
        # Compiler-generated nested closures map to the source enclosing type.
        while typename not in locations and "+" in typename:
            typename = typename.rsplit("+", 1)[0]
        candidates = locations.get(typename, [])
        if len(candidates) != 1:
            raise ValueError(f"Ambiguous/missing source type: {caller}: {candidates}")
        location = candidates[0]
        text, clean = sources[location["path"]]
        name = signature.split("(", 1)[0].split()[-1]
        if name in (".ctor", ".cctor"):
            name = typename.split("+")[-1].split(".")[-1]
        elif name.startswith(("set_", "get_")):
            name = name[4:]
            if name == "Item":
                name = "this"
        body = clean[location["start"]:location["end"]]
        declaration = r"\b(?:public|protected|internal|private)\s+[^;{}=]*?\b" + re.escape(name) + r"\s*(?:\(|\{|\[)"
        matches = list(re.finditer(declaration, body))
        lines = [text.count("\n", 0, location["start"] + match.start()) + 1 for match in matches]
        return {"Source": location["path"], "DeclarationLines": ";".join(map(str, lines or [location["line"]])),
                "Locator": "method/property declaration" if lines else "enclosing type declaration"}

    def review(caller):
        matches = [rule for rule in rules if re.search(rule["pattern"], caller)]
        if len(matches) != 1:
            raise ValueError(f"Expected exactly one reviewed family for {caller}: {[r['id'] for r in matches]}")
        rule = matches[0]
        used_rules.add(rule["id"])
        return {"ReviewId": rule["id"], "Classification": rule["classification"],
                "TriggerAndTarget": rule["trigger"], "LocksAndCaps": rule["guards"],
                "MaximumExcess": rule["maximum"], "Reason": rule["reason"]}

    counts = {}
    for source, destination in (("compiled-mutation-calls.csv", "mutation-path-register.csv"),
                                ("reflection-writes.csv", "reflection-write-register.csv")):
        rows = read_csv(run / source)
        joined = [dict(row, **locate(row["Caller"]), **review(row["Caller"])) for row in rows]
        write_csv(output / destination, joined, list(joined[0]))
        counts[destination] = len(joined)
    unused = [rule["id"] for rule in rules if rule["id"] not in used_rules]
    if unused:
        raise ValueError(f"Unused review rules: {unused}")

    npc = read_csv(run / "npc-skill-setter-calls.csv")
    npc_joined = [dict(row, **locate(row["Caller"]), ReviewId="NPC", Classification="cap-safe",
                       Reason="BaseCreature.SetSkill receiver cannot be PlayerMobile; NPC individual/total caps are separate.") for row in npc]
    write_csv(output / "npc-skill-setter-register.csv", npc_joined, list(npc_joined[0]))
    regions = read_csv(run / "region-entry-calls.csv")
    region_joined = [dict(row, **locate(row["EffectiveHandler"])) for row in regions]
    write_csv(output / "region-entry-register.csv", region_joined, list(region_joined[0]))
    write_csv(output / "source-coverage.csv", manifest, list(manifest[0]))
    unlisted = [row for row in manifest if row["RuntimeScript"] and not row["ProjectIncluded"]]
    write_csv(output / "runtime-scripts-omitted-from-project.csv", unlisted, list(manifest[0]))

    # These are relevant text inputs only, never serialized world/account data or logs.
    data_files = rg_files(root, "--hidden", "--no-ignore", "-g", "*.xml", "-g", "*.npc", "-g", "*.cfg",
                         "-g", "*.json", "-g", "*.ini", "-g", "!output/**", "-g", "!Saves*/**",
                         "-g", "!Backups/**", "-g", "!Logs/**", "-g", "!**/bin/**", "-g", "!**/obj/**",
                         "-g", "!**/.git/**", "-g", "!**/.vs/**", "-g", "!docs/**", "-g", "!.codex/**")
    data_files = [p for p in data_files if p.startswith(("Data/Scripts/", "Data/System/", "Spawns/", "Info/", "DioXMLSpawn/")) or "/" not in p]
    pattern = re.compile(r"(?<![A-Za-z])SKILL\s*,|skills\.[A-Za-z]+\.(?:base|cap)|skillscap|skill_start|skill_boost|skill_ether|skills\.total", re.I)
    data_manifest = []
    data_hits = []
    for path in data_files:
        raw = (root / path).read_bytes()
        hits = [(i, line.strip()) for i, line in enumerate(raw.decode("utf-8-sig", errors="replace").splitlines(), 1) if pattern.search(line)]
        data_manifest.append({"Path": path, "SHA256": hashlib.sha256(raw).hexdigest(), "MutationLiteralMatches": len(hits)})
        for i, line in hits:
            comment = path == "Info/settings.xml" and line.startswith("// If true, then players have a chance for some slight magical properties")
            data_hits.append({"Path": path, "Line": i, "Text": line,
                              "Classification": "cap-safe" if comment else "unresolved",
                              "ReviewNote": "English comment contains 'skill, material'; no executable property action." if comment else "New text match requires manual review."})
    write_csv(output / "data-coverage.csv", data_manifest, ["Path", "SHA256", "MutationLiteralMatches"])
    write_csv(output / "data-mutation-literals.csv", data_hits, ["Path", "Line", "Text", "Classification", "ReviewNote"])
    summary = {"baseline": "75bc9dfb", "runtime_script_files": sum(row["RuntimeScript"] for row in manifest),
               "core_source_files": sum(not row["RuntimeScript"] for row in manifest),
               "runtime_scripts_omitted_from_project": len(unlisted),
               "missing_project_entries": sorted((ide | core) - set(files)),
               **counts, "npc_setter_calls": len(npc), "region_types": len(regions),
               "reviewed_families": len(used_rules), "unmapped_compiled_calls": 0,
               "text_data_files_scanned": len(data_manifest), "text_data_mutation_literal_matches": len(data_hits),
               "unresolved_data_matches": sum(hit["Classification"] == "unresolved" for hit in data_hits),
               "data_pattern": pattern.pattern,
               "limits": ["No current world saves or incident logs were supplied.",
                          "Text scans do not resolve generated strings or saved XMLSpawner instances.",
                          "IL inventory covers the compiled baseline configuration, not arbitrary future builds or external DLL internals.",
                          "DeclarationLines locate source methods; IL offsets distinguish individual calls and overload signatures.",
                          "Region verification can also run on a newly entered parent; a false handler row is not a whole-transition exemption."]}
    (output / "coverage-summary.json").write_text(json.dumps(summary, indent=2) + "\n", encoding="utf-8", newline="\n")
    print(json.dumps(summary, indent=2))


if __name__ == "__main__":
    main()
