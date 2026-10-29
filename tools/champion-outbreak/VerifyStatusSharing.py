"""Reproduce Windows replacement denial and verify share-delete readers preserve atomic publication."""
from pathlib import Path
import argparse
import json
import subprocess
from FreezeBenchmark import open_shared_delete


def verify(root):
    root=root.resolve();assert str(root).lower().startswith('d:\\conficturauotesttools\\')
    root.mkdir(exist_ok=False)
    script=root/'replace.ps1'
    script.write_text("param([string]$Source,[string]$Destination)\n$ErrorActionPreference='Stop'\ntry { [System.IO.File]::Replace($Source,$Destination,[NullString]::Value); exit 0 } catch { $code=$_.Exception.GetBaseException().HResult -band 65535; if ($code -in @(5,32)) { exit 32 }; throw }\n")
    destination=root/'state.xml';replacement=root/'next.xml'
    destination.write_bytes(b'<state value="old"/>');replacement.write_bytes(b'<state value="new"/>')
    command=['powershell','-NoProfile','-File',str(script),'-Source',str(replacement),'-Destination',str(destination)]
    with destination.open('rb') as ordinary:
        result=subprocess.run(command,capture_output=True)
        denied=result.returncode
    (root/'ordinary-reader.json').write_text(json.dumps(dict(exit=denied,stderr=result.stderr.decode(errors='replace'))))
    assert denied==32,'Expected the ordinary Windows reader to deny replacement'
    # Windows can remove the destination name before reporting a sharing violation.
    destination.write_bytes(b'<state value="old"/>');replacement.write_bytes(b'<state value="new"/>')
    with open_shared_delete(destination) as shared:
        allowed=subprocess.run(command,capture_output=True).returncode
        old_handle=shared.read()
    assert allowed==0 and old_handle==b'<state value="old"/>' and destination.read_bytes()==b'<state value="new"/>'
    report=dict(ordinary_reader_denied_replace=True,share_delete_reader_allowed_replace=True,
                existing_handle_retained_old_complete_bytes=True,new_open_sees_new_complete_bytes=True,production_touched=False)
    (root/'verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    verify(parser.parse_args().root)
