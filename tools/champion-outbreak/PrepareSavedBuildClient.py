"""Configure only a fresh disposable representative saved-build client; no original profiles or saved characters change."""
from pathlib import Path
import argparse,ast,json,shutil,uuid
from PrepareNativeAcceptance import sha


def prepare(root, serial, policy, clones):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text())
    assert prepared['fixture']=='archmage-preparation' and not(evidence/'archmage-state.xml').exists()
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert not(evidence/'server-process.json').exists(),'Never refresh a launched fixture'
    harness=root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs'
    shutil.copy2(repo/'tests/ArachnidArchmagePreparation.cs',harness)
    prepared['harness_sha256']=sha(harness);prepared['prelaunch_fixture_refresh']='Latest test-only fixture copied before launch and input freeze; production inputs unchanged'
    (evidence/'prepared.json').write_text(json.dumps(prepared,indent=2))
    for name,text in [('archmage-mode.txt','route'),('active-build-serial.private',str(serial)),('infestation-case.txt','Persistent infestation; actual saved representative'),('weak-frontier-case.txt','Natural tier-zero daughter, no moved founders or tier overrides')]:
        assert not(evidence/name).exists();(evidence/name).write_text(text)
    if clones:
        (evidence/'mixed-clones.private').write_text('\n'.join(str(n) for n in clones))
    (evidence/'saved-build-policy.txt').write_text(policy)
    folder=root/'client/ClassicAssist/PrivateTamer';folder.mkdir()
    source=(repo/'tools/champion-outbreak/BenchmarkClient.py').read_text()
    names={'local_goals','boss_reacquisition_goals','navigate'}
    functions=[ast.get_source_segment(source,node) for node in ast.parse(source).body if isinstance(node,ast.FunctionDef) and node.name in names]
    assert len(functions)==len(names)
    helper=folder/'EncounterNavigation.py';helper.write_text('# Unchanged native movement helpers from BenchmarkClient.py\n'+'\n\n'.join(functions)+'\n')
    driver=root/'client/ClassicAssist/SavedBuildPreparationClient.py';shutil.copy2(repo/'tools/champion-outbreak'/driver.name,driver)
    profile=root/'client/ClassicAssist/Profiles/settings.json';data=json.loads(profile.read_text(encoding='utf-8-sig'))
    for macro in data['Macros']['Macros']:macro['IsAutostart']=False
    if 'General' in data:
        data['General']['Autologin']=False;data['General']['AutologinUsername']='';data['General']['AutologinPassword']=''
    name='Private representative frontier acceptance'
    data['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,Disableable=True,Group='',Global=False,Keys={},Macro=f'OUTBREAK_ROOT = {str(root)!r}\nexecfile({str(driver)!r})\n'))
    data['Macros']['Selected']=name;profile.write_text(json.dumps(data,indent=2))
    settings=root/'client/settings.json';data=json.loads(settings.read_text(encoding='utf-8-sig'))
    assert data['ip']=='127.0.0.1' and data['port']==prepared['port'];plugin=root/'client/ClassicAssist/ClassicAssist.dll';assert plugin.exists()
    data['plugins']=[str(plugin)];settings.write_text(json.dumps(data,indent=2))
    files=[driver,helper,profile,settings,root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs']+([evidence/'mixed-clones.private'] if clones else [])+[evidence/name for name in ('archmage-mode.txt','active-build-serial.private','infestation-case.txt','weak-frontier-case.txt','saved-build-policy.txt')]
    (evidence/'saved-build-input-freeze.private.json').write_text(json.dumps(dict(files={p.relative_to(root).as_posix():sha(p) for p in files},scope='Actual saved Player, unchanged skills/gear/resources, finite legal client actions, natural tier-zero frontier. Optional at most two offline proxy allies use normal OmniAI with test-only encounter-target allegiance with a declared shared Team; no hired status, follower-cap override or training claim.'),indent=2))
    print('Configured fresh private representative client and froze finite-action driver; no processes launched.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--serial',type=int,required=True);parser.add_argument('--policy',choices=['melee','mage'],required=True)
    parser.add_argument('--clones',type=int,nargs='*',default=[]);args=parser.parse_args();assert len(args.clones)<=2
    prepare(args.root,args.serial,args.policy,args.clones)
