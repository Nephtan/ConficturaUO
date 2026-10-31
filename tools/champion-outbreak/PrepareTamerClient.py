"""Configure only a fresh disposable Sargar client; no original profiles or saved characters change."""
from pathlib import Path
import argparse,ast,json,shutil,uuid
from PrepareNativeAcceptance import sha


def prepare(root):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text())
    assert prepared['fixture']=='archmage-preparation' and not(evidence/'archmage-state.xml').exists()
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    for name,text in [('archmage-mode.txt','route'),('active-build.txt','Sargar'),('infestation-case.txt','Persistent infestation; actual saved tamer')]:
        assert not(evidence/name).exists();(evidence/name).write_text(text)
    folder=root/'client/ClassicAssist/PrivateTamer';folder.mkdir()
    source=(repo/'tools/champion-outbreak/BenchmarkClient.py').read_text()
    names={'local_goals','boss_reacquisition_goals','navigate'}
    functions=[ast.get_source_segment(source,node) for node in ast.parse(source).body if isinstance(node,ast.FunctionDef) and node.name in names]
    assert len(functions)==len(names)
    helper=folder/'EncounterNavigation.py';helper.write_text('# Unchanged native movement helpers from BenchmarkClient.py\n'+'\n\n'.join(functions)+'\n')
    driver=root/'client/ClassicAssist/TamerPreparationClient.py';shutil.copy2(repo/'tools/champion-outbreak'/driver.name,driver)
    profile=root/'client/ClassicAssist/Profiles/settings.json';data=json.loads(profile.read_text(encoding='utf-8-sig'))
    for macro in data['Macros']['Macros']:macro['IsAutostart']=False
    if 'General' in data:
        data['General']['Autologin']=False;data['General']['AutologinUsername']='';data['General']['AutologinPassword']=''
    name='Private saved tamer acceptance'
    data['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,Disableable=True,Group='',Global=False,Keys={},Macro=f'OUTBREAK_ROOT = {str(root)!r}\nexecfile({str(driver)!r})\n'))
    data['Macros']['Selected']=name;profile.write_text(json.dumps(data,indent=2))
    settings=root/'client/settings.json';data=json.loads(settings.read_text(encoding='utf-8-sig'))
    assert data['ip']=='127.0.0.1' and data['port']==prepared['port'];plugin=root/'client/ClassicAssist/ClassicAssist.dll';assert plugin.exists()
    data['plugins']=[str(plugin)];settings.write_text(json.dumps(data,indent=2))
    files=[driver,helper,profile,settings,root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs']+[evidence/name for name in ('archmage-mode.txt','active-build.txt','infestation-case.txt')]
    (evidence/'tamer-input-freeze.private.json').write_text(json.dumps(dict(files={p.relative_to(root).as_posix():sha(p) for p in files},scope='Actual historical copied Sargar; actual saved CragCats restored by normal login or trainer claims within existing follower capacity; archery, pet commands, finite potions and veterinary bandages; ordinary client walking. No grants or human-optimality claim.'),indent=2))
    print('Configured fresh private tamer client and froze finite-action driver; no processes launched.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);prepare(parser.parse_args().root)
