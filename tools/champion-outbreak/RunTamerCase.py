"""Freeze and observe an actual copied-client tamer case; a loss is a valid outcome."""
from pathlib import Path
import argparse,json,time,xml.etree.ElementTree as ET
from FreezeBenchmark import read_status,sha
from FixtureHeartbeat import bind_fixture


def run(root):
    root=root.resolve();e=root/'evidence';repo=Path(__file__).resolve().parents[2]
    prepared=json.loads((e/'prepared.json').read_text());assert prepared['fixture']=='archmage-preparation'
    assert (e/'active-build.txt').read_text()=='Sargar' and (e/'infestation-case.txt').exists()
    alive=bind_fixture(root,e);deadline=time.monotonic()+180
    while time.monotonic()<deadline:
        alive();state=read_status(e/'archmage-state.xml')
        if state.get('state')=='prepared':break
        assert state.get('state') not in ('failed','client-failed','dead','stopped'),state.attrib
        time.sleep(.2)
    assert state.get('state')=='prepared' and state.get('online')=='True' and state.get('blessed')=='False'
    freeze=json.loads((e/'tamer-input-freeze.private.json').read_text())
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
    assert skills(original)==skills(ready),'Skills changed before combat'
    pets=[item.attrib for item in ready.findall('pet')];assert 1<=len(pets)<=int(original.get('followersMax'))
    saved={item.get('serial'):item for item in original.findall('savedCompanion')}
    assert all(pet['serial'] in saved and pet['type']==saved[pet['serial']].get('type') and pet['hitsMax']==saved[pet['serial']].get('hitsMax') for pet in pets),'Pet identity or saved capacity changed'
    (e/'tamer-observation-frozen.private.json').write_text(json.dumps(dict(prepared=ready.attrib,pets=pets,mode='Actual client archery/pet orders/veterinary/finite potions, no grants; normal trainer method fixture claims; not human-optimality evidence'),indent=2))
    (e/'archmage-combat-freeze.private.json').write_text((e/'tamer-observation-frozen.private.json').read_text())
    (e/'archmage-command.txt').write_text('start-route');deadline=time.monotonic()+660
    terminal={'completed','dead','aborted','failed','client-failed','stopped','timeout'}
    with(e/'tamer-observations.private.jsonl').open('a') as output:
        while time.monotonic()<deadline:
            alive();state=read_status(e/'archmage-state.xml');row=dict(time=time.time(),state=state.attrib,pets=[n.attrib for n in state.findall('pet')]);output.write(json.dumps(row)+'\n');output.flush()
            if state.get('state') in terminal:break
            time.sleep(1)
    if state.get('state') not in terminal:
        (e/'archmage-command.txt').write_text('stop');raise RuntimeError('Observer deadline; requested stop')
    result=dict(state=state.attrib,pets=[n.attrib for n in state.findall('pet')],winning_required=False,accepted_gameplay_evidence=state.get('state') not in {'failed','client-failed'} and int(state.get('outgoingDirect','0'))+int(state.get('outgoingPets','0'))>0)
    (e/'tamer-observation.private.json').write_text(json.dumps(result,indent=2));print(json.dumps(result,indent=2))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);run(parser.parse_args().root)
