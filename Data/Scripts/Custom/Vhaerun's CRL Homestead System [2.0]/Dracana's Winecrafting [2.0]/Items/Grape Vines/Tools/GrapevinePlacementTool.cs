//================================================//
// Created by dracana				  //
// Desc: Tool used by players to place grapevines //
//       in their houses.                         //
//================================================//
using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class GrapevinePlacementTool : Item
    {
        [Constructable]
        public GrapevinePlacementTool()
            : base(0xD1A)
        {
            Movable = true;
            Hue = 0x489;
            Name = "Grapevine Placement Tool";
        }

        public GrapevinePlacementTool(Serial serial)
            : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new AddGrapeVineGump(from, null, 0));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    goto case 0;
                }
                case 0:
                {
                    break;
                }
            }
        }
    }
}
