"""Summarize accepted finite observations without replay, valuation or profit inference."""
from pathlib import Path
import argparse
import json
import xml.etree.ElementTree as ET
from VerifyBenchmark import sha


def record(output):
    repo=Path(__file__).resolve().parents[2]
    series_path=repo/'docs/champion-outbreak/VETERAN_SERIES.json'
    series=json.loads(series_path.read_text());assert series['three_comparable_complete_runs']
    rows=[];evidence={str(series_path):sha(series_path)}
    for run in series['runs']:
        root=Path(run['root']);e=root/'evidence'
        assert root.resolve().is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
        assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
        v=json.loads((e/'benchmark-verification.json').read_text());c=json.loads((e/'priced-cost-observation.json').read_text())
        assert v['reference_clear_passed'] and v['provenance_preservation_and_teardown_passed'] and not v['player_dead']
        assert v['seconds']==run['seconds'] and v['supplies_consumed']==run['supplies_consumed']
        assert all(x['consumed']*x['base_unit']==x['subtotal'] for x in c['prices'].values())
        assert sum(x['subtotal'] for x in c['prices'].values())+v['tithing_net_reduction']==c['priced_replacement_subtotal']
        events=[json.loads(x) for x in (e/'benchmark-client.jsonl').read_text().splitlines()]
        rows.append(dict(copy=root.name,mode=run['mode'],seconds=v['seconds'],player_deaths=0,
            native_journey_cast_requests=sum(x.get('event')=='cast' and x.get('spell')=='Sacred Journey' for x in events),
            native_retreat_arrivals=sum(x.get('event')=='retreat_arrived' for x in events),
            supplies_consumed=v['supplies_consumed'],tithing_net_reduction=v['tithing_net_reduction'],
            priced_recurring_replacement_subtotal=c['priced_replacement_subtotal'],gold_collected=None,
            corpse_loot_collected=None,personal_rewards_collected=None,net_profit=None))
        for name in ('benchmark-verification.json','priced-cost-observation.json','benchmark-client.jsonl'):
            evidence[str(e/name)]=sha(e/name)
    probes=[]
    for mode in ('stationary','route'):
        e=Path('D:/ConficturaUOTestTools/outbreak-archmage-'+mode+'-20261029-002/evidence')
        p=e/'archmage-preparation-verification.json';v=json.loads(p.read_text());assert v['preservation_and_teardown_passed']
        before_path=e/'archmage-prepared.private.xml';after_path=e/'archmage-final.private.xml'
        before={x.get('serial'):x for x in ET.parse(before_path).getroot().findall('.//item')}
        after={x.get('serial'):x for x in ET.parse(after_path).getroot().findall('.//item')}
        gold=lambda nodes:sum(int(x.get('amount')) for x in nodes if x.get('type')=='Server.Items.Gold')
        probes.append(dict(mode=mode,seconds=v['seconds'],outcome=v['outcome'],player_deaths=v['player_deaths'],
            pending_glands=v['pending_glands'],pending_skulls=v['pending_skulls'],pending_artifacts=v['pending_artifacts'],
            new_ground_gold=v['new_ground_gold'],backpack_gold_before=gold(before.values()),backpack_gold_after=gold(after.values()),
            new_backpack_entry_count=len(set(after)-set(before)),corpse_loot_assessed=False,
            retreat_count=None,supplies_observation='Private endpoint item/research balance changes retained; no per-cast consumption claim',
            scope='One bounded original-macro observation; route adds documented native walking assistance; no repeatable-farm/current-live-character inference'))
        for path in (p,before_path,after_path):evidence[str(path)]=sha(path)
    result=dict(veteran_references=rows,archmage_observations=probes,source_evidence_sha256=evidence,
        omissions=['Reward pickup and ordinary corpse contents/value in references','Petals and antivenom ingredients/trade value','Repairs, travel, time and equipment acquisition','Actual checkout, tax and NPC stock','Repeated farming, enjoyment and market demand'],
        interpretation='Finite consumption and nominal base-price replacement only. Tactical kiting is not counted as a Sacred Journey retreat. Pending rewards and ground awards are separate from collected backpack items. No profit, comparative antivenom causality or market value claim.',
        gameplay_replayed=False,production_activation=False,site_approval=False)
    assert not output.exists(),'Use a fresh observation output'
    output.write_text(json.dumps(result,indent=2)+'\n');print('Recorded three completed references and two bounded archmage observations; no replay or profit inference.')


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('--output',type=Path,required=True);record(p.parse_args().output)
