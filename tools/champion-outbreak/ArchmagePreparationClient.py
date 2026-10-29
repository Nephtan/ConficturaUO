# IronPython: run the original snapshotted macro and helper, without combat.
import os
import sys
import time
import json
import clr
clr.AddReference('System.Xml')
from System.Diagnostics import Process
from System.IO import FileStream, FileMode, FileAccess, FileShare
from System.Xml import XmlDocument
from Assistant import Engine

ROOT = os.path.abspath(OUTBREAK_ROOT)
EVIDENCE = os.path.join(ROOT, 'evidence')
if not os.path.normcase(str(Process.GetCurrentProcess().MainModule.FileName)).startswith(os.path.normcase(os.path.join(ROOT, 'client') + os.sep)):
    raise Exception('Not the disposable copied client')
with open(os.path.join(EVIDENCE, 'prepared.json')) as source:
    if json.load(source)['fixture'] != 'archmage-preparation':
        raise Exception('Wrong private fixture')
with open(os.path.join(EVIDENCE, 'native-private-root.txt')) as source:
    if os.path.normcase(source.read().strip()) != os.path.normcase(os.path.join(ROOT, 'shard')):
        raise Exception('Wrong private root')

def status():
    stream = FileStream(os.path.join(EVIDENCE, 'archmage-state.xml'), FileMode.Open,
                        FileAccess.Read, FileShare.ReadWrite | FileShare.Delete)
    try:
        document = XmlDocument()
        document.XmlResolver = None
        document.Load(stream)
        return document.DocumentElement
    finally:
        stream.Dispose()

original_enemy = GetEnemy
original_message = SysMessage
prepared_scan = False

def diagnostic(message, *args):
    with open(os.path.join(EVIDENCE, 'archmage-diagnostics.private.jsonl'), 'a') as output:
        output.write(json.dumps(dict(epoch=time.time(), message=str(message))) + '\n')
    return original_message(message, *args)

def preparation_enemy(*args):
    global CASM_TEST_MAX_LOOPS, prepared_scan
    # This first scan follows successful original maintenance. Summon discovery
    # scans earlier in maintenance still use the unmodified native command.
    macro_state = globals().get('state', {})
    if int(macro_state.get('next_maintenance_at', 0)) > 0:
        CASM_TEST_MAX_LOOPS = 1
        prepared_scan = True
        return False
    return original_enemy(*args)

try:
    deadline = time.time() + 90
    while time.time() < deadline:
        if Engine.Connected and status().GetAttribute('state') == 'ready':
            break
        time.sleep(.2)
    if not Engine.Connected or status().GetAttribute('state') != 'ready':
        raise Exception('Copied preparation not ready')
    GetEnemy = preparation_enemy
    SysMessage = diagnostic
    CASM_TEST_ENABLE_DIAGNOSTICS = True
    CASM_TEST_MAX_LOOPS = 10000
    sys.path.insert(0, os.path.join(ROOT, 'client', 'ClassicAssist', 'PrivateArchmage'))
    execfile(os.path.join(ROOT, 'client', 'ClassicAssist', 'PrivateArchmage', 'archmage-overlord.py'), globals())
    if not prepared_scan or globals().get('state', {}).get('stop_reason') != 'normal':
        raise Exception('Original macro did not complete preparation and stop at its first combat scan')
    entries=[]
    for entry in Engine.Journal.GetEntireBuffer():
        if entry is not None:
            entries.append(dict(text=unicode(entry.Text or ''), speaker=unicode(entry.Name or '')))
    with open(os.path.join(EVIDENCE, 'archmage-journal.private.json'), 'w') as output:
        json.dump(entries, output, indent=2)
    with open(os.path.join(EVIDENCE, 'archmage-macro-prepared.txt'), 'w') as output:
        output.write('Original maintenance executed; first subsequent enemy scan withheld; no combat or movement issued by wrapper.')
except Exception as error:
    with open(os.path.join(EVIDENCE, 'archmage-client-error.txt'), 'w') as output:
        output.write(str(error))
    raise
