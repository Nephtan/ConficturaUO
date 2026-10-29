"""Request bounded controlled native probes; never starts a measured encounter."""
from pathlib import Path
import argparse
import json
import os
import time
import uuid
from FreezeBenchmark import read_status

CASES = ['strength','agility','refresh','petals','cleanse','cure','bandage','heal','close-wounds',
         'consecrate','fury','enemy-one','holy-light','dispel']+['ability-'+str(i) for i in range(1,6)]+['remove-curse']
EXTRA_CASES = ['journey','return','noble','regeneration']


def run(args):
    root=args.root.resolve();evidence=root/'evidence'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert json.loads((evidence/'prepared.json').read_text())['fixture']=='benchmark'
    results=[]
    for name in args.cases:
        state=read_status(evidence/'benchmark-state.xml')
        assert state.get('online')=='True' and state.get('state') in ('ready','toolkit-complete','toolkit-failed'), 'Not an idle connected probe scene'
        assert state.get('seconds')=='0', 'A measured run has already started'
        sequence=int(state.get('probeSequence','0'))+1
        command=evidence/'benchmark-command.txt';assert not command.exists(),'Pending command'
        temporary=evidence/(str(uuid.uuid4())+'.tmp');temporary.write_text('probe-'+name);os.replace(temporary,command)
        deadline=time.monotonic()+35
        result=evidence/('toolkit-result-'+str(sequence)+'.xml')
        while not result.exists() and time.monotonic()<deadline:
            time.sleep(.2)
        assert result.exists(),'Probe result timeout: '+name
        state=read_status(result)
        entry=dict(name=name,sequence=sequence,state=state.get('state'),mana_before=int(state.get('probeManaBefore')),
                   mana_after=int(state.get('mana')),hits_before=int(state.get('probeHitsBefore')),hits_after=int(state.get('hits')),
                   str=int(state.get('str')),dex=int(state.get('dex')),stam=int(state.get('stam')),stam_max=int(state.get('stamMax')),
                   poisoned=state.get('poisoned'),tithing_spent=int(state.get('probeTithingBefore'))-int(state.get('tithing')),
                   target_health_loss=int(state.get('probeTargetHitsBefore'))-int(state.get('probeTargetHits')),
                   scope=state.get('probeScope'),result_file=str(result))
        results.append(entry)
        report=evidence/('toolkit-run-'+str(sequence)+'.json')
        report.write_text(json.dumps(results,indent=2))
        print(name+': '+entry['state']+'; mana '+str(entry['mana_before'])+'->'+str(entry['mana_after'])+
              '; health '+str(entry['hits_before'])+'->'+str(entry['hits_after']),flush=True)
        assert entry['state']=='toolkit-complete','Retained failed probe: '+name
    print('Controlled native probes complete; no encounter/pacing acceptance.',flush=True)


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('root',type=Path)
    parser.add_argument('--cases',nargs='+',choices=CASES+EXTRA_CASES,default=CASES)
    run(parser.parse_args())
