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
if not os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName)).startswith(os.path.normcase(os.path.join(ROOT,'client')+os.sep)):
    raise Exception('Disposable client required')
with open(os.path.join(EVIDENCE,'saved-build-policy.txt')) as source:POLICY=source.read().strip()
with open(os.path.join(EVIDENCE,'prepared.json')) as source:
    if json.load(source)['fixture']!='archmage-preparation':raise Exception('Wrong fixture')

LOG = os.path.join(EVIDENCE,'saved-build-actions.private.jsonl')
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
TERMINAL=('completed','dead','aborted','failed','client-failed','stopped','timeout','stationary-window-ended','stationary-idle','frontier-window-ended')

def cast_known(name,bit,target):
    if not learned&(1<<bit):return False
    log('cast_attempt',spell=name,target=int(target));Cast(name)
    if WaitForTarget(4000):Target(int(target));log('cast',spell=name,target=int(target));return True
    log('cast_cursor_not_observed',spell=name);return False

try:
    deadline=time.time()+120
    while time.time()<deadline:
        if Engine.Connected and Engine.Player is not None and status().GetAttribute('state')=='ready':break
        time.sleep(.2)
    if not Engine.Connected:raise Exception('Copied client did not connect')
    initial=document('archmage-original.private.xml')
    if int(Engine.Player.Serial)!=value(initial,'serial'):raise Exception('Wrong saved Player')
    if initial.GetAttribute('access')!='Player' or initial.GetAttribute('blessed')!='False':raise Exception('Ineligible actual saved Player')
    learned=0
    for book in initial.SelectNodes('.//*[@spellContent]'):
        if book.GetAttribute('type')=='Server.Items.Spellbook':learned|=int(book.GetAttribute('spellContent'))
    if POLICY=='mage' and not learned&(1<<4):raise Exception('No learned Magic Arrow; do not invent toolkit')
    if value(initial,'backpack'):UseObject(value(initial,'backpack'));time.sleep(.5)
    for container in initial.SelectNodes('.//item[item]'):UseObject(value(container,'serial'));time.sleep(.2)
    supplies={'bandage':[],'heal':[],'cure':[]}
    for item in initial.SelectNodes('.//item'):
        kind=item.GetAttribute('type').split('.')[-1]
        if kind=='Bandage':supplies['bandage'].append(value(item,'serial'))
        elif kind in ('GreaterHealPotion','HealPotion','LesserHealPotion'):supplies['heal'].append(value(item,'serial'))
        elif kind in ('GreaterCurePotion','CurePotion','LesserCurePotion'):supplies['cure'].append(value(item,'serial'))
    if POLICY=='melee' and supplies['bandage'] and not any(Engine.Items.GetItem(int(n)) is not None for n in supplies['bandage']):
        time.sleep(2)
        if not any(Engine.Items.GetItem(int(n)) is not None for n in supplies['bandage']):raise Exception('Finite saved healing supplies not client-visible')
    chivalry=any(n.GetAttribute('type').endswith('.BookOfChivalry') and int(n.GetAttribute('spellContent') or '0')==1023 for n in initial.SelectNodes('.//*[@spellContent]'))
    primary=None
    for n in initial.SelectNodes('equipment'):
        if n.GetAttribute('primarySkillEligible')=='True':primary=n
    if primary is not None:Msg('[SAD',0);time.sleep(.4);Msg('[Set1',0)
    log('toolkit',policy=POLICY,learned=learned,supplies=supplies,chivalry=chivalry,primary=primary.GetAttribute('primary') if primary is not None else None,omissions='Only audited standard spells, melee, eligible primary ability, owned Close Wounds and finite healing. No research/bard/other chivalry/secondary automation or human-optimality claim.')
    marker('archmage-macro-prepared.txt','Actual saved Player audited, client-visible finite toolkit; no grants.')
    deadline=time.time()+660
    while time.time()<deadline and status().GetAttribute('state') not in ('running-route',)+TERMINAL:time.sleep(.2)
    last_heal=last_cast=last_attack=last_potion=last_decision=last_special=0;start=time.time()
    while time.time()-start<210 and not os.path.exists(os.path.join(EVIDENCE,'native-stop.txt')):
        observed=status();mode=observed.GetAttribute('state')
        if mode in TERMINAL:break
        if mode!='running-route':time.sleep(.2);continue
        if not Engine.Connected:raise Exception('Client disconnected')
        now=time.time();px=int(Engine.Player.X);py=int(Engine.Player.Y);pz=int(Engine.Player.Z)
        visible=[]
        for enemy in observed.SelectNodes('enemy'):
            serial=value(enemy,'serial');mobile=Engine.Mobiles.GetMobile(int(serial))
            if mobile is None or mobile.IsDead or enemy.GetAttribute('type')=='ArachnidOutbreakChampion':continue
            visible.append((max(abs(int(mobile.X)-px),abs(int(mobile.Y)-py)),serial,mobile))
        # Prefer visibly named small frontier creatures; do not use server rank/health/location metadata for targeting.
        visible.sort(key=lambda row:(row[0]+(12 if 'guardian' in str(row[2].Name).lower() else 0),row[0]))
        nearby=sum(row[0]<=4 for row in visible);hurt=value(observed,'hits')*100<value(observed,'hitsMax')*85;poisoned=observed.GetAttribute('poisoned')=='True'
        danger=nearby>=3 or value(observed,'hits')*100<value(observed,'hitsMax')*40
        if now-last_decision>1:
            log('decision',x=px,y=py,hits=value(observed,'hits'),mana=value(observed,'mana'),visible=[{'serial':row[1],'distance':row[0],'name':str(row[2].Name)} for row in visible],nearby=nearby,danger=danger,poisoned=poisoned,frozen=observed.GetAttribute('frozen'),paralyzed=observed.GetAttribute('paralyzed'),spell=observed.GetAttribute('spell'),ability=observed.GetAttribute('currentAbility'));last_decision=now
        if (hurt or poisoned) and now-last_potion>11:
            for serial in supplies['cure' if poisoned else 'heal']:
                if use(serial):last_potion=now;break
        if (hurt or poisoned) and now-last_heal>10:
            last_heal=now
            if POLICY=='mage' and (not visible or min(row[0] for row in visible)>=4) and value(observed,'mana')>=6:
                stop_walk();cast_known('Cure' if poisoned else 'Heal',10 if poisoned else 3,int(Engine.Player.Serial));last_cast=now
            elif chivalry and not poisoned and value(observed,'mana')>=10 and value(initial,'tithing')>=10:
                stop_walk();log('chivalry_attempt',spell='Close Wounds');Cast('Close Wounds')
                if WaitForTarget(4000):Target(int(Engine.Player.Serial));log('chivalry_target',spell='Close Wounds')
                else:log('chivalry_cursor_not_observed',spell='Close Wounds')
                last_cast=now
            else:
                for serial in supplies['bandage']:
                    if use(serial,int(Engine.Player.Serial)):break
        if visible:
            distance,serial,mobile=visible[0];nearest=min(visible,key=lambda row:row[0])
            if danger or POLICY=='mage' and nearest[0]<5:
                nearest_mobile=nearest[2];dx=1 if px>=int(nearest_mobile.X) else -1;dy=1 if py>=int(nearest_mobile.Y) else -1
                navigate(px+dx*8,py+dy*8,pz,1,now)
            elif POLICY=='mage':
                if distance>10:navigate(int(mobile.X),int(mobile.Y),int(mobile.Z),8,now)
                else:
                    stop_walk()
                    if now-last_cast>3 and value(observed,'mana')>=4:
                        if value(observed,'mana')>=14 and learned&(1<<17):cast_known('Fireball',17,serial)
                        else:cast_known('Magic Arrow',4,serial)
                        last_cast=now
            else:
                if distance>7:navigate(int(mobile.X),int(mobile.Y),int(mobile.Z),7,now)
                elif distance>1:
                    # A short approach after a visible, bounded scout; retreat instead of charging a crowd.
                    navigate(int(mobile.X),int(mobile.Y),int(mobile.Z),1,now)
                else:stop_walk()
                if primary is not None and now-last_special>5 and value(observed,'mana')>=value(primary,'primaryMana'):
                    Msg('[Set1',0);last_special=now;log('primary_ability_requested',ability=primary.GetAttribute('primary'))
                if now-last_attack>2:Attack(int(serial));last_attack=now;log('attack',enemy=serial,distance=distance)
        time.sleep(.25)
    log('finished',state=status().GetAttribute('state'));stop_walk()
    marker('archmage-macro-finished.txt','Bounded actual saved-build frontier play; winning not required.')
except Exception as error:
    marker('archmage-client-error.txt',str(error));log('client_error',error=str(error));raise
