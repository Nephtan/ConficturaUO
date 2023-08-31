//================================================//
// Created by dracana				  //
// Desc: Admin command for placing grapevines.    //
//================================================//
using System;
using Server.Items;
using Server.Items.Crops;
using System.Collections;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using Server.Commands;

namespace Server.Commands
{
    public class gvCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "addgv",
                AccessLevel.Administrator,
                new CommandEventHandler(gv_OnCommand)
            );
        }

        [Usage("addgv")]
        [Description("Add different varieties of grape vines for winecrafting.")]
        public static void gv_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new AddGrapeVineGump(e.Mobile, null, 0));
        }
    }
}
