using System;
using System.Text;
using Server;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Mobiles
{
	public class SetDeactivation
	{
		public static double DefaultDeactivationDelay = 20.0; // default AI deactivation delay in minutes

		public static void Initialize()
		{
            CommandSystem.Register("SetDeactivation", AccessLevel.Administrator, new CommandEventHandler(SetDeactivation_OnCommand));
		}

        [Usage( "SetDeactivation [minutes]" )]
        [Description( "Sets/reports the default AI deactivation delay for the PlayerRangeSensitive mod in minutes" )]
        public static void SetDeactivation_OnCommand( CommandEventArgs e )
        {
                if( e.Arguments.Length > 0 ){
                  try{
                    DefaultDeactivationDelay = double.Parse(e.Arguments[0]);
                  } catch{}
                }
                e.Mobile.SendMessage("Default AI deactivation delay set to {0} minutes",DefaultDeactivationDelay);
        }
    }
}
