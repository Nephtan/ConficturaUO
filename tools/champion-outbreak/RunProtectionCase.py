"""Observe finite actual-player/pet transitions through shared native simulation; never acquire process handles."""
from pathlib import Path
import argparse,datetime,json,time,xml.etree.ElementTree as ET
from FreezeBenchmark import read_status,sha
from FixtureHeartbeat import bind_fixture


def run(root):
    root=root.resolve();e=root/'evidence';alive=bind_fixture(root,e)
    snapshot=e/'observer-source';assert not snapshot.exists(),'Fresh observer source snapshot required'
    snapshot.mkdir()
    for source_name in [Path(__file__).name,'FixtureHeartbeat.py','FreezeBenchmark.py']:
        source=Path(__file__).resolve().parent/source_name
        (snapshot/source_name).write_bytes(source.read_bytes())
    (e/'observer-source-sha256.private.json').write_text(json.dumps({q.name:sha(q) for q in snapshot.iterdir()},indent=2))

    prepared=json.loads((e/'prepared.json').read_text());frozen=json.loads((e/'protection-inputs.private.json').read_text())
    for name,expected in frozen['files'].items():
        if not(name.endswith('/settings.json')):assert sha(root/name)==expected,name
    def state():
        alive();s=read_status(e/'protection-state.xml')
        assert s.get('phase')!='failed',s.get('failure')
        for role in ('main','partner'):
            error=e/('protection-'+role+'-error.txt')
            assert not error.exists(),error.read_text() if error.exists() else ''
        stamp=datetime.datetime.fromisoformat(s.get('utc').replace('Z','+00:00'))
        assert (datetime.datetime.now(datetime.timezone.utc)-stamp).total_seconds()<10
        return s
    deadline=time.monotonic()+180
    while time.monotonic()<deadline:
        s=state();main=read_status(e/'archmage-state.xml')
        if s.get('mainOnline')=='True' and s.get('partnerOnline')=='True' and main.get('state')=='prepared' and (e/'partner-prepared.private.xml').exists():break
        time.sleep(.2)
    assert main.get('state')=='prepared' and s.get('mainOnline')==s.get('partnerOnline')=='True','Two prepared actual clients required'
    for original_name,ready_name in [('archmage-original.private.xml','archmage-prepared.private.xml'),('partner-original.private.xml','partner-prepared.private.xml')]:
        original=ET.parse(e/original_name).getroot();ready=ET.parse(e/ready_name).getroot()
        for key in ('serial','access','blessed','rawStr','rawDex','rawInt','hitsMax','stamMax','manaMax','followersMax'):
            assert original.get(key)==ready.get(key),(ready_name,key)
        assert original.get('access')=='Player' and original.get('blessed')=='False'
        skills=lambda n:sorted(tuple(sorted(q.attrib.items())) for q in n.findall('skill'))
        assert skills(original)==skills(ready),'Saved skills changed'
        equipment=lambda n:sorted(tuple(sorted(q.attrib.items())) for q in n.findall('equipment') if q.get('layer')!='Mount')
        assert equipment(original)==equipment(ready),'Saved equipment changed'
    saved={}
    for name in ['archmage-original.private.xml','partner-original.private.xml']:
        original=ET.parse(e/name).getroot();saved.update({n.get('serial'):n.get('type') for n in original.findall('savedCompanion')})
        if original.get('mountSerial'):saved[original.get('mountSerial')]=original.get('mountType')
    assert all(saved.get(p.get('serial'))==('Server.Mobiles.'+p.get('type')) for p in s.findall('pet')),'Companion was not an original saved mount or stabled identity'
    (e/'archmage-combat-freeze.private.json').write_text(json.dumps(dict(source=prepared['candidate'],scope=frozen['scope'],no_stat_skill_gear_or_supply_grants=True),indent=2))
    rows=[]
    def phase(command,seconds):
        (e/'protection-command.txt').write_text(command)
        deadline=time.monotonic()+5
        while time.monotonic()<deadline:
            s=state()
            if s.get('phase')==('quiet-growth' if command=='start' else command):break
            time.sleep(.1)
        assert s.get('phase')==('quiet-growth' if command=='start' else command),'Phase command not acknowledged'
        end=time.monotonic()+seconds
        with(e/'protection-observations.private.jsonl').open('a') as output:
            while time.monotonic()<end:
                s=state();assert all(q.get('alive')=='True' for q in s.findall('player')+s.findall('partner')),'Actual player death: valid loss, remaining living-player protection phases unfulfilled'
                row=dict(utc=s.get('utc'),state=s.attrib,players=[n.attrib for n in s.findall('player')+s.findall('partner')],pets=[n.attrib for n in s.findall('pet')],colonies=[dict(state=n.attrib,enemies=[m.attrib for m in n.findall('enemy')],residents=[m.attrib for m in n.findall('resident')]) for n in s.findall('colony')])
                rows.append(row);output.write(json.dumps(row)+'\n');output.flush();time.sleep(1)
        print(command,'samples',len(rows),'actors',s.get('aggregateActors'),'movement',s.get('moves'),'petDamage',s.get('petDamageEvents'),flush=True)
    try:
        for command,seconds in [('start',120),('separated',35),('approach',35),('overlap',30),('first-leaves',25),('chase',8),('teleport-combat',25),('complete',1)]:phase(command,seconds)
        (e/'protection-terminal.private.xml').write_bytes(ET.tostring(state(),encoding='utf-8'))
        summary=dict(phases=sorted({r['state']['phase'] for r in rows}),samples=len(rows),maximum_registered=max(int(r['state']['aggregateActors']) for r in rows),maximum_pet_count=max(len(r['pets']) for r in rows),movement_requests=int(rows[-1]['state']['moves']),damage_events=int(rows[-1]['state']['damageEvents']),pet_damage_events=int(rows[-1]['state']['petDamageEvents']),winning_required=False,scope=frozen['scope'],limitations=['Native fixture relocation is not a learned teleport-spell test','No claim of full balance, all interactions, long-term stability or VPS throughput'])
        (e/'feedback-native-protection.json').write_text(json.dumps(summary,indent=2));print(json.dumps(summary),flush=True)
    finally:
        (e/'native-stop.txt').write_text('Bounded native multiplayer case finished; no-save cleanup')


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('root',type=Path);args=p.parse_args()
    try:run(args.root)
    except Exception as error:
        e=args.root/'evidence'
        (e/'feedback-protection-failure.json').write_text(json.dumps(dict(classification='native protection fixture or client failure',error=str(error),reviewed_training_example=False),indent=2))
        (e/'native-stop.txt').write_text('Bounded observer failed; preserve failure and stop owned fixtures without save')
        raise
