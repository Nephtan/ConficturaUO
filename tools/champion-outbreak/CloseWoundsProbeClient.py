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

LOG = os.path.join(EVIDENCE,'healing-probe-actions.private.jsonl')
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


try:
    deadline=time.time()+120
    while time.time()<deadline:
        if Engine.Connected and Engine.Player is not None and status().GetAttribute('state')=='ready':break
        time.sleep(.2)
    original=document('archmage-original.private.xml')
    if int(Engine.Player.Serial)!=value(original,'serial') or value(original,'karma')<0 or value(original,'tithing')<10:raise Exception('Actual saved chivalry eligibility missing')
    marker('archmage-macro-prepared.txt','Actual saved vulnerable fighter; stationary Close Wounds toolkit probe, no grants')
    deadline=time.time()+120
    while time.time()<deadline and document('healing-probe-state.xml').GetAttribute('phase')=='waiting':time.sleep(.2)
    attempts=0
    while attempts<5 and document('healing-probe-state.xml').GetAttribute('phase')=='running':
        attempts+=1;Cast('Close Wounds');log('cast_attempt',spell='Close Wounds',attempt=attempts)
        if WaitForTarget(4000):Target(int(Engine.Player.Serial));log('target_sent',spell='Close Wounds')
        else:log('cursor_not_observed',spell='Close Wounds')
        time.sleep(3)
    log('finished',phase=document('healing-probe-state.xml').GetAttribute('phase'))
except Exception as error:
    marker('archmage-client-error.txt',str(error));log('client_error',error=str(error));raise
