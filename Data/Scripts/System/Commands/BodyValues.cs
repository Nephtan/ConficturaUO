using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;

namespace Server.Scripts.Commands
{
    public class BodyValues
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "BodyValues",
                AccessLevel.Counselor,
                new CommandEventHandler(BodyValuess)
            );
        }

        [Usage("BodyValues")]
        [Description("Changes the body value of the target by 1 point higher.")]
        public static void BodyValuess(CommandEventArgs e)
        {
            e.Mobile.SendMessage("What target do you want to change?");
            e.Mobile.Target = new InternalTarget();
        }

        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(8, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                {
                    Mobile m = (Mobile)targeted;

                    m.Body = m.Body + 1;
                }
            }
        }
    }
}
