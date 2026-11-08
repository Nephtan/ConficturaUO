# Build a fresh local world

Use a separate checkout or server directory for a new local world. Keep its
generated saves and backups outside version control. The public inputs are
the map assets, decoration and spawn definitions, and runtime scripts in
this repository.

1. Build the Release x86 server from the [server project](../Data/System/Source/Server.csproj).
   Put the executable in the local server directory alongside the checked-in
   `Data`, `Info`, and `Spawns` directories.
2. For a private local shard, bind the [listener](../Data/Scripts/System/Misc/SocketOptions.cs)
   to loopback and configure a local-only port before starting. Keep external
   integrations disabled in the local copy. Start the server interactively
   with `-nocache` so it compiles scripts and can prompt for a local owner
   account when there are no accounts.
3. Point ClassicUO at the local listener and use a client asset directory
   with the same map and static files as the server. Keep each
   `staidxN.mul` with its matching `staticsN.mul`.
4. Sign in as the local owner and run `[buildworld` on the empty world.
   Wait for the command to finish, then save with `[save`. This generates
   the checked-in decorations, PremiumSpawner placements, and
   script-generated content. It also works through the supplemental files
   in [Info/Custom](../Info/Custom).
5. Load any explicitly selected XMLSpawner package separately with
   `[XmlLoad <filename>`. The [Spawns directory](../Spawns) contains both
   authored packages and examples. The loader uses each definition's facet
   and coordinates and replaces matching GUIDs.

   For Sosaria's six cemetery nest controllers, run
   `[XmlLoad UndeadNests-Sosaria.xml` on the fresh local world.
   [Cemetery spawn definitions](../Spawns/UndeadNests-Sosaria.xml)

   For the nine champion controllers and nine monitors, run
   `[XmlLoad RWChamp.xml` followed by `[XmlLoad RWChamp_Monitors.xml` on the
   fresh world. These imports start the authored champion sequence. On an
   existing world, verify the saved controller identities and encounter state
   before importing either package.
   [Champion controllers](../Spawns/RWChamp.xml) ·
   [Champion monitors](../Spawns/RWChamp_Monitors.xml)

6. On the fresh world, initialize the three fixed Sosaria Unholy Terror
   champion encounters once. Run these commands as the local owner. Set
   each radius after its controller is in place so the spawn area centers
   on the correct coordinates.

   ```text
   [go Sosaria
   [go 2168 361 15
   [Self Add ChampionSpawn set Type UnholyTerror
   [Global Set SpawnRadius 15 Active true where ChampionSpawn Map = Sosaria X = 2168 Y = 361 Z = 15
   [go 2513 950 16
   [Self Add ChampionSpawn set Type UnholyTerror
   [Global Set SpawnRadius 50 Active true where ChampionSpawn Map = Sosaria X = 2513 Y = 950 Z = 16
   [go 5720 2348 15
   [Self Add ChampionSpawn set Type UnholyTerror
   [Global Set SpawnRadius 25 Active true where ChampionSpawn Map = Sosaria X = 5720 Y = 2348 Z = 15
   [go 5717 2348 0
   [save
   ```

   The encounters start at the first wave. Each controller builds its linked
   platform, altar, and idol. Subsequent rebuilds keep the controllers and
   their encounter state. Existing saves retain their fixed controllers.
   [Champion controller](../Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs)

PremiumSpawner entries support `set` for construction-time properties and
`setafter` for properties that must follow the normal spawn hook. The
Atlantis creature replacements use `setafter` to retain their saved XML
overrides after regional adjustments. The optional `localspawn` keyword
follows the creature type. It uses the configured spawn footprint and
sets each creature's home to its spawn point, with the walking range
measured from that point. Use `bounds <minX> <minY> <maxX> <maxY>` after
`localspawn` to specify inclusive coordinates for a rectangular area.
In the first spawn slot, `refillone` immediately after `localspawn` limits
each timer tick to filling one vacancy across the configured slots.
When upgrading a copied save,
verify and retire each replaced XML controller by identity before testing
the rebuild. [Custom spawn definitions](../Info/Custom/Spawns.map)

Sosaria and Atlantis are separate server facets. Both use file index 1, so
an edit to the index-1 MUL statics appears at the same coordinates on both.
Facet-specific scenery and functional placements belong in the appropriate
facet's configuration. Umber Veil's main locations are on Sosaria.
[Map definitions](../Data/Scripts/System/Misc/MapDefinitions.cs) ·
[Staff destinations](../Data/System/XML/go.xml)

For an existing world, rehearse the rebuild on a disposable copy first.
`[buildworld` removes every PremiumSpawner and sweeps decorations marked
with weight `-2` before regeneration. Saved-only and moved placements can
disappear or return at older coordinates. When adopting updated static
assets on an existing world, compare the frozen tiles with the dynamic
items at the same coordinates and retire only the verified duplicate
scenery on a copy before rollout.
[Rebuild and decoration implementation](../Data/Scripts/System/Misc/Build.cs)
