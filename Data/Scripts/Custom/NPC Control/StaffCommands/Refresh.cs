using System;
using System.Collections.Generic;
using Server.Targeting;
using Server.Network;
using System.Reflection;
using Server.Items;
using System.Collections;
using Server.Mobiles;
using Server.Commands;

namespace Server.Commands
{
    public class RefreshCommand
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;
        private static List<Mobile> m_HearAll = new List<Mobile>();

        public static void Initialize()
        {
            CommandSystem.Register(
                "Refresh",
                accessLevel,
                new CommandEventHandler(Refresh_OnCommand)
            );
        }

        [Usage("Refresh")]
        [Description("Sets all targets stats to full.")]
        public static void Refresh_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new freshTarget();
        }

        public class freshTarget : Target
        {
            public freshTarget()
                : base(12, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile targ = (Mobile)targeted;
                    if (!from.CanSee(targ))
                    {
                        from.SendMessage("The target is not in your line of sight!");
                    }
                    else
                    {
                        targ.Hits = targ.HitsMax;
                        targ.Mana = targ.ManaMax;
                        targ.Stam = targ.StamMax;
                    }
                }
            }
        }
    }
}
