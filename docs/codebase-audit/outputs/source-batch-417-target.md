# SOURCE-BATCH-417 TarotCards Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-417`
- Candidate: `SB417-CAND-001`
- System: `Items:Special / Rares / TarotCards`
- Files:
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoDeckOfTarot.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoDeckOfTarot2.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot2.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot3.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot4.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot5.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot6.cs`
  - `Data/Scripts/Items/Special/Rares/TarotCards/DecoTarot7.cs`
- Behavior: add stale/null mobile, deleted source tarot item, stale gump response, and static gump-send guard coverage around the TarotCards interaction path.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Tarot item IDs, names, movable flags, and stackable flags.
- Range check and localized reach message.
- `TarotCardsGump` layout.
- Random tarot text generation.
- Card image selection.
- Refresh and cancel button behavior.
- Gypsy sound and base sound `0x5BB`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
