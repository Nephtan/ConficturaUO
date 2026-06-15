# System Name: Offline Skill Training

## Overview

Offline Skill Training is implemented by `StudyBook` items plus the `StudyBookbinder` vendor. A player schedules one book while logged in, the logout event converts that scheduled book into an active study session, and the next login resolves the elapsed offline time into normal `SkillCheck.Gain` attempts for the book's configured skill.

The system does not write skill values directly. It only creates repeated gain rolls, so final gains still depend on the shard's normal skill-gain rules, the player's current skill, the player's cap, and the study book's cap.

## Script Inventory

* `Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs`
* `Data/Scripts/Custom/Offline Skill Training/Items/StandardStudyBooks.cs`
* `Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs`
* `Data/Scripts/Custom/Offline Skill Training/Items/LegendaryStudyBooks.cs`
* `Data/Scripts/Custom/Offline Skill Training/Items/RandomStudyBooks.cs`
* `Data/Scripts/Custom/Offline Skill Training/Vendors/StudyBookbinder.cs`
* `Data/Scripts/Custom/Offline Skill Training/Vendors/SBStudyBookbinder.cs`
* `Data/Scripts/System/Skills/SkillCheck.cs` for the accelerated-gain multiplier

## Player Flow

1. Put a study book in your backpack.
2. Set the matching skill lock to `Up`.
3. Double-click the book while out of combat.
4. The book stores `Reader = from` and `_WillStartStudyNextLogout = true`.
5. On logout, `StudyBook.OnLogout` scans the player's backpack and calls `StartStudy` on matching books.
6. `StartStudy` marks the book in use, makes it immovable, clears the pending flag, and records `DateTime.Now`.
7. On the next login, `StudyBook.OnLogin` scans the backpack again and calls `EndStudy`.
8. `EndStudy` converts offline duration into gain attempts, restores the book to movable, clears `Reader`, and may consume the book.

Double-clicking a scheduled book before logout cancels it. Double-clicking an in-use book owned by the same player calls `EndStudy`, which matters mainly for edge cases such as persisted active books after a restart.

## Code-Verified Behavior

* **Book tiers:** Standard books train up to `700` fixed-point skill (70.0), advanced books to `1000` (100.0), and legendary books to `1200` (120.0).
* **Random books:** Random study books choose one skill at construction from the hard-coded list in `RandomStudyBooks.cs`. The list includes custom skills such as `Knightship`, `Elementalism`, `Seafaring`, `Psychology`, `Mercantile`, `Searching`, and `Spiritualism`.
* **Scheduling restrictions:** The book must be in the caller's backpack, the target skill must still be below both the book cap and the character's skill cap, the skill lock must be `Up`, and the player cannot be in combat.
* **No enforced safe-region rule:** The script contains TODO comments for jail, town, and house-region restrictions, but those checks are commented out and do not run.
* **One scheduled book per player:** When a player schedules a book, the backpack is scanned for any other `StudyBook` whose `Reader` is already that player. If found, the new book is rejected.
* **Offline time conversion:** `EndStudy` computes `toGain = floor(elapsedSeconds / 30)`. Each point in `toGain` represents one 0.1-skill gain attempt before later caps and randomness.
* **Book-cap clamp:** `toGain` is reduced if the player would exceed `MaxSkillTrained`.
* **Post-100.0 slowdown:** If the attempted gains cross `1000` fixed-point skill, the portion above 100.0 is halved before the attempt loop runs.
* **Per-session ceiling:** `toGain` is capped at `350`, which is a maximum of 35.0 skill worth of attempts before randomness.
* **Attempt chance:** Each attempt recomputes a chance as `0.9 - ((skill.Value / skill.Cap) * 0.8)`, then clamps it to the range 10% through 90%.
* **Actual gain call:** Successful attempts call `SkillCheck.Gain(from, skill)`. Failed attempts do nothing.
* **Loop stop conditions:** The attempt loop exits early if the player has no total skill cap, reaches the book cap, gains 35.0 actual skill during this session, or reaches the skill's own cap.
* **Book decay:** After study resolves, the book has a 5% chance to delete itself.
* **Acceleration reward:** Studying for at least 5 hours grants 30 minutes of accelerated gain for the studied skill if the reader is a `PlayerMobile` and acceleration is enabled.
* **Acceleration math:** During that 30-minute window, `SkillCheck.Gain` multiplies the fixed-point gain by a random value from 2 through 5 when the gained skill matches `PlayerMobile.AcceleratedSkill`.

## Vendor Behavior

* `StudyBookbinder` is a mage-guild vendor named "the training manual bookbinder".
* `SBStudyBookbinder` only adds standard study books to the buy list.
* Each standard book is gated independently by `MyServerSettings.SellChance()`, so binder inventory is randomized rather than guaranteed.
* Dropping a study book onto the binder deletes the book and pays:
  * `500` gold for books under 100.0 cap
  * `2500` gold for books with 100.0 cap
  * `10000` gold for books with 120.0 cap

## Technical Trace

