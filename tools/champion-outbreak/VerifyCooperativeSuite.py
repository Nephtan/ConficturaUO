"""Combine preserved independent native role cases without claiming one six-case run."""
from pathlib import Path
import argparse
import json
from VerifyCooperative import verify
from VerifyBenchmark import sha


def suite(roots,output):
    accepted={};inputs=None;plugin=None;reports=[]
    for root in roots:
        root=root.resolve();verify(root)
        e=root/'evidence';path=e/'cooperative-verification.json';v=json.loads(path.read_text())
        p=json.loads((e/'prepared.json').read_text());source=json.loads((Path(p['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
        assert v['preservation_and_acknowledged_teardown_passed']
        if inputs is None:inputs=source
        else:assert source==inputs,'Different production snapshots cannot be silently combined'
        observed=sha(root/'client/ClassicAssist/ClassicAssist.dll')
        assert observed==sha(root/'ally-client/ClassicAssist/ClassicAssist.dll')
        if plugin is None:plugin=observed
        else:assert plugin==observed,'Different copied client plugins'
        reports.append(dict(root=str(root),report_sha256=sha(path),current_fixture_matches=v['current_fixture_matches'],current_client_driver_matches=v['current_client_driver_matches']))
        for row in v['completed_controlled_native_cases']:
            name=row['case'];assert name not in accepted,'Choose one preserved result per role'
            delta=row['points_after']-row['points_before']
            if name=='heal':assert delta>0
            elif name in ('cure','rescue','prevent'):assert delta>=1.99
            elif name=='bard':assert delta>=.5
            accepted[name]=dict(row,source_copy=root.name)
    assert set(accepted)=={'heal','cure','rescue','treat','prevent','bard'}
    result=dict(all_six_independent_native_role_types_verified=True,single_six_case_run=False,synthetic_roles=True,
        production_inputs_identical_and_current=len(inputs),copied_plugin_observed_sha256=plugin,
        cases=list(accepted.values()),source_reports=reports,
        scope='Independent connected synthetic role scenes; owned injury/poison/party and finite supplies staged; enemy AI paused; production bard observer sampled on wall clock; no group difficulty/pacing/economics/actual-Zanthus capability or Computer Use role acceptance',
        production_activation=False,site_approval=False)
    assert not output.exists(),'Use a fresh aggregate path'
    output.write_text(json.dumps(result,indent=2)+'\n');print('Six independent native role types verified; no single-run, AI or pacing claim.')


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('roots',nargs='+',type=Path);p.add_argument('--output',type=Path,required=True)
    args=p.parse_args();suite(args.roots,args.output)
