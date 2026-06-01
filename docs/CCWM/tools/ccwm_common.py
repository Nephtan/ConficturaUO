#!/usr/bin/env python3
"""Shared helpers for CCWM maintenance tools."""

from __future__ import annotations

import csv
import json
import re
from dataclasses import dataclass
from pathlib import Path
from typing import Iterable, Iterator, List, Optional, Sequence, Tuple


CCWM_DIR = Path("docs") / "CCWM"
RUN_HEADING_RE = re.compile(r"^## Run (\d+) - (.+?)\s*$", re.MULTILINE)


@dataclass(frozen=True)
class RunEntry:
    number: int
    title: str
    line: int
    body: str


def repo_root() -> Path:
    return Path(__file__).resolve().parents[3]


def ccwm_path(*parts: str) -> Path:
    return repo_root().joinpath(CCWM_DIR, *parts)


def read_text(path: Path) -> str:
    try:
        return path.read_text(encoding="utf-8-sig")
    except UnicodeDecodeError:
        return path.read_text(encoding="utf-8")


def write_if_changed(path: Path, content: str) -> bool:
    if path.exists() and read_text(path) == content:
        return False

    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content, encoding="utf-8", newline="\n")
    return True


def parse_runs(log_text: str) -> List[RunEntry]:
    matches = list(RUN_HEADING_RE.finditer(log_text))
    entries: List[RunEntry] = []

    for index, match in enumerate(matches):
        start = match.end()
        end = matches[index + 1].start() if index + 1 < len(matches) else len(log_text)
        line = log_text.count("\n", 0, match.start()) + 1
        entries.append(
            RunEntry(
                number=int(match.group(1)),
                title=match.group(2).strip(),
                line=line,
                body=log_text[start:end].strip(),
            )
        )

    return entries


def clean_inline_value(value: str) -> str:
    value = value.strip()
    if value in ("", "null", "Null", "NULL"):
        return ""

    if len(value) >= 2 and value[0] == value[-1] and value[0] in ("'", '"'):
        return value[1:-1]

    return value


def yaml_scalar(text: str, path: Sequence[str]) -> Optional[str]:
    stack: List[str] = []

    for raw_line in text.splitlines():
        if not raw_line.strip() or raw_line.lstrip().startswith("#"):
            continue

        stripped = raw_line.lstrip()
        if stripped.startswith("- "):
            continue

        if ":" not in stripped:
            continue

        indent = len(raw_line) - len(stripped)
        level = indent // 2
        key, value = stripped.split(":", 1)
        key = key.strip()

        if not key:
            continue

        stack = stack[:level]
        stack.append(key)

        if list(path) == stack:
            return clean_inline_value(value)

    return None


def yaml_section_keys(text: str) -> List[str]:
    keys: List[str] = []

    for raw_line in text.splitlines():
        if not raw_line.strip() or raw_line.startswith(" ") or raw_line.startswith("\t"):
            continue

        if ":" in raw_line:
            key = raw_line.split(":", 1)[0].strip()
            if key:
                keys.append(key)

    return keys


def manifest_counts(text: str) -> dict[str, int]:
    result: dict[str, int] = {}
    in_counts = False

    for raw_line in text.splitlines():
        stripped = raw_line.strip()

        if stripped == "counts:":
            in_counts = True
            continue

        if in_counts:
            if raw_line and not raw_line.startswith(" "):
                break

            if ":" not in stripped:
                continue

            key, value = stripped.split(":", 1)
            value = value.strip()
            if value.isdigit():
                result[key.strip()] = int(value)

    return result


def count_jsonl(path: Path, validate_json: bool = False) -> Tuple[int, Optional[str]]:
    count = 0

    with path.open("r", encoding="utf-8-sig") as handle:
        for line_number, line in enumerate(handle, start=1):
            if not line.strip():
                continue

            count += 1

            if validate_json:
                try:
                    json.loads(line)
                except json.JSONDecodeError as exc:
                    return count, f"{path}: line {line_number}: {exc}"

    return count, None


def marker_rows(marker_dir: Path) -> Tuple[int, int, List[str]]:
    files = 0
    rows = 0
    bad: List[str] = []

    for path in sorted(marker_dir.glob("*.csv")):
        files += 1
        with path.open("r", encoding="utf-8-sig", newline="") as handle:
            reader = csv.reader(handle)
            for row_number, row in enumerate(reader, start=1):
                if not row or all(not cell.strip() for cell in row):
                    continue

                rows += 1
                if len(row) != 7:
                    bad.append(f"{path}: line {row_number}: expected 7 columns, got {len(row)}")

    return files, rows, bad


def sentence_from(text: str, keywords: Iterable[str]) -> str:
    lowered_keywords = [keyword.lower() for keyword in keywords]
    candidates = re.split(r"(?<=[.!?])\s+", " ".join(text.split()))

    for sentence in candidates:
        lowered = sentence.lower()
        if any(keyword in lowered for keyword in lowered_keywords):
            return shorten(sentence, 180)

    return ""


def shorten(text: str, limit: int = 120) -> str:
    compact = " ".join(text.replace("|", "\\|").split())

    if len(compact) <= limit:
        return compact

    return compact[: limit - 3].rstrip() + "..."


def first_narrative_sentence(body: str) -> str:
    for raw_line in body.splitlines():
        line = raw_line.strip()
        if not line or line.startswith("- ") or line.startswith("Simulation Log"):
            continue

        if line.startswith("I ") or line.startswith("Mira ") or line.startswith("The "):
            return shorten(line, 180)

    return shorten(sentence_from(body, ["I ", "Mira", "The "]), 180)


def next_decision(body: str) -> str:
    for raw_line in reversed(body.splitlines()):
        line = raw_line.strip("- ").strip()
        lowered = line.lower()
        if "next client-visible decision" in lowered or "next logical action" in lowered:
            if ":" in line:
                return shorten(line.split(":", 1)[1].strip(), 180)
            return shorten(line, 180)

    return ""


def point_summary(body: str) -> str:
    matches = re.findall(r"Point3D\([^)]+\)", body)
    if matches:
        return matches[-1]

    return ""


def quoted(value: str) -> str:
    return json.dumps(value, ensure_ascii=True)
