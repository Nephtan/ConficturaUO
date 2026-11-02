# tuning_native_driver_v2: finite actual saved tools, no resource grants.
import os,time,json,sys,clr,heapq
clr.AddReference('System.Xml')
clr.AddReference('ClassicAssist')
from System import Action,Array,Object,Int32
from System.Xml import XmlDocument
from System.IO import FileStream,FileMode,FileAccess,FileShare
from System.Diagnostics import Process
from Assistant import Engine
from ClassicAssist.UO.Data import Skills as ClientSkills
from System.Reflection import BindingFlags

# execfile does not consistently supply the loaded file's __file__; bind to the actual copied process.
CLIENT=os.path.dirname(str(Process.GetCurrentProcess().MainModule.FileName))
CONFIG=json.load(open(os.path.join(CLIENT,'ClassicAssist','polish-role.private.json')))
ROOT=CONFIG['root'];ROLE=CONFIG['role'];SERIAL=int(CONFIG['serial']);EVIDENCE=os.path.join(ROOT,'natural','evidence')
last_heartbeat=0
if os.path.normcase(CLIENT)!=os.path.normcase(CONFIG['client']):raise Exception('Wrong copied client')
nutrition_path=os.path.join(EVIDENCE,'food-readiness.private.json')
nutrition=json.load(open(nutrition_path)).get(ROLE,{}) if os.path.exists(nutrition_path) else {}
pathfinder=Engine.ClassicAssembly.GetType('ClassicUO.Game.Pathfinder');walk=pathfinder.GetMethod('WalkTo')
archmage_loaded=False
navigation=None
route_goal=None
route=[]
route_index=0

def log(event,**values):
    values.update(event=event,epoch=time.time(),role=ROLE)
    with open(os.path.join(EVIDENCE,'client-'+ROLE+'.jsonl'),'a') as output:output.write(json.dumps(values)+'\n')
def read():
    global last_heartbeat
    # Atomic server replacements can briefly deny an open despite shared-read/delete access.
    # Preserve the unchanged macro's state across that external observation race.
    for attempt in range(20):
        stream=None
        try:
            stream=FileStream(os.path.join(EVIDENCE,'native-state.xml'),FileMode.Open,FileAccess.Read,FileShare.ReadWrite|FileShare.Delete)
            document=XmlDocument();document.XmlResolver=None;document.Load(stream);root=document.DocumentElement
            if time.time()-last_heartbeat>=1:
                with open(os.path.join(EVIDENCE,'driver-heartbeat-'+ROLE+'.txt'),'w') as output:output.write(str(SERIAL))
                last_heartbeat=time.time()
            return root,root.SelectSingleNode("actor[@role='"+ROLE+"']")
        except:
            if attempt==19:raise
            time.sleep(.05)
        finally:
            if stream is not None:stream.Dispose()
def value(node,name,default=0):
    text=node.GetAttribute(name);return int(text) if text else default
def bard_skill_name():
    # Read the existing client table used by UseSkill; do not invent a display-name alias.
    field=clr.GetClrType(ClientSkills).GetField('_lazySkillData',BindingFlags.Static|BindingFlags.NonPublic)
    lazy=field.GetValue(None)
    table=lazy.GetType().GetProperty('Value').GetValue(lazy,None)
    entry=table.GetType().GetProperty('Item').GetValue(table,Array[Object]([Int32(15)]))
    name=entry.GetType().GetField('Name',BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic)
    return str(name.GetValue(entry)) if name is not None else str(entry.GetType().GetProperty('Name').GetValue(entry,None))
