# IronPython 2.7; native client movement during a natural reaction turn.
import os
import json
import time
import clr
clr.AddReference('System.Xml')
from System import Action, Array, Object, Int32
from System.Diagnostics import Process
from System.IO import FileStream, FileMode, FileAccess, FileShare
from System.Xml import XmlDocument
from Assistant import Engine
from ClassicAssist.Data.Macros.Commands.MsgCommands import Msg

ROOT=os.path.abspath(OUTBREAK_ROOT)
EVIDENCE=os.path.join(ROOT,'evidence')
if os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName))!=os.path.normcase(os.path.join(ROOT,'client','ClassicUO.exe')):
    raise Exception('Live dodge requires its exact disposable client')
with open(os.path.join(EVIDENCE,'prepared.json')) as source:
    if json.load(source)['fixture']!='acceptance':raise Exception('Wrong fixture')
with open(os.path.join(EVIDENCE,'live-turn-case.json')) as source:configuration=json.load(source)
if configuration['case']!='dodge':raise Exception('Wrong frozen client case')
walk=Engine.ClassicAssembly.GetType('ClassicUO.Game.Pathfinder').GetMethod('WalkTo')

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

def move(goal):
    def issue():
        try:
            accepted=bool(walk.Invoke(None,Array[Object]([Int32(n) for n in goal])))
            log('native_dodge_path',goal=goal,accepted=accepted)
        except Exception as error:log('failed',error=str(error))
    Engine.TickWorkQueue.Enqueue(Action(issue))

try:
    deadline=time.time()+120
    while not Engine.Connected and time.time()<deadline:time.sleep(.1)
    if not Engine.Connected:raise Exception('Private client connection deadline')
    with open(os.path.join(EVIDENCE,'live-turn-client-ready.txt'),'w') as output:output.write('Connected ordinary client; no combat or backend actions issued.')
    deadline=time.time()+180;phase='waiting';ended_turn=-1;reaction_started=0
    origin=None;reaction_turn=None;boss_turn=None;before_ap=None;movement_ap=None;old_warning=None
    goals=[];goal_index=0;last_move=0;observation_expired=False
    while time.time()<deadline:
        if not os.path.exists(os.path.join(EVIDENCE,'live-turn-state.xml')):time.sleep(.1);continue
        state=status()
        if state.GetAttribute('schedulerRunning')!='True':raise Exception('Natural scheduler stopped')
        if state.GetAttribute('alive')!='True':raise Exception('Actor died; retain failed case')
        current=number(state,'current')==number(state,'actor')
        turn=number(state,'actorTurns');warning=number(state,'warning')
        if state.GetAttribute('held')=='True':raise Exception('Restraint observed in dodge case')
        if phase=='waiting':
            if warning:
                old_warning=warning;phase='reaction';log('natural_warning',turn=turn,warning=warning,warning_turn=number(state,'warningTurn'),current=current)
            elif current and turn!=ended_turn:
                Msg('[EndTurn');ended_turn=turn;log('native_end_turn_before_warning',turn=turn)
        elif phase=='reaction' and current:
            if not reaction_started:
                origin=(int(Engine.Player.X),int(Engine.Player.Y),int(Engine.Player.Z))
                if origin[:2]!=(number(state,'x'),number(state,'y')):time.sleep(.1);continue
                reaction_started=time.time();reaction_turn=turn;boss_turn=number(state,'bossTurns');before_ap=number(state,'actorAP')
                goals=[(origin[0]+dx,origin[1]+dy,origin[2],0) for dx,dy in ((1,0),(0,1),(-1,0),(0,-1))]
                log('reaction_turn_observed',turn=turn,boss_turn=boss_turn,before_ap=before_ap,origin=origin)
                with open(os.path.join(EVIDENCE,'live-dodge-observation-ready.json'),'w') as output:
                    json.dump(dict(turn=turn,warning=old_warning,origin=origin,scope='Reaction turn waiting; no client movement yet'),output)
            released=(not configuration['observe_warning'] or os.path.exists(os.path.join(EVIDENCE,'live-dodge-observed.txt')))
            if not released and time.time()-reaction_started>=8:
                if not observation_expired:log('visual_handoff_expired_without_acceptance');observation_expired=True
                released=True
            if time.time()-reaction_started>=3.5 and released:
                if warning!=old_warning:raise Exception('Original warning vanished before movement')
                phase='move';move(goals[goal_index]);goal_index+=1;last_move=time.time()
        elif phase=='move':
            if not current or turn!=reaction_turn:raise Exception('Dodge left the reaction turn before client ended it')
            position=(number(state,'x'),number(state,'y'))
            if position!=origin[:2]:
                movement_ap=before_ap-number(state,'actorAP')
                if movement_ap<=0:raise Exception('Native movement did not spend AP')
                if warning!=old_warning:
                    log('native_dodge_moved',turn=turn,boss_turn=boss_turn,origin=origin,position=position,before_ap=before_ap,after_ap=number(state,'actorAP'),movement_ap=movement_ap)
                    Msg('[EndTurn');phase='await-next-turn';log('native_reaction_end_turn',turn=turn)
            elif time.time()-last_move>=1.5:
                if goal_index==len(goals):raise Exception('No legal adjacent native dodge path')
                move(goals[goal_index]);goal_index+=1;last_move=time.time()
        elif phase=='await-next-turn' and current and turn>reaction_turn and number(state,'bossTurns')>boss_turn:
            if warning==old_warning or (number(state,'x'),number(state,'y'))==origin[:2]:raise Exception('Original tile/warning returned')
            result=dict(passed=True,case='natural-warning-reaction-native-dodge',movement_ap_observed=movement_ap,
                        native_origin='ClassicAssist native pathfinder and ordinary EndTurn speech; not Computer Use input',
                        scheduler_stopped=False,warning_or_restraint_constructed_by_fixture=False,
                        original_warning_cancelled=True,later_boss_turn_observed=True,computer_use_proven_by_client=False)
            with open(os.path.join(EVIDENCE,'live-turn-client-result.json'),'w') as output:json.dump(result,output,indent=2)
            log('passed');break
        time.sleep(.1)
    else:raise Exception('Live dodge deadline; no restart')
except Exception as error:
    log('failed',error=str(error))
    with open(os.path.join(EVIDENCE,'live-turn-client-error.txt'),'w') as output:output.write(str(error))
    raise
