# Confictura Codex World Map

## Run 1 - Before Fate

I wake up in the Forest at `Map.Sosaria`, `Point3D(3579,3423,0)`, with the welcome window still in my face and no tarot card chosen. The code has already made me a real `PlayerMobile`: human, not young, public MyRunUO on, hunger and thirst full at 20, stat cap 250, default skill cap 100.0, and a starter backpack. I am not in Lodoria, Savage, Titan anything, Atlantis, or any other earned place. I have no discovery flags yet.

The first thing I can actually see is not the tarot deck. A client-range scan around my tile catches the potion shelf and the invisible backpack hint in the gypsy camp, and it catches the Protection Goddess spawner at `3563,3413,0`. The gypsy spawner is nearby at `3567,3401,0`, but it is outside the strict 18-tile box from my wake-up tile. So I walk northwest toward the visible camp instead of pretending I can already pick a destination.

That walk takes me to `Point3D(3570,3416,0)`. It is still inside the Forest start region. I am now close enough for Thuvia, the Goddess Of Protection, to turn toward me and talk about saying `help` if I want to choose PvP or PvE. I also brush the invisible `gypsy bag` helper, which tells me to open my backpack from the paperdoll. From here the gypsy, the visitor journal, and the potion shelf are all in normal visual range, but I do not answer Thuvia, I do not open the PvP/PvE gump, I do not talk to the gypsy, and I do not double-click the potion shelf. This run is movement plus passive visible reactions only.

Mechanical friction learned:

- Character creation puts a new human at `3579,3423,0` in `Map.Sosaria`, then a `WelcomeTimer` opens the start flow; tarot is not chosen during character creation.
- The Forest start region blocks player-vs-player harmful action and spell casting, and it sends the Forest welcome gump on entry.
- The gypsy only opens the tarot gump if I am sitting at `X=3567,Y=3404`, or if my `RaceID` is already greater than zero. I am a fresh human with `RaceID=0`, so walking near the tent is not enough.
- Normal tarot paging for this account would expose Sosaria starts and the alien crash page because alien choice is enabled. Lodoria pages are hidden until any account character has discovered `the Land of Lodoria`; the Savage page is hidden until any account character has discovered `the Savaged Empire`.
- The page-12 `STRNGTH` branch exists in `EnterLand`, but it is not a normal visible choice for this account. The current state must keep Savage locked.

Next pressure:

I am standing between Thuvia and the gypsy camp. The shiny thing on screen is Thuvia asking whether I want PvP/PvE information; the main progression thing is still the gypsy table at `3567,3404`. A fair next move is either say `help` to Thuvia, step to the table and talk to the gypsy, or inspect the potion shelf from close range if monster races are allowed.

## Run 2 - Thuvia Has a Shorter Ear Than Voice

I am still pre-tarot in the Forest, standing at `Point3D(3570,3416,0)`. Before doing anything clever, I scan the actual client box again. The gypsy and Thuvia are the only traced mobiles in range. The potion shelf, visitor journal, two invisible help tiles, and two forest sound items are the traced items in range. Nothing here makes Savage, Lodoria, or any other hidden route real.

Thuvia is the thing tugging at my attention. She can call out when I pass within 8 tiles, but the code does not let me casually yell back from the edge of that bubble. Her speech hook only claims speech from very close range, then her `OnSpeech` path checks range again before it calls `CheckSpeech`. So I walk up to `Point3D(3565,3414,0)`, close enough to the spawn-home position at `3563,3413,0`, and type `help`.

The result is just help. She explains that PvE means fighting monsters and PvP means fighting other players, then tells me to say `choose` if I want to make a choice. I do not say `choose`, I do not open the PvP/PvE gump, and I do not press any PvP/PvE button. My `NONPK` state is still neutral/null. This is information, not commitment.

Mechanical friction learned:

- `Map.GetItemsInRange` and `Map.GetMobilesInRange` use the default 18-tile update rectangle, so the camp should be treated as visible once I am near it.
- Thuvia's movement prompt is wider than her reliable speech interaction. Prompt range is 8; the explicit speech path is close-range and then gated by `CheckSpeech`.
- Saying `help` to Thuvia does not open a gump or set PvP/PvE state. Only `choose` opens the `PKNONPK` gump, and only that gump's normal buttons would set PvP, PvE, or neutrality.
- The gypsy tarot gate is unchanged: I am human with `RaceID=0`, not sitting at `3567,3404`, and no account character has discovered Lodoria or the Savaged Empire.

Next pressure:

I am now beside Thuvia, still in the start forest. The nearest real fork is whether to say `choose` and deal with PvP/PvE consent now, or leave her and sit at the gypsy table so the visible tarot deck can finally start.

## Run 3 - The Consent Window Is a Door, Not a Choice

I am still at `Point3D(3565,3414,0)`, before tarot, close enough to Thuvia that I can answer like a normal player instead of shouting across camp. I refresh the client-range scan before moving my hands. Thuvia is still the closest traced mobile. The gypsy is visible but the tarot gate is still the table tile at `3567,3404`; the potion shelf and visitor journal are visible camp furniture, not urgent from this spot.

So I say `choose`.

The result is not PvP and not PvE. It is a gump. Thuvia's speech path accepts `choose` and sends the `PKNONPK` window. That window has three visible buttons: `Mark me for PvP`, `Mark me for PvE`, and `Remain Neutral`. I do not press any of them. The code only mutates `NONPK` and title inside `PKNONPK.OnResponse`, so my consent state stays neutral/null for now. The gump also marks itself `Closable = false`, which makes the printed "close this window to remain neutral" line mechanically weaker than the actual `Remain Neutral` button.

Mechanical friction learned:

- Thuvia can prompt at range 8, but the speech route I used is the close route: `HandlesOnSpeech` claims speech within 3 tiles, then `OnSpeech` checks range 6 before `CheckSpeech`.
- `choose` only opens `PKNONPK`; it does not set PvP, PvE, title, or neutrality.
- `PKNONPK` has visible reply buttons 1 for PvP, 2 for PvE, and 0 for neutral. Until one of those normal buttons is pressed, the player is still `NONPK.Null`.
- Tarot remains untouched. I am still human `RaceID=0`, not at the gypsy table, and the account still has no Lodoria or Savaged Empire discovery flags.

Next pressure:

The screen is now asking for a visible consent choice. The honest next fork is choosing one of the three `PKNONPK` buttons or deciding whether the unclosable gump can be ignored while walking back toward the gypsy table.

## Run 4 - Neutral Is a Real Button

I am still before tarot, still in the Forest at `Point3D(3565,3414,0)`, and now the thing occupying the screen is not the gypsy or the card deck. It is Thuvia's `PKNONPK` window. I refresh the visible camp in my head before touching it: Thuvia is still beside me, the gypsy is visible to the north, the potion shelf and visitor journal are in the camp, and none of them outrank a modal choice that is already open.

So I press `Remain Neutral`.

This is not the same as closing the window. The window says closing would remain neutral, but the gump is explicitly not closable. The actual normal player path is the visible neutral reply button. Its response keeps `PlayerMobile.NONPK` at `Null`, clears any PvP/PvE title, sends the neutrality text, and closes the gump. The weird part is that `Null` is also what the command treats as not-yet-chosen, so neutral is mechanically reversible from the selector. I still have no tarot card, no Lodoria flag, no Savage flag, and no reason to act like I know the rest of the shard.

Mechanical friction learned:

- Thuvia's `choose` speech only opened the consent gump. The state change happens later in `PKNONPK.OnResponse`.
- `Remain Neutral` is visible as a button even though its reply ID is `0`, so this run can use it without forging a hidden action.
- Neutral is not PvE protection and not PvP enlistment. The code leaves the stored consent enum as `NONPK.Null` and the title as `null`; because `[ChoosePvP` reopens the gump for `Null`, neutrality is not stored as a distinct committed mode.
- The next honest progression target is the gypsy table tile at `3567,3404`, not a tarot destination. I am still a human with `RaceID=0`, and `ShardGreeterEntry.OnClick` will only open the deck from that table position.

Next pressure:

The modal is gone. The camp is still visible. I can now walk north to the table, pass the potion shelf and visitor journal on the way, and finally talk to the gypsy from the one tile that matters.

## Run 5 - The Table Tile Finally Matters

I am still pre-tarot, still in the Forest, starting at `Point3D(3565,3414,0)`. Before moving, I scan the client box again. Thuvia is behind me and no longer has an open window. The gypsy is visible at `3567,3401,0`; the potion shelf is visible at `3564,3406,0`; the visitor journal is on the table at `3570,3400,7`; and the invisible `gypsy help` tile sits exactly where a new player would walk, at `3565,3404,0`.

So I walk to `Point3D(3567,3404,0)`.

The walk itself teaches the next friction. The `gypsy help` helper fires when I get within five tiles and tells me to single-click the gypsy and choose `Talk`. I am now satisfying the hard table requirement for a human player, but I still have not clicked the gypsy. No tarot card is visible yet. The potion shelf is now close enough to use, and monster characters appear enabled by default, so it is a real temptation, but I do not double-click it. I do not use the visitor journal. I do not pick a hidden destination.

Mechanical friction learned:

- The visual range around both the old tile and the table tile contains the same two traced mobiles: Thuvia and the ShardGreeter. The gypsy is no longer just "visible"; I am now on the table coordinate that her context-menu gate checks.
- The `gypsy help` tile is not scenery. It is a decorated `HelpMessage` item, and walking near it sends the normal player instruction to single-click the gypsy and choose `Talk`.
- The `RacePotions` shelf is close enough from the table and `MonstersAllowed()` returns true from the default settings path, so a normal player could branch into the creature-race gump here. I leave it alone because the chronological action was movement to the table, not drinking a potion.
- Tarot access is still gated by a visible player action: single-click the gypsy and choose `Talk`. Until that happens, `GypsyTarotGump.OnResponse` and `EnterLand` are not in play.
- Lodoria pages remain hidden by `visitLodor == false`, and the Savage card remains hidden by `visitSavage == false`. Reaching the table does not grant a discovery flag.

Next pressure:

I am seated where the gypsy wanted me. The next honest move is either single-click the gypsy and choose `Talk`, or get distracted by the now-reachable potion shelf or visitor journal before any tarot page is exposed.

## Run 6 - Talk Opens the Deck, Not the Door

I am at `Point3D(3567,3404,0)`, still pre-tarot, with the camp crammed into the client box. The gypsy is three tiles north at `3567,3401,0`. The potion shelf is close enough to double-click, the visitor journal is right there on the table, and Thuvia is still behind me. The shelf and journal are tempting, but the gypsy is the visible progression NPC and I am finally on the tile her code demanded.

So I single-click the gypsy and choose `Talk`.

This is not a destination choice. The gypsy plays her sound, closes the welcome window and any race-potion window, then opens `GypsyTarotGump` at page `0`. Page `0` is the speech/introduction page. It has the visible next button that would send reply `99` and reopen the gump at page `1`; it does not have a card selection button and it does not call `EnterLand`. I have not picked Britain, Devil Guard, Savage, Lodoria, or the alien crash. I have only gotten the deck onto the screen, and the screen is now full of text I have not actually read yet.

Mechanical friction learned:

- The table gate is now satisfied by position, so `ShardGreeterEntry.OnClick` opens the tarot gump through normal context-menu flow instead of telling me to sit down.
- `GypsyTarotGump` page `0` is an introduction page. Cards are only drawn when the gump is constructed with `page > 0 && page < 50`; the current visible next action is the page-advance button, not a destination.
- The intro body is not decoration. It explains how the deck controls work, points back to the potion shelf, Thuvia, and the visitor journal, and warns that travel, dungeons, traps, merchants, and safer public buildings matter.
- `EnterLand` is still out of reach. It only runs from `OnResponse` when a visible card page sends a reply button at `page + 100`.
- The future visible card sequence is still governed by `pageShow`: Lodoria pages are skipped because no account character has discovered `the Land of Lodoria`, and the Savage page is skipped because no account character has discovered `the Savaged Empire`.
- The potion shelf and visitor journal remain real nearby distractions, but neither was double-clicked during this action.

Next pressure:

The deck is open on its intro page. The next honest move is to read the introduction and think over what it is telling me before I press the next button, close the gump, or get distracted by the potion shelf or visitor journal.

## Run 7 - Reading Is Not Drawing

I am still at `Point3D(3567,3404,0)`, still pre-tarot, and the open thing on the screen finally matters more than the camp furniture around me. I read the gypsy's introduction instead of pretending the next button has already been pressed.

The speech is not a wiki page and it is not a teleport. It is player-facing pressure. The gypsy tells me the deck is operated by the top-right button, then arrow buttons, then an OK button on a visible card. She points over my shoulder at the potion shelf before the reading, names Thuvia as the person outside the tent for PvP/PvE safety, and warns that taverns, inns, banks, guards, variable merchants, settlement outskirts, minor dungeons, darkness, hunger, traps, and death are all real concerns. She also says the visitor journal behind her is the last normal chance to change my name before drawing fate.

I do not press the top-right button. I do not page to the Emperor. I do not use the potion shelf or the journal. Reading gives me knowledge, not flags. My account still has no Lodoria discovery and no Savaged Empire discovery, so those hidden cards stay hidden.

Mechanical friction learned:

