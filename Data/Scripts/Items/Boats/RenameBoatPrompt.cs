using System;
using Server;
using Server.Prompts;

namespace Server.Multis
{
    public class RenameBoatPrompt : Prompt
    {
        private BaseBoat m_Boat;

        public RenameBoatPrompt(BaseBoat boat)
        {
            m_Boat = boat;
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (from == null || from.Deleted || m_Boat == null || m_Boat.Deleted)
                return;

            if (text == null)
                text = String.Empty;

            m_Boat.EndRename(from, text);
        }
    }
}
