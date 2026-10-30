"""Observe bounded independent role cases through two ordinary copied clients."""
from pathlib import Path
import argparse
import json
import time
from FreezeBenchmark import read_status
from RunArchmageCase import bind_server,publish

CASES=['heal','cure','rescue','treat','prevent','bard']


def run(root):
    root=root.resolve();e=root/'evidence'
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    frozen=json.loads((e/'cooperative-inputs.json').read_text());assert frozen['synthetic_roles']
    cases=frozen.get('expected_cases',CASES);assert cases and len(set(cases))==len(cases) and set(cases)<=set(CASES)
    alive=bind_server(root,e)
    try:
        deadline=time.monotonic()+90
        while time.monotonic()<deadline:
            alive();s=read_status(e/'cooperative-state.xml')
            if s.get('patientConnected')==s.get('helperConnected')=='True':break
            time.sleep(.1)
        assert s.get('patientConnected')==s.get('helperConnected')=='True','Both native roles must connect'
        assert s.get('phase')=='waiting' and s.get('sequence')=='0','Fresh unstarted role cases required'
        for sequence,name in enumerate(cases,1):
            publish(e,'cooperative-command.txt',name);deadline=time.monotonic()+100
            result=e/('cooperative-result-'+str(sequence)+'.xml')
            while not result.exists() and time.monotonic()<deadline:
                alive()
                for path in e.glob('cooperative-*-error.txt'):raise RuntimeError(path.read_text())
                if (e/'cooperative-error.txt').exists():raise RuntimeError((e/'cooperative-error.txt').read_text())
                time.sleep(.1)
            assert result.exists(),'Role result deadline: '+name
            s=read_status(result);assert s.get('passed')=='True' and s.get('phase')==name
            assert s.get('patientConnected')==s.get('helperConnected')=='True'
            print(name+': native role result passed',flush=True)
            time.sleep(2) # Let ordinary spell/item recovery finish between independent cases.
        (e/'cooperative-complete.txt').write_text('Requested independent controlled native cases complete; rendering not inferred; owned cleanup remains required.')
        print(str(len(cases))+' role cases retained. No AI/pacing/economics acceptance. Request owned stop after any targeted observation.',flush=True)
    except BaseException:
        if not (e/'native-stopped.txt').exists() and not (e/'cooperative-command.txt').exists():publish(e,'cooperative-command.txt','stop')
        raise


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('root',type=Path);run(p.parse_args().root)