- `GypsyTarotGump` page `0` is a scrollable introduction body sourced from `GypsySpeech(from)`, with one visible top-right reply button `99`. It is closable, draggable, and not resizable.
- Reply `99` would reopen the same gump on page `1`, but this run does not send that response.
- `EnterLand` is still unreachable. It only runs from a visible card response at `ButtonID >= 100`; page `0` has no card-selection OK button.
- The introduction teaches real nearby options: the `RacePotions` shelf, Thuvia, and the `ChangeName` visitor journal. Those options remain physical/player actions, not automatic state changes.
- Tarot availability is unchanged. A future page turn can expose the normal Sosaria cards and the alien crash card, but Lodoria pages and the Savage card remain hidden until account discovery gates are earned.

Next pressure:

Now I have actually read the deck instructions. The next honest move is either press the visible top-right button to see page `1`, close the gump, or get distracted by the shelf or journal the gypsy just made sound important.

## Run 8 - The First Card Is Still Not Fate

I am still sitting at `Point3D(3567,3404,0)`, with the gypsy, potion shelf, visitor journal, Thuvia, and the same little forest noises still inside the client box. Nothing physical moves. The open tarot page is the thing under my cursor now, so I press the visible top-right button the gypsy told me about.

The speech page closes and the first real card appears: `THE EMPEROR`, `The City of Britain`. I read it as a Britain start, not as a completed choice. The card talks about Lord British, the castle, Britanny Bay, tavern rumors of something under the castle, farms, cemeteries, and the British tomb. That is player knowledge on the screen. My feet are still in the Forest.

The controls matter more than the card art. There is no back arrow from the first card. There is a right arrow to the next card, and there is an OK button at the top right that would draw this card. I do not press OK. I do not press the right arrow. Britain is visible, but it is not selected.

Mechanical friction learned:

- Page `0` reply `99` only closes the old tarot/welcome/race-potion gumps, sends `GypsyTarotGump` page `1`, and plays a page-turn sound.
- Page `1` is the first normal human card: `THE EMPEROR`, destination line `The City of Britain`, card graphic `1106`, no previous arrow, next arrow reply `2`, and OK reply `101`.
- `EnterLand(1)` would be reachable only by pressing the visible OK reply `101`. That is the path that would move a human to `Point3D(2999,1030,0)` on `Map.Sosaria` and set `the Land of Sosaria`.
- Because I only opened/read the card, no `MoveToWorld` happened, no discovery flag changed, and the Savage/Lodoria gates are still exactly where they were.

Next pressure:

The first actual fate is now staring at me. The honest next move is to decide whether to draw Britain with OK, page forward to see the Devil Guard card, close the deck, or get distracted by the potion shelf or visitor journal before committing.

## Run 9 - Paging Is Still Not Choosing

I am still on the table tile at `Point3D(3567,3404,0)`, still before fate, with `THE EMPEROR` already read and no destination chosen. I refresh the screen in my head before touching the deck: the gypsy is three tiles north, the potion shelf and visitor journal are still close enough to tempt me, Thuvia is still behind me, and the open gump has the visible right arrow I have not used yet. The furniture can wait. The card deck is the thing in my hand.

So I press the right arrow.

The old card closes and the next one opens with the page-turn sound: `THE DEVIL`, `The Town of Devil Guard`. I read it as a mountain-locked Sosarian town, once accessible through a magical gate, later opened by a cavern tunnel after Exodus. The text also ties the valley to a castle that crashed from the sky and to people trying to guard others from ancient daemons. That is useful player knowledge, but it is still just text on a visible card.

The controls are the real mechanical truth. Page 2 has a back arrow to Britain, a forward arrow to Grey, and an OK button that would draw Devil Guard. I do not press OK. `EnterLand(2)` does not run, so I am not moved to `Point3D(1617,1502,2)`, I do not gain the Sosaria discovery flag, and my account still has no Lodoria or Savaged Empire access.

Mechanical friction learned:

- The page-1 right arrow is a normal exposed reply, not a hidden helper call. It sends button `2` through `GypsyTarotGump.OnResponse`.
- A positive non-OK tarot reply closes the current tarot/welcome/race-potion gumps, sends a new `GypsyTarotGump` for that page, and plays the page-turn sound. It does not travel.
- Page 2 is `THE DEVIL`, destination line `The Town of Devil Guard`, card graphic `1105`, previous arrow reply `1`, next arrow reply `3`, and OK reply `102`.
- The Devil Guard travel path is still gated behind the visible OK button. Until that button is pressed, the character remains in the Forest start flow with no tarot selection.
- The hidden page gates are unchanged. Lodoria pages still require an account character with `the Land of Lodoria`; Savage still requires `the Savaged Empire`; Alien remains visible later only because the server setting allows it.

Next pressure:

The second fate is now open and read. The honest next move is to draw Devil Guard with OK, backtrack to Britain, page forward to the Hermit/Grey card, close the deck, or finally give in to the potion shelf or visitor journal before choosing a life.

## Run 10 - Grey Is Still a Card

I am still on the table tile at `Point3D(3567,3404,0)`, still in the Forest, with the Devil Guard card already read. I do the client-box scan before touching anything: the gypsy is still just north of me, the potion shelf is still within arm's reach, the visitor journal is still on the table, Thuvia is behind me, and the same forest sound items sit around the camp. None of them moved me, named me, or changed my race while the tarot gump was open.

The visible right arrow on `THE DEVIL` is the next clean deck action, so I press it.

The old card closes and page 3 opens with the page-turn sound: `THE HERMIT`, `The Village of Grey`. I read the visible text as a Sosarian village tied to old Exodus clues, rumors of star-flying ships, solitude, forest-floor ore, and a cemetery secret whispered about by necromancers. That is screen knowledge, not travel. My character is still sitting in the start camp.

The controls keep me honest. Page 3 has a back arrow to Devil Guard, a forward arrow to Montor, and an OK button that would draw Grey. I do not press OK. `EnterLand(3)` does not run, so I am not moved to `Point3D(851,2062,1)`, I do not gain `the Land of Sosaria`, and Lodoria/Savage remain locked behind their account discovery gates.

Mechanical friction learned:

- The page-2 right arrow is a normal visible reply `3`. `GypsyTarotGump.OnResponse` treats it as paging, not fate selection.
- The page-3 card is `THE HERMIT`, destination line `The Village of Grey`, card graphic `1110`, previous arrow reply `2`, next arrow reply `4`, and OK reply `103`.
- The Grey travel path is one visible OK press away, but only the OK reply would call `EnterLand(3)`.
- For a human page-3 OK, `EnterLand` would set the destination to `Point3D(851,2062,1)` on `Map.Sosaria`, then mark `the Land of Sosaria` discovered. Reading or paging to the card does neither.
- Nearby shelf/journal/NPC options remain visible but unused. The player is still pre-tarot, still human `RaceID=0`, and still has no Lodoria or Savaged Empire discovery.

Next pressure:

The third fate is open and read. The honest next move is to draw Grey with OK, go back to Devil Guard, page forward to Montor, close the deck, or finally use the nearby shelf or visitor journal before choosing.

## Run 11 - Montor Is Still a Card

I am still on the table tile at `Point3D(3567,3404,0)`, still pre-tarot, with `THE HERMIT` already read. The client box is unchanged in the way that matters: the gypsy is north of me, the potion shelf is within reach, the visitor journal is still on the table, and Thuvia is behind me. The deck is open, and the visible right arrow is the clean next action.

So I press the right arrow.

The Grey card closes and page 4 opens with the page-turn sound: `THE TOWER`, `The City of Montor`. I read it as a busy Sosarian city with shops, ship trade, Four Cards and Ambrosia rumors, a small eastern mine, and a northeastern tower rumor. That is all screen knowledge. My body is still in the Forest.

Mechanical friction learned:

- The page-3 right arrow is visible reply `4`, so this is normal gump flow, not a forged helper call.
- Page 4 is `THE TOWER`, destination line `The City of Montor`, card graphic `1122`, previous arrow reply `3`, next arrow reply `5`, and OK reply `104`.
- `EnterLand(4)` is still gated behind the OK button. If pressed by this human character, it would move to `Point3D(3220,2606,1)` on `Map.Sosaria` and mark `the Land of Sosaria` discovered.
- I do not press OK, so no travel, discovery flag, inventory change, race change, or PvP/PvE mutation happens.

Next pressure:

Montor is open and read. I can draw it, go back to Grey, page forward to Moon, close the deck, or abandon the cards long enough to use the nearby shelf or visitor journal.

## Run 12 - Moon Is Still Just Ink

I start with `THE TOWER` already read and no fate chosen. Before touching the next arrow, I look around the same 18-tile box: ShardGreeter, Thuvia, the potion shelf, visitor journal, gypsy help tile, and forest sound items are still the visible traced camp. The shelf and journal are still tempting, but the open card is already read and its right arrow is visible.

So I press the right arrow.

The Montor card closes and page 5 opens with the page-turn sound: `THE MAGICIAN`, `The Town of Moon`. The text makes Moon sound like a calmer coastal and farming town built over the ashes of corrupt mages, with Erstam and Serpent Island rumors left behind and desert travelers talking about riches from the Ancient Pyramid. Useful, but still just ink on the card.

Mechanical friction learned:

- The page-4 right arrow is visible reply `5`. `GypsyTarotGump.OnResponse` treats positive replies under `100` as paging, closes the old tarot/welcome/race-potion gumps, sends the requested page, and plays a page-turn sound.
- Page 5 is `THE MAGICIAN`, destination line `The Town of Moon`, card graphic `1116`, previous arrow reply `4`, next arrow reply `6`, and OK reply `105`.
- `EnterLand(5)` would only run if I press OK. For this human character, that would move to `Point3D(806,710,5)` on `Map.Sosaria` and set `the Land of Sosaria` discovery flag.
- I do not press OK. Moon is visible and read, but not selected. Lodoria and Savage are still hidden by account discovery gates.

Next pressure:

The fifth fate is open and read. The honest next move is to draw Moon with OK, page back to Montor, page forward to Mountain Crest, close the deck, or finally step away to the potion shelf or visitor journal before choosing.

## Run 13 - Mountain Crest Is Snow, Not Commitment

I start with `THE MAGICIAN` already read and no fate chosen. I check the same client box before touching the card: the gypsy is still north of the table, Thuvia is still behind me, and the potion shelf, visitor journal, help tile, and forest sounds are still the traced things around the camp. They matter, but the open gump has a visible right arrow and I have already read Moon.

So I press the right arrow.

Moon closes and page 6 opens with the page-turn sound: `THE FOOL`, `The Town of Mountain Crest`. I read it as a harder Sosarian start on small winter islands, with other settlements nearby, mountain caverns and dungeons, and an old wizard tower. That sounds dangerous enough to make me hesitate, but it is still only a visible card. My body is still on the Forest table tile.

Mechanical friction learned:

- The page-5 right arrow is visible reply `6`. `GypsyTarotGump.OnResponse` treats it as paging because it is positive and under `100`.
- Page 6 is `THE FOOL`, destination line `The Town of Mountain Crest`, card graphic `1108`, previous arrow reply `5`, next arrow reply `7`, and OK reply `106`.
- `EnterLand(6)` would only run from the visible OK button. For this human character, that would move to `Point3D(4546,1267,2)` on `Map.Sosaria` and set `the Land of Sosaria`.
- I do not press OK. Mountain Crest is visible and read, but not selected. Moon was also only read before I paged away.
- Lodoria and Savage remain hidden by the same account-discovery gates; page 6 does not unlock or skip them.

Next pressure:

The sixth fate is open and read. I can draw Mountain Crest, page back to Moon, page forward to Umbra, close the deck, or finally use the shelf or visitor journal before committing.

## Run 14 - Umbra Is a Warning, Not a Teleport

I start with `THE FOOL` already read and no fate chosen. Before touching anything, I run the same screen-check: I am still at `Point3D(3567,3404,0)` in the Forest, with the gypsy north of me, Thuvia behind me, the potion shelf close at hand, the visitor journal on the table, the gypsy help tile under the camp path, and forest sound items around the tent. None of those entities has moved me or changed me. The open card is already read, and its right arrow is visible.

So I press the right arrow.

Mountain Crest closes and page 7 opens with the page-turn sound: `DEATH`, `The Undercity of Umbra`. I read it as a necromancer sanctuary hidden in mountains southeast of Britain, with shops underground, a huge cavern, a death knight tomb nearby, and the Fires of Hell close enough to hike to. That is the first card that feels like it is looking back at me, but it is still only text on the gump. My character is still sitting at the gypsy table in the start forest.

Mechanical friction learned:

- The page-6 right arrow is visible reply `7`. `GypsyTarotGump.OnResponse` treats it as paging because it is positive and under `100`.
- Page 7 is `DEATH`, destination line `The Undercity of Umbra`, card graphic `1104`, previous arrow reply `6`, next arrow reply `8`, and OK reply `107`.
- `EnterLand(7)` would only run from the visible OK button. For this human character, that path recolors clothing through `MorphingTime.ColorOnlyClothes(m, 0, 1)`, moves to `Point3D(2666,3325,0)` on `Map.Sosaria`, and then sets `the Land of Sosaria`.
- I do not press OK. Umbra is visible and read, but not selected. Mountain Crest was only read before I paged away.
- Lodoria pages and Savage remain locked by account discovery gates. Page 7 does not expose, unlock, or skip those hidden routes.

Next pressure:

The seventh fate is open and read. I can draw Umbra, page back to Mountain Crest, page forward to Yew, close the deck, or finally step away to the potion shelf or visitor journal before committing.

## Run 15 - Yew Is Shade, Not Shelter

