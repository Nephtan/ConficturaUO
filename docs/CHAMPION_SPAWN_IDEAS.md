# Champion spawns: a menu for adventure

**Prepared September 26, 2026.** Staff brainstorming and implementation estimates, grounded in the September 17–25 general discussion, the September 25 admin discussion, and the current repository.

**My recommendation:** make champions dependable, give each destination a useful specialty, and let those discoveries lead into crafting and another adventure. Start with clear scroll labels, Underworld Krystals, a small regional supply cache, and a modest skull exchange. Add one Bowcaster blueprint after that. These give players something to use, trade, seek out, and tell friends about without requiring a new progression system.

Only the loot parity repair described below is implemented in this batch. Every other entry is an option for discussion. Comments and promises in the exports are feedback, not proof of deployment or authorization to implement the entire menu.

## 1. What the repair delivers

Every playable map now uses the existing Lodor champion reward rules:

- A qualifying boss death distributes **six Power Scrolls total**, shared among eligible boss-damage contributors. They raise a skill cap to **110, 115, or 120**, approximately **60%, 35%, or 5%** respectively. They are not six per participant, and an individual is not guaranteed a scroll in a large group.
- One boss skull goes to a random eligible looter, or into the boss corpse if nobody qualifies.
- With the ML expansion flag enabled, tracked wave kills credited to a player have the same **0.1%** scroll chance everywhere: 25/49 of successful rolls produce a **0.6–1.0 skill-point** Scroll of Transcendence; 24/49 produce a **105-cap** Power Scroll. Pet kills resolve to the master. The old alternative wave roll on three maps has been removed.
- Luck and ET/alien origin do not gate those rewards. Existing damage rights, Justice copies, dead-player delivery, full-backpack fallback, gold scatter, and boss artifact rules remain in place. The Star Room travel gate is a separate system and was not expanded.
- Null maps and the internal storage map are excluded. All seven registered playable maps are covered, including Atlantis if staff place a champion there.

