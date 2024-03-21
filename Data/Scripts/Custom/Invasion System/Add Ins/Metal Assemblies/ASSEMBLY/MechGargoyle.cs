using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a mechnical gargoyle corpse")]
    public class MechGargoyle : BaseAssembly
    {
        [Constructable]
        public MechGargoyle()
            : base()
        {
            Body = 755;
            BaseSoundID = 372;
            Name = "a mechnical gargoyle";
            Hue = OreInfo.Gold.Hue;
            SetHits(500, 600);

            SetDamage(50, 60);
            SetStr(200, 250);
            SetDex(45, 45);
            SetInt(45, 45);
            SetSkill(SkillName.Anatomy, 71, 71);
            SetSkill(SkillName.Swords, 71, 71);
            SetSkill(SkillName.Tactics, 77, 77);
            SetSkill(SkillName.MagicResist, 200, 200);

            MF_RobotRevealer = true;
            MF_Displacer = true;

            switch (Utility.Random(6))
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
            }
            switch (Utility.Random(5))
            {
                case 0:
                    PackItem(new GargoylePowerCore());
                    break;
                case 1:
                    PackItem(new GoldIngot(50));
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
                    PackItem(new GoldIngot(25));
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

            VirtualArmor = 28;
            SetFameLevel(1);
            SetKarmaLevel(1);

            VikingSword weapon = new VikingSword();
            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Low;
            AddItem(weapon);
        }

        public MechGargoyle(Serial serial)
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
