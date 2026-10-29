# IronPython 2.7, executed only by the copied ClassicAssist profile.
import os
import json
import time
import clr
clr.AddReference('System.Xml')
from System import Action, Array, Object, Int32
from System.IO import FileStream, FileMode, FileAccess, FileShare
from System.Xml import XmlDocument
from System.Diagnostics import Process
from Assistant import Engine
from ClassicAssist.Data.Macros.Commands.ActionCommands import Attack
from ClassicAssist.Data.Macros.Commands.ObjectCommands import UseObject
from ClassicAssist.Data.Macros.Commands.TargetCommands import Target, WaitForTarget
from ClassicAssist.Data.Macros.Commands.SpellCommands import Cast
from ClassicAssist.Data.Macros.Commands.MsgCommands import Msg

ROOT = os.path.abspath(OUTBREAK_ROOT)
EVIDENCE = os.path.join(ROOT, 'evidence')
if not os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName)).startswith(os.path.normcase(os.path.join(ROOT, 'client') + os.sep)):
    raise Exception('Macro is not running inside the disposable copied client')
with open(os.path.join(EVIDENCE, 'prepared.json')) as source:
    prepared = json.load(source)
if prepared.get('fixture') != 'benchmark':
    raise Exception('Not a benchmark copy')
with open(os.path.join(EVIDENCE, 'native-private-root.txt')) as source:
    if os.path.normcase(source.read().strip()) != os.path.normcase(os.path.join(ROOT, 'shard')):
        raise Exception('Private root mismatch')

LOG = os.path.join(EVIDENCE, 'benchmark-client.jsonl')
native = Engine.ClassicAssembly.GetType('ClassicUO.Game.Pathfinder')
walk = native.GetMethod('WalkTo')
walking = native.GetProperty('AutoWalking')
last_goal = None
last_walk = 0
last_target = 0
last_attack = 0
last_item = 0
last_spell = 0
last_consecrate = 0
last_enemy = 0
last_heal = 0
last_antivenom = 0
last_petals = 0
last_heartbeat = 0
warning_serial = 0
warning_seen = 0
route_stage = -1
route_index = 0
navigation_rejections = 0
probe_sequence = -1
probe_attempt = 0
probe_attack = False
probe_route_index = 0
pending_cast = None
last_bandage = last_strength = last_agility = last_special = last_dispel = 0
previous_ability = ''
retreat_requested = 0
dodge_requested = 0
boss_last_seen = None
boss_search_route = None
boss_search_index = 0
last_search_goal = None

def log(kind, **values):
    values.update(event=kind, utc_epoch=time.time())
    with open(LOG, 'a') as output:
        output.write(json.dumps(values, sort_keys=True) + '\n')

def read_state():
    stream = FileStream(os.path.join(EVIDENCE, 'benchmark-state.xml'), FileMode.Open,
                        FileAccess.Read, FileShare.ReadWrite | FileShare.Delete)
    try:
        document = XmlDocument()
        document.XmlResolver = None
        document.Load(stream)
        return document.DocumentElement
    finally:
        stream.Dispose()

def value(node, key, default=0):
    text = node.GetAttribute(key)
    return float(text) if text else default

def local_goals(start, goal):
    x,y,z=start
    distance=max(abs(goal[0]-x),abs(goal[1]-y))
    if distance<=1:return [goal]
    step=min(6,distance)
    candidates=[(x+dx*step,y+dy*step,z,1) for dx in (-1,0,1) for dy in (-1,0,1) if dx or dy]
    ordered=sorted(candidates,key=lambda p:(p[0]-goal[0])**2+(p[1]-goal[1])**2)
    return ([goal] if distance<=24 else [])+[p for p in ordered if p!=goal]

def dodge_goals(x,y,z,center_x,center_y):
    candidates=[(x-2,y,z,0),(x+2,y,z,0),(x,y-2,z,0),(x,y+2,z,0)]
    return sorted(candidates,key=lambda p:(p[0]-center_x)**2+(p[1]-center_y)**2)

