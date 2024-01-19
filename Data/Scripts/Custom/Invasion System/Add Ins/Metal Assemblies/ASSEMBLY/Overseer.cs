using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an overseer corpse")]
    public class Overseer : BaseAssembly
    {
        [Constructable]
        public Overseer()
            : base()
        {
            Body = 756;
            BaseSoundID = 372;
            Name = "an overseer";
            Hue = OreInfo.Agapite.Hue;
            SetHits(300, 400);

            SetDamage(24, 26);
            SetStr(350, 350);
            SetDex(55, 55);
            SetInt(65, 65);
            SetSkill(SkillName.Anatomy, 81, 81);
            SetSkill(SkillName.Swords, 81, 81);
            SetSkill(SkillName.Tactics, 87, 87);
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
                    PackItem(new OverseerPowerCore());
                    break;
            }
            switch (Utility.Random(5))
            {
                case 0:
                    PackItem(new OverseerPowerCore());
                    break;
                case 1:
                    PackItem(new AgapiteIngot(50));
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
                    PackItem(new AgapiteIngot(25));
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

            VirtualArmor = 38;
            SetFameLevel(1);
            SetKarmaLevel(1);

            VikingSword weapon = new VikingSword();
            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Low;
            AddItem(weapon);
        }

        public Overseer(Serial serial)
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
