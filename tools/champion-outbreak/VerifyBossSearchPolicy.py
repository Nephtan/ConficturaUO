"""Check remembered-position priority and geometric client-view coverage, without claiming terrain access."""
from pathlib import Path
import argparse
import ast
import hashlib
import json


def verify(output):
    source=Path(__file__).with_name('BenchmarkClient.py').read_bytes()
    tree=ast.parse(source);function=next(n for n in tree.body if isinstance(n,ast.FunctionDef) and n.name=='boss_reacquisition_goals')
    scope={};exec(compile(ast.Module(body=[function],type_ignores=[]),'<boss-search-planner>','exec'),scope)
    goals=scope['boss_reacquisition_goals'];center=(2514,2088,0)
    remembered=(2520,2104,0);actual=(2526,2119,0)
    route=goals(center,remembered)
    assert route[0]==remembered+(1,)
    assert max(abs(center[i]-actual[i]) for i in range(2))>18
    assert max(abs(route[0][i]-actual[i]) for i in range(2))<=18
    grid=goals(center,None);worst=0;points=0
    assert grid and all(max(abs(p[i]-center[i]) for i in range(2))<=56 for p in grid)
    for x in range(center[0]-56,center[0]+57):
        for y in range(center[1]-56,center[1]+57):
            distance=min(max(abs(p[0]-x),abs(p[1]-y)) for p in grid)
            assert distance<=18
            worst=max(worst,distance);points+=1
    assert goals(center,(0,0,0))==grid
    result=dict(remembered_client_location_first=True,illustrative_memory_priority_case=True,
        arena_tiles_checked=points,worst_nearest_search_distance=worst,outside_memory_ignored=True,
        client_source_sha256=hashlib.sha256(source).hexdigest(),
        scope='Retained actor/boss separation with a synthetic remembered point; planner geometry only; no line-of-sight, native terrain, enemy movement or combat acceptance')
    assert not output.exists();output.write_text(json.dumps(result,indent=2));print(json.dumps(result))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('output',type=Path)
    verify(parser.parse_args().output)
