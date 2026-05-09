# System Name: Animal Shape Stones

## Overview

Animal Shape Stones are constructable, blessed, non-movable `Item` scripts that temporarily change a `Mobile`'s `BodyValue`, and for most variants also change `Hue`. The system is implemented as five independent `Server.Items` classes under `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/`.

Each stone toggles a single stored transformed flag. A double-click calls `Transform(Mobile from)` when the stone is not transformed and `UnTransform(Mobile from)` when it is transformed. The same transform and untransform methods can also be triggered by exact speech keywords when the stone is inside the speaker's backpack.

There are no custom `[Command]` registrations, `EventSink` hooks, packet handlers, XMLSpawner attachments, or combat/stat formulas in this system. Administration uses standard RunUO item placement and property inspection for the constructable stone classes.

Code-Verified: 2026-05-07

## Script Inventory

| Script | Type | Role |
| --- | --- | --- |
| `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/DragonShapeChangeStone-body.cs` | `DragonShapeShiftStone` | Dragon-family shape stone with a context-menu `Gump` for selecting the dragon body. |
| `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/FaeryShapeChangeStone.cs` | `FaerieShapeShiftStone` | Faerie shape stone with fixed male/female faerie bodies and hue reset to `0`. |
| `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/FelinusShapeChangeStone.cs` | `FelinusShapeShiftStone` | Cat shape stone with context-menu `Gump` for selecting the applied hue. |
| `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/RatShapeChangeStone.cs` | `RatShapeShiftStone` | Rat shape stone with context-menu `Gump` for selecting the applied hue. |
| `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/WolvenShapeChangeStone.cs` | `WolvenShapeShiftStone` | Wolf shape stone with context-menu `Gump` for selecting the applied hue. |

`Data/Scripts/Scripts.csproj` explicitly compiles all five scripts.

## Constructable Items

| Constructable | ItemID | Name | Movable | LootType | Stone Hue |
| --- | ---: | --- | --- | --- | ---: |
| `DragonShapeShiftStone` | `0x1870` | `a Dragon ShapeShift Stone` | `false` | `Blessed` | `37` |
| `FaerieShapeShiftStone` | `0x1870` | `a Faerie ShapeShift Stone` | `false` | `Blessed` | `1952` |
| `FelinusShapeShiftStone` | `0x1870` | `a Felinus ShapeShift Stone` | `false` | `Blessed` | `1952` |
| `RatShapeShiftStone` | `0x1870` | `a Rat ShapeShift Stone` | `false` | `Blessed` | `1952` |
| `WolvenShapeShiftStone` | `0x1870` | `a Wolven ShapeShift Stone` | `false` | `Blessed` | `1952` |

## Player Interaction

| Entry Point | Requirements In Code | Behavior |
| --- | --- | --- |
| Double-click | No backpack, range, alive, ownership, or access check in `OnDoubleClick(Mobile from)`. | Toggles the stone between transformed and untransformed states for the double-clicking `Mobile`. |
| Speech: `transform` | `HandlesOnSpeech == true`; `!e.Handled`; exact speech text is `transform`; stone is `IsChildOf(e.Mobile.Backpack)`; `m_Transformed == 0`. | Calls `Transform(e.Mobile)` and marks speech handled. |
| Speech: `untransform` | `HandlesOnSpeech == true`; `!e.Handled`; exact speech text is `untransform`; stone is `IsChildOf(e.Mobile.Backpack)`; `m_Transformed == 1`. | Calls `UnTransform(e.Mobile)` and marks speech handled. |
| Context menu | Dragon, Felinus, Rat, and Wolven variants only; `from.Alive` must be true. | Adds a context entry with cliloc `0313` that opens a configuration `Gump`. |

The speech keywords are case-sensitive because the scripts switch directly on `e.Speech` without normalizing it.

## Transformation Effects

| Stone | Stored Before Transform | Body Applied While Transformed | Hue Applied While Transformed | Restore On Untransform |
| --- | --- | --- | --- | --- |
| `DragonShapeShiftStone` | `m_BodyValue = 401` for female, otherwise `400`. | `SBodyValue` selected by the dragon body `Gump`. Default is `0` until selected or loaded. | None. | Restores `BodyValue` to `m_BodyValue`; hue is not changed. |
| `FaerieShapeShiftStone` | `m_BodyValue = 401` for female, otherwise `400`; `m_SkinHue = from.Hue`. | Female: `176`; male: `58`. | `0`. | Restores `BodyValue` to `m_BodyValue`; restores `Hue` to `m_SkinHue`. |
| `FelinusShapeShiftStone` | `m_BodyValue = 401` for female, otherwise `400`; `m_SkinHue = SHue`. | `214`. | `SHue` selected by the hue `Gump`; default is `0` until selected or loaded. | Restores `BodyValue` to `m_BodyValue`; restores `Hue` to `m_SkinHue`. |
| `RatShapeShiftStone` | `m_BodyValue = 401` for female, otherwise `400`; `m_SkinHue = SHue`. | `238`. | `SHue` selected by the hue `Gump`; default is `0` until selected or loaded. | Restores `BodyValue` to `m_BodyValue`; restores `Hue` to `m_SkinHue`. |
| `WolvenShapeShiftStone` | `m_BodyValue = 401` for female, otherwise `400`; `m_SkinHue = SHue`. | `225`. | `SHue` selected by the hue `Gump`; default is `0` until selected or loaded. | Restores `BodyValue` to `m_BodyValue`; restores `Hue` to `m_SkinHue`. |

No stone modifies stats, skills, damage, resistances, equipment, movement, AI, access level, notoriety, or account state.

## Configuration Gumps

### Dragon Body Gump

