using System;
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
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            string argString = e.ArgString == null ? String.Empty : e.ArgString;
            string toSay = argString.Trim();

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
                if (from == null || from.Deleted)
                    return;

                Mobile mobile = targeted as Mobile;

                if (mobile != null)
                {
                    if (!mobile.Deleted)
                        mobile.Say(m_toSay);

                    return;
                }

                Item item = targeted as Item;

                if (item != null && !item.Deleted)
                    item.PublicOverheadMessage(MessageType.Regular, 0, false, m_toSay);
            }
        }
    }
}
