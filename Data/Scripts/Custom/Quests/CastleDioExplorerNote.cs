using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    public class CastleDioExplorerNote : SimpleNote
    {
        [Constructable]
        public CastleDioExplorerNote()
        {
            Name = "Mysterious Note";
            TitleString = "Note from a Cowardly Explorer";
            NoteString = "I heard them speak the magic word of escape, \"Egress\"";
        }

        public CastleDioExplorerNote(Serial serial)
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
