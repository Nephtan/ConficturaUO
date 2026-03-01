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
    public class SayThisCommand
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;

        public static void Initialize()
        {
            CommandSystem.Register(
                "SayThis",
                accessLevel,
                new CommandEventHandler(SayThis_OnCommand)
            );
        }

        [Usage("SayThis <text>")]
        [Description("Forces Target to Say <text>.")]
        public static void SayThis_OnCommand(CommandEventArgs e)
        {
            string toSay = e.ArgString.Trim();

            if (toSay.Length > 0)
                e.Mobile.Target = new SayThisTarget(toSay);
            else
                e.Mobile.SendMessage("Format: SayThis \"<text>\"");
        }

        private class SayThisTarget : Target
        {
            private string m_toSay;

            public SayThisTarget(string s)
                : base(-1, false, TargetFlags.None)
            {
                m_toSay = s;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile targ = (Mobile)targeted;
                    targ.Say(m_toSay);
                }
                else if (targeted is Item)
                {
                    Item objet = targeted as Item;
                    objet.PublicOverheadMessage(MessageType.Regular, 0, false, "" + m_toSay + "");
                }
            }
        }
    }
}
