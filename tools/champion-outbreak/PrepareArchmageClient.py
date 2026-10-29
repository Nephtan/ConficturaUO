"""Configure only a fresh disposable client for bounded original-macro preparation."""
from pathlib import Path
import argparse
import ast
import json
import shutil
import uuid
from PrepareNativeAcceptance import sha


def prepare(root, snapshot, inspection, mode='preparation'):
    repo=Path(__file__).resolve().parents[2]
    root=root.resolve();snapshot=snapshot.resolve();evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='archmage-preparation'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert not (evidence/'archmage-state.xml').exists(),'Configure before launching the private server'
    mode_path=evidence/'archmage-mode.txt';assert not mode_path.exists()
    assert mode in ('preparation','stationary','route');mode_path.write_text(mode,encoding='utf-8')
    if mode!='preparation':
        shutil.copy2(evidence/'prepared.json',evidence/'prepared-before-mode-selection.json')
        prepared['scope']='Actual historical copied Zanthus and original macro; '+mode+' normal-AI combat; no grants/production activation.'
        (evidence/'prepared.json').write_text(json.dumps(prepared,indent=2),encoding='utf-8')
    provenance=json.loads((snapshot/'source-provenance.private.json').read_text(encoding='utf-8-sig'))
    for original,record in provenance.items():
        assert sha(Path(original))==record['sha256']
        assert sha(snapshot/Path(original).name)==record['sha256']
    destination=root/'client/ClassicAssist/PrivateArchmage'
    assert not destination.exists();destination.mkdir()
    for name in ('archmage-overlord.py','archmage_combat.py'):
        shutil.copy2(snapshot/name,destination/name)
    if mode=='route':
        source=(repo/'tools/champion-outbreak/BenchmarkClient.py').read_text()
        names={'log','value','local_goals','boss_reacquisition_goals','navigate'}
        functions=[ast.get_source_segment(source,n) for n in ast.parse(source).body if isinstance(n,ast.FunctionDef) and n.name in names]
        assert len(functions)==len(names)
        (destination/'EncounterNavigation.py').write_text('# Selected unchanged helpers from BenchmarkClient.py\n'+'\n\n'.join(functions)+'\n',encoding='utf-8')
    driver=root/'client/ClassicAssist/ArchmagePreparationClient.py'
    shutil.copy2(repo/'tools/champion-outbreak/ArchmagePreparationClient.py',driver)
    profiles=[Path(name).name for name in provenance if Path(name).suffix.lower()=='.json']
    assert len(profiles)==1,'Source provenance must identify one selected profile'
    personal_profile=snapshot/profiles[0]
    personal=json.loads(personal_profile.read_text(encoding='utf-8-sig'))
    entries=personal['Macros']['Macros']
    selected=next(n for n in entries if n['Name']==personal['Macros']['Selected'])
    assert selected['Name']=='archmage-overlord' and Path(selected['FilePath']).name=='archmage-overlord.py'
    profile=root/'client/ClassicAssist/Profiles/settings.json'
    copied=json.loads(profile.read_text(encoding='utf-8-sig'))
    # Copy gameplay selection behavior, never personal autologin or unrelated macros.
    for section in ('Options','Friends'):
        copied[section]=personal[section]
    copied['General']['Autologin']=False
    copied['General']['AutologinUsername']='';copied['General']['AutologinPassword']=''
    for macro in copied['Macros']['Macros']:macro['IsAutostart']=False
    name='Private original archmage preparation'
    copied['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,
        DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,
        Disableable=True,Group='',Global=False,Keys={},
        Macro=f'OUTBREAK_ROOT = {str(root)!r}\nexecfile({str(driver)!r})\n'))
    copied['Macros']['Selected']=name
    profile.write_text(json.dumps(copied,indent=2),encoding='utf-8')
    settings_path=root/'client/settings.json'
    settings=json.loads(settings_path.read_text(encoding='utf-8-sig'))
    assert settings['ip']=='127.0.0.1' and settings['port']==prepared['port']
    plugin=root/'client/ClassicAssist/ClassicAssist.dll';assert plugin.is_file()
    settings['plugins']=[str(plugin)]
    settings_path.write_text(json.dumps(settings,indent=2),encoding='utf-8')
    shutil.copy2(inspection,evidence/'source-character-inspection.private.xml')
    files=[destination/'archmage-overlord.py',destination/'archmage_combat.py',mode_path,
           driver,profile,settings_path,root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs']
    if mode=='route':
        from FreezeBenchmark import movement_policy
        movement=movement_policy(root);path=root/movement['path']
        data=json.loads(path.read_text(encoding='utf-8-sig'));data['always_run']=True
        path.write_text(json.dumps(data,indent=2),encoding='utf-8')
        files.append(destination/'EncounterNavigation.py')
    freeze=dict(files={str(p.relative_to(root)):sha(p) for p in files},
                original_provenance=provenance,
                source_character_inspection_sha256=sha(inspection),
                source_profile_sha256=sha(personal_profile),
                ignore_list_option=personal['Options'].get('GetFriendEnemyUsesIgnoreList'),
                mode=mode,navigation_source_sha256=(sha(repo/'tools/champion-outbreak/BenchmarkClient.py') if mode=='route' else None),
                movement_policy=(movement_policy(root) if mode=='route' else None),
                scope='Original code/settings; frozen start; navigation at idle scan boundaries only for explicit route mode.')
    (evidence/'archmage-input-freeze.private.json').write_text(json.dumps(freeze,indent=2),encoding='utf-8')
    print('Private original macro/module, gameplay options, wrapper, fixture and loopback client frozen; personal autologin excluded.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--snapshot',required=True,type=Path);parser.add_argument('--inspection',required=True,type=Path)
    parser.add_argument('--mode',choices=['preparation','stationary','route'],default='preparation')
    args=parser.parse_args();prepare(args.root,args.snapshot,args.inspection,args.mode)
