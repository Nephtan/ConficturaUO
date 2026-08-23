# Organic City Invasion Placement Audit

## Review Basis

This is the redacted placement record for Organic City Invasions v1. It replaces the provisional coordinates selected from the May snapshot.

The review loaded the August 22, 2026 `Saves/` through the normal RunUO world deserializers in read-only mode. It did not read or export `accounts.xml`. The private tactical atlas combined the current saved world with the Sosaria terrain, static, tile-flag, elevation, collision, bridge, road, wall, building, and multi layers.

The loaded save contained 18,333 spawners, 66,137 mobiles, and 686,903 world items. The redacted placement layer retained 81,865 relevant objects. Spawner records duplicated in the item layer were joined by serial before spatial analysis; serials, account data, player names, and house names are not included here.

Map files reviewed:

- `map1.mul`: SHA-256 `1D973D6948F8C75319F7AF9407964885AB54413AB399DDC389183BF7FF13506D`
- `staidx1.mul`: SHA-256 `A8BF870F27A9656BF4031B4FB22BA6EA23C06910C84ECCC26226A913D104E629`
- `statics1.mul`: SHA-256 `35BDBA4953C456A2041C927F3B431518E28FB33446F5D3A53F3462E1804A1C71`
- `tiledata.mul`: SHA-256 `8D01DF7C38AAD95D1EDC8E813560E491371153D418E5C4F523381B7D263D8389`

## Applied Gates

- Rituals require a connected 5x5 focus area.
- Camps require a connected 17x17 scene area and two three-tile-wide approaches.
- Civic wards require connected 7x7 interaction space and at least three tiles of clearance from saved spawner home points.
- Final battles require a connected 21x21 combat area.
- Occupation services require connected 15x15 civic-service space.
- A saved spawner home point may not be inside a ritual, camp, or final-battle footprint.
- Doors, teleporters, moongates, house signs, player structures, working spots, and training fixtures block a footprint.
- Each external ritual or camp uses a true 24-tile conflict circle. The circle plus an eight-tile safety buffer must remain clear of player-house footprints and linked house objects.
- Runtime validation may snap an anchor to the nearest legal tile within 12 tiles. It does not increase that range; an unresolved footprint disables the city.

## Britain

| Objective | Provisional | Accepted | Placement rationale |
| --- | --- | --- | --- |
| Ritual | `(3125,1035,0)` | `(3224,1088,0)` | Moves the focus out of the occupied eastern housing field to visible dry ground related to the far-east road and stone-circle approach. |
| West camp | `(2914,1068,0)` | `(2920,1010,0)` | Uses open ground on the western approach while clearing nearby house objects and preserving two travel lanes. |
| North camp | `(2999,870,0)` | `(3020,848,0)` | Moves off the castle-edge activity area to the broad northern mainland approach. |
| East camp | `(3115,1059,0)` | `(3230,1048,0)` | Leaves the player-house belt entirely and relates the camp to the far-east road without overlapping the ritual scene. |
| West ward | `(2961,1073,0)` | `(2968,1074,0)` | Shifts to a clearer western civic interaction space. |
| Castle ward | `(2995,936,0)` | `(2991,908,0)` | Uses the castle civic court while avoiding the previous saved spawner and working-area conflicts. |
| East ward | `(3048,1057,0)` | `(3042,1061,0)` | Preserves the east-town relationship with better door and spawner clearance. |
| Final battle | `(2999,1053,0)` | `(3000,1037,0)` | Uses the connected central civic plaza instead of the previous working area. A saved `MeetingSpots` scheduler marker is inside the conservative square; it is nonphysical and is logged as a warning rather than treated as collision. |

## Montor

