# Test Center

The Test Center restores the traditional RunUO-style testing tools for an online test shard. It is controlled by the final positional value in `Info/settings.xml` and is enabled on the SAR branch.

## Configuration

Setting 91 is a Boolean value:

```xml
<setting>true</setting>
```

- `true` enables Test Center registration during server startup.
- `false`, a missing setting, or an invalid Boolean value leaves Test Center disabled.
- A server restart is required after changing the value.

The server prints a prominent console warning whenever Test Center is enabled. Turning it off prevents future use but does not remove claimed supplies or undo changes players already made.

## Player Features

Say `help` to open the Test Center help gump. The following speech commands adjust stats and skills within the character's existing caps:

- `set str 125`
- `set dex 125`
- `set int 125`
- `set Magery 120`

Skill names are matched case-insensitively against Confictura's complete `SkillName` enum. Values cannot exceed the individual skill cap or the total skill cap. Stats must be between 10 and 125 and cannot exceed the total stat cap.

The player commands are:

- `[TCSupplies` places the one-time Test Center supply package in the bank. The package raises StatCap to at least 250 and every individual skill cap to at least 120 without lowering any higher custom caps.
- `[TCResurrect` resurrects a dead character at a valid location without charging currency or applying `Death.Penalty`. Confictura's normal resurrection path restores full vitals.
- `[TCCorpses` finds every active, top-level world corpse owned by the character, sorts them newest first, and moves them onto distinct valid tiles within 12 tiles of the character. Corpses that cannot be placed separately remain where they are, and the command reports moved and skipped counts.

The standard Help menu stuck option has no two-use daily quota while Test Center is enabled. Jail, region, combat, criminal, frozen-state, house, and delayed-teleport behavior remain unchanged. Test Center uses do not consume the normal quota that applies after Test Center is disabled.

## Supply Package And Reset

New characters receive the package automatically. Existing characters claim it with `[TCSupplies`.

The package is a non-movable standard wooden box named `Test Center Supplies` in the player's bank. Its contents can be removed normally, but the box remains as the one-time claim marker. Staff can allow a replacement claim by deleting that marker with existing staff tools.

The package follows the traditional AOS/SE Test Center bundle and includes currency, potion kegs, crafting and runic tools, ammunition, treasure maps, raw materials, spellbooks and reagents, ethereal mounts, artifacts, Tokuno rewards, and enhanced bows. Confictura uses its local treasure-map and message-in-a-bottle constructors and substitutes `MagicPigment` for the unavailable `PigmentsOfTokuno` type.

## Source Trace

- Configuration: `Data/Scripts/System/Misc/Settings.cs`, `MyServerSettings.TestCenterEnabled`, and setting 91 in `Info/settings.xml`.
- Runtime surface: `Data/Scripts/System/Misc/TestCenter.cs`, `TestCenter.Enabled`, `TestCenter.Initialize`, `TestCenter.GiveSupplies`, and `TestCenter.TCHelpGump`.
- New-character provisioning: `Data/Scripts/System/Misc/CharacterCreation.cs`, `EventSink_CharacterCreated`.
- Unlimited quota integration: `Data/Scripts/System/Help/Gumps/HelpGump.cs` and `Data/Scripts/System/Help/StuckMenu.cs`.
- Save compatibility: the claim marker is an existing `WoodenBox`; no new serialized type or `PlayerMobile` serialization field is introduced.
