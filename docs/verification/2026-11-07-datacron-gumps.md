# Jedi and Sith Datacron verification

## Reproduction and repair

On baseline `2483e23b`, double-click an owned Datacron in the backpack and click a learned power while wearing the faction's equipment and wielding a sword. The Jedi Force Grip and Sith Psychokinesis buttons closed their books without casting. Typing `[Psychokinesis` opened the expected target cursor on the same Sith setup. Both gumps tested the encoded reply ID against learned powers before removing the 100 offset.

Open the construction page while eligible at the faction's tomb, leave the tomb with the page open, and click a sword button. The baseline created a Jedi single blade and a Sith double blade in the wilderness. Each spent 10,000 gold, consumed the gem and durasteel, and reset fame and karma.

## Source and entry points

- [Jedi gump](../../Data/Scripts/Magic/Jedi/Gumps/JediSpellBookGump.cs) and [Sith gump](../../Data/Scripts/Magic/Syth/Gumps/SythSpellBookGump.cs) handle player replies from their owned, equipped or top-level backpack books. Jedi replies 380–389 decode to spell IDs 280–289. Sith replies 370–379 decode to 270–279. Details use the spell IDs directly. Both cast through the existing faction dispatcher, which invokes the Player commands from the [Jedi command list](../../Data/Scripts/Magic/Jedi/JediCommandList.cs) and [Sith command list](../../Data/Scripts/Magic/Syth/SythCommandList.cs). The [spell registry](../../Data/Scripts/Magic/Base/Initializer.cs) supplies the corresponding spell types.
- [Jedi book](../../Data/Scripts/Magic/Jedi/JediSpellbook.cs) and [Sith book](../../Data/Scripts/Magic/Syth/SythSpellbook.cs) provide the double-click and quick bar entry points. Quick bar replies check their originating book and recheck learned powers. The serialized version-0 read and write code is byte-identical to baseline.
- Construction replies 7 and 8 recheck life, faction, skills, fame, karma, tomb, gem, durasteel and gold before consuming the fee. The page and response share the check. Failed construction reports the reason and refreshes the page. Back explicitly returns to the main page, and closing a subpage cancels it.

## ClassicUO results

Testing used Xeron and his existing two Datacrons in a copied world under a disposable Player login. Fixtures prepared faction equipment, skills, resources, learned powers, locations, and a combat target. ClassicUO clicks initiated every tested cast and construction. The shard displays Sith as Syth.

| Case | Observed result |
| --- | --- |
| Jedi main power replies | All 10 reached their intended spells. The run observed Throw Sabre damage, Soothing Touch healing 144, Celerity's footwear restriction and subsequent activation, Stasis Field rejecting a frozen target and later freezing a roaming goat, and Replicate creating its crystal. |
| Sith main power replies | All 10 reached their intended spells. The run observed Throw Sword and Psychic Blast damage, Lightning splash, Drain Life, the Speed effect, and Clone creating its crystal. Absorption reached the existing protection check and rejected the already-active Deflection. |
| Both layouts and navigation | Opened both Help pages and returned with Back. Verified first-to-last and last-to-first detail navigation for both books and closed a Jedi detail page by right-clicking. Opened both horizontal and vertical quick bars for each faction, toggled vertical labels, clicked a representative power in each orientation, and used Close Quick Bars. |
| Live access and resources | Moving the Jedi book to the ground rejected a stale quick bar reply. Removing ownership rejected a stale Sith Help reply. Removing Clone from the Sith book rejected its stale cast reply without spending crystals. The detail page still showed its requirements and learning hint. A zero-crystal Psychokinesis attempt reported the six-crystal requirement. |
| Construction with changed state | Leaving either tomb after opening the construction page blocked the old button and preserved gold, materials, fame and karma. Removing a Jedi gem after opening the page blocked construction. Opening at zero gold disabled both construction choices and displayed the price. |
| Valid construction | Each faction made one single and one double blade. Each click added exactly one matching sword, cost 10,000 gold, cleared the stored gem and durasteel, and reset fame and karma. The last build used exactly the gold on hand. |

All 20 registry mappings were matched to native spell entry evidence. Absorption's successful application and the post-death behavior of the replica and clone crystals were outside the verified outcomes. The copied historical save loaded for the native tests. Save/reload after gameplay was outside the run.

## Build and evidence

The Release x86 server project built successfully. Baseline and candidate started with `-service -nocache` on loopback. An additional `-compileonly -nocache` run compiled the production script payload in a separate directory without test instrumentation. Source and input hashes were checked before copying the four tested scripts into the checkout. The isolated client and server exited and their port was verified closed.

Local evidence is under `D:/ConficturaUOTestTools/datacron-native-20261107-080725/evidence/`. The main files are `baseline-events.tsv`, `candidate-events.tsv`, `runtime-compile-production-payload.log`, `source-and-protection-check.json` and `shutdown.json`. The event trace captures spell checks and before/after state. The ClassicUO transcript shows the buttons, target cursors, effects, and journal messages.
