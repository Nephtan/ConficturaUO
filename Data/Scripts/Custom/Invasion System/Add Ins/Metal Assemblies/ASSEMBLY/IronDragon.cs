using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an iron dragon corpse")]
    public class IronDragon : BaseAssembly
    {
        [Constructable]
        public IronDragon()
            : base()
        {
            Body = 46;
            BaseSoundID = 711;
            Name = "an iron dragon";
            Hue = OreInfo.Valorite.Hue;
            SetHits(1200, 1300);

            SetDamage(90, 95);
            SetStr(900, 1000);
            SetDex(100, 100);
            SetInt(100, 100);
            SetSkill(SkillName.Anatomy, 95, 95);
            SetSkill(SkillName.Bludgeoning, 95, 95);
            SetSkill(SkillName.Tactics, 95, 95);
            SetSkill(SkillName.MagicResist, 200, 200);
            MF_RobotRevealer = true;
            MF_Displacer = true;
            VirtualArmor = 68;
            SetFameLevel(1);
            SetKarmaLevel(1);

            Maul weapon = new Maul();
            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Low;
            AddItem(weapon);
            switch (Utility.Random(8))
            {
                case 0:
                    PackItem(new Gears(5));
                    break;
                case 1:
                    PackItem(new DragonsBlood(50));
                    break;
                case 2:
                    PackItem(new RunicClockworkAssembly());
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
                    PackItem(new DragonPowerCore());
                    break;
                case 1:
                    PackItem(new DragonsBlood(50));
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
                case 5:
                    PackItem(new ValoriteIngot(50));
                    break;
            }
            switch (Utility.Random(8))
            {
                case 0:
                    PackItem(new ValoriteIngot(50));
                    break;
                case 1:
                    PackItem(new DragonsBlood(50));
                    break;
                case 2:
                    PackItem(new RunicClockworkAssembly());
                    break;
                case 3:
                    PackItem(new Gears(10));
                    break;
                case 4:
                    PackItem(new IronIngot(50));
                    break;
            }
        }

        public IronDragon(Serial serial)
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
