using System;
using Server;

namespace Server.Items
{
    public class EarthStrike : WeaponAbility
    {
        public EarthStrike() { }

        public override int BaseMana
        {
            get { return 20; }
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!Validate(attacker) || !CheckMana(attacker, true))
                return;
            ClearCurrentAbility(attacker);
            attacker.SendMessage("You strike them with extreme physical force!");
            defender.SendMessage("You where struck with extreme physical force!");

            BaseWeapon weapon = attacker.Weapon as BaseWeapon;
            if (weapon == null)
                return;
            Skill skill = attacker.Skills[weapon.Skill];
            double reqSkill = GetRequiredSkill(attacker);
            double skilltouse = 0.0;
            if (skill != null)
                skilltouse = skill.Value;
            if (weapon.WeaponAttributes.UseBestSkill > 0)
            {
                double skilltouse2 = 0.0;
                if (attacker.Skills[SkillName.Swords].Value >= reqSkill)
                    skilltouse2 = attacker.Skills[SkillName.Swords].Value;
                if (
                    attacker.Skills[SkillName.Bludgeoning].Value >= reqSkill
                    && attacker.Skills[SkillName.Bludgeoning].Value > skilltouse2
                )
                    skilltouse2 = attacker.Skills[SkillName.Bludgeoning].Value;
                if (
                    attacker.Skills[SkillName.Fencing].Value >= reqSkill
                    && attacker.Skills[SkillName.Fencing].Value > skilltouse2
                )
                    skilltouse2 = attacker.Skills[SkillName.Fencing].Value;
                if (
                    attacker.Skills[SkillName.Lumberjacking].Value >= reqSkill
                    && attacker.Skills[SkillName.Lumberjacking].Value > skilltouse2
                )
                    skilltouse2 = attacker.Skills[SkillName.Lumberjacking].Value;
                if (skilltouse2 > skilltouse)
                    skilltouse = skilltouse2;
            }
            int todam = (int)(skilltouse / 20);
            AOS.Damage(
                defender,
                attacker,
                (Utility.RandomMinMax(15, 35) + todam),
                true,
                100,
                0,
                0,
                0,
                0
            );
            base.OnHit(attacker, defender, damage);
        }
    }
}