I start with `DEATH` already read and no fate chosen. The screen around me has not earned any new secrets: I am still at `Point3D(3567,3404,0)` in the Forest, with the gypsy north of the table, Thuvia behind me, the potion shelf and visitor journal still close enough to touch, the gypsy help tile under the camp path, and forest sound items around the tent. The open card has already been read, and its visible right arrow points onward.

So I press the right arrow.

Umbra closes and page 8 opens with the page-turn sound: `THE SUN`, `The Village of Yew`. I read it as a Sosarian forest-valley start west of Britain and east of Moon, with the largest trees, a wood trade hub, old Great Earth Serpent history, a nearby mining cave, and something on the south side of the mountains that miners avoid. That sounds useful, but it is not safety and it is not travel. My body is still at the gypsy table.

Mechanical friction learned:

- The page-7 right arrow is visible reply `8`. `GypsyTarotGump.OnResponse` treats it as paging because it is positive and under `100`.
- Page 8 is `THE SUN`, destination line `The Village of Yew`, card graphic `1120`, previous arrow reply `7`, next arrow reply `9`, and OK reply `108`.
- `EnterLand(8)` would only run from the visible OK button. For this human character, that would move to `Point3D(2460,893,7)` on `Map.Sosaria` and set `the Land of Sosaria`.
- I do not press OK. Yew is visible and read, but not selected. Umbra was only read before I paged away.
- Lodoria pages and Savage remain locked by account discovery gates. Page 8 does not expose or unlock them; it merely sits before the visible fugitive start on page 9.

Next pressure:

The eighth fate is open and read. I can draw Yew, page back to Umbra, page forward to the Britain Dungeons fugitive card, close the deck, or finally use the nearby shelf or visitor journal before committing.

## Run 16 - Fugitive Is a Sentence, Not a Selection

I start with `THE SUN` already read and no fate chosen. The table tile is still `Point3D(3567,3404,0)` in the Forest. The gypsy is north of me, Thuvia is behind me, the potion shelf and visitor journal are still close enough to touch, and the help tile and forest sound items are still the traced camp clutter. None of that outranks the open tarot gump; Yew is read, and its visible right arrow is the next exposed deck control.

So I press the right arrow.

Yew closes and page 9 opens with the page-turn sound: `THE HANGED MAN`, `The Britain Dungeons`. I read it as the fugitive start. It is not just a harder town card. The text says I would start wanted for murder, locked out of civilized areas except certain public places like inns, taverns, and banks, attacked by guards on sight, chased away by merchants, barred from most local guilds, charged double resurrection tribute, and given a larger skill ceiling because survival is self-reliant. It also says the first job is escaping a prison cell and that Stonewall lies northwest.

This is still only ink until I press OK. The card has a back arrow to Yew, an OK button for the fugitive start, and a forward arrow that jumps to the alien crash card. Lodoria and Savage did not become visible in between; the page calculation skips pages 10 and 11 because no account character has discovered Lodoria, and skips page 12 because no account character has discovered the Savaged Empire.

Mechanical friction learned:

- The page-8 right arrow is visible reply `9`. `GypsyTarotGump.OnResponse` treats it as paging because it is positive and under `100`.
- Page 9 is `THE HANGED MAN`, destination line `The Britain Dungeons`, card graphic `1109`, previous arrow reply `8`, next arrow reply `13`, scrollbar enabled, and OK reply `109`.
- `SkillGypsy("fugitive")` reports `13` grandmaster skills before any configured boost, while the normal default reports `10`; that text is visible on the card, but it does not mutate the character.
- `EnterLand(9)` would only run from the visible OK button. For this human character, that would move to `Point3D(4104,3232,0)` on `Map.Sosaria` and set `the Land of Sosaria`.
- I do not press OK. I am not wanted, not moved, not a fugitive, and not given the fugitive skill cap. Yew was only read before I paged away.
- Lodoria pages and Savage remain locked by account discovery gates. The next visible forward page is the alien crash site only because `AllowAlienChoice()` returns true.

Next pressure:

The ninth fate is open and read. I can draw the fugitive start, page back to Yew, page forward to the alien crash card, close the deck, or abandon fate long enough to use the potion shelf or visitor journal.

## Run 17 - The Star Is the Last Visible Card, Not a Crash

I start with `THE HANGED MAN` already read and no fate chosen. The client box is the same cramped camp: the gypsy is still north of the table, Thuvia is still behind me, the potion shelf is still close enough to use, the visitor journal is still on the table, and the help and forest sound items have not become destinations just because I can see them. The open gump is already read, and its visible right arrow is the clean chronological action.

So I press the right arrow.

The fugitive card closes and page 13 opens with the page-turn sound. The missing pages matter more than the drama: page 10 and page 11 are still hidden because no account character has discovered Lodoria, and page 12 is still hidden because no account character has discovered the Savaged Empire. The card I actually get is `THE STAR`, `The Shuttle Crash Site`, because alien choice is enabled.

I read it as the alien start. It promises no memory, no starting skills, no Sosarian gold, a 40-skill grandmaster ceiling, ugly resurrection penalties, no luck benefit, expensive guilds, a crash-site terminal for appearance changes, and a survival start with a knife, alien meat, and an empty canteen. That is a lot of pressure, but it is still only text while the OK button sits untouched.

The code behind the OK button is sharper than the card text. Pressing OK would call `EnterLand(13)`, move me to `Point3D(4109,3775,2)` in `Map.Sosaria`, set `the Land of Sosaria`, and drop me inside the CrashRegion. Then `CrashRegion.OnEnter` would call `SetSpaceMan` because my `SkillStart` is not already 40000. That is the path that would wipe my backpack, zero my skills, set the alien skill ceiling, dress me, give me the named knife, alien meat, empty canteen, and medical record, then leave me at the crash site. I do not press OK, so none of that happens.

Mechanical friction learned:

- The page-9 right arrow is visible reply `13`; `GypsyTarotGump.OnResponse` treats it as paging, not fate selection.
- Page 13 is `THE STAR`, destination line `The Shuttle Crash Site`, card graphic `1118`, previous arrow reply `9`, no next arrow, scrollbar enabled, and OK reply `113`.
- The alien card is available only because `AllowAlienChoice()` currently returns true. If that setting were false, pageShow would skip it for this human.
- The real alien mutation path is not just card text: OK leads to `EnterLand(13)`, then CrashRegion entry leads to `SetSpaceMan`.
- I do not press OK. I am still human, still in the Forest, still carrying my starter gear and gold, still at 50 Healing and 50 Swords, and still have no Sosaria, Lodoria, Savage, or alien-start commitment.

Next pressure:

The last visible fate is open and read. I can draw the alien crash start, page back to the fugitive start, close the deck, or finally step away from the cards to use the potion shelf or visitor journal before fate is chosen.

## Run 18 - The Star Takes Everything

I start with `THE STAR` open and already read. The potion shelf and visitor journal are still close enough to matter in the old camp, but the open gump is the screen. The OK button is visible, the left arrow is visible, and there is no right arrow. I stop circling the deck and press OK.

This time it is not ink. The tarot gump closes, the code takes reply `113`, subtracts `100`, and calls `EnterLand(13)`. My body leaves the gypsy table for `Point3D(4109,3775,2)` in `Map.Sosaria`, inside `the Crash Site`. The Land of Sosaria flag gets written before the region finishes with me. Then the crash region catches that my skill start is still the ordinary `10000` and runs the alien initializer.

The initializer is brutal in a way the card only hinted at. It wipes my backpack, strips the starter clothes, zeroes every skill base, sets the skill cap to `40000`, dresses me in a shirt, boots, and long pants, puts a renamed dagger, `knife`, in my hand, and leaves only three known pack items: ten pieces of `cooked alien meat`, an `empty canteen`, and a `Medical Record` with my name on it. My gold is gone. Healing and Swords are gone. I am still mechanically human by `RaceID`, but the build has marked my life as the alien start.

The new client-range scan is not the forest camp anymore. I see a `computer terminal` at `4108,3779,0` and a matching terminal light above it. It is within double-click range and the code says it opens a damaged computer gump for appearance colors, but I do not touch it yet. I do not invent a monster; the static spawn files did not put a deterministic mobile inside the 18-tile box. The next shiny thing is the terminal, not a fight.

Mechanical friction learned:

- Page 13 was a legal choice only because the page was visible and `AllowAlienChoice()` is true; Lodoria and Savage were still skipped by account discovery gates.
- `EnterLand(13)` does not directly wipe me. It moves me into `CrashRegion`; `CrashRegion.OnEnter` calls `PlayerSettings.SetSpaceMan` because `SkillStart != 40000`.
- `SetSpaceMan` is the destructive alien-start gate: it deletes pack contents, removes clothes, zeroes skills, sets `SkillBegin("alien")`, equips the minimal clothing and knife, and adds alien meat, empty canteen, and Medical Record.
- The crash site blocks player-vs-player harmful action and spell casting, has zero logout delay, and immediately tells me I am near a crashed shuttle.
- The crash-site terminal is visible and usable from the arrival tile, but it is still a separate double-click action. The next run should not pretend I have read its gump.

Next pressure:

I am broke, skillless, and standing by the terminal with a medical record in my pack. The honest next move is to double-click the terminal, read the Medical Record, or scan a step away from the shuttle before hunger and distance start mattering.

## Run 19 - The Terminal Is a Mirror With Teeth

I start at `Point3D(4109,3775,2)` in `the Crash Site`, with no tarot window left and no forest camp in sight. Before I touch anything, I do the screen check again. The only deterministic thing close enough to matter is the `computer terminal` at `4108,3779,0`, with a matching glow above it. No traced mobile is standing inside the normal client box. My pack still has alien meat, an empty canteen, and the `Medical Record`; the record is interesting, but the terminal is the visible machine four tiles away.

So I double-click the terminal.

The range gate passes. The terminal does not move me, feed me, teach me a skill, or unlock a world. It closes any old terminal window, opens `ComputerDatabaseGump`, and plays the machine sound. I read the text before touching the swatches. The computer says most of its data is ruined, but it can still see a fragment of my medical record. It points back at the Medical Record in my pack for where I came from, then admits that the record does not say what people from that planet look like. The rest of the screen is a skin-color column and a hair-color grid. Closing the terminal keeps my current look. The first skin entry and first hair entry are not safe cancel buttons in the ordinary sense; they are random human color choices.

I do not press a swatch. That matters because the swatches are not a preview. A reply from `1` through `8` writes skin hue, and a reply from `9` through `32` writes hair and facial-hair hue, records the feature choice, reopens the gump, and plays a different sound. Since I only opened and read the terminal, my unresolved appearance stays unresolved, my pack stays the same, and the Medical Record remains unread.

Mechanical friction learned:

- The crash-site terminal is a normal visible item interaction: `ComputerDatabase.OnDoubleClick` only opens its gump if the player is within range `4`.
- Opening the terminal gump is informational plus cosmetic setup. The mutation lives in `ComputerDatabaseGump.OnResponse`, not in the double-click.
- There is no typed name, planet, or character-history field in this terminal. It only offers skin and hair color replies.
- The terminal text makes the Medical Record more tempting, but reading that record is a separate backpack double-click.
- No new tarot, Lodoria, Savage, quest, skill, hunger, thirst, inventory, or movement state changed during this action.

Next pressure:

The terminal window is open and read. The honest next move is to close it unchanged, press one visible color swatch and accept the immediate appearance change, or leave the terminal alone long enough to read the Medical Record.

## Run 20 - The Swatch Is Not a Preview

I start with the damaged computer still open. The outside scan has not become more heroic while I stare at it: the `computer terminal` at `4108,3779,0` and its matching glow are still the only deterministic things in the 18-tile box, and the static spawn files still do not put a mobile beside me. The Medical Record is in my pack, but the visible thing asking for a decision is the terminal gump.

I avoid the first skin entry because the text makes it a random human color, not a harmless cancel. Instead I press the second visible skin swatch.

That click is immediate. There is no preview, no confirmation, and no undo prompt. Reply `2` maps to skin hue `0x6F6`; `ComputerDatabaseGump.OnResponse` writes it to both my live `Hue` and `RecordSkinColor`, reopens the same terminal gump, plays the different machine sound, and calls `RecordFeatures(true)`. That last call quietly records whatever my current hair and beard colors are, but I still do not know those hues because I never pressed a hair swatch and the original creation colors were unresolved.

The important part is the shape of the trap: the terminal's color buttons are real mutations, not selectors waiting for OK. I still have no skills, no gold, no water, and no new world route. The Medical Record is still unread. The gump is still on the screen, now after a skin-change refresh.

Mechanical friction learned:

- The 18-tile scan around `Point3D(4109,3775,2)` still finds the decorated terminal and glow, not a deterministic nearby NPC.
- `ComputerDatabaseGump` skin reply `1` is random; reply `2` is the first deterministic skin swatch and sets hue `0x6F6`.
- A swatch reply is not a preview. The response immediately mutates the character, sends a fresh terminal gump, and plays sound `0x54B`.
- `RecordFeatures(true)` means even a skin-only click also stores the current hair and beard colors in the record fields; those values remain unresolved because they came from unresolved current appearance.
- No Medical Record, hair swatch, movement, hunger, thirst, skill, quest, tarot, Lodoria, or Savage state changed.

Next pressure:

The terminal is still open, but I have learned enough about its teeth. The honest next move is to close it and read the Medical Record, or deliberately press a visible hair swatch and accept that it will also be immediate.

## Run 21 - The Record Gives Me a Name, Not a Rescue

I start with the damaged computer still open after the skin swatch. The terminal is still muttering about color choices, but I have already learned that its buttons bite immediately. The only deterministic world object in the 18-tile crash-site scan is still the terminal and its glow. The more useful thing now is the `Medical Record` in my backpack, because the terminal itself told me that is where my origin fragment lives.