def distance(x,y):return max(abs(int(Engine.Player.X)-int(x)),abs(int(Engine.Player.Y)-int(y)))
def walking(goal):
    global navigation,route_goal,route,route_index
    origin=(int(Engine.Player.X),int(Engine.Player.Y),int(Engine.Player.Z));start=origin[:2];target=(int(goal[0]),int(goal[1]))
    if max(abs(target[0]-start[0]),abs(target[1]-start[1]))<=1:return
    if navigation is None:
        supplied=json.load(open(os.path.join(EVIDENCE,'navigation-grid.json')))
        navigation=dict(((int(n[0]),int(n[1])),(int(n[2]),int(n[3]))) for n in supplied['nodes'])
        log('supplied_terrain_route_loaded',nodes=len(navigation),scope=supplied['scope'])
    if target not in navigation:
        nearby=[(max(abs(p[0]-target[0]),abs(p[1]-target[1])),p) for p in navigation if max(abs(p[0]-target[0]),abs(p[1]-target[1]))<=2]
        if not nearby:
            if max(abs(target[0]-start[0]),abs(target[1]-start[1]))<=24:
                accepted=bool(Pathfind(int(goal[0]),int(goal[1]),int(goal[2]),True,1))
                log('ordinary_local_walk',origin=origin,goal=goal,accepted=accepted,scope='Short native path outside supplied geometry grid; no arrival implied')
            else:log('route_unavailable',origin=origin,goal=goal,reason='Outside supplied legal terrain and native short-route bound')
            return
        target=min(nearby)[1]
    nearest=min(range(route_index,min(len(route),route_index+40)),key=lambda i:max(abs(route[i][0]-start[0]),abs(route[i][1]-start[1]))) if route_index<len(route) else None
    if route_goal!=target or nearest is None or max(abs(route[nearest][0]-start[0]),abs(route[nearest][1]-start[1]))>2:
        if start not in navigation:log('route_unavailable',origin=origin,goal=goal,reason='Actual player tile outside supplied terrain');return
        frontier=[(0,0,start)];cost={start:0};parent={};directions=((0,-1),(1,-1),(1,0),(1,1),(0,1),(-1,1),(-1,0),(-1,-1));visited=0
        found=False
        while frontier and visited<100000:
            priority,spent,point=heapq.heappop(frontier)
            if spent!=cost.get(point):continue
            visited+=1
            if point==target:found=True;break
            mask=navigation[point][1]
            for i,delta in enumerate(directions):
                if not mask&(1<<i):continue
                nxt=(point[0]+delta[0],point[1]+delta[1]);new=spent+(14 if i%2 else 10)
                if nxt in navigation and new<cost.get(nxt,1000000000):
                    cost[nxt]=new;parent[nxt]=point;dx=abs(nxt[0]-target[0]);dy=abs(nxt[1]-target[1]);heuristic=10*max(dx,dy)+4*min(dx,dy)
                    heapq.heappush(frontier,(new+heuristic,new,nxt))
        if not found:route=[];route_goal=None;log('route_unavailable',origin=origin,goal=goal,visited=visited,reason='No copied-terrain path');return
        route=[target]
        while route[-1]!=start:route.append(parent[route[-1]])
        route.reverse();route_index=0;route_goal=target;nearest=0
        log('supplied_route_planned',origin=origin,goal=goal,tiles=len(route),visited=visited)
    route_index=nearest
    waypoint=route[min(len(route)-1,route_index+14)]
    request=(waypoint[0],waypoint[1],navigation[waypoint][0])
    # Public macro command queues native WalkTo itself. Never call it from TickWorkQueue.
    if Engine.Connected and int(Engine.Player.Serial)==SERIAL and not Engine.Player.IsDead:
        accepted=bool(Pathfind(request[0],request[1],request[2],True,1))
        log('ordinary_walk',origin=origin,goal=goal,waypoint=request,accepted=accepted)
def done(view,outcome):
    key=view.GetAttribute('caseId')
    marker=os.path.join(EVIDENCE,'done-'+ROLE+'-'+key+'.txt')
    if os.path.exists(marker):return
    with open(marker,'w') as output:output.write(outcome)
    log('case_finished',mode=view.GetAttribute('mode'),outcome=outcome)
def saved_items():
    document=XmlDocument();document.XmlResolver=None;document.Load(os.path.join(EVIDENCE,ROLE+'-saved.private.xml'))
    return list(document.DocumentElement.SelectNodes('carried'))+list(document.DocumentElement.SelectNodes('equipped'))

def available(item):
    return Engine.Items.GetItem(value(item,'serial')) is not None

