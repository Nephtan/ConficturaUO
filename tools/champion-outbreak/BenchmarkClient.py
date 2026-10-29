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
        if Engine.Connected and not os.path.exists(os.path.join(EVIDENCE, 'native-stop.txt')):
            try:
                accepted = bool(walk.Invoke(None, Array[Object]([Int32(n) for n in goal])))
                log('walk', goal=goal, accepted=accepted)
            except Exception as error:
                log('walk_error', error=str(error))
    Engine.TickWorkQueue.Enqueue(Action(issue))

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

log('started', policy='reference-v1; waiting for explicit server start')
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
        if not mode.startswith('running'):
            if mode in ('completed', 'dead', 'failed', 'aborted', 'stopped'):
                log('finished', state=mode)
                break
            continue
        now = time.time()
        player = Engine.Player
        supplies = dict((n.GetAttribute('key'), n) for n in state.SelectNodes('supply'))
        if now - last_heartbeat > 2:
            last_heartbeat = now
            log('heartbeat', x=int(player.X), y=int(player.Y), hits=value(state, 'hits'), mana=value(state, 'mana'))

        # Server observation supplies ownership, not actions. Only client-visible enemies can be attacked.
        enemies = []
        for node in state.SelectNodes('enemy'):
            mobile = Engine.Mobiles.GetMobile(int(value(node, 'serial')))
            if mobile is not None and not mobile.IsDead:
                distance = max(abs(int(mobile.X) - int(player.X)), abs(int(mobile.Y) - int(player.Y)))
                enemies.append((distance, int(mobile.Serial), mobile))
        enemies.sort(key=lambda item: (item[0], item[1]))
        current = next((entry for entry in enemies if entry[1] == last_target), None)
        if current is not None and (current[0] <= 1 or not enemies or enemies[0][0] > 1):
            enemies.remove(current)
            enemies.insert(0, current)
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
                log('attack', target=serial)
            if distance > 1 and not warning:
                navigate(enemy.X, enemy.Y, enemy.Z, 1, now)
        else:
            mask = int(value(state, 'clearedBroods'))
            stage = next((n for n in range(3) if not mask & (1 << n)), 3)
            anchor = state.SelectSingleNode("anchor[@stage='%d']" % stage)
            if anchor is not None:
                navigate(value(anchor, 'x'), value(anchor, 'y'), value(anchor, 'z'), 1, now)

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
    raise
