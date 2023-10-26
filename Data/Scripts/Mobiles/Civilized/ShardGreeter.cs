using System;
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;
using System.Net;
using System.Diagnostics;
using Server.Accounting;

namespace Server.Mobiles
{
    public class ShardGreeter : BasePerson
    {
        public override bool IsInvulnerable
        {
            get { return true; }
        }

        [Constructable]
        public ShardGreeter()
            : base()
        {
            Direction = Direction.South;
            CantWalk = true;
            Female = true;

            SpeechHue = Utility.RandomTalkHue();
            Hue = 1009;
            NameHue = 0x92E;
            Body = 0x191;
            Name = NameList.RandomName("female");
            Title = "the gypsy";

            FancyDress dress = new FancyDress(0xAFE);
            dress.ItemID = 0x1F00;
            AddItem(dress);
            AddItem(new Sandals());

            Utility.AssignRandomHair(this);
            HairHue = 0x92E;
            HairItemID = 8252;
            FacialHairItemID = 0;
        }

        public ShardGreeter(Serial serial)
            : base(serial) { }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new ShardGreeterEntry(from, this));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public class ShardGreeterEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public ShardGreeterEntry(Mobile from, Mobile giver)
                : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;

                if ((m_Mobile.X == 3567 && m_Mobile.Y == 3404) || m_Mobile.RaceID > 0)
                {
                    m_Giver.PlaySound(778);
                    mobile.CloseGump(typeof(GypsyTarotGump));
                    mobile.CloseGump(typeof(WelcomeGump));
                    mobile.CloseGump(typeof(RacePotions.RacePotionsGump));
                    mobile.SendGump(new GypsyTarotGump(m_Mobile, 0));
                }
                else
                {
                    m_Giver.Say("Please, " + m_Mobile.Name + ". Take a seat and we will begin.");
                }
            }
        }
    }
}

