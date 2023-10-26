using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using System.Collections;
using System.Globalization;

namespace Server.Items
{
    public class DynamicBook : Item
    {
        [Constructable]
        public DynamicBook()
            : base(0x1C11)
        {
            Weight = 1.0;

            if (BookTitle == "" || BookTitle == null)
            {
                ItemID = RandomThings.GetRandomBookItemID();
                Hue = Utility.RandomColor(0);
                SetBookCover(0, this);
                BookTitle = Server.Misc.RandomThings.GetBookTitle();
                Name = BookTitle;
                BookAuthor = Server.Misc.RandomThings.GetRandomAuthor();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Written by " + BookAuthor);
        }

        public class DynamicSythGump : Gump
        {
            public DynamicSythGump(Mobile from, DynamicBook book)
                : base(100, 100)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                this.AddPage(0);

                AddImage(0, 0, 30521);
                AddImage(51, 41, 11428);
                AddImage(52, 438, 11426);
                AddHtml(
                    275,
                    45,
                    445,
                    20,
                    @"<BODY><BASEFONT Color=#FF0000>"
                        + book.BookTitle
                        + " by "
                        + book.BookAuthor
                        + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddHtml(
                    275,
                    84,
                    445,
                    521,
                    @"<BODY><BASEFONT Color=#00FF06>" + book.BookText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.SendSound(0x54D);
            }
        }

        public class DynamicJediGump : Gump
        {
            public DynamicJediGump(Mobile from, DynamicBook book)
                : base(100, 100)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                this.AddPage(0);

                AddImage(0, 0, 30521);
                AddImage(51, 41, 11435);
                AddImage(52, 438, 11433);
                AddHtml(
                    275,
                    45,
                    445,
                    20,
                    @"<BODY><BASEFONT Color=#308EB3>"
                        + book.BookTitle
                        + " by "
                        + book.BookAuthor
                        + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddHtml(
                    275,
                    84,
                    445,
                    521,
                    @"<BODY><BASEFONT Color=#00FF06>" + book.BookText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.SendSound(0x54D);
            }
        }

        public class DynamicBookGump : Gump
        {
            public DynamicBookGump(Mobile from, DynamicBook book)
                : base(100, 100)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                string color = "#d6c382";

                this.AddPage(0);

                AddImage(0, 0, 7005, book.Hue - 1);
                AddImage(0, 0, 7006);
                AddImage(0, 0, 7024, 2736);
                AddImage(362, 55, 1262, 2736);
                AddImage(408, 94, book.BookCover, 2736);
                AddHtml(
                    73,
                    49,
                    251,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">" + book.BookTitle + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddHtml(
                    73,
                    76,
                    251,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">by "
                        + book.BookAuthor
                        + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddHtml(
                    73,
                    105,
                    251,
                    290,
                    @"<BODY><BASEFONT Color=" + color + ">" + book.BookText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.SendSound(0x55);
            }
        }

        public static void SetBookCover(int cover, DynamicBook book)
        {
            if (cover == 0)
            {
                cover = Utility.RandomMinMax(1, 80);
            }

            switch (cover)
            {
                case 1:
                    book.BookCover = 0x4F1;
                    break; // Man Fighting Skeleton
                case 2:
                    book.BookCover = 0x4F2;
                    break; // Dungeon Door
                case 3:
                    book.BookCover = 0x4F3;
                    break; // Castle
                case 4:
                    book.BookCover = 0x4F4;
                    break; // Old Man
                case 5:
                    book.BookCover = 0x4F5;
                    break; // Sword and Shield
                case 6:
                    book.BookCover = 0x4F6;
                    break; // Lion with Sword
                case 7:
                    book.BookCover = 0x4F7;
                    break; // Chalice
                case 8:
                    book.BookCover = 0x4F8;
                    break; // Two Women
                case 9:
                    book.BookCover = 0x4F9;
                    break; // Dragon
                case 10:
                    book.BookCover = 0x4FA;
                    break; // Dragon
                case 11:
                    book.BookCover = 0x4FB;
                    break; // Dragon
                case 12:
                    book.BookCover = 0x4FC;
                    break; // Wizard Hat
                case 13:
                    book.BookCover = 0x4FD;
                    break; // Skeleton Dancing
                case 14:
                    book.BookCover = 0x4FE;
                    break; // Skull Crown
                case 15:
                    book.BookCover = 0x4FF;
                    break; // Devil Pitchfork
                case 16:
                    book.BookCover = 0x500;
                    break; // Sun Symbol
                case 17:
                    book.BookCover = 0x501;
                    break; // Griffon
                case 18:
                    book.BookCover = 0x502;
                    break; // Unicorn
                case 19:
                    book.BookCover = 0x503;
                    break; // Mermaid
                case 20:
                    book.BookCover = 0x504;
                    break; // Merman
                case 21:
                    book.BookCover = 0x505;
                    break; // Crown
                case 22:
                    book.BookCover = 0x506;
                    break; // Demon
                case 23:
                    book.BookCover = 0x507;
                    break; // Hell
                case 24:
                    book.BookCover = 0x514;
                    break; // Arch Devil
                case 25:
                    book.BookCover = 0x515;
                    break; // Grim Reaper
                case 26:
                    book.BookCover = 0x516;
                    break; // Castle
                case 27:
                    book.BookCover = 0x517;
                    break; // Tombstone
                case 28:
                    book.BookCover = 0x518;
                    break; // Dragon Crest
                case 29:
                    book.BookCover = 0x519;
                    break; // Cross
                case 30:
                    book.BookCover = 0x51A;
                    break; // Village
                case 31:
                    book.BookCover = 0x51B;
                    break; // Knight
                case 32:
                    book.BookCover = 0x51C;
                    break; // Alchemy
                case 33:
                    book.BookCover = 0x51D;
                    break; // Symbol Man Magic Dragon
                case 34:
                    book.BookCover = 0x51E;
                    break; // Throne
                case 35:
                    book.BookCover = 0x51F;
                    break; // Ship
                case 36:
                    book.BookCover = 0x520;
                    break; // Ship with Fish
                case 37:
                    book.BookCover = 0x579;
                    break; // Bard
                case 38:
                    book.BookCover = 0x57A;
                    break; // Thief
                case 39:
                    book.BookCover = 0x57B;
                    break; // Witches
                case 40:
                    book.BookCover = 0x57C;
                    break; // Ship
                case 41:
                    book.BookCover = 0x57D;
                    break; // Village Map
                case 42:
                    book.BookCover = 0x57E;
                    break; // World Map
                case 43:
                    book.BookCover = 0x57F;
                    break; // Dungeon Map
                case 44:
                    book.BookCover = 0x580;
                    break; // Devil with 2 Servants
                case 45:
                    book.BookCover = 0x581;
                    break; // Druid
                case 46:
                    book.BookCover = 0x582;
                    break; // Star Magic Symbol
                case 47:
                    book.BookCover = 0x583;
                    break; // Giant
                case 48:
                    book.BookCover = 0x584;
                    break; // Harpy
                case 49:
                    book.BookCover = 0x585;
                    break; // Minotaur
                case 50:
                    book.BookCover = 0x586;
                    break; // Cloud Giant
                case 51:
                    book.BookCover = 0x960;
                    break; // Skeleton Warrior
                case 52:
                    book.BookCover = 0x961;
                    break; // Lich
                case 53:
                    book.BookCover = 0x962;
                    break; // Mind Flayer
                case 54:
                    book.BookCover = 0x963;
                    break; // Lizard
                case 55:
                    book.BookCover = 0x521;
                    break; // Mondain
                case 56:
                    book.BookCover = 0x522;
                    break; // Minax
                case 57:
                    book.BookCover = 0x523;
                    break; // Serpent Pillar
                case 58:
                    book.BookCover = 0x524;
                    break; // Gem of Immortality
                case 59:
                    book.BookCover = 0x525;
                    break; // Wizard Den
                case 60:
                    book.BookCover = 0x526;
                    break; // Guard
                case 61:
                    book.BookCover = 0x527;
                    break; // Shadowlords
                case 62:
                    book.BookCover = 0x528;
                    break; // Gargoyle
                case 63:
                    book.BookCover = 0x529;
                    break; // Moongate
                case 64:
                    book.BookCover = 0x52A;
                    break; // Elf
                case 65:
                    book.BookCover = 0x52B;
                    break; // Shipwreck
                case 66:
                    book.BookCover = 0x52C;
                    break; // Black Demon
                case 67:
                    book.BookCover = 0x52D;
                    break; // Exodus
                case 68:
                    book.BookCover = 0x52E;
                    break; // Sea Serpent
                case 69:
                    book.BookCover = 0x530;
                    break; // Hydra
                case 70:
                    book.BookCover = 0x531;
                    break; // Beholder
                case 71:
                    book.BookCover = 0x532;
                    break; // Flying Castle
                case 72:
                    book.BookCover = 0x533;
                    break; // Serpent
                case 73:
                    book.BookCover = 0x534;
                    break; // Ogre
                case 74:
                    book.BookCover = 0x535;
                    break; // Skeleton Graveyard
                case 75:
                    book.BookCover = 0x536;
                    break; // Shrine
                case 76:
                    book.BookCover = 0x537;
                    break; // Volcano
                case 77:
                    book.BookCover = 0x538;
                    break; // Castle
                case 78:
                    book.BookCover = 0x539;
                    break; // Dark Knight
                case 79:
                    book.BookCover = 0x53A;
                    break; // Skull Ring
                case 80:
                    book.BookCover = 0x53B;
                    break; // Serpents of Balance
            }
        }

        public override void OnDoubleClick(Mobile e)
        {
            if (
                this.Weight == -50.0
                || (e.InRange(this.GetWorldLocation(), 5) && e.CanSee(this) && e.InLOS(this))
            )
            {
                if (ItemID == 0x4CDF)
                {
                    e.CloseGump(typeof(DynamicBookGump));
                    e.CloseGump(typeof(DynamicSythGump));
                    e.CloseGump(typeof(DynamicJediGump));
                    e.SendGump(new DynamicSythGump(e, this));
                    e.SendSound(0x54D);
                }
                else if (ItemID == 0x543C)
                {
                    e.CloseGump(typeof(DynamicBookGump));
                    e.CloseGump(typeof(DynamicSythGump));
                    e.CloseGump(typeof(DynamicJediGump));
                    e.SendGump(new DynamicJediGump(e, this));
                    e.SendSound(0x54D);
                }
                else
                {
                    e.CloseGump(typeof(DynamicSythGump));
                    e.CloseGump(typeof(DynamicBookGump));
                    e.CloseGump(typeof(DynamicJediGump));
                    e.SendGump(new DynamicBookGump(e, this));
                    e.SendSound(0x55);
                }
                Server.Gumps.MyLibrary.readBook(this, e);
            }
            else
            {
                e.SendMessage("That is too far away to read.");
            }
        }

        public DynamicBook(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(BookCover);
            writer.Write(BookTitle);
            writer.Write(BookAuthor);
            writer.Write(BookText);
            writer.Write(BookRegion);
            writer.Write(BookMap);
            writer.Write(BookWorld);
            writer.Write(BookItem);
            writer.Write(BookTrue);
            writer.Write(BookPower);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            BookCover = reader.ReadInt();
            BookTitle = reader.ReadString();
            BookAuthor = reader.ReadString();
            BookText = reader.ReadString();
            BookRegion = reader.ReadString();
            BookMap = reader.ReadMap();
            BookWorld = reader.ReadString();
            BookItem = reader.ReadString();
            BookTrue = reader.ReadInt();
            BookPower = reader.ReadInt();
        }

        public int BookCover;

        [CommandProperty(AccessLevel.Owner)]
        public int Book_Cover
        {
            get { return BookCover; }
            set
            {
                BookCover = value;
                InvalidateProperties();
            }
        }

        public string BookTitle;

        [CommandProperty(AccessLevel.Owner)]
        public string Book_Title
        {
            get { return BookTitle; }
            set
            {
                BookTitle = value;
                InvalidateProperties();
            }
        }

        public string BookAuthor;

        [CommandProperty(AccessLevel.Owner)]
        public string Book_Author
        {
            get { return BookAuthor; }
            set
            {
                BookAuthor = value;
                InvalidateProperties();
            }
        }

        public string BookText;

        [CommandProperty(AccessLevel.Owner)]
        public string Book_Text
        {
            get { return BookText; }
            set
            {
                BookText = value;
                InvalidateProperties();
            }
        }

        public string BookRegion;

        [CommandProperty(AccessLevel.Owner)]
        public string Book_Region
        {
            get { return BookRegion; }
            set
            {
                BookRegion = value;
                InvalidateProperties();
            }
        }

        public Map BookMap;

        [CommandProperty(AccessLevel.Owner)]
        public Map Book_Map
        {
            get { return BookMap; }
            set
            {
                BookMap = value;
                InvalidateProperties();
            }
        }

        public string BookWorld;

        [CommandProperty(AccessLevel.Owner)]
        public string Book_World
        {
            get { return BookWorld; }
            set
            {
                BookWorld = value;
                InvalidateProperties();
            }
        }

        public string BookItem;

        [CommandProperty(AccessLevel.Owner)]
        public string Book_Item
        {
            get { return BookItem; }
            set
            {
                BookItem = value;
                InvalidateProperties();
            }
        }

        public int BookTrue;

        [CommandProperty(AccessLevel.Owner)]
        public int Book_True
        {
            get { return BookTrue; }
            set
            {
                BookTrue = value;
                InvalidateProperties();
            }
        }

        public int BookPower;

        [CommandProperty(AccessLevel.Owner)]
        public int Book_Power
        {
            get { return BookPower; }
            set
            {
                BookPower = value;
                InvalidateProperties();
            }
        }

        public static string BasicHelp()
        {
            string text =
                "BASICS OF THE GAME<BR><BR>Playing "
                + Server.Misc.ServerList.ServerName
                + " is pretty simple as the interface is quite intuitive. Although the game is over 20 years old, some explanation is in order. After you log in and are in the game world, you will see a book open with abilities. Right-click on this book to close it as you will not need it for this game. Almost any window can be closed with a right-click. Your character is always in the center of the playing screen. To travel, simply move the mouse over the game world display, then right-click and hold. The mouse cursor will always point away from your character, who will move in the indicated direction. For example, if you wish to walk up the screen, hold the cursor above your character. You will continue to head in that direction until you come to an obstruction or release the mouse button. The further away the cursor is from your character, the faster the character will move. Double right-clicking will cause your character to move to the exact point where the cursor was, unless disabled in the options.<BR><BR>PAPERDOLL: Your character paperdoll will be open when you start. If it is not, pressing Alt + P will open it for you. Below, I will explain what this does. The left side shows boxes for the slots, showing what is on your head, ears, neck, finger, and wrist. The bottom will display your name and title. Sometimes it is custom, while mostly it is your best-practiced skill, along with any fame and/or karma you have gained.<BR><BR>The right side has various buttons. Pressing the HELP button brings up a simple help menu. The only thing you should ever use here is when your character is physically stuck in the game world and needs to be teleported out. It will teleport you to a safe area somewhere in the land. The OPTIONS button will bring up your game options (discussed later). The LOG OUT button logs you out of the game. Make sure you are in a safe place. The STATS button will display some vital statistics about your character (discussed later). The SKILLS button will show all the available skills in the game. Here, you can manage your skill progression (discussed later). The GUILD button enables you to start your own guild. It will cost money to get started, but you can invite other players and share homes and chat with each other. The PEACE button toggles whether you are ready to fight or not. Lastly, the STATUS button will display the status of your character (discussed later).<BR><BR>The center shows your character. Here, you can drag and drop clothing, armor, and other equipment worn by your character. Double-click the left scroll to see how old your account is. Double-click the right scroll to organize a party of other players. This is important if you plan to share the rewards of dungeon delving. Double-click the backpack to open your backpack (discussed later).<BR><BR>MENU BAR: This menu bar allows you to access certain items more quickly. It can be disabled in the options. The small triangle minimizes the menu bar. The MAP button opens a mini map of your surroundings. Pressing it again will enlarge the map (Alt + R also does this). The PAPERDOLL button opens your paperdoll. The INVENTORY button opens your backpack (discussed later). The JOURNAL button opens your journal, which displays the most recent things you saw or heard. The CHAT button is disabled, so use the command [c instead. The HELP button brings up the help menu discussed earlier. The ? button brings up an outdated information screen that is best not to use.<BR><BR>BACKPACK: When you double-click the backpack on the paperdoll, it will open (Alt I will do that as well). You can only carry a certain amount of weight based on your strength. If your strength is extremely high, then you are limited by how much the backpack can hold. The image on the right shows how you can have containers within the backpack to help organize things better. You can drag and drop items between the containers. Sometimes your containers will close when you travel between different worlds. If you close a container that has other containers within it open, those containers will also close.<BR><BR>OPTIONS: Pressing the options button will open this window (as will pressing Alt O). You can change many things in the options section. You can control the volume of music and sounds. You can change the fonts and colors of the fonts. You can set up macros to create shortcut keys commonly used series of commands. You can also filter obscenities. This is also where you can set your pathfinding, war mode, targeting system, and menu bar options. You can choose to offset your interface windows (like containers) when opening. Pay attention to the macro options, as you can learn about some of the pre-built shortcut keys... along with learning how to steer ships when you can afford to buy one.<BR><BR>STATS: There are many aspects of your character, and the stats button will display this for you. You can see what comprises your abilities and if you have any bonuses for regenerating statistics like mana, stamina, or hit points. You can also see the values of your karma and fame, which can also appear as a title on your paperdoll. Your hunger and thirst will be shown so you can determine if you need to eat or drink (of course, the game will tell you when you are starving or dying of thirst without this statistic). You can determine how fast you can cast spells or apply bandages. If you have murdered anyone innocent, you can see that value here. If you use tithing points (people pursuing knightship), you can see that value here to know if you need to make a donation of gold to a shrine sooner or later.<BR><BR>SKILLS: Pressing the Skills button (or using Alt K) will open a menu where you can view the various skills available in the game for your character to improve. You have a maximum of 1,000 skill points to allocate. Skills with blue dots on the left side are typically active most of the time, even when running in the background. You can click and drag these skills off the menu to create a button on your screen for easier activation in the future. On the right side of each skill, there is an up arrow that can be changed to a down arrow or a lock icon. Locking a skill at a certain value prevents it from increasing or decreasing beyond that point. If you have used up all 1,000 skill points, changing it to a down arrow signifies that the skill will decrease if another skill is raised. Some magic items can also enhance your skills, which will be reflected here. If you only want to see the base skill values without the influence of magic items, you can click the 'show real' option at the bottom right. The 'show caps' option reveals the maximum value that a skill can reach. Each skill can go up to 100 individually (without exceeding a total of 1,000). Scrolls of power can be found to allow skills to surpass 100, and this option will display that information. Skills are categorized, and you can even create your own group by clicking the 'new group' button. This allows you to organize and keep track of specific sets of skills for your character.<BR><BR>STATUS: The status window displays various attributes of your character, including strength, dexterity, intelligence, hit points, stamina, mana, luck, carried weight, followers, damage, and carried gold. Additionally, you can view the maximum values allowed for strength, dexterity, and intelligence combined, which is set to 250. By double clicking on the window, you can switch between a detailed view and a smaller bar view. You have the ability to adjust your strength, dexterity, and intelligence using the arrows located to the left of each value. On the right side of the window, you can see your defense values against physical, fire, cold, poison, and energy attacks. All creatures possess these values, and certain attacks may cause damage in one or more of these categories. It is beneficial to have high values in each category, with a maximum of 70%. The remaining features of the game can be learned through gameplay. For instance, you can discover additional commands by reading the message of the day. Alternatively, you can visit a sage and purchase a scroll that provides detailed explanations and instructions for all skills. To access various commands, you can type a '[' symbol followed by the desired command in the bottom left corner of the world view window. For example, '[c' opens the chat window, '[status' displays the stats window, and '[motd' reveals the message of the day. In the message of the day window, you can press '?' in the upper right corner to learn more commands. The game encourages exploration and leaves it up to you to discover and engage with its various aspects. Now, let's delve into some common activities that you will likely partake in during gameplay.<BR><BR>CHAT: This is a means to communicate within the game world when you are not on the same screen as another player. Type '[c' to begin using the chat system. This also lets you send a message to another player that they can read later on. Keep in mind that this is character-specific and not account-specific. If you send a message to a character, but the player logs in with a different character, they will not see the message until they log in with 'that character'. This chat feature has many options that Internet chat systems have. You can see who is online. You can establish channels. You can even set some privacy levels to ignore others or not be seen at all.<BR><BR>CITIZENS: Many citizens have a context menu when you single left-click them. This brings up a list of services they provide for you. Some may be grayed out as they cannot provide 'you' in particular. For example, if they train tailoring and you are already a master at it, it will be grayed out because they cannot teach you any more about it. Many citizens have a 'hire' option. Make sure to explore what you can hire them to do for you as it may come in handy later. Be careful when single left-clicking them as you may still be in war mode and accidentally attack them. If they live through your initial attack, run away or risk becoming a murderer. Murders take about 8 hours of real-time (while in the game) to go away. If you continue to murder while you are a murderer, it will take 40 hours of real-time (while in the game) to go away. ";
            if (Server.Misc.MyServerSettings.AllowBribes() >= 1000)
            {
                text =
                    text
                    + "Another option is to visit the Assassin Guildmaster, where you can hire them for "
                    + Server.Misc.MyServerSettings.AllowBribes()
                    + " gold to persuade the guards to overlook a single murder you may have committed. Assassin Guild members pay only half of that amount, but fugitives are too notorious to be pardoned by bribes. If you lack sufficient gold in your pack, they will simply deduct it from your bank box. ";
            }
            text =
                text
                + "You will not be allowed in a settlement if you are a murderer, unless you disguise yourself. However, criminals will lose their criminal status within a few minutes.<br><br>BANKS: Banks serve as secure locations for storing your valuables. It is impractical to carry everything with you, and utilizing a bank is a suitable alternative until you can afford a home. Furthermore, considering the inherent dangers of the world, it is wise to store additional equipment in the bank to serve as a backup in case you misplace your preferred set.<br><br>Bankers are attentive professionals who can grant you access to your bank box upon your arrival at the bank building and the utterance of the word 'bank.' Moreover, banks are the only establishments that accept non-gold currency and offer currency exchange services, converting your copper and silver coins into gold. By entrusting these coins to a banker, you can have them converted conveniently. Alternatively, you can place the coins in your bank box and double-click them to initiate the conversion process.<br><br>In the event that you need to transport a substantial amount of gold, you can convert your funds into an official document known as a check. To do this, simply state the word 'check' followed by the desired amount.<br><br>INNS & TAVERNS: Inns (candle signs) and taverns (wine/grape signs) are safe places for adventurers to rest and relax. You cannot cast spells here or attack anyone else. These places are ideal for negotiating trades, buying food and drink, or simply chatting and playing the games they offer (the tavern offers games, not inns). You instantly log out when in these places; otherwise, it will take a few minutes for your character to log out when out and about. Taverns are also excellent for hiring henchmen to adventure with. Sometimes, bar patrons in taverns will share information about places to go and rumors they have heard.<br><br>PRACTICE: Some settlements offer designated areas for practicing weapon skills. To equip a weapon, simply place it in your paperdoll hand and then double-click on a training dummy. It's worth noting that the training dummies have a maximum skill level for weapon training, but they provide a good starting point. If you prefer ranged weapons, you can utilize an archery butte for practice. Additionally, if you come across a thief, they often have dummies available for practicing pickpocketing skills.<br><br>COMBAT: To initiate a fight with someone or something, press the PEACE button on the paperdoll to switch to war mode. Then, double-click on your target to engage in combat. Remember to press the button again to return to peace mode to avoid accidentally attacking anyone. Most weapons require close proximity to your opponent, while bows allow you to attack from a distance. Additionally, you can also attack using followers or spells. Initiating a harmful spell on another individual will also initiate a fight. It's important to note that there may be times when it's necessary to retreat from a fight. It's better to live and fight another day. Killing citizens or other players will result in murder counts, with the exception of killing players who are already murderers or criminals. You can identify them by highlighting them. If they appear red, they are a murderer. If they appear grey, they are a criminal.<br><br>WEAPONS: There are numerous weapons available in the game. Hovering your cursor over them reveals their statistics, including the damage they inflict and the type of damage they deal. You can also see the weapon's swing or shooting frequency. Additionally, each weapon has a strength requirement and corresponds to a specific skill type (such as swordsmanship or marksmanship) for effective usage. Moreover, the weapons are categorized as one-handed or two-handed, which is crucial if you intend to equip a shield, torch, or lantern. Two-handed weapons do not allow for such equipment. Some weapons may possess magical properties, which will also be indicated. Furthermore, every weapon has a durability rating, and if it becomes too low, your item will break. To address this, you can either repair it yourself (typically through blacksmithing or bowcrafting skills) or hire a citizen to fix it for you.<br><br>ARMOR: Armor not only encompasses the medieval metal armors worn by knights, but also includes smaller items such as leather gloves and hats. By looking to the right, you can observe that the wizard's hat provides bonuses to resistances (refer to the section on Status). Like weapons, armor has both a strength requirement and durability. It is possible to purchase multiple pieces of armor that cover different parts of the body. These areas include the hands, arms, legs, neck, chest, and head, each of which can be protected by a specific type of armor. Similar to weapons or jewelry, you can equip armor by dragging and dropping it onto the paperdoll. If the armor does not equip, it is likely because you are either not strong enough or because there is already a piece of armor or clothing in that particular spot. It is important to note that heavier armor reduces your ability to meditate or move stealthily. Leather armor is often a suitable option for such activities, as even switching to studded leather armor may prove too cumbersome. However, any armor labeled as 'mage armor' will allow you to meditate or move stealthily regardless of its weight.<br><br>MAGIC GATES: You may encounter a magical gate that leads to a different location. You have the choice to either walk away or enter it. These gates can be found in various colors, such as blue, red, or black. Skilled wizards are capable of summoning these gates, while certain necromantic potions have the power to summon black gates. By discovering the destinations of these gates, you can explore new worlds or navigate the current world more efficiently. Occasionally, defeating a mystical creature can result in the creation of a gate, although these occurrences are extremely rare.<br><br>MAGIC RUNES: Recall runes are small brown stones with a golden ankh symbol on them. One can cast a Mark Spell on them to mark the current location of the rune. By using magic, one can then transport back to that spot. When runes are marked, they change color depending on the world they are taken to. To facilitate easier use, you can purchase or create runebooks that allow you to store up to 16 rune stones. Additionally, by dropping Recall Spell scrolls onto the book, you can increase the charges and use the book to teleport to those locations without the need for magical knowledge.<br><br>BOOKS OF MAGIC: There are several books of magic in the world. The one used by wizards is a simple spellbook, while necromancers use a necromancer spellbook. Both of these books require spell scrolls to be scribed in them by dropping the appropriate spell scroll on the book. If you double-click a scroll and try to cast from it, it will simply fade afterwards. However, putting it in a book allows you to keep using it over and over. The other three books are for knightship, samurai, and ninjas. These books are not like spellbooks; they simply allow you to use the special abilities associated with each. Regardless of the book, you can drag and drop icons from them for quicker access to casting or using the abilities. The wizard and necromancer spells require spell components. Additionally, you have the option to visit a sage or scribe for even easier ways to use this magic.<br><br>DEATH: You might find yourself falling victim to a series of unfortunate events... and thus departing from the land of the living. Fear not! You have several options to make your return. The first choice is to be resurrected, albeit with certain penalties to your skills and abilities. This method is quick, but it will cause a significant setback to your character's progressions. Another option is to have a comrade resurrect you using either magic or healing. Alternatively, you might be fortunate enough to have consumed a resurrection potion before your demise. Lastly, you could embark on a soul-searching journey across the land to find a healer or shrine that can bring you back to life. In this case, a healer will require a donation for their services, while a shrine will demand tribute to the gods. To take advantage of this, you will need to have some gold saved in your bank box or have gold tithed. However, if your total stats are 90 or less and your skill total is 200 or less, you can be resurrected without any fees or penalties.<br><br>FINALLY: This should provide you with ample information to embark on your adventure in this world. Explore and experiment with various activities, and you will uncover numerous options for your character. One of the initial suggestions would be to seek out a sage, as they offer valuable information that can enhance your understanding of how things operate. They possess scrolls containing instructions on blacksmithing, tailoring, and gathering resources from the environment through activities like mining or hunting. Additionally, you can acquire knowledge on woodcutting, dungeon looting, trap evasion, and various reagent types. It is crucial to strategize your approach to acquiring food and drink, as they will be essential for your journey. Best of luck in your adventures!";

            return text;
        }

        public static void SetStaticText(DynamicBook book)
        {
            if (book is TendrinsJournal)
            {
                book.BookText =
                    "Entry 1 - Today is looking to be a good day for Skara Brae. All of the townsfolk are getting ready for the fall celebration. I, myself, am making sweet rolls for the grand dinner tonight. I better get to work.<br><br>Entry 2 - The celebration did not go well. Mangar was there, and he was threatening many of the council members. Apparently, there is some legend of a lost artifact in ruins below Skara Brae, and he wants permission to dig. The council is weak, but with the backing of Lord British...Mangar can do very little to force them into his demands. He eventually stormed off to his tower. I will ask the council if they want me to scout out his tower tomorrow. It is in the center of Sosaria, so it will be a far walk...but I think we need to know what he is up to.<br><br>Entry 3 - I am off with the council's blessing. It will be a long journey, but I should be there early tomorrow. I have set up camp and am ready to rest for the night.<br><br>Entry 4 - I am back in Skara Brae, as I traveled all night to get here. Matters are getting worse. Mangar was not in his tower when I got there, but I scaled the walls and searched his study. There were many scrolls and books. From what I could read, he is planning something horrific for us. His plan is to magically transport Skara Brae into the Void. If he were to accomplish this, then we would not be able to get outside help, and Mangar can do with Skara Brae as he pleases. I went over this with the council tonight, and they sent a messenger to Britain for help.<br><br>Entry 5 - Mangar arrived in Skara Brae this morning and has been in with the council since he got here. I went over to the building to eavesdrop on the conversation. He urged the council to meet his demands to dig. If they do not, he would unleash horrific magic on us and send us into the Void. When one of the council members stated that Lord British's wizards would come find us and return us from the Void...that is when Mangar said, 'No one in Sosaria will ever know we are in the Void.' What does that mean?<br><br>Entry 6 - I returned to Mangar's tower in the hope to put an end to his tyranny. He was not there yet again, and the tower seemed to be unkempt, to say the least. Searching through more scrolls, I found out what he meant the other day. His plan is to make it appear Skara Brae lies in ruins. All of Sosaria will think we perished in some type of disaster. I must return and warn the others.<br><br>Entry 7 - I am too late. Skara Brae is gone, and there is nothing but destroyed homes around me. I am sitting on the floor of what was once my home, bleeding out from a knife wound to my back. Distracted by the magical vortex earlier, Mangar crept up behind me and stabbed me. He then teleported away with a sinister laugh. It appears he has won. May the gods help all of those trapped in Skara Brae.";
            }
            else if (book is BookGuideToAdventure || book is LoreGuidetoAdventure)
            {
                book.BookText = BasicHelp();
            }
            else if (book is BookBottleCity)
            {
                book.BookText =
                    "It began with just a few. An experiment conceived by the Black Knight's mind. Vordo, one of his most powerful mages, toiled for years to perfect the spell that eventually engulfed the small island of Kuldar near the Serpent Island. The gargoyles dreaded the Black Knight's might, believing the island to be obliterated. However, the truth was something far more sinister. Within the mystical bottle, the island floats, housing the life that was brought with it. The Black Knight banished some of his prisoners to the bottle, where they would live out their remaining years. Centuries passed, and these prisoners cultivated the land, erected a city, bore children, and thrived. Vordo, desiring to rule this land as the Black Knight ruled his, informed the Black Knight that the bottle was destroyed in an accident—an issue that concerned the Black Knight little as he had moved on to other matters. Vordo lowered his castle into the bottle, causing it to crash down beside the city within. Using his magic, he entered the bottle and conjured a moongate, allowing him the possibility of leaving one day. He governed with an iron fist for nearly a decade until the citizens rebelled and put an end to his tyranny. They sealed off his castle, imprisoning whatever horrors Vordo had created inside. Legends speak of his ghost haunting these halls, carrying the notes that would grant him freedom from the bottle. If I could banish his spirit to eternal rest, if only for a brief moment, I might seize his notes and employ gate or recall magic to escape this place.";
            }
            else if (book is BookofDeadClue)
            {
                book.BookText =
                    "It sailed across the world, ensnaring the numerous lost souls that swam in its vicinity. The solitary captain, a necromancer, steered the vessel into treacherous waters of death and misery. Those who are alive seek out the blackened ship, in a quest for the legendary Book of the Dead. With this coveted power, a skilled necromancer can utilize the body parts of the deceased to create a formidable creature devoid of consciousness. However, the acquisition of the dark heart is essential to perform such a ritual. Legend has it that the ominous vessel frequently retreats to Ambrosia, where the sole means of boarding is to utter the fatal incantation, 'necropalyx'. Remember this word diligently, as it must be spoken to escape the clutches of the ship. In order to gain access to the deadly hold, one must locate the sinister pentagram and speak the designated word. The ship should be anchored in close proximity.";
            }
            else if (book is CBookTombofDurmas)
            {
                book.BookText =
                    "King Durmas IV had a high mage on his council who was seeking the magic of immortality. Although charged to do this by the King, the mage was actually seeking the power for themselves. The true success of this mage was not known until centuries later when they rose from their grave and desired to control all the dead in the world, slaughtering the living in their wake. Until we can gain control over this powerful lich, they will forever remain entombed in the crypt of the Durmas family. There is only one entrance and exit to the crypt, with a stone altar built within. Speaking the word 'xormluz' on the altar will magically transport the speaker into the sealed crypt. Uttering the words on the opposite side's altar will bring individuals back from the crypt. Research on this matter is ongoing.";
            }
            else if (book is CBookElvesandOrks)
            {
                book.BookText =
                    "It is said that elves and orks exist, but their lands are worlds apart. Orks, the more civilized relatives of orcs, reside within the Savaged Empire. The elves, who possess a rich culture and rare treasures, inhabit the vast land of Lodor. The connection between the valley and Lodor is believed to be a frigid cave. The elves only venture there to explore the mountains, where it is rumored that the gods can create extraordinary and precious artifacts.";
            }
            else if (book is MagestykcClueBook)
            {
                book.BookText =
                    "The Council of Mages has grown tired of the barbaric practices of the Magestykc group. Summoning demons to our realms is unacceptable and will not be tolerated. While we may not be able to find all of them, we have banished the majority to a section of the underworld where they will spend the rest of their days. There, they can summon the lords of hell and be exiled alongside them. Unfortunately, their grand wizard has managed to escape and has the ability to create a magical gate between Sosaria and the underworld prison. I fear the day will come when he activates this gate, and we must prepare for the impending apocalypse. To determine if the portal has been created, we will send some of our most skilled wizards. Since they commonly speak their group name to activate the portal, locating it should be relatively easy.";
            }
            else if (book is FamiliarClue)
            {
                book.BookText =
                    "I have spent days in this accursed dungeon, searching for clues about the existence of the gargoyle lands. I had enough food and water to last for days, but I couldn't carry it all. I heard from other mages that the guildmasters often search for rubies, as they are apparently used for certain types of spellcasting. They happily accept donations, but if an apprentice were to offer them 20 or more, they are often given a gift. It doesn't matter if you practice magic or dark magic, as long as you have reached the apprentice level of skill in such fields. I also heard something similar from another spellcaster, that the guildmaster of black magic has a fondness for star sapphires. However, I found none of those. <BR><BR>During my last journey, I discovered 23 rubies in a metal chest and presented them to the wizard guildmaster. In return, he gave me a crystal ball that summons a familiar to serve me. While the familiar doesn provide many services, it can some of my belongings and keep me company. The crystal only had 5 charges, but I can hire the mage guildmasters to recharge it further. Initially, I was given an imp as a traveling companion, but I desired a black cat. So, I returned the crystal ball to the mage, who gave me another to examine. I continued this process until I finally obtained the cat I sought. The mage informed me that I could use colors from common dye tubs and pour them onto the crystal ball. This would allow the familiar to take on the color of the dye. He was right. Finally, I had my black cat familiar, just like other famous wizards before me. I named him Moonbeam. <BR><BR>Now, I am resting in the depths of this place. I will resume my quest in the morning. I hear something nearby, so I should investigate.";
            }
            else if (book is LodorBook)
            {
                book.BookText =
                    "For years, I searched for a way to journey to the world of Lodor, also known as the land of elves. Although considered a myth by many, I have finally reached this new world. It appears to be a peaceful place with numerous cities. I discovered the City of Lodoria, where the sages taught me how to enhance my wizardry abilities. As time passes, I feel my life slipping away, but this newfound will enable me to complete the rituals required to become a lich. In death, I will wander this world, just as I did in life, from atop my dark tower, where the citizens of Montor will no longer ridicule me. I will leave the magic mirror intact, so that by gazing into it and uttering the word 'xetivat', I can magically transport myself to Lodor. Simply reversing the word in Lodor's mirror will allow me to return to Sosaria. Perhaps with my newfound power, I can conquer both worlds. Exhaustion overwhelms me, and I must rest now.";
            }
            else if (book is CBookTheLostTribeofSosaria)
            {
                book.BookText =
                    "Those who lived long ago constructed an immense pyramid, which is now concealed beneath layers of sand and stone in the northwestern region of Sosaria. The identity of these individuals remains uncertain, yet legends recount their departure from Sosaria via a magical portal, ultimately establishing a new territory abundant in forests, animal hides, and mineral resources.";
            }
            else if (book is LillyBook)
            {
                book.BookText =
                    "Centuries ago, a peaceful race of gargoyles fled the land of Sosaria to settle on Serpent Island. It was long forgotten until Archmage Zekylis came to the Mages' Guild in Fawn to boast of his discovery. He found a tunnel that leads to this world in the frozen lands but would not speak of the exact location. He told tales of a tropical land with the City of Furnace. There, he learned the art of creating stone statues and the ability to turn sand into glass for other items. What intrigues me is that I have sent agents from the Thieves' Guild to follow him and see if they can discover the tunnel's location. They believe they found it in the treacherous mountains of the frozen lands but cannot climb them. They have witnessed Zekylis magically appear on top of the tower, suggesting he has another means of reaching it. Years have passed without any new information. It was only by accident that a hunter at the Sleepy Island Inn told tales of a crazy wizard living in the nearby jungle of Umber Veil. Word reached the Thieves' Guild, and they found Zekylis' home. Apparently, he married a woman named Lilly from Renika. She died from a giant serpent bite and was buried next to their home. Her parents, who likely died from old age, are also buried there. A spy hid in the nearby shadows, observing and listening. Late one night, Zekylis came out of the house and approached Lilly's grave. The spy had to duck behind a gravestone to avoid being discovered. Zekylis stood in front of her grave and said, 'I love you, Lilly.' The spy waited for some time but did not hear Zekylis leave. Growing weary, the spy peeked around and saw that Zekylis had vanished. He never returned home, as if he had disappeared without a trace. Magic jewels were found in his home, but their effects could never be determined. How Zekylis escaped so easily from the spy remains a mystery. He also took his learned secrets with him. I fear we may never know how to access his tower.";
            }
            else if (book is LearnTraps)
            {
                book.BookText =
                    "There is more to fear in dungeons and tombs than simply monsters and undead. Those with a good 'searching' skill can find hidden traps, which are almost always present. To disable them, one needs a good 'remove trap' skill, a ten-foot pole to trigger them, or magic that renders the trap useless. When walking over a hidden trap, there is a passive attempt to disable it. If the skill is high enough, it will be disabled. If a ten-foot pole is available, tapping it will trigger the trap before it can harm the person. If there is remove trap magic, an item in the pack will function like a ten-foot pole. All three elements will be checked if they are available for the character. Luck also plays a role, with greater luck increasing the chances of avoiding the trap. Containers can be targeted for trap removal or magic spells, but there are also passive checks for traps on containers. There are four possible traps for containers: magic, explosion, dart, or poison. Hidden traps can be found on the floors of dangerous places, and there are 27 different effects they can have. Some are merely annoyances, while others are deadly and can result in the loss of a beloved item.<BR><BR>- Reveals your location if hidden<BR>- Causes you to trip and drop your backpack<BR>- Causes you to trip and drop an item<BR>- Turns coins into lead<BR>- Ruins an equipped item<BR>- Causes a loss of 1 strength<BR>- Causes a loss of 1 dexterity<BR>- Causes a loss of 1 intelligence<BR>- Inflicts poisoning<BR>- Reduces hit points to 1<BR>- Reduces stamina to 1<BR>- Reduces mana to 1<BR>- Turns gems into stone<BR>- Ruins reagents<BR>- Places books in a magic box<BR>- Teleports you far away<BR>- Lowers your fame<BR>- Curses an equipped item<BR>- Activates a spike trap<BR>- Activates a saw blade trap<BR>- Activates a fire trap<BR>- Activates a giant spike trap<BR>- Triggers an explosion trap<BR>- Triggers an electrical trap<BR>- Breaks bolts and arrows<BR>- Ruins bandages<BR>- Breaks potion bottles<BR><BR>Some traps have avoidance checks, which test against resistances or magic resistance skills. This means that walking into a trap does not guarantee certain doom. Ten-foot poles are the least effective and also weigh quite a bit. Magic is more effective, depending on the wizard's skill in magery. The most effective method is to be skilled in the remove trap skill, but it is generally best to avoid traps altogether.";
            }
            else if (book is LearnTitles)
            {
                book.BookText =
                    "I have taught many from one end of Sosaria to the other. During this time, I am always curious about the need for people to group others by their skills and trades. My research into this matter has proven to be extensive as society has come up with many words to describe the skilled of the world. Below I document my findings. <br> <br>Alchemy <br>-- Alchemist <br>Anatomy <br>-- Biologist <br>Druidism <br>-- Naturalist <br>Arms Lore <br>-- Man-at-arms <br>Begging <br>-- Beggar <br>Blacksmithy <br>-- Blacksmith <br>Bludgeoning <br>-- Bludgeoner <br>Bowcrafting <br>-- Bowyer <br>Bushido <br>-- Samurai <br>Camping <br>-- Explorer <br>Carpentry <br>-- Carpenter <br>Cartography <br>-- Cartographer <br>Cooking <br>-- Chef <br>Discordance <br>-- Demoralizer <br>Elementalism <br>-- Elementalist <br>Fencing <br>-- Fencer <br>Fist Fighting <br>-- Brawler <br>Focus <br>-- Driven <br>Forensics <br>-- Undertaker <br>Healing <br>-- Healer or Mortician <br>Herding <br>-- Shepherd <br>Hiding <br>-- Skulker <br>Inscription <br>-- Scribe <br>Knightship <br>-- Knight <br>Lockpicking <br>-- Lockpicker <br>Lumberjacking <br>-- Lumberjack <br>Magery <br>-- Wizard or Sorceress <br>-- Archmage if there is a<br>   raw grandmaster talent<br>   in Magery and Necromancy <br>Magic Resistance <br>-- Magic Warder <br>Marksmanship <br>-- Marksman <br>Meditation <br>-- Meditator <br>Mercantile <br>-- Merchant <br>Mining <br>-- Miner <br>Musicianship <br>-- Bard <br>Necromancy <br>-- Necromancer or Witch <br>-- Archmage if there is a<br>   raw grandmaster talent<br>   in Magery and Necromancy <br>Ninjitsu <br>-- Ninja or Yakuza <br>Parrying <br>-- Duelist <br>Peacemaking <br>-- Pacifier <br>Poisoning <br>-- Assassin <br>Provocation <br>-- Rouser <br>Psychology <br>-- Scholar <br>Remove Trap <br>-- Trespasser <br>Seafaring <br>-- Sailor or Pirate <br>Searching <br>-- Scout <br>Snooping <br>-- Spy <br>Spiritualism <br>-- Spiritualist <br>Stealing <br>-- Thief <br>Stealth <br>-- Sneak <br>Swordsmanship <br>-- Swordsman <br>Tactics <br>-- Tactician <br>Tailoring <br>-- Tailor <br>Taming <br>-- Beastmaster <br>Tasting <br>-- Food Taster <br>Tinkering <br>-- Tinker <br>Tracking <br>-- Ranger <br>Veterinary <br>-- Veterinarian <br> <br>Oriental Titles <br><br>Alchemy <br>-- Waidan <br>Fencing <br>-- Yuki Ota <br>Fist Fighting <br>-- Karateka <br>Healing <br>-- Shukenja <br>Knightship <br>-- Youxia <br>Magery <br>-- Wu Jen <br>Marksmanship <br>-- Kyudo <br>Necromancy <br>-- Fangshi <br>Spiritualism <br>-- Neidan <br>Swordsmanship <br>-- Kensai <br>Tactics <br>-- Sakushi <br> <br>Evil Titles<br><br>Magery <br>-- Warlock <br>-- or <br>-- Enchantress <br> <br>Barbaric Titles<br><br>Alchemy <br>-- Herbalist <br>Bludgeoning <br>-- Barbarian (Amazon) <br>Druidism <br>-- Beastmaster <br>Camping <br>-- Wanderer <br>Fencing <br>-- Barbarian (Amazon) <br>Knightship <br>-- Chieftain (Valkyrie) <br>Herding <br>-- Beastmaster <br>Magery <br>-- Shaman <br>Marksmanship <br>-- Barbarian (Amazon) <br>Musicianship <br>-- Chronicler <br>Necromancy <br>-- Witch Doctor <br>Parrying <br>-- Defender <br>Seafaring <br>-- Atlantean <br>Swordsmanship <br>-- Barbarian (Amazon) <br>Tactics <br>-- Warlord <br>Taming <br>-- Beastmaster <br>Veterinary <br>-- Beastmaster<br><br>";
            }
            else if (book is GoldenRangers)
            {
                book.BookText =
                    "This guide is intended for explorers and rangers on their quest to find the elusive golden feathers. By keeping this manual with you, you may increase your chances of discovering these mythical feathers and ultimately bless an item at the Altar of Golden Rangers. To prove worthy of the golden feathers, one must hunt either a harpy or, for the more daring, a phoenix. While these creatures are rare to come across, it is believed that the goddess may be observing as you defeat them and bestow the feathers upon you. Once obtained, you can bring the feathers to the Altar of Golden Rangers. Place a single weapon or piece of armor on the altar and utter the word 'Aurum'. The item will then be transformed into gold and blessed by the goddess of rangers. It is crucial to keep this book in your possession during your hunt. Remember, only the one who slays the beast will be rewarded with the gift of the feathers, making it a privilege exclusive to master rangers or explorers. Good luck, and do not succumb to greed, as the goddess will not grant you feathers if you already possess them. Instead, she will simply bring them to you as a reminder of your past rewards.";
            }
            else if (book is AlchemicalElixirs)
            {
                book.BookText =
                    "The magical enhancement of the mind and body is something that can be explored within the realm of alchemical elixirs. Reading this book will familiarize you with the different types of potions and allow you to start mixing your own. Like other forms of alchemy, you will need a mortar and pestle and the appropriate reagents. An empty bottle is also required. There are 49 different types of elixirs, and each one enhances specific skills for a certain period of time. The only skills that cannot be enhanced are those of a magical nature, such as magery, necromancy, ninjitsu, bushido, and knightship. All other skills can be enhanced with elixirs.<BR><BR>Elixirs have varying levels of effect, depending on a few factors. Some elixirs will last for about 1 to 6 minutes, while others will last for about 2 to 13 minutes. Each type of elixir is listed in this book, along with its potential duration.<BR><BR>The duration of an elixir is determined by three factors. 40% relies on the drinker's cooking skill, 40% relies on the drinker's tasting ability, and the remaining 20% is based on the drinker's alchemy skill, as well as any potion enhancement properties they may possess. The better these elements are, the longer the elixir will last. The strength of the elixir is also based on these same factors, with a potential increase of +10 to +60 to the skill that the elixir is meant to enhance. While a particular elixir is in effect, you cannot drink another elixir of the same type, nor can you be under the influence of more than two elixirs at a time. Below is a list of various elixirs.<br><br>- Elixir of Alchemy<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Anatomy<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Druidism<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Arms Lore<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Begging<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Blacksmithing<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Bludgeoning<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Bowcrafting<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Camping<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Carpentry<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Cartography<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Cooking<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Discordance<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Fencing<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Fist Fighting<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Focus<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Forensics<br>    Lasts 1 to 6 minutes<br><br>- Elixir of the healer<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Herding<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Hiding<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Inscription<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Lockpicking<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Lumberjacking<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Magic Resistance<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Marksmanship<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Meditating<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Mercantile<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Mining<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Musicianship<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Parrying<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Peacemaking<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Poisoning<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Provocation<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Psychology<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Removing Trap<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Seafaring<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Searching<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Snooping<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Spiritualism<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Stealing<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Stealth<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Sword Fighting<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Tactics<br>    Lasts 1 to 6 minutes<br><br>- Elixir of Tailoring<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Taming<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Tasting<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Tinkering<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Tracking<br>    Lasts 2 to 13 minutes<br><br>- Elixir of Veterinary<br>    Lasts 1 to 6 minutes";
            }
            else if (book is AlchemicalMixtures)
            {
                book.BookText =
                    "The mixing of ingredients with other potions allows a skilled alchemist to create mixtures with various effects when dumped on the ground. Some mixtures are spread over the ground, causing anyone who walks over the liquid to experience the effects. Others create magically sentient slimes that obey the alchemist's commands. This book familiarizes you with different types of potions, enabling you to start mixing your own. Just like other forms of alchemy, you will need a mortar and pestle, the appropriate reagents, a specific type of potion, and an empty jar.<BR><BR>The effects of the mixtures will vary based on several factors. The duration of the effects is determined by three factors. Forty percent relies on the alchemist's cooking skill, another forty percent depends on their ability to taste, and the remaining twenty percent is based on the alchemist's alchemy skill and any potion enhancement properties they possess. The better these elements are, the longer the effects of the mixture will last when dumped. The strength of the dumped mixture also depends on these same factors, with some slimes and liquids causing more damage and being more resilient. However, it's important to note that liquids can harm the alchemist as well as others, so it's advisable to maintain a safe distance.";
            }
            else if (book is BookOfPoisons)
            {
                book.BookText =
                    "Poisons are commonly used by tavern keepers to rid their cellars of vermin that feast on their wares. Others, of a more nefarious nature, use poisons to meet their vile goals. Alchemists and assassins are experts when it comes to poisons. Poisons can be created in two ways: using the leaves of the nightshade to alchemically create them, or extracting venom from the venom sacks of creatures into a bottle. To master the poisoning skill, it is best to start with weaker poisons before moving on to more deadly ones.<BR><BR>The different types of po and their corresponding skill levels are follows:<BR>- Lesser Poison (0-40)<BR>- Poison (20-60)<BR> Greater Poison (40-80)<BR>- Deadly Poison (-100)<BR>- Lethal Poison (80-120)<BR><BR>Those skilled in poisoning can throw the contents of the bottle onto the ground. Anyone who walks over the spill may be poisoned, including the person who dumped it. Those who are not skilled enough may accidentally drink the contents and suffer the effects.<BR><BR>The skills needed to dump these poison bottles on the ground are as follows:<BR>- Apprentice: Lesser<BR>- Journeyman: Regular<BR>- Expert: Greater<BR>- Adept: Deadly<BR>- Master: Lethal<BR><BR>The strength of the dumped poison depends on three factors: 40% relies on one's alchemy skill, another 40% relies on one's tasting ability, and the last 20% is based on one's poisoning skill. The better these elements are, the more deadly the dumped poison will be.<BR><BR>One can also taint food with these poisons or soak their bladed weapon with it. Assassins have two methods for handling poisoned weapons. The simple method involves soaking the blade and having it poison whenever it strikes the opponent. With this method, there is little control over the dosage, but it is easier to maneuver. The tactical method, on the other hand, allows the assassin to control when the poison is administered with each hit, but only certain weapons can be poisoned. This method also allows the potential for poisoning certain arrows. The choice of methods can be switched at any time, but only one method can be in use at a given time.";
            }
            else if (book is WorkShoppes)
            {
                book.BookText =
                    "The world is filled with opportunities, where adventurers seek the help of others in order to achieve their goals. With filled coin purses, they seek experts in various crafts to acquire their skills. Some need armor repaired, maps deciphered, potions concocted, scrolls translated, clothing fixed, or many other things. The merchants in the cities and villages often cannot keep up with the demand for these requests. This provides an opportunity for those who practice a trade and have their own home from which to conduct business. Seek out a tradesman and see if they have an option for you to have them build you a Shoppe of your own. These Shoppes usually require you to part with 10,000 gold, but they can quickly pay for themselves if you are good at your craft. You may only have one type of each Shoppe at any given time. So if you are skilled in two different types of crafts, then you can have a Shoppe for each. You will be the only one to use the Shoppe, but you may give permission to others to transfer the gold out into a bank check for themselves. Shoppes need to be stocked with tools and resources, and the Shoppe will indicate what those are. Simply drop such things onto your Shoppe to amass an inventory. When you drop tools onto your Shoppe, the number of tool uses will add to the Shoppe's tool count. A Shoppe may only hold 1,000 tools and 5,000 resources. After a set period of time, customers will make requests of you which you can fulfill or refuse. Each request will display the task, who it is for, the number of tools needed, the amount of resources required, your chance to fulfill the request (based on the difficulty and your skill), and the amount of reputation your Shoppe will acquire if you are successful.<BR><BR>If you fail to perform a selected task or refuse to do it, your Shoppe's reputation will drop by the same value you would have been rewarded with. Word of mouth travels fast in the land, and you will have less prestigious work if your reputation is low. If you find yourself reaching the lows of becoming a murderer, your Shoppe will be useless as no one deals with murderers. Any gold earned will stay within the Shoppe until you single click the Shoppe and transfer the funds out of it. Your Shoppe can have no more than 500,000 gold at a time, and you will not be able to conduct any more business in it until you withdraw the funds so it can amass more. The reputation for the Shoppe cannot go below 0, and it cannot go higher than 10,000. Again, the higher the reputation, the more lucrative work you will be asked to do. If you are a member of the associated crafting guild, your reputation will have a bonus based on your crafting skill. Below are the available Shoppes, the skills required, and the merchants that will build them for you:<br><br>Alchemist Shoppe<br>- Alchemy of 50<br>-- Alchemists<br><br><br>Baker Shoppe<br>- Cooking of 50<br>-- Bakers<br>-- Cooks<br>-- Culinaries<br><br><br>Blacksmith Shoppe<br>- Blacksmithing of 50<br>-- Blacksmiths<br><br><br>Bowyer Shoppe<br>- Bowcrafting of 50<br>-- Bowyers<br>-- Archers<br><br><br>Carpenter Shoppe<br>- Carpentry of 50<br>-- Carpenters<br><br><br>Cartography Shoppe<br>- Cartography of 50<br>-- Mapmakers<br>-- Cartographers<br><br><br>Herbalist Shoppe<br>- Druidism of 50<br>-- Druids<br>-- Herbalists<br><br><br>Librarian Shoppe<br>- Inscription of 50<br>-- Sages<br>-- Scribes<br>-- Librarians<br><br><br>Tailor Shoppe<br>- Taloring of 50<br>-- Tailors<br>-- Weavers<br>-- Leather Workers<br><br><br>Tinker Shoppe<br>- Tinkering of 50<br>-- Tinkers<br><br><br>Witches Shoppe<br>- Forensic of 50<br>-- Witches<br><br>If you wish to increase your gold earnings from the comfort of your own abode, consult the nearby provisioner and inquire about the availability of merchant crates. These specialized containers enable you to create various items and store them within the crate, subsequently to be collected by the esteemed Merchants Guild after a predetermined duration. However, should you desire to retrieve any specific items from the crate, be certain to do so prior to the guild's arrival.";
            }
            else if (book is GreyJournal)
            {
                book.BookText =
                    "It has been years since we discovered the place where Weston the tinker worked on the legendary sky ship. Long before that, most had forgotten where Weston's home was built. Long since burned to the ground, managed to find the basement. Everything appeared undisturbed. The sky ship seemed to be more than just a myth; it was right before our eyes. If one were to believe the historical significance of the relic, then the valley where Devil Guard was settled had a colorful history indeed. It's amazing to think that the stranger caused the castle to fall from the sky. Even more astonishing is the fact that the castle was sent to the past before its descent. It's often hard to believe. We keep removing items from this small ship and packing them in crates. How they make the contraption work is beyond our comprehension. We decided to make this cavern a little more hospitable, perhaps spending some nights below. This would allow us to continue our work when discoveries keep us working late into the night.";
            }
            else if (book is RuneJournal)
            {
                book.BookText =
                    "With reagents being rare in the Abyss, I began researching alternative methods to cast magic spells. I stumbled upon ancient stone tablets that describe the use of rune stones. These stones bear symbols that correspond to the incantation needed for a spell. Once the correct runes are gathered, they are placed in a magical rune bag. The bag acts as a conduit for the spell's power. This process is not simple, as acquiring the runes can be tedious, but it serves as a viable option when resources are scarce. Interestingly, the runes and bag seem to form a bond with the caster. I thought I had lost them at one point, but they mysteriously reappeared. While I could misplace my spell book and reagents, the runes provide a means for me to continue practicing magic. <BR><BR>My personal quest has been to find a spell for summoning a daemon, and I have already obtained the necessary runes to perform this spell without relying on a rare scroll. Many mages disregard the use of runes, but to me, they have proven to be a valuable form of arcana that I cannot do without. I aim to document my findings on these ancient methods of spellcasting so that others may benefit from them in the future.<BR><BR>Below is a compilation of my research on rune magic, including the known spells and the corresponding rune symbols.<BR><BR>Rune Bags:<BR><BR>Rune bags and runes possess the power to aid casters in spellcasting without the need for reagents. To use a rune bag, place one of each required rune stone inside it. This can be done by dropping the runes onto the bag or opening the bag and selecting 'OPEN' from the menu. To cast a spell, double-click the bag, ensuring that the proper runes are contained within.<BR><BR>It is important to note that even with the runes, a mage still requires the willpower and magical prowess to successfully cast a spell.<BR><BR>Following is a complete list of all known spells and the runes needed to cast them.<BR><BR><BR>Meanings of Runes<BR><BR>An - Negate/Dispel<BR>Bet - Small<BR>Corp - Death<BR>Des - Lower/Down<BR>Ex - Freedom<BR>Flam - Flame<BR>Grav - Energy/Field<BR>Hur - Wind<BR>In - Make/Create/Cause<BR>Jux - Danger/Trap/Harm<BR>Kal - Summon/Invoke<BR>Lor - Light<BR>Mani - Life/Healing<BR>Nox - Poison<BR>Ort - Magic<BR>Por - Move/Movement<BR>Quas - Illusion<BR>Rel - Change<BR>Sanct - Protection<BR>Tym - Time<BR>Uus - Raise/Up<BR>Vas - Great<BR>Wis - Knowledge<BR>Xen - Creature<BR>Ylem - Matter<BR>Zu - Sleep<BR><BR>Runes must be used in combinations to form spells of power! The meanings are the key!<BR><BR><BR>MAGERY 1ST CIRCLE<BR><BR>Clumsy<BR>  Uus Jux<BR>Create Food<BR>  In Mani Ylem<BR>Feeblemind<BR>  Rel Wis<BR>Heal<BR>  In Mani<BR>Magic Arrow<BR>  In Por Ylem<BR>Night Sight<BR>  In Lor<BR>Reactive Armor<BR>  Flam Sanct<BR>Weaken<BR>  Des Mani<BR><BR><BR>MAGERY 2ND CIRCLE<BR><BR>Agility<BR>  Ex Uus<BR>Cunning<BR>  Uus Wis<BR>Cure<BR>  An Nox<BR>Harm<BR>  An Mani<BR>Magic Trap<BR>  In Jux<BR>Magic Untrap<BR>  An Jux<BR>Protection<BR>  Uus Sanct<BR>Strength<BR>  Uus Mani<BR><BR><BR>MAGERY 3RD CIRCLE<BR><BR>Bless<BR>  Rel Sanct<BR>Fireball<BR>  Vas Flam<BR>Magic Lock<BR>  An Por<BR>Poison<BR>  In Nox<BR>Telekinesis<BR>  Ort Por Ylem<BR>Teleport<BR>  Rel Por<BR>Unlock<BR>  Ex Por<BR>Wall of Stone<BR>  In Sanct Ylem<BR><BR><BR>MAGERY 4TH CIRCLE<BR><BR>Arch Cure<BR>  Vas An Nox<BR>Arch Protection<BR>  Vas Uus Sanct<BR>Curse<BR>  Des Sanct<BR>Fire Field<BR>  In Flam Grav<BR>Greater Heal<BR>  In Vas Mani<BR>Lightning<BR>  Por Ort Grav<BR>Mana Drain<BR>  Ort Rel<BR>Recall<BR>  Kal Ort Por<BR><BR><BR>MAGERY 5TH CIRCLE<BR><BR>Blade Spirits<BR>  In Jux Hur Ylem<BR>Dispel Field<BR>  An Grav<BR>Incognito<BR>  Kal In Ex<BR>Magic Reflection<BR>  In Jux Sanct<BR>Mind Blast<BR>  Por Corp Wis<BR>Paralyze<BR>  An Ex Por<BR>PoisonField<BR>  In Nox Grav<BR>Summon Creature<BR>  Kal Xen<BR><BR><BR>MAGERY 6TH CIRCLE<BR><BR>Dispel<BR>  An Ort<BR>Eenrgy Bolt<BR>  Corp Por<BR>Explosion<BR>  Vas Ort Flam<BR>Invisibility<BR>  An Lor Xen<BR>Mark<BR>  Kal Por Ylem<BR>Mass Curse<BR>  Vas Des Sanct<BR>Paralyze Field<BR>  In Ex Grav<BR>Reveal<BR>  Wis Quas<BR><BR><BR>MAGERY 7TH CIRCLE<BR><BR>Chain Lightning<BR>  Vas Ort Grav<BR>Energy Field<BR>  In Sanct Grav<BR>Flame Strike<BR>  Kal Vas Flam<BR>Gate Travel<BR>  Vas Rel Por<BR>Mana Vampire<BR>  Ort Sanct<BR>Mass Dispel<BR>  Vas An Ort<BR>Meteor Swarm<BR>  Flam Kal Des Ylem<BR>Polymorph<BR>  Vas Ylem Rel<BR><BR><BR>MAGERY 8TH CIRCLE<BR><BR>Earthquake<BR>  In Vas Por<BR>Energy Vortex<BR>  Vas Corp Por<BR>Resurrection<BR>  An Corp<BR>Air Elemental<BR>  Kal Vas Xen Hur<BR>Summon Daemon<BR>  Kal Vas Xen Corp<BR>Earth Elemental<BR>  Kal Vas Xen Ylem<BR>Fire Elemental<BR>  Kal Vas Xen Flam<BR>Water Elemental<BR>  Kal Vas Xen An Flam<BR><BR><BR>NECROMANCY<BR><BR>Curse Weapon<BR>  An Sanct Grav Corp<BR>Blood Oath<BR>  In Jux Mani Xen<BR>Corpse Skin<BR>  In An Corp Ylem<BR>Evil Omen<BR>  Por Tym An Sanct<BR>Pain Spike<BR>  In Sanct<BR>Wraith Form<BR>  Rel Xen Uus<BR>Mind Rot<BR>  Wis An Bet<BR>Summon Familiar<BR>  Kal Xen Bet<BR>Animate Dead<BR>  Uus Corp<BR>Horrific Beast<BR>  Rel Xen Vas Bet<BR>Poison Strike<BR>  In Vas Nox<BR>Wither<BR>  Kal Vas An Flam<BR>Strangle<BR>  In Bet Nox<BR>Lich Form<BR>  Rel Xen Corp Ort<BR>Exorcism<BR>  Ort Corp Grav<BR>Vengeful Spirit<BR>  Kal Xen Bet Bet<BR>Vampiric Embrace<BR>  Rel Xen An Sanct<BR><BR><BR><BR>";
            }
        }
    }

    public class TendrinsJournal : DynamicBook
    {
        [Constructable]
        public TendrinsJournal()
        {
            Weight = 1.0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(30, this);
            SetStaticText(this);
            BookTitle = "Tendrin's Journal";
            Name = BookTitle;
            BookAuthor = "Tendrin Horum";
        }

        public TendrinsJournal(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class CBookNecroticAlchemy : DynamicBook
    {
        [Constructable]
        public CBookNecroticAlchemy()
        {
            Weight = 1.0;
            Hue = 0x4AA;
            ItemID = 0x2253;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(32, this);
            SetStaticText(this);
            BookTitle = "Necrotic Alchemy";
            Name = BookTitle;
            switch (Utility.RandomMinMax(0, 3))
            {
                case 0:
                    BookAuthor = NameList.RandomName("vampire") + " the Vampire";
                    break;
                case 1:
                    BookAuthor = NameList.RandomName("ancient lich") + " the Lich";
                    break;
                case 2:
                    BookAuthor = NameList.RandomName("evil mage") + " the Warlock";
                    break;
                case 3:
                    BookAuthor = NameList.RandomName("evil witch") + " the Witch";
                    break;
            }
        }

        public CBookNecroticAlchemy(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
            Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerCallback(Delete));
        }
    }

    public class CBookDruidicHerbalism : DynamicBook
    {
        [Constructable]
        public CBookDruidicHerbalism()
        {
            Weight = 1.0;
            ItemID = 0x2D50;
            Hue = 0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(45, this);
            SetStaticText(this);
            BookTitle = "Druidic Herbalism";
            Name = BookTitle;
            BookAuthor = NameList.RandomName("druid") + " the Druid";
        }

        public CBookDruidicHerbalism(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            Hue = 0;
            SetStaticText(this);
            Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerCallback(Delete));
        }
    }

    public class LoreGuidetoAdventure : DynamicBook
    {
        [Constructable]
        public LoreGuidetoAdventure()
        {
            Weight = 1.0;
            ItemID = Utility.RandomList(0x4FDF, 0x4FE0);

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(5, this);
            SetStaticText(this);
            BookTitle = "Guide to Adventure";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public LoreGuidetoAdventure(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class BookGuideToAdventure : DynamicBook
    {
        public Mobile owner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [Constructable]
        public BookGuideToAdventure()
        {
            Weight = 1.0;
            ItemID = Utility.RandomList(0x4FDF, 0x4FE0);

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(5, this);
            SetStaticText(this);
            BookTitle = "Guide to Adventure";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public BookGuideToAdventure(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
            writer.Write((Mobile)owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
            owner = reader.ReadMobile();
        }
    }

    public class BookBottleCity : DynamicBook
    {
        [Constructable]
        public BookBottleCity()
        {
            Weight = 1.0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(30, this);
            SetStaticText(this);
            BookTitle = "The Bottle City";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public BookBottleCity(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class BookofDeadClue : DynamicBook
    {
        [Constructable]
        public BookofDeadClue()
        {
            Weight = 1.0;
            Hue = 932;
            ItemID = 0x2253;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(35, this);
            SetStaticText(this);
            BookTitle = "Barge of the Dead";
            Name = BookTitle;
            switch (Utility.RandomMinMax(0, 3))
            {
                case 0:
                    BookAuthor = NameList.RandomName("vampire") + " the Vampire";
                    break;
                case 1:
                    BookAuthor = NameList.RandomName("ancient lich") + " the Lich";
                    break;
                case 2:
                    BookAuthor = NameList.RandomName("evil mage") + " the Necromancer";
                    break;
                case 3:
                    BookAuthor = NameList.RandomName("evil witch") + " the Witch";
                    break;
            }
        }

        public BookofDeadClue(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class CBookTombofDurmas : DynamicBook
    {
        [Constructable]
        public CBookTombofDurmas()
        {
            Weight = 1.0;
            Hue = 0x966;
            ItemID = 0x2253;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(14, this);
            SetStaticText(this);
            BookTitle = "Tomb of Durmas";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public CBookTombofDurmas(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class CBookElvesandOrks : DynamicBook
    {
        [Constructable]
        public CBookElvesandOrks()
        {
            Weight = 1.0;
            Hue = 956;
            ItemID = 0xFF4;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(64, this);
            SetStaticText(this);
            BookTitle = "Elves and Orks";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public CBookElvesandOrks(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class MagestykcClueBook : DynamicBook
    {
        [Constructable]
        public MagestykcClueBook()
        {
            Weight = 1.0;
            Hue = 509;
            ItemID = 0x22C5;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(12, this);
            SetStaticText(this);
            BookTitle = "Wizards in Exile";
            Name = BookTitle;
            switch (Utility.RandomMinMax(0, 1))
            {
                case 0:
                    BookAuthor = NameList.RandomName("evil mage") + " the Wizard";
                    break;
                case 1:
                    BookAuthor = NameList.RandomName("evil witch") + " the Sorceress";
                    break;
            }
        }

        public MagestykcClueBook(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class FamiliarClue : DynamicBook
    {
        [Constructable]
        public FamiliarClue()
        {
            Weight = 1.0;
            Hue = 459;
            ItemID = 0x22C5;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(46, this);
            SetStaticText(this);
            BookTitle = "Journal";
            Name = BookTitle;
            switch (Utility.RandomMinMax(0, 1))
            {
                case 0:
                    BookAuthor = NameList.RandomName("male") + " the Awkward";
                    break;
                case 1:
                    BookAuthor = NameList.RandomName("female") + " the Awkward";
                    break;
            }
        }

        public FamiliarClue(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class LodorBook : DynamicBook
    {
        [Constructable]
        public LodorBook()
        {
            Weight = 1.0;
            Hue = 0;
            ItemID = 0x1C11;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(64, this);
            SetStaticText(this);
            BookTitle = "Diary";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public LodorBook(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class CBookTheLostTribeofSosaria : DynamicBook
    {
        [Constructable]
        public CBookTheLostTribeofSosaria()
        {
            Weight = 1.0;
            Hue = 0;
            ItemID = 0xFEF;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(42, this);
            SetStaticText(this);
            BookTitle = "Lost Tribe of Sosaria";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public CBookTheLostTribeofSosaria(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class LillyBook : DynamicBook
    {
        [Constructable]
        public LillyBook()
        {
            Weight = 1.0;
            Hue = 0;
            ItemID = 0x225A;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(57, this);
            SetStaticText(this);
            BookTitle = "Gargoyle Secrets";
            Name = BookTitle;
            BookAuthor = RandomThings.GetRandomAuthor();
        }

        public LillyBook(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class LearnTraps : DynamicBook
    {
        [Constructable]
        public LearnTraps()
        {
            Weight = 1.0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(2, this);
            SetStaticText(this);
            BookTitle = "Hidden Traps";
            Name = BookTitle;
            BookAuthor = "Girmo the Legless";
        }

        public LearnTraps(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class LearnTitles : DynamicBook
    {
        [Constructable]
        public LearnTitles()
        {
            Weight = 1.0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(17, this);
            SetStaticText(this);
            BookTitle = "Titles of the Skilled";
            Name = BookTitle;
            BookAuthor = "Cartwise the Librarian";
        }

        public LearnTitles(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class GoldenRangers : DynamicBook
    {
        [Constructable]
        public GoldenRangers()
        {
            Weight = 1.0;
            Hue = 0;
            ItemID = 0x222D;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(48, this);
            SetStaticText(this);
            BookTitle = "The Golden Rangers";
            Name = BookTitle;
            BookAuthor = "Vara the Explorer";
        }

        public GoldenRangers(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class AlchemicalElixirs : DynamicBook
    {
        [Constructable]
        public AlchemicalElixirs()
        {
            Weight = 1.0;
            Hue = 0;
            ItemID = 0x2219;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(32, this);
            SetStaticText(this);
            BookTitle = "Alchemical Elixirs";
            Name = BookTitle;
            BookAuthor = "Vragan the Mixologist";
        }

        public AlchemicalElixirs(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class AlchemicalMixtures : DynamicBook
    {
        [Constructable]
        public AlchemicalMixtures()
        {
            Weight = 1.0;
            Hue = 0;
            ItemID = 0x2223;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(32, this);
            SetStaticText(this);
            BookTitle = "Alchemical Mixtures";
            Name = BookTitle;
            BookAuthor = "Miranda the Chemist";
        }

        public AlchemicalMixtures(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class BookOfPoisons : DynamicBook
    {
        [Constructable]
        public BookOfPoisons()
        {
            Weight = 1.0;
            Hue = 0xB51;
            ItemID = 0x2253;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(72, this);
            SetStaticText(this);
            BookTitle = "Venom and Poisons";
            Name = BookTitle;
            BookAuthor = "Seryl the Assassin";
        }

        public BookOfPoisons(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class WorkShoppes : DynamicBook
    {
        [Constructable]
        public WorkShoppes()
        {
            Weight = 1.0;
            Hue = 0xB50;
            ItemID = 0x2259;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(59, this);
            SetStaticText(this);
            BookTitle = "Work Shoppes";
            Name = BookTitle;
            BookAuthor = "Zanthura of the Coin";
        }

        public WorkShoppes(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class GreyJournal : DynamicBook
    {
        [Constructable]
        public GreyJournal()
        {
            Weight = 1.0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(71, this);
            SetStaticText(this);
            BookTitle = "Legend of the Sky Castle";
            Name = BookTitle;
            BookAuthor = "Ataru Callis";
            ItemID = 0x1C13;
            Hue = 0;
        }

        public GreyJournal(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }

    public class RuneJournal : DynamicBook
    {
        [Constructable]
        public RuneJournal()
        {
            Weight = 1.0;

            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            SetBookCover(46, this);
            SetStaticText(this);
            BookTitle = "Rune Magic";
            Name = BookTitle;
            BookAuthor = "Garamon the Wizard";
            ItemID = 0x5687;
            Hue = 0xAFE;
        }

        public RuneJournal(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            SetStaticText(this);
        }
    }
}
