# Phase 3 High-Risk Root Summary

Generated: 2026-06-13T23:31:16.7524335-05:00

| Root | Main Role | Main Risk | First Follow-Up |
| --- | --- | --- | --- |
| Custom | Mixed custom gameplay, imported packages, and staff tools | Package boundaries, global hooks, project include drift, and persistence-heavy custom systems | Prioritize XMLSpawner, Government, PvP Consent, Homestead, AI, and Random Encounters system cards. |
| Items | Item content and persistent item save surfaces | Large serialized save surface and moved gump project drift | Use serialization register before item moves; review missing gump compile targets. |
| Magic | Spell framework and magic school content | Spell registry coupling and high-ID spell family registration | Map spell framework dependencies and registry limits before edits. |
| Mobiles | Mobile content and AI behavior | Serialized mobiles, AI assumptions, and save compatibility | Prioritize high-risk serialized mobiles in Phase 6. |
| Quests | Quest systems, gumps, and rewards | Quest state, gump responses, and reward side effects | Trace quest entry points and serialized quest state. |
| System | Runtime wiring, commands, framework, help, skills, and policy | Global hooks, command access, packet handlers, and project drift | Use runtime hook map to review command/event/packet surfaces. |
| Trades | Crafting, harvest, economy, bulk orders, and gardening | Economy loops, pooled enumerable usage, and moved gump project drift | Review crafting roots and bulk order/gardening gump drift. |
| ServerCore | Core server framework | Shared runtime framework and build dependency for scripts | Keep server framework separate from script reorganization unless specifically required. |
