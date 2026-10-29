"""Load a fresh private copy of the selected offline world; never initialize a server."""
from pathlib import Path
import argparse
import json
import os
import shutil
import subprocess
import xml.etree.ElementTree as ET
from VerifyBenchmark import sha, hashes


def run(args):
    repo=Path(__file__).resolve().parents[2]
    workspace=Path('D:/ConficturaUOTestTools').resolve()
    root=args.output.resolve();candidate=args.candidate.resolve()
    saves=args.saves.resolve();baseline=args.baseline.resolve();assets=args.assets.resolve()
    for path in (root,candidate,saves,baseline,assets):
        assert path.is_relative_to(workspace) and path!=workspace,'Private test workspace required'
    assert root.name.startswith('outbreak-reserve-preflight-') and not root.exists(),'Use a fresh private root'
    assert saves.name=='Saves' and saves.is_dir()
    inputs=json.loads((candidate/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/name)==expected for name,expected in inputs.items()),'Candidate does not match current production inputs'
    assert sha(candidate/'Data.dll')==sha(candidate/'Data/Data.bin'),'Script assembly copies differ'
    before=hashes(saves);assert before,'Empty selected snapshot'
    root.mkdir()
    shutil.copytree(saves,root/'Saves')
    def ignored(directory,names):
        return [n for n in names if n in ('Scripts','Files','Source','Logs','logs','Backups','bin','obj','.git')
                or n in ('Data.bin','Data.hash','Data.hashes','Data.pdb') or n.endswith('.disabled')]
    shutil.copytree(baseline/'Data',root/'Data',ignore=ignored)
    regions=root/'Data/System/XML/Regions.xml';regions.parent.mkdir(parents=True,exist_ok=True)
    shutil.copy2(repo/'Data/System/XML/Regions.xml',regions)
    for path in candidate.iterdir():
        if path.is_file() and (path.suffix.lower()=='.dll' or path.name in ('ConficturaServer.exe','ConficturaServer.exe.config')):
            shutil.copy2(path,root/path.name)
    source=repo/'tools/champion-outbreak/LoadedReservationPreflight.cs'
    shutil.copy2(source,root/source.name)
    assert hashes(root/'Saves')==before==hashes(saves),'Snapshot changed during preparation'
    record=dict(selected_snapshot='2026-10-17 offline test world, explicitly selected by user',
                source_saves=str(saves),source_saves_sha256=before,candidate=str(candidate),
                current_commit=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),
                production_inputs=inputs,core_sha256=sha(root/'ConficturaServer.exe'),
                scripts_sha256=sha(root/'Data.dll'),probe_sha256=sha(source),
                regions_sha256=sha(regions),assets=str(assets),production_activation=False,
                scope='Hypothetical reservation clearance in the selected snapshot; no live occupancy or geography approval')
    (root/'preflight-inputs.json').write_text(json.dumps(record,indent=2))
    compiler=Path(os.environ['WINDIR'])/'Microsoft.NET/Framework/v4.0.30319/csc.exe'
    executable=root/'LoadedReservationPreflight.exe'
    command=[str(compiler),'/nologo','/target:exe','/platform:x86','/out:'+str(executable),
             '/reference:'+str(root/'ConficturaServer.exe'),'/reference:'+str(root/'Data.dll'),str(root/source.name)]
    built=subprocess.run(command,cwd=root,capture_output=True,text=True,timeout=60,creationflags=subprocess.CREATE_NO_WINDOW)
    (root/'compile.log').write_text(built.stdout+built.stderr)
    (root/'compile-result.json').write_text(json.dumps(dict(exit_code=built.returncode,
        selected_and_private_saves_unchanged=hashes(root/'Saves')==before==hashes(saves)),indent=2))
    assert built.returncode==0,'Probe compile failed; retain compile.log'
    with (root/'run.log').open('w') as out,(root/'run.stderr.log').open('w') as err:
        completed=subprocess.run([str(executable),str(assets)],cwd=root,stdout=out,stderr=err,timeout=120,creationflags=subprocess.CREATE_NO_WINDOW)
    preserved=(hashes(root/'Saves')==before==hashes(saves))
    unchanged=(sha(root/'ConficturaServer.exe')==record['core_sha256']
               and sha(root/'Data.dll')==record['scripts_sha256'] and sha(regions)==record['regions_sha256']
               and all(sha(repo/name)==expected for name,expected in inputs.items()))
    (root/'preservation.json').write_text(json.dumps(dict(selected_and_private_saves_unchanged=preserved,
        assemblies_regions_and_production_inputs_unchanged=unchanged,probe_exit_code=completed.returncode,
        server_initialize_or_listening_started=False,world_saved=False),indent=2))
    assert preserved and unchanged,'Input preservation failed; retain copy without restoring over source'
    assert completed.returncode==0,'Loaded probe failed; inspect retained private logs/XML'
    xml=ET.parse(root/'preflight.xml').getroot();rows=[dict(n.attrib) for n in xml.findall('reserve')]
    assert len(rows)==6 and len({n['id'] for n in rows})==6 and all(n['passed']=='True' for n in rows)
    assert xml.get('hypotheticalReservationsOnly')=='True' and xml.get('productionActivation')=='False'
    assert xml.get('activeMalformedLegacyControllers')=='0'
    report=dict(selected_snapshot_preflight_passed=True,selected_snapshot_date='2026-10-17',
                production_inputs_checked=len(inputs),selected_save_files=len(before),
                core_sha256=record['core_sha256'],scripts_sha256=record['scripts_sha256'],regions_sha256=record['regions_sha256'],
                static_sosaria_regions=int(xml.get('staticSosariaRegions')),
                inactive_malformed_legacy_controllers=int(xml.get('malformedLegacyControllers')),
                rows=rows,inputs_preserved=True,site_or_reservation_approval=False,production_activation=False,
                scope=record['scope'])
    (root/'preflight-verification.json').write_text(json.dumps(report,indent=2))
    print(json.dumps(report,indent=2))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    for name in ('output','candidate','saves','baseline','assets'):parser.add_argument('--'+name,type=Path,required=True)
    run(parser.parse_args())
