"""Publish the three dated review records as navigable guide chapters.

Keeps the review evidence and the guide synchronized without manually maintaining
two copies. The dated source reports remain the review provenance records.
"""

import argparse
import os
from pathlib import Path
import re
from urllib.parse import quote, unquote, urlsplit

ROOT = Path(__file__).resolve().parents[3]
GUIDE = ROOT / "docs/game-balance"
REVIEWS = ROOT / "docs/codebase-audit/outputs/subagent-findings"
CHAPTERS = (
    ("2026-09-27-gameplay-progression.md", "character-and-progression.md", "Character and progression"),
    ("2026-09-27-gameplay-combat.md", "combat-and-builds.md", "Combat and builds"),
    ("2026-09-27-gameplay-economy-world.md", "adventure-and-economy.md", "Adventure and economy"),
)


def chapter(source, target, title):
    text = source.read_text(encoding="utf-8").split("\n", 1)[1].lstrip()
    text = re.split(r"(?m)^## Verification (?:record|and provenance|and reproducibility record)\s*$", text)[0].rstrip()
    text = text.replace("Source baseline supplied by parent:", "Source baseline:")
    text = text.replace("The user's design direction", "The design direction you chose")
    text = text.replace("The parent configuration review mapped", "The configuration review mapped")
    text = text.replace("The parent review consolidates this material; this report does not advance the historical audit phase controller.", "This is a gameplay study separate from the historical audit phase controller.")

    def rebase(match):
        label, url = match.groups()
        parsed = urlsplit(url)
        if parsed.scheme or parsed.netloc or not parsed.path:
            return match.group(0)
        resolved = (source.parent / unquote(parsed.path)).resolve()
        relative = os.path.relpath(resolved, target.parent).replace("\\", "/")
        return "[" + label + "](" + quote(relative, safe="/-._~") + ("#" + parsed.fragment if parsed.fragment else "") + ")"

    text = re.sub(r"\[([^\]\n]+)\]\(([^)\n]+)\)", rebase, text)
    nav = "[Start here](README.md) · [Systems at a glance](systems-at-a-glance.md) · [Connections](connections-and-balance.md) · [Coverage](coverage-and-evidence.md)"
    provenance = "../codebase-audit/outputs/subagent-findings/" + source.name
    return f"# {title}\n\n{nav}\n\n{text}\n\n## Review record\n\nPublished from the [dated source review]({provenance}). The [verification record](VERIFICATION.md) applies to the complete documentation package. Regenerate this chapter with `python docs/game-balance/tools/publish_chapters.py` after changing its review record.\n"


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true")
    args = parser.parse_args()
    stale = []
    for name, output, title in CHAPTERS:
        target = GUIDE / output
        text = chapter(REVIEWS / name, target, title)
        if args.check:
            if not target.exists() or target.read_text(encoding="utf-8") != text:
                stale.append(output)
        else:
            target.write_text(text, encoding="utf-8", newline="\n")
    if stale:
        raise SystemExit("Stale guide chapters: " + ", ".join(stale))
    print("PASS: three chapters reproduce exactly" if args.check else "Published three guide chapters")


if __name__ == "__main__":
    main()
