"""Freeze a ready copied benchmark and optionally request its first real combat run."""
from pathlib import Path
import argparse
import hashlib
import json
import os
import shutil
import time
import uuid
import xml.etree.ElementTree as ET


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def read_status(path):
    # A Windows reader can collide with the observer's atomic replacement.
    for attempt in range(10):
        try:
            return ET.fromstring(path.read_bytes())
        except PermissionError:
            if attempt == 9:
                raise
            time.sleep(.1)


def capability_signature(path):
    reference = ET.parse(path).getroot()
    fields = ('access','race','rawStr','rawDex','rawInt','str','dex','int','hitsMax','stamMax','manaMax',
              'skillCap','baseSkillTotal','physical','fire','cold','poison','energy','fame','karma',
              'tithing','skillOrigin','skillBoost','skillEther')
    equipment = []
    for item in reference.findall('equipment'):
        attributes = sorted(tuple(sorted(n.attrib.items())) for n in item.findall('attribute'))
        properties = sorted(tuple(sorted(n.attrib.items())) for n in item.findall('property')
                            if n.get('name') not in ('HitPoints','MaxHitPoints'))
        equipment.append((item.get('type'),item.get('layer'),attributes,properties))
    return dict(character={k:reference.get(k) for k in fields},
                skills=sorted(tuple(sorted(n.attrib.items())) for n in reference.findall('skill')),
                equipment=sorted(equipment))


def freeze(args):
    repo = Path(__file__).resolve().parents[2]
    root = args.root.resolve()
    evidence = root/'evidence'
    prepared = json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture'] == 'benchmark'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve() == root/'shard'
    state = read_status(evidence/'benchmark-state.xml')
    assert state.get('state') == 'ready' and state.get('online') == 'True'
    assert state.get('seconds') == '0' and state.get('blessed') == 'True' and state.get('alive') == 'True'
    inputs = json.loads((Path(prepared['candidate'])/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/p) == expected for p, expected in inputs.items()), 'Production source changed'
    files = dict(reference_xml=evidence/'benchmark-reference.xml',
                 fixture=repo/'tests/ArachnidOutbreakBenchmark.cs',
                 client_policy=repo/'tools/champion-outbreak/BenchmarkClient.py',
                 policy_document=repo/'docs/champion-outbreak/BENCHMARK.md')
    plan = ET.Element('routes')
    for route in state.findall('route'):
        plan.append(route)
    if len(plan):
        route_file = evidence/'benchmark-routes.xml'
        assert not route_file.exists(), 'Never overwrite an existing navigation plan'
        ET.ElementTree(plan).write(route_file,encoding='utf-8',xml_declaration=True)
        files['navigation_plan'] = route_file
    assert sha(files['fixture']) == sha(root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakBenchmark.cs')
    assert sha(files['client_policy']) == sha(root/'client/ClassicAssist/OutbreakBenchmarkClient.py')
    comparison = None
    if args.compare_reference:
        comparison = args.compare_reference.resolve()
        assert capability_signature(comparison) == capability_signature(files['reference_xml']), 'Reference capabilities changed'
    destination = evidence/'reference-freeze.json'
    snapshots = evidence/'frozen-inputs'
    assert not destination.exists() and not snapshots.exists(), 'Never overwrite a frozen run'
    snapshots.mkdir()
    for key, path in files.items():
        shutil.copy2(path, snapshots/(key+path.suffix))
    frozen = dict(utc=state.get('utc'),state='ready; no combat yet',policy=args.policy,
                  production_input_count=len(inputs),
                  compared_reference=None if comparison is None else str(comparison),
                  capability_comparison='Same capabilities; ordinary durability rolls and NPC randomness retained',
                  files={key:dict(path=str(path),sha256=sha(path)) for key,path in files.items()})
    destination.write_text(json.dumps(frozen,indent=2),encoding='utf-8')
    if args.start:
        command = evidence/'benchmark-command.txt'
        assert not command.exists(), 'Another command is pending'
        temporary = evidence/(str(uuid.uuid4())+'.tmp')
        temporary.write_text('start-'+args.start,encoding='utf-8')
        os.replace(temporary,command)
    print('Frozen actual reference, source, policy and snapshots before combat; start request: '+str(args.start))


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('root',type=Path)
    parser.add_argument('--policy',required=True)
    parser.add_argument('--compare-reference',type=Path)
    parser.add_argument('--start',choices=['no-antivenom','antivenom','navigation'])
    freeze(parser.parse_args())
