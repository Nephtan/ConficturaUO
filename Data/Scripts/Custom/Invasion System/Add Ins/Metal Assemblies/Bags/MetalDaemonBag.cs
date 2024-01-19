using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class IronDaemonAssemblyBag : Bag
    {
        [Constructable]
        public IronDaemonAssemblyBag()
            : this(1)
        {
            Movable = true;
            //Hue = 0x386;
            Name = "Iron Dragon Assembly Bag";
        }

        [Constructable]
        public IronDaemonAssemblyBag(int amount)
        {
            DropItem(new DaemonPowerCore());
            DropItem(new IronIngot(50));
            DropItem(new VeriteIngot(100));
            DropItem(new Gears(10));
            DropItem(new DaemonBone(50));
            DropItem(new RunicClockworkAssembly());
            DropItem(new AssemblyUpgradeKit());
        }

        public IronDaemonAssemblyBag(Serial serial)
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
