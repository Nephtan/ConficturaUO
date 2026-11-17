# Player body alignment survey

Verified on November 7, 2026 in Windows ClassicUO 1.1.0.0 with the saved player Nier, the saved gypsy potion shelf, and a private copy of the world. The same shard stayed running on loopback through the baseline and repair. It loaded the copied save after forced runtime script compilation with `-service -nocache`.

## Survey

The copied shard enabled all three size tiers. The survey traced 172 creature choices (108 tier 1, 39 tier 2, and 25 tier 3), both Human appearances, and player body changes from Polymorph, Animal Form, Necromancy, Incognito, Deception, Rock Flesh, Séance, Astral Projection, and Barracoon's forced polymorph. Image-copy spells use the caster's existing graphic. The survey covered 205 distinct body IDs in 21 groups of up to ten models, with each model rendered on a marked tile in ClassicUO. This exercised the rendering for every cataloged body. The actual shelf and spell actions below exercised the specified entry points separately. [Race definitions](../../Data/Scripts/Mobiles/Races/BaseRace.cs) · [Potion shelf](../../Data/Scripts/Mobiles/Races/RacePotions.cs) · [Polymorph menu](../../Data/Scripts/System/Gumps/PolymorphGump.cs) · [Animal Form](../../Data/Scripts/Magic/Ninjitsu/AnimalForm.cs)

The survey started with the previously repaired Spirit animation. Seven other bodies each used a negative half-height anchor in every frame, placing the middle of the source canvas at the tile. Their visible support points appeared 30 to 81 pixels below the intended vertical origin in standing poses. The Fungal offset also reproduced after both actual potion selections with the server coordinates held fixed. The Fairy and floating Skeleton served as comparison bodies for airborne poses. [Wraith verification](2026-11-07-wraith-body-anchor.md) · [ClassicUO drawing origin](https://raw.githubusercontent.com/ClassicUO/ClassicUO/1.1.0.0/src/ClassicUO.Client/Game/GameObjects/Views/MobileView.cs#L681-L702)

| Shelf choice | Body | `anim4` slot | Baseline standing displacement below origin | Repaired shelf choice and marked tile |
| --- | ---: | ---: | ---: | --- |
| Blue Fungal | 341 | 49 | 42–50 px | Pass |
| Red Fungal | 342 | 50 | 34–49 px | Pass |
| Salamander | 673 | 59 | 43–81 px | Pass |
| Neptar | 676 | 62 | 30–73 px | Pass |
| Neptar | 677 | 63 | 38–56 px | Pass |
| Tritun | 678 | 64 | 38–63 px | Pass |
| Tritun | 690 | 65 | 38–63 px | Pass |

## Repair and retest

For each affected direction, the patch places the idle silhouette at the tile and centers the walking sequence on its median contact row. The other actions use the idle reference for the same facing. The correction shifts the run coordinates by the corresponding amount so that the decoder reconstructs the same frame pixels. An independent check covered all 400 affected records, 1,750 frames, and 307,835 pixel runs. It compared the decoded coordinates and colors, verified texture bounds, and confirmed that every byte outside the seven affected slots matched the Spirit-only file. [ClassicUO sprite decoder](https://raw.githubusercontent.com/ClassicUO/ClassicUO/1.1.0.0/src/ClassicUO.Assets/AnimationsLoader.cs#L1371-L1435)

One ClassicUO restart loaded the combined asset. The same marked gallery showed all seven corrected bodies at their tiles. Nier accepted every corrected potion through the real shelf gump. The fixture moved Nier to an open reference tile for visual comparison. An eastward step was observed in blue Fungal and the second Tritun, with the shard advancing X from 6606 to 6607. The Human potion restored body 401. Wraith Form, Lich Form, Horrific Beast, and Vampiric Embrace all rendered after native casts on that same tile.

Local evidence is under `D:/ConficturaUOTestTools/body-survey-native-20261107-110132`. See `evidence/catalog.tsv`, `evidence/baseline-assets.json`, `evidence/baseline-events.tsv`, `evidence/asset-swap.tsv`, and `evidence/repair-verification.json`. The survey covered body alignment, with live movement sampled separately from the frame checks. It used the local Windows client and the available Confictura client assets.

## Stage the combined client asset

From the repository root, run the repair with Python 3.11 or later. Supply a matching client asset directory and an unused path in an existing staging directory. Either known unpatched variant or the Spirit-only file produces the same combined result. The previous Spirit-only command remains available for its original inputs.

```powershell
python scripts/Repair-SpiritAnimation.py --player-forms 'D:/ConficturaClient/Files' 'D:/client-staging/anim4.mul'
```

Expected SHA-256 is `2ee189376602fa67d4a4a7ecc961016cd73529e65bc49a8660319735659612ad`. Package the staged file as `Files/anim4.mul` with the existing index and body conversion files. Keep the previous package for rollback. The client must reload the asset after installation. [Repair script](../../scripts/Repair-SpiritAnimation.py)
