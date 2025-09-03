# Bard Song Magic

## Overview
Bard Song Magic allows musically inclined adventurers to weave songs as magical effects. The SongBook holds sixteen unique songs and tracks an instrument used to perform them, acting much like a spellbook for bards.

## Requirements
- **SongBook** – specialized spellbook storing 16 bardic songs【F:Data/Scripts/Magic/Bard/SongBook.cs†L12-L23】
- **Instrument** – a musical instrument must be assigned to the book; it is saved with the book and consumed over time【F:Data/Scripts/Magic/Bard/SongBook.cs†L25-L55】【F:Data/Scripts/Magic/Bard/SongSpells.cs†L16-L53】
- **Skills** – all songs use *Musicianship* for casting and effect. Effect strength uses the sum of Musicianship, Provocation, Discordance, and Peacemaking【F:Data/Scripts/Magic/Bard/SongSpells.cs†L59-L88】

## Using the SongBook
1. Double–click the SongBook to open the Bardic Songs gump.
2. Click **Assign Instrument** and target a musical instrument in your backpack to set it for the book.【F:Data/Scripts/Magic/Bard/SongBookGump.cs†L57-L76】【F:Data/Scripts/Magic/Bard/SongBookGump.cs†L724-L760】
3. The instrument must remain in your pack; otherwise songs cannot be played.

## Casting Songs
Songs can be triggered either from the SongBook gump or by using chat commands. Each song has a command matching its name (e.g., `[ArmysPaeon]`). Commands require the song to be learned in your book.【F:Data/Scripts/Magic/Bard/SongCommandList.cs†L15-L105】【F:Data/Scripts/Magic/Bard/SongCommandList.cs†L120-L137】

## Available Songs
The SongBook contains the following pieces:

- ArmysPaeon – heals nearby allies over time【F:Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs†L151-L170】
- EnchantingEtude – increases an ally’s Intelligence.
- EnergyCarol – grants energy resistance to nearby allies【F:Data/Scripts/Magic/Bard/Spells/EnergyCarolSong.cs†L81-L94】
- EnergyThrenody – lowers a target’s energy resistance【F:Data/Scripts/Magic/Bard/Spells/EnergyThrenodySong.cs†L98-L132】
- FireCarol – grants fire resistance to nearby allies.
- FireThrenody – lowers a target’s fire resistance.
- FoeRequiem – damages foes in range.
- IceCarol – grants cold resistance to nearby allies.
- IceThrenody – lowers a target’s cold resistance.
- KnightsMinne – raises physical resistance for allies.
- MagesBallad – regenerates mana for allies.
- MagicFinale – damages or dispels nearby enemies.
- PoisonCarol – grants poison resistance to nearby allies.
- PoisonThrenody – lowers a target’s poison resistance.
- SheepfoeMambo – boosts the party’s attack speed.
- SinewyEtude – increases an ally’s Strength.

## Example
To increase your party’s energy resistance:
1. Assign an instrument to your SongBook.
2. Use the command `[EnergyCarol]` or click **Energy Carol** in the book. Nearby allies gain an energy resistance buff whose strength and duration scale with your combined bard skills.

## Tips
- Instruments have limited uses. Successful or failed songs consume charges and may destroy the instrument when depleted.【F:Data/Scripts/Magic/Bard/SongSpells.cs†L34-L53】
- Songs do not require free hands, allowing bards to keep weapons or shields equipped while playing.【F:Data/Scripts/Magic/Bard/SongSpells.cs†L73-L76】
- If a song fails, “The song fizzles” appears overhead to indicate the failure.【F:Data/Scripts/Magic/Bard/SongSpells.cs†L91-L93】