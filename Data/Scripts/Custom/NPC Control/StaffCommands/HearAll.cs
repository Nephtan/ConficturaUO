using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;

namespace Server.Commands
{
    public class HearAllCommand
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;
        private static List<Mobile> m_HearAll = new List<Mobile>();

        public static void Initialize()
        {
            CommandSystem.Register(
                "HearAll",
                accessLevel,
                new CommandEventHandler(HearAll_OnCommand)
            );
            EventSink.Speech += new SpeechEventHandler(HearAllOnSpeech);
        }

        public static void HearAllOnSpeech(SpeechEventArgs e)
        {
            if (e == null || e.Mobile == null || m_HearAll.Count == 0)
                return;

            string msg = String.Format("({0}): {1}", e.Mobile.RawName, e.Speech);

            for (int i = m_HearAll.Count - 1; i >= 0; --i)
            {
                Mobile listener = m_HearAll[i];

                if (
                    listener == null
                    || listener.Deleted
                    || listener.NetState == null
                    || listener.AccessLevel < accessLevel
                )
                {
                    m_HearAll.RemoveAt(i);
                    continue;
                }

                listener.SendMessage(msg);
            }
        }

        [Usage("HearAll")]
        [Description("Enable or Disable hearing everything in the world.")]
        public static void HearAll_OnCommand(CommandEventArgs e)
        {
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            if (m_HearAll.Contains(e.Mobile))
            {
                m_HearAll.Remove(e.Mobile);
                e.Mobile.SendMessage("HearAll deactivated.");
            }
            else
            {
                m_HearAll.Add(e.Mobile);
                e.Mobile.SendMessage("HearAll enabled.");
            }
        }
    }
}
