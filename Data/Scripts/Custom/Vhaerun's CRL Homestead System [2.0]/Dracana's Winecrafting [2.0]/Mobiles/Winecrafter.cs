using System;
using System.Collections.Generic;
using Server;

namespace Server.Mobiles
{
    public class Winecrafter : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        [Constructable]
        public Winecrafter()
            : base("the Winecrafter")
        {
            SetSkill(SkillName.Alchemy, 100.0, 120.0);
            SetSkill(SkillName.Tasting, 80.0, 100.0);
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWinecrafter());
        }

        public override VendorShoeType ShoeType
        {
            get { return VendorShoeType.Boots; }
        }

        public override int GetShoeHue()
        {
            return 0;
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new Server.Items.WideBrimHat(Utility.RandomNeutralHue()));
        }

        public Winecrafter(Serial serial)
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
