# System: Magic Schools

## Classification

GameplayLayer

## Summary

Player-facing magic school content layered on the spell framework.

## Source Files

Matched source files: 267.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Magic/Bard/SongBook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Bard/SongCommandList.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Bard/SongSpells.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Bard/Gumps/SongBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Bard/Scrolls/ArmysPaeon.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/EnchantingEtude.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/EnergyCarol.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/EnergyThrenody.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/FireCarol.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/FireThrenody.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/FoeRequiem.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/IceCarol.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/IceThrenody.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/KnightsMinne.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/MagesBallad.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/MagicFinale.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/PoisonCarol.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/PoisonThrenody.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/SheepfoeMambo.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Scrolls/SinewyEtude.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/EnchantingEtudeSong.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Bard/Spells/EnergyCarolSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/EnergyThrenodySong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/FireCarolSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/FireThrenodySong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/FoeRequiemSong.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Bard/Spells/IceCarolSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/IceThrenodySong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/KnightsMinneSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/MagesBalladSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/MagicFinaleSong.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Bard/Spells/PoisonCarolSong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/PoisonThrenodySong.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Bard/Spells/SheepfoeMamboSong.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Bard/Spells/SinewyEtudeSong.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/DeathKnightCommands.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Death Knight/DeathKnightSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/DeathKnightSpellBook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Death Knight/DeathSkulls.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Death Knight/DevilPact.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Death Knight/SoulLantern.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Death Knight/Gumps/DeathKnightSpellBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Death Knight/Spells/BanishSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/DemonicTouchSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/DevilPactSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/GrimReaperSpell.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Magic/Death Knight/Spells/HagHandSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/HellfireSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Death Knight/Spells/LucifersBoltSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/OrbOfOrcusSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/ShieldOfHateSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Death Knight/Spells/SoulReaperSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Death Knight/Spells/StrengthOfSteelSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/StrikeSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Death Knight/Spells/SuccubusSkinSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Death Knight/Spells/WrathSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/ElementalCommands.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Elementalism/ElementalEffect.cs | StartupWiring | CustomTimerSubclass;OnMovement | Yes |  |
| Data/Scripts/Magic/Elementalism/ElementalExit.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/ElementalScrolls.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/ElementalShrine.cs | GumpUI | OnSpeech | Yes | SendGump |
| Data/Scripts/Magic/Elementalism/ElementalSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/ElementalSpellbook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Elementalism/Gumps/ElementalSpellbookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledAir.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledEarth.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledFire.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledWater.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendAir.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendEarth.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendFire.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendWater.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalLordAir.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalLordEarth.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalLordFire.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalLordWater.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritAir.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritEarth.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritFire.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritWater.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSteed.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSummonEnt.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSummonIce.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSummonLightning.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Mobiles/ElementalSummonMagma.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Elementalism/Sphere 1/Elemental_Armor.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 1/Elemental_Bolt.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 1/Elemental_Mend.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 1/Elemental_Sanctuary.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 2/Elemental_Pain.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 2/Elemental_Protection.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 2/Elemental_Purge.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 2/Elemental_Steed.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 3/Elemental_Call.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 3/Elemental_Force.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 3/Elemental_Wall.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Elementalism/Sphere 3/Elemental_Warp.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 4/Elemental_Field.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Elementalism/Sphere 4/Elemental_Restoration.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 4/Elemental_Strike.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 4/Elemental_Void.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 5/Elemental_Blast.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 5/Elemental_Echo.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 5/Elemental_Fiend.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 5/Elemental_Hold.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 6/Elemental_Barrage.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 6/Elemental_Rune.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 6/Elemental_Storm.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 6/Elemental_Summon.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 7/Elemental_Devastation.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 7/Elemental_Fall.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 7/Elemental_Gate.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Elementalism/Sphere 7/Elemental_Havoc.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 8/Elemental_Apocalypse.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 8/Elemental_Lord.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Elementalism/Sphere 8/Elemental_Soul.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Magic/Elementalism/Sphere 8/Elemental_Spirit.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/HolyManCommands.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Holy Man/HolyManSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/HolyManSpellBook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Holy Man/HolySymbol.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Holy Man/HolySymbols.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Holy Man/MalletStake.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Holy Man/Gumps/HolyManSpellBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Holy Man/Spells/BanishEvilSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Holy Man/Spells/DampenSpiritSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/Spells/EnchantSpell.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Holy Man/Spells/HammerOfFaithSpell.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Holy Man/Spells/HeavenlyLightSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/Spells/NourishSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/Spells/PurgeSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/Spells/RebirthSpell.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Magic/Holy Man/Spells/SacredBoonSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Holy Man/Spells/SanctifySpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Holy Man/Spells/SeanceSpell.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Holy Man/Spells/SmiteSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/Spells/TouchOfLifeSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Holy Man/Spells/TrialByFireSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/JediCommandList.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Jedi/JediDatacrons.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Jedi/JediSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/JediSpellbook.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Magic/Jedi/KaranCrystal.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Jedi/Gumps/JediSpellBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Jedi/Spells/Celerity.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Jedi/Spells/Deflection.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/ForceGrip.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/MindsEye.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/Mirage.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Jedi/Spells/PsychicAura.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/Replicate.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/SoothingTouch.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/StasisField.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Jedi/Spells/ThrowSabre.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/MysticCommandList.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Mystic/MysticMonkRobe.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/MysticPack.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/MysticSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/MysticSpellBook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Mystic/Gumps/MysticSpellBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Mystic/Scrolls/AstralProjectionScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/AstralTravelScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/CreateRobeScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/GentleTouchScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/LeapScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/PsionicBlastScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/PsychicWallScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/PurityOfBodyScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/QuiveringPalmScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Scrolls/WindRunnerScroll.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Mystic/Spells/AstralProjection.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Mystic/Spells/AstralTravel.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/Spells/CreateRobe.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/Spells/GentleTouch.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/Spells/Leap.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/Spells/PsionicBlast.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Magic/Mystic/Spells/PsychicWall.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/Spells/PurityOfBody.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Mystic/Spells/QuiveringPalm.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Mystic/Spells/WindRunner.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/AncientSpellBook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Magic/Research/ResearchBag.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Magic/Research/ResearchCommands.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Research/ResearchFunctions.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/ResearchSpell.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Gumps/AncientSpellBookGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchAerialServant.cs | Persistence | OnMovement | Yes |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchClone.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchConjure.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchCreateGold.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchDeathVortex.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchExtinguish.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchMagicSteed.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchSwarm.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchCreateGolem.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchDeathSpeak.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchGrantPeace.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchMaskofDeath.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchOpenGround.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchRockFlesh.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchSummonDead.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Death/ResearchWithstandDeath.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchCauseFear.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchCharm.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs | StartupWiring | CustomTimerSubclass;EventSink;OnLogout | Yes |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchMassMight.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchMassSleep.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchSleep.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchSleepField.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Research/Spells/Enchanting/ResearchSneak.cs | StartupWiring | CustomTimerSubclass;EventSink;OnLogout | No |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchConflagration.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchCreateFire.cs | StartupWiring | CustomTimerSubclass;OnMovement | Yes |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchEndureCold.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchEndureHeat.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchExplosion.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchFlameBolt.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchIgnite.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Sorcery/ResearchRingofFire.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonAcidElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonBloodElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonElectricalElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonGemElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonIceElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonMudElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonPoisonElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Summoning/ResearchSummonWeedElemental.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchBanishDaemon.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchCallDestruction.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchConfusionBlast.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchDevastation.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchEtherealTravel.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchMeteorShower.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchSummonCreature.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Thaumaturgy/ResearchSummonDevil.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchAirWalk.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchDivination.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchFadefromSight.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchHealingTouch.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchIntervention.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchRestoration.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchSeeTruth.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Theurgy/ResearchWizardEye.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchAvalanche.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchFrostField.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchFrostStrike.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchGasCloud.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchHailStorm.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchIcicle.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Research/Spells/Wizardry/ResearchMassDeath.cs | CombatPolicy |  | No |  |
| ... | ... | ... | ... | Full file list is in phase-04-system-owner-map.csv. |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Bard/SongCommandList.cs; Line=110; LikelySystem=Magic:Bard; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Bard/SongCommandList.cs:110 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Death Knight/DeathKnightCommands.cs; Line=77; LikelySystem=Magic:Death Knight; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Death Knight/DeathKnightCommands.cs:77 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Elementalism/ElementalCommands.cs; Line=74; LikelySystem=Magic:Elementalism; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Elementalism/ElementalCommands.cs:74 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Holy Man/HolyManCommands.cs; Line=65; LikelySystem=Magic:Holy Man; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Holy Man/HolyManCommands.cs:65 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Jedi/JediCommandList.cs; Line=62; LikelySystem=Magic:Jedi; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Jedi/JediCommandList.cs:62 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Mystic/MysticCommandList.cs; Line=78; LikelySystem=Magic:Mystic; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Mystic/MysticCommandList.cs:78 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Research/ResearchCommands.cs; Line=324; LikelySystem=Magic:Research; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Research/ResearchCommands.cs:324 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Magic/Syth/SythCommandList.cs; Line=66; LikelySystem=Magic:Syth; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/Magic/Syth/SythCommandList.cs:66 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;EventSink;OnLogout;CustomTimerSubclass;OnMovement;Initialize;OnMovement;OnSpeech;Timer.DelayCall

## Serialized State

Serialized marker files: 89. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Spell Framework; spellbooks; combat policy; gumps

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Magery_Spell_Color_Setting.md;docs/wiki/Magery_Spell_System.md;docs/wiki/Misc_Magic_Spells.md;docs/wiki/Witchcraft.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
