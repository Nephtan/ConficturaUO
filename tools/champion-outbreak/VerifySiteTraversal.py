"""Verify copied-client route traces and preserved source/save inputs after teardown."""
from pathlib import Path
import argparse
import csv
import hashlib
import json
import subprocess


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def hashes(path):
    return {str(p.relative_to(path)).replace('\\','/'):sha(p) for p in path.rglob('*') if p.is_file()}


def verify(root, fixture_ref=None, sites=None, report_name='site-verification.json'):
    root = root.resolve()
    evidence = root/'evidence'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve() == root/'shard'
    prepared = json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture'] == 'site-traversal'
    assert (evidence/'site-forced-stop.txt').is_file(), 'No successful owned no-save stop marker'
    repo = Path(__file__).resolve().parents[2]
    fixture = 'ArachnidOutbreakSiteTraversal.cs'
    fixture_commit = None
    fixture_source = (repo/'tests'/fixture).read_bytes()
    if fixture_ref:
        fixture_commit = subprocess.check_output(['git','-C',str(repo),'rev-parse','--verify','--end-of-options',fixture_ref+'^{commit}'],text=True).strip()
        fixture_source = subprocess.check_output(['git','-C',str(repo),'show',fixture_commit+':tests/'+fixture])
    expected_fixture = hashlib.sha256(fixture_source).hexdigest()
    assert sha(root/'shard/Data/Scripts/TestOnly'/fixture) == expected_fixture == prepared['harness_sha256']
    assert sha(root/'shard/ConficturaServer.exe') == prepared['core_sha256'] == sha(repo/'ConficturaServer.exe')
    assert hashes(root/'shard/Saves') == prepared['source_saves_sha256']
    protected = json.loads((evidence/'protected-source-inputs.json').read_text(encoding='utf-8-sig'))
    for name, expected in protected.items():
        p = Path(name)
        assert (hashes(p) if p.is_dir() else sha(p)) == expected, 'Original source changed: '+name
    inputs = json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/p) == value for p,value in inputs.items()), 'Candidate production source drifted'
    plans = {}
    for row in csv.DictReader((evidence/'site-routes.csv').open(encoding='utf-8-sig')):
        plans.setdefault(row['site'], []).append(row)
    events = list(csv.DictReader((evidence/'site-events.csv').open(encoding='utf-8-sig')))
    assert len(plans) == 6
    selected = list(plans) if sites is None else sites
    assert selected and len(set(selected)) == len(selected) and set(selected).issubset(plans), 'Unknown or duplicate selected site'
    assert Path(report_name).name == report_name and report_name.endswith('.json'), 'Report must stay in evidence directory'
    macros = json.loads((evidence/'site-macros.json').read_text())
    assert macros['plans_sha256'] == sha(evidence/'site-routes.csv'), 'Route plan changed since macro installation'
    packet_summary = None
    if b'site-packet-summary.json' in fixture_source:
        assert (evidence/'site-packet-summary.json').is_file(), 'Tested observer fixture requires its diagnostic summary'
    if (evidence/'site-packet-summary.json').exists():
        packet_summary = json.loads((evidence/'site-packet-summary.json').read_text())
        assert packet_summary['diagnosticFailures'] == 0, 'Diagnostic I/O lost observations'
    results = []
    for site, rows in plans.items():
        if site not in selected:
            continue
        starts = [i for i,e in enumerate(events) if e['site'] == site and e['event'] == 'scripted-site-start']
        trace = [e for e in events[starts[-1]+1:] if e['site'] == site] if starts else []
        checkpoints = [e['event'] for e in trace if e['event'] != 'walk-step']
        error = None
        try:
            assert starts, 'Site never selected'
            assert len(trace) == len(rows)-1, 'Incomplete or unexpected step count'
            for expected, actual in zip(rows[1:], trace):
                assert actual['event'] != 'off-planned-route', 'Retained deviation in latest attempt'
                assert actual['connected'] == 'True', 'Disconnected movement observation'
                assert all(actual[k] == expected[k] for k in ('step','x','y','z')), 'Position or sequence mismatch'
                assert actual['event'] == (expected['checkpoint'] or 'walk-step'), 'Checkpoint mismatch'
            assert checkpoints == ['brood-0','brood-1','boss','brood-2','south-exit'], 'Missing checkpoint'
        except AssertionError as exc:
            error = str(exc)
        results.append(dict(site=site, passed=error is None, error=error, observed_moves=len(trace), expected_steps=len(rows)-1,
            attempts=len(starts), start=events[starts[-1]]['utc'] if starts else None,
            end=trace[-1]['utc'] if trace else None, checkpoints=[x for x in checkpoints if x != 'off-planned-route']))
    passed = all(r['passed'] for r in results)
    result = dict(passed=passed, preservation_passed=True, scope='Instrumented normal-client walking and preservation; native visual observations are separate. No combat, current-world approval or activation.',
                  selected_sites=selected, full_six_site_gate=len(selected)==6, six_site_gate_passed=passed and len(selected)==6, fixture_commit=fixture_commit,
                  fixture_sha256=expected_fixture, packet_summary=packet_summary,
                  source_inputs=len(inputs), results=results, core_sha256=prepared['core_sha256'])
    (evidence/report_name).write_text(json.dumps(result,indent=2))
    print(str(sum(r['passed'] for r in results))+'/'+str(len(selected))+' selected connected-client routes; all source/profile/save inputs preserved. Earlier attempts remain in raw events.')
    return 0 if passed else 1


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--fixture-ref', help='Explicit Git commit for historical fixture identity; production inputs must still match current source')
    parser.add_argument('--sites', nargs='+', help='Explicit subset; does not establish the full six-site gate')
    parser.add_argument('--report-name', default='site-verification.json')
    args = parser.parse_args()
    raise SystemExit(verify(args.output, args.fixture_ref, args.sites, args.report_name))
