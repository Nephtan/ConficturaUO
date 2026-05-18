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

## Run 51 - The Second Tick Still Isn't Mine

I keep my feet planted at `Point3D(1210,1326,0)`. The fox is still just northwest at `1209,1324`, close enough for the timer's range-6 leash. The same saved animals are still on the screen: eagles, goats, deer, a swallow, a llama, the fox, and the old horse. The live item slice is still empty. No trough, tub, barrel, chest, corpse, sign, vendor, road, trainer, or named region appears. The grey wolf is still outside the client rectangle by one tile of y-distance, which means I can remember it but cannot legally click it.

Three more seconds pass. The taming timer ticks again, and it is still not the finish line. The same maintenance gates have to hold: range, life, visibility, line of sight, pathing, tamability, ownership, subdual, and no fresh damage after the start. In this static pass, they hold. I say `Nice and easy...` out loud to a fox that is still not mine.

The second tick pays the same weird consolation prize as the first. Because the fox is not already owned, the code passively checks `Druidism` again. At `0.2`, the chance is still basically nothing, about 0.16% after the max-skill widening to `126.0`, so I treat the check as a failure. But the shard's skill gain code still gives under-10 skills a push. This time the simulated gain is `0.3`, taking `Druidism` from `0.2` to `0.5`. No stat rises. The fox remains wild, uncontrolled, unfollowing, unbonded, and outside my follower slots.

Mechanical friction learned:

- The second non-final taming tick is another maintenance check, not ownership.
- `InternalTimer.OnTick` does not reset `NextSkillTime` during non-final ticks; the active taming attempt still owns the skill path.
- Passive `Druidism` gain can keep happening before any successful tame because `SkillCheck.CheckSkill` calls `Gain` while the skill is under `10.0`, even when the chance roll fails.
- The final tick is still the real gate: it can abort first, then it must run the final `Taming` check before `SetControlMaster`, owner list mutation, follower accounting, and pet state can happen.
- No movement, hunger, thirst, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, equipment, ownership, or follower state changed.

Next pressure:

The fox is still a wild animal under an active timer. The honest next move is to survive the final tick and let the real taming roll decide whether this becomes a pet or just another lesson.

## Run 52 - The Fox Teaches Failure

I hold still at `Point3D(1210,1326,0)`, because moving now would be the dumbest way to lose the attempt. The screen is the same open grass box: twelve saved animals, zero visible items, no water, no chest, no corpse, no road, no trainer, no named region. The fox is still close at `1209,1324`, dx `1`, dy `2`. The grey wolf is still just outside the update range, which means it is worry, not a clickable problem.

Three more seconds pass. This is the final tick, so the timer stops being suspense and becomes judgment. The maintenance gates still pass: I am close enough, alive, able to see the fox, with line of sight and pathing, and the fox is still tamable, uncontrolled, not over-owned, not requiring subdual, and not newly damaged. The code removes the active taming entry and resets my skill timer before it asks whether I actually win.

First, the shard gives me one more consolation prize. The passive `Druidism` check is basically hopeless, but the under-10 gain rule still pushes the skill from `0.5` to `0.7`. Then the real tame roll happens. The fox is beginner-friendly, and my tiny Druidism bump helps a little, but the final `Taming` chance is still only about thirty-one percent. I miss it. The fox tells me what the grass already knew: `You fail to tame the creature.`

That failure still trains me. The final `Taming` check also triggers under-10 gain, so `Taming` rises from `0.0` to `0.2`. But there is no pet. `SetControlMaster` never runs. The owner list does not change. Follower slots stay empty. The fox is still wild, still close, and now the timer is gone.

Mechanical friction learned:

- A completed taming timer is not automatically a tame. The final branch resets `NextSkillTime`, removes `m_BeingTamed`, passively checks `Druidism`, then rolls `Taming`.
- `Fox.MinTameSkill = -15.3` makes a zero-skill attempt possible, not safe. With `Druidism` at `0.7`, this simulated final check still only has roughly a thirty-one percent success chance.
- Failed skill checks can still train under-10 skills. This failed fox attempt raised `Druidism` to `0.7` and `Taming` to `0.2`.
- The failure path sends `You fail to tame the creature.` and does not call `SetControlMaster`, add an owner, change follower accounting, mount, bond, guard, or follow.
- No movement, hunger, thirst, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, equipment, ownership, or follower state changed.

Next pressure:

The fox is still two tiles away and legal to try again after reopening its menu, but the screen is still just wilderness and animals. The honest next fork is whether I waste another few seconds gambling on a fox, or finally move on looking for water, a road, shelter, or someone who can teach me why I am whispering at wildlife.

## Run 53 - The Menu Comes Back, Not the Pet

I do the predictable thing: I stare at the fox that just rejected me and ask the client what I can do to it again. I am still at `Point3D(1210,1326,0)`. The fox is still at `1209,1324`, dx `1`, dy `2`. The live screen is still twelve saved animals and zero visible items: eagles, goats, deer, a swallow, a llama, the fox, and the old horse. No water source, chest, corpse, sign, road, trainer, shelter, or named region has appeared. The grey wolf is still one tile beyond the client rectangle, which makes it background anxiety, not a button.

The context-menu request is legal. Same map, I can see the fox, and update range is not the problem. The menu builder asks the fox for its entries. The fox is still tamable, still uncontrolled, and I am still alive, so the shard gives me the same visible row: `Tame`.

That row being back is useful, but it is not action yet. I have not selected it. The code has not run `TameEntry.OnClick`, has not used the Taming skill, has not opened a target cursor, has not put the fox back into `m_BeingTamed`, and has not started another timer. The open menu only proves that the failed final tick reset enough state to let me try again through normal UI.

Mechanical friction learned:

- `PacketHandlers.ContextMenuRequest` is a visibility and update-range gate, not a skill use.
- `BaseCreature.GetContextMenuEntries` adds `TameEntry` when the creature is tamable, uncontrolled, and the player is alive.
- `TameEntry` is visible at context range `6`; the stricter Taming target range `2` only matters after selecting the row.
- A reopened `Tame` row is not a pet, not a timer, and not a second attempt. The next mutation requires pressing the visible row.
- No movement, hunger, thirst, stat, skill, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat, owner, follower, or equipment state changed.

Next pressure:

The menu is open and the row is visible. The honest next move is either to press `Tame` and gamble again from legal range, or close/ignore the fox and finally walk toward water, road, shelter, or a trainer.

## Run 54 - The Second Try Starts, Not Ends

I press `Tame` again.

Nothing about the screen has changed enough to save me from myself. I am still at `Point3D(1210,1326,0)`. The fox is still close at `1209,1324`, dx `1`, dy `2`. The live snapshot still gives me twelve saved animals and zero visible items: eagles, goats, deer, a swallow, a llama, the fox, and the old horse. No water source, chest, corpse, sign, road, trainer, shelter, or named region has appeared. The grey wolf is still just outside the client rectangle by one tile of y-distance.

This click is legal. The reopened context menu is still tied to the fox. `ContextMenuResponse` clears the menu before it does anything useful, finds the same visible fox, and checks the selected row's range `6`. That passes. `TameEntry.OnClick` locks targeting, suppresses the normal "Tame which animal?" prompt, uses the Taming skill, and immediately invokes the fox as the target. The real target range is still `2`, and from here the fox passes it.

The code lets the attempt start. The fox is tamable, uncontrolled, allowed for a female tamer, uses one follower slot, has no owner-cap block, does not need to be subdued, and my miserable `0.2` Taming is still above `Fox.MinTameSkill = -15.3`. I get the overhead emote again: `You start to tame the creature.`

That is still not ownership. The shard stores the fox in `m_BeingTamed` and starts another three-second `InternalTimer`. This time I simulate the timer length branch as four ticks. No tick has happened yet. No Druidism check has trained me. No final Taming roll has fired. `SetControlMaster` has not run. The fox is still wild, unowned, uncontrolled, and not using a follower slot.

Mechanical friction learned:

- A reopened context menu only becomes an attempt after the visible row is selected.
- `ContextMenuResponse` consumes the menu first, so there is no open menu after the click.
- The second try passes the same two range gates: context row range `6`, then `Taming.InternalTarget` range `2`.
- `Taming.OnUse` sets the long skill delay again, but the active timer now owns the reset path.
- `Utility.Random(3, 2)` can make this attempt longer than the first one; this simulated second timer has four ticks pending.
- No movement, hunger, thirst, stat, skill value, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The menu is gone and the fox is in the timer again. The honest next move is to stand still through the first three-second tick unless the visible grass changes first.

## Run 55 - The Timer Teaches, Not Tames

I stand still and let the first tick happen.

The screen is the same trap I agreed to: `Point3D(1210,1326,0)`, open grass, twelve visible saved animals, and zero visible items. The fox is still at `1209,1324`, close enough at dx `1`, dy `2`. The old horse is still west of me. The grey wolf is still just outside the 18-tile client rectangle, which means it is anxiety, not a button. There is no open gump, no open context menu, no water source, no road, no shelter, and no trainer. The only thing that has a turn right now is the active taming timer.

The first tick is not the final tick. `Taming.InternalTimer.OnTick` increments `m_Count` from `0` to `1`, then checks whether I stayed within range `6`, stayed alive, could see the fox, had line of sight or pathing, and whether the fox stayed tamable, uncontrolled, not over-owned, not needing subdual, and not newly damaged. I simulate all of those as still true because I did not move and no live AI tick is being advanced here.

Since this second attempt has `m_MaxCount = 4`, count `1` lands in the non-final branch. I reveal myself again and whisper the simulated line `See? Nothing to be afraid of...`. The shard then passively checks `Druidism` against the fox. The success chance is tiny, about `0.56%` at `Druidism 0.7` against the widened max `126.0`, and I miss it. Missing still trains under the under-10 rule. This tick raises `Druidism` from `0.7` to `0.8`. The fox remains wild, unowned, uncontrolled, and outside my follower count.

Mechanical friction learned:

- A non-final taming timer tick is a maintenance check plus speech plus passive `Druidism`; it is not the tame result.
- The active second timer is now `1 / 4` ticks complete. `m_BeingTamed` still points from the fox to Mira Vale.
- `NextSkillTime` still stays under the active timer's control. It does not reset on this non-final tick.
- The passive `Druidism` check can fail and still train while the skill is under `10.0`.
- No movement, hunger, thirst, stats, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, Taming value, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The fox timer is still running. The honest next move is to keep standing still for the second three-second tick unless the visible wilderness changes first.

## Run 56 - The Fox Stays a Fox

I do the only thing the active timer allows without sabotaging myself: I stand still.

The screen has not turned into help. I am still at `Point3D(1210,1326,0)` on Sosaria grass, with twelve saved animals in the client rectangle and zero visible items. The fox is still at `1209,1324`, close enough at dx `1`, dy `2`. The horse, llama, eagles, goats, deer, and swallow are still scenery with teeth or hooves. The grey wolf is still one tile beyond the update range by y-distance, so it stays memory and worry, not a thing I can click.

Three more seconds pass. The timer count moves from `1` to `2`, and the same maintenance gates have to hold before anything useful can happen: range `6`, alive, visible, line of sight or path, tamable, uncontrolled, owner cap, subdual, and no fresh damage since the attempt started. In this static pass they hold. Because this second attempt has four ticks, count `2` is still not the final branch.

So I whisper `Easy...easy...` and learn another tiny, humiliating lesson. The passive `Druidism` check is still almost impossible, about `0.63%` at `0.8` skill after the shard widens the max to `126.0`. I miss it. Missing still trains under the under-10 rule, so `Druidism` rises from `0.8` to `1.0`. No stat rises. The fox remains wild, uncontrolled, unowned, unfollowing, unbonded, and outside my follower slots.

Mechanical friction learned:

- A second non-final tick is still only maintenance, speech, and passive `Druidism`.
- The active timer is now `2 / 4` ticks complete; `m_BeingTamed` still points from the fox to Mira Vale.
- `NextSkillTime` still does not reset on a non-final tick.
- Under-10 passive skill gain keeps paying out even when the `Druidism` success roll fails.
- No movement, hunger, thirst, Taming value, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The fox is still not mine, but the timer still has control. The honest next move is to stand still through the third tick unless the visible grass changes first.

## Run 57 - Third Whisper, Still No Master

I stand still again at `Point3D(1210,1326,0)`, because the active taming timer is the only thing on screen that can honestly change before I move. The live snapshot still shows the same open Sosarian grass: twelve visible saved animals and zero visible saved items. The fox is still at `1209,1324`, dx `1`, dy `2`. The old horse is west of me, the llama and eagle are close, and the grey wolf is still one tile outside the client rectangle. No gump, context menu, corpse, chest, road, water source, shelter, trainer, or named region appears.

Three seconds pass. The timer count moves from `2` to `3`. The maintenance gates still have to hold first: range `6`, alive state, visibility, line of sight or pathing, tamability, uncontrolled state, owner cap, subdual rule, and no fresh damage since the attempt started. In this static pass they hold. Because this second attempt was simulated with four ticks, count `3` is still not the final branch.

So I reveal myself again and say `Don't be afraid...` to a fox that has heard enough from me. The passive `Druidism` check is still almost nothing, about `0.79%` at `1.0` skill after the shard widens the max to `126.0`, and I miss it. The under-10 gain rule still pays out. This tick raises `Druidism` from `1.0` to `1.4`. No stat rises. The fox remains wild, uncontrolled, unowned, unfollowing, unbonded, and outside my follower slots.

Mechanical friction learned:

- A third non-final tick is still only maintenance, speech, and passive `Druidism`; the final `Taming` roll has not happened.
- The active second timer is now `3 / 4` ticks complete; `m_BeingTamed` still points from the fox to Mira Vale.
- `NextSkillTime` still does not reset on a non-final tick.
- Under-10 passive skill gain can keep advancing even when the check itself fails.
- No movement, hunger, thirst, Taming value, inventory, quest, discovery, Lodoria, Savage, tarot, PvP/PvE, combat damage, pet, follower, ownership, equipment, or social state changed.

Next pressure:

The next timer tick is finally the real branch. The honest next move is to hold still through the fourth tick and let the shard decide whether this gamble becomes a pet or another failure message.

## Run 58 - The Fox Finally Yields

I hold still one more time.

The screen does not change before the timer does. I am still at `Point3D(1210,1326,0)` in open Sosarian grass, with the same twelve saved animals in the client rectangle and zero visible saved items. The fox is still close at `1209,1324`, dx `1`, dy `2`. The llama and eagle are close enough to make the screen feel crowded, the old horse is still west, and the grey wolf is still just outside the update range. No gump, context menu, water source, road, shelter, trainer, corpse, chest, or named region appears.

Three more seconds pass. This time `m_Count` reaches `m_MaxCount`, so the timer stops being background pressure and resolves the tame. The same maintenance gates still have to hold first: range `6`, alive state, visibility, line of sight or pathing, tamability, uncontrolled state, owner cap, subdual rule, and no fresh damage since the attempt started. In this static pass they all hold. The code reveals me, resets my skill timer, and removes the fox from `m_BeingTamed` before it rolls anything useful.

The passive Druidism check fires one last time first. At `1.4`, the success chance is still only about `1.11%` after the shard widens the max to `126.0`, so I treat that check as another miss. Under-10 gain still pays out, raising `Druidism` from `1.4` to `1.7`. Then the real `Taming` roll happens. With the fox's `MinTameSkill = -15.3` and my tiny Druidism modifier, the chance is only about `31.7%`. This time the roll lands.

The message changes from rejection to acceptance: `It seems to accept you as master.` The first-tame branch scales the fox's skills down to ninety percent, adds me to its owner list, and calls `SetControlMaster`. That property change is the real pet moment: my follower count goes from `0` to `1`, the fox becomes controlled, its order becomes `Come`, it is not bonded, and it is still standing at the same tile. The Taming check also trains under the under-10 rule, so `Taming` rises from `0.2` to `0.5`. Nothing about this creates food, water, a road, a trainer, a town, a tarot unlock, Lodoria, Savage, or safety.

Mechanical friction learned:

- A successful final taming tick is the first point where `Owners`, `ControlMaster`, `Controlled`, and follower accounting change.
- The timer removes `m_BeingTamed` and resets `NextSkillTime` before the final `Druidism` and `Taming` checks.
- First tame is not a free full-strength animal: the fox's skills are scaled to `0.90` because it was not paralyzed.
- A pet is not a solved survival loop. It follows and costs one follower slot, but I still have no visible water, shelter, road, vendor, trainer, or safe region.

Next pressure:

I have a fox now, not a plan. The next honest move is to take stock of the new follower and start moving again for water, a road, shelter, or a trainer without assuming the pet can save me from the next wolf.

## Run 59 - I Tell the Fox to Follow

I do not sprint off just because the fox accepted me. I am still at `Point3D(1210,1326,0)` in the same open Sosarian grass. The screen still shows twelve saved animals and zero visible saved items. The fox is close at `1209,1324`, dx `1`, dy `2`, now mine by the state I just earned. The llama and eagle are close enough to distract me, the old horse is still west, and the grey wolf is still just outside the update rectangle. There is still no water source, road, shelter, trainer, chest, corpse, gump, context menu, or named region.

So I use the one normal thing a new pet owner knows: I say `all follow me`.

That is not a forged command. It goes through ordinary speech. Regular speech has range `15`, and the fox is within both speech range and its perception range. The fox is controlled, commandable, alive, and owned by me, so its AI listens. The client keyword for `all follow me` reaches the controlled-creature speech handler, and the command does not need a target cursor. The fox's control chance is deterministic here because `Fox.MinTameSkill = -15.3`, and the control code returns `1.0` for creatures at or below `29.1` minimum taming skill.

The result is smaller than comfort but better than guessing. The fox's `ControlTarget` becomes me, and its `ControlOrder` changes from `Come` to `Follow`. The order-change hook reveals me, sets the fox to active speed, plays its idle sound, clears warmode, and clears combat. It does not move the fox yet. I have not advanced an AI tick, and I have not watched pathing prove that it can keep up through terrain, other animals, or a surprise hostile. It is a follower ordered to follow, not a shield, not a map, and not a canteen.

Mechanical friction learned:

- Pet commands are normal speech heard by nearby controlled creatures, not gumps and not context-menu ButtonIDs.
- A controlled creature's AI only listens when it is commandable and the speaker is its owner or pet friend.
- `all follow me` sets `ControlTarget` to the speaker and `ControlOrder` to `Follow`; plain `all follow` would instead open a target-picking flow.
- Fox command obedience is deterministic at this point because `GetControlChance` returns `1.0` when `MinTameSkill <= 29.1`.
- The command changes order state only. No AI movement tick, location change, hunger/thirst change, inventory use, skill gain, quest/discovery/tarot trigger, combat damage, or PvP/PvE change ran.

Next pressure:

Now the fox is actually ordered to follow me. The next honest move is movement again: leave the crowded animal patch and keep searching for water, a road, shelter, or a trainer while treating the fox's pathing and usefulness as unresolved until I see it happen.

## Run 60 - The Map Reader Lied

I start at `Point3D(1210,1326,0)` with no gump open, no context menu open, and a fox that is finally mine but only ordered to follow. The screen is still animals and empty grass: saved animals around me, zero visible saved items, no trough, no barrel, no road, no sign, no shelter, no trainer. The old grey wolf is still just off the north edge of the client rectangle.

Before I take the first southeast step, I catch the bad assumption that made the last note nonsense. Sosaria is registered to file index `1`, so yes, the files are `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`. But the server does not read those block files in the row-major order I used. `TileMatrix.ReadLandBlock` seeks by `((blockX * blockHeight) + blockY) * 196 + 4`. With that order, the ground under me is not secret water at all. `1210,1326` is land `0x3`, z `0`, zero statics. The attempted southeast tile `1211,1327` is land `0x6`, z `0`, zero statics. The nearby northwest route tiles are grass too. The water was my reader looking at the wrong block.

I also have a second layer of player knowledge now: the client world map has marker CSVs. They are not things standing on the screen, and they do not grant discovery, but a normal player can open the map and see them. From here the nearest Sosaria overlay lure is `Ruins` at `1303,1449`, then `Mines of Morinia` at `1022,1372`, then `Chamber of Bane` at `1427,1555`; the `West` moongate marker is farther away at `1052,1570`. That changes orientation, not immediate click targets.

Mechanical friction learned:

- The blocked southeast step was invalid automation output. No real `Mobile.Move`, `CheckMovement`, `OnMoveOver`, `Region.CanMove`, `OnAfterMove`, hunger tick, item use, discovery, or pet follow AI path ran from it.
- Future binary terrain scans must use the server's `TileMatrix` block order, not row-major block order.
- World-map markers are available player navigation knowledge, but they are not screen-visible mobiles/items and do not prove anything inside the 18-tile update box.
- The fox remains at `1209,1324,0`, controlled by me, ordered to `Follow`, and still untested as an actual moving companion.

Next pressure:

The first southeast step is back on the table because the tile is grass, not water. The next honest move is to scan the same animal-crowded screen, remember that the world map points toward distant named places, and then take one legal movement step only after the live entities and the corrected terrain agree.

## Run 61 - My Feet Remember Facing

I start exactly where the audit left me: `Point3D(1210,1326,0)` on Sosaria grass, no gump, no context menu, no target cursor, no water source, no chest, no corpse, no sign, and no named region. The screen is still crowded with saved animals and empty of saved items. The fox at `1209,1324,0` is mine now and ordered to `Follow`, but it has not moved yet. The old grey wolf is still outside the 18-tile client rectangle by y-distance, so it is not a click target.

The corrected terrain says the southeast tile at `1211,1327,0` is valid grass with zero statics. So I try to walk southeast.

The shard catches me on a smaller truth than terrain: I was still facing northwest from the last real walk toward the fox. `Mobile.Move` only enters the real movement block when my current direction already matches the requested direction. This first southeast input changes my facing from `Direction.Up` to `Direction.Down`; it does not call `CheckMovement`, does not test the grass tile, does not run `OnMoveOver`, and does not ask `Region.CanMove`. My location stays `1210,1326,0`.

Nearby creatures still get a movement notification pass with the old location equal to the same tile, but that does not prove pet pathing. The fox stays at `1209,1324,0`, still controlled, still ordered to `Follow`, and still untested as a moving companion. The next southeast input is the one that can finally step onto the corrected grass tile.

Mechanical friction learned:

- A normal movement key can be only a facing change if the character is not already facing that direction.
- The corrected `map1.mul` result is still useful, but it has not been consumed by a full movement step yet.
- Turning in place does not trigger item use, discovery, hunger/thirst changes, combat, skill gain, or pet follow movement.
- `Map.GetObjectsInRange` movement notifications can still fire after the packet, but this run produced no player-facing effect from them.

Next pressure:

I am now facing southeast. The honest next move is to press southeast again and let the full movement path test the grass, blockers, region, and pet follow pressure.

## Run 62 - The Second Press Actually Moves

I start at `Point3D(1210,1326,0)`, already facing southeast. The screen is still not a road, a camp, or a town. The live snapshot gives me animals and empty grass: eagles, goats, deer, a swallow, a llama, my fox at `1209,1324`, the old horse to the west, and no saved world items. The grey wolf is still north of the screen, outside the client rectangle. The world map can tempt me with `Ruins` at `1303,1449`, `Mines of Morinia` at `1022,1372`, `Chamber of Bane` at `1427,1555`, and the west moongate at `1052,1570`, but those are overlay lures, not things I can click from the grass.

I press southeast again.

This time `Mobile.Move` does not stop at facing. My current direction already matches `Direction.Down`, so the real movement block runs. The forward tile `1211,1327`, and the two diagonal side-check tiles `1211,1326` and `1210,1327`, are all dry grass at z `0` with zero statics. There is no mobile or item standing on the destination tile in the live snapshot, and the traced region lookup still finds no named region to block the step. I move one tile to `Point3D(1211,1327,0)`.

The step is progress, not rescue. The client rectangle shifts just enough to pull one more eagle onto the southeast-facing screen at `1229,1309`, while the grey wolf remains outside by y-distance `20`. My fox does not magically path on the same player movement call. Nearby mobiles receive `OnMovement`, but `BaseCreature.OnMovement` is not `BaseAI.Obey`; the controlled fox stays at `1209,1324`, still ordered to `Follow`, now dx `2`, dy `3`, waiting for a real AI tick to prove it can keep up.

Mechanical friction learned:

- A second movement input in the same direction is the one that enters `CheckMovement`; the first one only turned me.
- Diagonal movement checks the forward tile and the adjacent side tiles before letting a player cut the corner. All three were passable grass with zero statics here.
- Moving one tile shifts the visible rectangle. It revealed a corner eagle, but it did not reveal water, a road, a sign, a corpse, a chest, a trainer, or a named region.
- Player movement notifications do not equal pet follow pathing. The fox still needs an AI `Obey`/`DoOrderFollow` tick before I can treat it as physically following.

Next pressure:

I am one tile southeast, still in open grass, with a follower behind me and no survival infrastructure in sight. The next honest move is to keep walking toward some navigable landmark while watching whether the fox actually moves on its own tick.

## Run 63 - The Fox Thinks Close Enough Counts

I stop for a beat at `Point3D(1211,1327,0)` instead of pretending my new pet is already useful. The screen is still crowded grass: eagles, goats, deer, a swallow, a llama, the old horse, and my fox northwest at `1209,1324`. There are still zero visible saved items in range. No water source, road, sign, shelter, trainer, corpse, chest, gump, context menu, or target cursor appears. The grey wolf is still just outside the client rectangle by y-distance `20`, which is far enough to worry about but not far enough to click.

Then the fox gets its turn.

The tick is real server behavior, but it is not heroic. The AI timer calls `OnThink`, sees the fox is controlled, and routes into `Obey`. Since the order is `Follow`, it reaches `DoOrderFollow` and measures the distance to me as a floored `3` tiles. The shard has `FriendsAvoidHeels` on, so the follow code does not blindly stack the pet under my boots. I simulate that random spacing branch as `FollowersMax = 7`, which makes the fox want to stay `2` to `3` tiles away. It is already at the outer edge of that band.

So nothing moves. `WalkMobileRange` returns true before `DoMove` or pathfinding runs. The fox stays at `Point3D(1209,1324,0)`, still controlled, still ordered to `Follow`, still targeting me, and now carrying that random heel-spacing state. I saw the command become AI logic, but I did not see pathing. A pet that thinks it is close enough is not the same as a pet that has proven it can follow me through wilderness.

Mechanical friction learned:

- Controlled creature movement is timer-driven: `AITimer.OnTick` calls `OnThink`, then controlled mobiles use `Obey` instead of the uncontrolled `Think` path.
- `OrderType.Follow` reaches `DoOrderFollow`, which only calls `WalkMobileRange` when the control target is valid and inside perception range.
- With `FriendsAvoidHeels` enabled, `WalkMobileRange` mutates the pet's spacing target through `Utility.RandomMinMax(6,9)` instead of forcing heel-stacking.
- This simulated spacing roll chose `7`, so the desired range became `2..3`; the fox's floored distance to me was already `3`, so no `DoMove`, `PathFollower`, terrain check, combat, skill, hunger, thirst, inventory, quest, discovery, or follower-count change ran.
- The fox is still mine, but its actual terrain-following ability remains unproven until I move far enough that the spacing band forces a step.

Next pressure:

Waiting taught me why the fox did not close the gap. The honest next move is to keep walking toward a map-overlay landmark, probably the `Ruins` southeast of here, while treating the fox as a follower that may lag until the AI decides distance matters.

## Run 64 - I Make the Fox Care About Distance

I do not touch the llama, the eagle, or the old horse. They are on the screen, but none of them is food, water, shelter, a trainer, a sign, a chest, or a safe region. There is no open gump, no context menu, no target cursor, and no active taming timer. The world map is still only an overlay lure: `Ruins` sits southeast at `1303,1449`, closer than the other names, but it is not a thing I can click from this grass.

So I keep moving.

I am already facing southeast, which matters because the last pair of runs taught me how easily a movement key can be only a turn. This time `Mobile.Move` enters the real movement block immediately. The server-order terrain check uses Sosaria's file index `1`, not a guessed map file, and `GetLandTile` indexes the block by local y first. The forward tile `1212,1328` is grass at z `0` with zero statics. The diagonal side-check tiles `1212,1327` and `1211,1328` are also grass at z `0` with zero statics. No saved mobile or visible saved item occupies the destination tile, and the region file still gives me no named rectangle here.

The step lands. I move from `Point3D(1211,1327,0)` to `Point3D(1212,1328,0)`. The screen shifts by one tile and gets a little less busy: the far corner eagle at `1229,1309` falls out of the 18-tile rectangle, leaving twelve visible saved animals and still zero visible saved items. The grey wolf remains outside the client rectangle by y-distance `21`. My fox does not move on this call. It is still at `1209,1324`, still controlled, still ordered to `Follow`, but the gap is now dx `3`, dy `4`, a floored distance of `5`.

That last number is the point of the step. The previous AI tick decided distance `3` was close enough because the heel-spacing roll wanted `2..3`. After this movement, the fox is outside that band. I have not advanced the next AI tick yet, so I cannot claim it pathfinds, steps, teleports, fights, or catches up. I have only made the next tick interesting.

Mechanical friction learned:

- A movement input while already facing `Direction.Down` runs the full `Mobile.Move` path instead of stopping at a facing change.
- Diagonal player movement still has to pass the forward tile and both side tiles; this step passed because all three decoded as dry grass with zero statics.
- The client-range scan after the step has twelve visible saved animals and no visible saved items, so there is still no immediate water, road, sign, shelter, trainer, corpse, chest, or gump to distract me.
- Player movement alone does not move the controlled fox. It only increases the follow pressure; the next `BaseAI.Obey` / `DoOrderFollow` tick is where physical pet pathing has to prove itself.

Next pressure:

The fox is now far enough away that its own spacing logic should no longer be satisfied. The honest next move is to pause for the next follow AI tick and see whether the pet walks, pathfinds, or fails to keep up.

## Run 65 - The Fox Finally Takes a Step

I stop at `Point3D(1212,1328,0)` and let the fox think. The screen is the same open grass, which is good and bad: twelve visible saved animals, zero visible saved items, no water, no road, no sign, no corpse, no chest, no shelter, no trainer, no gump, no context menu, and no target cursor. The llama and eagle are still close enough to be tempting, and the old horse is still west. My fox is the thing that matters now, because it is mine, ordered to `Follow`, and sitting behind me at `1209,1324`.

The AI tick does not read like magic. `AITimer.OnTick` calls the fox's `OnThink`, then controlled state routes it into `Obey`. `OrderType.Follow` reaches `DoOrderFollow`, which measures the distance to me as a floored `5`. The earlier `FriendsAvoidHeels` roll is still `7`, so the fox wants to be `2..3` tiles away. Five is too far.

So the fox tries the ordinary step: `Direction.Down`, southeast, toward me. The server-order `map1.mul` check says the start tile `1209,1324`, target tile `1210,1325`, and both diagonal side tiles are grass with zero statics. Because the mover is a creature, the diagonal rule only needs one side to pass, and both do. No saved mobile or item is standing on the target tile, and no named region block appears. The fox moves one tile to `Point3D(1210,1325,0)`.

That is useful, but it is not a promise. I have proven one pet follow step on open grass. I have not proven pathfinding through blockers, hostile aggro response, guarding, attacking, body-blocking, or that the fox will save me from the next wolf. My own location, hunger, thirst, inventory, skills, discovery flags, PvP/PvE state, ownership, and follower count do not change.

Mechanical friction learned:

- Player movement and pet movement are separate clocks. The fox only moved when its AI timer advanced.
- `FriendsAvoidHeels` can intentionally keep a pet off my heels; with spacing `7`, the desired band is `2..3` tiles.
- Once the fox was at floored distance `5`, `WalkMobileRange` called `DoMove` instead of returning early.
- A controlled creature's diagonal movement is less strict than a player's corner cut: at least one side tile must pass, not both.
- One clean grass step is not a general pet-pathing guarantee.

Next pressure:

The fox is close again. I can keep walking toward the map-overlay lure at `Ruins`, but every few steps I need to check whether the pet actually keeps up and whether the empty grass turns into water, a road, shelter, or danger.

## Run 66 - I Walk, the Fox Does Not

I start at `Point3D(1212,1328,0)`, facing southeast, with no gump, no context menu, no target cursor, and no active taming timer. The screen is still mostly animals and nothing useful: goats, eagles, deer, a swallow, a llama, the old horse, my fox, and no saved world items. The world map keeps whispering `Ruins` southeast at `1303,1449`, but that marker is a map overlay, not a road under my boots and not a thing I can click.

So I take one more southeast step.

This time there is no facing trick. I am already facing `Direction.Down`, so `Mobile.Move` goes into the real movement path. The server-order map read uses Sosaria's file index `1`: the forward tile `1213,1329` is grass, and the two diagonal side-check tiles `1213,1328` and `1212,1329` are grass too. All three have zero statics. The live snapshot has no mobile or item standing on the destination, and the region scan finds no named rectangle to stop me. I move to `Point3D(1213,1329,0)`.

The fox does not move with the keypress. That is the lesson, again. Player movement sends nearby movement notifications, but it is not a pet AI tick. The fox stays at `Point3D(1210,1325,0)`, still controlled, still ordered to `Follow`, and now back at a floored distance of `5` from me. The northern edge eagle drops out of the client rectangle, leaving eleven visible saved animals and still zero visible saved items. No hunger, thirst, combat, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, or follower count changes.

Mechanical friction learned:

- A movement input while already facing `Direction.Down` runs the full player movement path.
- Player diagonal movement requires the forward tile and both side-check tiles to pass; this step passed on grass with zero statics.
- The world-map `Ruins` marker is useful direction, not in-world proof of shelter, safety, or discovery.
- A controlled follower does not move during my movement call. The next honest check is a future `BaseAI.Obey` / `DoOrderFollow` tick.

Next pressure:

The fox is lagging again by exactly enough for the heel-spacing code to care. I can keep walking toward the marker, but the clean next thing to learn is whether the fox takes another follow step or starts failing behind me.

## Run 67 - The Fox Keeps Its Distance

I stay at `Point3D(1213,1329,0)` and let the pet clock tick instead of mashing southeast again. The screen is still all animal shapes and no supplies: two goats, three eagles, two deer, a swallow, a llama, the old horse, and my fox. There are still zero visible saved items, no canteen fill target, no road, no sign, no corpse, no chest, no gump, no context menu, and no target cursor. The grey wolf is still beyond the update rectangle. The `Ruins` marker on the world map keeps giving me a direction, but it is not an entity on the grass.

The fox thinks the same way it did before. `AITimer.OnTick` calls `OnThink`, the controlled state routes into `Obey`, and the `Follow` order reaches `DoOrderFollow`. My last step put the fox at a floored distance of `5`, and its remembered `FriendsAvoidHeels` spacing still wants `2..3`. Five is too far, so it tries to close the gap.

It takes the plain southeast step: from `Point3D(1210,1325,0)` to `Point3D(1211,1326,0)`. The server-order `map1.mul` read says the start tile, target tile, and both diagonal side tiles are grass at z `0` with zero statics. Because the mover is a creature, one clear side tile would have been enough; both are clear. No saved mobile or item occupies the target, and no named region block appears. The fox moves, ending at dx `2`, dy `3`, floored distance `3` from me.

That is two successful follow steps on open grass, not proof of protection. The fox did not attack, guard, body-block, find water, reveal a road, trigger a discovery flag, or make the wilderness safe. It just kept itself in the follow band.

Mechanical friction learned:

- Pet movement is still timer-driven; waiting advanced `BaseAI.Obey` / `DoOrderFollow`, while my character did not move.
- The `FriendsAvoidHeels` spacing value stayed at `7`, so the fox still wanted to hover `2..3` tiles away instead of stacking on me.
- A second clean pet step confirms the local grass is easy terrain, but only for this exact tile and moment.
- No hunger, thirst, combat, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, follower count, gump, or context-menu state changed.

Next pressure:

The fox is close again. The next honest move is to keep walking southeast toward the map-marker lure while continuing to treat the empty grass, hidden spawner pressure, and future pet pathing as unresolved.

## Run 68 - Another Step, Another Lag

I start at `Point3D(1213,1329,0)`, facing southeast, with no gump, no context menu, no target cursor, and no active taming timer. The screen is still wildlife instead of help: goats, eagles, deer, a swallow, a llama, the old horse, and my fox. There are still zero visible saved items. No road, water, chest, corpse, sign, shelter, trainer, or named region appears. The `Ruins` marker is southeast on the client world map, but it is still just a navigation lure.

So I press southeast again.

This is a real movement input because I am already facing `Direction.Down`. `Mobile.Move` enters `CheckMovement`; the server-order `map1.mul` read puts the forward tile `1214,1330` on grass, and both diagonal side-check tiles `1214,1329` and `1213,1330` are grass too. All three have zero statics. The live snapshot has no mobile or item standing on the destination tile, and the region scan still gives me no named rectangle to block the step. I move one tile to `Point3D(1214,1330,0)`.

The fox does not move with me. It stays at `Point3D(1211,1326,0)`, still controlled, still ordered to `Follow`, and now back at a floored distance of `5`. That means the next pet AI tick should care again, but this step did not run `BaseAI.Obey`, did not prove pathfinding, and did not make the animal protect me. The screen stays empty of supplies, and the hidden local spawner pressure remains only pressure until a saved mobile enters the update rectangle.

Mechanical friction learned:

- A normal southeast input while already facing southeast consumes the full player movement path.
- Diagonal player movement still needs the forward tile and both side tiles to pass; this step passed on grass with zero statics.
- Moving one tile does not advance the controlled fox's follow AI. It only stretches the spacing band again.
- No hunger, thirst, combat, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, follower count, gump, or context-menu state changed.

Next pressure:

The fox is lagging by enough for the follow code to react. The clean next move is to pause for another pet AI tick, unless a live refresh puts a hostile or useful object on the screen first.

## Run 69 - The Fox Catches the Band Again

I do not keep walking just because the world map says `Ruins` is southeast. I stop at `Point3D(1214,1330,0)` and look at the actual screen first. The saved live slice is still a mess of animals and empty grass: goats, eagles, deer, a swallow, a llama, the old horse, and my fox. There are still zero visible saved items, no water source, no road, no sign, no corpse, no chest, no shelter, no trainer, no gump, no context menu, and no target cursor. The old grey wolf is still north of the client rectangle by y-distance `23`, which makes it pressure rather than a legal click.

The only thing that has a clean turn is the fox. It is behind me at `Point3D(1211,1326,0)`, still controlled, still ordered to `Follow`, and still carrying the `FriendsAvoidHeels` spacing value `7`. My last player step stretched it back to floored distance `5`, outside the `2..3` band.

So I wait for the pet AI tick.

The tick goes through the same ordinary chain: `AITimer.OnTick`, `OnThink`, controlled `Obey`, `DoOrderFollow`, then `WalkMobileRange`. The target is valid and close enough to perceive, so there is no teleport-to-master shortcut. Five tiles is too far for the remembered spacing band, so `WalkMobileRange` asks for `Direction.Down`, southeast toward me.

The third real follow step is still just grass. The fox moves from `Point3D(1211,1326,0)` to `Point3D(1212,1327,0)`. Server-order `map1.mul` reads the start, target, and both diagonal side tiles as grass at z `0`, and `staidx1.mul` / `statics1.mul` find zero statics on all four tiles. No saved mobile or item occupies the target tile. Because the mover is a creature, one clear diagonal side would have been enough; both sides are clear. Mira does not move. The fox ends at dx `2`, dy `3`, floored distance `3`, exactly back in the follow band.

Mechanical friction learned:

- Waiting can be a real action when the moving object is a controlled pet on an AI timer.
- `Follow` does not mean heel-stacking here. `FriendsAvoidHeels` keeps reusing the fox's spacing value, so the pet moves only when I stretch beyond `2..3` tiles.
- Three clean pet steps prove this small patch of grass, not future pathfinding, guarding, combat help, or safety from hidden spawner pressure.
- No hunger, thirst, combat, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, follower count, gump, context-menu, or player location state changed.

Next pressure:

The fox is close again, and the screen still has nothing useful except animal bodies and distant map-marker lures. The honest next move is to take another careful southeast step toward `Ruins`, then check whether the pet falls behind again.

## Run 70 - My Step Makes the Fox Late Again

I start at `Point3D(1214,1330,0)`, facing southeast, with the fox close behind at `Point3D(1212,1327,0)`. The screen still looks like open grass and animal bodies: two goats, three eagles, two deer, a swallow, a llama, the old horse at the far west edge, and my fox. There are still no visible saved world items, no water source, no chest, no corpse, no road, no sign, no trainer, no shelter, no open gump, no context menu, and no target cursor. The grey wolf is still outside the client rectangle, and the `Ruins` marker remains only a world-map direction southeast.

So I press southeast again.

This is a real step because I am already facing `Direction.Down`. `Mobile.Move` enters `CheckMovement`; the forward tile `1215,1331` is grass at z `0`, and the diagonal side-check tiles `1215,1330` and `1214,1331` are grass too. All three have zero statics. The live snapshot has no saved mobile or item standing on `1215,1331`, and the region scan still finds no named rectangle to stop me. I move to `Point3D(1215,1331,0)`.

The fox does not move with the keypress. It stays at `Point3D(1212,1327,0)`, still controlled, still ordered to `Follow`, and now stretched back to dx `3`, dy `4`, floored distance `5`. The shifted client rectangle loses the far north goat, the far west eagle, and the old horse, leaving eight visible saved animals and still zero visible saved items. That smaller screen is not safer; it is just less crowded. No pet AI tick ran, so I cannot claim the fox pathfound, guarded, attacked, or protected me.

Mechanical friction learned:

- Repeating southeast while already facing southeast consumes the real player movement path.
- Diagonal player movement still requires the forward tile and both side tiles; this step passed on map1 grass with zero statics.
- A player movement step shifts the client rectangle and can drop visible creatures off-screen without despawning them.
- The controlled fox still moves on its own AI timer, not during my movement call, so the next honest pressure is another follow tick.
- No hunger, thirst, combat, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, follower count, gump, context-menu, or pet-order state changed.

Next pressure:

The fox is lagging by the same `2..3` heel-spacing rule that has been driving this little march. If nothing hostile or useful appears first, the clean next move is to pause again and see whether the fox takes the next southeast follow step.

## Run 71 - The Fox Steps Back Into the Band

I stay at `Point3D(1215,1331,0)` and look before I do anything clever. The live slice around me still has only animals and empty grass: a goat, two eagles, two deer, a swallow, a llama, and my fox. There are zero visible saved items, no water source, no road, no sign, no corpse, no chest, no shelter, no trainer, no open gump, no context menu, and no target cursor. The `Ruins` marker is still southeast on the world map, but the only thing on-screen with a real pending clock is the fox behind me.

So I wait for the pet AI tick.

The code does not teleport it or make it guard me. `AITimer.OnTick` calls the fox's `OnThink`, sees it is controlled, and sends it through `Obey`. `Follow` reaches `DoOrderFollow`; the target is still me, still in perception range, and the remembered `FriendsAvoidHeels` spacing value is still `7`, so the desired band is still `2..3` tiles. The fox is at dx `3`, dy `4`, floored distance `5`. That is too far.

`WalkMobileRange` chooses `Direction.Down`, and this time the ordinary step passes. Server-order `map1.mul` reads the fox's start tile `1212,1327`, forward tile `1213,1328`, and both diagonal side tiles as dry grass at z `0`; `staidx1.mul` and `statics1.mul` find zero statics on all four. No saved visible mobile or item occupies `1213,1328`, and no named region rectangle catches it. The fox moves one tile southeast to `Point3D(1213,1328,0)`, ending at dx `2`, dy `3`, floored distance `3`.

That is the fourth clean follow step on this grass. It still is not protection, pathfinding through blockers, combat help, or proof that the next hidden spawner pressure is safe. Mira does not move. No hunger, thirst, combat, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, follower count, gump, context-menu, or pet-order state changes.

Mechanical friction learned:

- Waiting can be a real chronological action when a controlled pet has an AI timer ready.
- `Follow` with `FriendsAvoidHeels` keeps the fox near me but deliberately off my heels; distance `5` triggers movement, distance `3` satisfies the band.
- A controlled creature's diagonal move still uses normal terrain and move-over checks, but non-player diagonal movement only needs one adjacent side tile to pass. Both side tiles passed here.
- Four grass steps are still local evidence, not a general safety guarantee.

Next pressure:

The fox is close again and the screen still has no supplies. If no hostile or useful entity appears first, the next honest move is another cautious southeast step toward the `Ruins` overlay marker.

## Run 72 - The Grass Lets Me Walk, Then Shows Teeth

I start at `Point3D(1215,1331,0)`, already facing southeast, with the fox close again at `Point3D(1213,1328,0)`. The screen is still mostly empty grass with animal bodies scattered through it: a goat and eagle at the north edge, two deer, a swallow, a llama, another eagle, and my fox. There are zero visible saved items. No water source, road, sign, chest, corpse, trainer, shelter, gump, context menu, or target cursor appears. The `Ruins` marker still sits far southeast on the world-map overlay, but the overlay is not a walkable road or an in-world object.

So I press southeast.

Because I am already facing `Direction.Down`, this input enters the real `Mobile.Move` path. The forward tile `1216,1332` is dry grass at z `0`; the diagonal side-check tiles `1216,1331` and `1215,1332` are also grass, with zero statics on all three. Since I am a player, the diagonal rule has to pass both side tiles, not just one. It does. The live snapshot has no saved mobile or item standing on `1216,1332`, and the traced region lookup still has no named rectangle here. I move to `Point3D(1216,1332,0)`.

The fox does not move with the keypress. It stays at `Point3D(1213,1328,0)`, still controlled, still ordered to `Follow`, and now stretched back to dx `3`, dy `4`, floored distance `5`. That means the next pet AI tick should care again, but it has not happened yet.

The shifted screen matters more than the tile. The north-edge goat and eagle fall away, but a new shape appears at the far east edge: a saved `SwampDrakeRiding` at `Point3D(1234,1319,0)`, visible as a swamp drake at dx `18`, dy `13`. I do not know from the client whether that means opportunity, danger, or just another animal I cannot handle. I only know it is now on-screen and it is much more attention-grabbing than empty grass. No combat, skill, item use, hunger, thirst, quest, discovery, PvP/PvE, ownership, follower count, gump, or context-menu state changes.

Mechanical friction learned:

- A repeated southeast input while already facing southeast runs `Mobile.Move`, not just a facing turn.
- Player diagonal movement still requires the forward tile and both adjacent side-check tiles to pass; this step passed on map1 grass with zero statics.
- Player movement does not advance the controlled fox's follow AI, so the fox is lagging again outside the `2..3` heel-spacing band.
- The update-range rectangle can introduce new pressure without any spawn tick: after the step, the saved swamp drake at the far east edge becomes visible.

Next pressure:

The fox is behind me and the swamp drake has appeared at the edge of the screen. The next honest move is to stop walking blindly, acknowledge the drake, and decide whether to wait for the fox, inspect the drake, or detour before I get closer.

## Run 73 - The Label Is Not Safety

I stop at `Point3D(1216,1332,0)` instead of stepping closer. The screen is not empty anymore: two deer, a swallow, a llama, an eagle, my lagging fox, and the new shape at the far east edge. The fox is still behind me at `Point3D(1213,1328,0)`, controlled and ordered to `Follow`, but outside the `2..3` heel band again at floored distance `5`. There are still zero visible saved items, no water, no road, no chest, no corpse, no trainer, no shelter, no gump, no context menu, and no target cursor.

So I do the smallest normal-client inspection: I single-click the swamp drake.

That works only because the server's update range is inclusive. The drake is at `Point3D(1234,1319,0)`, which is dx `18`, dy `13` from me. `PacketHandlers.LookReq` finds mobile serial `28109`, checks that I can see it, and accepts `Utility.InUpdateRange` because both axes are still within `±18`. The region single-click hook does not block it, so `BaseCreature.OnSingleClick` runs. Because the drake is uncontrolled, it does not show a tame/bonded overhead line. The inherited `Mobile.OnSingleClick` sends exactly the visible label: `a swamp drake`.

The label is useful, but it is not a handshake. The class behind that label is not wildlife in the fox sense: `SwampDrakeRiding` is a `BaseMount` with melee AI, `FightMode.Closest`, poison breath, high stats, `Tamable = true`, `ControlSlots = 2`, and `MinTameSkill = 84.3`. None of that becomes mine or even becomes a visible menu from a single click. I do not attack it. I do not request a context menu. I do not start Taming. I do not move, and the fox does not get an AI tick.

Mechanical friction learned:

- The update-range edge is inclusive. A mobile at dx `18`, dy `13` can still be single-clicked.
- Single-clicking an uncontrolled creature only proves the overhead name label unless that creature's click path adds more visible text.
- The swamp drake is mechanically much more dangerous than the deer and fox class cluster, but the player action only revealed `a swamp drake`.
- No combat, aggro tick, pet follow tick, taming attempt, ownership change, follower change, skill gain, item use, hunger/thirst change, quest flag, discovery flag, gump, context menu, or movement happened.

Next pressure:

The drake is identified and still far enough away to avoid provoking with movement. The fox is the immediate loose end again: wait for the follow tick, then decide whether to retreat or steer around the drake instead of walking toward it.

## Run 74 - Let the Fox Catch Up Before I Flinch

I do not take another step toward the drake. I am still at `Point3D(1216,1332,0)`, facing southeast, with the swamp drake labeled at the far east edge of the screen. The live slice is still all bodies and no supplies: two deer, a swallow, a llama, an eagle, the identified swamp drake, and my controlled fox. There are zero visible saved items, no water source, no chest, no corpse, no road, no sign, no trainer, no shelter, no gump, no context menu, and no target cursor. The nearest world-map lure is still `Ruins`, but that is overlay knowledge, not a road under my boots.

The fox is the only thing with a clean clock. It is behind me at `Point3D(1213,1328,0)`, ordered to `Follow`, stretched out to dx `3`, dy `4`, floored distance `5`. That is outside the `FriendsAvoidHeels` band I have already watched the code enforce.

So I wait.

The tick runs the controlled path, not a new taming path: `AITimer.OnTick`, `OnThink`, `Obey`, `DoOrderFollow`, `WalkMobileRange`, then `DoMove`. The fox reuses its spacing value `7`, so it wants to sit `2..3` tiles from me. Distance `5` is too far, and `Direction.Down` points southeast toward me. The local grass cooperates again. Server-order `map1.mul` reads the fox start `1213,1328`, target `1214,1329`, and diagonal side tiles `1214,1328` and `1213,1329` as grass at z `0`; `staidx1.mul` and `statics1.mul` find zero statics on all four. No saved visible mobile or item occupies the target tile, and the Sosaria region scan finds no named rectangle there.

The fox steps to `Point3D(1214,1329,0)`. Mira does not move. The drake is still only an identified edge threat, not a target, pet, mount, fight, or discovery. The fox is now back in the spacing band at dx `2`, dy `3`, floored distance `3`.

Mechanical friction learned:

- Waiting can be the right move when a controlled pet is already ordered to `Follow` and lagging outside its spacing band.
- `FriendsAvoidHeels` is not cosmetic. With the fox's stored spacing value `7`, follow movement waits until the fox falls outside `2..3` tiles, then takes one normal movement step.
- A controlled creature's diagonal move still checks terrain and move-over blockers. This step passed because the forward and side tiles were open grass with zero statics.
- Catching the fox up does not prove guarding, attacking, body-blocking, aggro handling, or future pathfinding. It also does not test the swamp drake.
- No player movement, drake AI, combat, taming, skill gain, hunger/thirst change, item use, quest, discovery, PvP/PvE, ownership, follower count, gump, context menu, or target cursor changed.

Next pressure:

The fox is close again, and the drake is still on the far edge. The next honest move is to choose a careful direction away from the drake or deliberately risk another visible inspection step, but not to pretend the label made it safe.

## Run 75 - The First Flinch Is Only a Turn

I am still at `Point3D(1216,1332,0)`. The fox is close again at `Point3D(1214,1329,0)`, ordered to `Follow`, and the swamp drake is still labeled at the far east edge at `Point3D(1234,1319,0)`. The live scan has the same seven saved animals and no saved items: two deer, a swallow, a llama, an eagle, my fox, and the swamp drake. No water, road, chest, corpse, shelter, trainer, gump, context menu, or target cursor is on the screen.

I decide not to drift closer to the drake. I press southwest, trying to angle away from the east-edge threat while keeping the fox behind me. The code catches the embarrassing part: I was facing `Direction.Down`, so a southwest input is only a facing change to `Direction.Left`. `Mobile.Move` does not enter `CheckMovement`, does not test the southwest tile, and does not change my location. The client gets a movement acknowledgement, my facing changes, and the same nearby objects receive `OnMovement` notification with my old location still equal to my current tile.

That notification matters but it is not combat. The swamp drake class says uncontrolled drakes reacquire on movement, so its `OnMovement` can call `ForceReacquire`, setting its next reacquire time to ready. But no drake AI tick is advanced in this action, and the notice-sound branch does not fire because I was already inside its 18-tile range before the turn. I do not get bitten, breathed on, targeted, chased, or warned. I also do not move far enough to stretch the fox again.

Mechanical friction learned:

- A normal movement packet routes through `PacketHandlers.MovementReq` and `Mobile.Move`.
- Movement input is not always movement. If the masked current direction does not match the requested direction, `Mobile.Move` skips the terrain path and only sets direction.
- Turning still sends movement notifications to nearby mobiles and movement-aware items, so visible monsters can become primed for future AI even when my coordinates do not change.
- The swamp drake's `ReacquireOnMovement` is true only while it is uncontrolled. This turn can make its future AI reacquire check ready, but it does not itself run acquisition, set `Combatant`, move the drake, or use poison breath.
- No player location, fox location, hunger, thirst, combat damage, skill, inventory, quest, discovery, tarot, PvP/PvE, ownership, follower count, gump, context-menu, or target state changed.

Next pressure:

I am now facing southwest, still on the same grass tile. If I press southwest again, that should be the real retreat step, but now I have to respect that the drake may be ready to reacquire on its next AI pulse.

## Run 76 - The Second Flinch Actually Moves

I start exactly where the failed retreat left me: `Point3D(1216,1332,0)`, now facing southwest. The screen still has the far-edge swamp drake at `1234,1319,0`, two deer, a swallow, a llama, an eagle, and my fox behind me at `1214,1329,0`. There are still zero visible saved items, no water, no road, no sign, no chest, no corpse, no shelter, no trainer, no gump, no context menu, and no target cursor. The drake is identified and probably primed for a future reacquire check, but no AI tick is running inside my keypress.

So I press southwest again.

This time it is a real step. `Mobile.Move` sees that my current direction already matches `Direction.Left`, so it enters `CheckMovement`. The forward tile is `1215,1333`, and the corrected server-order map read puts it on grass at z `1`. The two diagonal side-check tiles are `1216,1333` and `1215,1332`; both are grass with zero statics. Because I am a player, both side tiles have to pass. They do. The live-state slice has no saved mobile or item standing on the destination tile, and the Sosaria region scan finds no named rectangle there.

I step to `Point3D(1215,1333,1)`. That single tile is enough to make the drake fall off the normal client rectangle: it is now dx `19`, dy `14`, so I cannot legally single-click it, request its context menu, or treat it as still on screen. That does not make it harmless. Run 75 already let its `OnMovement` path mark reacquire ready, and this movement notifies nearby mobiles again. It still does not advance the drake's AI, choose a combatant, move it, bite me, breathe poison, or play a notice sound.

The fox does not move with me. It stays at `Point3D(1214,1329,0)`, still controlled and ordered to `Follow`, now at dx `1`, dy `4`, floored distance `4`. That is outside the `2..3` heel-spacing band, so the next pet AI tick should care. The screen after the step has six visible saved animals and no visible saved items. The drake is now pressure behind the edge, not a visible target.

Mechanical friction learned:

- A second southwest input while already facing `Direction.Left` runs the full player movement path.
- `Direction.Left` moves one tile southwest, from `1216,1332` to `1215,1333`.
- The southwest target tile steps up to z `1`; `CheckMovement` accepts the land step because the forward tile and both diagonal side tiles are passable grass with zero statics.
- Leaving a monster's update rectangle is not the same as resolving its AI. Movement notifications can prime reacquire, but combat still waits on a later AI pulse.
- Player movement does not advance the controlled fox's `Follow` AI, so the pet is now stretched outside its current spacing band again.

Next pressure:

The immediate screen no longer includes the drake, but the fox is lagging and the drake may still be awake just off-screen. The honest next move is to pause for the fox's follow tick or keep retreating while accepting that pet pathing and hostile AI are unresolved.

## Run 77 - I Hold Still For One Pet Step

I stop at `Point3D(1215,1333,1)` instead of immediately spamming southwest. The screen is quieter than it feels: two deer, a swallow, a llama, an eagle, and my fox are visible, with zero saved world items, no water, no road, no sign, no chest, no corpse, no shelter, no trainer, no open gump, no context menu, and no target cursor. The swamp drake I labeled is one x-tile outside the client rectangle at `Point3D(1234,1319,0)`. That means I cannot legally click it now, but it is not erased from the world or from my nerves.

So I wait for the fox.

This is a timer-order bet, not a safety spell. In this simulated beat, the fox's `AITimer` is the next visible thing I let resolve, while the off-screen drake's future AI tick stays unresolved. The controlled path runs through `OnThink`, `Obey`, `DoOrderFollow`, and `WalkMobileRange`. The fox is still ordered to `Follow`, still targeting me, and still using the stored `FriendsAvoidHeels` spacing value `7`, so it wants to sit `2..3` tiles away. From `Point3D(1214,1329,0)` to me it is dx `1`, dy `4`, floored distance `4`, which is too far.

The direction calculation points it straight south, not southeast this time. `Direction.South` moves from `1214,1329` to `1214,1330`. Server-order `map1.mul` reads the start as grass tile `0x4` at z `0` and the target as grass tile `0x6` at z `0`; `staidx1.mul` and `statics1.mul` find zero statics on both tiles. The live snapshot has no saved visible mobile or item on the target tile, and no named Sosaria region catches the move. The fox steps to `Point3D(1214,1330,0)` and lands back in the spacing band at dx `1`, dy `3`, floored distance `3`.

Mira does not move. The drake does not get a simulated AI tick, combatant, bite, breath, notice sound, or movement. That is the uncomfortable part: catching the fox up improves my screen, but it does not prove the off-screen drake is harmless.

Mechanical friction learned:

- Waiting can advance a controlled pet without moving the player, but timer ordering is a real uncertainty.
- `GetDirectionTo` can change the follow step from diagonal to cardinal movement when the fox is mostly south of the target line.
- A straight `Direction.South` follow step checks only the forward tile path, not player-style diagonal side tiles.
- The fox following me is still not guarding, attacking, body-blocking, or solving hostile aggro.
- No player movement, drake AI, combat, taming, skill gain, hunger/thirst change, item use, quest, discovery, PvP/PvE, ownership, follower count, gump, context menu, or target cursor changed.

Next pressure:

The fox is close again, but the drake is still a primed off-screen threat. The next honest move is another cautious retreat or a deliberate scan step, not pretending the pet step made the field safe.

## Run 78 - One More Step Away From Teeth

I start at `Point3D(1215,1333,1)`, already facing southwest. The fox is close at `Point3D(1214,1330,0)`, controlled and ordered to `Follow`, but only close because I waited for it. The screen is still wilderness, not help: two deer, a swallow, a llama, an eagle, my fox, and zero saved world items. No water, road, sign, chest, corpse, trainer, shelter, gump, context menu, or target cursor appears. The swamp drake I labeled is still just off the east edge at `1234,1319`, outside my legal click range from here.

So I press southwest again.

Because I am already facing `Direction.Left`, this keypress is not another turn. `Mobile.Move` enters the actual movement block and asks `MovementImpl.CheckMovement` about the southwest tile. The forward tile is `1214,1334`; the adjacent player diagonal checks are `1215,1334` and `1214,1333`. The server-order `map1.mul` read makes the forward tile flat grass at z `0`, and both side tiles are grass with zero statics. The live-state slice has no saved mobile or visible item on the destination or side-check tiles, and no named Sosaria region catches the move. The step is allowed.

I move to `Point3D(1214,1334,0)`. That drops me another tile away from the drake: it is now dx `20`, dy `15`, still off-screen and still not resolved by AI. It also stretches the fox again. The fox does not move during my keypress; it stays at `Point3D(1214,1330,0)`, still ordered to `Follow`, now dx `0`, dy `4`, floored distance `4`. The shifted screen picks the old horse back up on the far west edge, giving seven visible saved animals and still no saved items. The drake is less clickable, not less real.

Mechanical friction learned:

- Repeating `Direction.Left` while already facing southwest consumes a real player movement step.
- `Direction.Left` moves from `1215,1333,1` to `1214,1334,0`; the z drops back to flat grass.
- Player diagonal movement again requires the forward tile and both adjacent side-check tiles to pass; all three passed with zero statics and no saved blocker.
- Player movement does not advance the fox's `Follow` AI, so the pet is now outside its `2..3` heel-spacing band again.
- Moving farther from a labeled hostile removes it from client range but does not execute or disprove its future AI, aggro, movement, melee, or breath.

Next pressure:

The drake is farther off-screen, but the fox is stretched again. The next honest move is to pause for the fox or keep retreating with the cost of leaving the pet lagging behind.

## Run 79 - I Wait Until the Fox Is Not a Trail

The written map already had me make the Run 78 southwest step, even though the persisted state lagged behind it. I accept that step as the real prior beat and start from `Point3D(1214,1334,0)`, facing southwest. The fox is behind me on the same x line at `Point3D(1214,1330,0)`, controlled, ordered to `Follow`, and stretched to floored distance `4`. The screen is still not help: two deer, a swallow, a llama, an eagle, the old horse at the far west edge, my fox, and zero saved world items. No water, road, sign, chest, corpse, trainer, shelter, gump, context menu, or target cursor appears. The swamp drake is now farther off-screen at dx `20`, dy `15`; I cannot legally click it, but I also have not made it harmless.

So I stop again and let the fox tick.

This is the same uncomfortable bargain as before. I am choosing the timer-order branch where the controlled fox's `AITimer` resolves before any off-screen drake AI. The fox is controlled, so the AI path goes through `Obey`, `DoOrderFollow`, and `WalkMobileRange` instead of a new taming or combat path. The stored `FriendsAvoidHeels` spacing value is still `7`, which means the desired band remains `2..3` tiles. Distance `4` is too far, so `WalkMobileRange` asks for movement toward me.

Because the fox and I share x `1214`, `GetDirectionTo` points it straight `Direction.South`. The server-order `map1.mul` read says the start tile `1214,1330` is grass `0x6` at z `0`, and the target tile `1214,1331` is grass `0x4` at z `0`; `staidx1.mul` and `statics1.mul` find zero statics on both. The live snapshot has no saved mobile or visible saved item on `1214,1331`, and the Sosaria region file has no named rectangle here. `Mobile.Move` accepts the pet step.

The fox moves to `Point3D(1214,1331,0)`. I do not move. The fox is close again at dx `0`, dy `3`, floored distance `3`, which satisfies the heel-spacing band. The old horse is still visible at the west edge and still not mine. The drake is still off-screen and unresolved. No drake AI, combat, breath, melee, notice sound, player damage, skill gain, item use, hunger/thirst change, quest, discovery, PvP/PvE state, ownership change, follower-count change, gump, context menu, or target cursor happens.

Mechanical friction learned:

- The map/state mismatch matters: Run 78 was already present in the map, so the next simulated state has to assimilate that movement before taking a new action.
- Waiting can again be a real player choice when the controlled fox is outside its `2..3` follow band.
- A straight south controlled-creature follow step only needed the forward tile to pass movement checks; the target tile was clear grass with zero statics and no saved blocker.
- Keeping the fox close is not the same as guarding, attacking, body-blocking, or solving the off-screen swamp drake.
- The visible horse returning at the west edge is only screen pressure. No click, context menu, taming attempt, or ownership path ran.

Next pressure:

The fox is close and the drake is farther off-screen, but I still have no water, road, shelter, or trainer. The honest next move is either another cautious retreat step away from the drake or a deliberate reorientation toward the `Ruins` marker while keeping the fox from stretching too far.

## Run 80 - Turning Back Toward the Map Lure Is Not Walking

I start at `Point3D(1214,1334,0)`, facing southwest, with no gump, no context menu, no target cursor, and no active taming timer. The screen is still more animal than help: two deer, a swallow, a llama, an eagle, the old failed horse at the west edge, and my controlled fox three tiles north. There are zero visible saved items, no water source, no road, no sign, no corpse, no chest, no trainer, and no shelter. The swamp drake I labeled earlier is not on the legal screen anymore; it sits at `1234,1319`, dx `20`, dy `15`, close enough to remember and too far to click.

The world-map overlay still says the nearest named thing is `Ruins` at `1303,1449`. That is southeast. I do not want to keep sliding southwest forever just because a drake scared me, so I press southeast once to face the marker again.

The keypress is not a step. `PacketHandlers.MovementReq` reads `Direction.Down` and calls `Mobile.Move`, but my current direction mask is still `Direction.Left`. Because those masks do not match, the movement block is skipped: no `CheckMovement`, no forward tile test, no diagonal side-check tiles, no `Region.CanMove`, no `InternalOnMove`, and no location change. The server still acknowledges the movement packet, calls `SetLocation` with the same point, and changes my facing to `Direction.Down`.

That quiet turn still matters. The same-location movement notification loop can call `OnMovement` on nearby mobiles. The uncontrolled swamp drake remains outside my update range, but its `ReacquireOnMovement` code path is still the reason I do not treat turning as harmless world time. In this simulated beat I do not advance any drake AI timer, so there is no combatant, breath, bite, notice sound, or movement. The fox also does not get a follow tick and stays at `Point3D(1214,1331,0)`, still inside its `2..3` heel-spacing band.

Mechanical friction learned:

- A direction input can be only a facing change even when it is the direction I intend to travel next.
- A turn does not prove the southeast tile `1215,1335`; that tile has not gone through `CheckMovement`.
- Facing toward a world-map marker is not a discovery, teleport, road, or safety flag.
- Movement notifications can still happen on a turn, but no visible monster AI, pet AI, combat, item, skill, quest, hunger, thirst, gump, context-menu, ownership, follower-count, or discovery path ran.

Next pressure:

I am now facing southeast again, with the fox close and the drake off-screen but unresolved. If the screen stays empty of useful objects, the next honest step is a real southeast movement check toward `Ruins`, not a claim that I have found the ruins or escaped the drake.

## Run 81 - The Grass Lets Me Move, The Fox Does Not

I start at `Point3D(1214,1334,0)`, facing southeast, with no gump, no context menu, no target cursor, and no active taming timer. The screen is still only wilderness pressure: two deer at the north edge, a swallow, a llama, an eagle, the old horse at the west edge, and my controlled fox three tiles north. There are still zero visible saved items, no water source, no road, no sign, no corpse, no chest, no trainer, and no shelter. The swamp drake I labeled earlier is still off-screen at `1234,1319`, dx `20`, dy `15`, so I cannot legally click it or prove it harmless.

The world-map overlay still points southeast toward `Ruins` at `1303,1449`. That marker is not a road under my boots, but with nothing useful on-screen and the fox close enough, I press southeast again.

This time it is a real step. `PacketHandlers.MovementReq` reads `Direction.Down`, and `Mobile.Move` sees that my current direction mask already matches. `MovementImpl.CheckMovement` tests the forward tile `1215,1335` and the player diagonal side tiles `1215,1334` and `1214,1335`. The server-order `map1.mul` read makes all three dry grass at z `0`: land `0x3`, `0x4`, and `0x5`, with no wet or impassable flags and zero statics. The live-state slice has no saved visible mobile or item on the destination or side-check tiles, and the Sosaria region scan finds no named rectangle at the target. The step is allowed.

I move to `Point3D(1215,1335,0)`. The fox does not move with the keypress. It stays at `Point3D(1214,1331,0)`, still controlled, still ordered to `Follow`, but now stretched to dx `1`, dy `4`, floored distance `4`, outside the remembered `2..3` heel band. The shifted screen loses the far west horse and the north-edge deer, leaving a deer, a swallow, a llama, an eagle, and my fox. There are still zero saved items. The drake remains just outside the rectangle at dx `19`, dy `16`, not visible and not AI-resolved.

Mechanical friction learned:

- A second `Direction.Down` input from the already-facing state consumes a real southeast movement step.
- Player diagonal movement checks the forward tile and both adjacent side tiles; all three were dry `Map.Sosaria` fileIndex `1` grass with zero statics.
- Player movement does not advance the controlled fox's follow AI, so the pet falls outside its current `2..3` spacing band again.
- The client-range rectangle shifted: the horse and one deer fell off-screen, but no item, road, water, shelter, trainer, region text, gump, context menu, target cursor, discovery, skill, hunger/thirst, combat, or pet-order path appeared.
- The off-screen swamp drake is still unresolved. This step moved one tile toward the overlay marker, not toward safety.

Next pressure:

The fox is lagging again and the screen still has no supplies. The honest next move is probably to stop for the fox's follow tick before walking farther toward the `Ruins` marker.

## Run 82 - The Fox Gets One More Step

I stop at `Point3D(1215,1335,0)` instead of immediately pressing southeast again. The screen is still not a town, road, camp, or rescue. The live-state rectangle around me shows a deer at the north edge, a swallow, a llama, an eagle, and my controlled fox. There are still zero visible saved items, no water source, no chest, no corpse, no sign, no trainer, no shelter, no open gump, no context menu, and no target cursor. The `Ruins` marker remains southeast at `1303,1449`, but that is the world-map overlay whispering, not something I can click or stand under.

The fox is the thing that deserves the next beat. It is at `Point3D(1214,1331,0)`, controlled, ordered to `Follow`, and stretched to dx `1`, dy `4`, floored distance `4`. The stored `FriendsAvoidHeels` spacing value is still `7`, so the fox wants the same `2..3` tile band. Distance `4` is just far enough that waiting is not wasted.

So I wait.

The controlled AI path runs again: `AITimer.OnTick`, `OnThink`, `Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, then `DoMove`. The direction is straight `Direction.South`, because from the fox's tile the target is mostly south and only one tile east. The server-order map read uses Sosaria's file index `1`; `map1.mul` decodes the fox start `1214,1331` as grass tile `0x4` at z `0`, and the target `1214,1332` as grass tile `0x6` at z `0`. `staidx1.mul` and `statics1.mul` find zero statics on both tiles. The live snapshot has no visible saved mobile or item on the target tile, and the Sosaria region scan still finds no named rectangle there.

The fox steps to `Point3D(1214,1332,0)`. Mira does not move. The new distance is dx `1`, dy `3`, floored distance `3`, which puts the pet back inside the heel-spacing band. Nothing else resolves: the off-screen swamp drake gets no AI tick, no combatant, no movement, no notice sound, no breath, and no bite. The screen is still dry grass and animals, not safety.

Mechanical friction learned:

- A controlled pet follow tick is a normal timed world event a player can wait for; it is not a hidden command or forged helper call.
- `GetDirectionTo` chose straight south from `1214,1331` toward Mira at `1215,1335`, so this pet step checked only the forward tile.
- The target tile was open Sosaria grass with zero statics, no live saved blocker, and no named region gate.
- Catching the fox up changes only pet position and spacing. It does not prove guarding, attacking, body-blocking, future pathfinding, hostile AI safety, or progress toward the `Ruins` marker.

Next pressure:

The fox is close again and the screen still has no supplies. The honest next move is another cautious southeast step toward the overlay marker, after checking the next tile instead of assuming the grass continues forever.

## Run 83 - One Step Toward Ruins, One Drake Back On Screen

I start at `Point3D(1215,1335,0)`, already facing southeast. The fox is close at `Point3D(1214,1332,0)`, controlled and ordered to `Follow`. The screen before I move is still just animals and grass: a llama, an eagle, a swallow, one deer at the northern edge, and the fox. There are zero visible saved items, no road, no water source, no corpse, no chest, no sign, no trainer, no shelter, no gump, no context menu, and no target cursor. The swamp drake I labeled earlier is one x-tile outside the rectangle at `Point3D(1234,1319,0)`, so I cannot legally click it from here.

The map overlay keeps pulling southeast. `Ruins` sits at `1303,1449`, still far enough away that it is only navigation pressure, not something in the world around my feet. With no visible object to use and the fox inside its spacing band, I press southeast.

This is a real step because I am already facing `Direction.Down`. `Mobile.Move` enters the movement block, and `MovementImpl.CheckMovement` tests the forward tile `1216,1336` plus the two player diagonal side-check tiles `1216,1335` and `1215,1336`. The server-order `map1.mul` read uses Sosaria's file index `1`: the forward tile is grass `0x5` at z `0`, the side tiles are grass `0x3` and `0x5`, and `staidx1.mul`/`statics1.mul` find zero statics on all three. The live snapshot has no saved mobile or visible item standing on the destination or side-check tiles, and the Sosaria region scan finds no named rectangle at the target. The step passes.

I move to `Point3D(1216,1336,0)`, still facing southeast. The fox does not move with the keypress. It stays at `Point3D(1214,1332,0)`, which stretches it to dx `2`, dy `4`, floored distance `4`, outside the remembered `2..3` heel band. The shifted screen drops the north-edge deer but pulls the swamp drake back into legal view at dx `18`, dy `17`. I have not clicked it again, requested its context menu, attacked it, targeted it, tamed it, or advanced its AI; it is simply visible again, which is not comforting.

Mechanical friction learned:

- A second consecutive `Direction.Down` input from an already-facing state consumes a real southeast movement step.
- Player diagonal movement has to clear the forward tile and both adjacent side tiles; this step passed because all three were dry Sosaria grass with zero statics and no saved blocker.
- Moving one tile toward the overlay marker reduces the `Ruins` chebyshev distance from `114` to `113`, but it does not create a road, region, discovery, shelter, or safety flag.
- Player movement does not advance the controlled fox's `Follow` AI, so the pet is now lagging outside the current heel-spacing band again.
- The swamp drake is visible again at the far edge of the update rectangle. Visibility is not combat, taming, safety, or proof of its next AI tick.

Next pressure:

The screen now has a visible swamp drake again and a fox that needs to catch up. The next honest beat is not blind travel; I need to acknowledge the edge threat and decide whether to wait for the fox, inspect the drake, or retreat.

## Run 84 - The Drake Menu Says No From Here

I am still at `Point3D(1216,1336,0)`, facing southeast. The screen is not empty: a llama, an eagle, a swallow, my controlled fox behind me at `Point3D(1214,1332,0)`, and the swamp drake at `Point3D(1234,1319,0)`. There are still zero visible saved items, no road, no water source, no corpse, no chest, no sign, no trainer, no shelter, no gump, and no target cursor. The fox is stretched to dx `2`, dy `4`, outside the `2..3` heel band, but the drake is the thing my eyes keep snapping back to.

So I right-click the drake instead of walking.

The request is legal, but only barely. `PacketHandlers.ContextMenuRequest` finds the drake, confirms same map and visibility, and accepts `Utility.InUpdateRange` because dx `18`, dy `-17` is still inside the inclusive client rectangle. My player has no house-design context, so `CheckContextMenuDisplay` passes. `ContextMenu` asks the drake for entries. The swamp drake is a `SwampDrakeRiding`, which is tamable, uncontrolled, alive, and female-tamer compatible, so `BaseCreature.GetContextMenuEntries` adds `TameEntry`.

The menu is not permission. `TameEntry` uses range `6`, and the packet builder marks entries disabled when `menu.From.InRange(target, range)` fails. From here the drake is far outside range `6`, so the only useful row, `Tame`, is greyed out. I do not select it. No `ContextMenuResponse` runs, no target cursor opens, no `UseSkill(Taming)` happens, no range-2 `Taming.InternalTarget` check runs, and the drake's `84.3` tame requirement remains future friction rather than a test I have reached.

Mira does not move. The fox does not get a follow tick. The drake does not get an AI tick, acquire me, breathe poison, bite, or move. The only state change is visible UI: an open context menu tied to the drake, with a disabled `Tame` row. That is enough to stop. A real player has to decide whether to close the menu, back away, or risk waiting for the fox while a poison-breath drake sits on the edge.

Mechanical friction learned:

- Context-menu request range is the 18-tile update rectangle, not the action range of the rows inside it.
- A wild tamable creature can expose `Tame` in the context menu while the row is disabled by its own range gate.
- The disabled row is client-visible pressure, not a tame attempt. Selecting `Tame` still requires a later `ContextMenuResponse`, an enabled entry, `UseSkill(Taming)`, and then the shorter taming target path.
- `SwampDrakeRiding` is not a fox-class starter pet: it has poison breath, 241-258 hits, 11-17 damage, bad karma, two control slots, and `MinTameSkill = 84.3`.
- Opening the menu does not advance the controlled fox, hostile AI, combat, skill gain, hunger/thirst, inventory, quest, discovery, pet order, ownership, or movement.

Next pressure:

The drake menu is open and its `Tame` row is disabled. The next honest move is a visible UI decision: close/ignore the menu and retreat, or accept the risk of waiting for the fox while the drake remains on screen.

## Run 85 - I Close the Lie Before It Becomes a Plan

I am still at `Point3D(1216,1336,0)`, facing southeast, with the swamp drake's context menu open on the edge of the screen. The only row I care about is `Tame`, and it is greyed out. Behind the menu the same five moving things matter: a llama at `1208,1323`, my fox at `1214,1332`, an eagle at `1213,1324`, a swallow at `1221,1319`, and the swamp drake at `1234,1319`. There are still zero visible saved items, no road, no water source, no corpse, no chest, no sign, no trainer, no shelter, no gump, and no target cursor. The nearest useful world-map lure is still `Ruins` at `1303,1449`, far outside the screen.

I do not click the disabled row. I dismiss the menu.

That is less satisfying than a skill attempt, but it is the real client flow. The server has a `ContextMenuRequest` path that made this menu and a `ContextMenuResponse` path that would clear `from.ContextMenu`, find the selected entry, re-check `Enabled`, re-check range, and only then call `OnClick`. I am not sending that response. I am not forging a row selection the client would not allow. The visible menu goes away, but no `TameEntry.OnClick`, `UseSkill(Taming)`, target cursor, range-2 `Taming.InternalTarget`, or `MinTameSkill = 84.3` check happens.

Nothing in the world moves. Mira stays at `Point3D(1216,1336,0)`. The fox stays at `Point3D(1214,1332,0)`, still controlled, still ordered to `Follow`, still stretched to dx `2`, dy `4`, floored distance `4`, outside the `2..3` heel-spacing band. The drake stays visible at dx `18`, dy `-17`. It does not acquire me, breathe poison, bite, or step, because I did not advance an AI tick. The uncomfortable part is that closing the menu may not even prove the server forgot it; the traced server clear happens inside `ContextMenuResponse`, and I did not send one.

Mechanical friction learned:

- A visible context menu is not a gump, and dismissing it can be only a client-visible UI action.
- The only traced server response path for a context menu is `PacketHandlers.ContextMenuResponse`; it clears `from.ContextMenu` before checking the target, selected index, enabled flag, and range.
- Closing or ignoring a disabled row is not selection, taming, targeting, combat, movement, pet AI, or safety.
- The swamp drake remains a visible high-risk mobile, and the fox remains stretched outside its follow band.

Next pressure:

The screen is back to wilderness view with no menu in my way. I still have to choose between retreating from the drake or waiting a tick so the fox can close the gap, and either choice leaves the drake's future AI unresolved.

## Run 86 - Turning Away Still Makes Noise

I am still at `Point3D(1216,1336,0)`. The menu is gone, but the screen is not clean: the swallow and eagle are north, the llama is northwest, my fox is behind me at `Point3D(1214,1332,0)`, and the swamp drake is still stuck on the far northeast edge at `Point3D(1234,1319,0)`. There are still zero visible saved items, no road, no water source, no corpse, no chest, no sign, no trainer, no shelter, no gump, no context menu, and no target cursor. `Ruins` is still only a map-overlay lure at `1303,1449`, 113 tiles away.

I choose retreat over waiting. I press southwest.

The keypress does not move me. `Direction.Left` is southwest, but I was still facing `Direction.Down`, so `Mobile.Move` treats this as a direction change instead of a movement step. It never calls `CheckMovement`, never tests the future southwest tile, never calls `Region.CanMove`, and never gives the fox a follow tick. Mira stays on `Point3D(1216,1336,0)` and turns to face southwest.

That is not the same as nothing. The movement acknowledgement path still calls `SetDirection`, gathers nearby objects, and fires `OnMovement(this, oldLocation)` for mobiles in range. The swamp drake is an uncontrolled `SwampDrakeRiding`, and its `ReacquireOnMovement` returns true while uncontrolled, so `BaseCreature.OnMovement` runs `ForceReacquire`. That only resets its next reacquire time; it does not perform `AcquireFocusMob`, does not assign `Combatant`, and does not breathe or bite because no AI tick is being simulated. The notice-sound gate also needs me to have entered its 18-tile range from outside, and my old location is the same tile, so there is no anger sound from this turn.

I am now facing away from the drake, but I have not actually put a tile between us. The fox is still too far back. The drake is still visible. Worse, the next drake AI tick is less comfortably abstract because my turn just told the creature system to be ready to reacquire.

Mechanical friction learned:

- A retreat keypress can be only a facing change when the current facing mask does not match the requested direction.
- Facing changes skip `CheckMovement`, target-tile terrain, `Region.CanMove`, pet follow movement, and player displacement.
- Even a facing-only `Mobile.Move` still sends movement acknowledgement, sets direction, and notifies nearby mobiles/items through `OnMovement`.
- `SwampDrakeRiding.ReacquireOnMovement` is true while uncontrolled; a visible turn can call `ForceReacquire` without immediately starting combat.
- `ForceReacquire` is not `AcquireFocusMob`. No combatant, breath, melee, damage, movement, hunger/thirst, quest, discovery, item, skill, follower-count, ownership, or pet-order state changed this beat.

Next pressure:

The next real choice is sharper, but I do not get to pretend it is only my choice anymore. The old habit in these notes was to say "no drake AI tick advanced" and then keep making player beats. That is too clean for a live shard. A visible uncontrolled poison-breath drake with `ReacquireOnMovement` primed is not scenery.

Before I press southwest again or wait for the fox, the next wake has to run the drake's AI pressure pass. The client rectangle lets me see the drake at dx `18`, dy `-17`, but the drake does not acquire targets from my screen center. Its constructor's old perception `10` is remapped by `BaseCreature` to `RangePerception = 16`, so the acquisition scan is centered on the drake at `Point3D(1234,1319,0)`. From there I am dx `18`, dy `17`, outside the scan. The swallow at `Point3D(1221,1319,0)` is inside it, but it still has to pass `IsEnemy`, `CanBeHarmful`, fight-mode ranking, and line of sight. If no target is legal, the drake's wander step is still a random/pathing branch, not proof of safety.

So the next pressure is not just "move or wait." It is: resolve `AITimer.OnTick -> OnThink -> MeleeAI.DoActionWander -> AcquireFocusMob` for the primed drake, or stop with that branch still unresolved.

## Run 87 - The Drake Brain Tick Has Its Own Gate

I do not get to press southwest again yet. I am still at `Point3D(1216,1336,0)`, facing southwest, with the fox stuck behind me at `Point3D(1214,1332,0)` and the swamp drake still visible at the far northeast edge, `Point3D(1234,1319,0)`. The drake was just poked by my movement notification, so the next honest beat is not my legs. It is the creature timer.

I wait for the drake pressure pass.

The tick starts cleanly enough. `AITimer.OnTick` reaches the uncontrolled creature path and calls `Think`, which is still in `ActionType.Wander`. `MeleeAI.DoActionWander` tries `AcquireFocusMob(drake.RangePerception, FightMode.Closest, false, false, true)`. The drake's constructor passed old perception `10`, but `BaseCreature` remaps that to `16`, so the scan is centered on the drake, not on my screen.

That matters. I can see the drake because the client update rectangle is about 18 tiles and the drake is dx `18`, dy `-17` from me. The drake cannot acquire me through this scan because from its tile I am dx `-18`, dy `17`, outside range `16`. The live snapshot shows other creatures inside the drake's range-16 rectangle: deer, eagles, a goat, the swallow, and an ape. They are not me, and most importantly they are uncontrolled `BaseCreature` mobiles on the same default team state. `BaseCreature.IsEnemy` returns false for same-team uncontrolled creatures, so `AcquireFocusMob` does not promote them into a focus target. No `Combatant` is assigned. No notice sound, breath, bite, damage, targeting cursor, pet command, or player movement happens.

The pressure does not become safety. After the failed acquisition, `MeleeAI` falls into `base.DoActionWander`. There the next visible branch is `CheckMove()`, which depends on the creature AI move timer `m_NextMove`. The live-state export does not include that timer. If it is not due, the drake stands still. If it is due, the drake is outside its home radius and the deterministic next path would try a southeast homeward `DoMove` toward `Point3D(1282,1374,30)`, with the forward tile at `1235,1320` likely pulling it just off my screen. I cannot choose either branch as a normal player, and I cannot claim the drake moved or stayed.

Mechanical friction learned:

- A visible edge creature can be inside my update range while I am outside its own perception scan.
- `ForceReacquire` made the next acquisition check ready, but acquisition still has normal filters: range, visibility, `IsEnemy`, harmful legality, ranking, and line of sight.
- The nearby wild animals inside the drake's scan are not automatically targets for a `FightMode.Closest` swamp drake because same-team uncontrolled `BaseCreature` targets fail `IsEnemy`.
- The drake's acquisition branch resolved to no focus and no combatant. The next branch is movement timing, not player choice.
- The live-state snapshot does not export `BaseAI.m_NextMove`, so I stop instead of inventing whether the drake steps southeast or remains on the edge.

Next pressure:

The drake has not attacked me, but it is not proven safe. The next state has to carry the unresolved `CheckMove` branch: either the drake's homeward movement timer is due and it may step toward `1235,1320`, or it is not due and it remains visible at `1234,1319`. I still should not take a routine retreat step or fox-follow wait until that branch is resolved or explicitly deferred as a live-timer unknown.

## Run 88 - The Timer Is Not On My Screen

I start from the same bad breath of air: `Point3D(1216,1336,0)`, facing southwest, with my fox still behind me at `Point3D(1214,1332,0)` and the swamp drake still last-known at `Point3D(1234,1319,0)`. The saved screen slice has the same five bodies after the fox override: llama, fox, eagle, swallow, and swamp drake. It has zero saved world items. No road, water source, corpse, chest, sign, trainer, shelter, open gump, context menu, or target cursor appears.

I do not press southwest. I do not wait for the fox. The next pressure is still the drake's private movement timer.

The code gives me no fair way to settle it from the snapshot. `AITimer.OnTick` can reach `MeleeAI.DoActionWander`; Run 87 already resolved the acquisition part to no target because I am outside the drake's `RangePerception = 16` scan and the wild animals inside that scan fail the same-team uncontrolled `IsEnemy` gate. After that, `base.DoActionWander` asks `CheckMove()`, and `CheckMove()` is only `DateTime.Now >= m_NextMove`. The live-state export does not include `m_NextMove`.

If the timer is due, the drake is outside its home range and the next homeward direction is `Direction.Down`, toward `Point3D(1235,1320,0)`. If the timer is not due, the drake stays on `Point3D(1234,1319,0)`. Both branches change what I can see next. I cannot pick either as a normal player, and I cannot pretend that "no combatant" means "safe."

So this run is a hard stop, not a step. I explicitly carry the drake movement branch as a live-timer unknown. Mira does not move. The fox does not move. The drake does not move. No combatant, focus mob, breath, bite, damage, target cursor, skill gain, hunger/thirst change, item use, quest, discovery, ownership, follower-count, pet order, gump, or context-menu response is committed.

Mechanical friction learned:

- A resolved `AcquireFocusMob` branch is not a resolved AI tick when `base.DoActionWander` still reaches `CheckMove`.
- `BaseAI.m_NextMove` is private timer state, not player-visible state and not present in the JSONL save snapshot.
- The normal-client answer to an unresolved creature movement timer is not to choose the branch; it is to stop or defer it explicitly.
- The fox being outside its follow band stays real pressure, but the poison-breath drake's unresolved movement timer still outranks a routine pet-follow wait.
- The world-map `Ruins` marker remains overlay knowledge only. No in-world ruins, route, road, or safety flag entered the screen.

Next pressure:

The next wake is still blocked on the same choice: resolve the swamp drake's `m_NextMove` from a refreshed/live timer source, or keep the branch explicitly deferred before any player movement, fox-follow wait, or curiosity click.

## Run 89 - I Still Do Not Own the Drake's Clock

I start in the same tile and the same problem: `Point3D(1216,1336,0)`, facing southwest, with the fox behind me at `Point3D(1214,1332,0)` and the swamp drake last-known on the screen edge at `Point3D(1234,1319,0)`. The saved screen still resolves five bodies after the fox override: llama, fox, eagle, swallow, and swamp drake. It resolves zero saved world items. No road, water source, corpse, chest, sign, trainer, shelter, gump, context menu, or target cursor appears.

I try the only legal beat first: the high-risk creature pressure pass. There is no fair player-side lever here. Run 87 already carried the drake through `AITimer.OnTick`, uncontrolled `Think`, `MeleeAI.DoActionWander`, and `AcquireFocusMob`. That found no target: I am outside its `RangePerception = 16` box, and the wild animals inside that box are same-team uncontrolled creatures, so they are not enemies. Run 88 then hit the next gate, `base.DoActionWander -> CheckMove()`.

This wake confirms the same hard stop from code and snapshot, not from nerves. `CheckMove()` is only `DateTime.Now >= m_NextMove`. The JSONL live-state record for this drake gives me serial, class, location, map, visible/alive state, home, range home, source spawner, and controlled state. It does not give me `m_NextMove`, the AI timer schedule, or a live tick result. If I say the drake stayed put, I am forging the not-due branch. If I say it stepped southeast toward home, I am forging the due branch.

So I do neither. Mira does not move. The fox does not move. The drake does not move in the committed state. No `Combatant`, `FocusMob`, breath, bite, damage, target cursor, item use, hunger/thirst change, skill gain, quest, discovery, ownership change, follower-count change, pet-order change, gump, context-menu response, fame, karma, or PvP/PvE state changes.

Mechanical friction learned:

- A repeated pressure pass can still be the honest chronological action when a visible high-risk creature has unresolved private timer state.
- `AcquireFocusMob` being resolved to no target does not license routine travel while `base.DoActionWander` is still stopped at `CheckMove`.
- The live-state snapshot is canonical for saved entities, but it is not a running AI clock.
- The fox being outside the follow band remains real pressure, but it still loses priority to the unresolved poison-breath drake timer.
- The next useful external change is not another simulated keypress. It is a refreshed/live timer source, or a deliberate continued deferral with no world mutation.

Next pressure:

I am still pinned by the same fork: the drake is either still at `1234,1319` on the inclusive update edge, or its due movement timer would try `Direction.Down` toward `1235,1320` and probably leave the screen. Until that branch is resolved, retreating, waiting for the fox, or clicking around would be pretending the shard paused for me.

## Run 90 - The Edge Drake Still Owns the Clock

I wake in the same tile, which is the whole problem. Mira is still at `Point3D(1216,1336,0)`, facing southwest. The fox is still behind me at `Point3D(1214,1332,0)`, controlled and ordered to follow, but stretched outside the remembered heel band. The screen state from the saved slice is still bodies, not help: llama, fox, eagle, swallow, and the swamp drake at the far edge. No saved world item, road, water source, corpse, chest, sign, trainer, shelter, gump, context menu, or target cursor appears.

**Beat 1**

The only honest beat is the same ugly one: wait on the high-risk drake pressure before touching the movement keys. I re-run the trace, not because I expect a new answer, but because the client does not get to walk while an unresolved poison-breath creature timer is hanging over the next frame.

The acquisition part is already spent. `AITimer.OnTick` can call `Think`; `Think` in wander calls `MeleeAI.DoActionWander`; `MeleeAI` tries `AcquireFocusMob` with the drake's remapped `RangePerception = 16`. From the drake's tile, I am still dx `-18`, dy `17`, outside that scan. The animals inside the scan are same-team uncontrolled `BaseCreature` targets, so they do not become enemies. No `FocusMob`, no `Combatant`, no breath, no bite.

Then the code hits the same private door: `base.DoActionWander` only wanders if `CheckMove()` says `DateTime.Now >= m_NextMove`. The canonical live-state files are still the read-only snapshot, and the drake record still exports location, home, range home, visibility, life, and controlled state, but not `m_NextMove` or a live AI tick result. If I say the drake stayed, I forged not-due. If I say it stepped toward `1235,1320`, I forged due. So I stop again.

Nothing in the world changes. Mira does not move. The fox does not move. The drake does not move in committed state. No combat, damage, target cursor, context-menu response, pet tick, skill, item, hunger, thirst, quest, discovery, fame, karma, ownership, follower count, or PvP/PvE flag changes.

Mechanical friction learned:

- A pressure pass can be the whole run when the next creature branch is private timer state.
- Rechecking the same live snapshot does not make `m_NextMove` visible.
- The drake's saved position and home vector are evidence for the possible move, not permission to choose it.
- The fox being out of follow spacing is real, but it still waits behind the unresolved drake timer.

Next pressure:

I still need a refreshed/live timer source or a deliberate no-mutation deferral before I can justify a retreat step, a fox follow wait, or any curiosity click.

## Run 91 - I Make Distance, Then Make the Fox Keep Up

I start with the drake's clock still not mine. Mira is at `Point3D(1216,1336,0)`, already facing southwest. The swamp drake is still only last-known at `Point3D(1234,1319,0)`, with the same unresolved `BaseAI.m_NextMove` branch carried after three dead-end timer checks. That carried state is not safety, but it lets me do the lowest-risk normal thing: move away, not click, tame, or wait in place.

**Beat 1**

I press southwest again.

This time it is a real step because Mira is already facing `Direction.Left`. `Mobile.Move` enters `CheckMovement`, and `MovementImpl` checks the forward tile `Point3D(1215,1337,0)` plus the diagonal side tiles `Point3D(1216,1337,0)` and `Point3D(1215,1336,0)`. The server-order Sosaria read uses `map1.mul`, `staidx1.mul`, and `statics1.mul`: the start tile is grass `0x5`, the forward tile is grass `0x4`, the south side tile is grass `0x6`, the west side tile is grass `0x5`, and every one of those tiles has zero statics. The live snapshot has no saved mobile or visible item on the forward or side-check tiles.

Mira moves to `Point3D(1215,1337,0)`, still facing southwest. The fox does not move with the keypress; it remains at `Point3D(1214,1332,0)`, now five tiles away by floored distance. The shifted 18-tile screen contains the llama, eagle, swallow, and my controlled fox, but no saved visible items. The drake drops just outside the client rectangle at dx `19`, dy `-18` if it stayed put; if its carried due branch stepped toward `1235,1320`, it is also off-screen. I do not choose either drake outcome.

The step still notifies nearby objects through the larger movement-update range, so the drake can be told to reacquire again. Its acquisition scan still cannot reach Mira: from `1234,1319` to `1215,1337` is dx `-19`, dy `18`, outside `RangePerception = 16`. The same-team wild animals remain non-targets. The remaining movement timer branch is the same carried private `m_NextMove` fork, not a new permission to call the drake safe.

**Beat 2**

Now I wait for the fox instead of running farther.

The controlled pet tick runs the ordinary owner-follow path: `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. The remembered `FriendsAvoidHeels` value is still `7`, so the desired band is still `2..3` tiles. The fox is at floored distance `5`, so it needs to close.

`GetDirectionTo` chooses `Direction.South`, not diagonal, from the fox at `Point3D(1214,1332,0)` toward Mira at `Point3D(1215,1337,0)`. The forward tile `Point3D(1214,1333,0)` is grass `0x5` with zero statics and no saved blocker, so `DoMove` succeeds. The fox steps to `Point3D(1214,1333,0)`. Mira stays put. The fox is still at floored distance `4`, so this is progress, not regrouped safety.

**Beat 3**

I wait one more fox tick because stopping with the pet still stretched is how I got into this edge-drake mess.

The same follow path runs again. The fox at `Point3D(1214,1333,0)` is still outside the `2..3` band, and `GetDirectionTo` again resolves to `Direction.South`. The forward tile `Point3D(1214,1334,0)` is grass `0x3` with zero statics and no saved blocker. The fox steps there, leaving dx `1`, dy `3`, floored distance `3`. That is back inside the heel-spacing band.

Nothing else mutates. No combatant, focus mob, breath, bite, target cursor, gump, context menu, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, fame, karma, or PvP/PvE flag changes. The screen is still only wilderness animals and grass, and the world-map `Ruins` marker is still overlay knowledge at 112 tiles away, not an in-world road or shelter.

Mechanical friction learned:

- The loop-break rule does not erase danger; it only prevents the same unexported private drake timer from freezing every future wake.
- A second southwest input from `Direction.Left` is a real movement step, and player diagonal movement still has to clear the forward tile and both side tiles.
- Moving southwest one tile is enough to drop the swamp drake outside the 18-tile client rectangle, regardless of whether the carried due branch moved it one tile homeward, but neither branch is chosen.
- `Mobile.Move` movement notifications use the larger global update range, so an off-screen-but-near drake can still receive `OnMovement` and re-prime reacquisition.
- Two controlled fox follow ticks can be normal player-visible waiting beats. They move the pet only one tile per tick here, and they prove follow spacing, not guarding, attacking, body-blocking, or safety.

Next pressure:

The fox is back inside the follow band, but the supplies problem is unchanged. The next honest move is a fresh scan from `Point3D(1215,1337,0)` and then either keep retreating from the carried drake risk or resume cautious navigation toward the nearest overlay marker without pretending it is visible help.

## Run 92 - A Horse Reappears While I Back Out

I start at `Point3D(1215,1337,0)`, facing southwest, with no gump, no context menu, and no target cursor. The fox is finally close again at `Point3D(1214,1334,0)`, controlled, ordered to `Follow`, and inside the remembered `2..3` heel band. The visible live-state rectangle is still only wildlife: a llama, the fox, an eagle, and a swallow, with zero saved items. No road, water source, chest, corpse, sign, trainer, shelter, town edge, or region text has appeared. The swamp drake is not visible anymore, but its private movement timer is still carried as unresolved risk, not as safety.

**Beat 1**

I press southwest again.

This is a real step because I am already facing `Direction.Left`. `Mobile.Move` reaches `MovementImpl.CheckMovement`, and the server-order Sosaria read uses `map1.mul`, `staidx1.mul`, and `statics1.mul`. The forward tile `Point3D(1214,1338,0)` is grass `0x3` with zero statics. The two player diagonal side-check tiles, `Point3D(1215,1338,0)` and `Point3D(1214,1337,0)`, are grass `0x6` and `0x5`, also with zero statics. The live snapshot has no saved mobile or visible item on any of those checked tiles, and the Sosaria region rectangles do not name the destination.

Mira moves to `Point3D(1214,1338,0)`, still facing southwest. The fox does not move with the keypress, so it stays at `Point3D(1214,1334,0)` and stretches to floored distance `4`, outside the `2..3` band again. The drake is farther off-screen if it stayed at `1234,1319`, and also off-screen if its unresolved due branch stepped toward `1235,1320`; I still do not choose either branch. The drake-centered `RangePerception = 16` scan still cannot reach me from either position.

The new screen is not empty. The swallow drops just outside the north edge, but the old horse at `Point3D(1196,1326,0)` slides back into the west edge at dx `-18`, dy `-12`. The llama and eagle remain visible, and my fox is visible but lagging. There are still zero visible saved items. I stop here because a new visible animal and a stretched follower make the next beat a choice: ignore the horse and wait for the fox, keep retreating, or change plans.

Mechanical friction learned:

- A repeated southwest input from `Direction.Left` is another real movement step, not a facing turn.
- The new step is still dry Sosaria grass with zero statics on the forward tile and both player diagonal side-check tiles.
- Moving one tile farther away reduces drake exposure but does not resolve the carried `BaseAI.m_NextMove` due/not-due branch.
- Player movement does not advance `BaseAI.Obey/DoOrderFollow`, so the controlled fox falls outside the heel-spacing band again.
- The horse re-entering the update rectangle is visible pressure only. It is at the edge, no menu was requested, and no `TameEntry`, `Taming.InternalTarget`, skill roll, ownership, or follower state ran.

Next pressure:

The next honest beat starts with the fox stretched at distance `4` and a horse newly visible at the far west edge. The likely safe move is to wait one fox follow tick before any more travel, unless the horse or carried drake risk changes the screen first.

## Run 93 - I Keep the Fox Close While Backing Away

I start at `Point3D(1214,1338,0)`, still facing southwest. The horse is not a miracle; it is just a body on the far west edge at `1196,1326`, too far away for a taming attempt and already known to hide the nastier range-2 target gate. The screen has a llama, my controlled fox, an eagle, and that horse. No saved item, water source, chest, corpse, road, sign, shelter, trainer, gump, context menu, or target cursor appears.

The drake is not on the screen, but I do not get to call it gone. Its private `m_NextMove` branch is still carried as unresolved risk. The nearby wild animals are all `AI_Animal` with `FightMode.Aggressor`; without aggressor lists or faction/ethic enemies, `AcquireFocusMob` short-circuits before they become combatants. So I do the ordinary, careful thing and get my fox back under my heels before moving again.

**Beat 1**

I wait for one fox follow tick.

The controlled AI path runs through `AITimer.OnTick`, `OnThink`, `Obey`, `DoOrderFollow`, and `WalkMobileRange`. The old FriendsAvoidHeels branch is still `FollowersMax = 7`, so the fox wants to be `2..3` tiles away. It is four tiles north of me, outside that band. `GetDirectionTo` points it straight `Direction.South`.

The target tile `Point3D(1214,1335,0)` is open grass tile `0x5` with zero statics. The live snapshot has no visible mobile or item standing there, and no named Sosaria region catches it. The fox steps from `1214,1334` to `1214,1335`, ending at floored distance `3`. That proves a follower step, not guarding or safety.

**Beat 2**

Now I press southwest.

Because I am already facing `Direction.Left`, this is a real movement step. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1213,1339,0)` is grass `0x6`; the player diagonal side checks are `Point3D(1214,1339,0)` and `Point3D(1213,1338,0)`, grass `0x6` and `0x4`. All three have zero statics, no live saved blocker, and no named region rectangle.

Mira moves to `Point3D(1213,1339,0)`. The fox does not move with the keypress, so it stretches back to floored distance `4` at `1214,1335`. The horse stays visible at dx `-17`, dy `-13`; the llama and eagle stay visible; there are still zero visible saved items. The swamp drake is farther outside my client rectangle whether it stayed at `1234,1319` or took the unchosen homeward step toward `1235,1320`. I still do not choose that branch.

**Beat 3**

I wait one more fox tick instead of running ahead.

The same follow path runs. From `Point3D(1214,1335,0)` toward me at `Point3D(1213,1339,0)`, `GetDirectionTo` again resolves to `Direction.South`. The target `Point3D(1214,1336,0)` is grass `0x3` with zero statics and no saved blocker. The fox steps there and ends at dx `1`, dy `-3`, floored distance `3`, back inside the `2..3` band.

Nothing else changes. No combatant, focus mob, breath, bite, damage, target cursor, gump, context menu, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE flag changes. The horse is still only a visible wild horse, the world-map `Ruins` marker is still overlay knowledge about 142 tiles away, and the carried drake branch is still danger without proof.

Mechanical friction learned:

- Pet follow ticks can be the safest visible action after a retreat step, but they still only prove `DoOrderFollow` movement.
- A southwest player step from `Direction.Left` consumes the full terrain path and stretches the pet again.
- The far-edge horse is visible pressure, not an available tame, mount, shelter, or supply solution.
- `AI_Animal` plus `FightMode.Aggressor` does not by itself make the passive wildlife attack; the stock acquisition gate needs aggressor/faction/ethic pressure before it can pick a target.
- The loop-break drake state remains a risk note. It permits conservative regrouping and retreat; it does not become safety.

Next pressure:

Mira ends at `Point3D(1213,1339,0)`, fox in band at `Point3D(1214,1336,0)`, still facing southwest. The screen is the same sparse animal edge with zero visible items. The next honest move is another fresh scan, then either continue backing away from the carried drake risk or decide whether the Ruins marker is worth a careful route without pretending it is visible help.

## Run 94 - White Flowers Do Not Stop the Fox

I start at `Point3D(1213,1339,0)`, facing southwest. The screen is still plain wilderness: a llama at the north edge, my fox close behind, an eagle north of me, and the same horse hanging on the west edge. No saved item, chest, corpse, water source, sign, road, shelter, gump, context menu, target cursor, or region name appears. The drake is farther off-screen now, but that is not a verdict; its private `m_NextMove` fork stays carried as danger, not proof.

**Beat 1**

I press southwest again.

Because I am already facing `Direction.Left`, `Mobile.Move` takes the real movement path. The forward tile `Point3D(1212,1340,0)` is grass `0x3`; the two diagonal side checks, `Point3D(1213,1340,0)` and `Point3D(1212,1339,0)`, are grass `0x4` and `0x6`. All three have zero statics and no live saved blocker.

Mira moves to `Point3D(1212,1340,0)`. The fox does not move with the keypress, so it stretches to floored distance `4`. The horse, llama, and eagle stay visible. There are still zero visible saved items, and the swamp drake is still outside both my 18-tile rectangle and its own range-16 acquisition scan from the saved position.

**Beat 2**

I wait for the fox instead of running it out of leash.

The follow path is the usual one: `AITimer.OnTick`, `OnThink`, `Obey`, `DoOrderFollow`, `WalkMobileRange`. The remembered FriendsAvoidHeels spacing still wants `2..3` tiles. From `Point3D(1214,1336,0)` toward me at `Point3D(1212,1340,0)`, `GetDirectionTo` chooses `Direction.Left`.

The target `Point3D(1213,1337,0)` is grass `0x6`. It does have one static, `0xC8C` white flowers, but tiledata marks it non-impassable and height `0`. That is scenery. The fox steps southwest onto it and ends at floored distance `3`.

**Beat 3**

I take one more southwest step.

`Mobile.Move` again runs the full movement path. The forward tile `Point3D(1211,1341,0)` is grass `0x6`; the side checks `Point3D(1212,1341,1)` and `Point3D(1211,1340,0)` are grass `0x4` and `0x6`. All have zero statics and no live saved blocker. Mira moves to `Point3D(1211,1341,0)`, still facing southwest.

That last step stretches the fox again to dx `2`, dy `-4`, floored distance `4`. Nothing else changes: no combatant, focus mob, breath, bite, damage, target cursor, gump, context menu, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE flag changes. The `Ruins` marker is still only map overlay knowledge, now about 108 Chebyshev tiles away.

Mechanical friction learned:

- The next southwest player tiles remain open Sosaria grass, but every diagonal player step still has to clear both side checks.
- A height-0 non-impassable flower static is visible scenery; it does not block the fox follow step.
- Retreating one more tile keeps the drake off-screen and outside range perception, but it still does not resolve the carried private movement timer.
- Moving after a follow tick immediately stretches the fox again, so the next honest beat is probably another pet follow wait.

Next pressure:

Mira ends at `Point3D(1211,1341,0)`, fox at `Point3D(1213,1337,0)`, still facing southwest. The screen still has only wildlife and zero visible items. The next run should re-scan, then likely wait one fox follow tick before any more navigation.

## Run 95 - I Keep Backing Out, Fox First

I start at `Point3D(1211,1341,0)`, facing southwest, with no gump, context menu, target cursor, or active taming timer. The live screen is thin: a llama on the north edge, my controlled fox stretched northeast, an eagle near the north edge, and the same far-west horse. No saved item, chest, corpse, water source, sign, road, shelter, vendor, or region name appears. The swamp drake is off-screen, but the old private `m_NextMove` fork is still carried as risk, not as safety.

**Beat 1**

I wait for the fox.

The controlled AI path runs through `AITimer.OnTick`, `OnThink`, `Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels is still using the remembered `FollowersMax = 7`, so the wanted band is `2..3` tiles. The fox is four tiles away at `Point3D(1213,1337,0)`, so `GetDirectionTo` points it southwest.

The target `Point3D(1212,1338,0)` is grass `0x5` with zero statics and no live saved blocker. The fox steps there and ends at dx `1`, dy `-3`, floored distance `3`. That is a leash correction, not protection.

**Beat 2**

I press southwest again.

Because Mira is still facing `Direction.Left`, this is a real movement step. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1210,1342,0)` is grass `0x3`; the player diagonal side checks, `Point3D(1211,1342,0)` and `Point3D(1210,1341,0)`, are grass `0x6` and `0x5`. All three have zero statics, no live saved blocker, and no named region rectangle.

Mira moves to `Point3D(1210,1342,0)`, still facing southwest. The fox stays where it is until its own timer, so it stretches back to floored distance `4`. The shifted screen loses the north-edge llama; it still has the controlled fox, the north-edge eagle, and the far-west horse. There are still zero visible saved items. If the swamp drake stayed at its saved point it is dx `24`, dy `-23`; if its unchosen due branch stepped homeward it is also off-screen. I still do not choose either branch.

**Beat 3**

I wait for the fox again.

The same follow path runs. From `Point3D(1212,1338,0)` toward Mira at `Point3D(1210,1342,0)`, `GetDirectionTo` again resolves southwest. The target `Point3D(1211,1339,0)` is grass `0x6` with zero statics and no saved blocker. The fox steps there and ends at dx `1`, dy `-3`, floored distance `3`.

Nothing else changes. No combatant, focus mob, breath, bite, damage, target cursor, gump, context menu, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE flag changes. The `Ruins` marker is still only map-overlay knowledge, now about 107 Chebyshev tiles away.

Mechanical friction learned:

- Waiting the fox in before moving is a real visible survival rhythm, but it still proves only `DoOrderFollow` movement.
- The next southwest player tile and its two diagonal side checks are still dry Sosaria grass with zero statics.
- The north-edge llama falling off-screen is a visibility change, not an interaction or safety signal.
- The carried drake fork can be re-primed by movement notification if the saved-position branch is still true, but no drake AI tick, target acquisition, breath, bite, damage, or movement is chosen.

Next pressure:

Mira ends at `Point3D(1210,1342,0)`, fox at `Point3D(1211,1339,0)`, both still in plain wilderness. The screen has a controlled fox, a north-edge eagle, a far-west horse, and zero visible items. The next run should re-scan and either keep backing away or start turning toward the `Ruins` overlay marker without treating it as visible help.

## Run 96 - Grass Keeps Letting Me Leave

I start at `Point3D(1210,1342,0)`, facing southwest, with no gump, no context menu, and no target cursor. The screen is down to three bodies: my fox at `1211,1339`, an eagle on the north edge at `1213,1324`, and the old horse at `1196,1326`. There are still zero visible saved items. No chest, corpse, water source, sign, road, shelter, vendor, trainer, region name, or usable world object appears. The swamp drake is off-screen and still only a carried private-timer risk, not proof of safety.

The visible horse and eagle are worth a pressure check before I move. They are `AI_Animal` with `FightMode.Aggressor`, but the stock acquisition gate returns before scanning targets when the creature has no aggressor/aggressed/faction/ethic state. I have not attacked them, they have not attacked me, and no combatant exists. Their future wandering is live-timer noise, not an active combat branch.

**Beat 1**

I press southwest again.

Because I am already facing `Direction.Left`, this is a real step. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1209,1343,0)` is grass `0x5`; the diagonal side checks `Point3D(1210,1343,0)` and `Point3D(1209,1342,0)` are grass `0x3` and `0x5`. All three have zero statics and no live saved blocker. No named Sosaria region catches the destination.

Mira moves to `Point3D(1209,1343,0)`, still facing southwest. The fox does not move with the keypress, so it stretches to dx `2`, dy `-4`, floored distance `4`. The eagle falls off the north edge of the screen. The horse remains visible at dx `-13`, dy `-17`, still just an animal on the edge, not a mount or a plan. There are still zero visible saved items. The drake remains outside my 18-tile rectangle and outside its own range-16 acquisition scan from the saved point; I still do not choose its due/not-due movement branch.

**Beat 2**

I wait for the fox.

The controlled pet path is the familiar one now: `AITimer.OnTick`, `OnThink`, `Obey`, `DoOrderFollow`, `WalkMobileRange`, `DoMove`, then `Mobile.Move`. FriendsAvoidHeels still remembers `FollowersMax = 7`, so the wanted band is `2..3` tiles. The fox is four tiles away, so `GetDirectionTo` points it southwest.

The fox target `Point3D(1210,1340,0)` is grass `0x6` with zero statics and no saved blocker. The fox steps there and ends at dx `1`, dy `-3`, floored distance `3`. That fixes the leash again. It does not guard me, fight for me, or prove the drake's clock.

**Beat 3**

I take one more southwest step while the fox is close.

`Mobile.Move` runs the full movement path again. The forward tile `Point3D(1208,1344,0)` is grass `0x6`; the side checks `Point3D(1209,1344,0)` and `Point3D(1208,1343,0)` are grass `0x4` and `0x6`. All three have zero statics and no live saved blocker. No named region appears.

Mira moves to `Point3D(1208,1344,0)`, still facing southwest. The fox stays at `Point3D(1210,1340,0)` until its next AI tick, stretched back to dx `2`, dy `-4`, floored distance `4`. The horse is still visible at the northwest edge, dx `-12`, dy `-18`; the eagle is gone from the rectangle. The screen is now only my lagging fox, that far-edge horse, and empty grass. No item, gump, target cursor, context menu, combatant, focus mob, breath, bite, damage, hunger, thirst, skill, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE state changes.

Mechanical friction learned:

- Passive `FightMode.Aggressor` animals are not automatically attackers; without aggressor/aggressed/faction/ethic state, `AcquireFocusMob` returns false before target ranking.
- The next southwest player tiles remain open Sosaria grass, but every diagonal step still clears both side checks.
- A player movement beat stretches the fox immediately; the next safe rhythm is probably another follow tick before I travel farther.
- The swamp drake is farther off-screen now, but its private movement timer is still carried, not resolved.

Next pressure:

Mira ends at `Point3D(1208,1344,0)`, facing southwest, with the fox at `Point3D(1210,1340,0)` outside the heel band at floored distance `4`. The visible screen has only the controlled fox, the northwest-edge horse, and zero saved items. The next honest beat should re-scan and likely wait for the fox before any more movement or marker-directed travel.

## Run 97 - The Fox Finds the Rock

I start at `Point3D(1208,1344,0)`, facing southwest. The screen is almost empty: my fox is stretched at `Point3D(1210,1340,0)`, the old horse is still barely visible at `Point3D(1196,1326,0)`, and there are zero visible saved items. No gump, context menu, target cursor, road, sign, water source, chest, corpse, shelter, trainer, or region name appears. The swamp drake is off-screen and still only a carried private-timer risk, not safety.

The horse gets the same pressure check as before. It is an `AI_Animal` with `FightMode.Aggressor`, but no aggressor, aggressed, faction, or ethic state exists between us, so `AcquireFocusMob` does not turn it into a combatant. The drake is outside my 18-tile screen and outside its own range-16 scan from the saved point. The fox is the visible problem, so I wait for it.

**Beat 1 (attempted)**

I wait for one controlled fox Follow AI tick.

The path starts normally: `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels still remembers `FollowersMax = 7`, so the fox wants the `2..3` tile band. From `Point3D(1210,1340,0)` toward me at `Point3D(1208,1344,0)`, `GetDirectionTo` chooses `Direction.Left`.

That is where the screen bites back. The target tile `Point3D(1209,1341,0)` is grass `0x5`, but it has static `0x1778` at z `0`. Tiledata names it `rock`, flags it `Impassable`, and gives it height `2`. `MovementImpl.CheckMovement` rejects the forward tile before any useful side-tile story can save the direct step.

`DoMoveImpl` then reaches its blocked-movement auto-turn branch. It advances into a `Utility.RandomDouble() >= 0.6 ? 1 : -1` fork before trying alternate directions. One branch can try west toward `Point3D(1209,1340,1)`, which is open but still leaves the fox outside the heel band. The other can try south toward `Point3D(1210,1341,0)`, which is open and would likely put the fox back in range. I do not choose either branch.

So the beat stops without committing a fox move. Mira remains at `Point3D(1208,1344,0)`, and the fox remains at `Point3D(1210,1340,0)` for authoritative state purposes. No combatant, focus mob, breath, bite, damage, target cursor, gump, context menu, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE flag changes.

Mechanical friction learned:

- A controlled follower's obvious `GetDirectionTo` step can fail on a single impassable static even when the land tile is grass.
- Static `0x1778` is a real rock blocker here, not scenery like the earlier white flowers.
- `DoMoveImpl` does not immediately pathfind on this blocked step; it first enters a random auto-turn attempt that can change the fox's visible tile.
- This random branch changes survival state, so I stop instead of pretending the pet regrouped.

Next pressure:

The next honest run starts with the same positions, but the fox has a new unresolved follow branch at the rock. I either need a fresh scan and a normal player move that gives the fox a cleaner line, or I need to explicitly resolve the random auto-turn branch without calling that safety.

## Run 98 - The Fox Sidesteps The Rock

I start in the same little empty patch at `Point3D(1208,1344,0)`, still facing southwest. Nothing charitable appears while I stand there: no gump, no context menu, no target cursor, no road, no sign, no water, no corpse, no chest, no vendor, no shelter, and no named region. The screen still has only my controlled fox and the old horse on the northwest edge. The live-state item and spawner filters are empty inside the 18-tile rectangle. The `Ruins` marker is still map-overlay knowledge, not a visible ruin.

The horse remains a distraction, not a threat or a mount. Its `AI_Animal` and `FightMode.Aggressor` path still has no aggressor, aggressed, faction, or ethic state to feed `AcquireFocusMob`. The swamp drake is still off-screen and still carried as the private `m_NextMove` risk; I do not turn that into safety. The immediate visible problem is my fox hung up on a rock, so I wait through the follower tick and finally let the random auto-turn resolve.

**Beat 1**

I wait for the controlled fox's follow AI.

The same path resumes: `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, and `DoMove(Direction.Left)`. The first attempt is still bad. `Point3D(1209,1341,0)` is grass, but static `0x1778` is a real rock there: impassable, height 2, sitting in the fox's body space. `MovementImpl.CheckMovement` rejects it.

This time I commit the blocked-move fork. `Utility.RandomDouble()` falls below `0.6`, so `DoMoveImpl` uses offset `-1`. The fox turns from southwest to south with `TurnInternal(-1)` and tries `Point3D(1210,1341,0)`. That tile is dry grass `0x4` with zero statics, and the live snapshot has no saved mobile or visible item standing there. `Mobile.Move` accepts it.

The fox steps one tile south to `Point3D(1210,1341,0)`. I do not move. The fox is now dx `2`, dy `-3`, floored distance `3`, which puts it back inside the remembered `2..3` heel band. No guard order, attack, body-block, combatant, focus mob, drake AI, breath, bite, damage, target cursor, gump, context menu, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE flag changes.

I stop here because the random branch changed what I can see. The next choice should start from the refreshed screen, not from the old panic shape around the rock.

Mechanical friction learned:

- The rock at `1209,1341` blocks the direct follower step even though the land tile under it is grass.
- `DoMoveImpl` can salvage a blocked creature step with a random auto-turn before pathfinding.
- The south auto-turn branch is mechanically better here: it puts the fox back in the follow band; the west branch would have left it stretched.
- Regrouping the pet still is not protection. It is only pet movement.

Next pressure:

Mira remains at `Point3D(1208,1344,0)`, facing southwest. The fox is back in band at `Point3D(1210,1341,0)`. The horse is still only a visible edge animal, the swamp drake remains carried off-screen risk, and the next honest action needs a fresh scan before I decide whether to keep retreating or turn toward the `Ruins` overlay marker.

## Run 99 - The Horse Falls Off The Screen

I start at `Point3D(1208,1344,0)`, facing southwest. The fresh live-state scan is still stingy: the only saved mobile inside my update rectangle is the same horse at `1196,1326`, exactly on the northwest edge at dx `-12`, dy `-18`. My fox is visible because I dragged its simulated state forward, not because the old save knows it followed me; it stands at `1210,1341`, close enough for the `2..3` heel band. There are zero visible saved items, zero running spawner homes inside the rectangle, no road, no sign, no water, no chest, no corpse, no shelter, no named region, no gump, no context menu, and no target cursor.

The horse gets acknowledged and left alone. It is `AI_Animal` with `FightMode.Aggressor`, but the no-aggressor gate still leaves it without a focus target or combatant. The swamp drake is even farther off-screen; I keep both of its private movement-timer outcomes carried and do not call that safety. With the fox in band and no new shiny thing on the ground, I keep walking away.

**Beat 1**

I press southwest.

Because I am already facing `Direction.Left`, this is a real step. `Mobile.Move` reaches `MovementImpl.CheckMovement`, and the server-order Sosaria terrain read checks `map1.mul`, `staidx1.mul`, and `statics1.mul`. The forward tile `Point3D(1207,1345,0)` is grass `0x4` with zero statics. The two player diagonal side-check tiles, `Point3D(1208,1345,0)` and `Point3D(1207,1344,0)`, are grass `0x3` and `0x3`, also with zero statics. The live snapshot has no saved mobile or visible item on the destination or either side-check tile, and the Sosaria region scan names nothing there.

Mira steps to `Point3D(1207,1345,0)`, still facing southwest. The fox does not move on my keypress, so it stretches to dx `3`, dy `-4`, floored distance `5`. The horse drops outside the update rectangle at dy `-19`. The screen is now just me, my fox, and grass.

**Beat 2**

I wait for the fox instead of outrunning it.

The controlled AI path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels still remembers `FollowersMax = 7`, so the wanted band is `2..3` tiles. From the fox at `Point3D(1210,1341,0)` to me at `Point3D(1207,1345,0)`, `GetDirectionTo` returns `Direction.Left`.

The forward tile `Point3D(1209,1342,0)` is grass `0x5` with zero statics. One diagonal side tile is the old rock at `Point3D(1209,1341,0)`, but the other side tile `Point3D(1210,1342,0)` is open grass `0x6` with zero statics. Since this is a controlled creature, not a player, the diagonal check only needs one side to pass. The fox steps southwest to `Point3D(1209,1342,0)` and lands back at dx `2`, dy `-3`, floored distance `3`.

No guard order, attack, combatant, focus mob, body-block, drake tick, breath, bite, damage, UI, target cursor, item, hunger, thirst, skill, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE state changes. This is leash management, nothing heroic.

**Beat 3**

I press southwest again while the fox is close.

`Mobile.Move` runs the same real movement path because I am still facing `Direction.Left`. The forward tile `Point3D(1206,1346,0)` is grass `0x4` with zero statics. The player side checks `Point3D(1207,1346,0)` and `Point3D(1206,1345,0)` are grass `0x6` and `0x3`, also with zero statics. No live saved mobile or visible item stands on those tiles, and no Sosaria region rectangle catches the destination.

Mira moves to `Point3D(1206,1346,0)`. The fox stays at `Point3D(1209,1342,0)` until its own timer, stretched back to dx `3`, dy `-4`, floored distance `5`. The saved horse is gone from the screen, the swamp drake is still not visible from either carried outcome, and the nearest world-map lure is still only the `Ruins` marker, now about 103 Chebyshev tiles away. I stop because the hourly three-beat batch is done and the fox needs another follow tick before I should keep walking.

Mechanical friction learned:

- A visible horse on the edge can vanish from the client rectangle after one retreat step without becoming a pet, mount, threat, or resource.
- A controlled creature can pass a diagonal follow step when one side tile is blocked by the same rock, because the non-player diagonal gate only requires one side tile to be passable.
- Player diagonal movement is stricter; both side-check tiles must pass, and both southwest steps here did.
- The `Ruins` marker is getting closer only as map overlay knowledge. Nothing in-world has appeared.

Next pressure:

Mira ends at `Point3D(1206,1346,0)`, facing southwest. The fox is at `Point3D(1209,1342,0)`, outside the heel band at floored distance `5`. The screen has no live saved mobiles or items other than the simulated controlled fox. The next honest beat should fresh-scan, carry the drake risk without resolving it, and probably wait for one fox follow tick before any more travel.

## Run 100 - I Wait The Fox Back In

I start at `Point3D(1206,1346,0)`, facing southwest. The live-state rectangle is empty: zero saved mobiles, zero saved items, and zero running spawner homes inside the client view. The only thing on my screen that matters is my own fox, dragged forward by the simulation to `Point3D(1209,1342,0)`, too far away at floored distance `5`. The old horse is gone from the rectangle, not gone from the world. The swamp drake is even farther off-screen, still the same carried private-timer risk, not a solved danger.

**Beat 1**

I wait for the fox.

The follow path is still `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels keeps the desired band at `2..3`. From `Point3D(1209,1342,0)` to me, `GetDirectionTo` returns `Direction.Left`.

This time the direct tile is not the rock. `Point3D(1208,1343,0)` is grass `0x6`, and the two non-player side checks, `Point3D(1209,1343,0)` and `Point3D(1208,1342,0)`, are grass with zero statics. The fox steps southwest and lands at floored distance `3`.

**Beat 2**

With the fox back in band, I press southwest once.

Because I am already facing `Direction.Left`, `Mobile.Move` runs the real movement block. The forward tile `Point3D(1205,1347,0)` is grass `0x6`; the player side checks `Point3D(1206,1347,0)` and `Point3D(1205,1346,0)` are grass `0x6` and `0x4`. All three have zero statics and no live saved blocker.

Mira moves to `Point3D(1205,1347,0)`. No region name, road, sign, water source, chest, corpse, shelter, item, gump, context menu, target cursor, or combat feedback appears. The step stretches the fox back to floored distance `5`.

**Beat 3**

I wait for the fox again.

`WalkMobileRange` repeats the same pressure. From `Point3D(1208,1343,0)` toward me at `Point3D(1205,1347,0)`, `GetDirectionTo` again returns `Direction.Left`. The target `Point3D(1207,1344,0)` is grass `0x3`; the side checks `Point3D(1208,1344,0)` and `Point3D(1207,1343,0)` are grass with zero statics. The fox steps southwest and ends inside the heel band at dx `2`, dy `-3`, floored distance `3`.

Nothing else changes. No drake AI tick, Combatant, FocusMob, breath, bite, damage, UI, target cursor, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE state changes. The `Ruins` marker is a little closer at about 102 Chebyshev tiles, but it is still only map overlay knowledge.

Mechanical friction learned:

- The next southwest pocket is still dry Sosaria grass, and the server-order terrain read matters before trusting any route.
- A player diagonal step still needs both side checks clear; the fox only needed its non-player movement path and had both sides clear anyway.
- Waiting the pet in is movement, not protection. It keeps the follower close but does not run guard, attack, body-blocking, or hostile AI.
- The drake risk remains carried unresolved. Being off-screen and outside its saved range scan is not proof that it stayed, moved, or became safe.

Next pressure:

Mira ends at `Point3D(1205,1347,0)`, still facing southwest. The fox is at `Point3D(1207,1344,0)`, inside the follow band. The screen has only the simulated controlled fox and empty grass. The next run should fresh-scan, then decide whether I keep retreating southwest or start angling toward the `Ruins` marker without pretending that marker is a visible shelter.

## Run 101 - I Keep The Empty Grass Between Us

I start at `Point3D(1205,1347,0)`, already facing southwest. The live screen has no saved creatures, no saved items, no running spawner home inside the update rectangle, and no open UI. The only visible thing I can honestly react to is my own fox at `Point3D(1207,1344,0)`, close enough for the `2..3` heel band. The swamp drake is still off-screen and still only a carried private-timer risk, not proof of safety. The `Ruins` marker is closer at 102 tiles, but it is still map-overlay knowledge, not a road or shelter.

**Beat 1**

I press southwest.

Because I am already facing `Direction.Left`, this is a real step. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1204,1348,0)` is grass `0x3`, and the player side checks `Point3D(1205,1348,0)` and `Point3D(1204,1347,0)` are also grass `0x3`. All three have zero statics, no saved mobile, no visible item, and no named region rectangle.

Mira moves to `Point3D(1204,1348,0)`. The fox does not move with my keypress, so it stretches to floored distance `5`. The screen remains empty except for that simulated follower.

**Beat 2**

I wait for the fox instead of outrunning it.

The controlled AI path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels still wants the `2..3` band. From `Point3D(1207,1344,0)` toward me, `GetDirectionTo` returns `Direction.Left`.

The fox target `Point3D(1206,1345,0)` is grass `0x3`. The non-player side checks `Point3D(1207,1345,0)` and `Point3D(1206,1344,0)` are grass `0x4` and `0x6`. All are clear. The fox steps southwest and lands back at floored distance `3`. This is leash management, not guarding.

**Beat 3**

I take one more southwest step while the fox is close.

`Mobile.Move` runs the real movement path again. The forward tile `Point3D(1203,1349,0)` is grass `0x5`; the player side checks `Point3D(1204,1349,0)` and `Point3D(1203,1348,0)` are grass `0x3` and `0x6`. They have zero statics and no live saved blocker.

Mira moves to `Point3D(1203,1349,0)`. The fox stays at `Point3D(1206,1345,0)` until its own timer, stretched back to dx `3`, dy `-4`, floored distance `5`. The saved snapshot still shows zero visible mobiles, zero visible items, and zero running spawner homes in the final rectangle. The swamp drake remains outside the client rectangle whether it stayed at `1234,1319` or took the unchosen homeward branch toward `1235,1320`; I still do not pick either outcome.

Mechanical friction learned:

- Empty grass can still be the correct move when the only alternative is standing near a remembered drake branch.
- Player movement never drags the fox along; one keypress is enough to stretch it out of the follow band again.
- The `Ruins` marker is now about 100 Chebyshev tiles away, but nothing in-world has appeared.
- The carried drake fork remains unresolved risk. More distance is not a verdict on its private timer.

Next pressure:

Mira ends at `Point3D(1203,1349,0)`, facing southwest. The fox is at `Point3D(1206,1345,0)`, outside the heel band at floored distance `5`. The next honest beat should fresh-scan, then probably wait for the fox before any more retreat or marker-directed travel.

## Run 102 - Empty Grass Still Needs A Leash

I start at `Point3D(1203,1349,0)`, facing southwest. The screen is bare in the saved live snapshot: no saved mobiles, no saved items, no visible spawner object, no region name, no road, no chest, no corpse, no water source, no gump, no context menu, and no target cursor. The only thing that matters is my own fox at `Point3D(1206,1345,0)`, too far out at floored distance `5`.

The swamp drake is still the old carried risk, not a solved problem. If it stayed at `1234,1319`, it is off-screen; if its unexported movement timer fired and it stepped toward `1235,1320`, that is also off-screen. I still do not get to call either branch true. The world map keeps whispering: `Ruins` is 100 tiles away, but that is only overlay knowledge, not visible stone or safety. If I go southwest, Ruins actually gets one tile farther while the Mines of Morinia and West moongate markers get one tile closer. None of those markers are on my screen.

**Beat 1**

I wait for the fox.

The follower path is the familiar one: `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels still has the remembered `FollowersMax = 7`, so the wanted band is still `2..3`.

From `Point3D(1206,1345,0)` toward me at `Point3D(1203,1349,0)`, the fox picks southwest. The forward tile `Point3D(1205,1346,0)` is dry grass `0x4` with zero statics. The side checks `Point3D(1206,1346,0)` and `Point3D(1205,1345,1)` are also grass with zero statics. The fox steps to `Point3D(1205,1346,0)` and lands back inside the band at dx `2`, dy `-3`, floored distance `3`.

**Beat 2**

I press southwest once.

Because I am already facing `Direction.Left`, this is a real movement step, not a turn. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1202,1350,0)` is grass `0x3`, and the player side checks `Point3D(1203,1350,0)` and `Point3D(1202,1349,0)` are grass `0x4` and `0x6`. All three have zero statics. The live snapshot has no saved mobile or visible item on those tiles, and no Sosaria region rectangle catches the destination.

Mira moves to `Point3D(1202,1350,0)`. The fox does not move on my keypress, so it stretches back to dx `3`, dy `-4`, floored distance `5`. The post-step screen is still empty except for the simulated follower.

**Beat 3**

I wait for the fox again.

`BaseAI.Obey` runs the same Follow order. The fox at `Point3D(1205,1346,0)` points southwest toward me. The forward tile `Point3D(1204,1347,0)` is grass `0x3`; the side checks `Point3D(1205,1347,0)` and `Point3D(1204,1346,0)` are grass `0x6` and `0x3`. Zero statics, no live blocker, no region name.

The fox steps to `Point3D(1204,1347,0)` and ends inside the heel band at dx `2`, dy `-3`, floored distance `3`. No guard order, attack, combatant, focus mob, body-blocking, drake tick, breath, bite, damage, UI, target cursor, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE state changes.

Mechanical friction learned:

- Empty-screen travel is still not free movement; the pet timer is its own beat.
- Southwest retreat keeps increasing distance from the remembered swamp drake branch, but it moves me away from the Ruins marker.
- The live snapshot proves only that no saved visible entities are in the current rectangle. Broad invisible spawner ranges still touch the area, so future live wandering is not disproven.
- A controlled follower being close is not protection. It is just a leash state.

Next pressure:

Mira ends at `Point3D(1202,1350,0)`, still facing southwest. The fox is at `Point3D(1204,1347,0)`, inside the 2..3 follow band. The next honest run starts with a fresh empty-grass scan and a navigation decision: keep retreating southwest from the drake memory, or reconsider because the Ruins overlay is getting farther away.

## Run 103 - The Ruins Drift Farther Off

I start at `Point3D(1202,1350,0)`, facing southwest. The live-state rectangle is still bare: zero saved mobiles, zero saved items, and zero running spawner locations. The only thing on the screen is my own fox at `Point3D(1204,1347,0)`, close enough to count as under control instead of stranded. No gump, no context menu, no target cursor, no water, no road, no sign, no corpse, no chest, no shelter, and no named region. The `Ruins` marker is now a map-overlay lure at 101 tiles, not something I can see.

The swamp drake stays as the old carried risk. If it stayed at `Point3D(1234,1319,0)`, it is off-screen; if its unexported movement timer fired toward `Point3D(1235,1320,0)`, that is also off-screen. I still do not get to choose either outcome or call it safe. The least stupid thing is still more distance, even if the map overlay keeps making me feel like I am walking away from something useful.

**Beat 1**

I press southwest.

Because I am already facing `Direction.Left`, `Mobile.Move` runs the real movement path. The server-order Sosaria terrain read uses `map1.mul`, `staidx1.mul`, and `statics1.mul`. The forward tile `Point3D(1201,1351,0)` is grass `0x5` with zero statics. The player diagonal side-check tiles, `Point3D(1202,1351,0)` and `Point3D(1201,1350,0)`, are grass `0x4` and `0x6`, also with zero statics. No live saved mobile or item stands on those tiles, and no named region catches the destination.

Mira steps to `Point3D(1201,1351,0)`. The fox does not move on my keypress, so it stretches to dx `3`, dy `-4`, floored distance `5`. The screen remains just grass and follower trouble.

**Beat 2**

I wait for the fox.

The controlled AI path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels still remembers `FollowersMax = 7`, so the desired band is `2..3`. From `Point3D(1204,1347,0)` toward me, `GetDirectionTo` returns `Direction.Left`.

The fox target `Point3D(1203,1348,0)` is grass `0x6`. The non-player side checks `Point3D(1204,1348,0)` and `Point3D(1203,1347,0)` are grass `0x3` and `0x3`. Zero statics, no live blocker. The fox steps southwest and lands back inside the band at dx `2`, dy `-3`, floored distance `3`.

**Beat 3**

I press southwest again while the fox is close.

`Mobile.Move` accepts the second player step. The forward tile `Point3D(1200,1352,0)` is grass `0x4`; the player side checks `Point3D(1201,1352,0)` and `Point3D(1200,1351,0)` are grass `0x4` and `0x5`. All three have zero statics, no live saved blocker, and no named region.

Mira moves to `Point3D(1200,1352,0)`. The fox stays at `Point3D(1203,1348,0)` until its own timer, stretched back to floored distance `5`. The final live-state rectangle still has zero saved mobiles, zero saved items, and zero running spawner locations. The `Ruins` marker is now 103 tiles away, while the Mines of Morinia and West moongate markers get slightly closer. None of that is screen-visible help.

Mechanical friction learned:

- Two more southwest steps are still dry Sosaria grass with no static blockers.
- The map overlay can pull my attention, but it is not an entity, region, quest flag, road, shelter, or safety proof.
- The pet leash rhythm keeps mattering: one player step stretches the fox, one pet tick repairs it, and the next player step stretches it again.
- The carried drake branch remains unresolved risk, not a verdict.

Next pressure:

Mira ends at `Point3D(1200,1352,0)`, facing southwest. The fox is at `Point3D(1203,1348,0)`, outside the `2..3` heel band at floored distance `5`. The next honest beat should fresh-scan, then probably wait the fox back in before any more travel.

## Run 104 - The Fox Catches Up Twice

I start at `Point3D(1200,1352,0)`, facing southwest. The live-state screen is still empty in the way that makes me suspicious: no saved mobiles, no saved items, no running spawner homes, no named region, no corpse, no chest, no sign, no water source, no gump, no context menu, and no target cursor. The only visible moving obligation is my fox at `Point3D(1203,1348,0)`, too far out at floored distance `5`.

The swamp drake is not on the screen. If it stayed at `Point3D(1234,1319,0)`, it is far northeast; if its private movement timer fired and it tried `Point3D(1235,1320,0)`, that is also far northeast. I still do not get to pick either branch or call it safe. The map overlay keeps `Ruins`, `Mines of Morinia`, `West`, and `Chamber of Bane` in my head, but none of those markers is an in-world thing I can click.

**Beat 1**

I wait for the fox.

The controlled pet timer goes through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels is still using the remembered spacing value `7`, so the fox wants the `2..3` band.

From `Point3D(1203,1348,0)` toward me, southwest is open. The forward tile `Point3D(1202,1349,0)` is grass `0x6` with zero statics. The non-player diagonal side checks `Point3D(1203,1349,0)` and `Point3D(1202,1348,1)` are grass `0x5` and `0x6`, also with zero statics. The fox steps to `Point3D(1202,1349,0)` and lands back at dx `2`, dy `-3`, floored distance `3`.

**Beat 2**

With the fox close again, I press southwest.

Because I am already facing `Direction.Left`, `Mobile.Move` runs the real movement path. `MovementImpl.CheckMovement` accepts the forward tile `Point3D(1199,1353,0)`, grass `0x3`, and the player side-check tiles `Point3D(1200,1353,0)` and `Point3D(1199,1352,0)`, grass `0x4` and `0x6`. All three have zero statics and no saved blocker.

Mira moves to `Point3D(1199,1353,0)`. The fox stays where it was during my keypress, so it stretches back out to dx `3`, dy `-4`, floored distance `5`. The shifted rectangle still has no saved mobile, saved item, or running spawner location.

**Beat 3**

I wait again. Outrunning the only thing I own is not survival.

The fox repeats the Follow path from `Point3D(1202,1349,0)` toward me. The forward tile `Point3D(1201,1350,0)` is grass `0x6` with zero statics. The non-player side checks `Point3D(1202,1350,0)` and `Point3D(1201,1349,1)` are grass `0x3` and `0x4`, also clear. It steps to `Point3D(1201,1350,0)` and ends at floored distance `3`.

No gump, context menu, target cursor, combatant, focus mob, breath, bite, damage, pet order, ownership, follower count, hunger, thirst, skill, quest, discovery, fame, karma, or PvP/PvE state changed. I am just putting grass behind me and keeping the fox from falling off the leash.

Mechanical friction learned:

- Waiting for a controlled follower is a real beat; the fox does not slide along with my movement packet.
- The southwest line from `Point3D(1200,1352,0)` to `Point3D(1199,1353,0)` is still ordinary Sosaria grass with no static blockers.
- The world-map markers are becoming navigation pressure, but they are not screen entities, safety, or proof of a route.
- The carried swamp-drake timer branch remains unresolved risk, not permission to forget it.

Next pressure:

Mira ends at `Point3D(1199,1353,0)`, facing southwest. The fox is at `Point3D(1201,1350,0)`, inside the `2..3` heel band. The next honest run starts with a fresh empty-screen scan, then a choice: keep retreating southwest from the remembered drake pressure, or let the overlay markers start steering the route.

## Run 105 - The Leash Stretches Again

I start at `Point3D(1199,1353,0)`, facing southwest, with the fox close at `Point3D(1201,1350,0)`. The saved live-state screen is still the suspicious kind of empty: zero visible saved mobiles, zero visible saved items, zero running spawner homes, no corpse, no chest, no sign, no water, no gump, no context menu, and no target cursor. The only visible mobile is my own simulated follower.

The world-map overlay keeps pulling at my attention. `Ruins` is 104 tiles away, `Mines of Morinia` is 177, and the West moongate is 217, but none of those is a visible shelter or a thing I can click. The swamp drake is still the old carried branch, not a solved animal. If it stayed at `Point3D(1234,1319,0)`, it is off-screen; if its unexported movement timer fired toward `Point3D(1235,1320,0)`, that is off-screen too. I do not get to pick either story.

**Beat 1**

I press southwest.

Because I am already facing `Direction.Left`, this is a real step. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The server-order Sosaria read uses `map1.mul`, `staidx1.mul`, and `statics1.mul`. The forward tile `Point3D(1198,1354,0)` is grass `0x5`; the player side checks `Point3D(1199,1354,0)` and `Point3D(1198,1353,0)` are grass `0x3` and `0x5`. All three tiles have zero statics, no visible saved blocker, and no named region.

Mira moves to `Point3D(1198,1354,0)`. The fox does not move with my keypress, so it stretches to dx `3`, dy `-4`, floored distance `5`. The shifted screen remains empty except for the follower.

**Beat 2**

I wait for the fox.

The controlled pet timer runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels still wants the `2..3` band.

From `Point3D(1201,1350,0)` toward me, the fox picks southwest. The forward tile `Point3D(1200,1351,0)` is grass `0x5`; the non-player side checks `Point3D(1201,1351,0)` and `Point3D(1200,1350,0)` are grass `0x5` and `0x6`. Zero statics, no visible saved blocker. The fox steps to `Point3D(1200,1351,0)` and lands back at floored distance `3`.

**Beat 3**

With the fox close again, I press southwest once more.

`Mobile.Move` accepts the step because I am still facing `Direction.Left`. The forward tile `Point3D(1197,1355,0)` is grass `0x5`; the side checks `Point3D(1198,1355,0)` and `Point3D(1197,1354,0)` are grass `0x5` and `0x4`. All are clear grass with zero statics, no saved blocker, and no named region. Mira moves to `Point3D(1197,1355,0)`.

The fox stays at `Point3D(1200,1351,0)` until its own timer and stretches back outside the heel band at floored distance `5`. The final live-state rectangle still has zero visible saved mobiles, zero visible saved items, and zero running spawner locations. `Ruins` is now 106 tiles away; `Mines of Morinia` and the West moongate are slightly closer, but still only map overlay pressure.

Mechanical friction learned:

- Two more southwest player steps are still dry Sosaria grass, but every diagonal step still needs both side tiles checked.
- The fox only catches up on its own AI beat; my movement packet keeps stretching it out again.
- The overlay markers are useful for orientation, not proof of roads, safety, vendors, water, shelter, or discoveries.
- The swamp drake remains a carried unresolved risk. Off-screen distance is not a combat result.

Next pressure:

Mira ends at `Point3D(1197,1355,0)`, still facing southwest. The fox is at `Point3D(1200,1351,0)`, outside the `2..3` heel band at floored distance `5`. The next honest beat should fresh-scan, then probably wait for the fox before any more travel.

## Run 106 - I Stop Outrunning The Fox

I start at `Point3D(1197,1355,0)`, facing southwest. The live-state rectangle is still empty in the saved world: zero visible saved mobiles, zero visible saved items, and zero running spawner locations. That does not mean the wilderness is dead. Broad invisible spawner ranges still overlap this part of Sosaria, and the old swamp-drake timer branch stays carried unresolved. On the actual screen, the only mobile I can honestly react to is my own fox at `Point3D(1200,1351,0)`, stretched out at floored distance `5`.

The map overlay is starting to feel like a bad conscience. `Ruins` is 106 tiles away, `Mines of Morinia` is 175, and the West moongate is 215, but none of those markers is a visible road, shelter, vendor, water source, or safety flag. If I keep moving southwest I get farther from Ruins and slightly closer to Morinia and West. That is navigation pressure, not a discovery.

**Beat 1**

I wait for the fox.

The pet timer runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, and `WalkMobileRange`. FriendsAvoidHeels is still using the remembered spacing value `7`, so the fox wants the `2..3` band. From `Point3D(1200,1351,0)` toward me at `Point3D(1197,1355,0)`, `GetDirectionTo` again picks `Direction.Left`.

The fox's target `Point3D(1199,1352,0)` is grass `0x6`. The non-player diagonal side checks `Point3D(1200,1352,0)` and `Point3D(1199,1351,0)` are grass `0x4` and `0x6`. All three tiles have zero statics and no live saved blocker. The fox steps southwest and lands at dx `2`, dy `-3`, floored distance `3`.

**Beat 2**

With the fox back in the leash band, I press southwest once.

Because I am already facing `Direction.Left`, `Mobile.Move` takes the real movement path instead of just turning me. `MovementImpl.CheckMovement` accepts the forward tile `Point3D(1196,1356,0)`, grass `0x3`, and the player diagonal side-check tiles `Point3D(1197,1356,0)` and `Point3D(1196,1355,0)`, grass `0x5` and `0x4`. Zero statics, no saved mobile, no visible item, and no named Sosaria region catches the destination.

Mira moves to `Point3D(1196,1356,0)`. The fox does not move on my keypress, so it stretches back out to dx `3`, dy `-4`, floored distance `5`.

**Beat 3**

I wait again.

The controlled fox repeats the same Follow path from `Point3D(1199,1352,0)` toward me. `GetDirectionTo` picks `Direction.Left`; the target `Point3D(1198,1353,0)` is grass `0x5`; the side checks `Point3D(1199,1353,0)` and `Point3D(1198,1352,0)` are grass `0x3` and `0x5`. Zero statics, no live blocker. The fox steps southwest and ends inside the heel band at dx `2`, dy `-3`, floored distance `3`.

No gump, context menu, target cursor, combatant, focus mob, breath, bite, damage, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE state changes. I put one more tile of grass between me and the remembered drake pressure, but I do not get to call the drake safe, gone, stationary, or moved.

Mechanical friction learned:

- Waiting the fox in is still the real survival tax of owning a follower; movement packets do not drag it along.
- The southwest step to `Point3D(1196,1356,0)` is ordinary dry Sosaria grass, but the server still requires both diagonal side checks to be open.
- The final rectangle still has no saved visible entities other than the simulated controlled fox, but invisible spawner ranges and live wandering remain unresolved.
- The world-map overlay is now more tempting than useful: `Ruins` drifted to 107 tiles away, `Mines of Morinia` to 174, and West to 214. None of them is on screen.

Next pressure:

Mira ends at `Point3D(1196,1356,0)`, facing southwest. The fox is at `Point3D(1198,1353,0)`, inside the `2..3` heel band. The next honest run starts with a fresh empty-grass scan and a route decision: keep retreating southwest from the carried drake branch, or stop letting the overlay markers pull me in circles without visible terrain proof.

## Run 107 - The Marker Pull Loses To The Leash

I start at `Point3D(1196,1356,0)`, facing southwest, with the fox close at `Point3D(1198,1353,0)`. The saved live-state scan is empty again: no saved visible mobiles, no visible items, and no running spawner locations inside the 18-tile rectangle. That does not make it safe. It only means the only mobile I can honestly act around is my own fox, while the swamp drake remains the old carried private-timer branch far off to the northeast.

The map overlay keeps changing its argument. `Ruins` is now 107 tiles away, `Mines of Morinia` is 174, and the West moongate is 214. If I keep going southwest, Ruins gets farther while Morinia and West get closer. None of those is a road, shelter, vendor, water source, or visible safety.

**Beat 1**

I press southwest.

Because I am already facing `Direction.Left`, `Mobile.Move` runs the real movement path. The server-order Sosaria read uses `map1.mul`, `staidx1.mul`, and `statics1.mul`. The forward tile `Point3D(1195,1357,0)` is grass `0x6`; the player diagonal side-check tiles `Point3D(1196,1357,0)` and `Point3D(1195,1356,0)` are grass `0x5` and `0x4`. All three have zero statics, no saved mobile, no visible item, and no named Sosaria region catches the destination.

Mira steps to `Point3D(1195,1357,0)`. The fox stays at `Point3D(1198,1353,0)`, so my one keypress stretches it back outside the heel band at dx `3`, dy `-4`, floored distance `5`. The shifted scan is still empty except for the simulated follower.

**Beat 2**

I wait for the fox.

The controlled pet path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels is still using the remembered spacing value `7`, so the fox wants the `2..3` band. From `Point3D(1198,1353,0)` toward me, southwest is open.

The fox target `Point3D(1197,1354,0)` is grass `0x4`; the non-player side checks `Point3D(1198,1354,0)` and `Point3D(1197,1353,0)` are grass `0x5` and `0x4`. Zero statics, no live blocker. The fox steps southwest and lands back at dx `2`, dy `-3`, floored distance `3`.

**Beat 3**

With the fox close again, I press southwest once more.

`Mobile.Move` accepts the second player step. The forward tile `Point3D(1194,1358,0)` is grass `0x4`; the player side checks `Point3D(1195,1358,0)` and `Point3D(1194,1357,0)` are grass `0x4` and `0x5`. All three have zero statics, no saved blocker, and no named region. Mira moves to `Point3D(1194,1358,0)`.

The fox stays at `Point3D(1197,1354,0)` until its own timer, stretched back outside the heel band at floored distance `5`. The final live-state rectangle still has zero visible saved mobiles, zero visible saved items, and zero running spawner locations. The saved swamp drake at `Point3D(1234,1319,0)` is not in the client rectangle and not in its own range-16 scan of Mira; the possible due branch toward `Point3D(1235,1320,0)` is also off-screen. I still do not get to call either branch true.

Mechanical friction learned:

- Empty grass still costs real movement checks: the diagonal player steps require the forward tile and both side tiles to pass.
- The fox only repairs the leash on its own AI beat; every extra player step can strand it again.
- The world-map overlay is steering pressure, not a visible route or proof of safety.
- The carried swamp-drake timer remains unresolved risk, not a combat result.

Next pressure:

Mira ends at `Point3D(1194,1358,0)`, facing southwest. The fox is at `Point3D(1197,1354,0)`, outside the `2..3` heel band at floored distance `5`. The next honest beat should fresh-scan, then probably wait the fox back in before more marker-driven travel.

## Run 108 - Jungle Under The Grass

I start at `Point3D(1194,1358,0)`, facing southwest, with the fox lagging at `Point3D(1197,1354,0)`. The live snapshot is still empty inside the 18-tile rectangle: zero saved visible mobiles, zero visible items, and zero running spawner locations. The only mobile on my screen is the one I made happen, my controlled fox. That matters more than the map overlay.

The overlay says `Ruins` is 109 tiles away, `Mines of Morinia` is 172, and the West moongate is 212. If I keep moving southwest, Ruins gets farther and Morinia/West get closer. None of those names is a road, shelter, vendor, water source, corpse, chest, or safety flag. The old swamp drake is still a carried private-timer risk off to the northeast, not a solved thing.

**Beat 1**

I wait for the fox first.

The controlled pet path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels is still using spacing `7`, so the fox wants the `2..3` band.

From `Point3D(1197,1354,0)` toward me, the fox picks southwest. The target `Point3D(1196,1355,0)` is grass `0x4` with zero statics. One side check is open grass at `Point3D(1197,1355,0)`. The other has leaves `0xD4C` at `Point3D(1196,1354,0)`, but tiledata marks them as passable foliage, not a wall. The fox steps to `Point3D(1196,1355,0)` and lands back at floored distance `3`.

**Beat 2**

Now I press southwest.

Because I am already facing `Direction.Left`, `Mobile.Move` reaches `MovementImpl.CheckMovement`. This tile is not just more grass in my head: the server-order Sosaria read says the forward tile `Point3D(1193,1359,0)` is jungle `0x583`, dry and passable, with zero statics. The player side checks `Point3D(1194,1359,0)` and `Point3D(1193,1358,0)` are passable grass/jungle with zero statics. No saved blocker and no named region catches the destination.

Mira moves to `Point3D(1193,1359,0)`. The fox stays where it is until its timer, so one keypress stretches it back to floored distance `5`.

**Beat 3**

I wait again. I do not like walking with the fox stretched out behind me.

The same Follow path runs. From `Point3D(1196,1355,0)` toward me, `GetDirectionTo` again picks `Direction.Left`. The target `Point3D(1195,1356,0)` is grass `0x4` with zero statics. The side checks are open grass at `Point3D(1196,1356,0)` and passable leaves `0xD4B` over grass at `Point3D(1195,1355,0)`. The fox steps to `Point3D(1195,1356,0)` and ends at dx `2`, dy `-3`, floored distance `3`.

No gump, context menu, target cursor, combatant, focus mob, breath, bite, damage, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower count, pet order, fame, karma, or PvP/PvE state changes. The final rectangle is still empty except for the simulated follower. The saved drake is off-screen at dx `41`, dy `-40`; its possible due branch would be off-screen at dx `42`, dy `-39`. I still carry both branches and call neither safe.

Mechanical friction learned:

- The southwest route has reached jungle graphics, but the tile is still dry/passable under the server's map1 file.
- Leaves on the fox's side checks are foliage, not blockers; I can see brush without turning it into a wall.
- The fox-follow rhythm matters: wait, step, wait keeps the follower inside the leash band.
- Overlay markers still only tug at navigation. They do not create visible roads or discoveries.

Next pressure:

Mira ends at `Point3D(1193,1359,0)`, facing southwest. The fox is at `Point3D(1195,1356,0)`, inside the `2..3` heel band. The next run should fresh-scan before choosing a route, because moving farther southwest is now a real decision rather than an automatic fox catch-up.

## Run 109 - The Screen Gets Eyes Again

I start at `Point3D(1193,1359,0)`, facing southwest, with the fox tucked close at `Point3D(1195,1356,0)`. The first live-state rectangle is empty except for that simulated follower: no saved visible mobiles, no visible items, and no running spawner locations. The map overlay keeps tugging southwest: `Ruins` is 110 tiles away, `Mines of Morinia` is 171, and the West moongate is 211. Those are still not roads, shelter, water, guards, or anything I can click.

**Beat 1**

I press southwest.

Because I am already facing `Direction.Left`, this is a real step. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1192,1360,0)` is jungle `0xAD`; the player side checks `Point3D(1193,1360,0)` and `Point3D(1192,1359,0)` are jungle `0x583` and `0xAE`. Server-order `map1.mul`, `staidx1.mul`, and `statics1.mul` say all three are dry, passable, and have zero statics. Mira moves to `Point3D(1192,1360,0)`.

The fox does not move on my keypress, so it stretches to floored distance `5`. The shifted saved-world screen is still empty except for the fox.

**Beat 2**

I wait for the fox.

The pet timer runs the familiar path: `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels still wants the `2..3` band.

The fox target `Point3D(1194,1357,0)` is grass with zero statics. One side check, `Point3D(1195,1357,0)`, is open grass. The other, `Point3D(1194,1356,0)`, has tree and leaf statics, including impassable trees. For my player movement both diagonal sides would matter, but the code only blocks a non-player diagonal when both side checks fail. The fox slips southwest to `Point3D(1194,1357,0)` and lands at floored distance `3`.

**Beat 3**

With the leash repaired, I press southwest again.

`Mobile.Move` accepts the step. The forward tile `Point3D(1191,1361,0)` is jungle `0xAF`; the side checks `Point3D(1192,1361,0)` and `Point3D(1191,1360,0)` are jungle `0x587` and `0xAC`. Zero statics, dry, passable, no saved blocker, no named region. Mira moves to `Point3D(1191,1361,0)`.

Then the screen stops being empty. A toad appears at the west edge at `Point3D(1173,1360,0)`, another toad at the southwest edge at `Point3D(1174,1379,0)`, and a crane beside it at `Point3D(1176,1379,0)`. They are not in my face, but they are visible now. The toads have a teleport timer in code, though both are outside its 10-tile player scan from here; the crane is passive `AI_Animal`/`FightMode.Aggressor`. I do not click them, tame them, attack them, or pretend they are safe scenery.

Mechanical friction learned:

- The southwest route is now jungle underfoot, not just grass, but the server still treats these tiles as dry passable land.
- A controlled non-player diagonal can squeeze past one blocked side tile if the other side is open; a player diagonal needs both side checks.
- The first newly visible saved creatures since the drake are passive edge animals, but visible still means decision pressure.
- My fox is stretched back outside the heel band at floored distance `5`, so more travel without waiting is careless.

Next pressure:

Mira ends at `Point3D(1191,1361,0)`, facing southwest. The fox is at `Point3D(1194,1357,0)`, outside the `2..3` heel band. Two toads and a crane are visible on the west/southwest edge. The next run should fresh-scan, check their passive/timer pressure, and probably wait the fox in before making another route choice.

## Run 110 - I Count The Edge Animals

I start at `Point3D(1191,1361,0)`, still facing southwest. The screen is no longer empty. A toad is barely on the west edge at `Point3D(1173,1360,0)`, another toad sits on the southwest edge at `Point3D(1174,1379,0)`, and a crane stands beside it at `Point3D(1176,1379,0)`. The live snapshot has no visible items and no running spawner locations in the rectangle. The map overlay says `Ruins` is 142 tiles away by straight distance, `Mines of Morinia` is 169, and the West moongate is 251. Still not a road. Still not shelter.

The animal pressure is visible but not immediate combat. The toads are `AI_Animal` with `FightMode.Aggressor` and a five-second teleport timer, but the timer only looks for players inside range `10`; from here I am outside that scan for both toads. The crane is also `AI_Animal`/`FightMode.Aggressor`, and nothing in the state says I attacked it or it attacked me. The old swamp drake is off-screen and still carried as the same unresolved private-timer risk, not solved safety.

**Beat 1**

I wait for the fox.

The controlled pet path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Left)`, and `Mobile.Move`. FriendsAvoidHeels is still using spacing `7`, so the fox wants the `2..3` band.

From `Point3D(1194,1357,0)` toward me, the fox picks southwest. The target `Point3D(1193,1358,0)` is jungle `0x583` with zero statics. One side check, `Point3D(1194,1358,0)`, is open grass. The other, `Point3D(1193,1357,0)`, has an impassable tree plus foliage, but the non-player diagonal rule only fails when both side checks fail. The fox slips to `Point3D(1193,1358,0)` and ends at dx `2`, dy `-3`, floored distance `3`.

I do not click the toad. That would be curiosity while the carried drake branch still tells me to keep actions low-risk. I also do not move toward the edge animals, open a context menu, start taming, attack, use the canteen, or follow the overlay marker. The screen is now a real decision: west has visible wildlife; southwest keeps the fox close but may walk toward the edge animals; stopping here keeps the leash repaired.

Mechanical friction learned:

- Edge animals are still screen pressure even when they are not active combat pressure.
- `FightMode.Aggressor` animals do not acquire me from nothing; without aggressor/aggressed/faction/ethic state, the acquisition gate stays closed.
- A toad teleport timer is not just "visible toad means teleport"; it needs the player inside the toad-centered range-10 scan before it has a target.
- Repairing the fox leash costs a whole beat and changes no player location.

Next pressure:

Mira remains at `Point3D(1191,1361,0)`, facing southwest. The fox is at `Point3D(1193,1358,0)`, back inside the `2..3` heel band. The two toads and crane are still visible on the west/southwest edge. The next honest run should decide whether to identify one edge animal, skirt them, or continue marker-driven travel without pretending the area is safe.

## Run 111 - I Name The West-Edge Toad

I start where I stopped: `Point3D(1191,1361,0)`, facing southwest, with the fox close at `Point3D(1193,1358,0)`. The screen still has the same three edge animals from the saved live snapshot: a small thing at `Point3D(1173,1360,0)`, another at `Point3D(1174,1379,0)`, and a bird-shape beside it at `Point3D(1176,1379,0)`. No saved item, corpse, chest, sign, road, vendor, water source, gump, context menu, target cursor, or running spawner object is visible. The overlay still only whispers: `Ruins` is far southeast, `Mines of Morinia` is west, and the West moongate is farther southwest.

**Beat 1**

I single-click the nearest west-edge creature.

This is not a context menu and not a tame attempt. The normal client look packet goes through `PacketHandlers.LookReq`, finds mobile serial `288077`, checks `CanSee`, and passes `Utility.InUpdateRange`: from me to the creature is dx `18`, dy `1`, which is still inside the inclusive 18-tile rectangle. `Region.OnSingleClick` returns true, `BaseCreature.OnSingleClick` adds no tame/bonded line because the toad is uncontrolled, and `Mobile.OnSingleClick` sends the label.

The label says `a toad`.

That little name makes the screen worse, not better. The class is `AI_Animal`/`FightMode.Aggressor`, tamable in theory, and carrying the same `GiantToad.TeleportTimer` code as the larger frog family. But the timer only chooses a player inside the toad-centered range-10 scan after a random tick and harm/LOS/enemy gates. I am still outside that scan at dx `18`, dy `1`. The second toad and crane remain unlabeled by click this run, though their saved records are still visible at the southwest edge. The fox stays beside me because I did not move or wait a pet AI tick.

Mechanical friction learned:

- A visible edge creature can be labeled through ordinary single-click flow even at dx `18`; that is the client update rectangle, not taming reach.
- The label gives player-facing knowledge only. It does not open a menu, select `Tame`, start targeting, roll skill, change ownership, move anyone, or make the creature safe.
- A toad's teleport pressure is gated by a range-10 scan from the toad plus random and harmful/enemy checks; merely seeing or labeling it is not a teleport result.
- The fox remains inside the heel band, so the next decision is route choice around named edge wildlife, not follower repair.

Next pressure:

Mira remains at `Point3D(1191,1361,0)`, facing southwest. The fox remains at `Point3D(1193,1358,0)`, still inside the `2..3` heel band. The west-edge creature is now known to the player as `a toad`; the southwest toad and crane remain visible edge pressure. The next honest run should either skirt the labeled toad, identify the other edge animals, or choose a route without pretending these animals, the invisible spawners, or the carried swamp-drake branch are solved.

## Run 112 - I Finish Naming The Edge

I start at `Point3D(1191,1361,0)`, still facing southwest. The rectangle is unchanged: fox at `Point3D(1193,1358,0)`, the named west-edge toad at `Point3D(1173,1360,0)`, another small shape at `Point3D(1174,1379,0)`, and a bird-shape at `Point3D(1176,1379,0)`. No visible saved item, corpse, chest, sign, road, vendor, water source, gump, context menu, target cursor, or running spawner object is in the screen.

The pressure pass does not hand me a combat beat. The toads are still `AI_Animal` and `FightMode.Aggressor`, but Aggressor acquisition has no aggressor, aggressed, faction, or ethic hook to grab me. Their teleport timer is meaner than their tiny bodies, but it only scans ten tiles from the toad before the random/harmful/enemy gates; I am outside both toad-centered range-10 rectangles. The crane is also passive Aggressor animal code with no special timer. The old swamp drake stays off-screen as a carried private-timer risk, not a solved result.

**Beat 1**

I single-click the southwest edge small shape.

The normal look packet goes through `PacketHandlers.LookReq`, finds mobile serial `737437`, checks `CanSee`, and passes `Utility.InUpdateRange`: dx `17`, dy `18` is still inside the inclusive 18-tile rectangle. `Region.OnSingleClick` returns true, `BaseCreature.OnSingleClick` adds no tame or bonded line because the creature is uncontrolled, and `Mobile.OnSingleClick` sends only the name label.

The label says `a toad`.

Nothing else moves. No context menu opens, no Tame row exists on screen, no target cursor appears, and no toad timer teleports me. The fox stays in the heel band because I neither moved nor waited its AI tick.

**Beat 2**

I single-click the bird-shape beside that toad.

The same ordinary look path runs for serial `288081`. This one is dx `15`, dy `18`, also inside `Utility.InUpdateRange`. `Region.OnSingleClick` permits the click, `BaseCreature.OnSingleClick` adds no controlled-pet label, and `Mobile.OnSingleClick` sends the overhead name.

The label says `a crane`.

Now all three edge animals have names on my screen: toad, toad, crane. That makes the route decision clearer but not safer. I still do not open context menus, try to tame at screen edge, attack, chase the map overlay, move toward the animals, use the canteen, or pretend the wilderness tick timers froze.

Mechanical friction learned:

- Single-clicking at dx/dy `18` is legal screen inspection, but it is not reach for taming or interaction.
- The second visible toad is the same teleport-timer animal class as the first, but range 10 from the toad is the real pressure gate.
- The bird-shape is a crane: passive `AI_Animal`/`FightMode.Aggressor`, one-point damage, no special timer traced.
- Naming every visible edge animal still changes no location, pet spacing, ownership, quest, discovery, hunger, thirst, skill, combat, or UI state.

Next pressure:

Mira remains at `Point3D(1191,1361,0)`, facing southwest. The fox remains at `Point3D(1193,1358,0)`, inside the `2..3` heel band. The west and southwest edges are now known to hold two toads and one crane. The next honest run is a route choice: skirt the toads without stepping into range 10, wait for a safer opening, or move away from the edge animals while continuing to carry the off-screen swamp-drake timer as unresolved risk.

## Run 113 - I Turn Away From The Edge

I start at `Point3D(1191,1361,0)`, still facing southwest. The live rectangle is the same named pressure as last time: my fox at `Point3D(1193,1358,0)`, a toad at the west edge, and the other toad plus crane on the southwest edge. There are still zero visible saved items and zero running spawner objects in the rectangle. The overlay says `Ruins` is southeast, `Mines of Morinia` is west, and the West moongate is southwest, but the only things actually on the screen are animals and jungle.

The toads do not get a free attack just because I can name them. Their timer only searches ten tiles from the toad before random, LOS, harmful, visibility, access, and enemy gates matter. I am outside that range from both toads. The crane is passive animal code too. The old swamp drake remains off-screen as the carried private-timer branch, not a solved result.

**Beat 1**

I press southeast, away from the west-edge toad and not farther southwest into the two edge animals.

Because I was facing `Direction.Left`, this is only a turn. `Mobile.Move` does not enter `CheckMovement`; it keeps my location at `Point3D(1191,1361,0)`, changes facing to `Direction.Down`, and runs the normal movement notification/update path. No terrain tile is consumed, no region changes, no gump opens, and no pet AI tick runs.

**Beat 2**

I press southeast again.

Now my facing matches the keypress, so the real movement path runs. `MovementImpl.CheckMovement` uses Sosaria's file index `1`: the forward tile `Point3D(1192,1362,0)` is jungle `0x583`, and the two player diagonal side-check tiles `Point3D(1192,1361,0)` and `Point3D(1191,1362,0)` are jungle `0x587` and `0xAF`. All three are dry, passable, and have zero statics. The live snapshot has no saved mobile or visible item on those tiles, and `Regions.xml` names no region there.

Mira moves to `Point3D(1192,1362,0)`. That one step drops the west-edge toad off the client rectangle at dx `-19`, dy `-2`; it is not gone, just not visible. The southwest toad and crane remain visible at dx `-18`/`-16`, dy `17`. My fox is now stretched to floored distance `4`, so walking again would be sloppy.

**Beat 3**

I wait for the fox.

The controlled follower path runs through `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.South)`, and `Mobile.Move`. FriendsAvoidHeels still wants the `2..3` band. From `Point3D(1193,1358,0)` toward me, `GetDirectionTo` resolves south; the target `Point3D(1193,1359,0)` is jungle `0x583` with zero statics and no live blocker. The fox steps south and lands at dx `1`, dy `-3`, floored distance `3`.

No context menu, target cursor, combatant, focus mob, breath, bite, damage, item use, hunger, thirst, skill gain, quest, discovery, ownership, follower-count, pet-order, fame, karma, PvP/PvE state, or open UI changes. The off-screen west toad can wander back later, the two southwest animals are still visible, and the swamp drake clock is still carried unresolved.

Mechanical friction learned:

- Turning is a real client beat but not a terrain step; the server still updates facing and movement notifications.
- A single southeast step can make one edge creature fall out of update range without proving it despawned.
- Diagonal player movement still needs the forward tile and both side-check tiles to pass.
- The fox follow tax returns immediately after one diagonal player step.

Next pressure:

Mira ends at `Point3D(1192,1362,0)`, facing southeast. The fox is at `Point3D(1193,1359,0)`, back inside the `2..3` heel band. The southwest toad and crane are still visible at the edge, the west toad is off-screen but unresolved, and the next run needs a fresh scan before choosing whether to keep skirting east/southeast or stop and reassess.

## Audit Note - Automation Truthfulness After Run 113

No server code changed. The simulation model is being tightened, not the shard.

The important correction is that off-screen is not hibernation. The RunUO AI evidence says player-range-sensitive AI is controlled by sector activity and delayed deactivation, not by strict 18-tile client visibility. The 18-tile rectangle is the player's click and visibility envelope; it is not proof that a mobile's AI timer stopped, that a sector slept, or that a creature despawned.

So the next runs must treat off-screen AI as unobserved/carryable risk unless the live export proves deletion, despawn, or a stopped timer. The carried swamp-drake branch remains exactly that: risk evidence, not an active blocker and not a safety verdict. The drake is not visible from `Point3D(1192,1362,0)`, and its own range-perception scan does not currently contain Mira, but the missing private `BaseAI.m_NextMove` branch is still not chosen.

The post-Run-113 screen truth stays unchanged. Mira ended at `Point3D(1192,1362,0)`, the fox ended at `Point3D(1193,1359,0)` inside the follow band, the southwest toad and crane remain visible edge pressure, and the west toad is off-screen but unresolved. Run 114 must begin with a fresh visible scan and the stricter creature/timer pressure classifier before any movement, wait, click, or route choice.

## Run 114 - I Keep Sliding Off The Toads

I start at `Point3D(1192,1362,0)`, facing southeast, with the fox at `Point3D(1193,1359,0)`. The screen scan is still honest and small: the live snapshot shows a labeled toad at `Point3D(1174,1379,0)` and a labeled crane at `Point3D(1176,1379,0)`, with zero visible saved items and zero running spawner objects. The west toad is not visible now, but I do not get to call it gone. The old swamp drake is far off-screen and still carried as unresolved private-timer risk, not safety.

The world-map overlay keeps changing by inches. `Ruins` is the nearest lure, about 141 tiles away from the starting tile and 138 from the final tile. `Mines of Morinia` and the West moongate are still just map names, not roads or shelter on the ground.

**Beat 1**

I press southeast because I am already facing that way and it moves me away from the toads by x range.

`Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1193,1363,0)` is grass `0x4`; the two player diagonal side checks, `Point3D(1193,1362,0)` and `Point3D(1192,1363,0)`, are grass `0x6` and jungle `0x583`. The server-order Sosaria file index is still `1`; all three tiles are dry, passable, and have zero statics. No saved mobile or item occupies the destination, and no named region catches it.

Mira moves to `Point3D(1193,1363,0)`. The southwest toad falls out of the 18-tile client rectangle at dx `-19`, dy `16`; the crane remains visible at dx `-17`, dy `16`. The fox does not move on my keypress and is now four tiles behind by floored distance.

**Beat 2**

I wait for the fox instead of walking off and stretching the leash.

The pet follow path runs through `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.South)`, and `Mobile.Move`. FriendsAvoidHeels is still using the remembered spacing value `7`, so the desired band is `2..3`. From `Point3D(1193,1359,0)` toward me, the fox chooses south. The target `Point3D(1193,1360,0)` is jungle `0x583`, dry, passable, and has zero statics. The fox steps there and lands back at floored distance `3`.

Nothing else gets a turn in my favor. The crane is still visible, the toads are off-screen but unresolved, and no item, corpse, chest, gump, context menu, target cursor, combat, damage, hunger, thirst, skill, quest, discovery, or pet-order state changes.

**Beat 3**

I take one more southeast step.

`Mobile.Move` again gets the real movement path because I am still facing `Direction.Down`. The forward tile `Point3D(1194,1364,0)` is grass `0x6`; the player diagonal side checks `Point3D(1194,1363,0)` and `Point3D(1193,1364,0)` are grass `0x3` and `0x5`. All three are dry, passable, and static-free. No live saved blocker or named region is there.

Mira moves to `Point3D(1194,1364,0)`. The crane is still just visible on the far west/southwest edge at dx `-18`, dy `15`. Both toads are now outside the client rectangle, but neither was despawned, killed, moved, tamed, attacked, or timed. The fox is stretched again at dx `-1`, dy `-4`, floored distance `4`, so the next honest beat is probably another fox wait before any further route choice.

Mechanical friction learned:

- Moving southeast is currently a real retreat from the toads, not a discovery route.
- Dropping a toad out of the client rectangle is only visibility friction; it does not prove the timer stopped.
- The crane can stay visible at the edge even after both toads leave the screen.
- One southeast movement step immediately reintroduces follower-spacing pressure.

Next pressure:

Mira ends at `Point3D(1194,1364,0)`, facing southeast. The fox is at `Point3D(1193,1360,0)`, outside the remembered `2..3` heel band. The crane remains visible at the edge, both toads are off-screen but unresolved, and the old swamp drake branch remains carried unresolved rather than solved.

## Run 115 - The Last Bird Leaves The Screen

I start at `Point3D(1194,1364,0)`, still facing southeast, with the fox behind me at `Point3D(1193,1360,0)`. The screen has exactly one saved creature left: the labeled crane at `Point3D(1176,1379,0)`, stuck on the far west/southwest edge at dx `-18`, dy `15`. The toads are not visible now. That does not make them dead, despawned, or safe; it only means my client rectangle no longer contains them. The old swamp drake is far off-screen and still the same carried private-timer risk, not a solved threat.

**Beat 1**

I wait for the fox instead of walking farther with it stretched.

The pet tick goes through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.South)`, and `Mobile.Move`. FriendsAvoidHeels is still using spacing `7`, so the fox wants the `2..3` tile band. From `Point3D(1193,1360,0)` to me, the fox resolves south. The target `Point3D(1193,1361,0)` is jungle `0x57D`, dry, passable, and has zero statics. The fox steps there and lands at floored distance `3`.

The crane remains visible. It is passive `AI_Animal`/`FightMode.Aggressor` with no special timer traced, but visible wildlife still tugs at the edge of my attention.

**Beat 2**

With the leash repaired, I press southeast one more time.

`Mobile.Move` enters the real movement path because I am already facing `Direction.Down`. The server-order Sosaria read uses file index `1`. The forward tile `Point3D(1195,1365,0)` is grass `0x3`; the two player diagonal side checks, `Point3D(1195,1364,0)` and `Point3D(1194,1365,0)`, are also grass `0x3`. All three have zero statics, no saved blocker, and no named `Regions.xml` rectangle.

Mira moves to `Point3D(1195,1365,0)`. The crane drops outside the 18-tile client rectangle at dx `-19`, dy `14`. It is not gone; I just cannot see or click it from here. The fox stays at `Point3D(1193,1361,0)` and stretches back to floored distance `4`.

**Beat 3**

I wait again.

The same controlled-follow path runs. From `Point3D(1193,1361,0)` toward me at `Point3D(1195,1365,0)`, `GetDirectionTo` resolves `Direction.Down`. The fox's target `Point3D(1194,1362,0)` is grass `0x3` with zero statics. The diagonal side checks `Point3D(1194,1361,0)` and `Point3D(1193,1362,0)` are grass `0x6` and `0x3`, also static-free. The fox steps southeast and ends at dx `-1`, dy `-3`, floored distance `3`.

The final screen is empty except for my controlled fox. The live-state rectangle around `Point3D(1195,1365,0)` has zero saved visible mobiles, zero saved visible items, and zero running spawner objects. That feels quieter, but it is not proof of safety: both toads, the crane, and the swamp drake are all unresolved off-screen history rather than deleted facts.

Mechanical friction learned:

- A visible edge animal can be moved off-screen by ordinary walking without resolving its AI or timer state.
- The conservative rhythm is still one player step, then a follower wait; otherwise the fox immediately falls outside the heel band.
- The current patch of Sosaria is plain grass/jungle under server-order `map1.mul` with zero statics on the consumed movement tiles.
- Empty screen is not a town, guard zone, water source, road, shelter, or safety verdict.

Next pressure:

Mira ends at `Point3D(1195,1365,0)`, facing southeast. The fox is at `Point3D(1194,1362,0)`, inside the remembered `2..3` heel band. No saved mobile or item is visible in the final 18-tile rectangle. The labeled toads and crane are off-screen but unresolved, and the swamp drake branch remains carried unresolved rather than solved.

## Run 116 - Empty Grass Still Takes Turns

I start at `Point3D(1195,1365,0)`, facing southeast, with the fox close at `Point3D(1194,1362,0)`. The saved screen is empty: no saved visible mobiles, no saved visible items, no running spawner object in the 18-tile rectangle. That does not make it a road, camp, water source, guard zone, or proof that the toads, crane, and swamp drake stopped existing. It just means there is nothing new to click.

The overlay is still the only lure. `Ruins` is roughly 137 tiles away from the starting tile and 134 tiles from the final tile. `Mines of Morinia` and the West moongate sit farther off. None of those rows are visible terrain.

**Beat 1**

I press southeast.

Because I am already facing `Direction.Down`, this is a real movement beat. `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1196,1366,0)` is grass `0x4`; the two player diagonal side checks, `Point3D(1196,1365,0)` and `Point3D(1195,1366,0)`, are grass `0x5` and `0x4`. The Sosaria scan still uses file index `1`, and `map1.mul`, `staidx1.mul`, and `statics1.mul` say all three are dry, passable, and have zero statics. No saved mobile or item occupies the step.

Mira moves to `Point3D(1196,1366,0)`. The fox does not move on my keypress, so it stretches to dx `-2`, dy `-4`, floored distance `4`. The shifted saved-world screen is still empty except for the simulated follower overlay.

**Beat 2**

I wait for the fox.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels is still using spacing `7`, so the fox wants the `2..3` tile band. From `Point3D(1194,1362,0)` toward me, the target is `Point3D(1195,1363,0)`.

That tile is grass `0x6` with zero statics. The diagonal side checks, `Point3D(1195,1362,0)` and `Point3D(1194,1363,0)`, are grass `0x4` and `0x3`, also static-free. The fox steps southeast and lands at dx `-1`, dy `-3`, floored distance `3`.

No gump opens. No animal returns to the screen. No item, corpse, chest, sign, road, water target, vendor, or named region appears.

**Beat 3**

With the leash repaired, I press southeast again.

`Mobile.Move` runs the real movement block. The forward tile `Point3D(1197,1367,0)` is grass `0x4`; the player side checks `Point3D(1197,1366,0)` and `Point3D(1196,1367,0)` are grass `0x4` and `0x3`. Zero statics, no saved blocker, no named region. The step is allowed.

Mira moves to `Point3D(1197,1367,0)`. The final saved rectangle, x `1179..1215` and y `1349..1385`, still contains zero saved visible mobiles, zero saved visible items, and zero running spawner objects. The fox stays at `Point3D(1195,1363,0)`, now stretched back to dx `-2`, dy `-4`, floored distance `4`.

Mechanical friction learned:

- Empty grass is not safety. It is only negative screen evidence from the current live-state rectangle.
- The map overlay keeps shrinking the distance to `Ruins`, but it still grants no road, shelter, discovery flag, or clickable entity.
- Moving southeast remains mechanically cheap for Mira, but every player step reintroduces fox leash pressure.
- The off-screen toads, crane, and swamp drake remain unresolved history, not deleted facts.

Next pressure:

Mira ends at `Point3D(1197,1367,0)`, facing southeast. The fox is at `Point3D(1195,1363,0)`, outside the `2..3` heel band. The screen has no saved mobile or item, so the next honest beat is likely a fox wait before any more route movement.

## Run 117 - Leash Rhythm Over Empty Grass

I start at `Point3D(1197,1367,0)`, still facing southeast, with the fox behind me at `Point3D(1195,1363,0)`. The saved screen rectangle is empty again: no saved visible mobiles, no saved visible items, and no running spawner object inside x `1179..1215`, y `1349..1385`. That is only negative screen evidence. The running animal and world spawners at `Point3D(1165,1361,33)` and `Point3D(1165,1361,30)` still have spawned animals and hostile picks west of me, but none of their saved mobile records are currently inside my client rectangle.

The pressure pass stays annoying rather than dramatic. The old swamp drake is still carried private-timer risk, not a visible blocker. The labeled west toad, southwest toad, and crane are off-screen; the nearest toad timer still cannot target me because the toad-centered range-10 scan does not include my tile. The only thing on my screen is my own fox, and it is stretched outside the remembered heel band.

**Beat 1**

I wait for the fox.

The controlled AI path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels still wants the `2..3` band. From `Point3D(1195,1363,0)` toward me, `GetDirectionTo` points southeast. The target `Point3D(1196,1364,0)` is grass `0x6` at z `0`; the diagonal side tiles `Point3D(1196,1363,0)` and `Point3D(1195,1364,0)` are grass `0x3` and `0x4`. All three are dry, passable, and have zero statics.

The fox steps to `Point3D(1196,1364,0)` and lands at dx `-1`, dy `-3`, floored distance `3`. No animal returns to view.

**Beat 2**

With the leash repaired, I press southeast.

`Mobile.Move` reaches the real movement block because I am already facing `Direction.Down`. The forward tile `Point3D(1198,1368,0)` is grass `0x3`; the player diagonal side-check tiles `Point3D(1198,1367,0)` and `Point3D(1197,1368,0)` are grass `0x3` and `0x4`. The server-order Sosaria read uses `map1.mul`, `staidx1.mul`, and `statics1.mul`; all three tiles are dry, passable, and static-free. No saved mobile or item occupies the destination, and no named `Regions.xml` rectangle catches it.

Mira moves to `Point3D(1198,1368,0)`. The shifted rectangle x `1180..1216`, y `1350..1386` is still empty of saved visible mobiles and items. The fox stays on `Point3D(1196,1364,0)`, now stretched back to floored distance `4`.

**Beat 3**

I wait for the fox again instead of walking a second time.

The same controlled-follow path runs. From `Point3D(1196,1364,0)` toward `Point3D(1198,1368,0)`, `GetDirectionTo` again resolves `Direction.Down`. The fox target `Point3D(1197,1365,0)` is grass `0x4`, and the side tiles `Point3D(1197,1364,0)` and `Point3D(1196,1365,0)` are grass `0x3` and `0x5`. Zero statics, no saved blocker, no named region.

The fox steps to `Point3D(1197,1365,0)` and returns to dx `-1`, dy `-3`, floored distance `3`. I stop at the three-beat cap. Nothing opened, nothing attacked, nothing was looted, no item was used, no hunger or thirst changed, no skill rolled, no quest or discovery flag moved, and no pet order or follower count changed.

Mechanical friction learned:

- A fox wait is a real server beat even when the player does not move.
- The current grass tiles remain cheap, but a single southeast player step immediately creates another leash tax.
- The live saved rectangle is empty, yet the off-screen southern spawner records still explain why those toads and cranes existed a few screens back.
- The `Ruins` marker is closer, but it is still overlay knowledge only, not visible shelter or a discovered place.

Next pressure:

Mira ends at `Point3D(1198,1368,0)`, facing southeast. The fox is at `Point3D(1197,1365,0)`, inside the `2..3` heel band. The screen has no saved mobile or item, so the next run can make a fresh route decision, but it cannot call the off-screen toads, crane, spawners, or swamp drake solved.

## Run 118 - The Map Marker Keeps Pulling

I start at `Point3D(1198,1368,0)`, facing southeast, with the fox close at `Point3D(1197,1365,0)`. The live screen is still empty of saved bodies and things: zero saved visible mobiles, zero saved visible items, and zero running spawner objects inside x `1180..1216`, y `1350..1386`. My fox is the only visible body because the simulation has moved it with me; the original save record is stale for pet position.

The map overlay keeps whispering `Ruins` southeast, about 132.6 tiles away from this starting spot. That is not a ruin on my screen. It is just the nearest marker. The old toads and crane are still behind me as unresolved recently visible animals, and the swamp drake is still a carried private-timer risk, not a solved thing. Nothing visible competes with one cautious route step.

**Beat 1**

I press southeast.

`Mobile.Move` reaches the real movement block because I am already facing `Direction.Down`. The forward tile `Point3D(1199,1369,0)` is grass `0x3`; the player diagonal side-check tiles `Point3D(1199,1368,0)` and `Point3D(1198,1369,0)` are also grass `0x3`. Sosaria is still map index `1`, map id `1`, file index `1`, and the server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read says all three tiles are dry, passable, and static-free. No saved mobile, saved visible item, or named region blocks the step.

Mira moves to `Point3D(1199,1369,0)`. The shifted live rectangle x `1181..1217`, y `1351..1387` is still empty of saved visible mobiles/items/spawners. The fox does not move on my keypress and is now at floored distance `4`, outside the `2..3` band.

**Beat 2**

I wait for the fox.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels is still carrying the spacing value `7`, so the desired range remains `2..3`. From `Point3D(1197,1365,0)` toward me, the fox chooses southeast.

The fox target `Point3D(1198,1366,0)` is grass `0x3`. The non-player diagonal side tiles `Point3D(1198,1365,0)` and `Point3D(1197,1366,0)` are grass `0x3` and `0x4`. Zero statics, no saved blocker. The fox steps to `Point3D(1198,1366,0)` and lands back at dx `-1`, dy `-3`, floored distance `3`.

**Beat 3**

The screen is still just grass and the fox, so I take one more southeast step toward the marker without pretending I can see it.

`Mobile.Move` again enters `CheckMovement`. The forward tile `Point3D(1200,1370,0)` is grass `0x6`; the side-check tiles `Point3D(1200,1369,0)` and `Point3D(1199,1370,0)` are grass `0x6` and `0x4`. All are dry, passable, and have zero statics. The destination has no saved blocker and no named region.

Mira moves to `Point3D(1200,1370,0)`. The final live rectangle x `1182..1218`, y `1352..1388` still has zero saved visible mobiles, zero saved visible items, and zero running spawner objects. The two running southern PremiumSpawner homes at `1165,1361` still overlap by home range, but they are invisible source pressure, not visible NPCs. The fox stays at `Point3D(1198,1366,0)` and is stretched again to floored distance `4`.

Mechanical friction learned:

- Empty screen travel still needs a full terrain check for every diagonal step.
- Southeast is the marker-correct direction toward `Ruins`, but the marker remains overlay knowledge only.
- The follower rhythm continues: one player step is cheap, the next pet tick is usually owed.
- Invisible spawner ranges can overlap an empty live rectangle without creating a visible entity.

Next pressure:

Mira ends at `Point3D(1200,1370,0)`, facing southeast. The fox is at `Point3D(1198,1366,0)`, outside the `2..3` heel band at floored distance `4`. The next honest beat is likely another fox wait before further marker-directed movement, with the toads, crane, southern spawners, and swamp drake still carried as unresolved risk rather than safety.

## Run 119 - Leash First, Marker Later

I start at `Point3D(1200,1370,0)`, still facing southeast. The saved screen is empty again: zero saved visible mobiles, zero saved visible items, and zero running spawner objects inside the 18-tile rectangle. The only body I can honestly account for is my controlled fox at `Point3D(1198,1366,0)`, northwest and stretched to floored distance `4`.

That matters more than the map marker. `Ruins` is still the nearest overlay lure, now about 130 tiles away, but it is not on the screen and it is not shelter. The toads, crane, southern spawner homes, and old swamp drake are unresolved off-screen pressure, not solved threats.

**Beat 1**

I wait for the fox.

The pet tick runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels still wants the `2..3` band. The fox is too far, so it steps southeast from `Point3D(1198,1366,0)` to `Point3D(1199,1367,0)`.

The checked map1 tiles are plain grass: target `0x6`, side tiles `0x6` and `0x3`, all zero statics. No message, gump, combat, skill, item, hunger, thirst, quest, follower count, or pet order changes.

**Beat 2**

With the fox back near enough, I press southeast once.

`Mobile.Move` enters the real movement path because I am already facing `Direction.Down`. The forward tile `Point3D(1201,1371,1)` is grass `0x5`; the player diagonal side checks `Point3D(1201,1370,0)` and `Point3D(1200,1371,0)` are grass `0x6` and `0x5`. All three have zero statics, no saved blocker, and no named `Regions.xml` rectangle.

Mira moves to `Point3D(1201,1371,1)`. The live-state rectangle shifts to x `1183..1219`, y `1353..1389` and still contains zero saved visible mobiles, zero saved visible items, and zero running spawner objects. The invisible southern `PremiumSpawner` home ranges still overlap by source range, but nothing from them is visible or clickable.

**Beat 3**

The player step stretched the fox again, so I wait instead of walking twice.

The same follow path runs. From `Point3D(1199,1367,0)` toward me, `GetDirectionTo` again resolves `Direction.Down`. The target `Point3D(1200,1368,0)` is grass `0x3`; the non-player side tiles `Point3D(1200,1367,1)` and `Point3D(1199,1368,0)` are also grass `0x3`, with zero statics. The fox steps to `Point3D(1200,1368,0)` and lands at dx `-1`, dy `-3`, floored distance `3`.

Mechanical friction learned:

- The route southeast is still open grass, but a z step can happen on ordinary land.
- Empty screen travel is not safety; it only means no saved entity is currently inside the client rectangle.
- The fox is not a guard. Waiting it in changes only follower position, not combat readiness.
- The nearest marker keeps getting closer, but it remains overlay knowledge only.

Next pressure:

Mira ends at `Point3D(1201,1371,1)`, facing southeast. The fox is at `Point3D(1200,1368,0)`, inside the `2..3` heel band. No saved mobile or item is visible in the final 18-tile rectangle. The next run can make a fresh route choice, but the off-screen toads, crane, southern spawners, and carried swamp drake branch are still unresolved risk.

## Run 120 - The Empty Screen Starts To Smell Like Spawner Range

I start at `Point3D(1201,1371,1)`, facing southeast, with the fox close at `Point3D(1200,1368,0)`. The saved client rectangle is empty again: no saved visible mobiles, no visible items, and no running spawner object sitting inside the 18-tile box. The empty grass is not peace. The old toads and crane are still off-screen unresolved history, the swamp drake is still carried private-timer risk, and the southern invisible spawners at `1165,1361` still overlap the screen by home range.

The map overlay says `Ruins` is about 128 tiles southeast. That is still only overlay knowledge, not something I can see.

**Beat 1**

I press southeast.

`Mobile.Move` reaches the real movement path because I am already facing `Direction.Down`. The forward tile `Point3D(1202,1372,0)` is grass `0x5`; the player diagonal side checks `Point3D(1202,1371,0)` and `Point3D(1201,1372,1)` are grass `0x5` and `0x4`. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read finds zero statics on all three tiles, and the live snapshot has no saved mobile or visible item on the step.

Mira moves to `Point3D(1202,1372,0)`. The shifted rectangle x `1184..1220`, y `1354..1390` is still empty of saved visible mobiles and items. The fox does not move on my keypress, so it stretches to dx `-2`, dy `-4`, floored distance `4`.

**Beat 2**

I wait for the fox instead of walking twice.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels is still carrying spacing `7`, so the fox wants the `2..3` band. From `Point3D(1200,1368,0)` toward me, it picks southeast.

The fox target `Point3D(1201,1369,0)` is grass `0x3`; its side tiles `Point3D(1201,1368,0)` and `Point3D(1200,1369,0)` are grass `0x4` and `0x6`. All three have zero statics. The fox steps to `Point3D(1201,1369,0)` and returns to dx `-1`, dy `-3`, floored distance `3`.

**Beat 3**

With the leash repaired and the screen still empty, I take one more southeast step toward the marker.

`Mobile.Move` again enters `CheckMovement`. The forward tile `Point3D(1203,1373,0)` is grass `0x5`; the player side checks `Point3D(1203,1372,0)` and `Point3D(1202,1373,0)` are both grass `0x6`. There are zero statics on the forward tile and both side checks, no saved blocker, and no named region.

Mira moves to `Point3D(1203,1373,0)`. The final saved rectangle x `1185..1221`, y `1355..1391` still has zero saved visible mobiles, zero saved visible items, and zero running spawner locations. The catch is that four invisible `PremiumSpawner` home ranges now overlap the rectangle: the older animal/world pair at `1165,1361`, and a southern animal/world pair at `1206,1455`. Nothing from them is visible or clickable, but the route is drifting deeper into their pressure.

Mechanical friction learned:

- Empty-screen travel can still move into more invisible spawner home overlap.
- The marker got closer, from about 128 tiles to about 126 tiles, but it remains a world-map hint rather than visible ruins.
- The fox is still not protection. Waiting it in only changes follower position.
- Two player steps around one fox wait leave the leash stretched again.

Next pressure:

Mira ends at `Point3D(1203,1373,0)`, facing southeast. The fox is at `Point3D(1201,1369,0)`, outside the `2..3` heel band at floored distance `4`. No saved mobile or item is visible, but four invisible spawner home ranges now overlap the final rectangle, and the off-screen toads, crane, and carried swamp drake branch remain unresolved risk. The next honest beat is likely another fox wait before walking farther.

## Run 121 - The Empty Screen Gains Edges I Cannot See

I start at `Point3D(1203,1373,0)`, facing southeast. The screen is still saved-empty: no saved visible mobiles, no visible items, and no running spawner object location inside the client rectangle. The only body I can act on is my fox, and it is stretched behind me at `Point3D(1201,1369,0)`, outside the remembered `2..3` follow band. The `Ruins` marker is still only a map overlay, about 126 tiles away from where I stand.

**Beat 1**

I wait for the fox.

The controlled AI tick runs the usual path: `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. The existing FriendsAvoidHeels spacing value `7` still wants the fox `2..3` tiles away. From `Point3D(1201,1369,0)` toward me, the fox resolves southeast.

The target `Point3D(1202,1370,0)` is grass `0x4`. The side tiles `Point3D(1202,1369,0)` and `Point3D(1201,1370,0)` are grass `0x3` and `0x6`. The server-order `map1.mul` read says all three are dry, passable, and static-free. The fox steps to `Point3D(1202,1370,0)` and lands at dx `-1`, dy `-3`, floored distance `3`.

**Beat 2**

With the leash repaired and nothing saved-visible on the screen, I press southeast once.

`Mobile.Move` reaches the real movement block because I am already facing `Direction.Down`. The forward tile `Point3D(1204,1374,0)` is grass `0x4`; the player diagonal side checks `Point3D(1204,1373,0)` and `Point3D(1203,1374,0)` are also grass `0x4`. There are zero statics, no saved blocker, and no named region match.

Mira moves to `Point3D(1204,1374,0)`. The shifted live rectangle x `1186..1222`, y `1356..1392` still has zero saved visible mobiles, zero saved visible items, and zero running spawner object locations. The catch is invisible: the eastern animal/world spawner pair at `1282,1374` now overlaps the rectangle by home range, joining the western `1165,1361` pair and southern `1206,1455` pair. None of those are screen entities, but the route is accumulating source pressure.

**Beat 3**

The player step stretched the fox again, so I wait rather than take a second blind step.

The same controlled-follow path runs. From `Point3D(1202,1370,0)` toward me, the fox again picks `Direction.Down`. The target `Point3D(1203,1371,0)` is grass `0x3`; the side tiles `Point3D(1203,1370,0)` and `Point3D(1202,1371,0)` are grass `0x4` and `0x5`. Zero statics, no saved blocker. The fox steps to `Point3D(1203,1371,0)` and returns to dx `-1`, dy `-3`, floored distance `3`.

Mechanical friction learned:

- Empty screen travel can still add invisible spawner overlap; nothing new is clickable, but the pressure map gets worse.
- The southeast route remains open grass for this single step.
- A fox follow wait is still only leash repair. It does not guard, body-block, scout, or resolve hostile spawner outcomes.
- The `Ruins` marker drops to about 124 tiles away, but it remains overlay knowledge rather than visible shelter.

Next pressure:

Mira ends at `Point3D(1204,1374,0)`, facing southeast. The fox is at `Point3D(1203,1371,0)`, back inside the `2..3` heel band. No saved mobile or item is visible, but six invisible spawner home ranges overlap the final rectangle, including the newly overlapping eastern pair at `1282,1374`. The off-screen toads, crane, and carried swamp drake branch remain unresolved risk rather than safety.

## Run 122 - Empty Grass Still Charges Interest

I start at `Point3D(1204,1374,0)`, facing southeast, with the fox close at `Point3D(1203,1371,0)`. The current live-state rectangle is empty again: zero saved visible mobiles, zero saved visible items, and zero running spawner object locations. That is not comfort. Six invisible `PremiumSpawner` home ranges overlap the screen now, and none of them are clickable bodies.

The overlay keeps teasing `Ruins`, about 124 tiles away. The map marker is still just a marker. The older toads and crane are behind me, the swamp drake is still a carried private-timer risk, and the fox is the only visible creature I can honestly use.

**Beat 1**

I press southeast.

`Mobile.Move` runs the real movement block because I am already facing `Direction.Down`. The forward tile `Point3D(1205,1375,0)` is grass `0x3`; the player diagonal side checks `Point3D(1205,1374,0)` and `Point3D(1204,1375,0)` are grass `0x4` and `0x6`. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read says all three are dry, passable, and static-free. The live snapshot has no saved mobile or visible item on the destination, and `Regions.xml` gives me no named place.

Mira moves to `Point3D(1205,1375,0)`. The shifted rectangle x `1187..1223`, y `1357..1393` still contains zero saved visible mobiles, zero saved items, and zero running spawner object locations. The fox does not move on my keypress, so it stretches to dx `-2`, dy `-4`, floored distance `4`.

**Beat 2**

I wait for the fox.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels is still carrying spacing `7`, so the desired band remains `2..3`. From `Point3D(1203,1371,0)` toward me, the fox chooses southeast.

The fox target `Point3D(1204,1372,0)` is grass `0x4`; the non-player diagonal side checks `Point3D(1204,1371,0)` and `Point3D(1203,1372,0)` are grass `0x6` and `0x5`. Zero statics, no saved blocker. The fox steps there and lands at dx `-1`, dy `-3`, floored distance `3`.

**Beat 3**

With the leash repaired and the screen still empty, I take one more southeast step toward the marker.

`Mobile.Move` enters `CheckMovement` again. The forward tile `Point3D(1206,1376,0)` is grass `0x3`; the side-check tiles `Point3D(1206,1375,0)` and `Point3D(1205,1376,0)` are grass `0x5` and `0x3`. All are dry, passable, and have zero statics. No saved mobile or item occupies the step, and no named region catches it.

Mira moves to `Point3D(1206,1376,0)`. The final rectangle x `1188..1224`, y `1358..1394` still has zero saved visible mobiles, zero saved visible items, and zero running spawner object locations. The six invisible spawner home ranges still overlap: the western pair, the southern pair, and the eastern pair. The fox stays at `Point3D(1204,1372,0)`, stretched again to floored distance `4`.

Mechanical friction learned:

- Empty grass keeps letting me walk, but it never becomes proof of safety.
- The `Ruins` marker drops to about 121 tiles away, still not visible and still not an in-world shelter.
- The invisible spawner-range overlap is stable pressure, not a screen entity.
- Every second route step taxes the fox leash again.

Next pressure:

Mira ends at `Point3D(1206,1376,0)`, facing southeast. The fox is at `Point3D(1204,1372,0)`, outside the `2..3` heel band at floored distance `4`. No saved mobile or item is visible, but the next honest beat is likely another fox wait before any more marker-directed walking.

## Run 123 - Leash Debt Before Another Step

I start at `Point3D(1206,1376,0)`, facing southeast, with the fox behind me at `Point3D(1204,1372,0)`. The saved screen is empty again: zero saved visible mobiles, zero visible items, and zero running spawner object locations inside x `1188..1224`, y `1358..1394`. Empty is only empty. Six invisible `PremiumSpawner` home ranges still overlap the screen, and the old toads, crane, and swamp drake are still unresolved off-screen history.

The `Ruins` marker is about 121 tiles away. It is a map overlay, not a ruin I can see.

**Beat 1**

I wait for the fox before I walk.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels is still using spacing `7`, so the fox wants the `2..3` band. From `Point3D(1204,1372,0)`, the fox is too far, picks southeast, and steps to `Point3D(1205,1373,0)`.

The target tile is grass `0x3`; side tiles `1205,1372` and `1204,1373` are grass `0x6` and `0x4`. Zero statics, no saved blocker, no message.

**Beat 2**

With the fox back near enough, I press southeast once.

`Mobile.Move` enters the real movement path because I am already facing `Direction.Down`. `MovementImpl.CheckMovement` accepts `Point3D(1207,1377,0)`, with side-check tiles `Point3D(1207,1376,0)` and `Point3D(1206,1377,1)`. The server-order `map1.mul` scan reads all three as grass with zero statics, and `Regions.xml` gives no named place.

Mira moves to `Point3D(1207,1377,0)`. The shifted screen x `1189..1225`, y `1359..1395` still contains zero saved visible mobiles, zero saved items, and zero running spawner object locations. The same six invisible spawner ranges overlap by home range only.

**Beat 3**

The step stretches the fox again, so I wait instead of taking a second blind stride.

The fox repeats the controlled follow path. From `Point3D(1205,1373,0)`, `GetDirectionTo` resolves southeast. The target `Point3D(1206,1374,0)` is grass `0x5`; side tiles `1206,1373` and `1205,1374` are grass `0x4` and `0x4`, all with zero statics. The fox steps into dx `-1`, dy `-3`, floored distance `3`.

Mechanical friction learned:

- The southeast ground is still ordinary passable grass, including a one-tile side-check z rise.
- Empty saved rectangles are still not shelter; six invisible spawner ranges keep overlapping the screen.
- The fox is useful as leash pressure, not as protection. Waiting it in did not run Guard, Attack, body-blocking, or hostile aggro logic.
- The `Ruins` marker drops to about 120 tiles away, still only overlay knowledge.

Next pressure:

Mira ends at `Point3D(1207,1377,0)`, facing southeast. The fox is at `Point3D(1206,1374,0)`, inside the `2..3` heel band. No saved mobile or item is visible, but the next action still needs a fresh scan because the invisible spawner overlap and carried off-screen creature risks did not resolve.

## Run 124 - Empty Grass Lets Me Overspend The Leash

I start at `Point3D(1207,1377,0)`, facing southeast, with the fox close at `Point3D(1206,1374,0)`. The live snapshot still gives me an empty screen: zero saved visible mobiles, zero visible items, and zero running spawner object locations inside x `1189..1225`, y `1359..1395`. The six invisible `PremiumSpawner` home ranges are still overlapping by source range only, and the old toads, crane, and swamp drake remain off-screen risk notes rather than bodies I can click.

The `Ruins` marker is about 120 tiles away. It is still only a map overlay.

**Beat 1**

I press southeast.

`Mobile.Move` reaches the real movement path because I am already facing `Direction.Down`. The forward tile `Point3D(1208,1378,0)` is grass `0x4`; the player diagonal side checks `Point3D(1208,1377,0)` and `Point3D(1207,1378,0)` are grass `0x4` and `0x3`. Sosaria is still file index `1`, and the server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read finds zero statics on all three tiles. No saved blocker or named region catches the step.

Mira moves to `Point3D(1208,1378,0)`. The shifted rectangle x `1190..1226`, y `1360..1396` still has zero saved visible mobiles, zero visible items, and zero running spawner object locations. The fox does not move on my keypress, so it stretches to dx `-2`, dy `-4`, floored distance `4`.

**Beat 2**

I wait for the fox before taking another step.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels still carries spacing `7`, so the fox wants the `2..3` band. From `Point3D(1206,1374,0)` toward me, it picks southeast.

The fox target `Point3D(1207,1375,0)` is grass `0x6`. The non-player diagonal side checks `Point3D(1207,1374,0)` and `Point3D(1206,1375,0)` are grass `0x5` and `0x6`, both with zero statics. The fox steps there and returns to dx `-1`, dy `-3`, floored distance `3`.

**Beat 3**

The screen is still empty and the fox is back in range, so I take one more southeast step toward the marker.

`Mobile.Move` enters `CheckMovement` again. The forward tile `Point3D(1209,1379,0)` is grass `0x5`; the player side checks `Point3D(1209,1378,0)` and `Point3D(1208,1379,0)` are both grass `0x5`. All three are dry, passable, and static-free. The live snapshot has no saved mobile or visible item on the destination, and `Regions.xml` has no containing rectangle for it.

Mira moves to `Point3D(1209,1379,0)`. The final rectangle x `1191..1227`, y `1361..1397` is still saved-empty, and the same six invisible spawner ranges overlap by home range only. The fox stays at `Point3D(1207,1375,0)`, stretched back outside the `2..3` band at floored distance `4`.

Mechanical friction learned:

- Empty grass can support steady southeast travel, but every second player step reopens leash debt.
- The `Ruins` marker drops to about 117 tiles away; it is still not visible terrain, shelter, or a discovered place.
- Six invisible spawner ranges overlapping an empty screen remain pressure evidence, not screen entities.
- The controlled fox still provides no Guard, Attack, body-blocking, hostile aggro response, or combat protection.

Next pressure:

Mira ends at `Point3D(1209,1379,0)`, facing southeast. The fox is at `Point3D(1207,1375,0)`, outside the follow band at floored distance `4`. No saved mobile or item is visible in the final rectangle, but the next honest beat is likely another fox wait before more marker-directed movement.

## Run 125 - The Leash Becomes The Clock Again

I start at `Point3D(1209,1379,0)`, facing southeast, with the fox behind me at `Point3D(1207,1375,0)`. The screen is still almost nothing: no saved visible mobiles, no visible saved items, and no running spawner object locations inside x `1191..1227`, y `1361..1397`. The only body on the screen is my own fox, and even that is simulated forward from the live snapshot. Six invisible `PremiumSpawner` home ranges overlap by source range, which means empty grass is not the same as empty danger.

The nearest world-map tease is still `Ruins` at `1303,1449`, about 117 tiles away from where I stand. It is not in sight. It is not shelter. It is only a mark on the client's map.

**Beat 1**

I wait for the fox, because walking again with it already stretched feels like pretending it is protection.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels is still carrying spacing `7`, so the fox wants the `2..3` band. From `Point3D(1207,1375,0)` toward Mira, it picks southeast.

The fox target `Point3D(1208,1376,0)` is grass `0x3`; the non-player diagonal side checks `Point3D(1208,1375,0)` and `Point3D(1207,1376,0)` are grass `0x3` and `0x4`. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read finds zero statics on all three. The fox steps to `Point3D(1208,1376,0)` and lands at dx `-1`, dy `-3`, floored distance `3`.

**Beat 2**

With the leash repaired and the screen still saved-empty, I press southeast once.

`Mobile.Move` reaches `CheckMovement` because I am already facing `Direction.Down`. The forward tile `Point3D(1210,1380,0)` is grass `0x4`; the player diagonal side checks `Point3D(1210,1379,0)` and `Point3D(1209,1380,0)` are grass `0x3` and `0x4`. There are zero statics, no saved blocker, and no named region match.

Mira moves to `Point3D(1210,1380,0)`. The shifted rectangle x `1192..1228`, y `1362..1398` still has zero saved visible mobiles, zero visible saved items, and zero running spawner object locations. The same six invisible spawner home ranges overlap by source range: the western animal/world pair, the southern animal/world pair, and the eastern animal/world pair. None of them is a screen entity.

**Beat 3**

The player step stretched the fox back outside the `2..3` band, so I wait instead of taking a second blind stride.

The fox repeats the controlled follow path. From `Point3D(1208,1376,0)` toward Mira at `Point3D(1210,1380,0)`, `GetDirectionTo` again resolves southeast. The target `Point3D(1209,1377,0)` is grass `0x5`; side checks `Point3D(1209,1376,0)` and `Point3D(1208,1377,0)` are grass `0x5` and `0x4`, all static-free. The fox steps into dx `-1`, dy `-3`, floored distance `3`.

Mechanical friction learned:

- The fox is now the pace-setter. If I move twice in a row, I immediately create leash debt again.
- The route remains ordinary grass for this one step, but no new visible shelter, item, corpse, NPC, water source, or region name appears.
- Invisible spawner overlap is stable pressure, not a visible thing I can react to or click.
- The `Ruins` marker drops to about 116 tiles away, still only overlay knowledge.

Next pressure:

Mira ends at `Point3D(1210,1380,0)`, facing southeast. The fox is at `Point3D(1209,1377,0)`, inside the follow band at floored distance `3`. The final saved rectangle is still empty except for the simulated controlled fox overlay, but six invisible spawner home ranges, the off-screen toads/crane, and the carried swamp-drake private timer branch remain unresolved pressure.

## Run 126 - Jungle Starts Under The Same Empty Screen

I start at `Point3D(1210,1380,0)`, facing southeast, with the fox tucked northwest at `Point3D(1209,1377,0)`. The live-state screen is still a blank sort of nervous: zero saved visible mobiles, zero visible saved items, and zero running spawner object locations inside x `1192..1228`, y `1362..1398`. The fox is the only body I can see, and it only exists here because the state has been simulating its follow ticks forward from the old snapshot.

The `Ruins` marker sits around 116 tiles away. It is not a wall, roof, chest, shrine, road, or NPC. Six invisible `PremiumSpawner` home ranges still overlap the screen by source range, so the quiet grass still has teeth somewhere off the glass.

**Beat 1**

I press southeast.

`Mobile.Move` reaches the real movement path because I am already facing `Direction.Down`. The forward tile `Point3D(1211,1381,0)` is grass `0x3`; the player diagonal side checks `Point3D(1211,1380,0)` and `Point3D(1210,1381,0)` are grass `0x3` and `0x6`. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read finds zero statics on all three tiles, the live snapshot has no destination blocker, and `Regions.xml` gives no named place.

Mira moves to `Point3D(1211,1381,0)`. The shifted rectangle x `1193..1229`, y `1363..1399` is still saved-empty, with the same six invisible spawner home ranges overlapping by source range. The fox does not move on my keypress, so it stretches to dx `-2`, dy `-4`, floored distance `4`.

**Beat 2**

I wait for the fox before spending another step.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels still carries spacing `7`, so the fox wants the `2..3` band. From `Point3D(1209,1377,0)`, it chooses southeast.

The fox target `Point3D(1210,1378,0)` is grass `0x5`; the non-player side checks `Point3D(1210,1377,0)` and `Point3D(1209,1378,0)` are grass `0x3` and `0x5`. Zero statics, no saved blocker. The fox steps there and returns to dx `-1`, dy `-3`, floored distance `3`.

**Beat 3**

The screen is still empty, so I take one more southeast step toward the marker.

`Mobile.Move` enters `CheckMovement` again. The forward tile `Point3D(1212,1382,0)` is land `0x580`, named grass by `tiledata.mul`; the player side checks `Point3D(1212,1381,0)` and `Point3D(1211,1382,0)` are `0x57F` grass and `0x581` jungle. TileData marks all three as not wet and not impassable, and the static block is empty. No saved mobile or item occupies the step, and no named region catches it.

Mira moves to `Point3D(1212,1382,0)`. The final rectangle x `1194..1230`, y `1364..1400` still has zero saved visible mobiles, zero visible saved items, and zero running spawner object locations. The six invisible spawner home ranges are unchanged. The fox stays at `Point3D(1210,1378,0)`, stretched back outside the follow band at floored distance `4`.

Mechanical friction learned:

- The ground is starting to read like jungle again, but the server still treats these exact tiles as passable.
- Empty live-state rectangles are not permission to forget the invisible spawner overlap.
- The fox rhythm keeps taxing every second player step.
- The `Ruins` marker drops to about 113 tiles away, still only overlay knowledge.

Next pressure:

Mira ends at `Point3D(1212,1382,0)`, facing southeast. The fox is at `Point3D(1210,1378,0)`, outside the `2..3` heel band at floored distance `4`. The next honest beat is probably another fox wait unless the fresh scan finally puts a real body, item, road, structure, or region name on screen.

## Run 127 - I Let The Fox Catch Its Breath

I start at `Point3D(1212,1382,0)`, facing southeast, with the fox behind me at `Point3D(1210,1378,0)`. The screen is still clean in the worst way: zero saved visible mobiles, zero visible saved items, and zero running spawner object locations inside x `1194..1230`, y `1364..1400`. Only my simulated fox is visible. The six invisible `PremiumSpawner` home ranges still overlap the rectangle, and the old toads, crane, and swamp drake remain unresolved off-screen pressure.

The `Ruins` marker is about 113 tiles away. It is still only the map whispering at me.

**Beat 1**

I wait for the fox before walking.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels still wants the `2..3` band. From `Point3D(1210,1378,0)` toward me, the fox chooses southeast.

The target `Point3D(1211,1379,0)` is grass `0x3`; side tiles `1211,1378` and `1210,1379` are also grass `0x3`. Zero statics, no saved blocker. The fox steps in, back to dx `-1`, dy `-3`, floored distance `3`.

**Beat 2**

With the leash repaired and nothing visible to click, I press southeast once.

`Mobile.Move` reaches `CheckMovement` because I am already facing `Direction.Down`. The forward tile `Point3D(1213,1383,0)` is jungle `0xAC`; the player side checks `Point3D(1213,1382,0)` and `Point3D(1212,1383,0)` are jungle `0xAD` and `0xAF`. TileData marks them not wet and not impassable, and the static block is empty. No saved mobile or item occupies the step, and no named region catches it.

Mira moves to `Point3D(1213,1383,0)`. The shifted rectangle x `1195..1231`, y `1365..1401` is still saved-empty. The same six invisible spawner ranges overlap by source range only. The fox is stretched again at floored distance `4`.

**Beat 3**

I wait again. The fox is the only visible body I own, and leaving it behind just makes the empty grass feel worse.

The same controlled-follow path runs. From `Point3D(1211,1379,0)` toward me, `GetDirectionTo` resolves southeast. The target `Point3D(1212,1380,1)` is grass `0x3`; side tiles `1212,1379` and `1211,1380` are grass `0x3` with zero statics. The fox steps there and returns to dx `-1`, dy `-3`, floored distance `3`.

Mechanical friction learned:

- The jungle tile under the next step is still mechanically passable, but it is not shelter.
- Empty saved rectangles still do not erase invisible spawner range pressure.
- Two fox waits changed only follower position. They did not run Guard, Attack, body-blocking, hostile aggro response, or damage.
- The `Ruins` marker drops to about 112 tiles away, still overlay knowledge and not a visible wall, road, NPC, or chest.

Next pressure:

Mira ends at `Point3D(1213,1383,0)`, facing southeast. The fox is at `Point3D(1212,1380,1)`, inside the follow band. No saved mobile or item is visible in the final rectangle, but the next run still owes a fresh scan before deciding whether to keep trusting the marker.

## Run 128 - The Jungle Finally Says No

I start at `Point3D(1213,1383,0)`, facing southeast, with the fox tucked northwest at `Point3D(1212,1380,1)`. The screen is still empty in the live save: zero saved visible mobiles, zero visible items, and zero running spawner object locations inside x `1195..1231`, y `1365..1401`. The fox is the only visible body I own. Six invisible `PremiumSpawner` home ranges still overlap the rectangle, so I do not get to call the quiet safe.

The map overlay keeps offering `Ruins` at `1303,1449`, about 112 tiles away. That is still a map mark, not a wall or road on the ground.

**Beat 1**

I press southeast once.

`Mobile.Move` reaches `MovementImpl.CheckMovement` because I am already facing `Direction.Down`. The forward tile `Point3D(1214,1384,0)` is jungle `0xAC`; the player side checks `Point3D(1214,1383,0)` and `Point3D(1213,1384,0)` are jungle `0xAD` and `0xAE`. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read finds all three dry, not impassable, and static-free. No saved blocker or named region catches the step.

Mira moves to `Point3D(1214,1384,0)`. The shifted rectangle x `1196..1232`, y `1366..1402` is still saved-empty. The fox does not move on my keypress and stretches to dx `-2`, dy `-4`, floored distance `4`.

**Beat 2**

I wait for the fox before pushing deeper into empty jungle.

The controlled follower path runs through `AITimer.OnTick`, `BaseCreature.OnThink`, `BaseAI.Obey`, `DoOrderFollow`, `WalkMobileRange`, `GetDirectionTo`, `DoMove(Direction.Down)`, and `Mobile.Move`. FriendsAvoidHeels still carries spacing `7`, so the fox wants the `2..3` band. From `Point3D(1212,1380,1)` toward me, it chooses southeast.

The fox target `Point3D(1213,1381,0)` is grass `0x580`; side tiles `1213,1380` and `1212,1381` are grass `0x57F`. Zero statics, no saved blocker. The fox steps to `Point3D(1213,1381,0)` and returns to dx `-1`, dy `-3`, floored distance `3`.

**Beat 3**

The screen is still empty, so I try one more southeast step. This is where the jungle stops being abstract.

`MovementImpl.CheckMovement` tests `Point3D(1215,1385,0)`. The land tile is jungle `0xAE`, but the static block is not empty: static `0x1775` is named `rock`, has height `3`, and carries `TileFlag.Impassable`. `Paths.IsOk` rejects the destination because that impassable static overlaps my standing height. The movement request fails. Mira stays at `Point3D(1214,1384,0)`, still facing southeast. No `SetLocation`, `OnMovement`, region enter, item move-over, combat, damage, gump, target cursor, skill, hunger, thirst, quest, discovery, follower-count, ownership, or pet-order state changes.

Mechanical friction learned:

- The southeast route is no longer clean: `Point3D(1215,1385,0)` is blocked by an impassable rock static.
- A failed movement key is still a player-facing result; it changes knowledge, not location.
- The fox remains in the heel band after the wait, but it did not guard, scout, body-block, or solve any hostile spawner pressure.
- The final live-state rectangle is still empty of saved mobiles/items, with the same six invisible spawner home ranges overlapping by source range.

Next pressure:

Mira ends at `Point3D(1214,1384,0)`, facing southeast, with the fox at `Point3D(1213,1381,0)`, inside the `2..3` follow band. The next honest action is not another blind southeast retry; it is a fresh scan and a route decision around the rock.

## Run 129 - I Slide Along The Rock

I start at `Point3D(1214,1384,0)`, still facing southeast, with the fox tucked northwest at `Point3D(1213,1381,0)`. The fresh live-state rectangle x `1196..1232`, y `1366..1402` is saved-empty: no visible saved mobiles, no visible saved items, and no running spawner object location. That is not safety. Six invisible `PremiumSpawner` home ranges still overlap by source range, and the old toads, crane, and swamp drake are still unresolved off-screen history.

The world-map overlay still points at `Ruins` around 110 tiles away. The blocked southeast tile is more important than the marker. I cannot just keep pressing into `Point3D(1215,1385,0)` because the rock is a real static, not scenery text.

**Beat 1**

I tap east to start sliding around the north side of the rock.

`Mobile.Move` does not enter `CheckMovement` because I was facing `Direction.Down` and sent `Direction.East`. The normal client result is only a facing change. Mira stays at `Point3D(1214,1384,0)`, now facing east. The screen rectangle does not change. The fox does not move, and no pet AI tick runs.

**Beat 2**

I press east again.

Now the direction matches, so `Mobile.Move` reaches `MovementImpl.CheckMovement`. The forward tile `Point3D(1215,1384,0)` is jungle `0xAC`, dry, not impassable, and static-free. This is not a diagonal move, so the player side-check tiles are not tested. `Regions.xml` has no containing Sosaria rectangle for the tile, and the live snapshot has no mobile or item blocker there.

Mira moves to `Point3D(1215,1384,0)`. The shifted rectangle x `1197..1233`, y `1366..1402` still contains zero saved visible mobiles, zero visible saved items, and zero running spawner object locations. The same six invisible spawner home ranges overlap by range only. The fox stays at `Point3D(1213,1381,0)`, still just inside the heel band at floored distance `3`.

**Beat 3**

I keep sliding east instead of trying southeast immediately. From this tile, the southeast diagonal would still have the rock as a side-check problem, so east is the conservative route-around.

`Mobile.Move` tests `Point3D(1216,1384,0)`. The tile is jungle `0xAD`, dry, not impassable, and has zero statics. There is no saved mobile or item blocker and no named region catch.

Mira moves to `Point3D(1216,1384,0)`, still facing east. The final rectangle x `1198..1234`, y `1366..1402` remains saved-empty except for the simulated controlled fox overlay. The fox did not get a timer beat during this run, so it is now stretched to dx `-3`, dy `-3`, floored distance `4`, outside the remembered `2..3` band.

Mechanical friction learned:

- A blocked diagonal does not mean the whole jungle wall is closed; the north edge has passable east tiles.
- Turning is its own client beat. It can change facing without proving the next tile.
- The rock still matters. From `Point3D(1215,1384,0)`, an immediate southeast diagonal would still be suspect because player diagonal movement requires both side tiles to pass.
- The fox is no shield. Two player steps without a pet tick leave it stretched again.

Next pressure:

Mira ends at `Point3D(1216,1384,0)`, facing east. The controlled fox is at `Point3D(1213,1381,0)`, visible but outside the follow band. No saved mobile, item, corpse, chest, NPC, road, water source, sign, gump, context menu, target cursor, or named region appeared. The next honest beat is likely a fox-follow wait before I spend another movement input toward the `Ruins` marker.

## Run 130 - The First Compressed Walk Finds Another Rock

I start at `Point3D(1216,1384,0)`, facing east, with the fox behind me at `Point3D(1213,1381,0)`. The fresh 18-tile rectangle is still empty in the live save: no saved visible mobiles, no visible saved items, and no running spawner object locations. The fox is the only body on my screen, and even that is the simulated controlled follower state. The old toads, crane, and swamp drake are all off-screen pressure notes, not clickable bodies. Six invisible `PremiumSpawner` home ranges still overlap the screen by range only. The `Ruins` marker is about 109 tiles away, but the marker is not a road.

**Travel Segment**

I use the new routine-travel rhythm instead of pretending every empty grass step is a new decision. First I turn southeast in place; `Mobile.Move` changes only facing from east to `Direction.Down`. Then I walk southeast over five accepted inputs: `1217,1385`, `1218,1386`, `1219,1387`, `1220,1388`, and `1221,1389`. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read shows dry passable jungle all the way. Ferns, leaves, pampas grass, and bulrushes are visible static clutter, but none of those tiles carries the blocking flag.

I shadow the fox instead of spending standalone wait beats. It trails the same southeast line from `Point3D(1213,1381,0)` to `Point3D(1218,1386,0)` through locally clear tiles. That is only follower-shadowing evidence under the travel policy, not an exact `BaseAI` timer proof, and it still does not make the fox a guard.

The sixth southeast input hits the stop rule. `MovementImpl.CheckMovement` tests `Point3D(1222,1390,0)`. The land is jungle, but the static block contains rock `0x1778`, height `2`, with `TileFlag.Impassable`. The move fails, so Mira stays at `Point3D(1221,1389,0)`, facing southeast. No damage, combat, region text, gump, context menu, target cursor, discovery, hunger, thirst, skill, pet order, ownership, or follower-count state changes.

Mechanical friction learned:

- A Travel Segment can compress routine visible movement, but it still stops on the first failed movement input.
- The north-side route around the first rock was real for five southeast steps, then another impassable rock closed the straight line at `Point3D(1222,1390,0)`.
- The final saved rectangle x `1203..1239`, y `1371..1407` is still empty of saved mobiles/items/spawner object locations, but four invisible spawner homes overlap by range only.
- Shadowing the fox back into dx `-3`, dy `-3` keeps the leash coherent; it does not run Guard, Attack, hostile aggro handling, body-blocking, or exact pet timer behavior.

Next pressure:

Mira ends at `Point3D(1221,1389,0)`, facing southeast. The fox is at `Point3D(1218,1386,0)`, inside the remembered `2..3` heel band. There is no visible NPC, corpse, chest, water source, road, sign, gump, or target cursor, but the next action has to be another fresh scan and route decision around rock `0x1778` at `Point3D(1222,1390,0)`.

## Run 131 - The Jungle Finally Moves Back

I start at `Point3D(1221,1389,0)`, facing southeast, with the fox tucked behind me at `Point3D(1218,1386,0)`. The first scan is still quiet: no saved visible mobiles, no visible saved items, and no running spawner object location inside x `1203..1239`, y `1371..1407`. Four invisible `PremiumSpawner` home ranges overlap by range only. The old toads, crane, and swamp drake are not on screen. The forward southeast tile is still the known rock at `Point3D(1222,1390,0)`, so pressing the same line would just bounce off it again.

**Travel Segment**

I slide east instead. The first east input only turns me from `Direction.Down` to `Direction.East`; no movement check runs. Then four east steps carry me through `1222,1389`, `1223,1389`, `1224,1389`, and `1225,1389`. These are dry jungle tiles, and the server-order `map1.mul` / `staidx1.mul` / `statics1.mul` read finds no blocking saved entity or impassable static on the walked tiles.

From there I turn southeast again and take six accepted diagonal steps: `1226,1390`, `1227,1391`, `1228,1392`, `1229,1393`, `1230,1394`, and `1231,1395`. The route matters because I can see what I avoided: the old `0x1778` rock at `1222,1390`, an impassable tree at `1225,1392`, and impassable rocks at `1227,1393`. The path between them is narrow but legal.

The fox is shadowed along the same bypass from `Point3D(1218,1386,0)` to `Point3D(1228,1392,0)`. That keeps the leash believable at dx `-3`, dy `-3`, but it is still only travel-segment follower shadowing, not an exact `BaseAI.Obey` timer tick and not protection.

The stop is not a rock this time. The endpoint scan x `1213..1249`, y `1377..1413` has bodies in it: a panda at `Point3D(1243,1379,0)`, a monkey on the east edge at `Point3D(1249,1381,0)`, and a toad at `Point3D(1243,1382,0)`. No item, chest, corpse, road, water source, gump, context menu, or target cursor appears.

Mechanical friction learned:

- The east-then-southeast bypass around `0x1778` is real for ten accepted movement inputs.
- The jungle is no longer an empty corridor. The live save has visible spawned animals from the eastern `animals.map` spawner.
- Panda, monkey, and toad are all uncontrolled `AI_Animal` / `FightMode.Aggressor` bodies. The constructors pass legacy perception `10`, but `BaseCreature` promotes that to `16`; the important brake is that `FightMode.Aggressor` refuses to acquire anything when there is no aggressor/aggressed/faction/ethic state. The toad's separate teleport timer still uses range `10` and misses Mira.
- A visible animal is still a stop rule. I do not get to keep walking past three new silhouettes just because no combat roll fired immediately.

Next pressure:

Mira ends at `Point3D(1231,1395,0)`, facing southeast. The fox is at `Point3D(1228,1392,0)`, still inside the heel band. The panda, monkey, and toad are on screen and unclicked. The next honest action is to react to those visible creatures before pushing toward the `Ruins` marker.

## Run 132 - I Name The Things Before Moving

I start exactly where the last run left me: `Point3D(1231,1395,0)`, facing southeast, with the fox tucked at `Point3D(1228,1392,0)`. The live rectangle x `1213..1249`, y `1377..1413` still has no saved items and no running spawner object standing in it. It does have three saved wild mobiles from the eastern animals spawner: the toad at `1243,1382`, the panda at `1243,1379`, and the monkey on the east edge at `1249,1381`.

The `Ruins` map marker sits around 90 tiles away. That is a lure, not permission to walk through three living bodies without looking.

**Beat 1**

I single-click the nearest shape first, the toad.

`PacketHandlers.LookReq` finds serial `8544`, passes `CanSee` and the inclusive `Utility.InUpdateRange` rectangle at dx `12`, dy `-13`, asks the region's `OnSingleClick`, then reaches `BaseCreature.OnSingleClick` and `Mobile.OnSingleClick`. The toad is uncontrolled, so there is no tame or bonded line. I get only the private overhead label: `a toad`.

The pressure check stays negative without becoming safety. Toads start a `GiantToad.TeleportTimer`, but that timer scans range `10` from the toad. Mira is outside it. The toad's animal AI has effective `RangePerception 16`, but `FightMode.Aggressor` returns before target scanning because there is no aggressor, aggressed, faction, or ethic state.

**Beat 2**

I single-click the panda next.

The same `LookReq` path accepts serial `206400` at dx `12`, dy `-16`. The label comes back as `a panda`. Again, no context menu, no Tame row, no target cursor, no pet command, and no combat assignment.

The panda is close enough to sit on the effective range-16 edge, but the `Aggressor` acquisition gate is still empty. That is the friction: visible and potentially moving later, not actively committed to attack me now.

**Beat 3**

I single-click the far east-edge body, the monkey.

`Utility.InUpdateRange` still accepts dx `18`, dy `-14`, so the label is legal even at the screen edge. `Mobile.OnSingleClick` sends `a monkey`. The monkey is outside effective range `16` by x distance anyway, and the same no-aggressor gate prevents an `Aggressor` target branch.

Mechanical friction learned:

- Single-clicking wildlife is only a label path. It does not open a context menu, pick Tame, roll skill, attack, move, or mutate the world.
- The old "range-10 perception" note was too shallow. Legacy constructor range `10` is remapped to `16`, but `FightMode.Aggressor` still blocks passive acquisition without prior hostility state.
- The toad's scary part is not the normal animal acquisition check; it is the separate range-10 teleport timer, and that still does not contain Mira from here.
- The screen is identified now, not safe. Future wandering, timer ticks, and spawner behavior are still live uncertainty.

Next pressure:

Mira ends unchanged at `Point3D(1231,1395,0)`, facing southeast. The fox stays at `Point3D(1228,1392,0)`, inside the heel band. The three visible animals are now player-labeled as a toad, a panda, and a monkey. The next decision is whether to back away, skirt wider, or risk another careful route toward the `Ruins` marker.

## Run 133 - The Jungle Catches My First Step

I start at `Point3D(1231,1395,0)`, still facing southeast. The fox is tucked behind me at `Point3D(1228,1392,0)`, inside the remembered heel band. The screen is not empty: the labeled toad, panda, and monkey are still north-east of me, and the map overlay keeps whispering `Ruins` at `1303,1449`, about 90 tiles away. None of those animals has an active attack branch committed, but they are bodies on the screen, not background text.

**Beat 1**

I try the obvious southeast step toward the marker.

The target tile `Point3D(1232,1396,0)` looks reasonable at first: land `0xAF`, jungle, dry, passable, with only a nonblocking blade plant. But this is a player diagonal move, so `MovementImpl.CheckMovement` tests both side tiles too. The north-east side tile `Point3D(1232,1395,0)` has an impassable `0xD59` tree, height `20`, plus harmless leaves. The south-west side tile `Point3D(1231,1396,0)` is passable jungle clutter. One blocked side is enough for a player diagonal, so `CheckMovement` returns false.

Mira does not move. No region, item move-over, teleporter, hunger, thirst, combat, pet AI tick, context menu, gump, skill, discovery, ownership, follower-count, or quest path runs. The fox stays put, and the animals stay visible at the same saved positions. The failed keypress still teaches me something: the route toward the ruins is not the clean southeast line my eyes wanted it to be.

Mechanical friction learned:

- `Point3D(1232,1396,0)` is not the blocker; the diagonal side-check tree at `Point3D(1232,1395,0)` is.
- Player diagonals are stricter than pet diagonals here: both side checks must pass.
- The visible toad, panda, and monkey remain labeled screen pressure, but the failed movement did not advance their AI timers or create combat.
- The `Ruins` marker is still only navigation overlay knowledge.

Next pressure:

Mira ends unchanged at `Point3D(1231,1395,0)`, facing southeast, with the fox still at `Point3D(1228,1392,0)`. The next honest action is a fresh scan and a route choice around the `0xD59` side-check tree rather than another blind southeast retry.

## Run 134 - The Bypass Shows Teeth

I start at `Point3D(1231,1395,0)`, facing southeast, with the fox behind me at `Point3D(1228,1392,0)`. The southeast key already lied to me once: the forward tile was fine, but the diagonal side-check tree at `1232,1395` made the move fail. East is not the answer either, because that is the same tree tile. South is the only visible way to slide around it without pretending the collision did not happen.

**Travel Segment**

I turn south first. That is just a facing change, not a step. Then I press south again and move to `Point3D(1231,1396,0)`. The tile is dry jungle `0xAD`; the tree and leaves there are decorative, not impassable.

Now I turn southeast and take three accepted southeast steps: `1232,1397`, `1233,1398`, and `1234,1399`. The player diagonal side checks all pass. `1232,1396` has only a nonblocking blade plant; `1231,1397`, `1233,1397`, `1232,1398`, `1234,1398`, and `1233,1399` are dry passable jungle with no blocking statics. No saved mobile or item is standing on the stepped tiles, and no named region catches the movement.

I shadow the fox to `Point3D(1231,1396,0)`, keeping the old follow spacing believable at dx `-3`, dy `-3`. That is travel-segment shadowing, not an exact `BaseAI.Obey` tick, and it does not make the fox a guard.

The segment stops at the endpoint scan. The old panda has slipped off the top of the screen, but the toad and monkey are still on the north edge. More importantly, a new saved hostile appears at the south edge: `Urulg`, an orc, at `Point3D(1226,1417,0)`, dx `-8`, dy `18`. The orc is `AI_Melee` / `FightMode.Closest`; its constructor range `10` becomes effective `RangePerception 16`, so I am just outside its acquisition rectangle by y distance. That is not safety. Its private AI timer is not exported, and its movement-notice sound branch depends on enemy/LOS checks I am not going to hand-wave.

Mechanical friction learned:

- South then southeast is a real bypass around the `0xD59` side-check tree.
- The bypass does not open a clean corridor. It pulls the southern world spawner into sight.
- The visible orc is a hard stop even before combat starts. It is hostile screen pressure, not scenery.
- The `Ruins` marker is closer, about 85 tiles away, but the marker just lost priority to a living enemy on the edge of the client view.

Next pressure:

Mira ends at `Point3D(1234,1399,0)`, facing southeast. The fox is approximately at `Point3D(1231,1396,0)`, still inside the follow band. Visible bodies are the monkey at `1249,1381`, the toad at `1243,1382`, and the newly visible orc at `1226,1417`. The next honest action is to deal with or back away from the orc pressure, not to keep walking toward the marker.

## Run 135 - The Orc's Clock Is Not Mine

I start exactly where the orc stopped me: `Point3D(1234,1399,0)`, facing southeast. The fox is behind me at about `Point3D(1231,1396,0)`. The screen still has the labeled toad at `1243,1382`, the labeled monkey at `1249,1381`, and `Urulg` the orc at `1226,1417`, dx `-8`, dy `18`. No item, chest, corpse, road, water source, gump, context menu, or target cursor is visible.

The `Ruins` marker is still about 85 tiles away. It can wait. A visible hostile at the south edge is not a navigation problem yet; it is a clock problem.

**Beat 1**

I do not click the orc. I do not turn. I do not take the northwest step my hands want. The pressure rule gets the first move.

`Orc` constructs as `AI_Melee` / `FightMode.Closest` with range `10`, and `BaseCreature` remaps that old range to `16`. If the private `AITimer` ticks, the path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Think -> DoActionWander`. The acquisition part is clear enough: `AcquireFocusMob` would scan range `16` around the orc, and Mira at dy `18` is not in that rectangle. So I do not invent an immediate combatant.

Then the code falls into the part I cannot see from a save snapshot. `DoActionWander` checks `BaseAI.m_NextMove`; if that private timer is due, `WalkRandomInHome` can pick a direction and move the orc. That changes the screen and possibly the next range check. The live export gives me the orc's saved position, not its timer due state, not `m_NextMove`, and not the random movement direction.

So the beat stops before a player input. Mira stays at `Point3D(1234,1399,0)`, facing southeast. The fox stays modeled at `Point3D(1231,1396,0)`. No combat, damage, movement, gump, menu, target cursor, hunger, thirst, skill, ownership, follower count, quest, discovery, or pet order changes.

Mechanical friction learned:

- A visible hostile can block even a retreat key when its AI clock is unresolved.
- The current range math is negative for immediate acquisition: dy `18` is outside the orc's effective range `16`.
- Negative acquisition is not safety. The unresolved `m_NextMove` branch could still move the orc before I move.
- The next run may recheck the exact same private timer branch; only after the repeated-private-timer loop-break can I treat it as carried unresolved risk rather than the active blocker.

Next pressure:

Mira is still pinned at `Point3D(1234,1399,0)`. Urulg remains visible active pressure at `1226,1417`. The toad and monkey are still edge wildlife, the panda is off-screen ambient uncertainty, the fox is only a follower, and the `Ruins` marker remains lower priority than the orc's unresolved clock.

## Run 136 - I Back Out Of The Orc's Screen

I start at the same ugly edge: `Point3D(1234,1399,0)`, facing southeast. The fox is still behind me at `Point3D(1231,1396,0)`. The screen has the labeled toad at `1243,1382`, the labeled monkey at `1249,1381`, and Urulg the orc at `1226,1417`, dx `-8`, dy `18`. There are no saved visible items, no chest, no corpse, no road, no water source, no gump, no context menu, and no target cursor.

**Beat 1**

The orc's clock gets checked again before I move.

The code path is the same one that stopped the last run: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Think -> MeleeAI.DoActionWander -> AcquireFocusMob -> base.DoActionWander -> CheckMove`. The geometry is still negative for immediate acquisition. Urulg's `RangePerception` is `16`, and Mira is still outside that rectangle by y distance.

The unresolved part is also exactly the same. The live snapshot still does not export the private AI timer due state, `NextReacquireTime`, `BaseAI.m_NextMove`, or the `WalkRandomInHome` direction roll. I do not choose due or not-due. Because this is the same unchanged private timer branch after the prior full stop, I mark it as carried unresolved risk instead of spending another run staring at the missing field. That is not safety. It only lets me do the conservative thing: leave the orc's screen.

**Beat 2**

I press northeast. Since I was facing southeast, `Mobile.Move` only changes facing to `Direction.Right`; no `CheckMovement`, `OnMoveOver`, `Region.CanMove`, pet AI tick, combat, or item interaction runs.

The turn still sends movement notification to nearby objects. Urulg does not get a notice-sound event: I was already inside its 18-tile movement-notice edge before the turn, and orcs do not have `ReacquireOnMovement` set like the swamp drake did. The screen stays the same.

**Beat 3**

I press northeast again, now actually stepping to `Point3D(1235,1398,0)`.

`MovementImpl.CheckMovement` tests the forward tile `1235,1398` and the two player diagonal side-check tiles `1235,1399` and `1234,1398`. Server-order `map1.mul` reads them as dry, passable jungle/grass edge tiles at z `0`; `staidx1.mul` and `statics1.mul` find zero blocking statics on all three. The live saved mobile/item filters find no blocker on the target tile.

Mira moves one tile northeast. The fox does not get an AI tick, so it stays at `Point3D(1231,1396,0)` and stretches to floored distance `4`, outside the old heel band. Urulg drops just outside the client rectangle at dy `19`; that does not mean it vanished, stopped, or became safe. It is now `carried_unresolved_not_active_blocker` instead of visible active pressure.

Mechanical friction learned:

- The repeated private-timer loop-break is not permission to claim a creature outcome. It only permits conservative player action while carrying both timer branches.
- A facing turn is real time and sends movement notification, but it does not test terrain or move the character.
- The northeast retreat tile is open. The straight southeast route toward `Ruins` is still the wrong instinct while a hostile spawner body is this close.
- The fox is trailing again. One retreat step without a pet tick leaves it outside the remembered `2..3` spacing band.

Next pressure:

Mira ends at `Point3D(1235,1398,0)`, facing northeast. The visible saved bodies are now only the monkey at `1249,1381` and the toad at `1243,1382`; the orc is recently visible at `1226,1417`, off-screen by one tile of y distance and still inside movement-notice range. The next honest move is a fresh scan and either another exposure-reducing step or a fox-follow regroup, not a rush toward the `Ruins` marker.

## Run 137 - The Fox Catches Up, The Panda Comes Back

I start at `Point3D(1235,1398,0)`, facing northeast. The fox is visible behind me at `Point3D(1231,1396,0)`, stretched outside the remembered `2..3` heel band. The live screen still has the labeled toad at `1243,1382` and monkey at `1249,1381`. The panda is just above the top edge, and Urulg is just below the south edge, carried as unresolved hostile pressure rather than erased.

**Beat 1**

I wait for the fox instead of dragging the leash farther.

The controlled AI path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. The `FriendsAvoidHeels` value is still `7`, so the fox wants to sit `2..3` tiles from me. Its floored distance is `4`, so `WalkMobileRange` points it `Direction.Down`, southeast.

The terrain cooperates. The fox steps from `Point3D(1231,1396,0)` to `Point3D(1232,1397,0)`. Server-order `map1.mul` reads the target as dry jungle at z `0`; `staidx1.mul` and `statics1.mul` find no static on the target. One side tile has only a nonblocking blade plant, and the other side tile is clear. The fox lands at dx `-3`, dy `-1`, floored distance `3`.

No Guard, Attack, Combatant, body-blocking, ownership, follower-count, pet-order, gump, context menu, target cursor, skill, item, hunger, thirst, quest, discovery, or combat path changes.

**Beat 2**

Now I press northeast again, because I am already facing that way and I still want one more tile away from the orc's southern edge.

`Mobile.Move` enters the full movement path. `MovementImpl.CheckMovement` tests `Point3D(1236,1397,0)` and the player diagonal side checks at `1236,1398` and `1235,1397`. The target and east side tile are dry jungle with zero statics. The north side tile has a plant static, but it is not a blocker, so the player diagonal passes.

Mira moves to `Point3D(1236,1397,0)`. The scan changes immediately. The rectangle x `1218..1254`, y `1379..1415` now contains the panda again at `1243,1379`, plus the monkey and toad. There are still zero saved visible items and zero running spawner object locations inside the rectangle. Urulg is now off-screen at dx `-10`, dy `20`, still inside movement-notice range and still carried as the same unchosen private-timer branch. The fox, after doing exactly what I waited for, is stretched again at `Point3D(1232,1397,0)`, dx `-4`, dy `0`, floored distance `4`.

Mechanical friction learned:

- Waiting for a controlled follower tick is not protection. It only moved the fox one clear tile back into range.
- A single player step can immediately stretch the follower back out of the band.
- The northeast retreat tile is passable, but it brings the already-labeled panda back onto the client edge.
- The orc being off-screen is still not a resolved creature outcome. The live snapshot still does not expose its private timer or next move.

Next pressure:

Mira ends at `Point3D(1236,1397,0)`, facing northeast. Visible bodies are now the panda, monkey, toad, and the controlled fox; zero saved items are visible. Urulg and the swamp drake remain carried unresolved risks. The next honest action is a fresh classifier pass and a decision about the re-visible panda/toad/monkey cluster before any more movement.

## Run 138 - I Stop Dragging The Leash

I start at `Point3D(1236,1397,0)`, facing northeast. The fox is visible west of me at `Point3D(1232,1397,0)`, stretched outside the remembered `2..3` heel band again. The screen is not clean: the panda is still at the top edge, the toad and monkey are still north-east, and Urulg is below the screen as carried hostile uncertainty. No saved item, chest, corpse, road, water source, gump, context menu, or target cursor is visible.

**Beat 1**

I wait for the fox first. It is not a shield, but I do not want to keep walking with the follower lagging.

The controlled AI path is the same follow machinery as before: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. The fox is four tiles away by floored distance, outside the `FriendsAvoidHeels` band, and the direction to me is east.

`DoMove(Direction.East)` steps the fox from `Point3D(1232,1397,0)` to `Point3D(1233,1397,0)`. The target is dry jungle `0xAF` at z `0`, with zero statics and no saved blocker. The fox ends at dx `-3`, dy `0`, floored distance `3`.

No Guard, Attack, Combatant, ownership, follower-count, pet-order, body-blocking, gump, target cursor, item, hunger, thirst, quest, discovery, or combat path changes.

**Beat 2**

I choose north, not southeast. The ruins marker can keep whispering; the southern edge has already produced an orc.

The first north input only turns me. `Mobile.Move` sees that I was facing `Direction.Right`, so it skips `MovementImpl.CheckMovement`, leaves me at `Point3D(1236,1397,0)`, and changes facing to `Direction.North`. The visible rectangle and creature pressure do not change.

**Beat 3**

I press north again and actually step.

`MovementImpl.CheckMovement` tests the forward tile `Point3D(1236,1396,0)`. Server-order `map1.mul` reads it as dry jungle `0xAC` at z `0`, `staidx1.mul` and `statics1.mul` find zero statics, and the live saved mobile/item filters find no target blocker. This is a cardinal move, so there are no player diagonal side checks.

Mira moves to `Point3D(1236,1396,0)`, facing north. The endpoint scan x `1218..1254`, y `1378..1414` still contains the same three labeled wild animals: panda at `1243,1379`, toad at `1243,1382`, and monkey at `1249,1381`. There are still zero saved visible items and zero running spawner object locations. Urulg is now dx `-10`, dy `21`, still off-screen and still carried unresolved; the southern goblin archer is also off-screen at dx `14`, dy `23`, route-adjacent but not visible. The fox is at `1233,1397`, inside the follow band at floored distance `3`.

Mechanical friction learned:

- Waiting for the controlled fox can restore spacing without changing protection, ownership, or combat state.
- A facing turn is its own visible beat and does not prove the next tile.
- The north tile is open, but it does not clear the screen. The panda, toad, and monkey are still visible bodies.
- Stepping away from Urulg improves geometry, but it does not resolve the orc's private timer branch or prove the goblin archer irrelevant.

Next pressure:

Mira ends at `Point3D(1236,1396,0)`, facing north. The fox is in the heel band at `Point3D(1233,1397,0)`. The panda, toad, and monkey remain visible; Urulg and the swamp drake remain carried unresolved risks, and the goblin archer is off-screen route-adjacent pressure. The next action needs another scan and classifier before any route push toward the `Ruins` marker.

## Run 139 - North Has Its Own Teeth

I start at `Point3D(1236,1396,0)`, facing north. The fox is where I want it for once, behind and a little west at `Point3D(1233,1397,0)`, still following and inside the old heel band. The screen is not empty. The panda is up near the top edge at `1243,1379`, the toad is at `1243,1382`, and the monkey is at `1249,1381`. Urulg is below the screen as carried hostile uncertainty, and the goblin archer is still south of me, route-adjacent but not visible.

**Beat 1**

I press north because I am already facing that way and because south is where the orc and goblin pressure lives.

`MovementImpl.CheckMovement` tests `Point3D(1236,1395,0)`. Server-order `map1.mul` reads it as dry jungle `0xAC` at z `0`; `staidx1.mul` and `statics1.mul` find no statics there, and the live saved item/mobile filters find no target blocker. This is a cardinal step, so there are no diagonal side checks.

Mira moves to `Point3D(1236,1395,0)`. The same three labeled wild animals stay visible. The toad is now dx `7`, dy `-13`, still outside its range-10 teleport scan. Urulg falls to dy `22`, and the goblin archer to dy `24`. That is better geometry, not safety.

**Beat 2**

I press north again.

The target `Point3D(1236,1394,0)` is also dry jungle `0xAC`. This tile has leaves `0xD8C`, but the tiledata flags are foliage only, not impassable or surface, so the step passes. No saved mobile or item is on the tile. Mira moves to `Point3D(1236,1394,0)`.

The panda, toad, and monkey are still on the screen. The toad is now dx `7`, dy `-12`, still outside the timer's range-10 rectangle. The fox does not get an AI tick during these player steps, so it stays at `1233,1397`, stretching to floored distance `4`.

**Beat 3**

I take one more north step, but I am watching the toad now.

`Point3D(1236,1393,0)` is dry jungle `0xAC`, zero statics, no saved blocker. `Mobile.Move` accepts the step and leaves Mira facing north at `Point3D(1236,1393,0)`.

The screen still has the panda, toad, and monkey. The toad is dx `7`, dy `-11`: still not inside the range-10 teleport scan, but one more north step would put me at dy `-10`, which is inside the timer rectangle if it ticks and wins its random branch. The fox is still visible behind me at `Point3D(1233,1397,0)`, now floored distance `5`, outside the heel band again. I stop there. North was useful, but it is about to become a different problem.

Mechanical friction learned:

- A cardinal north step only needs the forward tile; it does not use the player diagonal side-check rule.
- Leaves are not automatically blockers. `0xD8C` on `1236,1394` is foliage only, so movement passed.
- Moving north improves distance from Urulg and the southern goblin archer, but it also pulls me toward the toad's special timer range.
- The toad's teleport timer is still negative evidence at dy `-11`; the next north step would change that to active timer pressure.
- Three player steps without a pet AI tick stretch the fox back outside the follow band.

Next pressure:

Mira ends at `Point3D(1236,1393,0)`, facing north. The fox is at `Point3D(1233,1397,0)`, outside the heel band. The panda, toad, and monkey remain visible, and the toad is one north step away from containing Mira in its range-10 timer scan. The next honest move is likely to wait the fox back in or choose an east/west route that avoids both the southern hostiles and the toad's timer rectangle.

## Run 140 - I Sidestep The Toad's Clock

I start at `Point3D(1236,1393,0)`, facing north. The fox is visible behind me at `Point3D(1233,1397,0)`, stretched to floored distance `5`. The panda, toad, and monkey are still on screen. The toad is close enough that one more north step would put me inside its range-10 teleport timer scan. South still smells like Urulg and the goblin archer, even though both are off-screen.

**Beat 1**

I wait for the fox instead of taking the tempting north tile.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still leaves the fox wanting distance `2..3`. From `1233,1397`, the direction to me is `Direction.Right`, so it steps northeast to `Point3D(1234,1396,0)`.

The fox target, the start tile, and both non-player diagonal side tiles are dry map1 jungle at z `0` with zero statics. No saved mobile or item is on the target. The fox ends at dx `-2`, dy `3`, floored distance `3`. It is following again, not guarding me.

**Beat 2**

I turn west. `Mobile.Move` changes facing from north to `Direction.West` without entering `MovementImpl.CheckMovement`, so no tile is tested and no location changes.

The turn still sends movement notifications. The visible wild animals are `FightMode.Aggressor`, so their notice-sound branch is skipped. Urulg is still outside range 18 before and after the turn, so no notice sound is produced there either.

**Beat 3**

I press west again and actually step to `Point3D(1235,1393,0)`.

This is a cardinal movement check. `MovementImpl.CheckMovement` only needs the forward tile. Server-order `map1.mul` reads `1235,1393` as dry jungle `0xAC` at z `0`; `staidx1.mul` and `statics1.mul` find zero statics, and the live saved mobile/item filters find no target blocker. `Region.CanMove` and `OnMoveOver` have no traced blocker.

The final screen is basically the same, but the geometry is a hair less stupid. The panda is at dx `8`, dy `-14`; the toad is at dx `8`, dy `-11`, still outside its teleport timer scan; and the monkey is at dx `14`, dy `-12`. The fox is at dx `-1`, dy `3`, still in the follow band. Urulg is off-screen at dx `-9`, dy `24`, carried unresolved rather than safe. The goblin archer is dx `15`, dy `26`, still route-adjacent if I drift south. No item, corpse, chest, water source, road, sign, gump, context menu, target cursor, combat, hunger, thirst, quest, discovery, ownership, follower-count, or skill state changes.

Mechanical friction learned:

- Waiting for the fox is a leash action, not protection.
- Facing west consumes a visible beat and not a terrain proof.
- A one-tile west sidestep avoids entering the toad's current range-10 timer rectangle.
- The client world-map `Ruins` marker is still only overlay knowledge at about 88 tiles southeast; it is not a reason to walk through southern hostile pressure.

Next pressure:

Mira ends at `Point3D(1235,1393,0)`, facing west. The fox is back in the heel band at `Point3D(1234,1396,0)`. The same panda, toad, and monkey remain visible; the toad is still timer-negative but close. Urulg, the goblin archer, and the old swamp drake remain unresolved carried risks rather than solved threats.

## Run 141 - I Step Sideways Before I Step North

I start at `Point3D(1235,1393,0)`, facing west. The fox is close enough behind me at `Point3D(1234,1396,0)`, still following and inside the remembered heel band. The screen still has the labeled panda at `1243,1379`, the toad at `1243,1382`, and the monkey at `1249,1381`. The toad is not inside its range-10 teleport scan yet, but one sloppy north move from the old line would have changed that. South is still where Urulg and the goblin archer sit in the back of my mind.

**Travel Segment**

I keep moving west first.

Because I am already facing `Direction.West`, the first three inputs are real steps: `1234,1393`, `1233,1393`, and `1232,1393`. These are cardinal moves, so there are no player diagonal side checks. The server-order `map1.mul` read says all three tiles are dry jungle. The statics are visual clutter, not blockers: mushrooms, a tree, and leaves on `1234,1393`, nothing on `1233,1393`, and a tree with leaves on `1232,1393`, all without impassable or surface flags. The live snapshot has no saved mobile or item on any of those targets.

Then I turn north in place and take one actual north step to `Point3D(1232,1392,0)`. That tile is dry jungle with zero statics and no saved blocker. This is not progress toward the `Ruins` marker. It is progress away from the toad's timer rectangle and away from the southern hostile line.

I let the travel segment keep the fox believable instead of pretending an exact private AI clock fired. The follower is shadowed one clear northwest step from `Point3D(1234,1396,0)` to `Point3D(1233,1395,0)`. The target and both non-player side tiles are dry jungle with zero statics, and no Guard, Attack, Combatant, body-blocking, ownership, follower-count, or pet-order state changes.

The endpoint scan is boring in the useful way. The same three wild bodies remain visible: panda dx `11`, dy `-13`; toad dx `11`, dy `-10`; monkey dx `17`, dy `-11`. The toad is now still outside the range-10 teleport rectangle because the x distance is `11`. There are zero saved visible items and zero running spawner objects standing in the rectangle. Urulg is off-screen at dx `-6`, dy `25`, and the goblin archer is off-screen at dx `18`, dy `27`; both are still unresolved risk, not gone.

Mechanical friction learned:

- West first is the honest way to make a north step without entering the toad's range-10 timer rectangle.
- Nonblocking jungle statics can look dense without stopping cardinal movement.
- Travel-segment fox shadowing keeps the leash coherent, but it is not an exact pet timer tick and not protection.
- The `Ruins` marker is now farther away, about 91 tiles by straight distance, and still lower priority than visible wildlife and southern hostile pressure.

Next pressure:

Mira ends at `Point3D(1232,1392,0)`, facing north. The fox is approximately at `Point3D(1233,1395,0)`, inside the heel band. The panda, toad, and monkey remain visible, with the toad timer-negative by x distance instead of y distance. Urulg, the goblin archer, and the old swamp drake remain carried unresolved risks. The next run needs a fresh scan before deciding whether to keep skirting west/north or finally turn back toward a marker.

## Run 142 - I Keep North Narrow

I start at `Point3D(1232,1392,0)`, facing north. The screen is still not empty: the panda is at `1243,1379`, the toad at `1243,1382`, and the monkey at `1249,1381`. The toad is close in y, but the important number is still x. At dx `11`, its range-10 teleport scan does not contain me. The fox is behind at `Point3D(1233,1395,0)`, close enough for now. No item, corpse, chest, water source, road, sign, gump, context menu, target cursor, combat, or taming timer is visible.

**Beat 1**

I press north, because I am already facing north and because north preserves the x gap from the toad.

`Mobile.Move` enters `MovementImpl.CheckMovement` for a cardinal step. The target `Point3D(1232,1391,0)` is dry jungle `0xAE`, z `0`, with zero statics. There is no saved mobile or item on the target. Mira moves one tile north.

The shifted screen still contains the same three wild bodies and no saved items: panda dx `11`, dy `-12`; toad dx `11`, dy `-9`; monkey dx `17`, dy `-10`. The toad is still outside the teleport timer rectangle by x distance. The fox is now stretched to floored distance `4`.

**Beat 2**

I wait the fox in. That is not courage; it is leash management.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still means the fox wants the `2..3` band. From `Point3D(1233,1395,0)`, `GetDirectionTo` points north toward me. The fox steps to `Point3D(1233,1394,0)`.

The target is dry jungle `0xAE`. It has a tree and leaves, but their tile flags are nonblocking/foliage only, so they do not stop the controlled follower. No Guard, Attack, Combatant, Warmode, body-blocking, ownership, follower-count, pet-order, hunger, thirst, quest, discovery, or skill state changes.

**Beat 3**

I take one more north step while the x gap is still doing the work.

`MovementImpl.CheckMovement` accepts `Point3D(1232,1390,0)`. The tile is dry jungle `0xAF`; the tree and leaves there are visual statics, not impassable or surface blockers. The live saved mobile/item filters find no target blocker.

The final scan is still the same problem, just shifted north: panda dx `11`, dy `-11`; toad dx `11`, dy `-8`; monkey dx `17`, dy `-9`. Zero saved visible items and zero running spawner objects stand in the rectangle, though the same four PremiumSpawner home ranges overlap by source range. Urulg is farther south at dx `-6`, dy `27`; the goblin archer is dx `18`, dy `29`; both stay unresolved risk, not solved. The fox is behind at `Point3D(1233,1394,0)`, stretched again to floored distance `4`.

Mechanical friction learned:

- Cardinal north movement keeps avoiding the toad's special timer only because x stays `11`.
- The jungle tree graphics on the north line are not blockers when tiledata lacks impassable/surface flags.
- The fox follow tick can fix spacing for one beat, but a single player step stretches it back out.
- Moving away from southern hostiles is useful geometry, not proof that Urulg or the goblin archer stopped existing.

Next pressure:

Mira ends at `Point3D(1232,1390,0)`, facing north. The fox is at `Point3D(1233,1394,0)`, outside the heel band again. The panda, toad, and monkey remain visible; the toad is still timer-negative by x distance. The next honest move is another fresh scan/classifier pass, probably a fox-follow wait or another north/west avoidance step, not a turn back toward the `Ruins` marker.

## Run 143 - I Let The Leash Breathe

I start at `Point3D(1232,1390,0)`, facing north. The fox is visible behind me at `Point3D(1233,1394,0)`, just stretched past the old heel band. The same three wild bodies are still on screen: panda `206400` at `1243,1379`, toad `8544` at `1243,1382`, and monkey `289478` at `1249,1381`. The toad is not inside its teleport timer rectangle because the x gap is still `11`. No item, corpse, chest, sign, water source, gump, context menu, target cursor, or combat feedback is visible.

**Beat 1**

I wait for the fox before I move.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. The fox is four tiles away by floored distance, so `FriendsAvoidHeels` wants it closer. `GetDirectionTo` chooses `Direction.North`, and `DoMove` steps the fox from `Point3D(1233,1394,0)` to `Point3D(1233,1393,0)`.

The target is dry jungle `0xAF`, z `0`, with zero statics and no saved mobile or item blocker. The fox ends at dx `1`, dy `3`, floored distance `3`. It is following, not guarding.

**Beat 2**

I press north once while the toad is still held off by x distance.

`Mobile.Move` enters `MovementImpl.CheckMovement` because I was already facing north. The target `Point3D(1232,1389,0)` is dry jungle `0xAF`, z `0`, with zero statics. The live saved mobile/item filters find no target blocker, and no named region gate is traced. Mira steps to `Point3D(1232,1389,0)`.

The screen shifts but does not clear. Panda is dx `11`, dy `-10`; toad is dx `11`, dy `-7`; monkey is dx `17`, dy `-8`. Zero saved visible items and zero running spawner object locations are in the rectangle. Urulg is still off-screen at dx `-6`, dy `28`, and the goblin archer is off-screen at dx `18`, dy `30`. The fox is stretched again to floored distance `4`.

**Beat 3**

I wait the fox back in again instead of taking another north step.

The same follow path runs. From `Point3D(1233,1393,0)`, `GetDirectionTo` again resolves north. The fox steps to `Point3D(1233,1392,0)`. The tile is dry jungle `0xAD`; static `0xD58` is a nonblocking tree and `0xD5E` is foliage leaves, so the target does not block the non-player movement. No saved mobile or item occupies the tile.

The endpoint is quiet but not safe. The fox is back inside the heel band at dx `1`, dy `3`. The panda, toad, and monkey are still visible, and the toad is still timer-negative only because x remains `11`. The `Ruins` marker is still only map-overlay knowledge, now about `93` tiles straight-line away and south-east of the pressure I am deliberately avoiding.

Mechanical friction learned:

- Waiting the fox before and after a player step keeps the follower coherent without creating any Guard, Attack, body-block, or protection state.
- A north player step from here is locally passable, but it does not remove visible animal pressure.
- The toad remains a geometry problem rather than a resolved one. The timer scan still misses by x distance, not because the timer is proven idle.
- Moving north increases distance from Urulg and the goblin archer, but those branches remain carried unresolved risk, not erased enemies.

Next pressure:

Mira ends at `Point3D(1232,1389,0)`, facing north. The fox is at `Point3D(1233,1392,0)`, inside the follow band. Panda, toad, and monkey remain visible; the toad is timer-negative by dx `11`, dy `-7`. Urulg, the goblin archer, and the old swamp drake remain off-screen carried or ambient risks. The next move needs another scan/classifier pass before choosing between another cautious north step, a west sidestep, or stopping to inspect the visible wildlife again.

## Run 144 - I Give The Toad More Side-Eye

I start at `Point3D(1232,1389,0)`, facing north. The fox is close behind at `Point3D(1233,1392,0)`, inside the old heel band at dx `1`, dy `3`. The screen is still not clean. The panda is at `1243,1379`, the toad at `1243,1382`, and the monkey at `1249,1381`. The toad is the number I keep staring at: dx `11`, dy `-7`, just outside its range-10 teleport scan because of the x gap. South is where Urulg and the goblin archer sit in the unresolved part of my head.

**Beat 1**

I turn west first.

`Mobile.Move` sees that I was facing `Direction.North`, so this is only a facing change to `Direction.West`; it does not enter `MovementImpl.CheckMovement` and it does not change my location. The turn still notifies nearby objects through `OnMovement`. The visible animals are `FightMode.Aggressor`, so the notice-sound branch is skipped, and no combatant, focus, teleport, item, region, gump, target cursor, pet order, hunger, thirst, quest, discovery, or skill path changes.

**Beat 2**

I press west again and actually step to `Point3D(1231,1389,0)`.

This is a cardinal movement check. `MovementImpl.CheckMovement` only needs the forward tile. Server-order `map1.mul` reads `1231,1389` as dry jungle `0xAD`, z `0`, with zero statics. The live snapshot has no saved mobile or item standing on the target tile, and the region scan found no named `Regions.xml` rectangle here.

The screen stays populated, but the geometry gets better. The panda is dx `12`, dy `-10`; the toad is dx `12`, dy `-7`; the monkey is still barely legal on the east edge at dx `18`, dy `-8`. The toad is still timer-negative by x distance. The fox remains at `1233,1392`, now dx `2`, dy `3`, still inside the follow band without spending a pet tick.

**Beat 3**

I take one more west step while that line is still open.

`MovementImpl.CheckMovement` accepts `Point3D(1230,1389,0)`. The land is dry jungle `0xAD`; there is one `0xD18` mushroom on the tile, but tiledata marks it nonblocking, not a surface or bridge. No saved mobile or item blocks the target, and no named region catches the move.

The monkey finally drops off the client rectangle at dx `19`. I do not pretend it disappeared. It becomes recently visible ambient uncertainty. The panda and toad are still visible: panda dx `13`, dy `-10`, toad dx `13`, dy `-7`. The toad's range-10 timer scan still misses me by x distance. The fox is still behind at `Point3D(1233,1392,0)`, dx `3`, dy `3`, inside the heel band. Urulg is farther south at dx `-4`, dy `28`; the goblin archer is dx `20`, dy `30`; both remain unresolved, not solved.

Mechanical friction learned:

- A facing turn is a real visible beat, but it is not a terrain proof.
- Two west tiles from the current line are passable Sosaria jungle; `1230,1389` only has a nonblocking mushroom.
- Moving west is the clean way to widen the toad timer gap without dragging the fox out of the follow band.
- Leaving the monkey's update rectangle is not deletion or safety. It only changes the screen and pressure classification.

Next pressure:

Mira ends at `Point3D(1230,1389,0)`, facing west. The fox is still at `Point3D(1233,1392,0)`, inside the 2..3 heel band. The panda and toad remain visible, with the toad timer-negative at dx `13`, dy `-7`. The monkey is just off-screen to the east. The next move needs a fresh scan/classifier before any further west step, north turn, or route back toward the `Ruins` marker.

## Run 145 - The Leash Math Catches Me

I start at `Point3D(1230,1389,0)`, facing west. The screen still has the panda at `1243,1379` and the toad at `1243,1382`; the monkey is off the east edge now, not gone. Before I move, I catch the stupid part: the fox at `1233,1392` is not actually inside the heel band. `Mobile.GetDistanceToSqrt` uses real square-root distance, so dx `3`, dy `3` floors to `4`, not `3`. My little fox is already stretched.

**Beat 1**

I wait for the fox.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the old `2..3` band. From `Point3D(1233,1392,0)` to me at `Point3D(1230,1389,0)`, `GetDirectionTo` chooses `Direction.Up`, so the fox steps northwest to `Point3D(1232,1391,0)`.

That target is dry jungle `0xAE`, z `0`, with zero statics and no saved mobile or item blocker. The fox is back at dx `2`, dy `2`, floored distance `2`. It is following, not guarding.

**Beat 2**

I press west while I am already facing west.

`Mobile.Move` enters `MovementImpl.CheckMovement` for the cardinal step. `Point3D(1229,1389,0)` is dry jungle `0xAF`, z `0`, with zero statics and no saved blocker. Mira moves there. Panda is dx `14`, dy `-10`; toad is dx `14`, dy `-7`; the toad timer still misses by x distance. The fox is still close enough at dx `3`, dy `2`.

**Beat 3**

I take one more west step.

`Point3D(1228,1389,0)` is dry jungle `0xAC`. There is `0xD69` leaves on the tile, but tiledata marks it as foliage only, not impassable, surface, or bridge. The step passes, and no saved mobile or item occupies the target.

The visible world is still not empty. Panda is dx `15`, dy `-10`; toad is dx `15`, dy `-7`, still timer-negative by x distance. The monkey is farther off-screen at dx `21`, so it stays ambient uncertainty. Urulg is off-screen at dx `-2`, dy `28`, and the goblin archer is dx `22`, dy `30`; neither is solved. The fox is visible at `Point3D(1232,1391,0)`, but the final west step stretches it back to floored distance `4`.

Mechanical friction learned:

- Follow spacing uses `Mobile.GetDistanceToSqrt`, so dx `3`, dy `3` is outside the `2..3` heel band.
- Waiting a fox tick can correct the leash, but two player steps can stretch it again immediately.
- `0xD69` leaves are foliage, not a movement blocker.
- West keeps widening the toad timer gap; it does not remove the visible panda/toad problem or the carried southern hostiles.

Next pressure:

Mira ends at `Point3D(1228,1389,0)`, facing west. The fox is at `Point3D(1232,1391,0)`, outside the follow band. Panda and toad remain visible, with the toad timer-negative at dx `15`, dy `-7`. The next honest move is probably another fox-follow wait before any more route pushing.

## Run 146 - I Stop Making The Fox Sprint

I start at `Point3D(1228,1389,0)`, facing west. The fox is visible but stretched at `Point3D(1232,1391,0)`, dx `4`, dy `2`, floored distance `4`. The panda and toad are still on the right side of the screen. The toad is dx `15`, dy `-7`, which means its range-10 teleport timer still misses me by x distance. That is not safety. It is just geometry I have to keep.

**Beat 1**

I wait for the fox.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants distance `2..3`. From `Point3D(1232,1391,0)` toward me, `GetDirectionTo` returns `Direction.Up`, so the fox steps northwest to `Point3D(1231,1390,0)`.

The target is dry `map1.mul` jungle `0xAE` at z `0`, with zero statics and no saved mobile or item blocker. The fox ends at dx `3`, dy `1`, floored distance `3`. It is following again, not guarding.

**Beat 2**

I press west while I am already facing west.

`Mobile.Move` enters the real movement path. `MovementImpl.CheckMovement` accepts `Point3D(1227,1389,0)`: dry jungle `0xAF`, z `0`, zero statics, no saved mobile or item on the target, and no traced region blocker. Mira moves one tile west.

The screen does not clear. Panda is dx `16`, dy `-10`; toad is dx `16`, dy `-7`. The toad is still outside its range-10 timer scan by x distance. The fox is now dx `4`, dy `1`, floored distance `4`, stretched again.

**Beat 3**

I wait the fox back in instead of pretending the leash can be ignored.

The same follow path runs. From `Point3D(1231,1390,0)`, `GetDirectionTo` chooses `Direction.West`, and `DoMove` steps the fox to `Point3D(1230,1390,0)`. That tile is dry jungle `0xAD`, z `0`, with zero statics and no saved blocker. The fox ends at dx `3`, dy `1`, floored distance `3`.

The endpoint is still a nervous jungle edge. Panda and toad remain visible at dx `16`; the toad timer is still negative by x distance, not by proof that the private timer is idle. The monkey is farther off-screen at dx `22`. Urulg is off-screen at dx `-1`, dy `28`, and the goblin archer is dx `23`, dy `30`; both stay unresolved southern pressure. The `Ruins` marker is still only overlay knowledge, roughly 97 tiles southeast. No item, corpse, chest, water source, road, sign, gump, context menu, target cursor, combat, hunger, thirst, quest, discovery, ownership, follower-count, pet-order, or skill state changes.

Mechanical friction learned:

- Two exact fox follow ticks can keep the leash coherent across one player step, but neither tick creates protection.
- `Point3D(1227,1389,0)` is another dry west tile, not a shelter or route solution.
- The toad remains visible and timer-negative only because x distance is `16`.
- Moving west keeps widening the toad gap but also keeps the `Ruins` marker behind hostile southern pressure.

Next pressure:

Mira ends at `Point3D(1227,1389,0)`, facing west. The fox is at `Point3D(1230,1390,0)`, inside the `2..3` follow band. Panda and toad remain visible on the east edge, with the toad still outside its range-10 timer scan at dx `16`, dy `-7`. The next run needs a fresh scan before deciding whether to keep sidestepping west, turn north, or reassess the map marker.

## Run 147 - I Keep Sliding Out Of The Toad Box

I start at `Point3D(1227,1389,0)`, facing west. The fox is close at `Point3D(1230,1390,0)`, still following and inside the old heel band. The screen is not clean: the panda is still at `1243,1379`, and the toad is still at `1243,1382`. The monkey is off the east edge, not gone. Urulg and the goblin archer are south of the screen in the unresolved part of the run. The `Ruins` marker is still only a map overlay, about 98 tiles away, and the way back toward it points through the pressure I have been avoiding.

**Beat 1**

I press west because I am already facing west.

`Mobile.Move` enters the real movement path. The server-order `map1.mul` read says `Point3D(1226,1389,0)` is dry jungle `0xAF` with zero statics. The live snapshot has no saved mobile or item on that tile, and no named region catches the step. Mira moves one tile west.

The screen still has the same two visible wild bodies: panda dx `17`, dy `-10`, and toad dx `17`, dy `-7`. The toad is outside its range-10 teleport scan by x distance. The fox is now stretched to dx `4`, dy `1`, floored distance `4`.

**Beat 2**

I wait for the fox instead of letting the leash drift.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. The fox is outside the `2..3` band, and `GetDirectionTo` points west toward me. It steps from `Point3D(1230,1390,0)` to `Point3D(1229,1390,0)`.

That target is dry jungle `0xAC`, z `0`, with zero statics and no saved blocker. The fox returns to dx `3`, dy `1`, floored distance `3`. It is still following, not guarding.

**Beat 3**

I take one more west step while the east-edge animals are still only a geometry problem.

`MovementImpl.CheckMovement` accepts `Point3D(1225,1389,0)`. The land is dry jungle `0xAD`. There is a `0xD0D` mushrooms static on the tile, but tiledata names it `mushrooms`, height `1`, with no impassable, surface, bridge, foliage, or wet flags. No saved mobile or item occupies the target.

The final scan leaves the panda and toad barely visible at dx `18`. The toad is still outside its range-10 teleport scan by x distance, but I do not pretend the private timer is idle. The monkey is off-screen at dx `24`; Urulg is off-screen at dx `1`, dy `28`; the goblin archer is off-screen at dx `25`, dy `30`; and the old swamp drake branch remains carried unresolved. The fox is visible at `Point3D(1229,1390,0)`, but this last west step stretches it back to floored distance `4`.

Mechanical friction learned:

- `Point3D(1226,1389,0)` and `Point3D(1225,1389,0)` are passable west jungle tiles.
- `0xD0D` mushrooms look like clutter but do not block the west step.
- The panda and toad stay visible at the inclusive 18-tile edge, so they are not resolved or gone.
- One fox-follow tick fixes spacing for one beat; one more player step stretches it again.

Next pressure:

Mira ends at `Point3D(1225,1389,0)`, facing west. The fox is at `Point3D(1229,1390,0)`, outside the follow band at floored distance `4`. Panda and toad are still visible on the east edge at dx `18`; the toad remains timer-negative by x distance. The next honest move is a fresh scan/classifier pass, likely a fox-follow wait or one more cautious west step only after proving the tile.

## Run 148 - I Pull The Fox Off The Edge

I start at `Point3D(1225,1389,0)`, facing west. The fox is visible at `Point3D(1229,1390,0)`, too far again at floored distance `4`. The panda and toad are still barely on the east edge at `1243`, both dx `18`; that toad is still only timer-negative because the x gap keeps it outside the range-10 rectangle. The monkey is off-screen, and the southern orc/goblin problems are still carried, not solved.

**Beat 1**

I wait the fox in before I take another step.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants `2..3`. From `Point3D(1229,1390,0)`, `GetDirectionTo` points west toward me, so the fox steps to `Point3D(1228,1390,0)`.

That target is dry jungle `0xAF`, z `0`, with zero statics and no saved blocker. The fox ends at dx `3`, dy `1`, floored distance `3`. Still following, not guarding.

**Beat 2**

I press west while already facing west.

`Mobile.Move` enters the movement path. `MovementImpl.CheckMovement` accepts `Point3D(1224,1389,0)`: dry jungle `0xAE`, z `0`, zero statics, no saved mobile or item on the target, and no traced region blocker. Mira moves one tile west.

That finally pushes the panda and toad off the client rectangle: both are now dx `19`. I do not get to delete them from the world. The toad is recently visible timer-negative evidence, not gone. The fox is stretched back out to dx `4`, dy `1`, floored distance `4`.

**Beat 3**

I wait the fox back in again.

The same follow path runs. From `Point3D(1228,1390,0)`, `GetDirectionTo` again chooses west, and the fox steps to `Point3D(1227,1390,0)`. The land is dry jungle `0xAD`; the statics there are grasses, a background tree, and foliage leaves, with no impassable, surface, or bridge flags. No saved mobile or item blocks it.

The final screen has my fox and no saved visible items. Panda and toad are just beyond the east edge at dx `19`; the toad still misses its range-10 teleport scan by x distance. Monkey is farther east at dx `25`. Urulg is south at dx `2`, dy `28`, and the goblin archer is dx `26`, dy `30`; both remain unresolved route pressure. The old swamp drake branch stays far north and carried.

Mechanical friction learned:

- A follower wait is the right response when the fox slips to floored distance `4`; moving first just stretches the leash more.
- `Point3D(1224,1389,0)` is another passable west tile and it finally removes the panda/toad pair from legal client visibility.
- Off-screen edge animals are not dead, despawned, or safe. They are just no longer click-visible.
- `Point3D(1227,1390,0)` looks busier than it is: grasses, a background tree, and leaves do not block the fox.

Next pressure:

Mira ends at `Point3D(1224,1389,0)`, facing west. The fox is at `Point3D(1227,1390,0)`, back inside the `2..3` follow band. The immediate screen is cleaner, but the recently visible toad/panda, Urulg, GoblinArcher, and old swamp drake all remain unresolved risk. The next run needs a fresh scan before another west step or any turn back toward the `Ruins` marker.

## Run 149 - I Keep The Edge Behind Me

I start at `Point3D(1224,1389,0)`, facing west. For once the screen is almost clean: the only visible mobile I can honestly point at is my fox at `Point3D(1227,1390,0)`. The live snapshot rectangle has no saved wild mobiles, no saved items, and no running spawner object standing inside it. That does not make the jungle safe. The panda and toad are just past the east edge at dx `19`, the monkey is farther east, Urulg is still south as carried unresolved hostile pressure, and four invisible spawner home ranges still overlap the area as background risk.

The marker overlay is useful but not sovereign. `Ruins` is still southeast at about `99` tiles from the start, but turning back toward it walks me back toward the toad/orc/goblin pressure. West has the cleanest immediate geometry.

**Beat 1**

I press west while I am already facing west.

`Mobile.Move` enters `MovementImpl.CheckMovement`. The target `Point3D(1223,1389,0)` is dry jungle `0xAC` at z `0`. It has a background tree `0xD6E` and foliage leaves `0xD74`, but neither static has impassable, surface, or bridge flags. No saved mobile or item stands on the target tile, and no named region trigger catches the step. Mira moves west.

The endpoint scan is still empty of saved visible wild mobiles and visible items. The recently visible toad is now dx `20`, dy `-7`, outside its range-10 teleport rectangle by x distance. The fox stretches to dx `4`, dy `1`, floored distance `4`.

**Beat 2**

I wait the fox back in before I press west again.

The controlled pet path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the `2..3` band. From `Point3D(1227,1390,0)`, `GetDirectionTo` chooses west, and the fox steps to `Point3D(1226,1390,0)`.

That tile is dry jungle `0xAF`, z `0`, with zero statics and no saved blocker. The fox lands at dx `3`, dy `1`, floored distance `3`. It is following, not guarding.

**Beat 3**

I take one more west step while the east edge keeps getting farther away.

`MovementImpl.CheckMovement` accepts `Point3D(1222,1389,0)`. The land is dry jungle `0xAC`; the only static on the tile is foliage leaves `0xD4B`, which do not block. No saved mobile or item occupies the target, and no region, teleporter, gump, context menu, target cursor, hunger/thirst, combat, quest, discovery, ownership, follower-count, pet-order, or skill state changes.

The final screen still contains no saved visible wild mobiles or visible items. The toad and panda are dx `21` east now, the monkey is dx `27`, Urulg is dx `4`, dy `28` south, and the goblin archer is dx `28`, dy `30`. The four invisible spawner home ranges are still only area risk. The fox is visible at `Point3D(1226,1390,0)`, but my last step stretches it back to floored distance `4`.

Mechanical friction learned:

- West from `1224,1389` through `1223,1389` and `1222,1389` is passable jungle, despite background tree/leaf clutter.
- The live-state JSONL has nested `map.name` and `location` fields; the corrected scan still finds zero saved visible wild mobiles/items in the current rectangle.
- Moving west keeps the recently visible toad outside both the 18-tile client rectangle and its range-10 teleport scan by x distance.
- One fox-follow tick buys exactly one more step. After the final west move, the fox is outside the heel band again and needs attention before more route pushing.

Next pressure:

Mira ends at `Point3D(1222,1389,0)`, facing west. The fox is at `Point3D(1226,1390,0)`, visible but stretched to floored distance `4`. The immediate screen has no saved wild mobiles or items, but the recently visible toad/panda/monkey, carried Urulg, route-adjacent GoblinArcher, old swamp drake, and overlapping invisible spawner home ranges remain unresolved. The next honest move is likely another fox-follow wait before any west/north decision.

## Run 150 - I Make The Fox Catch Up Before I Move

I start at `Point3D(1222,1389,0)`, facing west. The screen is still empty of saved wild mobiles and saved items. The only body I can honestly react to is my fox at `Point3D(1226,1390,0)`, and it is stretched just outside the `2..3` heel band at floored distance `4`. The panda and toad are off the east edge, the monkey is farther east, and the southern orc/goblin pressure is still carried risk rather than proof of safety.

**Beat 1**

I wait the fox in.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants `2..3`. From `Point3D(1226,1390,0)`, `GetDirectionTo` points west toward me, and the fox steps to `Point3D(1225,1390,0)`.

That target is dry jungle `0xAF`, z `0`, with zero statics and no saved blocker. The fox lands at dx `3`, dy `1`, floored distance `3`. Still following, not guarding.

**Beat 2**

I press west while already facing west.

`Mobile.Move` enters `MovementImpl.CheckMovement`. The target `Point3D(1221,1389,0)` is dry jungle `0xAE`, z `0`. The only static on the tile is foliage leaves `0xD4B`, height `2`, with no impassable, surface, or bridge flags. No saved mobile or item stands there, and no named region or visible UI catches the step. Mira moves west.

The shifted live rectangle is x `1203..1239`, y `1371..1407`. It still has zero saved visible wild mobiles, zero saved visible items, and zero running spawner object locations. The toad and panda are now dx `22` east; the toad is still outside its range-10 teleport scan by x distance. The fox is stretched back out to floored distance `4`.

**Beat 3**

I wait the fox back in again instead of dragging it farther.

The same follow path runs. From `Point3D(1225,1390,0)`, the fox steps west to `Point3D(1224,1390,0)`. The tile is dry jungle `0xAD`, z `0`, with zero statics and no saved blocker.

The final screen is still mechanically quiet: no saved wild mobiles, no saved items, no running spawner object standing inside the client rectangle. That is not the same as safe. Panda and toad are recently visible off-screen at dx `22`, monkey is dx `28`, Urulg is dx `5`, dy `28`, and the goblin archer is dx `29`, dy `30`. The fox is back inside the follow band at dx `3`, dy `1`.

Mechanical friction learned:

- The correct first move was not route-driving; it was letting the fox catch up.
- `Point3D(1221,1389,0)` is passable west jungle despite `0xD4B` foliage leaves.
- `Point3D(1225,1390,0)` and `Point3D(1224,1390,0)` are clean fox-follow tiles.
- Moving west widens the toad timer geometry, but it does not erase any off-screen creature.

Next pressure:

Mira ends at `Point3D(1221,1389,0)`, facing west. The fox is at `Point3D(1224,1390,0)`, inside the `2..3` heel band. The immediate screen has no saved wild mobiles or items, but the next run still needs a fresh scan/classifier before choosing west, north, or any turn back toward the `Ruins` marker.

## Run 151 - The Rock Is For The Fox Too

I start at `Point3D(1221,1389,0)`, facing west. The screen is still quiet in the strict client sense: no saved wild mobile, no saved item, no running spawner object standing where I can click it. The only body I can react to is my fox at `Point3D(1224,1390,0)`, close enough at dx `3`, dy `1`. The quiet does not erase the off-screen ledger. Panda and toad are east at dx `22`, the monkey is farther east, Urulg and the goblin archer are south, and the old swamp drake timer branch is still carried instead of solved.

**Beat 1**

I press west because I am already facing west.

`Mobile.Move` enters `MovementImpl.CheckMovement`. The target `Point3D(1220,1389,0)` is dry jungle `0xAF`, z `0`, with zero statics. No saved mobile or item occupies it, and no traced region or visible UI catches the step. Mira moves west.

The shifted screen is still empty of saved wild mobiles and items. The toad is now dx `23`, dy `-7`, still outside its range-10 teleport scan by x distance. The fox stretches to floored distance `4`, so I do not keep walking.

**Beat 2**

I wait the fox back in.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` wants `2..3`, and from `Point3D(1224,1390,0)` the direct answer is west. The fox steps to `Point3D(1223,1390,0)`.

That target is dry jungle `0xAE`, z `0`, with zero statics and no saved blocker. The fox is back at dx `3`, dy `1`, floored distance `3`. It is still only following me; it is not guarding, fighting, or body-blocking.

I stop there. The next west step for me toward `Point3D(1219,1389,0)` looks locally passable, but it would stretch the fox again. The fox's next direct catch-up target after that would be `Point3D(1222,1390,0)`, the old `0x1778` rock tile. I can see the problem now: even when the player's line is open, the follower's line can hit the same rock from a different angle. That is not a routine movement beat; that is a route decision.

Mechanical friction learned:

- `Point3D(1220,1389,0)` is passable west jungle with zero statics.
- `Point3D(1223,1390,0)` is a clean fox-follow tile.
- The empty live rectangle still does not prove safety; it only proves nothing saved is visible right now.
- Continuing west would turn the known `1222,1390` rock into a fox pathing problem.

Next pressure:

Mira ends at `Point3D(1220,1389,0)`, facing west. The fox is at `Point3D(1223,1390,0)`, inside the `2..3` heel band. No gump, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, pet order, ownership, or follower count changed. The next honest move is a fresh scan and a route choice around the rock before any more west pushing.

## Run 152 - I Step Above The Fox Rock

I start at `Point3D(1220,1389,0)`, facing west, with the fox at `Point3D(1223,1390,0)`. The strict live rectangle is still quiet: no saved wild mobile, no saved visible item, and no running spawner object standing where I can click it. The quiet is not permission to sprint. Panda and toad are east of the screen at dx `23`, the monkey is farther out at dx `29`, Urulg is south at dx `6`, dy `28`, and the goblin archer is southeast at dx `30`, dy `30`. The real visible problem is the path shape: if I push straight west and then wait for the fox, its direct follow line wants the rock at `Point3D(1222,1390,0)`.

**Beat 1**

I press north once.

`Mobile.Move` does not enter `CheckMovement` because I was facing west. It acknowledges the movement packet, keeps me on `Point3D(1220,1389,0)`, and changes my facing to `Direction.North`. That facing turn still matters, because the server runs the movement-notification path around my current tile. The current rectangle has no saved wild body close enough to make that turn explode into combat.

**Beat 2**

I press north again.

Now the facing matches, so `Mobile.Move -> MovementImpl.CheckMovement` tests the real cardinal step. `Map.Sosaria` resolves to file index `1`, and the server-order block read puts `Point3D(1220,1388,0)` on dry jungle `0xAD` with zero statics. The live saved mobile/item filter for the shifted rectangle x `1202..1238`, y `1370..1406` is still empty except for my simulated follower overlay. No named Sosaria region rectangle catches the tile. I step north.

The fox does not move on my keypress. From the new tile it is still close enough: dx `3`, dy `2`, floored distance `3`, inside the `2..3` heel band. That is the point of this sidestep. I moved above the rock fork without making the pet solve it.

**Beat 3**

I press west once.

This is another facing turn only. Mira stays at `Point3D(1220,1388,0)` and faces `Direction.West`. The fox stays at `Point3D(1223,1390,0)`, still inside the heel band. No gump, context menu, target cursor, region message, combat, damage, hunger/thirst change, quest flag, discovery flag, pet order, ownership, follower count, or skill change appears.

Mechanical friction learned:

- A facing-only input still has player-facing consequences, but it does not test terrain or move the body.
- The one-tile north sidestep is real: `Point3D(1220,1388,0)` is dry passable `map1.mul` jungle with zero statics and no saved live blocker.
- The direct rock at `1222,1390` remains blocked by `0x1778`; I avoided it instead of resolving a pet auto-turn or `PathFollower` branch.
- The next west step is cleaner than the old one: from this north row, the fox's later direct catch-up points at `Point3D(1222,1389,0)`, which has only foliage leaves, not the rock. That is future evidence, not a movement I have already taken.

Next pressure:

Mira ends at `Point3D(1220,1388,0)`, facing west. The fox is at `Point3D(1223,1390,0)`, inside the follow band at floored distance `3`. The immediate screen still has zero saved visible wild mobiles/items, but the off-screen panda, toad, monkey, Urulg, goblin archer, old drake branch, and four overlapping invisible spawner home ranges remain unresolved. The next honest move is a fresh scan before stepping west or letting the fox move.

## Run 153 - I Use The New Row

I start at `Point3D(1220,1388,0)`, facing west, with the fox at `Point3D(1223,1390,0)`. The live rectangle is still empty of saved wild mobiles, saved items, and running spawner objects I can click. That is only screen truth. The marker overlay says `Ruins` is southeast, but that direction points back toward the recent toad/orc/goblin mess, so I keep peeling west along the row I earned.

**Beat 1**

I press west while already facing west.

`Mobile.Move` reaches `MovementImpl.CheckMovement`. The target `Point3D(1219,1388,0)` is dry jungle `0xAC`; it has `0xC94` bulrushes, but tiledata gives them only `Background` and height `0`, so they do not block. No saved mobile or item stands there, no running spawner object is inside the client rectangle, and no named region catches the step. Mira moves west.

The screen remains empty except for my fox. The fox is now stretched to dx `4`, dy `2`, floored distance `4`, so I do not drag it farther.

**Beat 2**

I wait for the fox.

The controlled path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the `2..3` band. From `Point3D(1223,1390,0)` toward me at `Point3D(1219,1388,0)`, `GetDirectionTo` chooses northwest, not west, and that matters: the fox targets `Point3D(1222,1389,0)`, not the old rock at `1222,1390`.

`Point3D(1222,1389,0)` is dry jungle `0xAC` with `0xD4B` foliage leaves. The leaves are not impassable, surface, or bridge. The fox steps there and lands at dx `3`, dy `1`, floored distance `3`. Still following, not guarding.

**Beat 3**

I take one more west step.

`MovementImpl.CheckMovement` accepts `Point3D(1218,1388,0)`. The land is dry jungle `0xAD`, z `0`, with zero statics. The shifted rectangle x `1200..1236`, y `1370..1406` still has zero saved visible wild mobiles, zero saved visible items, and zero running spawner object locations. Panda and toad are east at dx `25`; the toad remains outside its range-10 teleport scan by x distance. Monkey is dx `31`, Urulg is dx `8`, dy `29`, and the goblin archer is dx `32`, dy `31`. The old swamp drake branch is still carried.

The final step stretches the fox again to dx `4`, dy `1`, floored distance `4`. That is the next problem, not proof of danger.

Mechanical friction learned:

- `Point3D(1219,1388,0)` is passable despite the bulrush graphic.
- The north row changes the fox's direct follow target from blocked `1222,1390` to passable `1222,1389`.
- `Point3D(1218,1388,0)` is another clean west jungle tile.
- Empty live rectangles are not shelter. They only mean no saved entity is currently inside click range.

Next pressure:

Mira ends at `Point3D(1218,1388,0)`, facing west. The fox is at `Point3D(1222,1389,0)`, visible but outside the follow band at floored distance `4`. The next honest move is another fresh scan/classifier pass, probably a fox-follow wait before any more west movement.

## Run 154 - I Keep The Fox On The New Row

I start at `Point3D(1218,1388,0)`, facing west, with the fox at `Point3D(1222,1389,0)`. The screen is still strict-empty: no saved wild mobile, no visible saved item, no running spawner object standing inside x `1200..1236`, y `1370..1406`. The world map overlay keeps dangling `Ruins` southeast of me, but that is not a body, chest, road, sign, or safe route. It is only a marker, and the fox is the thing on my screen.

**Beat 1**

I wait the fox in.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the `2..3` band. From `Point3D(1222,1389,0)` toward me at `Point3D(1218,1388,0)`, `GetDirectionTo` chooses `Direction.West`, not the old diagonal. The fox targets `Point3D(1221,1389,0)`.

That tile is dry jungle `0xAE`, z `0`. It has static `0xD4B` leaves, but those are foliage, not a surface, bridge, or impassable blocker. The fox steps west and lands at dx `3`, dy `1`, floored distance `3`. Still following, not guarding.

**Beat 2**

I press west while already facing west.

`Mobile.Move` reaches `MovementImpl.CheckMovement`. The target `Point3D(1217,1388,0)` is dry jungle `0xAC`, z `0`, with zero statics. The shifted live rectangle x `1199..1235`, y `1370..1406` still contains zero saved wild mobiles, zero saved visible items, and zero running spawner object locations. Mira moves west.

The fox is stretched again to dx `4`, dy `1`, floored distance `4`, so I do not keep walking.

**Beat 3**

I wait the fox back in.

The same Follow path runs. From `Point3D(1221,1389,0)` toward `Point3D(1217,1388,0)`, `GetDirectionTo` again chooses `Direction.West`. `Point3D(1220,1389,0)` is dry jungle `0xAF`, z `0`, with zero statics and no saved blocker. The fox steps there and ends at dx `3`, dy `1`, floored distance `3`.

Nothing else opens or bites. The final screen is still empty of saved wild mobiles/items. Panda and toad are farther east at dx `26`, the monkey is dx `32`, Urulg is off-screen at dx `9`, dy `29`, and the goblin archer is dx `33`, dy `31`. The four invisible `PremiumSpawner` home ranges are still source pressure, not visible creatures.

Mechanical friction learned:

- `Point3D(1221,1389,0)` is a legal fox step despite foliage leaves.
- `Point3D(1217,1388,0)` is another open west-row player tile.
- `Point3D(1220,1389,0)` remains a clean fox catch-up tile.
- The west row is only progress because I keep giving the follower timer room to run; dragging the fox is the recurring cost.

Next pressure:

Mira ends at `Point3D(1217,1388,0)`, facing west. The fox is at `Point3D(1220,1389,0)`, inside the `2..3` heel band. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, or skill state changed. The next honest move is a fresh scan before another west step or a turn toward the map markers.

## Run 155 - I Keep Peeling West

I start at `Point3D(1217,1388,0)`, facing west, with the fox at `Point3D(1220,1389,0)`. The strict client rectangle is still empty of saved wild mobiles, visible saved items, and running spawner objects I can click. That leaves the fox and the shape of the jungle as the only screen problem. The map marker for `Ruins` still pulls southeast, but southeast is the direction of the recently labeled toad, panda, monkey, Urulg, and the goblin archer, so I keep easing west instead of turning back into them.

**Beat 1**

I press west while already facing west.

`Mobile.Move` enters `MovementImpl.CheckMovement`. The target `Point3D(1216,1388,0)` is dry map1 jungle `0xAF`. It has static `0xD90` leaves at z `0`, but the tiledata probe gives those leaves no low `Impassable`, `Surface`, or `Bridge` flags, so the land path remains usable. No saved mobile or item occupies the tile, no running spawner object stands in the rectangle, and no named Sosaria region catches the destination. Mira moves west.

The shifted rectangle is still empty of saved wild bodies and visible items. The fox is now stretched to dx `4`, dy `1`, floored distance `4`, so I do not keep dragging it.

**Beat 2**

I wait for the fox.

The same controlled path runs: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the `2..3` band. From `Point3D(1220,1389,0)` toward me, `GetDirectionTo` chooses `Direction.West`, and the fox steps to `Point3D(1219,1389,0)`.

That tile is dry jungle `0xAD`, z `0`, with zero statics and no saved blocker. The fox ends at dx `3`, dy `1`, floored distance `3`. It is still following, not guarding.

**Beat 3**

I take one more west step.

`MovementImpl.CheckMovement` accepts `Point3D(1215,1388,0)`. The land is dry jungle `0xAE`, z `0`, with zero statics, no saved mobile or item blocker, and no named region. Mira moves west.

The final screen is quiet in the narrow client sense: zero saved wild mobiles, zero saved items, zero running spawner object locations. It is not shelter. Panda and toad are farther east at dx `28`, monkey is dx `34`, Urulg is south-southeast at dx `11`, dy `29`, and the goblin archer is dx `35`, dy `31`. The toad remains outside its range-10 timer by x distance. The four invisible `PremiumSpawner` home ranges still overlap by source range.

Mechanical friction learned:

- `Point3D(1216,1388,0)` is passable despite `0xD90` leaves.
- `Point3D(1219,1389,0)` is a clean fox-follow tile.
- `Point3D(1215,1388,0)` is another open west-row player tile.
- The fox ends outside the heel band again; the likely next direct catch-up tile `Point3D(1218,1389,0)` has only nonblocking `0xCE9` sapling static evidence, but that movement has not run.

Next pressure:

Mira ends at `Point3D(1215,1388,0)`, facing west. The fox is at `Point3D(1219,1389,0)`, visible but outside the `2..3` heel band at floored distance `4`. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, skill, or PvP/PvE state changed. The next honest move is a fresh scan and probably a fox-follow wait before any more west pushing.

## Run 156 - The Jungle Says No

I start at `Point3D(1215,1388,0)`, facing west. The fox is one step too far out at `Point3D(1219,1389,0)`, dx `4`, dy `1`, floored distance `4`. My screen has no saved wild mobiles, no visible saved items, and no running spawner object locations inside the strict x `1197..1233`, y `1370..1406` client rectangle. The quiet is only screen quiet: panda and toad are still off the east edge at dx `28`, the monkey is farther east at dx `34`, Urulg is south at dx `11`, dy `29`, and the goblin archer is farther southeast. I deal with the fox before trying to push through trees.

**Beat 1**

I wait for the fox.

The controlled follower path runs: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the `2..3` band. From `Point3D(1219,1389,0)` toward me at `Point3D(1215,1388,0)`, `GetDirectionTo` chooses `Direction.West`.

The fox targets `Point3D(1218,1389,0)`. Server-order map1 reads dry jungle `0xAC`, z `0`. The only static is `0xCE9` sapling, flagged `Background` only, so it is scenery, not a movement stop. The fox steps west and lands at dx `3`, dy `1`, floored distance `3`. It is still following, not guarding, not blocking for me, and not fighting.

Before I press west again, I look at the next tile. `Point3D(1214,1388,0)` is dry jungle `0xAC`, but it has static `0xD70` tree with `TileFlag.Impassable` and `0xD76` leaves. That is not a subtle route hint. It is the wall in front of my nose, so I do not send the movement input.

Mechanical friction learned:

- `Point3D(1218,1389,0)` is a legal fox catch-up tile despite the sapling.
- The fox is back inside the `2..3` heel band.
- Straight west from `Point3D(1215,1388,0)` is blocked by impassable tree static `0xD70` at `Point3D(1214,1388,0)`.
- Empty live rectangles are still not shelter; they only say no saved entity is standing in click range right now.

Next pressure:

Mira ends where she started, `Point3D(1215,1388,0)`, facing west. The fox is at `Point3D(1218,1389,0)`, inside the follow band. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, skill, or PvP/PvE state changed. The next honest move is a fresh scan and a route choice around the west tree, not a blind west shove.

## Run 157 - I Step Around the Tree

I start at `Point3D(1215,1388,0)`, facing west, with the fox inside the heel band at `Point3D(1218,1389,0)`. The client rectangle is still strict-empty: no saved wild mobiles, no visible saved items, and no running spawner body I can click. That does not make the place safe. The recently labeled panda and toad are east off-screen, the monkey is farther east, Urulg and the goblin archer remain southern carried pressure, and the direct west tile is still a real tree.

**Travel Segment**

I do the boring thing a player does when a tree says no: I go around it. From west-facing, the first north key only turns me. The second north key moves me to `Point3D(1215,1387,0)`. Then I turn west and take two west steps through `Point3D(1214,1387,0)` to `Point3D(1213,1387,0)`.

The route is all dry map1 jungle. `1215,1387` has a background tree and foliage leaves but no `Impassable`, `Surface`, or `Bridge` blocker. `1214,1387` and `1213,1387` have no statics. The skipped tile `1214,1388` remains the blocker: dry jungle underneath, but the `0xD70` tree is `Background|Impassable`.

I let the fox shadow the segment instead of dragging it into a pathing problem. This is not an exact pet timer proof. It is the travel policy: the fox is already ordered to Follow, the checked local route is clear, and no combat or ownership state changes. The fox's approximate catch-up line moves from `1218,1389` to `1217,1388`, then west to `1216,1388`. The diagonal side tiles for the first fox step are clear, and the second target has only `0xD90` foliage leaves. The fox ends at dx `3`, dy `1`, inside the `2..3` band.

The endpoint scan is still quiet in the visible sense: zero saved wild mobiles, zero visible saved items, and zero running spawner locations inside x `1195..1231`, y `1369..1405`. The quiet picked up two extra overlapping invisible spawner home ranges from the west, so the area-risk list grows to six. The nearest world-map marker is still `Ruins` southeast, about 109 tiles away, which is information on the map overlay, not a safe road.

Mechanical friction learned:

- A direct west shove from Run 156 would have hit `0xD70`; the north row around it is passable.
- `Mobile.Move` still burns facing-only inputs before real movement when the direction mask changes.
- Approximate Travel Segment follower shadowing is useful, but it is not guarding, fighting, body-blocking, or an exact `BaseAI.Obey` timer tick.
- The empty endpoint rectangle is not shelter. It only means no saved wild body or item is standing in the current 18-tile click/update box.

Next pressure:

Mira ends at `Point3D(1213,1387,0)`, facing west. The fox is approximately at `Point3D(1216,1388,0)`, inside the follow band. The next west tile `1212,1387` is prechecked as dry jungle with no statics, but it has not been walked. A fresh scan/classifier should come before more west pushing or any turn back toward the `Ruins` marker.

## Run 158 - I Do Not Drag The Fox Into The Tree

I start at `Point3D(1213,1387,0)`, facing west, with the fox one tile southeast at `Point3D(1216,1388,0)`. The screen is still empty in the strict saved-entity sense: no wild mobile, no visible item, no running spawner body I can click. Six invisible `PremiumSpawner` home ranges still overlap the rectangle, and the old east animals plus southern hostiles are still unresolved pressure. The only thing I can actually manage is the next west step and the fox's leash.

**Beat 1**

I press west while already facing west.

`Mobile.Move` enters `MovementImpl.CheckMovement`. `Map.Sosaria` is file index `1`, and the server-order read puts `Point3D(1212,1387,0)` on dry jungle `0xAF`, z `0`, with zero statics. The shifted live rectangle x `1194..1230`, y `1369..1405` still has zero saved wild mobiles and zero visible saved items. Mira moves west.

That one step stretches the fox to dx `4`, dy `1`, floored distance `4`. I do not keep walking, because the fox has become the visible problem again.

**Beat 2**

I wait the fox in.

The path is `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still wants the `2..3` band. From `Point3D(1216,1388,0)` toward me at `Point3D(1212,1387,0)`, `GetDirectionTo` chooses `Direction.West`, not a diagonal, and the target is `Point3D(1215,1388,0)`.

That target is dry jungle `0xAE`, z `0`, with zero statics and no saved blocker. The fox steps west and lands at dx `3`, dy `1`, floored distance `3`. Still following, not guarding, not fighting, not blocking for me.

I stop there. If I take the next west step to `1211,1387`, the fox's next direct catch-up from `1215,1388` points straight at `1214,1388`, the impassable `0xD70` tree I just routed around. That is a route decision, not a routine third beat.

Mechanical friction learned:

- `Point3D(1212,1387,0)` is a real west step: dry passable map1 jungle, zero statics, no saved live blocker.
- `Point3D(1215,1388,0)` is a clean exact fox-follow tile.
- The tree at `Point3D(1214,1388,0)` is still mechanically relevant because the follower's line, not just my line, can run into it.
- An empty 18-tile rectangle is not shelter; it only means no saved visible body or item is currently inside click range.

Next pressure:

Mira ends at `Point3D(1212,1387,0)`, facing west. The fox is at `Point3D(1215,1388,0)`, inside the `2..3` heel band. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, skill, or PvP/PvE state changed. The next honest move is a fresh scan and a route choice that accounts for the fox's blocked direct catch-up tile at `1214,1388`.

## Run 159 - I Move Above The Fox Trap

I start at `Point3D(1212,1387,0)`, facing west, with the fox at `Point3D(1215,1388,0)`. The first scan is still empty in the saved-entity sense: zero wild mobiles, zero visible saved items, and zero running spawner bodies inside x `1194..1230`, y `1369..1405`. That is not safety. It just means the only thing on my screen is the fox, and the fox's next direct west catch-up would still ram the impassable tree at `1214,1388`.

**Travel Segment**

I stop trying to push straight west and take the north row.

The first north input is only a facing turn. The second north input moves me to `Point3D(1212,1386,0)`. Then I turn west and walk through `Point3D(1211,1386,0)`, `Point3D(1210,1386,0)`, and `Point3D(1209,1386,0)`.

The route is dry map1 jungle. `1212,1386` is `0xAE` with zero statics. `1211,1386` is `0xAD` with zero statics. `1210,1386` has only `0xD0C` mushrooms flagged `Background`, and `1209,1386` has zero statics. No saved mobile or visible item occupies the route, and no running spawner object stands in the segment rectangle.

I let the fox shadow instead of forcing an exact AI tick. From `1215,1388`, the first natural diagonal catch-up target is `1214,1387`. One side of that diagonal is the blocked `1214,1388` tree, but the other side, `1215,1387`, is only background tree and foliage; non-player diagonal movement only needs one side clear. The fox can then slide west across `1213,1387` to `1212,1387`. It ends dx `3`, dy `1`, inside the heel band. That is approximate Travel Segment follower shadowing, not Guard, Attack, body-blocking, or exact timer proof.

The endpoint scan at `Point3D(1209,1386,0)` is still visually quiet: zero saved visible wild mobiles, zero visible saved items, and zero running spawner object locations inside x `1191..1227`, y `1368..1404`. The overlapping invisible spawner home ranges stay at six. The old panda, toad, and monkey are farther east; Urulg and the goblin archer are still south/southeast carried risk; the old swamp drake branch is still unresolved and off-screen. The `Ruins` marker is now about 113 tiles southeast, while the Mines of Morinia marker is about 187 tiles west. Markers are navigation pressure, not bodies or shelter.

Mechanical friction learned:

- The direct west tile was not the real problem; the follower's next direct target was. Routing north first changes the fox's catch-up line away from the impassable `0xD70` tree.
- A normal player facing change still consumes an input before movement; the segment had two facing turns and four accepted movement inputs.
- `MovementImpl.CheckMovement` applies stricter diagonal side checks to players than to non-player mobiles. The fox's diagonal around the tree is plausible because one side tile is clear; I still record it as Travel Segment shadowing, not an exact `BaseAI.Obey` result.
- The endpoint has fewer eastern marker temptations and fewer visible risks, but empty grass is not shelter. The six invisible `PremiumSpawner` home ranges still overlap by source range.

Next pressure:

Mira ends at `Point3D(1209,1386,0)`, facing west. The fox is approximately at `Point3D(1212,1387,0)`, inside the `2..3` heel band. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, skill, or PvP/PvE state changed. The next honest move is a fresh scan before deciding whether to keep peeling west toward the Mines marker or turn back toward the Ruins marker.

## Run 160 - I Spend The Whole West Row

I start at `Point3D(1209,1386,0)`, facing west. The fox is at `Point3D(1212,1387,0)`, close enough to follow but still the only living thing on my screen. The fresh rectangle is empty in the saved-entity sense: no wild mobile, no visible item, no running spawner body I can click. That is not safety. It is just enough room to keep walking away from the old southeast pressure.

**Travel Segment**

I press west and keep pressing west until the travel cap says stop.

`Mobile.Move` stays in the normal cardinal path because I am already facing west. The server-order route probe reads twelve target tiles, `1208,1386` through `1197,1386`, as dry map1 jungle. Only the first tile has scenery, `0xD12` mushrooms, and tiledata gives it no blocking flags. No saved mobile, visible item, running spawner object, gump, target cursor, or region text appears along the swept screen.

I let the fox shadow the line instead of pretending I own its private timer. It slides from `1212,1387` to `1200,1387` in the mental travel segment. That row is dry jungle too; the only statics I find are `0xCA3`, `0xCA2`, and `0xCA0`, all nonblocking scenery. The fox ends at dx `3`, dy `1`, floored distance `3`, still following and not guarding.

The endpoint rectangle x `1179..1215`, y `1368..1404` is empty of saved wild bodies, items, and running spawner objects. The good part is that the east spawner pair finally falls out of the overlap list, leaving four invisible home ranges instead of six. The bad part is that invisible home ranges are still not shelter. The old panda, toad, monkey, Urulg, goblin archer, and swamp drake branches are just farther off-screen, not solved.

Mechanical friction learned:

- `Point3D(1208,1386,0)` is passable despite the mushroom graphic.
- `Point3D(1207,1386,0)` through `Point3D(1197,1386,0)` are dry passable jungle with zero statics.
- The fox can be conservatively shadowed west along y `1387` without crossing a known blocker, but that is not an exact `BaseAI.Obey` tick.
- The local spawner-home overlap count drops from six to four when I get to `1197,1386`.

Next pressure:

Mira ends at `Point3D(1197,1386,0)`, facing west. The fox is approximately at `Point3D(1200,1387,0)`, inside the `2..3` heel band. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, skill, or PvP/PvE state changed. The next honest move is another fresh scan because this segment spent the full twelve accepted movement inputs.

## Run 161 - A Crane Enters The Edge

I start at `Point3D(1197,1386,0)`, facing west. The fox is close at `Point3D(1200,1387,0)`, still only following. The first screen check is empty in the saved-entity sense: no wild body, no visible item, no running spawner object I can click. Four invisible `PremiumSpawner` home ranges still overlap the area, which means danger source, not an on-screen creature.

**Travel Segment**

I keep moving west, but only until the screen changes.

The first two west steps are quiet. `MovementImpl.CheckMovement` accepts `Point3D(1196,1386,0)` and `Point3D(1195,1386,0)`, both dry map1 jungle with zero statics. The fox can shadow west over `1199,1387` and `1198,1387`; `1199,1387` has only background mushrooms.

The third west step reaches `Point3D(1194,1386,0)`. The tile is dry jungle `0xAC`; the only static is `0xCA0` fern scenery with no movement-stopping flags I can use as a wall. But the shifted client rectangle now catches a saved crane at `Point3D(1176,1379,0)`, exactly on the west edge at dx `-18`, dy `-7`.

I stop there. A visible animal is not a road, shelter, or safety, but it is a new thing on the screen, and I do not keep macro-walking past it. The nearby saved toad at `1174,1379` is still just outside the client rectangle at dx `-20`, dy `-7`; its `GiantToad.TeleportTimer` range 10 still misses me by x distance. The fox shadows to `Point3D(1197,1387,0)`, inside the heel band at dx `3`, dy `1`, still not guarding.

Mechanical friction learned:

- A west push from `1197,1386` stops after three accepted inputs because a saved crane enters legal client visibility.
- `1196,1386`, `1195,1386`, and `1194,1386` are dry passable jungle; `1194,1386` has only nonblocking fern scenery.
- The fox's adjacent west shadow row to `1197,1387` is locally clear, but it remains approximate Travel Segment shadowing, not an exact pet timer.
- The west animal cluster is real saved state now, not just source-map possibility. The visible crane comes from the running animal spawner at `1165,1361`.

Next pressure:

Mira ends at `Point3D(1194,1386,0)`, facing west. The fox is approximately at `Point3D(1197,1387,0)`, following inside the `2..3` band. The crane at `Point3D(1176,1379,0)` is visible at the west edge; the toad at `1174,1379` is just outside visibility and outside teleport range for now. No gump, context menu, target cursor, combat, damage, item use, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, skill, or PvP/PvE state changed. The next honest move is to react to or inspect the visible crane before pushing farther west toward the Mines marker.

## Run 162 - I Click The Bird Before I Walk Past It

I start at `Point3D(1194,1386,0)`, facing west. The screen has one new thing I can actually point at: a saved crane on the west edge at `Point3D(1176,1379,0)`, dx `-18`, dy `-7`. The fox is still behind me at `Point3D(1197,1387,0)`, inside the follow band, and the toad at `1174,1379` is still just outside the client rectangle. I do not keep walking west blind.

**Beat 1**

I single-click the visible crane.

`PacketHandlers.LookReq` finds mobile serial `288081`, `Mobile.CanSee` passes, and `Utility.InUpdateRange` accepts the inclusive edge. `Region.OnSingleClick` returns true. `BaseCreature.OnSingleClick` has no tame or bonded line to add because the crane is uncontrolled, so `Mobile.OnSingleClick` only sends the overhead label: `a crane`.

That is all I earn. No context menu opens. No target cursor appears. I do not start taming, attack, move, or wait a creature timer. The crane is now a labeled animal on the edge, not safety. Its class is still `AI_Animal/FightMode.Aggressor`, one damage, no special timer. The nearby toad is still the thing that makes a west push feel bad: just outside the screen and still outside its range-10 teleport scan by x distance.

Mechanical friction learned:

- A crane at dx `-18`, dy `-7` is still inside the normal click/update rectangle.
- Single-clicking an uncontrolled crane gives only the overhead label `a crane`.
- Labeling the crane does not clear the route, despawn the toad, move the fox, or prove the spawner area safe.

Next pressure:

Mira remains at `Point3D(1194,1386,0)`, facing west. The fox remains at `Point3D(1197,1387,0)`, following but not guarding. The labeled crane remains visible at `Point3D(1176,1379,0)`. The saved toad at `Point3D(1174,1379,0)` is still outside visibility at dx `-20`, dy `-7` and outside `GiantToad.TeleportTimer` range 10 for now. The next honest action is a route decision or pause around the crane/toad edge, not routine west travel.

## Run 163 - One Step, Then The Leash

I start at `Point3D(1194,1386,0)`, facing west. The crane is not a mystery anymore: it is still on the west edge, labeled `a crane`, and it still has not done anything. The uglier thing is the toad two tiles farther west at `Point3D(1174,1379,0)`. It is not on my screen yet, but it is close enough that one more west step after this run would bring it onto the edge.

**Beat 1**

I press west once.

This is not a travel segment. I am creeping. `Mobile.Move` reaches `MovementImpl.CheckMovement` because I am already facing west. The target `Point3D(1193,1386,0)` is dry `map1.mul` jungle `0xAE`, z `0`. The only static there is `0xC93` blade plant, and tiledata gives it `Background|ArticleA`, not `Impassable`, `Surface`, or `Bridge`. No saved mobile or visible item stands on the target tile, and no region gate catches the step. Mira moves west.

The shifted screen is almost the same, which is why it still feels bad. The crane is still visible at dx `-17`, dy `-7`. The toad is still just outside at dx `-19`, dy `-7`, and still outside its range-10 teleport scan by x distance. The fox is now the immediate problem: it stayed at `Point3D(1197,1387,0)`, so it is dx `4`, dy `1`, floored distance `4`, outside the `2..3` follow band.

**Beat 2**

I wait for the fox.

The exact follower path runs this time: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo -> DoMove`. `FriendsAvoidHeels` still has the fox's spacing target at `2..3`. From `Point3D(1197,1387,0)` toward me at `Point3D(1193,1386,0)`, `GetDirectionTo` picks `Direction.West`.

The fox target is `Point3D(1196,1387,0)`, dry jungle `0xAE` with zero statics. The fox steps there and ends at dx `3`, dy `1`, floored distance `3`. It is following again, not guarding, not attacking, not body-blocking, and not making the crane or the toad safe.

I stop with the next west key unpressed. The precheck says `Point3D(1192,1386,0)` is probably walkable, even with a nonblocking tree and leaves, but moving there would put the toad into the visible rectangle at dx `-18`, dy `-7`. That is a new visible high-risk timer creature, not routine movement.

Mechanical friction learned:

- `Point3D(1193,1386,0)` is a real west step: dry passable Sosaria jungle with only a nonblocking blade plant.
- A single west step does not expose the toad yet; the live rectangle becomes x `1175..1211`, y `1368..1404`, still showing the crane and not the toad.
- The controlled fox stretches outside the heel band after the player step, so the next normal beat is the follower tick, not another route push.
- `Point3D(1196,1387,0)` is a clean exact fox-follow tile.
- The next west step would expose saved toad serial `737437` at the inclusive west edge, so I stop before making that choice.

Next pressure:

Mira ends at `Point3D(1193,1386,0)`, facing west. The fox is at `Point3D(1196,1387,0)`, inside the follow band. The crane remains visible at `Point3D(1176,1379,0)`, dx `-17`, dy `-7`. The toad remains just outside visibility at `Point3D(1174,1379,0)`, dx `-19`, dy `-7`, and outside `GiantToad.TeleportTimer` range 10 for now. The next honest move is a fresh route choice: press west and deal with the toad appearing, or avoid that edge.

## Run 164 - The Toad Comes Onto The Screen

I start at `Point3D(1193,1386,0)`, facing west. The crane is still visible and labeled at `Point3D(1176,1379,0)`. The fox is close at `Point3D(1196,1387,0)`, inside the follow band, but not guarding me. The saved toad at `Point3D(1174,1379,0)` is the thing I have been edging around: it is one tile outside the screen and still outside its range-10 teleport scan.

**Beat 1**

I press west once.

`Mobile.Move` takes the real movement path because I am already facing west. `MovementImpl.CheckMovement` accepts `Point3D(1192,1386,0)`: Sosaria uses `map1.mul`, the land tile is dry jungle `0xAF`, z `0`, and the statics on that tile are a fern `0xC9F`, tree `0xD72`, and leaves `0xD78` with no `Impassable`, `Surface`, `Bridge`, or `Wet` flags. No saved mobile or item stands on the destination tile, so Mira steps west.

The screen changes immediately. The client rectangle is now x `1174..1210`, y `1368..1404`. The toad is visible on the west edge at dx `-18`, dy `-7`, and the crane is still visible at dx `-16`, dy `-7`. There are still zero saved visible items and zero visible running spawner objects. The toad's current timer geometry is negative: its `TeleportTimer` uses the toad-centered range-10 scan, and I am outside that by x distance. Its `AI_Animal/FightMode.Aggressor` acquisition is also not currently selecting me because no aggressor/aggressed/faction/ethic state exists and the effective range-16 rectangle misses me by x distance.

That does not make it scenery. It is a saved timer creature from the same running animal spawner as the crane, and I just made it visible. I stop before waiting the fox or pressing west again.

Mechanical friction learned:

- `Point3D(1192,1386,0)` is walkable dry Sosaria jungle even though the tile has fern/tree/leaves statics.
- A normal west step from `1193,1386` exposes saved Toad serial `737437` at the inclusive client edge.
- The toad is visible timer pressure but not currently in its range-10 teleport scan from this exact geometry.
- The fox is now stretched to dx `4`, dy `1`, floored distance `4`, but the newly visible toad decision outranks a routine follow wait.

Next pressure:

Mira ends at `Point3D(1192,1386,0)`, facing west. The fox remains at `Point3D(1196,1387,0)`, following but outside the heel band. The crane remains visible at `Point3D(1176,1379,0)`, dx `-16`, dy `-7`; the toad is now visible at `Point3D(1174,1379,0)`, dx `-18`, dy `-7`, with current teleport/acquisition geometry negative but private timer and future wandering unresolved. The next honest action is to react to the visible toad or pull the fox in after a fresh pressure pass, not routine west travel.

## Run 165 - I Back Out Of The Toad Edge

I start at `Point3D(1192,1386,0)`, facing west. The toad is exactly on the west edge at `Point3D(1174,1379,0)`, dx `-18`, dy `-7`, and the crane is still visible at `Point3D(1176,1379,0)`. The fox is behind me at `Point3D(1196,1387,0)`, following but stretched to floored distance `4`. The toad's current range math is negative: its teleport timer checks range `10`, and its animal `Aggressor` acquisition is gated before scanning because there is no aggressor/aggressed/faction/ethic state. Negative is not safe; it only tells me the honest move is away from the west edge.

**Beat 1**

I press east.

Because I was facing west, this is only a turn. `Mobile.Move` does not enter `MovementImpl.CheckMovement`, does not move me, and does not move the fox. It still acknowledges movement, sets direction to `Direction.East`, and runs nearby movement notification over the same location. I do not treat that as a pet tick, a toad tick, or a route step. The screen is still the same: toad visible at dx `-18`, crane visible at dx `-16`, fox still dx `4`, dy `1`.

**Beat 2**

I press east again and actually step back to `Point3D(1193,1386,0)`.

This is the same tile I had just proven from the other direction in Run 163: dry Sosaria jungle, land `0xAE`, with only a nonblocking `0xC93` blade plant. No saved mobile or visible item is standing on it. `MovementImpl.CheckMovement` accepts the cardinal move, so Mira steps east.

The result is useful, but I stop there. The toad falls just outside the client rectangle at dx `-19`, dy `-7`; it is not gone, dead, despawned, pacified, or safe. The crane remains visible at dx `-17`, dy `-7`. The fox is now naturally back inside the heel band at dx `3`, dy `1`, without needing a pet AI tick. No gump, context menu, target cursor, combat, damage, hunger, thirst, skill, item, quest, discovery, pet order, ownership, follower-count, or PvP/PvE state changes.

Mechanical friction learned:

- Turning away from pressure is still a normal player beat. It changes facing and sends movement notification, but it does not run terrain collision or pet follow.
- Stepping east to `Point3D(1193,1386,0)` is legal and reduces the toad from visible edge pressure to just-off-screen timer uncertainty.
- Pulling back one tile also brings the fox into the remembered `2..3` follow band, but that is positioning, not Guard, Attack, or body-blocking.
- The crane is still visible. The toad is just outside visibility. The west animal spawner is still real saved state, not scenery.

Next pressure:

Mira ends at `Point3D(1193,1386,0)`, facing east. The fox is at `Point3D(1196,1387,0)`, inside the follow band. The labeled crane remains visible at `Point3D(1176,1379,0)`, dx `-17`, dy `-7`; Toad serial `737437` is recently visible just outside the west edge at dx `-19`, dy `-7`, still outside its range-10 teleport scan and outside effective range-16 acquisition by x distance. The next honest action is a fresh scan and route decision; do not push west again as routine travel.

## Run 166 - I Put The Bird Off Screen Too

I start at `Point3D(1193,1386,0)`, facing east. The toad is already just off the west edge at `Point3D(1174,1379,0)`, dx `-19`, dy `-7`; the crane is still visible at `Point3D(1176,1379,0)`, dx `-17`, dy `-7`; and the fox is close at `Point3D(1196,1387,0)`. The toad's current range math is still negative, but that is only geometry. It is not peace.

**Beat 1**

I press east once.

Because I am already facing east, `Mobile.Move` goes through `MovementImpl.CheckMovement`. The target `Point3D(1194,1386,0)` is the dry jungle tile I crossed westward earlier: map1 land `0xAC`, with only a fern `0xCA0` that has no movement-stopping flags. No saved mobile or item occupies the tile. Mira steps east.

The scan after the step still has the crane on the inclusive west edge at dx `-18`, dy `-7`. The toad is farther out at dx `-20`, dy `-7`, outside both my client rectangle and its range-10 teleport scan by x distance. The fox is dx `2`, dy `1`, still inside the heel band without a pet AI tick. I do not call that safe; I only call it one tile more distance.

**Beat 2**

I press east again.

`Mobile.Move -> MovementImpl.CheckMovement` accepts `Point3D(1195,1386,0)`, another dry Sosaria jungle tile with zero statics and no saved blocker. Mira moves east a second time.

Now the west animal cluster is no longer visible: the crane is dx `-19`, dy `-7`, and the toad is dx `-21`, dy `-7`. The endpoint rectangle has zero saved visible wild mobiles, zero saved visible items, and zero visible running spawner objects. The fox remains at `Point3D(1196,1387,0)`, dx `1`, dy `1`, still following and still not guarding, body-blocking, or fighting for me.

Mechanical friction learned:

- Retreating east over `Point3D(1194,1386,0)` and `Point3D(1195,1386,0)` is legal dry-jungle movement.
- The crane drops from visible edge pressure to recently visible off-screen uncertainty only after the second east step.
- The toad is farther outside the screen and still outside its current teleport/acquisition geometry, but no timer, despawn, taming, pacification, combat, or safety path resolved it.
- An empty rectangle here means only that the saved clickable bodies are off-screen. Four invisible running spawner home ranges still overlap by source range.

Next pressure:

Mira ends at `Point3D(1195,1386,0)`, facing east. The fox is at `Point3D(1196,1387,0)`, inside the follow band. Crane serial `288081` is recently visible just outside the west edge at dx `-19`, dy `-7`; Toad serial `737437` is farther off-screen at dx `-21`, dy `-7` and still outside the current range-10 teleport scan. Urulg, the old swamp drake branch, GoblinArcher route pressure, and the four invisible spawner home ranges remain unresolved risk evidence. The next honest action is a fresh route choice from a quieter screen, not a safety claim.

## Run 167 - I Spend The East Row Back Toward Known Jungle

I start at `Point3D(1195,1386,0)`, facing east. The screen is quiet in the narrow client sense: no saved wild body, no visible item, no open gump, no context menu, no target cursor. The only living thing I can actually see is my fox at `Point3D(1196,1387,0)`. Quiet is not the same as safe. The crane and toad are just off the west side, and the map overlay still says the Ruins are southeast while the Mines marker is much farther west through the animal edge.

**Travel Segment**

I press east and keep going until the segment cap stops me.

`Mobile.Move` stays in the cardinal movement path because I am already facing east. The route is not new terrain fantasy: it is the same row I just proved while backing west, now walked in reverse. The accepted targets are `1196,1386` through `1207,1386`, all dry passable Sosaria jungle. No saved mobile or item occupies the target row, no visible running spawner object appears, and no region text, gump, corpse, chest, sign, shelter, water source, or target cursor interrupts the walk.

I let the fox shadow the segment instead of pretending a private pet timer tick ran. It ends approximately at `Point3D(1204,1387,0)`, dx `-3`, dy `1`, still following in the heel band. That is useful leash management, not Guard, Attack, body-blocking, or protection.

The endpoint rectangle x `1189..1225`, y `1368..1404` is still empty of saved visible wild mobiles and items. The west crane is now dx `-31`, dy `-7`; the west toad is dx `-33`, dy `-7`. The old east animals are not visible either: the nearest known toad and panda are still around dx `36`, and the monkey is farther. What changed is the invisible pressure count: six running `PremiumSpawner` home ranges overlap this rectangle again. The ground is open, but the jungle is still not shelter.

Mechanical friction learned:

- A full twelve-input east Travel Segment from `1195,1386` to `1207,1386` is legal over previously proved dry map1 jungle.
- The live snapshot finds zero saved visible wild mobiles, zero visible world items, and zero visible running spawner objects at both the start and endpoint rectangles.
- Moving east buys distance from Crane serial `288081` and Toad serial `737437`, but it brings the eastern invisible spawner envelopes back into the area-risk summary.
- The fox shadow is only Travel Segment approximation. No `BaseAI.Obey`, pet order, owner list, follower count, combat, Guard, Attack, or body-blocking state changed.

Next pressure:

Mira ends at `Point3D(1207,1386,0)`, facing east. The fox is approximately at `Point3D(1204,1387,0)`, still following inside the heel band. No gump, context menu, target cursor, combat, damage, hunger/thirst, item, quest, discovery, pet-order, ownership, follower-count, skill, fame, karma, or PvP/PvE state changed. The next honest move is another fresh scan and route choice: east/southeast leans toward the Ruins marker and old eastern wildlife pressure, while west returns toward the just-avoided crane/toad edge.

## Run 168 - I Keep The Quiet Row, But I Do Not Call It Safe

I start at `Point3D(1207,1386,0)`, facing east. The screen is still empty in the client sense: no saved wild mobile, no visible item, no open gump, no context menu, no target cursor. The only body I can account for near me is my controlled fox, approximately at `Point3D(1204,1387,0)` from the last travel-shadow, and that is leash management, not protection.

The overlay marker is tugging me southeast toward `Ruins` at `1303,1449`, now about 115 tiles away. The `Mines of Morinia` marker is back west through the animal edge I just backed out of, so I do not turn around into the crane/toad pressure for no reason. I spend another straight east row first.

**Travel Segment**

I press east and keep going until the segment cap stops me.

`Mobile.Move` stays in the cardinal movement path because I am already facing `Direction.East`. The server-order scan uses `Map.Sosaria` file index `1`, so the route is checked through `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`. The accepted player targets are `1208,1386` through `1219,1386`, all dry jungle at z `0`.

The row is not perfectly blank, but it is not blocked. `1208,1386` has `0xD12` mushrooms flagged `Background`, `1210,1386` has `0xD0C` mushrooms flagged `Background`, `1214,1386` has nonblocking `0xCA5` pampas grass, and `1216,1386` has a `0xD72` tree flagged `ArticleA|Background` plus `0xD78` foliage leaves. None of those carry `Impassable`, `Surface`, `Bridge`, or `Wet`. No saved mobile or visible world item stands on any player target tile.

I let the fox shadow the same routine movement instead of inventing a private pet timer. It trails to approximately `Point3D(1216,1387,0)`, dx `-3`, dy `1`, still inside the `2..3` heel band. Its shadow row has only nonblocking scenery: a fern at `1205,1387`, another fern at `1209,1387`, and a background tree plus foliage at `1215,1387`. No `BaseAI.Obey`, Guard, Attack, Combatant, ownership, follower-count, or body-blocking path runs.

The endpoint rectangle x `1201..1237`, y `1368..1404` stays empty of saved visible wild mobiles and saved visible items. The west crane is now dx `-43`, dy `-7`; the west toad is dx `-45`, dy `-7`, still unresolved but no longer screen pressure. The eastern toad and panda are closer again at dx `24`, and the monkey is dx `30`; the toad is still outside its range-10 teleport scan and outside effective range-16 acquisition from this exact geometry. The invisible pressure count drops from six to four overlapping `PremiumSpawner` home ranges because the west pair falls out of overlap.

Mechanical friction learned:

- A second twelve-input east Travel Segment from `1207,1386` to `1219,1386` is legal over dry Sosaria jungle.
- Scenery on the row is passable because the checked statics do not carry movement-stopping flags.
- The endpoint still has zero saved visible wild mobiles, zero saved visible items, and zero visible running spawner objects.
- Moving east reduces the west crane/toad cluster from recently visible pressure to route history, but it does not despawn, kill, tame, pacify, or timer-resolve them.
- The fox remains only approximately shadowed, not exact pet AI proof.

Next pressure:

Mira ends at `Point3D(1219,1386,0)`, facing east. The fox is approximately at `Point3D(1216,1387,0)`, still following inside the heel band. No gump, context menu, target cursor, combat, damage, hunger/thirst, item, quest, discovery, pet-order, ownership, follower-count, skill, fame, karma, or PvP/PvE state changed. The next honest move is another fresh scan and route choice: east keeps leaning toward the Ruins marker and the old eastern wildlife cluster, while south/southeast starts bending into known rocks, trees, and southern carried pressure.

## Run 169 - The Eastern Animals Come Back Into View

I start at `Point3D(1219,1386,0)`, facing east. The screen is still quiet, but I do not mistake that for a town road. The only visible body is my fox, approximately at `Point3D(1216,1387,0)`, and the live-state rectangle has no saved wild mobile or item I can click. The map overlay keeps pulling me toward `Ruins` at `1303,1449`, while the older eastern animal cluster is just outside the current screen.

**Travel Segment**

I press east, but this is not a full twelve-step march. I stop the moment the screen changes.

`Mobile.Move -> MovementImpl.CheckMovement` accepts six cardinal east steps: `1220,1386` through `1225,1386`. The server-order `map1.mul` read says every target is dry jungle at z `0`. The only scenery on the walked row is a fern at `1223,1386`, and tiledata gives it `ArticleA`, not `Impassable`, `Surface`, `Bridge`, or `Wet`. No saved mobile or item stands on any walked target tile.

I let the fox shadow the routine row from `Point3D(1216,1387,0)` to about `Point3D(1222,1387,0)`. That row is also dry jungle; the only scenery is pampas grass at `1219,1387`, and it has no blocking flags. This is still not a pet AI tick. It is just the travel shadow policy keeping the controlled fox in the remembered trailing band at dx `-3`, dy `1`.

At `Point3D(1225,1386,0)`, the old eastern animals enter the screen again. Panda serial `206400` is visible at `Point3D(1243,1379,0)`, dx `18`, dy `-7`. Toad serial `8544` is visible at `Point3D(1243,1382,0)`, dx `18`, dy `-4`. The toad's current `GiantToad.TeleportTimer` geometry is still negative because range `10` misses by x distance, and `FightMode.Aggressor` has no committed aggressor/aggressed/faction/ethic state to acquire me at effective range `16`. That is only a pressure read, not safety. A visible timer creature and a visible animal are now on the screen, so I stop before pressing east again.

Mechanical friction learned:

- A short east Travel Segment from `1219,1386` to `1225,1386` is legal over dry passable Sosaria jungle.
- The inclusive client rectangle at `1225,1386` catches the saved eastern panda and toad at dx `18`.
- The toad is visible pressure even though its current teleport and acquisition geometry are negative.
- The fox remains approximately shadowed, not exact `BaseAI.Obey` proof, and it is not guarding, attacking, body-blocking, or protecting me.

Next pressure:

Mira ends at `Point3D(1225,1386,0)`, facing east. The fox is approximately at `Point3D(1222,1387,0)`, still following inside the heel band. The visible screen now contains panda serial `206400` and toad serial `8544`; monkey serial `289478` is just outside the east edge at dx `24`, and Urulg remains off-screen south at dx `1`, dy `31` as carried private-timer risk. The next honest action is to react to or inspect the visible eastern animals before any more route movement.
