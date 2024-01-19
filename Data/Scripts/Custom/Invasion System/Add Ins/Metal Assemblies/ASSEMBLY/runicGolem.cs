using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a  runic golem invader corpse")]
    public class RunicGolemInvader : BaseAssembly
    {
        [Constructable]
        public RunicGolemInvader()
            : base()
        {
            Body = 752;
            BaseSoundID = 268;
            Name = "a Runic Golem Invader";
            Hue = OreInfo.Bronze.Hue;
            SetHits(600, 700);

            SetDamage(60, 70);
            SetStr(500, 600);
            SetDex(75, 75);
            SetInt(75, 75);
            SetSkill(SkillName.Anatomy, 91, 91);
            SetSkill(SkillName.Bludgeoning, 91, 91);
            SetSkill(SkillName.Tactics, 97, 97);
            SetSkill(SkillName.MagicResist, 200, 200);

            MF_RobotRevealer = true;
            MF_Displacer = true;

            switch (Utility.Random(8))
            {
                case 0:
                    PackItem(new Gears(5));
                    break;
                case 1:
                    PackItem(new RunicClockworkAssembly());
                    break;
                case 2:
                    PackItem(new AssemblyUpgradeKit());
                    break;
                case 3:
                    PackItem(new IronIngot(25));
                    break;
                case 4:
                    PackItem(new RunicGolemInvaderPowerCore());
                    break;
            }
            switch (Utility.Random(5))
            {
                case 0:
                    PackItem(new RunicGolemInvaderPowerCore());
                    break;
                case 1:
                    PackItem(new BronzeIngot(50));
                    break;
                case 2:
                    PackItem(new Gears(10));
                    break;
                case 3:
                    PackItem(new AssemblyUpgradeKit());
                    break;
                case 4:
                    PackItem(new IronIngot(25));
                    break;
            }
            switch (Utility.Random(6))
            {
                case 0:
                    PackItem(new BronzeIngot(25));
                    break;
                case 1:
                    PackItem(new RunicClockworkAssembly());
                    break;
                case 2:
                    PackItem(new Gears(10));
                    break;
                case 3:
                    PackItem(new IronIngot(50));
                    break;
            }
            VirtualArmor = 48;
            SetFameLevel(1);
            SetKarmaLevel(1);

            Club weapon = new Club();
            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Low;
            AddItem(weapon);
        }

        public RunicGolemInvader(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
