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

## Run 170 - Names On The Edge, Then I Turn Away

I start at `Point3D(1225,1386,0)`, still facing east. The screen is no longer empty: a toad is on the east edge at `Point3D(1243,1382,0)`, dx `18`, dy `-4`, and a panda is just above it at `Point3D(1243,1379,0)`, dx `18`, dy `-7`. The fox is behind me at about `Point3D(1222,1387,0)`, close enough to follow, but not guarding. I do not keep walking into the edge just because the range math is currently negative.

**Beat 1**

I single-click the visible toad.

`PacketHandlers.LookReq` finds mobile serial `8544`, `Mobile.CanSee` passes, and `Utility.InUpdateRange` accepts the inclusive edge. `Region.OnSingleClick` returns true. `BaseCreature.OnSingleClick` adds no tame or bonded line because the toad is uncontrolled, so `Mobile.OnSingleClick` sends only the overhead label: `a toad`.

That label is all I earn. The toad is still `AI_Animal/FightMode.Aggressor`, still has `GiantToad.TeleportTimer`, and still currently misses me because range `10` and effective range `16` both fail by x distance. No teleport, attack, target cursor, context menu, skill, quest, item, pet order, or movement path runs.

**Beat 2**

I single-click the panda.

The same normal click path accepts the edge mobile: `LookReq`, `CanSee`, inclusive `InUpdateRange`, `Region.OnSingleClick`, then `BaseCreature.OnSingleClick` and `Mobile.OnSingleClick`. The overhead label is `a panda`. It is a visible animal, not a friendly signpost: `PandaRiding` is `AI_Animal/FightMode.Aggressor`, has no special timer, and its effective range still misses me by x distance. No context menu opens and no taming or combat state changes.

**Beat 3**

I press west.

Because I was facing east, this is only a turn. `Mobile.Move` does not enter `MovementImpl.CheckMovement`, does not test `Point3D(1224,1386,0)`, and does not move me or the fox. It changes my facing to `Direction.West` and sends the usual movement notification over the same location. The visible animals do not get a new committed attack from this: their default `ReacquireOnMovement` is false, and the notice-sound branch explicitly skips `FightMode.Aggressor` creatures.

Mechanical friction learned:

- The eastern toad and panda are both real saved mobiles inside the inclusive 18-tile client rectangle.
- Single-clicking either one only labels it; it does not open a menu, begin taming, pull aggro, move the fox, or resolve the spawner.
- A west input from east-facing `Point3D(1225,1386,0)` is only a facing turn. No west tile has been entered yet.
- The toad's current timer/acquisition geometry is negative, but its private five-second timer, 25 percent random gate, and future wandering are still unresolved.

Next pressure:

Mira ends at `Point3D(1225,1386,0)`, facing west. The toad and panda remain visible and now labeled on the east edge. The monkey remains just off-screen to the east at dx `24`, the west crane/toad are far behind, and Urulg plus the old swamp-drake branch remain carried risk evidence. The next honest action is a route decision from a turned-away stance: likely step west to reduce the visible animal edge, but only after a fresh scan and movement precheck.

## Run 171 - I Back Off Until The Edge Is Quiet

I start at `Point3D(1225,1386,0)`, facing west. The toad and panda are still on the east edge, both already labeled, both still wild, and neither one is solved by the fact that the current range math misses me. The fox is a few tiles west of me, following but not guarding. So I do the plain human thing: I back away from the edge instead of walking deeper into it.

**Beat 1**

I press west once.

This time it is not just a turn. `Mobile.Move -> MovementImpl.CheckMovement` takes the real movement path because I am already facing west. The target `Point3D(1224,1386,0)` is the dry jungle tile I crossed in Run 169: `map1.mul` land `0xAF`, z `0`, zero statics, no saved mobile, and no saved item on the tile. Mira steps west.

That one tile matters. The labeled toad and panda slide just outside the client rectangle at dx `19`; the monkey is farther east at dx `25`. They are not gone, killed, tamed, pacified, or despawned. They are just off-screen now. The fox is still visible at `Point3D(1222,1387,0)`, dx `-2`, dy `1`, inside the heel band.

**Beat 2 - Travel Segment**

The screen is quiet enough to compress routine movement, so I keep backing west along the proved row.

I press west twelve times, from `Point3D(1224,1386,0)` through `Point3D(1212,1386,0)`. This is the same dry Sosaria jungle row already proved by the eastward runs: `1223,1386` has only a nonblocking fern, `1208..1219,1386` have zero or nonblocking scenery, and no live saved mobile or item stands on the traveled tiles. No gump, target cursor, region text, corpse, chest, shelter, sign, road, or water source appears.

The fox is only travel-shadowed, not exact-timer advanced. I keep it in the believable follow band, around `Point3D(1215,1387,0)`, dx `3`, dy `1`. That is leash bookkeeping, not `BaseAI.Obey` proof and not protection.

**Beat 3 - Travel Segment**

I keep the same conservative line for one more capped segment.

Twelve more west inputs take me from `Point3D(1212,1386,0)` to `Point3D(1200,1386,0)`. The targets `1211..1200,1386` are still the previously proved dry jungle row. `1210,1386` has only background mushrooms, `1208,1386` has only nonblocking mushrooms, and the rest of the walked targets are clean enough for cardinal movement. The live-state filter at the endpoint has zero saved visible wild mobiles, zero saved visible items, and zero visible running spawner bodies.

I do not call that safe. The west crane is now just outside movement-notice distance at dx `-24`, the west toad is dx `-26`, the eastern toad and panda are far off-screen at dx `43`, and Urulg plus the old drake branch remain carried private-timer risk. The fox is approximately at `Point3D(1203,1387,0)`, still following inside the heel band. I stop because I have spent three beats and the next action is a fresh route decision, not because the jungle became friendly.

Mechanical friction learned:

- A west input from Run 170's west-facing stance finally enters `MovementImpl.CheckMovement`; the prior west input did not.
- `Point3D(1224,1386,0)` is dry, passable Sosaria jungle with no saved mobile/item blocker, so stepping away from the labeled east-edge animals is legal.
- Once the player center shifts to `1224,1386`, the labeled eastern toad and panda become recently visible off-screen uncertainty at dx `19`, not visible click targets.
- Two westward Travel Segments can reuse the already proved `1223..1200,1386` dry-jungle row, but follower movement remains approximate shadowing, not a pet AI timer result.
- The endpoint at `Point3D(1200,1386,0)` is visually quiet in the saved snapshot, but overlapping invisible spawner ranges and recently visible animals remain risk evidence.

Next pressure:

Mira ends at `Point3D(1200,1386,0)`, facing west. The fox is approximately at `Point3D(1203,1387,0)`, following inside the heel band. The nearest overlay marker is now `Ruins` about 121 tiles southeast; `Mines of Morinia` is about 179 tiles west. The immediate screen has no saved wild mobile or item, but the west crane/toad are route history just beyond the screen and the southern/eastern spawner risk is still carried. The next honest action is a fresh scan and a route choice, not a safety claim.

## Run 172 - The Crane Reappears On The West Edge

I start at `Point3D(1200,1386,0)`, facing west. The screen is quiet in the strict client sense: no saved wild body, no saved visible item, no gump, no context menu, no target cursor. The fox is the only nearby body I can account for, trailing at about `Point3D(1203,1387,0)`. Quiet is not a blessing. The map overlay still says `Mines of Morinia` is west, and I already know the west route has a crane and a toad tucked just beyond the rectangle.

**Travel Segment**

I press west, but I do not spend the full segment cap.

`Mobile.Move -> MovementImpl.CheckMovement` accepts six cardinal west steps: `1199,1386` through `1194,1386`. Sosaria is still file index `1`, so the route is checked against `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul` with the server's column-major block order. The walked tiles are dry jungle at z `0`: `0xAD`, `0xAF`, `0xAE`, `0xAF`, `0xAE`, then `0xAC`. The final target has a fern `0xCA0`, but it is only `ArticleA|Unknown3`; it is not `Impassable`, `Surface`, `Bridge`, or `Wet`. No saved mobile or item stands on any target tile.

I let the fox shadow the same westward line from about `Point3D(1203,1387,0)` to `Point3D(1197,1387,0)`. Its local row is also dry jungle. The only traced static on that shadow row is mushrooms `0xD15` at `1199,1387`, flagged `Background`, so I keep the follower in the believable heel band without pretending an exact `BaseAI.Obey` timer fired. The fox is still following, not guarding, body-blocking, or fighting.

At `Point3D(1194,1386,0)`, the reason to stop is back on the screen: Crane serial `288081` is visible at `Point3D(1176,1379,0)`, dx `-18`, dy `-7`. It is the same crane I labeled earlier, but the screen does not know "same old pressure" as permission to march past it. Toad serial `737437` is still off-screen at dx `-20`, dy `-7`, outside its current range-10 teleport check and outside effective range-16 acquisition by x distance. That is not safety; it is just current geometry. The west spawner homes overlap the rectangle again, and the next west movement would push closer to the toad edge.

Mechanical friction learned:

- A six-input west Travel Segment from `1200,1386` to `1194,1386` is legal over dry Sosaria jungle.
- The travel stop is visibility, not collision: the crane re-enters at the inclusive west edge.
- The west toad is still just outside the client rectangle and outside current `GiantToad.TeleportTimer` geometry, but it remains route pressure.
- Approximate follower shadowing keeps the fox around `1197,1387`; no exact pet AI, Guard, Attack, Combatant, ownership, follower-count, or body-blocking path runs.

Next pressure:

Mira ends at `Point3D(1194,1386,0)`, facing west. The fox is approximately at `Point3D(1197,1387,0)`, still following inside the heel band. The crane is visible on the west edge at dx `-18`, and the west toad is just beyond it at dx `-20`. The immediate honest action is to react to the visible crane and nearby toad edge, not to keep walking west toward the Mines as routine travel.

## Run 173 - I Turn My Back On The Crane Line

I start at `Point3D(1194,1386,0)`, facing west, with the crane visible on the west edge and the toad just behind it outside the client rectangle. The crane is already named from the earlier click, and it is only a one-damage animal, but the toad behind it has the five-second teleport timer code. I do not keep walking toward that line just because the current range math misses me.

**Beat 1**

I press east.

This is only a turn. `Mobile.Move` sees that my current direction mask is west and the requested direction is east, so it skips `MovementImpl.CheckMovement`, leaves me at `Point3D(1194,1386,0)`, sets facing to `Direction.East`, and still sends movement notifications from the same tile. That notification does not create a committed attack: the crane and toad are `FightMode.Aggressor`, their default `ReacquireOnMovement` path is not firing, and the notice-sound branch skips Aggressor animals.

The screen is still the same rectangle. Crane serial `288081` remains visible at dx `-18`, and Toad serial `737437` remains just outside at dx `-20`. The fox stays at about `Point3D(1197,1387,0)`, following but not guarding.

**Beat 2**

I press east again.

Now the movement path is real. `MovementImpl.CheckMovement` accepts `Point3D(1195,1386,0)`, the same dry jungle tile I proved while retreating before: map1 land `0xAE`, z `0`, zero statics, no saved mobile, and no saved item blocker. I step east.

That one tile does what I wanted. The crane falls just outside the west edge at dx `-19`; the toad is farther off-screen at dx `-21` and still outside its range-10 teleport geometry and effective range-16 acquisition by x distance. I have not killed, tamed, pacified, despawned, or timer-resolved either creature. I have only stopped staring at them from the edge.

**Beat 3 - Travel Segment**

The screen is quiet in the narrow client sense, so I use the row I have already earned instead of picking at every blade of grass.

I press east twelve accepted times, from `Point3D(1196,1386,0)` through `Point3D(1207,1386,0)`. This is the same dry Sosaria jungle row from the earlier east retreat. No saved mobile or item stands on the targets, no visible running spawner object appears, and no gump, target cursor, region text, corpse, chest, sign, shelter, road, or water source interrupts the line.

The fox is shadowed, not exact-timer advanced, from about `Point3D(1197,1387,0)` to `Point3D(1204,1387,0)`. It ends at dx `-3`, dy `1`, inside the remembered heel band. That is still not Guard, Attack, body-blocking, or proof that a private pet AI tick fired.

The endpoint rectangle x `1189..1225`, y `1368..1404` has zero saved visible wild mobiles, zero visible saved items, and zero visible running spawner bodies. The west crane is now dx `-31`; the west toad is dx `-33`. The eastern toad and panda are not visible either, sitting around dx `36`, and the southern orc branch remains carried risk rather than a current screen blocker. Six invisible `PremiumSpawner` home ranges overlap the rectangle, so the quiet row is still not a safe place. It is just a better place to breathe than the crane/toad edge.

Mechanical friction learned:

- A direction key opposite the current facing can be only a turn. The first east input changed facing but did not enter `MovementImpl.CheckMovement` or move Mira.
- The second east input was a real step to `1195,1386`, which clears the visible crane from the client rectangle without resolving the crane or toad.
- A capped east Travel Segment from `1195,1386` to `1207,1386` is legal over previously proved dry jungle.
- The live snapshot at the endpoint has no saved visible wild mobile or item, but the overlapping spawner envelopes and unresolved western/eastern animals remain pressure evidence.

Next pressure:

Mira ends at `Point3D(1207,1386,0)`, facing east. The fox is approximately at `Point3D(1204,1387,0)`, still following inside the heel band. The nearest overlay marker is `Ruins` about 115 tiles southeast; `Mines of Morinia` is now about 186 tiles west through the crane/toad edge. No gump, context menu, target cursor, combat, damage, hunger/thirst, item, quest, discovery, pet-order, ownership, follower-count, skill, fame, karma, or PvP/PvE state changed. The next honest action is a fresh scan and route choice from a quiet rectangle, not a safety claim.

## Run 174 - I Skirt North Of The Animal Line

I start at `Point3D(1207,1386,0)`, facing east. The screen is quiet, but it is not a road. The west crane/toad pair is behind me, the east toad/panda pair is ahead of me, and Urulg is south enough that walking southeast would be a dumb way to prove courage. The overlay still tempts me toward `Ruins`, but I take the safer shape first: north, then east above the old animal line.

**Beat 1 - Travel Segment**

I turn north and walk twelve accepted steps to `Point3D(1207,1374,0)`.

`Mobile.Move` only treats a key as movement after the facing matches, so the first north input is just the turn. After that, `MovementImpl.CheckMovement` accepts the cardinal north targets `1207,1385` through `1207,1374`. Sosaria is still file index `1`, and the server-order `map1.mul` / `staidx1.mul` / `statics1.mul` probe says the line is dry jungle/grass with zero statics. No saved mobile, saved item, visible spawner object, region text, gump, corpse, chest, shelter, sign, road, water source, or target cursor appears.

The fox is shadowed, not exact-timer advanced, from about `Point3D(1204,1387,0)` to `Point3D(1204,1375,0)`. Its row is also dry grass/jungle with zero statics, so the leash stays believable at dx `-3`, dy `1`. That is not Guard, Attack, body-blocking, or protection.

**Beat 2 - Travel Segment**

I turn east and take twelve accepted east steps to `Point3D(1219,1374,0)`.

The row `1208,1374` through `1219,1374` is plain dry grass, all zero statics. The live-state rectangle at the endpoint still has no saved wild mobile or saved item inside it. The eastern toad and panda are closer but still off-screen at dx `24`, and Urulg is farther south at dy `43`. I do not call any of them resolved.

The fox shadows the same line to about `Point3D(1216,1375,0)`, again over dry grass with zero statics. No pet AI timer, ownership, follower-count, pet order, combat, or body-blocking state changes.

**Beat 3 - Travel Segment**

I keep going east, but only five steps.

`MovementImpl.CheckMovement` accepts `1220,1374` through `1224,1374`, all dry grass with zero statics and no saved target blocker. I stop there on purpose. One more east step would put the labeled toad and panda at dx `18` on the screen edge again. From here they are dx `19`, just outside the client rectangle. The toad is also outside its range-10 `TeleportTimer` scan by x distance, but it is still a timer creature from a running spawner, not solved scenery.

The fox shadows to about `Point3D(1221,1375,0)`, still dx `-3`, dy `1`, still following, still not guarding.

Mechanical friction learned:

- A north skirt from `1207,1386` to `1207,1374` is legal dry movement and avoids walking south toward Urulg's carried branch.
- The east skirt at y `1374` is clean grass through `1224,1374`, with no static blocker and no saved item/mobile blocker.
- Stopping at `1224,1374` keeps the labeled east toad and panda just outside the 18-tile client rectangle; `1225,1374` would expose them.
- Approximate follower shadowing remains only leash bookkeeping. It is not exact `BaseAI.Obey`, Guard, Attack, or body-blocking proof.

Next pressure:

Mira ends at `Point3D(1224,1374,0)`, facing east. The fox is approximately at `Point3D(1221,1375,0)`, inside the heel band. The immediate screen has zero saved wild mobiles and zero saved items, but eastern Toad serial `8544` and Panda serial `206400` sit just outside the east edge at dx `19`; Monkey serial `289478` is dx `25`; Urulg is off-screen south at dx `2`, dy `43`; and four invisible `PremiumSpawner` home ranges overlap by source range. The next honest input is not routine east travel. It is the decision whether to expose the toad/panda edge again, bend north, or abandon this route.

## Run 175 - A Different Monkey Catches The Corner

I start at `Point3D(1224,1374,0)`, facing east. The old toad and panda are still one tile beyond the east edge. I do not step straight into them. I try the boring answer first: climb farther north, then edge east above their line.

**Beat 1 - Travel Segment**

I turn north and walk twelve accepted steps to `Point3D(1224,1362,0)`.

`Mobile.Move` spends the first north input as a facing change. After that, `MovementImpl.CheckMovement` accepts `Point3D(1224,1373,0)` through `Point3D(1224,1362,0)`. Sosaria is still map file index `1`, and the server-order `map1.mul` / `staidx1.mul` / `statics1.mul` probe reads every target as dry grass at z `0`, with zero statics. No saved mobile, saved item, visible spawner object, gump, context menu, target cursor, region text, corpse, chest, sign, shelter, road, or water source appears.

The fox is only shadowed, not exact-timer advanced, from about `Point3D(1221,1375,0)` to about `Point3D(1221,1363,0)`. That column is also dry grass with zero statics. The fox stays in the familiar trailing band at dx `-3`, dy `1`, still following and still not guarding.

**Beat 2 - Travel Segment**

I keep the skirt going, but the screen stops me before the segment cap.

Two more north steps take me to `Point3D(1224,1360,0)`, then I turn east and move through `Point3D(1225,1360,0)`, `Point3D(1226,1360,0)`, and `Point3D(1227,1360,0)`. The five accepted targets are dry map1 grass at z `0`, with zero statics, no saved target blocker, and no item blocker.

At `Point3D(1227,1360,0)`, a different monkey enters the northeast corner: serial `206380`, `Point3D(1245,1366,0)`, dx `18`, dy `6`. That is not the old off-screen monkey serial `289478`; it is another saved spawn from the same eastern animal home. Its current `FightMode.Aggressor` acquisition is negative because the Aggressor gate has no aggressor/aggressed/faction/ethic state, and its effective range `16` also misses by x distance. Still, it is a real visible body on the screen. I stop there instead of smearing it into route scenery.

The fox shadows to about `Point3D(1224,1361,0)`, again by travel policy rather than `BaseAI.Obey`. It is close, but it is not a shield.

Mechanical friction learned:

- The north skirt from `1224,1374` to `1224,1362` is legal over dry zero-static grass.
- The next north/east bend is legal only until `1227,1360`; that point exposes saved Monkey serial `206380` at the inclusive edge.
- The previously labeled eastern toad and panda are now off-screen by y distance, not resolved.
- A visible saved animal interrupts route movement even when its current Aggressor acquisition is negative.

Next pressure:

Mira ends at `Point3D(1227,1360,0)`, facing east. The fox is approximately at `Point3D(1224,1361,0)`, inside the heel band. The visible screen contains Monkey serial `206380` at dx `18`, dy `6`, zero saved visible items, and zero visible spawner objects. Panther serial `289483` would appear farther east at dx `25`, and the old toad/panda line is south-east off-screen. The next honest action is to react to or inspect the visible monkey before any more east travel.

## Run 176 - I Name The Monkey And Back Off One Tile

I start at `Point3D(1227,1360,0)`, facing east. The monkey is the only wild thing on the screen, right on the northeast edge at `Point3D(1245,1366,0)`, dx `18`, dy `6`. The fox is behind me around `Point3D(1224,1361,0)`, still following and still not a guard dog in code. The monkey is small, but it is not scenery: it is a saved wild mobile from the same animal spawner line that has already thrown toads, pandas, and panthers into this route.

**Beat 1**

I single-click the monkey.

`PacketHandlers.LookReq` finds serial `206380`, `CanSee` passes, and `Utility.InUpdateRange` accepts the inclusive dx `18` edge. `Region.OnSingleClick` allows the click, `BaseCreature.OnSingleClick` adds no tame or bonded line because the monkey is uncontrolled, and `Mobile.OnSingleClick` sends the private overhead label: `a monkey`.

That is all. No context menu opens, no Tame row is selected, no target cursor appears, no taming timer starts, no banana drops out of its backpack, and no combat state changes. Its `AI_Animal/FightMode.Aggressor` path is still currently negative because there is no aggressor/aggressed/faction/ethic state and the effective range `16` misses me by x distance.

**Beat 2**

I press west.

Because I was facing east, this is only a turn. `Mobile.Move` skips `MovementImpl.CheckMovement`, leaves me at `Point3D(1227,1360,0)`, and changes facing to `Direction.West`. The monkey is still visible at the edge. The fox does not move; no pet AI tick has run.

**Beat 3**

I press west again.

Now the movement path is real. `MovementImpl.CheckMovement` accepts `Point3D(1226,1360,0)`: server-order `map1.mul` says dry grass `0x4` at z `0`, and `staidx1/statics1` finds zero statics. No saved mobile or item occupies the tile. I step west.

The monkey slides just outside the screen at dx `19`, dy `6`. It is not gone, dead, tamed, pacified, or resolved. It is merely off the client rectangle after one cautious step. The panther is still farther east at dx `26`, the old toad/panda line remains off-screen by y distance, and the four invisible spawner homes are still only area risk, not visible targets.

Mechanical friction learned:

- A saved monkey at the inclusive 18-tile edge is a legal single-click target, but the normal click only labels it.
- The first west input from an east-facing stance is just a facing turn; no movement check runs.
- The second west input legally steps to `Point3D(1226,1360,0)` over dry grass with zero statics.
- Moving one tile west clears Monkey serial `206380` from visible range without resolving its future AI, wandering, taming, combat, corpse, or loot paths.

Next pressure:

Mira ends at `Point3D(1226,1360,0)`, facing west. The fox remains approximately at `Point3D(1224,1361,0)`, inside the heel band by shadowing only. The immediate screen has zero saved wild mobiles and zero saved visible items, but Monkey serial `206380` is just off-screen at dx `19`, Panther serial `289483` is dx `26`, eastern Toad serial `8544` and Panda serial `206400` are off-screen by y distance, and Urulg plus the old swamp-drake branch remain carried unresolved risk. The next honest action is a fresh scan and route choice, likely north or west rather than re-exposing the monkey line.

## Run 177 - I Skirt A Pear Tree And Stop Before The Toad

I start at `Point3D(1226,1360,0)`, facing west. The screen is empty of saved wild mobiles and saved items, but it is not blank. The west row has a visible pear tree static at `Point3D(1210,1360,0)`, and the live save still puts the old animal pressure lines just outside reach: the monkey I labeled is behind me to the east, and a toad line waits somewhere west. The fox is around `Point3D(1224,1361,0)`, following, not guarding.

**Beat 1 - Travel Segment**

I walk west twelve accepted steps to `Point3D(1214,1360,0)`.

`Mobile.Move` can enter the movement block immediately because I already face west. `MovementImpl.CheckMovement` accepts the cardinal west targets. The server-order `map1.mul` / `staidx1.mul` / `statics1.mul` probe reads the line as dry Sosaria grass with zero statics, and the live-state rectangle at the endpoint has zero saved visible wild mobiles, zero saved items, and zero visible spawner objects. The fox is shadowed to about `Point3D(1217,1361,0)` over the adjacent clear row.

**Beat 2 - Travel Segment**

The pear tree blocks the direct row, so I do not try to walk through it. I turn north to `Point3D(1214,1359,0)`, walk west through `Point3D(1209,1359,0)`, step south to `Point3D(1209,1360,0)`, then continue west to `Point3D(1204,1360,0)`.

The important tile is `Point3D(1210,1360,0)`: static `0xDA8` is a pear tree with `TileFlag.Impassable` and height `20`, while `0xDA9` leaves are harmless visual clutter. The north bypass row is dry grass with zero blocking statics, so the path is legal and the tree stays a real obstacle rather than scenery. No gump, context menu, target cursor, corpse, chest, sign, shelter, water source, region text, combat, hunger, thirst, item, quest, discovery, ownership, follower-count, or pet-order path opens. The fox is shadowed to about `Point3D(1207,1361,0)`.

**Beat 3 - Travel Segment**

I continue west twelve accepted steps to `Point3D(1192,1360,0)`.

The route is still dry map1 grass/jungle with zero statics on accepted targets. The endpoint client rectangle `x=1174..1210,y=1342..1378` has zero saved visible wild mobiles, zero saved visible items, and zero visible running spawner object locations. That quiet result is narrow: Toad serial `288077` sits just outside the west edge at `Point3D(1173,1360,0)`, dx `-19`, dy `0`; Toad serial `737437` and Crane serial `288081` are just below the southwest edge at dy `19`. The fox shadows to about `Point3D(1195,1361,0)`, still only a follower in the heel band.

Mechanical friction learned:

- The direct west row is blocked at `1210,1360` by an impassable pear tree static; the legal player route must skirt it.
- `Map.Sosaria` still uses file index `1`, and the server-order binary scan matters because the accepted route is proved from `map1/staidx1/statics1`, not from guessed terrain.
- Three Travel Segment beats can move Mira from `1226,1360` to `1192,1360` without exposing a saved visible wild mobile, but only because the route bends around the tree and stops before the western toad edge.
- The controlled fox position is approximate travel shadowing, not exact `BaseAI.Obey`, Guard, Attack, or protection.

Next pressure:

Mira ends at `Point3D(1192,1360,0)`, facing west. The fox is approximately at `Point3D(1195,1361,0)`, inside the 2..3 heel band. The visible screen has no saved wild mobile or saved item, but Toad serial `288077` is one tile beyond the west edge and is a `GiantToad.TeleportTimer` creature by code. Current geometry keeps Mira outside that range-10 timer and outside effective range perception, but the next westward route choice would expose it. The next honest action is a fresh scan/classifier and a player decision about the toad edge, not another blind travel segment.

## Run 178 - I Dodge The West Toad And Find The Horse Again

I start at `Point3D(1192,1360,0)`, facing west. The screen is technically empty of wild saved bodies, but the edge math is loud: Toad serial `288077` is one tile beyond the west border at `Point3D(1173,1360,0)`. I can go west and make it visible again, or I can stop pretending the west marker is worth stepping into a timer creature. The map overlay says `Mines of Morinia` is west and `Ruins` is southeast, but neither marker is a road, shelter, vendor, or water source.

**Beat 1 - Travel Segment**

I turn north, take one step to `Point3D(1192,1359,0)`, then immediately discover the straight north line is not open. The server-order static probe shows trees at `1192,1358` and `1192,1357`; both carry `TileFlag.Impassable`. I do the thing a player actually does on screen: sidestep around the trunks instead of walking into them.

The accepted path is twelve movement inputs: `1192,1359`, `1193,1359`, `1193,1358`, `1194,1358`, `1194,1357`, `1195,1357`, then north through `1195,1351`. `Mobile.Move -> MovementImpl.CheckMovement` accepts every target. The route is dry Sosaria jungle/grass in `map1.mul`; the only statics on accepted tiles are nonblocking foliage leaves at `1195,1355` and `1195,1354`. A peach tree at `1194,1356` is also impassable, so the bypass stays east of it.

The fox is shadowed, not exact-timer advanced, along the same bend from about `Point3D(1195,1361,0)` to about `Point3D(1198,1352,0)`. That keeps the leash coherent over locally clear grass, but it is not `BaseAI.Obey`, Guard, Attack, or body-blocking proof.

At `Point3D(1195,1351,0)`, the live-state rectangle still contains zero saved visible wild mobiles, zero saved items, and zero visible running spawner objects. The west toad is farther off-screen at dx `-22`, dy `9`; its range-10 teleport scan and range-16 perception still miss by x distance. I have avoided it, not solved it.

**Beat 2 - Travel Segment**

I keep moving north, but only seven accepted steps.

`MovementImpl.CheckMovement` accepts `Point3D(1195,1350,0)` through `Point3D(1195,1344,0)`. These are dry grass targets with zero blocking statics; the foliage already behind me does not matter. The fox shadows to about `Point3D(1198,1345,0)`, still a following animal in the heel band and still not a shield.

At `Point3D(1195,1344,0)`, the top edge of the screen catches a familiar shape: Horse serial `200909` at `Point3D(1196,1326,0)`, dx `1`, dy `-18`. It is uncontrolled, alive, and visible. Its constructor is `AI_Animal` with `FightMode.Aggressor`, `MinTameSkill` normally `29.1` unless its rare random branch made it `49.1`. I do not know that branch from the save export, and I do not choose it. I also do not click or tame the horse yet. A visible animal at the edge is enough reason for a human to stop and look.

Mechanical friction learned:

- The straight north escape from `1192,1360` is blocked by impassable tree statics at `1192,1358` and `1192,1357`.
- The legal normal-player bypass bends east around those trees and the impassable peach tree at `1194,1356`.
- Moving north avoids exposing western Toad serial `288077`; it remains off-screen and unresolved, not safe.
- The second north segment stops when Horse serial `200909` enters the inclusive 18-tile client rectangle at the top edge.

Next pressure:

Mira ends at `Point3D(1195,1344,0)`, facing north. The fox is approximately at `Point3D(1198,1345,0)`, inside the 2..3 heel band by travel shadowing only. The screen has visible Horse serial `200909` at dx `1`, dy `-18`, zero saved visible items, and no open gump, context menu, or target cursor. The west toad is off-screen at dx `-22`, dy `16`; the older west toad and crane are farther south; Urulg, GoblinArcher, and the old swamp-drake branch remain carried unresolved risk. The next honest action is to inspect or react to the visible horse before more travel.

## Run 179 - The Horse Gets A Name

I start exactly where the last run left me: `Point3D(1195,1344,0)`, facing north, with my fox tucked southeast around `Point3D(1198,1345,0)`. The screen is not empty. The horse is still on the top edge at `Point3D(1196,1326,0)`, dx `1`, dy `-18`. The western toad line is off-screen now, but not solved. The horse is the visible thing.

**Beat 1**

I single-click the horse.

The client path is the plain label path. `LookReq` finds serial `200909`, `CanSee` passes, and the 18-tile update rectangle accepts the top-edge click. The region allows the single-click, the horse is not controlled, so `BaseCreature.OnSingleClick` adds no tame or bonded line, and the inherited mobile click sends the private overhead label: `a horse`.

That is all I get. I do not ask for the context menu. I do not select `Tame`. I do not walk north into the horse, and I do not pretend I know whether this is the normal `29.1` horse or the rare `49.1` one. The horse is now named on screen, but it is still wild, still visible, and still a decision point.

Mechanical friction learned:

- The inclusive update range makes a top-edge horse at dx `1`, dy `-18` a legal single-click target.
- A normal single-click is not the context menu. It does not expose or select the `Tame` row.
- The horse constructor makes it tamable and one control slot, with a normal `MinTameSkill` of `29.1` and a rare branch to `49.1`, but the visible label does not reveal which branch this saved horse took.
- No movement, pet AI tick, combat, targeting cursor, ownership, follower-count, skill, inventory, hunger, thirst, quest, discovery, corpse, loot, gump, or PvP/PvE path ran.

Next pressure:

Mira remains at `Point3D(1195,1344,0)`, facing north. The fox remains approximately at `Point3D(1198,1345,0)`, following but not guarding. The current screen still contains the labeled Horse serial `200909`, zero saved visible items, and no visible running spawner objects. The next honest choice is whether to request the horse's context menu, step closer, back away, or route around it. The label is knowledge, not safety.

## Run 180 - The Horse Menu Is A Gray Tease

I start without moving: `Point3D(1195,1344,0)`, facing north. The fox is still tucked southeast around `Point3D(1198,1345,0)`. The horse is still right on the top edge at `Point3D(1196,1326,0)`, dx `1`, dy `-18`, labeled but wild. The screen has no saved visible items, no chest, no corpse, no road, no shelter, no water source, no gump, and no target cursor. The horse is the only thing asking for a click.

**Beat 1**

I ask the horse for its normal context menu.

The server accepts the request because the horse is on the same map, visible, and inside the 18-tile update rectangle. `ContextMenu` asks the horse for entries, and `BaseCreature.GetContextMenuEntries` adds `Tame` because the horse is tamable, uncontrolled, and I am alive.

Then the range friction lands on the screen. `TameEntry` is localization `6130` with range `6`, but the menu packet checks my current distance to the horse before drawing the row as usable. At dx `1`, dy `-18`, the row is visible but disabled. I have not selected it. `ContextMenuResponse` has not run, the menu has not been cleared, `TameEntry.OnClick` has not called `UseSkill(Taming)`, and the hidden range-2 `Taming.InternalTarget` gate has not even had a chance to reject me.

Mechanical friction learned:

- A visible mobile at the inclusive 18-tile edge can still open a context menu.
- The `Tame` row exists for the wild horse, but the client receives it disabled because `DisplayContextMenu` applies the row's range-6 gate.
- Opening the menu changes visible UI state only. It does not move Mira, start taming, open a target cursor, roll a skill check, claim ownership, fill a follower slot, or resolve the horse's normal `29.1` versus rare `49.1` tame branch.

Next pressure:

Mira remains at `Point3D(1195,1344,0)`, facing north. The fox remains approximately at `Point3D(1198,1345,0)`, following but not guarding. The horse menu is open with a disabled `Tame` row, and the horse remains wild at dx `1`, dy `-18`. Western Toad serial `288077`, west Toad serial `737437`, Crane serial `288081`, Urulg, GoblinArcher, and the old swamp-drake branch remain unresolved off-screen risk. The next honest action is to ignore/close the menu, step closer, or back away; pressing a disabled row is not a real progress path.

## Run 181 - Two Steps Toward The Horse, Then Wings

I start at `Point3D(1195,1344,0)`, facing north, with the horse menu still sitting on the screen like a gray dare. `Tame` is visible but disabled. I do not press it. The horse is not closer because I understand it better; it is still eighteen tiles up-screen, wild, and outside the entry range.

**Beat 1**

I press north.

Because I am already facing north, this is real movement, not a turn. `Mobile.Move` reaches `MovementImpl.CheckMovement`, and the server-order map probe accepts `Point3D(1195,1343,0)`: map1 grass `0x6`, z `0`, zero statics, no saved mobile or item blocker on the target. The client-visible horse menu drops off the screen because I moved, but I did not send `ContextMenuResponse`. No row was selected, no target cursor opened, and the stale server-side menu may still exist until a response or replacement menu.

The horse is still visible at dx `1`, dy `-17`. The fox is still only an approximate follower behind me around `Point3D(1198,1345,0)`, now dx `3`, dy `2`, still not guarding.

**Beat 2**

I press north again.

`MovementImpl.CheckMovement` accepts `Point3D(1195,1342,0)`: map1 grass `0x4`, z `0`, zero statics, no saved target blocker. That second step brings the horse a little closer, dx `1`, dy `-16`, but it also pulls a new body onto the screen. Eagle serial `444927` appears at `Point3D(1213,1324,0)`, right on the northeast corner at dx `18`, dy `-18`.

The live save also still lists Fox serial `200274` at `Point3D(1209,1324,0)`, but that is my fox's old saved coordinate. The simulation already won that animal through the taming timer and has it following at `Point3D(1198,1345,0)`. I do not hallucinate a second wild fox out of the stale export.

Mechanical friction learned:

- Moving with a context menu visible is a normal way to drop the client-visible menu, but it is not `ContextMenuResponse` and does not clear/select a row through the traced server path.
- The direct north approach from `1195,1344` to `1195,1342` is legal dry grass with zero statics.
- At `1195,1342`, the horse is still outside `TameEntry` range `6`; it is visible at dy `-16`, not reachable for the row yet.
- A saved eagle enters at the inclusive 18-tile corner after only two north steps, so the approach is no longer routine travel.
- The immutable live-state fox coordinate is overridden by the simulated controlled-follower state for the same serial.

Next pressure:

Mira ends at `Point3D(1195,1342,0)`, facing north. The fox is approximately at `Point3D(1198,1345,0)`, inside the 2..3 heel band by floored distance, still following and still not protection. The visible screen contains Horse serial `200909` at dx `1`, dy `-16` and Eagle serial `444927` at dx `18`, dy `-18`, with zero saved visible items. Both animals use `AI_Animal`/`FightMode.Aggressor`; the current Aggressor gate is negative because there is no aggressor/aggressed/faction/ethic state involving me or the fox. That makes them visible decisions, not active attacks. The next honest action is to inspect or react to the new eagle before taking another horse-approach step.

## Run 182 - The Eagle Gets Its Name

I start at `Point3D(1195,1342,0)`, facing north. The horse is still up-screen at `Point3D(1196,1326,0)`, dx `1`, dy `-16`, wild and still outside the range-6 `Tame` menu gate. The new thing is the bird in the northeast corner: Eagle serial `444927` at `Point3D(1213,1324,0)`, dx `18`, dy `-18`. The old live-save fox record at `1209,1324` is not a second wild fox; it is the same serial I already tamed, and the simulation has it following behind me around `Point3D(1198,1345,0)`.

**Beat 1**

I single-click the eagle.

`PacketHandlers.LookReq` finds serial `444927`, `CanSee` passes, and `Utility.InUpdateRange` accepts the inclusive 18-tile corner. The region allows the single-click, `BaseCreature.OnSingleClick` adds no tame or bonded overhead because the eagle is uncontrolled, and the inherited mobile click sends the private label: `an eagle`.

That is all. No context menu opens, no `Tame` row appears, no target cursor starts, no taming timer begins, no pet command runs, and no combat state changes. The eagle is only named. Its class says it is an `AI_Animal` with `FightMode.Aggressor`, physical damage `5-10`, one control slot, and `MinTameSkill = 17.1`, but the current Aggressor acquisition is still negative because there is no aggressor/aggressed/faction/ethic state involving me or the fox, and its range-16 perception misses by x/y distance from this corner.

Mechanical friction learned:

- A saved eagle at dx `18`, dy `-18` is still a legal single-click target because update range is an inclusive rectangle.
- A normal single-click labels the eagle only. It does not ask for a context menu, use Taming, assign Combatant, move the eagle, move the fox, or mutate inventory, skills, quests, hunger, thirst, fame, karma, or discovery flags.
- The horse remains visible but still too far for an enabled `Tame` context row at dx `1`, dy `-16`.
- The controlled fox remains a simulated follower override, not the stale wild live-save coordinate.

Next pressure:

Mira remains at `Point3D(1195,1342,0)`, facing north. The fox remains approximately at `Point3D(1198,1345,0)`, inside the heel band by floored distance. The visible screen contains labeled Horse serial `200909` and labeled Eagle serial `444927`, zero saved visible items, and no open gump, context menu, target cursor, combat, corpse, loot, skill timer, quest, or discovery change. The next honest action is a fresh decision: keep approaching the horse, request a menu from something closer, or back away from the wildlife edge.

## Run 183 - One Step Closer, Then A Llama

I start at `Point3D(1195,1342,0)`, facing north. The horse is still the reason my eyes keep drifting up the screen: `Point3D(1196,1326,0)`, dx `1`, dy `-16`, labeled and still too far for the range-6 `Tame` row. The eagle I just named sits on the northeast edge at dx `18`, dy `-18`. The fox is behind me around `Point3D(1198,1345,0)`, following but not guarding.

**Beat 1**

I press north.

Because I am already facing north, this is a real movement attempt. `Mobile.Move` enters `CheckMovement`, and the server-order map/static probe accepts `Point3D(1195,1341,0)`: map1 grass `0x4`, z `0`, zero statics, and no saved mobile or item blocker on the target.

That one step does move the horse closer, to dx `1`, dy `-15`, but it does not make the horse reachable. It also changes the top edge of the screen. RidableLlama serial `226037` appears at `Point3D(1208,1323,0)`, dx `13`, dy `-18`. The eagle remains visible at dx `18`, dy `-17`. The live-save fox row at `1209,1324` is still overridden by my simulated controlled follower at `1198,1345`; I do not split one fox into two animals just because the stale save row is still there.

Mechanical friction learned:

- A single north step from `1195,1342` to `1195,1341` is legal dry Sosaria grass with zero statics.
- The horse is still outside the visible context-menu `Tame` row range: dx `1`, dy `-15` is not range `6`.
- The first new thing revealed by the approach is not the horse becoming usable. It is a wild llama on the inclusive top edge.
- The fox did not get a `BaseAI.Obey` tick, so it stays at `1198,1345` and stretches outside the 2..3 heel band at floored distance `4`.

Next pressure:

Mira ends at `Point3D(1195,1341,0)`, facing north. The screen contains Horse serial `200909`, RidableLlama serial `226037`, and labeled Eagle serial `444927`, with zero saved visible items and no open UI. All three are `AI_Animal`/`FightMode.Aggressor` bodies with current empty Aggressor state, so they are not active attacks, but the newly visible llama is a real screen interruption. The next honest action is to inspect or route around the llama/horse/eagle cluster before any more north movement.

## Run 184 - The Llama Menu Is Gray Too

I start at `Point3D(1195,1341,0)`, facing north. The screen is a little crowded now: the horse is still up-screen at dx `1`, dy `-15`, the eagle is still labeled on the northeast edge, and the new llama is sitting at `Point3D(1208,1323,0)`, dx `13`, dy `-18`. The fox is behind me at about `Point3D(1198,1345,0)`, following but stretched outside the heel band.

**Beat 1**

I single-click the llama.

`PacketHandlers.LookReq` finds serial `226037`, `CanSee` passes, and `Utility.InUpdateRange` accepts the inclusive top-edge rectangle. `Region.OnSingleClick` allows the click. Because the llama is uncontrolled, `BaseCreature.OnSingleClick` adds no tame or bonded line, and the inherited mobile click sends only the private overhead label: `a llama`.

Nothing else moves. No target cursor opens, no taming timer starts, no skill delay changes, and the fox does not get an AI tick.

**Beat 2**

I ask the labeled llama for its normal context menu.

The request is legal because the llama is visible and still inside the 18-tile update rectangle. `ContextMenu` asks the target for entries, and `BaseCreature.GetContextMenuEntries` adds `Tame` because a ridable llama is tamable, uncontrolled, and I am alive. The menu opens, but it is another gray tease: `TameEntry` uses range `6`, and from dx `13`, dy `-18` I am nowhere near it. `DisplayContextMenu` draws the row disabled.

I do not select it. There is no `ContextMenuResponse`, no `TameEntry.OnClick`, no `Taming.OnUse`, no target cursor, no ownership change, and no follower-count change. The stale horse menu is replaced by the llama menu as the current server-side context menu reference, but that is only UI state.

Mechanical friction learned:

- A wild ridable llama at dx `13`, dy `-18` is close enough to label and request a context menu through update range.
- The llama is tamable by class, with `AI_Animal`, `FightMode.Aggressor`, one control slot, and `MinTameSkill = 29.1`, but the visible row is range-gated.
- `Tame` is present but disabled at range `6`; the hidden range-2 `Taming.InternalTarget` gate still has not run.
- Labeling and opening the menu do not advance movement, pet follow AI, combat, hunger, thirst, skills, quests, loot, corpses, or discovery.

Next pressure:

Mira remains at `Point3D(1195,1341,0)`, facing north. The llama menu is open with a disabled `Tame` row. Horse serial `200909` and Eagle serial `444927` remain visible, the fox remains approximate at `Point3D(1198,1345,0)` and outside the 2..3 heel band, and zero saved visible items are on screen. The next honest action is to close/ignore the llama menu, back away, or reposition with the visible wildlife cluster still unresolved.

## Run 185 - Close The Gray Menu, Wait The Fox

I start at `Point3D(1195,1341,0)`, facing north, with the llama menu still in my face. The only tempting row is `Tame`, and it is gray because the llama is way up on the top edge at dx `13`, dy `-18`, outside the row's range `6`. The horse is still visible at dx `1`, dy `-15`, the eagle is still visible at dx `18`, dy `-17`, and my fox is behind me at `Point3D(1198,1345,0)`, stretched farther than the heel band.

**Beat 1**

I close or ignore the disabled llama menu.

That is a client-screen action, not a server-side selection. I do not send `ContextMenuResponse`, so the server path that clears `from.ContextMenu`, range-checks the entry, and calls `OnClick` never runs. No `TameEntry.OnClick`, no `Taming.OnUse`, no target cursor, no ownership change, no follower-count change. I just stop staring at a gray row I cannot press.

**Beat 2**

I wait long enough for the fox to catch up once.

`BaseAI.Obey` routes into `DoOrderFollow`, and the fox's corrected spacing is not four. From `Point3D(1198,1345,0)` to me at `Point3D(1195,1341,0)`, dx `3`, dy `4` floors to distance `5`, which is outside the remembered FriendsAvoidHeels band. `WalkMobileRange` reuses spacing value `7`, wants `2..3`, chooses `Direction.Up`, and `Mobile.Move` accepts `Point3D(1197,1344,0)`. The target and non-player side tiles are dry map1 grass with zero statics and no saved blocker.

The fox is now close enough again: dx `2`, dy `3`, floored distance `3`. I did not move. Nothing got tamed. The wildlife line did not stop watching me.

Mechanical friction learned:

- Closing or ignoring a visible context menu is not the same as selecting a row; without `ContextMenuResponse`, the disabled llama `Tame` row does no work.
- The fox's dx `3`, dy `4` leash is floored distance `5`, not `4`, so one exact follow tick is legal and expected before more movement.
- The fox can step northwest to `1197,1344` over clear dry grass; that brings it back into the 2..3 heel band without changing orders, ownership, combat, or follower count.
- Horse serial `200909`, RidableLlama serial `226037`, and Eagle serial `444927` remain visible player decisions, not resolved systems.

Next pressure:

Mira remains at `Point3D(1195,1341,0)`, facing north. No gump, context menu, target cursor, taming timer, combat, corpse, loot, quest, discovery, hunger, thirst, or skill path is open. The controlled fox is at `Point3D(1197,1344,0)`, following and inside the heel band. The screen still has the horse, the labeled llama, and the labeled eagle, with zero saved visible items. The next honest action is another visible-wildlife decision, not routine travel.

## Run 186 - The Eagle Menu Is Gray Too

I start at `Point3D(1195,1341,0)`, facing north. The horse is still almost straight up-screen at dx `1`, dy `-15`, the llama is still on the top edge at dx `13`, dy `-18`, and the eagle is still named in the northeast corner at dx `18`, dy `-17`. My fox is finally close again at `Point3D(1197,1344,0)`, following and not protecting me. The screen is not empty enough for travel.

**Beat 1**

I ask the labeled eagle for its normal context menu.

`PacketHandlers.ContextMenuRequest` finds Eagle serial `444927`, checks that we are on the same map, visible, and still inside `Utility.InUpdateRange`. `ContextMenu` asks the eagle for entries. Because the eagle is a tamable, uncontrolled `BaseCreature` and I am alive, `BaseCreature.GetContextMenuEntries` adds `Tame`. Then `Mobile.ContextMenu` sends `DisplayContextMenu`.

The row appears, but it is gray. `TameEntry` uses range `6`, and the eagle is at dx `18`, dy `-17`. The packet marks the entry disabled. I do not press it. No `ContextMenuResponse` is sent, so no `TameEntry.OnClick`, no `Taming.OnUse`, no target cursor, no ownership change, no follower-count change, no skill timer, and no combat path runs.

Mechanical friction learned:

- A saved eagle at dx `18`, dy `-17` can still receive a normal context-menu request because the client update rectangle is inclusive.
- The eagle is tamable by class, with `MinTameSkill = 17.1`, but the visible `Tame` row is disabled by the context-entry range `6`.
- Opening the eagle menu replaces the prior stored menu reference, but without `ContextMenuResponse` it does not select, clear, tame, attack, move, or resolve anything.
- The horse, llama, and eagle remain visible wildlife decisions. The fox remains a follower, not a shield.

Next pressure:

Mira remains at `Point3D(1195,1341,0)`, facing north. The eagle context menu is open with a disabled `Tame` row. Horse serial `200909`, RidableLlama serial `226037`, and Eagle serial `444927` remain visible; the controlled fox is at `Point3D(1197,1344,0)` inside the heel band. The next honest action is to close or ignore the gray eagle menu, then decide whether to reposition, back away, or keep inching toward the horse without pretending any animal is solved.

## Run 187 - I Stop Staring At The Gray Row

I start at `Point3D(1195,1341,0)`, facing north, with the eagle's menu still open. The only row I care about is `Tame`, and it is gray because the eagle is still way out at dx `18`, dy `-17`. The horse is closer but still not close enough at dx `1`, dy `-15`; the llama is still high on the screen at dx `13`, dy `-18`. My fox is close behind me at `Point3D(1197,1344,0)`, following, not guarding.

**Beat 1**

I close or ignore the disabled eagle menu.

That is not a selection. The traced server work would be `PacketHandlers.ContextMenuResponse`: clear `from.ContextMenu`, find the menu target, check the entry index, require `e.Enabled`, require `from.InRange(target, range)`, and only then call `OnClick`. I do not send that packet. No `TameEntry.OnClick`, no `Taming.OnUse`, no target cursor, no skill timer, no ownership change, and no follower-count change runs.

The screen underneath has not improved. The same three wild bodies are still there, and the live snapshot rectangle is still `x=1177..1213,y=1323..1359`: Horse serial `200909`, RidableLlama serial `226037`, and Eagle serial `444927`, with zero saved visible items or visible running spawner objects. The old exported fox at `1209,1324` is still my already-tamed serial, overridden by the simulated follower at `1197,1344`; I do not split it into another animal.

Mechanical friction learned:

- Closing or ignoring a context menu is not `ContextMenuResponse`.
- A disabled row can teach me the range gate, but it cannot be pressed after I close it.
- The eagle is still only labeled and visible. It is not tame, targeted, fought, moved, pacified, killed, or resolved.
- Removing the open UI does not make the horse/llama/eagle cluster routine travel.

Next pressure:

Mira remains at `Point3D(1195,1341,0)`, facing north. No gump, client-visible context menu, target cursor, taming timer, combat, corpse, loot, quest, discovery, hunger, thirst, skill path, or pet AI tick is open. The controlled fox stays at `Point3D(1197,1344,0)`, inside the heel band. The horse, llama, and eagle remain visible unresolved wildlife decisions. The next honest action is to inspect, reposition, approach, or retreat; it is not a travel segment and it is not selecting the gray row I just closed.

## Run 188 - The Horse Is Closer, Not Close

I start exactly where the last menu left me: `Point3D(1195,1341,0)`, facing north, with no gump or context menu open. The fox is tucked behind me at `Point3D(1197,1344,0)`. The live screen is still not empty: the horse is the nearest unresolved body at `Point3D(1196,1326,0)`, dx `1`, dy `-15`; the llama is labeled on the top edge at dx `13`, dy `-18`; and the eagle is labeled in the northeast at dx `18`, dy `-17`. There are zero saved visible items, no corpse, no chest, no road, no shelter, no water source, and no running spawner object on-screen. The map overlay still whispers `Ruins` southeast and `Mines of Morinia` west, but those are not screen objects.

**Beat 1**

I ask the horse for its normal context menu again.

The request is legal. The horse is on `Map.Sosaria`, visible, and inside the inclusive 18-tile update rectangle, so `PacketHandlers.ContextMenuRequest` lets the click reach `ContextMenu`. `BaseCreature.GetContextMenuEntries` adds `Tame` because the horse is tamable, uncontrolled, and I am alive. Then the same old range gate bites down: `TameEntry` is localization `6130` with range `6`, and the packet disables the row because the horse is still dx `1`, dy `-15` from me.

This does not start taming. I do not send `ContextMenuResponse`, do not select a row, do not get a target cursor, and do not reach `Taming.OnUse` or `Taming.InternalTarget`. The horse is merely closer than it was in Run 180, not close enough. Its hidden normal-versus-rare tame difficulty branch is still not visible to me.

Mechanical friction learned:

- Reopening a context menu from a new distance is normal play, but it only retests the visible menu gate.
- The horse can be clicked and menued from dx `1`, dy `-15`, yet the `Tame` row remains disabled because the row range is `6`.
- The range-2 taming target gate, skill timer, ownership path, follower count, combat, item use, hunger/thirst, quest, discovery, fame, karma, and pet AI do not run from a disabled row.
- The llama and eagle remain labeled, wild, and unresolved. The fox remains a follower, not a shield.

Next pressure:

Mira remains at `Point3D(1195,1341,0)`, facing north. The horse context menu is open with a disabled `Tame` row. Horse serial `200909`, RidableLlama serial `226037`, and Eagle serial `444927` are still visible; the controlled fox is still at `Point3D(1197,1344,0)` inside the heel band. The next honest action is to close or ignore the gray horse row, step closer, or retreat. Pressing the disabled row is still not a real action.

## Run 189 - I Close The Horse Menu

I start at `Point3D(1195,1341,0)`, facing north, with the horse menu still open from the last click. The row says `Tame`, but it is gray because the horse is still at dx `1`, dy `-15`, far outside the row's range `6`. The llama and eagle are still labeled on the top edge, and my fox is still close behind me at `Point3D(1197,1344,0)`.

**Beat 1**

I close or ignore the disabled horse menu.

That is a client-screen dismissal, not a choice. The server path that would matter is `PacketHandlers.ContextMenuResponse`: clear `from.ContextMenu`, find the same target, read a selected row index, require `e.Enabled`, require `from.InRange(target, range)`, and only then call `OnClick`. I do not send that packet. The disabled `Tame` row does not become an action just because I saw it.

The live rectangle is unchanged: `x=1177..1213,y=1323..1359`. I still see Horse serial `200909` at dx `1`, dy `-15`, RidableLlama serial `226037` at dx `13`, dy `-18`, and Eagle serial `444927` at dx `18`, dy `-17`. There are zero saved visible items and zero visible running spawner objects. The exported fox at `1209,1324` is still my already-tamed serial, overridden by the simulated follower at `1197,1344`; I do not invent a second fox.

Mechanical friction learned:

- Closing or ignoring the horse menu does not send `ContextMenuResponse`.
- The server-side clear/row-selection/`OnClick` path did not run.
- `TameEntry.OnClick`, `Taming.OnUse`, target cursor assignment, range-2 taming targeting, skill timers, ownership, follower count, combat, loot, hunger, thirst, quest, discovery, fame, karma, and PvP/PvE state are unchanged.
- Removing the gray menu only returns me to the same wildlife screen.

Next pressure:

Mira remains at `Point3D(1195,1341,0)`, facing north. No gump, client-visible context menu, target cursor, taming timer, combat, corpse, loot, quest, discovery, hunger, thirst, skill path, or pet AI tick is open. The controlled fox stays at `Point3D(1197,1344,0)`, inside the heel band. The horse, llama, and eagle remain visible unresolved wildlife decisions. The next honest action is to step closer, retreat, or inspect another visible animal; it is not a travel segment and it is not selecting the gray row I just closed.

## Run 190 - Two Steps Toward The Horse

I start at `Point3D(1195,1341,0)`, facing north, with no menu left to hide behind. The horse is still the closest useful-looking body at `Point3D(1196,1326,0)`, dx `1`, dy `-15`; the llama is still labeled up at dx `13`, dy `-18`; the eagle is still labeled on the northeast edge at dx `18`, dy `-17`. My fox is behind me at `Point3D(1197,1344,0)`, following, not guarding.

**Beat 1**

I press north.

Because I am already facing north, `Mobile.Move` takes the real movement path. `MovementImpl.CheckMovement` tests the forward tile and accepts `Point3D(1195,1340,0)`: server-order map1 block `76455`, grass tile `0x4`, average movement z `0`, no statics, and no saved mobile or visible item blocker.

The horse gets a little closer, not close. It is now dx `1`, dy `-14`, still far outside the visible context-entry range `6`. The llama and eagle stay visible at dx `13`, dy `-17` and dx `18`, dy `-16`. The fox is now stretched behind me at dx `2`, dy `4`, floored distance `4`, so I do not keep walking as if it is glued to my heel.

**Beat 2**

I wait for the fox to catch up once.

`BaseAI.Obey` routes into `DoOrderFollow`. FriendsAvoidHeels is still using the remembered `FollowersMax = 7`, so the desired band is `2..3`. The fox's floored distance is `4`, and `GetDirectionTo` chooses `Direction.Up`. `DoMove` forces the direction and moves the fox northwest to `Point3D(1196,1343,0)` over grass tile `0x5`, average movement z `0`, zero statics, and no saved blocker.

That fixes spacing, not danger. The fox is now dx `1`, dy `3`, floored distance `3`, still only a follower. No Guard, Attack, combatant, body-blocking, ownership, follower-count, or pet-order state changes.

**Beat 3**

I press north again.

The player movement path accepts the second forward tile, `Point3D(1195,1339,0)`: grass tile `0x6`, average movement z `0`, no statics, and no saved blocker. The screen does not reveal a new body, chest, corpse, water source, road, sign, or gump. It is the same little animal wall, just closer.

The horse is dx `1`, dy `-13`; the llama is dx `13`, dy `-16`; the eagle is dx `18`, dy `-15`. Zero saved visible items are in the rectangle, and no visible running spawner object stands on-screen. The raw live save still lists fox serial `200274` at `1209,1324`, but that serial is already mine in the simulation and now stands at `1196,1343`; I do not invent a second wild fox.

Mechanical friction learned:

- A north step to `1195,1340` and another to `1195,1339` are both legal dry Sosaria grass with zero statics and no saved blockers.
- The controlled fox can follow one tile northwest to `1196,1343` through the exact `BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> DoMove` path, but that still does not make it a guard or shield.
- The horse is closer but still not usable: dx `1`, dy `-13` fails the visible `TameEntry` range `6`, so the range-2 `Taming.InternalTarget` gate still has not run.
- The llama has moved into range-perception geometry by rectangle math, but `FightMode.Aggressor` still short-circuits without aggressor/aggressed/faction/ethic state. The eagle remains visible but outside range perception by x distance.
- The four overlapping invisible PremiumSpawner home ranges remain area risk only; their saved spawned refs are the visible animals already acknowledged.

Next pressure:

Mira ends at `Point3D(1195,1339,0)`, facing north. The fox is visible behind her at `Point3D(1196,1343,0)`, stretched again to dx `1`, dy `4`, floored distance `4`, so the next honest beat is probably another fox-follow wait before any more approach. The screen still contains Horse serial `200909`, RidableLlama serial `226037`, and Eagle serial `444927`, with zero saved visible items and no open UI. The next player decision is whether to keep inching toward the horse, back away from the animal cluster, or inspect the screen again; it is not a travel segment and not a tame attempt.

## Run 191 - The Fox Keeps Me Honest

I start at `Point3D(1195,1339,0)`, facing north. The horse is the obvious temptation now, almost straight ahead at `Point3D(1196,1326,0)`, dx `1`, dy `-13`, but that is still nowhere near the `Tame` row's range `6`. The llama and eagle are still up on the same screen edge, and my fox is stretched behind me at `Point3D(1196,1343,0)`, following but one tile too loose.

**Beat 1**

I wait for the fox instead of pretending it is attached to my boots.

`BaseAI.Obey` goes through `DoOrderFollow`, `WalkMobileRange`, and the same FriendsAvoidHeels spacing. The desired band is still `2..3`; the fox is at floored distance `4`, so it needs to move. From `1196,1343` toward me at `1195,1339`, `GetDirectionTo` resolves to `Direction.North`, not northwest, and `DoMove` accepts `Point3D(1196,1342,0)`. The tile is dry map1 grass `0x6`, z `0`, with zero statics and no saved blocker.

That puts the fox back at dx `1`, dy `3`. No Guard order appears. No combat state appears. It is just following.

**Beat 2**

I press north once.

Because I am already facing north, this is a real `Mobile.Move` attempt. `MovementImpl.CheckMovement` accepts `Point3D(1195,1338,0)`: dry map1 grass `0x4`, z `0`, zero statics, no saved mobile, and no saved item on the target.

The horse is closer, but not close. It is now dx `1`, dy `-12`, still outside the visible `TameEntry` range `6`. The llama is dx `13`, dy `-15`; the eagle is dx `18`, dy `-14`. Nothing new enters the screen, and no menu opens. The fox is stretched again at dx `1`, dy `4`, because I moved after it did.

**Beat 3**

I wait for the fox again.

The same follow path runs: `BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo(Direction.North) -> DoMove`. The fox steps from `Point3D(1196,1342,0)` to `Point3D(1196,1341,0)`. The tile is dry map1 grass `0x3`, z `0`, zero statics, and no saved blocker. It ends inside the heel band at dx `1`, dy `3`.

Mechanical friction learned:

- Two exact fox Follow ticks can keep the follower coherent while I creep north, but they still do not make the fox a guard, shield, or combat participant.
- The north step to `1195,1338` is legal dry Sosaria grass with zero statics and no saved blocker.
- Horse serial `200909` is closer but still not usable: dx `1`, dy `-12` fails the visible range-6 `Tame` row, so `TameEntry.OnClick`, `Taming.OnUse`, and `Taming.InternalTarget` still have not run.
- The live snapshot rectangle `x=1177..1213,y=1320..1356` still shows the same horse, llama, and eagle, zero saved visible items, and zero visible running spawner objects.

Next pressure:

Mira ends at `Point3D(1195,1338,0)`, facing north. The fox is at `Point3D(1196,1341,0)`, following and inside the heel band. The horse is visible at dx `1`, dy `-12`; the llama at dx `13`, dy `-15`; the eagle at dx `18`, dy `-14`. The next honest action is a fresh screen decision: step closer, ask for a new context menu from the new distance, or back away. It is still not a travel segment, and it is still not a tame attempt.

## Run 192 - Still Too Far To Say Tame

I start at `Point3D(1195,1338,0)`, facing north, with no menu open. The horse is still the thing pulling my eye: `Point3D(1196,1326,0)`, dx `1`, dy `-12`. The llama and eagle are still visible up-screen, and the fox is close enough behind me at `Point3D(1196,1341,0)`. The screen is not empty, so I do this one tile at a time instead of calling it travel.

**Beat 1**

I press north.

`Mobile.Move` takes the real movement path because I am already facing `Direction.North`. `MovementImpl.CheckMovement` accepts `Point3D(1195,1337,0)`: server-order `map1.mul` says grass tile `0x6`, z `0`, `staidx1/statics1` has no statics there, and the live save has no mobile or item blocker on the tile.

The horse is closer, not usable: dx `1`, dy `-11`. The llama is dx `13`, dy `-14`; the eagle is dx `18`, dy `-13`. The fox is now stretched to dx `1`, dy `4`, so I pause before taking another step.

**Beat 2**

I wait for the fox to catch up once.

`BaseAI.Obey` goes through `DoOrderFollow` and `WalkMobileRange`. The remembered FriendsAvoidHeels band still wants distance `2..3`; the fox's floored distance is `4`, so `GetDirectionTo` chooses `Direction.North`. `DoMove/Mobile.Move` accepts `Point3D(1196,1340,0)`, another dry grass tile with zero statics and no saved blocker.

That puts the fox at dx `1`, dy `3`, back in the band. It still is not guarding, fighting, blocking, or protecting me.

**Beat 3**

I press north again.

The second player step also enters the real movement path and accepts `Point3D(1195,1336,0)`: dry grass tile `0x3`, z `0`, zero statics, no saved blocker. The shifted live rectangle still shows the same unresolved bodies and no items: horse dx `1`, dy `-10`, llama dx `13`, dy `-13`, eagle dx `18`, dy `-12`. The stale live JSONL fox row at `1209,1324` is still my simulated follower serial, not a second wild fox.

Mechanical friction learned:

- Two more north inputs over clear grass do not make the horse interactable. At dx `1`, dy `-10`, it still fails the visible `TameEntry` range `6`.
- The fox can follow one tile north to `1196,1340`, but the final player step stretches it back outside the heel band at floored distance `4`.
- No `ContextMenuRequest`, `ContextMenuResponse`, `TameEntry.OnClick`, `Taming.OnUse`, `Taming.InternalTarget`, skill timer, ownership, follower count, combat, loot, corpse, quest, discovery, hunger, or thirst path runs from walking closer.
- The map overlay still only gives navigation lures: `Ruins` southeast and `Mines of Morinia` west. They are not visible entities on this screen.

Next pressure:

Mira ends at `Point3D(1195,1336,0)`, facing north. The fox is at `Point3D(1196,1340,0)`, following but stretched again to dx `1`, dy `4`. The horse is visible at dx `1`, dy `-10`, still outside range `6`; the llama and eagle remain visible and unresolved. The next honest beat is probably another fox-follow wait or another cautious approach step, not a tame attempt and not routine travel.

## Run 193 - One Step Closer, Still Not Tame

I start at `Point3D(1195,1336,0)`, facing north, with the horse still almost straight ahead at `Point3D(1196,1326,0)`. It is visible, labeled from earlier, and still too far away. The llama and eagle are still in the same upper-screen cluster, and my fox is stretched behind me at `Point3D(1196,1340,0)`, following but loose. I do not pretend this is a road. I am creeping through animal pressure one tiny correction at a time.

**Beat 1**

I wait for the fox to catch up.

`BaseAI.Obey` follows the `Follow` order into `DoOrderFollow`, then `WalkMobileRange`. FriendsAvoidHeels still wants the fox in the 2..3 tile band. From dx `1`, dy `4`, the floored distance is `4`, so the fox needs one north step. `GetDirectionTo` resolves `Direction.North`, and `DoMove/Mobile.Move` accepts `Point3D(1196,1339,0)`: dry map1 grass tile `0x5`, z `0`, zero statics, and no saved blocker.

The fox is back in the band at dx `1`, dy `3`. It is still a follower, not a guard.

**Beat 2**

I press north once.

Because I am already facing north, this is real movement, not just a turn. `Mobile.Move` enters `CheckMovement` and accepts `Point3D(1195,1335,0)`: dry map1 grass tile `0x5`, z `0`, zero statics, and no saved mobile or item blocker.

The horse closes to dx `1`, dy `-9`. That is progress, but not interaction. The `Tame` context row uses range `6`, and I have not even requested a fresh context menu from this distance. The llama is dx `13`, dy `-12`; the eagle is dx `18`, dy `-11`; zero saved visible items are in the rectangle. The fox is stretched again to dx `1`, dy `4`, so I do not keep walking.

**Beat 3**

I wait for the fox again.

The same follow path runs. `WalkMobileRange` sees floored distance `4`, chooses `Direction.North`, and `DoMove/Mobile.Move` accepts `Point3D(1196,1338,0)`: dry map1 grass tile `0x6`, z `0`, zero statics, and no saved blocker. The fox returns to dx `1`, dy `3`.

Mechanical friction learned:

- Waiting can legally advance the controlled fox through the exact follow AI path, but it does not create Guard, Attack, Combatant, body-blocking, or protection state.
- The north step to `1195,1335` is legal dry Sosaria grass with zero statics and no saved blocker.
- Horse serial `200909` is closer at dx `1`, dy `-9`, but still outside the visible `TameEntry` range `6`. No context menu was requested, no row was selected, no target cursor opened, and `Taming.OnUse`/`Taming.InternalTarget` did not run.
- The llama and eagle remain visible unresolved wildlife. The stale live JSONL row for fox serial `200274` is still my simulated controlled follower, now at `1196,1338`, not a second wild fox.

Next pressure:

Mira ends at `Point3D(1195,1335,0)`, facing north. The fox is at `Point3D(1196,1338,0)`, following and inside the heel band. The horse is visible at dx `1`, dy `-9`; the llama at dx `13`, dy `-12`; the eagle at dx `18`, dy `-11`. The next honest action is another cautious approach step, a fresh context-menu check from the new distance, or retreat. It is still not a tame attempt and still not a travel segment.

## Run 194 - The Deer Joins The Problem

I start at `Point3D(1195,1335,0)`, facing north, with no menu open. The horse is still almost straight ahead at `Point3D(1196,1326,0)`, close enough to tempt me and still not close enough to touch the `Tame` row. The llama and eagle are still up-screen. My fox is tucked behind me at `Point3D(1196,1338,0)`, following and inside the heel band for the moment.

**Beat 1**

I press north once.

Because I am already facing north, this is a real movement attempt. `Mobile.Move` reaches `MovementImpl.CheckMovement`, and the server-order map probe accepts `Point3D(1195,1334,0)`: map1 grass tile `0x3`, average movement z `0`, zero statics, and no saved mobile or item blocker on the target.

The step changes the screen, so I stop. A deer I did not have on-screen before appears at the top edge: Hind serial `323473` at `Point3D(1203,1316,0)`, dx `8`, dy `-18`. The horse is now dx `1`, dy `-8`; that is closer, but the `TameEntry` range is still `6`, and I have not requested a fresh context menu anyway. The llama sits at dx `13`, dy `-11`; the eagle is still on the east edge at dx `18`, dy `-10`. The live item scan is still empty.

My fox did not get a follow tick after I moved. It stays at `Point3D(1196,1338,0)`, now dx `1`, dy `4`, outside the 2..3 heel band. That is a leash problem, not protection.

Mechanical friction learned:

- One north step to `1195,1334` is legal dry Sosaria grass with zero statics and no saved target blocker.
- A new visible deer stops the batch. Hind constructs as `AI_Animal` with `FightMode.Aggressor`, but current acquisition is negative because there is no aggressor, aggressed, faction, or ethic state involving me or the fox.
- Horse serial `200909` is closer at dx `1`, dy `-8`, but still outside `TameEntry` range `6`. No context menu, row selection, target cursor, `Taming.OnUse`, or `Taming.InternalTarget` ran.
- The fox is following, not guarding, and now needs a catch-up decision before I pretend I can keep creeping north.

Next pressure:

Mira ends at `Point3D(1195,1334,0)`, facing north. The controlled fox is at `Point3D(1196,1338,0)`, outside the heel band. The visible wildlife set is now deer, horse, llama, and eagle, with zero saved visible items and no open UI. The next honest action is to react to the newly visible deer, wait the fox back in, inspect a visible animal, or retreat. It is not a travel segment and not a tame attempt.

## Run 195 - I Let The Fox Catch Up

I start at `Point3D(1195,1334,0)`, facing north. The new deer is still on the top edge, the horse is still almost straight ahead, and the llama and eagle are still sitting in the same bad little screen full of options I cannot safely call plans. My fox is the one body I actually own, and after the last step it is stretched behind me at dx `1`, dy `4`. So I do not lunge for the horse menu or pretend the deer is solved. I wait.

**Beat 1**

The controlled fox takes its follow tick.

`BaseAI.Obey` follows the `Follow` order into `DoOrderFollow`, then `WalkMobileRange`. FriendsAvoidHeels is still using the remembered spacing value `7`, so the desired band is `2..3`. From `Point3D(1196,1338,0)` to me, the floored distance is `4`; `GetDirectionTo` chooses `Direction.North`, and `DoMove/Mobile.Move` accepts `Point3D(1196,1337,0)`. The tile is dry map1 grass with zero statics and no saved blocker.

The fox is back in the band at dx `1`, dy `3`. It is still a follower, not a guard.

**Beat 2**

I press north once.

Because I am already facing north, this is real movement. `Mobile.Move` enters `MovementImpl.CheckMovement` and accepts `Point3D(1195,1333,0)`: dry map1 grass, zero statics, and no saved mobile or item blocker on the target.

The horse is now dx `1`, dy `-7`. That is close enough to keep tempting me and still one tile outside the `TameEntry` range `6`. The deer is dx `8`, dy `-17`; the llama is dx `13`, dy `-10`; the eagle is dx `18`, dy `-9`. The screen does not gain a chest, corpse, water source, sign, shelter, road, gump, or target cursor.

The fox is stretched again at dx `1`, dy `4`, so I stop myself from chaining another step.

**Beat 3**

I wait for the fox again.

The same follow path runs: `BaseAI.Obey -> DoOrderFollow -> WalkMobileRange -> GetDirectionTo(Direction.North) -> DoMove/Mobile.Move`. The fox moves from `Point3D(1196,1337,0)` to `Point3D(1196,1336,0)`, another dry map1 grass tile with zero statics and no saved blocker. It ends at dx `1`, dy `3`, back inside the heel band.

Mechanical friction learned:

- A newly visible deer is screen pressure, not a reason to sprint. The first safe thing to resolve was the stretched controlled follower.
- One north step to `1195,1333` is legal dry Sosaria grass with zero statics and no saved target blocker.
- The horse is closer but still not usable: dx `1`, dy `-7` fails the visible context-entry range `6`, so no `ContextMenuRequest`, `ContextMenuResponse`, `TameEntry.OnClick`, `Taming.OnUse`, target cursor, skill timer, ownership, or follower-count path ran.
- The deer, horse, llama, and eagle are all visible unresolved wildlife. Current `FightMode.Aggressor` acquisition stays negative because no aggressor/aggressed/faction/ethic state involving me or the fox is committed.
- The fox follow ticks restore leash spacing only. They do not create Guard, Attack, Combatant, body-blocking, or protection.

Next pressure:

Mira ends at `Point3D(1195,1333,0)`, facing north. The controlled fox is at `Point3D(1196,1336,0)`, following and inside the heel band. The screen still contains Hind serial `323473`, Horse serial `200909`, RidableLlama serial `226037`, and Eagle serial `444927`, with zero saved visible items and no open UI. The horse is close but still outside the `Tame` row's range. The next honest action is a fresh visible-wildlife decision: ask for a new menu anyway, take one more cautious step, inspect the deer, or retreat. It is not routine travel and not a tame attempt.

## Run 196 - One Tile Makes The Horse Menu Real

I start at `Point3D(1195,1333,0)`, facing north, with no menu open. The horse is still the thing I want, sitting at `Point3D(1196,1326,0)`, dx `1`, dy `-7`. That is close enough to be annoying and still one tile outside the `Tame` row's range `6`. The deer, llama, and eagle are still visible, and my fox is in heel spacing at `Point3D(1196,1336,0)`.

**Beat 1**

I press north once.

Because I am already facing north, `PacketHandlers.MovementReq` reaches the real `Mobile.Move` path. `MovementImpl.CheckMovement` accepts `Point3D(1195,1332,0)`: Sosaria is map/file index `1`, the server-order `map1.mul` read uses `((blockX * blockHeight) + blockY)`, the target tile is dry grass `0x3` at z `0`, its average movement z is `0`, and `staidx1/statics1` has zero statics there. The live save has no mobile or item blocker on the destination.

The step matters immediately, so I stop. A second eagle appears at the top edge: Eagle serial `444945` at `Point3D(1196,1314,0)`, dx `1`, dy `-18`. The horse is now dx `1`, dy `-6`, which means a fresh context menu should be able to draw an enabled `Tame` row. That is not a tame attempt yet. I have not requested the menu, selected a row, opened a target cursor, or touched `Taming.InternalTarget`, whose true targeting range is only `2`.

My fox did not get a follow tick after the step. It stays at `Point3D(1196,1336,0)`, now dx `1`, dy `4`, outside the 2..3 heel band. The screen now holds two eagles, the deer, the horse, and the llama, plus the stale live JSONL fox row that I must keep overriding as my already-tamed follower.

Mechanical friction learned:

- One north step to `1195,1332` is legal dry Sosaria grass with zero statics and no saved target blocker.
- Moving one tile closer changes the horse from "visible but menu row out of range" to "close enough for the range-6 context row." That still does not run `ContextMenuRequest`, `ContextMenuResponse`, `TameEntry.OnClick`, `Taming.OnUse`, or the range-2 target gate.
- Eagle serial `444945` enters the inclusive 18-tile screen at the top edge, so this cannot be compressed as route travel.
- The fox is following, not guarding, and is now stretched to floored distance `4`.

Next pressure:

Mira ends at `Point3D(1195,1332,0)`, facing north. The controlled fox is at `Point3D(1196,1336,0)`, following but outside the heel band. The visible wildlife set is now Eagle `444945`, Hind `323473`, Horse `200909`, RidableLlama `226037`, Eagle `444927`, and the overridden controlled fox serial `200274`. The next honest action is a real decision: request the horse menu from range, wait the fox back in, inspect the new eagle, or back away. It is not a tame attempt until the visible menu path is actually used.

## Run 197 - The Tame Row Wakes Up

I start exactly where that one north step left me: `Point3D(1195,1332,0)`, facing north, with the horse at `Point3D(1196,1326,0)`. It is only six tiles away by the context-menu rule now. The screen is still crowded: the new eagle at the top edge, the deer, the llama, the old labeled eagle on the east edge, and my fox stretched behind me at dx `1`, dy `4`. I do not move again, and I do not let the fox tick. The horse is the visible thing I have been working toward, so I ask for its context menu.

**Beat 1**

The menu request is accepted.

`PacketHandlers.ContextMenuRequest` finds Horse serial `200909`, confirms we share `Map.Sosaria`, confirms I can see it, and passes `Utility.InUpdateRange`. `ContextMenu` asks the horse for entries. `BaseCreature.GetContextMenuEntries` adds `Tame` because the horse is tamable, uncontrolled, and I am alive. This time `DisplayContextMenu` does not gray the row out: `TameEntry` has range `6`, and I am at dx `1`, dy `-6`.

That is still not a tame. I do not select the row. No `ContextMenuResponse` runs, so `TameEntry.OnClick` does not lock targeting, `Taming.OnUse` does not assign the range-2 target, and `Target.Invoke` does not reject or accept the horse. The fox stays behind me at `Point3D(1196,1336,0)`, still outside the heel band. The same five wild animals remain visible, and zero saved items are in range.

Mechanical friction learned:

- Opening the horse menu is a separate normal-client action from selecting `Tame`.
- The range-6 context entry is now genuinely enabled for Horse serial `200909`, unlike the earlier disabled horse menus at longer range.
- The true taming target gate is still `Taming.InternalTarget` range `2`; the current dx `1`, dy `-6` would not satisfy that if I selected the row next.
- No movement, pet follow tick, skill use, target cursor, taming timer, ownership, follower-count, combat, item, hunger, thirst, quest, discovery, fame, karma, or PvP/PvE state changed.

Next pressure:

The horse context menu is open with a visible enabled `Tame` row. The honest next action is a UI decision: select `Tame` and probably learn the range-2 failure again, close/ignore the menu, or step closer only after dealing with the open menu and the stretched fox. This is not a travel segment.

## Run 198 - The Menu Lied By Six Tiles

I start frozen in the same grass tile, `Point3D(1195,1332,0)`, facing north. The horse menu is already open, and this matters more than the fox being loose behind me. The row says `Tame`. It is not grey anymore. A normal player does not know the hidden skill target is shorter than the context row, so I click the visible `Tame` entry.

**Beat 1**

The row click is real, but it does not start taming.

`PacketHandlers.ContextMenuResponse` clears the stored context menu, finds the same Horse serial `200909`, checks the response index, and accepts the row because `TameEntry` uses range `6`. `TameEntry.OnClick` locks targeting, suppresses the normal "Tame which animal?" prompt, and calls `UseSkill(Taming)`. `Taming.OnUse` creates an `InternalTarget`, but `TameEntry` immediately invokes it on the horse.

That target has range `2`. From my tile the horse is still dx `1`, dy `-6`, so `Target.Invoke` stops before `Taming.OnTarget` and sends `500446`: `That is too far away.` The target is cleared, `NextSkillTime` is reset to now by `InternalTarget.OnTargetFinish`, and the menu is gone. No taming timer starts. No skill check rolls. No skill gain happens. The horse is still wild.

The screen is otherwise the same crowded animal problem: the top-edge eagle, the deer, the horse, the llama, the east-edge eagle, and my controlled fox at dx `1`, dy `4`, still following but outside the heel band. I stop there, because the visible message and the closed menu force a new decision: step closer, wait the fox in, retreat, or inspect something else.

Mechanical friction learned:

- The enabled `Tame` context row is only a range-6 menu gate. It is not proof that the actual taming target can reach.
- Selecting that row immediately runs the range-2 `Taming.InternalTarget` path against the same animal. From dx `1`, dy `-6`, the result is the normal "That is too far away" message.
- Because the target fails range before `Taming.OnTarget`, the horse's `MinTameSkill` branch, ownership, follower slots, taming timer, combat anger, skill gain, and pet state are untouched.
- The row click consumes the open context menu. There is no open target cursor afterward because `Target.Invoke` clears the target during the immediate failed invoke.
- The fox is still a follower, not a guard, and remains stretched outside the 2..3 FriendsAvoidHeels band.

Next pressure:

Mira ends at `Point3D(1195,1332,0)`, facing north, with no open gump, no open context menu, and the horse still visible six tiles away. The next honest move is a new screen decision: wait the fox back into heel range, take one careful step closer before trying the horse again, or back out of the animal cluster. It is still not a travel segment and still not a tame attempt.

## Run 199 - The Goat At The Edge Breaks The Plan

I start where the failed horse click left me: `Point3D(1195,1332,0)`, facing north. The horse is still only six tiles away, which is close enough for the context menu to lie to me and too far for the real taming target. The fox is stretched behind me at `Point3D(1196,1336,0)`, and the screen already has too many animals in it: the top-edge eagle, the deer, the horse, the llama, and the east-edge eagle. So I do the boring thing first and let my one tame creature get back into place.

**Beat 1**

I wait for the fox to follow.

`BaseAI.Obey` goes through `DoOrderFollow` and `WalkMobileRange`. FriendsAvoidHeels is still using the remembered `FollowersMax` value `7`, so the fox wants the `2..3` tile band. From `Point3D(1196,1336,0)` to me, the floored distance is `4`; `GetDirectionTo` resolves north, and `DoMove/Mobile.Move` accepts `Point3D(1196,1335,0)`. The tile is dry map1 grass `0x3`, z `0`, with zero statics and no saved blocker.

That puts the fox at dx `1`, dy `4` by raw offset but floored distance `3`, inside the remembered heel band. It is still only following. No guard order, combat target, body block, ownership change, follower count change, or protection appears.

**Beat 2**

I press north once, trying to close the real horse range instead of pressing the same bad menu again.

Because I am already facing north, `Mobile.Move` enters `CheckMovement`. The target `Point3D(1195,1331,0)` decodes as dry grass `0x5`, z `0`, with no Impassable/Wet flag and zero statics, and the live save has no mobile or item blocker on that tile. The step succeeds.

The horse is closer at dx `1`, dy `-5`, but still not close enough for `Taming.InternalTarget` range `2`. More importantly, the shifted screen exposes a new goat at the north edge: Goat serial `200925` at `Point3D(1208,1313,0)`, dx `13`, dy `-18`. That stops me. The screen now has goat, eagle, deer, horse, llama, and eagle, with zero visible saved items and no visible running spawner object. My fox did not get another follow tick after the player step, so it sits at `Point3D(1196,1335,0)`, dx `1`, dy `4`, outside the 2..3 band again.

Mechanical friction learned:

- The fox follow wait is a real timer beat, not free protection. It only moves the controlled fox one north tile and restores spacing before I move.
- The north step to `1195,1331` is legal dry Sosaria grass with zero statics and no saved blocker.
- Horse serial `200909` is still outside the true taming target range: dx `1`, dy `-5` fails the range-2 `InternalTarget` even though the range-6 context row would be available.
- Goat serial `200925` enters the visible update rectangle at the top edge after the step. That is a new player-facing animal decision, so this cannot continue as routine movement.

Next pressure:

Mira ends at `Point3D(1195,1331,0)`, facing north. The controlled fox is at `Point3D(1196,1335,0)`, following but stretched back outside the heel band after Mira's step. Visible wildlife now includes the goat at dx `13`, dy `-18`, eagle at dx `1`, dy `-17`, deer at dx `8`, dy `-15`, horse at dx `1`, dy `-5`, llama at dx `13`, dy `-8`, and eagle at dx `18`, dy `-7`. No menu, target cursor, gump, item, corpse, combat, skill roll, ownership change, or discovery is open. The next honest action is another visible-screen decision: wait the fox again, step closer, inspect the new goat/eagle cluster, or back away.

## Run 200 - Still Too Far From The Horse

I start at `Point3D(1195,1331,0)`, facing north. The goat that broke the last run is still on the top edge, the eagle above the horse is still there, the deer and llama are still in the same crowded strip, and the horse is close enough to keep dragging my eyes back to it. But my fox is behind me at `Point3D(1196,1335,0)`, outside the heel band again. I do not open the horse menu from five tiles away and repeat the same hidden range mistake. I let the pet catch up first.

**Beat 1**

I wait for the controlled fox.

`BaseAI.Obey` follows the `Follow` order into `DoOrderFollow`, then `WalkMobileRange`. FriendsAvoidHeels still wants the `2..3` tile band. From `Point3D(1196,1335,0)`, the fox is floored distance `4` from me, so `GetDirectionTo` resolves north and `DoMove/Mobile.Move` accepts `Point3D(1196,1334,0)`. The tile is dry map1 grass `0x3`, z `0`, with zero statics and no saved blocker.

The fox is back inside the band at dx `1`, dy `3`. It is following, not guarding.

**Beat 2**

I press north once.

Because I am already facing north, this is movement, not just a turn. `Mobile.Move` enters `MovementImpl.CheckMovement` and accepts `Point3D(1195,1330,0)`: dry map1 grass `0x3`, z `0`, zero statics, and no saved mobile or item blocker on the target.

The horse is now dx `1`, dy `-4`. That is still outside the true `Taming.InternalTarget` range `2`, even though the context row would be easy to wake up. The goat, top eagle, deer, llama, and east-edge eagle remain visible; zero saved items or visible running spawner objects appear. The live JSONL row for fox serial `200274` would be in the rectangle at its old saved tile, but the simulation already owns and moves that serial, so I keep the override instead of inventing a second fox.

The player step stretches my fox again to floored distance `4`, so I stop walking.

**Beat 3**

I wait for the fox again.

The same follow path runs. `WalkMobileRange` sees the fox outside the desired band, `GetDirectionTo` resolves north, and `DoMove/Mobile.Move` accepts `Point3D(1196,1333,0)`. The tile is dry map1 grass `0x4`, z `0`, with zero statics and no saved blocker. The fox ends at dx `1`, dy `3`, back in the band.

Mechanical friction learned:

- Cautious approach is still a pet-leash rhythm, not a travel segment. Each player step toward the horse stretches the fox; each wait only restores follow spacing.
- The north step to `1195,1330` is legal dry Sosaria grass with zero statics and no saved blocker.
- Horse serial `200909` is closer, but dx `1`, dy `-4` still fails the true range-2 `Taming.InternalTarget`. No context menu, row response, target cursor, taming timer, skill roll, ownership, follower-count, mount, combat, fame, karma, quest, or discovery path ran.
- The goat, eagles, deer, horse, and llama are visible unresolved wildlife. Their current `FightMode.Aggressor` acquisition remains negative because no aggressor/aggressed/faction/ethic state involving me or the fox is committed.
- The controlled fox is still only following. No Guard, Attack, Combatant, Warmode, body-blocking, or protection state appears.

Next pressure:

Mira ends at `Point3D(1195,1330,0)`, facing north. The controlled fox is at `Point3D(1196,1333,0)`, following and inside the heel band. Visible wildlife now includes the goat at dx `13`, dy `-17`, eagle at dx `1`, dy `-16`, deer at dx `8`, dy `-14`, horse at dx `1`, dy `-4`, llama at dx `13`, dy `-7`, and eagle at dx `18`, dy `-6`. No menu, target cursor, gump, item, corpse, combat, skill roll, ownership change, or discovery is open. The next honest move is still a visible-screen decision: step closer, request another horse menu knowing it will not reach the true target, inspect the goat/eagle cluster, or back away.

## Run 201 - Finally Inside The Real Horse Range

I start at `Point3D(1195,1330,0)`, facing north. The horse is four tiles north by y, so the visible context menu would still lie to me if I asked for `Tame` right now. The fox is close at `Point3D(1196,1333,0)`, and the whole top of the screen is still animal noise: goat, eagle, deer, horse, llama, and the east-edge eagle. I keep moving carefully because every north step stretches the fox again.

**Beat 1**

I press north once.

`Mobile.Move` takes the real movement path because I am already facing north. `MovementImpl.CheckMovement` accepts `Point3D(1195,1329,0)`: map1 grass `0x5`, z `0`, zero statics, no saved mobile blocker, and no saved item blocker. The horse shifts to dx `1`, dy `-3`. That is still outside the true `Taming.InternalTarget` range `2`, even though the range-6 context row would be available.

The fox does not get a free step during my movement. It is now dx `1`, dy `4`, outside the remembered 2..3 heel band. The visible saved animal set is unchanged except for offsets, and the old live fox row is still overridden by my controlled follower state.

**Beat 2**

I wait for the fox.

The controlled fox follows through `BaseAI.Obey -> DoOrderFollow -> WalkMobileRange`. FriendsAvoidHeels still wants the 2..3 band. From `Point3D(1196,1333,0)`, the fox is floored distance `4`, so it takes a north step to `Point3D(1196,1332,0)`. That tile is map1 grass `0x5`, z `0`, with zero statics and no saved blocker. It is back inside the heel band, still only following.

**Beat 3**

I press north again.

`Mobile.Move` accepts `Point3D(1195,1328,0)`: map1 grass `0x3`, z `0`, zero statics, and no saved mobile or item blocker. Now the horse is at dx `1`, dy `-2`. For the first time in this approach, the real `Taming.InternalTarget` range box would reach it if I opened the horse menu and selected `Tame`.

That is not a tame attempt. I do not open the menu yet, and no `ContextMenuRequest`, `ContextMenuResponse`, `TameEntry.OnClick`, `Taming.OnUse`, `Target.Invoke`, skill timer, ownership, follower-count, or mount path runs.

The step also exposes a new top-edge eagle: Eagle serial `226270` at `Point3D(1198,1310,0)`, dx `3`, dy `-18`. The rest of the visible animals remain: goat dx `13`, dy `-15`; eagle dx `1`, dy `-14`; deer dx `8`, dy `-12`; horse dx `1`, dy `-2`; llama dx `13`, dy `-5`; and the east-edge eagle dx `18`, dy `-4`. There are still zero visible saved items and no visible running spawner object. The fox is stretched again at dx `1`, dy `4`.

Mechanical friction learned:

- Two more north steps are legal dry Sosaria grass with zero statics and no saved blockers.
- Horse serial `200909` is finally inside the true range-2 `Taming.InternalTarget` box from `Point3D(1195,1328,0)`.
- Being in target range is only a precondition. No horse menu or taming target has been requested this run.
- The new eagle at the top edge is a fresh visible animal, so I stop before treating the next action as routine.
- The fox is again outside the heel band after the final player step. It remains Follow-only, not Guard, Attack, body-blocking, or protection.

Next pressure:

Mira ends at `Point3D(1195,1328,0)`, facing north. Horse serial `200909` is finally close enough for the hidden range-2 target, but the fox is stretched behind at `Point3D(1196,1332,0)`, and a new unlabeled eagle has appeared on the top edge. No menu, target cursor, gump, combat, damage, skill roll, ownership change, or discovery is open. The next honest action is a screen decision: wait the fox, inspect the new eagle, or try the now-legitimate horse menu path.

## Run 202 - The Horse Menu Is Real Now

I start at `Point3D(1195,1328,0)`, facing north, with the horse finally close enough to matter for the hidden target range. The screen is not quiet: the new top-edge eagle is still above me, with the goat, another eagle, deer, horse, llama, and east-edge eagle all in the same 18-tile rectangle. But the fox is stretched behind me at `Point3D(1196,1332,0)`, outside the heel band, and I do not want to start another animal interaction with my only pet lagging.

**Beat 1**

I wait for the fox.

`BaseAI.Obey` follows the `Follow` order into `DoOrderFollow`, then `WalkMobileRange`. FriendsAvoidHeels still uses the remembered `FollowersMax` value `7`, so the desired leash is 2..3 tiles. From `Point3D(1196,1332,0)`, the fox is floored distance `4`, so it steps north to `Point3D(1196,1331,0)`. The tile is map1 grass `0x6`, z `0`, with zero statics and no saved blocker. The fox is back inside the band at dx `1`, dy `3`. It is still following, not guarding.

**Beat 2**

Now I ask for the horse's context menu.

The request passes the normal screen gates. The horse is on the same map, visible, and inside the update rectangle at dx `1`, dy `-2`. `BaseCreature.GetContextMenuEntries` adds `Tame` because the horse is tamable, uncontrolled, and I am alive. This time the menu row is not just range-6 close; it is also close enough that the later range-2 target would be reachable if I choose it.

I stop there. The context menu is open with `Tame` visible and enabled, but I do not select it. No `ContextMenuResponse`, `TameEntry.OnClick`, `Taming.OnUse`, `Target.Invoke`, `Taming.OnTarget`, skill timer, skill gain, ownership, follower-count, mount, combat, corpse, loot, quest, discovery, hunger/thirst, fame, karma, or PvP/PvE path runs.

Mechanical friction learned:

- Waiting the fox is an exact pet AI beat. It restores the leash to the 2..3 band, but it does not create Guard, Attack, body-blocking, or protection.
- The fox follow target `1196,1331` is dry Sosaria grass with zero statics and no saved blocker.
- Horse serial `200909` now exposes an enabled `Tame` row from inside both the range-6 context gate and the range-2 taming target geometry.
- Opening a context menu is not selecting a row. The next state is an open UI decision, not taming progress.

Next pressure:

Mira remains at `Point3D(1195,1328,0)`, facing north. The fox is at `Point3D(1196,1331,0)`, following and back inside the heel band. The horse context menu is open with a visible enabled `Tame` row. The surrounding animals remain unresolved visible wildlife, and the old off-screen toad, Urulg, GoblinArcher, and swamp-drake branches are still risk evidence rather than safety. The next honest action is to choose `Tame`, dismiss the menu, or change my mind before touching another key.

## Run 203 - The Horse Does Not Even Start

I start at `Point3D(1195,1328,0)`, facing north, with the horse menu already open. The screen is still packed with animals: the horse at dx `1`, dy `-2`, goat, deer, llama, three eagles, and my fox standing behind me inside the heel band. The menu row says `Tame`, it is not grey, and this time the horse is close enough for the hidden range-2 target. So I click it.

**Beat 1**

The menu disappears, and the real wall is not range anymore. `ContextMenuResponse` clears the stored menu, confirms the same horse, and accepts the enabled range-6 row. `TameEntry.OnClick` locks targeting, suppresses the normal "Tame which animal?" prompt, runs the Taming skill, and immediately invokes the horse through `Taming.InternalTarget`. From here the range-2 target reaches the horse.

Then the skill gate hits. The horse is tamable and uncontrolled, I am alive, the follower slots would fit, and the horse does not need to be subdued. But my Taming is only `0.5`. A normal horse wants `29.1`, and the rare branch would want `49.1`; either way I am nowhere close. `CheckMastery` does not help because it only covers wolf-family cases through a dark wolf familiar. The horse sends the overhead message: `You have no chance of taming this creature.`

That means there is no taming timer. No anger roll, no `m_BeingTamed` entry, no `InternalTimer`, no Druidism gain, no Taming gain, no owner list mutation, no follower-count change, no mount, no combat, no corpse, no loot, and no movement. The fox is still following, not guarding. The horse is still wild.

Mechanical friction learned:

- The range problem is solved only to reveal the skill problem. A visible enabled `Tame` row plus range-2 target access still cannot start taming if `Taming.Value < MinTameSkill`.
- Horse serial `200909` remains a live saved wild mobile. Its exact normal-versus-rare MinTameSkill branch is still not exported, but both possible branches are above Mira's `0.5` Taming, so the failure is deterministic.
- `Taming.OnTarget` checks the skill threshold before the anger branch and before creating `m_BeingTamed`, so the failed horse attempt does not provoke the horse and does not start a waitable taming timer.
- The context menu is consumed. There is no open context menu, no target cursor, no gump, and no active skill timer afterward because `InternalTarget.OnTargetFinish` resets `NextSkillTime` on this failure path.

Next pressure:

Mira remains at `Point3D(1195,1328,0)`, facing north. The controlled fox remains at `Point3D(1196,1331,0)`, following and inside the heel band. The horse is still visible two tiles north and now clearly too difficult, while goat, deer, llama, and three eagles crowd the same screen. The next honest decision is not another immediate horse click; it is whether to back away, inspect another animal, or go find a trainer, road, water, or shelter.

## Run 204 - Backing Out Of The Horse Trap

I start where the failed horse attempt left me: `Point3D(1195,1328,0)`, facing north, no menu open and no target cursor. The horse is still close enough to touch, but the last click proved the wall is not range. My `Taming` is `0.5`; the horse wants `29.1` or the rare `49.1`. The other visible animals are not starter pets either: goat wants `11.1`, eagle wants `17.1`, deer wants `23.1`, and llama wants `29.1`. The screen is an animal cluster, not an opportunity.

**Travel Segment - Beat 1**

I turn south and walk away.

The first south input is only a facing change because `Mobile.Move` does not call `CheckMovement` until my current direction mask already matches the requested direction. After that, twelve accepted south inputs carry me from `1195,1328` to `1195,1340`. This is not new terrain. The same column was already proved in reverse during the horse approach: dry `map1.mul` grass, zero statics on the accepted target tiles, and no saved mobile or item blockers on `x=1195,y=1329..1340`.

I do not claim exact fox AI ticks during compressed travel. The fox is only shadowed under the travel policy from `Point3D(1196,1331,0)` to about `Point3D(1196,1343,0)`, keeping the familiar 2..3 tile band. No Guard, Attack, body-blocking, combat, ownership, follower-count, pet-order, skill, hunger, thirst, quest, region, gump, context menu, or target cursor changes.

The endpoint screen is thinner but not empty. I can still see the horse at dx `1`, dy `-14`, the llama at dx `13`, dy `-17`, and the east-edge eagle at dx `18`, dy `-16`. The old live fox row is still overridden by my controlled follower. There are zero saved visible items and zero visible running spawner objects.

**Travel Segment - Beat 2**

I keep backing south until the animal cluster finally falls off the screen.

From `Point3D(1195,1340,0)`, already facing south, eleven more accepted inputs reach `Point3D(1195,1351,0)`. The route reuses proved dry grass evidence from the earlier north escape and horse approach; the live snapshot has no saved mobile or item blocker on `x=1195,y=1341..1351`. The fox is again approximate, shadowed to about `Point3D(1196,1354,0)` and still only following.

At `Point3D(1195,1351,0)`, the immediate screen finally has zero saved wild mobiles, zero saved visible items, and zero visible running spawner objects. That is not safety. It only means the horse, llama, eagle, goat, deer, toads, crane, monkey, panther, Urulg, GoblinArcher, and the old drake branch are off-screen or carried risk rather than clickable bodies. The client map overlay is now the useful pressure: South moongate is about `107` tiles east-southeast, Ruins about `146` tiles southeast, Mines of Morinia about `174` tiles west, and West moongate about `262` tiles southwest.

Mechanical friction learned:

- A failed horse `Tame` row should change behavior. Re-clicking it is blocked by the same skill gate until real Taming gain or training happens.
- Travel can be compressed only because the south column was already mechanically probed and the live snapshot showed no route blockers.
- Follower shadowing during travel is approximate state maintenance, not proof that `BaseAI.Obey` timers fired.
- The endpoint quiet screen is just a visibility fact. It does not despawn or pacify any earlier creature, and it does not create a road, water source, trainer, shelter, discovery flag, or safe region.

Next pressure:

Mira ends at `Point3D(1195,1351,0)`, facing south. The controlled fox is approximately at `Point3D(1196,1354,0)`, following inside the heel band. No entity is currently visible in the 18-tile client rectangle, but the next move is a real navigation decision: choose whether to angle toward the South moongate/Ruins overlay, head back west toward Mines of Morinia, or continue scouting for water, road, shelter, or a trainer.

## Run 205 - East Until The Monkey Moves Back Onto Screen

I start at `Point3D(1195,1351,0)`, facing south, with no menu, target cursor, gump, corpse, chest, road, water source, sign, or saved wild mobile in the 18-tile screen. Before I move, the client overlay gets corrected: the tempting `South` moongate row at `1300,1372` is not a Sosaria marker. It is in `Moongates.csv` on map `3`, so it is not current-map navigation knowledge. On Sosaria, the nearest real overlay lure east/southeast is `Ruins` at `1303,1449`; `Mines of Morinia` is back west, and `West` moongate is much farther southwest.

**Travel Segment - Beat 1**

I turn east and walk twelve steps.

The first east input is only a facing change. `Mobile.Move` does not run `MovementImpl.CheckMovement` until my current direction mask already matches the requested direction. After that, twelve accepted east inputs carry me from `Point3D(1195,1351,0)` to `Point3D(1207,1351,0)`.

The route is plain Sosaria grass under the server-order `map1.mul` read, with zero statics on the accepted target tiles and no saved mobile or item blockers on the route. I shadow the controlled fox approximately from `Point3D(1196,1354,0)` to about `Point3D(1205,1354,0)`, keeping the follow order coherent without claiming exact pet AI ticks. At the endpoint, the live-state rectangle still has zero saved wild mobiles, zero saved items, and zero visible running spawner object locations.

**Travel Segment - Beat 2**

I keep walking east.

Because I am already facing east, twelve more accepted east inputs carry me from `Point3D(1207,1351,0)` to `Point3D(1219,1351,0)`. The same movement path applies: `MovementReq -> Mobile.Move -> MovementImpl.CheckMovement`, cardinal target tiles only, dry grass, zero statics, and no saved route blockers. The fox is only approximate again, shadowed to about `Point3D(1217,1354,0)`.

The endpoint scan is still quiet: no saved visible wild mobiles, no saved visible items, and no visible running spawner object locations in the client rectangle.

**Travel Segment - Beat 3**

I take the third eastward segment, and the screen finally changes.

Twelve accepted east inputs carry me to `Point3D(1231,1351,0)`, still on dry grass with no blocking statics and no route blockers. The fox shadows to about `Point3D(1229,1354,0)`, inside the 2..3 tile follow band.

Now a saved animal appears in the 18-tile box: Monkey serial `206380`, `a monkey`, at `Point3D(1245,1366,0)`, dx `14`, dy `15`. The monkey is not combat yet. It constructs as `AI_Animal` with `FightMode.Aggressor`, has no special timer, and the current committed state has no aggressor/aggressed/faction/ethic relationship involving me or the fox, so the acquisition gate is negative. But it is visible, inside the client rectangle, inside the broad movement-notice rectangle, and inside the remapped `RangePerception` rectangle. That is enough to stop routine travel.

Mechanical friction learned:

- The earlier South moongate lure was a marker-filter mistake. The `South` moongate row near `1300,1372` is map `3`, not current Sosaria map `1`.
- The east row from `x=1196` through `x=1231`, `y=1351`, is mechanically walkable dry `map1` grass with no blocking statics or saved live route blockers in the snapshot.
- Three Travel Segment beats can move the player east only because no unread UI, target cursor, active combat, visible interactable, or active high-risk timer pressure exists before the segment endpoints.
- The monkey is a normal-screen stop. It does not grant discovery, combat, loot, taming progress, safety, or a route unlock. It only forces the next player decision.

Next pressure:

Mira ends at `Point3D(1231,1351,0)`, facing east. The controlled fox is approximately at `Point3D(1229,1354,0)`, following only. Monkey serial `206380` is visible at `Point3D(1245,1366,0)`, dx `14`, dy `15`; the current classifier is timer-negative, not safe. No item, gump, context menu, target cursor, combat, damage, ownership change, follower-count change, hunger/thirst tick, quest, region, discovery, loot, corpse, fame, karma, or PvP/PvE mutation ran. The next honest action is to inspect, avoid, or route around the monkey before treating eastward movement as routine again.

## Run 206 - I Make The Monkey Real On Screen

I start exactly where the eastward walking stopped: `Point3D(1231,1351,0)`, facing east, with the fox close behind at about `Point3D(1229,1354,0)`. The screen has one real wild body on it, Monkey serial `206380`, at `Point3D(1245,1366,0)`, dx `14`, dy `15`. I do not keep walking just because the current Aggressor check is quiet. I click the animal.

**Beat 1**

`PacketHandlers.LookReq` takes the serial, finds the mobile, and checks the ordinary screen gates. The monkey is visible, `CanSee` passes, and `Utility.InUpdateRange` accepts dx `14`, dy `15` inside the inclusive 18-tile rectangle. The region allows the single-click. `BaseCreature.OnSingleClick` adds no `(tame)` or `(bonded)` line because this monkey is uncontrolled. `Mobile.OnSingleClick` sends only the private overhead label: `a monkey`.

That is all I get. No context menu opens, no target cursor appears, no `Taming.OnUse`, no combat, no corpse, no banana, no pet AI tick, no movement, no hunger or thirst change, no quest, no discovery flag, and no route unlock. The fox does not become a guard dog. The monkey is just named now.

Mechanical friction learned:

- A LookReq click is a label, not an interaction contract. It proves the monkey is a screen-visible mobile and exposes its overhead name, but it does not start taming, fighting, looting, or movement.
- The monkey still constructs as `AI_Animal` with `FightMode.Aggressor`; the current acquisition gate remains negative because no aggressor/aggressed/faction/ethic state involving me or the fox exists.
- The endpoint scan is unchanged: one visible saved wild mobile, zero saved visible items, and zero visible running spawner object locations in `x=1213..1249,y=1333..1369`.
- The future AI timer, wandering, context-menu branch, taming branch, corpse/banana path, and route choice all remain unresolved.

Next pressure:

Mira remains at `Point3D(1231,1351,0)`, facing east. The controlled fox remains approximately at `Point3D(1229,1354,0)`, following only and inside the heel band. Monkey serial `206380` is still visible and now labeled at dx `14`, dy `15`. No UI is open. The next honest decision is to avoid it, open its context menu, or route around it; routine travel is still blocked by the visible animal.

## Run 207 - The Monkey Menu Lies With Gray Ink

I start at `Point3D(1231,1351,0)`, still facing east. The monkey I clicked last run is still there at `Point3D(1245,1366,0)`, dx `14`, dy `15`. The fox is close behind me at about `Point3D(1229,1354,0)`, following only. I already know the animal's name, so I try the next normal thing a player can do without moving: open its context menu.

**Beat 1**

The request is legal, but the useful row is not.

`PacketHandlers.ContextMenuRequest` finds Monkey serial `206380`, checks that it is on my map, visible, and inside `Utility.InUpdateRange`. A `ContextMenu` is built for the target. `BaseCreature.GetContextMenuEntries` adds `Tame` because the monkey is tamable, uncontrolled, and I am alive. `TameEntry` uses localization `6130` and range `6`.

Then the client packet decides what my hand can actually use. `DisplayContextMenu` compares my location to the monkey with that entry range. At dx `14`, dy `15`, range `6` fails, so the visible `Tame` row is gray/disabled. I can see the idea of taming it, but I cannot press that row from here.

That stops me. I do not select a row, and I do not forge a response. No `ContextMenuResponse`, no `TameEntry.OnClick`, no `UseSkill(Taming)`, no target cursor, no `Taming.InternalTarget`, no skill roll, no timer, no ownership change, no follower-count change, no combat, no movement, no corpse, no banana, no loot, and no pet AI tick runs.

Mechanical friction learned:

- A visible animal inside the 18-tile screen can still be too far for its context-menu action.
- The monkey is easier than the horse on paper: `Monkey.MinTameSkill` is `5.9`. That does not matter while the only `Tame` row is disabled by range `6`.
- Opening a context menu is a UI state, not progress. The next action is to close/ignore it, move closer, or back away.
- The monkey remains `AI_Animal/FightMode.Aggressor` with negative current Aggressor acquisition because no aggressor/aggressed/faction/ethic state involving me or the fox exists. That is not safety, just current negative pressure evidence.

Next pressure:

Mira remains at `Point3D(1231,1351,0)`, facing east. The monkey context menu is open on serial `206380` with a visible disabled `Tame` row. The monkey is still visible at dx `14`, dy `15`; the fox remains approximately at `Point3D(1229,1354,0)` inside the heel band, following only. No gump, target cursor, combat, damage, hunger/thirst tick, item, quest, discovery, skill gain, ownership, follower-count, or movement state changed. The next honest action is a UI decision before any travel: close the menu, step closer, or step away.

## Run 208 - Three Steps Toward The Monkey, Then Teeth

I start at `Point3D(1231,1351,0)`, facing east, with the monkey's context menu still hanging open. The row says `Tame`, but it is gray. I cannot pretend I clicked it, and I cannot turn a disabled row into progress. I close the menu in my head by clicking away; no row response goes to the server.

**Beat 1**

I dismiss the open monkey menu.

There is no `ContextMenuResponse`, no selected row index, and no `TameEntry.OnClick`. The only server proof I have is the path I am not taking: `PacketHandlers.ContextMenuResponse` would clear `from.ContextMenu`, verify the stored target, read the row index, and invoke only an enabled entry still within range. I do none of that. The visible UI is gone, but the monkey is still standing there at dx `14`, dy `15`.

**Travel Segment - Beat 2**

I try to close distance southeast.

The first southeast input only turns me from `Direction.East` to `Direction.Down`. Then three accepted southeast movement inputs carry me to `Point3D(1234,1354,0)`. `Mobile.Move` reaches `MovementImpl.CheckMovement` for the three real steps, and the server-order `map1.mul`/`staidx1.mul`/`statics1.mul` read shows the forward and diagonal side-check tiles are dry grass with zero statics:

- `1232,1352`: grass `0x4`, z `0`
- `1233,1353`: grass `0x3`, z `0`
- `1234,1354`: grass `0x4`, z `0`

The route has no saved live mobile or item blocker. I do not advance an exact fox AI tick; I only shadow the following fox from about `Point3D(1229,1354,0)` to about `Point3D(1231,1356,0)`, keeping the old heel band without claiming Guard, Attack, body-blocking, or a real `BaseAI.Obey` timer.

Then the screen changes. Panther serial `289483` appears at `Point3D(1252,1371,0)`, dx `18`, dy `17`. The original monkey is still visible at dx `11`, dy `12`; it is closer, but still outside the range-6 menu row. The panther is not a special-timer creature, but it is a much nastier `AI_Animal/FightMode.Aggressor` body than the monkey, and it is now legally on my screen. I stop.

Mechanical friction learned:

- A disabled context menu row can be dismissed locally without sending a legal server response, but that is not taming progress.
- Approaching the monkey from this angle exposes the panther after only three accepted southeast steps, long before the monkey's `Tame` row becomes usable.
- The route to `1234,1354` is mechanically walkable dry Sosaria grass with no statics and no saved route blockers.
- The monkey remains too far for context-menu taming from the new point: dx `11`, dy `12` still fails range `6`.
- Panther pressure now outranks the monkey experiment. No combat, damage, taming, loot, corpse, discovery, region, hunger/thirst, ownership, follower-count, or pet-order mutation has run.

Next pressure:

Mira ends at `Point3D(1234,1354,0)`, facing southeast. The controlled fox is approximately at `Point3D(1231,1356,0)`, following only and inside the heel band. The visible saved wildlife is Monkey serial `206380` at dx `11`, dy `12` and Panther serial `289483` at dx `18`, dy `17`; zero visible saved items and zero visible running spawner object locations were found. The next honest action is no longer "keep walking toward the monkey." It is react to the panther: inspect it, retreat, or choose a safer line.

## Run 209 - The Cat Gets A Name

I start at `Point3D(1234,1354,0)`, facing southeast, with the monkey still down the slope at dx `11`, dy `12` and the new cat shape on the edge at dx `18`, dy `17`. The monkey is still too far for the `Tame` row; the panther is not a special-timer monster, but it is a wild `AI_Animal/FightMode.Aggressor` feline with enough teeth that I do not keep walking toward it.

**Beat 1**

I single-click the panther.

The click is legal because the panther is inside the inclusive 18-tile update rectangle. `LookReq` finds Panther serial `289483`, checks `CanSee`, checks `Utility.InUpdateRange`, lets the region approve the single-click, then calls the mobile's click path. It is uncontrolled, so there is no `(tame)` or `(bonded)` line from `BaseCreature.OnSingleClick`. The ordinary mobile click prints the overhead label: `a panther`.

That is all. I do not open a context menu, do not choose `Tame`, do not target, do not swing, do not move, do not order the fox, and do not wait for any AI timer. The panther's current Aggressor acquisition still has no committed aggressor/aggressed/faction/ethic state involving me or the fox, and at dx `18`, dy `17` it is outside effective `RangePerception 16`; that is negative current target evidence, not safety. The monkey remains visible and still outside range `6`.

Mechanical friction learned:

- A visible edge animal can be identified with a normal single-click without mutating combat, taming, inventory, discovery, or movement state.
- Panther serial `289483` is a saved live mobile from the same running animal spawner as the monkey, not scenery or a marker.
- The panther's constructor has two random stat branches, but both are still a panther and both are far above Mira's current taming reach. This run does not resolve which branch the live save rolled.
- `FightMode.Aggressor` currently fails target acquisition without an existing aggressor/aggressed/faction/ethic relationship, and `RangePerception 16` misses Mira from the panther's tile. Future private AI timing and wandering remain unresolved.

Next pressure:

Mira remains at `Point3D(1234,1354,0)`, facing southeast. The controlled fox remains approximately at `Point3D(1231,1356,0)`, following only. The screen now has labeled Monkey serial `206380` at dx `11`, dy `12` and labeled Panther serial `289483` at dx `18`, dy `17`, with zero saved visible items. The next honest decision is retreat or pick a safer line; continuing southeast toward the monkey would walk closer to the panther without a new safety proof.

## Run 210 - I Back Out Of The Cat's Edge

I start at `Point3D(1234,1354,0)`, facing southeast. The monkey is still visible at dx `11`, dy `12`, still too far for the range-6 `Tame` row, and the panther I just named is right on the screen edge at dx `18`, dy `17`. The cat is not actively on me, but that is only because the current `FightMode.Aggressor` gate has no aggressor history and its range-16 perception misses me. I do not walk toward it.

**Beat 1**

I press northwest.

This is only a turn. `Mobile.Move` compares the requested direction with my current direction first, and because `Direction.Down` does not match `Direction.Up`, it skips `CheckMovement`. Mira stays at `Point3D(1234,1354,0)` and turns to `Direction.Up`.

The turn still goes through the normal movement acknowledgement/update path and notifies nearby mobiles. That does not make the monkey or panther attack. Both are `FightMode.Aggressor`, so the notice-sound branch that is reserved for actually aggressive monsters does not fire, and neither animal has `ReacquireOnMovement`.

**Beat 2**

I press northwest again, and this time I actually step away.

The reverse of the last southeast approach is already mechanically proved. `Point3D(1233,1353,0)` is dry Sosaria grass, the diagonal side-check tiles `1234,1353` and `1233,1354` are dry grass with zero statics, and the live snapshot has no saved mobile or item blocker on the destination. `MovementReq -> Mobile.Move -> MovementImpl.CheckMovement -> Region.CanMove -> SetLocation/SetDirection -> OnMovement` accepts the step.

The step does what I wanted: the panther falls off the client rectangle by one tile, now dx `19`, dy `18`. I do not claim it left, despawned, stopped, or became safe. It is still recently visible, still within the broader movement-notice rectangle, and its private AI timer and future wandering are not exported. The monkey remains on screen at dx `12`, dy `13`, still outside the range-6 context action. My fox stays at `Point3D(1231,1356,0)` because no exact pet AI tick ran; from the new player point it is still inside the 2..3 follow band at dx `-2`, dy `3`.

I stop here because the visible range changed. The immediate cat pressure is no longer clickable, but the monkey is still visible and the panther is only one tile beyond screen edge, not solved.

Mechanical friction learned:

- A retreat key can be only a facing turn. The server does not test the target tile until the current direction mask matches the requested direction.
- Facing changes still run the normal movement update/OnMovement notification path, but these specific animals do not use movement reacquire and their `FightMode.Aggressor` setting suppresses the notice-sound branch.
- The northwest retreat step to `1233,1353` is the safe reverse of the proved southeast grass route.
- Moving one tile back is enough to drop the panther from `Utility.InUpdateRange`, but it is not a despawn, pacification, AI tick, combat resolution, or safety proof.

Next pressure:

Mira ends at `Point3D(1233,1353,0)`, facing northwest. The controlled fox remains at `Point3D(1231,1356,0)`, following only and inside the heel band. Monkey serial `206380` is still visible at dx `12`, dy `13`; Panther serial `289483` is recently visible just outside the southeast screen edge at dx `19`, dy `18`. There are still zero saved visible items. The next honest action is either keep backing away while accounting for the monkey and fox leash, or choose a different route that does not walk back into the panther.

## Run 211 - One More Step Out Of The Cat Line

I start at `Point3D(1233,1353,0)`, facing northwest. The monkey is still a body on the screen at dx `12`, dy `13`, still too far for the range-6 `Tame` row. The panther is just past the southeast edge at dx `19`, dy `18`. That is better than touching it, but it is not a solved animal. I do not turn back toward the cat.

**Beat 1**

I press northwest again.

This time it is a real step, not a turn. `Mobile.Move` already sees my direction mask as `Direction.Up`, so it runs the normal movement block. The target `Point3D(1232,1352,0)` is the reverse of the dry route I used while approaching the monkey: Run 208 already proved that tile as map1 grass with zero statics, and the two player diagonal side-check tiles `1232,1353` and `1233,1352` were also zero-static grass. The live snapshot has no saved mobile or item standing on the destination.

The step moves me away from the panther and a little away from the monkey. The panther is now dx `20`, dy `19`, outside the client rectangle and outside its effective range-16 perception geometry, though not despawned or pacified. The monkey remains visible at dx `13`, dy `14`, still only labeled screen pressure and still outside range 6. The fox does not get an exact AI tick during my movement; it stays at `Point3D(1231,1356,0)`, now dx `-1`, dy `4`, outside the 2..3 heel band.

I stop because the next decision is no longer "just keep walking." I have a visible monkey still behind the retreat line, a recently visible panther farther off-screen, and my follower stretched enough that the next honest beat is either let the fox catch up or keep moving with a fresh pressure check.

Mechanical friction learned:

- A second northwest key from `Direction.Up` enters the full movement path; the facing-turn friction is gone for this input.
- The northwest retreat target `1232,1352` is legal because it reuses the Run 208 server-order grass/static proof in reverse.
- Moving one tile farther does not resolve the monkey or panther. It only changes their visibility geometry.
- Player movement does not automatically advance the controlled fox's `Follow` AI tick. The fox is now outside the remembered heel band and cannot be treated as guarding or exact protection.

Next pressure:

Mira ends at `Point3D(1232,1352,0)`, facing northwest. The controlled fox remains at `Point3D(1231,1356,0)`, ordered to Follow but outside the heel band at floored distance `4`. Monkey serial `206380` remains visible at dx `13`, dy `14`; Panther serial `289483` is recently visible off-screen at dx `20`, dy `19`. No item, gump, context menu, target cursor, combat, damage, skill gain, hunger/thirst tick, quest, discovery, ownership, follower-count, pet-order, corpse, loot, or safety flag changed.

## Run 212 - Let The Fox Catch Its Breath

I start at `Point3D(1232,1352,0)`, still facing northwest. The monkey is still on screen at dx `13`, dy `14`, too far for the range-6 `Tame` row. The panther is farther past the southeast edge at dx `20`, dy `19`. The thing I actually dragged out of shape last run is my fox: it is four tiles behind me, ordered to Follow, not guarding anything.

**Beat 1**

I wait for the fox instead of pressing another key.

The controlled AI path runs: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange`. The remembered `FriendsAvoidHeels` band still wants distance `2..3`. From `Point3D(1231,1356,0)` to me, the floored distance is `4`, so `GetDirectionTo` points the fox straight `Direction.North`. `DoMove/Mobile.Move` accepts `Point3D(1231,1355,0)`: `map1.mul` says grass `0x6`, z `0`, and `staidx1/statics1` finds zero statics.

The fox is closer, but my own position has not changed. The monkey is still visible. The panther is still off-screen, not solved.

**Beat 2**

I take one more northwest step.

Because I am already facing `Direction.Up`, this is a real movement input. `Mobile.Move` enters `MovementImpl.CheckMovement`. The target `Point3D(1231,1351,0)` is dry `map1` grass `0x4` with zero statics. The player diagonal side-check tiles, `Point3D(1231,1352,0)` and `Point3D(1232,1351,0)`, are dry grass with zero statics too. No saved mobile or item blocks the destination.

The step shifts the screen without changing the problem into a win. Monkey serial `206380` is still visible at dx `14`, dy `15`, still outside range 6. Panther serial `289483` is now dx `21`, dy `20`, farther outside the client rectangle and outside its range-16 perception geometry, but not despawned, pacified, or AI-resolved. My fox is stretched again, now at dx `0`, dy `4`.

**Beat 3**

I wait for the fox one more time.

The same follow path runs again. `WalkMobileRange` sees floored distance `4`, `GetDirectionTo` again chooses `Direction.North`, and the fox steps from `Point3D(1231,1355,0)` to `Point3D(1231,1354,0)`. The target is dry `map1` grass `0x5`, z `0`, with zero statics and no saved blocker. The fox ends at dx `0`, dy `3`, back inside the heel band.

Mechanical friction learned:

- Controlled pet follow is a timer beat, not passive protection. It moves the fox, but it does not set Guard, Attack, Combatant, body-blocking, ownership, follower count, or a new pet order.
- One northwest player step from here is still mechanically legal dry grass, but every step away from the monkey/panther line can stretch the fox again.
- The monkey remains a visible saved wild mobile, not scenery. Its `FightMode.Aggressor` acquisition is currently negative only because no aggressor/aggressed/faction/ethic state involves me or the fox.
- The panther leaving the client rectangle is not safety. It is just off-screen risk with private AI timing and future wandering unresolved.

Next pressure:

Mira ends at `Point3D(1231,1351,0)`, facing northwest. The controlled fox is at `Point3D(1231,1354,0)`, ordered to Follow and inside the heel band. Monkey serial `206380` remains visible at dx `14`, dy `15`; Panther serial `289483` is recently visible off-screen at dx `21`, dy `20`. There are still zero saved visible items and zero visible running spawner objects. The next honest action is another route decision that keeps distance from the monkey/panther line, not a return southeast.

## Run 213 - I Slide West, Not Back Toward The Cat

I start at `Point3D(1231,1351,0)`, facing northwest, with the fox tucked behind me at `Point3D(1231,1354,0)`. The monkey is still on the screen at dx `14`, dy `15`, labeled and tempting in the stupid way visible animals are tempting. Its `Tame` row was already gray from here, and the panther is farther southeast off-screen at dx `21`, dy `20`. I do not open the monkey menu again. I also do not pull northwest and stretch the fox. West is the cleaner retreat line.

**Beat 1**

I press west.

This is only a turn. `MovementReq(Direction.West)` reaches `Mobile.Move`, but my current direction mask is still `Direction.Up`, so the real movement branch never calls `CheckMovement`. The server acknowledges the movement packet, leaves me at `Point3D(1231,1351,0)`, and changes my facing to `Direction.West`. Nearby `OnMovement` notifications still exist as a system path, but there is no combat assignment, no target cursor, no menu, no taming, and no fox tick.

The screen is unchanged: one visible monkey, no saved visible items, panther still off-screen.

**Beat 2**

I press west again, and this one moves.

`Mobile.Move` now sees the requested direction matches my facing, so it enters `MovementImpl.CheckMovement`. The target `Point3D(1230,1351,0)` is dry `map1` grass `0x6`, z `0`, with zero statics. Cardinal west movement does not need diagonal side-check tiles. The live snapshot has no saved mobile or item on the destination. The step lands.

The monkey shifts to dx `15`, dy `15`. That is still visible, still outside range `6`, and still not a solved animal. The panther shifts to dx `22`, dy `20`, farther outside the client rectangle. The fox stays at `Point3D(1231,1354,0)`, now dx `1`, dy `3`, still inside the heel band without needing a timer beat.

**Beat 3**

I press west a third time.

The second west target, `Point3D(1229,1351,0)`, is also plain dry grass: land `0x4`, z `0`, zero statics, no saved live blocker. `MovementReq -> Mobile.Move -> MovementImpl.CheckMovement -> Region.CanMove -> SetLocation/SetDirection -> OnMovement` accepts it.

That spends the hour. The monkey is still visible at dx `16`, dy `15`, and because its `AI_Animal/FightMode.Aggressor` path has no aggressor/aggressed/faction/ethic state involving me or the fox, the current acquisition result stays negative. That is not safety. It is just why nothing bites during these three beats. The panther is now dx `23`, dy `20`, farther off-screen and outside its effective range-16 perception geometry, but still not gone. The fox remains close at dx `2`, dy `3`.

Mechanical friction learned:

- West from a northwest-facing stance costs a facing beat before it becomes movement.
- Cardinal movement avoids the player diagonal side-check rule, but the forward tile still has to be proved.
- The west row at `1230,1351` and `1229,1351` is dry grass with zero statics and no saved route blockers.
- Moving west keeps the fox inside the heel band without pretending it guarded, body-blocked, attacked, or ran a fresh `BaseAI.Obey` tick.
- The monkey is still visible screen pressure. West bought distance from the panther, not permission to ignore the monkey.

Next pressure:

Mira ends at `Point3D(1229,1351,0)`, facing west. The controlled fox remains at `Point3D(1231,1354,0)`, ordered to Follow and inside the heel band. Monkey serial `206380` remains visible at dx `16`, dy `15`; Panther serial `289483` is recently visible off-screen at dx `23`, dy `20`. There are still zero saved visible items and zero visible running spawner objects. No gump, context menu, target cursor, taming, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 214 - I Do Not Drag The Fox Blind

I start at `Point3D(1229,1351,0)`, facing west. The monkey is still visible at `Point3D(1245,1366,0)`, dx `16`, dy `15`, which means it is still a real thing on the screen even if the old `Tame` row was useless from this distance. The panther is farther southeast and off-screen at dx `23`, dy `20`, but that is only distance, not a solved animal. My fox is tucked at `Point3D(1231,1354,0)`, just inside the follow band.

**Beat 1**

I press west.

Because I am already facing `Direction.West`, this is not a turn. `Mobile.Move` enters `MovementImpl.CheckMovement`, tests the forward tile, and accepts `Point3D(1228,1351,0)`. The server-order map read says dry Sosaria grass `0x6`, z `0`, with zero statics and no saved live mobile or item blocker on the destination. The monkey shifts to dx `17`, dy `15`, still visible and still far outside range `6`. The panther shifts to dx `24`, dy `20`, outside the client rectangle and outside its effective range-16 perception geometry.

The fox did not tick during my movement. It is now at dx `3`, dy `3`, floored distance `4`, outside the 2..3 heel band. I do not keep walking and pretend that is fine.

**Beat 2**

I wait for the fox.

The controlled follow path runs: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange`. `FriendsAvoidHeels` still makes the desired band 2..3. From `Point3D(1231,1354,0)` to me at `Point3D(1228,1351,0)`, the fox chooses `Direction.Up`. `MovementImpl.CheckMovement` accepts `Point3D(1230,1353,0)`: dry grass `0x5`, z `0`, zero statics on the forward tile, and at least one clear non-player diagonal side tile. The fox ends at dx `2`, dy `2`, inside the band again.

That is still not guarding. No `Guard`, `Attack`, `Combatant`, body-blocking, ownership, follower count, or pet order changes. It is just the pet catching up.

**Beat 3**

I press west again.

The target `Point3D(1227,1351,0)` is another dry grass tile, `0x6`, z `0`, with zero statics and no saved route blocker. The movement path accepts it. The monkey slides to the inclusive edge at dx `18`, dy `15`: still visible, now outside the monkey's effective range-16 perception by x distance, but not gone. The panther is dx `25`, dy `20`, now outside both the client rectangle and the 24-tile movement-notice box, but no despawn, pacification, AI tick, combat, corpse, or safety proof ran.

Mechanical friction learned:

- Westward retreat from this tile is mechanically open grass, but visible animals still control the pace.
- Controlled pet follow is a separate timer beat. One west step stretched the fox outside the heel band; waiting let the exact follow path move it northwest onto a clear tile.
- The monkey being on the edge is not a taming opportunity. It still fails the range-6 context row and now also misses range-16 acquisition geometry by x distance.
- The panther is less immediate only because I keep moving away from it. It is still a saved wild mobile from the running animal spawner, not a resolved threat.

Next pressure:

Mira ends at `Point3D(1227,1351,0)`, facing west. The controlled fox is at `Point3D(1230,1353,0)`, ordered to Follow and inside the heel band. Monkey serial `206380` remains visible at dx `18`, dy `15`; Panther serial `289483` is recently visible off-screen at dx `25`, dy `20`. There are still zero saved visible items and zero visible running spawner objects. No gump, context menu, target cursor, taming, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 215 - The Monkey Finally Leaves The Screen

I start at `Point3D(1227,1351,0)`, facing west. The monkey is still barely visible at the far east edge, dx `18`, dy `15`, too far for the old `Tame` row and outside its effective range-16 perception by x distance. The panther is farther southeast and off-screen at dx `25`, dy `20`. My fox is close at `Point3D(1230,1353,0)`, but one more west step will stretch it. I take the step anyway because west is still the only direction that does not walk back toward the animals.

**Beat 1**

I press west.

`MovementReq(Direction.West)` reaches `Mobile.Move`, and because I am already facing west the server enters the full movement branch. `MovementImpl.CheckMovement` accepts `Point3D(1226,1351,0)`: dry Sosaria `map1` grass `0x3`, z `0`, zero statics, and no saved mobile or item blocker on the destination.

The monkey drops off the client rectangle: it is now dx `19`, dy `15`, not visible and not clickable. I do not call that safe. It is still a saved wild mobile from the running animal spawner, only now off-screen. The fox does not tick during my movement and ends at dx `4`, dy `2`, floored distance `4`, outside the 2..3 follow band.

**Beat 2**

I wait for the fox instead of dragging it blind.

The controlled follow path runs: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange`. With Mira at `Point3D(1226,1351,0)` and the fox at `Point3D(1230,1353,0)`, `GetDirectionTo` resolves `Direction.Up`, so the fox tries the northwest diagonal. `MovementImpl.CheckMovement` accepts `Point3D(1229,1352,0)`: dry grass `0x4`, z `0`, zero statics. The non-player diagonal side tiles `Point3D(1229,1353,0)` and `Point3D(1230,1352,0)` are also dry grass with zero statics, so the one-clear-side rule is satisfied.

The fox ends at dx `3`, dy `1`, floored distance `3`, back in the heel band. This is not Guard, Attack, body-blocking, or proof of the next AI tick. It is just the follower catching up.

**Beat 3**

I press west one more time.

The next west target, `Point3D(1225,1351,0)`, is dry `map1` grass `0x4`, z `0`, with zero statics and no saved route blocker. `Mobile.Move` accepts it.

The final scan is quiet in the narrow client sense: no saved wild mobiles, no saved visible items, and no running spawner object locations inside x `1207..1243`, y `1333..1369`. The monkey is off-screen at dx `20`, dy `15`; the panther is off-screen at dx `27`, dy `20`; neither is despawned, pacified, killed, tamed, or AI-resolved. The fox is now stretched again at dx `4`, dy `1`, floored distance `4`, so the next honest pressure is the leash, not curiosity.

Mechanical friction learned:

- One more west step is enough to push the visible monkey out of `Utility.InUpdateRange`, but visibility loss is not a world-state solution.
- The fox follow tick from this geometry resolves northwest, not west, because `GetDirectionTo` uses the isometric `rx/ry` direction calculation.
- Non-player diagonal movement is less strict than player diagonal movement here: one clear side tile is enough, and both side tiles were clear.
- A quiet 18-tile rectangle can still have off-screen route risk. The saved monkey and panther still exist in the snapshot, just outside the client view.

Next pressure:

Mira ends at `Point3D(1225,1351,0)`, facing west. The controlled fox is at `Point3D(1229,1352,0)`, ordered to Follow but outside the heel band at floored distance `4`. No wild mobile or item is visible in the current 18-tile rectangle. Monkey serial `206380` is recently visible off-screen at dx `20`, dy `15`; Panther serial `289483` is farther off-screen at dx `27`, dy `20`. No gump, context menu, target cursor, taming, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 216 - I Let The Leash Set The Pace

I start at `Point3D(1225,1351,0)`, facing west. The screen is quiet, but only in the narrow client sense. The live snapshot rectangle has no saved wild mobile, no saved visible item, and no visible running spawner object in range. The monkey I backed away from is still in the save at `Point3D(1245,1366,0)`, now dx `20`, dy `15`, and the panther is farther out at dx `27`, dy `20`. They are not gone. They are just no longer on the screen.

The thing actually out of shape is my own fox. It is at `Point3D(1229,1352,0)`, ordered to Follow, but it is four tiles away by floored distance. I do not keep walking and drag it blind.

**Beat 1**

I wait for the fox.

The controlled follow path runs again: `AITimer.OnTick -> BaseCreature.OnThink -> BaseAI.Obey -> DoOrderFollow -> WalkMobileRange`. This time the isometric direction math changes. From `Point3D(1229,1352,0)` to me at `Point3D(1225,1351,0)`, `GetDirectionTo` resolves `Direction.West`, not `Direction.Up`. The fox steps to `Point3D(1228,1352,0)`. The tile is dry `map1` grass `0x3`, z `0`, with zero statics and no saved blocker.

The fox is back inside the heel band at dx `3`, dy `1`. Nothing else appears. No wild AI tick, combat, target cursor, menu, item, skill, quest, discovery, hunger, thirst, or pet-order change runs.

**Beat 2**

I press west once.

Because I am already facing `Direction.West`, this is a real movement input. `Mobile.Move` enters `MovementImpl.CheckMovement` and accepts `Point3D(1224,1351,0)`: dry Sosaria grass `0x6`, z `0`, zero statics, no saved mobile or item blocker. The shifted client rectangle is still empty of saved wild mobiles and items. The monkey slides farther out to dx `21`, dy `15`; the panther to dx `28`, dy `20`.

The fox does not tick during my step and is stretched again at dx `4`, dy `1`. That is the cost of moving west safely: I am still buying distance, one leash check at a time.

**Beat 3**

I wait for the fox again.

`DoOrderFollow` repeats the same westward catch-up. From `Point3D(1228,1352,0)` toward me at `Point3D(1224,1351,0)`, `GetDirectionTo` again resolves `Direction.West`, and `DoMove/Mobile.Move` accepts `Point3D(1227,1352,0)`. The target is dry grass `0x3`, z `0`, zero statics, and no live saved blocker.

The fox ends at dx `3`, dy `1`, back inside the 2..3 follow band. That is not guarding or body-blocking. It is just the pet catching up. I stop at the three-beat cap with the screen still quiet, the monkey and panther still unresolved off-screen, and the west route still not a safe road or shelter.

Mechanical friction learned:

- From this geometry, the fox's exact follow tick resolves west because `GetDirectionTo`'s `rx/ry` test lands in the `Direction.West` branch.
- A normal west step to `Point3D(1224,1351,0)` is mechanically open grass, but it stretches the fox right back outside the follow band.
- The empty 18-tile rectangle at `1224,1351` is only negative visibility evidence. It is not a despawn, pacification, AI resolution, safe region, road, water source, or discovery.
- Pet follow waits mutate only pet position and speed state. They do not set Guard, Attack, Combatant, owner state, follower count, or protection.

Next pressure:

Mira ends at `Point3D(1224,1351,0)`, facing west. The controlled fox is at `Point3D(1227,1352,0)`, ordered to Follow and inside the heel band. No wild mobile or item is visible in the current 18-tile rectangle. Monkey serial `206380` is recently visible off-screen at dx `21`, dy `15`; Panther serial `289483` is farther off-screen at dx `28`, dy `20`. No gump, context menu, target cursor, taming, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 217 - The West Row Ends At Another Toad

I start at `Point3D(1224,1351,0)`, facing west. The screen is empty except for my own fox at my shoulder. That does not make the jungle safe. It only means the monkey and panther are now off-screen behind me, and the next honest thing to do is keep the route small enough that I can stop when the screen changes.

**Travel Segment 1**

I hold west for twelve accepted steps.

The row stays mechanically boring in the useful way: `MovementReq -> Mobile.Move -> MovementImpl.CheckMovement` accepts `Point3D(1223,1351,0)` through `Point3D(1212,1351,0)`. The server-order `map1.mul` read uses Sosaria file index `1` and the column-major `TileMatrix` block order. Every target is dry grass at z `0`, land `0x3/0x4/0x5/0x6`, with zero statics and no saved mobile or item blocker. I do not pretend the fox's timer fired twelve exact times; I shadow it conservatively along the clear row behind me, ending around `Point3D(1215,1352,0)`, still just behind the heel band.

The endpoint scan at `Point3D(1212,1351,0)` is still empty of saved wild mobiles and items. The Mines of Morinia marker is closer, but a marker is not shelter.

**Travel Segment 2**

I keep west for another twelve accepted steps.

The same route proof carries across `Point3D(1211,1351,0)` through `Point3D(1200,1351,0)`: dry grass, no statics, no saved route blocker. One tile at `Point3D(1203,1351,1)` lifts to z `1`, but it is still grass with no blocking static. The fox is only approximate follower evidence, shadowed back to about `Point3D(1203,1352,0)`.

The scan at `Point3D(1200,1351,0)` still has no saved wild mobile, item, or visible spawner object. That is negative visibility evidence, not a safe road.

**Travel Segment 3**

I press west again, but I only get nine accepted inputs before the screen matters.

The accepted targets `Point3D(1199,1351,0)` through `Point3D(1191,1351,0)` are passable: mostly dry grass, then jungle at `1192` and `1191`. `Point3D(1191,1351,0)` has only `0xD4C` foliage leaves, not an impassable, surface, bridge, or wet blocker. The fox is shadowed to about `Point3D(1194,1352,0)`.

Then the west edge gives me a thing with eyes. Toad serial `288077` is visible at `Point3D(1173,1360,0)`, dx `-18`, dy `9`. Its current `RangePerception 16` acquisition misses by x distance, and the `GiantToad.TeleportTimer` range `10` also misses by x distance, so I do not invent a hit. But it is still a visible timer creature from a running spawner, and that stops route-driving immediately.

Mechanical friction learned:

- The old east row can be used in reverse only while the live rectangle stays empty and the server-order statics stay clear.
- Three Travel Segment beats moved Mira from `Point3D(1224,1351,0)` to `Point3D(1191,1351,0)` without opening UI, changing inventory, mutating pet ownership, or resolving off-screen animals.
- Follower shadowing is still approximate; it preserved the fox near `Point3D(1194,1352,0)`, but it is not an exact `BaseAI.Obey` timer and not Guard, Attack, body-blocking, or protection.
- A visible toad at the inclusive edge is enough to stop, even when the current timer and acquisition geometry are negative.

Next pressure:

Mira ends at `Point3D(1191,1351,0)`, facing west. The controlled fox is approximately at `Point3D(1194,1352,0)`, ordered to Follow and inside the trailing band. Toad serial `288077` is visible at dx `-18`, dy `9`; its current target/timer geometry is negative, but its private timer and future wandering are unresolved. No item, gump, context menu, target cursor, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 218 - I Bend North, Then The West Fills In

I start at `Point3D(1191,1351,0)`, facing west. The toad is not on top of me, but it is still on the west-south edge at `Point3D(1173,1360,0)`. I do not keep driving straight at it just because the first range band is negative. I turn the route north, where the screen has room.

**Beat 1**

I run north ten steps to `Point3D(1191,1341,0)`.

The crossed tiles are plain grass and jungle at z `0..1`. A few visible leaf, mushroom, and vine statics sit on the west line I am preparing to use, but the checked statics on the route are foliage/background only, not impassable, wet, surface, bridge, wall, or door. The toad drops off the screen by y distance. That does not delete it; it just means I have put the current client rectangle above it.

The north edge now shows passive animals instead: a horse at `Point3D(1196,1326,0)` and a llama at `Point3D(1208,1323,0)`. The old saved fox serial would also be up there in the live snapshot, but that serial is already my controlled follower in this continuity, so I do not count it as a new wild fox. I keep the route west, away from the horse and llama.

**Beat 2**

I run west along the northern line from `Point3D(1191,1341,0)` to `Point3D(1171,1341,0)`.

This gets me closer to the Mines marker without closing on the toad below. The fox is only approximate follower evidence during this macro movement, trailing around `Point3D(1174,1342,0)` on Follow. It is not Guard, not Attack, and not protection.

At `Point3D(1171,1341,0)`, the west side fills in. A boar appears at `Point3D(1153,1342,0)`, dx `-18`, dy `1`. A gorilla at `Point3D(1156,1356,0)` and another boar at `Point3D(1156,1357,0)` sit southwest, both still in the potential-threat band. That is enough to stop. The next move is no longer simple west travel; it is a choice about whether to bend north harder, back off, or inspect the animal line.

Mechanical friction learned:

- The north bend from `1191,1351` to `1191,1341` is a visible route correction around the west-south toad, not a hidden mechanic.
- The northern west line contains jungle foliage, mushrooms, vines, and leaves, but the checked route statics are non-blocking visual clutter.
- The live-state snapshot still contains the fox's old saved row, but player continuity overrides that serial as Mira's controlled follower.
- The Mines of Morinia marker is closer, now dx `-149`, dy `31`, but the marker remains navigation knowledge, not proof of shelter or safety.

Next pressure:

Mira ends at `Point3D(1171,1341,0)`, facing west. The controlled fox is approximately at `Point3D(1174,1342,0)`, ordered to Follow. Visible route pressure is now west/southwest: boar serial `288070` at dx `-18`, dy `1`, gorilla serial `288068` at dx `-15`, dy `15`, and boar serial `634017` at dx `-15`, dy `16`. No item, gump, context menu, target cursor, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 219 - The North Detour Finds Trouble Too

I start at `Point3D(1171,1341,0)`, facing west. The Mines marker is still pulling me west-southwest, but the actual screen says not to walk straight into the animal line. The boar at `Point3D(1153,1342,0)` is on the west edge, and the gorilla/boar pair is southwest. There are no visible items, no open gump, no target cursor, and no friendly sign or road to make this a route instead of wilderness.

**Beat 1**

I bend north until the boar line drops off the client rectangle.

Mira runs to `Point3D(1171,1323,0)`. I do not audit every grass tile; this is ordinary open-terrain correction away from visible animals. The follower is still on Follow and is treated as practical travel presence, not an exact timer proof. At the new screen center, the live snapshot has no saved visible mobile or item in range. That does not make the place safe. It only gives me a clean screen for the next west attempt.

**Beat 2**

I try west again, staying on the northern line.

The route only gets as far as `Point3D(1145,1323,0)` before the west edge fills in again. A crane sits at `Point3D(1127,1314,0)`, dx `-18`, dy `-9`; that is just visible wildlife. The real stop is the kobold shaman at `Point3D(1127,1324,0)`, dx `-18`, dy `1`, with a goblin at `Point3D(1151,1341,0)`, dx `6`, dy `18`. Both are edge-of-screen danger, not a private AI prediction. I stop before the route turns into closing distance on them.

Mechanical friction learned:

- Bending north is enough to break sight on the boar/gorilla/boar cluster without discovering anything or opening UI.
- The west route toward the Mines marker now exposes a humanoid/mage threat line at the edge of vision.
- No hidden teleporter, custom gump gate, item interaction, combat swing, pet command, discovery flag, or inventory mutation explains the stop; the client screen does.
- The Mines marker is still navigation knowledge only. It has not become a visible mine entrance, shelter, road, or safe region.

Next pressure:

Mira ends at `Point3D(1145,1323,0)`, facing west. The controlled fox is approximately at `Point3D(1148,1324,0)`, ordered to Follow. Visible route pressure is west/south: kobold shaman serial `30224` at dx `-18`, dy `1`, and goblin serial `27177` at dx `6`, dy `18`; crane serial `288065` is passive edge context. The previous boar/gorilla/boar cluster is off-screen to the south and east of this new line, not solved. No item, gump, context menu, target cursor, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 220 - I Back Out, Then The North Bypass Shows Teeth

I start at `Point3D(1145,1323,0)`, facing west. The Mines marker is still west-southwest, but the real screen has a kobold shaman on the west edge and a goblin on the south edge. I do not step closer to either of them just to keep a straight line.

**Beat 1**

I backtrack east along the just-learned northern line to `Point3D(1171,1323,0)`.

This is retreat, not exploration. The kobold shaman, goblin, and crane all fall off the client rectangle. Nothing opens, no target cursor appears, and the fox is still only a follower keeping up in practical range.

**Beat 2**

With the west-south pressure off-screen, I run north to `Point3D(1171,1305,0)`.

The new screen is quiet in the live snapshot: no saved mobile, item, sign, road, corpse, chest, shelter, or water source inside the 18-tile rectangle. That gives me a clean moment to try the bypass, not proof of safety.

**Beat 3**

I test west on the northern bypass, stopping at `Point3D(1155,1305,0)`.

The first useful screen change is not a mine entrance. A monkey appears at `Point3D(1137,1300,0)`, dx `-18`, dy `-5`; a panther appears at `Point3D(1138,1295,0)`, dx `-17`, dy `-10`; and a parrot sits at `Point3D(1151,1299,0)`, dx `-4`, dy `-6`. The parrot is just context. The panther is close enough on the west-north edge that I stop before turning this bypass into another blind push.

Mechanical friction learned:

- Retreating east from the kobold/goblin line clears the immediate route pressure without solving it.
- The northern bypass is client-quiet only until `Point3D(1155,1305,0)`; after that, a panther marks the next westward risk.
- The Mines of Morinia marker is now dx `-133`, dy `67`. It is still navigation knowledge only, not a visible entrance, road, shelter, or safe region.
- No hidden teleporter, gump, quest, item interaction, combat swing, pet command, discovery flag, or inventory mutation explains the stop; the client screen does.

Next pressure:

Mira ends at `Point3D(1155,1305,0)`, facing west. The controlled fox is approximately at `Point3D(1158,1306,0)`, ordered to Follow. Visible route pressure is west-north: panther serial `288058` at dx `-17`, dy `-10`; monkey serial `200106` at dx `-18`, dy `-5`; parrot serial `288067` is passive context. The kobold shaman and goblin are off-screen to the southwest now, not solved. No item, gump, context menu, target cursor, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 221 - I Climb Above One Panther And Find More

I start at `Point3D(1155,1305,0)`, facing west. The Mines marker still pulls west-southwest, but the visible screen has a panther on the west-north edge. I do not keep pushing into that line.

**Beat 1**

I backtrack east to `Point3D(1171,1305,0)`.

That clears the panther, monkey, and parrot from the client rectangle. Nothing opens, no target cursor appears, no item enters reach, and the fox is still just a follower keeping up in practical range.

**Beat 2**

I run north to `Point3D(1171,1287,0)` to climb above the panther line before trying west again.

The screen is not empty, but it is not an attack decision either. A moose, goat, hawk, eagle, and mountain goat sit east and north in the saved live snapshot. They read as wildlife context, not a reason to fight or open code.

**Beat 3**

I test west to `Point3D(1155,1287,0)`.

The west edge answers fast. Four panthers are now visible: one at `Point3D(1138,1283,0)`, one at `Point3D(1144,1276,0)`, one at `Point3D(1138,1295,0)`, and one at `Point3D(1140,1273,0)`. A monkey and the same parrot are also visible. None of the panthers is on top of me, but the route west would close distance into a predator cluster, so I stop.

Mechanical friction learned:

- The east retreat to `1171,1305` is a clean client-side escape from the first panther edge.
- The north line at `1171,1287` shows wildlife only; no item, sign, road, shelter, water source, gump, target cursor, or custom shard result appears.
- The west test at `1155,1287` exposes a panther cluster. That is visible route pressure, not a hidden teleporter, tarot gate, quest gate, or private AI timer.
- The Mines of Morinia marker is still only overlay knowledge. From the new stop it is dx `-133`, dy `85`, with no visible entrance or road.

Next pressure:

Mira ends at `Point3D(1155,1287,0)`, facing west. The controlled fox is approximately at `Point3D(1158,1288,0)`, ordered to Follow. Visible route pressure is west and northwest: panther serial `288060` at dx `-17`, dy `-4`; panther serial `200891` at dx `-11`, dy `-11`; panther serial `288058` at dx `-17`, dy `8`; and panther serial `200730` at dx `-15`, dy `-14`. Monkey serial `200106` and parrot serial `288067` are visible context. The kobold shaman/goblin line and the older southern animal clusters remain off-screen route risks, not current blockers. No item, gump, context menu, target cursor, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 222 - The North Bypass Keeps Throwing Predators At Me

I start at `Point3D(1155,1287,0)`, facing west. The Mines marker is still southwest, but the actual screen is a wall of cats on the west and northwest edge. None of them is biting me yet, so I do not fight; I get distance first.

**Beat 1**

I backtrack east to `Point3D(1171,1287,0)`.

The panthers, monkey, and parrot fall off the client rectangle. The screen becomes the same wildlife pocket I already know: moose, goat, hawk, eagle, and mountain goat. There is still no road, sign, water source, corpse, chest, shelter, gump, or target cursor. This is just enough breathing room to try climbing above the cat line.

**Beat 2**

I run north to `Point3D(1171,1269,0)`.

The move does not hit a wall or custom warning. The east and northeast side fills with wildlife, but one of the shapes is not harmless: a cougar at `Point3D(1186,1264,0)`, dx `15`, dy `-5`. It is only a potential threat from this range, and moving west would put distance between us, so I do not stop to fight it.

**Beat 3**

I test west to `Point3D(1155,1269,0)`.

The route west still does not turn into a mine entrance. It turns into another predator line: panthers at `Point3D(1144,1276,0)`, `Point3D(1140,1273,0)`, and `Point3D(1138,1283,0)`, with a swamp drake at `Point3D(1137,1264,0)` on the west edge. The nearest panther is still outside the immediate 0-10 tile band, but pushing farther west would close distance into them. I stop facing west.

Mechanical friction learned:

- Retreating east from the `1155,1287` panther cluster clears that screen without opening UI or changing character state.
- The north move to `1171,1269` exposes a cougar to the east/northeast, so farther north is no longer a casual bypass.
- Testing west from the northern line exposes three panthers and a swamp drake before the Mines marker becomes visible as anything real in-world.
- The Mines of Morinia marker is now dx `-133`, dy `103`. It is still only world-map overlay knowledge, not a visible entrance, road, shelter, or safe region.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, or inventory mutation explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1155,1269,0)`, facing west. The controlled fox is approximately at `Point3D(1158,1270,0)`, ordered to Follow. Visible route pressure is west and southwest: panther serial `200891` at dx `-11`, dy `7`; panther serial `200730` at dx `-15`, dy `4`; panther serial `288060` at dx `-17`, dy `14`; and swamp drake serial `27755` at dx `-18`, dy `-5`. The cougar serial `444834` is now remembered as an east/northeast route risk, not the thing I am walking toward. No item, visible spawner object, gump, context menu, target cursor, combat, damage, loot, corpse, hunger/thirst, quest, discovery, ownership, follower-count, pet-order, PvP/PvE, or region state changed.

## Run 223 - I Stop Forcing The Mines Line

I start at `Point3D(1155,1269,0)`, facing west. The client screen still says the same thing the map marker does not: west is cats and a swamp drake, not a mine entrance. I am not close enough to be bitten, but I am close enough that one more push west turns curiosity into combat.

**Beat 1**

I retreat southeast to the known open pocket at `Point3D(1171,1287,0)`.

The panthers and swamp drake drop off the client rectangle. I do not get a gump, target cursor, discovery message, corpse, chest, road, sign, shelter, or water source. The screen goes back to ordinary wildlife: moose, goat, hawk, eagle, and mountain goat. That is not safety; it is just no immediate reason to fight.

**Beat 2**

I stop trying to force west and run east to `Point3D(1187,1287,0)`.

This is the first clean-feeling route change after several failed Mines pushes. The visible screen is crowded, but with passive animals: goat, horse, moose, hawk, llama, eagle, another llama, fox, more horses, and a mountain goat. I still remember the cougar north/east of the last bend, so I do not drift north into it. I stop here because the next choice is a real one: skirt the herd southeast toward the Ruins marker, keep looping east, or turn back toward the Mines predator bands.

Mechanical friction learned:

- The immediate west/northwest Mines approach is not blocked by a hidden system. It is blocked by what a player sees: repeated predator lines before any mine entrance, road, shelter, or safe region appears.
- Retreating through `1171,1287` remains a clean client-side escape from the northern panther/swamp-drake screen.
- The east test to `1187,1287` shows passive wildlife only. There is still no item, gump, target cursor, visible spawner object, discovery, combat, loot, corpse, hunger/thirst change, quest state, follower-count change, or region text.
- The Mines of Morinia marker is now dx `-165`, dy `85`; the Ruins marker is dx `116`, dy `162`. Both remain world-map overlay knowledge only.

Next pressure:

Mira ends at `Point3D(1187,1287,0)`, facing east. The controlled fox is approximately at `Point3D(1190,1288,0)`, ordered to Follow. The current screen has no visible hostile in the saved live-state snapshot; it has passive animals close enough to route around, including horse serial `200950` at dx `4`, dy `-3`, goat serial `227062` at dx `2`, dy `-1`, and llama serial `227057` at dx `11`, dy `3`. The west panthers and swamp drake are off-screen route risk now, and the cougar is remembered north/east rather than visible. No UI, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 224 - I Take The South Bend Instead

I start at `Point3D(1187,1287,0)`, facing east. The herd around me is noisy but not hostile: goat, horse, llama, moose, hawk, eagle, and the fox following me. The Ruins marker is southeast, but I do not cut straight into it just because the map says so. The first southeast line would run into a grey wolf, and the east line would run me toward a cougar and an elder brown bear. I take the quieter south bend.

**Beat 1**

I run south to the old open pocket around `Point3D(1187,1323,0)`.

The screen stays readable. Deer, eagles, and a horse are visible; no hostile is inside the client rectangle, no corpse or chest appears, no sign or road appears, and no gump or target cursor opens. The fox is still just following, not doing anything clever for me.

**Beat 2**

I keep south to `Point3D(1187,1359,0)`.

That gets me below the wolf line without making a fight out of it. A toad sits west at `Point3D(1173,1360,0)`, dx `-14`, dy `1`; I do not walk west into it, but it is not on top of me. The next real problem is terrain: the east side is the familiar visible tree line around `Point3D(1192,1358,0)`, `Point3D(1192,1357,0)`, and `Point3D(1194,1356,0)`. I stop before turning route correction into blind pathing through trees.

Mechanical friction learned:

- The direct southeast route toward the Ruins marker is not clean from `1187,1287`; it exposes grey wolf serial `445033` near `Point3D(1207,1307,0)`.
- The east-first bypass is worse: cougar serial `200945` and elder brown bear serial `27182` show up close enough around the `1237,1287` line that I will not route through them casually.
- The south bend to `1187,1359` has no immediate 0-10 threat, item, road, shelter, water source, corpse, chest, gump, target cursor, combat, discovery, pet command, or inventory change.
- The Mines marker is now nearly due west, dx `-165`, dy `13`, and the Ruins marker is southeast, dx `116`, dy `90`. Both are still overlay knowledge only.
- The tree line is ordinary visible terrain friction. It does not justify opening C# or inventing a hidden gate.

Next pressure:

Mira ends at `Point3D(1187,1359,0)`, facing south. The controlled fox is approximately nearby and ordered to Follow. Visible route context is a toad west and a tree-blocked east/southeast turn, so the sane next move is finding a gap without drifting west. The grey wolf, cougar, elder brown bear, west panthers, swamp drake, kobold, and goblin are off-screen route risks, not current blockers. No UI, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 225 - I Slip Through The Tree Gap And Meet The East Edge

I start at `Point3D(1187,1359,0)`, facing south. The west side still has the toad at `Point3D(1173,1360,0)`, dx `-14`, dy `1`, so I do not turn the Mines marker into a reason to walk west. The east/southeast tree line is the real screen problem. The visible map overlay still says `Ruins` is southeast at dx `116`, dy `90`; `Mines of Morinia` is nearly due west at dx `-165`, dy `13`; and the West moongate is farther southwest. None of those markers is a road, shelter, water, or a thing standing on the grass.

**Beat 1**

I edge east along the clear row, using the gap just below the trees rather than trying to push through the trunks.

The movement is ordinary terrain correction. The avoided blockers are the tree and peach-tree tiles around `1192,1358`, `1192,1357`, and `1194,1356`, and the pear tree at `1210,1360`. The walked row at `y=1359` stays passable grass/jungle with no blocking static on the route. No gump opens, no target cursor appears, no message fires, and the fox remains a following companion rather than protection.

**Beat 2**

I continue east, then turn south at `x=1225`, aiming the bend toward the Ruins marker without cutting back into the old wolf/bear line.

That is where the screen stops me. At `Point3D(1225,1370,0)`, a panda appears on the east edge at `Point3D(1243,1379,0)`, dx `18`, dy `9`, and a toad appears beside that edge at `Point3D(1243,1382,0)`, dx `18`, dy `12`. The panda reads like passive edge context. The toad is still only a potential threat from the far edge, but pressing east or southeast would close the gap into another timer-creature line. I stop instead of pretending the Ruins marker is a safe road.

Mechanical friction learned:

- The south-first scout from the old stop would immediately bring the crane and second toad back onto the screen, so I avoid that line.
- The east row from `1187,1359` to `1225,1359`, then south to `1225,1370`, is a clean visible-terrain bypass: 49 checked route tiles, zero route blockers.
- The known tree/pear blockers remain ordinary client terrain, not a hidden shard gate.
- The Ruins marker is closer now, dx `78`, dy `79`, but it is still only overlay knowledge. The new real screen fact is the panda/toad edge.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, or inventory mutation explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1225,1370,0)`, facing south. The controlled fox is approximately at `Point3D(1222,1371,0)`, ordered to Follow. Visible edge pressure is east/southeast: panda serial `206400` at dx `18`, dy `9`, and toad serial `8544` at dx `18`, dy `12`. The old west toad, southwest crane/toad, grey wolf, cougar, elder brown bear, west panthers, swamp drake, kobold, and goblin are off-screen route risks, not active blockers. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 226 - I Slip South And Find An Orc Line

I start at `Point3D(1225,1370,0)`, facing south. The real screen problem is still the east edge: the panda at `Point3D(1243,1379,0)` and the toad at `Point3D(1243,1382,0)`. The Ruins marker is southeast at dx `78`, dy `79`, but walking east or southeast would close toward the toad. I choose the less stupid move: go south first and make the toad fall behind the client box.

**Beat 1**

I run south, but not in a straight blind line. The column at `x=1225` has visible tree trouble, so I slide west around it, pass down the `x=1224` side, then step back to `x=1225` once the trees are behind me. The route lands at `Point3D(1225,1401,0)`.

The checked route is grass and jungle at z `0..1`. The route itself avoids the impassable tree at `Point3D(1225,1387,0)` and the impassable tree at `Point3D(1225,1392,0)`. Other nearby leaves, mushrooms, pampas grass, and tree art are visual clutter unless their tile is actually flagged impassable. No gump opens, no target cursor appears, no message fires, no item enters reach, and the fox remains a follower instead of a shield.

The panda and toad are now off-screen to the north/east. That is not safety; it is just distance. The new screen answer is south: an orc named `Urulg` stands at `Point3D(1226,1417,0)`, dx `1`, dy `16`. It is not in my face, but direct south travel would close on a hostile-looking humanoid. I stop before the route becomes combat by accident.

Mechanical friction learned:

- The south bend is ordinary terrain correction around visible tree blockers, not a hidden shard gate.
- The straight `x=1225` line is blocked at `Point3D(1225,1387,0)` and `Point3D(1225,1392,0)`; the west-side wiggle around them stays passable.
- The east-edge panda/toad line drops off-screen by the end of the move, but it remains route memory if I drift back north/east.
- The Ruins marker is now dx `78`, dy `48`. It is still only overlay knowledge, not a visible road, shelter, water source, or safe region.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, or inventory mutation explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1225,1401,0)`, facing south. The controlled fox is approximately at `Point3D(1222,1398,0)`, ordered to Follow. Visible route pressure is south: orc serial `29314`, `Urulg`, at dx `1`, dy `16`. Nearby tree and rock blockers are ordinary jungle terrain; the east-edge panda/toad pair is off-screen route memory, not an active blocker. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 227 - I Bend East And Catch An Archer Edge

I start at `Point3D(1225,1401,0)`, facing south. The orc named `Urulg` is still down the screen at `Point3D(1226,1417,0)`, dx `1`, dy `16`. That is not close enough to force a fight, but walking south or southeast would make it my problem. The Ruins marker is still southeast; the client screen says east is the safer bend.

**Beat 1**

I run east along the open jungle row to `Point3D(1243,1401,0)`.

The route is ordinary movement, not a gate or trick. The stepped row from `1226,1401` through `1243,1401` stays on jungle ground at z `0`. Mushrooms and leaves at `1234,1401`, and tree-and-leaf art at `1241,1401`, are scenery on the route, not blocking statics. No item comes into reach, no gump opens, no target cursor appears, no message fires, and the fox just keeps following.

The orc drops to the southwest edge instead of staying in front of me: dx `-17`, dy `16`. That is better, but not clean. A goblin archer appears on the southern edge at `Point3D(1250,1419,0)`, dx `7`, dy `18`. If I keep pressing south toward the Ruins marker, the archer becomes the next close-range problem. I stop before the map marker talks me into walking into a shot line.

Mechanical friction learned:

- The east bend from `1225,1401` to `1243,1401` is a clear terrain route through jungle, with no blocking route statics.
- Bending east keeps the orc out of the immediate path, but it does not open a safe road south.
- The new real screen pressure is goblin archer serial `26444` at the south edge; the Ruins marker is now dx `60`, dy `48`, still overlay knowledge only.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, or inventory mutation explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1243,1401,0)`, facing east. The controlled fox is approximately at `Point3D(1240,1398,0)`, ordered to Follow. Visible threat pressure is split: orc serial `29314`, `Urulg`, at dx `-17`, dy `16`, and goblin archer serial `26444` at dx `7`, dy `18`. There are no visible saved items in the client box. The sane next move is to decide whether to keep edging east/northeast away from the archer line, pause and inspect the southern pressure, or retreat west before committing toward the Ruins marker. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 228 - I Climb Away From The Archer, Not Into The Toad

I start at `Point3D(1243,1401,0)`, facing east. The orc is no longer in front of me, but it is still visible on the southwest edge, and the goblin archer is still on the southern edge. The Ruins marker keeps pulling southeast, but that would close the archer line. Going straight east would bring a gorilla line into my face. Going straight north would drag the old toad/panda edge back too fast.

**Beat 1**

I run northeast, but only to `Point3D(1251,1393,0)`.

This is a route correction, not a new gate. The diagonal route is `1244,1400`, `1245,1399`, `1246,1398`, `1247,1397`, `1248,1396`, `1249,1395`, `1250,1394`, then `1251,1393`. The checked player diagonal movement tiles have no land blockers. The five route statics are fallen log, morning glory, and mushroom decoration, all non-impassable. No item comes into reach, no visible spawner object appears, no gump opens, no target cursor appears, no message fires, and the fox is still just following.

The move does what I wanted, but only halfway. The orc and archer drop off-screen. The new screen has the old toad at `Point3D(1243,1382,0)`, dx `-8`, dy `-11`, close enough that one more northeast push would pull it into the 0-10 danger band. A gorilla also appears at `Point3D(1265,1402,0)`, dx `14`, dy `9`, so east or southeast is not a clean escape either. A monkey and two pandas are visible context, not a reason to fight.

Mechanical friction learned:

- The northeast bend from `1243,1401` to `1251,1393` is physically passable: 24 player diagonal forward/side-check tiles, zero land blockers, five non-impassable route statics.
- A full northeast run would be too greedy. At the next step, the old toad would enter the immediate 0-10 threat band.
- Bending northeast clears the southern archer and southwest orc from the screen, but it does not open a safe Ruins road.
- The Ruins marker is now dx `52`, dy `56`; Mines of Morinia is dx `-229`, dy `-21`; both remain overlay knowledge only.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, or inventory mutation explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1251,1393,0)`, facing northeast. The controlled fox is approximately at `Point3D(1248,1390,0)`, ordered to Follow. Visible route pressure is split: toad serial `8544` northwest at dx `-8`, dy `-11`, and gorilla serial `206393` southeast at dx `14`, dy `9`. Monkey serial `289478` and pandas serial `206400` and `289468` are visible context. The orc and goblin archer are off-screen route memory now, not current blockers. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 229 - I Break Sideways From The Split

I start at `Point3D(1251,1393,0)`, facing northeast. The screen is not empty enough to keep running. The old toad sits northwest at `Point3D(1243,1382,0)`, close enough that another northeast step would make it immediate. The gorilla sits southeast at `Point3D(1265,1402,0)`, so east and southeast are not clean either. I do not need a script trace for that. It is just what the client is showing me.

**Beat 1**

I stop and read the split instead of pressing into it.

There is no gump, no target cursor, no item, no sign, no road, no corpse, no chest, and no shelter on screen. The monkey and pandas north of me read as wildlife context. The toad and gorilla are the route decision.

**Beat 2**

I run west-southwest to `Point3D(1235,1396,0)`, breaking sideways out of the gorilla line without stepping north toward the toad.

That does what I need: the gorilla falls off the screen, and the southern archer/orc line stays off-screen instead of becoming the next problem. The toad is still visible north-northeast at dx `8`, dy `-14`; the monkey is at dx `14`, dy `-15`; and the closer panda is at dx `8`, dy `-17`. None of them is on top of me, but the next move cannot be blind. The west/southwest side is the same cluttered jungle I already learned, with tree and rock blockers around `1224,1397`, `1224,1402`, `1227,1393`, and `1229,1400`. I stop before turning a route correction into another tree scrape.

Mechanical friction learned:

- The toad/gorilla split at `1251,1393` is solved by a lateral break, not by tarot, a hidden teleporter, or a private helper.
- The west-southwest break clears gorilla serial `206393` from the client rectangle without pulling toad serial `8544` into the immediate 0-10 band.
- The Ruins marker is now dx `68`, dy `53`; Mines of Morinia is dx `-213`, dy `-24`; both are still overlay knowledge only.
- The next friction is ordinary visible jungle terrain plus the north-edge animal line, not a shard-specific no-op or custom gate.

Next pressure:

Mira ends at `Point3D(1235,1396,0)`, facing southwest. The controlled fox is approximately at `Point3D(1238,1397,0)`, ordered to Follow. Visible route pressure is north/northeast: toad serial `8544` at dx `8`, dy `-14`, monkey serial `289478` at dx `14`, dy `-15`, and panda serial `206400` at dx `8`, dy `-17`. The gorilla, orc, goblin archer, and far panda are off-screen route memory now. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 230 - I Thread The Gap And See The Orc Again

I start at `Point3D(1235,1396,0)`, facing southwest. The toad, monkey, and panda are still up north/northeast; none is close enough to force a fight, but turning back that way would be asking for it. The Ruins marker still pulls southeast and the Mines marker still pulls west, but the only thing my client is really showing is jungle clutter and a way to slide away from the toad.

**Beat 1**

I read the screen before moving.

There is no gump, target cursor, sign, road, shelter, water source, chest, corpse, or usable item in the client box. The visible saved world has no item or visible spawner object here. The real choice is whether I can slip around the trees and rocks without pushing south into the old orc/archer line.

**Beat 2**

I run west first, then a little south once I am past the worst of the trees, landing at `Point3D(1219,1399,0)`.

The path is clean enough to feel like normal movement instead of a gate: the checked route from `1235,1396` to `1219,1399` uses 19 stepped tiles, has zero impassable land or static blockers, and only nonblocking jungle decoration on the route. The toad line drops off the screen. That is the good part. The bad part is that Urulg, the orc I was trying not to feed myself to earlier, is visible again on the south edge at `Point3D(1226,1417,0)`, dx `7`, dy `18`. It is not immediate, but direct south travel would make it immediate fast, so I stop.

Mechanical friction learned:

- The west-then-south gap from `1235,1396` to `1219,1399` is ordinary passable jungle, not a hidden shard gate.
- The nearby blockers around `1224,1397`, `1224,1402`, `1227,1393`, and `1229,1400` are still just trees and rocks to route around.
- The move gets the toad, monkey, and panda off-screen, but it brings Urulg back as south-edge route pressure.
- The Ruins marker is now dx `84`, dy `50`; Mines of Morinia is dx `-197`, dy `-27`; West moongate is dx `-167`, dy `171`. These are still overlay markers, not visible roads or safety.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, or inventory mutation explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1219,1399,0)`, facing southwest. The controlled fox is approximately at `Point3D(1222,1400,0)`, ordered to Follow. The visible route pressure is south/southeast: orc serial `29314`, `Urulg`, at dx `7`, dy `18`. The toad, monkey, panda, gorilla, and goblin archer are off-screen route memory now. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 231 - I Slip West Until The Old Toad Shows

I start at `Point3D(1219,1399,0)`, facing southwest. Urulg is not close enough to swing at me, but the orc is exactly the kind of thing that turns a south key into a mistake. The screen has no gump, target cursor, sign, road, shelter, corpse, chest, usable item, or saved world item. The fox is still only following. The sane choice is not to push south toward the Ruins marker; it is to slide west and a little north toward the Mines marker while I still have room.

**Beat 1**

I read the screen and treat Urulg as south-edge pressure, not as a combat target.

There is nothing to click. The map overlay is still louder than the ground: Ruins is southeast, Mines of Morinia is west-northwest, and the West moongate is far southwest. None of those markers is a road or shelter on my screen.

**Beat 2**

I run west and northwest through the jungle, stopping at `Point3D(1192,1391,0)` when the old toad line comes back onto the client edge.

This is a cautious squeeze, not a straight road. The route is `1218,1399`, `1217,1398`, `1216,1397`, `1215,1396`, `1214,1395`, then west and northwest through `1213,1395`, `1212,1395`, `1211,1395`, `1210,1394`, `1209,1393`, `1208,1392`, `1207,1392`, `1206,1392`, `1205,1391`, `1204,1391`, `1203,1390`, `1202,1389`, `1201,1388`, `1200,1388`, `1199,1388`, `1198,1389`, `1197,1389`, `1196,1389`, `1195,1389`, `1194,1389`, `1193,1390`, and finally `1192,1391`. The route check covers 27 stepped tiles and 55 route/diagonal side-check tiles. It finds jungle underfoot, no land blockers, and no impassable route statics. The tree, leaves, mushrooms, and fallen log I pass are scenery, not a new gate.

Urulg drops off-screen. That part works. The stop is the northwest edge: a crane appears at `Point3D(1176,1379,0)`, dx `-16`, dy `-12`, and the toad appears at `Point3D(1174,1379,0)`, dx `-18`, dy `-12`. The crane reads as wildlife context. The toad is the old route pressure again, far enough to avoid but close enough that I do not keep macro-running into it.

The new screen also has a small water-looking patch just east of me around `1195-1197,1390-1392`, plus rocks and trees close enough to make the lane feel cramped. I do not use the canteen here. Thirst is still fine, and this water art is not a proven fill source. The empty canteen stays empty.

Mechanical friction learned:

- The west/northwest squeeze from `1219,1399` to `1192,1391` is ordinary passable jungle once I route around visible clutter.
- The exact path checked 27 stepped tiles and 55 route/diagonal side-check tiles with zero blocking route tiles.
- The move clears Urulg from the screen but brings the old toad/crane line back on the northwest edge.
- A nearby water-looking static patch is visible, but it is not a saved item, not a clicked source yet, and not worth overriding the current route/threat decision.
- The Mines marker is now dx `-170`, dy `-19`; Ruins is dx `111`, dy `58`; West moongate is dx `-140`, dy `179`. These remain overlay navigation only.
- No hidden teleporter, tarot gate, quest gate, combat swing, pet command, discovery flag, region text, gump, target cursor, item pickup, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1192,1391,0)`, facing northwest. The controlled fox is approximately at `Point3D(1195,1393,0)`, ordered to Follow. The visible route pressure is northwest: toad serial `737437` at dx `-18`, dy `-12`, with crane serial `288081` beside it as context. The visible terrain also shows impassable rocks, trees, and water-looking statics close to the lane. Urulg, the toad/monkey/panda line to the east, the gorilla, and the goblin archer are off-screen route memory now. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 232 - The Nearby Water Art Still Will Not Fill The Canteen

I start at `Point3D(1192,1391,0)`, facing northwest, with no gump open and no target cursor. The client screen has the same pressure as the last stop: the toad sits on the northwest edge at `Point3D(1174,1379,0)`, dx `-18`, dy `-12`, with a crane beside it. The fox is close behind me on Follow. There are still no saved visible items, signs, roads, shelters, corpses, chests, vendors, or spawner objects in the live snapshot.

The tempting thing is the water-looking patch just east of me. I have already learned that some shoreline art is fake for the canteen, but this patch is close enough to test without walking toward the toad or the old orc line.

**Beat 1**

I double-click the empty canteen in my backpack.

Nothing targets the ground. The canteen checks nearby water by itself, and the result is the blunt message: `You can only fill this at a water trough, tub, or barrel!`

The reason is mechanical but player-visible enough to matter. `Waterskin.OnDoubleClick` calls `DrinkingFunctions.CheckWater(from, 3, out soaked)`. That helper looks through nearby items and static tiles, but only exact IDs in `CheckWaterTarget` count. The water-looking statics inside this three-tile box include `0x17A6`, `0x17A4`, `0x17A8`, `0x179F`, `0x1797`, `0x17A1`, `0x17A5`, `0x179E`, and `0x17A7`. None is accepted. The canteen stays `empty canteen`, weight `1.0`, and my thirst stays `20`.

I stop there because the game just answered the visible question. The next decision is route choice again, not another canteen click.

Mechanical friction learned:

- The current water-looking patch around `1195-1197,1390-1392` is scenery for the canteen, not a fill source.
- The canteen does not open a target cursor. It runs a three-tile nearby-water check on double-click.
- The failure message points to real sources: water trough, tub, or barrel. None is visible on the current screen.
- No movement, combat, discovery, gump, context menu, target cursor, follower order, vitals, hunger, thirst, or inventory state changed.

Next pressure:

Mira remains at `Point3D(1192,1391,0)`, facing northwest. The controlled fox is still approximately at `Point3D(1195,1393,0)`, ordered to Follow. The toad/crane line remains northwest edge pressure, and the east water art is now tested scenery rather than a possible survival resource. Urulg and the east/northeast animal line remain off-screen route memory. The next client-visible decision is where to move after ruling out the nearby water patch: avoid blind west/northwest into the toad, avoid a careless southeast turn toward Urulg, and pick a cautious bend through the jungle.

## Run 233 - I Reset On The Open Row

I start at `Point3D(1192,1391,0)`, facing northwest, with no gump open and no target cursor. The canteen answer is still sitting in my head: the water-looking patch east of me is scenery for filling purposes. The northwest edge has the toad at `Point3D(1174,1379,0)`, dx `-18`, dy `-12`, with a crane beside it. There are still no saved visible items, roads, signs, shelter, corpses, chests, vendors, or visible spawner objects in the live snapshot.

**Beat 1**

I read the screen before touching the keyboard. West and northwest are not worth it with the toad on that edge. South and southeast are still the old Urulg line. The safer play is not heroic: climb to the clear row I already know, then move east away from the toad and the fake water.

**Beat 2**

I turn north, take the short north leg to `y=1386`, then turn east and run along the open jungle row to `Point3D(1214,1386,0)`.

This is normal movement through clutter, not a shard trick. The checked route is `1192,1390` through `1192,1386`, then `1193,1386` through `1214,1386`. Sosaria is still file index `1`, so the terrain probe reads `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`. All 27 target tiles are dry jungle at z `0`, with zero blocking route tiles. The eight statics on the route are harmless scenery: fern, tree/leaves art that is not impassable on this tile, blade plant, mushrooms, and pampas grass. I do not need to click the canteen again, and no new gump, target cursor, message, item, region text, combat, discovery, hunger, thirst, ownership, or pet-order state appears.

The stop is a chosen reset point. The toad and crane fall off-screen to the west. The old water patch is now behind me. The row ahead is not proven safe forever: the old east animal line and the old south orc/archer line are both route memory, and the visible jungle still has trees and rocks south/east of the row. But the client box itself is quiet.

Mechanical friction learned:

- The north-then-east reset from `1192,1391` to `1214,1386` is passable dense jungle: 27 checked target tiles, zero blocking route tiles, eight nonblocking route statics.
- Moving east along the row clears the northwest toad/crane pressure without walking southeast toward Urulg.
- The tested water art stays behind as scenery. No trough, tub, barrel, or real water source appears on the new screen.
- The Ruins marker is now dx `89`, dy `63`; Mines of Morinia is dx `-192`, dy `-14`; West moongate is dx `-162`, dy `184`. They are still overlay navigation, not visible roads or safety.
- No hidden teleporter, tarot gate, quest gate, custom item no-op, combat swing, pet command, discovery flag, region text, gump, target cursor, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1214,1386,0)`, facing east. The controlled fox is approximately nearby at `Point3D(1211,1387,0)`, ordered to Follow. No hostile or dangerous saved mobile is visible in the endpoint client box. The south/east jungle still carries known ordinary blockers around `1214,1388`, `1214,1390`, `1215,1385`, `1222,1390`, `1225,1387`, and nearby trees/rocks, so the next move is not blind speed. The next client-visible decision is whether to keep edging east toward the Ruins marker or pause and bend around the remembered east animal and south orc/archer lines.

## Run 234 - I Stop When The Toad Reappears

I start at `Point3D(1214,1386,0)`, facing east. There is no gump, no context menu, no target cursor, and no new message. The client box itself is quiet: no saved visible wild mobile, no visible item, no sign, no road, no corpse, no chest, no shelter, no water source, and no visible spawner object. The fox is still just following. The Ruins marker pulls southeast, but the marker is not a road, and the old animal line is still somewhere ahead.

**Beat 1**

I read the quiet row instead of pretending the overlay is ground truth.

The visible problem is not what is on top of me. It is what the next east push will uncover. The known trees and rocks south of the row make a blind southeast bend ugly, and the old Urulg/archer line is still south route memory. So I choose a short east probe, not a sprint.

**Beat 2**

I run east along the same `y=1386` row and stop at `Point3D(1225,1386,0)`.

The route is `1215,1386` through `1225,1386`. Sosaria still resolves to file index `1`, so the terrain probe reads `map1.mul`, `staidx1.mul`, `statics1.mul`, and `tiledata.mul`. All 11 target tiles are jungle at z `0`, with zero blocking route tiles. The only route statics are tree/leaves art at `1216,1386` and a fern at `1223,1386`; neither blocks this row.

The stop is the east edge. The old toad is visible again at `Point3D(1243,1382,0)`, dx `18`, dy `-4`. A panda is beside it at `Point3D(1243,1379,0)`, dx `18`, dy `-7`. The panda looks like context. The toad is far enough not to force a swing, but close enough that continuing east or northeast would be greedy. I stop before the Ruins marker talks me into closing the gap.

Mechanical friction learned:

- The east row from `1214,1386` to `1225,1386` is passable dense jungle: 11 checked target tiles, zero blocking route tiles, and two nonblocking static clusters.
- Moving east makes the remembered toad/panda line visible on the east/northeast edge instead of leaving the screen quiet.
- The Ruins marker is now dx `78`, dy `63`; Mines of Morinia is dx `-203`, dy `-14`; West moongate is dx `-173`, dy `184`. They remain overlay navigation only.
- No hidden teleporter, tarot gate, quest gate, combat swing, item interaction, pet command, discovery flag, region text, gump, target cursor, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1225,1386,0)`, facing east. The controlled fox is approximately at `Point3D(1222,1387,0)`, ordered to Follow. The visible route pressure is east/northeast: toad serial `8544` at dx `18`, dy `-4`, with panda serial `206400` beside it as context. Urulg, the goblin archer, the monkey, and the gorilla are off-screen route memory. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed.

## Run 235 - I Back Off To The Quiet Row

I start at `Point3D(1225,1386,0)`, facing east. The screen is not attacking me, but it is no longer empty: the toad sits at `Point3D(1243,1382,0)`, dx `18`, dy `-4`, and the panda is above it at `Point3D(1243,1379,0)`, dx `18`, dy `-7`. The fox is behind me, still following. The Ruins marker is southeast, but the ground in front of me is an animal edge, not a road.

**Beat 1**

I read the edge and decide not to press east or northeast.

The toad is still potential pressure, not an immediate swing. That makes the right move boring: get space before the marker talks me into a bad line. South has the old Urulg and archer memory. East closes on the toad. West is the proved row I just used.

**Beat 2**

I turn west and run back along `y=1386`, stopping at `Point3D(1207,1386,0)`.

This is retreat, not a new discovery. The row from `1225` back through `1214` and then to `1207` is already proved open jungle/grass, so I do not re-audit every tile. The toad and panda drop off the east edge instead of staying visible. The old west toad line is still too far west to appear here. The endpoint screen has no saved visible wild mobile, no saved visible item, and no visible running spawner object. The fox is only kept in practical follow range, approximately around `Point3D(1204,1387,0)`.

Mechanical friction learned:

- Backing west from the toad/panda edge is a normal client-side escape route, not a hidden gate or code problem.
- `Point3D(1207,1386,0)` is a breathing spot between the east toad/panda line and the old west toad/crane line, but it is not safety.
- The Ruins marker is now dx `96`, dy `63`; Mines of Morinia is dx `-185`, dy `-14`; West moongate is dx `-155`, dy `184`. They are still overlay navigation only.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, gump, target cursor, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1207,1386,0)`, facing west. The controlled fox is approximately at `Point3D(1204,1387,0)`, ordered to Follow. The current client rectangle has no saved visible wild mobile, no saved visible item, and no visible spawner object. The toad/panda line is now off-screen east route memory; the old west toad/crane line is off-screen west route memory. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed. The next client-visible decision is where to go from this quiet row without turning either edge into a close-range problem.

## Run 236 - North Stops At The Wolf Edge

I start at `Point3D(1207,1386,0)`, facing west, with no gump open and no target cursor. The screen itself is quiet: no saved visible wild mobile, no visible item, no road, no sign, no chest, no corpse, no shelter, and no water source. The fox is still the practical follower. The overlay is still pulling in three different directions: Ruins southeast, Mines of Morinia west-northwest, and the West moongate far southwest. None of that is ground truth.

**Beat 1**

I read the quiet row and pick north.

East brings back the toad and panda. West risks the old toad and crane line. South or southeast is the old Urulg and archer memory. North is not safe, but it is the only direction that does not immediately reopen the same pressure.

**Beat 2**

I run north through the grass and jungle, stopping at `Point3D(1207,1325,0)` when a grey wolf appears on the northern edge of the client.

This is not a straight line. Direct north has an impassable rock at `1207,1357` and another at `1207,1328`, so I jog east around both of them: the useful parts are `1207,1386` north to `1207,1358`, east through `1208,1357` to `1208,1354`, back to the north line, then near the end `1207,1331` to `1207,1329`, east through `1208,1328` to `1208,1326`, and back northwest to `1207,1325`.

The terrain check is boring in the useful way: 61 moved steps, zero blocking route tiles, zero statics on the stepped route, no wet land, and only grass with two jungle tiles. One or two land tiles rise to z `1`, but nothing announces a region, teleports me, opens a gump, starts combat, mutates hunger or thirst, or changes the inventory. The fox keeps up as a follower in practical range.

The stop is the new northern screen. A llama, horse, goats, deer, eagles, and a swallow are visible context. The important thing is the grey wolf at `Point3D(1207,1307,0)`, dx `0`, dy `-18`. It is not on top of me, but it is exactly the kind of edge pressure that changes the route decision. I do not keep running into it.

The screen has no saved visible item, no visible spawner object, no trough, no tub, no barrel, and no water static in range. A rock sits close behind at `1207,1328`, with trees and more rocks around the grass lane. That is route-shaping terrain, not a code problem.

Mechanical friction learned:

- The north run from `1207,1386` to `1207,1325` is passable if I sidestep the visible rocks at `1207,1357` and `1207,1328`.
- The route is 61 moved steps with zero blocking route tiles and zero statics on the stepped path.
- The northern animal cluster is passive context until the grey wolf appears at the 18-tile edge.
- The Ruins marker is now dx `96`, dy `124`; Mines of Morinia is dx `-185`, dy `47`; West moongate is dx `-155`, dy `245`. They remain overlay navigation only.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, gump, target cursor, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1207,1325,0)`, facing northwest after the final sidestep. The controlled fox is approximately at `Point3D(1209,1324,0)`, ordered to Follow. The grey wolf at `Point3D(1207,1307,0)` is potential northern threat, not immediate contact. The passive animals around the grass are context, and the old toad, orc, archer, gorilla, and Ruins-side animal lines are now off-screen route memory. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed. The next client-visible decision is whether to back away from the wolf edge, skirt east or west around it, or stop and prepare for a safer route.

## Run 237 - I Give The Wolf Space, Then The Toad Reappears

I start at `Point3D(1207,1325,0)`, facing north, with no gump, no context menu, and no target cursor. The wolf is still the reason my hand is not on the north key: `Point3D(1207,1307,0)`, dx `0`, dy `-18`. The horse, llama, goat, deer, eagles, and swallow are screen context. There is still no visible item, road, sign, corpse, chest, shelter, trough, tub, barrel, water source, or visible spawner object.

**Beat 1**

I read the edge and choose space over bravado. I do not swing at the wolf, and I do not keep running north just because it is only on the edge. The useful move is back through the gap I already learned, around the rock behind me.

**Beat 2**

I retreat south around the `1207,1328` rock and stop at `Point3D(1207,1344,0)`.

The route is the reverse of the last sidestep: `1208,1326`, `1208,1327`, `1208,1328`, then back to the `1207` line from `1329` down to `1344`. The terrain check is clean: 19 moved steps, grass only, zero blocking route tiles, and zero wet or blocking statics. The wolf drops off-screen. The only saved wild mobile left in view is the horse on the northern edge at dx `-11`, dy `-18`, which is not a reason to stop forever.

**Beat 3**

With the wolf gone, I turn west toward the Mines of Morinia marker instead of drifting south toward the old orc/archer memory. I run the short west probe to `Point3D(1189,1344,0)` and stop when the toad shows again.

The west leg from `1207,1344` to `1189,1344` is also clean terrain: 18 moved steps over grass and jungle, zero blocking route tiles, and zero wet or blocking statics. The problem is not the ground. It is the new edge of the screen: a toad at `Point3D(1173,1360,0)`, dx `-16`, dy `16`. That is potential southwest pressure, close enough that I do not keep charging west/southwest toward the Mines marker. The horse remains passive at `Point3D(1196,1326,0)`, dx `7`, dy `-18`. The fox stays in practical follow range.

Mechanical friction learned:

- Backing off the northern wolf edge is normal movement through a known gap, not a custom gate.
- The retreat to `Point3D(1207,1344,0)` is 19 checked steps with no route blockers.
- The west probe to `Point3D(1189,1344,0)` is 18 checked steps with no route blockers.
- Moving toward Mines of Morinia brings a toad onto the southwest edge before the marker becomes useful ground truth.
- Mines of Morinia is now dx `-167`, dy `28`; Ruins is dx `114`, dy `105`; West moongate is dx `-137`, dy `226`. They remain overlay navigation only.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, gump, target cursor, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1189,1344,0)`, facing west. The controlled fox is approximately nearby, ordered to Follow. The toad at `Point3D(1173,1360,0)` is potential southwest threat; the horse at the northern edge is passive context. The old wolf is off-screen north/east route memory, and the old Urulg, goblin archer, gorilla, Ruins-side toad, panda, and monkey lines are still off-screen route memory. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed. The next client-visible decision is whether to back east/northeast from the toad edge, skirt north toward the Mines line, or stop and prepare.

## Run 238 - I Skirt North Of The Toad And Hit A Goblin Edge

I start at `Point3D(1189,1344,0)`, facing west. The screen has a toad on the southwest edge at `Point3D(1173,1360,0)`, dx `-16`, dy `16`, and a horse on the north edge at `Point3D(1196,1326,0)`, dx `7`, dy `-18`. The fox is still only my follower. There is no visible item, no road, no sign, no chest, no corpse, no shelter, no water source, no gump, no context menu, and no target cursor.

**Beat 1**

I do not keep running west-southwest into the toad. I take the boring player move: angle northwest, putting the toad below the screen while still keeping the Mines marker generally ahead.

I stop the first bend at `Point3D(1179,1336,0)`. The toad is gone from the client rectangle by y distance. The horse is still only edge context to the east-northeast, now around dx `17`, dy `-10`. Nothing opens, nothing attacks, and the fox stays close enough on Follow.

**Beat 2**

With the toad off-screen, I try the west line again.

The client answers before this turns into a long run. I stop at `Point3D(1169,1336,0)` when the west edge fills in: a goblin at `Point3D(1151,1341,0)`, dx `-18`, dy `5`, and a boar at `Point3D(1153,1342,0)`, dx `-16`, dy `6`. The goblin is the real route problem. The boar makes the line feel crowded, but I am not pretending either one is already swinging. I just stop before the next west step turns the Mines push into combat.

Mechanical friction learned:

- Skirting northwest from the southwest toad is normal client movement, not a hidden gate or scripted result.
- The toad drops off-screen when I hold the northern line, but the west route immediately exposes a goblin/boar edge before any mine entrance, road, shelter, or resource appears.
- Mines of Morinia is now dx `-147`, dy `36`; Ruins is dx `134`, dy `113`; West moongate is dx `-117`, dy `234`. They remain world-map overlay knowledge only.
- No hidden teleporter, tarot gate, quest gate, item interaction, combat swing, pet command, discovery flag, region text, gump, target cursor, hunger/thirst mutation, or inventory change explains the stop. The client screen does.

Next pressure:

Mira ends at `Point3D(1169,1336,0)`, facing west. The controlled fox is approximately nearby, ordered to Follow. Visible route pressure is now west/southwest: goblin serial `27177` at dx `-18`, dy `5`, and boar serial `288070` at dx `-16`, dy `6`. The toad is off-screen south route memory, the horse is off-screen east/northeast context, and the old wolf, Urulg, archer, gorilla, Ruins-side animals, and northern predator bands remain route memory rather than current blockers. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, or inventory state changed. The next client-visible decision is whether to back east from the goblin edge, bend north harder, or deliberately prepare for a fight.

## Run 239 - I Bend North Instead Of Testing The Goblin Line

I start at `Point3D(1169,1336,0)`, facing west. The screen is not safe enough for another west step: the goblin at `Point3D(1151,1341,0)` is dx `-18`, dy `5`, and the boar at `Point3D(1153,1342,0)` is dx `-16`, dy `6`. Neither one is in my face yet, but the next straight push toward Mines of Morinia would close the line. There is still no visible road, sign, corpse, chest, shelter, water source, gump, context menu, target cursor, or item to click.

**Beat 1**

I choose the less heroic move and turn north. This is not a clever mine approach; it is just making y-distance from the goblin and the boar while the ground still reads as ordinary open grass/jungle.

I run the north line to `Point3D(1169,1318,0)`, then stop because the immediate screen finally clears. The goblin and boar are now below the client rectangle by y distance, not solved or killed. No new saved wild mobile, saved visible item, running spawner object, gump, target cursor, region text, discovery, combat, pet order, hunger/thirst, inventory, ownership, or skill state appears. The fox remains practical follower context, not protection.

Mechanical friction learned:

- Backing away from the goblin/boar edge is ordinary client movement. There is no hidden gate, tarot choice, scripted route helper, or C# destination call involved.
- The west Mines line is still only map pressure. From here Mines of Morinia is dx `-147`, dy `54`; Ruins is dx `134`, dy `131`; West moongate is dx `-117`, dy `252`.
- A quiet client rectangle is not a safety proof. The goblin and boar are off-screen south/southwest route memory, and the older northern predator band is still a reason not to blindly sweep northwest.

Next pressure:

Mira ends at `Point3D(1169,1318,0)`, facing north. The controlled fox is approximately nearby, ordered to Follow. The current screen has no saved wild mobile, saved visible item, visible running spawner object, road, sign, corpse, chest, shelter, water source, gump, context menu, or target cursor. The next honest decision is whether to use this quieter northern rectangle to scout east/northeast, hold and re-scan before any northwest bend, or deliberately retry the Mines line with the goblin/boar memory still behind it.

## Run 240 - East Scout Brings Back The Wolf Edge

I start at `Point3D(1169,1318,0)`, facing north. The client rectangle is quiet: no saved wild mobile, no visible item, no visible spawner object, no road, no sign, no chest, no corpse, no shelter, no water source, no gump, no context menu, and no target cursor. The fox is still just my follower. Mines of Morinia is west-southwest on the overlay, but the remembered goblin and boar are behind that line. The east/northeast side is the cleaner thing to test.

**Beat 1**

I read the empty screen and choose an east scout. I am not trying to path to a marker yet; I just want to see whether the quiet northern rectangle opens into anything safer than the goblin edge.

**Beat 2**

I run east across ordinary open ground to `Point3D(1189,1318,0)` and stop as soon as the eastern/northern screen fills in.

The stop is not terrain. Nothing blocks, opens, teleports, attacks, or changes the inventory. The new screen shows passive animals first: a deer at `Point3D(1194,1306,0)`, eagles at `Point3D(1198,1310,0)` and `Point3D(1196,1314,0)`, another deer at `Point3D(1203,1316,0)`, and the horse at `Point3D(1196,1326,0)`. The important edge is the grey wolf at `Point3D(1207,1307,0)`, dx `18`, dy `-11`. That is only potential threat range, but it is enough to stop the east push before I make it immediate.

Mechanical friction learned:

- The east scout from `1169,1318` to `1189,1318` is ordinary client movement; no C# destination helper, hidden teleporter, tarot gate, region gate, or terrain audit explains it.
- The passive animal cluster east of the quiet row is visible context, not a goal.
- The grey wolf line returns at the far edge before the east scout becomes a safe route.
- From the new stop, Mines of Morinia is dx `-167`, dy `54`; Ruins is dx `114`, dy `131`; West moongate is dx `-137`, dy `252`. These remain overlay navigation only.

Next pressure:

Mira ends at `Point3D(1189,1318,0)`, facing east. The controlled fox is approximately nearby, ordered to Follow. The grey wolf at the far northeast edge is the current route pressure; the deer, eagles, and horse are passive context. The goblin and boar behind the old west/southwest line remain route memory, not current blockers. No UI, context menu, target cursor, combat, discovery, item, ownership, pet-order, vitals, hunger, thirst, skill, or inventory state changed. The next client-visible decision is whether to back west from the wolf edge, bend south below the animal line, or deliberately prepare before testing the northeast again.

## Run 241 - I Skirt Under The Wolf Edge

I start at `Point3D(1189,1318,0)`, facing east. The screen is busy but not immediately on me: deer at `Point3D(1194,1306,0)` and `Point3D(1203,1316,0)`, eagles at `Point3D(1198,1310,0)` and `Point3D(1196,1314,0)`, and the horse at `Point3D(1196,1326,0)`. The grey wolf is the thing that matters, sitting at `Point3D(1207,1307,0)`, dx `18`, dy `-11`. There is still no visible item, road, sign, corpse, chest, shelter, trough, tub, barrel, water source, gump, context menu, or target cursor.

**Beat 1**

I do not press northeast into the wolf. I also do not pretend the Ruins marker is a trail. The normal player move is to make distance first, then see whether the grass opens below the animal line.

**Beat 2**

I run south to `Point3D(1189,1336,0)`. The wolf falls off the client rectangle. The horse stays visible to the north at dx `7`, dy `-10`, but it is still only passive context. Nothing opens, blocks, attacks, targets, or changes in my pack.

**Beat 3**

With the wolf off-screen, I turn east along the lower grass and stop at `Point3D(1207,1336,0)`.

This new screen is quieter in the one way that matters: no visible hostile is forcing my hand. A llama at `Point3D(1208,1323,0)`, a fox at `Point3D(1209,1324,0)`, an eagle at `Point3D(1213,1324,0)`, the horse at `Point3D(1196,1326,0)`, and a swallow at `Point3D(1221,1319,0)` are visible context. The old grey wolf is now off-screen north, not solved. The goblin, boar, and toad lines are behind me as route memory.

Mechanical friction learned:

- Skirting under the grey wolf edge is ordinary client movement, not a hidden teleporter, tarot gate, custom destination helper, or C# result.
- The south leg to `1189,1336` clears the wolf without exposing a new hostile in the client rectangle.
- The east leg to `1207,1336` exposes only passive animals in the saved snapshot.
- From the new stop, Mines of Morinia is dx `-185`, dy `36`; Ruins is dx `96`, dy `113`; West moongate is dx `-155`, dy `234`. They remain overlay navigation only.
- No region text, discovery flag, gump, target cursor, combat, pet command, hunger/thirst mutation, ownership change, item interaction, or inventory change explains the stop. The player stopped because the next route choice is open again.

Next pressure:

Mira ends at `Point3D(1207,1336,0)`, facing east. The controlled fox is still treated as a practical follower on Follow. The visible screen has only passive animals and no item, road, sign, corpse, chest, shelter, water source, gump, context menu, or target cursor. The grey wolf is off-screen north route memory, while the goblin, boar, and toad remain west/southwest memory. The next client-visible decision is whether to continue southeast toward the Ruins overlay marker, cut south to keep distance from the northern wolf band, or back west if the route starts to crowd again.

## Run 242 - The Ruins Line Shows A Panther Edge

I start at `Point3D(1207,1336,0)`, facing east. The screen is quiet enough to move: a llama at `Point3D(1208,1323,0)`, a fox follower close enough to count as practical Follow context, an eagle at `Point3D(1213,1324,0)`, the horse back at `Point3D(1196,1326,0)`, and a swallow at `Point3D(1221,1319,0)`. There is still no visible item, road, sign, corpse, chest, shelter, trough, tub, barrel, water source, gump, context menu, or target cursor.

**Beat 1**

I read the screen and choose the southeast pull. That is not trust in the Ruins marker; it is just the cleanest visible direction that does not drift back into the northern wolf band or the old west goblin line.

**Beat 2**

I run southeast over ordinary open ground to `Point3D(1234,1353,0)` and stop as soon as the next screen gives me a reason to think.

The first new things on the southeast edge are a monkey at `Point3D(1245,1366,0)`, dx `11`, dy `13`, and a panther at `Point3D(1252,1371,0)`, dx `18`, dy `18`. The monkey is context. The panther is the decision. It is not biting me yet, but continuing straight toward the Ruins marker would turn a far-edge threat into a near one.

Mechanical friction learned:

- The southeast move from `1207,1336` to `1234,1353` is ordinary client movement. No terrain failure, hidden teleporter, tarot gate, destination helper, region gate, or C# trace explains the stop.
- The Ruins overlay is closer now, dx `69`, dy `96`, but the visible panther edge outranks the marker.
- The wider southeast animal pocket is starting to show: the boar east of the new rectangle, plus monkey, panda, toad, and panther memory beyond it, means I should not keep pushing blindly along the marker line.
- No region text, discovery flag, gump, target cursor, combat, pet command, hunger/thirst mutation, ownership change, item interaction, or inventory change appears.

Next pressure:

Mira ends at `Point3D(1234,1353,0)`, facing southeast. The fox is still treated as a practical follower on Follow. The current screen has a visible monkey and a far-edge panther, with no visible item, road, sign, corpse, chest, shelter, water source, gump, context menu, or target cursor. The next client-visible decision is whether to back west/northwest from the panther edge, bend away while keeping distance, or deliberately prepare before testing any more southeast movement toward Ruins.

## Run 243 - The Owner Pulls Me To Britain

I start at `Point3D(1234,1353,0)`, facing southeast. The monkey is still southeast at `Point3D(1245,1366,0)`, and the panther is still the thing that makes my hands stop at `Point3D(1252,1371,0)`. There is no road, sign, shelter, water source, chest, corpse, gump, context menu, target cursor, or safe-looking route on the screen. The Ruins marker is only a marker, and every honest direction I have tried has turned into another animal edge.

**Beat 1**

Then the wilderness just stops being where I am.

This is not something I do. I do not click a card, say a word, press a gump button, step into a moongate, use a destination helper, or find a hidden teleporter. The shard owner, invisible to me, recognizes that I am only learning how to flinch away from threats and pulls me out. From my side of the client, the jungle and the panther pressure vanish, and the screen snaps to Britain at `Point3D(2999,1060,0)`.

**Beat 2**

I do the only normal thing after a sudden relocation: stand still and read the new screen.

This is not empty wilderness. People have names and jobs here. Alec the town herald is west-southwest at `Point3D(2984,1050,0)`. Peter the cook is southwest at `Point3D(2982,1073,1)`. Mabel, Franklin, and Blythe look like adventurers around the south side. Markos the fishing trainer is south at `Point3D(2992,1076,0)`. Ryba is high to the northwest, and I can see a training dummy west-northwest. The Bank of Britain sign and public bank door are just east at `Point3D(3015,1056,5)` and `Point3D(3017,1055,5)`.

The world map finally agrees with what the screen is showing: Britain is here, not far away. The nearby overlay is dense with useful names instead of lures through hostile ground: Bank of Britain, Defenders of Sosaria, The Cleaver, The Unicorn's Horn, The Hammer and Anvil, The Bard's Tale, The Grand Clock Makers, Rolling Log Goods, The Best Hides of Britain, Strength and Steel, Profuse Provisions, and The Lord's Clothiers.

Mechanical friction learned:

- The move to Britain is explicit out-of-band shard-owner intervention. It is not a normal player route, tarot branch, visible moongate, hidden teleporter discovery, scripted destination helper, or gump response.
- Mira does not learn why the owner acted. The only player-facing fact is sudden relocation from the panther edge to Britain.
- Britain at `Point3D(2999,1060,0)` gives immediate safety and learning hooks: a herald, trainer, bank, shop signs, adventurer-looking NPCs, and a training dummy.
- No vitals, hunger, thirst, inventory, skills, quest flags, discovery flags, pet order, target cursor, gump, context menu, combat, damage, corpse, loot, or ownership state changes.

Next pressure:

Mira ends at `Point3D(2999,1060,0)`, facing southeast but standing still in Britain. The fox is treated as teleported nearby on Follow. The panther, monkey, boar, panda, toad, wolf, goblin, and old wilderness blockers are off-screen route memory only. The next client-visible decision is no longer wilderness survival; it is choosing a safe town interaction: click Alec the town herald, inspect the Bank of Britain door/sign, talk to Markos the fishing trainer, inspect the training dummy, approach Peter the cook, read nearby shop signs, or single-click the adventurers.

## Run 244 - I Ask The Town Crier What This City Knows

I start at `Point3D(2999,1060,0)`, facing the Britain town screen. There is no hostile on me. The visible pressure is not survival now; it is choosing which safe town thing to trust first. Alec the town crier is northwest at `Point3D(2984,1050,5)`, the Bank of Britain sign and public door are east, Markos the fishing trainer is south, Peter the cook is southwest, and a training dummy is west-northwest. The old panther, wolf, goblin, and boar are only off-screen route memory.

**Beat 1**

I choose Alec because he is a named herald, not a shop door. I run northwest across the town paving toward him and stop close enough to talk, around `Point3D(2987,1052,5)`.

Crossing into his range makes him behave like a town crier. A line of crier chatter fires overhead, but it is not a concrete route, quest, or discovery flag from my side of the screen. It is just city noise unless I decide to follow up.

**Beat 2**

The closer screen is busier than the arrival screen. I can now see the Defenders of Sosaria side of town: Taima the warrior guildmaster, training dummies, training targets, several adventurer-looking people, Ryba high to the north, a visible coffer, and doors into the training building. None of it is hostile. The bank marker is still nearby on the world map, but it has fallen outside the immediate client rectangle.

**Beat 3**

I open Alec's visible context menu entry and choose the talk/news option.

A gump opens with the title `THE NEWS FROM THE TOWN CRIER`. It has a close button and six visible categories: `Deeds in the Realm`, `Exploration in the Realm`, `Victories in Battle`, `Gossip in the Realm`, `Recent Deaths`, and `Wanted Murderers`. The body area on the first page is empty, so the real next choice is not movement. It is whether to press one of the visible category buttons or close the gump.

Mechanical friction learned:

- Alec is a normal visible town NPC interaction, not a tarot route, destination helper, hidden teleporter, or owner action.
- Town heralds talk automatically when a player moves within 10 tiles, then throttle their next shout.
- The visible context-menu talk/news entry opens the town crier news gump because the shard's logging-events system is enabled.
- I did not press any gump category button this run. The gump is open and unread beyond its title and category labels.
- Near Alec, the client rectangle shifts from bank/cook/fishing context to Defenders of Sosaria training context: guildmaster, training mobiles, dummies, coffer, doors, and adventurers.
- No vitals, hunger, thirst, inventory, skills, quest flags, discovery flags, pet order, target cursor, combat, damage, corpse, loot, ownership, or bank state changed.

Next pressure:

Mira ends near Alec at about `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open. The fox is still practical Follow context. The next client-visible decision is to read the open town-crier gump by choosing a visible category, close it, or ignore it and inspect the nearby Defenders/training/coffer screen.

## Run 245 - The Crier Has No Exploration News

I start at `Point3D(2987,1052,5)`, standing beside Alec with `THE NEWS FROM THE TOWN CRIER` already open. This is not a movement problem yet. The screen behind the gump still has the Defenders of Sosaria sign, Taima the warrior guildmaster, training dummies, a coffer, adventurers, doors, Ryba above the street, and no visible hostile forcing me to break away.

**Beat 1**

I press the visible `Exploration in the Realm` category because it sounds like the one that might explain where a lost crash survivor should go next.

**Beat 2**

The gump redraws instead of moving me anywhere. The exploration tab is selected, the same category buttons are still visible, and the body gives me the useful bad news: Alec has no new exploration tales for Mira. I stop there because the result is information, not a route.

Mechanical friction learned:

- The crier news gump is a custom information gump, but this action uses only the visible category label.
- The `Exploration in the Realm` page is backed by the shard's `Info/journies.txt` log. In this checkout that log is absent/empty, so the player-facing result is the default no-exploration-news message.
- Pressing the category closes and reopens the same crier gump on its exploration page. It does not move Mira, discover a place, open a target cursor, change inventory, change skills, or create a route marker.
- No tarot, destination helper, hidden teleporter, context menu, combat, pet order, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira remains at `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open on `Exploration in the Realm`. The page has been read and gave no route help. The next client-visible decision is to choose another visible category, close the gump, or step away from Alec to inspect the Defenders/training/coffer screen.

## Run 246 - The Crier Has No Deeds News

I start at `Point3D(2987,1052,5)`, still beside Alec with `THE NEWS FROM THE TOWN CRIER` open. The gump is the thing in front of me, not the street. Behind it I still know the Defenders of Sosaria sign, training dummies, doors, coffer, guildmaster, adventurer-looking people, and my nearby fox are the current town context. Nothing hostile is forcing me to close the window.

**Beat 1**

I press the visible `Deeds in the Realm` category because exploration was empty and deeds are the next most likely clue category.

**Beat 2**

The gump redraws again. I am still in the same place, with the same category buttons, and now the body reads like a polite dead end: Alec has no new tales of deeds for Mira.

Mechanical friction learned:

- The town-crier category buttons are visible gump controls, not hidden route choices.
- `Deeds in the Realm` reads the shard's `Info/quests.txt` log through the crier system. In this checkout that log is absent/empty, so the player-facing result is the default no-deeds-news message.
- Pressing the category closes and reopens the same crier gump on its deeds page. It does not move Mira, create a target cursor, discover a place, start a quest, change inventory, change skills, or change vitals.
- No tarot, destination helper, hidden teleporter, context menu, combat, pet order, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira remains at `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open on `Deeds in the Realm`. The deeds page has been read and gave no route help. The next client-visible decision is to choose another visible crier category, close the gump, or step away from Alec to inspect the Defenders/training/coffer screen.

## Run 247 - The Crier Has No Battle News

I start at `Point3D(2987,1052,5)`, still beside Alec with `THE NEWS FROM THE TOWN CRIER` open on the deeds page. The gump is still the loudest thing on the screen. Behind it, the Defenders of Sosaria sign, Taima the warrior guildmaster, training dummies, doors, a coffer, adventurer-looking people, Ryba, and my nearby fox remain town context rather than threats.

**Beat 1**

I press the visible `Victories in Battle` category because exploration and deeds have both gone nowhere, and battle news might still be useful in a city full of training dummies and guild people.

**Beat 2**

The crier window redraws in place. I am not moved, targeted, rewarded, or sent anywhere. The selected page now reads `Victories In The Realm`, and the body gives me another polite blank: Alec has no new tales of bravery for Mira.

Mechanical friction learned:

- `Victories in Battle` is a visible crier category, not a hidden destination choice.
- The victories page reads the shard's `Info/battles.txt` log through the crier system. In this checkout that log is absent/empty, so the player-facing result is the default no-battle-news message.
- Pressing the category closes and reopens the same crier gump on its victories page. It does not move Mira, create a target cursor, discover a place, start a quest, change inventory, change skills, or change vitals.
- No tarot, destination helper, hidden teleporter, context menu, combat, pet order, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira remains at `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open on `Victories in Battle`. The victories page has been read and gave no route help. The next client-visible decision is to choose one of the remaining visible crier categories, close the gump, or step away from Alec to inspect the Defenders/training/coffer screen.

## Run 248 - The Crier Has No Gossip News

I start at `Point3D(2987,1052,5)`, still beside Alec with `THE NEWS FROM THE TOWN CRIER` open on the victories page. The window is still the active thing on the screen. Behind it, the Defenders of Sosaria sign, Taima the warrior guildmaster, training dummies, doors, a coffer, adventurer-looking people, Ryba, and my nearby fox remain town context rather than threats.

**Beat 1**

I press the visible `Gossip in the Realm` category because the useful-sounding news pages have all come back empty, and gossip might still point at a local problem or warning.

**Beat 2**

The crier window redraws in place. I am still beside Alec. No target cursor appears, no one hands me anything, and I do not learn a route. The selected page now reads `Gossip In The Realm`, and the body is another dead end: Alec has no new gossip for Mira.

Mechanical friction learned:

- `Gossip in the Realm` is a visible crier category, not a hidden destination choice.
- The gossip page reads the shard's `Info/adventures.txt` log through the crier system. In this checkout that log is absent/empty, so the player-facing result is the default no-gossip message.
- Pressing the category closes and reopens the same crier gump on its gossip page. It does not move Mira, create a target cursor, discover a place, start a quest, change inventory, change skills, or change vitals.
- No tarot, destination helper, hidden teleporter, context menu, combat, pet order, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira remains at `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open on `Gossip in the Realm`. The gossip page has been read and gave no route help. The next client-visible decision is to choose `Recent Deaths`, choose `Wanted Murderers`, close the gump, or step away from Alec to inspect the Defenders/training/coffer screen.

## Run 249 - The Crier Reports No Recent Deaths

I start at `Point3D(2987,1052,5)`, still beside Alec with `THE NEWS FROM THE TOWN CRIER` open on the gossip page. The street behind the window is safe town context: the Defenders of Sosaria sign, Taima the warrior guildmaster, training dummies, doors, a coffer, adventurer-looking people, Ryba, and my nearby fox. Nothing on screen is forcing me to run.

**Beat 1**

I press the visible `Recent Deaths` category because the crier still has one civic report left that might explain danger nearby.

**Beat 2**

The crier window redraws in place. I am not moved, targeted, rewarded, or warned about an active corpse. The selected page now reads `Recent Deaths In The Realm`, and the body is calm instead of useful: Alec says all of Sosaria's citizens are alive and well.

Mechanical friction learned:

- `Recent Deaths` is a visible town-crier category, not a hidden destination choice.
- The deaths page reads the shard's `Info/deaths.txt` log through the crier system. There is no deaths log content in this checkout, so the player-facing result is the default all-alive message.
- Pressing the category closes and reopens the same crier gump on its deaths page. It does not move Mira, create a target cursor, discover a place, start a quest, change inventory, change skills, or change vitals.
- No tarot, destination helper, hidden teleporter, context menu, combat, pet order, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira remains at `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open on `Recent Deaths`. The deaths page has been read and gave no route help. The next client-visible decision is to choose `Wanted Murderers`, close the gump, or step away from Alec to inspect the Defenders/training/coffer screen.

## Run 250 - The Crier Reports No Wanted Murderers

I start at `Point3D(2987,1052,5)`, still beside Alec with `THE NEWS FROM THE TOWN CRIER` open on the recent-deaths page. The town screen behind the gump has the same safe clutter: the Defenders of Sosaria sign, Taima the warrior guildmaster, training dummies, doors, a coffer, adventurer-looking people, Ryba, and my nearby fox. No visible hostile is close enough to interrupt the window.

**Beat 1**

I press the visible `Wanted Murderers` category because it is the last unread crier report on the window.

**Beat 2**

The crier window redraws in place again. I am still beside Alec, no target cursor appears, and nothing on the street changes. The selected page now reads `Murderers In The Realm`, and the body is another civic all-clear: Alec says no one is wanted for murder.

Mechanical friction learned:

- `Wanted Murderers` is a visible town-crier category, not a hidden destination choice.
- The murderers page reads the shard's `Info/murderers.txt` log through the crier system. There is no murderer log content in this checkout, so the player-facing result is the default no-wanted-murderers message.
- Pressing the category closes and reopens the same crier gump on its murderers page. It does not move Mira, create a target cursor, discover a place, start a quest, change inventory, change skills, or change vitals.
- No tarot, destination helper, hidden teleporter, context menu, combat, pet order, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira remains at `Point3D(2987,1052,5)`, facing northwest, with `THE NEWS FROM THE TOWN CRIER` open on `Wanted Murderers`. Every visible crier category has now been opened and read. The next client-visible decision is to close the gump or step away from Alec to inspect the Defenders/training/coffer screen.

## Run 251 - Leaving The Crier Window For The Defenders Sign

I start at `Point3D(2987,1052,5)`, still beside Alec with `THE NEWS FROM THE TOWN CRIER` open on the wanted-murderers page. The page is read, and every visible crier category has already come back empty. The street behind the window is still calm town clutter: my fox, Alec, the Defenders of Sosaria sign to the north, Taima the warrior guildmaster, training dummies, doors, a coffer, adventurer-looking people, and shop markers on the map overlay.

**Beat 1**

I close the crier gump instead of pressing the same categories again. The window disappears. No target cursor appears, no message interrupts me, and Alec stays behind me as a town crier rather than a route.

**Beat 2**

I walk north through the open town floor toward the nearby `Defenders of Sosaria` marker. This is ordinary city movement, not a hidden route test. My fox is still following close enough to count as nearby, and nothing hostile steps into the screen.

**Beat 3**

I stop near `Point3D(2987,1040,5)`, close enough that the Defenders sign is basically next to me. The screen has shifted from civic news to training context: Taima is off to the west, dummies and a daemon target sit around the yard, two coffers are visible in the buildings, and the Hammer and Anvil / blacksmith area is now also inside view. A cat is nearby, but it is just town wildlife.

Mechanical friction learned:

- Closing the town-crier gump only clears the UI. It does not move Mira, discover a place, create a target cursor, change inventory, change vitals, or change the follower order.
- The short northward move to the Defenders marker is routine open-town pathing. No terrain, door, teleporter, or custom movement friction showed up.
- The hidden teleporter object near the western door remains a snapshot clue only. From the player screen, the visible thing is a door/transition edge, not proof of a usable hidden route.
- No tarot, destination helper, context menu, combat, pet command, bank state, hunger, thirst, damage, loot, or ownership state changed.

Next pressure:

Mira is now near `Point3D(2987,1040,5)`, facing north with no gump open. The next client-visible decision is whether to talk to Taima, try a training dummy or daemon target, inspect a coffer/door, or step north toward the Hammer and Anvil blacksmith area.

## Run 252 - Taima Quotes Sword Training

I start at `Point3D(2987,1040,5)`, standing by the `Defenders of Sosaria` sign with no gump open. This is a training-yard screen, not a road problem: Taima the warrior guildmaster is west of me, dummies and a daemon target are scattered around the yard, coffers and doors are visible in the buildings, and the Hammer and Anvil marker sits north. Nothing on screen is hostile.

**Beat 1**

I run west across the open town floor and stop close to Taima at about `Point3D(2975,1041,5)`. The move is short and plain. My fox keeps up as follow-context, and the west side of the screen adds more adventurer-looking people, archery buttes, and another coffer.

**Beat 2**

I open Taima's context menu instead of guessing at hidden speech words. The menu is useful: she is not just scenery. I can see normal vendor choices, a guild-join choice, and a stack of teachable warrior skills such as arms lore, parry, resisting spells, tactics, sword fighting, blunt weapons, and fencing.

**Beat 3**

I choose the visible sword-training line. No training gump opens and no skill jumps immediately. Taima answers like a trainer: she will teach me if I pay the quoted amount in full, and she says less money means less teaching. My backpack still has no gold, so this is a price quote, not a completed lesson.

Mechanical friction learned:

- Taima is a real warrior guildmaster/vendor/trainer interaction, exposed through the normal context menu rather than a special Defenders sign gump.
- A teach option starts a payment quote and leaves the lesson pending. It does not grant the skill until gold is paid.
- The sword-training quote depends on Taima's saved skill value, but the live-state snapshot does not expose that exact skill number. From the player side, the important blocker is simpler: Mira has `0` gold.
- The short move to Taima did not create a target cursor, open a gump, trigger combat, change vitals, discover a place, move through a hidden teleporter, change hunger/thirst, or change the fox's follow order.

Next pressure:

Mira is now near `Point3D(2975,1041,5)`, facing west beside Taima with no gump open. The newest visible lesson is that guildmaster training wants gold first. The next client-visible decision is to find money, use a visible training dummy/daemon for free practice, inspect a coffer or door, ask Taima to buy/sell/join, or step north toward the blacksmith shop.

## Run 253 - The Coffer Wants Snooping First

I start at `Point3D(2975,1041,5)`, still beside Taima with no gump open. Her paid sword-training quote is fresh in my head, and my backpack still has no gold. The screen is busy but calm: my fox is close, Taima is two steps west, the Defenders sign is behind me, training targets and doors sit around the yard, the Hammer and Anvil marker is north, and a coffer is visible just north of where I am standing.

**Beat 1**

I try the nearby coffer because it looks like the most direct answer to "I need money." I do not get a loot pack, a target cursor, a crime warning, or a pile of coins. The coffer answers with a skill wall instead: I should probably get better at snooping first.

Mechanical friction learned:

- The visible coffer is not a normal free chest. It is a custom thief coffer, so double-clicking it probes the snooping/stealing flow instead of opening a container.
- From the player side, the first blocker is simple: Mira's Snooping is still effectively untrained, so the coffer refuses even the peek step.
- The traced mechanic is sharper than the message: a successful snoop can reveal about how much gold is inside, and the later theft step is a separate Stealing action. Mira did not reach either of those steps.
- No gold moved, no item appeared, no gump opened, no target cursor appeared, no criminal/wanted state changed, no combat started, no skill gain was visible, and the fox's follow order did not change.

Next pressure:

Mira remains at `Point3D(2975,1041,5)`, facing west beside Taima with no UI open. The newest visible lesson is that the obvious coffer-money route is gated by Snooping before it is gated by Stealing. The next client-visible decision is to find a legal money source, look for Snooping or thief training, try free weapon practice on a visible target, use Taima's other context choices, or walk north toward the blacksmith shop.

## Run 254 - I Follow The Bank Marker Instead Of Picking The Coffer Again

I start at `Point3D(2975,1041,5)`, beside Taima with no gump open. The coffer just told me to get better at snooping, and I still have no gold for Taima's sword lesson. The training-yard screen is still calm: fox nearby, guildmaster beside me, Defenders sign behind me, Hammer and Anvil north, training targets and doors in the building slices, and no hostile inside my immediate range.

**Beat 1**

I stop trying to make the coffer be a free chest and run east/southeast through open Britain streets toward the `Bank of Britain` marker on the world map overlay. This is normal town movement, not a hidden route test. I keep the route on the city floor and do not enter any doors along the way.

I reach the bank exterior around `Point3D(3015,1056,5)`. The screen is quieter than the Defenders yard: the `Bank of Britain` sign is right there, the public bank door is just east/northeast, and a trash barrel sits nearby. I can see a bard named Merele at the north edge and a couple of adventurers to the south. The only thing that makes me tense is a ruby elemental-looking humanoid about fifteen tiles south; it is not on top of me, but I do not want to drift that way while trying to find a banker.

Mechanical friction learned:

- The bank marker leads me to the public exterior door/sign, not directly into an open bank box.
- No banker is visible from the exterior tile yet, so the next normal action is to open or enter the public bank door before trying bank speech or bank-box assumptions.
- The ruby elemental-looking mobile is a potential south-side route risk, not an immediate blocker to the bank door.
- No tarot, destination helper, hidden teleporter, gump, target cursor, combat, pet order, inventory, gold, skill, hunger, thirst, damage, discovery flag, criminal state, or bank state changed.

Next pressure:

Mira is at the Bank of Britain exterior near `Point3D(3015,1056,5)`, facing the public bank door with no UI open. The next client-visible decision is to open/enter the bank door, inspect the sign or barrel, or back away from the south-side ruby elemental if it moves closer.

## Run 255 - The Bank Door Drops Me Into A Vault Room

I start at `Point3D(3015,1056,5)`, facing the visible public `Bank of Britain` door with no UI open. The bank sign is under my feet, the door is just east/northeast, and the trash barrel is close by. Merele the bard is still barely on the north edge, Vanya and Franklin are south, and the ruby elemental-looking humanoid is still far enough south to be a route risk instead of a door blocker.

**Beat 1**

I use the visible public bank door. I do not get a banker window, a bank box, a target cursor, or a warning. The client snaps me into a different interior room around `Point3D(3446,3442,0)`.

**Beat 2**

I stop because the new screen is dense with money systems. There are `Bank Vault` chests right next to me, more vaults and trash barrels along the east side, donation boxes and public boards on the west wall, the `Merchant Advertiser`, `Laws of the Land`, and `News From The Nobles`. Susan the minter is north of me, Bridget the bard is west, and several adventurer-looking people are scattered around the room. No hostile is visible, and the ruby elemental from outside is no longer on screen.

Mechanical friction learned:

- The exterior `Bank of Britain` door is a normal visible transition into a shared bank-vault room. It does not directly open Mira's bank box.
- The interior is not marked by a nearby world-map overlay marker; the useful marker was the outside `Bank of Britain` marker that led to the door.
- The nearby `Bank Vault` chests are real bank access controls, not decoration: the traced `BankChest` double-click path opens the player's personal bank box when used within four tiles, and otherwise only says it is too far away.
- I have not opened the vault yet, so the bank-box contents are still player-unknown.
- No tarot, hidden destination helper, forged gump response, context menu, combat, pet command, inventory, gold, skill, hunger, thirst, damage, discovery flag, criminal state, or bank-box contents changed.

Next pressure:

Mira is now inside the Bank of Britain vault room at `Point3D(3446,3442,0)`, facing east with no gump open. The next client-visible decision is to double-click the nearest `Bank Vault`, talk to or context-click Susan the minter, read one of the public boards, inspect the donation boxes, or leave through the visible doors if the bank route stalls.

## Run 256 - The Vault Opens Empty

I start inside the Bank of Britain vault room at `Point3D(3446,3442,0)`, no gump open, no target cursor, and no hostile visible. The nearest useful thing is obvious: a `Bank Vault` chest sits four tiles west of me at `Point3D(3442,3442,0)`. Susan the minter is north, the public boards and donation boxes are west, and the exit doors are behind me, but the vault is the first thing that looks like it might solve the money problem.

**Beat 1**

I double-click the nearest `Bank Vault`.

It works from this exact spot. I do not get the "too far away" message, and I do not have to talk to Susan first. A standard bank storage container opens, and the overhead message is blunt: `Your bank storage has 0 items, 0 stones`.

**Beat 2**

I look over the open bank box instead of immediately clicking another object through it. There are no visible coins, checks, starter tools, deeds, spare clothes, or hidden rescue supplies in the box. This bank route proves I can access storage, but it does not solve Taima's unpaid training quote or give me legal cash.

Mechanical friction learned:

- The nearest `Bank Vault` is usable from `Point3D(3446,3442,0)` because the chest is exactly four tiles away.
- A `Bank Vault` opens the player's personal bank box directly. It is not decoration and does not require the minter if I am close enough.
- The bank box is currently empty from the client view: `0 items, 0 stones`.
- Opening the vault did not move Mira, close the room, create a target cursor, start a conversation, change gold, change inventory, change skills, change vitals, mark a discovery, flag a crime, or change the fox's follow order.

Next pressure:

Mira remains at `Point3D(3446,3442,0)` in the bank-vault room with the empty bank storage container open. The next client-visible decision is to close or ignore the empty bank box, then try Susan the minter's context/speech options, ask a bank balance question, read the public boards, inspect donation boxes, or leave through the visible doors.

## Run 257 - Susan Confirms The Bank Is Still Empty

I start at `Point3D(3446,3442,0)` in the shared bank-vault room, facing east with the empty bank storage container still open. Susan the minter is north of me, close enough to hear bank speech but not close enough that I want to pretend every context-menu choice is usable from here. No hostile is on screen.

**Beat 1**

I close the empty bank storage container.

Nothing spills out and nothing changes in my pack. The last useful fact from the box remains the same: `0 items, 0 stones`.

**Beat 2**

I say `balance` toward Susan.

She answers from the north side of the room: my current bank balance is `0` gold. That removes the last hopeful loophole from the vault route. The box was not hiding a check, and Susan does not see any stored gold either.

**Beat 3**

I walk north/northwest through the open bank room to stand near Susan at about `Point3D(3444,3435,0)`, facing north.

The move is short interior floor movement. No door, wall, target cursor, gump, warning, discovery, or combat interrupts it. Up close, Susan is the obvious next interaction. The screen now also shows more bank-room clutter: vaults, trash barrels, donation boxes, public boards, vote stones, `Local Guilds`, `Sage Advice`, and the quest boards north and west. None of those are money in my pack.

Mechanical friction learned:

- Saying `balance` to Susan works from the original spot because banker speech is heard within the normal bank-speech range.
- Susan reports `0` bank gold, matching the empty bank box result.
- The bank route gives legal storage access, but not starter money for Taima's sword-training quote.
- Susan is now within close interaction range for the next normal action; I have not opened her context menu, buy list, sell list, talk gump, or bank box from her yet.
- No tarot, hidden destination helper, forged gump response, item transfer, inventory change, skill change, hunger/thirst change, damage, crime flag, follower order change, or discovery flag changed.

Next pressure:

Mira ends at `Point3D(3444,3435,0)`, facing north beside Susan the minter with no gump open and no target cursor. The next client-visible decision is to context-click Susan, choose a visible Talk/Buy/Sell/Open Bankbox option, read one of the now-visible public boards, inspect donation boxes/vote stones, or leave through the visible doors.

## Run 258 - Susan Explains What A Minter Actually Wants

I start at `Point3D(3444,3435,0)`, facing north beside Susan the minter. No gump or target cursor is open. The bank-vault room is still calm: Susan is three tiles north, my fox is beside me, adventurers and Bridget the bard are scattered around the room, and the nearby clutter includes vault chests, vote stones, donation boxes, public boards, trash barrels, and even a barbaric satchel sitting by an east-side vault. No hostile is inside the screen.

**Beat 1**

I context-click Susan instead of guessing another bank keyword.

The context menu is useful but not magic money. I can see bank/vendor choices: open the bank box, buy, sell, and talk. The bank-box route is already empty, and I have no gold for buying, so `Talk` is the only option that might explain what Susan does.

**Beat 2**

I choose `Talk`.

A small information gump opens with the title `Copper and Silver Coins`. Susan does not open a trade list or hand me anything. There is no target cursor, no bank box, no pack change, and no coin appears.

**Beat 3**

I read the gump before pressing its close button.

The useful player-facing lesson is narrower than the wall of text: Susan is a currency exchanger. She wants old coin stacks and loot-currency piles, not empty hands. She says copper and silver convert at poor rates, jewels and unusual gemstones can become gold, crystals are valuable, and green coins from an ancient sky-fallen castle are also exchangeable. She also sells safes, but that is irrelevant while I have `0` gold.

Mechanical friction learned:

- Susan's visible context menu path confirms she is both banker and vendor: `Open Bankbox`, `Buy`, `Sell`, and `Talk` are available from close range.
- The `Talk` choice opens a read-only `SpeechGump` titled `Copper and Silver Coins`; it teaches currency-conversion clues but does not create money, a quest flag, a discovery flag, a target cursor, or a bank container.
- The practical bank/minter lesson is that I need to bring Susan convertible loot first: copper, silver, jewels, gemstones, crystals, gold nuggets, or green xormite-style coins. Nothing in my current backpack qualifies.
- The visible bank room still has boards, vote stones, donation boxes, vaults, trash barrels, a barbaric satchel, and ordinary doors, but none of them changed during this interaction.

Next pressure:

Mira remains at `Point3D(3444,3435,0)`, facing north beside Susan with the `Copper and Silver Coins` gump open and already read. The next client-visible decision is to close the gump, then either inspect the unusual satchel/boards in the room, try Susan's buy/sell lists despite having no money, or leave the bank to look for convertible coin loot instead of expecting starter gold.

## Run 259 - The Barbaric Satchel Erases Itself

I start at `Point3D(3444,3435,0)`, facing north beside Susan with her `Copper and Silver Coins` gump still open but already read. I am not in combat, no target cursor is up, and the room still looks like a bank-vault clutter pile: Susan north of me, my fox beside me, adventurers around the room, boards and vote stones on the walls, vault chests to the east and south, and the odd `barbaric satchel` sitting on the east-side vault.

**Beat 1**

I close Susan's gump.

Nothing spills out of it. No bank box opens, no trade list appears, no target cursor appears, and no money changes. I am back to the raw room view.

**Beat 2**

I try the visible barbaric satchel because it is the weirdest object in the room and it looks like it might be a container.

It is not a container for me. It does not open. I do not get the barbaric information gump, a loot window, a "too far away" message, a theft warning, or a target cursor. The satchel simply disappears from the client view.

Mechanical friction learned:

- The satchel is player-facing bait unless it belongs to the character. For Mira, the visible world satchel is not loot, not storage, and not a starter gear source.
- The custom satchel double-click path checks ownership before it checks the normal in-range gump branch. A non-owner touching a weighted satchel makes it delete, which is why the client just loses the object instead of showing a useful window.
- The owner/in-range branch would have opened a `BARBARIC SATCHEL` explanation and alteration UI, but Mira never reaches that branch from this world-placed satchel.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, pet order, or position changed.

Next pressure:

Mira remains at `Point3D(3444,3435,0)`, facing north beside Susan with no UI open and no target cursor. The suspicious satchel is gone. The next client-visible decision is to read one of the public boards, test Susan's buy/sell lists despite having no gold, inspect donation boxes or vote stones, or leave the bank to look for real currency loot.

## Run 260 - I Read The Sage Board

I start at `Point3D(3444,3435,0)`, facing north beside Susan. No gump is open, no target cursor is up, and the room is still the same calm bank interior: Susan to the north, my fox close by, adventurer-looking people scattered around, vaults and trash barrels to the east, public boards on the north and west walls, vote stones nearby, and ordinary exit doors to the south. The satchel is gone, so I stop treating the bank clutter like it might be a container route.

**Beat 1**

I walk northwest through the open bank floor to stand under the north-wall boards at about `Point3D(3439,3425,0)`, facing north.

This is short interior movement, not exploration. No hostile enters the screen, no door or wall stops me, no target cursor appears, and no discovery or region message fires. From here `Sage Advice` is close enough to use; the quest boards, `Local Guilds`, and the invisible `Dio's Door` teleporter location are nearby, but only the visible board matters to the player.

**Beat 2**

I double-click `Sage Advice`.

The board answers with a `SAGE ADVICE` gump instead of a quest, vendor list, target cursor, or free item. It has a close button in the upper-right, a scrollable body, and no visible choice that would spend money or accept a quest from this first read.

**Beat 3**

I read the gump and do not press anything else.

The useful lesson is expensive artifact hunting. Sages sell artifact encyclopedia advice for thousands of gold, better lore levels cost more, the book lets me choose an artifact, and the torn page gives the first clue and coordinates. The gump also warns that the search can cross lands I may not know, expects a sextant, and may end in gold, a false legend, a fake artifact, or another clue. For Mira right now, with `0` gold and no sextant, this board is knowledge, not a route out of poverty.

Mechanical friction learned:

- The north-wall `Sage Advice` board is a normal double-click board once Mira is within four tiles.
- Its first response is read-only `BoardGump` information titled `SAGE ADVICE`; it does not start an artifact quest, spend gold, create an artifact book, open a vendor list, create a target cursor, or change inventory.
- Artifact advice is gated by future money and probably travel tools: the board talks in `5,000` to `10,000` gold terms and says coordinates matter, so this is not a starter-cash answer.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, pet order, or follower count changed.

Next pressure:

Mira ends at `Point3D(3439,3425,0)`, facing north with the `SAGE ADVICE` gump open and read. The next client-visible decision is to close it, then read another nearby board, context-click a quest board, inspect the visible vote stones, or leave the bank because the sage route needs money she does not have.

## Run 261 - Local Guilds Want Cash Too

I start at `Point3D(3439,3425,0)`, facing north under the bank's north-wall boards, with the `SAGE ADVICE` gump open and already read. The room is still safe-looking: fox at my feet by continuity, bankers and adventurer types around the room, Susan behind me, quest boards and `Local Guilds` on the same wall, vote stones to the east, donation boxes and other public boards west, and no hostile on the screen.

**Beat 1**

I close `SAGE ADVICE`.

Nothing else opens. No money appears, no target cursor appears, and I am still standing under the boards.

**Beat 2**

I double-click `Local Guilds`, two tiles east and four tiles north of me.

It opens a `LOCAL GUILDS` window, not a guild-join prompt. I do not have to move. The visible controls are just the upper-right close button and the scrollable text body; because I am not in a local guild, there is no visible resign choice for me.

**Beat 3**

I read it and stop.

The board lists the local trade/skill guilds and explains the catch: joining means finding the right guildmaster, choosing `Join`, then dropping the exact entry fee on them. The first entry fee is `2,000` gold, later guilds double in cost, replacement rings cost `400` gold, and theft against other players requires the Thieves Guild. For Mira, this is another useful rule sheet, not a starter-money route.

Mechanical friction learned:

- `Local Guilds` is usable from the current tile because the board is within four tiles.
- Double-clicking it opens `GuildBoardGump`; it lists guilds and membership rules but does not auto-join, open a vendor list, create a target cursor, or change money, skills, inventory, crime state, or discovery flags.
- Guild joining is not the same as paid skill training. A guildmaster's visible `Join` action still needs the exact fee, and Mira has `0` gold.
- The board can expose a resign control only for a player already in a local guild. Mira is not in one, so this read shows no useful button beyond close.

Next pressure:

Mira remains at `Point3D(3439,3425,0)`, facing north with the `LOCAL GUILDS` gump open and read. The next client-visible decision is to close it, then read a quest board, inspect vote stones or donation boxes, or leave the bank because both artifact advice and guild membership require money she does not have.

## Run 262 - I Step Over To The Adventurer Board

I start at `Point3D(3439,3425,0)`, facing north under the bank's north-wall boards, with the `LOCAL GUILDS` window still open and already read. The screen is still a safe-looking bank interior: my fox is treated as following close by, bankers and adventurer-looking people are scattered around, the standard and fishing quest boards are west of me on the same wall, vote stones and bank vaults sit east, donation boxes and public boards sit west and south, and no hostile is on screen.

**Beat 1**

I close `LOCAL GUILDS`.

The board does not turn into a join prompt or payment target. No money, ring, skill change, inventory item, or target cursor appears. I am back to the room view.

**Beat 2**

I walk a few steps west along the open bank floor to stand under the quest boards at about `Point3D(3434,3425,0)`, still facing north.

This is ordinary interior movement. Nothing blocks the floor, no door opens, no region message appears, no hostile enters the screen, and the fox is just assumed to keep up. From here both `Seeking Brave Adventurers` and `Seeking Brave Sailors` are close enough to use.

**Beat 3**

I double-click `Seeking Brave Adventurers`.

It opens a `SEEKING BRAVE ADVENTURERS` board window instead of immediately assigning a bounty. I stop on the new unread gump. The visible controls are the usual board-window close button and a text body; I have not opened the board's context menu or chosen any hire/done path.

Mechanical friction learned:

- `Seeking Brave Adventurers` is not usable from the old `Point3D(3439,3425,0)` spot because it is seven tiles west and four north, but it is usable after stepping to `Point3D(3434,3425,0)`.
- Normal double-click opens a read-only `BoardGump` titled `SEEKING BRAVE ADVENTURERS`; it does not by itself start a quest, pay gold, create loot, open a target cursor, or mutate the quest log.
- The actual quest-taking flow is not proven by the double-click text window. I have only opened the information gump, so no hire/done choice should be assumed until I use a visible context-menu path.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, pet order, or follower count changed.

Next pressure:

Mira ends at `Point3D(3434,3425,0)`, facing north with the `SEEKING BRAVE ADVENTURERS` gump open and unread. The next client-visible decision is to read the new quest-board text before deciding whether to context-click the board for a visible hire/done option, inspect the sailor board, or leave the bank.

## Run 263 - I Read The Adventurer Board, Then Only Open Its Menu

I start at `Point3D(3434,3425,0)`, facing north with `SEEKING BRAVE ADVENTURERS` already open and unread. The bank screen is unchanged enough to trust: my fox is treated as close by, Darlene and Jebidiah stand north, Lionel and Greger are near the floor behind me, Susan and the bankers are still bank-room context, and the north wall has the adventurer board, sailor board, sage board, and guild board. Vote stones sit to the east, donation boxes and public boards sit west and south, and no hostile is visible.

**Beat 1**

I read the `SEEKING BRAVE ADVENTURERS` window instead of pressing anything.

The board is not a job offer yet. It tells me the townsfolk give bounties for kills or retrievals, one quest has to be finished before another, failed quests may require reparations, and fame improves the chance of a better-paying bounty. The useful warning is travel: these quests do not send me to lands I have never been, but they can send me into any dungeon in lands I have traveled. The board also says the quest log is `[quests`, rewards are gold and fame, and karma can move when the quest is done.

**Beat 2**

I close the board window.

Nothing else appears. No quest log opens, no target cursor appears, no item or gold lands in my pack, and no overhead message says I accepted anything. I am just back under the board.

**Beat 3**

I context-click `Seeking Brave Adventurers`.

Now I get the actual decision surface. The context menu gives me a read/talk entry plus the quest action and the completion action the board text described as asking to be `hire`d and returning when `done`. I stop there, because seeing the menu is not the same as choosing a quest. No row has been clicked.

Mechanical friction learned:

- The double-click gump is only instructions. Reading and closing it does not start a standard quest, set a target, open the quest log, pay gold, or change Mira's pack.
- The real standard-quest flow is exposed by the board context menu after the gump is closed. From the current tile, the board is still close enough for that menu.
- The menu proves there are separate information, take-quest, and turn-in actions, but no `StandardQuest` state should be assumed until I choose the quest action.
- The board warns that a normal starter quest can point into any dungeon in a land I have already traveled. Since Mira has only Sosaria discovered, accepting one may still send her into dangerous known-land dungeon travel rather than a safe town errand.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, pet order, or follower count changed.

Next pressure:

Mira remains at `Point3D(3434,3425,0)`, facing north with no gump open and the `Seeking Brave Adventurers` context menu open. The next client-visible decision is to choose the visible quest action if I am willing to risk a random standard bounty, choose the completion action just to confirm I have no active quest, close the menu and read the sailor board, or leave the bank.

## Run 264 - The Done Row Does Nothing, Then I Read Sailors

I start at `Point3D(3434,3425,0)`, facing north under the bank's north-wall boards. No gump is open, but the `Seeking Brave Adventurers` context menu is still open from the last run. The screen is a calm bank interior: adventurer-looking people and bankers are still in the room, the standard and sailor quest boards are both within reach, donation boxes and public boards sit to the west and south, vote stones sit east, and no hostile is visible.

**Beat 1**

I choose the visible `done` row on `Seeking Brave Adventurers`.

The board gives only an overhead response: `You are not currently on a quest.` It does not open a reputation-payment gump, take money, create a target cursor, or change my pack. The context menu is gone after the click.

**Beat 2**

I double-click the adjacent `Seeking Brave Sailors` board.

It opens a `SEEKING BRAVE SAILORS` board window. The visible controls are the same simple board-window close button and a text body; I do not get a quest just from the double-click.

**Beat 3**

I read the sailor board and stop.

The sailor version is a sea bounty board: pirates, sea creatures, or retrieved items; one quest at a time; reparations if I fail; fame can improve the payout; the quest log is still `[quests`; and the board says these jobs do not send me to seas I have never been. For Mira right now, this is information only. I still have no boat, no gold, no active sailor quest, and no reason to assume the ocean route is safer than the land bounty route.

Mechanical friction learned:

- The standard board's `done` action is safe to test with no active quest. It only says I am not currently on a quest and leaves money, inventory, target cursor, and quest state unchanged.
- `Seeking Brave Sailors` mirrors the adventurer board's split flow: double-clicking opens read-only instructions, while the actual hire/done choices live behind the context menu.
- Sailor quests are not a starter-cash shortcut from this bank tile. The board teaches the route, but accepting one would still be a random quest action and likely needs sea access.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, pet order, or follower count changed.

Next pressure:

Mira remains at `Point3D(3434,3425,0)`, facing north with the `SEEKING BRAVE SAILORS` gump open and read. The next client-visible decision is to close it, context-click the sailor board for its visible hire/done choices, return to the adventurer board and accept a land bounty, inspect the nearby public boards and donation boxes, or leave the bank.

## Run 265 - Sailor Board Menu, Not A Quest Yet

I start at `Point3D(3434,3425,0)`, facing north under the bank's north-wall quest boards. The `SEEKING BRAVE SAILORS` gump is still open and already read. The room still looks safe: adventurers and bankers are scattered around the vault, Susan is behind and east, public boards and donation boxes sit along the west wall, vote stones are east, doors are south, and no hostile is on the screen.

**Beat 1**

I close `SEEKING BRAVE SAILORS`.

The gump disappears without handing me a quest. No target cursor appears, no quest log opens, no item or gold drops into my pack, and the board is still on the north wall.

**Beat 2**

I context-click `Seeking Brave Sailors`.

The actual decision menu appears. I can see the same split the board text implied: a read/talk entry, a `hire` quest entry, and a `done` completion entry. I do not click a row yet, because opening the menu is only exposing choices, not accepting a random sea job.

**Beat 3**

I pause on the menu and think it through.

The `hire` row is the first real mutation path for a sailor quest, but it may produce a random sea target and I still have no boat, no gold, and no active fishing quest. The `done` row is probably harmless the way the adventurer board was, but it would only confirm I am not on a sailor quest. I stop with the menu open instead of pretending I know which job the board would roll.

Mechanical friction learned:

- The sailor board's double-click text is separate from its context-menu action surface.
- From this tile, `Seeking Brave Sailors` is close enough to context-click after closing the information gump.
- The visible context menu exposes separate read/talk, `hire`, and `done` choices, but no fishing quest state changes until one of the quest rows is actually selected.
- No movement, gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, or follower count changed.

Next pressure:

Mira remains at `Point3D(3434,3425,0)`, facing north with no gump open and the `Seeking Brave Sailors` context menu open. The next client-visible decision is to choose `hire` if I am willing to accept a random sailor job, choose `done` to test the no-active-sailor-quest response, close the menu and take a land bounty instead, inspect nearby public boards and donation boxes, or leave the bank.

## Run 266 - I Clear The Sailor Menu And Open The Laws

I start at `Point3D(3434,3425,0)`, facing north under the bank's north-wall quest boards, with the `Seeking Brave Sailors` context menu still open. The room is still a bank interior, not a road or dungeon: my fox is treated as following close by, Lionel and Greger are near me, Susan and Bridget are south of the boards, the public boards and donation boxes are along the west wall, vote stones and vaults sit east, and no hostile is visible.

**Beat 1**

I choose the visible `done` row on `Seeking Brave Sailors`.

The board answers exactly like the adventurer board did: `You are not currently on a quest.` No reputation-payment gump opens, no target cursor appears, no gold or item moves, and no fishing quest is created. The sailor context menu is gone after the click.

**Beat 2**

I walk south along the open bank floor toward the west-wall public boards and stop near `Point3D(3434,3435,0)`, facing west toward `Laws of the Land`.

This is short interior movement, not a route puzzle. The north-wall quest boards slide toward the top edge of the screen, the west-wall donation boxes and public boards become the obvious next interactives, and no wall, door, warning, hostile, target cursor, or region change interrupts me.

**Beat 3**

I double-click `Laws of the Land`.

It opens a `LAWS OF THE LAND` board window. I stop on the newly opened text instead of clicking away or using another object. The visible text starts as a rules list, with illegal acts and acts the civilized dislike, but I have not taken the next beat to read and think through it yet.

Mechanical friction learned:

- The sailor board's `done` row is another safe no-active-quest check. With no fishing quest, it only gives the overhead `You are not currently on a quest.` response.
- I did not choose `hire`, so no random sea quest target, fishing quest cooldown, quest log state, target cursor, gold, item, fame, karma, hunger, thirst, damage, crime flag, discovery flag, pet order, or follower count changed.
- Short movement from the north-wall quest boards to the west-wall rules board is ordinary open bank-floor travel. No terrain or binary-map check was needed.
- `Laws of the Land` is usable from the new tile and opens a read-only `BoardGump`; the player-facing next step is reading it, not treating the rules as already assimilated.

Next pressure:

Mira ends near `Point3D(3434,3435,0)`, facing west with the `LAWS OF THE LAND` gump open and unread. The next client-visible decision is to read the rules text before closing it, then decide whether to inspect `News From The Nobles`, the nearby donation boxes, the merchant advertiser, the status board, the quest boards, or the south doors.

## Run 267 - I Read The Laws, Then Open The News Board

I start at `Point3D(3434,3435,0)`, facing west beside the bank's west-wall public boards. `LAWS OF THE LAND` is already open and unread. The room is still quiet bank space: Bridget stands a couple steps away, Greger and Lionel are behind me, Susan is northeast, public boards, donation boxes, trash barrels, vaults, vote stones, and south doors are visible, and no hostile is on screen.

**Beat 1**

I read `LAWS OF THE LAND`.

The rules are blunt. Murder, theft from others, grave robbing, carving human corpses, being a Death Knight, and being a Syth are illegal across much of the land. Begging, necromancy, eating human hearts, killing good-natured creatures, and poison dealing are not called illegal here, but the civilized dislike them. This does not give me gold or a task, but it tells me the world may punish more than just combat death.

**Beat 2**

I close the laws window.

The gump goes away with no reward, target cursor, quest log, crime flag, skill change, discovery flag, item movement, or overhead warning. I am back at the west-wall boards.

**Beat 3**

I double-click `News From The Nobles`.

It opens a news-list gump headed by `Lord British`. The visible text says these are recent messages from throughout the land and asks me to select an article below. Only one article row is visible: `Welcome to Confictura: Legend & Adventure (Original Ruins & Riches Article) - January 19th, 2021`. I stop there instead of pressing the article button, because opening the list is not the same as reading the article body.

Mechanical friction learned:

- `Laws of the Land` is a short read-only rules board. It teaches illegal and socially disliked acts, but closing it does not mutate inventory, stats, quests, target cursor, crime status, or discovery flags.
- `News From The Nobles` is usable from the same west-wall tile and opens a custom article-list gump, not a plain text board.
- The news board currently exposes a visible article-selection row before any article body is shown. Pressing that row is the next real UI choice.
- No movement, gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, or follower count changed.

Next pressure:

Mira remains at `Point3D(3434,3435,0)`, facing west with the `News From The Nobles` article-list gump open and unread. The next client-visible decision is to read the visible list deliberately, open the single visible article, close the news board, inspect the nearby donation boxes, Status Board, Merchant Advertiser, or leave through the south doors.

## Run 268 - I Read Lord British's Welcome

I start at `Point3D(3434,3435,0)`, facing west beside the west-wall public boards, with the `News From The Nobles` list already open. The bank room has not turned dangerous while I stare at the parchment: Bridget is still beside me, Greger and Lionel are close, Susan is northeast, boards and donation boxes line the west and north walls, vaults and vote stones sit east, and the south doors are still the visible way out. No hostile is on screen.

**Beat 1**

I read the news list without touching anything.

It is a `Lord British` notice board, not a quest offer. The text says these are recent messages from throughout the land and tells me to select an article. There is only one visible row: `Welcome to Confictura: Legend & Adventure (Original Ruins & Riches Article) - January 19th, 2021`.

**Beat 2**

I press the single visible article row.

The list changes into the article body. The header remains `Lord British`, the article title and date are shown at the top, and a long scrollable message opens underneath. No target cursor, quest gump, reward, bank box, combat warning, or movement result appears.

**Beat 3**

I read the welcome article.

The useful part is not lore flavor. The article tells me the game expects in-world learning, not a checklist. Food, drink, equipment, and carrying capacity matter. Towns and merchants are useful because loot can be odd objects, not only coins. The bank and other public areas are shared hub spaces that return me to the town door I entered from when I leave. Sosaria's dungeons are the normal baseline difficulty, but dungeons still mean traps, resource drain, loot weight, and monsters. That makes the bank-room boards more like survival orientation than a quest chain.

Mechanical friction learned:

- `News From The Nobles` is a two-step gump: the visible article list must be selected before the article body exists on screen.
- Selecting the row only opens a `BoardMessage` article view. It does not start a quest, grant gold, move items, set a discovery flag, open a target cursor, or change crime, hunger, thirst, stats, skills, followers, bank balance, or position.
- The article view has a visible close/return control. Using that control is the next UI action if I want to return to the list before inspecting other bank objects.
- No movement or terrain check was needed; this run stayed in stationary public-board UI.

Next pressure:

Mira remains at `Point3D(3434,3435,0)`, facing west with the Lord British welcome article open and read. The next client-visible decision is to close/return from the article, then either leave the news board alone, inspect the nearby donation boxes, Status Board, Merchant Advertiser, vote stones, or head out through the south doors.

## Run 269 - I Back Out Of The News Board

I start at `Point3D(3434,3435,0)`, facing west beside the bank's west-wall public boards. The Lord British welcome article is still open and already read. Nothing in the room has pressed me while I read: my fox is close by, Bridget is just south of me, Greger and Lionel are nearby, Susan is northeast, the donation boxes, trash barrel, laws board, news board, vaults, vote stones, and south doors are still visible, and no hostile is on the screen.

**Beat 1**

I use the visible close/return control on the article.

The article body disappears, but I am not done with the news board yet. It drops me back to the `Lord British` article list: the same recent-messages heading and the single `Welcome to Confictura: Legend & Adventure (Original Ruins & Riches Article) - January 19th, 2021` row are visible again. No reward, target cursor, quest log, bank box, or movement result appears.

**Beat 2**

I close the article list instead of choosing the same row again.

The news gump is gone. I am back to the bank room view, still beside the west-wall boards, with no context menu and no target cursor.

**Beat 3**

I walk north along the open bank floor toward the upper west-wall boards and stop around `Point3D(3434,3428,0)`, facing north.

This is short interior movement, not a terrain problem. The Status Board and the upper donation box are now close enough to be the next obvious touchable objects, and the Merchant Advertiser is also close. The north-wall quest boards slide back into clearer view, and another adventurer at the far north edge of the room becomes visible. No hostile, blocked tile, door transition, overhead warning, stamina drop, pet order change, or gump interrupts the movement.

Mechanical friction learned:

- The `BoardMessage` article close/return path does not directly close the news system; it returns to the article list first.
- Closing the returned article list with no article row selected leaves the character state alone: no quest, gold, item, target cursor, discovery flag, crime flag, stat, skill, hunger, thirst, bank, follower, or position mutation beyond the later voluntary walk.
- The Status Board is not a from-anywhere wall sign. I had to step closer before treating it as a normal next interaction.
- The move from the news board to the Status Board approach tile is ordinary open bank-floor travel; no terrain or binary-map check was needed.

Next pressure:

Mira ends near `Point3D(3434,3428,0)`, facing north with no gump or context menu open. The next client-visible decision is to double-click the nearby `Status Board`, inspect the adjacent donation box or Merchant Advertiser, return to the quest boards, or leave through the visible south doors.

## Run 270 - I Check The Status Board

I start near `Point3D(3434,3428,0)`, facing north in the Bank of Britain vault room. The screen is still public bank space: my fox is close, Lionel and Greger are nearby, Susan is east-southeast, Bridget is south, Jebidiah and Araminta are toward the north edge, and the nearby objects are the `Status Board`, a donation box, and `The Merchant Advertiser`. No hostile is on screen.

**Beat 1**

I double-click the nearby `Status Board`.

A large status window opens. It is not a quest board, not a bank box, and not a vendor. The top strip shows the shard name plus live server counters for online clients, accounts, uptime, and RAM in use. Under that is a roster table with columns for name, guild, stats, skills, karma, fame, and kills.

**Beat 2**

I read the status window instead of clicking its tiny page arrows.

This is mostly a public who-is-online sheet. The player row is useful as a mirror: it shows Mira's beginner stat total, skill total, karma, fame, and kills, but it does not offer a job, reward, travel link, training price, storage, or target cursor. If there are enough clients, the arrow controls page through more names; that is not a survival action right now.

Mechanical friction learned:

- `Status Board` is range-gated like a normal wall object. From the current tile it is close enough, so double-clicking opens `StatusGump` instead of giving a too-far message.
- `StatusGump` is a live status/roster surface built from connected clients. It filters hidden higher-access characters from a normal player view and pages the list only when there are more rows to show.
- Reading it does not mutate money, bank contents, inventory, skills, stats, hunger, thirst, fame, karma, kills, quests, discovery flags, target cursor, crime state, pet order, or position.
- No movement, macro route, combat, terrain check, or map transition happened.

Next pressure:

Mira remains at `Point3D(3434,3428,0)`, facing north with the `Status Board` window open and read. The next client-visible decision is to close it, inspect the adjacent donation box or Merchant Advertiser, return to the quest boards, or leave through the south doors.

## Run 271 - The Donation Box Has A Shorter Reach

I start at `Point3D(3434,3428,0)`, facing north with the `Status Board` roster still open and already read. The bank room is still calm: my fox is close by, Lionel and Greger are near me, Susan and the bankers are off to the east and north, Bridget is south, and the west-wall objects in arm's reach are the Status Board, the upper donation box, and `The Merchant Advertiser`. No hostile is on the screen.

**Beat 1**

I close the `Status Board` window.

That leaves me looking at the bank again. No page arrow is used, no row is selected, no target cursor appears, and the roster does not change my pack, money, skills, stats, bank, quest state, or pet order.

**Beat 2**

I double-click the nearby donation box from where I am standing.

It only answers with the normal too-far message. The box is visibly near the Status Board, but it is still a container, and containers are fussier about reach than that board was. I do not get a container window, loot, a reward, a target cursor, or a donation prompt.

**Beat 3**

I take a short step northwest to about `Point3D(3433,3427,0)`, facing the donation box.

This is just a bank-floor correction after the reach check, not a route. The donation box is now close enough to try again next, while the Merchant Advertiser, quest boards, bankers, adventurers, and south doors remain visible route choices. Nothing attacks, no gump opens, and no terrain check is needed.

Mechanical friction learned:

- Closing the Status Board is a pure UI action. It leaves no gump, context menu, target cursor, money, item, bank, quest, discovery, crime, hunger, thirst, skill, stat, fame, karma, kill, position, or pet-order change.
- The donation box is a `TrashChest`, not a special reward chest. Its object properties call it a donation box and say it empties every 24 hours.
- Unlike the Status Board's four-tile use reach, the donation box follows normal container reach. From `Point3D(3434,3428,0)` to the box at `Point3D(3431,3426,0)`, the double-click is too far away and opens nothing.
- Stepping northwest to `Point3D(3433,3427,0)` is enough ordinary bank-floor correction to put the upper donation box inside normal container reach.
- No movement beyond that correction, macro route, combat, terrain check, map transition, gold, loot, bank content, quest state, target cursor, follower change, or discovery flag happened.

Next pressure:

Mira ends near `Point3D(3433,3427,0)`, facing the upper donation box with no gump, context menu, or target cursor open. The next client-visible decision is to double-click the donation box again from inside range, inspect `The Merchant Advertiser`, return to the quest boards, cross toward the vote stones, or leave through the south doors.

## Run 272 - The Donation Box Finally Opens

I start at `Point3D(3433,3427,0)`, facing northwest beside the upper donation box. The previous too-far click has already been corrected by one bank-floor step. The room is still safe-looking: my fox is treated as close by, Lionel is only a couple of steps southeast, Greger is farther south, Susan and the bankers are visible to the east and north, Bridget is south, the north-wall quest boards and west-wall public boards are still in view, and the vote stones and south doors remain route choices. No hostile is on screen.

**Beat 1**

I double-click the upper donation box again from beside it.

This time it opens like a normal container instead of answering with the too-far message. I get a plain donation-box container window, not a reward gump, vendor list, quest prompt, bank box, target cursor, or travel choice. I do not drag anything into it or take anything out; the new visible state is simply an open public donation container that needs inspection before I move on.

Mechanical friction learned:

- Standing at `Point3D(3433,3427,0)` is close enough for the upper donation box at `Point3D(3431,3426,0)`.
- The donation box behaves as a normal `TrashChest` container once the two-tile container reach check passes.
- Opening it does not pay gold, start a quest, change bank contents, mutate inventory, alter hunger or thirst, change stats or skills, set a discovery flag, open a target cursor, change pet order, or move Mira.
- The box is not a loot chest route by itself. Its visible property text says it empties every 24 hours, so putting items into it is disposal/donation behavior unless another visible player reason appears.
- No movement, macro route, combat, terrain check, map transition, or hostile threat happened.

Next pressure:

Mira remains at `Point3D(3433,3427,0)`, facing northwest with the upper donation-box container window open. The next client-visible decision is to inspect the open container contents and close it or deliberately donate something, then decide whether to inspect `The Merchant Advertiser`, return to the quest boards, cross toward the vote stones, or leave through the south doors.

## Run 273 - The Merchant Board Is Only A Directory So Far

I start at `Point3D(3433,3427,0)`, facing northwest beside the upper donation box. Its plain container window is still open from the last click. The bank room has not turned dangerous: my fox is still treated as close by, Lionel is only a couple of steps southeast, Greger is farther south, Susan and the bankers are visible to the east and north, Bridget is south, and the quest boards, public boards, vote stones, bank vaults, and south doors remain in sight. No hostile is on screen.

**Beat 1**

I look through the open donation box before touching anything else.

There is nothing useful in it. I do not see loot, gold, a hidden reward list, a prompt, or a target cursor. It is just an empty public container window.

**Beat 2**

I close the donation-box container instead of donating my food, canteen, or medical record.

The bank room view is clean again. My pack, bank balance, hunger, thirst, skills, stats, quest state, follower order, and position stay the same.

**Beat 3**

I double-click `The Merchant Advertiser` from the same tile.

A vendor directory opens. It is a broad window with `Shop Name`, `Owner`, and `Location` columns, a footer that says there are 21 vendors in the world, and page arrows at the top. This first page is not a shop, a vendor buy list, a bank box, a quest, or a travel confirmation. I do not click anything yet, because the rows look like labels and the only obvious controls are the page arrows and close box.

Mechanical friction learned:

- The upper donation box had no visible contents this run. Closing it does not move or mutate anything.
- `The Merchant Advertiser` opens a `Home Vendors` directory from a normal double-click. It lists shop name, owner, and location, and it has page controls when more than twelve vendors exist.
- The current directory page does not show visible row buttons. Even though the custom response code has a hidden branch for vendor-row IDs, a normal player cannot use that branch unless the gump exposes a visible control. I stop at the open gump instead of inventing a row selection.
- No movement, macro route, combat, terrain check, map transition, gold, loot, bank content, quest state, target cursor, follower change, or discovery flag happened.

Next pressure:

Mira remains at `Point3D(3433,3427,0)`, facing northwest with the Merchant Advertiser vendor-directory gump open and not yet read through. The next client-visible decision is to read the first page, use the visible next-page arrow, or close the directory and move on to the quest boards, vote stones, or south doors.

## Run 274 - The Vendor Directory Ends At Page Two

I start at `Point3D(3433,3427,0)`, facing northwest with the `Home Vendors` directory still open. The bank room behind the gump is unchanged: my fox is treated as close, Lionel is nearby, Susan and the bankers are off to the east and north, Bridget is south, and the quest boards, public boards, vote stones, bank vaults, and south doors are still the visible room choices. No hostile is on screen.

**Beat 1**

I read the first Merchant Advertiser page instead of poking hidden row IDs.

The page is only a directory. It shows twelve vendor rows under `Shop Name`, `Owner`, and `Location`, with a footer saying there are 21 vendors in the world. The rows do not open buy packs while I read them, and they do not look like buttons. Most of the shop labels are not useful enough to be a shopping plan by themselves, and the location text feels like far-away sextant shorthand rather than a walkable route from this bank room.

**Beat 2**

I press the visible next-page arrow at the top of the directory.

The gump redraws to the second page. No target cursor appears, no vendor pack opens, no confirmation asks where I want to travel, and I do not move.

**Beat 3**

I read the second page and stop.

This is the last page. It has the remaining nine vendor rows, a working previous-page arrow, and a dead next-page marker. A couple of shop names are clearer, like furniture or artifacts, but the window still does not expose a buy list, price list, row-select button, travel confirmation, or safe walking route. I leave it open because closing it is the next real decision, not something to rush past.

Mechanical friction learned:

- The visible Merchant Advertiser controls are close, previous page, and next page. Page rows are labels, not exposed row buttons.
- Page 1 shows twelve rows; pressing the visible next arrow opens page 2. Page 2 shows the final nine rows and disables the next arrow.
- The directory can advertise where player vendors exist, but it does not prove those locations are reachable, safe, discovered, or useful to a broke new character.
- The custom response path contains a teleport branch for row IDs, but the gump does not expose visible row buttons. A normal player still cannot use that branch.
- The location column can be misleading for normal shopping decisions. The gump calculates special-region names for some vendors, but many rows collapse to repeated far-corner-style coordinates instead of an obvious local route.
- No movement, macro route, combat, terrain check, map transition, gold, loot, bank content, quest state, target cursor, follower change, or discovery flag happened.

Next pressure:

Mira remains at `Point3D(3433,3427,0)`, facing northwest with the Merchant Advertiser directory open on page 2 and read. The next client-visible decision is to close it, use the previous-page arrow, or stop using bank-room noticeboards and move on to the vote stones, quest boards, bank vaults, or south doors.

## Run 275 - I Read The Vote Stone Before Clicking It

I start at `Point3D(3433,3427,0)`, facing northwest with the Merchant Advertiser directory open on its second and final page. The bank room behind it is still safe-looking: my fox is treated as close by, Lionel is nearby, Susan and the bankers are off east and north, Bridget is south, the quest boards and public boards are still behind me, and the vote stones are visible across the room. No hostile is on screen.

**Beat 1**

I close the `Home Vendors` directory.

That ends the vendor-directory loop cleanly. I do not press the previous-page arrow again, and I do not pretend the label rows are buttons. No buy pack, target cursor, travel prompt, item movement, or vendor teleport appears.

**Beat 2**

I run east and a little north across the open bank floor to the vote-stone row, stopping around `Point3D(3447,3424,0)` facing north.

This is normal interior movement. The vote stones are now directly in front of me, with more bankers and adventurer-looking people visible around the east side of the vault room. Bank vaults, trash barrels, dark wood doors, and the east-side public doors are also in view. The old barbaric-satchel spot is in the stale live snapshot, but I already made that satchel vanish earlier, so I do not treat it as a usable object now. Nothing blocks the floor, no region transition fires, no hostile enters the screen, and my fox is assumed to follow.

**Beat 3**

I inspect the nearest `Vote Stone` instead of double-clicking it.

The object properties make the purpose clear: using it launches a browser to cast a vote for `Confictura: Legend & Adventure`, and a configured vote-site URL is shown when the stone is valid. That is not a shop, bank box, quest, local map route, or in-world reward prompt. I stop before double-clicking, because the next click would be an external browser/cooldown action rather than another readable in-game board.

Mechanical friction learned:

- Closing the Merchant Advertiser is a pure UI action and leaves no gump, context menu, target cursor, vendor list, travel confirmation, item, gold, quest, bank, or discovery change.
- The short route from the donation-box tile to the vote-stone row is open bank-floor travel. No terrain or binary-map check was needed.
- `Vote Stone` object properties are enough to warn a normal player what the object does before activating it.
- The vote-stone double-click path is custom: it casts a vote request, and if the site is valid and the cooldown allows it, the player receives a thank-you message, the browser opens to the vote URL, and an account last-vote tag is written. If it is too early, the visible result is a cooldown message instead.
- I only inspected the stone this run, so no browser launched, no cooldown tag was intentionally consumed, and no gold, item, bank content, skill, stat, hunger, thirst, quest, target cursor, pet order, crime flag, or discovery flag changed.

Next pressure:

Mira ends near `Point3D(3447,3424,0)`, facing north with no gump, context menu, or target cursor open. The next client-visible decision is to double-click a vote stone and accept the browser/cooldown flow, inspect the nearby bank vaults or dark wood doors, return to the boards, or leave through the visible public doors.

## Run 276 - I Leave Before The Vote Stone Opens A Browser

I start at `Point3D(3447,3424,0)`, facing north at the east-side vote stones in the shared Bank of Britain vault room. No gump, context menu, or target cursor is open. The nearest readable stone already told me the next double-click is an external voting/browser action, not bank survival. Bankers, adventurer-looking people, vaults, trash barrels, dark wood doors, and the east public doors are still visible; no hostile is inside the room.

**Beat 1**

I decide not to use the `Vote Stone`.

There is no in-world reason to spend the browser/cooldown flow while I still have no gold, no training, and no route plan. The stone stays in front of me, and nothing changes because I do not double-click it.

**Beat 2**

I run east along the open bank floor to the visible public doors and use the nearest one.

The route is ordinary interior floor. No wall, vault, banker, trash barrel, vote stone, gump, target cursor, hostile, or terrain snag interrupts me. The door acts like the public bank transition instead of a hinged room door.

**Beat 3**

The door drops me back outside at the Bank of Britain marker around `Point3D(3015,1056,5)`, facing east.

The street screen is not empty. The `Bank of Britain` sign is under me, the bank door is just east/northeast, and a trash barrel is close by. Merele the bard is barely on the north edge, Vanya and Franklin are south, and the ruby elemental-looking humanoid is back in view about fifteen tiles south-southeast. It is not on top of me, but it is close enough that I stop instead of drifting south into it.

Mechanical friction learned:

- Leaving the bank through a visible `PublicDoor` uses the stored public-door return from when I entered the bank. It clears that stored return and teleports me back to the exterior Bank of Britain spot instead of opening a container, gump, shop, target cursor, or ordinary hinged door.
- The interior door's saved destination field is not the player-facing exit destination here. The player-facing behavior is "return to the town door I came through."
- The world-map overlay is useful again outside: `Bank of Britain` is underfoot, `Britain` is west, and nearby shop markers surround the street. Those markers are navigation labels, not proof of safety inside any building.
- The ruby elemental south of the bank returns to potential-threat status within the 11-18 tile band. It does not block standing at the bank, but it changes the next route decision.
- No browser launched, no vote cooldown was consumed, no money, item, bank content, skill, stat, hunger, thirst, quest, target cursor, follower order, crime flag, or discovery flag changed.

Next pressure:

Mira ends outside the Bank of Britain at `Point3D(3015,1056,5)`, facing east with no gump, context menu, or target cursor open. The next client-visible decision is to route away from the ruby elemental, inspect the nearby trash barrel or bank sign, re-enter the bank, or use the world-map markers to choose a safer town stop such as Defenders of Sosaria, The Cleaver, The Bard's Tale, or Profuse Provisions.

## Run 277 - I Put The Bank Behind Me And Reach The Cleaver

I start outside the Bank of Britain at `Point3D(3015,1056,5)`, facing east with no gump, context menu, or target cursor open. The bank sign is under me, the public bank door is just east, and a trash barrel is close by. Merele the bard is barely on the north edge, Vanya and Franklin are south, and the ruby elemental-looking humanoid is still south-southeast in the potential-threat band. It is not close enough to force a fight, but it is close enough that I do not wander south.

**Beat 1**

I run west and a little south through Britain toward the world-map marker for `The Cleaver`, keeping the ruby elemental behind me instead of cutting toward the southern provision marker.

This is macro travel by intent, not a tile audit. The route is town street and open frontage, and nothing visible suggests water, mountain, wall, dungeon, or dense trees. The fox is assumed to follow. The ruby elemental falls off behind me instead of entering the ten-tile danger band, no new hostile appears, and I stop around `Point3D(2968,1062,0)`, facing the shop frontage.

**Beat 2**

I take in the new screen before touching a vendor.

`The Cleaver` sign is under me, a dark wood door is just southwest, and a light wood gate and coffer sit farther west. Gareth the butcher is visible to the west, with a cow nearby, and Peter the cook, Natane the town guard, a town herald, birds, adventurer-looking people, and training-yard objects fill the edges of the screen. That reads like a town food/shop corner, not a wild fight. The coffer is tempting, but I do not touch it while broke and untrained.

**Beat 3**

I double-click the nearest dark wood door at the Cleaver frontage.

It behaves like an ordinary door so far. No shop list, vendor pack, bank box, quest, target cursor, trap, crime warning, or teleport appears. I leave the doorway as the next physical decision instead of pretending I have already talked to Gareth or looted the coffer.

Mechanical friction learned:

- Moving west from the bank is the safer town route because it increases distance from the ruby elemental instead of closing toward the south-side marker cluster.
- `The Cleaver` marker resolves into visible food-shop context only after I get there: a sign, door, gate, coffer, butcher, cook, livestock, guard, and town pedestrians.
- World-map markers remain navigation hints. The marker did not prove the shop interior, prices, vendor options, or safety until the screen actually showed the local objects and mobiles.
- The nearby Cleaver door is just an ordinary visible door at this point. Opening it does not by itself open buy/sell UI, move me inside, start a quest, reveal loot, or change crime state.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, quest state, discovery flag, target cursor, pet order, follower count, or criminal flag changed.

Next pressure:

Mira ends at `Point3D(2968,1062,0)`, facing southwest at `The Cleaver` frontage with no gump, context menu, or target cursor open and the nearest door opened. The next client-visible decision is to step through the doorway toward Gareth the butcher, context-click or talk to the visible food vendors once in range, inspect the coffer or gate, or backtrack toward Defenders of Sosaria or the bank.

## Run 278 - Gareth Will Not Buy My Starter Pack

I start at `Point3D(2968,1062,0)`, facing southwest at `The Cleaver` frontage. No gump, context menu, or target cursor is open, and the nearby dark wood door is already open. The sign is still under me, Gareth the butcher is west of me, the coffer and light wood gate are between the shop front and the animals, a cow and a guard are visible, and the bank-side ruby elemental is off-screen behind me. Nothing on this screen is trying to kill me.

**Beat 1**

I step through the opened doorway and move west along the shop frontage until Gareth is close enough to deal with, stopping around `Point3D(2961,1062,0)` and facing west.

This is not a new journey. It is a short move through the already-open door toward the visible butcher. The fox keeps with me, the coffer is now just north of my shoulder, the cow is a few steps southeast, and the guard is south. Peter the cook and the bank-side street have slipped off the screen. No wall, warning, hostile, target cursor, gump, or teleport interrupts the move.

**Beat 2**

I context-click Gareth instead of yelling random vendor words across the room.

The menu proves he is a real vendor surface, not just scenery. The visible choices include the normal buy and sell rows. I do not treat opening the menu as a trade by itself; no price list, backpack list, gold change, target cursor, or speech result has happened yet.

**Beat 3**

I choose the visible sell row first, because I have no gold and need to know whether my starting pack is worth anything.

Gareth answers, `You have nothing I would be interested in.` That is the whole result. My cooked alien meat, empty canteen, medical record, worn clothing, boots, and knife do not turn into cash here. No sell list opens, no item moves, no gold appears, no crime flag triggers, and no buy window opens from the sell attempt.

Mechanical friction learned:

- Gareth is a `Butcher` vendor, so the visible context menu can expose buy and sell actions once I stand close enough.
- The sell path checks my backpack for movable standard loot matching the butcher's sell list. My current starter inventory does not match it, so the vendor only gives the no-interest speech.
- The nearby coffer remains a separate theft/snooping temptation, not the butcher's trade stock.
- This did not change money, items, equipment, bank contents, skills, stats, hunger, thirst, damage, quest state, discovery flags, target cursor, pet order, follower count, or criminal status.

Next pressure:

Mira ends at `Point3D(2961,1062,0)`, facing west beside Gareth the butcher with no gump, context menu, or target cursor open. The next client-visible decision is to context-click Gareth again and inspect buy prices with zero gold, leave the shop for another visible Britain marker, test the nearby gate or wanted sign, or deliberately risk the coffer knowing the snooping blocker already exists.

## Run 279 - Gareth Sells Meat I Cannot Afford

I start at `Point3D(2961,1062,0)`, facing west beside Gareth the butcher. No gump, context menu, or target cursor is open. The screen is still The Cleaver frontage: Gareth is two steps west, the coffer is just north, the light wood gate is southeast, the cow and Natane the guard are close, birds and adventurer-looking people sit on the edges, and the bank-side ruby elemental is still off-screen behind me. Nothing here is acting hostile.

**Beat 1**

I context-click Gareth again and choose the visible buy row.

This time the client opens the normal vendor buy window instead of the dead-end sell response. Gareth gives the usual shop greeting and the list is a butcher's shelf, not a gift table. It is meat and butcher tools with prices and quantities, and my gold readout is still zero.

**Beat 2**

I read the buy window instead of clicking a row.

The useful lesson is immediate: even the cheap meat is money-gated. Raw ribs are a visible 16-gold kind of item here, and chance-stock rows such as bacon, ham, sausage, raw meat pieces, or knives do not help a broke starter. My cooked alien meat already works as food, so buying more meat would not solve the actual blocker anyway.

**Beat 3**

I close the buy window without buying anything.

No item jumps into my pack, no gold is deducted, no target cursor appears, and Gareth does not convert the window into a training or quest prompt. I am back beside him with the coffer, gate, and wanted sign still tempting me from the same screen.

Mechanical friction learned:

- Gareth's visible vendor surface has both rows: sell is useless for Mira's starter inventory, while buy does open the standard shop list.
- Zero gold does not prevent opening the buy window. It only makes every listed item unaffordable.
- The butcher's stock is survival-adjacent but not a cash route. Buying food or butcher tools requires money first, and Mira already has cooked alien meat.
- The coffer remains a separate snooping/theft temptation, not Gareth's trade stock.
- No movement, gold, item, bank content, skill, stat, hunger, thirst, damage, quest state, discovery flag, target cursor, pet order, follower count, or criminal flag changed.

Next pressure:

Mira remains at `Point3D(2961,1062,0)`, facing west beside Gareth with no gump, context menu, vendor window, or target cursor open. The next client-visible decision is to leave The Cleaver for another Britain marker that might produce cash or training value, read the wanted sign, test the gate, or deliberately risk the coffer despite the known Snooping-first blocker.

## Run 280 - The Wanted Sign Is Just a Name

I start at `Point3D(2961,1062,0)`, facing west beside Gareth at The Cleaver. No gump, context menu, vendor window, or target cursor is open. Gareth is no longer useful to a broke starter, the coffer is still a Snooping trap, and the wanted sign on the east edge of the screen is the one visible thing that looks like it might point to money.

**Beat 1**

I leave Gareth and jog southeast through the open town frontage toward the wanted sign, stopping near `Point3D(2980,1072,1)`.

The short route does not turn into a fight or maze. The fox keeps with me. The Cleaver slides behind me, the sign is now beside me, Peter the cook is close enough to talk to, three adventurer-looking people are gathered around the tavern-side porch, Markos is farther east, and The Unicorn's Horn sign and public door are south. I do not see a hostile in the 18-tile screen.

**Beat 2**

I single-click and then double-click the wanted sign, trying to read more than the object label.

The only readable thing is the name: `Wanted! Blackbart Roberts`. No parchment, bounty board, quest offer, target cursor, gump, payment hint, map marker, or criminal warning appears. Mechanically it behaves like a named static sign, not an interactive bounty system.

**Beat 3**

Since Peter the cook is standing right next to the sign, I context-click him instead of wandering away.

His context menu opens and gives me the citizen speech/talk row. I stop there because choosing the row is the next visible decision; it could be just a rumor or it could open one of the citizen service gumps, and I have not clicked it yet.

Mechanical friction learned:

- The Britain wanted sign is only useful as screen text at this point. Double-clicking it does not expose a bounty flow.
- The actual Blackbart Roberts mobile exists elsewhere, but the sign does not reveal a route, quest state, or reward to a normal client click.
- Peter is not a standard vendor surface like Gareth. He is a citizen/cook surface with a talk-style context row, so his result should be tested separately from shop buying and selling.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, quest state, discovery flag, target cursor, pet order, follower count, or criminal flag changed.

Next pressure:

Mira ends near `Point3D(2980,1072,1)`, facing east beside the wanted sign with Peter's context menu open. The next client-visible decision is to choose Peter's speech/talk row, close the menu and test The Unicorn's Horn public door, or move east/north toward another Britain marker.

## Run 281 - I Use The Tavern Door Instead Of Peter's Row

I start at `Point3D(2980,1072,1)`, facing east beside the wanted sign, with Peter the cook's context menu still open. The row is visible, but choosing it could be a random citizen rumor or one of the cook-service gumps, and I already have a clearer thing on screen: `The Unicorn's Horn` sign and its public door south of me. Nothing hostile is in the client box.

**Beat 1**

I click away from Peter's context menu.

That is a real choice. I do not choose his speech row, so there is no rumor, no cook-service window, no crate offer, no backpack inspection, no target cursor, and no trade. Peter remains a nearby cook, but his talk result is still untested.

**Beat 2**

I run south along the open town frontage to the visible `The Unicorn's Horn` public door, stopping around `Point3D(2981,1089,5)` and facing south.

The move is short and clean. The wanted sign and Peter slide behind me, the tavern sign is now the obvious marker, Alexander and Merrick are farther south/east town context, and no hostile enters the 18-tile screen. The fox is assumed to follow.

**Beat 3**

I double-click the public door for `the Unicorn's Horn Tavern`.

It is not an ordinary hinged door. It takes me into the shared tavern interior at `Point3D(3721,3495,0)` on `Map.Sosaria`, inside `the Tavern` public region. The first thing I see is a crowded room, not a wilderness route: patrons and waiters are all around, the exit doors are beside me, dice, tarot cards, a chessboard, high-low cards, tarot poker cards, trash barrels, a dark wood door, and a barbaric satchel are visible. I stop because this new room has several readable or usable objects and named people within range.

Mechanical friction learned:

- Closing Peter's context menu invokes no hidden talk path. His citizen/cook row remains unresolved because I did not select it.
- The short route from the wanted sign to The Unicorn's Horn is ordinary open Britain frontage. No terrain check was needed, and no visible threat interrupted the run.
- The exterior `the Unicorn's Horn Tavern` object is a `PublicDoor` that sends the player to the tavern public region at `Point3D(3721,3495,0)` and stores the outside return point.
- The interior tavern exit doors show placeholder destinations in the live snapshot, but the public-door code will use the stored return while Mira is inside a `PublicRegion`; the visible next use should be treated as "leave back to the town door," not travel to `0,0`.
- Public-door travel teleports pets with the player. No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, quest state, discovery flag, target cursor, pet order, follower count, or Peter talk result changed.

Next pressure:

Mira ends at `Point3D(3721,3495,0)`, facing south inside The Unicorn's Horn tavern with no gump, context menu, or target cursor open. The next client-visible decision is to read the crowded tavern screen before choosing an exit door, talking to a waiter or patron, inspecting the games, or touching the visible satchel.

## Run 282 - Sidney Opens A Tavern Buy Window

I start at `Point3D(3721,3495,0)`, facing south inside The Unicorn's Horn tavern. No gump, context menu, or target cursor is open. The exit doors are one step east, Sidney the waiter is northwest of me, Thomas and Sine are close to the south, and the rest of the room is packed with passive patrons, dice, tarot cards, card games, trash barrels, a dark wood door, and another barbaric satchel. Nothing on the screen looks hostile.

**Beat 1**

I read the room instead of running back outside.

This is not a safehouse with one obvious service counter. It is a crowded public tavern with several temptations. The satchel is interesting, but the last one vanished when I clicked it without owning it, so I leave that alone for now. The games are also visible, but I have zero gold and do not know which ones cost money. Sidney is the closest service-looking NPC, so I test him first.

**Beat 2**

I context-click Sidney.

His menu opens like a real vendor surface, not like Peter's single citizen-talk row. The visible choices are the normal buy and sell rows. I do not treat opening the menu as trade, because no list, backpack scan, gold change, or item movement has happened yet.

**Beat 3**

I choose the visible buy row.

The client opens the standard vendor buy window for Sidney. I stop with the window open instead of clicking a stock row: the list has item names, prices, quantities, and my gold is still zero. This confirms the tavern waiter can show a food-and-drink shop surface, but I have not bought anything, sold anything, or read the full list closely enough to choose a row.

Mechanical friction learned:

- Sidney the waiter is a `BaseVendor` waiter, not just scenery or a rumor patron.
- The visible context menu exposes normal buy and sell rows from the vendor system.
- Choosing buy opens a standard vendor buy window even when Mira has 0 gold.
- No purchase happened, no sell attempt happened, no item moved, no gold changed, no target cursor appeared, and no hunger, thirst, damage, skill, crime, quest, discovery, pet order, follower count, or region state changed.

Next pressure:

Mira remains at `Point3D(3721,3495,0)`, facing south inside The Unicorn's Horn tavern, with Sidney's standard vendor buy window open and unread. The next client-visible decision is to read the buy list, close it, or use the sell row after closing/returning to Sidney's menu.

## Run 283 - Sidney Will Not Buy My Pack

I start at `Point3D(3721,3495,0)`, facing south inside The Unicorn's Horn tavern with Sidney's vendor buy window still open. The room around the window has not become dangerous: Sidney is northwest of me, Thomas and Sine are close to the south, the public exit doors are one step east, the room is full of passive patrons, and the visible dice, tarot cards, card games, trash barrels, dark wood door, and suspicious satchel are still just background until I choose one.

**Beat 1**

I read Sidney's buy list instead of clicking a stock row.

It is exactly what a broke person does not need: tavern food and drink with prices and quantities. I can see cheap staples such as bread and ale-priced drink rows, with other bottles, pitchers, bowls, meat, cheese, and pie-style food in the same shop surface. The key fact is not the full menu. My gold is still zero, and every visible row costs more than zero.

**Beat 2**

I close the buy window without buying anything.

No food, drink, bottle, pitcher, bowl, or receipt lands in my backpack. No gold is removed, no target cursor opens, and no training or quest prompt replaces the shop list. I am back to the tavern screen with Sidney still close enough to test.

**Beat 3**

I context-click Sidney again and choose the visible sell row.

That does not open a sell list. Sidney only gives the vendor refusal: `You have nothing I would be interested in.` My cooked alien meat, empty canteen, medical record, worn clothes, boots, and knife still do not become cash. The context menu closes, and I am left standing in the same crowded tavern with no gump, buy window, sell list, or target cursor open.

Mechanical friction learned:

- Sidney's buy list is a real waiter stock surface, but it is money-gated at the row level. Zero gold still allows reading the menu, not buying from it.
- The visible sell row is a dead end for Mira's current starter pack. The waiter does not expose a sell list for anything she carries.
- The tavern did not solve starter cash. The remaining visible tavern leads are the game objects, the suspicious satchel, other patrons/waiters, the dark wood door, trash barrels, or the public exit back outside.
- No movement, gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, quest state, discovery flag, target cursor, pet order, follower count, or region state changed.

Next pressure:

Mira remains at `Point3D(3721,3495,0)`, facing south inside The Unicorn's Horn tavern with no gump, context menu, vendor window, sell list, or target cursor open. The next client-visible decision is to inspect another visible tavern object or NPC, try the suspicious satchel deliberately, or use the public exit door to return to Britain.

## Run 284 - The Card Table Opens Before It Costs Me

I start at `Point3D(3721,3495,0)`, facing south inside The Unicorn's Horn tavern. No gump, context menu, vendor window, sell list, or target cursor is open. Sidney has already failed as a starter-cash route, the exit doors are still one step east, the room is crowded with passive patrons, and the visible game tables, dark wood door, trash barrel, and suspicious satchel are the remaining tavern leads. I do not touch the satchel first because I have already watched one of those erase itself.

**Beat 1**

I choose the visible High-Low card table at the south edge of the tavern instead of asking another waiter the same buy/sell question.

That is a normal player choice, not a hidden route. The game object is visible, it has a readable name, and gambling is at least plausibly related to money. The nearby patrons and Sidney remain passive, and nothing on the screen looks like a hostile.

**Beat 2**

I walk south through the tavern floor to the card table, stopping around `Point3D(3719,3511,0)` just north of the `High-Low Card Game`.

This is a short interior move, not a wilderness route. I pass closer to the trash barrel, dark wood door, barbaric satchel, tarot poker cards, dice, chessboard, and more patrons. No one blocks me, no target cursor appears, no door or teleport fires, and the fox keeps with me.

**Beat 3**

I double-click the `High-Low Card Game`.

The game opens a gump before it takes money. I stop there and read the surface instead of pressing a random button. The title says `High-Low Card Game`; the panel shows blank cards, a current bet of 100, credits at 0, last pay at 0, a pay table, and visible controls for `Play`, `Cash Out`, `ATM`, bet arrows, help, and close. No gold is spent just by opening it, but the screen is clearly telling me this is a cash/credit machine.

Mechanical friction learned:

- The High-Low table is not scenery. Double-clicking it from close range opens a real game gump.
- Opening the gump is not the same as playing. The visible `Play` and `ATM` buttons are still unpressed, so no bet, bank withdrawal, payout, or insufficient-funds message has happened yet.
- The first visible money pressure is inside the gump: current bet 100, credits 0, and an ATM control that points back to bank balance rather than free starter cash.
- The known satchel lesson remains active. A world-placed barbaric satchel is a bad first touch for Mira, so I avoided repeating that deletion while another visible lead existed.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, quest state, discovery flag, target cursor, pet order, follower count, or region state changed.

Next pressure:

Mira ends at `Point3D(3719,3511,0)`, facing south beside the High-Low table with the `High-Low Card Game` gump open and unread controls visible. The next client-visible decision is to read the gump fully, press help, try `Play` with 0 credits, try `ATM` against a 0-gold bank balance, cash out/close, or step away from the table.

## Run 285 - The High-Low Help Panel Says This Is Bank-Funded

I start at `Point3D(3719,3511,0)`, facing south beside the south `High-Low Card Game` table in The Unicorn's Horn. The game gump is already open. The room behind it is still passive tavern noise: Hiro and Akasma are close, Sidney is at the north edge of the screen, the fox is with me by continuity, and the nearby trash barrel, dark wood door, suspicious satchel, tarot-poker deck, dice, chessboard, and exit doors are all still visible but unchosen.

**Beat 1**

I read the open High-Low panel before pressing anything.

The first surface is all machine math. It shows the title, blank card backs, `Current bet: 100`, `Credits: 0`, `Last Pay: 0`, a guess/win pay table, and visible controls for `Play`, `Cash Out`, `ATM`, bet arrows, help, and close. That means the table opened for free, but it did not give me playable credit.

**Beat 2**

I click the visible help arrow instead of trying `Play` or `ATM` blind.

The same gump redraws with a help panel attached on the right. No card flips, no bet starts, no target cursor appears, and no gold or bank message fires. This is still an information click, not gambling.

**Beat 3**

I read the help panel.

The rules are straightforward enough to matter: guess the next card, more consecutive guesses pay more, aces are high, ties go to the house, and being idle for more than five minutes can let someone else take the game away. The controls explain the small bet arrows, the later high/low guess arrows after pressing `Play`, and the important money line: the `ATM` withdraws gold directly from the bank. Since my known bank balance is 0, the table is not a starter-cash loophole unless I press buttons just to confirm the visible shortage. I stop with the help panel open because the next click is a real choice: close/cash out, try the empty-bank `ATM`, or press `Play` with zero credits.

Mechanical friction learned:

- High-Low exposes its instructions through a visible help arrow on the gump; using it is a normal client action and does not spend gold.
- The help text confirms the money path is bank-funded: `ATM` means bank withdrawal, not free tavern credit.
- The game has an idle warning. Sitting on the open gump is not permanent ownership of the table.
- No bet, payout, ATM withdrawal, bank change, gold change, item movement, target cursor, combat, hunger, thirst, skill, stat, crime flag, quest state, discovery flag, pet order, follower count, region change, or movement happened.

Next pressure:

Mira remains at `Point3D(3719,3511,0)`, facing south beside the High-Low table with the `High-Low Card Game` help panel open and read. The next client-visible decision is to close/cash out and leave the table, press `ATM` despite the known 0 bank balance, or press `Play` with 0 credits to see the machine's refusal.

## Run 286 - High-Low Refuses Empty Credit

I start at `Point3D(3719,3511,0)`, facing south beside the south `High-Low Card Game` in The Unicorn's Horn. The help panel is still open and read. The room behind the gump is unchanged tavern clutter: Hiro and Akasma are close, Sidney is at the northern edge of the screen, the fox is still with me by continuity, and the nearby trash barrel, dark wood door, satchel, tarot-poker deck, dice, chessboard, and exit doors are all visible but unchosen. No one on-screen looks hostile.

**Beat 1**

I press the visible `ATM` button.

The machine does not invent money. The gump redraws with the refusal `Insufficient funds for ATM withdrawal.` Credits stay at 0, my carried gold stays 0, and nothing is pulled from the known-empty bank.

**Beat 2**

I read the refreshed panel instead of pressing another control instantly.

It is still the same High-Low screen with the help panel open, the same `Current bet: 100`, `Credits: 0`, `Last Pay: 0`, and the same visible `Play`, `Cash Out`, `ATM`, bet-arrow, help, and close controls. The only new thing is the ATM refusal line.

**Beat 3**

I press the visible `Play` button with 0 credits.

That also refuses cleanly. No cards flip, no high/low arrows become the next choice, and no round starts. The gump redraws with `Insufficient funds to play!` while the help panel remains open and the credits still show 0. This table is now confirmed as a cash sink, not a starter-cash source.

Mechanical friction learned:

- The High-Low `ATM` button is bank-funded and needs actual bank gold. With Mira's known 0 bank balance, it only shows `Insufficient funds for ATM withdrawal.`
- The `Play` button is not free. With no casino token, no backpack gold, no existing credit, and no cash check, it shows `Insufficient funds to play!`
- Testing both visible money buttons did not cash out, close the gump, start a hand, flip cards, open a target cursor, move the character, or change bank, gold, inventory, skills, stats, hunger, thirst, crime, quest, discovery, pet order, follower count, or region state.

Next pressure:

Mira remains at `Point3D(3719,3511,0)`, facing south beside the High-Low table with the `High-Low Card Game` help panel open, read, and showing `Insufficient funds to play!` The next client-visible decision is to stop wasting clicks here by using `Cash Out`/close and leaving the table, or deliberately try a different visible tavern object such as dice, tarot poker, the chessboard, the trash barrel, the dark wood door, or the risky satchel.

## Run 287 - I Cash Out of Nothing and Leave

I start at `Point3D(3719,3511,0)`, facing south beside the south `High-Low Card Game` in The Unicorn's Horn. The help panel is still open and already read, and the table is showing `Insufficient funds to play!` The nearby patrons, Sidney, the dice, tarot-poker deck, chessboard, trash barrel, dark wood door, satchel, and exit doors are all just visible tavern clutter now. No one on-screen looks hostile.

**Beat 1**

I press the visible `Cash Out` button.

There is nothing to collect. The High-Low gump closes instead of paying me, no new gump replaces it, and no card, target cursor, gold pile, bank message, or backpack item appears. That is enough; the table has had every fair zero-money chance.

**Beat 2**

I run north through the open tavern floor toward the public exit doors.

This is ordinary indoor movement. I skirt past the passive patrons and the bar clutter, keeping away from the satchel and the other games. No one body-blocks me, no hostile enters the screen, no terrain catches me, and the fox keeps up.

**Beat 3**

I double-click the visible public exit door by the north wall.

The tavern snaps away and I am back outside at the saved Unicorn's Horn entrance, around `Point3D(2981,1089,5)` in the City of Britain. The fox comes with me. The exterior screen is busier than useful: Peter the cook and several adventurers are still up by the wanted sign, town citizens and tinker shop people are nearby, the `Wanted! Blackbart Roberts` sign is visible again, and the Grand Clock Makers door, basement trapdoor, and a coffer sit southeast. No gump, context menu, target cursor, payout, item movement, skill gain, stat change, crime flag, quest flag, or discovery flag appears.

Mechanical friction learned:

- `Cash Out` is only an exit when credits are 0. It closes the High-Low surface but does not create money.
- The Unicorn's Horn interior exit is a normal visible public-door return, not a new travel choice. It spends the stored tavern-entry return and puts me back on the Britain street.
- Leaving the tavern restores the earlier outside problem: Peter/sign/tinker-shop leads are visible, but no starter-cash route has opened.

Next pressure:

Mira ends at `Point3D(2981,1089,5)`, facing north outside The Unicorn's Horn with no gump, context menu, vendor window, sell list, or target cursor open. The next client-visible decision is whether to resolve Peter's still-untried service/talk row, inspect the Grand Clock Makers frontage and basement trapdoor, or walk back toward a known Britain route such as the bank or other shops.

## Run 288 - Peter Is Only A Rumor

I start at `Point3D(2981,1089,5)`, facing north outside The Unicorn's Horn. There is no open gump, context menu, vendor window, sell list, or target cursor. The screen is safe town noise: Peter the cook and the wanted sign are north by the porch, Mabel, Nyle, Blythe, Markos, Xylene, birds, and town citizens are passive, the tavern door is behind me, and the Grand Clock Makers frontage is southeast. No hostile is in the 18-tile box.

**Beat 1**

I run north along the same open Britain frontage back to the wanted sign and Peter, stopping near `Point3D(2980,1072,1)` and facing him.

This is short town movement, not a route problem. The Unicorn's Horn sign slides south, The Cleaver and its cow re-enter the screen to the west, the wanted sign is beside me, and no wall, door, hostile, target cursor, stamina failure, or region change interrupts the move. The fox is just assumed to keep up.

**Beat 2**

I context-click Peter and choose the visible speech/talk row this time.

The row is not a hidden shop. Peter just talks overhead: `I heard many tales of adventurers going to the Flooded Temple and seeing the 'Evil Kryss of Gayrone the Explorer'.`

**Beat 3**

I stop and read that as a clue instead of pretending it is a quest.

No service gump opens, no trade window appears, no food crate offer is shown, no target cursor appears, and nothing moves into my pack. It is useful world chatter, but it does not solve the immediate 0-gold problem and it does not prove a safe route to the Flooded Temple from this street.

Mechanical friction learned:

- Peter's visible citizen row resolves to an overhead rumor on this saved character, not a vendor buy/sell list or cook-service gump.
- The rumor names the Flooded Temple and an artifact, but it does not start a quest, create a marker on the current Sosaria screen, pay gold, or change the backpack.
- Peter is therefore not a starter-cash route from the Unicorn's Horn frontage. The remaining nearby visible leads are still town surfaces: The Cleaver, The Unicorn's Horn, the Bank route, Grand Clock Makers, and other shop/NPC markers.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, open UI, pet order, follower count, or region state changed.

Next pressure:

Mira ends near `Point3D(2980,1072,1)`, facing east by Peter and the `Wanted! Blackbart Roberts` sign with no gump, context menu, vendor window, sell list, or target cursor open. The next client-visible decision is to decide whether Peter's Flooded Temple rumor is only long-term dungeon gossip for now, then inspect the closer town leads: Markos the fishing trainer, the Grand Clock Makers/tinker frontage, The Cleaver frontage, or the known Bank of Britain route.

## Run 289 - I Only Open Markos's Menu

I start near `Point3D(2980,1072,1)`, facing east by Peter and the `Wanted! Blackbart Roberts` sign. There is no open gump, context menu, vendor window, sell list, or target cursor. Peter's Flooded Temple line is useful gossip, not food or money, so I look at the closer visible lead: Markos is east by the street, while the tavern and Cleaver are already tested and the Grand Clock Makers is still a little southeast. The screen is still ordinary town noise: Peter, Mabel, Nyle, Blythe, Xylene, birds, Alexander, Merrick, the wanted sign, the tavern sign, and the tavern door are all visible or close, and nothing looks hostile.

**Beat 1**

I treat Peter's rumor as long-term dungeon chatter for now and choose Markos as the next town surface to test.

That is only a choice of route, not a new quest. No UI appears and nothing in my pack changes.

**Beat 2**

I run east and a little south through the open Britain frontage, stopping one step west of Markos near `Point3D(2991,1076,0)` and facing east.

This is short street movement. The wanted sign and Peter slide behind me, Markos is now beside me, and the passive citizens and birds remain background. No wall, door, hostile, stamina failure, target cursor, discovery message, or region change interrupts the move. The fox is assumed to keep up.

**Beat 3**

I context-click Markos and only read the menu.

The visible useful row is the same kind of citizen speech/talk row I used on Peter. I do not click it yet. Opening the menu is not a lesson, not a purchase, not a quest, and not a fishing action; it is just the next choice surface. No gump, buy list, sell list, training quote, target cursor, item, gold, skill gain, or overhead speech appears yet.

Mechanical friction learned:

- Markos is close enough to reach by ordinary town movement from the wanted sign, with no visible threat or blocker on the route.
- His surface is a context menu, not an automatic vendor or training window. The player-visible next action is choosing the speech/talk row or closing the menu.
- Seeing a fishing-trainer-looking NPC with a pole does not grant a fishing pole, training, cash, or a quest by proximity alone.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, follower count, or region state changed.

Next pressure:

Mira ends near `Point3D(2991,1076,0)`, facing east beside Markos with no gump open and Markos's context menu open. The next client-visible decision is to choose the visible speech/talk row, close the menu and head toward the Grand Clock Makers/tinker frontage, or leave Markos alone and return toward the known Bank of Britain route.

## Run 290 - Markos Talks, Then I Reach the Clock Shop

I start at `Point3D(2991,1076,0)`, facing east beside Markos. His context menu is already open, and the only useful row I can see is the citizen speech/talk row. There is no gump, vendor window, sell list, target cursor, combat pressure, or visible hostile. The street is still town-safe: Peter and the wanted sign are behind me, Alexander and Merrick are south, the tavern is southwest, and the Grand Clock Makers marker is the next untested lead to the south-southeast.

**Beat 1**

I choose Markos's visible speech/talk row.

Markos does not teach me fishing, hand me a pole, sell anything, or open a service gump. He just talks overhead: `I overheard someone tell of dark elves summoning demons in dungeon destard.`

**Beat 2**

I read it as another long-term dungeon rumor.

That line is darker than Peter's artifact gossip, but it is still not an immediate route from this screen. It does not add a marker, start a quest, change a discovery flag, pay gold, move an item, start a target cursor, or change the fox's order.

**Beat 3**

I run south-southeast through the open Britain frontage toward the `The Grand Clock Makers` marker.

The route is short town movement, not a wilderness problem. I pass the tavern-side street noise and stop around the clock shop frontage near `Point3D(2996,1101,5)`. The fox keeps with me. No wall, locked door, hostile, stamina failure, discovery message, target cursor, region snap, or unexpected terrain block interrupts the run.

The new screen has more useful surfaces than Markos did. `The Grand Clock Makers` sign is here, a dark wood door is just south, a basement trapdoor is west-southwest, and two visible coffers sit nearby. Melinda the tinker and Rosanne the tinker guildmaster are inside/at the shop frontage; Suki the provisioner and `Profuse Provisions` are east, Taylor the fishing trainer is east, Donata the logger is west, and Paulin the town guard is south. I do not see a hostile in the 18-tile screen.

Mechanical friction learned:

- Markos's saved citizen row resolves to a zero-service overhead rumor, not a fishing lesson, vendor, cash route, item grant, or quest surface.
- The rumor names dungeon Destard and dark elves/demons, but it does not create any current-screen route, discovery flag, or visible safety proof.
- The Grand Clock Makers frontage is now the active client-visible choice cluster: sign, dark wood door, basement trapdoor, coffers, tinker, and tinker guildmaster.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, follower count, or map changed.

Next pressure:

Mira ends near `Point3D(2996,1101,5)`, facing south/southeast at the Grand Clock Makers frontage with no gump, context menu, vendor window, sell list, or target cursor open. The next client-visible decision is whether to context-click Rosanne or Melinda, single-click/read the shop sign, try the visible dark wood door, inspect the basement trapdoor, or leave the coffers alone until there is a reason to risk container friction.

## Run 291 - Melinda Turns Into A Target Cursor

I start at `Point3D(2996,1101,5)`, facing south/southeast on the `The Grand Clock Makers` sign. There is no open gump, context menu, vendor list, sell list, or target cursor. The current screen is safe town density: Melinda the tinker is northwest at about dx `-7`, dy `-5`, Rosanne the tinker guildmaster is west, Suki and `Profuse Provisions` sit east/southeast, Paulin the town guard is south, and the same dark wood door, basement trapdoor, and coffers are still visible. I do not see a hostile in the 18-tile screen.

**Beat 1**

I context-click Melinda from the frontage and actually read the menu instead of pretending proximity is a shop.

The menu is a real vendor surface. I can see buy/sell choices, and the tinker-specific service row is close enough from here because Melinda is still inside its generous range. That repair-looking row is the one that matters for a broke crash survivor, so I do not touch Rosanne, the trapdoor, or the coffers yet.

**Beat 2**

I choose Melinda's repair/identify service row.

The context menu closes and Melinda talks to me: `You want to hire me to repair what at 10 gold per durability? Or maybe identify an item for 200 gold?`

**Beat 3**

I stop with the target cursor up.

That cursor is not a free identify, not a purchase, and not a repaired knife. It is the next input waiting on my screen. With 0 gold, the prompt already smells money-gated, and I do not blindly target the starter knife or a coffer just to make something happen.

Mechanical friction learned:

- Melinda is a normal tinker vendor surface from the clock-shop frontage: buy/sell rows exist, but they still lead into the same 0-gold problem unless I find money or sellable goods.
- Her tinker service row opens a target cursor and quotes `10 gold per durability` for repair or `200 gold` for identify.
- The service is not a starter-cash route. It consumes a later target choice and likely payment; it does not create gold, items, skill gain, quest state, or discovery flags by itself.
- The Grand Clock Makers door, Rosanne, the basement trapdoor, Suki, and the coffers remain untested.

Next pressure:

Mira ends at `Point3D(2996,1101,5)`, still at the Grand Clock Makers frontage with Melinda's repair/identify target cursor active. The next client-visible decision is to cancel the cursor, target a known item and accept the likely 0-gold rejection, or leave the tinker service unresolved and test a different visible surface such as Rosanne, the door, the trapdoor, or Suki.

## Run 292 - The Knife Is Not Tinker Work

I start at `Point3D(2996,1101,5)`, facing south/southeast on the `The Grand Clock Makers` sign. Melinda's repair/identify target cursor is still up from the last click. The screen is still a safe Britain shop frontage: Melinda is northwest, Rosanne the tinker guildmaster is west, Suki and `Profuse Provisions` are east/southeast, Paulin the guard is south, and the dark wood door, basement trapdoor, two coffers, tavern sign, and provisioner sign are all visible. I do not see a hostile in the 18-tile screen.

**Beat 1**

I target the equipped starter knife with Melinda's cursor.

Melinda does not identify it, repair it, appraise it, or turn the prompt into a price quote. She just says: `I cannot repair that.`

That clears the target cursor. The knife stays equipped, my gold stays at 0, and nothing in the backpack changes.

**Beat 2**

With the cursor gone, I context-click Rosanne the tinker guildmaster and read the menu.

The useful rows are not the same as Melinda's repair row. Rosanne shows vendor buy/sell rows and training rows for tinker-related skills. The guild join row is visible but too far away from where I am standing, and the weapon-engraver recharge row is also useless from here without the right tool and range. I do not choose a training row yet because the menu itself is now the current decision surface.

Mechanical friction learned:

- Melinda's prompt mentions identify, but targeting the starter knife only returns `I cannot repair that.` The service does not identify ordinary starter gear and does not repair a normal dagger/knife.
- The tinker service target cursor resolves after the failed target and leaves no gump, no target cursor, no item movement, no gold movement, no skill gain, and no discovery flag.
- Rosanne is a different surface from Melinda: she exposes buy/sell and tinker-skill teaching rows, while guild joining and engraver recharge are visible but not immediately usable from the current spot.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, pet order, follower count, or map changed.

Next pressure:

Mira ends at `Point3D(2996,1101,5)`, still on the Grand Clock Makers frontage with Rosanne's context menu open. The next client-visible decision is to choose one of Rosanne's enabled skill-training rows, use buy/sell, move closer for the guild/recharge rows, close the menu, or leave Rosanne for the door, trapdoor, Suki, or the visible coffers.

## Run 293 - Rosanne Wants Paid First

I start at `Point3D(2996,1101,5)`, facing south/southeast on the `The Grand Clock Makers` sign with Rosanne's context menu still open. The useful visible choice is the tinker guildmaster's training surface, not the out-of-range guild row or the disabled weapon-engraver row. The screen is still quiet Britain shop-frontage noise: Melinda is northwest, Rosanne is west, Suki and `Profuse Provisions` are east/southeast, Paulin the guard is south, and the dark wood door, basement trapdoor, coffers, tavern sign, and provisioner sign are all still visible. I do not see a hostile in the 18-tile screen.

**Beat 1**

I choose Rosanne's visible `Tinkering` training row.

The context menu closes. Rosanne does not open a lesson gump and does not raise the skill immediately. She quotes the usual teaching deal instead: she will teach what she knows if I pay the full amount, and less payment means less teaching.

I have 0 gold, so the lesson stops as a payment problem on the screen. No item moves, no gold moves, no skill changes, no vendor window opens, no target cursor appears, and no discovery or quest flag fires.

Mechanical friction learned:

- Rosanne's enabled tinker-skill rows are teaching offers, not free training.
- Choosing `Tinkering` marks a pending lesson and requires handing her gold before any skill gain happens.
- With 0 gold, the visible result is only a price quote and the "less payment teaches less" warning.
- The dark wood door, basement trapdoor, Suki, Melinda's buy/sell rows, Rosanne's sell row, and the visible coffers remain untested.

Next pressure:

Mira ends at `Point3D(2996,1101,5)`, still at the Grand Clock Makers frontage with no context menu, gump, vendor window, sell list, or target cursor open. Rosanne has a pending `Tinkering` teaching offer, but Mira has 0 gold. The next client-visible decision is to context-click Rosanne again for sell/buy, leave for Suki or another cash lead, try the door or trapdoor, or abandon the lesson until there is money.

## Run 294 - I Walk Over To Suki

I start at `Point3D(2996,1101,5)`, facing south/southeast on the Grand Clock Makers frontage. Rosanne's Tinkering lesson is still just a payment problem. There is no gump, no target cursor, and no vendor window open. Suki the provisioner is visible east/southeast by `Profuse Provisions`, close enough to be a real next lead but not close enough for a clean vendor click from here.

**Beat 1**

I leave Rosanne's unpaid lesson alone and run east-southeast through the open shop frontage toward Suki.

This is ordinary Britain street movement. I stop beside Suki around `Point3D(3008,1104,0)`, facing east. The clock-shop sign, dark wood door, basement trapdoor, and one coffer slide behind me; `Profuse Provisions`, another coffer, a dark wood door, and the `Premier Gems` marker are now the near screen. Paulin the guard is still in view, Taylor is just northwest of Suki, and I can see a dog, a cat, citizens, and an adventurer-looking passerby east of the shop. None of them pushes into combat.

**Beat 2**

I context-click Suki and read the menu.

Suki is not an automatic cash payout. Her visible surface is a provisioner context menu: buy and sell rows, an unusual-item/value row, a speech/help row, and likely Camping or Mercantile teaching rows. I do not choose the sell row yet, because the menu itself is now the choice surface and I still need to decide whether to risk selling the empty canteen or listen to the survival-gear talk first.

Mechanical friction learned:

- Reaching Suki from the clock shop is a short safe-town move, not a terrain or threat problem.
- Suki's provisioner menu is richer than the earlier rumor citizens: it exposes buy/sell, item examination, speech/help, and teaching-style rows.
- The sell row has not been pressed yet. No sell list, sale confirmation, item movement, gold movement, skill gain, target cursor, gump, quest flag, discovery flag, damage, crime flag, or pet-order change happened.
- Code tracing suggests a provisioner may buy an empty `Waterskin` only if that vendor's generated buy list includes that type; the player-visible test is still selecting the sell row and seeing whether the canteen appears.

Next pressure:

Mira ends at `Point3D(3008,1104,0)`, facing east beside Suki with Suki's context menu open. The next client-visible decision is to choose `Sell`, choose the speech/help row, use the unusual-item/value row, buy nothing with 0 gold, pick a paid teaching row, or close the menu and move on.

## Run 295 - Suki Explains Survival Gear

I start at `Point3D(3008,1104,0)`, facing east beside Suki with her provisioner context menu already open and read. The screen is still a safe Britain shop frontage: my fox is treated as close, Suki stands one tile east, Taylor is northwest, Paulin the guard is south, `Profuse Provisions` and its coffer/door are just below me, `Premier Gems` is east/southeast, and the animals, citizens, jeweler, and adventurer-looking passerby are all passive context. No hostile is inside the 18-tile screen.

**Beat 1**

I leave the random sell row alone and choose Suki's visible speech/help row.

The context menu closes. Suki gives the normal vendor greeting and opens a gump titled `The Right Survival Gear`. This is not a buy list, sell list, price prompt, or target cursor. It is a scrollable help window with a close button, so I stop treating the menu as the active surface.

**Beat 2**

I read `The Right Survival Gear`.

The useful lesson is practical, not a quest. Light sources burn out. Ten-foot poles passively check traps in my pack. Bedrolls, campfires, and tents are the survival chain; small tents buy a short emergency pause outside, while camping tents are the long-run storage/rest tool and can even work underground when skilled enough. Suki also explains merchant crates for home crafting sales and says unusual decorative items can be valued by asking a provisioner, then sold to the merchant type she names.

Mechanical friction learned:

- Suki's speech/help row is deterministic and opens a `SpeechGump`; it does not sell, buy, appraise, teach, or move inventory by itself.
- The gump confirms provisioners are survival-route teachers: light, trap checking, camping, tents, merchant crates, and unusual-item appraisal all belong to this surface.
- The `Sell` row is still untested. I did not learn whether Suki's generated sell table contains the empty canteen, and I did not risk selling starter water storage.
- No movement, gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, follower count, or map changed.

Next pressure:

Mira remains at `Point3D(3008,1104,0)`, facing east beside Suki with `The Right Survival Gear` open and read. The next client-visible decision is to close the help gump, then either reopen Suki's menu for `Sell` or unusual-item appraisal, avoid paid teaching with 0 gold, test the Profuse Provisions door, or move east/southeast toward `Premier Gems`.

## Run 296 - Suki Will Not Price My Medical Record

I start at `Point3D(3008,1104,0)`, facing east beside Suki with `The Right Survival Gear` still open and already read. The screen is unchanged town frontage: my fox is treated as close, Suki is one tile east, Taylor is northwest, Paulin the guard is south, `Profuse Provisions` and its coffer/door are below me, and `Premier Gems` is east/southeast with Elana at the edge of the screen. The animals, citizens, guard, and passerby are passive context. No hostile is inside the 18-tile screen.

**Beat 1**

I close `The Right Survival Gear`.

The help gump disappears. Nothing else fires: no vendor list, no sell list, no target cursor, no skill, no gold, no item movement, no route marker, and no discovery flag.

**Beat 2**

I context-click Suki again and choose the unusual-item/value row she just taught me about.

Suki asks which unusual item she should examine for `25 gold`, and the cursor changes to a target cursor. This is not a price quote yet and not a free appraisal; it is the screen waiting for me to target something.

**Beat 3**

I target the `Medical Record` in my backpack.

Suki refuses it as appraisal stock: `I cannot put a value on that.` The target cursor clears. The record stays in my pack, my gold stays at `0`, and no sell list, buy list, quest, discovery, skill gain, or inventory change appears.

Mechanical friction learned:

- Suki's unusual-item row is a target-cursor service, separate from her help gump and separate from `Sell`.
- The service quote is `25 gold`, but non-relic targets fail before payment matters; the Medical Record is not something this provisioner values.
- The Medical Record remains lore/identity proof and a readable tech item, not a starter-cash object.
- The `Sell` row is still untested. The empty canteen may only matter if Suki's current generated buy list includes `Waterskin`; the visible test is still opening `Sell`, not assuming a sale.
- No movement, gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, pet order, follower count, or map changed.

Next pressure:

Mira remains at `Point3D(3008,1104,0)`, facing east beside Suki with no gump, context menu, vendor window, sell list, or target cursor open. The next client-visible decision is to reopen Suki for `Sell`, test the visible Profuse Provisions door, step east/southeast toward `Premier Gems`, or leave the shopfront cluster for a different cash lead.

## Run 297 - Suki Would Buy The Empty Canteen For One Gold

I start at `Point3D(3008,1104,0)`, facing east beside Suki with no gump, context menu, vendor window, sell list, or target cursor open. The screen is still safe Britain shop frontage: Suki stands one tile east, Taylor is northwest, Paulin the guard is south, `Profuse Provisions` and its coffer/door are below me, and `Premier Gems` plus Elana are east/southeast. The visible animals, citizens, guard, trainer, jeweler, and passerby are passive context. No hostile is inside the 18-tile screen.

**Beat 1**

I context-click Suki again and go straight for the visible `Sell` row.

The context menu closes and a vendor sell list opens. It is not asking for a target and it is not the unusual-item appraisal service. The only useful starter item it exposes is my `empty canteen`, and the price is bad: `1` gold.

**Beat 2**

I read the sell list and do not accept the sale yet.

Selling the canteen would turn the first real cash lead into only `1` gold while also giving up my water container. I leave the sell list open instead of pressing through the sale.

Mechanical friction learned:

- Suki's `Sell` row is the normal vendor sell surface. It searches my backpack for item types in her generated buy list, then opens a sell list if anything matches.
- In this visible attempt, the generated provisioner buy list includes `Waterskin`, so the empty canteen appears as sellable.
- The canteen's base buyback price is `2`, but the player-facing sell offer is halved to `1` gold.
- The cooked alien meat and Medical Record do not appear as current provisioner sell-list cash; the Medical Record already failed the separate appraisal cursor.
- No sale has been confirmed. The canteen, cooked alien meat, Medical Record, equipped knife, clothing, gold total, skills, stats, follower order, quest state, discovery flags, and map position are unchanged.

Next pressure:

Mira remains at `Point3D(3008,1104,0)`, facing east beside Suki with Suki's vendor sell list open and read. The next client-visible decision is to sell the empty canteen for `1` gold, cancel the sell list and keep the canteen, test the Profuse Provisions door, or walk east/southeast toward `Premier Gems`.

## Run 298 - One Gold Costs The Canteen

I start at `Point3D(3008,1104,0)`, facing east beside Suki with the vendor sell list still open and already read. The screen is the same safe Britain shop-front cluster: Suki is one tile east, Taylor is northwest, Paulin the guard is south, `Profuse Provisions` and its door are below me, `Premier Gems` and Elana sit east/southeast, and the visible dogs, cat, citizens, trainer, guard, jeweler, and passerby are all passive context. No hostile is inside the 18-tile screen.

**Beat 1**

I stop trying to keep the dead canteen as a hope object. I select the visible `empty canteen` row and press the sell/accept control.

The sell list closes. There is no target cursor, no buy window, no speech lesson, and no second confirmation gump. I hear the gold-drop result: the canteen is gone from my backpack, and the first real cash appears as `1` gold. That is not enough to make Suki's `25 gold` appraisal useful, and it probably will not satisfy any full training quote, but it is no longer zero.

Mechanical friction learned:

- This was the normal visible vendor sell-list flow, not Suki's unusual-item appraisal cursor and not a dropped-item barter.
- The sold canteen met the vendor sell checks: it was in my backpack, movable, standard loot, and on the provisioner's sell table.
- `GenericSellInfo` is not resellable here, so the canteen is consumed/deleted instead of moving into Suki's resale pack.
- The only payout is the listed `1` gold. The sale adds gold to my backpack and plays the gold-drop sound, but it does not open a new gump, grant a skill, set a quest flag, or discover a route.

Next pressure:

Mira remains at `Point3D(3008,1104,0)`, facing east beside Suki with no gump, context menu, vendor window, sell list, or target cursor open. The next client-visible decision is to test whether `1` gold changes any paid lesson or appraisal, try the Profuse Provisions or Premier Gems door, talk to Elana at `Premier Gems`, or leave the shopfront cluster for a better cash lead. The survival cost is real: Mira has food and a Medical Record, but no water container.

## Run 299 - Elana Quotes Merchant Lessons

I start at `Point3D(3008,1104,0)`, facing east beside Suki with no gump, context menu, vendor window, sell list, or target cursor open. The screen is still Britain shopfront safety: Suki is one tile east, Taylor is northwest, Paulin the guard is south, the Profuse Provisions door and coffer are below me, and `Premier Gems` plus Elana are visible east/southeast. The visible dogs, cat, citizens, trainer, guard, jeweler, and passerby are passive. No hostile is inside the 18-tile screen.

**Beat 1**

I run east/southeast along the open shopfront toward the `Premier Gems` marker instead of spending the single gold at Suki or Rosanne.

The move is ordinary town footing. I stop near `Point3D(3024,1106,0)`, facing east with Elana two tiles away. My fox is treated as following close enough for the client-side run. The new screen keeps `Premier Gems` and its door beside me, with a coffer inside/near the shop. `The Lord's Clothiers` comes into view northeast with Kachine the weaver, Malcolm the tailor, clothier tools, and another door. No hostile appears.

**Beat 2**

I context-click Elana and read the visible jeweler menu.

This is a real vendor surface, not a rumor citizen. The menu exposes buying, selling, a jeweler service row, and a Mercantile teaching row. I do not open buy or sell yet, and I do not touch the nearby coffer.

**Beat 3**

I choose the visible `Mercantile` teaching row.

The context menu closes. Elana quotes a paid lesson and gives the familiar warning that full payment teaches all she can, while less payment teaches less. I do not hand over the `1` gold yet, so there is no skill gain, no inventory change, no vendor list, no target cursor, no quest flag, and no discovery flag.

Mechanical friction learned:

- The nearby jeweler is a practical next lead, but not immediate cash by merely talking.
- Elana can teach Mercantile because vendors can teach skills they know well enough; the first click only sets up a paid learning offer.
- With only `1` gold, the lesson is now a choice: pay a tiny partial lesson, keep the coin, try Elana's buy/sell surface, try the Premier Gems door, or move on.
- The jeweler service row was visible, but I did not select it, so no identify target cursor opened and no item was tested.
- No terrain, combat, pet, door, coffer, hunger, thirst, damage, crime, bank, discovery, or tarot state changed.

Next pressure:

Mira ends at `Point3D(3024,1106,0)`, facing east near Elana at `Premier Gems`, with no gump, context menu, vendor window, or target cursor open. Elana's Mercantile lesson quote is pending in the player sense: the next client-visible decision is to drop the single gold for a tiny Mercantile lesson, reopen Elana for buy/sell or the jeweler service row, try the Premier Gems door, ignore the coffer, or continue northeast toward `The Lord's Clothiers`.

## Run 300 - Elana Cannot Identify My Record

I start at `Point3D(3024,1106,0)`, facing east beside `Premier Gems`. Elana is still two tiles east of me. The shopfront screen is safe: my fox is treated as close, the nearby dogs and cat are just town animals, Seward and Claire look like passersby, and Kachine and Malcolm are visible near `The Lord's Clothiers` to the northeast. The visible coffers and doors stay tempting but untested. No hostile is on the screen.

**Beat 1**

I context-click Elana again and go back to the visible jeweler service row instead of dropping my only gold on the Mercantile lesson.

The context menu is the same kind of vendor surface as before: buy, sell, the jeweler's identify/service row, and Mercantile teaching. I choose the service row. Elana immediately asks whether I want her to identify an item for `200` gold, and the cursor changes into a target cursor. This is not a vendor buy list, not a sell list, and not a teaching payment.

**Beat 2**

I read the prompt and target the `Medical Record` in my backpack, because it is still the strangest thing I own and Suki already refused to appraise it.

Elana refuses it too, but in a different way: `That does not need my services.` The target cursor clears. She does not take the `1` gold, does not identify anything, does not move the Medical Record, and does not open another window.

Mechanical friction learned:

- Elana's jeweler service row is an identify-item target cursor with a visible `200` gold quote.
- The target must be a shard identification item that belongs to a jeweler's service surface; ordinary starter objects do not even reach the payment check.
- The Medical Record is not jeweler-identifiable. It stays in the backpack as identity/lore evidence, not cash and not a paid identify lead.
- Elana's earlier Mercantile teaching quote remains unpaid. I still have `1` gold, `0.0` Mercantile, no vendor buy/sell window, no open gump, and no target cursor.
- No movement, terrain, combat, pet, door, coffer, hunger, thirst, damage, crime, bank, discovery, or tarot state changed.

Next pressure:

Mira remains at `Point3D(3024,1106,0)`, facing east near Elana at `Premier Gems`. The next client-visible decision is to spend the single gold on the pending Mercantile lesson, reopen Elana for buy or sell, try the Premier Gems door, ignore the coffer, or move northeast toward `The Lord's Clothiers`, Kachine, and Malcolm.

## Run 301 - Jewels Will Not Buy Rations

I start at `Point3D(3024,1106,0)`, facing east beside `Premier Gems` with Elana two tiles away. The screen is still Britain safety: my fox is treated as close, the dogs and cat are town animals, Seward and Claire are passersby, and Kachine the weaver plus Malcolm the tailor are visible northeast near `The Lord's Clothiers`. The coffers, shop doors, clothier trapdoor, loom, and spinning wheel are visible but untested. No hostile is on the screen.

**Beat 1**

I context-click Elana again and choose `Sell` instead of burning my only coin on the old Mercantile quote.

The context menu closes, but no sell list opens. Elana says, `You have nothing I would be interested in.` My backpack is unchanged: `1` gold, `10` cooked alien meat, and the `Medical Record`. The jeweler does not treat the meat or record as jewelry stock, and the sold canteen is already gone.

**Beat 2**

I leave the jeweler dead end and run east/northeast across the open shopfront toward the clothier marker.

The move is ordinary town footing, not a maze or threat problem. I stop near `Point3D(3033,1103,0)`, facing east, one tile west of Kachine. My fox is assumed to keep up. The screen tightens around the clothier cluster: Kachine is beside me, Malcolm is a few steps northeast, a coffer sits by the clothier side, and the spinning wheel, looms, basement trapdoor, door, and `The Lord's Clothiers` marker are all in view. Elana and `Premier Gems` remain behind me to the west/southwest. No hostile enters range.

**Beat 3**

I context-click Kachine and read the visible weaver menu.

This is another real vendor surface, but with clothier-specific friction. The visible choices include buy, sell, a speech/help row about altering cloaks and robes, a service row that asks for robe/cloak tailoring, hat repair, or item identification, and Tailoring teaching. I do not press a row yet, because the menu itself is the new decision point.

Mechanical friction learned:

- Elana's `Sell` row is not a cash path for cooked alien meat or the Medical Record. It fails visibly with the no-interest line and opens no sell list.
- The jeweler sell surface is jewelry/crystal/artifact-shaped, not a general pawnshop.
- Kachine the weaver is the next clothier-system lead. Her menu points at clothing alteration, hat repair, identify service, Tailoring teaching, buy/sell, and an information gump.
- The clothier-side coffer, basement trapdoor, door, loom, spinning wheel, Malcolm, Kachine's actual rows, and Kachine's buy/sell lists remain untested.
- No gold, item, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, vendor window, pet order, follower count, or map changed.

Next pressure:

Mira ends at `Point3D(3033,1103,0)`, facing east beside Kachine, with Kachine's context menu open and read. The next client-visible decision is to choose Kachine's speech/help row, buy, sell, service row, or Tailoring lesson; close the menu and try Malcolm; test the clothier door or basement trapdoor; or ignore the coffer.

## Run 302 - Kachine Explains Robe Altering

I start at `Point3D(3033,1103,0)`, facing east beside Kachine with her context menu already open and read. The screen is the same safe clothier frontage: Kachine is one tile east, Malcolm is a few steps northeast, Claire and Seward look like passersby, Elana is behind me to the west/southwest, and the nearby dogs, cat, and citizens are passive. The visible coffer, spinning wheel, looms, basement trapdoor, clothier door, and `The Lord's Clothiers` marker are still tempting but untested. No hostile is on the screen.

**Beat 1**

I choose Kachine's visible speech/help row about altering cloaks and robes.

The context menu closes. Kachine greets me and opens a custom speech gump titled `Altering Cloaks And Robes`. It is not a vendor buy list, not a sell list, not a teaching payment, and not a target cursor.

**Beat 2**

I read the gump instead of pressing through it.

The message is practical: if I dislike the look of a cloak or robe, a tailor/weaver can alter it back toward a normal-looking cloak or robe; odd garments may be serviceable even if they do not look like standard shop stock. Hoods and cowls can be double-clicked while worn to change color from equipped robes or cloaks, which is mainly outfit coordination, not survival cash.

Mechanical friction learned:

- Kachine's speech/help row is an information gump, not a service execution.
- The robe/cloak alteration lead still requires using the separate clothier service row, then targeting an eligible cloak, robe, hat, or tailor-identifiable item.
- My current gear is a fancy shirt, long pants, boots, and a knife. I have no visible cloak, robe, hood, cowl, hat, or tailor-identifiable item to use with that service right now.
- The single gold, cooked alien meat, Medical Record, equipment, skills, stats, hunger, thirst, crime state, follower order, quest state, discovery flags, map position, clothier coffer, basement trapdoor, clothier door, Malcolm, and Kachine's buy/sell/service/teaching rows are unchanged.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine, with the `Altering Cloaks And Robes` speech gump open and read. The next client-visible decision is to close/OK the gump, then either test Kachine's buy/sell/service/Tailoring rows, talk to Malcolm, try the clothier door or basement trapdoor, or keep ignoring the coffer.

## Run 303 - Kachine Does Not Want My Starter Kit

I start at `Point3D(3033,1103,0)`, facing east beside Kachine with the `Altering Cloaks And Robes` speech gump still open and already read. The clothier frontage is calm: Kachine is one tile east, Malcolm is a few steps northeast, Claire and Seward look like passersby, Elana is behind me to the west/southwest, and the nearby dogs, cat, citizens, coffer, spinning wheel, looms, basement trapdoor, clothier door, and `The Lord's Clothiers` sign are all visible context. No hostile is on the screen.

**Beat 1**

I press the visible close/OK control on the speech gump.

The gump closes cleanly. I am back on the same Britain shopfront screen with no target cursor and no vendor list. Nothing in my backpack, equipment, skills, follower state, quest state, discovery flags, gold, hunger, thirst, or position changes.

**Beat 2**

I context-click Kachine again and choose her visible `Sell` row.

The context menu closes, but no sell list opens. Kachine gives the same kind of dead-end vendor line Elana gave earlier: `You have nothing I would be interested in.` My `1` gold, `10` cooked alien meat, and `Medical Record` stay in the backpack. The weaver is not a pawnshop for alien rations or paperwork.

**Beat 3**

I do not touch the nearby coffer. I context-click Malcolm, the tailor standing a few steps northeast, and read his visible menu.

Malcolm is another clothier vendor surface. The menu exposes buy, sell, the same speech/help lead about altering cloaks and robes, the clothing repair/alter/identify service row, and Tailoring teaching. I stop with the menu open instead of picking a second clothier row blind.

Mechanical friction learned:

- Kachine's `Sell` row is a normal vendor sell check, not an appraisal. It looks inside my backpack for item types the weaver buys.
- My current backpack has gold, cooked alien meat, and the Medical Record. None match the visible cloth/weaver cash path, so no sell list opens.
- The weaver's sell table is cloth-and-tailor shaped: yarn, scissors, dyes, dye tubs, and paint canvas when those buyback rolls are active. Starter food and the Medical Record are outside that lane.
- Malcolm appears to offer the same broad tailor interaction family as Kachine, but I have not selected any of his rows yet.
- No movement, combat, terrain friction, pet order, door, trapdoor, coffer, target cursor, vendor window, skill gain, payment, discovery flag, or tarot state changed.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east near Kachine and Malcolm, with Malcolm's tailor context menu open and read. The next client-visible decision is to choose Malcolm's buy, sell, speech/help, service, or Tailoring row; close the menu and try the clothier door or basement trapdoor; or leave the coffer alone and move on.

## Run 304 - Malcolm Will Not Tailor My Shirt

I start at `Point3D(3033,1103,0)`, facing east beside Kachine, with Malcolm's tailor context menu already open and read. The same clothier frontage fills the screen: my fox is close, Kachine is beside me, Malcolm is four steps northeast, Claire and Seward look like passersby, Elana is behind me near `Premier Gems`, and the dogs, cat, citizens, coffer, spinning wheel, looms, basement trapdoor, clothier door, and `The Lord's Clothiers` marker are all visible. No hostile is on the screen.

**Beat 1**

I choose Malcolm's visible tailor service row instead of buying random shop stock or spending my only coin on a lesson.

The context menu closes. Malcolm gives the working quote out loud: robe or cloak alteration costs `5` gold, hat repair costs `10` gold per durability, and item identification costs `200` gold. A target cursor opens. This is not a buy list, sell list, or teaching payment.

**Beat 2**

I read the quote and target my equipped fancy shirt, because it is the only clothing I am actually wearing that sounds even close to a tailor job.

Malcolm refuses it with `That does not need my services.` The target cursor clears. He does not take my `1` gold, does not alter the shirt, does not repair anything, does not identify anything, and does not open another window.

Mechanical friction learned:

- Malcolm's service row is the same visible tailor service family Kachine hinted at: robe/cloak normalization, hat repair, or tailor-identification.
- A normal `FancyShirt` is not in the service lane. The service wants robe/outer-torso, cloak, hat, or a tailor-compatible unidentified item, not ordinary starter shirt clothing.
- The service quotes are far above my cash except for the `5` gold robe/cloak alteration, and I still only have `1` gold.
- Malcolm's buy, sell, speech/help, Tailoring lesson, the clothier door, basement trapdoor, and nearby coffer remain untested.
- No movement, combat, terrain friction, pet order, vendor window, payment, item movement, skill gain, quest state, discovery flag, or tarot state changed.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with no open gump, no open context menu, no vendor window, and no target cursor. The next client-visible decision is to context-click Malcolm again for buy, sell, speech/help, or Tailoring; try Kachine's untested buy/service/teaching rows; close out the clothier cluster by trying the door or basement trapdoor; or keep ignoring the coffer and move on.

## Run 305 - Malcolm Does Not Want My Pack

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and a few steps from Malcolm. No gump, context menu, vendor window, or target cursor is open. The same safe Britain clothier frontage is visible: my fox is close, Kachine stands one tile east, Malcolm is northeast, Claire and Seward look like passersby, Elana and `Premier Gems` are behind me, and the clothier coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign are all still on-screen. No hostile is in the 18-tile view.

**Beat 1**

I context-click Malcolm again and choose `Sell`, checking whether the tailor is any better than Kachine or Elana as a cash path.

The context menu closes. No sell list opens. Malcolm says, `You have nothing I would be interested in.` My backpack is still only `1` gold, `10` cooked alien meat, and the `Medical Record`; the equipped fancy shirt, long pants, boots, and knife stay worn and are not part of this backpack sell check.

**Beat 2**

Before leaving him, I context-click Malcolm again and choose the visible speech/help row.

The menu closes and Malcolm opens the same custom gump title Kachine used: `Altering Cloaks And Robes`. This is not a vendor buy list, not a sell list, not a Tailoring lesson, and not the service target cursor.

**Beat 3**

I read the gump instead of pressing past it.

The message is clothing-service advice only: tailors can make odd-looking robes or cloaks look normal again, and hoods or cowls can copy color from something equipped, such as a robe. That confirms Malcolm's help row is informational. It does not alter my shirt, sell anything, teach Tailoring, set a discovery flag, or open a target cursor.

Mechanical friction learned:

- Malcolm's `Sell` row is a normal vendor sell check against the backpack. It is not a pawnshop for alien meat or paperwork.
- The tailor sell lane can care about cloth, clothing, sewing tools, dyes, rare robes, and similar tailor stock, but this attempt had none of that in the backpack.
- Equipped clothing is not automatically offered to the vendor sell list. My starter fancy shirt may be a tailor-shaped object in general, but it stayed equipped and was not part of this no-interest sell check.
- Malcolm's speech/help row duplicates Kachine's robe/cloak information gump. It teaches player knowledge only; the actual paid service is still the separate target-cursor row.
- No movement, combat, terrain friction, pet order, vendor window, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, or tarot state changed.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with Malcolm's `Altering Cloaks And Robes` speech gump open and read. The next client-visible decision is to close/OK the gump, then either test Malcolm's buy or Tailoring row, try Kachine's untested buy/service/Tailoring rows, test the clothier side door or basement trapdoor, or leave the coffer alone and move on.

## Run 306 - Malcolm's Clothes Are Not One-Gold Clothes

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm with Malcolm's `Altering Cloaks And Robes` gump still open and already read. The screen has not moved: my fox is close by, Kachine is one step east, Malcolm is a few steps northeast, Claire and Seward look like harmless passersby, Elana is behind me near `Premier Gems`, and the clothier coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign are still visible. No hostile is in the 18-tile view.

**Beat 1**

I press the visible close/OK control on Malcolm's robe-and-cloak information gump.

The gump closes. I am back on the same shopfront screen with no target cursor and no vendor list yet. Nothing in my pack, equipment, skills, follower state, quest state, discovery flags, gold, hunger, thirst, crime state, or position changes.

**Beat 2**

I context-click Malcolm again and choose his visible `Buy` row.

The context menu closes and a vendor buy window opens. This is the first Malcolm row that shows actual shop stock instead of advice, a refusal line, or a target cursor.

**Beat 3**

I read the buy window without buying anything.

The useful news is also the bad news: Malcolm's list is tailor-shaped, and the cheap end is still above my money. I can treat uncut cloth at `3` gold, scissors at `11` gold, and bolts of cloth at `100` gold as real clothier prices; any shirts, pants, robes, dye gear, sewing kit, or study-book stock that appears is also outside my `1` gold reach. I do not click a quantity or confirm a purchase.

Mechanical friction learned:

- Malcolm's `Buy` row is a normal vendor buy surface. It opens the buy list and sends the shop inventory; it does not spend gold by itself.
- The reliable tailor stock floor already blocks me: `1` gold cannot buy uncut cloth at `3`, scissors at `11`, or a bolt at `100`.
- The `Buy` row is useful for price knowledge, not an immediate survival upgrade with my current cash.
- No movement, combat, terrain friction, pet order, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, or coffer interaction occurred.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with Malcolm's vendor buy window open and read. The next client-visible decision is to close/cancel the buy window, try Malcolm's Tailoring lesson row, return to Kachine's untested buy/service/teaching rows, test the clothier side door or basement trapdoor, or keep ignoring the coffer and move on.

## Run 307 - Malcolm Wants Payment Before Tailoring

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm with Malcolm's vendor buy window still open and already read. The screen is the same safe Britain clothier frontage: my fox is close, Kachine is one step east, Malcolm is a few steps northeast, Claire and Seward look like passersby, Elana is behind me near `Premier Gems`, and the clothier coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` marker are still visible. No hostile is in the 18-tile view.

**Beat 1**

I close/cancel Malcolm's buy window instead of clicking a quantity.

The buy list disappears. I am back on the shopfront with no vendor window, no target cursor, and no item movement. My `1` gold is still in the backpack. The remembered price floor is unchanged: the cheap tailor shelf starts above me at `3` gold for uncut cloth.

**Beat 2**

I context-click Malcolm again and choose his visible `Tailoring` teaching row.

The context menu closes. Malcolm does not open a gump or hand over skill for free. He gives the standard teacher pitch: he will teach all he knows if paid in full, and less payment teaches less. That means this row is a gold handoff path, not a free lesson button.

**Beat 3**

I read the offer and keep my only coin for the moment.

I do not drag the `1` gold onto Malcolm. Nothing is purchased and my Tailoring skill does not move. The important part is the client state: I now have a pending Tailoring lesson offer from Malcolm, but using it means sacrificing actual cash, even if the shard would accept a smaller payment for a smaller lesson.

Mechanical friction learned:

- Malcolm's `Tailoring` row is visible from the normal vendor context menu because he has Tailoring above the teach threshold.
- Selecting it sets up a paid teaching offer. The actual skill increase only happens after handing gold to Malcolm.
- A partial payment is mechanically meaningful, but my whole cash stack is only `1` gold, so this is now a decision rather than a free progression step.
- Malcolm's buy, sell, speech/help, and service rows are all understood enough to move on unless I deliberately spend gold or test doors/trapdoors.
- No movement, combat, terrain friction, pet order, purchase, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, or coffer interaction occurred.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with no open gump, no open context menu, no vendor window, and no target cursor. Malcolm's Tailoring payment offer is pending in the player/vendor teaching state. The next client-visible decision is to pay Malcolm some or all of the `1` gold for a tiny Tailoring lesson, test Kachine's still-untested buy/service/teaching rows, try the clothier side door or basement trapdoor, or leave the coffer alone and move on.

## Run 308 - One Coin Buys One Stitch of Skill

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm with no open gump, no context menu, no vendor window, and no target cursor. Malcolm's Tailoring lesson offer is still the active client-visible choice. The same safe Britain clothier frontage fills the screen: my fox is beside me, Kachine is one step east, Malcolm is a few steps northeast, Claire and Seward look like passersby, Elana is behind me near `Premier Gems`, and the clothier coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` marker are still visible. No hostile is in the 18-tile view.

**Beat 1**

I drag my single `1` gold stack from the backpack onto Malcolm to accept the partial Tailoring lesson.

He takes it. The lesson resolves with the teacher line and the skill-gain notice: Malcolm shows me something of how this is done, and my skill level increases. Since I only paid one coin, the visible result is tiny: Tailoring moves from `0.0` to `0.1`, the gold stack disappears, and the teaching offer clears.

**Beat 2**

I stop to read the result and check the practical fallout.

My backpack now holds only cooked alien meat and the Medical Record. I have no gold left for Malcolm's cloth, scissors, service fee, identify fee, or more training. Position, facing, pet order, hunger, thirst, combat state, crime state, quest state, discovery flags, doors, trapdoor, coffer, vendor windows, context menus, gumps, and target cursor do not change.

Mechanical friction learned:

- Malcolm's visible `Tailoring` row is a two-step teacher flow: choose the row, then hand gold to the teacher.
- Partial payment works. A `1` gold handoff gives only `0.1` Tailoring, then clears the pending learning state.
- Spending the only coin removes the immediate buy/training/service options that require cash. The nearby Kachine rows, clothier door, basement trapdoor, and shopfront route remain the visible next tests.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with no open gump, no open context menu, no vendor window, no target cursor, `0` gold, and `0.1` Tailoring. The next client-visible decision is to test Kachine's still-untested buy/service/teaching rows despite being broke, try the clothier side door or basement trapdoor, ignore the coffer, or leave the shopfront to look for another cash or water lead.

## Run 309 - Kachine Has Cloth But No Free Cloth

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm with no open gump, context menu, vendor window, or target cursor. The same Britain clothier frontage is on screen: my fox is still with me, Kachine is one step east, Malcolm is a few steps northeast, Claire and Seward look harmless, Elana is behind me near `Premier Gems`, and the clothier coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign are visible. No hostile is in the 18-tile view.

**Beat 1**

I context-click Kachine again instead of leaving the shopfront.

The menu is no longer just the old weaver choices. Because Malcolm's one-coin lesson left me with `0.1` Tailoring, Kachine now exposes a bulk-order row along with the normal `Buy`, `Sell`, robe/cloak help, clothing service, and `Tailoring` teaching rows. I read that as a real new job-looking option, but I do not click it yet.

**Beat 2**

I choose Kachine's visible `Buy` row first, using the shop list as price reconnaissance even though I have `0` gold.

The context menu closes and a vendor buy window opens. Nothing is bought just by opening it.

**Beat 3**

I read the buy window and leave it open.

Kachine's list confirms the broke-player problem from the clothier side. The reliable cloth shelf starts at `3` gold for uncut cloth and `100` gold for a bolt of cloth, and the weaver tool row includes scissors at `11` gold. Other weaver stock can include dyes or a dye tub at `8`, yarn at `18`, a tailoring study book at `7500`, or a canvas at `500`, but those are all fantasy prices while my backpack has no coin at all. I do not choose a quantity or confirm a purchase.

Mechanical friction learned:

- Kachine's `Buy` row is inspection-safe; opening it does not spend gold.
- `0` gold blocks every visible useful weaver purchase, including the cheap cloth at `3` gold.
- The new bulk-order row is a visible consequence of having any Tailoring skill. It is not tested yet, because I stopped with Kachine's buy list open and read.
- No movement, combat, terrain friction, pet order, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, coffer, or target cursor changed.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with Kachine's vendor buy window open and read, no context menu, no target cursor, `0` gold, and `0.1` Tailoring. The next client-visible decision is to close/cancel the buy window, then decide whether to test Kachine's newly visible bulk-order row, try her service or teaching row, test the clothier door or basement trapdoor, ignore the coffer, or leave for another cash or water lead.

## Run 310 - Kachine Offers Work Before Cloth

I start at `Point3D(3033,1103,0)`, facing east beside Kachine with her buy window still open and already read. The same Britain clothier frontage is on-screen: my fox is with me, Kachine is one step east, Malcolm is a few steps northeast, Claire and Seward look harmless, Elana is behind me near `Premier Gems`, and the coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign remain visible. No hostile is in the 18-tile view.

**Beat 1**

I close/cancel Kachine's buy window instead of clicking a quantity I cannot pay for.

The vendor list disappears. I am back on the shopfront with no vendor window, no target cursor, and no item movement. My pack is still just cooked alien meat and the Medical Record, with `0` gold.

**Beat 2**

I context-click Kachine again and choose the newly visible bulk-order row.

This is not a free cloth shelf and not a teaching pitch. Kachine answers that I can get an order now, then opens a bulk-order accept gump.

**Beat 3**

I read the gump and stop before pressing either button.

The window is a small tailoring work offer: title `A bulk order`, a request line thanking me and asking for help, `Amount to make`, `Item requested`, and the two visible choices `Ok` and `CANCEL`. At `0.1` Tailoring this is low-end cloth work, not leather, special material, or exceptional work. I do not accept it yet, so no deed is placed in my backpack and nothing is crafted, paid, bought, sold, moved, stolen, or trained.

Mechanical friction learned:

- Kachine's bulk-order row is usable with any positive Tailoring skill.
- Clicking the row is inspection-safe enough to reveal the job offer; it does not spend gold.
- The offer is a gump decision. `Ok` should place the deed in the backpack if there is room, while `CANCEL` should walk away from the offer.
- The row is still not a cash source by itself. It creates a work obligation I probably cannot finish without cloth, tools, or money.
- No movement, combat, terrain friction, pet order, purchase, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, coffer, or target cursor changed.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with Kachine's small tailoring bulk-order accept gump open and read. The next client-visible decision is whether to press `Ok` and carry the deed anyway, press `CANCEL`, or close the gump before testing Kachine's service/teaching rows, the clothier door, the trapdoor, or another cash/water lead.

## Run 311 - The Deed Fits, The Cloth Does Not

I start at `Point3D(3033,1103,0)`, facing east beside Kachine, with her small tailoring bulk-order accept gump still open and already read. The same Britain clothier frontage fills the screen: my fox is with me, Kachine is one step east, Malcolm is a few steps northeast, Claire and Seward look harmless, Elana is behind me near `Premier Gems`, and the coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign are all still visible. No hostile is in the 18-tile view.

**Beat 1**

I press `Ok` instead of walking away from the only job offer I have found.

The gump closes and the order becomes an actual backpack item. The client tells me the bulk order deed has been placed in my backpack. It does not hand me cloth, scissors, gold, or skill. My pack is now cooked alien meat, the Medical Record, and a small tailor bulk-order deed; my cash is still `0`.

**Beat 2**

I open the backpack deed and read it before trying to combine anything.

The deed opens its own `A bulk order` window. It repeats the same live request from Kachine's offer, shows `Amount to make`, `Amount finished: 0`, and `Item requested`, then gives me `Combine this deed with the item requested` and `EXIT`. That means the job is now portable, but it is still not solvable from my current pack. I have no cloth, no scissors, no sewing kit, and no money to buy the cheap `3` gold cloth from the same shop. I do not press `Combine`, so no target cursor opens.

Mechanical friction learned:

- Accepting the visible small BOD gump is free if the backpack has room, and it places a blessed bulk-order deed in the backpack.
- The accept button is not a reward payout. It creates a work ticket; payment and rewards require filling and turning in the deed later.
- Double-clicking the deed from the backpack opens the readable order gump. The next real action on that gump is `Combine`, which would need the requested crafted item in the backpack.
- At `0.1` Tailoring and `0` gold, this is useful route knowledge rather than an immediate cash path. I still need materials, tools, or money before the order can matter.
- No movement, combat, terrain friction, pet order, purchase, payment, sell list, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, coffer, or repeat bulk-order timer test occurred.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with the accepted small tailor bulk-order deed gump open and read. The next client-visible decision is to press `EXIT`, avoid `Combine` until there is cloth, then test Kachine's service/teaching rows, try the clothier door or trapdoor, or leave the shopfront for another cash/material lead.

## Run 312 - Kachine Teaches Only After Coin

I start at `Point3D(3033,1103,0)`, facing east beside Kachine, with the accepted small tailor bulk-order deed gump still open and already read. The same safe-looking Britain clothier frontage is on screen: my fox is beside me, Kachine is one step east, Malcolm is northeast, Claire and Seward look like passersby, Elana is behind me near `Premier Gems`, and the coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign are still visible. No hostile is in the 18-tile view.

**Beat 1**

I press `EXIT` on the deed gump.

The deed window closes cleanly. I do not press `Combine`, so no target cursor appears and the deed stays as a backpack item instead of becoming an active combine attempt. My pack is still cooked alien meat, the Medical Record, and the blessed small tailor bulk-order deed.

**Beat 2**

I context-click Kachine again instead of walking away.

Her familiar menu is still reachable from one tile away. The important visible rows are the bulk-order row, `Buy`, `Sell`, robe/cloak help, clothing service, and `Tailoring` teaching. Since the deed route is blocked by tools and cloth, I aim at the teaching row rather than opening the shop list again.

**Beat 3**

I choose Kachine's visible `Tailoring` row.

The menu closes and Kachine gives the normal teacher quote: she will teach all she knows if paid the amount in full, and less gold teaches less. The exact number is visible in the client message at the moment, but I have `0` gold, so there is nothing useful to drag onto her. No skill moves, no item moves, no vendor window opens, and no target cursor appears. This is a pending payment offer, not a free starter lesson.

Mechanical friction learned:

- The `EXIT` control on the accepted small BOD deed only closes the readable deed window. It does not delete the deed, combine an item, pay a reward, or open a target cursor.
- Kachine's `Tailoring` row is the same teacher flow Malcolm used: click the row, receive a price quote, then pay gold by dropping it onto the teacher.
- With `0` gold, the teaching quote is informational only. Partial teaching would require at least one coin, and full teaching would require the quoted amount.
- Kachine's service row, the clothier door, the basement trapdoor, and both visible coffers remain untested. The deed still cannot be filled without cloth, tools, and a matching crafted item.
- No movement, combat, terrain friction, pet order, purchase, sale, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, coffer, or repeat bulk-order timer test occurred.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with no open gump, no context menu, no vendor window, no target cursor, `0` gold, `0.1` Tailoring, and Kachine's unpaid Tailoring lesson offer pending. The next client-visible decision is to ignore the impossible lesson until she finds coin, test Kachine's service row, try the clothier door or basement trapdoor, avoid the visible coffers unless deliberately testing theft friction, or leave the shopfront for another cash/material lead.

## Run 313 - Kachine Also Wants a Target

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm with no open gump, context menu, vendor window, or target cursor. I am still broke: cooked alien meat, the Medical Record, the blessed small tailor bulk-order deed, starter clothes, boots, and knife, but `0` gold. The screen is still the Britain clothier frontage. Kachine is one step east, Malcolm is northeast, Elana is behind me near `Premier Gems`, my fox is beside me by continuity, and the visible shop props are the coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign. Claire, Seward, citizens, cats, and dogs look like town traffic, not threats.

**Beat 1**

I context-click Kachine again and read the menu instead of walking away.

The same normal rows are visible: bulk order, `Buy`, `Sell`, robe/cloak help, clothing service, and `Tailoring`. The Tailoring quote is useless while I have no coin, and the deed route is still blocked by cloth and tools, so the only untested vendor row that does not immediately require a purchase window is service.

**Beat 2**

I choose Kachine's visible clothing service row.

The menu closes. Kachine gives the same practical service quote Malcolm used: robe or cloak normalization costs `5` gold, hat repair costs `10` gold per durability, and identify costs `200` gold. Then the client gives me a target cursor. Nothing is paid, bought, repaired, identified, moved, trained, discovered, stolen, or crafted just by opening the service target.

**Beat 3**

I stop and read the prompt instead of clicking randomly into the backpack.

This is now a target-choice problem, not a travel problem. I do not visibly own a robe, cloak, hat, or tailor-identifiable unidentified item, and I have `0` gold even if I did. The Medical Record has already failed other appraisal/identify-style attempts, and the fancy shirt already failed Malcolm's tailor service. I leave the cursor unresolved because the client is waiting for an item target or a cancel.

Mechanical friction learned:

- Kachine's service row is not a free cloth or repair path. It opens a target cursor after quoting prices.
- The visible prices are `5` gold for robe/cloak normalization, `10` gold per hat durability, and `200` gold for identification.
- With `0` gold and no obvious robe, cloak, hat, or tailor-identifiable item, the service row adds no immediate progress.
- No movement, combat, terrain friction, pet order, purchase, sale, payment, item movement, skill gain, quest state, discovery flag, hunger/thirst change, tarot state, door, trapdoor, coffer, or bulk-order timer test occurred.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with Kachine's service target cursor active. The next client-visible decision is to cancel the cursor or deliberately target a visible item, then decide whether to try the clothier door/trapdoor, avoid the coffers, or leave the shopfront for another cash/material lead.

## Run 314 - The Shirt Is Not a Robe

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with Kachine's service target cursor still active from the last run. The screen has not moved: my fox is beside me by continuity, Kachine is one step east, Malcolm is northeast, Elana is behind me near `Premier Gems`, and the same clothier coffer, spinning wheel, looms, basement trapdoor, side door, and `The Lord's Clothiers` sign are visible. Claire, Seward, citizens, cats, and dogs still look like safe town traffic, not a fight.

**Beat 1**

I use the active service cursor on the one equipped clothing item that is actually mine and visible in the paperdoll: the fancy shirt.

Kachine answers the way Malcolm did earlier: `That does not need my services.` The target cursor clears. No payment is taken, no item changes, no identification happens, and no new gump opens.

**Beat 2**

I stop and read the result instead of immediately clicking the trapdoor or a coffer.

That matters because the service row is now resolved as a dead end for starter clothing. Kachine is not asking for a second target, and I am back to an ordinary shopfront decision with `0` gold, `0.1` Tailoring, a blessed small tailor bulk-order deed I cannot fill, cooked alien meat, the Medical Record, starter clothes, boots, and the knife.

Mechanical friction learned:

- Kachine's service target rejects the starter fancy shirt with `That does not need my services.`
- The weaver service path is narrower than the menu text sounds: it helps specific robe/cloak, hat-repair, or tailor-identification targets, not arbitrary starter clothing.
- Resolving the target cursor does not spend gold, move an item, train Tailoring, open a vendor window, change hunger/thirst, alter quest or discovery flags, or start a crime/theft flow.
- The clothier door, basement trapdoor, and nearby coffers are still visible and untested.

Next pressure:

Mira remains at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm, with no open gump, no context menu, no vendor window, and no target cursor. The next client-visible decision is now clean: try the clothier side door or basement trapdoor, avoid or deliberately test the coffers, or leave the shopfront for a cash/material lead.

## Run 315 - The Trapdoor Really Drops

I start at `Point3D(3033,1103,0)`, facing east beside Kachine and Malcolm with no open gump, context menu, vendor window, or target cursor. The same clothier screen is still in front of me: Kachine, Malcolm, Elana behind me, my fox, town animals and citizens, two visible coffers I am still not treating as free loot, the spinning wheel, looms, the clothier side door, and the basement trapdoor. Nobody on-screen looks hostile.

**Beat 1**

I walk north along the shop-floor edge until the trapdoor is close enough to use.

I do not step into the coffer, loom, or spinning wheel. This is just a short visible reposition from the vendor spot to the west/southwest side of the trapdoor, around `Point3D(3033,1098,0)`. No menu opens, no creature reacts, and the fox keeps up.

**Beat 2**

I double-click the visible basement trapdoor.

It is not a chest. It works like a public transition: the screen jumps, a door/teleport sound plays, and I land at `Point3D(4100,3534,20)` in `the Basement`. My fox comes with me. I still have `0` gold, cooked alien meat, the Medical Record, the blessed small tailor bulk-order deed, starter clothes, boots, and the knife.

**Beat 3**

I stop and read the new screen instead of immediately clicking another door.

The basement is a public indoor workshop/social space, not a dark monster room from the first glance. I can see nearby public doors north of me, another high door to the east, lower cloth-area doorways farther southeast, looms, a spinning wheel, a stretched hide, Barclay the leather tradesman, a few passive-looking patrons, and a weird `barbaric satchel` at the south edge of view. There is no hostile inside the 18-tile screen.

Mechanical friction learned:

- The clothier basement trapdoor is a visible transition, not loot, and it only works after I move within normal use range.
- The clothier branch sends me to the Basement entry at `Point3D(4100,3534,20)`.
- The trapdoor stores a return anchor at the tile I used it from, so the nearby public doors may be the way back out, but I have not tested them yet.
- The Basement immediately exposes more crafting/social leads, especially Barclay and the visible cloth/leather props, but none of that puts money or cloth into my backpack just by arriving.
- No combat, payment, item movement, skill gain, hunger/thirst change, quest flag, discovery flag, crime state, vendor window, gump, or target cursor occurred.

Next pressure:

Mira is now at `Point3D(4100,3534,20)` in `the Basement`, facing east with the fox still following and no open UI. The next client-visible decision is to try one of the nearby public doors, approach Barclay or the crafting props, inspect the visible barbaric satchel, or walk toward the lower cloth doorway.

## Run 316 - The Basement Door Remembers The Hatch

I start at `Point3D(4100,3534,20)` in `the Basement`, facing east with no gump, context menu, vendor window, or target cursor. The fox is still with me by continuity. The room is still a public workshop/social basement from the first glance: two doors are just north of me, Barclay and a stretched hide are east on the lower floor, looms and a spinning wheel sit nearby, passive patrons are farther south, and the barbaric satchel is still only an odd thing at the south edge of sight. Nothing on screen looks hostile.

**Beat 1**

I stop long enough to decide whether this place has an obvious exit before going deeper.

The closest thing that looks like an exit is the plain public door two tiles north. Barclay is still tempting, but I do not want to strand myself in a basement before proving the way back. I do not touch the satchel, lower cloth doorway, or props.

**Beat 2**

I double-click the north public door beside the entry.

The door does not open a menu. It gives the same kind of door/teleport snap and drops me back at `Point3D(3033,1098,0)` beside The Lord's Clothiers trapdoor and spinning wheel. The fox comes with me. I am back in the Britain clothier screen with Kachine south/east, Malcolm southeast, Elana farther south/west, the coffer still close enough to be suspicious, the clothier side door still east, and ordinary town people and animals around. No hostile appears.

**Beat 3**

I stop and read the returned screen instead of immediately diving back down.

This proved the basement entry is not a one-way trap. The return uses the remembered hatch spot, then that remembered public-door return is gone. If I want to go back to Barclay and the basement props, I can use the trapdoor again, but this tile is now just the ordinary clothier shopfront with no open UI.

Mechanical friction learned:

- The nearby Basement public door is a real exit when the character has a stored public-door return from the trapdoor.
- The exit returns to the exact remembered clothier-side use point, `Point3D(3033,1098,0)`, near the original hatch.
- Using the exit clears the stored return anchor. Re-entering the basement will need the visible trapdoor again.
- The public door does not open a gump, vendor window, context menu, target cursor, quest state, discovery flag, crime flag, payment, loot, item move, skill gain, hunger/thirst change, or combat state.
- Barclay, the basement crafting props, lower basement doorway, and barbaric satchel remain untested because I chose the exit first.

Next pressure:

Mira is back at `Point3D(3033,1098,0)` beside The Lord's Clothiers trapdoor, facing east with no open UI and no target cursor. The next client-visible decision is to re-enter the basement for Barclay and the crafting props, try the clothier side door, walk back to Kachine/Malcolm, leave the shopfront for another money lead, or deliberately test a nearby coffer/theft path.

## Run 317 - The Clothier Side Door Opens

I start at `Point3D(3033,1098,0)` beside The Lord's Clothiers trapdoor, facing east with no gump, context menu, vendor window, or target cursor open. The fox is with me by continuity. The screen is still safe-looking Britain shopfront: Kachine and Malcolm are southeast, Elana is farther southwest near Premier Gems, dogs/cats/townsfolk are just town traffic, and the close temptations are the spinning wheel, looms, coffer, trapdoor, and untried clothier side door. No hostile is on the screen.

**Beat 1**

I walk east/southeast around the spinning wheel and trapdoor until the clothier side door is within reach.

This is a short shop-floor reposition, not a road run. I stop around `Point3D(3037,1099,0)`, close enough to use the door without stepping into the coffer or bouncing off the props. Malcolm is now practically beside me, Kachine is just south/west, and the fox keeps up.

**Beat 2**

I double-click the visible DarkWoodDoor.

It behaves like a normal door. No menu opens, no locked message appears, and no target cursor or vendor window appears. The door swings open with ordinary door feedback, leaving the doorway as the new visible choice.

**Beat 3**

I stop and look at the open doorway before committing through it.

The important thing is that this side door is not another hidden toll, card gate, basement jump, or theft prompt. I still have `0` gold, cooked alien meat, the Medical Record, the blessed small tailor bulk-order deed, starter clothes, boots, and the knife. Hunger and thirst still feel full, nothing attacks, and the coffer stays ignored.

Mechanical friction learned:

- The clothier side door is reachable by a short normal reposition from the trapdoor side.
- The visible DarkWoodDoor opens normally once Mira is within use range.
- Opening it does not charge gold, train a skill, change inventory, open a gump, open a context menu, create a target cursor, trigger a crime flag, move to the Basement, or change any discovery flag.
- The next unknown is not the door itself, but whether stepping through the open doorway exposes anything useful or just more shop geometry.

Next pressure:

Mira is at `Point3D(3037,1099,0)`, facing east beside the now-open clothier side door, with no open UI and no target cursor. The next client-visible decision is to step through the open doorway, back away to the trapdoor and basement route, talk to Malcolm/Kachine again, leave the shopfront for a cash/material lead, or deliberately test the nearby coffer/theft path.

## Run 318 - Through the Clothier Doorway

I start at `Point3D(3037,1099,0)`, facing east beside the opened DarkWoodDoor at The Lord's Clothiers. There is no gump, vendor window, context menu, or target cursor. Malcolm is almost beside me, Kachine is a few steps back, the fox is with me by follow continuity, and the only close objects that matter are the open side doorway, the trapdoor behind me, the loom and spinning wheel, and the coffer I am still not treating as free loot. The townsfolk, dogs, and cats on screen look passive.

**Beat 1**

I step through the already-open side doorway instead of clicking another vendor or touching the coffer.

This is just threshold movement, not a teleport. I end up at about `Point3D(3039,1098,0)`, still in the same Britain clothier screen. No menu opens, no locked message appears, and nothing asks for a target. The sign for `The Lord's Clothiers` is right above me, Malcolm and Kachine are still behind me, Premier Gems remains off to the west/southwest, and the trapdoor is still visible behind the shop props.

**Beat 2**

I stop and read the screen from the doorway before turning this into a longer town run.

The door did not reveal a hidden vendor, chest, toll, region jump, or tutorial gump. The useful new thing is positioning: I am no longer stuck deciding whether the door works. From here the world-map overlay points to nearby town leads: Premier Gems is still close but already mostly exhausted, Profuse Provisions sits west/southwest as a possible food/vendor cash test, Rolling Log Goods and the bank sit north, and the Basement route remains one trapdoor behind me.

Mechanical friction learned:

- Stepping through the opened clothier side door is ordinary local movement, not a scripted transition.
- The doorway does not open a gump, vendor window, context menu, target cursor, lock message, crime warning, combat state, payment prompt, item move, skill change, hunger/thirst change, or discovery flag.
- The live screen still shows only passive town traffic and shop objects within range; no hostile forces a survival beat.
- The next real choice is route selection, not door handling: head west/southwest toward Profuse Provisions to test whether a provisioner will buy or help with starter food, return to the Basement for Barclay/crafting props, or leave the shop cluster for another marked Britain lead.

Next pressure:

Mira is at about `Point3D(3039,1098,0)`, facing east/northeast at The Lord's Clothiers threshold, with no open UI and no target cursor. The next client-visible decision is to choose a route: west/southwest toward Profuse Provisions, back through the shop to the basement trapdoor and Barclay, north toward bank/lumber leads, or deliberately test theft friction on a coffer.

## Run 319 - Profuse Provisions Has A Door First

I start at about `Point3D(3039,1098,0)`, just outside The Lord's Clothiers side doorway. No gump, vendor window, context menu, or target cursor is open. The fox is with me by follow continuity. Malcolm and Kachine are behind me, Premier Gems is off west/southwest, the clothier trapdoor is still behind the shop props, and the nearby coffer stays ignored. The overlay points to Profuse Provisions farther west/southwest, and nothing on the screen looks hostile.

**Beat 1**

I run west/southwest through the open Britain shopfront/street gap toward the Profuse Provisions marker.

This is ordinary town movement, not a dungeon route. I pass the passive dog, adventurer traffic, Premier Gems edge, and jeweler frontage without stopping. The provisioner sign and door come into view, and Suki the provisioner is visible inside the shop. A town guard is also visible southwest of the shopfront, which makes the nearby coffers feel even less like free starter loot.

**Beat 2**

I stop at the Profuse Provisions doorway, around `Point3D(3012,1110,0)`, and double-click the visible DarkWoodDoor at `Point3D(3011,1110,0)`.

The door behaves like a normal shop door. It swings open with no locked message, toll, gump, vendor window, context menu, target cursor, item movement, crime warning, teleport, or discovery flag. This is a door, not the provisioner interaction.

**Beat 3**

I step through the open doorway and stop just inside, around `Point3D(3010,1110,0)`, facing toward Suki.

Now the useful screen is the shop interior threshold. Suki is only a few tiles north, the `Profuse Provisions` sign is right beside me, a strange `oak shelf` public-door object and at least one coffer are visible inside, Premier Gems is still visible east, and a guard and townsfolk are nearby outside. I do not context-click Suki yet. I do not open the coffer or touch the oak shelf. The next action should be the provisioner menu, not pretending the shop itself paid me.

Mechanical friction learned:

- The west/southwest route from The Lord's Clothiers to Profuse Provisions is routine Britain town movement. No hostile entered the 0-10 tile threat range, and no terrain failure or custom route gate appeared.
- Profuse Provisions is a real visible shop target: sign at `Point3D(3009,1111,0)`, Suki the provisioner at `Point3D(3009,1104,0)`, and a DarkWoodDoor at `Point3D(3011,1110,0)`.
- Opening and crossing the provisioner door does not itself open the buy/sell surface, move items, create gold, set a target cursor, or change quest/discovery state.
- The visible coffer and `oak shelf` inside Profuse Provisions are separate interactives. They are not assumed safe loot or a food source just because I reached the shop.

Next pressure:

Mira is inside the Profuse Provisions threshold at about `Point3D(3010,1110,0)`, facing north toward Suki, with no open UI and no target cursor. The next client-visible decision is to context-click Suki for a normal provisioner interaction, or deliberately inspect the visible oak shelf/coffer only if I am choosing that risk instead of the vendor path.

## Run 320 - Suki Does Not Want My Starter Pack

I start just inside Profuse Provisions at about `Point3D(3010,1110,0)`, facing north with no gump, vendor window, context menu, or target cursor open. Suki is six tiles north. Taylor, Elana, guards, citizens, dogs, and a cat are all normal town traffic around the shop. The same coffer and oak shelf are close enough to tempt me, but the guard outside still makes the coffer feel like trouble, not food.

**Beat 1**

I single-click/context-click Suki and read the menu before pressing anything.

This is the normal vendor surface, not a hidden helper. The visible choices are the usual provisioner things: buy, sell, a talk/info row, and a paid examine/unusual-item service. Since I have `0` gold, buy is a window-shopping trap. Since the goal is to find starter cash, I aim at Sell.

**Beat 2**

I choose the visible Sell row.

Suki does not open a sell list. She simply says, `You have nothing I would be interested in.` The context menu closes. No vendor window stays open, no target cursor appears, and no item leaves my backpack.

**Beat 3**

I stop and read that result instead of immediately poking the coffer.

That closes the clean provisioner-cash test. My cooked alien meat, Medical Record, and blessed tailor bulk-order deed are still in the backpack; gold is still `0`; hunger and thirst are still full; and the fox is still with me. Suki may still be useful for buying survival gear later, reading her talk page, or examining an unusual item if I ever have gold, but her Sell row is not the starter payday.

Mechanical friction learned:

- Suki's provisioner context menu exposes normal Buy/Sell rows, a Talk/info row, and an unusual-item examine service.
- Choosing Sell with the current starter backpack does not open a sell list. Suki says `You have nothing I would be interested in.`
- The provisioner buy stock includes survival goods such as bags, torches, food, waterskins, books, tents, games, and a mercantile/camping study-book chance, but `0` gold keeps that path informational for now.
- The current starter inventory remains unchanged: cooked alien meat, Medical Record, blessed small tailor bulk-order deed, starter clothes, boots, and knife.
- The visible coffer and oak shelf remain separate risks/unknowns. Suki's rejected Sell row does not make them safe, free, or part of the vendor flow.

Next pressure:

Mira is still inside Profuse Provisions at about `Point3D(3010,1110,0)`, facing north toward Suki, with no open UI and no target cursor. The clean next choices are to open Suki's Talk/info page, use the paid examine service only after finding gold, leave for another cash lead, or deliberately test the oak shelf/coffer risk.

## Run 321 - Suki Talks Survival, Not Cash

I start inside Profuse Provisions at about `Point3D(3010,1110,0)`, facing north. Suki is still six tiles north, the fox is beside me by follow continuity, and there is no open gump, vendor window, context menu, or target cursor. The coffer and oak shelf are still close enough to click, but the guard outside and Suki's shop floor keep those in the "deliberate risk" bucket. Taylor, Elana, citizens, dogs, and a cat remain passive town context, not threats.

**Beat 1**

I context-click Suki again and read the menu instead of going straight for the coffer.

This is the same normal provisioner surface I saw before. Buy and Sell are there, the paid unusual-item examine row is there, and the Talk/info row is still visible. Since Sell already failed and Buy needs gold, I choose the row that costs nothing and actually teaches something.

**Beat 2**

I choose Suki's visible Talk/info row.

A `SpeechGump` opens titled `The Right Survival Gear`. Suki does not hand me food, coins, or gear. No target cursor appears. The shop floor stays the same behind the paper, and the close button is the only obvious control.

**Beat 3**

I read the gump instead of pressing through it.

The useful parts are practical: light sources burn out; a `10-foot pole` can passively check many invisible traps if I can buy or find one; bedrolls, campfires, small tents, and better camping tents are survival tools; strong Camping can turn tents into longer-term safety, food, water, and bank organization; merchant crates are a home/crafting money route; and Suki can sometimes appraise odd decorative/value items and name a buyer. That is information, not a payout.

Mechanical friction learned:

- Suki's Talk/info row is accessible from the visible context menu and opens an information gump titled `The Right Survival Gear`.
- The gump is closable, draggable, not resizable, and has a scrollable body plus a close button.
- Reading it does not change gold, inventory, skills, hunger, thirst, combat state, criminal state, discovery flags, target cursor, vendor window, or location.
- The Talk text reinforces that the paid examine service is for unusual decorative/value items, and that survival tools still require finding or buying gear.
- The coffer and oak shelf remain separate unresolved clicks. The survival talk does not make them safe, free, or part of Suki's vendor flow.

Next pressure:

Mira is still at about `Point3D(3010,1110,0)`, facing north inside Profuse Provisions, with Suki's `The Right Survival Gear` gump open and read. The next client-visible decision is to close the gump and choose a lead: search for a way to get a 10-foot pole/tent money, leave for another marked town service, re-enter the Basement for Barclay/crafting clues, or deliberately test the oak shelf/coffer risk.

## Run 322 - The Empty Shelf Is A Door

I start inside Profuse Provisions at about `Point3D(3010,1110,0)`, facing north with Suki's `The Right Survival Gear` gump still open and already read. The screen behind it is still a safe-looking Britain shop: Suki is a few steps north, the fox is beside me by follow continuity, Taylor and Elana are nearby town service people, Paulin and Gabrielle are guards, citizens and pets are passive, and no hostile is on the current screen. The close coffer still looks like theft bait, but the `oak shelf` is a visible oddity and Suki's talk made it clear the vendor path did not hand me cash.

**Beat 1**

I close Suki's survival gump.

The paper disappears. No vendor list opens, no target cursor appears, and no food, gold, light, pole, bedroll, tent, skill, hunger, thirst, crime flag, or discovery flag changes. I am just back in the shop.

**Beat 2**

I step a couple of tiles west/north inside the shop to get within use range of the `oak shelf`, stopping around `Point3D(3008,1109,0)`.

This is short shop-floor movement around the coffer, not a pathing problem. I do not open the coffer, do not talk to Suki again, and do not touch the door outside. The shelf is now close enough to use.

**Beat 3**

I double-click the visible `oak shelf`.

It is not a food shelf or a container. It acts like a public-door transition: the client plays the door/teleport flow, brings the fox with me, and drops me into the Thieves Guild at about `Point3D(3425,3187,17)`. I stop immediately because the screen changed from a provisioner shop to a hidden guild interior.

The new screen is dense but not immediately violent. I can see an exit `oak shelf` almost beside me, a `Bank Vault`, practice lockboxes from easy through hard, a training dummy, a pickpocket dip, strong wood doors, a secret-looking dungeon door, a `Stealing the Past` board, games on tables, Iain the thief, Lore the thief guildmaster, Zarek the waiter, tavern patrons, Stefano, Ragnar the alchemist, and a greater-strength cauldron. No gump or target cursor is open.

Mechanical friction learned:

- The Profuse Provisions `oak shelf` is a visible `PublicDoor`, not loot, storage, or ordinary decoration.
- From the original shop threshold tile it is too far to use cleanly, so the normal player has to step closer first.
- Double-clicking it stores a public-door return anchor at the use tile and teleports to the Thieves Guild destination at `Point3D(3425,3187,17)`.
- The shelf route does not require gold, guild membership, a tarot card, a discovery flag, a target cursor, or a private helper call.
- The transition itself does not award money, open a vendor window, open a gump, train a skill, change inventory, set a quest, flag a crime, alter hunger/thirst, or spend resources.
- The new guild screen exposes theft-training surfaces and NPCs, but seeing them is not the same as joining the guild, starting a job, opening the bank vault, or stealing from anything.

Next pressure:

Mira is now at about `Point3D(3425,3187,17)` in the Thieves Guild, facing west with no open UI and no target cursor. The next client-visible decision is to read the new room before touching anything: talk to Lore or Iain, read `Stealing the Past`, test the Bank Vault, inspect the practice lockboxes or pickpocket dip, open the nearby doors, or use the exit shelf to return to Profuse Provisions.

## Run 323 - Iain Opens A Thief Lesson

I start at about `Point3D(3425,3187,17)` inside the Thieves Guild, still facing the new room with no gump, vendor window, context menu, or target cursor open. The fox is beside me. Iain is only a few steps southeast, Lore is farther west, and the screen is crowded with thief tools: the exit `oak shelf`, a `Bank Vault`, practice lockboxes, a training dummy, a pickpocket dip, strong wood doors, a secret-looking dungeon door, the `Stealing the Past` board, and tavern/game clutter. None of the visible people are acting hostile.

**Beat 1**

I stop and read the room before moving deeper.

The guild is dense, but it is not an ambush screen. Iain and Lore look like the cleanest social leads. The board and training props look useful, but the board is across the room and the lockboxes/dip are training surfaces, not food or cash. The Bank Vault is right beside me, but prior bank checks were empty, so I do not spend the first guild action there.

**Beat 2**

I context-click Iain and read the menu.

He is a normal thief vendor, not a hidden teleport. The visible choices are the standard buy/sell rows, a talk/info row for `The Art Of Thievery`, and a paid service to unlock a box. With `0` gold, the paid unlock row is not the first thing to press. Buy is window shopping. Sell is possible later, but I still have only starter items and one blessed tailoring deed. The free talk row is the safest next press.

**Beat 3**

I choose Iain's visible talk/info row.

A `SpeechGump` opens titled `The Art Of Thievery`. It is closable and draggable, with a scrollable body and a close button. I stop there because the new paper is the screen now; reading it comes before touching Lore, the board, the vault, the lockboxes, or the exit shelf.

Mechanical friction learned:

- Iain is a thief vendor with normal Buy/Sell rows plus a free talk/info row and a paid box-unlock service.
- Choosing the talk row opens `The Art Of Thievery`; it does not open a buy list, sell list, target cursor, or job note.
- No gold, inventory, hunger, thirst, skill, guild membership, criminal state, combat state, discovery flag, location, or follower state changed.
- The paid unlock-box route remains untested and would require choosing that visible service before targeting anything; I did not spend or target.
- Lore, `Stealing the Past`, the Bank Vault, the practice lockboxes, the pickpocket dip, doors, games, and the exit shelf remain visible but untouched.

Next pressure:

Mira is still at about `Point3D(3425,3187,17)` in the Thieves Guild with Iain's `The Art Of Thievery` gump open and unread. The next client-visible decision is to read that visible gump, then decide whether to close it and test Lore, the free/paid thief services, the board, the vault, the training props, or the exit shelf.

## Run 324 - Iain's Lesson Points At Thief Work

I start in the same spot inside the Thieves Guild, at about `Point3D(3425,3187,17)`, with the fox beside me and Iain's `The Art Of Thievery` paper still covering the screen. Nothing is attacking me. The room behind the paper still has Lore, Iain, the Bank Vault, lockboxes, the pickpocket dip, the `Stealing the Past` board, doors, tavern people, and the exit shelf, but the open gump is the thing I have to deal with first.

**Beat 1**

I read Iain's lesson instead of pressing blindly past it.

It is not a payout, a job contract, or a vendor list. It is thief advice. Iain says a thief can be hired to open stubborn locked boxes, that dungeon stealing has its own profit route, that stealth training starts after getting hiding to `30.0`, and that traps and trap-removal matter. The town route is more dangerous than a shop counter: snoop merchants or coffers to see whether gold is present, then steal, with guards as the obvious punishment if I am caught. The most actionable line is that the thief guildmaster can give shady jobs even to non-members.

**Beat 2**

I close the `The Art Of Thievery` gump.

The paper goes away. I do not get gold, a job note, a target cursor, a buy list, a sell list, a skill gain, guild membership, a criminal flag, or a discovery flag. Iain's talk was a lesson only. With the screen clear again, Lore is the strongest social lead because the lesson explicitly pointed at the guildmaster, but I stop before moving or choosing a new context-menu row.

Mechanical friction learned:

- Iain's free talk row is informational only; reading and closing it does not mutate the character.
- The thief-cash path is not "take from shop shelf." The visible advice points toward snooping, stealing, traps, guards, dungeon theft, and guildmaster jobs.
- Iain's paid unlock-box service remains separate from the talk lesson and still untested.
- Lore, `Stealing the Past`, the Bank Vault, practice lockboxes, pickpocket dip, doors, games, cauldron, and the exit shelf remain visible but untouched this run.

Next pressure:

Mira is still around `Point3D(3425,3187,17)` in the Thieves Guild, facing west, with no open gump, no context menu, no vendor window, and no target cursor. The next client-visible decision is to pick a thief lead: move close enough to Lore and read her context menu, inspect the `Stealing the Past` board, test the Bank Vault, try the practice props, return to Iain's paid service later, or leave through the oak shelf.

## Run 325 - Lore Hands Me A Secret Note

I start beside the Thieves Guild arrival shelf at about `Point3D(3425,3187,17)`, facing west with the fox still following and no paper, vendor list, context menu, or target cursor open. Lore is visible across the lockboxes, Iain is still east of her, the Bank Vault is beside me, and the room has practice boxes, a pickpocket dip, doors, tavern people, games, and the `Stealing the Past` board. None of the visible people act hostile, so I follow Iain's lead about guildmaster jobs.

**Beat 1**

I walk west and a little south across the open guild floor toward Lore, stopping next to her around `Point3D(3419,3189,0)`.

This is ordinary interior movement. I pass the practice lockboxes and training clutter without using them. The move reveals more of the west and south side of the guild: Saxon the provisioner, Ciro the drunken pirate vendor, Goldie, extra strong wood doors, a trash barrel, a checkerboard, dice, a chessboard, a blackjack table, and a barbaric satchel. They look like room context and vendor/game clutter, not immediate threats.

**Beat 2**

I context-click Lore and read her menu before choosing anything.

Lore exposes the normal vendor rows plus thief-guild controls: Buy, Sell, a visible job row, and Join. Buy is likely guildmaster stock and I still have `0` gold. Join is not free; guildmasters quote a joining price when asked, and this alien-start character would pay the space-origin markup. The job row is the one Iain just told me about, so I choose that visible row.

**Beat 3**

Lore says, "Here is something I think you can handle." A `secret note` appears in my backpack.

No target cursor opens, no vendor window opens, and Lore does not make me pay or join first. I stop there because the new note is the next visible object to read, and the exact job text is the point of opening it rather than guessing from the room.

Mechanical friction learned:

- Lore's job row is visible to an alive, unblessed player and is not gated by current Thieves Guild membership.
- Choosing the job row creates a `secret note` in the backpack and plays the handoff/sound flow.
- If I already had a thief job note, Lore would give me a copy instead of rolling a second job.
- The job note itself is still unread; opening it is the next visible step.
- Lore's Join row remains separate from jobs. It would ask for a guild fee, and this character still has `0` gold.
- The `Stealing the Past` board, Bank Vault, lockboxes, pickpocket dip, doors, games, trash barrel, satchel, and exit shelf remain visible but untouched.

Next pressure:

Mira is now around `Point3D(3419,3189,0)`, next to Lore in the Thieves Guild and facing toward the guildmaster interaction. No gump, vendor window, context menu, or target cursor is open. The backpack now contains a new unread `secret note` coded for Mira. The next client-visible decision is to open and read that note before choosing a route, joining the guild, reading the board, or testing the training props.

## Run 326 - The Secret Note Opens, But I Need The Actual Line

I start beside Lore at about `Point3D(3419,3189,0)` in the Thieves Guild. The fox is close by, Lore is one tile west, Iain is east, and the same guild room clutter is still on screen: practice lockboxes, a pickpocket dip, a training dummy, the Bank Vault, strong wood doors, a secret-looking door, tavern people, games, Saxon, Ciro, and `Stealing the Past`. Nobody is threatening me, and no gump, context menu, vendor window, or target cursor is open.

**Beat 1**

I open my backpack and double-click the new `secret note` instead of touching the vault, doors, board, or training props.

The note is in my backpack and coded for me, so it opens a paper-style note gump. No target cursor appears, no item moves, no gold appears, and I do not get dragged into a route or theft action. This is the job instructions surface, not the job completion step.

**Beat 2**

I read the note window and stop on it.

The safe part of the paper is clear: keep the note while working the job; a dungeon theft means finding the pedestal bag or box and using it; a town merchant theft means finding the right coffer and using Stealing on it; getting caught can make me criminal and bring guards down on me; after I have the item, the note tells me where to deliver it; and if I lose the note, the thief guildmaster can give me a copy. The top assignment line is the important random part: who wants the item, what to steal, where it is, where to drop it, and how much gold pays out. That exact line is not something I can invent from the room, so I do not choose a coffer, dungeon, route, or delivery container yet.

Mechanical friction learned:

- Double-clicking the owned `secret note` opens a closable, draggable, scrollable note gump when the note is in the backpack and no thief-job cooldown blocks reading.
- The note itself teaches the two theft surfaces: dungeon pedestal bag/box versus town merchant coffer.
- The theft action is still separate. Opening the note does not use Stealing, open a target cursor, award the job item, pay gold, join the guild, change inventory, change skills, flag criminal, or move Mira.
- The exact randomized assignment line is required before normal route planning. The continuity state has the note item but not the saved `NoteStory`, so I stop rather than guessing a target.

Next pressure:

Mira is still around `Point3D(3419,3189,0)`, facing Lore in the Thieves Guild, with the secret note gump open and read for its generic rules. The next client-visible decision is to capture the exact assignment line from the visible note before closing it or choosing a route toward a coffer, dungeon pedestal, delivery stump, hay crate, board, vault, practice prop, or exit shelf.

## Run 327 - I Cannot Steal From A Missing Line

I start exactly where I stopped: beside Lore at about `Point3D(3419,3189,0)` in the Thieves Guild, facing west, with the fox on follow and the secret-note paper still open. The room behind the paper is unchanged. Lore is one tile west, Iain is east, the practice lockboxes and pickpocket dip are close, the Bank Vault and exit shelf are east, and the `Stealing the Past` board and tavern clutter sit farther south. None of those are more urgent than the open job note.

**Beat 1**

I try to read the top assignment line instead of closing the paper or guessing a target.

That is the only normal next action, but it does not give me a usable route in this saved continuity. The note window is still open and the fixed instructions are already known, but the actual first sentence that should name the patron, item, theft place, delivery container, delivery town, and payout was not captured anywhere in the playable state I have. I do not pretend the nearby coffer, board, vault, or secret door is the job. I do not start a thief route from the code's possible random branches.

Mechanical friction learned:

- The note gump is still the active screen object. Closing it, touching a coffer, entering a dungeon door, reading the board, or asking Lore to join would all be new choices made without the visible job line.
- The missing assignment line is not derivable from the room. `SetupNote` rolls many random components, saves them into `NoteStory`, and then the gump displays that saved string before the fixed instructions.
- The live CCWM artifact has the static world snapshot and the current YAML continuity, but it does not contain Mira's generated backpack note story. The normal player could read it from the client; this automation cannot recover it after it was omitted from state.
- No movement, target cursor, vendor window, combat state, theft roll, payout, inventory change, skill change, criminal flag, guild membership, discovery flag, hunger/thirst change, or follower change happens.

Next pressure:

Mira is still around `Point3D(3419,3189,0)`, facing Lore in the Thieves Guild, with the secret note gump open. The next client-visible decision is unchanged but sharper: manually capture the exact first assignment sentence from the visible note, or accept that this synthetic thief-job branch cannot be routed honestly from the saved state.

## Run 328 - The Thief Board Wants A Real Guild Thief

I start in the same tight spot beside Lore at about `Point3D(3419,3189,0)`, facing west, with the fox still on follow and the secret-note paper still open. The paper has already taught the generic theft rules, but the actual job sentence is still missing from the saved client continuity. I do not make up a coffer, dungeon, patron, delivery box, or payout from code possibilities.

**Beat 1**

I close the secret-note paper and abandon this synthetic job branch for now.

The note stays in my backpack, but the screen is clear again: no gump, no vendor window, no context menu, and no target cursor. Lore is still one tile west. Iain is east. The lockboxes, pickpocket dip, Bank Vault, exit shelf, doors, tavern people, and `Stealing the Past` board are still visible room leads. Nobody is close and dangerous.

**Beat 2**

I try the visible `Stealing the Past` board instead of pretending the note told me where to steal.

The board does not open for me. The client gets the plain message that it seems to be written in thieves' cant. That is a real gate, not a navigation hint: I am reading a thief board without actually being in the Thieves Guild.

**Beat 3**

I turn back to Lore, context-click her again, and choose the visible `Join` row.

Lore does not enroll me or open a payment gump. She quotes the joining price. For this alien-start character, the normal `2,000` gold guild price is multiplied by the space-origin markup, so the visible demand is `8,000` gold. I have `0` gold. Nothing is dragged to her, no ring appears, and the board remains unreadable.

Mechanical friction learned:

- Closing the note is just closing the paper. It does not cancel the item, pick a route, start a theft target, change skills, pay gold, flag criminal, or move me.
- `Stealing the Past` is gated by actual `NpcGuild.ThievesGuild` membership. A non-member gets the thieves' cant message before any useful board text appears.
- Lore's `Join` row is visible and safe to ask about, but it only quotes the joining fee. The actual join requires dropping the exact fee on the guildmaster.
- Mira's alien-start skill state makes the first guild fee `8,000` gold instead of `2,000`, and the current backpack/bank situation still has `0` gold.
- The secret note remains a dead-end lead until the exact randomized assignment sentence is manually captured.

Next pressure:

Mira is still around `Point3D(3419,3189,0)` in the Thieves Guild, facing Lore, with no open UI and no target cursor. The honest thief path now has two visible blockers: the job note lacks its captured assignment line, and the guild board needs paid Thieves Guild membership. The next client-visible decision is to either find a starter-money route elsewhere, test another visible guild object such as the Bank Vault or practice props, or leave through the oak shelf.

## Run 329 - The Guild Vault Is Still My Empty Bank

I start beside Lore at about `Point3D(3419,3189,0)`, facing west in the Thieves Guild with the fox still on follow and no gump, vendor window, context menu, or target cursor open. The visible room has the same passive people and thief props: Lore one tile west, Iain east, a line of practice lockboxes, the pickpocket dip, the training dummy, the Bank Vault, the exit shelf, strong wood doors, the secret-looking door, tavern people, games, and Saxon at the west edge. Nothing on screen is close and dangerous.

**Beat 1**

I walk east along the open guild floor toward the `Bank Vault`, stopping around `Point3D(3421,3189,0)`.

This is short interior movement, not a maze or terrain problem. I do not open the lockboxes, train on the dummy, touch the dip, talk to Iain, or use the exit shelf. From here the vault is within use range.

**Beat 2**

I double-click the visible `Bank Vault`.

It opens my personal bank box, not a guild treasure chest. The box is still empty. No coins appear, no storage item is waiting there, and no new bank-balance message or payment prompt appears.

**Beat 3**

I close the empty bank box.

The screen returns to the guild room with no open UI and no target cursor. The vault did not solve the `8,000` gold join fee, did not mutate inventory, did not train a skill, did not flag a crime, did not change the note job, and did not unlock `Stealing the Past`.

Mechanical friction learned:

- The guild `Bank Vault` uses the same personal bank access as the Britain bank vault. It is not loot, a shared guild fund, or a hidden starter-money chest.
- From Lore's tile the vault is too far to use cleanly, so a normal player has to step east into the four-tile use range before double-clicking it.
- Opening the vault only exposes Mira's bank box. Because the bank was already empty and nothing has been deposited since, the visible result is still an empty container.
- The thief route remains blocked by three separate facts: the job note's exact assignment line is absent from saved continuity, the board requires Thieves Guild membership, and joining still needs `8,000` gold.

Next pressure:

Mira is now around `Point3D(3421,3189,0)` in the Thieves Guild, facing east near the lockboxes and training dummy, with no open UI and no target cursor. The next client-visible decision is to test a visible training prop such as the pickpocket dip or practice lockboxes, ask Iain or Saxon for a possible money/service lead, leave through the oak shelf, or finally abandon the guild route until real starter money appears.

## Run 330 - The Easy Lockbox Is Not A Free Lesson

I start near the Thieves Guild Bank Vault at about `Point3D(3421,3189,0)`, facing east with the fox still following and no gump, vendor list, context menu, container window, or target cursor open. The same passive guild room is on screen: Lore west of me, Iain east, Saxon at the west edge, the tavern crowd south, the exit shelf and bank vault east, and the thief props in the middle. Nothing looks close and dangerous.

**Beat 1**

I step west and a little north along the open guild floor to the easiest practice lockbox, stopping around `Point3D(3417,3188,0)`.

This is not a route problem. I am close enough to the row of lockboxes now, with the easy box just northwest of me and the normal, difficult, challenging, and hard boxes lined up beside it.

**Beat 2**

I double-click the easy practice lockbox.

The box does not open and does not start training by itself. It gives the locked-container message: it appears to be locked. No target cursor appears, no lockpicking attempt starts, no skill message appears, no loot window opens, and nothing moves into my backpack. Looking back at my pack matters now: I still do not have a lockpick, so this prop is not usable just by clicking it.

Mechanical friction learned:

- The practice lockboxes are real locked containers, not gumps or free skill buttons.
- The easy box starts locked, immovable, and marked as the lowest lockpicking trainer in the row.
- Opening a locked container as a player only reports that it appears to be locked.
- A separate lockpick item is what would ask what I want to pick and create the target cursor. Mira does not have one.
- The pickpocket dip remains the only nearby thief-training prop that can be used without a tool, but that route is a live Stealing skill check and not a money source by itself.

Next pressure:

Mira is now around `Point3D(3417,3188,0)` beside the easy practice lockbox, facing toward it, with no open UI and no target cursor. The next client-visible decision is to find or buy lockpicks, ask Iain whether his service or stock solves the lockbox problem, try the pickpocket dip as pure Stealing practice, or leave through the oak shelf because the guild still has no starter-money answer.

## Run 331 - Iain Wants A Thousand Before He Touches The Box

I start beside the easy practice lockbox at about `Point3D(3417,3188,0)`, facing the locked trainer with no gump, vendor window, context menu, or target cursor open. Lore is close behind me, Iain is east, the Bank Vault and exit shelf sit near him, the lockbox row and pickpocket dip are still in reach, and the tavern people and games are farther south. Nobody on screen looks hostile, so I follow the only visible lead that might solve a lockbox without owning a lockpick.

**Beat 1**

I walk east and a little south along the open guild floor to Iain, stopping around `Point3D(3427,3189,0)`.

This is just a short interior move. I pass the lockboxes, vault, and shelf without clicking them. Saxon falls off the west edge of the screen, Iain becomes the center of attention, and nothing attacks, blocks me, opens a gump, changes region, or drains stamina.

**Beat 2**

I context-click Iain and choose the visible paid box-unlock service row.

Iain does not hand me a lockpick, open a buy list, or unlock anything immediately. He says that if I want him to unlock a box, it will cost `1000` gold, then the client gives me a target cursor. I stop there because the cursor is now the active screen state. Targeting the easy lockbox would be a new choice, and I still have `0` gold.

Mechanical friction learned:

- Iain's service is not free training and not a hidden lockpick source. The visible service row creates a target cursor after quoting a `1000` gold unlock fee.
- The service has not spent money or changed the lockbox yet. No box was targeted this run.
- The vendor's possible `Buy` list is a separate row. I did not open it, and I do not assume whether a lockpick is currently in Iain's randomized stock.
- The easy practice lockbox remains useful only if I either get a lockpick or deliberately target it under Iain's paid service to observe the payment failure.
- No inventory, bank contents, gold, skill, guild membership, quest state, crime flag, discovery flag, hunger/thirst, combat state, follower order, or route transition changed.

Next pressure:

Mira is now around `Point3D(3427,3189,0)`, beside Iain in the Thieves Guild, with his box-unlock target cursor active and the quote still fresh: `1000` gold to unlock a box. The next client-visible decision is to target the easy practice lockbox anyway and see the payment gate, or cancel the cursor before doing anything else.

## Run 332 - Iain Still Needs Gold

I start beside Iain at about `Point3D(3427,3189,0)` in the Thieves Guild, with no gump or vendor list open but with Iain's paid box-unlock target cursor still active. The easy practice lockbox is visible west of me in the lockbox row. Lore, the Bank Vault, the exit shelf, the pickpocket dip, the training dummy, doors, tavern people, games, Stefano, Ragnar, Chanah, Ciro, and the cauldron are all room context. Nobody on screen looks hostile, and the cursor is the thing I must resolve before doing anything else.

**Beat 1**

I target the visible easy practice lockbox under Iain's unlock-service cursor.

That does not unlock the box. Iain repeats the price in a sharper way: it would cost `1000` gold to have that unlocked. The client also tells me I do not have enough gold. The cursor clears after the target, and the easy lockbox stays a locked trainer. No lockpick appears, no vendor window opens, no container opens, and no skill check starts.

Mechanical friction learned:

- Iain's paid service accepts the practice lockbox as the right kind of target, but payment is checked from my backpack before anything changes.
- With `0` gold, the service fails cleanly: I get the `1000` gold cost line and the "not enough gold" message.
- The failed service does not spend money, change inventory, unlock the box, remove traps, change skills, change guild membership, flag a crime, change discovery flags, move me, open a gump, or open a vendor list.
- The easy lockbox remains blocked unless I find a lockpick or real money.

Next pressure:

Mira is still around `Point3D(3427,3189,0)`, beside Iain in the Thieves Guild, with the target cursor gone and no open UI. The immediate paid-service question is answered: Iain can target the box, but Mira cannot pay. The next client-visible decision is to inspect Iain's `Buy` stock for lockpicks despite having `0` gold, try the pickpocket dip as pure Stealing practice, leave through the oak shelf, or abandon this guild route until starter money appears.

## Run 333 - Iain's Stock Is Still A Money Problem

I start beside Iain at about `Point3D(3427,3189,0)` in the Thieves Guild. The fox is still on follow, Iain is one tile east, Lore is west, the Bank Vault and oak shelf are behind me, and the lockbox row, pickpocket dip, training dummy, doors, tavern tables, games, Stefano, Ragnar, Chanah, Ciro, Goldie, and the strength-liquid cauldron are all still room context. Nobody on screen is close and dangerous, and there is no target cursor left from Iain's failed unlock service.

**Beat 1**

I context-click Iain again, this time looking for the normal shop row instead of his paid box-unlock service.

The menu is the same kind of vendor menu I saw before: `Buy`, `Sell`, the thievery talk row, and the paid unlock service. I do not touch the service row again because the `1000` gold gate has already failed against my empty pack.

**Beat 2**

I choose `Buy`.

Iain does not hand me a lockpick directly. A vendor buy window opens and he gives the standard shop greeting to look around. This is a shop list now, not a target cursor and not a training gump.

**Beat 3**

I stop on the open buy window instead of clicking a row.

The important thing on the screen is that everything in Iain's stock still has a price and I still have `0` gold. His thief stock can include cheap training books and thief tools, and a lockpick is only useful if this current restock actually shows one and I can pay for it. I do not buy, drag, steal, close the window, or assume a hidden free lockpick just because the practice boxes are nearby.

Mechanical friction learned:

- Iain's `Buy` row is a separate normal vendor path from the paid box-unlock service.
- Opening `Buy` creates a vendor buy window and the usual "have a look around" vendor greeting; it does not create a target cursor, unlock a box, train Lockpicking, or move an item into my backpack.
- The thief stock path can offer `Learn Stealing Book` and `Learn Traps` for `5` gold, and a `Lockpick` row is a randomized `12` gold stock entry rather than guaranteed visible loot.
- With `0` gold, even a visible lockpick row would be another money gate, not a solved lockbox route.
- No inventory, bank contents, skill value, guild membership, quest state, crime flag, discovery flag, hunger/thirst, combat state, follower order, or location changed.

Next pressure:

Mira is still around `Point3D(3427,3189,0)`, beside Iain in the Thieves Guild, with Iain's buy window open and no target cursor. The next client-visible decision is to read or scroll the current shop list for a lockpick row, then close it because Mira has `0` gold, or back out and try a non-purchase route such as the pickpocket dip, the oak shelf exit, Saxon, Ragnar, Chanah, or the secret-looking door.

## Run 334 - I Back Out Of Iain's Prices

I start beside Iain at about `Point3D(3427,3189,0)` in the Thieves Guild with his buy window still open. The fox is still following, Iain is one tile east, Lore is west, the Bank Vault and oak shelf are behind me, and the lockbox row plus the pickpocket dip are still visible. Nobody on screen looks hostile or close enough to force a fight, and the open shop window is the thing I have to resolve before moving on.

**Beat 1**

I read and scroll Iain's buy list instead of selecting anything.

The list behaves like a normal shop list: item rows, prices, amounts, and the buy/cancel controls. Nothing on it is a free "take a lockpick" handoff. The cheap thief-learning rows are still purchases, and a lockpick row, if this restock shows one, is still a priced row. With `0` gold, clicking a row would only turn the missing-money problem into a failed purchase, so I leave the stock alone.

**Beat 2**

I close the vendor buy window.

No item moves, no gold is spent, no target cursor appears, no lockbox unlocks, and no skill check starts. Iain's shop is now just a remembered price gate, not an open UI blocking the screen.

**Beat 3**

I walk west along the open guild floor, passing the bank vault and lockbox row, and stop beside the pickpocket dip around `Point3D(3417,3190,0)`.

This is short interior movement, not a maze or terrain problem. Lore is now close to my northeast, Iain is still visible back east, Saxon is visible farther west, and the tavern crowd is south. The pickpocket dip is within arm's reach, but I do not double-click it yet because that would start the actual Stealing practice roll.

Mechanical friction learned:

- Iain's open buy list does not solve the missing-lockpick problem for a broke character.
- The buy window and the paid box-unlock service are separate routes, but both still require money.
- Closing the buy window leaves Mira free to use visible room objects again; it does not change inventory, bank contents, skills, guild membership, crime state, discovery flags, hunger/thirst, combat state, follower order, or the active quest note.
- The pickpocket dip is the next no-money thief prop in reach. It is training pressure, not a starter-gold route, and the skill attempt has not been made yet.

Next pressure:

Mira is now around `Point3D(3417,3190,0)`, facing the pickpocket dip in the Thieves Guild, with no open UI and no target cursor. The next client-visible decision is to double-click the dip for a Stealing practice attempt, or abandon the guild's no-money training loop and try Saxon, the oak shelf exit, the secret-looking door, or another visible vendor lead.

## Run 335 - The Dip Finally Gets Used

I start beside the pickpocket dip at about `Point3D(3417,3190,0)` in the Thieves Guild. The fox is still on follow, Lore is one step northeast, Iain is still visible east, Saxon is west, and the tavern crowd is south. The lockbox row, bank vault, exit shelf, doors, games, a trash barrel, and a barbaric satchel are all room clutter. Nobody on screen looks hostile, and there is no gump, vendor list, context menu, or target cursor in the way.

**Beat 1**

I double-click the pickpocket dip.

The first try is clumsy. I turn toward the dip, hear the use sound, and bump it hard enough that it starts swinging. The client tells me I carelessly bump the dip and start it swinging. This is not loot and not money. It is just a Stealing practice roll.

**Beat 2**

I wait for the dip to stop swinging.

There is no reason to click through the cooldown. The dip visibly settles after the short swing, and no hostile, gump, target cursor, inventory move, vendor window, or route change appears while I wait.

**Beat 3**

I double-click the dip again.

This time I avoid disturbing it while searching. The second message is the success line, not a payout line. The only useful change is training: Stealing finally moves off zero, to about `0.5` after the two checks. I am still broke, still not in the Thieves Guild, still holding the unresolved secret note, and still one tile from the same dip.

Mechanical friction learned:

- The pickpocket dip is usable from one tile away and does not need a lockpick, gold, guild membership, a target cursor, or a gump.
- A failed dip attempt starts a visible short swing state. While it swings, the next honest action is to wait instead of pretending another use went through.
- The dip trains Stealing up to the shard's pickpocket-dip cap, but it does not create loot, pay starter gold, unlock lockboxes, advance the thief note, change guild membership, flag a crime, move me, or reveal a route.
- The nearby lockbox and Iain routes remain money/tool blocked. The barbaric satchel and Saxon are visible west-side leads, but I did not walk to or inspect either one this run.

Next pressure:

Mira is still around `Point3D(3417,3190,0)`, facing the pickpocket dip in the Thieves Guild, with no open UI and no target cursor. The immediate choice is whether to keep grinding the dip for Stealing, inspect the visible Saxon/satchel/trash-barrel side of the room, leave by the oak shelf, or test the secret-looking door.

## Run 336 - The Satchel Was Not Mine

I start beside the pickpocket dip at about `Point3D(3417,3190,0)` in the Thieves Guild. The fox is still following. Lore is close northeast, Saxon is visible west by the trash barrel, the odd barbaric satchel is southwest on the furniture, and the tavern room opens farther south. Iain and the exit shelf are behind me to the east. Nothing on screen is hostile, and no gump, context menu, vendor list, or target cursor is open.

**Beat 1**

I walk west and south across the open guild floor toward the west-side clutter, stopping around `Point3D(3404,3201,0)` near Saxon, the trash barrel, and the barbaric satchel.

This is a short interior route. I do not open doors, use the secret dungeon door, talk to Saxon, click the trash barrel, or go back to the pickpocket dip. The new thing at arm's reach is the satchel.

**Beat 2**

I double-click the barbaric satchel.

It does not open like a backpack. It does not give me a container, a gump, a starter kit, money, a tool, or a target cursor. The satchel just disappears from the floor. That is strange enough that I stop instead of clicking the trash barrel or Saxon immediately afterward.

Mechanical friction learned:

- The west-side barbaric satchel is not a free container or loot lead for Mira.
- The satchel behavior is owner-gated. If the player using it is not the recorded owner and the satchel has weight, the double-click path deletes it before the normal owner gump can open.
- The actual barbaric satchel gump is for an owned/playstyle satchel that changes equipment appearance; this loose world satchel did not become mine and did not mutate any equipped item.
- Saxon, the trash barrel, the nearby secret-looking doors, and the forged census records farther south remain visible leads, but I did not touch them after the satchel vanished.

Next pressure:

Mira is now around `Point3D(3404,3201,0)` on the west/south side of the Thieves Guild, facing the spot where the barbaric satchel vanished, with no open UI and no target cursor. The next client-visible decision is to inspect Saxon, the trash barrel, or the nearby secret doors, or return to safer known leads such as the pickpocket dip.

## Run 337 - Saxon Talks Gear, Not Money

I start on the west side of the Thieves Guild around `Point3D(3404,3201,0)`. Saxon the provisioner is just north of me by the trash barrel, the fox is still on follow, and the vanished satchel spot is behind my shoulder. Lore, the lockboxes, the pickpocket dip, the tavern crowd, and the secret-looking doors are all still on the same screen. Nothing is hostile, and there is no target cursor or open window blocking me.

**Beat 1**

I context-click Saxon instead of poking the trash barrel.

His menu is ordinary vendor pressure: `Buy`, `Sell`, a repair/service-looking row, and a talk/info row for survival gear. I do not choose `Buy` because I am still broke, and I do not assume the trash barrel is loot just because it looks like a container.

**Beat 2**

I choose Saxon's talk/info row.

A gump opens titled `The Right Survival Gear`. Saxon is not handing over gold, lockpicks, a starter pack, or a thief-note route. This is the same kind of provisioner lesson I saw from Suki, just from the guild-side vendor.

**Beat 3**

I read the visible gump text and stop with it open.

The lesson is practical but not a payout: lights burn out, 10-foot poles passively check traps, bedrolls and campfires are rest tools, tents are emergency outdoor shelter, camping tents scale with Camping and can organize bank items underground, merchant crates are for crafters selling from a home, and strange decorative items may need the right merchant for appraisal. That keeps Saxon useful as a gear/advice lead, but it does not solve the immediate `0` gold problem.

Mechanical friction learned:

- Saxon's closest free interaction is an information gump, not starter money.
- Provisioner `Buy`, `Sell`, and repair/service rows remain visible commerce/service routes, so `0` gold still matters before trying them.
- The survival-gear gump is closable, draggable, scrollable text with no choice buttons beyond closing.
- No inventory, bank contents, skills, guild membership, crime flag, discovery flag, hunger/thirst, combat state, follower order, or location changed.

Next pressure:

Mira is still around `Point3D(3404,3201,0)` in the Thieves Guild, facing Saxon with `The Right Survival Gear` open and no target cursor. The next client-visible decision is to close or scroll Saxon's gump, then choose between the trash barrel, the nearby doors, the pickpocket dip grind, or the oak shelf exit.

## Run 338 - The Trash Barrel Opens Like Trash

I start on the west side of the Thieves Guild around `Point3D(3404,3201,0)`, still looking at Saxon's `The Right Survival Gear` gump. The fox is following, Saxon is north by the trash barrel, Lore and the lockbox row are northeast, the tavern crowd is east and south, and the secret-looking doors are still in sight. Nobody on screen is hostile. The open gump is the screen problem, not travel.

**Beat 1**

I close Saxon's survival-gear gump.

Nothing follows me out of the gump: no item handoff, no target cursor, no vendor list, no payment prompt, and no new message. I am just back in the room with the same `0` gold problem.

**Beat 2**

I walk a few steps north to the trash barrel, stopping around `Point3D(3405,3198,0)`.

This is open interior floor. Saxon is now close on my northwest side, the trash barrel is within reach, and the fox pads along. The lockboxes and pickpocket dip stay visible northeast, but they are not the thing under my cursor.

**Beat 3**

I double-click the trash barrel.

It opens as a normal container-style disposal window, not a stash. I do not drag any of my starter items into it because the whole point of a trash barrel is deletion pressure, and I still own almost nothing. No gold, lockpick, job clue, skill check, guild membership, discovery flag, combat state, route transition, hunger/thirst change, or follower change appears.

Mechanical friction learned:

- A trash barrel must be used from close range like a container; from the new spot it is within reach.
- Opening it is not a starter-loot route. It is a disposal container, and dropping items into it would start the deletion path rather than solve Mira's money/tool shortage.
- The useful thief leads are unchanged: the pickpocket dip trains only Stealing, lockboxes still need a lockpick or paid service, Lore's join fee is still `8,000` gold, and Saxon's free row was only gear advice.

Next pressure:

Mira is now around `Point3D(3405,3198,0)` in the Thieves Guild with the trash barrel container display open and no target cursor. The next client-visible decision is to close the barrel and choose a real route: return to the pickpocket dip, try the nearby doors, inspect another vendor/NPC, or use the oak shelf exit.

## Run 339 - I Open The Nearest Door

I start at about `Point3D(3405,3198,0)` in the Thieves Guild with the trash barrel container display still open. Saxon is just northwest by the barrel, Lore and the lockbox row are northeast, the pickpocket dip is still visible, and the nearest strong-wood door is a few steps east. Nobody on screen is acting hostile. The open barrel window is still the first thing I have to clear.

**Beat 1**

I close the trash barrel display.

Nothing spills out of it. No item moves, no target cursor appears, no message tells me I found a tool, and I am not dropping any of my starter things into a deletion-looking container.

**Beat 2**

I walk east across the open guild floor, stopping around `Point3D(3407,3198,0)`, two tiles from the nearest `StrongWoodDoor`.

This is just room movement. The fox keeps up, Saxon falls behind my left shoulder, the pickpocket dip and lockboxes stay visible northeast, and the secret-looking dungeon door is now close enough to make me nervous but not the one I am clicking.

**Beat 3**

I double-click the nearest strong-wood door.

It works like an ordinary door from this range. I get the door action instead of a locked message, gump, target cursor, vendor window, or teleport. The useful result is simple: the west-side room has a door I can open, but opening it does not solve the money, lockpick, guild-fee, or thief-note problem by itself.

Mechanical friction learned:

- The trash barrel remains only disposal pressure after closing; there is no delayed loot or prompt.
- The nearby `StrongWoodDoor` needs close range. From `Point3D(3407,3198,0)`, the door at `Point3D(3409,3198,0)` is within the normal two-tile use range.
- This door is not a public-door teleporter, quest object, or money route. It just opens the local guild interior path and leaves the secret-looking door as the next obvious risky object.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, follower count, or region changed.

Next pressure:

Mira ends around `Point3D(3407,3198,0)`, facing east beside the now-open strong-wood door, with no open UI and no target cursor. The next client-visible decision is to step through the opened doorway, test the nearby secret dungeon door, back out toward the pickpocket dip, talk to another tavern/vendor NPC, or leave by the oak shelf.

## Run 340 - I Nudge The Secret Door Open

I start around `Point3D(3407,3198,0)` in the Thieves Guild, no gump open and no target cursor, with the strong-wood door in front of me already open. Saxon is behind me near the trash barrel, Lore and the lockboxes are northeast, the pickpocket dip is still visible, tavern people are ahead and south, and nobody on screen is acting hostile. The secret-looking dungeon door is close enough to be the obvious risk.

**Beat 1**

I step through the opened strong-wood doorway and stop just past it, around `Point3D(3410,3198,0)`.

It is only a local doorway. Iain and a waiter become edge-visible farther east, the tavern crowd is clearer, and the secret dungeon door is now within reach. The fox follows; no message, target cursor, gump, region change, or hostile reaction interrupts the move.

**Beat 2**

I double-click the nearby `SecretDungeonDoor`.

It opens like a close-range door, not like a teleporter or quest object. There is no lock message, no payment prompt, no dungeon-warning gump, no target cursor, and no sudden travel. The scary part is the shape of the doorway, not a mechanical demand the client exposes yet.

**Beat 3**

I stop at the opened secret-looking doorway instead of charging through it.

The room behind me still has known safe practice surfaces and vendors; the passage in front of me is a possible deeper route with no visible reward on the current screen. With `0` gold, no lockpick, and a job note whose exact assignment line is still unresolved, stepping through is now a deliberate risk decision rather than an automatic next click.

Mechanical friction learned:

- The opened strong-wood door only connects local guild floor space. Walking through it does not change region, inventory, skills, follower state, discovery flags, or money.
- The nearby `SecretDungeonDoor` is usable from about `Point3D(3410,3198,0)` and opens as a normal close-range door. It does not by itself reveal a hidden access check, quest prompt, teleport, loot container, or dungeon-entry warning.
- Opening the secret-looking door still does not solve the starter money/tool loop: Lore's join fee remains `8,000` gold, Iain's service still needs `1,000` gold, and the lockboxes still need a lockpick or paid help.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, follower count, or map changed.

Next pressure:

Mira ends around `Point3D(3410,3198,0)`, facing east/southeast beside the now-open `SecretDungeonDoor`, with no open UI and no target cursor. The next client-visible decision is to step through the opened secret-looking doorway, back away to the pickpocket dip and lockbox row, inspect the tavern/vendor crowd, or leave by the oak shelf.

## Run 341 - The Secret Door Only Leads To Tavern Advice

I start at about `Point3D(3410,3198,0)` in the Thieves Guild with no gump open and no target cursor. The secret-looking door is already open beside me. Saxon is behind me near the trash barrel, Lore and the lockboxes are north, tavern people are ahead, and nobody on screen is hostile.

**Beat 1**

I step through the opened `SecretDungeonDoor` and stop on the tavern side around `Point3D(3414,3199,0)`.

Nothing snaps shut on me. This is just more guild floor: Bolton and Abra are almost shoulder-close, Chanah the barkeeper is a few steps south, table games and the `Stealing the Past` board are in reach, and the fox keeps up. No target cursor, warning gump, teleport, loot, money, skill gain, crime flag, discovery flag, or combat state appears.

**Beat 2**

I context-click Chanah and choose the visible talk/info row.

A speech gump opens titled `Best To Travel With Friends`. Chanah is not handing over a companion, lockpick, gold, or job clue. The gump is barkeeper advice, not a reward screen.

**Beat 3**

I read the visible gump and stop with it open.

The lesson is about henchmen: barkeepers, tavern keepers, and innkeepers can sell fighter, archer, or wizard help; each helper is tied to a henchman item; they need treasure or gold to stay with me; and they fight, but they do not pick locks or remove traps. The line that matters for my broke thief problem is ugly: `5` gold buys only `1` minute of service, and I still have `0` gold.

Mechanical friction learned:

- The opened secret-looking door is a local doorway into the tavern side, not a dungeon warning, teleport, or payout.
- Chanah's free talk row opens `Best To Travel With Friends`, an information gump about henchmen.
- Henchmen are a paid support route, not starter help. With `0` gold and no saleable treasure, the barkeeper does not solve the lockpick, join-fee, or thief-note route.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, discovery flag, quest state, target cursor, pet order, follower count, or map changed.

Next pressure:

Mira ends around `Point3D(3414,3199,0)`, facing Chanah with `Best To Travel With Friends` open and no target cursor. The next client-visible decision is to close or scroll Chanah's gump, then choose between the nearby `Forged Census Records`, the farther secret-looking door, the pickpocket dip, the lockbox row, another tavern NPC, or the oak shelf exit.

## Run 342 - The Forged Records Want Money Too

I start around `Point3D(3414,3199,0)` in the Thieves Guild with Chanah's `Best To Travel With Friends` gump still open and already read. The tavern side is crowded but calm: Bolton and Abra are close, Chanah is a few steps south, Ciro and Goldie are farther in the room, and the forged census records sit on a table to the south. Nobody on screen is hostile.

**Beat 1**

I close Chanah's henchman gump.

No helper appears, no target cursor opens, and no delayed message turns the barkeeper lesson into a reward. I am just back on the guild floor with `0` gold.

**Beat 2**

I walk southeast/south across the tavern floor to the records table, stopping beside the `Forged Census Records` around `Point3D(3420,3211,0)`.

This is short interior movement through the visible tavern crowd. Ciro, Goldie, diti, Condon, Carmel, Hesper, and Tanek are still passive context. The fox follows close enough to stay with me.

**Beat 3**

I double-click the `Forged Census Records` and read the gump that opens.

This is not a thief-job clue or free paperwork. It is a name-change form: the thieves guild says my name is on its forged list, gives me a text field, and says a new fantasy-appropriate name can be no longer than `16` characters. The ugly part is the price: it wants `2,000` gold, and I still have `0`.

Mechanical friction learned:

- The forged records open a custom census/name-change gump, not a loot container, job route, guild hint, or discovery route.
- The visible form has a text entry, a submit button, and a close/cancel button.
- The rename route is blocked by `2,000` gold before it matters whether I can think of an acceptable name.
- No name, gold, inventory, skill, stat, hunger, thirst, combat state, crime flag, guild membership, discovery flag, target cursor, follower order, or map changed.

Next pressure:

Mira ends around `Point3D(3420,3211,0)`, facing the records with the forged census/name-change gump open and read. The next client-visible decision is to close the records gump or deliberately type a new name despite having `0` gold, then choose between the nearby southern secret door, Carmel/other tavern NPCs, the game tables, the pickpocket dip route back north, or the oak shelf exit.

## Run 343 - I Close The Records And Size Up Carmel

I start around `Point3D(3420,3211,0)` in the Thieves Guild with the `Forged Census Records` rename gump still open and already read. It wants `2,000` gold for a name change, and I still have `0`. Carmel the assassin guildmaster is a few steps south; the southern secret-looking door is west-southwest; the tavern crowd is calm.

**Beat 1**

I close the forged-records gump.

The floor comes back without a delayed rename, payment prompt, target cursor, item handoff, clue, discovery, or thief-job update. The records are still just paid paperwork.

**Beat 2**

I sidestep south around the record table toward Carmel, stopping close enough to reach her context menu, around `Point3D(3420,3213,0)`.

This is only short interior movement. Ciro, Goldie, Carmel, Hesper, Tanek, diti, Condon, and the table games remain passive screen context. The fox keeps up. No hostile reaction, region shift, combat state, or route transition interrupts me.

**Beat 3**

I context-click Carmel and read the menu instead of choosing a row yet.

The useful thing is that Carmel is not a free supply handoff. I see guildmaster-style choices: `Buy` and `Sell` are present but not useful while I am not in her guild, `Join` is a visible guild commitment route, `Hire`/bribe is visible but clearly money-facing, and `Talk` is the safe information row. I stop here because picking one of those rows is the next decision, not something I should do accidentally.

Mechanical friction learned:

- The forged records close cleanly and still do not change name, gold, inventory, skills, guild state, discovery flags, target cursor, or route.
- Carmel is close enough from the records side to inspect through the normal context menu after a tiny step south.
- Assassin guildmaster `Buy`/`Sell` is gated by guild access; the visible useful rows are conversation, join, or paid bribe/hire pressure.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, guild membership, discovery flag, quest state, target cursor, pet order, follower count, or map changed.

Next pressure:

Mira ends around `Point3D(3420,3213,0)`, facing Carmel with Carmel's context menu open and no target cursor. The next client-visible decision is to choose `Talk`, `Join`, or `Hire`/bribe from Carmel's menu, close the menu and test the nearby southern secret door, or backtrack toward the thief-practice surfaces and oak shelf.

## Run 344 - Carmel Talks Like This Costs Later

I start around `Point3D(3420,3213,0)` in the Thieves Guild with Carmel's context menu already open and read. She is two steps south. The room is still packed with passive tavern and guild people: Ciro and Goldie behind me, Hesper and Tanek southeast, diti to the northeast edge, Condon west, and the southern secret-looking door a few tiles to the west. Nobody on screen is hostile, and no world-map marker is close enough to matter.

**Beat 1**

I choose Carmel's visible `Talk` row.

The context menu disappears and a speech gump opens titled `Death and Taxes`. This is not a buy window, a join confirmation, a target cursor, or a payment prompt.

**Beat 2**

I read the gump and stop with it open.

Carmel's pitch is useful but not helpful in the broke-thief way I was hoping for. She says this is the assassins' guild, that Xardok on a secluded Sosaria island handles assignments, and that she sells items useful to assassins. The money line is worse: if I have a murder problem, she can bribe guards for `50,000` gold, with guild members paying half. I have `0` gold and no murders, so this teaches a future crime-management route, not a starter route.

Mechanical friction learned:

- Carmel's `Talk` row opens a scrollable `Death and Taxes` speech gump with a single close button.
- The assassin talk gives navigation and guild context: Xardok is the assignment contact, not Carmel.
- The visible bribe price is `50,000` gold before any half-price guild discount, so Carmel is not a cash/tool solution for a fresh broke character.
- No name, gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, guild membership, discovery flag, quest state, target cursor, vendor window, pet order, follower count, location, or map changed.

Next pressure:

Mira ends around `Point3D(3420,3213,0)`, facing Carmel with `Death and Taxes` open and read. The next client-visible decision is to close the gump, then choose whether to test the southern secret door, talk to another tavern NPC, backtrack to the thief-practice surfaces, or use the oak shelf route out.

## Run 345 - The Southern Secret Door Is Just A Door

I start around `Point3D(3420,3213,0)` in the Thieves Guild with Carmel's `Death and Taxes` gump still open and already read. The room is still calm: Carmel is close, Ciro and Goldie are behind me, Hesper and Tanek are southeast, Condon is west, and the southern secret-looking door sits a short walk to the west. Nobody on screen is hostile, and the world-map overlay gives me no nearby marker to chase from inside this hidden guild room.

**Beat 1**

I close Carmel's speech gump.

The tavern floor comes back. There is no delayed assassin assignment, vendor window, target cursor, payment prompt, guild join, bribe, murder-count change, item, gold, discovery, or route jump.

**Beat 2**

I walk west along the open tavern floor until I am beside the southern `SecretDungeonDoor`, stopping around `Point3D(3416,3213,0)`.

This is only a few steps through the visible interior. The fox keeps up, the tavern people stay passive, and nothing dangerous moves into my face.

**Beat 3**

I double-click the southern `SecretDungeonDoor`.

It opens like the earlier secret-looking door did. No gump opens, no warning fires, no target cursor appears, and no teleport drags me somewhere else. It is not a free cache, not a starter-money route, and not an assignment lead. I stop before stepping through because the next real client choice is whether to cross that doorway or turn back toward the tavern and thief-practice leads.

Mechanical friction learned:

- Carmel's `Death and Taxes` gump closes cleanly and still does not become an assignment, bribe, shop, or join flow.
- The southern `SecretDungeonDoor` has to be approached before use; from the records/Carmel side it is visible but not a from-six-tiles-away interaction.
- Once used from beside it, the door opens as ordinary local interior movement. It does not produce loot, money, a lockpick, a thief clue, a guild state change, a discovery flag, a target cursor, a gump, a vendor window, combat, damage, hunger, thirst, or follower change.

Next pressure:

Mira ends around `Point3D(3416,3213,0)`, facing west beside the now-open southern `SecretDungeonDoor`. The next client-visible decision is to step through the doorway and see what is beyond it, or leave it open and choose another visible tavern/thief-guild lead.

## Run 346 - I Cross To Condon's Menu

I start around `Point3D(3416,3213,0)`, facing west beside the already-open southern `SecretDungeonDoor`. The screen is still an interior tavern/guild room, not a wilderness route: Condon is visible to the west, Carmel is east, Chanah and Ciro are north/northeast, the west-side doors and the old vanished-satchel spot are in route memory, and no hostile is close enough to force a fight or flee decision. The world-map overlay still gives me no nearby marker from inside this hidden room.

**Beat 1**

I step through the opened southern `SecretDungeonDoor`.

Nothing catches me. I do not get pulled into a dungeon instance, warned by a gate, shown a gump, or given a target cursor. It is just the west side of the same interior.

**Beat 2**

I walk west and slightly south across the open floor to stand close enough to Condon, stopping around `Point3D(3406,3214,0)`.

This is short indoor movement. Condon is now two tiles west, Carmel is farther back east, Chanah and Ciro remain north/northeast, and Saxon is only edge-visible far north by the trash barrel. The fox keeps up. No door, wall, hostile, region change, damage, or stamina problem interrupts the walk.

**Beat 3**

I context-click Condon and read the menu.

Condon is not a loose reward or a free mage starter kit. The menu is vendor-shaped: `Buy`, `Sell`, a free `Talk` row, and a mage service row are the visible pressure. With `0` gold and no magic wand in my pack, the money/service rows are not the safe first click. I stop on the menu because choosing `Talk`, buying, selling, or testing the service row is the next actual UI decision.

Mechanical friction learned:

- Crossing the opened southern `SecretDungeonDoor` is ordinary local movement, not a teleport, cache, warning, assignment, target cursor, or discovery.
- Condon is reachable from the west side after a short walk and opens a normal mage/vendor context menu.
- The visible Condon menu is not a cash route by itself. `Buy` needs money, `Sell` needs something the vendor accepts, and the mage service row is item/payment-facing. `Talk` is the clean next information row.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, murder count, guild membership, discovery flag, quest state, target cursor, vendor window, gump, pet order, follower count, location map, or route anchor changed.

Next pressure:

Mira ends around `Point3D(3406,3214,0)`, facing west toward Condon with Condon's context menu open and no target cursor. The next client-visible decision is to choose `Talk`, test `Sell`, open `Buy` despite having `0` gold, try the mage service row, close the menu and use the nearby west doors, or backtrack toward thief-practice leads.

## Run 347 - Condon Teaches Wizard Chores

I start around `Point3D(3406,3214,0)`, facing west at Condon with his context menu already open. This is still the same hidden guild/tavern interior: Condon is two tiles away, Carmel is east, Chanah and Ciro are north/northeast, Saxon and the trash barrel sit at the north edge of the screen, and the opened southern secret-looking door is behind me to the east. No hostile creature is on screen, the fox is still with me, and the world map still gives me no nearby marker for this interior.

**Beat 1**

I choose Condon's visible `Talk` row.

The context menu closes and a speech gump opens titled `The Mystical Art Of Wizardry`. It is not a buy list, a sell list, a wand target cursor, or a payment prompt.

**Beat 2**

I read the gump and stop with it open.

Condon is useful as a warning label, not as a broke starter rescue. He says mage service is wand charging: `100` gold per spell circle for 5 more charges. I have `0` gold and no wand, so that row is still a future service, not an answer. The practical parts are spell advice: `create food` also makes water, `remove trap` makes an orb that passively checks traps, `telekinesis` can move loose out-of-reach objects into the backpack, and `reveal` can expose traps and hidden treasure containers. He also says custom spell bars live behind Help, and that elementalism and magery conflict unless a cave crystal is used to forget one path.

Mechanical friction learned:

- Condon's `Talk` row opens `The Mystical Art Of Wizardry` speech gump with a close button.
- The mage service is not free: wand charging costs `100` gold per spell circle and also requires a magic wand target.
- The gump teaches future magic utility routes, especially food/water, trap detection, telekinesis, reveal, and spell bars, but it gives no starter cash, wand, reagents, spellbook, or destination.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, murder count, guild membership, discovery flag, quest state, target cursor, vendor window, pet order, follower count, location map, or route anchor changed.

Next pressure:

Mira ends around `Point3D(3406,3214,0)`, facing west toward Condon with `The Mystical Art Of Wizardry` gump open and read. The next client-visible decision is to close the gump, then choose whether to test Condon's `Buy`/`Sell`/wand service rows, use the nearby west doors, backtrack toward thief-practice leads, or leave through the oak shelf route.

## Run 348 - Condon Does Not Want My Pack

I start around `Point3D(3406,3214,0)`, facing west toward Condon with `The Mystical Art Of Wizardry` still open and already read. Condon is two tiles west, Carmel is far enough east to ignore for the moment, Chanah and Ciro are north/northeast, Saxon and the trash barrel sit at the north edge, and the west-side doors are still the nearest physical exits. The fox is with me. Nobody on screen is hostile, and the world-map overlay still does not give this hidden guild room a useful nearby marker; the closest marker I can place is far outside the client view.

**Beat 1**

I close the wizardry gump.

The room view comes back cleanly. No delayed wand offer, spellbook, reagent, gold, target cursor, vendor window, or route jump appears.

**Beat 2**

I context-click Condon again and read the familiar vendor menu.

It is still the same pressure surface: `Buy`, `Sell`, `Talk`, and the wand-charging service row. Since I have `0` gold, the only possible broke-player cash check is `Sell`.

**Beat 3**

I choose `Sell`.

Condon does not open a sell list. He just says, `You have nothing I would be interested in.` My cooked alien meat, medical record, tailor deed, and thief note do not become mage-shop money. Nothing leaves the backpack and no payment appears.

Mechanical friction learned:

- Closing `The Mystical Art Of Wizardry` produces no delayed reward, tool, target cursor, or route.
- Condon's `Sell` row is not a starter-cash route for Mira's current backpack.
- The mage vendor only becomes a sell window if the backpack contains items from his buy list. Mira's known pack contents do not match it.
- No gold, item, bank content, skill, stat, hunger, thirst, damage, crime flag, murder count, guild membership, discovery flag, quest state, target cursor, vendor window, pet order, follower count, location, map, or route anchor changed.

Next pressure:

Mira remains around `Point3D(3406,3214,0)`, facing west toward Condon with no gump, context menu, vendor window, or target cursor open. The next client-visible decision is to test Condon's `Buy` list despite having `0` gold, test the wand service row and cancel/target honestly, use the nearby west-side doors, backtrack toward thief-practice leads, or leave through the oak shelf route.