So I leave the terminal alone and double-click the `Medical Record`.

The record opens its own readable gump. It does not close the terminal, move me, feed me, fill the canteen, teach a skill, or unlock another tarot page. It is just a lighted tech item in my pack sending a medical file to the screen and playing the same machine sound. The header says it is a medical record for `Mira Vale`, by Dr. Thomas Witman. The body says I was the only survivor found alive in a dead derelict shuttle, unconscious and badly injured; security thought the ship had been attacked by the Kilrathi; the file says I came from the planet `Alaunrae V`; then the station lost orbit around a primitive world and put my stasis chamber into the last medical shuttle.

That is not a quest flag. It is a problem statement. I know I am a crashed patient with a generated planet name, not someone who can now claim Lodoria, Savage, water, skills, or shelter. The terminal is still open behind the record, and the crash site is still empty in the static scan.

Mechanical friction learned:

- A backpack item is still a normal player interaction. `Mobile.Use` checks update range, visibility, accessibility, alive state, region permission, and item-use hooks before `MedicalRecord.OnDoubleClick` runs.
- `MedicalRecord.OnDoubleClick` only closes any previous `MedicalRecordGump`, sends a new one, and plays sound `0x54D`; it does not close `ComputerDatabaseGump`.
- `MedicalRecordGump` is closable, disposable, draggable, not resizable, and has no reply buttons or text-entry fields. Reading it exposes text, not a mechanical choice.
- The visible patient line comes from `DataPatient`, which `SetSpaceMan` set to my character name. The visible planet line comes from `DataPlanet`, generated by `SetupMedicalRecord`; this simulation branch is `Alaunrae V`.
- No location, hunger, thirst, inventory count, skill, stat, quest, tarot, Lodoria, or Savage state changed.

Next pressure:

The foreground record is read, the terminal is still open behind it, and I am still skillless with an empty canteen. The honest next move is to close one of these windows, choose a hair swatch if I really care about appearance, eat alien meat, inspect the empty canteen, or take a careful first step away from the shuttle.

## Run 22 - Closing a Record Is Not Closure

I start with the `Medical Record` gump in front and the damaged terminal still open behind it. The record is already read. It has no buttons, no text entry, and no hidden prompt asking me to accept a quest. The world scan behind the paper has not changed: the crash-site terminal and its glow are still the only deterministic things in my client box, and no traced mobile is standing nearby.

So I click the normal close control on the Medical Record.

The important part is how little happens. The gump response packet removes the responding gump and calls that gump's `OnResponse`. The record's response handler only plays sound `0x54D`. It does not close the terminal, fill the canteen, teach a skill, move me, set a quest, make Alaunrae V a travel route, or unlock Lodoria or Savage. The terminal is now the only traced open gump, still read, still showing skin and hair swatches that mutate immediately if I press them.

Mechanical friction learned:

- `MedicalRecordGump` is closable, disposable, draggable, and not resizable; it displays text only and has no reply buttons or text fields.
- A normal close response removes only the gump that answered. It does not sweep unrelated gumps such as the already-open `ComputerDatabaseGump`.
- `MedicalRecordGump.OnResponse` ignores the button id and only plays sound `0x54D`.
- Closing the read record grants no rescue, quest state, discovery flag, travel route, item, food, water, appearance change, skill, stat, or location change.

Next pressure:

The terminal is still on the screen, and its visible swatches still bite. I can close it unchanged, choose a hair color on purpose, or stop fussing with my face long enough to eat, inspect the empty canteen, or step away from the shuttle.

## Run 23 - Close the Mirror, Keep the Scar

I start with the damaged computer still open and already read. There is no new text to study, no OK button, and no promise that another swatch will wait for confirmation. The outside scan is just as bare as before: the decorated `computer terminal` at `4108,3779,0`, its light at `4108,3779,5`, and no deterministic mobile inside the 18-tile box. The terminal is visible, but the terminal window is the thing blocking the screen.

So I click the normal close control instead of pressing a hair swatch.

This is the first tech action here that does almost nothing on purpose. The packet response removes the `ComputerDatabaseGump`, then calls its `OnResponse` with no color button selected. Since button `0` does not map to any skin or hair color, both color variables stay at zero and the handler falls through to the sound-only branch. It does not reopen the terminal, does not record features again, does not change my skin away from `0x6F6`, does not touch my unresolved hair or beard colors, and does not put the record, canteen, food, skills, location, quests, Lodoria, or Savage on a different track.

The screen is finally clear. That makes the next pressure less cosmetic and more survival-shaped: I am still beside the shuttle, still carrying ten pieces of alien meat, still holding an empty canteen, still broke, and still skillless. The terminal and its glow remain in view if I regret the hair color later.

Mechanical friction learned:

- `ComputerDatabaseGump` is closable, disposable, draggable, and not resizable, so the normal close control is a visible player action.
- The gump response packet removes the responding gump before `ComputerDatabaseGump.OnResponse` runs.
- Only reply buttons `1-8` set skin color, and only replies `9-32` set hair and facial-hair color. A normal close is button `0`, so it reaches the sound-only branch.
- Closing the terminal does not confirm or cancel a pending preview; there is no pending preview. The only appearance mutation already committed was the earlier skin swatch.
- The 18-tile crash-site scan still finds the terminal and terminal light, not a deterministic NPC, chest, corpse, or nearby spawn.

Next pressure:

The gumps are gone. I can now eat alien meat, inspect the empty canteen, double-click the terminal again, or take a careful first step away from the shuttle.

## Run 24 - Too Full to Panic-Eat

I start with a clear screen at `Point3D(4109,3775,2)` in `the Crash Site`. I scan before I rummage. The same `computer terminal` is still at `4108,3779,0`, with its glow above it, and the static files still do not put a deterministic mobile in my 18-tile box. No chest, corpse, vendor, or rescuer appears just because I closed the computer. The pack is the interesting thing now: ten pieces of `cooked alien meat`, an `empty canteen`, and the Medical Record I already read.

So I try the obvious survival reflex and double-click the alien meat.

The meat does not get eaten. It is not poisoned, cooked wrong, or locked behind a skill. I am simply already full. `Mobile.Use` lets the backpack item reach its normal double-click path, `CookedBird` is just a `Food` with `FillFactor = 10`, and `Food.OnDoubleClick` gets as far as `Eat`. Then `FillHunger` sees my `Hunger` is already `20`, sends the "too full" message, and returns `false`. Because that happens before `Consume()`, the stack stays at ten. No stamina tick, healing roll, mana bump, poison, skill use, thirst change, world unlock, Lodoria flag, Savage flag, or movement follows.

That matters more than it sounds. The alien start did give me food, but the current friction is timing: I cannot bank a bite while my hunger meter is capped. The food is a future resource, not an immediate buff button. I am still broke, skillless, beside a dead shuttle, and holding an empty canteen.

Mechanical friction learned:

- A backpack item can be used through the normal `Mobile.Use` gate when it is in update range, visible, accessible, alive-usable, item-use-allowed, and allowed by the current region.
- Food in a backpack resolves its world location through its root parent, so the range check is satisfied from my own tile.
- `CookedBird(10)` is not a special alien-consumption system. The alien start only renames and recolors it; the eating rules come from `Food`.
- Full hunger blocks every later food side effect. `Food.FillHunger` returns `false` at `Hunger >= 20`, so `Food.Eat` never consumes an item from the stack.
- The crash-site scan is still bare except for the terminal and glow. The meat attempt did not open a gump or create a new visible lead.

Next pressure:

The food is still intact, but it cannot help while I am stuffed. The honest next move is to inspect the empty canteen, reopen the terminal if I want more appearance changes, or take a careful first step away from the shuttle.

## Run 25 - The Canteen Is Just an Empty Shape

I start with the screen clear at `Point3D(4109,3775,2)` in `the Crash Site`. I do the same stubborn look-around before touching the pack. The terminal at `4108,3779,0` and its glow at `4108,3779,5` are still the only deterministic decorated things in the client box. No mobile, chest, corpse, trough, tub, barrel, or obvious water source shows up in the searched spawn and decoration files. The meat already refused me because I am full, so the other survival object gets its turn.

So I double-click the `empty canteen`.

The canteen is not secretly full. It is a `Waterskin` that the alien start renamed and gave item ID `0x4971`. The double-click passes the normal backpack item-use gates, then `Waterskin.OnDoubleClick` asks `DrinkingFunctions.CheckWater` whether I am within three tiles of a fill source. I have no traced trough, tub, barrel, or decorated water in range. With no fill source and an empty-canteen item ID, the code does not drink, does not refill, and does not open a gump. It just tells me, `You can only fill this at a water trough, tub, or barrel!`

That message is useful because it turns the empty canteen from a comforting icon into a job. Thirst is still capped at `20`, so I did not need water this second, but the container is dead weight until I find an actual fill source. I still have ten alien meat, the read Medical Record, a knife, no gold, no skills, no Lodoria or Savage unlock, and no deterministic company at the crash site.

Mechanical friction learned:

- `SetSpaceMan` creates the crash-site canteen by constructing a `Waterskin`, setting `ItemID = 0x4971`, naming it `empty canteen`, and adding it to the backpack.
- A canteen double-click is still a normal item-use action through `Mobile.Use`, not a helper call.
- `Waterskin.OnDoubleClick` calls `CheckWater(from, 3, out soaked)` before deciding whether an underfull container fills.
- Empty item IDs `0xA21` and `0x4971` hit the "totally empty" branch if no water source is found.
- The empty branch sends the fill-source message and changes no hunger, thirst, weight, item count, skills, quests, discovery flags, location, or open gumps.
- The code can also inspect static map tiles for water, but the text sources I can trace do not expose a visible fill source at the crash site. I cannot assume a hidden water source paid out.

Next pressure:

The survival loop is now blunt: food is saved for later because I am full, and the canteen needs a real water source. The honest next move is to walk carefully away from the shuttle and rescan, or reopen the terminal only if I decide appearance matters more than finding water.

## Run 26 - The Cave Has an Exit Line

I start at `Point3D(4109,3775,2)` with the canteen still empty, no open gumps, no visible rescuer, and the already-read terminal behind me. The canteen's message did the one useful thing it could: it made standing still feel stupid. I pick an ordinary direction instead of pretending I know the map, walking east along the crash site and stopping at simulated `Point3D(4120,3775,2)`.

The walk is still inside `the Crash Site`. I do not step onto a special tile, I do not get yanked to another map, and `SetSpaceMan` does not run again. The scan from the new spot finally changes the mechanical trace. The old terminal and its glow are still visible behind me, but now they are too far away to use without walking back. Ahead, the cave mouth reads like terrain continuing out of the crash cave. The repository data places a row of invisible `Teleporter` tiles at `4138,3769` through `4138,3773`, all pointing to `PointDest=(1075,1312,2)`, but the player-facing lure is the cave exit shape, not a shiny portal.

No trough, tub, barrel, corpse, chest, vendor, or deterministic mobile appears in the refreshed scan. I am still full enough not to eat, still carrying ten alien meat, still holding an empty canteen, and still at zero skills. The new pressure is ordinary: the cave has an exit-looking east edge, and if I keep walking that way the invisible row will probably move me outside.

Mechanical friction learned:

- Movement goes through the normal `Mobile.Move` path: spell/frozen/paralyzed checks, `CheckMovement`, move-over blockers, and `Region.CanMove` before the step is accepted.
- `the Crash Site` is a `CrashRegion` rectangle from `x=4086,y=3745,width=60,height=60`; `Point3D(4120,3775,2)` stays inside it, so this scouting move does not re-enter the crash region or rerun the alien initializer.
- The teleporter row is not flavor. The decorated tiles at `4138,3769` through `4138,3773` are invisible `Teleporter` items with `PointDest=(1075,1312,2)`, `Creatures=False`, no effects, no sound, and no delay.
- Tracing those tiles is not using them. `Teleporter.OnMoveOver` only starts teleporting when I actually move over a tile; this run stops short.
- The refreshed static scan still finds no deterministic mobile or decorated water source. Random encounter and live spawner state remain unresolved, not assumed safe.

Next pressure:

The eastern teleporter row is now a traced cave-exit mechanic, not a chosen destination. The next honest move is to keep walking through the exit-looking terrain and only learn the row's behavior if normal movement actually crosses it.

## Run 27 - The Exit Was Terrain, Not a Portal

I start at `Point3D(4120,3775,2)` with the canteen still useless, the terminal behind me, and no gump on the screen. I have to correct my own survival brain before moving: the static item scan caught the exit row, but the player does not see the teleporter item. `Teleporter` constructs itself with `Visible = false`. So I am not walking toward a shiny portal. I am walking east-northeast because the cave exit looks like the way out and the crash site has shown me no water, no person, no chest, and no other visible route.

That makes the teleport ordinary for a cave mouth, not random magic. I step onto the hidden tile at about `4138,3773,1`, and the exit transfers me outside. There is no warning text, no sound, no source effect, and no destination effect because the decorated row explicitly has those disabled. `Teleporter.OnMoveOver` fires, the item allows players even though `Creatures=False`, `StartTeleport` has zero delay, and `DoTeleport` uses my current map because no `MapDest` is set. I land at `Point3D(1075,1312,2)` on `Map.Sosaria`.

