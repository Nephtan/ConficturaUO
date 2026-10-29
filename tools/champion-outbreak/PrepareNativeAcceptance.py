"""Prepare one disposable copied-world native scene; never launches a process."""
from pathlib import Path
import argparse
import hashlib
import json
import shutil
import socket
import subprocess


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def hashes(path):
    return {str(p.relative_to(path)).replace('\\', '/'):sha(p) for p in path.rglob('*') if p.is_file()}


def prepare(args):
    repo = Path(__file__).resolve().parents[2]
    old, candidate, root = args.baseline.resolve(), args.candidate.resolve(), args.output.resolve()
    assert not root.exists(), 'Use a fresh output; never overwrite a prior native copy'
    inputs = json.loads((candidate/'inputs.json').read_text(encoding='utf-8-sig'))['files']
    assert all(sha(repo/p) == value for p, value in inputs.items()), 'Candidate inputs do not match repository'
    provenance = json.loads((old/'evidence/preparation-provenance.json').read_text(encoding='utf-8-sig'))
    assert all(sha(old/'shard/Data/Scripts'/p) == value for p, value in provenance['overlays'].items())
    protected = {str(old/'shard/Saves'):hashes(old/'shard/Saves'), str(old/'client/settings.json'):sha(old/'client/settings.json')}
    for rel in ('Data/Profiles', 'ClassicAssist/Profiles'):
        protected[str(old/'client'/rel)] = hashes(old/'client'/rel)
    root.mkdir(); (root/'evidence').mkdir(); shard = root/'shard'; shard.mkdir()
    (root/'evidence/protected-source-inputs.json').write_text(json.dumps(protected, indent=2), encoding='utf-8')

    def ignored(directory, names):
        if Path(directory) == old/'shard/Data':
            return [n for n in names if n in ('Scripts','Files','Data.bin','Data.hash','Data.hashes','Data.pdb')]
        return [n for n in names if n in ('.git','bin','obj','__pycache__') or n.endswith('.disabled')]

    shutil.copytree(old/'shard/Data', shard/'Data', ignore=ignored)
    for rel in ('Saves','Spawns','DioXMLSpawn','Info'):
        if (old/'shard'/rel).exists():
            shutil.copytree(old/'shard'/rel, shard/rel, ignore=ignored)
    shutil.copytree(candidate/'Data/Scripts', shard/'Data/Scripts')
    for p in (old/'shard/Data/Scripts').rglob('*'):
        if p.is_file() and p.suffix.lower() not in ('.cs','.disabled','.dll','.pdb'):
            destination = shard/'Data/Scripts'/p.relative_to(old/'shard/Data/Scripts')
            destination.parent.mkdir(parents=True, exist_ok=True); shutil.copy2(p, destination)
    for rel in provenance['overlays']:
        shutil.copy2(old/'shard/Data/Scripts'/rel, shard/'Data/Scripts'/rel)
    for p in candidate.iterdir():
        if p.is_file() and (p.name == 'ConficturaServer.exe' or p.suffix.lower() == '.dll'):
            shutil.copy2(p, shard/p.name)
    if (old/'shard/ConficturaServer.exe.config').exists():
        shutil.copy2(old/'shard/ConficturaServer.exe.config', shard/'ConficturaServer.exe.config')
    with socket.socket() as listener:
        listener.bind(('127.0.0.1', 0)); port = listener.getsockname()[1]
    p = shard/'Data/Scripts/System/Misc/SocketOptions.cs'
    text = p.read_text(encoding='utf-8-sig')
    assert text.count('IPAddress.Loopback, 25215') == 1
    p.write_text(text.replace('IPAddress.Loopback, 25215', f'IPAddress.Loopback, {port}'), encoding='utf-8')
    p = shard/'Data/Scripts/System/Misc/DataPath.cs'
    text = p.read_text(encoding='utf-8-sig').replace(str(old/'shard/Data/Files'), str(args.assets.resolve()))
    p.write_text(text, encoding='utf-8')
    for name in ('TurnBasedCombat.cfg','TurnActions.csv','TurnEffects.csv','TurnAI.csv'):
        destination = shard/'Data/TurnBasedCombat'/name
        destination.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(repo/'Data/TurnBasedCombat'/name, destination)
    text = (shard/'Data/TurnBasedCombat/TurnBasedCombat.cfg').read_text(encoding='utf-8-sig')
    assert 'Enabled=false' in text and 'RequireCompatibilityGate=true' in text
    fixture = 'ArachnidOutbreakSiteTraversal.cs' if args.fixture == 'site-traversal' else 'ArachnidOutbreakNativeAcceptance.cs'
    harness = shard/'Data/Scripts/TestOnly'/fixture
    harness.parent.mkdir(exist_ok=True)
    shutil.copy2(repo/'tests'/fixture, harness)
    (root/'evidence/native-private-root.txt').write_text(str(shard), encoding='utf-8')
    register = subprocess.run(['powershell','-NoProfile','-File',str(repo/'scripts/Generate-TurnBasedCombatCompatibility.ps1'),
                               '-RepositoryRoot',str(shard)], capture_output=True, text=True, timeout=120, check=True)
    (root/'evidence/compatibility-generation.log').write_text(register.stdout+register.stderr, encoding='utf-8')
    assert 'Unknown dispositions: 0' in register.stdout, 'Private source classification is incomplete'
    shutil.copytree(old/'client', root/'client', ignore=lambda directory,names:[n for n in names if n in ('Logs','logs','__pycache__')])
    p = root/'client/settings.json'
    settings = json.loads(p.read_text(encoding='utf-8-sig'))
    assert settings['ip'] in ('127.0.0.1','localhost')
    settings.update(ip='127.0.0.1', port=port, autologin=True, reconnect=False, plugins=[],
                    profilespath=str(root/'client/Data/Profiles'), ultimaonlinedirectory=str(args.assets.resolve()))
    p.write_text(json.dumps(settings, indent=2), encoding='utf-8')
    (root/'evidence/account-name.private').write_text(settings['username'], encoding='utf-8')
    record = dict(port=port, candidate=str(candidate), core_sha256=sha(shard/'ConficturaServer.exe'),
                  source_saves_sha256=protected[str(old/'shard/Saves')],
                  overlays={rel:sha(shard/'Data/Scripts'/rel) for rel in provenance['overlays']},
                  harness_sha256=sha(harness), fixture=args.fixture, private_compatibility_gate=True,
                  scope=('Planned copied-client site traversal; blessed travel-only Player; no encounter, site approval or pacing acceptance.'
                         if args.fixture == 'site-traversal' else
                         'Scene construction and stopped scheduler; no global TBC activation, AI, warning reaction or pacing acceptance.'))
    (root/'evidence/prepared.json').write_text(json.dumps(record, indent=2), encoding='utf-8')
    assert hashes(shard/'Saves') == protected[str(old/'shard/Saves')]
    for name, expected in protected.items():
        p = Path(name)
        assert (hashes(p) if p.is_dir() else sha(p)) == expected
    print(f'Prepared {root}; private loopback port {port}; no process launched; original inputs unchanged.')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--baseline', type=Path, required=True)
    parser.add_argument('--candidate', type=Path, required=True)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--assets', type=Path, required=True)
    parser.add_argument('--fixture', choices=['acceptance','site-traversal'], default='acceptance')
    prepare(parser.parse_args())
