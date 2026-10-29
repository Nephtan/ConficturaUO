"""Configure only an unstarted disposable acceptance copy for the live scheduler case."""
from pathlib import Path
import argparse
import json
import shutil
import uuid
from VerifyBenchmark import sha

def prepare(root,inspection=None,case='escape',observe_warning=False):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];e=root/'evidence'
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert json.loads((e/'prepared.json').read_text())['fixture']=='acceptance'
    assert not (e/'native-ready.txt').exists(),'Configure before private server startup'
    assert not (e/'native-server-pid.txt').exists(),'Configure before launching the private server'
    assert case in ('escape','dodge') and (not observe_warning or case=='dodge')
    extra=[]
    case_file=e/'live-turn-case.json';assert not case_file.exists()
    case_file.write_text(json.dumps(dict(case=case,observe_warning=observe_warning),indent=2));extra.append(case_file)
    if inspection is not None:
        selection=e/'native-actor.txt';assert not selection.exists();selection.write_text('Zanthus')
        copied=e/'native-actor-inspection.private.xml';shutil.copy2(inspection,copied);extra.extend([selection,copied])
    driver_name='LiveDodgeClient.py' if case=='dodge' else 'LiveTurnClient.py'
    driver=root/'client/ClassicAssist'/driver_name;assert not driver.exists()
    shutil.copy2(repo/'tools/champion-outbreak'/driver_name,driver)
    profile=root/'client/ClassicAssist/Profiles/settings.json';data=json.loads(profile.read_text(encoding='utf-8-sig'))
    data['General']['Autologin']=False
    for macro in data['Macros']['Macros']:macro['IsAutostart']=False
    name='Private natural outbreak turn case'
    data['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,Disableable=True,Group='',Global=False,Keys={},Macro='OUTBREAK_ROOT = '+repr(str(root))+'\nexecfile('+repr(str(driver))+')\n'))
    data['Macros']['Selected']=name;profile.write_text(json.dumps(data,indent=2))
    settings=root/'client/settings.json';data=json.loads(settings.read_text(encoding='utf-8-sig'))
    plugin=root/'client/ClassicAssist/ClassicAssist.dll';assert plugin.is_file()
    data['plugins']=[str(plugin)];settings.write_text(json.dumps(data,indent=2))
    (e/'live-turn-input-freeze.json').write_text(json.dumps(dict(case=case,client_policy=driver_name,files={str(p.relative_to(root)):sha(p) for p in [driver,profile,settings,root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakNativeAcceptance.cs']+extra},scope='Connected copied client actions; natural scheduler/production AI; explicit test-arm enrollment only'),indent=2))
    print('Private live-turn client frozen; no process started.')

if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);parser.add_argument('--zanthus-inspection',type=Path)
    parser.add_argument('--case',choices=['escape','dodge'],default='escape');parser.add_argument('--observe-warning',action='store_true')
    args=parser.parse_args();prepare(args.root,args.zanthus_inspection,args.case,args.observe_warning)
