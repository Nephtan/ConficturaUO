/*************************************************************
 * File: CheckClonesCommand.cs
 * Description: Administrative command for triggering offline
 *              player clone creation.
 *************************************************************/
using System.ComponentModel;
using Server.Commands;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Registers the <c>CheckClones</c> command so administrators can
    /// manually synchronize offline player clones with their originals.
    /// </summary>
    public static class CheckClonesCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "CheckClones",
                AccessLevel.Administrator,
                new CommandEventHandler(CheckClones_OnCommand)
            );
        }

        [Usage("CheckClones")]
        [Description("Creates clones of all offline players who don't currently have one.")]
        public static void CheckClones_OnCommand(CommandEventArgs e)
        {
            CloneOfflinePlayerCharacters.CheckFirstRun();
            e.Mobile.SendMessage("Clone check initiated.");
        }
    }
}