using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    // Decoration links adjacent doors of the same type. The saved Ordain leaves operate independently.
    public class OrdainRuinsSideIronGate : IronGate
    {
        [Constructable]
        public OrdainRuinsSideIronGate(DoorFacing facing)
            : base(facing)
        {
        }

        public OrdainRuinsSideIronGate(Serial serial)
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

    public class OrdainRuinsSideMediumWoodDoor : MediumWoodDoor
    {
        [Constructable]
        public OrdainRuinsSideMediumWoodDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public OrdainRuinsSideMediumWoodDoor(Serial serial)
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

    public class OrdainRuinsSideStrongWoodDoor : StrongWoodDoor
    {
        [Constructable]
        public OrdainRuinsSideStrongWoodDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public OrdainRuinsSideStrongWoodDoor(Serial serial)
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
