# Outbreak economic observations

Current status: three completed comparable references and two bounded original-macro observations are recorded. Reward pickup, ordinary corpse valuation, repeatable farming and live market demand remain unmeasured. This is an observation record, not an economic-balance or profit pass. See [ECONOMIC_OBSERVATIONS.json](ECONOMIC_OBSERVATIONS.json).

## Recorded spend and source prices

Retained pilot008 stops alive after all broods, without killing the boss. Its recorded consumption is 142 bandages, 4 greater cure, 1 greater heal, 11 greater strength, 13 greater agility, 3 total refresh and 2 orange petals. Tithing falls from the frozen initial balance to the terminal balance by 630 points. This measures net balance reduction rather than every spell's gross expenditure. These observations come from a partial, not a complete encounter.

| Item | Consumed | Listed NPC base price | Replacement subtotal |
| --- | ---: | ---: | ---: |
| Bandage | 142 | 2 | 284 |
| Greater cure | 4 | 60 | 240 |
| Greater heal | 1 | 60 | 60 |
| Greater strength | 11 | 60 | 660 |
| Greater agility | 13 | 60 | 780 |
| Total refresh | 3 | 30 | 90 |
| Tithing points | 630 | 1 gold per donated point | 630 |
| **Priced subtotal** | | | **2,744** |

Bandages use SBHealer's GenericBuyInfo at StoreSalesList.cs:16829. Potion entries are SBAlchemist at lines 8251, 8263, 8299, 8335, 8359. The potion offers are conditional on MyServerSettings.SellRareChance and initially stock random quantities; this is not guaranteed replenishment at one visit. BaseVendor.GetPriceScalar applies town taxation, and GenericBuyInfo.Price applies its scalar. TithingGump consumes the offered gold and adds the same number of points.

The table excludes petals, repairs, travel, obtaining equipment, time, taxes/actual checkout and potential empty-bottle resale. Crafting or player trade can change acquisition costs. Evidence and source hash/line extraction: D:/ConficturaUOTestTools/outbreak-economics-20261029-002/price-observation.json. This clarifies the first report's tithing label as net balance replacement. VerifyBenchmark.py now reports the observed finite tithing balance reduction alongside supplies. A source-price subtotal is neither an actual shopping receipt nor a net-profit result.

## First complete mounted no-antivenom reference

Mounted-v4 run001 finishes in 1452.7598366 seconds with no player death. It consumes 154 bandages, 21 greater cures, 8 greater heals, 9 greater strengths, 12 greater agilities, 8 total refreshes and 2 orange petals; net tithing reduction is 650. Applying the same source base prices gives 4198 gold in priced recurring replacement resources (308+1260+480+540+720+240+650). Petals, repairs, travel, equipment, actual vendor stock/tax/checkout and time remain excluded.

The owned horse came from one declared fixture cage before freezing; its nominal base acquisition price is 550, separate from recurring resources. Native cage release/mounting passed, but no NPC purchase or actual gold expenditure is claimed. No replacement horse was used during combat. Evidence: D:/ConficturaUOTestTools/outbreak-mounted-benchmark-20261029-001/evidence/benchmark-verification.json and priced-cost-observation.json.

This completes one resource-cost observation. The benchmark did not observe collection of the gold shower, ordinary corpse loot or pending personal rewards before cleanup, so subtracting 4198 from 15000 is not an observed profit. The separate archmage clear directly observed the 15,000 ground gold and pending 6 glands/1 skull; it does not substitute for collection in this reference case. This was the first accepted resource observation; the later three-run pacing series is now complete.

## Second complete reference with antivenom

Mounted-v4 run002 completes in 1526.8203048 seconds without a player death. Actual use is 171 bandages, 16 greater cures, 4 greater heals, 11 greater strengths, 13 greater agilities, 2 total refreshes, 2 orange petals and 12 antivenom treatments; net tithing reduction is 700. The same priced categories total 3742 gold (342+960+240+660+780+60+700), excluding all 12 treatments and other unpriced costs. The horse is the same declared preparation asset, with no replacement during combat. Evidence is run002's benchmark-verification.json and priced-cost-observation.json.

These two independent randomized encounters do not isolate antivenom's causal effect or establish a cheaper loadout, player demand or net profit. Both retain unobserved reward collection and corpse loot. Keep the actual doses and their unpriced ingredient/trade cost visible when comparing the subtotals; no reward change follows from them.

## Third complete reference and accepted pacing

