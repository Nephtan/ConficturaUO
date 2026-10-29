"""Verify preserved native original-macro preparation, keeping character details private."""
from pathlib import Path
import argparse
import ast
import json
import xml.etree.ElementTree as ET
from VerifyBenchmark import hashes, sha


def verify(root, require_ready=False, historical_fixture=False, setup_only=False):
    repo=Path(__file__).resolve().parents[2];root=root.resolve();evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='archmage-preparation'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    frozen=json.loads((evidence/'archmage-input-freeze.private.json').read_text(encoding='utf-8-sig'))
    mutable={'client/settings.json','client/ClassicAssist/Profiles/settings.json'}
    for name,expected in frozen['files'].items():
        if name.replace('\\','/') not in mutable:assert sha(root/name)==expected,name
    fixture=root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs'
    assert sha(fixture)==prepared['harness_sha256']
    current_fixture=sha(fixture)==sha(repo/'tests/ArachnidArchmagePreparation.cs')
    assert historical_fixture or current_fixture,'Fixture differs from current source; select retained historical evidence explicitly'
    for original,record in frozen['original_provenance'].items():
        assert sha(Path(original))==record['sha256'],'Personal source content changed'
        if Path(original).suffix=='.py':
            assert sha(root/'client/ClassicAssist/PrivateArchmage'/Path(original).name)==record['sha256']
    settings=json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'))
    assert settings['ip']=='127.0.0.1' and settings['port']==prepared['port']
    profile=json.loads((root/'client/ClassicAssist/Profiles/settings.json').read_text(encoding='utf-8-sig'))
    assert profile['Options']['GetFriendEnemyUsesIgnoreList']==frozen['ignore_list_option']
    assert not profile['General']['Autologin']
    assert settings['plugins']==[str(root/'client/ClassicAssist/ClassicAssist.dll')]
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
    state=ET.parse(evidence/'archmage-state.xml').getroot()
    if setup_only:
        assert not require_ready and state.get('state')=='ready' and state.get('online')=='False'
        assert (evidence/'preflight-not-run.json').exists()
        assert not any((evidence/name).exists() for name in ('archmage-macro-prepared.txt','archmage-diagnostics.private.jsonl','archmage-client-error.txt'))
        report=dict(preservation_and_teardown_passed=True,original_macro_executed=False,
                    native_maintenance_effects_passed=False,production_inputs_checked=len(inputs),
                    private_save_files=len(hashes(root/'shard/Saves')),production_activation=False,
                    scope='Retained server setup only; stopped before any client/macro execution')
        (evidence/'archmage-preparation-verification.json').write_text(json.dumps(report,indent=2))
        print(json.dumps(report));return
    assert state.get('state')=='prepared' and not state.get('failure')
    assert state.get('encounter')=='False' and (evidence/'archmage-macro-prepared.txt').exists()
    assert not (evidence/'archmage-client-error.txt').exists()
    original=ET.parse(evidence/'archmage-original.private.xml').getroot()
    source=ET.parse(evidence/'source-character-inspection.private.xml').getroot().find('character')
    assert original.get('serial')==source.get('serial')
    assert original.get('access')=='Player' and original.get('blessed')=='False'
    for field in ('rawStr','rawDex','rawInt','hitsMax','stamMax','manaMax'):
        assert original.get(field)==source.get(field),field
    signature=lambda n: sorted(tuple(sorted(s.attrib.items())) for s in n.findall('skill'))
    assert signature(original)==signature(source),'Original copied skills differ'
    equipment=lambda n: sorted((s.get('type'),s.get('layer')) for s in n.findall('equipment'))
    assert equipment(original)==equipment(source),'Original copied equipment differs'
    result=ET.parse(evidence/'archmage-prepared.private.xml').getroot()
    tree=ast.parse((root/'client/ClassicAssist/PrivateArchmage/archmage-overlord.py').read_text(encoding='utf-8-sig'))
    keys={'EXPECTED_MAX_HITS','EXPECTED_MAX_STAM','EXPECTED_MAX_MANA','WRAITH_BODY','JEWEL_GRAPHIC','JEWEL_HUE'}
    constants={n.targets[0].id:ast.literal_eval(n.value) for n in tree.body if isinstance(n,ast.Assign)
               and isinstance(n.targets[0],ast.Name) and n.targets[0].id in keys}
    assert set(constants)==keys
    pools=all(result.get(k)==str(constants[v]) for k,v in [('hitsMax','EXPECTED_MAX_HITS'),('stamMax','EXPECTED_MAX_STAM'),('manaMax','EXPECTED_MAX_MANA')])
    wraith=result.get('body')==str(constants['WRAITH_BODY']);summon=int(result.get('followers'))>0
    jewel=any(n.get('graphic')==str(constants['JEWEL_GRAPHIC']) and n.get('hue')==str(constants['JEWEL_HUE']) for n in result.iter('item'))
    rows=[json.loads(line)['message'] for line in (evidence/'archmage-diagnostics.private.jsonl').read_text().splitlines()]
    normal_stop=any('|stop|reason=normal' in line for line in rows)
    assert normal_stop
    ready=pools and wraith and summon and jewel and result.get('blessed')=='False'
    report=dict(preservation_and_teardown_passed=True,actual_copied_character_confirmed=True,
        original_macro_maintenance_completed=True,observed_expected_pools=pools,observed_wraith=wraith,
        observed_follower=summon,observed_jewel=jewel,native_maintenance_effects_passed=ready,
        blessed_during_preparation=result.get('blessed')=='True',fixture_matches_current_repository=current_fixture,
        production_inputs_checked=len(inputs),private_save_files=len(hashes(root/'shard/Saves')),
        scope='Bounded maintenance observation only; no combat, isolated spell-effect or current/live-character acceptance',
        production_activation=False)
    (evidence/'archmage-preparation-verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))
    if require_ready:assert ready,'Expected maintenance effects were not all observed'


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--require-ready',action='store_true')
    parser.add_argument('--historical-fixture',action='store_true')
    parser.add_argument('--setup-only',action='store_true')
    args=parser.parse_args();verify(args.root,args.require_ready,args.historical_fixture,args.setup_only)
