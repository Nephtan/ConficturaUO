"""Exercise native cage release/mounting before freezing an unstarted disposable reference."""
from pathlib import Path
import argparse
import json
import time
from FreezeBenchmark import read_status,read_shared,capability_signature
from RunArchmageCase import bind_server,publish

def prepare(root):
    root=root.resolve();e=root/'evidence'
    assert root.is_relative_to(Path('D:/ConficturaUOTestTools').resolve())
    p=json.loads((e/'prepared.json').read_text());assert p['fixture']=='benchmark'
    assert not (e/'reference-freeze.json').exists() and not (e/'mount-preparation.json').exists()
    alive=bind_server(root,e);deadline=time.monotonic()+120
    while time.monotonic()<deadline:
        alive();state=read_status(e/'benchmark-state.xml')
        assert state.get('state')!='failed',state.get('failure')
        if state.get('state')=='ready' and state.get('online')=='True':break
        time.sleep(.2)
    assert state.get('state')=='ready' and state.get('online')=='True' and state.get('mounted')=='False'
    before=e/'mount-preparation-before-reference.xml';assert not before.exists()
    before.write_bytes(read_shared(e/'benchmark-reference.xml'))
    publish(e,'benchmark-command.txt','prepare-mount');deadline=time.monotonic()+50
    while time.monotonic()<deadline:
        alive();state=read_status(e/'benchmark-state.xml')
        assert state.get('state')!='failed',state.get('failure')
        if state.get('state')=='ready' and state.get('mobilityPrepared')=='True':break
        time.sleep(.1)
    assert state.get('mobilityPrepared')=='True' and state.get('mounted')==state.get('mountOwned')==state.get('mountRider')=='True'
    assert state.get('mountType')=='Server.Mobiles.Horse'
    first=capability_signature(before);second=capability_signature(e/'benchmark-reference.xml')
    for signature in (first,second):
        for key in ('mounted','mountType','mountOwned','mountRider'):signature['character'].pop(key)
    assert first==second,'Mount preparation changed another reference capability'
    (e/'mount-preparation-after.xml').write_bytes(read_shared(e/'benchmark-state.xml'))
    report=dict(native_cage_release_and_mount_passed=True,other_capabilities_unchanged=True,
                horse_asset_quantity=1,nominal_source_cage_price=int(state.get('mountCageBasePrice')),
                actual_vendor_purchase=False,scope='Declared vendor-obtainable fixture asset; ordinary client cage and mount use; no direct Rider assignment or skill grant')
    (e/'mount-preparation.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))

if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('root',type=Path);prepare(parser.parse_args().root)
