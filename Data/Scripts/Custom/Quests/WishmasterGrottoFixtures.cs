using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    // Decoration pairs adjacent doors of the same type. Each cursed leaf opens on its own.
    public class WishmasterCursedDoor : MetalDoor
    {
        [Constructable]
        public WishmasterCursedDoor(DoorFacing facing)
            : base(facing)
        {
        }

        public WishmasterCursedDoor(Serial serial)
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

    public class WishmasterPoisonTrap : GasTrap
    {
        [Constructable]
        public WishmasterPoisonTrap()
            : base(GasTrapType.Floor, Poison.Regular)
        {
        }

        public WishmasterPoisonTrap(Serial serial)
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

    public class WishmasterLethalTrap : GasTrap
    {
        [Constructable]
        public WishmasterLethalTrap()
            : base(GasTrapType.Floor, Poison.Lethal)
        {
        }

        public WishmasterLethalTrap(Serial serial)
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

    public class WishmasterPaladinNote : SimpleNote
    {
        [Constructable]
        public WishmasterPaladinNote()
        {
            Name = "Mysterious Note";
            TitleString = "Note from a Mighty Paladin";
            NoteString = "I found it! The Wishmaster's cave. He won't get the better of me! I am too powerful for him.";
        }

        public WishmasterPaladinNote(Serial serial)
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

    public class WishmasterRangerNote : SimpleNote
    {
        [Constructable]
        public WishmasterRangerNote()
        {
            Name = "Mysterious Note";
            TitleString = "Note from a Crafty Ranger";
            NoteString = "I heard to only wish for something simple, but I can outsmart this Wishmaster. My quest depends on it!";
        }

        public WishmasterRangerNote(Serial serial)
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
