using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class SwordsAndShackles : Item
    {
        [Constructable]
        public SwordsAndShackles()
            : base(0x529D)
        {
            Weight = 1.0;
            Hue = 0x944;
            ItemID = Utility.RandomList(0x529D, 0x529E);
            Name = "Skulls and Shackles";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 4) || this.Weight == -50.0)
            {
                from.CloseGump(typeof(SwordsAndShacklesGump));
                from.SendGump(new SwordsAndShacklesGump(from, 1));
                Server.Gumps.MyLibrary.readBook(this, from);
            }
        }

        public SwordsAndShackles(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public class SwordsAndShacklesGump : Gump
        {
            private int m_Page;

            public SwordsAndShacklesGump(Mobile from, int page)
                : base(50, 50)
            {
                from.SendSound(0x55);
                string color = "#76b4d4";
                m_Page = page;

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                int max = 11; // MAX PAGES
                int page1 = m_Page - 1;
                if (page1 < 1)
                {
                    page1 = max;
                }
                int page2 = m_Page + 1;
                if (page2 > max)
                {
                    page2 = 1;
                }

                AddImage(0, 0, 7010, 2878);
                AddImage(0, 0, 7011);
                AddImage(0, 0, 7025, 2736);
                AddButton(110, 67, 4014, 4014, page1, GumpButtonType.Reply, 0);
                AddButton(906, 70, 4005, 4005, page2, GumpButtonType.Reply, 0);

                AddHtml(
                    596,
                    72,
                    299,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + "><CENTER>SKULLS & SHACKLES</CENTER></BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (m_Page == 1)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>THE BASICS OF FISHING</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Fishing is the patient art of luring fish towards your lure in the pursuit of feeding yourself. In its basic form, you would acquire a fishing pole and head towards the shore to see what you can catch. Fishing poles can be crafted by carpenters or purchased from fishermen. A fishing pole can only be used a set number of times and will eventually break from overuse. Simply hold the fishing pole, use it, and then select a nearby spot in the water to fish. During this initial practice, you will catch some fish or pull up soggy clothes or rusty armor. It may be morbid, but you might even pull up the remains of a drowning victim or a bag someone may have dropped in the water years ago.<BR><BR>You cannot practice on the shore forever if you plan to become even more proficient. After you reach the level of apprentice (50) in seafaring, you will have to acquire a ship and sail away from the safety of the shore. Here, you will catch more types of things but also risk fishing up sea serpents. If you face such a beast and claim victory over it, you may find special items. If you happen to catch a glimpse of a sunken ship below the surface of the waves, you may be able to pull up some decorative treasure from the wreckage below.<BR><BR>You will likely come across some unusually exotic fish out at sea. Although you are free to slice these up and cook them, they are often more valuable due to how rare they are. If you want to earn some gold for these types of fish, simply find a dock that has a fish tub and place the fish within it. You will be awarded an amount of gold that can be increased based on your seafaring skill.<BR><BR>Magic fish are often caught by expert fishermen, and you can simply eat these raw and have a temporary increase in strength, dexterity, or intelligence. Unusual seaweed may get caught on your line, and they should not simply be tossed away. Look these plants over because they commonly have alchemical properties that could be of great use when far from land. You will need empty bottles, and then you could use the seaweed to attempt to squeeze the fluids from them to create potions. As previously mentioned, you could likely pull up rusty armor and weapons. Although useless to adventurers, these rusty items can be melted down and reused by ironworkers and blacksmiths in town. Simply bring the items to their shops and place them into scrap iron barrels to acquire gold. The payment for such items is a gold coin per stone weight of the rusty item.<BR><BR>Those who choose to be known by this profession are either known as 'sailors' or 'pirates', depending on your karma. Those of barbaric backgrounds will have the title of 'Atlantean' as that is their seaworthy heritage. Grandmasters of this skill are given the additional title of 'captain'.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)true
                    );
                    AddImage(594, 180, 10887);
                }
                else if (m_Page == 2)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>THROWING HARPOONS</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Harpoons are often the preferred weapon of sailors on the high seas. With the skill of marksmanship, one can hurl a spear-like weapon at their adversary. While these weapons are sold by sailing merchants, blacksmiths have the ability to construct them. To effectively utilize a harpoon, one must be able to throw it and then retract it to throw again. This necessitates a sufficient supply of harpoon rope. This type of rope is both inexpensive and readily available from sailor merchants. Tailors are also capable of weaving such rope. When throwing a harpoon, these ropes are typically expended, so it is important to bring an ample supply on your journey if you choose to wield such a weapon. Additionally, a higher level of seafaring skill will enhance the effectiveness of these weapons.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddImage(594, 180, 10887);
                }
                else if (m_Page == 3)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>YOUR OWN SHIP</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">When you have earned enough gold from your various efforts, you may be able to own your very own ship. Shipwrights often sell ships, with the larger the ship, the larger the hold for storing your cargo. To launch a ship or put it in dry dock, you most commonly need to be at a dock. There are exceptions to this, as there are a small number of islands that allow you to launch and dock your ship. Sea captains do not suffer from this limitation, as they may launch and dock their ships from any shore. Sailing merchants sometimes sell docking lanterns, which are especially bright lanterns for ships to dock. These can only be used from your home and they allow you to build a seaside home to dock and launch your ship if you have not achieved that level of sailing skill. When you launch your ship, you are given a key for your pack and your bank. You can use this to secure your boat or cast recalling teleport magic on it to return to your ship from afar. If you are nowhere near your ship and you have no such magic, you can give your key to a shipwright, and they will transport you to your vessel. This will cost you 1,000 gold, of course.<BR><BR>To pilot your ship, simply be on board and double-click the tiller man. A steering mechanism will appear, and you can then sail the seas. This mechanism also allows you to rename your vessel and raise or drop the anchor. You must be on board when using this mechanism. The center of the mechanism is transparent, so you can position it over your radar map if you choose. The mechanism allows you to size it to match the two styles of radar maps to overlay.<BR><BR>Masters in the seafaring skill will have an additional feature to their ships that other sailors do not: a lower deck. This is a public area below your ship that has comforts such as a tavern, provisioner, bank, and healer. As long as you are not in combat on your ship, you can go below deck and relax. If your ship is commanded to sail, and you go below deck, your ship will sail onward until it is stopped by an obstacle. If your real-world comrades are below deck and you dock your ship, your comrades will appear on the land where you docked your ship.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)true
                    );
                    AddImage(638, 180, 10890);
                }
                else if (m_Page == 4)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>SAILING YOUR SHIP</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddImage(279, 305, 10921);
                    AddHtml(
                        126,
                        102,
                        801,
                        203,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">After launching your ship into the sea, it's time to board and set sail. Once on board, double-click the tillerman to open a window with a steering wheel. The gold buttons on the wheel control the vessel's movement in specific directions. The red button in the center will stop the ship. Additional buttons on the left are labeled with their respective functions, such as dropping or raising the anchor, naming your vessel, turning left or right, coming about, or enabling/disabling the one-step movement. Plotting a course using maps is also possible, but only on maps created by other characters; it cannot be done on the large world maps. To plot a course, open the map and select the course plotting option at the top. Choose the desired path points on the map, ensuring there are no land masses obstructing the route. Once finished, click the top of the map to indicate completion. The plotted course can be cleared using the option at the bottom. If you are satisfied with the plotted course, hand the map to the tillerman for verification. Instruct the tillerman to follow the course by saying 'start' while on the ship. The tillerman will then navigate according to the plotted course.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                }
                else if (m_Page == 5)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>FISHING NETS</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Fishing nets are rare items that fishermen use to discover what lies deep beneath the surface. They can be obtained by slaying sea monsters or acquiring ocean treasures, and there are four levels of difficulty to them:<BR><BR>- Fishing Net<BR>- Strong Fishing Net<BR>- Ancient Fishing Net<BR>- Neptune's Fishing Net<BR><BR>These nets cannot be utilized near any shores. When deployed, the net gradually descends into the sea accompanied by a trail of bubbles. A second wave of bubbles signifies the emergence of creatures surrounding your ship. Therefore, one must be prepared to engage in battle. These nets are reserved for the most courageous sailors seeking to establish a reputation on the vast ocean.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddImage(594, 180, 10887);
                }
                else if (m_Page == 6)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>MESSAGES IN A BOTTLE</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">On occasion, a lucky sailor will discover a message in a bottle. Looking at the note inside reveals a message from a sailor aboard a sinking ship. The notes appear old, so the chances of the ship surviving are slim. To attempt to retrieve the wreckage, you need a sextant and a fishing pole. Board your ship and sail to the coordinates on the note. Once you reach the spot, ensure you have the bottled message in your pack and then start fishing in the waters. Most sailors aim to retrieve the ship's chest of plunder that it held before sinking into the murky depths below. While attempting to retrieve the chest, you may bring up other parts of the wreckage, such as paintings and the bones of the sailors who met their fate on that ship. Keep in mind that these shipwrecks are not visible on the ocean floor. You can fish in those spots, but the chest of riches it contained was lost long ago.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddImage(594, 180, 10887);
                }
                else if (m_Page == 7)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>DESERTED BOATS</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Some individuals embark in the morning to catch fish for their families' consumption. Equipped with their small boats, they venture away from the shore in hopes of catching larger fish. However, there are some who never return. Whether they have fallen out of their boats and drowned or their anchor lines have broken, causing their boats to drift away, these abandoned vessels now float aimlessly at sea, waiting to be stumbled upon by others. Occasionally, these boats contain valuable items and treasures, providing an opportunity for those who come across them to search for potential spoils. Many sailors carry an axe to aid in dismantling these ships, allowing them to salvage usable wood. The expertise of the sailor greatly influences the quality of the salvaged wood, as their carpentry skills determine the amount of wood that can be successfully obtained.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddItem(675, 397, 8857);
                    AddItem(682, 203, 8862);
                }
                else if (m_Page == 8)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>SMALL BOATS</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">There are small boats out at sea that can be easily identified as they lack sails or a tiller man. However, they typically have a single crew member on board. These crew members may be innocent citizens out fishing or transporting cargo, or they could be roguish pirates trying to establish their reputation without a crew or galleon at their disposal. How you choose to handle these boats depends on your morality. The innocent sailors will usually ignore others unless they are murderers, criminals, or possess unsavory karma. The buccaneers and others of their ilk, however, will attack you on sight. It is up to you to decide how to deal with these sailors. You can choose to attack them from a distance or board their ship if you have a grappling hook in your possession. Grappling hooks can be purchased from sailor merchants and are specifically designed for boarding smaller boats or large galleons. To board a ship, simply use the grappling hook to target the crew member on the boat. If you manage to slay the crew member, their boat will sink with only a small section visible above the water. Fortunately, this section will be the hold of the boat, allowing you to search their belongings and take what you desire. Similar to abandoned boats, you can also dismantle these hulls to obtain wood.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddItem(638, 254, 20889);
                    AddItem(692, 307, 6045);
                    AddImage(809, 463, 10889);
                    AddItem(650, 352, 6055);
                    AddItem(711, 299, 6053);
                    AddItem(671, 329, 6045);
                }
                else if (m_Page == 9)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>GALLEONS</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Galleons are enormous ships that only a select few could ever acquire. It is highly unlikely that you would ever be in a position to own such a vessel, but there are individuals out there who possess them and can be seen on the high seas. Like smaller boats, galleons can be occupied by either innocent sailors or villains. Pirates tend to attack the innocent sailors' crew and plunder their wealth. Regardless of your motivations, you have the option to engage in combat with any galleon you come across. A galleon captain typically commands a crew of 9-12 members, all of whom are armed and ready to shoot or throw various projectiles at nearby enemies. Some may choose to throw rocks, daggers, harpoons, or even boulders. Others might possess magical abilities and can unleash bolts of fire, cold, or energy. Galleon captains often allow their crew to fight, intervening only when necessary, particularly if enemies have not yet boarded their ship.<BR><BR>While it is possible for you to engage the crew directly, you may not have the desire or means to do so. Just like with smaller boats, boarding a galleon requires the use of a grappling hook. Simply aim at a crewmember and use the grappling hook to launch yourself onto their ship. If you need to return to your own ship, you can use a plank. However, be aware that galleon captains also possess grappling hooks that they frequently utilize. It would be a mistake to assume that you can attack the captain safely from your deck. They will most likely employ their grappling hook to seize you and pull you towards their ship, making it easier for them to eliminate you.<BR><BR>Unlike smaller boats, galleon captains can prove to be formidable opponents due to their strength or extraordinary abilities. They wouldn't have acquired a galleon and crew without possessing such qualities. If you intend to attack such a vessel, it would be wise to assess the situation as the battle progresses. The more powerful the captain, the greater the rewards within the ship's hold. While pirates and sailors are often portrayed as humans or elves, the high seas are inhabited by various creatures that seek to pillage the waves. You might encounter a powerful ogre and their crew of monstrous beings. A devil from hell could command a vessel with a demonic crew searching for souls. If you are a pirate, you might encounter a militia crew of sailing soldiers, determined to bring criminals like you to justice. Regardless of the crew, they will fiercely support their captain. Any remaining crew members will attend to the captain's wounds, so it is essential to eliminate them before facing off against the captain.<BR><BR>If you manage to sink a galleon, you will see its submerged hold protruding from the water's surface. Similar to smaller boats, you can search through the hold for any desired goods and riches. If you are in need of quality deck planks, be sure to salvage what remains of the ship. It is important to thoroughly inspect a pirate's loot, as there may be a bounty on their head, and the wanted parchment could be found in the hold. By examining the parchment, you can determine the reward for capturing that particular pirate. Handing over this bounty parchment to a town or city guard will grant you the reward.<BR><BR>Lastly, not all adversaries on the high seas are pirates. Some are cultists who worship the vile water deities of myth and legend. They are solely focused on purging the seas of those who trespass in their gods' domain. Similar to pirates, they are often sought after for justice or feared by sailors. They take whatever they desire, as long as it aligns with their sinister plans, showing little sympathy for those from whom they take it.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)true
                    );
                    AddImage(598, 249, 10886);
                }
                else if (m_Page == 10)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>CARGO</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Most ships primarily deal in cargo, whether it is being transported from settlement to settlement or obtained through plunder. Cargo holds great value on the high seas. When you sink a vessel, be it a small boat or a grand galleon, you may find various types of cargo within its hold. This cargo can take the form of crates, chests, tubs, or barrels. By peeking into the contents of the cargo, you can determine its nature and how to make the best use of it.<BR><BR>If you claim cargo from pirates or villains, the cargo will indicate that it was seized, and you will gain karma if you choose to give the goods to the designated merchant who desires it. On the other hand, if you take cargo from innocent individuals, it will indicate that it was plundered, and you will lose karma if you decide to exchange it with merchants. Regardless of the source, acquiring cargo can earn you fame.<BR><BR>The value of cargo is determined by several factors. Firstly, there is a random value assigned to the cargo itself. Then, you take into account your skill as a sailor (seafaring) and your effectiveness as a merchant (mercantile). If you are a beggar by nature, and your demeanor reflects that, you may receive additional gold for the cargo. Additionally, being a member of the Mariners Guild grants a bonus to the value of the cargo. Lastly, whether you are in a port or not affects the amount of gold you receive when parting with the cargo. However, you are free to dispose of it anywhere you find an appropriate merchant.<BR><BR>In most cases, you can simply claim the cargo for yourself. Each cargo you claim will have the necessary option for you to do so. The cargo information will indicate the quantity of items stored within the container. If you choose to keep it, you will receive both the contents and the container itself. For example, if you desire a barrel of 100 lemons, you will receive the barrel along with the 100 lemons. The decision of which cargo to keep is entirely up to you.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)true
                    );
                    AddImage(594, 180, 10887);
                }
                else if (m_Page == 11)
                {
                    AddHtml(
                        151,
                        72,
                        299,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + "><CENTER>SAILING PORTS</CENTER></BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    AddHtml(
                        116,
                        107,
                        393,
                        486,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">There are sailing ports in many lands that are homes to sailors and pirates who have little interest in returning to the mainland.<BR><BR>- Sosaria: Port of Anchor Rock<BR>- Lodoria: Port of Kraken Reef<BR>- Serpent Island: Port of Serpent Sails<BR>- Isles of Dread: Port of Shadows<BR>- Savaged Empire: Port of Savage Seas<BR><BR>These ports are neutral territory for both sailors and pirates, so the laws are very strict to ensure no harm comes to those who visit. Some of the docks around these ports are larger than others. The Port of Shadows is hidden near the Forgotten Lighthouse, while Anchor Rock and Kraken Reef are built on top of large cavernous areas where some sailors choose to build their homes. Others opt to mine for nepturite or gather driftwood from the dead trees within. The settlement area, on top of these rocky formations, is where sailors come to rest and barter for goods and services. This particular part of the port is a public area, allowing visitors from any port village to interact with one another. Ports are common places where sailors or pirates unload their cargo, as they can get more gold for such transactions.<BR><BR>The dock area is where you can leave your ship if you choose to embark again later. Depending on the port you visit, the dock can vary in size. Some docks provide access to other public areas like the bank, tavern, or guilds. Any creature chasing your ship will often dive below the surface when approaching the docks, which is why many sailors seek refuge there when they are too weak to fight.</BASEFONT></BODY>",
                        (bool)false,
                        (bool)true
                    );
                    AddImage(596, 140, 10891);
                }
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                if (info.ButtonID > 0 || info.ButtonID < 0)
                {
                    from.SendGump(new SwordsAndShacklesGump(from, info.ButtonID));
                }
                else
                    from.PlaySound(0x55);
            }
        }
    }
}
