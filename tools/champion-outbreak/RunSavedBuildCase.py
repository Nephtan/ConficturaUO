"""Freeze and observe an actual saved-build frontier case; a loss is a valid outcome."""
from pathlib import Path
import argparse,json,time,xml.etree.ElementTree as ET
from FreezeBenchmark import read_status,sha
from FixtureHeartbeat import bind_fixture


def run(root):
    root=root.resolve();e=root/'evidence';repo=Path(__file__).resolve().parents[2]
    snapshot=e/'observer-source';assert not snapshot.exists(),'Fresh observer source snapshot required'
    snapshot.mkdir()
    for source_name in [Path(__file__).name,'FixtureHeartbeat.py','FreezeBenchmark.py']:
        source=Path(__file__).resolve().parent/source_name
        (snapshot/source_name).write_bytes(source.read_bytes())
    (e/'observer-source-sha256.private.json').write_text(json.dumps({q.name:sha(q) for q in snapshot.iterdir()},indent=2))

    prepared=json.loads((e/'prepared.json').read_text());assert prepared['fixture']=='archmage-preparation'
    assert (e/'active-build-serial.private').exists() and (e/'weak-frontier-case.txt').exists()
    alive=bind_fixture(root,e);deadline=time.monotonic()+180
    while time.monotonic()<deadline:
        alive();state=read_status(e/'archmage-state.xml')
        if state.get('state')=='prepared':break
        assert state.get('state') not in ('failed','client-failed','dead','stopped'),state.attrib
        time.sleep(.2)
    assert state.get('state')=='prepared' and state.get('online')=='True' and state.get('blessed')=='False'
    freeze=json.loads((e/'saved-build-input-freeze.private.json').read_text())
    mutable={'client/settings.json','client/ClassicAssist/Profiles/settings.json'}
    for name,value in freeze['files'].items():
        if name not in mutable:assert sha(root/name)==value,name
    inputs=json.loads((Path(prepared['candidate'])/'inputs.json').read_text())['files']
    assert all(sha(repo/name)==value for name,value in inputs.items())
    original=ET.parse(e/'archmage-original.private.xml').getroot();ready=ET.parse(e/'archmage-prepared.private.xml').getroot()
    fields=('serial','access','blessed','rawStr','rawDex','rawInt','str','dex','int','hitsMax','stamMax','manaMax','body','followersMax','ancientDamage','ancientCasting')
    assert all(original.get(name) is not None and ready.get(name) is not None for name in fields),'Required capability field absent'
    assert all(original.get(name)==ready.get(name) for name in fields),'Preparation changed a saved capability'
    equipment=lambda node:sorted(tuple(sorted(item.attrib.items())) for item in node.findall('equipment') if item.get('layer')!='Mount')
    assert equipment(original)==equipment(ready),'Equipment changed before combat'
    mounts=[item for item in ready.findall('equipment') if item.get('layer')=='Mount']
    if mounts:
        assert len(mounts)==1 and mounts[0].get('type')=='Server.Mobiles.MountItem'
        saved_mount={item.get('serial'):item for item in original.findall('savedCompanion')}.get(ready.get('mountSerial'))
        assert saved_mount is not None and saved_mount.get('type')==ready.get('mountType') and saved_mount.get('hitsMax')==ready.get('mountHitsMax'),'Mount was not the original saved companion restored by ordinary region/login rules'
    skills=lambda node:sorted(tuple(sorted(item.attrib.items())) for item in node.findall('skill'))
    if skills(original)!=skills(ready):
        before={q.get('name'):q.attrib for q in original.findall('skill')};changes=[]
        assert set(before)=={q.get('name') for q in ready.findall('skill')},'Unexpected skill-set change'
        for q in ready.findall('skill'):
            old=before.get(q.get('name'));new=q.attrib
            if old==new:continue
            assert old and q.get('name') in {'Meditation','Focus'} and old['cap']==new['cap'],'Unexpected capability change'
            delta=float(new['base'])-float(old['base'])
            assert 0<=delta<=.300001 and abs((float(new['effective'])-float(old['effective']))-delta)<.00001,'Unexpected passive-skill delta'
            changes.append(dict(skill=q.get('name'),before=old,after=new,source='RegenRates.CheckBonusSkill called by passive mana/stamina regeneration; no harness assignment'))
        assert changes,'Unclassified skill change'
        (e/'passive-preparation-skill-deltas.private.json').write_text(json.dumps(changes,indent=2))
    inventory=lambda node:sorted((item.get('serial'),item.get('type'),item.get('amount')) for item in node.findall('.//item'))
    assert inventory(original)==inventory(ready),'Preparation changed actual finite inventory'
    (e/'archmage-combat-freeze.private.json').write_text(json.dumps(dict(prepared=ready.attrib,scope='Actual saved active Player, natural young frontier, no grants; ordinary melee/owned standard spells/finite healing; declared toolkit omissions and optional autonomous clones.'),indent=2))
    (e/'archmage-command.txt').write_text('start-route');deadline=time.monotonic()+840
    terminal={'completed','dead','aborted','failed','client-failed','stopped','timeout','frontier-window-ended'}
    with(e/'saved-build-observations.private.jsonl').open('a') as output:
        while time.monotonic()<deadline:
            alive();state=read_status(e/'archmage-state.xml');row=dict(time=time.time(),state=state.attrib,pets=[n.attrib for n in state.findall('pet')],enemies=[n.attrib for n in state.findall('enemy')],clones=[n.attrib for n in state.findall('clone')],damage=[n.attrib for n in state.findall('damageEntry')],patches=[n.attrib for n in state.findall('patch')]);output.write(json.dumps(row)+'\n');output.flush()
            if state.get('state') in terminal:break
            time.sleep(1)
    if state.get('state') not in terminal:
        (e/'archmage-command.txt').write_text('stop');raise RuntimeError('Observer deadline; requested stop')
    actions=[json.loads(line) for line in (e/'saved-build-actions.private.jsonl').read_text().splitlines()]
    attacks=sum(row.get('event') in ('attack','cast') for row in actions)
    result=dict(state=state.attrib,clones=[n.attrib for n in state.findall('clone')],winning_required=False,frontier_balance_accepted=False,client_action_attempts=attacks,observed_active_case=state.get('state') not in {'failed','client-failed'} and state.get('frontierId') is not None and attacks>0,limitations=['Finite scouting/retreat/healing/eligible primary and owned Close Wounds policy; research, bard, other abilities/chivalry unautomated','Selected historical build is not an inferred human playstyle','Clone-only AI behavior remains a separate proxy limitation'])
    (e/'saved-build-observation.private.json').write_text(json.dumps(result,indent=2))
    (e/'feedback-saved-build.json').write_text(json.dumps(dict(classification='native client acceptance and driver limitations',expected='Real saved vulnerable Player reaches naturally established tier0 frontier and uses finite owned toolkit',observed=result,reviewed_training_example=False),indent=2))
    (e/'native-stop.txt').write_text('Bounded representative test ended; cleanup owned fixture without save')
    print(json.dumps(dict(state=state.get('state'),seconds=state.get('seconds'),incoming=state.get('incoming'),direct=state.get('outgoingDirect'),other=state.get('outgoingOther'),actions=attacks,clones=state.get('clones'),observed=result['observed_active_case'])))



if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);args=parser.parse_args()
    try:run(args.root)
    except Exception as error:
        (args.root/'evidence/feedback-saved-build-failure.json').write_text(json.dumps(dict(classification='fixture/client failure',error=str(error),reviewed_training_example=False),indent=2))
        (args.root/'evidence/native-stop.txt').write_text('Observer failed; bounded no-save cleanup')
        raise
