using System;
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
        public static AITacticalTargetProfile ResolveProfile(BaseCreature mobile)
        {
            return AITacticalTargetProfile.None;
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
