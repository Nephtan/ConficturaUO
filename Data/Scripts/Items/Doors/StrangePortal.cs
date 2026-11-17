using Server.Engines.XmlSpawner2;
using Server.Multis;
using System;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class StrangePortal : Item
    {
        public int GateLocation;
        private Strange_Portal m_ReturnGate;
        private bool m_ReadLegacyVersion;

        [CommandProperty(AccessLevel.Owner)]
        public int Gate_Location
        {
            get { return GateLocation; }
            set
            {
                GateLocation = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public StrangePortal()
            : base(0x3D5E)
        {
            Movable = false;
            Light = LightType.Circle300;

            string sCalled = "a strange";
            switch (Utility.RandomMinMax(0, 6))
            {
                case 0:
                    sCalled = "an odd";
                    break;
                case 1:
                    sCalled = "an unusual";
                    break;
                case 2:
                    sCalled = "a bizarre";
                    break;
                case 3:
                    sCalled = "a curious";
                    break;
                case 4:
                    sCalled = "a peculiar";
                    break;
                case 5:
                    sCalled = "a strange";
                    break;
                case 6:
                    sCalled = "a weird";
                    break;
            }

            Name = sCalled + " portal";
            Hue = Utility.RandomList(
                0xB96,
                0xB80,
                0xB7F,
                0xB79,
                0xB77,
                0xB71,
                0xB70,
                0xB64,
                0xB63,
                0,
                0xB61,
                0xB50,
                0xB51,
                0xB52,
                0xB53,
                0xB3D,
                0xB17,
                0xB09,
                0xB0A,
                0xB0B,
                0xB0C,
                0xB0F,
                0xAFE,
                0xAFF,
                0xB00,
                0xB01,
                0xB02,
                0xB03,
                0xAF8,
                0xABB,
                0xABC
            );
        }

        public StrangePortal(Serial serial)
            : base(serial) { }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile)
            {
                UseGate(m, GateLocation, null);
                Effects.PlaySound(m.Location, m.Map, 0x1FC);
            }

            return false;
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            int first = 15;
            int last = 26;
            string world = Worlds.GetMyWorld(this.Map, this.Location, this.X, this.Y);
            if (world == "the Land of Lodoria")
            {
                first = 0;
                last = 14;
            }
            else if (world == "the Serpent Island")
            {
                first = 27;
                last = 41;
            }
            else if (world == "the Isles of Dread")
            {
                first = 42;
                last = 47;
            }
            else if (world == "the Savaged Empire")
            {
                first = 48;
                last = 56;
            }
            else if (world == "the Underworld")
            {
                first = 57;
                last = 64;
            }

            if (m_ReturnGate != null && !m_ReturnGate.Deleted)
                m_ReturnGate.Delete();

            List<int> available = new List<int>();
            for (int route = first; route <= last; route++)
            {
                Point3D landing;
                Map facet;
                GetDestination(route, out landing, out facet);
                if (Strange_Portal.FindActiveReturn(facet, landing) == null)
                    available.Add(route);
            }

            if (available.Count == 0)
            {
                Delete();
                return;
            }

            GateLocation = available[Utility.Random(available.Count)];
            Strange_Portal gate = new Strange_Portal(this);
            gate.Gate_Location_X = this.X;
            gate.Gate_Location_Y = this.Y;
            gate.Gate_Location_Z = this.Z;
            gate.Gate_Location_M = this.Map;
            gate.Hue = this.Hue;
            gate.Name = this.Name;
            UseGate(null, GateLocation, gate);
            m_ReturnGate = gate;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_ReturnGate != null && !m_ReturnGate.Deleted)
                m_ReturnGate.Delete();
        }

        internal bool OwnsReturn(Strange_Portal gate)
        {
            return !Deleted && Parent == null && Map != null && Map != Map.Internal && m_ReturnGate == gate;
        }

        public static void Initialize()
        {
            RestoreLegacyReturns();
        }

        private static void RestoreLegacyReturns()
        {
            Dictionary<Strange_Portal, StrangePortal> candidates = new Dictionary<Strange_Portal, StrangePortal>();
            foreach (Item item in World.Items.Values)
            {
                StrangePortal entrance = item as StrangePortal;
                if (entrance == null || !entrance.m_ReadLegacyVersion)
                    continue;

                Strange_Portal gate = entrance.FindLegacyReturn();
                if (gate == null)
                    continue;

                // Reject a return claimed by multiple entrances before changing either side.
                candidates[gate] = candidates.ContainsKey(gate) ? null : entrance;
            }

            foreach (KeyValuePair<Strange_Portal, StrangePortal> pair in candidates)
            {
                if (pair.Value != null)
                {
                    pair.Value.m_ReturnGate = pair.Key;
                    pair.Key.LinkEntrance(pair.Value);
                }
            }
        }

        private Strange_Portal FindLegacyReturn()
        {
            if (GetType() != typeof(StrangePortal) || Deleted || Parent != null || Map == null || Map == Map.Internal
                || m_ReturnGate != null || ItemID != 0x3D5E || !Visible || Movable || String.IsNullOrEmpty(Name)
                || GateLocation < 0 || GateLocation > 64)
                return null;

            Point3D landing;
            Map facet;
            GetDestination(GateLocation, out landing, out facet);
            Strange_Portal match = null;
            foreach (Item item in facet.GetSector(landing).Items)
            {
                Strange_Portal gate = item as Strange_Portal;
                if (gate == null || gate.Deleted || gate.Parent != null || gate.Location != landing
                    || gate.GateLocation_M != Map || gate.GateLocation_X != X || gate.GateLocation_Y != Y || gate.GateLocation_Z != Z
                    || gate.ItemID != ItemID || gate.Hue != Hue || gate.Name != Name)
                    continue;

                // Count even protected lookalikes as ambiguous rather than picking an eligible one.
                if (match != null)
                    return null;
                match = gate;
            }

            return match != null && match.CanLinkLegacyEntrance() ? match : null;
        }


        private static void GetDestination(int portal, out Point3D loc, out Map map)
        {
            loc = Point3D.Zero;
            map = Map.Sosaria;

            switch (portal)
            {
                case 0:
                    loc = new Point3D(5773, 2804, 0);
                    map = Map.Lodor;
                    break; // the Crypts of Dracula
                case 1:
                    loc = new Point3D(5353, 91, 15);
                    map = Map.Lodor;
                    break; // the Mind Flayer City
                case 2:
                    loc = new Point3D(5789, 2558, -30);
                    map = Map.Lodor;
                    break; // Dungeon Covetous
                case 3:
                    loc = new Point3D(5308, 680, 0);
                    map = Map.Lodor;
                    break; // Dungeon Deceit
                case 4:
                    loc = new Point3D(5185, 2442, 6);
                    map = Map.Lodor;
                    break; // Dungeon Despise
                case 5:
                    loc = new Point3D(5321, 799, 0);
                    map = Map.Lodor;
                    break; // Dungeon Destard
                case 6:
                    loc = new Point3D(5869, 1443, 0);
                    map = Map.Lodor;
                    break; // the City of Embers
                case 7:
                    loc = new Point3D(6038, 200, 22);
                    map = Map.Lodor;
                    break; // Dungeon Hythloth
                case 8:
                    loc = new Point3D(5728, 155, 1);
                    map = Map.Lodor;
                    break; // the Frozen Hells
                case 9:
                    loc = new Point3D(5783, 23, 0);
                    map = Map.Lodor;
                    break; // Dungeon Shame
                case 10:
                    loc = new Point3D(5174, 1703, 2);
                    map = Map.Lodor;
                    break; // Terathan Keep
                case 11:
                    loc = new Point3D(5247, 436, 0);
                    map = Map.Lodor;
                    break; // the Halls of Undermountain
                case 12:
                    loc = new Point3D(5859, 3427, 0);
                    map = Map.Lodor;
                    break; // the Volcanic Cave
                case 13:
                    loc = new Point3D(5443, 1398, 0);
                    map = Map.Lodor;
                    break; // Dungeon Wrong
                case 14:
                    loc = new Point3D(6035, 2574, 0);
                    map = Map.Lodor;
                    break; // Stonegate Castle

                case 15:
                    loc = new Point3D(5854, 1756, 0);
                    map = Map.Sosaria;
                    break; // the Caverns of Poseidon
                case 16:
                    loc = new Point3D(5354, 923, 0);
                    map = Map.Sosaria;
                    break; // the Ancient Pyramid
                case 17:
                    loc = new Point3D(5965, 636, 0);
                    map = Map.Sosaria;
                    break; // Dungeon Exodus
                case 18:
                    loc = new Point3D(262, 3380, 0);
                    map = Map.Sosaria;
                    break; // the Cave of Banished Mages
                case 19:
                    loc = new Point3D(5981, 2154, 0);
                    map = Map.Sosaria;
                    break; // Dungeon Clues
                case 20:
                    loc = new Point3D(5550, 393, 0);
                    map = Map.Sosaria;
                    break; // Dardin's Pit
                case 21:
                    loc = new Point3D(5259, 262, 0);
                    map = Map.Sosaria;
                    break; // Dungeon Doom
                case 22:
                    loc = new Point3D(5526, 1228, 0);
                    map = Map.Sosaria;
                    break; // the Fires of Hell
                case 23:
                    loc = new Point3D(5587, 1602, 0);
                    map = Map.Sosaria;
                    break; // the Mines of Morinia
                case 24:
                    loc = new Point3D(5995, 423, 0);
                    map = Map.Sosaria;
                    break; // the Perinian Depths
                case 25:
                    loc = new Point3D(5638, 821, 0);
                    map = Map.Sosaria;
                    break; // the Dungeon of Time Awaits
                case 26:
                    loc = new Point3D(100, 3389, 0);
                    map = Map.SavagedEmpire;
                    break; // Forgotten Halls

                case 27:
                    loc = new Point3D(1955, 523, 0);
                    map = Map.SerpentIsland;
                    break; // the Ancient Prison
                case 28:
                    loc = new Point3D(2090, 863, 0);
                    map = Map.SerpentIsland;
                    break; // the Cave of Fire
                case 29:
                    loc = new Point3D(2440, 53, 2);
                    map = Map.SerpentIsland;
                    break; // the Cave of Souls
                case 30:
                    loc = new Point3D(2032, 76, 0);
                    map = Map.SerpentIsland;
                    break; // Dungeon Ankh
                case 31:
                    loc = new Point3D(1947, 216, 0);
                    map = Map.SerpentIsland;
                    break; // Dungeon Bane
                case 32:
                    loc = new Point3D(2189, 425, 0);
                    map = Map.SerpentIsland;
                    break; // Dungeon Hate
                case 33:
                    loc = new Point3D(2221, 816, 0);
                    map = Map.SerpentIsland;
                    break; // Dungeon Scorn
                case 34:
                    loc = new Point3D(1957, 710, 0);
                    map = Map.SerpentIsland;
                    break; // Dungeon Torment
                case 35:
                    loc = new Point3D(2361, 403, 0);
                    map = Map.SerpentIsland;
                    break; // Dungeon Vile
                case 36:
                    loc = new Point3D(2160, 173, 2);
                    map = Map.SerpentIsland;
                    break; // Dungeon Wicked
                case 37:
                    loc = new Point3D(2311, 912, 2);
                    map = Map.SerpentIsland;
                    break; // Dungeon Wrath
                case 38:
                    loc = new Point3D(2459, 880, 0);
                    map = Map.SerpentIsland;
                    break; // the Flooded Temple
                case 39:
                    loc = new Point3D(2064, 509, 0);
                    map = Map.SerpentIsland;
                    break; // the Gargoyle Crypts
                case 40:
                    loc = new Point3D(2457, 506, 0);
                    map = Map.SerpentIsland;
                    break; // the Serpent Sanctum
                case 41:
                    loc = new Point3D(2327, 183, 2);
                    map = Map.SerpentIsland;
                    break; // the Tomb of the Fallen Wizard

                case 42:
                    loc = new Point3D(729, 2635, -28);
                    map = Map.SavagedEmpire;
                    break; // the Blood Temple
                case 43:
                    loc = new Point3D(323, 2836, 0);
                    map = Map.SavagedEmpire;
                    break; // the Ice Queen Fortress
                case 44:
                    loc = new Point3D(366, 3886, 0);
                    map = Map.SavagedEmpire;
                    break; // the Scurvy Reef
                case 45:
                    loc = new Point3D(1968, 1363, 61);
                    map = Map.Underworld;
                    break; // the Glacial Scar
                case 46:
                    loc = new Point3D(6142, 3660, -20);
                    map = Map.Lodor;
                    break; // the Temple of Osirus
                case 47:
                    loc = new Point3D(6021, 1968, 0);
                    map = Map.Lodor;
                    break; // the Sanctum of Saltmarsh

                case 48:
                    loc = new Point3D(774, 1984, -28);
                    map = Map.SavagedEmpire;
                    break; // the Dungeon of the Mad Archmage
                case 49:
                    loc = new Point3D(51, 2619, -28);
                    map = Map.SavagedEmpire;
                    break; // the Tombs
                case 50:
                    loc = new Point3D(342, 2296, -1);
                    map = Map.SavagedEmpire;
                    break; // the Dungeon of the Lich King
                case 51:
                    loc = new Point3D(1143, 2403, -28);
                    map = Map.SavagedEmpire;
                    break; // the Halls of Ogrimar
                case 52:
                    loc = new Point3D(692, 2319, -27);
                    map = Map.SavagedEmpire;
                    break; // Dungeon Rock
                case 53:
                    loc = new Point3D(647, 3860, 39);
                    map = Map.SavagedEmpire;
                    break; // the Undersea Castle
                case 54:
                    loc = new Point3D(231, 3650, 25);
                    map = Map.SavagedEmpire;
                    break; // the Azure Castle
                case 55:
                    loc = new Point3D(436, 3311, 20);
                    map = Map.SavagedEmpire;
                    break; // the Tomb of Kazibal
                case 56:
                    loc = new Point3D(670, 3357, 20);
                    map = Map.SavagedEmpire;
                    break; // the Catacombs of Azerok

                case 57:
                    loc = new Point3D(1851, 1233, -42);
                    map = Map.Underworld;
                    break; // the Stygian Abyss
                case 58:
                    loc = new Point3D(6413, 2004, -40);
                    map = Map.Lodor;
                    break; // the Daemon's Crag
                case 59:
                    loc = new Point3D(7003, 2437, -11);
                    map = Map.Lodor;
                    break; // the Zealan Tombs
                case 60:
                    loc = new Point3D(6368, 968, 25);
                    map = Map.Lodor;
                    break; // the Hall of the Mountain King
                case 61:
                    loc = new Point3D(6826, 1123, -92);
                    map = Map.Lodor;
                    break; // Morgaelin's Inferno
                case 62:
                    loc = new Point3D(5950, 1654, -5);
                    map = Map.Lodor;
                    break; // the Depths of Carthax Lake
                case 63:
                    loc = new Point3D(5989, 484, 1);
                    map = Map.Lodor;
                    break; // Argentrock Castle
                case 64:
                    loc = new Point3D(1125, 3684, 0);
                    map = Map.Lodor;
                    break; // the Ancient Sky Ship
            }
        }

        public static void UseGate(Mobile m, int portal, Item gate)
        {
            Point3D loc;
            Map map;
            GetDestination(portal, out loc, out map);

            if (m is PlayerMobile)
            {
                Server.Mobiles.BaseCreature.TeleportPets(m, loc, map);
                m.MoveToWorld(loc, map);
            }
            else if (m is BaseCreature)
            {
                m.MoveToWorld(loc, map);
            }
            else if (gate is Strange_Portal)
            {
                gate.MoveToWorld(loc, map);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(GateLocation);
            writer.Write(m_ReturnGate);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            GateLocation = reader.ReadInt();
            m_ReadLegacyVersion = version == 0;
            if (version >= 1)
                m_ReturnGate = reader.ReadItem() as Strange_Portal;
        }
    }

    public class Strange_Portal : Item
    {
        public int GateLocation_X;
        public int GateLocation_Y;
        public int GateLocation_Z;
        public Map GateLocation_M;
        private StrangePortal m_Entrance;

        [CommandProperty(AccessLevel.Owner)]
        public int Gate_Location_X
        {
            get { return GateLocation_X; }
            set
            {
                GateLocation_X = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Owner)]
        public int Gate_Location_Y
        {
            get { return GateLocation_Y; }
            set
            {
                GateLocation_Y = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Owner)]
        public int Gate_Location_Z
        {
            get { return GateLocation_Z; }
            set
            {
                GateLocation_Z = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Owner)]
        public Map Gate_Location_M
        {
            get { return GateLocation_M; }
            set
            {
                GateLocation_M = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public Strange_Portal()
            : base(0x3D5E)
        {
            Movable = false;
            Light = LightType.Circle300;
            Name = "portal";
        }

        internal Strange_Portal(StrangePortal entrance)
            : this()
        {
            m_Entrance = entrance;
        }

        internal bool CanLinkLegacyEntrance()
        {
            return GetType() == typeof(Strange_Portal) && m_Entrance == null && Visible && !Movable
                && !IsLockedDown && !IsSecure && !Insured && BlessedFor == null && LootType == LootType.Regular
                && Spawner == null && Items.Count == 0 && Weight != -2 && Weight != -3
                && BaseHouse.FindHouseAt(this) == null && XmlAttach.FindAttachment(this) == null;
        }

        internal void LinkEntrance(StrangePortal entrance)
        {
            m_Entrance = entrance;
        }

        private bool HasActiveEntrance
        {
            get { return m_Entrance != null && m_Entrance.OwnsReturn(this); }
        }

        internal static Strange_Portal FindActiveReturn(Map map, Point3D point)
        {
            IPooledEnumerable nearby = map.GetItemsInRange(point, 0);
            try
            {
                foreach (Item item in nearby)
                {
                    Strange_Portal gate = item as Strange_Portal;
                    if (gate != null && !gate.Deleted && gate.Location == point && gate.HasActiveEntrance)
                        return gate;
                }

                return null;
            }
            finally
            {
                nearby.Free();
            }
        }

        public Strange_Portal(Serial serial)
            : base(serial) { }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile)
            {
                // The linked route has priority over legacy returns at the same landing.
                if (m_Entrance != null ? !HasActiveEntrance : FindActiveReturn(Map, Location) != null)
                    return true;

                UseGate(m);
                Effects.PlaySound(m.Location, m.Map, 0x1FC);
            }

            return false;
        }

        public void UseGate(Mobile m)
        {
            Point3D loc = HasActiveEntrance ? m_Entrance.Location : new Point3D(GateLocation_X, GateLocation_Y, GateLocation_Z);
            Map map = HasActiveEntrance ? m_Entrance.Map : GateLocation_M;

            if (m is PlayerMobile)
            {
                Server.Mobiles.BaseCreature.TeleportPets(m, loc, map);
                m.MoveToWorld(loc, map);
            }
            else if (m is BaseCreature)
            {
                m.MoveToWorld(loc, map);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(GateLocation_X);
            writer.Write(GateLocation_Y);
            writer.Write(GateLocation_Z);
            writer.Write(GateLocation_M);
            writer.Write(m_Entrance);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            GateLocation_X = reader.ReadInt();
            GateLocation_Y = reader.ReadInt();
            GateLocation_Z = reader.ReadInt();
            GateLocation_M = reader.ReadMap();
            if (version >= 1)
                m_Entrance = reader.ReadItem() as StrangePortal;
        }
    }
}
