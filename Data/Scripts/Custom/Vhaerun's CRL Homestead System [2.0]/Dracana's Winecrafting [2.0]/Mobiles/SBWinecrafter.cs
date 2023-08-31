using System;
using System.Collections.Generic;
using System.Collections;
using Server.Engines.Apiculture;
using Server.Items;
using Server.Items.Crops;
using Server.Misc;
using Server.Multis;
using Server.Guilds;
using Server.Engines.Mahjong;
using Server.Spells.Elementalism;

namespace Server.Mobiles
{
    public class SBWinecrafter : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBWinecrafter() { }

        public override IShopSellInfo SellInfo
        {
            get { return m_SellInfo; }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get { return m_BuyInfo; }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(WinecraftersTools), 500, 20, 0xF00, 0x530));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(EmptyWineBottle), 5, 20, 0x99B, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Keg), 300, 20, 0xE7F, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(WinecrafterSugar), 3, 999, 0x1006, 1150));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(WinecrafterYeast), 3, 999, 0x1039, 0));
                }
                if (MyServerSettings.SellRareChance())
                {
                    Add(new GenericBuyInfo(typeof(VinyardLabelMaker), 2000, 20, 0xFC0, 0x218));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(GrapevinePlacementTool), 2000, 20, 0xD1A, 0x489));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(CabernetSauvignonGrapes), 2, 20, 0x9D1, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(ChardonnayGrapes), 2, 20, 0x9D1, 0x1CC));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(CheninBlancGrapes), 2, 20, 0x9D1, 0x16B));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(MerlotGrapes), 2, 20, 0x9D1, 0x2CE));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(PinotNoirGrapes), 2, 20, 0x9D1, 0x2CE));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(RieslingGrapes), 2, 20, 0x9D1, 0x1CC));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(SangioveseGrapes), 2, 20, 0x9D1, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(SauvignonBlancGrapes), 2, 20, 0x9D1, 0x16B));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(ShirazGrapes), 2, 20, 0x9D1, 0x2CE));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(ViognierGrapes), 2, 20, 0x9D1, 0x16B));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(ZinfandelGrapes), 2, 20, 0x9D1, 0));
                }
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(CabernetSauvignonGrapes), 1);
                Add(typeof(ChardonnayGrapes), 1);
                Add(typeof(CheninBlancGrapes), 1);
                Add(typeof(MerlotGrapes), 1);
                Add(typeof(PinotNoirGrapes), 1);
                Add(typeof(RieslingGrapes), 1);
                Add(typeof(SangioveseGrapes), 1);
                Add(typeof(SauvignonBlancGrapes), 1);
                Add(typeof(ShirazGrapes), 1);
                Add(typeof(ViognierGrapes), 1);
                Add(typeof(ZinfandelGrapes), 1);
            }
        }
    }
}
