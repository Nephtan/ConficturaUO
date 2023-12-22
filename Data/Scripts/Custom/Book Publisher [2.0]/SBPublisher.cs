using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server.Engines.Publisher;
using Server.Items;

namespace Server.Mobiles
{
    public class SBPublisher : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBPublisher() { }

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
                //Add(new GenericBuyInfo(typeof(BlueBook), 10, 20, 0xFF2, 0x00));
                //Add(new GenericBuyInfo(typeof(TanBook), 10, 20, 0xFF0, 0x00));
                //Add(new GenericBuyInfo(typeof(RedBook), 10, 20, 0xFF1, 0x00));
                //Add(new GenericBuyInfo(typeof(BrownBook), 10, 20, 0xFEF, 0x00));
                string[] books = XmlBook.RandomBookIds(10);
                for (int i = 0; i < books.Length; i++)
                    this.Add(new BookBuyInfo(books[i]));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(BlankScroll), 1);
            }
        }
    }
}
