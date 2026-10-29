"""Configure only a fresh disposable client for bounded original-macro preparation."""
from pathlib import Path
import argparse
import json
import shutil
import uuid
from PrepareNativeAcceptance import sha


def prepare(root, snapshot, inspection):
    repo=Path(__file__).resolve().parents[2]
    root=root.resolve();snapshot=snapshot.resolve();evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='archmage-preparation'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert not (evidence/'archmage-state.xml').exists(),'Configure before launching the private server'
    provenance=json.loads((snapshot/'source-provenance.private.json').read_text(encoding='utf-8-sig'))
    for original,record in provenance.items():
        assert sha(Path(original))==record['sha256']
        assert sha(snapshot/Path(original).name)==record['sha256']
    destination=root/'client/ClassicAssist/PrivateArchmage'
    assert not destination.exists();destination.mkdir()
    for name in ('archmage-overlord.py','archmage_combat.py'):
        shutil.copy2(snapshot/name,destination/name)
    driver=root/'client/ClassicAssist/ArchmagePreparationClient.py'
    shutil.copy2(repo/'tools/champion-outbreak/ArchmagePreparationClient.py',driver)
    personal=json.loads((snapshot/'Zeph.json').read_text(encoding='utf-8-sig'))
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
    files=[destination/'archmage-overlord.py',destination/'archmage_combat.py',
           driver,profile,settings_path,root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs']
    freeze=dict(files={str(p.relative_to(root)):sha(p) for p in files},
                original_provenance=provenance,
                source_character_inspection_sha256=sha(inspection),
                source_profile_sha256=sha(snapshot/'Zeph.json'),
                ignore_list_option=personal['Options'].get('GetFriendEnemyUsesIgnoreList'),
                scope='Original maintenance code and gameplay selection settings; first post-maintenance enemy scan withheld; no encounter.')
    (evidence/'archmage-input-freeze.private.json').write_text(json.dumps(freeze,indent=2),encoding='utf-8')
    print('Private original macro/module, gameplay options, wrapper, fixture and loopback client frozen; personal autologin excluded.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--snapshot',required=True,type=Path);parser.add_argument('--inspection',required=True,type=Path)
    args=parser.parse_args();prepare(args.root,args.snapshot,args.inspection)
