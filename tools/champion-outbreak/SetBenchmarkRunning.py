"""Enable ordinary client running only before a fresh disposable benchmark is launched."""
from pathlib import Path
import argparse
import json
from FreezeBenchmark import movement_policy
from VerifyBenchmark import sha


def configure(root):
    root=root.resolve();evidence=root/'evidence'
    prepared=json.loads((evidence/'prepared.json').read_text(encoding='utf-8-sig'))
    assert prepared['fixture']=='benchmark'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve()==root/'shard'
    assert not (evidence/'benchmark-state.xml').exists() and not (evidence/'reference-freeze.json').exists()
    record=evidence/'running-configuration.json';assert not record.exists()
    before=movement_policy(root);path=root/before['path'];before_hash=sha(path)
    data=json.loads(path.read_text(encoding='utf-8-sig'));data['always_run']=True
    path.write_text(json.dumps(data,indent=2),encoding='utf-8')
    after=movement_policy(root);assert after['always_run'] is True
    record.write_text(json.dumps(dict(before=before,after=after,before_sha256=before_hash,after_sha256=sha(path),
        scope='Normal copied client setting only, before launch; no runtime ability/stat/speed override'),indent=2))
    print('Ordinary always_run enabled in the single copied profile before launch; originals untouched.')


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path)
    configure(parser.parse_args().root)
