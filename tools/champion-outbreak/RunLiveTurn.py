"""Observe one natural-scheduler copied-client case, or verify its retained result after owned teardown."""
from pathlib import Path
import argparse
from datetime import datetime
import json
import time
import xml.etree.ElementTree as ET
from FreezeBenchmark import read_shared,read_status
from RunArchmageCase import bind_server,publish
from VerifyBenchmark import sha,hashes

def context(root):
    root=root.resolve();e=root/'evidence';repo=Path(__file__).resolve().parents[2]
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    p=json.loads((e/'prepared.json').read_text());assert p['fixture']=='acceptance'
    frozen=json.loads((e/'live-turn-input-freeze.json').read_text())
    for name,expected in frozen['files'].items():
        if name.replace('\\','/') not in ('client/settings.json','client/ClassicAssist/Profiles/settings.json'):assert sha(root/name)==expected,name
    fixture=root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakNativeAcceptance.cs'
    assert sha(fixture)==p['harness_sha256']==sha(repo/'tests/ArachnidOutbreakNativeAcceptance.cs')
    case=frozen.get('case','escape');assert case in ('escape','dodge')
    driver='LiveDodgeClient.py' if case=='dodge' else 'LiveTurnClient.py'
    assert frozen.get('client_policy',driver)==driver
    assert sha(root/'client/ClassicAssist'/driver)==sha(repo/'tools/champion-outbreak'/driver)
    p['_case']=case
    return root,e,repo,p

def run(root):
    root,e,repo,p=context(root);alive=bind_server(root,e);deadline=time.monotonic()+150
    while time.monotonic()<deadline:
        alive()
        if (e/'live-turn-client-ready.txt').exists():
            try:
                state=json.loads(read_shared(e/'native-state.json'))
                if state['connected'] and state['alive'] and not state['blessed'] and state['access']=='Player':break
            except (OSError,ValueError):pass
        time.sleep(.1)
    else:raise RuntimeError('Connected scene deadline; retain setup, no restart')
    publish(e,'native-request.txt','live-turn')
    deadline=time.monotonic()+190;previous=None;next_progress=0
    while time.monotonic()<deadline:
        alive()
        if (e/'live-turn-client-error.txt').exists():
            if not (e/'native-request.txt').exists():publish(e,'native-request.txt','stop')
            raise RuntimeError('Client case failed; retained error and requested owned no-save stop')
        if (e/'live-turn-state.xml').exists():
            state=read_status(e/'live-turn-state.xml');key=tuple((k,v) for k,v in state.attrib.items() if k!='utc')
            if key!=previous:
                with (e/'live-turn-observer.jsonl').open('a') as out:out.write(json.dumps(dict(state.attrib))+'\n')
                previous=key
            if time.monotonic()>=next_progress:
                print('Natural scheduler observed; warning='+state.get('warning')+'; held='+state.get('held')+'; actor AP='+state.get('actorAP'),flush=True)
                next_progress=time.monotonic()+15
            if (e/'live-turn-client-result.json').exists():
                (e/'live-turn-terminal.xml').write_bytes(read_shared(e/'live-turn-state.xml'))
                publish(e,'native-request.txt','stop')
                print('Native scripted reaction/'+p['_case']+' retained; owned no-save stop requested.',flush=True);return
        time.sleep(.05)
    publish(e,'native-request.txt','stop')
    raise RuntimeError('Natural case deadline; stop requested, no restart')

