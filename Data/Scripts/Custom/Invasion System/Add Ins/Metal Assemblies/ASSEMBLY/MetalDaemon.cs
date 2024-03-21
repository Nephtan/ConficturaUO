using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a metal daemon corpse")]
    public class MetalDaemon : BaseAssembly
    {
        [Constructable]
        public MetalDaemon()
            : base()
        {
            Body = 102;
            BaseSoundID = 357;
            Name = "a metal daemon";
            Hue = OreInfo.Verite.Hue;
            SetHits(1000, 1100);

            SetDamage(80, 90);
            SetStr(500, 500);
            SetDex(80, 80);
            SetInt(150, 150);
            SetSkill(SkillName.Anatomy, 90, 90);
            SetSkill(SkillName.Swords, 90, 90);
            SetSkill(SkillName.Tactics, 90, 90);
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
                    PackItem(new DaemonPowerCore());
                    break;
            }
            switch (Utility.Random(6))
            {
                case 0:
                    PackItem(new DaemonPowerCore());
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
                case 5:
                    PackItem(new DaemonBone(25));
                    break;
            }
            switch (Utility.Random(7))
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
                case 4:
                    PackItem(new DaemonBone(25));
                    break;
            }

            VirtualArmor = 58;
            SetFameLevel(1);
            SetKarmaLevel(1);

            CrescentBlade weapon = new CrescentBlade();
            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Low;
            AddItem(weapon);
        }

        public MetalDaemon(Serial serial)
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
