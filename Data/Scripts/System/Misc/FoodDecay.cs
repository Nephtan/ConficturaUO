// FoodDecay.cs - Modified by Alari ( alarihyena@gmail.com )
// Includes hunger/thirst messages, opens the hunger/thirst gump when low, and
// lowers player HP if they become extremely dehydrated or begin to starve.
//
// There are two easy customizations you can make to this file:
//
//
// this code:  (don't actually modify this one, this is an example. Look further down.)
//
//    public static bool KeepAlive { get { return true; } }
//
// determines whether or not extreme hunger or thirst can kill the player.
//
// If true, the player's hit points will not be decreased if that would cause
// the player to go below 0 HP.
//
// If false, the player CAN die from starvation/dehydration.
//
//
// this code: (second verse, same as the first. Change the one further down.)
//
//    public static bool StaffImmune { get { return true; } }
//
// determines whether staff hunger and thirst will be allowed to decay below 10.
// (out of 20)
//
// If true, staff hunger/thirst will stop decaying below 10, and if lower when
// the timer fires it will be boosted back up to 10.
//
// If false, staff hunger/thirst can and will decay below 10 unless they eat and
// drink.
//
// I allow it to decay to 10 so that counselors, etc can still eat and drink,
// but won't be inconvenienced by it should they not wish to do so.