def boss_reacquisition_goals(center, remembered):
    x,y,z=center
    valid=remembered is not None and max(abs(remembered[0]-x),abs(remembered[1]-y))<=56
    origin=remembered if valid else center
    grid=[(x+dx,y+dy,z,1) for dx in (-40,-16,16,40) for dy in (-40,-16,16,40)]
    grid.sort(key=lambda p:(p[0]-origin[0])**2+(p[1]-origin[1])**2)
    return ([tuple(remembered)+(1,)] if valid else [])+grid

def navigate(x, y, z, distance, now):
    global last_goal, last_walk
    goal = (int(x), int(y), int(z), int(distance))
    if goal == last_goal and (bool(walking.GetValue(None)) or now - last_walk < 2):
        return
    last_goal, last_walk = goal, now
    def issue():
        global navigation_rejections
        if Engine.Connected and not os.path.exists(os.path.join(EVIDENCE, 'native-stop.txt')):
            try:
                origin=(int(Engine.Player.X),int(Engine.Player.Y),int(Engine.Player.Z))
                accepted=False
                for actual in local_goals(origin,goal):
                    accepted = bool(walk.Invoke(None, Array[Object]([Int32(n) for n in actual])))
                    log('walk',goal=actual,requested=goal,origin=origin,accepted=accepted)
                    if accepted:break
                navigation_rejections = 0 if accepted else navigation_rejections + 1
            except Exception as error:
                log('walk_error', error=str(error))
    Engine.TickWorkQueue.Enqueue(Action(issue))

def navigate_stage(state, stage, player, now):
    global route_stage, route_index
    if stage == 0:
        anchor = state.SelectSingleNode("anchor[@stage='0']")
        navigate(value(anchor,'x'),value(anchor,'y'),value(anchor,'z'),1,now)
        return
    if route_stage != stage:
        route_stage, route_index = stage, 0
        log('route_stage', stage=stage, x=int(player.X), y=int(player.Y))
    route = state.SelectSingleNode("route[@to='%d']" % stage)
    if route is None:
        raise Exception('Missing connected short-leg route')
    steps = list(route.SelectNodes('step'))
    while route_index < len(steps):
        step = steps[route_index]
        if max(abs(int(player.X)-value(step,'x')),abs(int(player.Y)-value(step,'y'))) > 1:
            navigate(value(step,'x'),value(step,'y'),value(step,'z'),1,now)
            return
        log('route_waypoint',stage=stage,index=route_index,x=int(player.X),y=int(player.Y),
            planned_x=value(step,'x'),planned_y=value(step,'y'))
        route_index += 1

def use(supplies, key, target=False):
    node = supplies.get(key)
    if node is None or value(node, 'remaining') <= 0:
        return False
    serial = int(value(node, 'serial'))
    UseObject(serial, True)
    if target and WaitForTarget(800):
        Target(Engine.Player.Serial)
    log('use', key=key, serial=serial)
    return True

