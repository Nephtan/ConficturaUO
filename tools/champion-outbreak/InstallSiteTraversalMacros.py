"""Install bounded walking macros in a stopped disposable client, never an installed profile."""
from pathlib import Path
import argparse
import csv
import hashlib
import json
import xml.etree.ElementTree as ET


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def install(root):
    root = root.resolve()
    evidence = root/'evidence'
    assert Path((evidence/'native-private-root.txt').read_text()).resolve() == root/'shard'
    prepared = json.loads((evidence/'prepared.json').read_text())
    assert prepared['fixture'] == 'site-traversal'
    profiles = list((root/'client/Data/Profiles').rglob('macros.xml'))
    assert len(profiles) == 1, 'Select one unambiguous copied character profile'
    profile = profiles[0]
    document = ET.parse(profile)
    assert not any(m.get('name', '').startswith('PRIVATE Survey') for m in document.getroot()), 'Use a fresh private profile'
    routes = {}
    for row in csv.DictReader((evidence/'site-routes.csv').open(encoding='utf-8-sig')):
        routes.setdefault(row['site'], []).append(row)
    assert len(routes) == 6
    before = digest(profile)
    installed = []

    def add(name, key, rows):
        macro = ET.SubElement(document.getroot(), 'macro', name=name, key=str(ord(key)),
                              mousebutton='0', wheelscroll='False', wheelup='False',
                              alt='False', ctrl='True', shift='True')
        actions = ET.SubElement(macro, 'actions')
        previous_direction = 0  # The guarded site setup faces North.
        for index, row in enumerate(rows[1:], 1):
            previous, direction = rows[index-1], int(row['direction'])
            dx, dy = int(row['x'])-int(previous['x']), int(row['y'])-int(previous['y'])
            assert (dx, dy) == {0:(0,-1), 2:(1,0), 4:(0,1), 6:(-1,0)}[direction]
            assert int(row['step']) == index
            # UO walk requests turn before translating; the client's Walk submenu
            # uses NW=1,N=2,NE=3,E=4,SE=5,S=6,SW=7,W=8.
            for _ in range(2 if direction != previous_direction else 1):
                ET.SubElement(actions, 'action', code='5', subcode=str((direction+1)%8+1), submenutype='1')
                ET.SubElement(actions, 'action', code='28', subcode='0', submenutype='2', text='500')
            if row['checkpoint']:
                ET.SubElement(actions, 'action', code='28', subcode='0', submenutype='2', text='4000')
            previous_direction = direction
        installed.append(dict(name=name, key='Control+Shift+'+key, steps=len(rows)-1))

    for key, (site, rows) in zip('abcdef', routes.items()):
        add('PRIVATE Survey '+site, key, rows)
    add('PRIVATE Survey calibration two steps', 'z', next(iter(routes.values()))[:3])
    document.write(profile, encoding='utf-8', xml_declaration=True)
    (evidence/'site-macros.json').write_text(json.dumps(dict(profile=str(profile), before=before,
        after=digest(profile), plans_sha256=digest(evidence/'site-routes.csv'), macros=installed,
        scope='Copied-client normal Walk/Delay actions. Calibrate before a full route; no staff movement or teleport actions.'), indent=2))
    print('Installed six private route macros and one two-step calibration; original profiles untouched.')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--output', type=Path, required=True)
    install(parser.parse_args().output)