| Objective | Provisional | Accepted | Placement rationale |
| --- | --- | --- | --- |
| Ritual | `(3243,2785,0)` | `(3246,2791,0)` | Moves out of saved undead spawners and metal doors to the clear southern temple ground. |
| West camp | `(3035,2607,0)` | `(3056,2666,0)` | Replaces the house-teleporter, house-sign, and tent-multi conflict with open southwest approach terrain. |
| North camp | `(3212,2545,0)` | `(3269,2552,0)` | Uses broad clear terrain on the northern approach to the two city halves. |
| East camp | `(3400,2607,0)` | `(3392,2616,1)` | Moves onto connected east-gate approach ground with manageable elevation. |
| West ward | `(3078,2610,0)` | `(3082,2610,0)` | Clears the western ward interaction ring from the nearest saved fixtures. |
| Central ward | `(3209,2624,0)` | `(3204,2610,0)` | Places the ward on the route joining the two civic halves without occupying a spawner point. |
| East ward | `(3356,2611,0)` | `(3352,2611,0)` | Preserves the east-city objective while gaining door and spawner clearance. |
| Final battle | `(3212,2610,0)` | `(3186,2598,0)` | Uses the largest connected western civic plaza. Buildings define the arena edge, but the required combat space remains connected rather than requiring every tile in the bounding square to be empty. |

## Devil Guard

| Objective | Provisional | Accepted | Placement rationale |
| --- | --- | --- | --- |
| Ritual | `(1800,1624,2)` | `(1809,1662,2)` | Replaces a water tile with dry southern approach ground. |
| West camp | `(1565,1500,2)` | `(1560,1493,2)` | Uses open western mainland while preserving the 32-tile player-house safety envelope. |
| North camp | `(1660,1425,2)` | `(1706,1438,5)` | Moves to broad northern approach terrain and away from the housing conflict envelope. |
| East camp | `(1800,1624,2)` | `(1821,1566,2)` | Replaces the shared water coordinate with a distinct, traversable east-mainland approach. |
| West ward | `(1605,1542,7)` | `(1610,1590,2)` | Moves to connected western civic ground with clearer interaction space. |
| Central ward | `(1671,1463,2)` | `(1667,1463,2)` | Retains the central relationship while clearing nearby saved fixtures. |
| East ward | `(1725,1538,2)` | `(1725,1534,2)` | Shifts the east ward away from its closest obstruction. |
| Final battle | `(1691,1464,15)` | `(1689,1439,5)` | Moves from a constrained elevated fixture area to the connected northern civic approach. |

Devil Guard's separate farmland rectangle remains excluded from the city and conflict-zone definition.

## Camp Scenes And Cleanup

Each accepted camp has a fixed facing and one of three layout variants. The capture ring remains clear, props stay in the rear and flank spaces, and two approaches remain open.

- Clockwork Dominion scenes contain a tent, forge, anvil, gears, repair machinery, metal supplies, barriers, and industrial light.
- Blood Court scenes contain a tent, braziers, a cage, crimson standards, trophies, provisions, barriers, and skirmisher lanes.
- Abyssal Legion scenes contain a tent, summoning focus, fires, bones, skull poles, infernal standards, supplies, and scorched barriers.

Every prop is a typed, serialized `InvasionCampProp` with session, city, faction, camp, facing, layout, role, and stable prop-index metadata. Reconciliation removes duplicate or invalid props only when their ownership matches the current session and reconstructs missing manifest entries. Camp cleansing, phase victory, abort, occupation, and liberation remove only owned session objects.

## Rollout Status

All three cities and the global engine remain disabled. This source review and map audit do not replace owner-run staging. Each city still requires in-game acceptance for pathing, scene appearance, banks and travel, occupation-service access, combat visibility, player warnings, save/restart recovery, and civilian restoration before activation.

## Source Trace

- Canonical city coordinates and true radial conflict geometry: `Data/Scripts/Custom/PvE/Invasions/InvasionTypes.cs`
- Footprint, saved-object, house-safety, approach, faction-construction, and legacy validation: `Data/Scripts/Custom/PvE/Invasions/InvasionLifecycle.cs`
- Typed faction camp props and layouts: `Data/Scripts/Custom/PvE/Invasions/InvasionCampScenes.cs`

## Audience

Staff and maintainers