def verify(root):
    root,e,repo,p=context(root)
    result=json.loads((e/'live-turn-client-result.json').read_text());assert result['passed']
    assert not (e/'live-turn-client-error.txt').exists()
    rows=[json.loads(line) for line in (e/'live-turn-observer.jsonl').read_text().splitlines()]
    assert rows and all(r['schedulerRunning']=='True' and r['alive']=='True' for r in rows)
    assert any(int(r['warning'])>0 and r['held']=='False' and r['current']==r['actor'] and int(r['actorTurns'])>int(r['warningTurn']) for r in rows)
    held=[r for r in rows if r['held']=='True']
    stamp=lambda s:datetime.fromisoformat(s.replace('Z','+00:00'))
    actions=[json.loads(line) for line in (e/'live-turn-client.jsonl').read_text().splitlines()]
    reaction=next(r for r in actions if r['event']=='reaction_turn_observed')
    ended=next(r for r in actions if r['event']=='native_reaction_end_turn')
    assert ended['epoch']-reaction['epoch']>=3.5
    terminal=ET.parse(e/'live-turn-terminal.xml').getroot()
    assert terminal.get('held')=='False' and terminal.get('pending')=='False' and terminal.get('current')==terminal.get('actor')
    if p['_case']=='escape':
        assert held
        assert (stamp(held[0]['utc'])-stamp(held[0]['warningObserved'])).total_seconds()>=3
        assert all(int(r['actorTurns'])>int(r['warningTurn']) for r in held)
        use=next(r for r in actions if r['event']=='native_web_use')
        assert int(terminal.get('actorAP'))==use['before_ap']-4
        assert any(r['current']==r['actor'] and r['actorAP']==str(use['before_ap']) and r['web']==str(use['web']) for r in held)
    else:
        assert result['case']=='natural-warning-reaction-native-dodge' and not held
        assert not any(r['event'] in ('failed','native_web_use') for r in actions)
        moved=next(r for r in actions if r['event']=='native_dodge_moved')
        assert moved['movement_ap']==moved['before_ap']-moved['after_ap']>0
        assert moved['turn']==reaction['turn']==ended['turn']
        assert moved['position']!=moved['origin'][:2]
        assert int(terminal.get('actorTurns'))>moved['turn'] and int(terminal.get('bossTurns'))>moved['boss_turn']
        assert (int(terminal.get('x')),int(terminal.get('y')))!=tuple(moved['origin'][:2])
        assert any(r['current']==r['actor'] and int(r['actorTurns'])==moved['turn'] and int(r['actorAP'])==moved['after_ap'] and r['warning']=='0' for r in rows)
    if(e/'native-actor.txt').exists():
        original=ET.parse(e/'native-actor-original.private.xml').getroot()
        source=ET.parse(e/'native-actor-inspection.private.xml').getroot().find('character')
        final=ET.parse(e/'native-actor-final.private.xml').getroot()
        assert original.get('serial')==source.get('serial')==final.get('serial')
        assert original.get('access')=='Player' and original.get('blessed')=='False'
        for key in ('rawStr','rawDex','rawInt','hitsMax','stamMax','manaMax'):assert original.get(key)==source.get(key)
        skills=lambda n:sorted(tuple(sorted(s.attrib.items())) for s in n.findall('skill'))
        assert skills(original)==skills(source)
        equipment=lambda n:sorted((s.get('type'),s.get('layer')) for s in n.findall('equipment'))
        assert equipment(original)==equipment(source)
        serials=lambda n:sorted(s.get('serial') for s in n.findall('equipment'))
        assert serials(original)==serials(final)
    inputs=json.loads((Path(p['candidate'])/'inputs.json').read_text())['files']
    assert all(sha(repo/name)==expected for name,expected in inputs.items())
    runtime_checked=0
    for name,expected in inputs.items():
        name=name.replace('\\','/')
        if name.startswith('Data/Scripts/'):
            expected=p['overlays'].get(name[len('Data/Scripts/'):],expected)
            assert sha(root/'shard'/name)==expected,name
            runtime_checked+=1
    settings=json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'))
    assert settings['ip']=='127.0.0.1' and settings['port']==p['port']
    assert settings['plugins']==[str(root/'client/ClassicAssist/ClassicAssist.dll')]
    profile=json.loads((root/'client/ClassicAssist/Profiles/settings.json').read_text(encoding='utf-8-sig'))
    assert not profile['General']['Autologin']
    assert sha(root/'shard/ConficturaServer.exe')==p['core_sha256']
    assert hashes(root/'shard/Saves')==p['source_saves_sha256']
    for name,expected in json.loads((e/'protected-source-inputs.json').read_text()).items():
        path=Path(name);assert (hashes(path) if path.is_dir() else sha(path))==expected
    teardown=json.loads((e/'teardown.json').read_text(encoding='utf-8-sig'))
    assert teardown['native_stop_acknowledged'] and teardown['forced_no_save'] and not teardown['graceful_shutdown_claimed']
    assert teardown['remaining_owned_processes']==teardown['remaining_private_listeners']==0
    report=dict(natural_scheduler_reaction_and_native_escape_passed=True,escape_ap=4,
                production_inputs_checked=len(inputs),private_runtime_inputs_checked=runtime_checked,private_save_files=len(hashes(root/'shard/Saves')),
                preserved_inputs_and_teardown=True,computer_use_proven_by_this_verifier=False,
                scope='Explicit test-arm enrollment; natural production boss warning/impact and live scheduler; ordinary client EndTurn/UseObject; no pacing/cooperation/site approval',
                production_activation=False)
    if p['_case']=='dodge':
        report.pop('natural_scheduler_reaction_and_native_escape_passed');report.pop('escape_ap')
        report.update(natural_scheduler_reaction_and_native_dodge_passed=True,
                      observed_movement_ap=moved['movement_ap'],
                      scope='Explicit test-arm enrollment; natural warning/live scheduler; native client movement cancels original warning and later boss turn causes no hold; no Computer Use/cooperation/site approval inferred')
    (e/'live-turn-verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))

if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);parser.add_argument('--verify',action='store_true')
    args=parser.parse_args();(verify if args.verify else run)(args.root)
