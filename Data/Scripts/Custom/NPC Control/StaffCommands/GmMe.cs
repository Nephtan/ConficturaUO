using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
    public class GmMeCommand
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;

        public static void Initialize()
        {
            CommandSystem.Register("GmMe", accessLevel, new CommandEventHandler(GmMe_OnCommand));
        }

        [Usage("GmMe")]
        [Description("Helps senior staff members set their body to GM style.")]
        public static void GmMe_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            CommandLogging.WriteLine(
                from,
                "{0} {1} is assuming a GM body",
                from.AccessLevel,
                CommandLogging.Format(from)
            );
            from.Blessed = true;
            DisRobe(from, Layer.Shoes);
            DisRobe(from, Layer.Pants);
            DisRobe(from, Layer.Shirt);
            DisRobe(from, Layer.Helm);
            DisRobe(from, Layer.Gloves);
            DisRobe(from, Layer.Neck);
            DisRobe(from, Layer.Hair);
            DisRobe(from, Layer.Waist);
            DisRobe(from, Layer.InnerTorso);
            DisRobe(from, Layer.MiddleTorso);
            DisRobe(from, Layer.Arms);
            DisRobe(from, Layer.Cloak);
            DisRobe(from, Layer.OuterTorso);
            DisRobe(from, Layer.OuterLegs);
            from.AddItem(new DragonRobe());
            from.AddItem(new ClothHood());
            from.AddItem(new BootsofHermes());

            for (int i = 0; i < from.Skills.Length; ++i)
                from.Skills[i].Base = 120;

            from.RawStr = 100;
            from.RawDex = 100;
            from.RawInt = 100;
        }

        private static void DisRobe(Mobile m_from, Layer layer)
        {
            if (m_from.FindItemOnLayer(layer) != null)
            {
                Item item = m_from.FindItemOnLayer(layer);
                m_from.PlaceInBackpack(item); // Place in a bag first?
            }
        }
    }
}