This fixes the map restriction behind the reported gold-only outcome. It does not guarantee loot for players without qualifying damage credit, make every optional artifact drop, or change where overflow lands. See the [mechanics guide](wiki/Champion_Spawns.md#rewards).

**Verification:** Release/x86 server build, real runtime script compile with `-compileonly -nocache`, and 616 assertions against the compiled scripts passed. The fixture covers map parity, normal/alien characters with zero or 2000 equipment Luck, damage rights, shared quantities, pet credit, expired damage, corpse/backpack delivery, and wave roll boundaries. No world saves were loaded and no listener was started. Production deployment and an in-client completion remain outstanding; a local commit is not a live restart.

## 2. What players and staff are asking for

| Feedback | Design response |
| --- | --- |
| Thant and Kvaran completed events and saw only gold; Luck and ET status became suspects. | Make the baseline reliable and explain where rewards went. Add practical staff diagnostics before a complicated loot overhaul. [G1] |
| Mun wants to retain scrolls as an earned alternative to buying them. Nephtan agreed. | Keep the repaired boss scroll allocation. Place new regional rewards alongside it. [G2] |
| Scrolls do not clearly state their caps. | Show skill and final cap explicitly, including plain champion-generated scrolls. [G3] |
| Mun and Nephtan want shared rewards plus distinct destination pools, rare reagents, and materials. | One small common pool and a short regional list; not nine complete new systems. [G4, A1] |
| Thant wants ammunition for the Bowcaster; Mun wants the weapon to justify the ammunition hunt. | Put useful Krystal bundles in the Underworld pool, then offer a carefully bounded named weapon or blueprint. [G5, A2] |
| The admin proposal includes Soulstones, high-level spell scrolls, local currencies, and hoard-style artifacts. | Reuse existing item families selectively, after checking their economy, account, and acquisition rules. [A1] |
| Mun wants to avoid the crowded trinket slot and does not want every reward to be a weapon. | Prefer consumables, craft discoveries, materials, tools, collectibles, and a few distinctive weapons. [A3] |
| Players want a purpose for skulls and a way to recover from equipment enhancement decisions made early in progression. | Offer a cheap first skull use; investigate enhancement history before promising a true reset. [G6] |
| Tsai proposes chosen-skill +1 cap scrolls, but also questions caps without meaningful benefits. | Treat skill-cap expansion as a separate balance project. Show an actual benefit for each approved skill before raising its ceiling. [G7] |
| Discussion of an unusual weapon made SpenceOne want to log in and explore. | Support discoveries with in-world rumors, recognizable rewards, and reasons to trade stories and supplies. [G8] |

**An unresolved proposal, not part of this fix:** the admin draft suggests 110–125 Power Scrolls and making everything except money probabilistic. That differs from Lodor's six base 110/115/120 scrolls. My recommendation is to preserve the repaired baseline and experiment with optional additions first. Do not silently turn this repair into fewer guaranteed scrolls or introduce 125-cap drops. [A1]

## 3. Menu and implementation effort

These are my estimates for focused engineering work, including source tracing, implementation, local regression checks, compilation, and documentation. They assume existing art and a small first release. Live testing and balance iteration add time. Rows are alternatives, not a commitment to build them all; estimates for additions to regional pools assume the shared pool foundation is already in place.

### Community proposals

| ID | Idea and smallest useful version | Player impact | Difficulty / estimate | Main work or decision |
| --- | --- | --- | --- | --- |
| C1 | **Readable scrolls.** Display the skill and final cap on all Power Scroll variants. | High confidence, low spectacle; rewards immediately make sense. | Easy, **half–1 day** | Use stored `Value` for plain scrolls without breaking named subclasses or shrine use rules. [S3] |
| C2 | **Regional spoils.** One themed bonus cache per boss, initially containing existing resources. | High: different places become worth visiting. | Moderate, **2–4 days** for the foundation and nine small lists | Resolve the champion's actual destination, establish shared distribution, and test boundaries. Do not use the looter's current position. [S1, S2] |
| C3 | **Underworld Krystal bundles.** A practical ammunition supply from successful hunts. | Very high for a small change; makes an unusual weapon usable. | Easy, **half–1 day** after C2 | Tune bundle size to expected ammunition consumption; do not confuse `Krystal` ammo with `Crystals` currency. [S4] |
| C4 | **A named Bowcaster.** One desirable variant with a clear role and existing visuals. | High: a memorable item people will discuss. | Small–moderate, **1–3 days** | Compare damage, speed, abilities, ammunition cost, and enhancement access against existing ranged weapons; avoid a universal best weapon. [S4] |
| C5 | **A recovered Bowcaster blueprint.** A learnable recipe requiring established crafting skills and a few expedition materials. | Very high: connects fighters, explorers, and crafters. | Moderate, **2–4 days** for one recipe | Reuse `RecipeScroll`, `AddRecipe`, and saved player recipe knowledge. Audit the selected craft menu/tool, allocate a unique recipe ID, and verify repair, exceptional quality, and resource consumption. [S5] |
| C6 | **Rare reagents and crafting supplies.** Two or three useful bundles per destination. | High repeat value and a reason to trade. | Easy, **1–2 days** after C2 | Select actual item types and useful quantities; preserve gathering and existing quest sources. [S6] |
| C7 | **Soulstone rewards.** A low-frequency opportunity to experiment with builds. | High utility, especially for established characters. | Small–moderate, **1–2 days** | Existing base, blue, red, and fragment classes have account/use rules. Decide binding, tradability, and which forms belong in the pool; test first acquisition and transfers. [S7] |
| C8 | **Spell caches.** A small selection of advanced Magery, Necromancy, or Elementalism scrolls. | Medium–high: rewards spellbook completion and trade. | Easy, **half–1 day** after C2 | Reuse appropriate scroll factories, but choose a deliberate high-level subset; the existing hoard example spans broader ranges. [S6] |
| C9 | **Local treasure.** A small regional currency cache accompanying the existing gold spectacle. | Medium; makes loot feel connected to place. | Small, **1–2 days** after C2 | Price by purchasing power. Equal item counts are not equal value; Underworld jewels and crash-site xormite are distinct existing precedents. [S6] |
| C10 | **A use for one skull.** A collector exchanges a skull for a choice of modest supplies or a trophy decoration. | Very high: every skull becomes a decision or a trade. | Moderate, **2–4 days** | Safely validate ownership and consume once. Decide whether old skulls qualify. Start with modest choices, not a new gear tier. [S8] |
| C11 | **The six-skull rite.** Completing a set opens a short encounter with an existing boss and a chosen reward. | High social pull and a longer collection goal. | Moderate–hard, **4–7 days** | Existing altar only consumes the set. Add a real payoff, cooldown/cleanup, and recovery on failure; first ensure all six skull types are obtainable. [S8] |
| C12 | **Enhancement reset reagent.** Recover from an early equipment-building decision. | High for affected crafters. | Hard, **5–10 days** after a focused feasibility review | Current upgrades modify the item directly. A refund/reset must distinguish purchased changes from original loot properties and other upgrades; that history cannot be assumed for old items. A limited crafting service voucher is a cheaper **1–3 day** alternative. [S9] |
| C13 | **Chosen-skill +1 cap scroll.** Initially a small allowlist of skills with proven benefits and an explicit ceiling. | Potentially high, with substantial balance consequences. | Moderate–hard, **3–6 days** for a bounded pilot | Honor total skill caps and existing use restrictions; verify the real formulas, save/load behavior, stacking, and value of each increment. Do not assume 125 or 150 has the same meaning for every skill. [G7, S3] |
| C14 | **Broad progression above 125.** Make higher skill values meaningfully affect many systems. | Uncertain until the affected formulas are known. | Large, **multiple weeks**; no defensible fixed estimate before a skill audit | This is combat, crafting, and progression balancing, not just a new scroll. Keep it out of the first releases; test a handful of skills through C13 instead. |

### Additional ideas from this review

| ID | Idea and smallest useful version | Player impact | Difficulty / estimate | Keep it small by… |
| --- | --- | --- | --- | --- |
| N1 | **Tavern rumors.** A barkeeper mentions a broad region where trouble or strange salvage has appeared. Expire the rumor when the event ends. | Very high: helps people find the content while preserving exploration. | Moderate, **2–3 days** | Using one in-game rumor surface and coarse directions. Verify the active controller exists before advertising it; reuse existing barkeeper patterns. No external service is needed. [S10] |
| N2 | **The expedition journal.** Earn a stamp for completing a champion in each of the nine destinations; a full page grants a title or display trophy. | High: a reason to travel rather than farm one convenient spot. | Moderate, **3–5 days** | One collection page and one cosmetic prize. Store progress in one versioned item or attachment with clear ownership; define eligible completion credit before implementation. |
| N3 | **Collector commissions.** A collector requests a particular skull type or regional salvage for a useful supply choice. | High: duplicate loot gains purpose and drives player trade. | Moderate, **2–4 days** | Two or three exchanges, no new currency, and a fixed reward budget. C10 can be the first version; do not build both separately. |
| N4 | **One encounter surprise.** A champion occasionally arrives with one thematic hazard or escort objective. | High variety without writing nine bosses. | Moderate, **3–5 days** for one modifier | One visible, understandable mechanic, existing effects, bounded adds, and reliable cleanup. Playtest solo and small groups before adding another. |
| N5 | **Hunt souvenirs with a story.** A named banner, book, or mounted trophy records the boss and destination. | Medium–high social value with little power inflation. | Small, **1–2 days** | Existing art and one item family; name the achievement clearly. Do not use a trinket slot. |
| N6 | **A reason for veterans to bring a friend.** A modest shared supply bonus for a legitimately participating group. | Potentially high social impact. | Moderate–hard, **3–5 days** | One bounded group reward after defining participation. Existing boss damage rights do not fully recognize healing/support, and party membership alone is too easy to exploit. |
| N7 | **Clear reward delivery.** A brief in-world message names the earned reward and says if it went to a corpse or the ground; staff can inspect a compact reward record. | High trust; fewer reports of missing loot. | Small, **1–2 days** | Instrumenting actual creation and delivery outcomes; use plain player language and keep detailed damage calculations in staff tools. |
| N8 | **A rotating expedition.** Staff highlight one existing destination for a limited run with a cosmetic souvenir and a lore hook. | High immediate draw with little new engineering. | Easy, **half–1 day** for a manual pilot; **2–4 days** to automate later | Running a staff-hosted pilot before building a calendar, seasonal system, or more currency. |

## 4. A first menu for all nine destinations

These are proposed themes, not new claims about canonical lore. Start with two useful existing-item bundles and one occasional discovery per area. A destination does not need an exclusive artifact weapon to feel different.

| Destination / XML controller | Proposed identity | Useful repeat reward | Occasional discovery or collection goal |
| --- | --- | --- | --- |
| Sosaria / `RWChamp_Sosaria` | Adventurers supplying the trade guilds | Crafting materials and practical reagents | A guild recipe or a named expedition banner |
| Lodoria / `RWChamp_Lodoria` | Veteran hunts and recovered battle relics | Martial supplies or a modest crafting service reward | A trophy set or unusual existing weapon variant |
| Underworld / `RWChamp_UnderW` | Lost technology and dangerous salvage | **Krystals**, with a separately valued jewel cache | A Bowcaster blueprint; later, one named Bowcaster |
| Serpent Island / `RWChamp_Serpent` | Serpentine antiquities and forgotten scholarship | Reagents and selected spell scrolls | A mosaic/trophy fragment or a recovered recipe |
| Savaged Empire / `RWChamp_Savage` | Great beasts and wilderness craft | Hides, bones, and other existing crafting resources | A hunting trophy or survival-themed craft pattern |
| Isles of Dread / `RWChamp_IslesD` | Spoils from dangerous creatures | Rare reagents and monster-derived materials | A specimen display or a bounded specialist weapon |
| Ambrosia / `RWChamp_Ambrosi` | Lost knowledge and difficult expeditions | Rare magical materials | A selected spell cache, a recipe, or a later C13 pilot reward after balance review |
| Umber Veil / `RWChamp_Umber` | Veiled relics and recovered histories | Reagents and restoration supplies | A lore book, funerary ornament, or collector commission |
| Bottle World of Kuldar / `RWChamp_Bottle` | Curious crafts from an enclosed world | Unusual existing craft components | A miniature scene, bottle display, or curious blueprint |

**The implementation trap:** those are nine destinations across **six physical maps**. Ambrosia, Umber Veil, and Kuldar share `Map.Sosaria`. A switch on `Map` would merge their rewards. Use the spawn's location and a reviewed destination classifier, with a common-pool fallback. `Worlds.GetMyWorld` is a useful starting point, but its rectangle edges do not exactly match every XML area: test borders, interiors, and region overrides explicitly. Do not copy display strings into an unchecked reward switch. [S2]

The XML has no Atlantis controller. Keep common rewards available there, and leave any new Atlantis-specific pool for an actual content decision. Likewise, the configured delays and live spawner state should be checked before advertising an exact daily count.

## 5. The short release path I would choose

1. **Reliable and understandable.** Deploy and playtest this parity fix, then C1 and N7. Confirm a qualifying player receives the promised rewards on Lodor and a non-Lodor map. Show actual cap values and delivery locations.
2. **Something worth seeking.** Build C2 with very small lists for all nine destinations, including C3. Add a rumor pilot from N1. A first release can use existing items throughout; no need to wait for nine new artifacts. Budget roughly **4–7 focused days** for this combined slice after loot quantities and locations are agreed.
3. **Something worth returning for.** Add C10, then one C5 blueprint. This joins combat, collecting, crafting, and trade. Budget roughly **4–7 focused days** for a deliberately small combined slice, rather than a full crafting overhaul.
4. **Expand what players actually use.** Choose the journal, a named Bowcaster, or one encounter surprise from observed interest. Keep the six-skull rite, enhancement resets, and broad skill-cap work separate until their prerequisites are resolved.

A sample player journey: hear about strange activity from a barkeeper, find the Underworld champion, earn ordinary champion rewards and usable ammunition, trade a recovered blueprint to a crafter, and return with friends to pursue a named weapon or complete an expedition collection. Each step can be released and enjoyed on its own.

## 6. Small design decisions that prevent expensive rework

- **Allocation comes before more loot.** Decide whether a new cache is one shared prize or one per qualifying player. My first choice is one shared bonus cache awarded through explicit contribution rules; always label that clearly. Per-player rewards multiply the economy and require stronger participation rules.
- **Keep base rewards dependable.** Add optional surprises to the restored scroll/skull baseline. Set bounded resource quantities, rare desirable discoveries, and a useful outlet for duplicates. Avoid an account-wide pity counter in the first release; it adds persistence and farming rules.
- **Use the existing economy deliberately.** Gold already has uses, and the gold shower is recognizable. Preserve that moment initially. Review the value of any added jewels, xormite, Soulstones, or crafting vouchers rather than compensating every concern with more currency.
- **Reuse item factories, not whole unrelated reward flows.** Hoard piles already select useful item families, but their Luck, skill, and location rules are not automatically the rules wanted for champions. Curate types and quantities without accidentally restoring the ET/Luck confusion. [S6]
- **Make skull acquisition achievable.** The current overworld XML selects Abyss, Arachnid, Vermin Horde, and Unholy Terror. Those produce Pain, Venom, Greed, and Death. Power and Enlightenment require other champion themes/sources; a six-skull rite needs an acquisition audit or an intentional exchange. Do not ask players to chase an unavailable set. [S1, S8]
- **Do not destroy old equipment to imitate a reset.** Existing enhancement values can contain original loot properties. An old-item reset needs a defined policy and verified provenance; a carefully limited service voucher avoids pretending that history exists. [S9]
- **Improve the shared reward path before adding costly prizes.** `ChampionSpawn.AwardArtifact` rolls from 1 through total damage but tests cumulative damage with `>`, leaving the final endpoint unassigned. Track a focused correction and boundary test before reusing it for regional caches; this separate issue is not repaired by map parity. Delivery also deletes an artifact if insertion fails. [S1]
- **Keep new persistence small.** Prefer existing recipe storage and independent versioned reward items. Journals, cooldowns, and new account rewards need save/load tests and explicit old-save defaults. Do not widen `PlayerMobile` just to launch a small experiment.

## 7. How to tell whether it is working

For the repair, use an ordinary player character with an empty pack and sufficient fresh boss damage on Lodor and a non-Lodor map. Repeat with an ET character, a pet build, and two participants. Check a dead recipient and a full pack. Observe a real gold scatter and artifact completion separately. Record the deployed revision and map; a source review cannot establish live acceptance.

For the first content pilot, ask and measure a few concrete things:

- Are players finding active champions more easily, and visiting more than one destination?
- Does a typical useful supply bundle support another outing without flooding the market?
- Are players using Bowcasters, trading recipes/materials, and inviting someone along?
- Do duplicate skulls remain worth picking up? Are any regions consistently ignored?
- Are rewards reaching their intended recipients, with no extra payout from relogging, controller deletion, or repeated completion processing?

Use a short pilot and adjust a few pool entries or quantities. Broad gear-stat inflation and a months-long content rewrite are unnecessary to answer those questions.

## 8. Discussion sources

Reviewed inputs: `Confictura_Champion_Spawns_Transcript.md` (91 retained messages, September 17–25) and `[DiscordKit] admin-chat_Confictura_ Legend and Adventure_20260926_114032.json` (30 messages, September 25). The transcript preserves four image references but no transcribed image contents; estimates here do not assume the pictured items' statistics. Admin links require access to that channel. Summaries above distinguish suggestions from verified mechanics.

- **G1:** [Reports and owner investigation](https://discord.com/channels/1060698288011104358/1060698288401154159/1552049518210453525); [map restriction discovered](https://discord.com/channels/1060698288011104358/1060698288401154159/1552436145726562335).
- **G2:** [Retain earned Power Scrolls](https://discord.com/channels/1060698288011104358/1060698288401154159/1552675478589149218).
- **G3:** [Missing cap labels and agreement to improve them](https://discord.com/channels/1060698288011104358/1060698288401154159/1552675716619960391).
- **G4:** [Destination-specific pools](https://discord.com/channels/1060698288011104358/1060698288401154159/1552668057418076281); [rare reagents/materials](https://discord.com/channels/1060698288011104358/1060698288401154159/1552676052659081377); [common pool plus local rewards](https://discord.com/channels/1060698288011104358/1060698288401154159/1552678938143105025).
- **G5:** [Thant's Krystal request](https://discord.com/channels/1060698288011104358/1060698288401154159/1552847198998495252).
- **G6:** [Give skulls a purpose](https://discord.com/channels/1060698288011104358/1060698288401154159/1552675241518563351); [enhancement reset reward suggestion](https://discord.com/channels/1060698288011104358/1060698288401154159/1551764310748037211).
- **G7:** [Chosen-skill +1 proposal](https://discord.com/channels/1060698288011104358/1060698288401154159/1552682689956679794); [concern about tangible benefits](https://discord.com/channels/1060698288011104358/1060698288401154159/1552753052317130813); [owner wants meaningful backend benefits](https://discord.com/channels/1060698288011104358/1060698288401154159/1552770024937103381).
- **G8:** [Discoveries encourage logging in and exploring](https://discord.com/channels/1060698288011104358/1060698288401154159/1553072325770092578).
- **A1:** [Admin common reward draft](https://discord.com/channels/1060698288011104358/1060698289684611226/1553004436434190387); [regional draft](https://discord.com/channels/1060698288011104358/1060698289684611226/1553007839205916713).
- **A2:** [Bowcaster blueprint](https://discord.com/channels/1060698288011104358/1060698289684611226/1552968017699143754); [artifact Bowcasters and ammunition value](https://discord.com/channels/1060698288011104358/1060698289684611226/1552997937343102979).
- **A3:** [Variety beyond artifact weapons](https://discord.com/channels/1060698288011104358/1060698289684611226/1552998761087770655); [avoid the trinket slot](https://discord.com/channels/1060698288011104358/1060698289684611226/1552999992422170666).

## 9. Source trace and implementation starting points

Paths and symbols were checked locally on September 26. Existing code is evidence of reusable components, not proof that a proposed feature is already wired into champions or works in the deployed world.

| Ref | Source | Why it matters |
| --- | --- | --- |
| S1 | [BaseChampion.cs](../Data/Scripts/Custom/Champions/System/Mobiles/BaseChampion.cs), `GivePowerScrolls`, `OnBeforeDeath`, `OnDeath`, `GetArtifact`; [ChampionSpawn.cs](../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs), `OnSlice`, `AwardArtifact`, `GiveArtifact`, `IsEligible`; [ChampionSpawnInfo.cs](../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnInfo.cs) | Current reward entry points, shared quantities, artifact selection/delivery, and theme tables. |
| S2 | [RWChamp.xml](../Spawns/RWChamp.xml), [RWChamp_Monitors.xml](../Spawns/RWChamp_Monitors.xml); [World.cs](../Data/Scripts/System/Misc/World.cs), `Worlds.GetMyWorld`; [MapDefinitions.cs](../Data/Scripts/System/Misc/MapDefinitions.cs) | Nine configured areas, six underlying maps, seven playable registrations, timers, location-based world names. |
| S3 | [PowerScroll.cs](../Data/Scripts/Items/Books/PowerScrolls/PowerScroll.cs), `CreateRandomNoCraft`, `GetPower`, `AddNameProperties`, `CanUse`, `Use`; [ScrollofTranscendence.cs](<../Data/Scripts/Items/Special/Special Scrolls/ScrollofTranscendence.cs>), `CreateRandom` | Stored caps, tooltip mismatch, shrine restrictions, and tenths-of-a-point conversion. |
| S4 | [KilrathiGun.cs](../Data/Scripts/Items/Technology/KilrathiGun.cs), [KilrathiHeavyGun.cs](../Data/Scripts/Items/Technology/KilrathiHeavyGun.cs), `AmmoType`; [Krystal.cs](../Data/Scripts/Items/Technology/Krystal.cs); [DataPad.cs](../Data/Scripts/Items/Technology/DataPad.cs) | Existing Bowcasters, their actual ammunition, and existing lost-technology flavor text. |
| S5 | [Recipes.cs](../Data/Scripts/Trades/Core/Recipes.cs), `Recipe`; [CraftSystem.cs](../Data/Scripts/Trades/Core/CraftSystem.cs), `AddRecipe`; [CraftItem.cs](../Data/Scripts/Trades/Core/CraftItem.cs), recipe check; [RecipeScroll.cs](../Data/Scripts/Items/Trades/Misc/RecipeScroll.cs), `OnDoubleClick`; [PlayerMobile.cs](../Data/Scripts/Mobiles/Base/PlayerMobile.cs), `HasRecipe`, `AcquireRecipe`, recipe serialization | Learnable recipes already exist. Adding one recipe is smaller than inventing an unlock framework. |
| S6 | [HoardPile.cs](../Data/Scripts/Quests/Hoard/HoardPile.cs), `HoardPiles.OnDoubleClick` | Existing artifact, spell, reagent, and regional-currency selection; includes unrelated Luck rules that should not be copied wholesale. |
| S7 | [SoulStone.cs](../Data/Scripts/Items/Special/SoulStone.cs), `SoulStone`, `BlueSoulstone`, `RedSoulstone`, `SoulstoneFragment` | Reusable items with binding, transfer, and use behavior to verify before changing availability. |
| S8 | [ChampionSkull.cs](../Data/Scripts/Custom/Champions/System/Items/ChampionSkull.cs); [ChampionSkullPlatform.cs](../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSkullPlatform.cs), `Validate`, `Clear`; [ChampionSkullBrazier.cs](../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSkullBrazier.cs), `EndSacrifice` | Existing cursed skulls and sacrifice UI; completed sets are consumed without a reward. |
| S9 | [EnhancementStoneProcess.cs](../Data/Scripts/Trades/Guild/EnhancementStoneProcess.cs), `GuildCraftingProcess.BeginUpgrade`, `SpendGold`; [AttributeHandler.cs](../Data/Scripts/Trades/Guild/AttributeHandler.cs), `Upgrade` | Guild enhancements charge gold and mutate attributes. No reset history is established by this path. |
| S10 | [PlayerBarkeeper.cs](../Data/Scripts/Mobiles/Base/PlayerBarkeeper.cs), `BarkeeperRumor`, rumor prompts | Existing in-world rumor pattern; live champion discovery, expiration, and staff management still need implementation. |
| S11 | [Test-ChampionRewards.ps1](../scripts/Test-ChampionRewards.ps1); [ChampionRewardRegression.cs](../tests/ChampionRewardRegression.cs); [Main.cs](../Data/System/Source/Main.cs), `CompileOnly` | Reproducible isolated build, script compile, and actual reward-path fixture checks; no live-world acceptance claim. |

No new reward tables, items, recipes, NPCs, cap increases, timers, or save fields from this menu have been added by the parity repair.
