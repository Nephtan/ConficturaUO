"""Check bounded web-dodge and local-reacquisition invariants without importing the game client."""
from pathlib import Path
import ast
import hashlib
import json
import argparse


def verify(output):
    source=Path(__file__).with_name('BenchmarkClient.py').read_bytes()
    tree=ast.parse(source)
    functions=[n for n in tree.body if isinstance(n,ast.FunctionDef) and n.name in ('local_goals','dodge_goals')]
    assert len(functions)==2
    scope={};exec(compile(ast.Module(body=functions,type_ignores=[]),'navigation-policy','exec'),scope)
    center=(2514,2088);position=center
    for _ in range(1000):
        point=scope['dodge_goals'](*position,0,*center)[0];position=point[:2]
        assert max(abs(position[i]-center[i]) for i in range(2))<=2
    position=(2572,2098,0);goal=(2514,2102,0,1);hops=0
    while max(abs(position[i]-goal[i]) for i in range(2))>24:
        candidates=scope['local_goals'](position,goal)
        assert all(max(abs(p[i]-position[i]) for i in range(2))<=6 for p in candidates)
        point=candidates[0]
        assert sum((point[i]-goal[i])**2 for i in range(2))<sum((position[i]-goal[i])**2 for i in range(2))
        position=point[:3];hops+=1;assert hops<20
    assert scope['local_goals'](position,goal)[0]==goal
    near=(2491,2105,2);near_goal=(2513,2107,0,1)
    candidates=scope['local_goals'](near,near_goal)
    assert candidates[0]==near_goal and len(candidates)>1
    assert all(max(abs(p[i]-near[i]) for i in range(2))<=6 for p in candidates[1:])
    report=dict(bounded_simulated_dodges=1000,reacquisition_local_hops=hops,short_goal_preserved=True,rejected_near_goal_has_local_alternatives=True,
                client_source_sha256=hashlib.sha256(source).hexdigest(),scope='Planner invariants only; native walking and combat remain separate')
    assert not output.exists();output.write_text(json.dumps(report,indent=2));print(json.dumps(report))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('output',type=Path)
    verify(parser.parse_args().output)
