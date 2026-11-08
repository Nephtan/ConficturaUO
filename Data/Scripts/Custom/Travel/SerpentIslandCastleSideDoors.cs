using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    // Decoration links adjacent doors of the same type. These castle leaves operate independently.
    public class SerpentIslandCastleSideIronGate : IronGate
    {
        [Constructable]
        public SerpentIslandCastleSideIronGate(DoorFacing facing)
            : base(facing)
        {
        }

        public SerpentIslandCastleSideIronGate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SerpentIslandCastleSideStrongWoodDoor : StrongWoodDoor
    {
        [Constructable]
        public SerpentIslandCastleSideStrongWoodDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public SerpentIslandCastleSideStrongWoodDoor(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
