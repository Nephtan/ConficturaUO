#!/usr/bin/env python3
"""Verify CCWM continuity, generated indexes, and live-state integrity."""

from __future__ import annotations

import argparse
import datetime as dt
import re
import subprocess
import sys
from dataclasses import dataclass
from pathlib import Path
from typing import Iterable, List

from ccwm_common import (
    CCWM_DIR,
    ccwm_path,
    count_jsonl,
    manifest_counts,
    marker_rows,
    parse_runs,
    read_text,
    repo_root,
    yaml_scalar,
    yaml_section_keys,
)


@dataclass
class CheckResult:
    severity: str
    name: str
    detail: str


class Reporter:
    def __init__(self) -> None:
        self.results: List[CheckResult] = []

    def ok(self, name: str, detail: str) -> None:
        self.results.append(CheckResult("OK", name, detail))

    def warn(self, name: str, detail: str) -> None:
        self.results.append(CheckResult("WARN", name, detail))

    def fail(self, name: str, detail: str) -> None:
        self.results.append(CheckResult("FAIL", name, detail))

    def print(self) -> None:
        for result in self.results:
            print("{0} {1}: {2}".format(result.severity, result.name, result.detail))

    def exit_code(self, strict: bool) -> int:
        has_fail = any(result.severity == "FAIL" for result in self.results)
        has_warn = any(result.severity == "WARN" for result in self.results)
        return 1 if has_fail or (strict and has_warn) else 0


def check_run_alignment(reporter: Reporter) -> None:
    log_text = read_text(ccwm_path("Confictura_Codex_World_Map.md"))
    state_text = read_text(ccwm_path("Simulation_State.yaml"))
    runs = parse_runs(log_text)

    if not runs:
        reporter.fail("run_alignment", "no run headings found in map log")
        return

    state_run_text = yaml_scalar(state_text, ("schema", "run_number"))
    if state_run_text is None or not state_run_text.isdigit():
        reporter.fail("run_alignment", "schema.run_number missing or not numeric")
        return

    latest_log_run = runs[-1].number
    state_run = int(state_run_text)

    if latest_log_run == len(runs) == state_run:
        reporter.ok(
            "run_alignment",
            "latest_log_run={0}, log_run_count={1}, state_run={2}".format(
                latest_log_run, len(runs), state_run
            ),
        )
    else:
        reporter.fail(
            "run_alignment",
            "latest_log_run={0}, log_run_count={1}, state_run={2}".format(
                latest_log_run, len(runs), state_run
            ),
        )


def check_state_sections(reporter: Reporter) -> None:
    state_text = read_text(ccwm_path("Simulation_State.yaml"))
    keys = set(yaml_section_keys(state_text))
    required = {
        "schema",
        "client_side_runtime_policy",
        "account",
        "identity",
        "lifecycle",
        "location",
        "perception",
    }
    missing = sorted(required - keys)

    if missing:
        reporter.fail("state_sections", "missing top-level sections: {0}".format(", ".join(missing)))
    else:
        reporter.ok("state_sections", "required active-state sections present")

    if "next_client_visible_decision" in state_text:
        reporter.ok("state_next_decision", "next client-visible decision is present")
    else:
        reporter.fail("state_next_decision", "next_client_visible_decision missing")


def check_referenced_files(reporter: Reporter) -> None:
    root = repo_root()
    texts = [
        read_text(ccwm_path("Simulation_State.yaml")),
        read_text(ccwm_path("live-state", "manifest.yaml")),
        read_text(ccwm_path("AGENTS.md")),
        read_text(ccwm_path("ccwm-autoprompt.md")),
    ]
    refs = set()

    for text in texts:
        for match in re.finditer(r"docs/CCWM/[A-Za-z0-9_./\\\\-]+", text):
            ref = match.group(0).rstrip(".,;:)'\"")
            refs.add(ref.replace("\\", "/"))

    missing = []
    for ref in sorted(refs):
        path = root / ref
        if not path.exists():
            missing.append(ref)

    if missing:
        reporter.fail("referenced_files", "missing: {0}".format(", ".join(missing[:12])))
    else:
        reporter.ok("referenced_files", "{0} docs/CCWM references exist".format(len(refs)))


