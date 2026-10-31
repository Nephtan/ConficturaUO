"""Configure two actual saved Players in one fresh private world; no grants or original credentials exported."""
from pathlib import Path
import argparse,json,secrets,shutil,subprocess,uuid
from PrepareSavedBuildClient import prepare
from PrepareNativeAcceptance import sha


def configure(root,serial,partner_serial):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];e=root/'evidence'
    assert not(e/'server-process.json').exists()
    prepare(root,serial,'mage',[])
    # The preliminary generic configuration is still unlaunched; this case tests multiplayer transitions rather than a frontier solo.
    marker=e/'weak-frontier-case.txt';assert marker.parent==e;marker.unlink()
    (e/'partner-serial.private').write_text(str(partner_serial))
    (e/'protection-account.private').write_text('infest-probe-'+uuid.uuid4().hex[:12]+'\n'+secrets.token_hex(12)+'\n')
    fixture=root/'shard/Data/Scripts/TestOnly/ArachnidMultiplayerProtection.cs'
    shutil.copy2(repo/'tests'/fixture.name,fixture)
    shutil.copytree(root/'client',root/'ally-client')
    frozen=[e/'partner-serial.private',e/'protection-account.private',fixture]
    for folder,role in [('client','main'),('ally-client','partner')]:
        client=root/folder;driver=client/'ClassicAssist/ProtectionPreparationClient.py'
        shutil.copy2(repo/'tools/champion-outbreak'/driver.name,driver)
        profile=client/'ClassicAssist/Profiles/settings.json';data=json.loads(profile.read_text(encoding='utf-8-sig'))
        for macro in data['Macros']['Macros']:macro['IsAutostart']=False
        name='Private actual saved '+role+' protection'
        data['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,Disableable=True,Group='',Global=False,Keys={},Macro='OUTBREAK_ROOT = '+repr(str(root))+'\nOUTBREAK_ROLE = '+repr(role)+'\nexecfile('+repr(str(driver))+')\n'))
        data['Macros']['Selected']=name;profile.write_text(json.dumps(data,indent=2))
        settings=client/'settings.json';data=json.loads(settings.read_text(encoding='utf-8-sig'));data['plugins']=[str(client/'ClassicAssist/ClassicAssist.dll')];data['profilespath']=str(client/'Data/Profiles')
        if role=='partner':data.update(username=(e/'protection-account.private').read_text().splitlines()[0],password='',lastcharactername='Sargar')
        settings.write_text(json.dumps(data,indent=2));frozen.extend([driver,profile,settings,client/'ClassicUO.exe',client/'ClassicAssist/PrivateTamer/EncounterNavigation.py'])
    frozen.extend([root/'shard/Data/Scripts/TestOnly/ArachnidArchmagePreparation.cs',e/'active-build-serial.private',e/'infestation-case.txt',e/'archmage-mode.txt'])
    record=dict(files={p.relative_to(root).as_posix():sha(p) for p in frozen},actor_provenance='actual_saved_players',expected_clients=2,scope='Actual saved standard-spell mage and Sargar, unchanged capabilities and finite inventory. Generated disposable in-memory login for partner. Normal AI, client walking/spells/pet orders. Declared fixture relocations and 16 shared directors; no learned teleport, human-optimality, balance or VPS capacity claim.')
    (e/'protection-inputs.private.json').write_text(json.dumps(record,indent=2))
    (e/'saved-build-input-freeze.private.json').write_text(json.dumps(record,indent=2))
    result=subprocess.run(['powershell','-NoProfile','-File',str(repo/'scripts/Generate-TurnBasedCombatCompatibility.ps1'),'-RepositoryRoot',str(root/'shard')],capture_output=True,text=True,timeout=120,check=True)
    (e/'protection-compatibility.log').write_text(result.stdout+result.stderr);assert 'Unknown dispositions: 0' in result.stdout
    print('Two actual saved-player clients configured; no process started; generated private credentials withheld.')


if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('root',type=Path);p.add_argument('--serial',type=int,required=True);p.add_argument('--partner-serial',type=int,required=True);args=p.parse_args();configure(args.root,args.serial,args.partner_serial)
