"""Observe a finite actual-client timing correction; toolkit evidence only."""
from pathlib import Path
import argparse,json,time,xml.etree.ElementTree as ET
from FreezeBenchmark import read_status,sha
from FixtureHeartbeat import bind_fixture


def run(root):
    root=root.resolve();e=root/'evidence';alive=bind_fixture(root,e)
    snapshot=e/'observer-source';snapshot.mkdir()
    for name in [Path(__file__).name,'FixtureHeartbeat.py','FreezeBenchmark.py']:(snapshot/name).write_bytes((Path(__file__).parent/name).read_bytes())
    freeze=json.loads((e/'saved-build-input-freeze.private.json').read_text())
    for name,expected in freeze['files'].items():
        if not name.endswith('/settings.json'):assert sha(root/name)==expected
    deadline=time.monotonic()+120
    while time.monotonic()<deadline:
        s=alive()
        assert s.get('state') not in {'failed','client-failed'},s.get('failure')
        if s.get('state')=='prepared':break
        time.sleep(.2)
    assert s.get('state')=='prepared' and s.get('online')=='True'
    original=ET.parse(e/'archmage-original.private.xml').getroot();ready=ET.parse(e/'archmage-prepared.private.xml').getroot()
    for key in ['serial','access','blessed','rawStr','rawDex','rawInt','hitsMax','manaMax','tithing','karma']:assert original.get(key)==ready.get(key),key
    (e/'healing-probe-begin.txt').write_text('Audited actual saved client; declared40 damage; no resources granted')
    deadline=time.monotonic()+90
    with(e/'healing-probe-observations.private.jsonl').open('a') as output:
        while time.monotonic()<deadline:
            alive();s=read_status(e/'healing-probe-state.xml');output.write(json.dumps(s.attrib)+'\n');output.flush()
            if s.get('phase') in {'healed','failed','window-ended'}:break
            time.sleep(.2)
    result=dict(observed=s.attrib,typed_self_healing_confirmed=int(s.get('healing','0'))>0,winning_required=False,scope=freeze['scope'],prior_frontier_cases_retain_prior_policy=True,reviewed_training_example=False)
    (e/'feedback-close-wounds-probe.json').write_text(json.dumps(result,indent=2));print(json.dumps(result),flush=True)


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('root',type=Path);args=p.parse_args()
    try:run(args.root)
    finally:(args.root/'evidence/native-stop.txt').write_text('Finite toolkit probe ended; owned no-save cleanup')