def check_manifest_jsonl(reporter: Reporter) -> None:
    manifest_text = read_text(ccwm_path("live-state", "manifest.yaml"))
    counts = manifest_counts(manifest_text)
    paths = {
        "spawners": ccwm_path("live-state", "spawners.jsonl"),
        "mobiles": ccwm_path("live-state", "mobiles.jsonl"),
        "interactable_world_items": ccwm_path("live-state", "items.jsonl"),
    }
    actual = {}
    errors = []

    for key, path in paths.items():
        if not path.exists():
            errors.append("{0} missing".format(path))
            continue

        count, error = count_jsonl(path, validate_json=True)
        actual[key] = count
        if error:
            errors.append(error)

    if errors:
        reporter.fail("jsonl_validity", "; ".join(errors[:5]))
    else:
        reporter.ok("jsonl_validity", "live-state JSONL files parse")

    mismatches = []
    for key, expected in counts.items():
        if key in actual and actual[key] != expected:
            mismatches.append("{0}: manifest={1}, actual={2}".format(key, expected, actual[key]))

    if mismatches:
        reporter.fail("manifest_counts", "; ".join(mismatches))
    else:
        reporter.ok("manifest_counts", "manifest counts match JSONL line counts")


def check_markers(reporter: Reporter) -> None:
    marker_dir = ccwm_path("client-worldmap-markers")
    files, rows, bad = marker_rows(marker_dir)

    if bad:
        reporter.fail("marker_shape", "{0} malformed rows, first: {1}".format(len(bad), bad[0]))
    else:
        reporter.ok("marker_shape", "{0} files, {1} rows, 0 malformed rows".format(files, rows))

    marker_stems = {path.stem.replace("-Common", "") for path in marker_dir.glob("*.csv")}
    manifest_text = read_text(ccwm_path("live-state", "manifest.yaml"))

    if "SavagedEmpire" in manifest_text and "SavegedEmpire" in marker_stems:
        reporter.ok(
            "map_aliases",
            "marker spelling SavegedEmpire is explicitly mapped to server map SavagedEmpire",
        )
    else:
        reporter.warn(
            "map_aliases",
            "expected SavegedEmpire marker alias and SavagedEmpire server map were not both found",
        )


def parse_generated_utc(value: str) -> dt.datetime | None:
    try:
        if value.endswith("Z"):
            return dt.datetime.fromisoformat(value[:-1] + "+00:00")
        return dt.datetime.fromisoformat(value)
    except ValueError:
        return None


def check_snapshot_freshness(reporter: Reporter, stale_days: int) -> None:
    manifest_text = read_text(ccwm_path("live-state", "manifest.yaml"))
    value = yaml_scalar(manifest_text, ("ccwm_live_state_snapshot", "generated_utc"))

    if not value:
        reporter.warn("snapshot_freshness", "generated_utc missing")
        return

    generated = parse_generated_utc(value)
    if generated is None:
        reporter.warn("snapshot_freshness", "generated_utc is not ISO-8601: {0}".format(value))
        return

    age = dt.datetime.now(dt.timezone.utc) - generated.astimezone(dt.timezone.utc)
    if age.days > stale_days:
        reporter.warn(
            "snapshot_freshness",
            "snapshot age is {0} days; refresh manually only when needed".format(age.days),
        )
    else:
        reporter.ok("snapshot_freshness", "snapshot age is {0} days".format(age.days))


def check_git_diff(reporter: Reporter) -> None:
    result = subprocess.run(
        ["git", "diff", "--check", "--", "docs/CCWM"],
        cwd=str(repo_root()),
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        text=True,
        check=False,
    )
    output = result.stdout.strip()

    if result.returncode != 0:
        reporter.fail("git_diff_check", output or "git diff --check failed")
    elif output:
        reporter.warn("git_diff_check", output.replace("\n", " | "))
    else:
        reporter.ok("git_diff_check", "no whitespace errors in docs/CCWM diff")


