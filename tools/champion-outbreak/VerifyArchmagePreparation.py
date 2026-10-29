"""Verify preserved native original-macro preparation, keeping character details private."""
from pathlib import Path
import argparse
import ast
import json
import xml.etree.ElementTree as ET
from VerifyBenchmark import hashes, sha


def verify(root, require_ready=False, historical_fixture=False, setup_only=False, stationary_case=False, route_case=False):
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
    source_profiles=[Path(name) for name in frozen['original_provenance'] if Path(name).suffix.lower()=='.json']
    assert len(source_profiles)==1
    original_profile=json.loads(source_profiles[0].read_text(encoding='utf-8-sig'))
    assert all(profile[key]==original_profile[key] for key in ('Options','Friends')),'Gameplay options or friends changed'
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
    combat_case=stationary_case or route_case
    assert not (stationary_case and route_case)
    state=ET.parse(evidence/('archmage-result.private.xml' if combat_case else 'archmage-state.xml')).getroot()
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
    if combat_case:
        mode='stationary' if stationary_case else 'route'
        assert frozen['mode']==mode and state.get('mode')==mode and state.get('encounter')=='True'
        combat=json.loads((evidence/'archmage-combat-freeze.private.json').read_text())
        assert combat['mode']==mode and combat['observation_window_seconds']==(300 if stationary_case else 2700)
        assert combat['idle_after_stage_seconds']==(120 if stationary_case else None)
        for name,expected in combat['files'].items():assert sha(evidence/name)==expected
        assert dict(ET.parse(evidence/'archmage-state.xml').getroot().attrib)==dict(state.attrib),'Terminal state differs from captured outcome'
        if route_case:
            from FreezeBenchmark import movement_policy
            assert movement_policy(root)==frozen['movement_policy'] and frozen['movement_policy']['always_run'] is True
    else:
        assert state.get('state')=='prepared' and not state.get('failure')
        assert state.get('encounter')=='False'
        assert not (evidence/'archmage-client-error.txt').exists()
    assert (evidence/'archmage-macro-prepared.txt').exists()
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
    normal_stop=any('|stop|reason=normal' in line or (combat_case and '|stop|reason=dead' in line) for line in rows)
    if not combat_case:assert normal_stop
    ready=pools and wraith and summon and jewel and result.get('blessed')=='False'
    report=dict(preservation_and_teardown_passed=True,actual_copied_character_confirmed=True,
        original_macro_maintenance_completed=True,observed_expected_pools=pools,observed_wraith=wraith,
        observed_follower=summon,observed_jewel=jewel,native_maintenance_effects_passed=ready,
        blessed_during_preparation=result.get('blessed')=='True',fixture_matches_current_repository=current_fixture,
        production_inputs_checked=len(inputs),private_save_files=len(hashes(root/'shard/Saves')),
        scope='Bounded maintenance observation only; no combat, isolated spell-effect or current/live-character acceptance',
        production_activation=False)
    case_passed=False
    if combat_case:
        errors=(evidence/'archmage-client-error.txt').exists()
        outcome=state.get('state')
        case_passed=(outcome in ('completed','dead','stationary-idle','stationary-window-ended')
                     and not state.get('failure') and not errors and normal_stop
                     and (evidence/'archmage-macro-finished.txt').exists())
        if stationary_case:case_passed=case_passed and state.get('movementRequests')=='0'
        else:
            events=[json.loads(line) for line in (evidence/'archmage-navigation.private.jsonl').read_text().splitlines()]
            case_passed=(case_passed and outcome=='completed' and state.get('bossKilled')=='True' and state.get('clearedBroods')=='7'
                         and state.get('phase')=='Cooldown' and int(state.get('movementRequests'))>0
                         and any(n['event']=='walk' and n['accepted'] for n in events)
                         and state.get('pendingGlands')=='6' and state.get('newGroundGold')=='15000')
        final=ET.parse(evidence/'archmage-final.private.xml').getroot()
        def quantities(node):
            values={}
            for item in node.iter('item'):values[item.get('type')]=values.get(item.get('type'),0)+int(item.get('amount'))
            return values
        before=quantities(result);after=quantities(final)
        def research(node):
            return {(item.get('serial'),n.get('field')):int(n.get('value')) for item in node.iter('item') for n in item.findall('research')}
        previous=research(result);latest=research(final)
        deltas=dict(inventory={k:after.get(k,0)-before.get(k,0) for k in set(before)|set(after) if after.get(k,0)!=before.get(k,0)},
                    research={str(k):latest.get(k,0)-previous.get(k,0) for k in set(previous)|set(latest) if latest.get(k,0)!=previous.get(k,0)},
                    scope='Net inventory and public numeric research-field changes, not per-cast accounting')
        (evidence/'archmage-resource-deltas.private.json').write_text(json.dumps(deltas,indent=2))
        report.update(stationary_case_observed=case_passed if stationary_case else False,
                      route_assisted_clear_passed=case_passed if route_case else False,outcome=outcome,seconds=float(state.get('seconds')),
                      cleared_broods=int(state.get('clearedBroods')),owned_enemy_deaths=int(state.get('ownedDeaths')),
                      player_deaths=int(state.get('playerDeaths')),boss_killed=state.get('bossKilled')=='True',
                      movement_requests=int(state.get('movementRequests')),pending_glands=int(state.get('pendingGlands','0')),
                      pending_skulls=int(state.get('pendingSkulls','0')),pending_artifacts=int(state.get('pendingArtifacts','0')),
                      new_ground_gold=int(state.get('newGroundGold','0')),corpse_loot_assessed=False,
                      scope=('Bounded original stationary macro on historical copy; no walking assistance, complete-clear or pacing inference from a partial'
                             if stationary_case else 'Original macro/helper combat with explicit idle walking adapter; historical copy only; no minimum duration or veteran pacing inference'))
    (evidence/'archmage-preparation-verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))
    if require_ready:assert ready,'Expected maintenance effects were not all observed'
    if combat_case:assert ready and case_passed,'Combat case did not establish the selected observation/clear'


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--require-ready',action='store_true')
    parser.add_argument('--historical-fixture',action='store_true')
    parser.add_argument('--setup-only',action='store_true')
    parser.add_argument('--require-stationary',action='store_true')
    parser.add_argument('--require-route',action='store_true')
    args=parser.parse_args();verify(args.root,args.require_ready,args.historical_fixture,args.setup_only,args.require_stationary,args.require_route)
