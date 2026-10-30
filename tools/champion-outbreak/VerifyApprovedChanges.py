"""Compile approved outbreak script changes and run save-free component regressions."""
from pathlib import Path
import argparse
import datetime
import hashlib
import json
import shutil
import subprocess


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def verify(args):
    repo = Path(__file__).resolve().parents[2]
    baseline, output = args.baseline.resolve(), args.output.resolve()
    assert not output.exists(), 'Use a fresh candidate; never overwrite evidence'
    original = json.loads((baseline/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    inputs = {name:sha(repo/name) for name in original}
    changed = sorted(name for name,value in inputs.items() if value != original[name])
    approved = {'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakCreatures.cs',
                'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakLedger.cs'}
    if args.geography:
        approved = {'Data/Scripts/Custom/Champions/Outbreak/WildernessEncounterReserve.cs',
                    'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakSites.cs'}
    if args.gold_scatter:
        approved = {'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs',
                    'Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakSite.cs'}
    assert set(changed) == approved, 'Unexpected source changes: '+repr(changed)
    assert sha(baseline/'ConficturaServer.exe') == sha(repo/'ConficturaServer.exe'), 'Core artifact mismatch'
    # Every core/config input is unchanged from the accepted source-matching core.
    assert all(inputs[name] == value for name,value in original.items() if not name.startswith('Data/Scripts/'))
    output.mkdir()
    shutil.copytree(baseline/'Data',output/'Data',ignore=lambda directory,names:
        [n for n in names if n in ('Files','bin','obj','__pycache__','Data.bin','Data.hash','Data.hashes','Data.pdb')])
    for path in baseline.iterdir():
        if path.is_file() and (path.name in ('ConficturaServer.exe','ConficturaServer.exe.config')
                               or path.suffix.lower()=='.dll' and path.name!='Data.dll'):
            shutil.copy2(path,output/path.name)
    expected = {name for name in inputs if name.startswith('Data/Scripts/') and name.endswith('.cs')}
    for name in expected:
        destination = output/name
        destination.parent.mkdir(parents=True,exist_ok=True)
        shutil.copy2(repo/name,destination)
    actual = {str(p.relative_to(output)).replace('\\','/') for p in (output/'Data/Scripts').rglob('*.cs')}
    assert actual == expected, 'Snapshot contains unlisted/missing runtime sources'
    head = subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip()
    (output/'inputs.json').write_text(json.dumps(dict(repository=str(repo),head=head,files=inputs,
        working_tree_snapshot=True,approved_changed_inputs=changed,core_reused_after_complete_input_match=True),indent=2))

    def run(name,command,timeout=60):
        began = datetime.datetime.now(datetime.timezone.utc).isoformat()
        forced = False
        try:
            result = subprocess.run([str(x) for x in command],cwd=output,capture_output=True,timeout=timeout,
                                    creationflags=subprocess.CREATE_NO_WINDOW)
            code,stdout,stderr = result.returncode,result.stdout,result.stderr
        except subprocess.TimeoutExpired as error:
            code,stdout,stderr,forced = None,error.stdout or b'',error.stderr or b'',True
        (output/(name+'.log')).write_bytes(stdout)
        (output/(name+'.err')).write_bytes(stderr)
        (output/(name+'.result.json')).write_text(json.dumps(dict(started=began,command=[str(x) for x in command],
            exit_code=code,forced_termination=forced,passed=code==0 and not forced),indent=2))
        assert code==0 and not forced, name+' failed; retained logs in '+str(output)
        return stdout.decode('utf-8',errors='replace')

    text = run('runtime-compile',[output/'ConficturaServer.exe','-nocache','-compileonly'])
    assert 'Scripts: Compile-only verification completed successfully.' in text
    shutil.copy2(output/'Data/Data.bin',output/'Data.dll')
    compiler = Path('C:/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe')
    fixture_names = ['ChampionRewardRegression','ArachnidOutbreakRegression','ArachnidOutbreakControllerRegression',
                     'ArachnidOutbreakRewardRegression','ArachnidOutbreakWebRegression']
    if args.geography:
        fixture_names = ['ArachnidOutbreakReservationRegression','ArachnidOutbreakControllerRegression']
    if args.gold_scatter:
        fixture_names = ['ChampionRewardRegression','ArachnidOutbreakControllerRegression',
                         'ArachnidOutbreakRewardRegression']
    fixtures = output/'fixtures';fixtures.mkdir()
    fixture_hashes = {}
    results = {}
    for name in fixture_names:
        source = repo/'tests'/(name+'.cs')
        snapshot = fixtures/source.name
        shutil.copy2(source,snapshot);fixture_hashes[source.name]=sha(snapshot)
        executable = output/(name+'.exe')
        run(name+'-compile',[compiler,'/nologo','/codepage:65001','/target:exe','/platform:x86',
            '/out:'+str(executable),'/reference:'+str(output/'ConficturaServer.exe'),
            '/reference:'+str(output/'Data.dll'),snapshot])
        arguments = [executable,args.assets.resolve()]
        if name == 'ArachnidOutbreakWebRegression':
            arguments.append(repo/'Data/TurnBasedCombat')
        if name == 'ArachnidOutbreakReservationRegression':
            arguments.append(repo/'Data/System/XML/Regions.xml')
        result = run(name+'-run',arguments)
        markers = [line.strip() for line in result.splitlines() if line.startswith('PASS')]
        assert markers, name+' returned success without its pass summary'
        results[name] = markers[-1]
        print(markers[-1])
    assert all(sha(repo/name)==value for name,value in inputs.items()), 'Source moved during verification'
    assert all(sha(repo/'tests'/name)==value for name,value in fixture_hashes.items()), 'Fixture moved during verification'
    assert not (output/'Saves').exists(), 'Save-free candidate unexpectedly contains Saves'
    summary = dict(production_inputs=len(inputs),changed_inputs=changed,core_sha256=sha(output/'ConficturaServer.exe'),
        script_sha256=sha(output/'Data.dll'),fixture_hashes=fixture_hashes,results=results,
        runtime_compile=True,core_build=False,source_input_match=True,no_world_or_listener=True)
    (output/'approved-changes-verification.json').write_text(json.dumps(summary,indent=2))
    print('Verified source-matching unchanged core, forced runtime compilation and '+str(len(fixture_names))+' component fixtures; no world/listener.')


if __name__ == '__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--baseline',type=Path,required=True)
    parser.add_argument('--output',type=Path,required=True)
    parser.add_argument('--assets',type=Path,required=True)
    parser.add_argument('--geography',action='store_true',help='Verify only the explicitly approved reservation/catalog source changes')
    parser.add_argument('--gold-scatter',action='store_true',help='Verify the two-file boss gold placement repair')
    verify(parser.parse_args())
