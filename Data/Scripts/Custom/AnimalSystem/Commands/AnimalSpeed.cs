using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Commands;
using Server.Multis;


namespace Server.Misc
{
    public class AnimalSpeedCommand
    {
        public static void Initialize()
		{           
            CommandSystem.Register("AnimalSpeed", AccessLevel.Player, new CommandEventHandler(AnimalSpeed_OnCommand));
            CommandSystem.Register("AS", AccessLevel.Player, new CommandEventHandler(AnimalSpeed_OnCommand));
        }

        [Usage( "AnimalSpeed" )]
		[Description( "Run as if mounted." )]        
		private static void AnimalSpeed_OnCommand( CommandEventArgs e )
		{
            PlayerMobile from = e.Mobile as PlayerMobile;

            if (from.Animal != 0)
            {
                if (from.AnimalSpeed == 0)
                {
                    if (from.Mount == null)
                    {
                        from.AnimalSpeed = 1;
                        from.Send(SpeedControl.MountSpeed);
                        from.SendMessage("Animal run mode enabled.");
                    }
                }
                else if (from.AnimalSpeed == 1)
                {
                    if (from.Mount == null)
                    {
                        {
                            from.AnimalSpeed = 0;
                            from.Send(SpeedControl.Disable);
                            from.SendMessage("Animal run mode disabled.");
                        }
                    }
                }
            }
            else
                from.SendMessage("Only animals can use this command");
		}
    }
}