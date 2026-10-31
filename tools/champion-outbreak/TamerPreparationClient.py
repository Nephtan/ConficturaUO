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
from ClassicAssist.Data.ClassicUO.Objects import Pathfinder as AssistantPathfinder

ROOT = os.path.abspath(OUTBREAK_ROOT)
EVIDENCE = os.path.join(ROOT, 'evidence')
if not os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName)).startswith(os.path.normcase(os.path.join(ROOT,'client')+os.sep)):
    raise Exception('Disposable client required')
with open(os.path.join(EVIDENCE,'active-build.txt')) as source:
    if source.read().strip()!='Sargar':raise Exception('Selected saved tamer required')
with open(os.path.join(EVIDENCE,'prepared.json')) as source:
    if json.load(source)['fixture']!='archmage-preparation':raise Exception('Wrong fixture')

LOG = os.path.join(EVIDENCE,'tamer-actions.private.jsonl')
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
def status():return document('archmage-state.xml')
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
execfile(os.path.join(ROOT,'client','ClassicAssist','PrivateTamer','EncounterNavigation.py'),globals())
TERMINAL=('completed','dead','aborted','failed','client-failed','stopped','timeout','stationary-window-ended','stationary-idle')
last_potion=last_vet=last_orders=0
target_serial=None

try:
    deadline=time.time()+120
    while time.time()<deadline:
        if Engine.Connected and Engine.Player is not None and status().GetAttribute('state')=='ready':break
        time.sleep(.2)
    if not Engine.Connected:raise Exception('Private client did not connect')
    initial=document('archmage-original.private.xml')
    if int(Engine.Player.Serial)!=value(initial,'serial'):raise Exception('Wrong copied character')
    marker('archmage-command.txt','claim-pets')
    deadline=time.time()+30
    while time.time()<deadline and not os.path.exists(os.path.join(EVIDENCE,'selected-pets-ready.txt')):time.sleep(.1)
    if not os.path.exists(os.path.join(EVIDENCE,'selected-pets-ready.txt')):raise Exception('Normal trainer claim did not complete')
    prepared=document('selected-tamer-prepared.private.xml')
    if prepared.SelectNodes('pet').Count==0:raise Exception('No actual saved cats claimed')
    for equipment in prepared.SelectNodes('equipment'):
        if equipment.GetAttribute('layer')=='Backpack':UseObject(value(equipment,'serial'));time.sleep(.4)
    if value(prepared,'backpack'):
        UseObject(value(prepared,'backpack'));time.sleep(.4)
    for container in prepared.SelectNodes('.//item[item]'):
        UseObject(value(container,'serial'));time.sleep(.2)
    supplies={'bandage':[],'heal':[],'cure':[]}
    for item in prepared.SelectNodes('.//item'):
        kind=item.GetAttribute('type').split('.')[-1];serial=value(item,'serial')
        if kind=='Bandage':supplies['bandage'].append(serial)
        elif kind in ('GreaterHealPotion','HealPotion','LesserHealPotion'):supplies['heal'].append(serial)
        elif kind in ('GreaterCurePotion','CurePotion','LesserCurePotion'):supplies['cure'].append(serial)
    visibility_deadline=time.time()+5
    while time.time()<visibility_deadline and not any(Engine.Items.GetItem(int(serial)) is not None for serial in supplies['bandage']):time.sleep(.1)
    if supplies['bandage'] and not any(Engine.Items.GetItem(int(serial)) is not None for serial in supplies['bandage']):raise Exception('Actual veterinary bandages not visible after opening saved containers; refuse incomplete toolkit start')
    log('prepared',pets=prepared.SelectNodes('pet').Count,finite_supplies=supplies)
    marker('archmage-macro-prepared.txt','Saved tamer, actual claimed cats, existing equipment and finite supplies; no grants.')
    while time.time()<deadline+180 and status().GetAttribute('state') not in ('running-route',)+TERMINAL:
        if os.path.exists(os.path.join(EVIDENCE,'native-stop.txt')):raise Exception('Stopped before combat')
        time.sleep(.1)
    start=time.time();limit=start+600
    while time.time()<limit and not os.path.exists(os.path.join(EVIDENCE,'native-stop.txt')):
        observed=status();mode=observed.GetAttribute('state')
        if mode in TERMINAL:break
        if mode!='running-route':time.sleep(.1);continue
        if not Engine.Connected:raise Exception('Client disconnected')
        now=time.time();px=value(observed,'x');py=value(observed,'y')
        visible=[]
        for enemy in observed.SelectNodes('enemy'):
            serial=value(enemy,'serial');mobile=Engine.Mobiles.GetMobile(int(serial))
            if mobile is None or mobile.IsDead:continue
            distance=max(abs(int(mobile.X)-px),abs(int(mobile.Y)-py));visible.append((distance,int(serial),mobile))
        visible.sort(key=lambda row:row[0])
        if visible and (now-last_orders>=6 or visible[0][1]!=target_serial):
            distance,serial,mobile=visible[0];Msg('all kill',0)
            if WaitForTarget(1000):Target(int(serial));log('pet_attack_order',enemy=int(serial))
            last_orders=now;target_serial=serial
        if now-last_potion>=11:
            kind='cure' if observed.GetAttribute('poisoned')=='True' else 'heal' if value(observed,'hits')*100<value(observed,'hitsMax')*65 else None
            if kind is not None:
                for serial in supplies[kind]:
                    if use(serial):last_potion=now;break
        wounded=[]
        for pet in observed.SelectNodes('pet'):
            if pet.GetAttribute('alive')!='True' and pet.GetAttribute('deadBonded')!='True':continue
            ratio=float(value(pet,'hits'))/max(1,value(pet,'hitsMax'))
            if ratio<.75 or pet.GetAttribute('poisoned')=='True' or pet.GetAttribute('deadBonded')=='True':wounded.append((ratio,pet))
        if wounded and now-last_vet>=8 and supplies['bandage']:
            last_vet=now
            wounded.sort(key=lambda row:row[0]);pet=wounded[0][1];distance=max(abs(px-value(pet,'x')),abs(py-value(pet,'y')))
            if distance>1:navigate(value(pet,'x'),value(pet,'y'),value(pet,'z'),1,now)
            else:
                stop_walk()
                for serial in supplies['bandage']:
                    if use(serial,value(pet,'serial')):break
        if visible:
            distance,serial,mobile=visible[0]
            if distance<5:
                dx=1 if px>=int(mobile.X) else -1;dy=1 if py>=int(mobile.Y) else -1
                navigate(px+dx*6,py+dy*6,value(observed,'z'),1,now)
            elif distance>10:navigate(int(mobile.X),int(mobile.Y),int(mobile.Z),8,now)
            else:
                stop_walk()
                Attack(int(serial))
        else:
            anchor=observed.SelectSingleNode("anchor[@stage='3']")
            if anchor is not None:navigate(value(anchor,'x'),value(anchor,'y'),value(anchor,'z'),8,now)
        time.sleep(.25)
    if time.time()>=limit and status().GetAttribute('state') not in TERMINAL:marker('archmage-command.txt','stop')
    log('finished',state=status().GetAttribute('state'))
    stop_walk()
    marker('archmage-macro-finished.txt','Bounded saved-tamer client observation ended; winning was not required.')
except Exception as error:
    marker('archmage-client-error.txt',str(error));log('client_error',error=str(error));raise
