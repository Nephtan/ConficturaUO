# IronPython 2.7; ordinary actions from a disposable copied ClassicAssist client.
import os
import json
import time
import clr
clr.AddReference('System.Xml')
from System import Action, Array, Object, Int32
from System.Diagnostics import Process
from System.IO import FileStream, FileMode, FileAccess, FileShare
from System.Xml import XmlDocument
from System.Threading import ManualResetEvent
from Assistant import Engine
from ClassicAssist.Data.Macros.Commands.ActionCommands import Attack
from ClassicAssist.Data.Macros.Commands.ObjectCommands import UseObject
from ClassicAssist.Data.Macros.Commands.TargetCommands import Target, WaitForTarget
from ClassicAssist.Data.Macros.Commands.MsgCommands import Msg
from ClassicAssist.Data.Macros.Commands.SpellCommands import Cast
from ClassicAssist.Data.ClassicUO.Objects import Pathfinder as AssistantPathfinder

ROOT = os.path.abspath(OUTBREAK_ROOT)
EVIDENCE = os.path.join(ROOT, 'evidence')
if not os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName)).startswith(os.path.normcase(os.path.join(ROOT,'client' if OUTBREAK_ROLE=='main' else 'ally-client')+os.sep)):
    raise Exception('Disposable client required')
with open(os.path.join(EVIDENCE,'prepared.json')) as source:
    if json.load(source)['fixture']!='archmage-preparation':raise Exception('Wrong fixture')

LOG = os.path.join(EVIDENCE,'protection-'+OUTBREAK_ROLE+'-actions.private.jsonl')
def log(event,**fields):
    fields.update(event=event,time=time.time())
    with open(LOG,'a') as output:output.write(json.dumps(fields)+'\n')
def value(node,name,default=0):
    text=node.GetAttribute(name)
    try:return int(text)
    except:return default
def document(name):
    for attempt in range(10):
        stream=None
        try:
            stream=FileStream(os.path.join(EVIDENCE,name),FileMode.Open,FileAccess.Read,FileShare.ReadWrite|FileShare.Delete)
            doc=XmlDocument();doc.XmlResolver=None;doc.Load(stream);return doc.DocumentElement
        except:
            if attempt==9:raise
            time.sleep(.1)
        finally:
            if stream is not None:stream.Dispose()
def status():return document('protection-state.xml')
def marker(name,text):
    with open(os.path.join(EVIDENCE,name),'w') as output:output.write(text)
def stop_walk():
    if not bool(walking.GetValue(None)):return
    finished=ManualResetEvent(False)
    def cancel():
        try:AssistantPathfinder.Cancel()
        finally:finished.Set()
    Engine.TickWorkQueue.Enqueue(Action(cancel))
    if not finished.WaitOne(1000):raise Exception('Native walking cancellation timed out')
def use(serial,target=None):
    serial=int(serial)
    if Engine.Items.GetItem(serial) is None:
        log('item_not_visible',serial=serial);return False
    UseObject(serial,True)
    if target is not None:
        if not WaitForTarget(1200):return False
        Target(int(target))
    log('use_item',serial=serial,target=target);return True

native=Engine.ClassicAssembly.GetType('ClassicUO.Game.Pathfinder')
walk=native.GetMethod('WalkTo');walking=native.GetProperty('AutoWalking')
last_goal=None;last_walk=0;navigation_rejections=0
# Only the established native walking helpers are reused; no server movement or damage calls.
execfile(os.path.join(ROOT,'client' if OUTBREAK_ROLE=='main' else 'ally-client','ClassicAssist','PrivateTamer','EncounterNavigation.py'),globals())
TERMINAL=('completed','dead','aborted','failed','client-failed','stopped','timeout','stationary-window-ended','stationary-idle','frontier-window-ended')


def known_cast(name,bit,target):
    if not learned&(1<<bit):return False
    Cast(name)
    if WaitForTarget(1200):Target(int(target));log('cast',spell=name,target=int(target));return True
    log('cast_no_cursor',spell=name);return False