The new screen is not a camp or a rescue. `Regions.xml` has no named region rectangle containing that coordinate in the traced file, and the decoration scan finds no deterministic item in the 18-tile box. No NPC is mechanically standing on the landing tile. The terrain context matters more than the item list: `Map.Sosaria` is registered to file index `1`, so the outside scan must use `map1.mul`, `staidx1.mul`, and `statics1.mul`. That corrected scan puts me on grass at `z=2`, with impassable rock and mountain pressure close to the west and northwest/farther north, and grass/jungle wilderness opening to the east and south. Several world and animal spawner envelopes overlap the client range, so a live server could have animal or hostile wilderness spawns near the edge of vision, but the static files do not tell me which ones are actually materialized right now. No decorated trough, tub, barrel, or obvious fill source appears in the text sources. The canteen is still empty.

Mechanical friction learned:

- A static `Map.GetItemsInRange` style trace can find invisible mechanics a normal player cannot see. The crash-site exit row is mechanically present, but `Teleporter.Visible` defaults to `false`; the visible affordance is the cave mouth terrain, not the item.
- The normal player action was continued scouting through an exit-looking cave edge, not selecting a known destination. The destination was earned only by walking over the hidden item in-world.
- `Teleporter.OnMoveOver` gates creatures, combat, delay, effects, and sound. This row has `Creatures=False`, but Mira is a player, combat check is false, delay is zero, and effects/sound are disabled.
- With no `MapDest`, `DoTeleport` falls back to the mobile's current map, so the row moves from the CrashRegion to `Point3D(1075,1312,2)` on `Map.Sosaria`.
- The destination does not set a discovery flag, open a gump, change inventory, refill the canteen, consume food, or rerun `SetSpaceMan`.
- The destination scan has no deterministic visible item or mobile. It does have corrected `map1.mul` terrain and overlapping `animals.map` and `world.map` spawner ranges, so live danger is possible but unresolved.

Next pressure:

I am out of the crash rectangle, still skillless, still full, still holding an empty canteen, and now standing at the mountain/forest edge of unlabelled Sosarian wilderness with possible spawns but no deterministic help. The honest next move is to pause, scan the immediate screen for live threats/water, then pick a direction without pretending the static spawner list is an actual visible monster.

## Run 28 - The Map File Was the Trap

I start at `Point3D(1075,1312,2)` after the silent crash-site exit. This time the first job is not to drink. It is to audit the terrain scan that just claimed I was standing over water. That claim used `map0.mul`, but the current map is `Map.Sosaria`, and `MapDefinitions` registers Sosaria with file index `1`. The corrected binary scan uses `map1.mul`, `staidx1.mul`, and `statics1.mul`.

That changes the whole picture. The tile under me is not water. It is `0x4` grass at `z=2`, dry and passable. The immediate west tile, `1074,1312`, is impassable grass at `z=5`, while direct north, `1075,1311`, remains passable grass at `z=2`; the wider northwest and farther-north rows rise into impassable rock/grass. East and south are walkable grass, with jungle/forest-style statics farther into the 18-tile box. This matches a one-way cave exit: mountain behind me and to the west/northwest, wilderness ahead, no visible way back into the cave.

The client-range entity scan is still stingy. No deterministic mobile, chest, corpse, vendor, barrel, tub, trough, or decorated water item appears in the 18-tile box. The corrected `staidx1/statics1` scan does find ordinary wilderness/cave-edge statics in the wider screen, bucketed mostly east with a smaller north slice, but none of them use the water-target item IDs the canteen code accepts. The spawner files still only give me possible animals and hostile wilderness at live-runtime positions. The normal player pressure is now direction and exposure, not "drink from water underfoot."

So I do not double-click the canteen again. There is no visible water source to use, and the prior canteen lesson already stands: it needs a trough, tub, barrel, or static/item water target that `DrinkingFunctions.CheckWater` recognizes. My thirst is still `20`, the canteen is still empty, the stack of alien meat is still ten, and no discovery flag, quest, skill, stat, or gump changes. The actual next problem is getting oriented outside the cave.

Mechanical friction learned:

- `Map.Sosaria` must use file index `1`; using `map0.mul` for Sosaria terrain was the bad assumption that created the false water-underfoot run.
- The corrected post-exit destination in `map1.mul` is `0x4` grass at `z=2`, dry and passable, not `0xAB` water.
- The immediate west and wider northwest/farther-north terrain include impassable rock or impassable grass, while direct north, east, and south remain walkable grass with wilderness/jungle context in the wider static scan.
- The 18-tile entity scan still has no deterministic visible mobile or decorated item. Spawner ranges remain possible live danger, not confirmed visible creatures.
- `DrinkingFunctions.CheckWater` still matters later, but it was not re-tested here because the corrected screen has no visible water source and no recognized item/static water target.

Next pressure:

I need orientation, not shoreline. The mountain blocks the cave side, the forest/wilderness opens east and south, and the canteen remains a future problem until I find an actual trough/tub/barrel or recognized static water target. The next honest move is to choose a walkable direction while remembering that possible spawners are not visible creatures until the live server materializes them.

## Run 29 - Water on the Screen, Not in the Canteen

I start at `Point3D(1075,1312,2)`, outside the crash cave, with the mountain behind me and no open gumps. The screen is not empty after all. A corrected `Map.Sosaria` scan catches a little patch of water-looking statics at the east-northeast edge of the client range, around `1092-1093,1304-1306`. I still do not see a person, chest, corpse, trough, tub, barrel, vendor, or actual item I can single-click. But I am holding an empty canteen, so water-looking pixels are enough to pull me.

I walk east-northeast instead of using the canteen from too far away. The route from `1075,1312` to `1090,1306` stays on dry passable `map1.mul` grass and jungle tiles. No teleporter row fires, no region message appears, no gump opens, and no item is used. I stop at `Point3D(1090,1306,0)`, close enough that the water graphics are now within the canteen's three-tile search radius.

The ugly part is the ID list. The nearby statics are named `water`, but their IDs are `0x1797`, `0x179F`, `0x17A1`, `0x17A4`, `0x17A6`, and `0x17A8`. `DrinkingFunctions.CheckWaterTarget` does not include those IDs. I have not double-clicked the canteen yet, so the player-facing failure has not happened, but mechanically this looks like scenery water rather than a fill source. The canteen is still empty, thirst is still `20`, and the meat stack is still ten.

Mechanical friction learned:

- `Map.Sosaria` still resolves through file index `1`; this run used `map1.mul`, `staidx1.mul`, and `statics1.mul`.
- `Mobile.Move` does the normal movement work through `CheckMovement`, move-over blockers, and `Region.CanMove`; the walked route stayed on passable land tiles and did not hit a decorated trigger.
- The current 18-tile box has no deterministic mobile or decorated usable item. `animals.map` line 602 and `world.map` line 385 overlap the scan as possible live spawner pressure only.
- The visible water graphics are within three tiles now, but the canteen code checks exact item/static IDs. These water graphics are not in that allowlist.
- Nothing about walking toward the water graphics changes discovery flags, skills, stats, hunger, thirst, inventory, quests, Lodoria, Savage, or tarot state.

Next pressure:

The normal player move is still to try the empty canteen against the visible water, because it looks like water and it is right there. The code trace is already warning me that the shard may answer like a desert anyway.

## Run 30 - The Shoreline Lies

I start at `Point3D(1090,1306,0)` with the empty canteen in my pack and no gump open. The water-looking statics are close now: `1092-1093,1304-1306`, within the three-tile canteen check. The screen still has no deterministic NPC, chest, corpse, trough, tub, barrel, or vendor. The only obvious thing a normal thirsty player can do is double-click the canteen and hope the water pixels count.

They do not. `Waterskin.OnDoubleClick` runs the normal backpack item path and calls `DrinkingFunctions.CheckWater(from, 3, out soaked)`. That scan can use map items or static tiles, but only if their exact IDs pass `CheckWaterTarget`. The six visible water statics here are named `water`, yet their IDs are `0x1797`, `0x179F`, `0x17A1`, `0x17A4`, `0x17A6`, and `0x17A8`; none are in the allowlist. Since my canteen is still the totally empty `0x4971` item, the only result is the same blunt message: `You can only fill this at a water trough, tub, or barrel!`

So the shoreline is scenery, not survival. The canteen stays `empty canteen`, weight `1.0`, item ID `0x4971`; thirst stays `20`; the meat stack stays ten; no skills, stats, quests, discovery flags, tarot gates, Lodoria, or Savage access change. The possible animal/world spawners near the edge of the scan remain possible only, not visible enemies.

Mechanical friction learned:

- `Map.Sosaria` still resolves through file index `1`; the current scan remains `map1.mul`, `staidx1.mul`, and `statics1.mul`.
- `DrinkingFunctions.CheckWater` checks both nearby map items and nearby static tiles, but `CheckWaterTarget` is an exact item-ID allowlist.
- Visible static water art is not enough. If it is not one of the accepted IDs, an empty canteen only prints the trough/tub/barrel message.
- This action opens no gump, moves nowhere, and mutates no world/account progression.

Next pressure:

I need to stop arguing with the shoreline. The next honest move is to follow the jungle/water edge for something crafted or civilized enough to be a real fill source, while remembering that the overlapping spawners can become trouble on a live server.

## Run 31 - The Water Blocks Better Than It Drinks

I start at `Point3D(1090,1306,0)`, still holding the same empty canteen, with no gump open and no new message after the shoreline failed me. I do the client-box look before moving. The old water tiles are still there, but the important correction is physical now: they are not just bad water targets, they are also impassable statics. Walking straight east would put my face into the same water art that refused the canteen.

So I do the normal thing a player does when shoreline pixels get in the way. I walk around them, east-southeast through dry jungle, and stop at `Point3D(1108,1308,0)`. No prompt fires. No region text appears. No gump opens. I do not use the canteen again, because the three-tile fill scan from here has no water-named static and no `CheckWaterTarget` ID at all.

The new screen is denser but not kinder. The old water graphics are still behind me in the wider view, and they still are not real drinkable targets. The static decoration scan finds no trough, tub, barrel, chest, corpse, vendor, or fixed NPC. The spawner files are closer now: `animals.map` line 602 and `world.map` line 385 put invisible `PremiumSpawner` homes at `1124,1293`, inside the 18-tile box. That is not a visible creature. `PremiumSpawner` makes itself invisible, so all I can honestly feel is pressure: something may already be wandering nearby on a live server, but the static files do not let me see it.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; the scan used `map1.mul`, `staidx1.mul`, and `statics1.mul`.
- The direct route through `1092-1094,1306` is blocked by impassable static water graphics, so the normal movement path has to route around them.
- The route to `1108,1308,0` stays on dry passable jungle and uses ordinary `Mobile.Move`, `CheckMovement`, item/mobile move-over checks, and `Region.CanMove`.
- `Regions.xml` still has no named rectangle at the new coordinate.
- The current three-tile canteen scan has no accepted water target. The wider screen still has 27 named water statics, but none are in `DrinkingFunctions.CheckWaterTarget`.
- Nearby `.map` spawner homes are invisible source pressure, not visible NPCs or loot.

Next pressure:

The canteen is still empty and the shoreline has stopped being a promise. I am now deeper in jungle, with possible live spawns near the edge of sight and no deterministic help. The next honest move is to keep scanning before every step and look for something built, named, or moving.

## Run 32 - The Jungle Gives Me Room, Not Help

I start at `Point3D(1108,1308,0)` with no gump open. The screen has no fixed person, corpse, chest, barrel, tub, trough, vendor, or readable object. The old water graphics are still visible behind me in the wider client box, but they are already proven scenery: 27 named `water` statics, zero `CheckWaterTarget` hits, and no three-tile canteen target. The only new pressure is the invisible kind. `animals.map` line 602 and `world.map` line 385 put `PremiumSpawner` homes at `1124,1293`, but `PremiumSpawner` itself is invisible, so I do not get to treat those lines as a monster on the screen.

So I keep moving by what a player can see: dry jungle and gaps between impassable trees/water. The passable route steps southeast and east from `1108,1308` through ordinary `Mobile.Move` checks and stops at `Point3D(1122,1320,0)`. No region label appears, no prompt fires, no gump opens, no item is used, and no visible enemy is proven by the static files.

