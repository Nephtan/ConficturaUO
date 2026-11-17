using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    // Decoration links adjacent doors of the same type. These manor leaves operate independently.
    public class MadDukesManorSideIronGate : IronGate
    {
        [Constructable]
        public MadDukesManorSideIronGate(DoorFacing facing)
            : base(facing)
        {
        }

        public MadDukesManorSideIronGate(Serial serial)
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

    public class MadDukesManorSideMetalDoor : MetalDoor
    {
        [Constructable]
        public MadDukesManorSideMetalDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public MadDukesManorSideMetalDoor(Serial serial)
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
