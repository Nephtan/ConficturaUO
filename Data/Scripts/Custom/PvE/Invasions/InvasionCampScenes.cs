using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Custom.Confictura.Invasions
{
    internal sealed class InvasionCampPropSpec
    {
        public int ItemID;
        public InvasionCampPropKind Kind;
        public int X;
        public int Y;
        public int Z;

        public InvasionCampPropSpec(int itemID, InvasionCampPropKind kind, int x, int y, int z)
        {
            ItemID = itemID;
            Kind = kind;
            X = x;
            Y = y;
            Z = z;
        }
    }

    public sealed class InvasionCampProp : Item, IInvasionOwned
    {
        private int m_Session;
        private InvasionCityId m_City;
        private InvasionFaction m_Faction;
        private int m_CampIndex;
        private int m_PropIndex;
        private InvasionCampFacing m_Facing;
        private InvasionCampLayout m_Layout;
        private InvasionCampPropKind m_PropKind;

        public int InvasionSession { get { return m_Session; } }
        public InvasionCityId InvasionCity { get { return m_City; } }
        public InvasionFaction InvasionFaction { get { return m_Faction; } }
        public InvasionSide InvasionSide { get { return InvasionSide.Invader; } }
        public InvasionRole InvasionRole { get { return InvasionRole.CampProp; } }
        public int InvasionObjectiveIndex { get { return m_CampIndex; } }
        public int PropIndex { get { return m_PropIndex; } }
        public InvasionCampFacing Facing { get { return m_Facing; } }
        public InvasionCampLayout Layout { get { return m_Layout; } }
        public InvasionCampPropKind PropKind { get { return m_PropKind; } }

        public InvasionCampProp(
            int session,
            InvasionCityId city,
            InvasionFaction faction,
            int campIndex,
            int propIndex,
            InvasionCampFacing facing,
            InvasionCampLayout layout,
            InvasionCampPropKind propKind,
            int itemID
        )
            : base(itemID)
        {
            m_Session = session;
            m_City = city;
            m_Faction = faction;
            m_CampIndex = campIndex;
            m_PropIndex = propIndex;
            m_Facing = facing;
            m_Layout = layout;
            m_PropKind = propKind;

            Movable = false;
            Hue = GetFactionHue(faction);
            Name = GetPropName(faction, propKind);

            if (propKind == InvasionCampPropKind.Fire || propKind == InvasionCampPropKind.Light)
                Light = LightType.Circle225;
            else if (propKind == InvasionCampPropKind.SummoningFocus)
                Light = LightType.Circle300;
        }

        public InvasionCampProp(Serial serial)
            : base(serial)
        {
        }

        private static int GetFactionHue(InvasionFaction faction)
        {
            switch (faction)
            {
                case InvasionFaction.ClockworkDominion:
                    return 0x972;
                case InvasionFaction.BloodCourt:
                    return 0x21;
                case InvasionFaction.AbyssalLegion:
                    return 0x455;
                default:
                    return 0;
            }
        }

        private static string GetPropName(InvasionFaction faction, InvasionCampPropKind kind)
        {
            string factionName;

            switch (faction)
            {
                case InvasionFaction.ClockworkDominion:
                    factionName = "Clockwork Dominion";
                    break;
                case InvasionFaction.BloodCourt:
                    factionName = "Blood Court";
                    break;
                case InvasionFaction.AbyssalLegion:
                    factionName = "Abyssal Legion";
                    break;
                default:
                    factionName = "invasion";
                    break;
            }

            return factionName + " " + kind.ToString().ToLowerInvariant();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Session);
            writer.Write((int)m_City);
            writer.Write((int)m_Faction);
            writer.Write(m_CampIndex);
            writer.Write(m_PropIndex);
            writer.Write((int)m_Facing);
            writer.Write((int)m_Layout);
            writer.Write((int)m_PropKind);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Session = reader.ReadInt();
            m_City = (InvasionCityId)reader.ReadInt();
            m_Faction = (InvasionFaction)reader.ReadInt();
            m_CampIndex = reader.ReadInt();
            m_PropIndex = reader.ReadInt();
            m_Facing = (InvasionCampFacing)reader.ReadInt();
            m_Layout = (InvasionCampLayout)reader.ReadInt();
            m_PropKind = (InvasionCampPropKind)reader.ReadInt();

            Movable = false;
        }
    }

    internal static class InvasionCampSceneFactory
    {
        private static readonly InvasionCampPropSpec[] m_Tent = new InvasionCampPropSpec[]
        {
            new InvasionCampPropSpec(563, InvasionCampPropKind.Tent, -1, -1, 0),
            new InvasionCampPropSpec(568, InvasionCampPropKind.Tent, -1, 0, 0),
            new InvasionCampPropSpec(568, InvasionCampPropKind.Tent, -1, 1, 0),
            new InvasionCampPropSpec(569, InvasionCampPropKind.Tent, 0, -1, 0),
            new InvasionCampPropSpec(569, InvasionCampPropKind.Tent, 1, -1, 0),
            new InvasionCampPropSpec(565, InvasionCampPropKind.Tent, 2, -1, 0),
            new InvasionCampPropSpec(562, InvasionCampPropKind.Tent, 2, 0, 0),
            new InvasionCampPropSpec(562, InvasionCampPropKind.Tent, 2, 1, 0),
            new InvasionCampPropSpec(1552, InvasionCampPropKind.Tent, 0, 0, 20),
            new InvasionCampPropSpec(1553, InvasionCampPropKind.Tent, 2, 0, 20),
            new InvasionCampPropSpec(1547, InvasionCampPropKind.Tent, 0, 1, 20),
            new InvasionCampPropSpec(1548, InvasionCampPropKind.Tent, 1, 0, 20),
            new InvasionCampPropSpec(1549, InvasionCampPropKind.Tent, 2, 1, 20),
            new InvasionCampPropSpec(1544, InvasionCampPropKind.Tent, 1, 1, 28),
            new InvasionCampPropSpec(564, InvasionCampPropKind.Tent, -1, 2, 0),
            new InvasionCampPropSpec(561, InvasionCampPropKind.Tent, 0, 2, 0),
            new InvasionCampPropSpec(561, InvasionCampPropKind.Tent, 1, 2, 0),
            new InvasionCampPropSpec(560, InvasionCampPropKind.Tent, 2, 2, 0),
            new InvasionCampPropSpec(1554, InvasionCampPropKind.Tent, 0, 2, 20),
            new InvasionCampPropSpec(1551, InvasionCampPropKind.Tent, 2, 2, 20),
            new InvasionCampPropSpec(1550, InvasionCampPropKind.Tent, 1, 2, 20)
        };

        public static List<InvasionCampPropSpec> GetSpecs(InvasionFaction faction, InvasionCampLayout layout)
        {
            List<InvasionCampPropSpec> specs = new List<InvasionCampPropSpec>();
            int tentX = layout == InvasionCampLayout.Open ? -7 : -6;
            int tentY = 4;
            int rearY = layout == InvasionCampLayout.Open ? 7 : 6;
            int flankX = layout == InvasionCampLayout.Compact ? 6 : 7;

            for (int i = 0; i < m_Tent.Length; ++i)
            {
                InvasionCampPropSpec piece = m_Tent[i];
                specs.Add(new InvasionCampPropSpec(piece.ItemID, piece.Kind, piece.X + tentX, piece.Y + tentY, piece.Z));
            }

            Add(specs, 0xDE3, InvasionCampPropKind.Fire, 4, 4, 0);
            Add(specs, 0x9A9, InvasionCampPropKind.Supplies, 5, 6, 0);
            Add(specs, 0xE77, InvasionCampPropKind.Supplies, 6, 6, 0);
            Add(specs, 0x15AE, InvasionCampPropKind.Banner, -5, 0, 0);

            Add(specs, 0x9A, InvasionCampPropKind.Barricade, -flankX, rearY, 0);
            Add(specs, 0x9B, InvasionCampPropKind.Barricade, -flankX + 1, rearY, 0);
            Add(specs, 0x9C, InvasionCampPropKind.Barricade, flankX - 1, rearY, 0);
            Add(specs, 0x9D, InvasionCampPropKind.Barricade, flankX, rearY, 0);

            if (faction == InvasionFaction.ClockworkDominion)
            {
                Add(specs, 0x10DE, InvasionCampPropKind.Machinery, 3, 6, 0);
                Add(specs, 0xFB0, InvasionCampPropKind.Machinery, 2, 6, 0);
                Add(specs, 0x1053, InvasionCampPropKind.Machinery, 5, 3, 0);
                Add(specs, 0x5739, InvasionCampPropKind.Machinery, 6, 3, 0);
                Add(specs, 0x9AB, InvasionCampPropKind.Supplies, 7, 5, 0);
                Add(specs, 0x19AA, InvasionCampPropKind.Light, -3, 6, 0);
            }
            else if (faction == InvasionFaction.BloodCourt)
            {
                Add(specs, 0xE31, InvasionCampPropKind.Light, 3, 6, 0);
                Add(specs, 0x19AA, InvasionCampPropKind.Light, -3, 6, 0);
                Add(specs, 0x570B, InvasionCampPropKind.Cage, 6, 2, 0);
                Add(specs, 0x2204, InvasionCampPropKind.Bones, -6, 1, 0);
                Add(specs, 0x1B09, InvasionCampPropKind.Bones, 5, 2, 0);
                Add(specs, 0xC0F, InvasionCampPropKind.Supplies, 7, 5, 0);
            }
            else
            {
                Add(specs, 0xFEA, InvasionCampPropKind.SummoningFocus, 4, 6, 0);
                Add(specs, 0x194D, InvasionCampPropKind.Light, -3, 6, 0);
                Add(specs, 0x2204, InvasionCampPropKind.Bones, -6, 1, 0);
                Add(specs, 0x1B09, InvasionCampPropKind.Bones, 6, 2, 0);
                Add(specs, 0x281D, InvasionCampPropKind.Supplies, 7, 5, 0);
                Add(specs, 0x19AA, InvasionCampPropKind.Fire, 2, 6, 0);
            }

            return specs;
        }

        public static Point3D Transform(Point3D origin, InvasionCampPropSpec spec, InvasionCampFacing facing)
        {
            int x = spec.X;
            int y = spec.Y;
            int rotatedX;
            int rotatedY;

            switch (facing)
            {
                case InvasionCampFacing.East:
                    rotatedX = -y;
                    rotatedY = x;
                    break;
                case InvasionCampFacing.South:
                    rotatedX = -x;
                    rotatedY = -y;
                    break;
                case InvasionCampFacing.West:
                    rotatedX = y;
                    rotatedY = -x;
                    break;
                default:
                    rotatedX = x;
                    rotatedY = y;
                    break;
            }

            return new Point3D(origin.X + rotatedX, origin.Y + rotatedY, origin.Z + spec.Z);
        }

        private static void Add(List<InvasionCampPropSpec> specs, int itemID, InvasionCampPropKind kind, int x, int y, int z)
        {
            specs.Add(new InvasionCampPropSpec(itemID, kind, x, y, z));
        }
    }
}
