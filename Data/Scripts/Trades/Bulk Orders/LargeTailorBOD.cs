using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Mat = Server.Engines.BulkOrders.BulkMaterialType;

namespace Server.Engines.BulkOrders
{
    public class LargeTailorBOD : LargeBOD
    {
        public static double[] m_TailoringMaterialChances = new double[]
        {
            0.857421875, // None
            0.125000000, // Spined
            0.015625000, // Horned
            0.001953125 // Barbed
        };

        public override int ComputeFame()
        {
            return TailorRewardCalculator.Instance.ComputeFame(this);
        }

        public override int ComputeGold()
        {
            return TailorRewardCalculator.Instance.ComputeGold(this);
        }

        [Constructable]
        public LargeTailorBOD(Mobile m)
        {
            LargeBulkEntry[] entries;
            bool useMaterials = false;
            double theirSkill = 0.0;

            if (m != null)
            {
                theirSkill = m.Skills[SkillName.Tailoring].Base;
            }

            int entrySwitchRNG = 0;

            int min, max;
            if (theirSkill >= 120.0)
            {
                min = 11; 
                max = 12;
            }
            else if (theirSkill >= 100.1)
            {
                min = 10; 
                max = 12;
            }
            else if (theirSkill >= 90.1)
            {
                min = 8; 
                max = 12;
            }
            else if (theirSkill >= 80.1)
            {
                min = 5; 
                max = 10;
            }
            else
            {
                min = 0; 
                max = 9;
            }
            entrySwitchRNG = Utility.RandomMinMax(min, max);

            switch (entrySwitchRNG)
            {
                default:
                case 0:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.HatSet); // 4 piece
                    break;
                case 1:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.Farmer); // 4 piece
                    break;
                case 2:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.FisherGirl); // 4 piece
                    break;
                case 3:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.Gypsy); // 4 piece
                    break;
                case 4:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.Jester); // 4 piece
                    break;
                case 5:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.Lady); // 4 piece
                    break;
                case 6:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.Pirate); // 4 piece
                    break;
                case 7:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.Wizard); // 4 piece
                    break;
                case 8:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.ShoeSet); // 4 piece
                    useMaterials = Core.ML;
                    break;
                case 9:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.TownCrier); // 5 piece
                    break;
                case 10:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.StuddedSet); // 5 piece
                    useMaterials = true;
                    break;
                case 11:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.FemaleLeatherSet); // 6 piece
                    useMaterials = true;
                    break;
                case 12:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.MaleLeatherSet); // 6 piece
                    useMaterials = true;
                    break;
                //case 13:
                //    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.BoneSet);
                //    useMaterials = true;
                //    break;
            }

            int hue = 0x483;
            int amountMax = 0;

            if (theirSkill >= 120.0)
            {
                amountMax = Utility.RandomList(15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20);
            }
            else if (theirSkill >= 100.1)
            {
                amountMax = Utility.RandomList(15, 20, 20, 20);
            }
            else if (theirSkill >= 90.1)
            {
                amountMax = Utility.RandomList(15, 20);
            }
            else if (theirSkill >= 80.1)
            {
                amountMax = Utility.RandomList(10, 15, 15, 15, 15, 15, 20, 20, 20, 20);
            }
            else
            {
                amountMax = Utility.RandomList(10, 10, 10, 10, 10, 15, 15, 15, 20, 20);
            }

            double excChance = 0.0;

            if (theirSkill >= 90.1)
                excChance = (theirSkill + 80.0) / 200.0;
            else if (theirSkill >= 80.1)
                excChance = (theirSkill + 75.0) / 200.0;
            else
                excChance = (theirSkill + 70.0) / 200.0;

            bool reqExceptional = (excChance > Utility.RandomDouble());

            BulkMaterialType material;

            if (useMaterials)
                material = GetRandomMaterial(BulkMaterialType.Spined, m_TailoringMaterialChances);
            else
                material = BulkMaterialType.None;

            this.Hue = hue;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = material;
        }

        public LargeTailorBOD(
            int amountMax,
            bool reqExceptional,
            BulkMaterialType mat,
            LargeBulkEntry[] entries
        )
        {
            this.Hue = 0x483;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = mat;
        }

        public override List<Item> ComputeRewards(bool full)
        {
            List<Item> list = new List<Item>();

            RewardGroup rewardGroup = TailorRewardCalculator.Instance.LookupRewards(
                TailorRewardCalculator.Instance.ComputePoints(this)
            );

            if (rewardGroup != null)
            {
                if (full)
                {
                    for (int i = 0; i < rewardGroup.Items.Length; ++i)
                    {
                        Item item = rewardGroup.Items[i].Construct();

                        if (item != null)
                            list.Add(item);
                    }
                }
                else
                {
                    RewardItem rewardItem = rewardGroup.AcquireItem();

                    if (rewardItem != null)
                    {
                        Item item = rewardItem.Construct();

                        if (item != null)
                            list.Add(item);
                    }
                }
            }

            return list;
        }

        public LargeTailorBOD(Serial serial)
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