`DragonShapeShiftStone` adds a context menu entry for living mobiles. The entry opens `BodyValueGump`, which stores the selected dragon body in `s_BodyValue`.

| Button ID | Label | Assigned `s_BodyValue` |
| ---: | --- | ---: |
| `1` | `BigBrown` | `12` |
| `2` | `BigRed` | `59` |
| `3` | `SmlBrown` | `60` |
| `4` | `SmlRed` | `61` |
| `5` | `Wyvern` | `62` |

The `OnResponse(NetState sender, RelayInfo info)` method closes the gump after assigning the value. It does not transform the player immediately; the new body value is used on the next transform.

### Hue Gumps

`FelinusShapeShiftStone`, `RatShapeShiftStone`, and `WolvenShapeShiftStone` each define the same `HueGump`. The selected hue is stored in `s_Hue`.

| Button ID | Display Label | Display Hue | Assigned `s_Hue` |
| ---: | --- | ---: | ---: |
| `1` | `@@@@@` | `0` | `1175` |
| `2` | `default` | `646` | `0` |
| `3` | `@@@@@` | `642` | `644` |
| `4` | `@@@@@` | `644` | `646` |
| `5` | `@@@@@` | `1153` | `1153` |

`FaerieShapeShiftStone` has no context menu and no configurable `Gump`.

## Command Properties

All five items expose diagnostic properties to `AccessLevel.GameMaster`.

| Stone | Property | Backing Field | Notes |
| --- | --- | --- | --- |
| All variants | `Transformed` | `m_Transformed` | `0` means untransformed; `1` means transformed. |
| All variants | `BodyValue` | `m_BodyValue` | Stores the human body value selected by the transform method. |
| Faerie, Felinus, Rat, Wolven | `SkinHue` | `m_SkinHue` | Faerie stores the caller's current hue; Felinus/Rat/Wolven store `SHue`. |
| Felinus, Rat, Wolven | `SHue` | `s_Hue` | Hue applied while transformed. |
| Dragon | `SBodyValue` | `s_BodyValue` | Body applied while transformed. |
| Dragon | `SkinBodyValue` | `m_SkinBodyValue` | Serialized field exposed as a property, but no traced code assigns or uses it during transform/untransform. |

The properties are getters only, so staff cannot set them directly through standard property editing unless the RunUO property system bypasses normal setters.

## Serialization

All five classes include the required `Serial` constructor and override `Serialize(GenericWriter writer)` and `Deserialize(GenericReader reader)`.

| Stone | Version Written | Serialized Payload After Version | Deserialize Pattern |
| --- | ---: | --- | --- |
| `DragonShapeShiftStone` | `1` | `s_BodyValue`, `m_Transformed`, `m_SkinBodyValue`, `m_BodyValue` | `switch (version)` with `case 1` reading `s_BodyValue`, then `goto case 0`. |
| `FaerieShapeShiftStone` | `0` | `m_Transformed`, `m_SkinHue`, `m_BodyValue` | Straight version `0` read order. |
| `FelinusShapeShiftStone` | `1` | `s_Hue`, `m_Transformed`, `m_SkinHue`, `m_BodyValue` | `switch (version)` with `case 1` reading `s_Hue`, then `goto case 0`. |
| `RatShapeShiftStone` | `1` | `s_Hue`, `m_Transformed`, `m_SkinHue`, `m_BodyValue` | `switch (version)` with `case 1` reading `s_Hue`, then `goto case 0`. |
| `WolvenShapeShiftStone` | `1` | `s_Hue`, `m_Transformed`, `m_SkinHue`, `m_BodyValue` | `switch (version)` with `case 1` reading `s_Hue`, then `goto case 0`. |

The write and read order matches for the traced versions. Version `0` saves of Dragon/Felinus/Rat/Wolven do not contain the newer selected body/hue field, so those fields default to `0` when loading old saves.

## Technical Trace

* Target acquisition: `docs/SystemAudit.md` marked `Animal Shape Stones` as `Missing` and pointed to `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/DragonShapeChangeStone-body.cs`.
* Compile inclusion: `Data/Scripts/Scripts.csproj` includes all five shape stone scripts.
* Entry points: each stone implements `OnDoubleClick(Mobile from)`, `HandlesOnSpeech`, and `OnSpeech(SpeechEventArgs e)`.
* Gump flow: Dragon uses `BodyValueGump`; Felinus, Rat, and Wolven use `HueGump`; Faerie has no gump.
* Persistence: each stone serializes its item-level transformed/configuration fields after `base.Serialize(writer)` and reads them after `base.Deserialize(reader)`.

## Known Issues

* Transform state is stored on the `Item`, not per `Mobile`. A shared stone can only remember one transformed state, one restore body, and one restore hue at a time.
* `OnDoubleClick(Mobile from)` has no range, backpack, alive, access, or ownership checks. World-placed non-movable stones can be toggled directly by any double-clicking mobile that reaches the item through the normal RunUO double-click path.
* The restore body is not captured from `from.BodyValue`; transform hard-codes `401` for female and `400` otherwise. Non-human or already-transformed body values are not restored accurately.
* `FelinusShapeShiftStone`, `RatShapeShiftStone`, and `WolvenShapeShiftStone` assign `m_SkinHue = SHue` during transform instead of storing `from.Hue`, so untransform restores the selected animal hue rather than the player's prior hue.
* `DragonShapeShiftStone` can transform to `BodyValue = 0` when `SBodyValue` has never been selected or loaded.
* Dragon's `m_SkinBodyValue`/`SkinBodyValue` is serialized and exposed but is not assigned or used by the traced transform logic.
* The nested gump response handlers read `sender.Mobile` and close the gump without null/deleted/mobile-state guards.
* There is no automatic cleanup on death, logout, item deletion, or world save/load for players left in a transformed body.
