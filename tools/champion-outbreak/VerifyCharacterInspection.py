"""Verify private copied-character inspection and preservation without publishing its contents."""
from pathlib import Path
import argparse
import json
import xml.etree.ElementTree as ET
from VerifyBenchmark import hashes, sha


def verify(root):
    repo=Path(__file__).resolve().parents[2];root=root.resolve();evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='profile-inspection'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    fixture=root/'shard/Data/Scripts/TestOnly/ArachnidCharacterInspection.cs'
    assert sha(fixture)==prepared['harness_sha256']==sha(repo/'tests/ArachnidCharacterInspection.cs')
    assert not (evidence/'character-inspection-error.private.txt').exists()
    observation=ET.parse(evidence/'character-inspection.private.xml').getroot()
    assert observation.get('target')=='Zanthus'
    matches=int(observation.get('matches'));assert matches==len(observation.findall('character'))
    inputs=json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/path)==expected for path,expected in inputs.items())
    assert sha(root/'shard/ConficturaServer.exe')==prepared['core_sha256']
    assert hashes(root/'shard/Saves')==prepared['source_saves_sha256']
    for path,expected in json.loads((evidence/'protected-source-inputs.json').read_text()).items():
        path=Path(path);assert (hashes(path) if path.is_dir() else sha(path))==expected
    teardown=json.loads((evidence/'teardown.json').read_text(encoding='utf-8-sig'))
    assert teardown['forced_no_save'] and not teardown['graceful_shutdown_claimed']
    assert teardown['remaining_owned_processes']==teardown['remaining_private_listeners']==0
    assert (evidence/'native-stopped.txt').exists()
    report=dict(preservation_and_teardown_passed=True,production_inputs_checked=len(inputs),private_save_files=len(hashes(root/'shard/Saves')),
                target_matches=matches,unique_copied_character_found=matches==1,
                scope='Offline historical copied-character observation only; no current/live profile, macro, client or combat acceptance',
                production_activation=False)
    (evidence/'character-inspection-verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    verify(parser.parse_args().root)