def check_exporter_placement(reporter: Reporter) -> None:
    root = repo_root()
    bad_paths: List[str] = []

    for base in (root / "Data" / "Scripts", root / "Data" / "System"):
        if not base.exists():
            continue

        for path in base.rglob("*"):
            if path.is_file() and "CCWMLiveStateExporter" in path.name:
                bad_paths.append(str(path.relative_to(root)))

        for path in base.rglob("*.cs"):
            try:
                text = read_text(path)
            except UnicodeDecodeError:
                continue

            if "CCWMLiveStateExporter" in text:
                bad_paths.append(str(path.relative_to(root)))

    if bad_paths:
        reporter.fail("exporter_placement", "exporter appears in server script/core paths: {0}".format(", ".join(sorted(set(bad_paths)))))
    else:
        reporter.ok("exporter_placement", "no CCWMLiveStateExporter source under Data/Scripts or Data/System")

    scripts_csproj = root / "Data" / "Scripts" / "Scripts.csproj"
    text = read_text(scripts_csproj)
    if "CCWMLiveStateExporter" in text:
        reporter.fail("scripts_csproj", "Data/Scripts/Scripts.csproj includes CCWMLiveStateExporter")
    else:
        reporter.ok("scripts_csproj", "Scripts.csproj does not include the CCWM exporter")

    exporter_dir = ccwm_path("tools", "live-state-exporter")
    required = [
        ccwm_path("tools", "CCWM.Tools.sln"),
        exporter_dir / "CCWMLiveStateExporter.csproj",
        exporter_dir / "Program.cs",
        exporter_dir / "README.md",
    ]
    missing = [str(path.relative_to(root)) for path in required if not path.exists()]
    if missing:
        reporter.fail("exporter_tool", "missing standalone exporter files: {0}".format(", ".join(missing)))
    else:
        reporter.ok("exporter_tool", "standalone exporter files exist under docs/CCWM/tools")


def check_generated_indexes(reporter: Reporter) -> None:
    run_index = ccwm_path("generated", "Run_Index.md")
    knowledge_index = ccwm_path("generated", "Knowledge_Index.yaml")

    if not run_index.exists() or not knowledge_index.exists():
        reporter.fail("generated_indexes", "Run_Index.md and Knowledge_Index.yaml must be generated")
        return

    log_text = read_text(ccwm_path("Confictura_Codex_World_Map.md"))
    latest_run = parse_runs(log_text)[-1].number
    run_index_text = read_text(run_index)
    knowledge_text = read_text(knowledge_index)

    if "| {0} |".format(latest_run) not in run_index_text:
        reporter.fail("generated_run_index", "latest run {0} missing from Run_Index.md".format(latest_run))
    else:
        reporter.ok("generated_run_index", "latest run {0} appears in Run_Index.md".format(latest_run))

    if "manual_capture_required:" in knowledge_text and "thief_note_assignment_line" in knowledge_text:
        reporter.ok("manual_capture", "thief-note manual-capture blocker is indexed")
    else:
        reporter.fail("manual_capture", "manual_capture_required thief-note blocker missing from knowledge index")


def run_checks(args: argparse.Namespace) -> Reporter:
    reporter = Reporter()

    check_run_alignment(reporter)
    check_state_sections(reporter)
    check_referenced_files(reporter)
    check_manifest_jsonl(reporter)
    check_markers(reporter)
    check_snapshot_freshness(reporter, args.stale_days)
    check_exporter_placement(reporter)
    check_generated_indexes(reporter)

    if not args.skip_git_diff_check:
        check_git_diff(reporter)

    return reporter


def main(argv: Iterable[str]) -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--strict", action="store_true", help="treat warnings as failures")
    parser.add_argument("--stale-days", type=int, default=14, help="warn when live-state snapshot is older than this many days")
    parser.add_argument("--skip-git-diff-check", action="store_true", help="skip git diff --check")
    args = parser.parse_args(list(argv))

    reporter = run_checks(args)
    reporter.print()
    return reporter.exit_code(args.strict)


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
