# IronPython 2.7; ordinary commands from the disposable ClassicAssist client.
import os
import json
import time
import clr
clr.AddReference('System.Xml')
from System.Diagnostics import Process
from System.IO import FileStream, FileMode, FileAccess, FileShare
from System.Xml import XmlDocument
from Assistant import Engine
from ClassicAssist.Data.Macros.Commands.MsgCommands import Msg
from ClassicAssist.Data.Macros.Commands.ObjectCommands import UseObject

ROOT=os.path.abspath(OUTBREAK_ROOT)
EVIDENCE=os.path.join(ROOT,'evidence')
if os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName))!=os.path.normcase(os.path.join(ROOT,'client','ClassicUO.exe')):
    raise Exception('Live turn client requires its exact disposable process')
with open(os.path.join(EVIDENCE,'prepared.json')) as source:
    if json.load(source)['fixture']!='acceptance':raise Exception('Wrong fixture')

def log(event,**values):
    values.update(event=event,epoch=time.time())
    with open(os.path.join(EVIDENCE,'live-turn-client.jsonl'),'a') as output:output.write(json.dumps(values)+'\n')

def status():
    for attempt in range(10):
        try:
            stream=FileStream(os.path.join(EVIDENCE,'live-turn-state.xml'),FileMode.Open,FileAccess.Read,FileShare.ReadWrite|FileShare.Delete)
            try:
                document=XmlDocument();document.Load(stream);return document.DocumentElement
            finally:stream.Close()
        except Exception:
            if attempt==9:raise
            time.sleep(.05)

def number(node,key):return int(node.GetAttribute(key))

try:
    deadline=time.time()+120
    while not Engine.Connected and time.time()<deadline:time.sleep(.1)
    if not Engine.Connected:raise Exception('Private client connection deadline')
    with open(os.path.join(EVIDENCE,'live-turn-client-ready.txt'),'w') as output:output.write('Connected ordinary client; no combat or backend actions issued.')
    deadline=time.time()+180;phase='waiting';ended_turn=-1;reaction_started=0;before_ap=None
    while time.time()<deadline:
        if not os.path.exists(os.path.join(EVIDENCE,'live-turn-state.xml')):time.sleep(.1);continue
        state=status()
        if state.GetAttribute('schedulerRunning')!='True':raise Exception('Natural scheduler stopped')
        if state.GetAttribute('alive')!='True':raise Exception('Actor died; retain failed case')
        current=number(state,'current')==number(state,'actor')
        turn=number(state,'actorTurns');warning=number(state,'warning');held=state.GetAttribute('held')=='True'
        if phase=='waiting':
            if held:raise Exception('Hold observed before a client reaction')
            if warning:
                phase='reaction';log('natural_warning',turn=turn,warning_turn=number(state,'warningTurn'),current=current)
            elif current and turn!=ended_turn:
                Msg('[EndTurn');ended_turn=turn;log('native_end_turn_before_warning',turn=turn)
        elif phase=='reaction':
            if held:raise Exception('Warning impacted before client ended reaction turn')
            if current:
                if not reaction_started:reaction_started=time.time();log('reaction_turn_observed',turn=turn)
                if time.time()-reaction_started>=3.5:
                    Msg('[EndTurn');phase='await-hold';log('native_reaction_end_turn',turn=turn)
        elif phase=='await-hold':
            if held and current:
                if turn<=number(state,'warningTurn'):raise Exception('No later reaction turn')
                serial=number(state,'web')
                if Engine.Items.GetItem(serial) is None:raise Exception('Web is not in actual client item memory')
                before_ap=number(state,'actorAP');UseObject(serial,True);phase='await-free'
                log('native_web_use',turn=turn,before_ap=before_ap,web=serial)
        elif phase=='await-free' and not held and state.GetAttribute('pending')=='False':
            if not current or number(state,'actorAP')!=before_ap-4:raise Exception('Escape did not cost exactly four AP in same turn')
            result=dict(passed=True,case='natural-warning-reaction-impact-native-self-escape',
                        native_origin='ClassicAssist ordinary EndTurn speech command and UseObject packet; not Computer Use input',
                        escape_ap=4,scheduler_stopped=False,warning_or_restraint_constructed_by_fixture=False)
            with open(os.path.join(EVIDENCE,'live-turn-client-result.json'),'w') as output:json.dump(result,output,indent=2)
            log('passed');break
        time.sleep(.1)
    else:raise Exception('Live turn case deadline; no restart')
except Exception as error:
    log('failed',error=str(error))
    with open(os.path.join(EVIDENCE,'live-turn-client-error.txt'),'w') as output:output.write(str(error))
    raise
