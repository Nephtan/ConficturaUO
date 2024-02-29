using System;
using Server.Gumps;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
    public class AncientSpellbook : Spellbook
    {
        public Mobile owner;
        public string names;
        public int paper;
        public int quill;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Paper
        {
            get { return paper; }
            set { paper = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quill
        {
            get { return quill; }
            set { quill = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Names
        {
            get { return names; }
            set { names = value; }
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
        public AncientSpellbook()
            : this((ulong)0) { }

        [Constructable]
        public AncientSpellbook(ulong content)
            : base(content, 0x65EC)
        {
            Layer = Layer.Talisman;
            Name = "ancient spellbook";
            Weight = 3.0;
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
                from.CloseGump(typeof(AncientSpellbookGump));
                from.SendGump(new AncientSpellbookGump(from, this, 1));
            }
            else
                from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (owner != null && owner.Name != names)
            {
                list.Add(1070722, "Belongs to " + owner.Name + "");
            }
        }

        public AncientSpellbook(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Mobile)owner);
            writer.Write(paper);
            writer.Write(quill);
            writer.Write(names);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            owner = reader.ReadMobile();
            paper = reader.ReadInt();
            quill = reader.ReadInt();
            names = reader.ReadString();
        }
    }
}
