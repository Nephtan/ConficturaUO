"""Verify an acknowledged copied startup smoke; no gameplay or live deployment claim."""
from pathlib import Path
import argparse
import json
from VerifyBenchmark import sha,hashes


def verify(root):
    root=root.resolve();e=root/'evidence';repo=Path(__file__).resolve().parents[2]
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    p=json.loads((e/'prepared.json').read_text());assert p['fixture']=='acceptance'
    inputs=json.loads((Path(p['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files'];count=0
    for name,expected in inputs.items():
        assert sha(repo/name)==expected,name
        if name.replace('\\','/').startswith('Data/Scripts/'):
            relative=name.replace('\\','/')[len('Data/Scripts/'):]
            assert sha(root/'shard'/name)==p['overlays'].get(relative,expected),name
            count+=1
    assert sha(root/'shard/ConficturaServer.exe')==p['core_sha256']
    assert sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakNativeAcceptance.cs')==p['harness_sha256']
    assert hashes(root/'shard/Saves')==p['source_saves_sha256']
    for name,expected in json.loads((e/'protected-source-inputs.json').read_text()).items():
        path=Path(name);assert (hashes(path) if path.is_dir() else sha(path))==expected,name
    settings=json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'))
    assert settings['ip']=='127.0.0.1' and settings['port']==p['port'] and settings['plugins']==[]
    milestone=json.loads((e/'startup-live-milestone.json').read_text(encoding='utf-8-sig'))
    assert milestone['listener_verified'] and milestone['native_ready'] and not milestone['client_launched']
    console=(e/'startup-console.log').read_text(errors='replace')
    assert 'Core: Running with arguments: -service -nocache' in console and 'Listening: 127.0.0.1:'+str(p['port']) in console
    stopped=json.loads((e/'teardown.json').read_text(encoding='utf-8-sig'))
    assert stopped['native_stop_acknowledged'] and stopped['forced_no_save'] and not stopped['graceful_shutdown_claimed']
    assert stopped['remaining_owned_processes']==stopped['remaining_private_listeners']==0
    report=dict(forced_runtime_compile_world_initialize_and_loopback_startup=True,production_inputs_checked=len(inputs),
        private_runtime_inputs_checked=count,private_save_files=len(p['source_saves_sha256']),
        originals_and_private_saves_preserved=True,acknowledged_forced_no_save_teardown=True,
        scope='Copied-world startup with guarded test fixture; no client/gameplay acceptance, no graceful exit or live deployment claim',production_activation=False)
    (e/'startup-verification.json').write_text(json.dumps(report,indent=2)+'\n');print(json.dumps(report,indent=2))


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('root',type=Path);verify(p.parse_args().root)
