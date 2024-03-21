using System;
using System.Collections;
using System.Net;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    [Flipable(0x577B, 0x577C)]
    public class GuildBoard : Item
    {
        [Constructable]
        public GuildBoard()
            : base(0x577B)
        {
            Weight = 1.0;
            Name = "Local Guilds";
            Hue = 0xB79;
        }

        public override void OnDoubleClick(Mobile e)
        {
            if (e.InRange(this.GetWorldLocation(), 4))
            {
                e.CloseGump(typeof(GuildBoardGump));
                e.SendGump(new GuildBoardGump(e));
            }
            else
            {
                e.SendLocalizedMessage(502138); // That is too far away for you to use
            }
        }

        public class GuildBoardGump : Gump
        {
            public GuildBoardGump(Mobile from)
                : base(100, 100)
            {
                from.SendSound(0x59);
                string guildMasters = "<br><br>";
                foreach (Mobile target in World.Mobiles.Values)
                    if (target is BaseGuildmaster)
                    {
                        guildMasters =
                            guildMasters
                            + target.Name
                            + "<br>"
                            + target.Title
                            + "<br>"
                            + Server.Misc.Worlds.GetRegionName(target.Map, target.Location)
                            + "<br><br>";
                    }

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);
                AddImage(0, 0, 9541, Server.Misc.PlayerSettings.GetGumpHue(from));

                PlayerMobile pm = (PlayerMobile)from;
                if (pm.NpcGuild != NpcGuild.None)
                {
                    AddHtml(
                        55,
                        402,
                        285,
                        20,
                        @"<BODY><BASEFONT Color=#e97f76>Resign From My Local Guild</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddButton(16, 401, 4005, 4005, 10, GumpButtonType.Reply, 0);
                }

                AddHtml(
                    11,
                    12,
                    562,
                    20,
                    @"<BODY><BASEFONT Color=#b6d593>LOCAL GUILDS</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddHtml(
                    12,
                    44,
                    623,
                    349,
                    @"<BODY><BASEFONT Color=#b6d593>There are numerous groups in the land that have established guild houses and are frequently seeking new members. These guilds are distinct from the various adventurer guilds that may exist independently, as they focus on a specific group of individuals with a particular skillset and trade. Below is a list of guild houses currently seeking members:<br><br>- Alchemists Guild<br>- Archers Guild<br>- Assassins Guild<br>- Bard Guild<br>- Blacksmith Guild<br>- Carpenters Guild<br>- Cartographers Guild<br>- Culinary Guild<br>- Druids Guild<br>- Elemental Guild<br>- Healer Guild<br>- Jesters Guild<br>- Librarians Guild<br>- Mage Guild<br>- Mariners Guild<br>- Merchant Guild<br>- Miner Guild<br>- Necromancers Guild<br>- Ranger Guild<br>- Tailor Guild<br>- Thief Guild<br>- Tinker Guild<br>- Warrior Guild<br><br>To join any of these guilds (provided you are not already a member of another local guild), you must pay the guildmaster 2,000 gold as an entry fee. To join a guild, locate the appropriate guildmaster and single-click them to select 'Join'. They will then ask you for the required amount of gold if you meet the qualifications. Simply drop the exact amount of gold on them to join. If you wish to resign from a guild, return to your guildmaster, single-click them, and select 'Resign' (alternatively, you can use this board to resign). After resigning, you can join another guild. However, please note that each guild you join will have double the fee of the previous one. For example, if you join a guild for 2,000 gold, the next guild you join will require 4,000 gold. The guild after that will cost 8,000 gold. One of the benefits of joining a local guild is the ability to receive more gold for goods sold to other guild members. Additionally, upon joining, you will receive a guild membership ring that is exclusive to you and provides assistance with skills related to the guild. If you happen to lose your ring, you can obtain a replacement from a guildmaster for 400 gold. The ring also accelerates the acquisition of skills associated with the guild. Lastly, as a member of a guild, you will have the opportunity to purchase items from guildmasters, as they offer additional items exclusively to guild members.<br><br>Please note that in order to engage in theft against other players, membership in the Thieves Guild is required."
                        + guildMasters
                        + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
                AddButton(609, 8, 4017, 4017, 0, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                PlayerMobile pm = (PlayerMobile)from;
                from.SendSound(0x59);

                if (info.ButtonID > 0)
                {
                    pm.NpcGuild = NpcGuild.None;
                    from.SendMessage(0X22, "You have resigned from the local guild.");

                    if (((PlayerMobile)from).CharacterGuilds > 0)
                    {
                        int fees = ((PlayerMobile)from).CharacterGuilds;
                        ((PlayerMobile)from).CharacterGuilds = fees * 2;
                    }
                    else
                    {
                        ((PlayerMobile)from).CharacterGuilds = 4000;
                    }

                    ArrayList targets = new ArrayList();
                    foreach (Item item in World.Items.Values)
                        if (item is GuildRings)
                        {
                            GuildRings guildring = (GuildRings)item;
                            if (guildring.RingOwner == from)
                            {
                                targets.Add(item);
                            }
                        }
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Item item = (Item)targets[i];
                        item.Delete();
                    }
                }
            }
        }

        public GuildBoard(Serial serial)
            : base(serial) { }

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
    }
}
