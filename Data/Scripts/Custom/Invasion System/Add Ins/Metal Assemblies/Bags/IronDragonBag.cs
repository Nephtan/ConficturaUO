using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class IronDragonAssemblyBag : Bag
    {
        [Constructable]
        public IronDragonAssemblyBag()
            : this(1)
        {
            Movable = true;
            //Hue = 0x386;
            Name = "Iron Dragon Assembly Bag";
        }

        [Constructable]
        public IronDragonAssemblyBag(int amount)
        {
            DropItem(new DragonPowerCore());
            DropItem(new IronIngot(50));
            DropItem(new ValoriteIngot(100));
            DropItem(new Gears(10));
            DropItem(new DragonsBlood(50));
            DropItem(new RunicClockworkAssembly());
            DropItem(new AssemblyUpgradeKit());
        }

        public IronDragonAssemblyBag(Serial serial)
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
