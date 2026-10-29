"""Verify controlled native toolkit evidence and no-save teardown; never accepts pacing."""
from pathlib import Path
import argparse
import csv
from datetime import datetime
import json
import xml.etree.ElementTree as ET
from RunToolkitProbes import CASES, EXTRA_CASES
from VerifyBenchmark import hashes, sha


def verify(root, require_complete, required_cases=()):
    repo=Path(__file__).resolve().parents[2];evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='benchmark'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    frozen=json.loads((evidence/'reference-freeze.json').read_text())
    for key,value in frozen['files'].items():
        assert sha(evidence/'frozen-inputs'/(key+Path(value['path']).suffix))==value['sha256']
    for key,path in [('fixture',root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakBenchmark.cs'),
                     ('client_policy',root/'client/ClassicAssist/OutbreakBenchmarkClient.py'),
                     ('reference_xml',evidence/'benchmark-reference.xml')]:
        assert sha(path)==frozen['files'][key]['sha256']
    inputs=json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/path)==expected for path,expected in inputs.items())
    assert hashes(root/'shard/Saves')==prepared['source_saves_sha256']
    for path,expected in json.loads((evidence/'protected-source-inputs.json').read_text()).items():
        path=Path(path);assert (hashes(path) if path.is_dir() else sha(path))==expected
    teardown=json.loads((evidence/'teardown.json').read_text(encoding='utf-8-sig'))
    assert teardown['forced_no_save'] and not teardown['graceful_shutdown_claimed']
    assert teardown['remaining_owned_processes']==teardown['remaining_private_listeners']==0
    assert (evidence/'native-stopped.txt').exists()
    settings=json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'))
    assert settings['ip']=='127.0.0.1' and settings['port']==prepared['port']
    assert settings['plugins']==[str(root/'client/ClassicAssist/ClassicAssist.dll')]
    reference=ET.parse(evidence/'benchmark-reference.xml').getroot()
    assert reference.get('access')=='Player' and sum(int(reference.get(k)) for k in ('rawStr','rawDex','rawInt'))==250
    assert reference.get('baseSkillTotal')=='9800' and len(reference.findall('equipment'))==10
    events=[json.loads(line) for line in (evidence/'benchmark-client.jsonl').read_text().splitlines()]
    assert not any(e['event'] in ('error','walk_error','attack','cast','escape') for e in events)
    health=list(csv.reader((evidence/'toolkit-health.csv').read_text().splitlines()))
    results=[]
    for path in sorted(evidence.glob('toolkit-result-*.xml'),key=lambda p:int(p.stem.rsplit('-',1)[1])):
        state=ET.parse(path).getroot();name=state.get('probe');sequence=int(state.get('probeSequence'))
        assert float(state.get('seconds'))==0 and state.get('bossKilled')=='False'
        assert int(state.get('enemyDeaths'))==0 and state.get('navigationOnly')=='False'
        observations=[e for e in events if e.get('sequence')==sequence]
        assert any(e['event']=='toolkit_case' and e['name']==name for e in observations)
        rows=[r for r in health if int(r[1])==sequence]
        success=state.get('state')=='toolkit-complete'
        actor=state.get('actor');target=state.get('probeTarget')
        damage=[r for r in rows if r[3]==actor and r[4]=='Damage' and int(r[6])<int(r[5])]
        mana_spent=int(state.get('probeManaBefore'))-int(state.get('mana'))
        if success:
            assert state.get('blessed')=='False' and state.get('alive')=='True'
            if name.startswith('ability-'):
                ability=['DoubleWhirlwindAttack','ArmorIgnore','DoubleStrike','FireStrike','DeathBlow'][int(name[-1])-1]
                assert any(e['event']=='toolkit_arm_observed' and e['ability']==ability for e in observations)
                assert mana_spent>0 and any(r[2]==target for r in damage)
                if name=='ability-1': assert len({r[2] for r in damage})==3
                if name in ('ability-3','ability-4'):
                    hits=[r for r in damage if r[2]==target]
                    assert any(abs((datetime.fromisoformat(a[0])-datetime.fromisoformat(b[0])).total_seconds())<.1
                               for i,a in enumerate(hits) for b in hits[i+1:]), 'No paired immediate hits'
            elif name=='regeneration':
                assert int(state.get('probeManaBefore'))==0 and int(state.get('mana'))>=10
                assert not any(e['event'] in ('toolkit_cast','use','toolkit_arm_requested') for e in observations)
            elif name=='return':
                steps=state.findall('returnStep')
                visits={int(e['index']):e for e in observations if e['event']=='toolkit_return_step'}
                for index,step in enumerate(steps[:-1]):
                    assert index in visits and max(abs(visits[index]['x']-int(step.get('x'))),abs(visits[index]['y']-int(step.get('y'))))<=1
                assert max(abs(int(state.get('x'))-int(state.get('stagingX'))),abs(int(state.get('y'))-int(state.get('stagingY'))))<=1
                assert any(e['event']=='walk' and e['accepted'] for e in events)
            elif name in ('strength','agility','refresh','petals','cure','bandage','heal'):
                supply=state.find("supply[@key='"+name+"']")
                assert int(supply.get('remaining'))<int(state.get('probeAmountBefore'))
                if name=='strength': assert state.get('str')=='150' and state.get('hitsMax')=='325'
                if name=='agility': assert state.get('dex')=='150' and state.get('stamMax')=='300'
                if name=='refresh': assert state.get('stam')==state.get('stamMax')
                if name in ('cure','bandage'): assert state.get('poisoned')=='False'
                if name=='heal': assert any(r[2]==actor and r[4]=='Healing' and int(r[6])>int(r[5]) for r in rows)
            else:
                assert any(e['event']=='toolkit_cast' for e in observations)
                assert mana_spent>0 and int(state.get('tithing'))<int(state.get('probeTithingBefore'))
                if name=='cleanse': assert state.get('poisoned')=='False'
                if name=='holy-light': assert any(r[2]==target for r in damage)
                if name=='close-wounds': assert any(r[2]==actor and r[4]=='Healing' for r in rows)
                if name=='journey': assert all(state.get(k)==state.get('refuge'+k.upper()) for k in ('x','y','z'))
                if name=='noble':
                    assert int(state.get('hits'))<=5 and int(state.get('mana'))<=5 and int(state.get('stam'))<=5
                    assert any(r[2]==target and r[4]=='Healing' for r in rows)
                    assert any(r[2]==actor and int(r[6])==1 for r in rows)
        started=next(e['utc_epoch'] for e in observations if e['event']=='toolkit_case')
        results.append(dict(name=name,sequence=sequence,passed=success,mana_net_spent=mana_spent,
                            seconds_from_client_observation=datetime.fromisoformat(state.get('utc')).timestamp()-started,
                            actor_health_events=[r for r in rows if r[2]==actor],damaged_target_count=len({r[2] for r in damage})))
    complete=set(CASES).issubset({r['name'] for r in results if r['passed']})
    report=dict(preservation_and_teardown_passed=True,production_inputs_checked=len(inputs),private_save_files=len(hashes(root/'shard/Saves')),
                complete_controlled_toolkit=complete,results=results,
                scope='Controlled native Player actions; stopped zero-melee targets; synthetic between-case setup; no encounter or pacing acceptance',
                encounter_clears=0,tuning_passes=0,production_activation=False)
    (evidence/'toolkit-verification.json').write_text(json.dumps(report,indent=2))
    print('Preservation/teardown passed; controlled cases passed '+str(sum(r['passed'] for r in results))+'/'+str(len(results))+'; all 20 covered: '+str(complete))
    if require_complete: assert complete and all(r['passed'] for r in results)
    assert set(required_cases).issubset({r['name'] for r in results if r['passed']})


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--require-complete',action='store_true')
    parser.add_argument('--require-cases',nargs='+',choices=CASES+EXTRA_CASES,default=[])
    args=parser.parse_args()
    verify(args.root.resolve(),args.require_complete,args.require_cases)