try:
    deadline=time.time()+120
    while time.time()<deadline:
        if Engine.Connected and Engine.Player is not None and status().GetAttribute('phase')=='ready':break
        time.sleep(.2)
    if not Engine.Connected:raise Exception('Actual saved client did not connect')
    initial=document('archmage-original.private.xml' if OUTBREAK_ROLE=='main' else 'partner-original.private.xml')
    if int(Engine.Player.Serial)!=value(initial,'serial'):raise Exception('Wrong actual saved Player')
    learned=0
    for book in initial.SelectNodes('.//*[@spellContent]'):
        if book.GetAttribute('type')=='Server.Items.Spellbook':learned|=int(book.GetAttribute('spellContent'))
    if OUTBREAK_ROLE=='main' and not learned&(1<<4):raise Exception('No actual learned Magic Arrow')
    if value(initial,'backpack'):UseObject(value(initial,'backpack'));time.sleep(.5)
    for container in initial.SelectNodes('.//item[item]'):UseObject(value(container,'serial'));time.sleep(.15)
    bandages=[value(n,'serial') for n in initial.SelectNodes('.//item') if n.GetAttribute('type').endswith('.Bandage')]
    log('toolkit',role=OUTBREAK_ROLE,learned=learned,bandages=bandages,omissions='Main bounded standard spells/normal walking; partner real pet commands and finite bandages. Other learned families and weapon specials not automated. Relocations are fixture inputs, not learned teleport spells.')
    marker('protection-'+OUTBREAK_ROLE+'-prepared.txt','Actual saved client and owned finite toolkit audited')
    if OUTBREAK_ROLE=='main':marker('archmage-macro-prepared.txt','Actual saved main Player, normal standard spells only')
    last_cast=last_heal=last_orders=last_vet=0;previous='';phase_start=time.time();chase_target=None
    while time.time()<deadline+900 and not os.path.exists(os.path.join(EVIDENCE,'native-stop.txt')):
        observed=status();phase=observed.GetAttribute('phase')
        if phase in ('failed','complete'):break
        if phase!=previous:
            stop_walk();log('phase',phase=phase);previous=phase;phase_start=time.time();chase_target=None
        if not Engine.Connected:raise Exception('Actual client disconnected')
        if phase in ('preparing','ready','quiet-growth'):time.sleep(.2);continue
        now=time.time();player=observed.SelectSingleNode('player' if OUTBREAK_ROLE=='main' else 'partner')
        if player.GetAttribute('alive')!='True':log('actual_player_dead');break
        px=int(Engine.Player.X);py=int(Engine.Player.Y);pz=int(Engine.Player.Z)
        visible=[]
        for n in observed.SelectNodes('colony/enemy'):
            serial=value(n,'serial');mobile=Engine.Mobiles.GetMobile(int(serial))
            if mobile is None or mobile.IsDead:continue
            # Queen is an actual encounter actor but not this controlled probe's target.
            if n.GetAttribute('type')=='ArachnidOutbreakChampion':continue
            visible.append((max(abs(int(mobile.X)-px),abs(int(mobile.Y)-py)),serial,mobile))
        visible.sort(key=lambda row:row[0])
        if OUTBREAK_ROLE=='main':
            if (value(player,'hits')*100<value(player,'hitsMax')*70 or document('archmage-state.xml').GetAttribute('poisoned')=='True') and now-last_heal>3 and value(player,'mana')>=6:
                own=document('archmage-state.xml');poisoned=own.GetAttribute('poisoned')=='True'
                known_cast('Cure' if poisoned else 'Heal',10 if poisoned else 3,int(Engine.Player.Serial));last_heal=last_cast=now
            if phase=='approach':
                waypoint=observed.SelectSingleNode('waypoint');navigate(value(waypoint,'x'),value(waypoint,'y'),value(waypoint,'z'),1,now)
            elif phase=='chase':
                if chase_target is None and visible and now-last_cast>3:
                    chase_target=visible[0][1];known_cast('Magic Arrow',4,chase_target);last_cast=now
                    marker('protection-main-attack.txt','Actual client targeted a visible enemy; no server damage injection')
                if chase_target is not None and now-phase_start>2:navigate(2440,2088,pz,1,now)
            elif phase=='teleport-combat' and visible and now-last_cast>5 and value(player,'mana')>=4:
                known_cast('Magic Arrow',4,visible[0][1]);last_cast=now
        else:
            if phase=='separated' and visible and now-last_orders>7:
                Msg('all kill',0)
                if WaitForTarget(1200):Target(int(visible[0][1]));log('pet_order',enemy=int(visible[0][1]))
                last_orders=now
            if bandages and now-last_vet>10:
                for pet in observed.SelectNodes('pet'):
                    if pet.GetAttribute('alive')=='True' and value(pet,'hits')*100<value(pet,'hitsMax')*60 and max(abs(px-value(pet,'x')),abs(py-value(pet,'y')))<=1:
                        use(bandages[0],value(pet,'serial'));last_vet=now;break
        time.sleep(.25)
    stop_walk();log('finished',phase=status().GetAttribute('phase'));marker('protection-'+OUTBREAK_ROLE+'-finished.txt','Bounded native protection client ended')
except Exception as error:
    marker('protection-'+OUTBREAK_ROLE+'-error.txt',str(error));log('client_error',error=str(error));raise
