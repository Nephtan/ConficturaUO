using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class CityManager : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.TinkersGuild; }
        }

        [Constructable]
        public CityManager()
            : base("the city manager") { }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCityManager());
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            // Define the mapping of item types to their sell-back value
            var itemValueMap = new Dictionary<Type, int>
            {
                { typeof(AsianCityHallDeed), 250000 },
                { typeof(NecroCityHallDeed), 250000 },
                { typeof(StoneCityHallDeed), 250000 },
                { typeof(FieldStoneCityHallDeed), 250000 },
                { typeof(PlasterCityHallDeed), 250000 },
                { typeof(WoodCityHallDeed), 250000 },
                { typeof(MarbleCityHallDeed), 250000 },
                { typeof(SandstoneCityHallDeed), 250000 },

                { typeof(AsianCityBankDeed), 50000 },
                { typeof(NecroCityBankDeed), 50000 },
                { typeof(StoneCityBankDeed), 50000 },
                { typeof(FieldstoneCityBankDeed), 50000 },
                { typeof(PlasterCityBankDeed), 50000 },
                { typeof(WoodCityBankDeed), 50000 },
                { typeof(MarbleCityBankDeed), 50000 },
                { typeof(SandstoneCityBankDeed), 50000 },

                { typeof(AsianCityHealerDeed), 55000 },
                { typeof(NecroCityHealerDeed), 55000 },
                { typeof(StoneCityHealerDeed), 55000 },
                { typeof(FieldstoneCityHealerDeed), 55000 },
                { typeof(PlasterCityHealerDeed), 55000 },
                { typeof(WoodCityHealerDeed), 55000 },
                { typeof(MarbleCityHealerDeed), 55000 },
                { typeof(SandstoneCityHealerDeed), 55000 },

                { typeof(AsianCityMoongateDeed), 25000 },
                { typeof(NecroCityMoongateDeed), 25000 },
                { typeof(MarbleCityMoongateDeed), 25000 },
                { typeof(SandstoneCityMoongateDeed), 25000 },
                { typeof(StoneCityMoongateDeed), 25000 },
                { typeof(WoodCityMoongateDeed), 25000 },

                { typeof(AsianCityStableDeed), 55000 },
                { typeof(NecroCityStableDeed), 55000 },
                { typeof(StoneCityStableDeed), 55000 },
                { typeof(FieldstoneCityStableDeed), 55000 },
                { typeof(PlasterCityStableDeed), 55000 },
                { typeof(WoodCityStableDeed), 55000 },
                { typeof(MarbleCityStableDeed), 55000 },
                { typeof(SandstoneCityStableDeed), 55000 },

                { typeof(AsianCityTavernDeed), 70000 },
                { typeof(NecroCityTavernDeed), 70000 },
                { typeof(StoneCityTavernDeed), 70000 },
                { typeof(FieldstoneCityTavernDeed), 70000 },
                { typeof(PlasterCityTavernDeed), 70000 },
                { typeof(WoodCityTavernDeed), 70000 },
                { typeof(MarbleCityTavernDeed), 70000 },
                { typeof(SandstoneCityTavernDeed), 70000 },

                { typeof(SmallCityParkDeed), 20000 },
                { typeof(MediumCityParkDeed), 35000 },
                { typeof(LargeCityParkDeed), 50000 },

                { typeof(SmallCityGardenDeed), 20000 },
                { typeof(MediumCityGardenDeed), 35000 },
                { typeof(LargeCityGardenDeed), 50000 },

                { typeof(CityContractOfEmployment), 1000 },
                { typeof(CityResourceBoxDeed), 5000 },

                { typeof(AsianCityMarketDeed), 50000 },
                { typeof(NecroCityMarketDeed), 50000 },
                { typeof(StoneCityMarketDeed), 50000 },
                { typeof(FieldStoneCityMarketDeed), 50000 },
                { typeof(PlasterCityMarketDeed), 50000 },
                { typeof(WoodCityMarketDeed), 50000 },
                { typeof(MarbleCityMarketDeed), 50000 },
                { typeof(SandstoneCityMarketDeed), 50000 }
            };

            if (item != null && itemValueMap.ContainsKey(item.GetType()))
            {
                int value = itemValueMap[item.GetType()];

                if (value <= 1000)
                {
                    from.AddToBackpack(new Gold(value));
                    from.SendMessage("{0} gold has been added to your pack.", value);
                }
                else
                {
                    // Issue a BankCheck if the value is 1000 or more
                    BankCheck check = new BankCheck(value);

                    if (from.Backpack != null && from.Backpack.TryDropItem(from, check, false))
                    {
                        from.SendMessage("A bank check for {0} gold has been placed in your pack.", value);
                    }
                    else if (from.BankBox != null && from.BankBox.TryDropItem(from, check, false))
                    {
                        from.SendMessage("Your backpack is full. A bank check for {0} gold has been placed in your bank box.", value);
                    }
                    else
                    {
                        // Fallback in case both backpack and bank box are somehow inaccessible
                        check.Delete(); // Important to avoid creating orphaned items in the world
                        from.SendMessage("Unable to add the bank check to your backpack or bank. Please ensure you have space and try again.");
                        return false;
                    }
                }

                item.Delete(); // Remove the item being sold back
                return true; // Indicate successful handling of the drag-and-drop operation
            }

            // For items not recognized by the CityManager, fallback to the default handling
            return base.OnDragDrop(from, item);
        }

        public CityManager(Serial serial)
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
