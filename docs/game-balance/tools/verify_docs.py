"""Check this documentation package's local links, source ranges and indexes.

This does not compile or execute game code, and does not certify the meaning of
every cited line. Semantic review and live measurements remain separate.
"""

import csv
import json
from pathlib import Path
import re
from urllib.parse import unquote, urlsplit

ROOT = Path(__file__).resolve().parents[3]
GUIDE = ROOT / "docs/game-balance"
REVIEW = ROOT / "docs/codebase-audit/outputs/subagent-findings"
reports = sorted(REVIEW.glob("2026-09-27-gameplay-*.md"))
documents = sorted(GUIDE.glob("*.md")) + reports
errors = []
links = 0
ranges = 0
qualified = set()
line_counts = {}


def line_count(path):
    if path not in line_counts:
        line_counts[path] = len(path.read_text(encoding="utf-8-sig", errors="replace").splitlines())
    return line_counts[path]


def check_range(path, start, end, context):
    global ranges
    if not path.is_file():
        errors.append(f"{context}: missing source {path}")
        return
    if not (1 <= int(start) <= int(end) <= line_count(path)):
        errors.append(f"{context}: invalid lines {start}-{end} for {path}")
    ranges += 1


def slug(heading):
    heading = re.sub(r"\[([^\]]+)\]\([^)]+\)", r"\1", heading)
    return re.sub(r"[^\w\- ]", "", heading.lower()).replace(" ", "-")


for document in documents:
    raw = document.read_bytes()
    text = raw.decode("utf-8-sig")
    relative = document.relative_to(ROOT).as_posix()
    # Git can materialize CRLF in this checkout; whitespace checks must accept
    # either newline convention without asking readers to rewrite their files.
    for index, line in enumerate(text.splitlines(), 1):
        if line.rstrip() != line:
            errors.append(f"{relative}:{index}: trailing whitespace")
    prose = re.sub(r"```.*?```", "", text, flags=re.S)
    for match in re.finditer(r"\[[^\]\n]+\]\(([^)\n]+)\)", prose):
        target = match.group(1).strip("<>")
        url = urlsplit(target)
        if url.scheme or url.netloc:
            continue
        path = (document.parent / unquote(url.path)).resolve() if url.path else document
        links += 1
        if not path.exists():
            errors.append(f"{relative}: missing link {target}")
            continue
        anchor = unquote(url.fragment)
        line_anchor = re.fullmatch(r"L(\d+)(?:-L(\d+))?", anchor)
        if line_anchor:
            check_range(path, line_anchor[1], line_anchor[2] or line_anchor[1], relative)
        elif anchor and path.suffix == ".md":
            target_text = path.read_text(encoding="utf-8-sig")
            headings = set(slug(h) for h in re.findall(r"(?m)^#{1,6} (.+)$", target_text))
            explicit = set(re.findall(r'\bid=["\']([^"\']+)["\']', target_text))
            if anchor not in headings | explicit:
                errors.append(f"{relative}: missing Markdown anchor {target}")
    # Explicitly rooted backtick source citations, including comma-separated
    # ranges. Short-name continuations are reviewed in their domain reports.
    for match in re.finditer(r"`((?:Data/|Info/|Spawns/)[^`\n]+?\.(?:cs|xml|cfg))(?::([\d, –-]+))?`", prose):
        source, spans = match.groups()
        if "*" in source:
            continue
        path = ROOT / source
        qualified.add(source)
        if not path.exists():
            errors.append(f"{relative}: missing cited path {source}")
        elif spans:
            for span in spans.split(","):
                parts = re.fullmatch(r"\s*(\d+)(?:[–-](\d+))?\s*", span)
                if parts:
                    check_range(path, parts[1], parts[2] or parts[1], relative)


def read_csv(name):
    with (GUIDE / "data" / name).open(encoding="utf-8", newline="") as handle:
        return list(csv.DictReader(handle))


inventory = read_csv("source-inventory.csv")
summary = json.loads((GUIDE / "data/inventory-summary.json").read_text())
if len(inventory) != summary["source_files"] or len({r["path"] for r in inventory}) != len(inventory):
    errors.append("Source count or unique paths do not reconcile")
if sum(int(r["lines"]) for r in inventory) != summary["source_lines"]:
    errors.append("Source line total does not reconcile")
for name, key in (("skills.csv", "skills"), ("regions.csv", "region_definitions"), ("gameplay-settings.csv", "settings_exported"), ("source-families.csv", "families")):
    if len(read_csv(name)) != summary[key]:
        errors.append(f"{name}: row count does not reconcile")

spells = json.loads((REVIEW / "2026-09-27-gameplay-combat-spells.json").read_text(encoding="utf-8"))
if len(spells["entries"]) != spells["registrationCount"]:
    errors.append("Spell registration count does not reconcile")
for entry in spells["entries"]:
    check_range(ROOT / entry["initializerPath"], entry["initializerLine"], entry["initializerLine"], "Spell index")
    if entry["acceptedByRegistry"] != (0 <= entry["spellId"] < 700):
        errors.append(f"Spell registry flag incorrect for {entry['spellId']}")
    if len(entry["definitions"]) != 1:
        errors.append(f"Unresolved/ambiguous definition for {entry['registeredType']}")
    for definition in entry["definitions"]:
        check_range(ROOT / definition["path"], definition["line"], definition["line"], "Spell index")

progression = json.loads((REVIEW / "2026-09-27-gameplay-progression.json").read_text(encoding="utf-8"))
for card in progression["cards"]:
    for ref in card["evidence"]:
        path, separator, anchor = ref.partition("#")
        path = unquote(path)
        if separator:
            check_range(ROOT / path, anchor[1:], anchor[1:], card["id"])
        elif not (ROOT / path).exists():
            errors.append(f"{card['id']}: missing source {path}")

if errors:
    print("\n".join(errors))
    raise SystemExit(f"FAILED: {len(errors)} documentation checks")

print(json.dumps({"result": "PASS", "markdown_documents": len(documents),
                  "local_links": links, "source_line_ranges": ranges,
                  "qualified_backtick_source_paths": len(qualified),
                  "inventory_files": len(inventory), "spell_registrations": len(spells["entries"]),
                  "note": "Path/range/index consistency; no game execution or live balance validation"}, indent=2))
