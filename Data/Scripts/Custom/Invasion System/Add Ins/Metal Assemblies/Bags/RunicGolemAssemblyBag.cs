using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class RunicGolemInvaderAssemblyBag : Bag
    {
        [Constructable]
        public RunicGolemInvaderAssemblyBag()
            : this(1)
        {
            Movable = true;
            //Hue = 0x386;
            Name = "Runic Golem Invader Assembly Bag";
        }

        [Constructable]
        public RunicGolemInvaderAssemblyBag(int amount)
        {
            DropItem(new RunicGolemInvaderPowerCore());
            DropItem(new IronIngot(150));
            DropItem(new BronzeIngot(75));
            DropItem(new Gears(25));
            //DropItem( new DaemonBone(50) );
            DropItem(new RunicClockworkAssembly());
            DropItem(new AssemblyUpgradeKit());
        }

        public RunicGolemInvaderAssemblyBag(Serial serial)
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