using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Misc
{
    public class FoodDecayTimer : Timer
    {
        // if true, staff hunger/thirst will not decay.
        public static bool StaffImmune
        {
            get { return true; }
        }

        public static void Initialize()
        {
            new FoodDecayTimer().Start();
        }

        public FoodDecayTimer()
            : base(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5))
        {
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            FoodDecay();
        }

        public static void FoodDecay()
        {
            foreach (NetState state in NetState.Instances)
            {
                HungerDecay(state.Mobile);
                ThirstDecay(state.Mobile);
            }
        }

        public static void HungerDecay(Mobile m)
        {
            if (m != null)
            {
                if (m is PlayerMobile)
                {
                    bool InsideInn = false;

                    if (Server.Misc.MyServerSettings.Belly())
                    {
                        if (m.Region is PublicRegion)
                            InsideInn = true;
                        else if (m.Region is CrashRegion)
                            InsideInn = true;
                        else if (m.Region is PrisonArea)
                            InsideInn = true;
                        else if (m.Region is SafeRegion)
                            InsideInn = true;
                        else if (m.Region is StartRegion)
                            InsideInn = true;
                        else if (m.Region is HouseRegion)
                            InsideInn = true;
                    }

                    if (m.Skills[SkillName.Camping].Value >= Utility.RandomMinMax(1, 200)) { }
                    else if (InsideInn) { }
                    else if (Server.Items.BaseRace.NoFood(m.RaceID))
                    {
                        m.Hunger = 20;
                    }
                    else if (Server.Items.BaseRace.NoFoodOrDrink(m.RaceID))
                    {
                        m.Thirst = 20;
                        m.Hunger = 20;
                    }
                    else if (StaffImmune && (m.AccessLevel > AccessLevel.Player))
                    {
                        m.Thirst = 20;
                        m.Hunger = 20;
                    }
                    else
                    {
                        if (m.Hunger >= 1)
                        {
                            m.Hunger -= 1;

                            if ((m.Hunger < 6) || (m.HasGump(typeof(gumpfaim))))
                            {
                                try
                                {
                                    m.CloseGump(typeof(gumpfaim));
                                    m.SendGump(new Server.Gumps.gumpfaim(m)); // popup hunger gump.
                                }
                                catch { }
                            }

                            switch (m.Hunger)
                            {
                                case 15:
                                {
                                    m.SendMessage("You are a little hungry.");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am a little hungry."
                                    );
                                    break;
                                }
                                case 10:
                                {
                                    m.SendMessage("You are hungry.");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am hungry."
                                    );
                                    break;
                                }
                                case 5:
                                {
                                    m.SendMessage(33, "You are really hungry.");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am really hungry."
                                    );

                                    if (m.Mana > (m.ManaMax / 20))
                                    {
                                        m.Mana -= (m.ManaMax / 20);
                                    }

                                    break;
                                }
                                case 2:
                                {
                                    m.SendMessage(33, "You are very hungry!");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am very hungry!"
                                    );

                                    if (m.Stam > (m.StamMax / 20))
                                    {
                                        m.Stam -= (m.StamMax / 20);
                                    }

                                    if (m.Mana > (m.ManaMax / 10))
                                    {
                                        m.Mana -= (m.ManaMax / 10);
                                    }

                                    break;
                                }
                                case 1:
                                {
                                    m.SendMessage("You are dying of starvation!");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am dying of starvation!"
                                    );

                                    if (m.Hits > (m.HitsMax / 20))
                                    {
                                        m.Hits -= (m.HitsMax / 20);
                                    }

                                    if (m.Stam > (m.StamMax / 10))
                                    {
                                        m.Stam -= (m.StamMax / 10);
                                    }

                                    if (m.Mana > (m.ManaMax / 5))
                                    {
                                        m.Mana -= (m.ManaMax / 5);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
                else if (m is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)m;

                    if (bc.Controlled && m.Hunger >= 1)
                    {
                        m.Hunger -= 1;
                    }
                }
            }
        }

        public static void ThirstDecay(Mobile m)
        {
            if (m != null)
            {
                if (m is PlayerMobile)
                {
                    bool InsideInn = false;

                    if (Server.Misc.MyServerSettings.Belly())
                    {
                        if (m.Region is PublicRegion)
                            InsideInn = true;
                        else if (m.Region is CrashRegion)
                            InsideInn = true;
                        else if (m.Region is PrisonArea)
                            InsideInn = true;
                        else if (m.Region is SafeRegion)
                            InsideInn = true;
                        else if (m.Region is StartRegion)
                            InsideInn = true;
                        else if (m.Region is HouseRegion)
                            InsideInn = true;
                    }

                    if (m.Skills[SkillName.Camping].Value >= Utility.RandomMinMax(1, 200)) { }
                    else if (InsideInn) { }
                    else if (Server.Items.BaseRace.NoFood(m.RaceID))
                    {
                        m.Thirst = 20;
                    }
                    else if (Server.Items.BaseRace.NoFoodOrDrink(m.RaceID))
                    {
                        m.Thirst = 20;
                        m.Hunger = 20;
                    }
                    else if (StaffImmune && (m.AccessLevel > AccessLevel.Player))
                    {
                        m.Thirst = 20;
                        m.Hunger = 20;
                    }
                    else
                    {
                        if (m.Thirst >= 1)
                        {
                            m.Thirst -= 1;

                            if ((m.Thirst < 6) || (m.HasGump(typeof(gumpfaim))))
                            {
                                try
                                {
                                    m.CloseGump(typeof(gumpfaim));
                                    m.SendGump(new Server.Gumps.gumpfaim(m)); // popup Thirst gump.
                                }
                                catch { }
                            }

                            switch (m.Thirst)
                            {
                                case 15:
                                {
                                    m.SendMessage("You are a little thirsty.");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am a little thirsty."
                                    );
                                    break;
                                }
                                case 10:
                                {
                                    m.SendMessage("You are thirsty.");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am thirsty."
                                    );
                                    break;
                                }
                                case 5:
                                {
                                    m.SendMessage(33, "You are really thirsty.");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am really thirsty."
                                    );

                                    if (m.Mana > (m.ManaMax / 20))
                                    {
                                        m.Mana -= (m.ManaMax / 20);
                                    }

                                    break;
                                }
                                case 2:
                                {
                                    m.SendMessage(33, "You are exhausted from dehydration!");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am very thirsty!"
                                    );

                                    if (m.Stam > (m.StamMax / 20))
                                    {
                                        m.Stam -= (m.StamMax / 20);
                                    }

                                    if (m.Mana > (m.ManaMax / 10))
                                    {
                                        m.Mana -= (m.ManaMax / 10);
                                    }

                                    break;
                                }
                                case 1:
                                {
                                    m.SendMessage(33, "You are dying of dehydration!");
                                    m.LocalOverheadMessage(
                                        MessageType.Emote,
                                        0xB1F,
                                        true,
                                        "I am dying of dehydration!"
                                    );

                                    if (m.Hits > (m.HitsMax / 20))
                                    {
                                        m.Hits -= (m.HitsMax / 20);
                                    }

                                    if (m.Stam > (m.StamMax / 10))
                                    {
                                        m.Stam -= (m.StamMax / 10);
                                    }

                                    if (m.Mana > (m.ManaMax / 5))
                                    {
                                        m.Mana -= (m.ManaMax / 5);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
                else if (m is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)m;

                    if (bc.Controlled && m.Thirst >= 1)
                    {
                        m.Thirst -= 1;
                    }
                }
            }
        }
    }
}
