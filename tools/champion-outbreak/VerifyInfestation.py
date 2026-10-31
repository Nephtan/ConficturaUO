"""Snapshot current infestation inputs and verify them without loading saves or listening."""
from pathlib import Path
import argparse,datetime,hashlib,json,shutil,subprocess

def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()

def verify(args):
    repo=Path(__file__).resolve().parents[2];base=args.baseline.resolve();out=args.output.resolve()
    assert not out.exists(),'Use a fresh candidate'
    old=json.loads((base/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    names=set(old)
    names.update(('Data/Scripts/Scripts.csproj','Data/Scripts/app.config','Info/settings.xml'))
    runtime={p.relative_to(repo).as_posix() for p in (repo/'Data/Scripts').rglob('*.cs') if not {'bin','obj'} & set(p.parts)}
    names={n for n in names if not n.startswith('Data/Scripts/') or not n.endswith('.cs')}|runtime
    inputs={n:sha(repo/n) for n in sorted(names)}
    if args.core_candidate:
        core=args.core_candidate.resolve();manifest=json.loads((core/'core-inputs.json').read_text())
        actual={p.relative_to(repo/'Data/System/Source').as_posix():sha(p) for p in (repo/'Data/System/Source').rglob('*') if p.is_file() and not {'bin','obj','.vs'}&set(p.parts)}
        assert actual==manifest,'Complete core source differs from the verified build'
        proof=json.loads((core/'sector-core-verification.json').read_text());assert proof['core_build'] and proof['forced_runtime_compile']
        assert sha(repo/'ConficturaServer.exe')==proof['core_sha256']==sha(core/'bin/ConficturaServer.exe')
    else:
        assert all(inputs[n]==v for n,v in old.items() if n.startswith('Data/System/Source/')),'Core source drift requires --core-candidate with a verified fresh build'
        assert sha(repo/'ConficturaServer.exe')==sha(base/'ConficturaServer.exe')
    out.mkdir();shutil.copytree(base/'Data',out/'Data',ignore=lambda d,ns:[n for n in ns if n in ('Scripts','Files','bin','obj','__pycache__','Data.bin','Data.hash','Data.hashes','Data.pdb')])
    for p in base.iterdir():
        if p.is_file() and (p.name in ('ConficturaServer.exe','ConficturaServer.exe.config') or p.suffix=='.dll' and p.name!='Data.dll'):shutil.copy2(p,out/p.name)
    shutil.copy2(repo/'ConficturaServer.exe',out/'ConficturaServer.exe')
    for n in inputs:
        p=out/n;p.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(repo/n,p)
    (out/'inputs.json').write_text(json.dumps(dict(repository=str(repo),head=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),files=inputs,working_tree_snapshot=True,changed_inputs=[n for n,v in inputs.items() if old.get(n)!=v],core_reused_after_complete_input_match=True),indent=2))
    def run(name,cmd,timeout=60):
        began=datetime.datetime.now(datetime.timezone.utc).isoformat()
        try:r=subprocess.run([str(x) for x in cmd],cwd=out,capture_output=True,timeout=timeout,creationflags=subprocess.CREATE_NO_WINDOW)
        except subprocess.TimeoutExpired as error:
            (out/(name+'.log')).write_bytes((error.stdout or b'')+(error.stderr or b''));raise
        (out/(name+'.log')).write_bytes(r.stdout+r.stderr)
        (out/(name+'.result.json')).write_text(json.dumps(dict(started=began,command=[str(x) for x in cmd],exit_code=r.returncode),indent=2))
        assert r.returncode==0,name+' failed; inspect retained log'
        return r.stdout.decode('utf-8',errors='replace')
    text=run('runtime-compile',[out/'ConficturaServer.exe','-service','-nocache','-compileonly'])
    if 'Compile-only verification completed successfully' not in text:
        log=out/'Data/Logs/Console.log';assert log.exists() and 'Compile-only verification completed successfully' in log.read_text(errors='replace')
    shutil.copy2(out/'Data/Data.bin',out/'Data.dll')
    results={};fixtures=out/'fixtures';fixtures.mkdir()
    if not args.compile_only:
        for name in ('ChampionRewardRegression','ArachnidOutbreakRewardRegression','BroodAntivenomRegression','ArachnidOutbreakWebRegression','ArachnidInfestationRegression','OfflineCloneFidelityRegression','PersistentInfestationRegression'):
            source=repo/'tests'/(name+'.cs');snapshot=fixtures/source.name;shutil.copy2(source,snapshot);exe=out/(name+'.exe')
            sources=[snapshot]
            if name=='BroodAntivenomRegression':
                dependency=fixtures/'PoisonChangeRegression.cs';shutil.copy2(repo/'tests'/dependency.name,dependency);sources.append(dependency)
            run(name+'-compile',['C:/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe','/nologo','/codepage:65001','/platform:x86','/target:exe','/main:'+name,'/out:'+str(exe),'/reference:'+str(out/'ConficturaServer.exe'),'/reference:'+str(out/'Data.dll')]+sources)
            command=[exe,args.assets.resolve()]
            if name=='ArachnidOutbreakWebRegression':command.append(repo/'Data/TurnBasedCombat')
            result=run(name+'-run',command);results[name]=[s for s in result.splitlines() if s.startswith('PASS')];assert results[name],name
            print(results[name][-1],flush=True)
    assert all(sha(repo/n)==v for n,v in inputs.items()),'Input drift during verification'
    (out/'infestation-verification.json').write_text(json.dumps(dict(inputs=len(inputs),runtime_sources=len(runtime),runtime_compile=True,core_sha256=sha(out/'ConficturaServer.exe'),script_sha256=sha(out/'Data.dll'),results=results,no_world_or_listener=True),indent=2))
    print('Current source snapshot compiled; '+str(len(results))+' component fixtures; no saves or listener.',flush=True)

if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('--baseline',type=Path,required=True);p.add_argument('--output',type=Path,required=True);p.add_argument('--assets',type=Path,required=True);p.add_argument('--core-candidate',type=Path);p.add_argument('--compile-only',action='store_true');verify(p.parse_args())
