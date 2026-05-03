using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    public enum AITacticalTargetProfile
    {
        None,
        Bruiser,
        Skirmisher,
        Captain,
        Support
    }

    public enum AITacticalReacquireReason
    {
        DamagedByNewAttacker,
        DamagedBySpell,
        HitByNewMeleeAttacker
    }

    public static class AITacticalTargeting
    {
        private static readonly TimeSpan m_DefaultMovementDecisionCadence = TimeSpan.FromSeconds(1.0);
        private static readonly TimeSpan m_SkirmisherMovementDecisionCadence = TimeSpan.FromSeconds(
            0.5
        );

        // Phase 3 is an exact-type whitelist. Default deny keeps later content additions,
        // service actors sharing stock shells, and shell-switching families out until audited.
        private static readonly Dictionary<Type, AITacticalTargetProfile> m_PhaseThreeProfiles =
            new Dictionary<Type, AITacticalTargetProfile>
            {
                { typeof(HeadlessOne), AITacticalTargetProfile.Bruiser },
                { typeof(Ratman), AITacticalTargetProfile.Bruiser },
                { typeof(Lizardman), AITacticalTargetProfile.Bruiser },
                { typeof(RatmanArcher), AITacticalTargetProfile.Skirmisher },
                { typeof(LizardmanArcher), AITacticalTargetProfile.Skirmisher }
            };

        public static AITacticalTargetProfile ResolveProfile(BaseCreature mobile)
        {
            if (
                mobile == null
                || mobile.Deleted
                || mobile.Controlled
                || mobile.Summoned
                || mobile.AIObject == null
                || mobile.FightMode != FightMode.Closest
            )
            {
                return AITacticalTargetProfile.None;
            }

            if (mobile.AI != AIType.AI_Melee && mobile.AI != AIType.AI_Archer)
            {
                return AITacticalTargetProfile.None;
            }

            AITacticalTargetProfile profile;

            if (m_PhaseThreeProfiles.TryGetValue(mobile.GetType(), out profile))
            {
                return profile;
            }

            return AITacticalTargetProfile.None;
        }

        // Phase 4 keeps cadence opt-in and whitelist-bound so stock timing remains unchanged
        // outside the active tactical cohort.
        public static TimeSpan GetMovementDecisionCadence(BaseCreature mobile)
        {
            switch (ResolveProfile(mobile))
            {
                case AITacticalTargetProfile.Skirmisher:
                    return m_SkirmisherMovementDecisionCadence;
                case AITacticalTargetProfile.Bruiser:
                case AITacticalTargetProfile.Captain:
                case AITacticalTargetProfile.Support:
                case AITacticalTargetProfile.None:
                default:
                    return m_DefaultMovementDecisionCadence;
            }
        }

        // Phase 4 only overrides combat spacing for whitelisted skirmishers.
        public static bool TryGetCombatSpacingBand(
            BaseCreature mobile,
            out int minimumRange,
            out int maximumRange
        )
        {
            minimumRange = 0;
            maximumRange = 0;

            if (ResolveProfile(mobile) != AITacticalTargetProfile.Skirmisher)
            {
                return false;
            }

            int weaponMaxRange = mobile != null && mobile.Weapon != null ? mobile.Weapon.MaxRange : 0;

            minimumRange = 3;
            maximumRange = Math.Min(6, Math.Max(3, weaponMaxRange - 1));

            return true;
        }

        public static double GetTargetBonus(
            BaseCreature self,
            Mobile target,
            FightMode acqType,
            bool bPlayerOnly
        )
        {
            return GetTargetBonus(
                self,
                target,
                acqType,
                bPlayerOnly,
                ResolveProfile(self),
                -1
            );
        }

        internal static double GetTargetBonus(
            BaseCreature self,
            Mobile target,
            FightMode acqType,
            bool bPlayerOnly,
            AITacticalTargetProfile profile,
            int nearbyTeamSize
        )
        {
            if (
                self == null
                || target == null
                || profile == AITacticalTargetProfile.None
                || acqType != FightMode.Closest
            )
            {
                return 0.0;
            }

            double bonus = 0.0;
            bool isCasting = target.Spell != null && target.Spell.IsCasting;
            double healthRatio =
                target.HitsMax > 0 ? target.Hits / (double)target.HitsMax : 1.0;

            if (profile != AITacticalTargetProfile.Support && target == self.Combatant)
            {
                bonus += 1.0;
            }

            switch (profile)
            {
                case AITacticalTargetProfile.Bruiser:
                    if (self.InRange(target, self.RangeFight + 1))
                    {
                        bonus += 2.0;
                    }

                    break;
                case AITacticalTargetProfile.Skirmisher:
                    if (isCasting)
                    {
                        bonus += 3.0;
                    }

                    if (healthRatio <= 0.35)
                    {
                        bonus += 2.0;
                    }

                    break;
                case AITacticalTargetProfile.Captain:
                    if (isCasting)
                    {
                        bonus += 2.0;
                    }

                    if (healthRatio <= 0.50)
                    {
                        bonus += 1.5;
                    }

                    if (nearbyTeamSize >= 2)
                    {
                        bonus += 1.0;
                    }

                    break;
                case AITacticalTargetProfile.Support:
                default:
                    break;
            }

            return Math.Max(0.0, Math.Min(4.0, bonus));
        }
    }
}
