# Confictura In-Game Player Documentation Style Guide

Audit target: current main runtime/data snapshot, last changed at 6b447099977f81d66585e8401e3ef904be7da3d9

Historical synchronization point: cbd03db1f1be73c2fa345ac6b4ee5abfa63ae840 (the 2023-10-25 informational-gump and talk-text revision)

This guide describes the player-facing documentation language already present in the game. It is a routing and writing standard for later incorporation batches, not authorization to expose secrets or to replace source review with prose.

## Core principles

- Teach in the world. A feature is documented only when an ordinary player can reach the explanation through Help, the Personal Library, a physical book, NPC speech, a system help gump, a quest or discovery journal, or contextual feedback.
- Put operational truth before flavor when loss is possible. Controls, costs, permissions, persistence, destructive effects, PvP rules, and stop or undo paths must be clear before commitment.
- Let discovery remain discovery. Explain clue paths, warning signs, and interaction vocabulary while withholding exact locations, passwords, solutions, hidden reward tables, and surprises.
- Write at the point of need. Use global Help for universal controls; Library references for persistent system knowledge; lore and NPC speech for world knowledge; system gumps for dense mechanics; journals and contextual feedback for stateful steps.
- Separate source-defined reachability from live reachability. This checkout contains checked-in spawn and configuration data but no world save. Never turn a source definition into a live-world availability claim without staging evidence.

## Established visual and interaction language

### Help and built-in reference gumps

Primary source: Data/Scripts/System/Help/Gumps/HelpGump.cs.

- The Help gump uses the shard's full-size art 9548, tinted through PlayerSettings.GetGumpHue, with a left column of compact page buttons, an explicit header, a standard close button, and a large scrollable HTML reading pane. Preserve this layout instead of introducing a visually unrelated manual.
- Its voice is direct, second-person, and operational: what a player can do, the command or button to use, and the immediate result.
- Keep command syntax literal and case-faithful. Pair every persistent toggle or automated action with its current state and stop or undo path.
- Keep global Help shallow. Link or route substantial systems to a Library reference or dedicated help gump rather than adding a wall of text to the main page.

Use Help for universal commands, account or character settings, privacy behavior, help requests, movement or interface conventions, and links to deeper in-game references.

### Personal Library and unlock behavior

Primary sources: Data/Scripts/System/Commands/Player/MyLibrary.cs, Data/Scripts/Mobiles/Base/PlayerSettings.cs, and Data/Scripts/Custom/LoreBooks/LoreBookCatalog.cs.

- The Personal Library uses art 9546, the player's configured gump hue, a four-column paged index, and Previous/Next controls. Its built-in entries are Basics, conditional Creature Help, Fame & Karma, Item Properties, Skills, and Weapon Abilities; additional entries appear only when their catalog IDs are unlocked.
- Built-in entries cover foundational mechanics; unlocked entries preserve the pleasure of collecting knowledge.
- A Library reference should have a stable title, compact subject grouping, short paragraphs, and enough mechanical detail to act without consulting an external page.
- Do not silently grant secret lore as a built-in reference. Unlock it through the established catalog or teach only the non-spoiling control layer globally.

The current catalog has 29 static entry declarations and 58 checked-in XML lore records. Later content batches must preserve unique identifiers and unlock semantics.

### Physical guide, learning, and lore books

Primary sources: Data/Scripts/Items/Books/DynamicBook.cs, Data/Scripts/Custom/LoreBooks/LoreBookCatalog.cs, and Data/Scripts/Custom/LoreBooks/LoreBooks.xml.

- Guide and learning books may be plain and instructional. Lore books should sound authored inside the world and may be unreliable in flavor, but controls and irreversible risk must not be misleading.
- Use short titled sections and page-sized passages. Let book discovery unlock the corresponding Library entry where the catalog supports it.
- Cite acquisition only when source or checked-in placement proves it. If live stock or placement depends on a world save, say that staging must confirm it.

### NPC dialogue and the shard greeter

Primary sources: Data/Scripts/System/Misc/Talk.cs and Data/Scripts/Mobiles/Civilized/ShardGreeter.cs.

- NPC teaching is conversational, local, and role-appropriate. A shipwright teaches sailing; a banker teaches banking speech; a greeter or sage routes newcomers to general help.
- Keep speech branches short and keyword-tolerant. Repeat the action word or phrase the player should use next.
- NPC hints may point toward a mystery but should not become an exhaustive mechanics manual. Route dense material to a book, Library entry, or system help gump.

### System-specific help gumps

Representative sources: Data/Scripts/Custom/Government System/Gumps/GovHelpGump.cs, Data/Scripts/Trades/Apiculture/BeeHiveHelp.cs, and Data/Scripts/Trades/Gardening/Network/DisplayHelpTopic.cs.

- Use a dedicated gump when a system has several pages of state, permissions, progression, costs, or terminology.
- Put the most dangerous or expensive rules early. Preserve stable navigation labels and page ordering so returning players can find information again.
- Derive numbers and state labels from shared constants when practical in the later implementation batch; otherwise record the source symbol beside the prose review.

### Quest, discovery, and contextual feedback

Representative sources: quest scripts under Data/Scripts/Quests, quest routing in Data/Scripts/System/Help/Gumps/HelpGump.cs, and interaction messages throughout items, mobiles, trades, and regions.

- Journals teach the current objective and next clue, not the complete solution.
- Contextual feedback should say why an action failed, whether anything was consumed, and what safe corrective action is available.
- Login notices and one-time announcements are reminders, not durable documentation. Pair important rules with Help or the Library.

## Surface routing matrix

| Need | Preferred surface | Why |
| --- | --- | --- |
| Universal control, command, setting, privacy, or support path | Help | Always reachable and operational. |
| Persistent mechanics reference, progression, crafting, harvesting, or combat school | LibraryReference | Durable and searchable after the player encounters the system. |
| World history, discovery clue, dangerous place, rare object, or secret-adjacent mechanic | LoreBook | Preserves voice and discovery. |
| Role-local introduction or keyword interaction | NPCSpeech | Teaches in context and can route deeper. |
| Multi-page rules, authority, cost, or stateful management | SystemHelpGump | Supports density and stable navigation. |
| Stateful objective or discovery chain | QuestJournal | Keeps next steps tied to player progress. |
| Immediate failure, consumption, cooldown, danger, or confirmation | ContextualFeedback | Appears at the exact decision point. |

## Content shape and acceptance

Every later incorporation item should answer, in this order: what this is; how the player encounters it; how to start; controls or syntax; costs and requirements; persistent or destructive effects; stop, undo, or recovery path; where to learn more. Use concise paragraphs, literal commands, and the vocabulary already shown by the owning gump or NPC.

Acceptance is player-path based. An ordinary staging character must be able to reach the surface without staff commands, follow every navigation control, reproduce the stated behavior, and see honest feedback for invalid, costly, destructive, and disabled states. Acquisition and placement require separate live verification because no world save is present in this checkout.
