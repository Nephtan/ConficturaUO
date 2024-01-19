using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MechanicalGargoyleAssemblyBag : Bag
    {
        [Constructable]
        public MechanicalGargoyleAssemblyBag()
            : this(1)
        {
            Movable = true;
            //Hue = 0x386;
            Name = "Mechanical Gargoyle Assembly Bag";
        }

        [Constructable]
        public MechanicalGargoyleAssemblyBag(int amount)
        {
            DropItem(new GargoylePowerCore());
            DropItem(new IronIngot(50));
            DropItem(new GoldIngot(75));
            DropItem(new Gears(10));
            //DropItem( new DaemonBone(50) );
            DropItem(new RunicClockworkAssembly());
            DropItem(new AssemblyUpgradeKit());
        }

        public MechanicalGargoyleAssemblyBag(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
