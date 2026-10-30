"""Extend an unstarted private acceptance copy for staged rendering inspection only."""
from pathlib import Path
import argparse
import json
import shutil
import subprocess
from VerifyBenchmark import sha


def prepare(root,inspection):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];e=root/'evidence'
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert json.loads((e/'prepared.json').read_text())['fixture']=='acceptance'
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert not (e/'native-server-pid.txt').exists() and not (e/'native-ready.txt').exists()
    assert not (e/'presentation-inputs.json').exists()
    source=repo/'tests/ArachnidOutbreakPresentation.cs'
    target=root/'shard/Data/Scripts/TestOnly'/source.name;assert not target.exists()
    shutil.copy2(source,target)
    actor=e/'native-actor.txt';assert not actor.exists();actor.write_text('Zanthus')
    copied=e/'native-actor-inspection.private.xml';shutil.copy2(inspection,copied)
    settings=json.loads((root/'client/settings.json').read_text(encoding='utf-8-sig'));assert settings['plugins']==[]
    generated=subprocess.run(['powershell','-NoProfile','-File',str(repo/'scripts/Generate-TurnBasedCombatCompatibility.ps1'),
        '-RepositoryRoot',str(root/'shard')],capture_output=True,text=True,timeout=120,check=True)
    (e/'presentation-compatibility.log').write_text(generated.stdout+generated.stderr)
    assert 'Unknown dispositions: 0' in generated.stdout
    record=dict(current_commit=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),
        files={str(p.relative_to(root)):sha(p) for p in (target,actor,copied,root/'client/settings.json',root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakNativeAcceptance.cs')},
        scope='Staged camera relocation and production warning graphics with AI/turn scheduler stopped; rendering only, no natural timing/AI/movement/pacing approval',
        production_activation=False,site_approval=False)
    (e/'presentation-inputs.json').write_text(json.dumps(record,indent=2))
    print('Staged presentation inputs frozen; plugins disabled; no process launched.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);parser.add_argument('--inspection',required=True,type=Path)
    args=parser.parse_args();prepare(args.root,args.inspection.resolve())
