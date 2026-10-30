# IronPython 2.7; two copied clients, controlled native role probes only.
import os
import json
import time
import clr
clr.AddReference('System.Xml')
from System.Diagnostics import Process
from System.IO import FileStream, FileMode, FileAccess, FileShare
from System.Xml import XmlDocument
from Assistant import Engine
from ClassicAssist.Data.Macros.Commands.ObjectCommands import UseObject
from ClassicAssist.Data.Macros.Commands.TargetCommands import Target
from ClassicAssist.Data.Macros.Commands.SpellCommands import Cast
from ClassicAssist.UO.Network.Packets import UseSkill as UseSkillPacket

ROOT=os.path.abspath(OUTBREAK_ROOT)
E=os.path.join(ROOT,'evidence')
with open(os.path.join(E,'cooperative-inputs.json')) as source:
    EXPECTED_CASES=json.load(source).get('expected_cases',['heal','cure','rescue','treat','prevent','bard'])
folder='client' if OUTBREAK_ROLE=='patient' else 'ally-client'
if os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName))!=os.path.normcase(os.path.join(ROOT,folder,'ClassicUO.exe')):
    raise Exception('Cooperative macro requires its exact private client/role')

def log(event,**values):
    values.update(event=event,epoch=time.time(),role=OUTBREAK_ROLE)
    with open(os.path.join(E,'cooperative-'+OUTBREAK_ROLE+'.jsonl'),'a') as output:output.write(json.dumps(values)+'\n')

def state():
    for attempt in range(10):
        try:
            stream=FileStream(os.path.join(E,'cooperative-state.xml'),FileMode.Open,FileAccess.Read,FileShare.ReadWrite|FileShare.Delete)
            try:
                doc=XmlDocument();doc.Load(stream);return doc.DocumentElement
            finally:stream.Close()
        except Exception:
            if attempt==9:raise
            time.sleep(.05)

def n(s,key):return int(s.GetAttribute(key))

def await_target(milliseconds):
    # WaitForTarget only waits for future packets in this copied CA version.
    # Instant skill cursors can already exist when the command returns.
    deadline=time.time()+milliseconds/1000.0
    while not Engine.TargetExists and time.time()<deadline:time.sleep(.025)
    if Engine.TargetExists:
        log('native_cursor_observed',cursor=int(Engine.TargetSerial),flags=str(Engine.TargetFlags))
        return True
    return False

def cast(spell,serial):
    Cast(spell)
    if not await_target(4000):
        log('native_spell_without_target',spell=spell)
        return
    Target(serial);log('native_spell',spell=spell,target=serial)

try:
    deadline=time.time()+120
    while not Engine.Connected and time.time()<deadline:time.sleep(.1)
    if not Engine.Connected:raise Exception('Private role connection deadline')
    log('connected');last_sequence=-1;last_action=0;patient_attacked=False;instrument_set=False
    deadline=time.time()+900
    while Engine.Connected and time.time()<deadline:
        if not os.path.exists(os.path.join(E,'cooperative-state.xml')):time.sleep(.1);continue
        s=state();phase=s.GetAttribute('phase');seq=n(s,'sequence')
        if Engine.Player is None:time.sleep(.1);continue
        expected=n(s,'patient' if OUTBREAK_ROLE=='patient' else 'helper')
        if Engine.Player.Serial!=expected:raise Exception('Connected native identity does not match declared role')
        if s.GetAttribute('patientAlive')!='True' or s.GetAttribute('helperAlive')!='True':raise Exception('Role actor died')
        if seq!=last_sequence:
            last_sequence=seq;last_action=0;patient_attacked=False;instrument_set=False;log('case_observed',phase=phase,sequence=seq)
        if phase=='complete' and seq==len(EXPECTED_CASES):
            log('all_role_cases_finished');break
        now=time.time()
        if OUTBREAK_ROLE=='patient':
            if phase=='bard' and now-last_action>10 and s.GetAttribute('pacifiedByHelper')!='True':
                last_action=now;cast('Magic Arrow',n(s,'enemy'));patient_attacked=True
        elif phase=='heal' and now-last_action>4:
            last_action=now;cast('Greater Heal',n(s,'patient'))
        elif phase=='cure' and s.GetAttribute('poisoned')=='True' and now-last_action>3:
            last_action=now;cast('Cure',n(s,'patient'))
        elif phase=='rescue' and n(s,'web') and now-last_action>1:
            last_action=now;serial=n(s,'web')
            if Engine.Items.GetItem(serial) is None:raise Exception('Ally web absent from ordinary client item memory')
            UseObject(serial,True);log('native_ally_web_use',web=serial)
        elif phase=='treat' and now-last_action>3:
            last_action=now;UseObject(n(s,'treatment'),True)
            if not await_target(3000):raise Exception('Party treatment target deadline')
            Target(n(s,'patient'));log('native_party_treatment')
        elif phase=='bard' and n(s,'enemyHits')<n(s,'enemyHitsMax') and s.GetAttribute('pacifiedByHelper')!='True' and now-last_action>10.5:
            last_action=now
            if not instrument_set:
                UseObject(n(s,'instrument'),True);time.sleep(.3);instrument_set=True
            Engine.SendPacketToServer(UseSkillPacket(9))
            log('native_skill_packet',skill_id=9)
            if not await_target(3000):
                log('native_bard_without_target');continue
            Target(n(s,'enemy'));log('native_peacemaking')
        time.sleep(.1)
    log('client_loop_finished')
except Exception as error:
    log('failed',error=str(error))
    with open(os.path.join(E,'cooperative-'+OUTBREAK_ROLE+'-error.txt'),'w') as output:output.write(str(error))
    raise
