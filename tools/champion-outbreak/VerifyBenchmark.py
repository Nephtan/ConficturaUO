"""Verify a retained benchmark's provenance and outcome; a valid failure is not a clear."""
from pathlib import Path
import argparse
import hashlib
import json
import xml.etree.ElementTree as ET


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def hashes(path):
    return {str(p.relative_to(path)).replace('\\', '/'):sha(p)
            for p in path.rglob('*') if p.is_file()}


def verify(root, require_clear=False):
    repo = Path(__file__).resolve().parents[2]
    evidence = root/'evidence'
    prepared = json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture'] == 'benchmark'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve() == (root/'shard').resolve()
    frozen = json.loads((evidence/'reference-freeze.json').read_text())
    for key, value in frozen['files'].items():
        snapshot = evidence/'frozen-inputs'/(key+Path(value['path']).suffix)
        assert sha(snapshot) == value['sha256'], f'Frozen input changed: {key}'
    assert sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakBenchmark.cs') == frozen['files']['fixture']['sha256']
    assert sha(root/'client/ClassicAssist/OutbreakBenchmarkClient.py') == frozen['files']['client_policy']['sha256']
    assert sha(evidence/'benchmark-reference.xml') == frozen['files']['reference_xml']['sha256']
    assert sha(root/'shard/ConficturaServer.exe') == prepared['core_sha256']
    inputs = json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/p) == expected for p, expected in inputs.items()), 'Production input changed'
    assert hashes(root/'shard/Saves') == prepared['source_saves_sha256'], 'Private saves changed'
    protected = json.loads((evidence/'protected-source-inputs.json').read_text())
    for path, expected in protected.items():
        path = Path(path)
        assert (hashes(path) if path.is_dir() else sha(path)) == expected, 'Original source/profile changed'
    settings = json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'))
    assert settings['ip'] == '127.0.0.1' and settings['port'] == prepared['port']
    assert settings['plugins'] == [str(root/'client/ClassicAssist/ClassicAssist.dll')]
    teardown = json.loads((evidence/'teardown.json').read_text(encoding='utf-8-sig'))
    assert teardown['forced_no_save'] and not teardown['graceful_shutdown_claimed']
    assert teardown['remaining_owned_processes'] == teardown['remaining_private_listeners'] == 0
    assert (evidence/'native-stopped.txt').is_file()

    reference = ET.parse(evidence/'benchmark-reference.xml').getroot()
    assert reference.get('access') == 'Player' and reference.get('race') == '0'
    assert sum(int(reference.get(k)) for k in ('rawStr','rawDex','rawInt')) == 250
    assert reference.get('skillCap') == '10000' and reference.get('baseSkillTotal') == '9800'
    assert len(reference.findall('equipment')) == 10
    state = ET.parse(evidence/'run-001-final.xml').getroot()
    events = [json.loads(line) for line in (evidence/'benchmark-client.jsonl').read_text().splitlines()]
    errors = [event for event in events if event['event'] in ('error','walk_error')]
    finished = [event for event in events if event['event'] == 'finished']
    assert finished and finished[-1]['state'] == state.get('state')
    assert state.get('blessed') == 'False' and float(state.get('seconds')) > 0
    clear = (state.get('state') == 'completed' and state.get('alive') == 'True'
             and state.get('bossKilled') == 'True' and not errors)
    starting = {'bandage':500,'cure':100,'heal':75,'petals':10,'antivenom':12}
    used = {n.get('key'):starting[n.get('key')]-int(n.get('remaining')) for n in state.findall('supply')}
    report = dict(provenance_preservation_and_teardown_passed=True, reference_clear_passed=clear,
                  outcome=state.get('state'), seconds=float(state.get('seconds')),
                  boss_killed=state.get('bossKilled') == 'True', cleared_brood_mask=int(state.get('clearedBroods')),
                  owned_enemy_deaths=int(state.get('enemyDeaths')), player_dead=state.get('alive') == 'False',
                  incoming_health_loss=int(state.get('incomingDamage')),
                  owned_enemy_health_loss=int(state.get('outgoingDamage')),
                  actual_restored_hits=int(state.get('restoredHits')), supplies_consumed=used,
                  client_errors=errors, production_inputs_checked=len(inputs), private_save_files=len(hashes(root/'shard/Saves')),
                  pacing_gate='Not evaluated: requires three complete frozen-reference runs',
                  tuning_passes=0, production_activation=False, site_approval=False)
    (evidence/'benchmark-verification.json').write_text(json.dumps(report, indent=2), encoding='utf-8')
    print(json.dumps(report, indent=2))
    if require_clear and not clear:
        raise SystemExit(1)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('root', type=Path)
    parser.add_argument('--require-clear', action='store_true')
    args = parser.parse_args()
    verify(args.root.resolve(), args.require_clear)
