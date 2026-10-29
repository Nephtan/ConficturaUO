"""Recheck three complete comparable runs before accepting the veteran pacing gate."""
from pathlib import Path
import argparse
import json
import statistics
import xml.etree.ElementTree as ET
from FreezeBenchmark import capability_signature
from VerifyBenchmark import verify


def series(roots, output):
    roots=[p.resolve() for p in roots]
    assert len(roots)==3 and len(set(roots))==3,'Exactly three distinct completed runs are required'
    assert not output.exists(),'Never overwrite a prior series result'
    rows=[];signatures=[];policies=[];fixtures=[];inputs=[];saves=[];stocks=[];cores=[]
    for root in roots:
        verify(root,require_clear=True)
        evidence=root/'evidence'
        state=ET.parse(evidence/'run-001-final.xml').getroot()
        assert state.get('clearedBroods')=='7' and state.get('phase')=='Cooldown'
        assert state.get('controllerStatus')=='Completed; cooldown active.' and not state.get('failure')
        assert state.get('blessed')=='False' and state.get('navigationOnly')=='False'
        reference=ET.parse(evidence/'benchmark-reference.xml').getroot()
        assert reference.get('version')=='3' and reference.get('playerPrepared')=='True'
        signatures.append(capability_signature(evidence/'benchmark-reference.xml'))
        frozen=json.loads((evidence/'reference-freeze.json').read_text())
        prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
        policies.append(frozen['files']['client_policy']['sha256'])
        fixtures.append(frozen['files']['fixture']['sha256'])
        inputs.append(json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files'])
        saves.append(prepared['source_saves_sha256']);cores.append(prepared['core_sha256'])
        stock={n.get('key'):int(n.get('amount')) for n in reference.findall('supply')}
        mode=state.get('runMode');assert mode in ('no-antivenom','antivenom')
        assert stock.pop('antivenom',0)==(12 if mode=='antivenom' else 0)
        stocks.append(stock)
        report=json.loads((evidence/'benchmark-verification.json').read_text())
        rows.append(dict(root=str(root),mode=mode,seconds=float(state.get('seconds')),
                         supplies_consumed=report['supplies_consumed'],
                         native_walk_rejections=report['native_walk_rejections']))
    for values in (signatures,policies,fixtures,inputs,saves,stocks,cores):
        assert all(value==values[0] for value in values[1:]),'Runs are not the same frozen reference/policy/source/starting world/stock'
    assert any(row['mode']=='no-antivenom' for row in rows),'At least one complete run must exclude antivenom'
    median=statistics.median(row['seconds'] for row in rows)/60
    passed=20<=median<=35
    result=dict(three_comparable_complete_runs=True,at_least_one_without_antivenom=True,
                median_minutes=median,pacing_passed=passed,runs=rows,
                policy_sha256=policies[0],fixture_sha256=fixtures[0],core_sha256=cores[0],
                scope='Veteran clear/pacing only; other native, site, economy and Zanthus gates remain separate',
                production_activation=False)
    output.write_text(json.dumps(result,indent=2));print(json.dumps(result,indent=2))
    if not passed:raise SystemExit('Completed series median is outside20–35 minutes; retain evidence and apply only agreed tuning bounds.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('roots',nargs='+',type=Path)
    parser.add_argument('--output',required=True,type=Path);args=parser.parse_args()
    series(args.roots,args.output)
