using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class OverseerAssemblyBag : Bag
    {
        [Constructable]
        public OverseerAssemblyBag()
            : this(1)
        {
            Movable = true;
            //Hue = 0x386;
            Name = "Overseer Assembly Bag";
        }

        [Constructable]
        public OverseerAssemblyBag(int amount)
        {
            DropItem(new OverseerPowerCore());
            DropItem(new IronIngot(50));
            DropItem(new AgapiteIngot(100));
            DropItem(new Gears(10));
            //DropItem( new DaemonBone(50) );
            DropItem(new RunicClockworkAssembly());
            DropItem(new AssemblyUpgradeKit());
        }

        public OverseerAssemblyBag(Serial serial)
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
