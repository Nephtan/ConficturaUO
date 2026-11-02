import os,time,json,clr
clr.AddReference('System.Xml');clr.AddReference('ClassicAssist')
from System import Array,Object,Int32
from System.Reflection import BindingFlags
from System.Xml import XmlDocument
from System.Diagnostics import Process
from System.IO import FileStream,FileMode,FileAccess,FileShare
from Assistant import Engine
from ClassicAssist.UO.Data import Skills as ClientSkills
client=os.path.dirname(str(Process.GetCurrentProcess().MainModule.FileName))
config=json.load(open(os.path.join(client,'ClassicAssist','polish-role.private.json')))
e=os.path.join(config['root'],'natural','evidence');role=config['role']
def log(event,**data):
    data.update(event=event,epoch=time.time(),role=role)
    with open(os.path.join(e,'safe-api-preflight-'+role+'.jsonl'),'a') as out:out.write(json.dumps(data)+'\n')
def state():
    stream=FileStream(os.path.join(e,'native-state.xml'),FileMode.Open,FileAccess.Read,FileShare.ReadWrite|FileShare.Delete)
    try:doc=XmlDocument();doc.XmlResolver=None;doc.Load(stream)
    finally:stream.Dispose()
    root=doc.DocumentElement;return root,root.SelectSingleNode("actor[@role='"+role+"']")
def check_intro():
    root,actor=state()
    if actor.GetAttribute('caseId')!='0' or actor.GetAttribute('mode')!='idle' or actor.GetAttribute('state')!='intro':raise Exception('Safe preflight forbids a running or previously staged trial')
    if int(Engine.Player.Serial)!=int(config['serial']) or actor.GetAttribute('alive')!='True':raise Exception('Wrong or dead saved actor')
    return actor
def point(root,name):
    node=root.SelectSingleNode(name);return tuple(int(node.GetAttribute(n)) for n in ('x','y','z'))
def actual():return (int(Engine.Player.X),int(Engine.Player.Y),int(Engine.Player.Z))
def walk_to(target):
    accepted=bool(Pathfind(target[0],target[1],target[2],True,0));until=time.time()+5
    while actual()!=target and time.time()<until:Pause(100)
    if actual()!=target:raise Exception('Native safe-area movement failed: '+str(actual())+' expected '+str(target))
    log('native_safe_step_arrived',target=target,accepted=accepted)
