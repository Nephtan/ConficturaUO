"""Freeze a fresh two-client synthetic role probe; never launch or mutate original inputs."""
from pathlib import Path
import argparse
import json
import secrets
import shutil
import subprocess
import uuid
from VerifyBenchmark import sha


def prepare(root,cases):
    root=root.resolve();repo=Path(__file__).resolve().parents[2];e=root/'evidence'
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    assert Path((e/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert json.loads((e/'prepared.json').read_text())['fixture']=='acceptance'
    assert not (e/'native-server-pid.txt').exists() and not (e/'native-ready.txt').exists()
    assert not (e/'cooperative-inputs.json').exists() and not (e/'native-actor.txt').exists()
    fixture=root/'shard/Data/Scripts/TestOnly/ArachnidOutbreakCooperative.cs';assert not fixture.exists()
    shutil.copy2(repo/'tests'/fixture.name,fixture)
    ally=root/'ally-client';assert not ally.exists();shutil.copytree(root/'client',ally)
    # Legacy account login packets carry at most 30 bytes per credential.
    username='outbreak-coop-'+uuid.uuid4().hex[:12];password=secrets.token_hex(12)
    assert len(username.encode('ascii'))<=30 and len(password.encode('ascii'))<=30
    (e/'cooperative-account.private').write_text(username+'\n'+password+'\n')
    frozen=[fixture,e/'cooperative-account.private']
    for folder,role in [(root/'client','patient'),(ally,'helper')]:
        driver=folder/'ClassicAssist/CooperativeClient.py';shutil.copy2(repo/'tools/champion-outbreak/CooperativeClient.py',driver)
        profile=folder/'ClassicAssist/Profiles/settings.json';data=json.loads(profile.read_text(encoding='utf-8-sig'))
        data['General']['Autologin']=False
        for macro in data['Macros']['Macros']:macro['IsAutostart']=False
        name='Private cooperative '+role
        data['Macros']['Macros'].append(dict(Id=str(uuid.uuid4()),Name=name,Loop=False,DoNotAutoInterrupt=False,PassToUO=False,IsBackground=False,IsAutostart=True,Disableable=True,Group='',Global=False,Keys={},Macro='OUTBREAK_ROOT = '+repr(str(root))+'\nOUTBREAK_ROLE = '+repr(role)+'\nexecfile('+repr(str(driver))+')\n'))
        data['Macros']['Selected']=name;profile.write_text(json.dumps(data,indent=2))
        settings=folder/'settings.json';data=json.loads(settings.read_text(encoding='utf-8-sig'))
        data['plugins']=[str(folder/'ClassicAssist/ClassicAssist.dll')];data['profilespath']=str(folder/'Data/Profiles')
        if role=='helper':data.update(username=username,password='',lastcharactername='private outbreak support')
        settings.write_text(json.dumps(data,indent=2));frozen.extend([driver,profile,settings,folder/'ClassicUO.exe',folder/'ClassicAssist/ClassicAssist.dll'])
    generated=subprocess.run(['powershell','-NoProfile','-File',str(repo/'scripts/Generate-TurnBasedCombatCompatibility.ps1'),'-RepositoryRoot',str(root/'shard')],capture_output=True,text=True,timeout=120,check=True)
    (e/'cooperative-compatibility.log').write_text(generated.stdout+generated.stderr)
    assert 'Unknown dispositions: 0' in generated.stdout
    record=dict(source_commit=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),files={str(p.relative_to(root)):sha(p) for p in frozen},synthetic_roles=True,expected_clients=2,helper_login='Launch copied helper with -username and -password read privately from cooperative-account.private; never print credentials or assume saved-password encoding',scope='Independent controlled native support roles; fixture injury/poison/party and finite grants, AI stopped, no pacing/economics/actual-Zanthus-power claim',production_activation=False,site_approval=False)
    record['expected_cases']=cases
    (e/'cooperative-inputs.json').write_text(json.dumps(record,indent=2));print('Two private role clients frozen; no process launched; credentials retained privately.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    parser.add_argument('--cases',nargs='+',choices=['heal','cure','rescue','treat','prevent','bard'],default=['heal','cure','rescue','treat','prevent','bard'])
    args=parser.parse_args();assert len(set(args.cases))==len(args.cases);prepare(args.root,args.cases)
