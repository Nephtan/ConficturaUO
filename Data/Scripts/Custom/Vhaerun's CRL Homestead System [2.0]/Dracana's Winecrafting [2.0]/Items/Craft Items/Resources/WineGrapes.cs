using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseWineGrapes : Item, ICommodity
    {
        private GrapeVariety m_Variety;

        [CommandProperty(AccessLevel.GameMaster)]
        public GrapeVariety Variety
        {
            get { return m_Variety; }
            set
            {
                m_Variety = value;
                InvalidateProperties();
            }
        }

        int ICommodity.DescriptionNumber
        {
            get { return LabelNumber; }
        }

        // Add the missing IsDeedable property
        bool ICommodity.IsDeedable
        {
            get { return true; }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Variety);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    m_Variety = (GrapeVariety)reader.ReadInt();
                    break;
                }
                case 0:
                {
                    WineGrapeInfo info;

                    switch (reader.ReadInt())
                    {
                        case 0:
                            info = WineGrapeInfo.CabernetSauvignon;
                            break;
                        case 1:
                            info = WineGrapeInfo.Chardonnay;
                            break;
                        case 2:
                            info = WineGrapeInfo.CheninBlanc;
                            break;
                        case 3:
                            info = WineGrapeInfo.Merlot;
                            break;
                        case 4:
                            info = WineGrapeInfo.PinotNoir;
                            break;
                        case 5:
                            info = WineGrapeInfo.Riesling;
                            break;
                        case 6:
                            info = WineGrapeInfo.Sangiovese;
                            break;
                        case 7:
                            info = WineGrapeInfo.SauvignonBlanc;
                            break;
                        case 8:
                            info = WineGrapeInfo.Shiraz;
                            break;
                        case 9:
                            info = WineGrapeInfo.Viognier;
                            break;
                        case 10:
                            info = WineGrapeInfo.Zinfandel;
                            break;
                        default:
                            info = null;
                            break;
                    }

                    m_Variety = WinemakingResources.GetFromWineGrapeInfo(info);
                    break;
                }
            }
        }

        public BaseWineGrapes(GrapeVariety variety)
            : this(variety, 1) { }

        public BaseWineGrapes(GrapeVariety variety, int amount)
            : base(0x9D1)
        {
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
            Hue = WinemakingResources.GetHue(variety);

            m_Variety = variety;
        }

        public BaseWineGrapes(Serial serial)
            : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Amount > 1)
                list.Add(
                    1050039,
                    "{0}\t{1}",
                    Amount,
                    "Bunches of " + WinemakingResources.GetName(m_Variety) + " Grapes"
                ); // ~1_NUMBER~ ~2_ITEMNAME~
            else
                list.Add("Bunch of " + WinemakingResources.GetName(m_Variety) + " Grapes");
        }

        public override void OnSingleClick(Mobile from)
        {
            //base.OnSingleClick( from );

            if (this.Amount > 1)
                this.LabelTo(
                    from,
                    "{0} Grape Bunches : {1}",
                    WinemakingResources.GetName(m_Variety),
                    Amount
                );
            else
                this.LabelTo(from, "{0} Grape Bunch", WinemakingResources.GetName(m_Variety));
        }
    }

    public class CabernetSauvignonGrapes : BaseWineGrapes
    {
        [Constructable]
        public CabernetSauvignonGrapes()
            : this(1) { }

        [Constructable]
        public CabernetSauvignonGrapes(int amount)
            : base(GrapeVariety.CabernetSauvignon, amount)
        {
            Name = "Cabernet Sauvignon Grapes";
        }

        public CabernetSauvignonGrapes(Serial serial)
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

    public class ChardonnayGrapes : BaseWineGrapes
    {
        [Constructable]
        public ChardonnayGrapes()
            : this(1) { }

        [Constructable]
        public ChardonnayGrapes(int amount)
            : base(GrapeVariety.Chardonnay, amount) { }

        public ChardonnayGrapes(Serial serial)
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

    public class CheninBlancGrapes : BaseWineGrapes
    {
        [Constructable]
        public CheninBlancGrapes()
            : this(1) { }

        [Constructable]
        public CheninBlancGrapes(int amount)
            : base(GrapeVariety.CheninBlanc, amount) { }

        public CheninBlancGrapes(Serial serial)
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

    public class MerlotGrapes : BaseWineGrapes
    {
        [Constructable]
        public MerlotGrapes()
            : this(1) { }

        [Constructable]
        public MerlotGrapes(int amount)
            : base(GrapeVariety.Merlot, amount) { }

        public MerlotGrapes(Serial serial)
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

    public class PinotNoirGrapes : BaseWineGrapes
    {
        [Constructable]
        public PinotNoirGrapes()
            : this(1) { }

        [Constructable]
        public PinotNoirGrapes(int amount)
            : base(GrapeVariety.PinotNoir, amount) { }

        public PinotNoirGrapes(Serial serial)
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

    public class RieslingGrapes : BaseWineGrapes
    {
        [Constructable]
        public RieslingGrapes()
            : this(1) { }

        [Constructable]
        public RieslingGrapes(int amount)
            : base(GrapeVariety.Riesling, amount) { }

        public RieslingGrapes(Serial serial)
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

    public class SangioveseGrapes : BaseWineGrapes
    {
        [Constructable]
        public SangioveseGrapes()
            : this(1) { }

        [Constructable]
        public SangioveseGrapes(int amount)
            : base(GrapeVariety.Sangiovese, amount) { }

        public SangioveseGrapes(Serial serial)
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

    public class SauvignonBlancGrapes : BaseWineGrapes
    {
        [Constructable]
        public SauvignonBlancGrapes()
            : this(1) { }

        [Constructable]
        public SauvignonBlancGrapes(int amount)
            : base(GrapeVariety.SauvignonBlanc, amount) { }

        public SauvignonBlancGrapes(Serial serial)
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

    public class ShirazGrapes : BaseWineGrapes
    {
        [Constructable]
        public ShirazGrapes()
            : this(1) { }

        [Constructable]
        public ShirazGrapes(int amount)
            : base(GrapeVariety.Shiraz, amount) { }

        public ShirazGrapes(Serial serial)
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

    public class ViognierGrapes : BaseWineGrapes
    {
        [Constructable]
        public ViognierGrapes()
            : this(1) { }

        [Constructable]
        public ViognierGrapes(int amount)
            : base(GrapeVariety.Viognier, amount) { }

        public ViognierGrapes(Serial serial)
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

    public class ZinfandelGrapes : BaseWineGrapes
    {
        [Constructable]
        public ZinfandelGrapes()
            : this(1) { }

        [Constructable]
        public ZinfandelGrapes(int amount)
            : base(GrapeVariety.Zinfandel, amount) { }

        public ZinfandelGrapes(Serial serial)
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