def load_inventory():
    document=XmlDocument();document.XmlResolver=None;document.Load(os.path.join(EVIDENCE,ROLE+'-saved.private.xml'))
    root=document.DocumentElement;items=list(root.SelectNodes('carried'));by_serial=dict((value(n,'serial'),n) for n in items+list(root.SelectNodes('equipped')))
    backpack=next((n for n in root.SelectNodes('equipped') if n.GetAttribute('type')=='Server.Items.Backpack'),None)
    if backpack is None:raise Exception('Saved backpack missing')
    UseObject(value(backpack,'serial'),True);Pause(1200)
    food_serials=set(int(n['serial']) for n in nutrition.get('foods',[])+nutrition.get('drinks',[]))
    wanted=[n for n in items if value(n,'serial') in food_serials or any(part in n.GetAttribute('type') for part in ('Bandage','Potion','JewelImmortality','Sapphire','Quill','SpellPage','ArcanePage','Reagent','Bolt','Arrow','Trumpet','Lute','Harp','Drum','Tambourine','Book'))]
    ancestors={}
    for item in wanted:
        chain=[];parent=value(item,'parent');seen=set()
        while parent in by_serial and parent!=value(backpack,'serial'):
            if parent in seen:raise Exception('Saved inventory parent cycle')
            seen.add(parent);node=by_serial[parent]
            if node.GetAttribute('container')!='True':raise Exception('Saved inventory ancestor is not a container')
            chain.append(node);parent=value(node,'parent')
        for depth,node in enumerate(reversed(chain)):ancestors[value(node,'serial')]=(depth,node)
    for depth,node in sorted(ancestors.values(),key=lambda pair:pair[0]):
        if not available(node):raise Exception('Owned supply container not client-visible after opening parent')
        UseObject(value(node,'serial'),True);Pause(1200);log('ordinary_supply_container_requested',serial=value(node,'serial'))
    missing=[value(n,'serial') for n in wanted if not available(n)]
    visibility_deadline=time.time()+10
    while missing and time.time()<visibility_deadline:
        Pause(200);missing=[value(n,'serial') for n in wanted if not available(n)]
    if missing:raise Exception('Finite saved supplies not client-visible: '+str(missing))
    with open(os.path.join(EVIDENCE,'inventory-visible-'+ROLE+'.json'),'w') as output:
        json.dump(dict(role=ROLE,visibleSupplies=[dict(serial=value(n,'serial'),type=n.GetAttribute('type'),savedAmount=value(n,'amount')) for n in wanted],scope='Read actual client item cache after ordinary owned-container opens; no item use or grants'),output)
    log('actual_inventory_visible',supplies=len(wanted),containers=len(ancestors))
def prepare():
    if ROLE!='veteran':
        used=0
        for food in nutrition.get('foods',[]):
            for unit in range(min(3-used,int(food['amount']))):
                serial=int(food['serial'])
                if Engine.Items.GetItem(serial) is None:break
                UseObject(serial,True);Pause(1000);used+=1;log('ordinary_saved_food_use',serial=serial,savedFill=int(food['fill']))
            if used>=3:break
        for drink in nutrition.get('drinks',[])[:1]:
            serial=int(drink['serial'])
            if Engine.Items.GetItem(serial) is not None:
                UseObject(serial,True)
                if WaitForTarget(1800):Target('self')
                Pause(1000);log('ordinary_saved_drink_attempt',serial=serial,content=drink['drink'])
    if ROLE=='novice':
        for item in saved_items():
            if item.GetAttribute('type')=='Server.Items.Dagger':EquipItem(value(item,'serial'),'OneHanded');Pause(800);log('equip_existing_dagger_requested',serial=value(item,'serial'));break
    if ROLE=='ranger':
        Msg('all follow me');Pause(1500);log('ordinary_follow_command')
        for item in saved_items():
            if item.GetAttribute('type')=='Server.Items.WoodlandProtectionPotion':
                UseObject(value(item,'serial'));Pause(2000);log('ordinary_woodland_protection_attempt',serial=value(item,'serial'));break