The new screen is quieter in one way and worse in another. The spawner homes that were inside the old 18-tile box are now outside this one, so the static source pressure drops, but that is not the same as safety on a live shard. The decoration scan still finds no deterministic item or fixed mobile. The binary static scan sees another little patch of water-looking art at `1118-1120,1331-1333`, but it is the same bad family of impassable non-target water IDs, too far south to solve the empty canteen and not a crafted fill source.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; this run used `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The walked route from `1108,1308,0` to `1122,1320,0` stays on dry passable jungle tiles and avoids impassable statics.
- Normal movement is still just `Mobile.Move` through `CheckMovement`, static/item/mobile blocking, and `Region.CanMove`; no special trigger or region-entry code ran.
- `Data/Decoration/Sosaria/decorate.cfg` has no deterministic decoration hit in the 18-tile box around the new point.
- The previous `animals.map`/`world.map` spawner homes at `1124,1293` are no longer inside the new 18-tile box. That reduces traced static pressure only; it does not prove the live server has no wandering spawn.
- The new visible water-looking statics are still not canteen targets and are not within three tiles anyway.

Next pressure:

I am not rescued. I am just farther from the false shoreline and the last traced spawner homes, with the same empty canteen, ten alien meat, zero skills, no gold, and no visible help. The next move stays boring on purpose: scan again, then look for something built, named, or moving before I spend another click.

## Run 33 - East Is Still Just Jungle

I start at `Point3D(1122,1320,0)`, no gump open, no fixed mobile on screen, and no deterministic item to click. The scan is not blank in a visual sense: there are rocks, ferns, mushrooms, leaves, snake plants, and water-looking art south of me. But none of that is a chest, corpse, NPC, vendor, trough, tub, barrel, readable object, or visible spawner creature. The old water lesson still holds too. The nearest water art is outside canteen range, and even the visible water IDs belong to the same non-target scenery family.

So I do the unglamorous thing and walk east through the dry gap, stopping at `Point3D(1138,1320,0)`. The route stays on `map1.mul` jungle land tiles at `z=0`. The mushrooms at `1124,1320`, leaves at `1128,1320`, and snake plant at `1136,1320` are static decoration but not impassable blockers, so ordinary movement can pass through. No prompt fires, no region announces itself, no gump opens, no item gets used, and no hidden teleporter row is traced under this path.

The new screen gives me more jungle, not rescue. Decoration data still has no fixed item in the 18-tile box. The `.map` spawn files still show no spawner home inside the box. Binary statics do show two little water-looking patches: one northeast around `1145-1147,1306-1308`, and the old south-edge scraps at `1120,1331-1333`. They are visible scenery only for now. None is within three tiles, and their IDs are not accepted by the canteen's water-target allowlist anyway.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; this run used `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The walked route from `1122,1320,0` to `1138,1320,0` stays on dry passable jungle tiles: `0xAD`, `0xAF`, and `0xAC` at `z=0`.
- Route statics were harmless decoration, not blockers: mushrooms `0xD0D`, leaves `0xD79`, and snake plant `0xCA9` have no impassable flag.
- Normal movement is still `Mobile.Move`: `CheckMovement`, move-over checks against mobiles/items, `Region.CanMove`, movement notifications, and `OnAfterMove`.
- Static source checks found no deterministic decorated item, no fixed mobile, no `.map` spawner home, no region rectangle, and no canteen water target in the new 18-tile screen.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, or PvP/PvE state changed.

Next pressure:

The screen has shifted from empty jungle to slightly different empty jungle. The only new lure is another water-looking patch to the northeast, but I have already learned that scenery water does not fill this canteen. The next honest move is still to scan before moving and keep looking for something built, named, or actually moving.

## Run 34 - Grass Is Not Civilization

I start at `Point3D(1138,1320,0)` with no gump open, no proven creature on the screen, and the same bad canteen lesson in my pocket. The client box is busy only if I count scenery: jungle land, tree statics, leaves, ferns, mushrooms, snake plants, rocks, and old water-looking art. None of it is a mobile, corpse, chest, vendor, trough, tub, barrel, readable object, or visible spawner. The `.map` spawn files also do not put a spawner home inside this 18-tile box. Possible live wandering is still possible, but the static sources do not let me point at a monster.

So I keep the action boring and walk east to `Point3D(1154,1320,0)`. The route is ordinary `Mobile.Move` work: movement check, move-over blockers, region gate, then the position changes. A later tiledata/static audit corrects the broad passability note here: this move landed cleanly, but the line farther east is not fully open because an impassable rock sits at `1156,1320`. No message fires, no region announces itself, no gump opens, no item is used, and the canteen stays empty.

The new scan still does not rescue me. Decoration data has no fixed item here, the spawn files have no home in range, and no crafted water source shows up. The only real change is environmental: the binary land scan is still mostly jungle, but grass tiles finally start appearing inside the east side of the 18-tile box. That is orientation, not safety. I have moved from false shoreline into a jungle/grass edge with ten alien meat, no gold, no skills, no Lodoria or Savage unlock, and nothing visible to click.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; the scan uses `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The pre-action 18-tile scan around `1138,1320,0` has no deterministic mobile, fixed item, chest, corpse, vendor, crafted water source, or `.map` spawner home.
- The movement route from `1138,1320,0` to `1154,1320,0` stays on dry passable jungle. Static scenery is dense, but the passability check leaves the eastward line open at `z=0`.
- The post-action 18-tile scan around `1154,1320,0` is still mostly jungle, with grass tiles now visible eastward and no deterministic interactives.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, or PvP/PvE state changed.

Next pressure:

The first grass in the scan feels like a direction, not a settlement. I should keep moving only after another screen check, watching for anything actually built, named, moving, or usable instead of chasing scenery water again.

## Run 35 - The First Grass Has Teeth

I start at `Point3D(1154,1320,0)`, still in Sosarian wilderness with no gump open. The screen has more shape than before: jungle under my boots, grass showing to the east, water-looking scenery back to the northeast, and enough trees, leaves, ferns, mushrooms, rocks, snake plants, rushes, and saplings to make a straight line feel suspicious. The client-range scan still finds no deterministic mobile, corpse, chest, vendor, trough, tub, barrel, readable object, decorated item, or `.map` spawner home. The nine visible water statics around `1145-1147,1306-1308` are the same impassable non-target scenery family, not canteen relief.

The grass is real, but the first direct route to it is not. A tile audit catches an impassable `rock` static at `1156,1320`, exactly where a lazy eastward walk would bump. So I do what a player would do on screen: step around the rock by jogging one tile south, then keep east through open jungle/grass tiles until I stop at `Point3D(1170,1321,0)`. No prompt fires, no region text appears, no gump opens, no item is used, and there is no hidden teleporter or public-door trigger under this path.

The new screen is half jungle, half grass, which feels better until the scan finishes. It is still just wilderness. `decorate.cfg` has no fixed item in the 18-tile box, `towns.map`, `animals.map`, and `world.map` have no spawner home in range, and `Regions.xml` still has no named Sosaria rectangle containing the point. The canteen stays empty because the post-move scan has no water, trough, tub, barrel, or `CheckWaterTarget` static at all.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; this scan used `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The pre-action scan around `1154,1320,0` is mostly jungle with 135 grass land tiles in range, plus nine visible impassable water statics that are not fill targets.
- A direct east line from `1154,1320` is blocked at `1156,1320` by static `0x1778` named `rock`.
- The chosen route jogs through clear tiles at `1155,1321`, `1156,1321`, then east to `1170,1321`, ending on dry passable `0x5` grass at `z=0`.
- The post-action scan around `1170,1321,0` has 740 jungle and 629 grass land tiles, 348 statics led by leaves, trees, mushrooms, fern, and rock, but no deterministic interactives, no fill source, no spawner home, and no named region.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, or PvP/PvE state changed.

Next pressure:

Grass is only orientation. I am still skillless, poor, carrying ten alien meat and an empty canteen, and now standing on the jungle/grass edge without a named road, settlement, NPC, or water source. The next honest move is another screen check before choosing a direction.

## Run 36 - The Woods Finally Move

I start at `Point3D(1170,1321,0)` under the old static-scan habit: no gump open, no fixed person, chest, corpse, trough, tub, barrel, vendor, journal, or road sign obvious in the tile files. That was the mistake. From this run forward, the live save snapshot is part of the screen. `.map` files explain where spawners came from; they do not decide whether the saved world is empty.

I still cannot walk straight east. The first east tile, `1171,1321`, is blocked by an impassable apple tree, and the same line hits impassable rocks at `1185,1321`. I jog one tile south to `1171,1322`, follow the clear grass east along `y=1322`, then step back to `Point3D(1186,1321,0)`. No message appears, no region name appears, no gump opens, no item is used, and no hidden trigger is traced under the route.