def toolkit(state):
    global probe_sequence, probe_attempt, probe_attack, probe_route_index
    sequence = int(value(state,'probeSequence'))
    name = state.GetAttribute('probe')
    now = time.time()
    if probe_sequence != sequence:
        probe_sequence, probe_attempt, probe_attack = sequence, 0, False
        probe_route_index = 0
        log('toolkit_case',sequence=sequence,name=name)
    if now-probe_attempt < 2.5:
        return
    probe_attempt = now
    supplies = dict((n.GetAttribute('key'),n) for n in state.SelectNodes('supply'))
    if name == 'regeneration':
        return
    if name == 'return':
        steps=list(state.SelectNodes('returnStep'))
        while probe_route_index < len(steps):
            step=steps[probe_route_index]
            if max(abs(int(Engine.Player.X)-value(step,'x')),abs(int(Engine.Player.Y)-value(step,'y'))) > 1:
                navigate(value(step,'x'),value(step,'y'),value(step,'z'),1,now)
                return
            log('toolkit_return_step',sequence=sequence,index=probe_route_index,x=int(Engine.Player.X),y=int(Engine.Player.Y))
            probe_route_index+=1
        return
    if name in ('strength','agility','refresh','petals','cure','bandage','heal'):
        if name == 'bandage' and state.GetAttribute('bandaging') == 'True':
            return
        use(supplies,name,name=='bandage')
        return
    if name.startswith('ability-'):
        expected = ['DoubleWhirlwindAttack','ArmorIgnore','DoubleStrike','FireStrike','DeathBlow'][int(name[-1])-1]
        if state.GetAttribute('currentAbility') != expected:
            Msg('[SAD',0)
            time.sleep(.3)
            Msg('[Set'+name[-1],0)
            log('toolkit_arm_requested',sequence=sequence,ability=expected)
        elif not probe_attack:
            log('toolkit_arm_observed',sequence=sequence,ability=expected)
            Attack(int(value(state,'probeTarget')))
            probe_attack=True
        return
    spells = {'cleanse':('Cleanse By Fire',True),'close-wounds':('Close Wounds',True),
        'consecrate':('Consecrate Weapon',False),'fury':('Divine Fury',False),
        'enemy-one':('Enemy of One',False),'holy-light':('Holy Light',False),
        'dispel':('Dispel Evil',False),'remove-curse':('Remove Curse',True),
        'journey':('Sacred Journey',True),'noble':('Noble Sacrifice',False)}
    spell,target = spells[name]
    Cast(spell)
    if target and WaitForTarget(4000):
        Target(int(value(supplies['rune'],'serial')) if name=='journey' else Engine.Player.Serial)
    log('toolkit_cast',sequence=sequence,spell=spell)

def player_cast(spell, target=None):
    global pending_cast,last_spell
    if target is not None: target=int(target)
    Cast(spell)
    last_spell=time.time()
    if target is not None:
        pending_cast=(target,last_spell+4)
    log('cast',spell=spell,target=target)