* Wiki claim: books give direct offline skill points -> traced `StudyBook.EndStudy` -> code converts elapsed time into repeated `SkillCheck.Gain` attempts, so gains remain probabilistic.
* Wiki claim: one gain tick equals 30 seconds -> traced `SkillGainInterval = 30` and `SkillGainedPerTick = 1` -> code awards one 0.1-skill attempt per full 30-second interval before caps.
* Wiki claim: sessions can grant up to 35.0 skill -> traced `SkillGainMax = 350` plus the `EndStudy` loop -> code caps attempted gain at 35.0 and also aborts if actual gained skill reaches 35.0 inside the loop.
* Wiki claim: gains above 100.0 are slower -> traced the `toGain` adjustment in `EndStudy` -> code halves only the attempted portion above 100.0, not the entire session unless the player is already above 100.0.
* Wiki claim: 5 hours of study grants an alacrity-style bonus -> traced `EndStudy` and `SkillCheck.Gain` -> code sets `PlayerMobile.AcceleratedSkill`, stores an expiration time in `AcceleratedStart`, and multiplies matching skill gains by 2 to 5 while the timer is still active.
* Wiki claim: study is restricted to safe regions -> traced `OnDoubleClick` -> the relevant jail, town, and house checks are commented out, so no region restriction is currently enforced.
* Wiki claim: the binder sells study books -> traced `SBStudyBookbinder` -> code only sells standard books, and each individual entry appears only when `MyServerSettings.SellChance()` returns true.

## Persistence

`StudyBook` serializes version `0` and then writes:

* `_TrainingSkill`
* `_MaxSkillTrained`
* `_Reader`
* `_IsInUse`
* `_StartStudy`
* `_WillStartStudyNextLogout`

The derived standard, advanced, legendary, and random study book classes each serialize their own version `0` after the base serializer. `StudyBookbinder` also serializes version `0` only.

## XMLSpawner

This system does not use XMLSpawner hooks, attachments, or XMLSpawner-driven configuration. It is driven by item double-clicks, login/logout event hooks, vendor stock generation, and vendor drag-drop handling.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0027; PBN-0041; PBN-0050; PBN-0126.
- Backlog rows: RB-06735; RB-06736; RB-06737; RB-06738.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs (CurrentFile)
- Data/Scripts/Custom/Offline Skill Training/Items/StandardStudyBooks.cs (CurrentFile)
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs (CurrentFile)
- Data/Scripts/Custom/Offline Skill Training/Items/LegendaryStudyBooks.cs (CurrentFile)
- Data/Scripts/Custom/Offline Skill Training/Items/RandomStudyBooks.cs (CurrentFile)
- Data/Scripts/Custom/Offline Skill Training/Vendors/StudyBookbinder.cs (CurrentFile)
- Data/Scripts/Custom/Offline Skill Training/Vendors/SBStudyBookbinder.cs (CurrentFile)
- Data/Scripts/System/Skills/SkillCheck.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Event=4; Initialize=3; Login=1; Logout=1; Timer=1.
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L74 Event EventSink access=GlobalOrInternal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L75 Event EventSink access=GlobalOrInternal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L226 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L257 Logout OnLogout access=Internal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L277 Login OnLogin access=Internal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L391 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L393 Event EventSink access=GlobalOrInternal
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs:L394 Event EventSink access=GlobalOrInternal
- Data/Scripts/System/Skills/SkillCheck.cs:L75 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/SkillCheck.cs:L96 Initialize Initialize access=GlobalOrInternal

### Serialization Evidence

- Serialized rows matched: 170.
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedAlchemyStudyBook version=0 serialize=L14 deserialize=L21 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedAnatomyStudyBook version=0 serialize=L38 deserialize=L45 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedArmsLoreStudyBook version=0 serialize=L110 deserialize=L117 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedBeggingStudyBook version=0 serialize=L158 deserialize=L165 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedBlacksmithStudyBook version=0 serialize=L182 deserialize=L189 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedBludgeoningStudyBook version=0 serialize=L998 deserialize=L1005 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedBowcraftStudyBook version=0 serialize=L206 deserialize=L213 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedBushidoStudyBook version=0 serialize=L1262 deserialize=L1269 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedCampingStudyBook version=0 serialize=L254 deserialize=L261 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedCarpentryStudyBook version=0 serialize=L278 deserialize=L285 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedCartographyStudyBook version=0 serialize=L302 deserialize=L309 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs:Server.Items.AdvancedCookingStudyBook version=0 serialize=L326 deserialize=L333 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/LegendaryStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/LegendaryStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/RandomStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/RandomStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/StandardStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/StandardStudyBooks.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Vendors/SBStudyBookbinder.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Vendors/SBStudyBookbinder.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Vendors/StudyBookbinder.cs=Keep
- Data/Scripts/Custom/Offline Skill Training/Vendors/StudyBookbinder.cs=Keep
- Data/Scripts/System/Skills/SkillCheck.cs=Keep
- Data/Scripts/System/Skills/SkillCheck.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