Run003 finishes without antivenom in 1465.6041709 seconds. Actual use: 199 bandages, 23 greater cures, 8 greater heals, 10 greater strengths, 12 greater agilities, 2 total refreshes and 2 petals; net tithing reduction 680. The same priced categories total 4318 (398+1380+480+600+720+60+680), with the earlier exclusions and no collected-loot/profit claim. All three now pass the pacing series median 24.426736 minutes. Resource costs and reward collection retain their separate evidence limits; the one antivenom case does not establish causal savings or market demand.

## Reward value remains a separate question

The specified 15,000 gold is a shared ground shower, not 15,000 per participant. RollVictory creates 25 stacks of 600 only after the boss victory path succeeds; pilot008 did not earn it. Four inherited UltraRich corpse packs and the skull/artifact opportunity are separate from that fixed sum. Gold actually collected, ordinary loot retained, travel/looting time and repairs remain unmeasured; all three completed-reference resource spends are recorded above.

Six personal glands from a fully qualified clear can make 12 treatments at guaranteed Alchemy 100 success, consuming 12 petals, 36 garlic and 12 bottles as well. Actual crafter prices, failure costs below 100, demand and support/pet use still need evidence. A skilled bandager's few potion cures do not show that protection has no value: prevention can preserve a healing action or protect another recipient. They also do not prove that hunters will buy the reward. Do not increase poison pressure merely to manufacture demand.

The completed references and controlled support/pet cases now supply bounded evidence. Repeatable farming and player demand remain future observations before declaring market value or changing rates. Keep the fixed gold shared, unique power inflation out of this reward, and the role of crafting/trade visible. No reward or general economy change follows from this subtotal.

## Source traces

- [StoreSalesList.cs](../../Data/Scripts/Mobiles/Base/StoreSalesList.cs): SBAlchemist/SBHealer buy offers; SellInfo values are not substituted for player purchase prices.
- [Settings.cs](../../Data/Scripts/System/Misc/Settings.cs): MyServerSettings.SellRareChance.
- [BaseVendor.cs](../../Data/Scripts/Mobiles/Base/BaseVendor.cs) and [GenericBuy.cs](../../Data/Scripts/Mobiles/Base/GenericBuy.cs): price scalar and actual listed-price calculation.
- [TithingGump.cs](../../Data/Scripts/System/Gumps/TithingGump.cs): gold donation/point conversion.
- [ArachnidOutbreakController.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs): RollVictory,25 validated gold positions, shared award routing.
- [Mephitis.cs](../../Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Mephitis.cs): inherited four UltraRich packs. [LootPack.cs](../../Data/Scripts/Items/Containers/LootPack.cs): actual pack definitions.
- [DefAlchemy.cs](../../Data/Scripts/Trades/Crafting/DefAlchemy.cs), [VerifyBenchmark.py](../../tools/champion-outbreak/VerifyBenchmark.py), [BENCHMARK.md](BENCHMARK.md): recipe, frozen/native supply observations and incomplete-run limits.

## Final measurement reconciliation

The completed references record0 player deaths and0 Sacred Journey cast requests/retreat arrivals. Tactical kiting and normal movement remain inside combat time; they are not relabeled as separate journey retreats. Antivenom consumption is0/12/0 across the three runs. Gold pickup, ordinary corpse contents/value and collected personal rewards are unknown in these references, rather than invented zeroes. The priced subtotals4198/3742/4318 exclude the stated costs, including all12 treatments in the middle run.

The actual Zanthus route observation separately records277.5015906 seconds,0 deaths,15,000 ground gold and6 gland/1 skull/0 artifact claims pending. Before/after backpack manifests both contain91 item entries, with0 new serials and0 backpack gold before/after. That verifies no inventory gain in those endpoint manifests; it does not identify every corpse item or substitute for a transaction history. The stationary observation remains a distinct partial case in the JSON. A zero artifact claim in one clear is not a drop-rate estimate.

[RecordEconomicObservations.py](../../tools/champion-outbreak/RecordEconomicObservations.py) reads the accepted reports, finite consumption/cost records, native action logs and private endpoint manifests; it never starts gameplay. Exact source evidence hashes remain in the private aggregate. This satisfies the specification's separate measurement record with explicit unknowns; it does not establish that15,000 was collected or that the event is profitable. A new20-35 minute reference replay solely to replace an unknown collection policy is not justified by the unchanged encounter. Native claim visibility/delivery has its separate controlled acceptance record. Sustained market demand, enjoyment and broader beginner/loot progression remain outside this prototype.
