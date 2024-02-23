using System;
using Server.Gumps;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
    public class ArchmageSpellbook : Spellbook
    {
        public Mobile owner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public override SpellbookType SpellbookType
        {
            get { return SpellbookType.Archmage; }
        }
        public override int BookOffset
        {
            get { return 600; }
        }
        public override int BookCount
        {
            get { return 64; }
        }

        [Constructable]
        public ArchmageSpellbook()
            : this((ulong)0) { }

        [Constructable]
        public ArchmageSpellbook(ulong content)
            : base(content, 0x65EC)
        {
            Layer = Layer.Talisman;
            Name = "archmage spellbook";
        }

        public override void OnDoubleClick(Mobile from)
        {
            Container pack = from.Backpack;

            if (owner != from)
            {
                from.SendMessage("These pages appears as scribbles to you.");
            }
            else if (Parent == from || (pack != null && Parent == pack))
            {
                from.SendSound(0x55);
                from.CloseGump(typeof(ArchmageSpellbookGump));
                from.SendGump(new ArchmageSpellbookGump(from, this, 1));
            }
            else
                from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (owner != null)
            {
                list.Add(1070722, "For " + owner.Name + "");
            }
        }

        public ArchmageSpellbook(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Mobile)owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            owner = reader.ReadMobile();
        }
    }
}