The corrected post-action screen is not empty. The live export loaded root `Saves/` through `World.Load()` and then scanned the 37-by-37 client rectangle around `1186,1321,0`. I can now see five saved mobiles in range: deer at `1194,1306`, eagle at `1198,1310`, eagle at `1196,1314`, deer at `1203,1316`, and horse at `1196,1326`. There are no visible world items and no invisible transition item in this slice, but the nearby spawner pressure is real: an animal spawner at `1209,1292,53`, a hostile/world spawner at `1209,1292,50`, and two southern jungle spawners centered around `1165,1361`.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; terrain/static attribution stays on `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The live-state snapshot is now authoritative for visible saved entities. `docs/CCWM/live-state/scans/run-36-1186-1321.yaml` uses the server's rectangular 18-tile client range: x `1168..1204`, y `1303..1339`.
- A direct east line from `1170,1321` is blocked immediately by an impassable apple tree at `1171,1321`, and later by rocks at `1185,1321`.
- The chosen route stays on dry passable grass: `1171,1322` through `1185,1322`, then back to `1186,1321`.
- The post-action live scan sees animals, not help: two deer, two eagles, and a horse. No visible item, fill source, cave exit, settlement, named region, or usable object appears in this screen.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, or PvP/PvE state changed.

Next pressure:

The woods are no longer just terrain. There is wildlife on screen and active spawner pressure nearby, but I still have ten alien meat, an empty canteen, no skills, no gold, and no visible help. The next move should treat moving creatures as part of the player experience before drifting toward anything built, named, or usable.

## Run 37 - A Horse Is Not a Horse Yet

I start at `Point3D(1186,1321,0)`, in grass with the jungle behind me and no gump open. The old static scan said empty. The live save scan says that was wrong in the way a player would immediately notice: a deer is north-east at `1194,1306`, two eagles are flitting around `1198,1310` and `1196,1314`, another deer is at `1203,1316`, and a horse is close enough to matter at `1196,1326`. There are still no world items, no corpse, no chest, no trough, no tub, no barrel, no sign, and no named region.

The horse is the one that makes me stop. I do not attack it, do not claim it, and do not open a taming menu from ten tiles away. I single-click it like a normal player checking what the moving shape is.

That click is only a label. The server accepts the single-click because the horse is visible, alive, on the same map, and inside the rectangular 18-tile update range. `BaseCreature.OnSingleClick` has no extra `(tame)` line for an uncontrolled wild animal, so the inherited mobile click path just sends the overhead name: `a horse`. It is still wild, still uncontrolled, still not a mount, still not following me, and still not proof that the nearby hostile spawner is safe.

Mechanical friction learned:

- The live-state snapshot outranks the old static-empty conclusion for saved entities. At this coordinate, the current screen has five visible saved mobiles and zero visible saved items.
- Single-clicking a mobile is gated by `CanSee` plus update range. The update range is rectangular: x and y each within 18 tiles.
- A wild `Horse` is a `BaseMount` with `Tamable = true`, `ControlSlots = 1`, and usually `MinTameSkill = 29.1`, but that is not granted by the label. The click does not run taming, does not open a gump, and does not change followers.
- The context menu path could expose a `Tame` entry for a tamable uncontrolled creature, but selecting that entry has its own range-6 gate and calls `UseSkill(Taming)`. I did not request or select that menu here.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, or location state changed.

Next pressure:

The animal label confirms the grass is alive, not settled. The horse is tempting because it could become movement or escape later, but from here it is still a wild mobile, ten tiles away, with active animal and hostile spawner pressure overlapping the screen. The next honest move is to either edge closer before trying any taming flow, or keep distance and continue looking for built shelter or water.

## Run 38 - Close Enough Is Still Not Tamed

I start at `Point3D(1186,1321,0)` with no gump open and the horse already labeled. The screen is still wildlife, not civilization: deer and eagles to the north and east, the horse at `1196,1326`, no visible item, no corpse, no chest, no trough, no tub, no barrel, no sign, no vendor, and no named region. The horse is the visible lure, but I am still ten tiles away from it. The `Tame` context entry, if I get that far, is not a magic button from anywhere on the screen.

So I move instead of choosing a menu entry I cannot legally reach. I walk southeast through clear grass, using the route `1187,1322`, `1188,1323`, `1189,1324`, `1190,1325`, `1191,1326`, and stop at `Point3D(1191,1327,0)`. The binary map/static check finds grass under every route tile and zero statics on those exact tiles. No message fires, no gump opens, no skill runs, no item is used, and the horse is not mine.

The new screen is busier. The horse is now within the rectangular range-6 context-menu gate at dx `5`, dy `1`, but the live snapshot also shows more animals inside the shifted client box: an eagle at `1198,1310`, a goat at `1208,1313`, an eagle at `1196,1314`, a deer at `1203,1316`, a llama at `1208,1323`, a fox at `1209,1324`, and the horse at `1196,1326`. There are still zero visible world items in the live JSONL slice. The hostile spawner source is still only pressure, not a visible hostile mobile in this rectangle.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`; this movement check used `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The route from `1186,1321,0` to `1191,1327,0` is normal `Mobile.Move` terrain movement through dry grass with no static blocker on the stepped tiles.
- The horse is now close enough that a normal context-menu request could expose `Tame`, because `BaseCreature.GetContextMenuEntries` adds it for a tamable, uncontrolled, living creature and `ContextMenuResponse` enforces the entry range before clicking it.
- Taming is still not a solved path. The visible menu reach is range `6`, but the actual `Taming.InternalTarget` uses range `2`. From dx `5`, dy `1`, a `Tame` click will fail distance before the horse's normal `29.1` or rare `49.1` skill requirement can matter.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, or ownership state changed.

Next pressure:

I am now close enough to make the horse menu a fair next temptation, but the code has already told me the character is skillless. The next honest move is to open the horse context menu and learn that friction through the visible `Tame` entry, or break off and keep searching for water, shelter, or a trainer.

## Run 39 - The Menu Is Not a Pet

I start at `Point3D(1191,1327,0)` with no gump open and a horse close enough to feel useful. The screen is still not a town. The live snapshot gives me wildlife, not shelter: eagle, goat, eagle, deer, llama, fox, and the horse at `1196,1326`. There are still zero visible world items in the shifted 18-tile client box. No trough, tub, barrel, chest, corpse, sign, vendor, or named region appears.

This time I do not invent a taming result. I ask the horse for its normal context menu. The server accepts the request because the horse is visible, alive, on `Map.Sosaria`, and inside the rectangular update range. Then `ContextMenu` asks the horse for entries, and `BaseCreature.GetContextMenuEntries` adds `Tame` because the horse is tamable, uncontrolled, and I am alive. The client receives the menu. The tempting word is finally on screen.

But a menu row is not an action result. I stop there. I have not clicked `Tame`, so `ContextMenuResponse` has not run, `TameEntry.OnClick` has not called `UseSkill(Taming)`, and `Taming.OnTarget` has not reached the horse's skill gate. The animal is still wild. My follower count is still empty. My Taming skill is still `0.0`, which is below the horse's normal `29.1` requirement and below its rare `49.1` branch.

Mechanical friction learned:

- A context-menu request is legal from here: same map, visible target, update range satisfied.
- The visible horse menu exposes `Tame` through `TameEntry` with range `6`; current distance is dx `5`, dy `1`.
- Opening the menu only sets/sends `from.ContextMenu`. It does not select the entry.
- Selecting `Tame` is a separate response. Only that response can call `UseSkill(SkillName.Taming)`.
- Even if I click it next, the first hard gate from this exact spot is the hidden target range: `Taming.InternalTarget` uses range `2`, while I am dx `5`, dy `1`. Only after moving closer would the `0.0` Taming versus at least `29.1` horse requirement matter.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, or location state changed.

Next pressure:

The horse is now an exposed bad bargain: close enough to see the word `Tame`, not close enough for the actual target. The next honest click is either to press the visible entry and eat the distance failure, or close/ignore the menu and keep looking for water, shelter, or someone who can teach me.

## Run 40 - Tame Has a Shorter Arm Than the Menu

I start at `Point3D(1191,1327,0)` with the horse menu already open. The visible screen has not become a town while I was staring at the word `Tame`: eagle, goat, eagle, deer, llama, fox, and the horse are still the only saved mobiles in the 18-tile client box, and the live item slice is still empty. No chest, corpse, trough, tub, barrel, sign, vendor, road, or named region appears. The horse is the shiny thing, and the menu row is already exposed, so I press it.

The click goes through the normal context-menu response path. The server clears my current context menu, checks that the target is still the same visible horse, finds the entry, checks the entry range, and runs `TameEntry.OnClick`. That shortcut does not show a targeting cursor; it locks targeting, starts the Taming skill handler, and immediately invokes the horse as the target.

Here is the correction: the menu row and the actual taming target do not use the same reach. `TameEntry` is enabled at range `6`, so the row was fair to click at dx `5`, dy `1`. But `Taming.OnUse` creates an `InternalTarget` with range `2`. The immediate target invoke hits the generic target range gate first and sends `That is too far away.` The horse's `29.1` or rare `49.1` skill requirement is still true for a later close attempt, but this attempt never gets that far. No taming timer starts, no soothing speech appears, no skill check is rolled, no follower slot is filled, and the horse stays wild. The failed instant target also resets the skill timer back to now, so I am not stuck in a six-hour skill lockout from this bad click.

Mechanical friction learned:

- Selecting `Tame` is a separate visible context-menu response after opening the horse menu.
- `ContextMenuResponse` clears `from.ContextMenu` before it validates the target, entry index, enabled flag, and range.
- `TameEntry.OnClick` uses `SkillName.Taming` with `TargetLocked = true`, then immediately invokes the horse target if skill use starts.
- `Taming.OnUse` creates `InternalTarget` with range `2`, so a range-6 context-menu click can still be too far away for the actual tame target.
- This character has `0.0` Taming, no familiar mastery, and no elixir/training buff, so the horse's skill gate would still matter later from within two tiles. It did not fire here.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, or location state changed.

Next pressure:

The horse is no longer a plan; it is a lesson. I am still standing in open grass with ten alien meat, an empty canteen, no gold, no skills, and active wilderness spawner pressure. The next honest move is to stop trying to own wildlife and look for water, shelter, a road, or a trainer.

## Run 41 - The Smaller Animal Has Company

I start at `Point3D(1191,1327,0)` with the horse menu gone and no gump open. The screen is still animals and grass: eagle, goat, eagle, deer, llama, fox, and the same horse that just told me I have no chance. There are still zero visible world items in the live slice. No chest, corpse, trough, tub, barrel, sign, vendor, road, or named region appears.

The horse is a dead end for now, but the smaller animals east of me are still the only moving things I can see. I do not select another context menu entry from across the screen. I walk east and a little north through clear grass, keeping off the horse's tile, and stop at `Point3D(1204,1324,0)`. The route is ordinary movement over `map1.mul` grass with no statics on the stepped tiles. No prompt fires, no region text appears, no gump opens, no item is used, and no animal becomes mine.

The new screen is the reward and the warning. The fox is now five tiles east, close enough that a future normal context-menu request could matter. The llama is also close. But the shifted live scan now exposes a grey wolf at `1207,1307`, along with more goats, deer, eagles, a swallow, the fox, llama, and the old horse. The wolf is not just scenery: its class is an aggressive animal with much higher fighting skills than my empty-skill crash survivor. I still do not know whether it will path or aggro on the next live tick, but I can see enough to stop treating the animal cluster as harmless.

Mechanical friction learned:

- `Map.Sosaria` still resolves to file index `1`, so this movement used `map1.mul`, `staidx1.mul`, and `statics1.mul`.
- The route from `1191,1327,0` to `1204,1324,0` stays on dry grass and has zero statics on every stepped tile.
- The live-state slice around `1204,1324,0` has fourteen visible saved mobiles and zero visible saved world items.
- `Fox` is tamable and has `MinTameSkill = -15.3`, so it may be a different future taming problem from the horse. I have not opened its menu or selected `Tame` yet.
- `GreyWolf` is also tamable, but it is an aggressive wolf with `MinTameSkill = 53.1`, stronger tactics/fighting, and no ownership by me. It is a visible risk, not a pet.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, or equipment state changed.

Next pressure:

The fox is finally close enough to test later, but the wolf is on the same screen. The next honest move is not "I have a pet"; it is either carefully inspect the fox's visible menu from range, back away from the wolf, or keep searching for something built and safer than wildlife.

## Run 42 - I Name the Risk

I start at `Point3D(1204,1324,0)` with no gump open, no context menu open, and the same crowded animal screen. The fox is close enough to tempt me, but the grey wolf is the thing that changes how the grass feels. It is up near `1207,1307`, only three tiles east but seventeen tiles north, which puts it inside my client update range and outside any sane "try to tame this right now" range. There are still zero visible world items in the live slice. No chest, corpse, trough, tub, barrel, sign, vendor, road, or named region appears.

So I do the smallest normal player action: I single-click the wolf to make the shape admit what it is.

The server accepts the click because the target is a visible, alive mobile on the same map and inside the rectangular eighteen-tile update range. The default region does not block single-click labels. `BaseCreature.OnSingleClick` would add a `(tame)` or `(bonded)` overhead line only for controlled and commandable creatures, and this wolf is neither. The inherited mobile click path just sends the label: `a grey wolf`.

That label is useful precisely because it gives me nothing else. No context menu opens. No `Tame` entry is selected. No Taming skill runs. No combat branch runs. No ownership, follower, pet, inventory, quest, discovery, hunger, thirst, fame, karma, PvP/PvE, or location state changes. The wolf remains a wild aggressive animal, not a pet and not scenery.

Mechanical friction learned:

- Single-clicking a visible mobile is gated by `CanSee` plus `Utility.InUpdateRange`, which is rectangular: x and y each within 18 tiles.
- The live grey wolf at `1207,1307,0` is dx `3`, dy `17` from me, so the label click is legal.
- The same wolf is not a near tame target from here. A normal `TameEntry` uses range `6`, and dy `17` fails that range even before the `53.1` Taming requirement matters.
- `GreyWolf` uses `FightMode.Aggressor`, `Tactics` and `FistFighting` ranges of `45.1-60.0`, `MinTameSkill = 53.1`, and `ControlSlots = 1`. Seeing its name does not pacify it.
- `BaseCreature.OnSingleClick` adds a tame/bonded overhead line only for controlled commandable creatures. This uncontrolled wolf only gets the ordinary inherited name label.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, equipment, or location state changed.

Next pressure:

Now I know the screen's danger by name. The fox is still nearby, but spending more attention on animal menus while a wild aggressive wolf is in sight feels worse than backing away or angling toward safer built evidence. The next honest move should account for the wolf first.

## Run 43 - Distance Is the First Tool

I start at `Point3D(1204,1324,0)` with no gump open and no context menu open. The screen has too many animals for comfort: deer, goats, eagles, a swallow, a llama, a fox, the failed horse, and the grey wolf I just labeled at `1207,1307`. The fox is close enough to tempt another context-menu experiment, but the wolf is the screen's loudest fact. There are still zero visible world items, no trough, no tub, no barrel, no chest, no corpse, no sign, no vendor, and no named region.

So I do not open the fox menu. I back away from the wolf, walking south-southeast over dry grass: `1205,1325`, `1206,1326`, `1207,1327`, `1208,1328`, `1209,1329`, `1210,1330`, `1211,1331`, `1212,1332`, `1213,1333`, `1214,1334`, `1215,1335`, `1216,1336`, then down to `1216,1340`. The map/static check stays boring in the good way: every stepped tile is `map1.mul` grass at z `0`, not wet, not impassable, and with zero statics on the route. No prompt fires, no region text appears, no gump opens, no item is used, no skill runs, and no animal becomes mine.

The new screen is quieter but not safe. The wolf drops outside the 18-tile client rectangle, which is the whole point of the move. The live snapshot now shows only three saved mobiles in view: the llama at `1208,1323`, the fox at `1209,1324`, and the eagle at `1213,1324`. They are all north of me and outside range-6 interaction pressure. There are still no visible saved world items in the new slice. The old spawner homes still matter as background wilderness pressure, but the normal player screen is no longer wolf-first.

Mechanical friction learned:

- Backing away is ordinary `Mobile.Move` movement, not a teleport or helper call.
- `Map.Sosaria` still resolves to file index `1`; this movement used `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The selected route from `1204,1324,0` to `1216,1340,0` stays on dry passable grass with zero statics on every stepped tile.
- The post-move live-state slice around `1216,1340,0` is x `1198..1234`, y `1322..1358`; it has three visible saved mobiles and zero visible saved items.
- The wolf at `1207,1307,0` is now outside that client rectangle by y distance `33`; it is not visible or clickable from the new position.
- The fox remains visible but is no longer close: dx `7`, dy `16`, which fails the normal range-6 context-menu entry pressure even though it remains inside update range.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, equipment, or social state changed.

Next pressure:

This is still not a road, settlement, water source, or trainer. The move bought space, not progress. The next honest move should keep scanning from the quieter grass, with the visible fox/llama/eagle acknowledged but no longer treated as immediate tools.

## Run 44 - Fox Range Is Not Fox Taming

I start at `Point3D(1216,1340,0)` with no gump open and no context menu open. The screen is quiet only because the wolf is gone from the rectangle. It is not civilized. The live slice shows a llama at `1208,1323`, a fox at `1209,1324`, and an eagle at `1213,1324`, with zero visible world items. No trough, tub, barrel, chest, corpse, sign, vendor, road, trainer, or named region appears.

The fox is the only thing here that looks like a plan. The horse already proved that "tamable" does not mean "mine", but this smaller animal has a much lower skill requirement in code. From here, though, it is still too far north for the range-6 menu pressure. I do not open a menu from across the screen. I walk north, then west, over dry grass: `1216,1339`, `1216,1338`, `1216,1337`, `1216,1336`, `1216,1335`, `1216,1334`, `1216,1333`, `1216,1332`, `1216,1331`, `1216,1330`, `1215,1330`, and stop at `Point3D(1214,1330,0)`.

The route is boring in the useful way. Every stepped tile is `map1.mul` grass with zero statics. One tile rises to z `1`, then drops back to z `0`, but this is still ordinary movement through `Mobile.Move` and `CheckMovement`, not a teleporter, region trigger, helper call, item use, or skill use. No prompt fires and no animal becomes mine.

The new screen gets busy again without bringing the wolf back. I can see goats, eagles, deer, a swallow, the llama, the fox, and the old horse; the grey wolf at `1207,1307` is still outside update range by y distance `23`. The fox is now close enough at dx `5`, dy `6` that a future normal context-menu request could expose `Tame`. I stop there. I have not requested the fox's menu, selected a row, invoked Taming, changed followers, gained skill, or made a pet.

Mechanical friction learned:

- The live-state slice around `1214,1330,0` is x `1196..1232`, y `1312..1348`; it has eleven visible saved mobiles and zero visible saved items.
- `Map.Sosaria` still resolves to file index `1`; this movement used `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`.
- The selected route from `1216,1340,0` to `1214,1330,0` stays on dry grass with zero statics on every stepped tile.
- `Fox` is tamable with `ControlSlots = 1` and `MinTameSkill = -15.3`, but that only makes a future action plausible. Movement into range does not call `ContextMenuRequest`, `TameEntry.OnClick`, `UseSkill(Taming)`, or `Taming.OnTarget`.
- The grey wolf remains off-screen from the new tile, so I cannot legally click it, open its context menu, or tame it from here.
- No hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The fox is finally a legal menu target, not a pet. The next honest move is to request its visible context menu and read the option, or break away from the animal trap and keep searching for built shelter, water, road, or a trainer.

## Run 45 - The Fox Menu Is Only a Menu

