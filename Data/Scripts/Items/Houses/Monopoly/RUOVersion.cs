/*
 * The two lines following this entry specify what RunUO version you are running.
 * In order to switch to RunUO 1.0 Final, remove the '//' in front of that setting
 * and add '//' in front of '#define RunUO_2_RC1'.  Warning:  If you comment both
 * out, many commands in this system will not work.  Enjoy!
 */

#define RunUO_2_RC1
//#define RunUO_1_Final

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Network;
using Server.Commands;

namespace Knives.TownHouses
{
    public class RUOVersion
    {
        private static Hashtable s_Commands = new Hashtable();

        public static void AddCommand(string com, AccessLevel acc, TownHouseCommandHandler cch)
        {
            s_Commands[com.ToLower()] = cch;

            Server.Commands.CommandSystem.Register(com, acc, new CommandEventHandler(OnCommand));
        }

        public static void OnCommand(CommandEventArgs e)
        {
            if (s_Commands[e.Command.ToLower()] == null)
                return;

            ((TownHouseCommandHandler)s_Commands[e.Command.ToLower()])(
                new CommandInfo(e.Mobile, e.Command, e.ArgString, e.Arguments)
            );
        }

        public static void UpdateRegion(TownHouseSign sign)
        {
            if (sign.House == null)
            {
                return;
            }

            sign.House.UpdateRegion();

            Rectangle3D rect = new Rectangle3D(Point3D.Zero, Point3D.Zero);

            for (int i = 0; i < sign.House.Region.Area.Length; ++i)
            {
                rect = sign.House.Region.Area[i];

                // Removing the house-location offset keeps the region aligned with the
                // townhouse's actual world coordinates.  The refreshed housing boundary
                // definitions rely on the region covering the real-world tiles.  Offsetting
                // by the multi's origin caused the region to be created near the map origin
                // instead of the townhouse, so players inside the townhouse were no longer
                // considered to be in a HouseRegion (breaking logout, hunger, and thirst
                // behavior).  Preserve the world X/Y values and only clamp the Z range to
                // the configured townhouse floors.
                rect = new Rectangle3D(
                    new Point3D(rect.Start.X, rect.Start.Y, sign.MinZ),
                    new Point3D(rect.End.X, rect.End.Y, sign.MaxZ)
                );

                sign.House.Region.Area[i] = rect;
            }

            sign.House.Region.Unregister();
            sign.House.Region.Register();
            sign.House.Region.GoLocation = sign.BanLoc;
        }

        public static bool RegionContains(Region region, Mobile m)
        {
            return region.GetMobiles().Contains(m);
        }

        public static Rectangle3D[] RegionArea(Region region)
        {
            return region.Area;
        }
    }

    public class VersionHouse : BaseHouse
    {
        public override Rectangle2D[] Area
        {
            get { return new Rectangle2D[5]; }
        }

        public override Point3D BaseBanLocation
        {
            get { return Point3D.Zero; }
        }

        public VersionHouse(int id, Mobile m, int locks, int secures)
            : base(id, m, locks, secures) { }

        public VersionHouse(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
