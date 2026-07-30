using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    public class HoardTile : Item
    {
        [Constructable]
        public HoardTile()
            : base(0x1BC3)
        {
            Movable = false;
            Visible = false;
            Name = "hoard tile";
        }

        public HoardTile(Serial serial)
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

namespace Server.Misc
{
    class HoardPile
    {
        public static void MakeHoard(Mobile from)
        {
            bool Lucky = false;

            Mobile killer = from.LastKiller;
            if (killer != null)
            {
                if (killer is BaseCreature)
                    killer = ((BaseCreature)killer).GetMaster();

                if (killer is PlayerMobile)
                {
                    Lucky = GetPlayerInfo.LuckyKiller(killer.Luck);
                }
            }

            if (
                from.Title != null
                && from.Title != ""
                && from.Fame >= 15000
                && (Utility.RandomMinMax(1, 5) == 1 || Lucky)
            )
            {
                ArrayList targets = new ArrayList();
                bool morePowerfulCreature = false;

                IPooledEnumerable eable1 = from.GetMobilesInRange(15);
                try
                {
                    foreach (Mobile creature in eable1)
                    {
                        if (
                            creature is BaseCreature
                            && creature != from
                            && ((BaseCreature)creature).ControlMaster == null
                            && creature.Fame >= from.Fame
                        )
                        {
                            morePowerfulCreature = true;
                        }
                    }
                }
                finally
                {
                    eable1.Free();
                }

                if (!morePowerfulCreature)
                {
                    IPooledEnumerable eable2 = from.GetItemsInRange(15);
                    try
                    {
                        foreach (Item spawner in eable2)
                        {
                            if (spawner is HoardTile)
                            {
                                targets.Add(spawner);
                            }
                        }
                    }
                    finally
                    {
                        eable2.Free();
                    }

                    HoardPiles MyHoard = null;
                    Point3D loc = from.Location;
                    Map map = from.Map;
                    Item spawn = null;

                    for (int i = 0; i < targets.Count; ++i)
                    {
                        bool buildTreasure = true;

                        IPooledEnumerable eable3 = from.GetItemsInRange(1);
                        try
                        {
                            foreach (Item loot in eable3)
                            {
                                if (loot is HoardPiles)
                                {
                                    buildTreasure = false;
                                }
                            }
                        }
                        finally
                        {
                            eable3.Free();
                        }

                        if (buildTreasure)
                        {
                            MyHoard = new HoardPiles();
                            if (Worlds.IsOnSpaceship(from.Location, from.Map))
                            {
                                MyHoard.ItemID = Utility.RandomList(0x096D, 0x096E);
                            }
                            spawn = (Item)targets[i];
                            loc = spawn.Location;
                            map = spawn.Map;
                            MyHoard.HoardName = from.Name + " " + from.Title;
                            MyHoard.MoveToWorld(loc, map);
                            Effects.SendLocationParticles(
                                EffectItem.Create(
                                    MyHoard.Location,
                                    MyHoard.Map,
                                    EffectItem.DefaultDuration
                                ),
                                0x3709,
                                10,
                                30,
                                5052
                            );
                            Effects.PlaySound(MyHoard, MyHoard.Map, 0x208);
                        }
                    }
                }
            }
        }
    }
}