def healing(view):
    global last_heal,last_bandage
    if ROLE=='veteran' and '_emergency_recovery' in globals():
        _emergency_recovery()
        return
    if ROLE in ('novice','journeyman','archer','bard','hybrid') and time.time()-last_bandage>=12 and (view.GetAttribute('poisoned')=='True' or value(view,'hits')*100<85*value(view,'hitsMax')):
        bandage=next((n for n in saved_items() if n.GetAttribute('type')=='Server.Items.Bandage' and available(n)),None)
        if bandage is not None:
            UseObject(value(bandage,'serial'),True)
            if WaitForTarget(1500):Target('self');log('ordinary_self_bandage',serial=value(bandage,'serial'))
            last_bandage=time.time()
    if time.time()-last_heal<10:return
    items=saved_items()
    if view.GetAttribute('poisoned')=='True':
        for kind in ('GreaterCurePotion','CurePotion','LesserCurePotion'):
            for item in items:
                if item.GetAttribute('type')=='Server.Items.'+kind and available(item):
                    UseObject(value(item,'serial'));last_heal=time.time();log('ordinary_cure_potion',serial=value(item,'serial'));return
    if value(view,'hits')*100>=70*value(view,'hitsMax'):return
    for kind in ('HerbalHealingPotion','GreaterHealPotion','HealPotion','LesserHealPotion'):
        for item in items:
            if item.GetAttribute('type')=='Server.Items.'+kind and available(item):
                UseObject(value(item,'serial'))
                if kind=='HerbalHealingPotion' and WaitForTarget(2500):Target('self')
                last_heal=time.time();log('ordinary_healing_potion',serial=value(item,'serial'));return
    if ROLE in ('intermediate','hybrid') and value(view,'tithing')>=10 and any('Paladinbook' in n.GetAttribute('type') or n.GetAttribute('type')=='Server.Items.BookOfChivalry' for n in items):
        Cast('Close Wounds')
        if WaitForTarget(1500):Target('self')
        last_heal=time.time();log('ordinary_close_wounds_attempt')

def veteran(preparation=False):
    global archmage_loaded
    original_pause,original_enemy,original_message=Pause,GetEnemy,SysMessage
    def enemy(*args):
        root,view=read()
        if preparation:
            if int(globals().get('state',{}).get('next_maintenance_at',0))>0:
                globals()['CASM_TEST_MAX_LOOPS']=1
                return False
            return original_enemy(*args)
        if view.GetAttribute('state')!='running' or view.GetAttribute('mode')!='veteran-combat':
            globals()['CASM_TEST_MAX_LOOPS']=1
            globals().get('state',{})['next_maintenance_at']=int(time.time()*1000)+3600000
            return False
        for companion in view.SelectNodes('companion'):
            serial=value(companion,'serial')
            if not InIgnoreList(serial):IgnoreObject(serial);globals().get('excluded_serials',set()).add(serial)
        return original_enemy(*args)
    last=[0]
    def pause(milliseconds):
        root,view=read()
        if view.GetAttribute('state')=='running' and int(milliseconds)<=300 and time.time()-last[0]>.8 and all(view.GetAttribute(n)=='False' for n in ('casting','frozen','paralyzed')):
            qx,qy=value(root,'queenX'),value(root,'queenY')
            if distance(qx,qy)>9 and value(view,'hits')*100>=60*value(view,'hitsMax'):
                walking((qx,qy,int(Engine.Player.Z)));last[0]=time.time()
        return original_pause(milliseconds)
    def diagnostic(message,*args):log('archmage_diagnostic',message=str(message))
    globals()['Pause']=pause;globals()['GetEnemy']=enemy;globals()['SysMessage']=diagnostic
    globals()['CASM_TEST_ENABLE_DIAGNOSTICS']=True;globals()['CASM_TEST_MAX_LOOPS']=10000
    globals()['CASM_TEST_VALIDATE_TARGETS']=True;globals()['CASM_TEST_FORCE_QUIET_LOOKUP']=True
    sys.path.insert(0,os.path.join(CLIENT,'ClassicAssist','PrivateArchmage'))
    try:
        if not archmage_loaded:
            execfile(os.path.join(CLIENT,'ClassicAssist','PrivateArchmage','archmage-overlord.py'),globals());archmage_loaded=True
        else:_run()
    finally:globals()['Pause']=original_pause;globals()['GetEnemy']=original_enemy;globals()['SysMessage']=original_message

