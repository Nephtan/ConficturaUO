using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    // Use one alternate leaf beside its base type. Decoration links adjacent doors of the same type.
    public class AlternateDecorationMetalDoor : MetalDoor
    {
        [Constructable]
        public AlternateDecorationMetalDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public AlternateDecorationMetalDoor(Serial serial)
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

    public class AlternateDecorationStrongWoodDoor : StrongWoodDoor
    {
        [Constructable]
        public AlternateDecorationStrongWoodDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public AlternateDecorationStrongWoodDoor(Serial serial)
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

    public class AlternateDecorationMediumWoodDoor : MediumWoodDoor
    {
        [Constructable]
        public AlternateDecorationMediumWoodDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public AlternateDecorationMediumWoodDoor(Serial serial)
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

    public class AlternateDecorationSecretDungeonDoor : SecretDungeonDoor
    {
        [Constructable]
        public AlternateDecorationSecretDungeonDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public AlternateDecorationSecretDungeonDoor(Serial serial)
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
