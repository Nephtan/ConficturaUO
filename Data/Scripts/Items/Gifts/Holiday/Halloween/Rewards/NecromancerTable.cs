using System;
using Server.Network;
using Server.Prompts;

namespace Server.Items
{
    [Furniture]
    [Flipable(0x149D, 0x149E)]
    public class NecromancerTable : Item
    {
        [Constructable]
        public NecromancerTable()
            : base(0x149D)
        {
            Weight = 20.0;
            Name = "necromancer table";
        }

        public NecromancerTable(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
