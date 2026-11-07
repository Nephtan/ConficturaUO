# Wraith Form and Spirit animation alignment

Verified on November 7, 2026 with ClassicUO 1.1.0.0, a copied world, and the saved player Nier. The isolated shard was built from `38954381` in Release x86 and started with `-service -nocache` on loopback port 55453. A test-only script provided a disposable Player login, a marked test tile, spell supplies, packet observations, and saved NPCs positioned on reference tiles. All spell casts and movement came from ClassicUO.

## Cause and scope

The server casts Wraith Form with body 84. The transformation sets the body, hue, resistance modifiers, and ability to pass through mobiles. The reported cast left the character at Sosaria `(6606, 1074, 0)`. [WraithFormSpell](../../Data/Scripts/Magic/Necromancy/WraithForm.cs) · [TransformationSpellHelper](../../Data/Scripts/Magic/Base/SpellHelper.cs)

The tested client maps body 84 to monster animation slot 33 in `anim4.mul` through `Bodyconv.def`. The archived art sets each frame's vertical center to half its height, which raises the sprite by 16–86 pixels. The installed variant zeroes those centers without moving the encoded pixel runs. ClassicUO then attempts to write some runs outside their texture. The repair sets the center to zero and shifts the encoded run origins to preserve every decoded pixel. [ClassicUO 1.1.0.0 sprite decoder](https://github.com/ClassicUO/ClassicUO/blob/1.1.0.0/src/ClassicUO.Assets/AnimationsLoader.cs#L1371-L1435)

| Body 84 use | Instances in the copied save |
| --- | ---: |
| Wraith Form | Applied to the saved test player by casting |
| `Wraith` named a wraith | 320 |
| `Spirit` named a spirit | 46 |
| `Spirit` named a ghost | 9 |
| `Undead` generated as a spirit | 20 |
| `Undead` generated as a wraith | 15 |

The tested conversion and index files resolve only body 84 to the changed art. Generic morphs that select body 84 also use it. The 410 existing NPCs include three types, with four distinct type and body assignment paths. The two names used by `Spirit` share the same assignment. [Wraith](../../Data/Scripts/Mobiles/Undead/Wraith.cs) · [Spirit](../../Data/Scripts/Mobiles/Undead/Spirit.cs) · [Undead](../../Data/Scripts/Mobiles/Undead/Undead.cs)

## Native checks

To reproduce, start ClassicUO with the archived asset set. Stand on a marked tile, cast Wraith Form from a Necromancy spellbook or the Cast Spell macro, and compare the robe to the tile before taking a step. Repeat with the repaired file after restarting the client.

| Check | Observed result |
| --- | --- |
| Archived art, fresh cast | The robe appeared above the marked tile. The server reported the same X, Y, and Z. Movement still reached the correct neighboring tile. |
| Archived art, other Necromancy transformations | Lich Form, Horrific Beast, and Vampiric Embrace each rendered without the Wraith jump. |
| Installed zero-center-only art, fresh cast | ClassicUO threw `IndexOutOfRangeException` in `AnimationsLoader.ReadSpriteData`. The shard recorded the disconnect with Wraith Form active and the same X, Y, and Z at 16:45:12 UTC. |
| Repaired art, same executable and running shard | Nier reconnected in Wraith Form at 16:45:43 UTC. The sprite reached the marked tile. Casting again returned Nier to human form and cleared the ability to pass through mobiles. A fresh Wraith cast restored both the sprite and the effect at the same tile at 16:50:07 UTC. |
| Repaired art, movement | Native north and east turns held the same coordinates. The next east input moved the sprite one tile. The server advanced X from 6606 to 6607 at 16:50:22 UTC. |
| Repaired and archived art, same NPC scene | Saved `Wraith`, `Spirit` named a ghost, and both `Undead` variants rendered at their reference tiles with the repair. Reopening the same executable on the same scene with archived art at 16:51 UTC raised Nier and all four NPC sprites above those tiles. |

The binary check covered all 60 populated animation records, 443 frames, and 125,545 pixel runs for the affected slot, including all encoded facings and action groups. It found 54,637 out-of-bounds runs in the installed variant. The repaired file preserves the archived frame pixels and decodes each frame within its bounds. The paired `anim.mul`, `anim2.mul`, `anim3.mul`, and `anim5.mul` were identical across the two asset sets. All other `anim4.mul` blocks and the index match across the two inputs and the repair.

Local evidence is under `D:/ConficturaUOTestTools/body-anchor-native-20261107-102832`. See `evidence/baseline-events.tsv`, `evidence/art-swap-events.tsv`, `evidence/asset-verification.json`, and `client/Logs/2026-11-07_10-45-06_crash.txt`. The isolated runtime and port were closed. Hash checks passed for the original saves, client profiles, animation inputs, and server sources.

## Prepare the client update

From the repository root, with Python 3.11 or later, run the repair against a matching client asset set. Use an existing staging directory and a new output path. The script checks both input hashes, accepts either affected variant, and verifies the output before publishing it to the staging path. It refuses an existing destination. [Repair script](../../scripts/Repair-SpiritAnimation.py)

```powershell
python scripts/Repair-SpiritAnimation.py 'D:/ConficturaClient/Files' 'D:/client-staging/anim4.mul'
```

Expected SHA-256 for `anim4.mul` is `205267a7d48b9fa7f7f78aa5408cf30b56c4331ed6c1dc39e4efaf2e9a7c4e79`. Package it as `Files/anim4.mul` with the existing `anim4.idx` and `Bodyconv.def`. Keep the previous package available for rollback. Players get the correction when they install the updated asset and restart ClassicUO. This verification used the local Windows client, and client distribution remains a separate release step.