I start at `Point3D(1214,1330,0)` with no gump open and no context menu open. The live screen is still just grass and animals: two goats, three eagles, two deer, a swallow, a llama, the fox, and the old failed horse. There are still zero visible world items. No trough, tub, barrel, chest, corpse, sign, vendor, road, trainer, or named region appears. The grey wolf remains off-screen by y distance `23`, so I cannot legally click it or ask for its menu from here.

The fox is close enough now: `Point3D(1209,1324,0)`, dx `5`, dy `6`. I right-click it like a player testing the edge of a bad idea, not like a script choosing success. The request passes the same-map, visibility, and update-range gates, builds a `ContextMenu` for the fox, asks the fox for entries, and `BaseCreature.GetContextMenuEntries` adds `Tame` because the fox is tamable, uncontrolled, and I am alive. `TameEntry` is localization `6130` with range `6`, so the row is enabled on my screen.

I stop there. A visible `Tame` row is not a tame attempt. No `ContextMenuResponse` has been sent, so the server has not cleared the menu, checked the selected row, called `TameEntry.OnClick`, invoked `UseSkill(Taming)`, targeted the fox, rolled a skill check, set a control master, filled a follower slot, or changed ownership. The useful new truth is smaller: unlike the horse, the fox's class says `MinTameSkill = -15.3`, so this is the first animal menu that looks mechanically plausible for a zero-skill crash survivor. Plausible is not done.

Mechanical friction learned:

- The current update-range scan remains eleven visible saved animals and zero visible saved items.
- The fox is visible, alive, uncontrolled, on `Map.Sosaria`, and inside both the 18-tile client rectangle and the range-6 `TameEntry` pressure.
- A context-menu request only opens visible UI through `DisplayContextMenu`; selecting `Tame` is a separate later response.
- `Fox` uses `FightMode.Aggressor`, has low animal combat skills, is tamable, uses one control slot, and has `MinTameSkill = -15.3`.
- No movement, hunger, thirst, skill, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, pet, follower, ownership, combat, or equipment state changed.

Next pressure:

The `Tame` row is open in front of me. The next honest action is either press that visible row and accept whatever the taming code does to a zero-skill alien, or close/ignore it and keep looking for water, shelter, road, or a trainer.

## Run 46 - The Word Tame Lies About Reach

I start at `Point3D(1214,1330,0)` with the fox menu already open and read. The screen itself has not changed: eleven saved animals, zero visible world items, no water source, no chest, no corpse, no sign, no vendor, no road, and no named region. The fox is the thing under my cursor at `1209,1324,0`, dx `5`, dy `6`. The row says `Tame`, and it is enabled, so I press it.

The menu lets me try. That is not the same as being close enough. `ContextMenuResponse` clears the open menu, confirms the same visible fox, accepts the selected `Tame` entry because `TameEntry` uses range `6`, and then runs `TameEntry.OnClick`. The taming shortcut locks targeting, suppresses the usual "Tame which animal?" prompt, starts the Taming skill handler, and immediately invokes the fox as the target.

Then the hidden shorter reach bites. `Taming.OnUse` creates its `InternalTarget` with range `2`, not range `6`. The target invoke checks distance before `Taming.OnTarget`, and dx `5`, dy `6` fails. The only player-facing result is the generic target message: `That is too far away.` The fox does not become mine. No taming timer starts, no animal speech starts, no `MinTameSkill = -15.3` roll happens, no owner list changes, no control master is set, no follower slot is filled, and no skill gain is attempted. The skill timer gets reset back to now because the real tame attempt never began.

Mechanical friction learned:

- `TameEntry` being visible at range `6` is only the context-menu gate.
- `Taming.OnUse` creates `InternalTarget` with range `2`; that target gate runs before `Taming.OnTarget`.
- From `Point3D(1214,1330,0)`, the fox at `Point3D(1209,1324,0)` is a legal menu target and an illegal taming target.
- This also corrects the earlier horse lesson: the horse click from dx `5`, dy `1` should have failed the same range-2 target gate before the horse skill requirement.
- `Fox.MinTameSkill = -15.3` still looks reachable for a zero-skill character, but only after I stand within two tiles.
- No movement, hunger, thirst, skill value, stat, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat, pet, follower, ownership, or equipment state changed.

Next pressure:

The menu is gone and the fox is still wild. The honest next move is not another menu click from here. I either step close enough to put the fox inside range `2`, or I finally admit this grass is wasting daylight and keep looking for water, shelter, road, or a trainer.

## Run 47 - Close Enough Is a Different State

I start at `Point3D(1214,1330,0)` with no gump open and no context menu open. The failed `Tame` click has left the fox exactly where it was, northwest at `1209,1324,0`, still wild and still not mine. The screen is still all animals and no tools: goats, eagles, deer, a swallow, a llama, the fox, and the old horse. There are zero visible saved world items. No trough, tub, barrel, chest, corpse, sign, vendor, road, trainer, water target, or named region appears.

The temptation is to hammer `Tame` again, but the last message already told me what the server cares about: not the menu range, the target range. So I move instead. I step northwest over flat empty terrain: `1213,1329`, `1212,1328`, `1211,1327`, and stop at `Point3D(1210,1326,0)`. No prompt fires. No item catches my feet. No hidden teleporter moves me. No gump opens. No context menu opens. No skill runs. No animal becomes mine.

The new screen is not safer, only more actionable. The fox is now at dx `1`, dy `2`, inside the real `Taming.InternalTarget` range. The llama is close too at dx `2`, dy `3`, and an eagle is near at dx `3`, dy `2`, but none of them are controlled. The grey wolf remains barely off-screen: it is still at `1207,1307`, dy `19` from me, one tile beyond the 18-tile client rectangle. The live snapshot now shows twelve visible saved mobiles and still zero visible saved items. I have made a future tame attempt legal by distance, but I have not requested a new menu, selected `Tame`, rolled the fox's `MinTameSkill = -15.3`, started a taming timer, gained skill, used a follower slot, or changed ownership.

Mechanical friction learned:

- Movement is ordinary `Mobile.Move`: direction, `CheckMovement`, move-over checks, `Region.CanMove`, `SetLocation`, movement notifications, and `OnAfterMove`.
- `Map.Sosaria` still resolves to file index `1`; the local binary route check used `map1.mul`, `staidx1.mul`, and `statics1.mul`.
- The four stepped route tiles to `1210,1326,0` have zero statics in the live binary static index. No visible mobile/item blocker, region trigger, gump, context menu, skill, combat, or item-use path ran.
- The post-move live-state slice is x `1192..1228`, y `1308..1344`. It contains twelve visible saved mobiles and zero visible saved items.
- The fox at `1209,1324,0` is now inside range `2`, so the next honest Tame attempt would no longer fail the immediate distance gate.
- The grey wolf at `1207,1307,0` is still outside update range by y distance `19`; I cannot click it or request its menu from here.
- No hunger, thirst, stat, skill value, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The fox is finally close enough for the actual taming target. The next honest move is to request its context menu again and press the visible `Tame` row from this closer tile, while remembering that success is still not guaranteed until `Taming.OnTarget` checks the creature and skill gates.

## Run 48 - The Row Is Back, Not the Pet

I start at `Point3D(1210,1326,0)` with the fox finally close enough to matter: `1209,1324`, dx `1`, dy `2`. The live screen still has no mercy built into it. I can see twelve saved animals, including the close fox, close llama, close eagle, goats, deer, a swallow, and the old horse. There are still zero visible saved world items. No trough, tub, barrel, chest, corpse, sign, vendor, road, trainer, water target, or named region appears. The grey wolf is still just off the screen at dy `19`, close enough to remember and too far away to legally click.

So I do the next normal thing instead of inventing the tame: I ask the fox for its context menu again. The server accepts the request because the fox is on `Map.Sosaria`, visible, alive, and inside the rectangular update range. `ContextMenu` asks the fox for entries. `BaseCreature.GetContextMenuEntries` adds `Tame` because the fox is tamable, uncontrolled, and I am alive. `TameEntry` is still localization `6130` with range `6`, and from this close tile the row is plainly enabled.

I stop there. The word `Tame` is on the screen, but I have not pressed it. That distinction matters more than it feels like it should. No `ContextMenuResponse` has cleared the menu. No `TameEntry.OnClick` has locked targeting. No `UseSkill(Taming)` has created the range-2 target. No `Taming.OnTarget` has tested `Fox.MinTameSkill = -15.3`, started a timer, rolled a skill check, or set a control master. The fox is still wild and uncontrolled. My follower count is still empty.

Mechanical friction learned:

- The current live-state slice remains x `1192..1228`, y `1308..1344`: twelve visible saved mobiles and zero visible saved items.
- A close fox context-menu request is legal from `Point3D(1210,1326,0)`: same map, visible target, and update range all pass.
- The visible close fox menu exposes an enabled `Tame` row through `TameEntry` with range `6`.
- Opening the menu only stores/sends `from.ContextMenu`; selecting the row is a separate later client response.
- Being inside range `2` removes the earlier immediate distance failure for a future attempt, but it does not run the future attempt by itself.
- No movement, hunger, thirst, stat, skill value, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The `Tame` row is open again, and this time the fox is close enough for the target range. The next honest action is to press that visible row and let the taming code decide, or close it and finally stop gambling survival on wildlife.

## Run 49 - Taming Starts Before Ownership

I start exactly where the last run left me: `Point3D(1210,1326,0)`, no gump, the fox menu open, and the visible `Tame` row already read. The fox is still at `1209,1324`, dx `1`, dy `2`. The screen is still empty of help: twelve saved animals, zero visible world items, no trough, no tub, no barrel, no chest, no corpse, no sign, no vendor, no road, no trainer, and no named region. The grey wolf remains just outside the client rectangle at dy `19`.

So I press the row.

This time the word `Tame` does not lie about reach. `ContextMenuResponse` clears the menu, finds the same visible fox, and accepts the row because `TameEntry` uses range `6`. `TameEntry.OnClick` locks targeting, suppresses the usual "Tame which animal?" prompt, uses the Taming skill, and immediately invokes the fox as the target. The shorter `Taming.InternalTarget` range is `2`, and from this tile the fox finally passes it.

That still does not make a pet. `Taming.OnTarget` checks the fox gates: tamable, uncontrolled, female tamer allowed, one follower slot available, no owner-cap problem, no subdual requirement, and my `0.0` Taming is still above the fox's `MinTameSkill = -15.3`. The result is only the beginning. The code stores the fox in `m_BeingTamed`, sends the overhead line that I start taming it, and starts a three-second timer. I do not own the fox yet. It is not following me, not bonded, not mounted, not a follower slot, and not a survival plan.

Mechanical friction learned:

- Selecting an open context-menu row consumes the menu before the entry action runs.
- The close fox passes both gates now: context-menu range `6` and Taming target range `2`.
- `Fox` is low enough skill for a zero-skill alien to start the attempt, but ownership waits for the timer and final skill roll.
- The taming timer can still collapse if I move too far away, die, lose sight or path, the fox becomes controlled or untamable, or the animal takes damage after the start time.
- `Taming.OnUse` has set a long skill delay for now; the timer completion or abort path is what resets it.
- No movement, hunger, thirst, stat, skill value, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The honest next move is not to rename the fox in my head. It is to stand there and survive the first taming timer tick while the wild animal and the rest of the grass decide whether this was a mistake.

## Run 50 - The First Tick Pays Me, Not the Fox

I do the hardest thing a new survivor can do in grass full of animals: nothing. I stay at `Point3D(1210,1326,0)`, close enough to the fox at `1209,1324` that the tame attempt can keep breathing. The screen has not become kinder. I still see the same twelve saved animals and zero visible world items: eagles, goats, deer, a swallow, a llama, the fox, and the old horse. No trough, tub, barrel, chest, corpse, sign, vendor, road, trainer, or named region appears. The grey wolf is still one tile outside the client rectangle, which is not comfort, just distance.

Three seconds pass and the server timer ticks. That tick is not the end of taming. It checks that I am still within range `6`, alive, able to see the fox, in line of sight, pathable, and not trying to tame something that became controlled, untamable, over-owned, unsubdued, or newly damaged. In this static slice, those gates pass. I get one public line out of my mouth: `I won't hurt you...`

The surprise is that the first tick trains me before it gives me anything. Because the fox is not already mine, the timer passively checks `Druidism` against the creature. At `0.0`, the actual check is not a success story, but the skill system still grants a gain while the skill is under `10.0`. I feel the tiniest useful click: `Druidism` rises from `0.0` to `0.2`. No stat rises with it this time. The fox remains wild. It is not owned, not controlled, not following, not bonded, not mounted, and not consuming a follower slot.

Mechanical friction learned:

- A non-final taming timer tick is a survival check, not a pet claim.
- The timer has a maintenance range of `6`, wider than the original `Taming.InternalTarget` range `2`, but it can still abort on range, death, sight, line of sight, pathing, tamability, ownership, subdual, or post-start damage.
- Non-final ticks run `RevealingAction`, say one random taming phrase, and passively call `CheckTargetSkill(Druidism, creature, 0.0, 125.0)` when the creature is not already owned.
- Under-10 skill gain is real here: even with a failed `Druidism` check at `0.0`, `SkillCheck.Gain` raises the skill. This run simulates a `0.2` gain and no Int stat gain.
- The final `Taming` roll, `SetControlMaster`, owner list mutation, follower accounting, and pet state still have not happened.

Next pressure:

Now I have a little Druidism and still no fox. The next honest move is to hold position through the second timer tick, unless the screen changes first.