log('started', policy='v3 finite preparation; state-based buffs; survival reserve; defender priority; native retreat')
try:
    while not os.path.exists(os.path.join(EVIDENCE, 'native-stop.txt')):
        time.sleep(.25)
        if not Engine.Connected or Engine.Player is None:
            continue
        try:
            state = read_state()
        except Exception:
            continue
        if int(value(state, 'actor')) != int(Engine.Player.Serial):
            raise Exception('Connected actor differs from frozen reference')
        mode = state.GetAttribute('state')
        if mode=='mounting':
            now=time.time()
            if now-last_item>=1:
                candidate=int(value(state,'mountCandidate'))
                if candidate:
                    if Engine.Mobiles.GetMobile(candidate) is None:continue
                    UseObject(candidate,True);log('mount_request',serial=candidate)
                else:
                    supplies=dict((n.GetAttribute('key'),n) for n in state.SelectNodes('supply'))
                    cage=supplies.get('mount-cage')
                    if cage is not None and int(value(cage,'remaining'))>0:
                        UseObject(int(value(cage,'serial')),True);log('mount_cage_use',serial=int(value(cage,'serial')))
                last_item=now
            continue
        if mode == 'preparing-player':
            now=time.time()
            if now-last_item >= 2:
                supplies=dict((n.GetAttribute('key'),n) for n in state.SelectNodes('supply'))
                if value(state,'str')<150:
                    use(supplies,'strength')
                elif value(state,'dex')<150:
                    use(supplies,'agility')
                elif value(supplies.get('petals'),'remaining',10)==10:
                    use(supplies,'petals');last_petals=now
                elif value(state,'hits')<value(state,'hitsMax') and state.GetAttribute('bandaging')!='True':
                    use(supplies,'bandage',True)
                last_item=now
            continue
        if mode.startswith('toolkit-'):
            if mode not in ('toolkit-complete','toolkit-failed'):
                toolkit(state)
            continue
        if mode == 'navigation':
            now = time.time()
            if navigation_rejections >= 10 and state.GetAttribute('paralyzed') != 'True' and state.GetAttribute('frozen') != 'True':
                raise Exception('Ten native navigation requests rejected without control effect')
            if state.GetAttribute('navigationNearRecovery')=='True':
                if state.GetAttribute('navigationPinnedPassed')!='True':
                    navigate(value(state,'pinnedX'),value(state,'pinnedY'),value(state,'pinnedZ'),1,now)
                else:
                    anchor=state.SelectSingleNode("anchor[@stage='3']")
                    navigate(value(anchor,'x'),value(anchor,'y'),value(anchor,'z'),1,now)
            else:navigate_stage(state,int(value(state,'navigationStage')),Engine.Player,now)
            continue
        if not mode.startswith('running'):
            if mode in ('completed', 'dead', 'failed', 'aborted', 'stopped', 'driver-failed', 'navigation-completed'):
                log('finished', state=mode)
                break
            continue
        now = time.time()
        if pending_cast is not None:
            if WaitForTarget(10):
                Target(pending_cast[0]);log('spell_target',target=pending_cast[0]);pending_cast=None
                last_spell=now
            elif now>pending_cast[1]:
                log('spell_target_timeout');pending_cast=None
        if retreat_requested and max(abs(value(state,'x')-value(state,'refugeX')),abs(value(state,'y')-value(state,'refugeY')))<=1:
            log('retreat_arrived',x=value(state,'x'),y=value(state,'y'))
            # Keep this outcome distinct from a completed encounter; recovery/re-entry is a separate policy gate.
            temporary=os.path.join(EVIDENCE,'retreat-stop.tmp')
            with open(temporary,'w') as output: output.write('stop')
            os.rename(temporary,os.path.join(EVIDENCE,'benchmark-command.txt'))
            retreat_requested=0
            continue
        if navigation_rejections >= 10 and state.GetAttribute('paralyzed') != 'True' and state.GetAttribute('frozen') != 'True':
            raise Exception('Ten native navigation requests rejected without control effect')
        player = Engine.Player
        supplies = dict((n.GetAttribute('key'), n) for n in state.SelectNodes('supply'))
        if now - last_heartbeat > 2:
            last_heartbeat = now
            log('heartbeat', x=int(player.X), y=int(player.Y), hits=value(state, 'hits'), mana=value(state, 'mana'),
                paralyzed=state.GetAttribute('paralyzed'), frozen=state.GetAttribute('frozen'),
                str=value(state, 'str'), dex=value(state, 'dex'), intelligence=value(state, 'int'))

        # Server observation supplies ownership, not actions. Only client-visible enemies can be attacked.
        enemies = []
        enemy_names = {}
        enemy_types = {}
        defenders = set()
        for node in state.SelectNodes('enemy'):
            mobile = Engine.Mobiles.GetMobile(int(value(node, 'serial')))
            if mobile is not None and not mobile.IsDead:
                distance = max(abs(int(mobile.X) - int(player.X)), abs(int(mobile.Y) - int(player.Y)))
                enemies.append((distance, int(mobile.Serial), mobile))
                enemy_names[int(mobile.Serial)] = node.GetAttribute('name')
                enemy_types[int(mobile.Serial)] = node.GetAttribute('type')
                if node.GetAttribute('type')=='ArachnidOutbreakChampion':
                    observed=(int(mobile.X),int(mobile.Y),int(mobile.Z))
                    if observed!=boss_last_seen:log('boss_client_seen',position=observed)
                    boss_last_seen=observed
                    boss_search_route=None
                if node.GetAttribute('name') == 'a dread spider':
                    defenders.add(int(mobile.Serial))
        enemies.sort(key=lambda item: (item[0], item[1]))
        nearby_defenders = [entry for entry in enemies if entry[1] in defenders and entry[0] <= 8]
        candidates = nearby_defenders if nearby_defenders else enemies
        current = next((entry for entry in candidates if entry[1] == last_target), None)
        chosen = current if current is not None and current[0] <= 1 else (candidates[0] if candidates else None)
        if chosen is not None:
            enemies.remove(chosen)
            enemies.insert(0, chosen)
        warning = int(value(state, 'warning'))
        if warning != warning_serial:
            warning_serial, warning_seen = warning, now
            if warning: log('warning_observed', serial=warning)
        if warning and now - warning_seen >= .75:
            if warning!=dodge_requested:
                dodge_requested=warning
                anchor=state.SelectSingleNode("anchor[@stage='3']")
                warning_tile=(int(value(state,'warningX')),int(value(state,'warningY')))
                goals=dodge_goals(warning_tile[0],warning_tile[1],int(player.Z),value(anchor,'x'),value(anchor,'y'))
                def dodge(goals=tuple(goals),warning_tile=warning_tile):
                    if not Engine.Connected or os.path.exists(os.path.join(EVIDENCE,'native-stop.txt')):return
                    if (int(Engine.Player.X),int(Engine.Player.Y))!=warning_tile:return
                    for goal in goals:
                        accepted=bool(walk.Invoke(None,Array[Object]([Int32(n) for n in goal])))
                        log('dodge_walk',goal=goal,accepted=accepted)
                        if accepted:break
                Engine.TickWorkQueue.Enqueue(Action(dodge))
        if enemies:
            distance, serial, enemy = enemies[0]
            if serial != last_target or now - last_attack > 2:
                Attack(serial)
                last_target, last_attack = serial, now
                log('attack', target=serial, name=enemy_names.get(serial,''))
            if distance > 1 and not warning:
                navigate(enemy.X, enemy.Y, enemy.Z, 1, now)
        else:
            mask = int(value(state, 'clearedBroods'))
            stage = next((n for n in range(3) if not mask & (1 << n)), 3)
            if stage==3:
                anchor=state.SelectSingleNode("anchor[@stage='3']")
                if boss_search_route is None:
                    center=(int(value(anchor,'x')),int(value(anchor,'y')),int(value(anchor,'z')))
                    boss_search_route=boss_reacquisition_goals(center,boss_last_seen)
                    boss_search_index=0
                while boss_search_index<len(boss_search_route):
                    goal=boss_search_route[boss_search_index]
                    if max(abs(int(player.X)-goal[0]),abs(int(player.Y)-goal[1]))>1:break
                    boss_search_index+=1
                if boss_search_index==len(boss_search_route):boss_search_index=0
                goal=boss_search_route[boss_search_index]
                if goal!=last_search_goal:
                    log('boss_search_goal',goal=goal,last_client_seen=boss_last_seen)
                    last_search_goal=goal
                navigate(goal[0],goal[1],goal[2],goal[3],now)
            else:navigate_stage(state,stage,player,now)

        fraction = value(state, 'hits') / max(1, value(state, 'hitsMax'))
        poisoned = state.GetAttribute('poisoned') == 'True'
        bandaging = state.GetAttribute('bandaging') == 'True'
        controlled = state.GetAttribute('paralyzed')=='True' or state.GetAttribute('frozen')=='True'
        mortal = state.GetAttribute('mortalWound')=='True'
        mana=value(state,'mana')
        can_cast=(not controlled and pending_cast is None and not state.GetAttribute('currentSpell')
                  and state.GetAttribute('targetPending')!='True' and value(state,'spellCooldownSeconds')<=0 and now-last_spell>.5)
        ability=state.GetAttribute('currentAbility')
        if previous_ability and not ability: last_special=now
        previous_ability=ability
        if now - last_item >= 1:
            held = int(value(state, 'restraint'))
            if held:
                UseObject(held, True)
                log('escape', serial=held)
                last_item = now
            elif poisoned and (fraction<.5 or (value(state,'poisonLevel')==4 and fraction<.65)) and use(supplies, 'cure'):
                last_item = now
            elif not poisoned and not mortal and fraction < .55 and now - last_heal >= 10 and use(supplies, 'heal'):
                last_item = last_heal = now
            elif (poisoned or (fraction < .92 and not mortal)) and not bandaging and pending_cast is None and use(supplies, 'bandage', True):
                last_item = last_bandage = now
            elif poisoned and bandaging and now-last_bandage>5 and use(supplies,'cure'):
                last_item=now
            elif value(state,'stam')<.6*value(state,'stamMax') and use(supplies,'refresh'):
                last_item=now
            elif state.GetAttribute('statCurse')!='True' and value(state,'str')<150 and now-last_strength>=120 and use(supplies,'strength'):
                last_item=last_strength=now
            elif state.GetAttribute('statCurse')!='True' and value(state,'dex')<150 and now-last_agility>=120 and use(supplies,'agility'):
                last_item=last_agility=now
            elif mode == 'running-antivenom' and value(state, 'antivenomCharges') == 0 and now - last_antivenom >= 120 and use(supplies, 'antivenom', True):
                last_item = last_antivenom = now
            elif now - last_petals >= 300 and use(supplies, 'petals'):
                last_item = last_petals = now

        if can_cast:
            if fraction<.25 and now-last_heal<10 and mana>=6 and not retreat_requested:
                player_cast('Sacred Journey',int(value(supplies['rune'],'serial')));retreat_requested=now
            elif (mortal or state.GetAttribute('statCurse')=='True') and mana>=12:
                player_cast('Remove Curse',player.Serial)
            elif poisoned and fraction>.45 and mana>=24 and value(supplies['cure'],'remaining')<25 and (not bandaging or now-last_bandage<2):
                player_cast('Cleanse By Fire',player.Serial)
            elif fraction<.4 and not poisoned and not mortal and mana>=6:
                player_cast('Close Wounds',player.Serial)
            elif len([e for e in enemies if e[0]<=3])>=4 and fraction<.55 and mana>=24 and now-last_dispel>=15:
                player_cast('Dispel Evil');last_dispel=now
            elif enemies and enemies[0][0]<=1 and not bool(walking.GetValue(None)):
                desired=enemy_types[enemies[0][1]]
                boss_stage=int(value(state,'clearedBroods'))==7
                active_type=state.GetAttribute('enemyOfOneType').rsplit('.',1)[-1]
                type_change=(active_type!=desired and state.GetAttribute('waitingForEnemy')!='True')
                if mana>=30 and type_change and (not boss_stage or desired.endswith('ArachnidOutbreakChampion')):
                    player_cast('Enemy of One')
                elif mana>=24 and state.GetAttribute('consecrated')!='True':
                    player_cast('Consecrate Weapon')
                elif mana>=27 and state.GetAttribute('divineFury')!='True':
                    player_cast('Divine Fury')
                elif mana>=30 and len([e for e in enemies if e[0]<=3])>=3:
                    player_cast('Holy Light')
        if enemies and enemies[0][0]<=1 and not controlled and not ability and now-last_special>=5 and not state.GetAttribute('currentSpell') and pending_cast is None:
            adjacent=len([e for e in enemies if e[0]<=1])
            number=1 if adjacent>=2 and value(state,'stam')>=150 else 4
            cost=12 if number==1 else 9
            if mana>=cost+18 and now-last_spell>1:
                Msg('[SAD',0);time.sleep(.3);Msg('[Set'+str(number),0);last_special=now
                log('ability_requested',number=number,adjacent=adjacent,mana=mana)
except Exception as error:
    log('error', error=str(error))
    with open(os.path.join(EVIDENCE,'benchmark-driver-failed.txt'),'w') as output:
        output.write(str(error))
    raise
