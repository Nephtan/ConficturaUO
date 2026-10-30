"""Verify staged-presentation provenance and recorded observations, never infer pixel quality."""
from pathlib import Path
import argparse
import hashlib
import json
import subprocess
import xml.etree.ElementTree as ET
from VerifyBenchmark import sha,hashes


def verify(root,require_six=False,production_ref=None):
    root=root.resolve();e=root/'evidence';repo=Path(__file__).resolve().parents[2]
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    prepared=json.loads((e/'prepared.json').read_text());assert prepared['fixture']=='acceptance'
    frozen=json.loads((e/'presentation-inputs.json').read_text())
    for name,expected in frozen['files'].items():
        if name.replace('\\','/')!='client/settings.json':assert sha(root/name)==expected,name
    assert sha(repo/'tests/ArachnidOutbreakPresentation.cs')==sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakPresentation.cs')
    assert sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakNativeAcceptance.cs')==prepared['harness_sha256']
    inputs=json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    historical=None;runtime=0
    for name,expected in inputs.items():
        if sha(repo/name)!=expected:
            assert production_ref,'Retained production source differs; explicit historical ref required'
            if historical is None:historical=subprocess.check_output(['git','rev-parse',production_ref+'^{commit}'],cwd=repo,text=True).strip()
            blob=subprocess.check_output(['git','show',historical+':'+name],cwd=repo)
            assert expected in [hashlib.sha256(b).hexdigest() for b in (blob,blob.replace(b'\r\n',b'\n').replace(b'\n',b'\r\n'))],name
        normalized=name.replace('\\','/')
        if normalized.startswith('Data/Scripts/'):
            private_expected=prepared['overlays'].get(normalized[len('Data/Scripts/'):],expected)
            assert sha(root/'shard'/name)==private_expected,name
            runtime+=1
    assert sha(root/'shard/ConficturaServer.exe')==prepared['core_sha256']
    assert hashes(root/'shard/Saves')==prepared['source_saves_sha256']
    for name,expected in json.loads((e/'protected-source-inputs.json').read_text()).items():
        p=Path(name);assert (hashes(p) if p.is_dir() else sha(p))==expected,name
    settings=json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'))
    assert settings['ip']=='127.0.0.1' and settings['port']==prepared['port'] and settings['plugins']==[]
    original=ET.parse(e/'native-actor-original.private.xml').getroot()
    source=ET.parse(e/'native-actor-inspection.private.xml').getroot().find('character')
    final=ET.parse(e/'native-actor-final.private.xml').getroot()
    assert original.get('serial')==source.get('serial')==final.get('serial')
    for name in ('rawStr','rawDex','rawInt','hitsMax','stamMax','manaMax'):assert original.get(name)==source.get(name)
    skills=lambda n:sorted(tuple(sorted(x.attrib.items())) for x in n.findall('skill'))
    assert skills(original)==skills(source)
    assert sorted(x.get('serial') for x in original.findall('equipment'))==sorted(x.get('serial') for x in final.findall('equipment'))
    teardown=json.loads((e/'teardown.json').read_text(encoding='utf-8-sig'))
    assert teardown['native_stop_acknowledged'] and teardown['forced_no_save'] and not teardown['graceful_shutdown_claimed']
    assert teardown['remaining_owned_processes']==teardown['remaining_private_listeners']==0
    observations=[json.loads(x) for x in (e/'visual-observations.jsonl').read_text().splitlines()]
    sites={x['site'] for x in observations if x.get('anchor')=='boss' and x.get('observed') and x.get('warning_visible') and x.get('boss_visible')}
    errored=(e/'presentation-error.txt').exists()
    complete=len(sites)==6 and not errored
    report=dict(preservation_and_owned_teardown_passed=True,production_inputs_checked=len(inputs),private_runtime_inputs_checked=runtime,
        private_save_files=20,copied_actor_identity_and_equipment_preserved=True,
        recorded_boss_location_samples=len(sites),six_recorded_boss_samples=complete,retained_fixture_error=errored,
        verifier_confirms_rendered_pixels=False,manual_computer_use_observations=observations,
        scope='Staged camera and production graphics with AI/turn scheduler stopped; manual sample observations only, no natural timing/AI/movement/pacing/geography approval',
        production_activation=False,site_approval=False,historical_production_ref=historical)
    (e/'presentation-verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
    if require_six:assert complete,'Six current non-errored manually recorded boss samples required'


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);parser.add_argument('--require-six',action='store_true');parser.add_argument('--production-ref')
    args=parser.parse_args();verify(args.root,args.require_six,args.production_ref)
