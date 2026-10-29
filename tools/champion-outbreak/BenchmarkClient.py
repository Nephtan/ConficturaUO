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
                accepted = bool(walk.Invoke(None, Array[Object]([Int32(n) for n in goal])))
                navigation_rejections = 0 if accepted else navigation_rejections + 1
                log('walk', goal=goal, accepted=accepted)
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
    global probe_sequence, probe_attempt, probe_attack
    sequence = int(value(state,'probeSequence'))
    name = state.GetAttribute('probe')
    now = time.time()
    if probe_sequence != sequence:
        probe_sequence, probe_attempt, probe_attack = sequence, 0, False
        log('toolkit_case',sequence=sequence,name=name)
    if now-probe_attempt < 2.5:
        return
    probe_attempt = now
    supplies = dict((n.GetAttribute('key'),n) for n in state.SelectNodes('supply'))
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
        'dispel':('Dispel Evil',False),'remove-curse':('Remove Curse',True)}
    spell,target = spells[name]
    Cast(spell)
    if target and WaitForTarget(4000):
        Target(Engine.Player.Serial)
    log('toolkit_cast',sequence=sequence,spell=spell)

log('started', policy='defender-first; connected short navigation legs; toolkit qualification pending')
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
        if mode.startswith('toolkit-'):
            if mode not in ('toolkit-complete','toolkit-failed'):
                toolkit(state)
            continue
        if mode == 'navigation':
            now = time.time()
            if navigation_rejections >= 10 and state.GetAttribute('paralyzed') != 'True' and state.GetAttribute('frozen') != 'True':
                raise Exception('Ten native navigation requests rejected without control effect')
            navigate_stage(state,int(value(state,'navigationStage')),Engine.Player,now)
            continue
        if not mode.startswith('running'):
            if mode in ('completed', 'dead', 'failed', 'aborted', 'stopped', 'driver-failed', 'navigation-completed'):
                log('finished', state=mode)
                break
            continue
        now = time.time()
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
        defenders = set()
        for node in state.SelectNodes('enemy'):
            mobile = Engine.Mobiles.GetMobile(int(value(node, 'serial')))
            if mobile is not None and not mobile.IsDead:
                distance = max(abs(int(mobile.X) - int(player.X)), abs(int(mobile.Y) - int(player.Y)))
                enemies.append((distance, int(mobile.Serial), mobile))
                enemy_names[int(mobile.Serial)] = node.GetAttribute('name')
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
            navigate(value(state, 'warningX') + 2, value(state, 'warningY'), player.Z, 0, now)
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
            navigate_stage(state,stage,player,now)

        fraction = value(state, 'hits') / max(1, value(state, 'hitsMax'))
        poisoned = state.GetAttribute('poisoned') == 'True'
        if now - last_item >= 1:
            held = int(value(state, 'restraint'))
            if held:
                UseObject(held, True)
                log('escape', serial=held)
                last_item = now
            elif poisoned and use(supplies, 'cure'):
                last_item = now
            elif fraction < .55 and now - last_heal >= 10 and use(supplies, 'heal'):
                last_item = last_heal = now
            elif fraction < .9 and state.GetAttribute('bandaging') != 'True' and use(supplies, 'bandage', True):
                last_item = now
            elif mode == 'running-antivenom' and value(state, 'antivenomCharges') == 0 and now - last_antivenom >= 120 and use(supplies, 'antivenom', True):
                last_item = last_antivenom = now
            elif now - last_petals >= 300 and use(supplies, 'petals'):
                last_item = last_petals = now

        if enemies and enemies[0][0] <= 1 and not bool(walking.GetValue(None)) and now - last_spell >= 2:
            mana = value(state, 'mana')
            if fraction < .35 and not poisoned and mana >= 10:
                Cast('Close Wounds')
                if WaitForTarget(1000): Target(player.Serial)
                last_spell = now
                log('cast', spell='Close Wounds')
            elif mana >= 15 and now - last_enemy >= 120:
                Cast('Enemy of One')
                last_spell = last_enemy = now
                log('cast', spell='Enemy of One')
            elif mana >= 10 and now - last_consecrate >= 9:
                Cast('Consecrate Weapon')
                last_spell = last_consecrate = now
                log('cast', spell='Consecrate Weapon')
except Exception as error:
    log('error', error=str(error))
    with open(os.path.join(EVIDENCE,'benchmark-driver-failed.txt'),'w') as output:
        output.write(str(error))
    raise
