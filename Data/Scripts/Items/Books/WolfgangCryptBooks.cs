using System;

namespace Server.Items
{
    public class DiosScroll : BlueBook
    {
        [Constructable]
        public DiosScroll()
            : base("Dio's Scroll", "Dio", 40, false)
        {
            ItemID = 8792;
            Hue = 2007;
            Movable = false;
            Pages[0] = new BookPageInfo(
                "Each of the three guardia",
                "ns holds a clue to unlock",
                " the fourth. Speak it upo",
                "n the lonely tile to meet",
                " Wolfgang's final challenge",
                ". Beyond it, I have left t",
                "he first of three keys in",
                "to my tower."
            );
        }

        public DiosScroll(Serial serial)
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

    public class WolfgangsTome : BlueBook
    {
        [Constructable]
        public WolfgangsTome()
            : base("Wolfgang's Tome", "Wolfgang", 40, false)
        {
            ItemID = 8792;
            Hue = 2007;
            Movable = false;
            Pages[0] = new BookPageInfo(
                "In this tomb are four tr",
                "easures with four magic ",
                "words. Speak them upon t",
                "he blue star tiles to ent",
                "er each treasure room. B",
                "ut beware, each room is ",
                "more dangerous than the ",
                "last."
            );
        }

        public WolfgangsTome(Serial serial)
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
