# IronPython: unchanged original macro/helper, bounded preparation or stationary combat.
import os
import sys
import time
import json
import clr
clr.AddReference('System.Xml')
from System.Diagnostics import Process
from System.IO import FileStream, FileMode, FileAccess, FileShare, IOException
from System import UnauthorizedAccessException
from System.Xml import XmlDocument
from Assistant import Engine
from System import Action,Array,Object,Int32
from System.Threading import ManualResetEvent

ROOT = os.path.abspath(OUTBREAK_ROOT)
EVIDENCE = os.path.join(ROOT, 'evidence')
if not os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName)).startswith(os.path.normcase(os.path.join(ROOT, 'client') + os.sep)):
    raise Exception('Not the disposable copied client')
with open(os.path.join(EVIDENCE, 'prepared.json')) as source:
    if json.load(source)['fixture'] != 'archmage-preparation':
        raise Exception('Wrong private fixture')
with open(os.path.join(EVIDENCE, 'native-private-root.txt')) as source:
    if os.path.normcase(source.read().strip()) != os.path.normcase(os.path.join(ROOT, 'shard')):
        raise Exception('Wrong private root')

def status():
    for attempt in range(10):
        stream=None
        try:
            stream=FileStream(os.path.join(EVIDENCE,'archmage-state.xml'),FileMode.Open,
                              FileAccess.Read,FileShare.ReadWrite | FileShare.Delete)
            document=XmlDocument();document.XmlResolver=None;document.Load(stream)
            return document.DocumentElement
        except (IOError,IOException,UnauthorizedAccessException):
            if attempt==9:raise
            time.sleep(.05)
        finally:
            if stream is not None:stream.Dispose()

original_enemy = GetEnemy
original_message = SysMessage
prepared_scan = False
mode_path=os.path.join(EVIDENCE,'archmage-mode.txt')
with open(mode_path) as source:
    case_mode=source.read().strip()
if case_mode not in ('preparation','stationary','route'):raise Exception('Unsupported case mode')
TERMINAL=('completed','dead','aborted','stationary-idle','stationary-window-ended','failed','client-failed','stopped','timeout')
original_pause,original_speech,original_cast=Pause,Msg,Cast
last_goal=None
last_walk=0
navigation_rejections=0
boss_last_seen=None
search_route=None
search_index=0
if case_mode=='route':
    native=Engine.ClassicAssembly.GetType('ClassicUO.Game.Pathfinder')
    walk=native.GetMethod('WalkTo');walking=native.GetProperty('AutoWalking')
    from ClassicAssist.Data.ClassicUO.Objects import Pathfinder as AssistantPathfinder
    LOG=os.path.join(EVIDENCE,'archmage-navigation.private.jsonl')
    execfile(os.path.join(ROOT,'client/ClassicAssist/PrivateArchmage/EncounterNavigation.py'),globals())

def stop_route_walk():
    if case_mode!='route' or not bool(walking.GetValue(None)):return
    done=ManualResetEvent(False)
    def cancel():
        try:AssistantPathfinder.Cancel()
        finally:done.Set()
    Engine.TickWorkQueue.Enqueue(Action(cancel))
    if not done.WaitOne(1000):raise Exception('Native path cancellation did not acknowledge')
    if bool(walking.GetValue(None)):raise Exception('Native path did not stop before casting')

def infestation_search_goals(center, remembered):
    origin=tuple(remembered) if remembered is not None else tuple(center)
    goals=[origin+(1,)]
    for radius in (16,40,80,120):
        for dx,dy in ((0,-1),(1,-1),(1,0),(1,1),(0,1),(-1,1),(-1,0),(-1,-1)):
            goals.append((origin[0]+dx*radius,origin[1]+dy*radius,origin[2],1))
    return goals

def route_navigation():
    global boss_last_seen,search_route,search_index
    observed=status()
    if observed.GetAttribute('state')!='running-route':return
    if navigation_rejections>=10:raise Exception('Ten route requests refused')
    player=Engine.Player;visible=[]
    for node in observed.SelectNodes('enemy'):
        mobile=Engine.Mobiles.GetMobile(int(value(node,'serial')))
        if mobile is not None and not mobile.IsDead:
            position=(int(mobile.X),int(mobile.Y),int(mobile.Z))
            distance=max(abs(position[0]-int(player.X)),abs(position[1]-int(player.Y)))
            visible.append((distance,int(mobile.Serial),position))
            if node.GetAttribute('type')=='ArachnidOutbreakChampion':
                boss_last_seen=position;search_route=None
    if visible:
        visible.sort();goal=visible[0][2]+(7,)
    else:
        mask=int(value(observed,'clearedBroods'))
        infestation=os.path.exists(os.path.join(EVIDENCE,'infestation-case.txt'))
        stage=3 if infestation else next((i for i in range(3) if not mask & (1<<i)),3)
        anchor=observed.SelectSingleNode("anchor[@stage='%d']"%stage)
        center=(int(value(anchor,'x')),int(value(anchor,'y')),int(value(anchor,'z')))
        if stage<3:goal=center+(7,)
        else:
            if search_route is None:
                search_route=infestation_search_goals(center,boss_last_seen) if infestation else boss_reacquisition_goals(center,boss_last_seen)
                search_index=0
            while search_index<len(search_route) and max(abs(int(player.X)-search_route[search_index][0]),abs(int(player.Y)-search_route[search_index][1]))<=1:
                search_index+=1
            if search_index==len(search_route):search_index=0
            goal=search_route[search_index]
    if max(abs(int(player.X)-goal[0]),abs(int(player.Y)-goal[1]))>goal[3]:
        navigate(goal[0],goal[1],goal[2],goal[3],time.time())

