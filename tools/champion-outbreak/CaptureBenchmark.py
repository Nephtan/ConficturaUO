"""Capture an observed terminal measured outcome unchanged; never classifies it as a clear."""
from pathlib import Path
import argparse
import hashlib
import json
import os
import time
import uuid
import xml.etree.ElementTree as ET
from FreezeBenchmark import read_status, read_shared

TERMINAL=('completed','dead','failed','aborted','stopped','driver-failed','navigation-completed')


def capture(root):
    root=root.resolve();evidence=root/'evidence'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert (evidence/'reference-freeze.json').exists()
    for attempt in range(10):
        try: blob=read_shared(evidence/'benchmark-state.xml');break
        except (PermissionError,FileNotFoundError):
            if attempt==9:raise
            time.sleep(.1)
    state=ET.fromstring(blob)
    assert state.get('state') in TERMINAL
    assert float(state.get('seconds'))>0,'No measured run started'
    final=evidence/'run-001-final.xml'
    assert not final.exists(),'Never replace a captured final outcome'
    temporary=evidence/(str(uuid.uuid4())+'.tmp');temporary.write_bytes(blob);os.rename(temporary,final)
    (evidence/'final-capture.json').write_text(json.dumps(dict(source='benchmark-state.xml',state=state.get('state'),
        utc=state.get('utc'),seconds=state.get('seconds'),sha256=hashlib.sha256(blob).hexdigest(),
        unchanged_native_observation=True),indent=2))
    print('Captured unchanged terminal observation: '+state.get('state')+'; '+state.get('seconds')+' seconds')


def wait_for_outcome(root):
    from RunArchmageCase import bind_server
    evidence=root.resolve()/'evidence';deadline=time.monotonic()+2700;last_report=0;stop_requested=False
    require_alive=bind_server(root.resolve(),evidence)
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root.resolve()/'shard'
    assert (evidence/'reference-freeze.json').exists()
    assert json.loads((evidence/'prepared.json').read_text())['fixture']=='benchmark'
    while True:
        require_alive()
        state=read_status(evidence/'benchmark-state.xml')
        if state.get('state') in TERMINAL:
            capture(root)
            acknowledgement=time.monotonic()+2
            while state.get('state')!='driver-failed' and time.monotonic()<acknowledgement:
                events=[]
                for line in (evidence/'benchmark-client.jsonl').read_text().splitlines():
                    try:events.append(json.loads(line))
                    except json.JSONDecodeError:pass
                if any(e.get('event')=='finished' and e.get('state')==state.get('state') for e in events):break
                time.sleep(.05)
            (evidence/'native-stop.txt').write_text('Terminal outcome captured; cleanup without saving.')
            return
        if time.monotonic()-last_report>=45:
            print('Observed '+state.get('state')+'; '+state.get('seconds')+'s; brood mask '+state.get('clearedBroods','0')+
                  '; HP '+state.get('hits','?')+'; mana '+state.get('mana','?'),flush=True)
            last_report=time.monotonic()
        if time.monotonic()>deadline and not stop_requested:
            command=evidence/'benchmark-command.txt';assert not command.exists()
            temporary=evidence/(str(uuid.uuid4())+'.tmp');temporary.write_text('stop');os.rename(temporary,command)
            (evidence/'pilot-timeout.txt').write_text('45-minute observation limit; retained stop, not a clear.')
            stop_requested=True
        if time.monotonic()>deadline+15: raise TimeoutError('No terminal acknowledgement after stop request')
        time.sleep(.2)


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--wait',action='store_true',help='Observe until terminal, capture immediately, then request guarded cleanup')
    args=parser.parse_args()
    if args.wait: wait_for_outcome(args.root)
    else: capture(args.root)
