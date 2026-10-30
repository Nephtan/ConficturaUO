"""Check final source/project/artifact inventory; this is not runtime or gameplay verification."""
from pathlib import Path
import argparse
import csv
import json
import re
from urllib.parse import unquote
import xml.etree.ElementTree as ET
from VerifyBenchmark import sha


def check(candidate,output):
    repo=Path(__file__).resolve().parents[2]
    inputs=json.loads((candidate/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    for name,expected in inputs.items():assert sha(repo/name)==expected,name
    assert sha(repo/'ConficturaServer.exe')==sha(candidate/'ConficturaServer.exe')
    project=ET.parse(repo/'Data/Scripts/Scripts.csproj').getroot()
    included={unquote(x.get('Include')).replace('\\','/') for x in project.iter() if x.tag.rsplit('}',1)[-1]=='Compile'}
    scripts=sorted((repo/'Data/Scripts/Custom/Champions/Outbreak').glob('*.cs'))
    for path in scripts:assert path.relative_to(repo/'Data/Scripts').as_posix() in included,path
    with (repo/'docs/champion-outbreak/ACCEPTANCE_REQUIREMENTS.csv').open(newline='') as source:rows=list(csv.DictReader(source))
    assert len(rows)==44 and len({r['id'] for r in rows})==44
    for row in rows:
        for field in ('implementation_source','test_source'):assert (repo/row[field]).is_file(),(row['id'],field)
    reserve=repo/'Data/Scripts/Custom/Champions/Outbreak/WildernessEncounterReserve.cs'
    proposals=re.findall(r'new WildernessEncounterReserve\("([^"]+)", (\d+), (\d+), (false|true)\)',reserve.read_text())
    with (repo/'docs/champion-outbreak/reservation-proposals.csv').open(newline='') as source:places=list(csv.DictReader(source))
    assert len(proposals)==6 and all(flag=='false' for _,_,_,flag in proposals)
    assert {(i,int(x),int(y)) for i,x,y,_ in proposals}=={(x['id'],int(x['x']),int(x['y'])) for x in places}
    controller=(repo/'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs').read_text()
    assert 'public static bool DeliveryReady { get { return false; } }' in controller
    catalog=(repo/'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakSites.cs').read_text()
    assert 'new ArachnidOutbreakSite(' not in catalog
    for fixture in ('ArachnidOutbreakCooperative.cs','ArachnidOutbreakPresentation.cs'):
        assert (repo/'tests'/fixture).is_file()
        assert not any((repo/'Data/Scripts').rglob(fixture))
    record=dict(current_production_inputs_checked=len(inputs),outbreak_runtime_scripts_in_project=len(scripts),
        requirement_source_test_paths_checked=len(rows),tracked_core_sha256=sha(repo/'ConficturaServer.exe'),
        source_activation=False,approved_reservations=0,active_catalog_entries=0,
        proposed_coordinates_match=True,test_fixtures_outside_runtime_tree=True,
        scope='Static source/project/artifact inventory only; runtime compilation, persistence and gameplay retain their separate recorded evidence',
        pending_decision='Explicit permanent reservation/site approval; no source flag changed')
    assert not output.exists(),'Use a fresh inventory output'
    output.write_text(json.dumps(record,indent=2)+'\n');print(json.dumps(record,indent=2))


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('--candidate',type=Path,required=True);p.add_argument('--output',type=Path,required=True)
    args=p.parse_args();check(args.candidate,args.output)
