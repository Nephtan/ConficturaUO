"""Publish one complete request to a guarded disposable native fixture."""
from pathlib import Path
import argparse
import os
import time
import uuid


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('command', choices=['reset','warn','arm','show','dose','turn-self','turn-pet','status','queue','claim','stop'])
    args = parser.parse_args()
    root = args.output.resolve()
    guard = root/'evidence/native-private-root.txt'
    assert guard.is_file() and Path(guard.read_text(encoding='utf-8')).resolve() == root/'shard', 'Not a guarded fixture root'
    request = root/'evidence/native-request.txt'
    assert not request.exists(), 'A prior request is still pending; wait for consumption before sending another'
    temporary = root/'evidence'/('request-'+uuid.uuid4().hex+'.tmp')
    temporary.write_text(args.command, encoding='utf-8')
    for attempt in range(10):
        try:
            os.replace(temporary, request)
            break
        except PermissionError:
            if attempt == 9:
                raise
            time.sleep(0.05)
    print('Published guarded fixture request: '+args.command)