try:
    case=None;goal_index=0;patrol_index=0;last_walk=0;last_attack=0;last_pet_command=0;last_heal=0;last_bandage=0;last_bard=0;last_pet_heal=0;last_warning_count=0;polish_prepared=False;role_prepared=os.path.exists(os.path.join(EVIDENCE,ROLE+'-preparation-commands.txt'));natural_start_epoch=None;inventory_ready=False
    log('driver_started',scope='Actual client packets and ordinary pathfinder; supplied geometry/targets; no server damage, teleport or resource grants')
    if ROLE=='bard':log('bard_skill_mapping_readonly',skillID=15,clientName=bard_skill_name())
    while Engine.Connected and not os.path.exists(os.path.join(EVIDENCE,'client-stop.txt')):
        Pause(200)
        if int(Engine.Player.Serial)!=SERIAL:raise Exception('Wrong active saved Player')
        if Engine.Player.IsDead:log('actual_player_dead');break
        root,view=read()
        if view is None:continue
        if root.GetAttribute('state')=='failed':raise Exception('Native fixture failed; stop actions and preserve its first error')
        if os.path.exists(os.path.join(EVIDENCE,'inject-safe-driver-error-'+ROLE+'.txt')):
            if view.GetAttribute('caseId')!='0':raise Exception('Failure sentinel is safe-intro only')
            raise RuntimeError('SAFE_DRIVER_ABORT_SENTINEL')
        if not inventory_ready and os.path.exists(os.path.join(EVIDENCE,'inventory-only-'+ROLE+'.txt')):
            load_inventory();inventory_ready=True
        if not role_prepared and os.path.exists(os.path.join(EVIDENCE,'prepare-'+ROLE+'.txt')) and ROLE!='veteran':
            prepare();role_prepared=True
            with open(os.path.join(EVIDENCE,ROLE+'-preparation-commands.txt'),'w') as output:output.write('Ordinary preparation commands submitted. Inspect native result and inventory; no granted resources.')
        if ROLE=='veteran' and not polish_prepared and os.path.exists(os.path.join(EVIDENCE,'prepare-veteran.txt')):
            veteran(True);polish_prepared=True
            if globals().get('state',{}).get('combat_blocked',True):log('normal_maintenance_incomplete',reason='Original macro safety blocked; no guard bypass')
            else:
                with open(os.path.join(EVIDENCE,'veteran-prepared.txt'),'w') as output:output.write('Original macro maintenance completed through native client; combat scan blocked after maintenance.')
                log('normal_maintenance_complete')
        if view.GetAttribute('state')!='running':
            prepared=polish_prepared and os.path.exists(os.path.join(EVIDENCE,'veteran-prepared.txt')) if ROLE=='veteran' else role_prepared
            if prepared and inventory_ready:
                with open(os.path.join(EVIDENCE,'driver-ready-'+ROLE+'.txt'),'w') as output:output.write(str(SERIAL))
            if ROLE=='veteran' and archmage_loaded:healing(view)
            elif role_prepared and inventory_ready:healing(view)
            continue
        current=view.GetAttribute('caseId');mode=view.GetAttribute('mode')
        if current!=case:
            case=current;goal_index=0;patrol_index=0;seen_warning=False;warning_at=0;natural_start_epoch=None
            if not role_prepared:prepare();role_prepared=True
            log('case_started',mode=mode,case=case)
            if ROLE=='bard':log('bard_skill_mapping',skillID=15,clientName=bard_skill_name())
        goals=[(value(n,'x'),value(n,'y'),value(n,'z')) for n in view.SelectNodes('goal')]
        if mode!='veteran-combat':healing(view)
        if ROLE=='ranger' and time.time()-last_pet_heal>=7:
            for companion in view.SelectNodes('companion'):
                if value(companion,'hits')>0 and value(companion,'hits')*100<60*value(companion,'hitsMax') and int(Distance(value(companion,'serial')))<=2:
                    bandage=next((n for n in saved_items() if n.GetAttribute('type')=='Server.Items.Bandage' and available(n)),None)
                    if bandage is not None:
                        UseObject(value(bandage,'serial'))
                        if WaitForTarget(1500):Target(value(companion,'serial'));log('ordinary_veterinary_bandage',serial=value(companion,'serial'))
                        last_pet_heal=time.time()
                        break
        if mode=='veteran-combat':
            veteran();continue
        if mode in ('build-combat','bard-support','frontier-combat'):
            if mode=='frontier-combat' and (time.time()-((int(case)-621355968000000000)/10000000.0))<float(CONFIG.get('passiveSeconds',5 if ROLE=='novice' else 0)):
                continue
            if mode=='frontier-combat' and view.GetAttribute('targetRetired')=='True':
                if goals and distance(goals[-1][0],goals[-1][1])>3:
                    if time.time()-last_walk>=.8:walking(goals[-1]);last_walk=time.time()
                    continue
                Msg('[OutbreakClaim');Pause(1000);done(view,'Reclaimed target weak frontier cell; ordinary reward collection requested');continue
            enemies=[]
            for enemy in root.SelectNodes('enemy'):
                serial=value(enemy,'serial');d=distance(value(enemy,'x'),value(enemy,'y')) if ROLE=='bard' else int(Distance(serial))
                if d<=18 and (ROLE=='bard' or not Dead(serial)) and enemy.GetAttribute('egg')!='True' and (mode!='frontier-combat' or value(enemy,'tier',4)==0):enemies.append((d,serial,enemy))
            enemies.sort(key=lambda x:(x[0],x[1]))
            retreat=value(view,'hits')*100<(50 if ROLE=='novice' else 35)*value(view,'hitsMax')
            if retreat:
                goal=goals[-1]
                if distance(goal[0],goal[1])<=3:done(view,'Ordinary retreat reached the legal opening after low health');continue
            elif enemies:
                d,serial,enemy=enemies[0];goal=(value(enemy,'x'),value(enemy,'y'),value(enemy,'z'))
                if ROLE=='bard':
                    if d<=10 and time.time()-last_bard>=8:
                        # Cache the actual carried instrument through its ordinary use first.
                        instrument=next((n for n in saved_items() if any(x in n.GetAttribute('type') for x in ('Trumpet','Lute','Harp','Drum','Tambourine')) and available(n)),None)
                        if instrument is None:raise Exception('No actual client-visible saved instrument')
                        UseObject(value(instrument,'serial'),True);Pause(400)
                        skill_name=bard_skill_name()
                        log('ordinary_bard_skill_request',serial=serial,skillID=15,clientName=skill_name,instrument=value(instrument,'serial'))
                        UseSkill(skill_name)
                        if WaitForTarget(1800):Target(serial);log('ordinary_discordance_attempt',serial=serial,instrument=value(instrument,'serial'))
                        else:log('bard_target_cursor_missing',serial=serial,clientName=skill_name)
                        last_bard=time.time()
                    if d<=8:Pause(200);continue
                else:
                    reach=8 if ROLE=='archer' else 1
                    if d<=reach and time.time()-last_attack>=1:
                        Attack(serial);last_attack=time.time();log('ordinary_build_attack',serial=serial)
                        if ROLE=='archer' or d<=1:continue
            else:goal=goals[0]
            if time.time()-last_walk>=.8:walking(goal);last_walk=time.time()
            continue
        if mode=='bystander':
            if value(view,'hits')*100>=40*value(view,'hitsMax'):
                supplied_bystander=os.path.join(EVIDENCE,'bystander-route-'+ROLE+'.json')
                if os.path.exists(supplied_bystander):
                    goal=tuple(json.load(open(supplied_bystander)))
                    if distance(goal[0],goal[1])>3 and time.time()-last_walk>=.8:walking(goal);last_walk=time.time()
                continue
            goal=goals[-1]
            if distance(goal[0],goal[1])<=3:done(view,'Uninvolved bystander withdrew after ordinary damage; responseDamage is separately measured');continue
        elif mode=='natural-watch':
            natural=root.SelectSingleNode('natural')
            if natural is None:continue
            if natural_start_epoch is None:natural_start_epoch=value(natural,'epoch')
            if value(natural,'epoch')>=natural_start_epoch+2:done(view,'Observed two further ordinary-clock founder establishments; inspect trajectory/timestamps');continue
            pending=natural.SelectSingleNode("patch[@established='False'][@retired='False'][@founder]")
            focus=pending if pending is not None else natural
            fx,fy=(value(focus,'founderX'),value(focus,'founderY')) if pending is not None else (value(natural,'queenX'),value(natural,'queenY'))
            goal=(fx-16,fy-10,value(focus,'founderZ') if pending is not None else value(natural,'queenZ'))
        elif mode=='queen-migration':
            if value(root,'queenArrivals')>0:done(view,'Queen shelter arrival recorded during ordinary client observation');continue
            goal=(value(root,'queenX')-18,value(root,'queenY')-12,int(Engine.Player.Z))
        elif mode=='passage':
            while goal_index<len(goals) and distance(goals[goal_index][0],goals[goal_index][1])<=3:goal_index+=1
            if goal_index>=len(goals):done(view,'Completed ordinary passage and return; native health/damage determine safety');continue
            goal=goals[goal_index]
        else:
            anchor_path=os.path.join(EVIDENCE,'hunt-anchor-'+ROLE+'.json')
            anchor=tuple(json.load(open(anchor_path))) if mode in ('resistance','ranger-hunt') and os.path.exists(anchor_path) else None
            retreat=value(view,'hits')*100<40*value(view,'hitsMax')
            if mode in ('resistance','ranger-hunt') and value(root,'warnings')>value(view,'startWarnings') and value(root,'warnings')!=last_warning_count:
                seen_warning=True;warning_at=time.time();last_warning_count=value(root,'warnings');log('warning_observed',count=last_warning_count)
            exhausted=any(value(n,'births')>=9 for n in root.SelectNodes('responseNest'))
            if seen_warning and (ROLE!='ranger' or exhausted) and time.time()-warning_at>=14:retreat=True
            if retreat:
                if len(goals)>1:goal=goals[-1]
                else:goal=(value(view,'x')-24,value(view,'y'),value(view,'z'))
                if len(goals)>1 and distance(goal[0],goal[1])<=3:done(view,'Withdrew through ordinary movement after pressure or low health');continue
            else:
                candidates=[]
                for enemy in root.SelectNodes('enemy'):
                    serial=value(enemy,'serial')
                    if int(Distance(serial))>18 or Dead(serial):continue
                    is_egg=enemy.GetAttribute('egg')=='True'
                    if mode=='sac' and (not is_egg or serial!=value(view,'targetSerial')):continue
                    if mode!='sac' and (is_egg or value(enemy,'tier',4)>0):continue
                    if anchor is not None and max(abs(value(enemy,'x')-anchor[0]),abs(value(enemy,'y')-anchor[1]))>30:continue
                    d=distance(value(enemy,'x'),value(enemy,'y'));candidates.append((d,serial,value(enemy,'x'),value(enemy,'y'),value(enemy,'z')))
                candidates.sort()
                if mode=='sac' and view.GetAttribute('targetRetired')=='True':done(view,'Target nest is retired; correlate health-event provenance for personal kill');continue
                if candidates:
                    nearest=candidates[0];goal=(nearest[2],nearest[3],nearest[4])
                    reach=8 if ROLE=='ranger' else 1
                    if ROLE=='ranger' and nearest[0]<=3:
                        px,py=int(Engine.Player.X),int(Engine.Player.Y);goal=(px+(4 if px>=nearest[2] else -4),py+(4 if py>=nearest[3] else -4),int(Engine.Player.Z))
                        if time.time()-last_walk>=.8:walking(goal);last_walk=time.time()
                        continue
                    if nearest[0]<=reach and time.time()-last_attack>=1:
                        if ROLE=='ranger' and time.time()-last_pet_command>=4:
                            Msg('all kill')
                            if WaitForTarget(1500):Target(nearest[1]);log('ordinary_pet_attack_command',serial=nearest[1])
                            last_pet_command=time.time()
                        Attack(nearest[1]);last_attack=time.time();log('ordinary_attack',serial=nearest[1])
                        if ROLE=='ranger' or nearest[0]<=1:continue
                else:
                    patrol=goals[:-1] if len(goals)>1 else goals
                    if anchor is not None:patrol=[p for p in patrol if max(abs(p[0]-anchor[0]),abs(p[1]-anchor[1]))<=24] or [anchor]
                    if patrol:
                        if distance(patrol[patrol_index%len(patrol)][0],patrol[patrol_index%len(patrol)][1])<=3:patrol_index+=1
                        goal=patrol[patrol_index%len(patrol)]
                    else:raise Exception('No preflighted patrol/retreat route supplied')
        if time.time()-last_walk>=.8:walking(goal);last_walk=time.time()
    log('driver_ended')
except Exception as error:
    log('driver_error',error=str(error))
    with open(os.path.join(EVIDENCE,'client-'+ROLE+'-error.txt'),'w') as output:output.write(str(error))
    # The private server observes this marker independently and invalidates/rescues
    # an exposed case. It never counts that test intervention as an ordinary escape.
    raise
