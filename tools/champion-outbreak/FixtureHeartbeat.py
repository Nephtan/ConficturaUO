"""Read-only monitoring of one guarded disposable fixture; no OS process handles or privilege fallback."""
from pathlib import Path
import datetime,json,time
from FreezeBenchmark import read_status,sha


def bind_fixture(root,evidence,max_age=10):
    root=root.resolve();evidence=evidence.resolve()
    assert evidence==root/'evidence'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    launch=json.loads((evidence/'server-process.json').read_text(encoding='utf-8-sig'))
    expected=root/'shard/ConficturaServer.exe';assert Path(launch['executable']).resolve()==expected
    prepared=json.loads((evidence/'prepared.json').read_text())
    assert sha(expected)==prepared['core_sha256']
    source=evidence/'archmage-state.xml';first=read_status(source)
    instance=first.get('fixtureInstance');assert instance and len(instance)==32,'Fixture has no instance identity'
    binding=evidence/'observer-fixture-binding.json'
    if binding.exists():
        previous=json.loads(binding.read_text())
        assert previous['instance']==instance and int(previous['pid'])==int(launch['processId']),'Refuse observer restart against a different fixture'
    def require_fresh():
        for attempt in range(10):
            try:
                modified=source.stat().st_mtime;state=read_status(source);break
            except FileNotFoundError:
                if attempt==9:raise
                time.sleep(.05)
        assert state.get('fixtureInstance')==instance,'Fixture instance changed; no automatic rebind'
        assert int(state.get('serverPid','0'))==int(launch['processId']),'Fixture PID differs from owned launch record'
        assert Path(state.get('serverExe','')).resolve()==expected,'Fixture executable differs from guarded private root'
        stamp=datetime.datetime.fromisoformat(state.get('heartbeatUtc','').replace('Z','+00:00'))
        age=(datetime.datetime.now(datetime.timezone.utc)-stamp).total_seconds()
        if age < -2 or age > max_age or time.time()-modified>max_age:
            marker=evidence/'observer-stale-fixture.json'
            if not marker.exists():marker.write_text(json.dumps(dict(instance=instance,age_seconds=age,scope='Stale or stalled fixture; process exit itself is not established'),indent=2))
            raise RuntimeError('Fixture heartbeat stale; stop observation, retain state and inspect; no restart')
        return state
    require_fresh()
    (evidence/'observer-fixture-binding.json').write_text(json.dumps(dict(instance=instance,pid=launch['processId'],executable=str(expected),core_sha256=prepared['core_sha256'],maximum_heartbeat_age_seconds=max_age,method='Workspace file reads; guarded fixture instance/PID/path/hash/freshness. No OpenProcess, privilege request or automatic rebind.',limitation='Detects an unresponsive or exited fixture within freshness window; not an OS process-handle proof.'),indent=2))
    return require_fresh
