"""Freeze a prepared copied archmage and retain one bounded stationary or route-assisted case."""
from pathlib import Path
import argparse
import atexit
import ast
import ctypes
from ctypes import wintypes
import json
import os
import time
import uuid
import xml.etree.ElementTree as ET
from FreezeBenchmark import read_status,read_shared
from VerifyBenchmark import sha

TERMINAL={'completed','dead','aborted','stationary-idle','stationary-window-ended','failed','client-failed','stopped','timeout'}

def bind_server(root,evidence):
    pid=int((evidence/'native-server-pid.txt').read_text(encoding='utf-8-sig').strip())
    api=ctypes.WinDLL('kernel32',use_last_error=True)
    api.OpenProcess.argtypes=[wintypes.DWORD,wintypes.BOOL,wintypes.DWORD];api.OpenProcess.restype=wintypes.HANDLE
    api.QueryFullProcessImageNameW.argtypes=[wintypes.HANDLE,wintypes.DWORD,wintypes.LPWSTR,ctypes.POINTER(wintypes.DWORD)]
    api.WaitForSingleObject.argtypes=[wintypes.HANDLE,wintypes.DWORD];api.WaitForSingleObject.restype=wintypes.DWORD
    api.CloseHandle.argtypes=[wintypes.HANDLE]
    handle=api.OpenProcess(0x101000,False,pid)
    if not handle:raise ctypes.WinError(ctypes.get_last_error())
    atexit.register(api.CloseHandle,handle)
    length=wintypes.DWORD(32768);buffer=ctypes.create_unicode_buffer(length.value)
    if not api.QueryFullProcessImageNameW(handle,0,buffer,ctypes.byref(length)):raise ctypes.WinError(ctypes.get_last_error())
    assert Path(buffer.value).resolve()==root/'shard/ConficturaServer.exe','Server process is not the private copy'
    def require_alive():
        result=api.WaitForSingleObject(handle,0)
        if result==0:
            marker=evidence/'observer-server-exited.json'
            if not marker.exists():marker.write_text(json.dumps(dict(pid=pid,scope='Actual bound server exited; stale XML is not a live run'),indent=2))
            raise RuntimeError('Bound private server exited; retain last observation and inspect failure, no restart')
        if result!=258:raise RuntimeError('Server liveness wait failed: '+str(result))
    require_alive()
    return require_alive


def publish(evidence,name,text):
    target=evidence/name;assert not target.exists(),'Another request is pending'
    temp=evidence/(str(uuid.uuid4())+'.tmp');temp.write_text(text);os.replace(temp,target)


def run(root,fixture_heartbeat=False):
    root=root.resolve();evidence=root/'evidence';repo=Path(__file__).resolve().parents[2]
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve()),'Observer requires a disposable test root'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='archmage-preparation'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    if fixture_heartbeat:
        from FixtureHeartbeat import bind_fixture
        require_alive=bind_fixture(root,evidence)
    else:
        require_alive=bind_server(root,evidence)
    mode=(evidence/'archmage-mode.txt').read_text();assert mode in ('stationary','route')
    deadline=time.monotonic()+150
    while time.monotonic()<deadline:
        require_alive()
        state=read_status(evidence/'archmage-state.xml')
        assert state.get('state') not in TERMINAL,state.get('failure')
        if state.get('state')=='prepared':break
        time.sleep(.2)
    assert state.get('state')=='prepared' and state.get('online')=='True' and state.get('blessed')=='False'
    initial=json.loads((evidence/'archmage-input-freeze.private.json').read_text())
    mutable={'client/settings.json','client/ClassicAssist/Profiles/settings.json'}
    for path,expected in initial['files'].items():
        if path.replace('\\','/') not in mutable:assert sha(root/path)==expected,path
    inputs=json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/path)==expected for path,expected in inputs.items())
    snapshot=ET.parse(evidence/'archmage-prepared.private.xml').getroot()
    code=(root/'client/ClassicAssist/PrivateArchmage/archmage-overlord.py').read_text(encoding='utf-8-sig')
    names={'EXPECTED_MAX_HITS','EXPECTED_MAX_STAM','EXPECTED_MAX_MANA','WRAITH_BODY'}
    constants={n.targets[0].id:ast.literal_eval(n.value) for n in ast.parse(code).body if isinstance(n,ast.Assign)
               and isinstance(n.targets[0],ast.Name) and n.targets[0].id in names}
    for key,name in [('hitsMax','EXPECTED_MAX_HITS'),('stamMax','EXPECTED_MAX_STAM'),('manaMax','EXPECTED_MAX_MANA'),('body','WRAITH_BODY')]:
        assert snapshot.get(key)==str(constants[name]),'Native maintenance effect absent'
    assert int(snapshot.get('followers'))>0
    freeze=evidence/'archmage-combat-freeze.private.json';assert not freeze.exists()
    files=['archmage-original.private.xml','archmage-prepared.private.xml','source-character-inspection.private.xml','archmage-input-freeze.private.json']
    freeze.write_text(json.dumps(dict(files={name:sha(evidence/name) for name in files},mode=mode,
        observation_window_seconds=300 if mode=='stationary' else 2700,idle_after_stage_seconds=120 if mode=='stationary' else None,
        scope='Actual historical copied character and original macro/helper; idle walking adapter only in route mode; no midfight grants'),indent=2))
    publish(evidence,'archmage-command.txt','start-'+mode)
    print('Actual prepared copied character and original inputs frozen; '+mode+' case requested.',flush=True)
    deadline=time.monotonic()+(370 if mode=='stationary' else 2770);next_progress=0
    while time.monotonic()<deadline:
        require_alive()
        state=read_status(evidence/'archmage-state.xml')
        if state.get('state') in TERMINAL:break
        if time.monotonic()>=next_progress:
            print('Observed '+state.get('state')+'; '+state.get('seconds','0')+'s; brood mask '+state.get('clearedBroods','0'),flush=True)
            next_progress=time.monotonic()+45
        time.sleep(.2)
    if state.get('state') not in TERMINAL:
        publish(evidence,'archmage-command.txt','stop')
        raise RuntimeError('Case observation deadline; stop requested, no restart')
    result=evidence/'archmage-result.private.xml';assert not result.exists()
    content=read_shared(evidence/'archmage-state.xml');assert ET.fromstring(content).get('state') in TERMINAL
    result.write_bytes(content)
    deadline=time.monotonic()+3
    while time.monotonic()<deadline and not (evidence/'archmage-macro-finished.txt').exists():time.sleep(.1)
    publish(evidence,'native-stop.txt','Terminal case retained; cleanup without saving.')
    print('Retained terminal '+mode+' observation: '+state.get('state')+'; '+state.get('seconds','0')+' seconds',flush=True)


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--fixture-heartbeat',action='store_true',help='Explicit guarded fixture-file liveness; no OS process-handle attempt or permission fallback')
    args=parser.parse_args();run(args.root,args.fixture_heartbeat)
