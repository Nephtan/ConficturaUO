// Necro Regs by Dougan Ironfist

using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DragonsBlood : BaseReagent, ICommodity
    {
        int ICommodity.DescriptionNumber
        {
            get { return LabelNumber; }
        }
        bool ICommodity.IsDeedable
        {
            get { return false; }
        }

        /*string ICommodity.Description
        {
            get
            {
                return String.Format( "{0} Dragon's Blood", Amount );
            }
        }*/

        [Constructable]
        public DragonsBlood()
            : this(1) { }

        [Constructable]
        public DragonsBlood(int amount)
            : base(0xF82, amount) { }

        public DragonsBlood(Serial serial)
            : base(serial) { }

        /*public override Item Dupe( int amount )
        {
            return base.Dupe( new DragonsBlood( amount ), amount );
        }*/

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
