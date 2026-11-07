# Animal Form and movement speed verification

## Reproduction

On baseline `907aa1e9`, equip Boots of Hermes, cast Animal Form, choose Wolf, then walk through the saved Castle Dio entrance in Sosaria at `(3492, 733, 40)`. The dungeon correctly suppresses the boots. The server then sends `SpeedControl.Disable` even though Wolf Form is still active and the server still calculates a 100 ms run step. ClassicUO applies the latest speed mode to its local step timing, so the same wolf moves at foot speed. Ending and recasting Wolf Form restores the client boost. [ClassicUO speed packet handler](https://github.com/ClassicUO/ClassicUO/blob/1.1.0.0/src/ClassicUO.Client/Network/PacketHandlers.cs#L4347-L4355) and [step timing](https://github.com/ClassicUO/ClassicUO/blob/1.1.0.0/src/ClassicUO.Client/Game/GameObjects/PlayerMobile.cs#L1437-L1461)

## Shared speed sources

- `AnimalTrainer.RefreshMovementSpeed` selects forced walking, mount-speed movement, or normal movement from the current effects. It uses the active Animal Form context, eligible footwear, and the four temporary speed spell tables. [AnimalTrainer](../../Data/Scripts/Mobiles/Civilized/Vendors/AnimalTrainer.cs)
- `PlayerMobile.OnLocationChange` still applies the existing no-mount and magic-speed region rules. Restricted dungeon entry suppresses footwear and removes temporary speed spells. It preserves an active speed-granting Animal Form. `Announce.World_Login` uses the same speed selection after its existing region checks. [PlayerMobile](../../Data/Scripts/Mobiles/Base/PlayerMobile.cs) and [Broadcast](../../Data/Scripts/System/Misc/Broadcast.cs)
- Equipment changes now recalculate speed from the remaining sources. This covers artifact and legacy Boots of Hermes, Sprinter's Sandals, and Hiking Boots for eligible custom races. Ending a speed-granting form likewise preserves active footwear or spells. [Artifact Hermes](../../Data/Scripts/Items/Magical/Artifacts/Artifact_BootsofHermes.cs), [legacy Hermes](../../Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_BootsofHermes.cs), [Sprinter's Sandals](../../Data/Scripts/Items/Magical/Artifacts/Artifact_SprintersSandals.cs), [Hiking Boots](../../Data/Scripts/Items/Armor/Leather/HikingBoots.cs), and [Animal Form](../../Data/Scripts/Magic/Ninjitsu/AnimalForm.cs)
- Cheetah Paws, Wind Runner, Jedi Celerity, and Sith Speed all previously reset the shared client mode during removal. They now recalculate it after changing their own active state. Ilhenir's forced walking takes priority, and its cleanup restores an eligible boost. Physical mounts use the mounted state for movement timing. [Cheetah Paws](../../Data/Scripts/Magic/Shinobi/Spells/CheetahPaws.cs), [Wind Runner](../../Data/Scripts/Magic/Mystic/Spells/WindRunner.cs), [Celerity](../../Data/Scripts/Magic/Jedi/Spells/Celerity.cs), [Sith Speed](../../Data/Scripts/Magic/Syth/Spells/Speed.cs), and [Ilhenir](../../Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Ilhenir.cs)

## Native ClassicUO results

DarkWind was selected from the copied saves with 125 Ninjitsu, an owned Book of Ninjitsu containing Animal Form, and owned artifact Boots of Hermes. The client equipped the boots, cast the spell, selected Wolf, and moved across the saved entrance. It also performed logout, reconnect, unequip, and the Cheetah Paws cast through the normal Player command. The test fixture supplied the learned Shinobi ability if needed. After a combat-interrupted first reproduction, four nearby creatures were moved off-map in the disposable copy for the timed comparisons.

For each timing run, the fixture reset the position in the same entrance corridor and gave ClassicUO the same pathfinding destination. The client generated all five accepted movement requests. The numbers below are median intervals across the four gaps, measured from the server's receipt of the real client packets.

| Case | Median step interval | Observed state |
| --- | ---: | --- |
| Baseline Wolf and Hermes entry | 200.9 ms | Wolf remained active, Hermes was suppressed, and the server sent `Disable`. |
| Baseline workaround | 100.3 ms | Ending and recasting Wolf restored `MountSpeed` in the same corridor. |
| Patched Wolf and Hermes entry | 100.7 ms | Hermes was suppressed and Wolf retained `MountSpeed` without a recast. |
| Patched logout and fresh-client reconnect | 100.8 ms | Wolf and the suppressed footwear state were both retained inside Castle Dio. |
| Patched Wolf and Cheetah Paws entry | 100.7 ms | Cheetah Paws was cast before transforming. Entry removed the buff and retained Wolf speed. |
| Patched final boost removed | 201.0 ms | With boots unequipped and Cheetah Paws removed, ending Wolf restored ordinary running. |

The connected-shard fixture passed 49 further assertions on speed packets and source state. It exercised all four footwear variants, the removal paths for all four temporary spells, an unboosted Cat form, and Ilhenir's forced walking and cleanup. It seeded spell state for those removal checks. Full native cast coverage was Animal Form and Cheetah Paws.

## Build and evidence

Release x86 built successfully. Both native shards started with `-service -nocache`. An additional `-compileonly -nocache` run used an uninstrumented server and the exact production script payload. Serialization methods and protected save and profile hashes were checked. The isolated server and client exited and their loopback port was verified closed.

Local evidence is under `D:/ConficturaUOTestTools/hermes-native-20261107-094606/evidence/`. See `acceptance-results.json`, `baseline-events.tsv`, `candidate-events.tsv`, the matching `*-speed.tsv` traces, `runtime-compile-production-payload.log`, `source-and-protection-check.json`, and `shutdown.json`.
