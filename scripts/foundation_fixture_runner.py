"""Bounded, no-save/no-listener component preflight for foundation repairs."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import shutil
import subprocess
import sys
import tempfile
import time
import uuid
from datetime import datetime, timezone


def sha(path):
    return hashlib.sha256(Path(path).read_bytes()).hexdigest()


def write(path, value):
    Path(path).write_text(json.dumps(value, indent=2) + "\n", encoding="utf-8")


def verify(root, records):
    for name, digest in records.items():
        path = root / name
        if not path.is_file() or sha(path) != digest:
            raise RuntimeError("stale snapshot: " + str(path))


def run(root, name, args, marker=None, timeout=180):
    original = name
    attempt = 1
    while (root / (name + ".log")).exists():
        attempt += 1
        name = original + "-attempt-" + str(attempt)
    log = root / (name + ".log")
    started = datetime.now(timezone.utc).isoformat()
    env = os.environ.copy()
    for key in ("TEMP", "TMP", "USERPROFILE", "APPDATA", "LOCALAPPDATA"):
        directory = root / "environment" / key
        directory.mkdir(parents=True, exist_ok=True)
        env[key] = str(directory)
    with log.open("w", encoding="utf-8") as output:
        process = subprocess.Popen([str(a) for a in args], cwd=root, env=env,
                                   stdout=output, stderr=subprocess.STDOUT,
                                   creationflags=subprocess.CREATE_NO_WINDOW)
        timed_out = False
        try:
            code = process.wait(timeout=timeout)
        except subprocess.TimeoutExpired:
            timed_out = True
            process.kill()
            code = process.wait()
    content = log.read_text(encoding="utf-8", errors="replace")
    reason = "timeout" if timed_out else "exit code" if code else "missing success marker" if marker and marker not in content else None
    result = dict(stage=name, started=started, command=[str(a) for a in args], pid=process.pid,
                  exit_code=code, forced_termination=timed_out, process_exited=True,
                  log=str(log), marker=marker, passed=reason is None, reason=reason)
    write(root / (name + ".result.json"), result)
    print(json.dumps(result), flush=True)
    if reason:
        raise RuntimeError(name + ": " + reason + "; see " + str(log))
    return result


def tooling():
    vswhere = Path(os.environ["ProgramFiles(x86)"]) / "Microsoft Visual Studio/Installer/vswhere.exe"
    build = subprocess.check_output([str(vswhere), "-latest", "-products", "*", "-requires",
                                    "Microsoft.Component.MSBuild", "-find", "MSBuild/**/Bin/MSBuild.exe"], text=True).strip().splitlines()[0]
    return build, Path(os.environ["WINDIR"]) / "Microsoft.NET/Framework/v4.0.30319/csc.exe"


def prepare(repo, root):
    root.mkdir(parents=True, exist_ok=False)
    paths = []
    for source in (repo / "Data/System/Source", repo / "Data/Scripts"):
        for path in source.rglob("*"):
            if path.is_file() and not path.name.endswith(".user") and not any(x in ("bin", "obj", ".vs") for x in path.relative_to(source).parts):
                if source.name == "Source" or path.suffix == ".cs":
                    paths.append(path)
    paths += [repo / p for p in ("Data/System/CFG/Assemblies.cfg", "Data/System/CFG/body.cfg",
              "Data/Bounds.bin", "OrbServerSDK.dll", "UOArchitectInterface.dll", "zlib32.dll", "zlib64.dll")]
    records = {}
    for source in paths:
        rel = source.relative_to(repo).as_posix()
        target = root / rel
        target.parent.mkdir(parents=True, exist_ok=True)
        before = sha(source)
        shutil.copy2(source, target)
        if sha(source) != before or sha(target) != before:
            raise RuntimeError("source changed during snapshot: " + str(source))
        records[rel] = before
    # Prove the combined core/scripts snapshot existed together, not merely per-file.
    verify(repo, records)
    write(root / "inputs.json", dict(repository=str(repo), head=subprocess.check_output(
        ["git", "rev-parse", "HEAD"], cwd=repo, text=True).strip(), files=records))
    return records


def build(root, records):
    verify(root, records)
    msbuild, _ = tooling()
    run(root, "server-build", [msbuild, root / "Data/System/Source/Server.csproj", "/nologo",
        "/verbosity:minimal", "/p:Configuration=Release", "/p:Platform=x86",
        "/p:OutputPath=" + str(root) + "/", "/p:IntermediateOutputPath=" + str(root / "server-obj") + "/"])
    run(root, "runtime-compile", [root / "ConficturaServer.exe", "-compileonly", "-nocache"],
        "Scripts: Compile-only verification completed successfully.")
    verify(root, records)
    shutil.copy2(root / "Data/Data.bin", root / "Data.dll")
    write(root / "assemblies.json", {p: sha(root / p) for p in ("ConficturaServer.exe", "Data.dll", "Data/Data.bin")})


def fixtures(repo, root, names, allow_historical):
    manifest = json.loads((repo / "tests/foundation-fixtures.json").read_text())
    _, csc = tooling()
    inputs = json.loads((root / "inputs.json").read_text())["files"]
    assemblies = json.loads((root / "assemblies.json").read_text())
    selected = []
    for name in names:
        entry = manifest[name]
        if entry["classification"] != "current-regression" and not allow_historical:
            raise RuntimeError("historical characterization requires --allow-historical: " + name)
        selected.append((name, entry))
    verify(root, inputs)
    verify(root, assemblies)
    prepared = []
    for name, entry in selected:
        attempt = name + "-" + uuid.uuid4().hex[:8]
        copies = []
        for source_name in entry["sources"]:
            source = Path(source_name)
            if not source.is_absolute():
                source = repo / source
            destination = root / "fixtures" / attempt / source.name
            destination.parent.mkdir(parents=True, exist_ok=True)
            digest = sha(source)
            shutil.copy2(source, destination)
            if sha(source) != digest or sha(destination) != digest:
                raise RuntimeError("fixture changed during snapshot: " + str(source))
            copies.append(destination)
        exe = root / (attempt + ".exe")
        identity = dict(classification=entry["classification"], source_hashes={str(p.relative_to(root)): sha(p) for p in copies},
                        assembly_hashes=assemblies, entry=entry, runner_sha256=sha(__file__),
                        manifest_sha256=sha(repo / "tests/foundation-fixtures.json"))
        write(root / (attempt + ".identity.json"), identity)
        run(root, name + "-compile", [csc, "/nologo", "/codepage:65001", "/target:exe", "/platform:x86",
            "/main:" + entry["main"], "/out:" + str(exe), "/reference:" + str(root / "ConficturaServer.exe"),
            "/reference:" + str(root / "Data.dll")] + copies, timeout=60)
        prepared.append((name, entry, exe, identity))
    # Every selected fixture compiles against the exact candidate before any executes.
    for name, entry, exe, identity in prepared:
        verify(root, inputs)
        verify(root, assemblies)
        verify(root, identity["source_hashes"])
        run(root, name + "-run", [exe, "D:/ConficturaUOTestTools/shard/Data/Files"] + entry.get("args", []),
            entry["marker"], timeout=entry.get("timeout", 120))


def selftest(root):
    """Exercise failures without a world/client, including compiler/reference failures."""
    root.mkdir(parents=True, exist_ok=False)
    _, csc = tooling()
    source = root / "Mismatch.cs"
    cases = []
    for name, text, references in (
        ("api-mismatch", "class T { static void Main() { System.String.DoesNotExist(); } }", []),
        ("wrong-reference", "class T { static void Main() {} }", ["/reference:" + str(root / "missing.dll")])):
        source.write_text(text)
        try:
            run(root, name, [csc, "/nologo", "/out:" + str(root / "probe.exe")] + references + [source])
        except RuntimeError:
            cases.append(name)
        else:
            raise RuntimeError("selftest failed to reject " + name)
    source.write_text("before")
    records = {source.name: sha(source)}
    source.write_text("after")
    try:
        verify(root, records)
    except RuntimeError:
        cases.append("stale-snapshot")
    for name, code, marker, timeout in (("missing-marker", "print('other')", "PASS", 10),
                                       ("timeout", "import time; time.sleep(10)", None, 1)):
        try:
            run(root, name, [sys.executable, "-c", code], marker, timeout)
        except RuntimeError:
            cases.append(name)
        else:
            raise RuntimeError("selftest failed to reject " + name)
    assert len(cases) == 5
    write(root / "selftest.json", dict(passed=cases, no_world_or_client=True))
    print("PASS preflight failure probes: " + ", ".join(cases))


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--snapshot", type=Path, required=True)
    parser.add_argument("--prepare", action="store_true")
    parser.add_argument("--selftest", action="store_true")
    parser.add_argument("--fixtures", nargs="*", default=[])
    parser.add_argument("--allow-historical", action="store_true")
    options = parser.parse_args()
    repository = Path(__file__).resolve().parents[1]
    try:
        if options.selftest:
            selftest(options.snapshot)
        else:
            if options.prepare:
                build(options.snapshot, prepare(repository, options.snapshot))
            fixtures(repository, options.snapshot, options.fixtures, options.allow_historical)
    except Exception as error:
        print("FAIL: " + str(error), file=sys.stderr)
        sys.exit(1)