def route_pause(milliseconds):
    if case_mode=='route' and int(milliseconds)==250 and globals().get('state',{}).get('idle') is True and sys._getframe(1).f_code.co_name=='_run':
        route_navigation()
    return original_pause(milliseconds)

def route_speech(message,*args):
    if str(message).startswith('[Cast'):stop_route_walk()
    return original_speech(message,*args)

def route_cast(*args):
    stop_route_walk()
    return original_cast(*args)

def diagnostic(message, *args):
    with open(os.path.join(EVIDENCE, 'archmage-diagnostics.private.jsonl'), 'a') as output:
        output.write(json.dumps(dict(epoch=time.time(), message=str(message))) + '\n')
    return original_message(message, *args)

def preparation_enemy(*args):
    global CASM_TEST_MAX_LOOPS, prepared_scan
    # This first scan follows successful original maintenance. Summon discovery
    # scans earlier in maintenance still use the unmodified native command.
    macro_state = globals().get('state', {})
    if not prepared_scan and int(macro_state.get('next_maintenance_at', 0)) > 0:
        prepared_scan = True
        if case_mode=='preparation':
            CASM_TEST_MAX_LOOPS = 1
            return False
        with open(os.path.join(EVIDENCE,'archmage-macro-prepared.txt'),'w') as output:
            output.write('Original maintenance complete; combat scan blocked pending frozen start.')
        deadline=time.time()+180
        while time.time()<deadline:
            observed=status().GetAttribute('state')
            if observed=='running-'+case_mode:break
            if observed in TERMINAL:raise Exception('Case ended before combat start: '+observed)
            time.sleep(.1)
        if status().GetAttribute('state')!='running-'+case_mode:raise Exception('Frozen start deadline')
    if case_mode!='preparation' and prepared_scan and status().GetAttribute('state') in TERMINAL:
        CASM_TEST_MAX_LOOPS=1
        macro_state['next_maintenance_at']=int(time.time()*1000)+3600000
        stop_route_walk()
        return False
    return original_enemy(*args)

try:
    deadline = time.time() + 90
    while time.time() < deadline:
        if Engine.Connected and status().GetAttribute('state') == 'ready':
            break
        time.sleep(.2)
    if not Engine.Connected or status().GetAttribute('state') != 'ready':
        raise Exception('Copied preparation not ready')
    GetEnemy = preparation_enemy
    SysMessage = diagnostic
    if case_mode=='route':Pause,Msg,Cast=route_pause,route_speech,route_cast
    CASM_TEST_ENABLE_DIAGNOSTICS = True
    CASM_TEST_MAX_LOOPS = 10000
    sys.path.insert(0, os.path.join(ROOT, 'client', 'ClassicAssist', 'PrivateArchmage'))
    execfile(os.path.join(ROOT, 'client', 'ClassicAssist', 'PrivateArchmage', 'archmage-overlord.py'), globals())
    reason=globals().get('state', {}).get('stop_reason')
    if not prepared_scan or reason not in (('normal',) if case_mode=='preparation' else ('normal','dead')):
        raise Exception('Original macro did not complete the selected bounded observation normally')
    if case_mode!='preparation' and status().GetAttribute('state') not in TERMINAL:
        raise Exception('Original macro stopped before a terminal case observation')
    entries=[]
    for entry in Engine.Journal.GetEntireBuffer():
        if entry is not None:
            entries.append(dict(text=unicode(entry.Text or ''), speaker=unicode(entry.Name or '')))
    with open(os.path.join(EVIDENCE, 'archmage-journal.private.json'), 'w') as output:
        json.dump(entries, output, indent=2)
    marker='archmage-macro-prepared.txt' if case_mode=='preparation' else 'archmage-macro-finished.txt'
    with open(os.path.join(EVIDENCE,marker), 'w') as output:
        movement_scope='separate idle walking adapter enabled' if case_mode=='route' else 'wrapper issued no movement'
        output.write('Original macro completed bounded '+case_mode+' observation; '+movement_scope+'.')
except Exception as error:
    with open(os.path.join(EVIDENCE, 'archmage-client-error.txt'), 'w') as output:
        output.write(str(error))
    raise