namespace Server.Gumps
{
    public class MonsterGump : Gump
    {
        public MonsterGump(Mobile from)
            : base(50, 50)
        {
            string color = "#efc99b";
            from.SendSound(0x4A);

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddImage(0, 0, 7034, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddHtml(
                12,
                12,
                425,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">"
                    + Server.Items.BaseRace.StartName(from.RaceID)
                    + " - "
                    + from.Name
                    + "</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            AddHtml(
                12,
                42,
                509,
                351,
                @"<BODY><BASEFONT Color=" + color + ">" + from.Profile + "</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            AddButton(496, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            from.SendSound(0x4A);
        }
    }

    public class WelcomeGump : Gump
    {
        public WelcomeGump(Mobile from)
            : base(400, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddImage(0, 0, 2610, Server.Misc.PlayerSettings.GetGumpHue(from));

            int header = 11474;
            if (MyServerSettings.ServerName() == "Ruins & Riches")
            {
                header = 11377;
            }
            AddImage(13, 12, header, 2126);

            AddHtml(
                13,
                58,
                482,
                312,
                @"<BODY><BASEFONT Color=#94C541>For you, the day was ordinary compared to any other. However, when the evening sun finally disappeared below the landscape, you retired to bed where sleep felt restless, and the dreams were more vivid. You cannot recall the details of the dream, but you remember being drawn from this world through a swirling portal. When you woke up, you found yourself in this forest. Your night clothes have disappeared, and you are now dressed in medieval garb, holding a light in your hand.<BR><BR>Through the darkness of the night, you see a campfire just ahead. A colorful tent is next to it with the welcoming glow of lanterns about. The sounds of the nearby stream provides a tranquility, and you can see a grizzly bear soundly sleeping next to the warmth of the fire. If you were to shrug off the worries of your current life, you feel like this would be the place to start anew. You decide to see who is camping here and to perhaps find out where you are.</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(468, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            from.SendSound(0x4A);
        }
    }

    public class GypsyTarotGump : Gump
    {
        public bool visitLodor(Mobile from)
        {
            bool visited = false;

            Account a = from.Account as Account;

            if (a == null)
                return false;

            int index = 0;

            for (int i = 0; i < a.Length; ++i)
            {
                Mobile m = a[i];

                if (m != null)
                {
                    if (PlayerSettings.GetDiscovered(m, "the Land of Lodoria"))
                    {
                        visited = true;
                    }
                }
                ++index;
            }
            return visited;
        }

        public bool visitSavage(Mobile from)
        {
            bool visited = false;

            Account a = from.Account as Account;

            if (a == null)
                return false;

            int index = 0;

            for (int i = 0; i < a.Length; ++i)
            {
                Mobile m = a[i];

                if (m != null)
                {
                    if (PlayerSettings.GetDiscovered(m, "the Savaged Empire"))
                    {
                        visited = true;
                    }
                }
                ++index;
            }
            return visited;
        }

        public int pageShow(Mobile from, int page, bool forward)
        {
            if (from.RaceID > 0)
            {
                if (forward)
                {
                    page++;

                    if (Server.Items.BaseRace.IsGood(from) && page == 2)
                    {
                        page++;
                    }
                    if (!visitLodor(from) && page == 3)
                    {
                        page++;
                    }
                    if ((Server.Items.BaseRace.IsGood(from) || !visitLodor(from)) && page == 4)
                    {
                        page++;
                    }
                    if (page > 4)
                    {
                        page = 20;
                    }
                }
                else
                {
                    page--;

                    if ((Server.Items.BaseRace.IsGood(from) || !visitLodor(from)) && page == 4)
                    {
                        page--;
                    }
                    if (!visitLodor(from) && page == 3)
                    {
                        page--;
                    }
                    if (Server.Items.BaseRace.IsGood(from) && page == 2)
                    {
                        page--;
                    }
                    if (page < 1)
                    {
                        page = 20;
                    }
                }
            }
            else
            {
                if (forward)
                {
                    page++;

                    if (!visitLodor(from) && page == 10)
                    {
                        page++;
                    }
                    if (!visitLodor(from) && page == 11)
                    {
                        page++;
                    }
                    if (!visitSavage(from) && page == 12)
                    {
                        page++;
                    }
                    if (!MyServerSettings.AllowAlienChoice() && page == 13 && from.RaceID == 0)
                    {
                        page++;
                    }
                    if (page > 13)
                    {
                        page = 20;
                    }
                }
                else
                {
                    page--;

                    if (!MyServerSettings.AllowAlienChoice() && page == 13 && from.RaceID == 0)
                    {
                        page--;
                    }
                    if (!visitSavage(from) && page == 12)
                    {
                        page--;
                    }
                    if (!visitLodor(from) && page == 11)
                    {
                        page--;
                    }
                    if (!visitLodor(from) && page == 10)
                    {
                        page--;
                    }
                    if (page < 1)
                    {
                        page = 20;
                    }
                }
            }
            return page;
        }

        public static string GypsySpeech(Mobile from)
        {
            string monst = "";
            string races = "\n\nLastly, this realm ";
            if (MyServerSettings.MonstersAllowed())
            {
                monst =
                    " There is a shelf over there with interesting potions you may want. So if you want one, drink it now and return here for your tarot card reading.";
                races =
                    "You may not actually be of human descent. You might actually be an ogre, troll, or satyr. There are many creatures you can actually be. If you want to explore these ideas, look through my potion shelf behind you, to the left of the entryway of my tent. There you will find various potions of alteration that can change your life. If you choose one of these creatures to be, consider changing your name to better represent the creature you chose to play. This leads me to my final words of advice... \n\nThis realm ";
            }

            return "Greetings, "
                + from.Name
                + "...you are about to enter one of the lands of "
                + MyServerSettings.ServerName()
                + ". Not too long ago, the Stranger arrived in Sosaria and foiled the evil plans of Exodus. Castle Exodus lies in ruins, and Sosaria is once again trying to rebuild in peace. Many vile monsters still roam the land, however, but hardy adventurers have bravely sought to rid us of these terrors. To begin your journey, simply choose your fate from my deck of tarot cards (begin by pressing the top-right button). Once you look through the deck (pressing the arrow buttons), you can draw a card of your choice (by pressing the OK button on the top-right)."
                + monst
                + "\n\nNow let me tell you some things about the world that fate has brought you to. Traveling the lands can be dangerous, as other adventurers may decide to kill you for your gold or property. The taverns, inns, and banks are safe from such threats, but there are also many guards in the settlements to keep the peace. They have been known to quickly dispatch murderers and criminals. You can choose to protect yourself from causing harm or being harmed by other adventurers by speaking to Thuvia, who is located just outside of this tent."
                + "\n\nThere are many merchants throughout the settlements. They are not able to sell or buy everything they normally deal in, as their choices of what they buy and sell change from day to day."
                + "\n\nThere are secrets to be learned and magic items to be found in the many dungeons. Each settlement in Sosaria is somewhat safe in the surrounding land, so hunting for food or skins should be relatively safe. I cannot say the same for other lands. There is also a minor dungeon near each settlement of Sosaria if you wish to begin traversing the dangers below before you are fully prepared. Be warned that the vile creatures are not all that you must face. There are many deadly traps in the rooms and halls of these places that could kill you quicker than the monster you may be fleeing from."
                + "\n\nPrepare to go forth and make your life your own. Become the finest craftsman in the land, a wealthy owner of lands and castles, the mightiest warrior, or even the most powerful wizard. The choice is yours."
                + "\n\nThis world can be traveled alone or with friends, where one could have great adventures. Like I stated already, your chosen course in life is whatever you want to do. You may be a mighty warrior or powerful wizard. You may simply start a potion shop near a large city. You may be a master of beasts or a mystical bard. This is a world where great wealth and artifacts can be obtained from the many dungeons throughout the land. You may be slain by a creature, die from hunger, get lost in the dark, or stumble onto a deadly trap. You may find powerful relics and enough gold to build your own castle."
                + "\n\n"
                + races
                + "is best served if you have a name that is commensurate with this rich fantasy world. You have one final chance to change your name if you need to, by simply using my journal on the table behind me. You cannot have a name that someone else already has, so it must be unique. If you want to change your name, proceed to the table where I keep my journal. Once your name is changed, return here for your tarot card reading.";
        }

        public GypsyTarotGump(Mobile from, int page)
            : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddImage(0, 0, 2611, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddImage(10, 8, 1102);

            if (page > 0 && page < 50)
            {
                int prev = pageShow(from, page, false);
                int next = pageShow(from, page, true);

                AddImage(640, 8, cardGraphic(page, from.RaceID));

                AddItem(269, 349, 4775);
                AddItem(586, 349, 4776);

                if (prev != 20)
                    AddButton(317, 375, 4014, 4014, prev, GumpButtonType.Reply, 0); // prev is always 20 when it's the last card
                if (next != 20)
                    AddButton(552, 375, 4005, 4005, next, GumpButtonType.Reply, 0); // next is always 20 when it's the last card

                AddHtml(
                    269,
                    12,
                    240,
                    20,
                    @"<BODY><BASEFONT Color=#DEC6DE>"
                        + cardText(page, 1, from.RaceID)
                        + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddHtml(
                    271,
                    47,
                    356,
                    297,
                    @"<BODY><BASEFONT Color=#DEC6DE>"
                        + cardText(page, 2, from.RaceID)
                        + "<BR><BR>"
                        + cardText(page, 3, from.RaceID)
                        + "</BASEFONT></BODY>",
                    (bool)false,
                    scrollBar(page, from.RaceID)
                );

                AddItem(566, 12, 4777);
                AddItem(580, 26, 4779);
                AddButton(599, 11, 4023, 4023, page + 100, GumpButtonType.Reply, 0);
            }
            else
            {
                int header = 11473;
                if (MyServerSettings.ServerName() == "Ruins & Riches")
                {
                    header = 11376;
                }

                AddImage(271, 13, header, 2813);

                AddHtml(
                    278,
                    73,
                    604,
                    320,
                    @"<BODY><BASEFONT Color=#DEC6DE>" + GypsySpeech(from) + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
                AddButton(819, 14, 4011, 4011, 99, GumpButtonType.Reply, 0);
                AddItem(851, 11, 4773);
            }
        }

        public int cardGraphic(int page, int creature)
        {
            int val = 0;

            if (creature > 0)
            {
                switch (page)
                {
                    case 1:
                        val = 1340;
                        break;
                    case 2:
                        val = 1341;
                        break;
                    case 3:
                        val = 1342;
                        break;
                    case 4:
                        val = 1343;
                        break;
                }
            }
            else
            {
                switch (page)
                {
                    case 1:
                        val = 1106;
                        break;
                    case 2:
                        val = 1105;
                        break;
                    case 3:
                        val = 1110;
                        break;
                    case 4:
                        val = 1122;
                        break;
                    case 5:
                        val = 1116;
                        break;
                    case 6:
                        val = 1108;
                        break;
                    case 7:
                        val = 1104;
                        break;
                    case 8:
                        val = 1120;
                        break;
                    case 9:
                        val = 1109;
                        break;
                    case 10:
                        val = 1111;
                        break;
                    case 11:
                        val = 1112;
                        break;
                    case 12:
                        val = 1119;
                        break;
                    case 13:
                        val = 1118;
                        break;
                }
            }
            return val;
        }

        public bool scrollBar(int page, int creature)
        {
            bool scroll = false;

            if (creature > 0)
            {
                switch (page)
                {
                    case 1:
                        scroll = true;
                        break;
                    case 2:
                        scroll = true;
                        break;
                    case 3:
                        scroll = true;
                        break;
                    case 4:
                        scroll = true;
                        break;
                }
                if (Server.Items.BaseRace.GetUndead(creature))
                    scroll = true;
            }
            else
            {
                switch (page)
                {
                    case 1:
                        scroll = true;
                        break;
                    case 2:
                        scroll = true;
                        break;
                    case 3:
                        scroll = true;
                        break;
                    case 4:
                        scroll = true;
                        break;
                    case 5:
                        scroll = true;
                        break;
                    case 6:
                        scroll = true;
                        break;
                    case 7:
                        scroll = true;
                        break;
                    case 8:
                        scroll = true;
                        break;
                    case 9:
                        scroll = true;
                        break;
                    case 10:
                        scroll = true;
                        break;
                    case 11:
                        scroll = true;
                        break;
                    case 12:
                        scroll = true;
                        break;
                    case 13:
                        scroll = true;
                        break;
                }
            }
            return scroll;
        }

        public string cardText(int page, int section, int creature)
        {
            string card = "";
            string town = "";
            string text = "";
            string lodor =
                "Most adventurers are born in the Land of Sosaria and only hear tales and legends of faraway lands. One of these lands is the elven world of Lodoria, which is slightly larger than Sosaria and has more challenging dungeons. However, Lodoria does offer familiar locations that veteran adventurers hold dear, such as Shame, Destard, and Wrong dungeons. The land is dotted with villages and cities, all inhabited by the benevolent elven people. The drow, a more malevolent faction of elves, seek to destroy those who embrace the light and impose their rule underground. If you choose to embark on your journey in Lodoria, you will be a human who grew up in this unfamiliar land, with no connection to Sosaria.";

            string fate = "If you choose this fate, ";
            switch (Utility.RandomMinMax(0, 8))
            {
                case 1:
                    fate = "If you choose this card, ";
                    break;
                case 2:
                    fate = "If you take this card, ";
                    break;
                case 3:
                    fate = "If this is the card you want, ";
                    break;
                case 4:
                    fate = "If this card is yours, ";
                    break;
                case 5:
                    fate = "If this fate is meant for you, ";
                    break;
                case 6:
                    fate = "If you draw this card, ";
                    break;
                case 7:
                    fate = "If you choose this path, ";
                    break;
                case 8:
                    fate = "If you take this road, ";
                    break;
            }

            string begin = "you will begin your journey";
            switch (Utility.RandomMinMax(0, 8))
            {
                case 1:
                    begin = "you will start your life";
                    break;
                case 2:
                    begin = "you will enter the world";
                    break;
                case 3:
                    begin = "you will be a citizen";
                    break;
                case 4:
                    begin = "you will have a new life";
                    break;
                case 5:
                    begin = "you may start a new life";
                    break;
                case 6:
                    begin = "you may have a new home";
                    break;
                case 7:
                    begin = "you may begin your journey";
                    break;
                case 8:
                    begin = "you may begin a new life";
                    break;
            }

            fate = fate + begin;

            if (creature > 0)
            {
                town = Server.Items.BaseRace.StartName(creature);
                string undead = "";
                if (Server.Items.BaseRace.GetUndead(creature))
                {
                    undead =
                        " Although you do not remember your past life, you feel different from the other undead. It seems that you have retained your soul, and this is bound to catch the attention of other undead. Consequently, they are likely to attack you, just as they do with the living.";
                }

                if (Server.Items.BaseRace.BloodDrinker(creature))
                {
                    undead =
                        undead
                        + " Having a soul, however, means that you can safely walk the land during daylight hours.";
                }

                switch (page)
                {
                    case 1:
                        card = "THE DAY";
                        text =
                            fate
                            + " "
                            + Server.Items.BaseRace.StartSentence(town)
                            + " of Sosaria."
                            + undead
                            + " This world has experienced three ages of darkness, during which a stranger emerged from a distant land to bring light to each of these events. Following the defeats of Mondain, Minax, and Exodus in their wicked schemes, Sosaria has attained a state of peace and prosperity. While most individuals desire to live humble lives as simple villagers, there are those who yearn to explore the ancient dungeons, tombs, ruins, and crypts scattered throughout the world. Embarking on this path will lead you to embrace the ways of civilized society, but in doing so, your own kindred will likely banish you from their presence. This matters little to you, as your preference lies in seeking fame and fortune in a world freed from the grip of the most formidable evils it has ever witnessed.";
                        break;

                    case 2:
                        card = "THE NIGHT";
                        text =
                            fate
                            + " "
                            + Server.Items.BaseRace.StartSentence(town)
                            + " of Sosaria."
                            + undead
                            + " This fate in Sosaria offers a more challenging life, where you have chosen to embrace your monstrous ways and seek power for yourself, leaving others of your kind behind. You will have the opportunity to become a grandmaster in 13 different skills, instead of the usual 10. Resurrecting with tributes will cost double the amount, potentially forcing you to face penalties. You will not be permitted to enter any civilized areas, unless you find a way to disguise yourself. However, there are exceptions for certain public areas like inns, taverns, and banks. Guards will attack you immediately upon sight, merchants will try to chase you away, and you will be unable to join any local guilds except for the Assassin, Thief, and Black Magic guilds. This is due to the fact that you are seen as a murderous beast. Nevertheless, everything you need can be found throughout the world, allowing you to embark on your journey.";
                        break;

                    case 3:
                        card = "THE LIGHT";
                        text =
                            fate
                            + " "
                            + Server.Items.BaseRace.StartSentence(town)
                            + " of Lodoria."
                            + undead
                            + " This world was once ruled by dwarves, but now their cities lie in ruins, and the elves have become the dominant civilized race. They have pushed the drow back into their deep underdark lairs. Many individuals now seek to explore this world. While most prefer to lead humble lives as simple villagers, there are those who yearn to delve into the ancient dungeons, tombs, ruins, and crypts scattered across the land. Embarking on this path will guide you towards assimilating into the elven society, where you can pursue glory and wealth that exceeds your wildest dreams.";
                        break;

                    case 4:
                        card = "THE DARK";
                        text =
                            fate
                            + " "
                            + Server.Items.BaseRace.StartSentence(town)
                            + " of Lodoria."
                            + undead
                            + " In Lodoria, your fate is to lead a more challenging life. Here, you may have left others of your kind behind, but you have made the choice to embrace your monstrous ways and seek power for yourself. As a result, you will have the ability to become a grandmaster in 13 different skills, exceeding the usual 10. However, there are consequences to consider. The cost of tributes for resurrection will be doubled, potentially forcing you to resurrect with penalties. Additionally, you will be prohibited from entering any civilized areas unless you can find a way to disguise yourself. There are a few exceptions, such as public areas like inns, taverns, and banks. It is important to note that guards will attack you on sight, merchants will try to chase you away, and you will not be able to join any local guilds, with the exception of the Assassin, Thief, and Black Magic guilds. This is due to the fact that you are perceived as a murderous beast. However, everything you need can still be found throughout the world, allowing you to embark on your journey.";
                        break;
                }
            }
            else
            {
                switch (page)
                {
                    case 1:
                        card = "THE EMPEROR";
                        town = "The City of Britain";
                        text =
                            fate
                            + " in the capital city of Sosaria and home of Lord British, the ruler of the land. His magnificent castle graces the northern part of the city, commanding a view of Britanny Bay. This towering structure stands as a testament to the finest architectural achievements of the modern era. Loyal subjects pay homage to His Majesty and reaffirm their allegiance whenever they find themselves in the vicinity of his castle. Whispers and rumors circulate in taverns, hinting at a concealed secret hidden beneath the castle's depths, so enigmatic that even the city's residents remain oblivious to its existence. Surrounding the city are sprawling farms, as well as cemeteries dedicated to the citizens and another reserved exclusively for the British Royal Family. Some have been witnessed entering the British tomb under the cover of darkness.";
                        break;

                    case 2:
                        card = "THE DEVIL";
                        town = "The Town of Devil Guard";
                        text =
                            fate
                            + " in a town completely enclosed by the Great Mountains during the Third Age of Darkness, it could only be accessed through a magical gate. However, after the destruction of Exodus, a cavernous tunnel was formed, offering an alternative path. According to ancient legends, a castle descended from the sky, colliding with the mountains and forming the valley where Devil Guard was eventually established. The tales narrate that the town was founded and populated by the inhabitants of the sky castle, who named it as they sought to safeguard others from the ancient daemons.";
                        break;

                    case 3:
                        card = "THE HERMIT";
                        town = "The Village of Grey";
                        text =
                            fate
                            + " in this village where the inhabitants, during the Third Age of Darkness, provided several clues to the Stranger who defeated Exodus. It was even rumored that they sold ships capable of flying to the stars, but none remaining possess the knowledge to create such marvels. According to legends, the Stranger soared through the sky and manipulated time and reality, causing a castle to regress in time and crash into the ancient land of Sosaria. Now, the village often serves as a sanctuary for those who seek solitude. Although there are no mountains for mining, some individuals have excavated beneath the forest floor to extract ore. Whispers among necromancers suggest that the cemetery holds a secret, spoken only in hushed tones.";
                        break;

                    case 4:
                        card = "THE TOWER";
                        town = "The City of Montor";
                        text =
                            fate
                            + " in a vast city where courage is particularly upheld and there are all the necessary shops for everyone. The inhabitants of Montors are knowledgeable about the mystical Four Cards that the Stranger needs to defeat Exodus, as well as the tales of the lost shrines of Ambrosia. Montor is the most visited and largest city in Sosaria, thanks to the trade brought by ships. To the east, there is a small mine, and to the northeast, there is a tower. This tower is rumored to be the home of a vile lich with a magic mirror that can traverse dimensions, although these rumors are often told over a tankard of ale.";
                        break;

                    case 5:
                        card = "THE MAGICIAN";
                        town = "The Town of Moon";
                        text =
                            fate
                            + " in the town where, during the Third Age of Darkness, there existed a city brimming with mages. Alas, these mages were of the corrupt and deceitful variety. Among them resided Erstam, who dedicated his days to conducting experiments in pursuit of immortality. However, when Lord British banished the wicked mages from the town following the devastation caused by Exodus, Erstam and his cohorts resolved to seek refuge on Serpent Island, where no authority could rein them in. Presently, this once tumultuous place has transformed into a serene haven, attracting visitors who engage in farming and coastal fishing. It enjoys popularity due to its moderate size, which allows for numerous bustling markets to explore. Adventurers frequently arrive from the neighboring desert, regaling tales of riches acquired from the Ancient Pyramid.";
                        break;

                    case 6:
                        card = "THE FOOL";
                        town = "The Town of Mountain Crest";
                        text =
                            fate
                            + " on some small islands in Sosaria that have a harsh wintery landscape, and which others believe is foolish to inhabit. Along with this town, there are also settlements to the west and east. There are various caverns and dungeons within the mountains, as well as an unusual tower built by a wizard long ago. This place is one of the more difficult areas to live in, but a snowy region may be your fate if you choose it.";
                        break;

                    case 7:
                        card = "DEATH";
                        town = "The Undercity of Umbra";
                        text =
                            fate
                            + " in a place that remains unknown to many, as it was constructed as a sanctuary for practitioners of the necrotic arts. Nestled deep within the mountains, specifically to the southeast of Britain, the eerie corridors and caverns exude a haunting atmosphere. However, the necromancers have established shops to cater to their essential needs. The cavern surrounding the city stands as one of the tallest ever witnessed, with some suggesting that its height is even suitable for constructing a castle, away from the sun's rays. Additionally, a tomb for a death knight has been erected in close proximity, while the Fires of Hell are merely a hike away.";
                        break;

                    case 8:
                        card = "THE SUN";
                        town = "The Village of Yew";
                        text =
                            fate
                            + " in a valley of thick forest, just west of Britain and east of the Moon, where the sun nurtures the largest trees in Sosaria. Yew is one of the major trading hubs for wood in the land. During the Third Age of Darkness, the Stranger visited Yew and learned the secrets of the Great Earth Serpent. This knowledge allowed the Stranger to free the serpent that had been blocking their ship from reaching the Castle of Exodus on the Isle of Fire. Some speculate that freeing the serpent caused an imbalance in the cosmos, but such claims may just be drunken wizards spinning tales. There is a nearby cave where you can mine, but miners have discovered something on the southern side of the mountain range that they dare not enter.";
                        break;

                    case 9:
                        card = "THE HANGED MAN";
                        town = "The Britain Dungeons";
                        text =
                            "You may choose a fate in this world that has a more challenging life, where you become a fugitive from justice. If you choose this path, you will be able to become a grandmaster in 13 different skills instead of the normal 10. This is because you will rely on yourself to survive. Tributes for resurrection will cost double the amount, potentially forcing you to resurrect with penalties. You will not be allowed to enter any civilized areas unless you find a way to disguise yourself. The exceptions are certain public areas such as inns, taverns, and banks. Guards will attack you on sight, merchants will try to chase you away, and you will be unable to join any local guilds except for the Assassin, Thief, and Black Magic guilds. The reason for this is that you are wanted for murder. You may have actually committed the act, or you could have been framed. The murder was against a powerful figure, so the lands will never forgive the deed. Whether it's truth or falsehood, that is up to you to tell. Do what you will with your life. You can live a life of criminal pursuits, or you can eradicate the evil that lurks in the darkest places of the land. If you choose such a life, you will be on your own, and you must first escape from your prison cell. From there, it's best to head for Stonewall to the northwest, but you may go wherever you like. Everything you need can be found throughout the world.";
                        break;

                    case 10:
                        card = "THE HIEROPHANT";
                        town = "The City of Lodoria";
                        text =
                            lodor
                            + " This city is the capital of Lodor and it has every merchant you may need. The Castle of Knowledge lies on the high mountain on the western side where scholars learn the ways of the world. It has a mine to the north and a cemetery in the south valley. The continent is large and adventurers tell tales of dungeons like Shame, Despise, and a cavern of lizardmen. Another small settlement lies to the northwest. Do you choose this fate?";
                        break;

                    case 11:
                        card = "THE HIGH PRIESTESS";
                        town = "The City of Elidor";
                        text =
                            lodor
                            + " This city is located on the second-largest continent, diverse with both a forest-covered south and a wintry north. The High Priestess of Elidor built the famous Hall of Illusions, where many of her subjects practice prismatic magic. There are other settlements, such as Springvale to the east and Glacial Hills to the north. Drunken adventurers often speak of riches from Wrong, Deceit, and the Frozen Hells. Do you wish to draw this card?";
                        break;

                    case 12:
                        card = "STRNGTH";
                        town = "The Savaged Empire";
                        text =
                            "You may choose a barbaric way of life to begin your journey, and it is not for the weak but those bestowed with strength. If you choose this path, you will be able to become a grandmaster in 11 different skills instead of the usual 10. This is because you rely on yourself to survive in an untamed land. Your adventure will begin as a barbarian in the Savaged Empire, one of the most difficult lands in the realms. It is filled with dangerous animals and colossal dinosaurs. There are no safe places to hunt for food, making it equally perilous to practice your combat skills. However, you will start with some leather armor that will help you survive the dangers away from the settlements. You will also have a talisman to aid you in camping and cooking, enabling you to live off the land more effectively. Additional gold, food, bandages, a steel dagger, and a durable camping tent will be provided. Keep in mind that any dungeons you enter will be more deadly than those in Sosaria, so choose this path with great consideration. Your journey will begin in the Village of Kurak, where the outskirts offer plentiful hunting opportunities but also pose many dangers you may need to flee from. To the north, there is a cave where you can mine for precious ores.";
                        break;

                    case 13:
                        card = "THE STAR";
                        town = "The Shuttle Crash Site";
                        text =
                            "All the information that the doctor knew about you as a patient has been entered into your medical record. You were on the brink of death, but being placed in the stasis chamber seemed to have facilitated the healing process. Your scans revealed a severe head trauma, which means that when you wake up from your coma, you will have no recollection of your past or your identity (and you will start with no skills). Unfortunately, the space station is plummeting towards Sosaria because the Stranger has drained the fuel reserves. In a desperate move, the doctor decided to put your stasis chamber onto their last medical shuttle craft and set it on auto-pilot, hoping for the best. Fortunately, the craft landed safely on Sosaria, giving you a chance to continue your life on this primitive world. Your advantage lies in the fact that you come from a more advanced race of beings, allowing you to retain and learn more information (you can become a grandmaster in 40 different skills).\n\nHowever, due to your advanced knowledge of logic and science, you struggle to fully understand certain elements of Sosaria. Concepts such as magical resurrection and deities are beyond your comprehension (resurrecting at a shrine or healer will cost three times as much gold). The system shock from such a resurrection would undoubtedly take its toll (paying full tribute would result in a 10% loss in fame and karma, and a 5% loss in skills and attributes), which could prove to be devastating (not paying any tribute at all would result in a 20% loss in fame and karma, and a 10% loss in skills and attributes).\n\nWhile you can learn some skills classified as magic or divine, you always rationalize them through the lens of science. Unlike the inhabitants of this world, you don't believe in the concept of luck (you will never benefit from luck). At the beginning, you don't possess any of Sosaria's currency for bartering (you start with no gold), and you don't anticipate getting along with the guildmasters who practice crude trades (guild membership costs four times as much as normal).\n\nIf you choose this path, you will find yourself at your crashed shuttle craft, which marks the start of your adventure. A nearby computer terminal allows you to change your appearance slightly, altering your skin and hair tones to reflect your alien heritage.\n\nUpon awakening, you will have no recollection of your past identity. You will discover yourself near the crashed shuttle on top of a mountain. The computer system has provided instructions on setting up a power source using the remaining fuel. Interestingly, an alien creature attached itself to the shuttle and perished in the crash. You have been using it as a source of food and managed to survive for a few days. However, your supplies are now running out, your canteen is empty, and all you have is a knife. To ensure your survival, you must venture out into the unknown.";
                        break;
                }
            }

            if (section == 1)
            {
                return card;
            }
            else if (section == 2)
            {
                return town;
            }
            return text;
        }

        public void EnterLand(int page, Mobile m)
        {
            Point3D loc = new Point3D(2999, 1030, 0);
            Map map = Map.Sosaria;

            if (m.RaceID > 0)
            {
                string start = Server.Items.BaseRace.StartArea(m.RaceID);
                string world = "the Land of Sosaria";

                if (start == "cave")
                {
                    loc = new Point3D(497, 4066, 0);
                }
                else if (start == "ice")
                {
                    loc = new Point3D(625, 3224, 0);
                }
                else if (start == "pits")
                {
                    loc = new Point3D(180, 4075, 0);
                }
                else if (start == "sand")
                {
                    loc = new Point3D(91, 3244, 0);
                }
                else if (start == "sea")
                {
                    loc = new Point3D(27, 4077, 0);
                }
                else if (start == "sky")
                {
                    loc = new Point3D(289, 3222, 20);
                }
                else if (start == "swamp")
                {
                    loc = new Point3D(92, 3978, 0);
                }
                else if (start == "tomb")
                {
                    loc = new Point3D(362, 3966, 0);
                }
                else if (start == "water")
                {
                    loc = new Point3D(27, 4077, 0);
                }
                else if (start == "woods")
                {
                    loc = new Point3D(357, 4057, 0);
                }

                List<Item> belongings = new List<Item>();
                foreach (Item i in m.Backpack.Items)
                {
                    belongings.Add(i);
                }
                foreach (Item stuff in belongings)
                {
                    stuff.Delete();
                }
                Server.Items.BaseRace.RemoveMyClothes(m);

                m.AddToBackpack(new Gold(Utility.RandomMinMax(100, 150)));

                switch (Utility.RandomMinMax(1, 2))
                {
                    case 1:
                        m.AddToBackpack(new Dagger());
                        break;
                    case 2:
                        m.AddToBackpack(new LargeKnife());
                        break;
                }

                if (Server.Items.BaseRace.NoFoodOrDrink(m.RaceID))
                {
                    // NO NEED TO CREATE FOOD OR DRINK
                }
                else if (Server.Items.BaseRace.NoFood(m.RaceID))
                {
                    m.AddToBackpack(new Waterskin());
                }
                else if (Server.Items.BaseRace.BloodDrinker(m.RaceID))
                {
                    Item blood = new BloodyDrink();
                    blood.Amount = 10;
                    m.AddToBackpack(blood);
                }
                else if (Server.Items.BaseRace.BrainEater(m.RaceID))
                {
                    Item blood = new FreshBrain();
                    blood.Amount = 10;
                    m.AddToBackpack(blood);
                }
                else
                {
                    Container bag = new Bag();
                    int food = 10;
                    while (food > 0)
                    {
                        food--;
                        bag.DropItem(Loot.RandomFoods());
                    }
                    m.AddToBackpack(bag);
                    m.AddToBackpack(new Waterskin());
                }

                if (!Server.Items.BaseRace.NightEyes(m.RaceID))
                {
                    int light = 2;
                    while (light > 0)
                    {
                        light--;
                        switch (Utility.RandomMinMax(1, 3))
                        {
                            case 1:
                                m.AddToBackpack(new Torch());
                                break;
                            case 2:
                                m.AddToBackpack(new Lantern());
                                break;
                            case 3:
                                m.AddToBackpack(new Candle());
                                break;
                        }
                    }
                }

                if (page == 1)
                {
                    PlayerSettings.SetDiscovered(m, "the Land of Sosaria", true);
                }
                else if (page == 2)
                {
                    PlayerSettings.SetDiscovered(m, "the Land of Sosaria", true);
                    PlayerSettings.SetBardsTaleQuest(m, "BardsTaleWin", true);
                    m.Skills.Cap = 13000;
                    m.Kills = 1;
                    ((PlayerMobile)m).Profession = 1;
                }
                else if (page == 3)
                {
                    PlayerSettings.SetDiscovered(m, "the Land of Lodoria", true);
                    world = "the Land of Lodoria";
                }
                else if (page == 4)
                {
                    PlayerSettings.SetDiscovered(m, "the Land of Lodoria", true);
                    PlayerSettings.SetBardsTaleQuest(m, "BardsTaleWin", true);
                    m.Skills.Cap = 13000;
                    m.Kills = 1;
                    ((PlayerMobile)m).Profession = 1;
                    world = "the Land of Lodoria";
                }
                m.Profile = Server.Items.BaseRace.BeginStory(m, world);

                if (world == "the Land of Sosaria")
                    m.RaceHomeLand = 1;
                else if (world == "the Land of Lodoria")
                    m.RaceHomeLand = 2;
            }
            else
            {
                switch (page)
                {
                    case 1:
                        loc = new Point3D(2999, 1030, 0);
                        map = Map.Sosaria;
                        break;
                    case 2:
                        loc = new Point3D(1617, 1502, 2);
                        map = Map.Sosaria;
                        break;
                    case 3:
                        loc = new Point3D(851, 2062, 1);
                        map = Map.Sosaria;
                        break;
                    case 4:
                        loc = new Point3D(3220, 2606, 1);
                        map = Map.Sosaria;
                        break;
                    case 5:
                        loc = new Point3D(806, 710, 5);
                        map = Map.Sosaria;
                        break;
                    case 6:
                        loc = new Point3D(4546, 1267, 2);
                        map = Map.Sosaria;
                        break;
                    case 7:
                        MorphingTime.ColorOnlyClothes(m, 0, 1);
                        loc = new Point3D(2666, 3325, 0);
                        map = Map.Sosaria;
                        break;
                    case 8:
                        loc = new Point3D(2460, 893, 7);
                        map = Map.Sosaria;
                        break;
                    case 9:
                        loc = new Point3D(4104, 3232, 0);
                        map = Map.Sosaria;
                        break;
                    case 10:
                        loc = new Point3D(2111, 2187, 0);
                        map = Map.Lodor;
                        break;
                    case 11:
                        loc = new Point3D(2930, 1327, 0);
                        map = Map.Lodor;
                        break;
                    case 12:
                        loc = new Point3D(251, 1949, -28);
                        map = Map.SavagedEmpire;
                        break;
                    case 13:
                        loc = new Point3D(4109, 3775, 2);
                        map = Map.Sosaria;
                        break;
                }
            }

            m.MoveToWorld(loc, map);
            Effects.SendLocationParticles(
                EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                0x376A,
                9,
                32,
                0,
                0,
                5024,
                0
            );
            m.SendSound(0x65C);
            m.SendMessage("The card vanishes from your hand as you magically appear elsewhere.");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            from.CloseGump(typeof(GypsyTarotGump));
            from.CloseGump(typeof(WelcomeGump));
            from.CloseGump(typeof(RacePotions.RacePotionsGump));

            if (info.ButtonID == 99)
            {
                from.SendGump(new GypsyTarotGump(from, 1));
                from.SendSound(0x5BB);
            }
            else if (info.ButtonID >= 100)
            {
                int go = info.ButtonID - 100;
                EnterLand(go, from);
            }
            else if (info.ButtonID > 0)
            {
                int page = info.ButtonID;
                if (page == 20)
                {
                    page = 0;
                }
                from.SendGump(new GypsyTarotGump(from, page));
                from.SendSound(0x5B9);
            }
        }
    }
}
