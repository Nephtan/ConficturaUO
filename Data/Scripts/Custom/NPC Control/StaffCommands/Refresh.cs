using System;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class RefreshCommand
    {
        public static AccessLevel accessLevel = AccessLevel.Counselor;

        public static void Initialize()
        {
            CommandSystem.Register(
                "Refresh",
                accessLevel,
                new CommandEventHandler(Refresh_OnCommand)
            );
        }

        [Usage("Refresh")]
        [Description("Sets all targets stats to full.")]
        public static void Refresh_OnCommand(CommandEventArgs e)
        {
            if (e == null || e.Mobile == null || e.Mobile.Deleted)
                return;

            e.Mobile.Target = new FreshTarget();
        }

        public class FreshTarget : Target
        {
            public FreshTarget()
                : base(12, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from == null || from.Deleted)
                    return;

                Mobile target = targeted as Mobile;

                if (target == null || target.Deleted)
                    return;

                if (!from.CanSee(target))
                {
                    from.SendMessage("The target is not in your line of sight!");
                    return;
                }

                target.Hits = target.HitsMax;
                target.Mana = target.ManaMax;
                target.Stam = target.StamMax;
            }
        }
    }
}
