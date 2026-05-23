using System;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    // Internal scoring lanes used to collapse Confictura's many class systems into stable
    // random encounter aliases and staff diagnostics without expanding the XML contract.
    public enum CharacterArchetype
    {
        Overall,
        Martial,
        Archer,
        Assassin,
        Ninja,
        Samurai,
        Knight,
        DeathKnight,
        ArcaneMage,
        Elementalist,
        Necromancer,
        Witch,
        Druid,
        HolyMan,
        MysticMonk,
        Jedi,
        Syth,
        Researcher,
        Ranger,
        Bard,
        Thief,
        Jester,
        Crafter,
        Gatherer,
        Merchant,
        Seafarer,
        CreatureRace,
        Alien
    }

    public sealed class CharacterLevelDiagnostics
    {
        public CharacterLevelDiagnostics()
        {
            ArchetypeLevels = new Dictionary<CharacterArchetype, int>();
            EncounterAliasLevels = new Dictionary<LevelType, int>();
        }

        public int OverallLevel { get; internal set; }
        public CharacterArchetype BestArchetype { get; internal set; }
        public int BestArchetypeLevel { get; internal set; }
        public int SkillCap { get; internal set; }
        public int StatCap { get; internal set; }
        public int SkillStart { get; internal set; }
        public int SkillBoost { get; internal set; }
        public int SkillEther { get; internal set; }
        public int Profession { get; internal set; }
        public int RaceID { get; internal set; }
        public NpcGuild NpcGuild { get; internal set; }
        public double SkillProgress { get; internal set; }
        public double StatProgress { get; internal set; }
        public double ReputationProgress { get; internal set; }
        public Dictionary<CharacterArchetype, int> ArchetypeLevels { get; private set; }
        public Dictionary<LevelType, int> EncounterAliasLevels { get; private set; }
    }

    public static class CharacterLevelService
    {
        private const double SkillAtFullArchetypeCredit = 100.0;
        private const double ReputationCap = 30000.0;

        private static readonly CharacterArchetype[] m_AdventureArchetypes =
        {
            CharacterArchetype.Martial,
            CharacterArchetype.Archer,
            CharacterArchetype.Assassin,
            CharacterArchetype.Ninja,
            CharacterArchetype.Samurai,
            CharacterArchetype.Knight,
            CharacterArchetype.DeathKnight,
            CharacterArchetype.ArcaneMage,
            CharacterArchetype.Elementalist,
            CharacterArchetype.Necromancer,
            CharacterArchetype.Witch,
            CharacterArchetype.Druid,
            CharacterArchetype.HolyMan,
            CharacterArchetype.MysticMonk,
            CharacterArchetype.Jedi,
            CharacterArchetype.Syth,
            CharacterArchetype.Researcher,
            CharacterArchetype.Ranger,
            CharacterArchetype.Bard,
            CharacterArchetype.Thief,
            CharacterArchetype.Jester
        };

        public static int GetOverallLevel(Mobile mobile)
        {
            if (mobile == null)
                return 1;

            if (!(mobile is PlayerMobile))
                return GetLegacyMobileLevel(mobile);

            return PowerToLevel(GetOverallPower(mobile));
        }

        public static int GetArchetypeLevel(Mobile mobile, CharacterArchetype archetype)
        {
            if (mobile == null)
                return 1;

            if (archetype == CharacterArchetype.Overall)
                return GetOverallLevel(mobile);

            return PowerToLevel(GetArchetypePower(mobile, archetype));
        }

        public static int GetEncounterLevel(Mobile mobile, LevelType levelType)
        {
            if (mobile == null)
                return 1;

            if (!(mobile is PlayerMobile))
                return GetLegacyMobileLevel(mobile);

            switch (levelType)
            {
                case LevelType.Fighter:
                    return MaxLevel(
                        mobile,
                        CharacterArchetype.Martial,
                        CharacterArchetype.Archer,
                        CharacterArchetype.Assassin,
                        CharacterArchetype.Ninja,
                        CharacterArchetype.Samurai,
                        CharacterArchetype.Knight,
                        CharacterArchetype.DeathKnight,
                        CharacterArchetype.MysticMonk,
                        CharacterArchetype.Jedi,
                        CharacterArchetype.Syth
                    );
                case LevelType.Ranger:
                    return MaxLevel(
                        mobile,
                        CharacterArchetype.Ranger,
                        CharacterArchetype.Druid,
                        CharacterArchetype.Archer,
                        CharacterArchetype.Bard
                    );
                case LevelType.Mage:
                    return MaxLevel(
                        mobile,
                        CharacterArchetype.ArcaneMage,
                        CharacterArchetype.Elementalist,
                        CharacterArchetype.HolyMan,
                        CharacterArchetype.Researcher
                    );
                case LevelType.Necromancer:
                    return MaxLevel(
                        mobile,
                        CharacterArchetype.Necromancer,
                        CharacterArchetype.Witch,
                        CharacterArchetype.DeathKnight
                    );
                case LevelType.Thief:
                    return MaxLevel(
                        mobile,
                        CharacterArchetype.Thief,
                        CharacterArchetype.Assassin,
                        CharacterArchetype.Ninja,
                        CharacterArchetype.Jester
                    );
                case LevelType.Overall:
                default:
                    return GetOverallLevel(mobile);
            }
        }

        public static CharacterLevelDiagnostics GetDiagnostics(Mobile mobile)
        {
            CharacterLevelDiagnostics diagnostics = new CharacterLevelDiagnostics();

            if (mobile == null)
                return diagnostics;

            PlayerMobile player = mobile as PlayerMobile;

            diagnostics.OverallLevel = GetOverallLevel(mobile);
            diagnostics.SkillCap = GetSkillCap(mobile);
            diagnostics.StatCap = GetStatCap(mobile);
            diagnostics.SkillProgress = GetSkillProgress(mobile);
            diagnostics.StatProgress = GetStatProgress(mobile);
            diagnostics.ReputationProgress = GetReputationProgress(mobile);
            diagnostics.RaceID = mobile.RaceID;

            if (player != null)
            {
                diagnostics.SkillStart = player.SkillStart;
                diagnostics.SkillBoost = player.SkillBoost;
                diagnostics.SkillEther = player.SkillEther;
                diagnostics.Profession = player.Profession;
                diagnostics.NpcGuild = player.NpcGuild;
            }

            foreach (CharacterArchetype archetype in Enum.GetValues(typeof(CharacterArchetype)))
            {
                int level = GetArchetypeLevel(mobile, archetype);

                diagnostics.ArchetypeLevels[archetype] = level;

                if (archetype != CharacterArchetype.Overall && level > diagnostics.BestArchetypeLevel)
                {
                    diagnostics.BestArchetype = archetype;
                    diagnostics.BestArchetypeLevel = level;
                }
            }

            foreach (LevelType levelType in Enum.GetValues(typeof(LevelType)))
                diagnostics.EncounterAliasLevels[levelType] = GetEncounterLevel(mobile, levelType);

            return diagnostics;
        }

        public static int GetLegacyMobileLevel(Mobile mobile)
        {
            if (mobile == null)
                return 1;

            int fame = Math.Min(Math.Max(mobile.Fame, 0), 15000);
            int karma = Math.Abs(mobile.Karma);

            if (karma > 15000)
                karma = 15000;

            int skills = mobile.Skills.Total;

            if (skills > 10000)
                skills = 10000;

            skills = (int)(1.5 * skills);

            int stats = mobile.RawStr + mobile.RawDex + mobile.RawInt;

            if (stats > 250)
                stats = 250;

            stats = 60 * stats;

            int level = (int)((fame + karma + skills + stats) / 600);
            level = (int)((level - 10) * 1.12);

            return ClampLevel(level);
        }

        private static double GetOverallPower(Mobile mobile)
        {
            double skillProgress = GetSkillProgress(mobile);
            double statProgress = GetStatProgress(mobile);
            double reputationProgress = GetReputationProgress(mobile);
            double skillPower = 0.75 * GetBestAdventurePower(mobile) + 0.25 * skillProgress;

            return Clamp01(0.60 * skillPower + 0.25 * statProgress + 0.15 * reputationProgress);
        }

        private static double GetBestAdventurePower(Mobile mobile)
        {
            double best = 0.0;

            for (int i = 0; i < m_AdventureArchetypes.Length; i++)
                best = Math.Max(best, GetArchetypePower(mobile, m_AdventureArchetypes[i]));

            return best;
        }

        private static double GetArchetypePower(Mobile mobile, CharacterArchetype archetype)
        {
            switch (archetype)
            {
                case CharacterArchetype.Overall:
                    return GetOverallPower(mobile);
                case CharacterArchetype.Martial:
                    return Weighted(
                        MaxOf(
                            Skill(mobile, SkillName.Swords),
                            Skill(mobile, SkillName.Fencing),
                            Skill(mobile, SkillName.Bludgeoning),
                            Skill(mobile, SkillName.FistFighting)
                        ),
                        Skill(mobile, SkillName.Tactics),
                        Skill(mobile, SkillName.Anatomy),
                        Skill(mobile, SkillName.Parry),
                        Skill(mobile, SkillName.Focus),
                        Skill(mobile, SkillName.Healing)
                    );
                case CharacterArchetype.Archer:
                    return Weighted(
                        Skill(mobile, SkillName.Marksmanship),
                        Skill(mobile, SkillName.Bowcraft),
                        Skill(mobile, SkillName.Tactics),
                        Skill(mobile, SkillName.Anatomy),
                        Skill(mobile, SkillName.Focus)
                    );
                case CharacterArchetype.Assassin:
                    return Weighted(
                        MaxOf(Skill(mobile, SkillName.Fencing), Skill(mobile, SkillName.Poisoning)),
                        Skill(mobile, SkillName.Hiding),
                        Skill(mobile, SkillName.Stealth),
                        Skill(mobile, SkillName.Tactics),
                        Skill(mobile, SkillName.Anatomy)
                    );
                case CharacterArchetype.Ninja:
                    return Weighted(
                        Skill(mobile, SkillName.Ninjitsu),
                        Skill(mobile, SkillName.Hiding),
                        Skill(mobile, SkillName.Stealth),
                        Skill(mobile, SkillName.Tactics),
                        Skill(mobile, SkillName.Focus),
                        Skill(mobile, SkillName.Fencing)
                    );
                case CharacterArchetype.Samurai:
                    return Weighted(
                        Skill(mobile, SkillName.Bushido),
                        Skill(mobile, SkillName.Swords),
                        Skill(mobile, SkillName.Tactics),
                        Skill(mobile, SkillName.Parry),
                        Skill(mobile, SkillName.Focus)
                    );
                case CharacterArchetype.Knight:
                    return Clamp01(
                        0.60 * Skill(mobile, SkillName.Knightship)
                            + 0.20
                                * AverageOf(
                                    Skill(mobile, SkillName.Swords),
                                    Skill(mobile, SkillName.Tactics),
                                    Skill(mobile, SkillName.Healing),
                                    Skill(mobile, SkillName.Spiritualism)
                                )
                            + 0.20 * PositiveKarmaPower(mobile)
                    );
                case CharacterArchetype.DeathKnight:
                    return Clamp01(
                        0.55 * Skill(mobile, SkillName.Knightship)
                            + 0.25
                                * AverageOf(
                                    Skill(mobile, SkillName.Swords),
                                    Skill(mobile, SkillName.Tactics),
                                    Skill(mobile, SkillName.Necromancy),
                                    Skill(mobile, SkillName.Spiritualism)
                                )
                            + 0.20 * NegativeKarmaPower(mobile)
                    );
                case CharacterArchetype.ArcaneMage:
                    return Weighted(
                        Skill(mobile, SkillName.Magery),
                        Skill(mobile, SkillName.Alchemy),
                        Skill(mobile, SkillName.Inscribe),
                        Skill(mobile, SkillName.Meditation),
                        Skill(mobile, SkillName.MagicResist),
                        Skill(mobile, SkillName.Psychology)
                    );
                case CharacterArchetype.Elementalist:
                    return Weighted(
                        Skill(mobile, SkillName.Elementalism),
                        Skill(mobile, SkillName.Meditation),
                        Skill(mobile, SkillName.Psychology),
                        Skill(mobile, SkillName.Focus)
                    );
                case CharacterArchetype.Necromancer:
                    return Clamp01(
                        0.70 * Skill(mobile, SkillName.Necromancy)
                            + 0.20
                                * AverageOf(
                                    Skill(mobile, SkillName.Spiritualism),
                                    Skill(mobile, SkillName.Poisoning),
                                    Skill(mobile, SkillName.MagicResist)
                                )
                            + 0.10 * NegativeKarmaPower(mobile)
                    );
                case CharacterArchetype.Witch:
                    return RequiredPair(
                        Skill(mobile, SkillName.Necromancy),
                        Skill(mobile, SkillName.Forensics),
                        Skill(mobile, SkillName.Poisoning),
                        Skill(mobile, SkillName.Spiritualism),
                        Skill(mobile, SkillName.Tasting),
                        NegativeKarmaPower(mobile)
                    );
                case CharacterArchetype.Druid:
                    return Weighted(
                        Skill(mobile, SkillName.Druidism),
                        Skill(mobile, SkillName.Veterinary),
                        Skill(mobile, SkillName.Taming),
                        Skill(mobile, SkillName.Herding),
                        Skill(mobile, SkillName.Cooking)
                    );
                case CharacterArchetype.HolyMan:
                    return Clamp01(
                        0.60 * Skill(mobile, SkillName.Spiritualism)
                            + 0.20
                                * AverageOf(
                                    Skill(mobile, SkillName.Healing),
                                    Skill(mobile, SkillName.Anatomy)
                                )
                            + 0.20 * PositiveKarmaPower(mobile)
                    );
                case CharacterArchetype.MysticMonk:
                    return Clamp01(
                        0.70 * Skill(mobile, SkillName.FistFighting)
                            + 0.20
                                * AverageOf(
                                    Skill(mobile, SkillName.Focus),
                                    Skill(mobile, SkillName.Anatomy),
                                    Skill(mobile, SkillName.Healing),
                                    Skill(mobile, SkillName.Mysticism)
                                )
                            + 0.10 * IdentityPower(Server.Misc.GetPlayerInfo.isMonk(mobile))
                    );
                case CharacterArchetype.Jedi:
                    return RequiredPair(
                        Skill(mobile, SkillName.Psychology),
                        Skill(mobile, SkillName.Swords),
                        Skill(mobile, SkillName.Tactics),
                        PositiveKarmaPower(mobile),
                        IdentityPower(Server.Misc.GetPlayerInfo.isJedi(mobile, false))
                    );
                case CharacterArchetype.Syth:
                    return RequiredPair(
                        Skill(mobile, SkillName.Psychology),
                        Skill(mobile, SkillName.Swords),
                        Skill(mobile, SkillName.Tactics),
                        NegativeKarmaPower(mobile),
                        IdentityPower(Server.Misc.GetPlayerInfo.isSyth(mobile, false))
                    );
                case CharacterArchetype.Researcher:
                    return Weighted(
                        Skill(mobile, SkillName.Inscribe),
                        Skill(mobile, SkillName.Alchemy),
                        Skill(mobile, SkillName.Magery),
                        Skill(mobile, SkillName.Elementalism),
                        Skill(mobile, SkillName.Meditation),
                        Skill(mobile, SkillName.Psychology)
                    );
                case CharacterArchetype.Ranger:
                    return Weighted(
                        Skill(mobile, SkillName.Tracking),
                        Skill(mobile, SkillName.Camping),
                        Skill(mobile, SkillName.Cartography),
                        Skill(mobile, SkillName.Marksmanship),
                        Skill(mobile, SkillName.Taming)
                    );
                case CharacterArchetype.Bard:
                    return Weighted(
                        Skill(mobile, SkillName.Musicianship),
                        Skill(mobile, SkillName.Provocation),
                        Skill(mobile, SkillName.Discordance),
                        Skill(mobile, SkillName.Peacemaking)
                    );
                case CharacterArchetype.Thief:
                    return Weighted(
                        MaxOf(Skill(mobile, SkillName.Stealing), Skill(mobile, SkillName.Snooping)),
                        Skill(mobile, SkillName.Lockpicking),
                        Skill(mobile, SkillName.Hiding),
                        Skill(mobile, SkillName.Stealth),
                        Skill(mobile, SkillName.Begging),
                        Skill(mobile, SkillName.Searching),
                        Skill(mobile, SkillName.RemoveTrap)
                    );
                case CharacterArchetype.Jester:
                    return Clamp01(
                        0.65 * Skill(mobile, SkillName.Begging)
                            + 0.20
                                * AverageOf(
                                    Skill(mobile, SkillName.Psychology),
                                    Skill(mobile, SkillName.Hiding),
                                    Skill(mobile, SkillName.Stealth)
                                )
                            + 0.15 * IdentityPower(Server.Misc.GetPlayerInfo.isJester(mobile))
                    );
                case CharacterArchetype.Crafter:
                    return CraftPower(mobile);
                case CharacterArchetype.Gatherer:
                    return Weighted(
                        MaxOf(Skill(mobile, SkillName.Mining), Skill(mobile, SkillName.Lumberjacking)),
                        Skill(mobile, SkillName.Camping),
                        Skill(mobile, SkillName.Cartography)
                    );
                case CharacterArchetype.Merchant:
                    return Weighted(
                        Skill(mobile, SkillName.Mercantile),
                        Skill(mobile, SkillName.ArmsLore),
                        Skill(mobile, SkillName.Tasting)
                    );
                case CharacterArchetype.Seafarer:
                    return Weighted(
                        Skill(mobile, SkillName.Seafaring),
                        Skill(mobile, SkillName.Cartography),
                        Skill(mobile, SkillName.Camping)
                    );
                case CharacterArchetype.CreatureRace:
                    return CreatureRacePower(mobile);
                case CharacterArchetype.Alien:
                    return AlienPower(mobile);
                default:
                    return 0.0;
            }
        }

        private static double CraftPower(Mobile mobile)
        {
            double[] skills =
            {
                Skill(mobile, SkillName.Blacksmith),
                Skill(mobile, SkillName.Carpentry),
                Skill(mobile, SkillName.Tailoring),
                Skill(mobile, SkillName.Tinkering),
                Skill(mobile, SkillName.Bowcraft),
                Skill(mobile, SkillName.Inscribe),
                Skill(mobile, SkillName.Alchemy),
                Skill(mobile, SkillName.Cooking),
                Skill(mobile, SkillName.Tasting),
                Skill(mobile, SkillName.ArmsLore)
            };

            return Clamp01(0.60 * MaxOf(skills) + 0.40 * AverageOf(skills));
        }

        private static double CreatureRacePower(Mobile mobile)
        {
            if (mobile == null || mobile.RaceID <= 0)
                return 0.0;

            return Clamp01(0.25 + 0.75 * GetBestAdventurePower(mobile));
        }

        private static double AlienPower(Mobile mobile)
        {
            PlayerMobile player = mobile as PlayerMobile;

            if (player == null || player.SkillStart != 40000)
                return 0.0;

            return Clamp01(0.25 + 0.75 * GetSkillProgress(mobile));
        }

        private static int MaxLevel(Mobile mobile, params CharacterArchetype[] archetypes)
        {
            int best = 1;

            for (int i = 0; i < archetypes.Length; i++)
                best = Math.Max(best, GetArchetypeLevel(mobile, archetypes[i]));

            return best;
        }

        private static double Skill(Mobile mobile, SkillName skillName)
        {
            if (mobile == null || mobile.Skills == null)
                return 0.0;

            return Clamp01(mobile.Skills[skillName].Value / SkillAtFullArchetypeCredit);
        }

        private static double GetSkillProgress(Mobile mobile)
        {
            if (mobile == null || mobile.Skills == null)
                return 0.0;

            return Clamp01((double)mobile.Skills.Total / GetSkillCap(mobile));
        }

        private static double GetStatProgress(Mobile mobile)
        {
            if (mobile == null)
                return 0.0;

            return Clamp01((double)(mobile.RawStr + mobile.RawDex + mobile.RawInt) / GetStatCap(mobile));
        }

        private static double GetReputationProgress(Mobile mobile)
        {
            if (mobile == null)
                return 0.0;

            double fame = Math.Min(Math.Max(mobile.Fame, 0), 15000);
            double karma = Math.Min(Math.Abs(mobile.Karma), 15000);

            return Clamp01((fame + karma) / ReputationCap);
        }

        private static int GetSkillCap(Mobile mobile)
        {
            if (mobile == null || mobile.Skills == null)
                return 1;

            if (mobile.Skills.Cap > 0)
                return mobile.Skills.Cap;

            PlayerMobile player = mobile as PlayerMobile;

            if (player != null && player.SkillStart + player.SkillBoost + player.SkillEther > 0)
                return player.SkillStart + player.SkillBoost + player.SkillEther;

            return 1;
        }

        private static int GetStatCap(Mobile mobile)
        {
            if (mobile == null || mobile.StatCap <= 0)
                return 225;

            return mobile.StatCap;
        }

        private static double PositiveKarmaPower(Mobile mobile)
        {
            if (mobile == null || mobile.Karma <= 0)
                return 0.0;

            return Clamp01((double)mobile.Karma / 15000.0);
        }

        private static double NegativeKarmaPower(Mobile mobile)
        {
            if (mobile == null || mobile.Karma >= 0)
                return 0.0;

            return Clamp01((double)Math.Abs(mobile.Karma) / 15000.0);
        }

        private static double IdentityPower(bool hasIdentity)
        {
            return hasIdentity ? 1.0 : 0.0;
        }

        private static double Weighted(double primary, params double[] supports)
        {
            return Clamp01(0.70 * primary + 0.30 * AverageOf(supports));
        }

        private static double RequiredPair(double first, double second, params double[] supports)
        {
            return Clamp01(
                0.55 * Math.Min(first, second)
                    + 0.25 * Math.Max(first, second)
                    + 0.20 * AverageOf(supports)
            );
        }

        private static double AverageOf(params double[] values)
        {
            if (values == null || values.Length == 0)
                return 0.0;

            double total = 0.0;

            for (int i = 0; i < values.Length; i++)
                total += values[i];

            return total / values.Length;
        }

        private static double MaxOf(params double[] values)
        {
            if (values == null || values.Length == 0)
                return 0.0;

            double best = values[0];

            for (int i = 1; i < values.Length; i++)
                best = Math.Max(best, values[i]);

            return best;
        }

        private static int PowerToLevel(double power)
        {
            return ClampLevel((int)Math.Round(1.0 + 99.0 * Clamp01(power), MidpointRounding.AwayFromZero));
        }

        private static int ClampLevel(int level)
        {
            if (level < 1)
                return 1;

            if (level > 100)
                return 100;

            return level;
        }

        private static double Clamp01(double value)
        {
            if (value < 0.0)
                return 0.0;

            if (value > 1.0)
                return 1.0;

            return value;
        }
    }
}
