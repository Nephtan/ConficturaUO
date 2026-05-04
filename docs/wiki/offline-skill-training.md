# System Name: Offline Skill Training

## Overview

Offline Skill Training is built around `StudyBook` items and the `StudyBookbinder` vendor. A player double-clicks a study book in their backpack to schedule study for the next logout. On logout, the selected book records the start time; on the next login, it converts elapsed offline time into skill-gain attempts for the book's configured skill.

The system does not directly grant guaranteed skill points. It attempts skill gains through `SkillCheck.Gain`, so the final amount depends on skill cap, current skill, randomized success checks, and the normal shard skill-gain rules.

---

## Player Use

1. Obtain a study book for the skill you want to train.
2. Put the book in your backpack.
3. Set that skill to increase.
4. Double-click the book to schedule study for your next logout.
5. Log out. The book begins studying only if it is still in your backpack.
6. Log back in to resolve the study session.

Double-clicking the scheduled book again before logout cancels the pending study. If a study session is already active for that character, the book resolves it instead.

---

## Code-Verified Behavior

* **Book state:** `StudyBook` stores `TrainingSkill`, `MaxSkillTrained`, `Reader`, `_IsInUse`, `_StartStudy`, and `_WillStartStudyNextLogout`.
* **Book tiers:** Standard books train to 70.0 skill, advanced books train to 100.0 skill, and legendary books train to 120.0 skill. Random books choose one skill from their built-in skill list and use the same tier caps.
* **Starting study:** Double-clicking a valid backpack book sets `Reader = from` and `_WillStartStudyNextLogout = true`.
* **Logout hook:** `StudyBook.Initialize()` registers login and logout handlers. `OnLogout` scans the player's backpack for a matching scheduled book and calls `StartStudy`.
* **Login hook:** `OnLogin` scans the player's backpack for a matching active book and calls `EndStudy`.
* **Eligibility checks:** The book must be in the player's backpack, the trained skill must be set to increase, the player must be below the book cap and skill cap, and the player cannot schedule a new book while in combat.
* **One book per player:** When scheduling a book, the backpack is checked for any other `StudyBook` whose `Reader` is the same player.
* **Gain math:** Offline time is converted into `toGain = (elapsedSeconds / 30)`, measured in fixed skill tenths. This is capped by the book's max skill, reduced for gains above 100.0, and capped at 350 attempts per session.
* **Randomized gains:** Each gain attempt calculates a chance from current skill versus skill cap, bounded between 10% and 90%, and only successful attempts call `SkillCheck.Gain`.
* **Accelerated gains:** Studying at least 5 hours applies a 30-minute accelerated-gain state for the trained skill. While active, matching `SkillCheck.Gain` increases are multiplied by a random 2 to 5.
* **Book wear:** After study resolves, the book has a 5% chance to disintegrate.
* **Vendor sales:** `StudyBookbinder` uses `SBStudyBookbinder`, which sells standard study books at 7,500 gold each when `MyServerSettings.SellChance()` includes that item in the vendor stock.
* **Book returns:** Dropping a `StudyBook` onto a `StudyBookbinder` deletes the book and pays 500 gold for standard-cap books, 2,500 gold for advanced-cap books, or 10,000 gold for legendary-cap books.

---

## Technical Trace

* Wiki claimed study books grant skill while logged out -> traced `StudyBook.EndStudy` -> code converts elapsed time into randomized skill-gain attempts, not guaranteed direct skill awards.
* Wiki claimed 30 seconds per 0.1 skill -> traced `SkillGainInterval` and `SkillGainedPerTick` -> code creates one fixed-point gain attempt per 30 seconds.
* Wiki claimed a 35.0 skill maximum per session -> traced `SkillGainMax` and the `EndStudy` loop -> code caps attempts at 350 fixed points and stops once actual gained skill reaches 35.0.
* Wiki claimed gains above 100.0 are half speed -> traced the `toGain` adjustment -> code halves only the attempted gain portion above 100.0 before randomized attempts run.
* Wiki claimed a 5-hour study grants a 30-minute accelerated buff -> traced `EndStudy` and `SkillCheck.Gain` -> code sets `PlayerMobile.AcceleratedSkill` and expiration time, then multiplies matching gains by 2 to 5 while active.
* Wiki claimed vendors sell and buy study books -> traced `SBStudyBookbinder` and `StudyBookbinder.OnDragDrop` -> code sells standard stock through randomized vendor availability and pays return rewards based on `MaxSkillTrained`.
* Wiki claimed only one book can be studied at a time -> traced `OnDoubleClick` -> code blocks scheduling when another backpack `StudyBook` already has the same player as `Reader`.

---

## Persistence

`StudyBook` serializes version `0`, then writes and reads:

* `_TrainingSkill`
* `_MaxSkillTrained`
* `_Reader`
* `_IsInUse`
* `_StartStudy`
* `_WillStartStudyNextLogout`

Derived standard, advanced, legendary, and random study book classes also write their own version `0` after calling the base serializer. `StudyBookbinder` serializes version `0` only.

---

## XMLSpawner

No XMLSpawner hooks, attachments, spawn definitions, or XMLSpawner configuration references are used by this system. It is driven by item double-clicks, login/logout events, vendor stock, and vendor drag-drop handling.
