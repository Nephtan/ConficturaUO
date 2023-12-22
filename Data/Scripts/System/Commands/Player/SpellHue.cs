using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    class SpellHue
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "spellhue",
                AccessLevel.Player,
                new CommandEventHandler(OnSpellHueChange)
            );
        }

        [Usage("spellhue [<name>]")]
        [Description("Changes the default color for magery spell effects.")]
        private static void OnSpellHueChange(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            int hue = 0;

            if (e.Length >= 1)
            {
                hue = e.GetInt32(0);
            }

            m.SendMessage(68, "You have changed your magery spell effects color.");
            ((PlayerMobile)m).MagerySpellHue = hue;
        }
    }
}
