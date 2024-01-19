using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class SkaraBraeInvasionStone : Item
    {
        [Constructable]
        public SkaraBraeInvasionStone()
            : base(0xED4)
        {
            Movable = false;
            Hue = 33;
            Name = "a skara brae invasion stone";
        }

        public virtual void CleanUpSpawns(string spawnername)
        {
            if (spawnername == "Spawner")
            {
                Console.WriteLine(
                    "Warning: Army Spawner did not clean up spawns due to name to delete was Spawner."
                );
                return;
            }

            ArrayList spawns = new ArrayList(World.Items.Values);
            foreach (Item item in spawns)
            {
                if ((item is Spawner) && (((Spawner)item).Name == spawnername))
                {
                    item.Delete();
                }
            }
        }

        public virtual void CleanUpWayPoints(string waypointname)
        {
            if (waypointname == "WayPoint")
            {
                Console.WriteLine(
                    "Warning: Army Spawner did not clean up waypoints due to name to delete was WayPoint."
                );
                return;
            }

            ArrayList waypoints = new ArrayList(World.Items.Values);
            foreach (Item item in waypoints)
            {
                if ((item is WayPoint) && (((WayPoint)item).Name == waypointname))
                {
                    item.Delete();
                }
            }
        }

        public virtual void CleanUpSkaraBraeLodor()
        {
            CleanUpSpawns("SkaraBraeInvasionLodor");
            CleanUpWayPoints("SkaraBraeInvasionLodor");
        }

        public virtual void StopSkaraBraeLodor()
        {
            ArrayList skarafel = new ArrayList(World.Items.Values);
            foreach (Item item in skarafel)
            {
                if (item is SkaraBraeInvasionStone)
                {
                    ((SkaraBraeInvasionStone)item).CleanUpSkaraBraeLodor();
                }
            }
        }

        public virtual void CleanUpSkaraBraeSosaria()
        {
            CleanUpSpawns("SkaraBraeInvasionSosaria");
            CleanUpWayPoints("SkaraBraeInvasionSosaria");
        }

        public virtual void StopSkaraBraeSosaria()
        {
            ArrayList skaratram = new ArrayList(World.Items.Values);
            foreach (Item item in skaratram)
            {
                if (item is SkaraBraeInvasionStone)
                {
                    ((SkaraBraeInvasionStone)item).CleanUpSkaraBraeSosaria();
                }
            }
        }

        public SkaraBraeInvasionStone(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Skara Brae is being invated");
        }
    }
}
