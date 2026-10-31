"""Configure a fresh stationary actual-client healing probe; no encounter or character grants."""
from pathlib import Path
import argparse,json,shutil,subprocess,uuid
from PrepareSavedBuildClient import prepare
from PrepareNativeAcceptance import sha


def configure(root,serial):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];e=root/'evidence'
    prepare(root,serial,'melee',[])
    marker=e/'weak-frontier-case.txt';assert marker.parent==e;marker.unlink()
    (e/'archmage-mode.txt').write_text('preparation');(e/'healing-probe-case.txt').write_text('Declared 40-point self-injury, no encounter, no health/mana/stat/gear/supply grants; actual saved Close Wounds client timing')
    harness=root/'shard/Data/Scripts/TestOnly/CloseWoundsClientProbe.cs';shutil.copy2(repo/'tests'/harness.name,harness)
    driver=root/'client/ClassicAssist/CloseWoundsProbeClient.py';shutil.copy2(repo/'tools/champion-outbreak'/driver.name,driver)
    profile=root/'client/ClassicAssist/Profiles/settings.json';data=json.loads(profile.read_text(encoding='utf-8-sig'))
    for macro in data['Macros']['Macros']:macro['IsAutostart']=False
    name='Private actual Close Wounds timing probe'
    data['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,Disableable=True,Group='',Global=False,Keys={},Macro='OUTBREAK_ROOT = '+repr(str(root))+'\nexecfile('+repr(str(driver))+')\n'))
    data['Macros']['Selected']=name;profile.write_text(json.dumps(data,indent=2))
    p=e/'saved-build-input-freeze.private.json';f=json.loads(p.read_text());f['files'].pop('evidence/weak-frontier-case.txt');f['files'].update({q.relative_to(root).as_posix():sha(q) for q in [harness,driver,e/'healing-probe-case.txt',e/'archmage-mode.txt',profile]});f['scope']='Stationary actual saved vulnerable fighter; declared 40-point self-injury; normal client Close Wounds with finite saved mana/tithing. No encounter, pacing or balance acceptance.';p.write_text(json.dumps(f,indent=2))
    result=subprocess.run(['powershell','-NoProfile','-File',str(repo/'scripts/Generate-TurnBasedCombatCompatibility.ps1'),'-RepositoryRoot',str(root/'shard')],capture_output=True,text=True,timeout=120,check=True)
    (e/'healing-probe-compatibility.log').write_text(result.stdout+result.stderr);assert 'Unknown dispositions: 0' in result.stdout
    print('Stationary actual-client probe frozen; declared injury, no grants, no process started.')


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('root',type=Path);p.add_argument('--serial',type=int,required=True);args=p.parse_args();configure(args.root,args.serial)
