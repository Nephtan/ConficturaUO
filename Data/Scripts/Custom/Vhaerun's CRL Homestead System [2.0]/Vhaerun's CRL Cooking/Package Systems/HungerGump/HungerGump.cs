// une petite commande .hungry parce que j'en ai marre de pas savoir si j'ai faim ou pas
// j'aurais pu faire plus simple pour la commande mais je voulais quelle soit integrable dans le Help
// elle affiche un pti gump mimi , qui vous donne num�riquement la valeur de votre faim et soif

/* a small hungry order because I have some enough of step knowledge if I am hungry or not
I could have made simpler for the order but I wanted which is integrable in Help it posts
a pti gump mimi, which numerically gives you the value of your hunger and thirst */

using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
    public class Hungry
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "Hunger",
                AccessLevel.Player,
                new CommandEventHandler(Hungry_OnCommand)
            );
            CommandSystem.Register(
                "Thirst",
                AccessLevel.Player,
                new CommandEventHandler(Hungry_OnCommand)
            );
        }

        [Usage("Hunger || Thirst")]
        [Description("Show your level of hunger and thirst.")]
        public static void Hungry_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(gumpfaim));
            from.SendGump(new gumpfaim(from));
        }
    }
}

namespace Server.Gumps
{
    public class gumpfaim : Gump
    {
        public gumpfaim(Mobile from)
            : base(0, 0)
        {
            Closable = true;
            Dragable = true;

            AddPage(0);

            //AddBackground(
            //    10,
            //    0, /*295*/
            //    245,
            //    144,
            //    5054
            //);
            AddBackground(
                14,
                27, /*261*/
                175,
                90,
                3500
            );
            AddLabel(
                60,
                52,
                from.Hunger < 6 ? 33 : 0,
                string.Format("Hunger: {0} / 20", from.Hunger)
            );
            AddLabel(
                60,
                75,
                from.Thirst < 6 ? 33 : 0,
                string.Format("Thirst: {0} / 20", from.Thirst)
            );
            AddItem(19, 52, 4155);
            AddItem(10, 75, 8093);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info) { }
    }
}