try:
    if os.path.normcase(client)!=os.path.normcase(config['client']) or role!='bard':raise Exception('Exact owned bard client required')
    check_intro();log('started_safe',scope='No encounter staging, combat target or stat/resource grant')
    # Deliberate failure path is caught locally before any client action.
    try:raise RuntimeError('SAFE_PREFLIGHT_SENTINEL')
    except RuntimeError as error:log('controlled_error_bounded',error=str(error),caseId=check_intro().GetAttribute('caseId'))
    manifest=XmlDocument();manifest.XmlResolver=None;manifest.Load(os.path.join(e,'bard-saved.private.xml'))
    nutrition=json.load(open(os.path.join(e,'food-readiness.private.json')))[role]
    carried=list(manifest.DocumentElement.SelectNodes('carried'));equipped=list(manifest.DocumentElement.SelectNodes('equipped'))
    def attr(node,name):return int(node.GetAttribute(name))
    backpack=next(n for n in equipped if n.GetAttribute('type')=='Server.Items.Backpack')
    by_serial=dict((attr(n,'serial'),n) for n in carried+equipped)
    wanted=[n for n in carried if n.GetAttribute('type')=='Server.Items.Bandage' or attr(n,'serial') in [int(f['serial']) for f in nutrition['foods']]]
    UseObject(attr(backpack,'serial'),True);Pause(1200)
    ancestors={}
    for item in wanted:
        chain=[];parent=attr(item,'parent');seen=set()
        while parent in by_serial and parent!=attr(backpack,'serial'):
            if parent in seen:raise Exception('Owned inventory parent cycle')
            seen.add(parent);node=by_serial[parent];chain.append(node);parent=attr(node,'parent')
        for depth,node in enumerate(reversed(chain)):ancestors[attr(node,'serial')]=(depth,node)
    for depth,node in sorted(ancestors.values(),key=lambda pair:pair[0]):UseObject(attr(node,'serial'),True);Pause(1200)
    supplies=[]
    for item in wanted:
        visible=Engine.Items.GetItem(attr(item,'serial'))
        if visible is None:raise Exception('Finite owned supply not visible: '+item.GetAttribute('type'))
        supplies.append(dict(serial=attr(item,'serial'),type=item.GetAttribute('type'),amount=int(visible.Count)))
    log('finite_supplies_verified',items=supplies)
    for food in nutrition['foods'][:1]:UseObject(int(food['serial']),True);Pause(1000);log('ordinary_owned_food_use',serial=int(food['serial']))
    bandage=next(n for n in wanted if n.GetAttribute('type')=='Server.Items.Bandage')
    UseObject(attr(bandage,'serial'),True)
    if not WaitForTarget(2500):raise Exception('Actual bandage produced no target cursor')
    Target('self');Pause(12500);check_intro();log('ordinary_owned_bandage_self_use',serial=attr(bandage,'serial'))
    instrument=next(n for n in manifest.DocumentElement.SelectNodes('equipped') if n.GetAttribute('type')=='Server.Items.Trumpet')
    serial=int(instrument.GetAttribute('serial'))
    if Engine.Items.GetItem(serial) is None:raise Exception('Actual equipped trumpet missing in client cache')
    log('actual_equipped_tool_visible',serial=serial,type=instrument.GetAttribute('type'))
    field=clr.GetClrType(ClientSkills).GetField('_lazySkillData',BindingFlags.Static|BindingFlags.NonPublic)
    lazy=field.GetValue(None);table=lazy.GetType().GetProperty('Value').GetValue(lazy,None)
    entry=table.GetType().GetProperty('Item').GetValue(table,Array[Object]([Int32(15)]))
    namefield=entry.GetType().GetField('Name',BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic)
    name=str(namefield.GetValue(entry)) if namefield is not None else str(entry.GetType().GetProperty('Name').GetValue(entry,None))
    log('actual_skill_mapping',skillID=15,clientName=name)
    UseObject(serial,True);Pause(1400);UseSkill(name)
    if not WaitForTarget(2500):raise Exception('Actual skill request produced no target cursor')
    instrument_prompt=bool(InJournal('What instrument shall you play'))
    if instrument_prompt:
        Target(serial)
        if not WaitForTarget(2500):raise Exception('Instrument selection produced no song target')
    log('actual_skill_cursor',instrumentPrompt=instrument_prompt,songPrompt=bool(InJournal('Choose the target for your song')))
    Target('self');Pause(500)
    if WaitForTarget(50):raise Exception('Target cursor remained pending after harmless self target')
    doc=XmlDocument();doc.XmlResolver=None;doc.Load(os.path.join(e,'safe-native-step.xml'))
    start=point(doc.DocumentElement,'start');step=point(doc.DocumentElement,'step')
    if actual()!=start:raise Exception('Saved safe-area position changed before movement preflight')
    walk_to(step);walk_to(start);check_intro()
    log('passed',caseId='0',scope='Actual equipped tool, skill cursor, harmless target, one-step native movement/return and bounded controlled error; no combat exposure')
    with open(os.path.join(e,'safe-client-preflight-bard-passed.txt'),'w') as out:out.write('PASS: safe native API and bounded error preflight; no encounter case begun')
except Exception as error:
    log('failed_closed',error=str(error),scope='No permission to stage any encounter case')
    with open(os.path.join(e,'safe-client-preflight-bard-failed.txt'),'w') as out:out.write(str(error))
    raise
