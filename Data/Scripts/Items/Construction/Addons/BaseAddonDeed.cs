using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
    [Flipable(0x14F0, 0x14EF)]
    public abstract class BaseAddonDeed : Item
    {
        private static readonly TextDefinition PlaceBlockedMessage =
            new TextDefinition("Placement failed: a roof or wall blocks the location.");

        private static readonly TextDefinition PlaceOutsideMessage =
            new TextDefinition("Placement failed: the addon would be outside the house.");

        public abstract BaseAddon Addon { get; }

        public BaseAddonDeed()
            : base(0x14F0)
        {
            Weight = 1.0;

            if (!Core.AOS)
                LootType = LootType.Newbied;
        }

        public BaseAddonDeed(Serial serial)
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

            if (Weight == 0.0)
                Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.Target = new InternalTarget(this);
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        private class InternalTarget : Target
        {
            private BaseAddonDeed m_Deed;

            public InternalTarget(BaseAddonDeed deed)
                : base(-1, true, TargetFlags.None)
            {
                m_Deed = deed;

                CheckLOS = false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                IPoint3D p = targeted as IPoint3D;
                Map map = from.Map;

                if (p == null || map == null || m_Deed.Deleted)
                    return;

                if (m_Deed.IsChildOf(from.Backpack))
                {
                    BaseAddon addon = m_Deed.Addon;

                    Server.Spells.SpellHelper.GetSurfaceTop(ref p);

                    List<BaseHouse> houses = new List<BaseHouse>();

                    PlayerMobile pm = (PlayerMobile)from; //NEW Added for player city
                    bool ismayor = false;
                    if (
                        pm.City != null
                        && pm.City.Mayor == pm
                        && PlayerGovernmentSystem.IsAtCity(from)
                    )
                        ismayor = true;

                    AddonFitResult res = addon.CouldFit(p, map, from, ref houses);

                    if (res == AddonFitResult.Valid)
                    {
                        addon.MoveToWorld(new Point3D(p), map);
                    }
                    else if (ismayor)
                    {
                        CityManagementStone stone = pm.City;
                        addon.MoveToWorld(new Point3D(p), map);
                        stone.AddOns.Add(addon);
                    }
                    else if (res == AddonFitResult.Blocked)
                    {
                        TextDefinition.SendMessageTo(from, PlaceBlockedMessage);
                    }
                    else if (res == AddonFitResult.NotInHouse)
                    {
                        Point3D loc = new Point3D(p);
                        BaseHouse houseAt = BaseHouse.FindHouseAt(loc, map, 16);

                        if (houseAt != null && houseAt.IsOwner(from))
                            TextDefinition.SendMessageTo(from, PlaceOutsideMessage);
                        else
                            from.SendLocalizedMessage(500274); // You can only place this in a house that you own!
                    }
                    else if (res == AddonFitResult.DoorTooClose)
                    {
                        from.SendLocalizedMessage(500271); // You cannot build near the door.
                    }
                    else if (res == AddonFitResult.NoWall)
                    {
                        from.SendLocalizedMessage(500268); // This object needs to be mounted on something.
                    }

                    if (res == AddonFitResult.Valid)
                    {
                        m_Deed.Delete();

                        foreach (BaseHouse h in houses)
                            h.Addons.Add(addon);
                    }
                    else if (ismayor)
                    {
                        m_Deed.Delete();
                    }
                    else
                    {
                        addon.Delete();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
            }
        }
    }
}
