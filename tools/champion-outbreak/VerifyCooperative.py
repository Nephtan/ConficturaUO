"""Verify controlled native role evidence, input preservation and acknowledged owned teardown."""
from pathlib import Path
import argparse
import json
import xml.etree.ElementTree as ET
from VerifyBenchmark import sha,hashes


def verify(root,require_complete=False):
    root=root.resolve();e=root/'evidence';repo=Path(__file__).resolve().parents[2]
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    p=json.loads((e/'prepared.json').read_text());frozen=json.loads((e/'cooperative-inputs.json').read_text())
    assert p['fixture']=='acceptance' and frozen['synthetic_roles'] and frozen['expected_clients']==2
    mutable={folder+'/'+name for folder in ('client','ally-client') for name in ('settings.json','ClassicAssist/Profiles/settings.json')}
    for name,expected in frozen['files'].items():
        if name.replace('\\','/') not in mutable:assert sha(root/name)==expected,name
    fixture_matches=sha(repo/'tests/ArachnidOutbreakCooperative.cs')==sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakCooperative.cs')
    driver_matches=all(sha(repo/'tools/champion-outbreak/CooperativeClient.py')==sha(root/folder/'ClassicAssist/CooperativeClient.py') for folder in ('client','ally-client'))
    inputs=json.loads((Path(p['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files'];runtime=0
    for name,expected in inputs.items():
        assert sha(repo/name)==expected,name
        normalized=name.replace('\\','/')
        if normalized.startswith('Data/Scripts/'):
            assert sha(root/'shard'/name)==p['overlays'].get(normalized[len('Data/Scripts/'):],expected),name
            runtime+=1
    assert sha(root/'shard/ConficturaServer.exe')==p['core_sha256']
    assert sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakNativeAcceptance.cs')==p['harness_sha256']
    assert hashes(root/'shard/Saves')==p['source_saves_sha256']
    for name,expected in json.loads((e/'protected-source-inputs.json').read_text()).items():
        path=Path(name);assert (hashes(path) if path.is_dir() else sha(path))==expected,name
    for folder in ('client','ally-client'):
        settings=json.loads((root/folder/'settings.json').read_text(encoding='utf-8-sig'))
        assert settings['ip']=='127.0.0.1' and settings['port']==p['port']
        assert settings['plugins']==[str(root/folder/'ClassicAssist/ClassicAssist.dll')]
    teardown=json.loads((e/'teardown.json').read_text(encoding='utf-8-sig'))
    assert teardown['native_stop_acknowledged'] and teardown['forced_no_save'] and not teardown['graceful_shutdown_claimed']
    assert teardown['remaining_owned_processes']==teardown['remaining_private_listeners']==0
    events={role:[json.loads(x) for x in (e/('cooperative-'+role+'.jsonl')).read_text().splitlines()] if (e/('cooperative-'+role+'.jsonl')).exists() else [] for role in ('patient','helper')}
    rows=[]
    for path in sorted(e.glob('cooperative-result-*.xml'),key=lambda x:int(x.stem.rsplit('-',1)[1])):
        s=ET.parse(path).getroot();name=s.get('phase');sequence=int(s.get('sequence'))
        assert s.get('passed')==s.get('patientConnected')==s.get('helperConnected')==s.get('patientAlive')==s.get('helperAlive')=='True'
        assert s.get('patientPlayer')==s.get('helperPlayer')=='True'
        assert s.get('patient')!=s.get('helper')
        # Prevention resolves synchronously from the staged owned poison attempt;
        # an idle client may legitimately miss that sub-tick phase.
        required_roles=('patient','helper') if name=='bard' else () if name=='prevent' else ('helper',)
        for role in required_roles:assert any(x['event']=='case_observed' and x['sequence']==sequence and x['phase']==name for x in events[role])
        native=[x for x in events['helper'] if x['event'].startswith('native_')]
        # Cross-reference ordinary command origins; backend success alone is insufficient.
        if name=='heal':assert any(x['event']=='native_spell' and x['spell']=='Greater Heal' for x in native)
        elif name=='cure':assert any(x['event']=='native_spell' and x['spell']=='Cure' for x in native)
        elif name=='rescue':assert any(x['event']=='native_ally_web_use' for x in native) and s.get('heldObserved')=='True'
        elif name=='treat':assert any(x['event']=='native_party_treatment' for x in native) and s.get('charges')=='3'
        elif name=='prevent':assert s.get('charges')=='2' and s.get('poisoned')=='False'
        elif name=='bard':
            assert any(x['event']=='native_peacemaking' for x in native) and s.get('pacifiedByHelper')=='True'
            assert any(x['event']=='native_spell' and x['spell']=='Magic Arrow' for x in events['patient'])
        else:raise AssertionError('Unknown role result')
        rows.append(dict(case=name,sequence=sequence,points_before=float(s.get('pointsBefore')),points_after=float(s.get('points')),both_connected=True,both_alive=True))
    requested=frozen.get('expected_cases',['heal','cure','rescue','treat','prevent','bard'])
    complete=[r['case'] for r in rows]==requested and not list(e.glob('cooperative*error.txt'))
    report=dict(preservation_and_acknowledged_teardown_passed=True,production_inputs_checked=len(inputs),private_runtime_inputs_checked=runtime,private_save_files=len(p['source_saves_sha256']),synthetic_roles=True,completed_controlled_native_cases=rows,six_cases_complete=complete,current_fixture_matches=fixture_matches,current_client_driver_matches=driver_matches,retained_pre_case_failure=(e/'precase-failure.json').exists(),scope=frozen['scope'],computer_use_role_acceptance=False,production_activation=False,site_approval=False)
    report['six_cases_complete']=complete and len(rows)==6
    report['requested_cases_complete']=complete;report['requested_cases']=requested
    (e/'cooperative-verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
    if require_complete:assert complete and fixture_matches and driver_matches,'All requested current controlled role results required'


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);parser.add_argument('--require-complete',action='store_true')
    args=parser.parse_args();verify(args.root,args.require_complete)
