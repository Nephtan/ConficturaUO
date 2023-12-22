using System;
using System.Collections;
using System.Globalization;
using System.Text;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class LoreBook : DynamicBook
    {
        [Constructable]
        public LoreBook()
        {
            Weight = 1.0;

            if (BookTrue > 0) { }
            else
            {
                writeBook(Utility.RandomMinMax(0, 46));
            }
        }

        public void writeBook(int val)
        {
            BookRegion = null;
            BookMap = null;
            BookWorld = null;
            BookItem = null;
            BookTrue = 1;
            BookPower = 0;

            ItemID = RandomThings.GetRandomBookItemID();
            Hue = Utility.RandomColor(0);
            SetBookCover(0, this);
            BookTitle = Server.Misc.RandomThings.GetBookTitle();
            Name = BookTitle;
            BookAuthor = Server.Misc.RandomThings.GetRandomAuthor();

            switch (val)
            {
                case 0:
                    BookTitle = "Akalabeth's Tale";
                    BookAuthor = "Shamino the Anarch";
                    SetBookCover(1, this);
                    break;
                case 1:
                    BookTitle = "The Lost Land";
                    BookAuthor = "Sentri the Seeker";
                    SetBookCover(42, this);
                    break;
                case 2:
                    BookTitle = "The Balance Vol I of II";
                    BookAuthor = "Dedric the Knight";
                    SetBookCover(80, this);
                    break;
                case 3:
                    BookTitle = "The Balance Vol II of II";
                    BookAuthor = "Dedric the Knight";
                    SetBookCover(80, this);
                    break;
                case 4:
                    BookTitle = "The Black Gate Demon";
                    BookAuthor = "Zalifar the Wizard";
                    SetBookCover(66, this);
                    break;
                case 5:
                    BookTitle = "The Blue Ore";
                    BookAuthor = "Jarg the Blacksmith";
                    SetBookCover(69, this);
                    break;
                case 6:
                    BookTitle = "Crystal Flasks";
                    BookAuthor = "Frug the Explorer";
                    SetBookCover(32, this);
                    break;
                case 7:
                    BookTitle = "The Curse of the Island";
                    BookAuthor = "Sempkin Burg";
                    SetBookCover(23, this);
                    break;
                case 8:
                    BookTitle = "The Dark Age";
                    BookAuthor = "Nedina the Ghastly";
                    SetBookCover(25, this);
                    break;
                case 9:
                    BookTitle = "The Dark Core";
                    BookAuthor = "Erethian the Mage";
                    SetBookCover(67, this);
                    break;
                case 10:
                    BookTitle = "Death to Pirates";
                    BookAuthor = "Granafla the Sailor";
                    SetBookCover(65, this);
                    break;
                case 11:
                    BookTitle = "The Death Knights";
                    BookAuthor = "Arul Martos";
                    SetBookCover(78, this);
                    break;
                case 12:
                    BookTitle = "The Darkness Within";
                    BookAuthor = "Cyrus Belmont";
                    SetBookCover(79, this);
                    break;
                case 13:
                    BookTitle = "The Destruction of Exodus";
                    BookAuthor = "Hafar of the Red Robe";
                    SetBookCover(67, this);
                    break;
                case 14:
                    BookTitle = "The Knight Who Fell";
                    BookAuthor = "Darun the Priest";
                    SetBookCover(78, this);
                    break;
                case 15:
                    BookTitle = "The Fall of Mondain";
                    BookAuthor = "Gram the Seventh";
                    SetBookCover(55, this);
                    break;
                case 16:
                    BookTitle = "Forging the Fire";
                    BookAuthor = "Malek the Smith";
                    SetBookCover(62, this);
                    break;
                case 17:
                    BookTitle = "Forgotten Dungeons";
                    BookAuthor = "Curan the Fighter";
                    SetBookCover(2, this);
                    break;
                case 18:
                    BookTitle = "The Cruel Game";
                    BookAuthor = "Killun the Poor";
                    SetBookCover(50, this);
                    break;
                case 19:
                    BookTitle = "The Ice Queen";
                    BookAuthor = "Suri the Bard";
                    SetBookCover(34, this);
                    break;
                case 20:
                    BookTitle = "Luck of the Rogue";
                    BookAuthor = "The Gray Mouser";
                    SetBookCover(13, this);
                    break;
                case 21:
                    BookTitle = "A Tattered Journal";
                    BookAuthor = "Unknown";
                    SetBookCover(0, this);
                    break;
                case 22:
                    BookTitle = "The Curse of Mangar";
                    BookAuthor = "Lemka the Cloaked";
                    SetBookCover(59, this);
                    break;
                case 23:
                    BookTitle = "The Times of Minax";
                    BookAuthor = "Halgram the Obscure";
                    SetBookCover(56, this);
                    break;
                case 24:
                    BookTitle = "Rangers of Lodoria";
                    BookAuthor = "Grimm the Tracker";
                    SetBookCover(77, this);
                    break;
                case 25:
                    BookTitle = "Gem of Immortality";
                    BookAuthor = "Batlin the Druid";
                    SetBookCover(58, this);
                    break;
                case 26:
                    BookTitle = "The Gods of Men";
                    BookAuthor = "Perdue the Magician";
                    SetBookCover(75, this);
                    break;
                case 27:
                    BookTitle = "Castles Above";
                    BookAuthor = "Harkan the Explorer";
                    SetBookCover(71, this);
                    break;
                case 28:
                    BookTitle = "Staff of Five Parts";
                    BookAuthor = "Zuri the Wizard";
                    SetBookCover(24, this);
                    break;
                case 29:
                    BookTitle = "The Story of Exodus";
                    BookAuthor = "Dreova of the Isles";
                    SetBookCover(67, this);
                    break;
                case 30:
                    BookTitle = "The Story of Minax";
                    BookAuthor = "Jaxina the Wise";
                    SetBookCover(56, this);
                    break;
                case 31:
                    BookTitle = "The Story of Mondain";
                    BookAuthor = "Milydor the Sage";
                    SetBookCover(55, this);
                    break;
                case 32:
                    BookTitle = "The Bard's Tale";
                    BookAuthor = "Ramzef the Bard";
                    SetBookCover(37, this);
                    break;
                case 33:
                    BookTitle = "Death Dealing";
                    BookAuthor = "Murgox the Warlock";
                    SetBookCover(27, this);
                    break;
                case 34:
                    BookTitle = "The Orb of the Abyss";
                    BookAuthor = "Gribs the High Mage";
                    SetBookCover(24, this);
                    break;
                case 35:
                    BookTitle = "The Underworld Gate";
                    BookAuthor = "Garamon the Wizard";
                    SetBookCover(2, this);
                    break;
                case 36:
                    BookTitle = "The Elemental Titans";
                    BookAuthor = "Xavier the Theurgist";
                    SetBookCover(46, this);
                    break;
                case 37:
                    BookTitle = "The Dragon's Egg";
                    BookAuthor = "Druv the Dwarf";
                    SetBookCover(9, this);
                    break;
                case 38:
                    BookTitle = "Magic in the Moon";
                    BookAuthor = "Selene the Wizard";
                    SetBookCover(71, this);
                    break;
                case 39:
                    BookTitle = "The Maze of Wonder";
                    BookAuthor = "Risa the Scholar";
                    SetBookCover(49, this);
                    break;
                case 40:
                    BookTitle = "The Pass of the Gods";
                    BookAuthor = "Mareskon the Elf";
                    SetBookCover(64, this);
                    break;
                case 41:
                    BookTitle = "Valley of Corruption";
                    BookAuthor = "Willum the Druid";
                    SetBookCover(45, this);
                    break;
                case 42:
                    BookTitle = "The Demon Shard";
                    BookAuthor = "Vanesa the Sorcereress";
                    SetBookCover(67, this);
                    break;
                case 43:
                    BookTitle = "The Syth Order";
                    BookAuthor = "Xandru the Jedi";
                    SetBookCover(78, this);
                    break;
                case 44:
                    BookTitle = "The Rule of One";
                    BookAuthor = "Asajj Ventress the Syth Lord";
                    SetBookCover(78, this);
                    ItemID = 0x4CDF;
                    Light = LightType.Circle225;
                    break;
                case 45:
                    BookTitle = "Antiquities";
                    BookAuthor = "Daran the Collector";
                    SetBookCover(7, this);
                    break;
                case 46:
                    BookTitle = "The Jedi Order";
                    BookAuthor = "Zoda the Jedi Master";
                    SetBookCover(16, this);
                    ItemID = 0x543C;
                    Light = LightType.Circle225;
                    break;
            }

            GetText(this);
            Name = BookTitle;
        }

        public LoreBook(Serial serial)
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

            if (BookTitle == "Staff of Five")
            {
                BookTitle = "Staff of Five Parts";
                Name = "Staff of Five Parts";
            }

            GetText(this);
        }

        public static void GetText(LoreBook book)
        {
            if (book.BookTitle == "Akalabeth's Tale")
            {
                book.BookText =
                    "Mondain, the second-born of Wolfgang, a great king of old, desired to obtain his brother's inheritance. To achieve this, he utilized his immense powers for malevolent purposes. Throughout many years, Mondain traversed the lands of Akalabeth, leaving a trail of evil and death in his wake. He constructed deep dungeons, so vast and unexplored that their lower depths remained a mystery. Within these dungeons, he unleashed even more malevolence. Thieves, skeletons, and snakes were sent to inhabit the upper levels, while daemons and balrons were tasked with guarding the depths. As a result, blood flowed freely in Akalabeth, and repugnant creatures began to roam closer to the surface. Mondain inflicted such sickness and pestilence upon Akalabeth that both humans and beasts lived in perpetual fear. Thus began the Dark Age of Akalabeth.";
            }
            else if (book.BookTitle == "The Lost Land")
            {
                book.BookText =
                    "Although I failed in my search for the Lost Continent, I bring the story often told by roaming minstrels, hoping it will aid you in your quest. Many years ago, a strong and sensitive people inhabited the continent of Ambrosia. These people had great powers over the forces of nature, and it is rumored that they even possessed the ability to change their physical form. From these stories, the legends of the magic shrines of Ambrosia emerged. As the years passed, the land prospered, and the mysterious power grew stronger. Suddenly, without warning, a tremendous and violent upheaval occurred, causing the earth to sink into the sea. The land was swallowed by a great whirlpool, which has since vanished. However, many claim to have discovered a moongate to the lost land of Ambrosia.";
            }
            else if (book.BookTitle == "The Balance Vol I of II")
            {
                book.BookText =
                    "I learned about the Ophidians from a sage in Britain. They reside on Serpent Island and follow a unique religion centered around three serpents that maintain balance in the universe. While many consider it folklore, I believe there is truth to this religion. These serpents are known as the Serpents of Balance, Order, and Chaos. When Exodus took the Serpent of Balance from Serpent Island to protect his castle, the balance started to falter. The Stranger managed to free the Serpent, allowing it to return, but the repercussions are still evident. Chaos and Order continue to battle each other in the void, with Order seemingly prevailing. Unfortunately, the power of Chaos was inadvertently released and possessed three unfortunate individuals. These individuals now wander in the dungeon of Bane as the Chaos Banes, each holding one of the blackrock serpents necessary to maintain balance. I attempted to defeat one of them but failed. A gypsy in Lodor informed me that to defeat all of the Chaos Banes, I would need three specific items. The Bane of Insanity can only be vanquished with the Orb of Logic. The Bane of Anarchy requires the Scales of Ethicality for defeat. Lastly, to eliminate the Bane of Wantonness, one must acquire the Lantern of Discipline. These items could be located anywhere in the realms, but the gypsy suggested searching the dungeons. It will undoubtedly be a challenging journey, but the preservation of balance is crucial for our survival. I was also cautioned that since these items are from the void, if I discover one, it could be lost if another person claims it from elsewhere. Therefore, I must locate these items swiftly and dispatch the Banes as soon as possible.";
            }
            else if (book.BookTitle == "The Balance Vol II of II")
            {
                book.BookText =
                    "The Blackrock Serpents are almost within my grasp. Each one represents the Serpents of Balance, Order, and Chaos. If I am ever to subdue the Serpents of Chaos and Order, I will need all three of these items to succeed. I found an ancient scroll in the library of Lord British that provided the much-needed information. Although the librarian categorized it as a legend, I believe it to be true. I need to locate the Wall of Lights in the Serpent Sanctum. If I approach it, I will appear in a room of both this world and the void. I must approach the statues of Order and Chaos and announce my purpose to restore the balance. That should grant one of the blackrock serpents the power of the Serpent of Balance. Then, by touching each of the statues, the serpent will manifest in this realm where I can subdue it. I must repeat this process with both serpents as they both need to be subdued. Once I capture the subdued souls of their blackrock forms, I can seek out the Great Earth Serpent and declare that I have maintained the balance. I am off to confront the Bane of Insanity now, as I have obtained the Orb of Logic. If I survive the upcoming battle, I will attempt to find the other two serpents.";
            }
            else if (book.BookTitle == "The Black Gate Demon")
            {
                book.BookText =
                    "For centuries, the greatest spellcasters have attempted to reach the Ethereal Plane. However, success remains elusive. Nevertheless, my latest research has uncovered a scroll hidden deep within the halls of Dungeon Doom. Despite being written in a strange drow language, I was able to decipher most of its contents. While many wizards summon aid from demons in other worlds, one particular mind flayer managed to summon a black gate demon, believed to originate from the Ethereal Plane. Striking a deal with this demon may not be an easy task. However, if it can be defeated in battle, a gateway to the desired destination may be opened. So far, I have summoned twelve demons, but none of them have been a black gate demon. Perhaps, if I could locate the hidden city of the mind flayers, I might discover the secrets to successfully summoning this spirit.";
            }
            else if (book.BookTitle == "The Blue Ore")
            {
                book.BookText =
                    "Caddellite is a fascinating blue metal with unique properties It has the ability to filter out harmful energy and can even cause physical attacks to harm the attacker. Additionally, it is more comfortable to wear compared to other plate metalors, providing the wearer with enhanced swiftness. This metal is not native to our world and can only be found in meteors that have fallen from the sky. I have discovered a rare deposit of this metal, which is rumored to have decimated the inhabitants of Ambrosia. Despite my skills as a master miner, I have been unsuccessful in extracting the ore that emits a bright glow.<BR><BR>Rumors circulate in the taverns about a legendary blacksmith named Zorn, who left these lands centuries ago to serve the Black Knight. It is said that Zorn possesses an adamantium pickaxe capable of chipping away at the glowing blue stone. Some even claim that he once served as a smith for the gods. In order to obtain this pickaxe, I have hired mercenaries to infiltrate the Black Knight's vault, where Zorn is believed to reside. With the pickaxe in my possession, I can then unearth some of this valuable ore and journey to the elven dragon forge.<BR><BR>To reach the forge, I must follow a map that leads to the valley where the elves reside in the Savaged Empire. If I can create a suit of armor from this unique metal for Lord British, it is possible that he may extend an invitation for me to join his court.";
            }
            else if (book.BookTitle == "Crystal Flasks")
            {
                book.BookText =
                    "I believe the creature was composed entirely of liquid. It approached me while I was exploring Dungeon Shame in search of an artifact. When I struck the beast, a thick, gooey substance sprayed out and coated the ground. It was certainly painful to walk on. I defeated the creature and decided to take some of the spilled liquid to the alchemist in Lodoria. However, the bottles I had were unable to hold the fluid. When I later recounted my tale to the alchemist, she informed me that I would need to somehow acquire a crystalline flask in order to collect the substance and bring it to her. She warned me to be cautious if I managed to obtain a flask filled with this peculiar liquid, as spilling it would have the same harmful effects as if it had been expelled by the monster. According to her knowledge, the process of creating crystalline flasks remains unknown, and they are exceedingly rare to come across. Since they were more prevalent centuries ago, I believe my best chance of finding one lies in exploring the dungeons of the realm, as they house relics from a bygone era.";
            }
            else if (book.BookTitle == "The Curse of the Island")
            {
                book.BookText =
                    "It all began with Batlin, a powerful druid with considerable magical abilities. He was born in Yew and grew up among the Druids. As a young adult, he embarked on a journey across the land and met Elizabeth and Abraham along the way. During this time, he visited Skara Brae and sought answers from Mangar regarding life and death. Upon discovering that there were none, Batlin's descent into darkness began.<BR><BR>Shortly after, the Guardian contacted him, Elizabeth, and Abraham. Together, they formed the Fellowship, driven by their hunger for power and the Guardian's desire to conquer the land of Sosaria. However, their wicked quest failed. It was during this time that Batlin overheard the Archmage Zekylis discussing a mysterious place called Serpent Island.<BR><BR>Under the orders of the Guardian, Batlin journeyed to Serpent Island. His mission was to capture the essences of powerful beings known as the Banes of Chaos, which the Guardian planned to unleash upon Sosaria. Motivated by his insatiable thirst for power, Batlin intended to seize control for himself using the Banes. However, something unexpected occurred, and the Banes were released from their magical prison. Three of Batlin's followers were possessed by the Banes, resulting in Batlin's demise. The Banes fled to different regions of Serpent Island.<BR><BR>This event had far-reaching consequences. The influence of the Banes cursed certain creatures, making them more formidable. Many adventurers have encountered these cursed monsters, often meeting their doom in battle. However, those who manage to defeat them sometimes return with unimaginable treasures. Scholars and sages remain uncertain about the duration and extent of the curse, as well as which creatures will be corrupted. Nonetheless, brave adventurers occasionally embark on quests to hunt down and destroy these cursed beasts. If you ever come face to face with them, may fortune favor you.";
            }
            else if (book.BookTitle == "The Dark Age")
            {
                book.BookText =
                    "'It is said that long ago, peace and tranquility covered the lands. Food and drink flowed freely, and man and beast lived in harmony. Gold and silver abounded, marking the golden age of Sosaria. However, Mondain, the second-born son of Wolfgang, a great king of old, desired his brother's inheritance. Using his great powers for evil, Mondain roamed the lands of Sosaria for many years, spreading death and evil wherever he went. He created extensive dungeons, the depths of which remained unexplored. Within these dungeons, he unleashed even more evil, sending thieves, skeletons, and snakes to dwell near the surface, while daemons and balrogs guarded the depths. Blood now flowed freely in Sosaria, and foul creatures roamed near the surface. Mondain cast sickness and pestilence upon Sosaria, instilling constant fear in both man and beast. This marked the dark age of Sosaria. However, a pure and just man emerged from the land to challenge the Dark Lord. British, the Champion of the White Light, engaged in a fierce battle with Mondain deep within the labyrinth of dungeons, ultimately driving him away from Sosaria forever. British of the White Light was then declared Lord British, the Protector of Sosaria. Sadly, the lands had suffered great damage. Thus, the Revival of Sosaria began.";
            }
            else if (book.BookTitle == "The Dark Core")
            {
                book.BookText =
                    "Some say it came from the future, a machine that seems more demon than metal. Exodus was a scourge upon the land of Sosaria, created by the hands of Mondain and Minax. When the stranger destroyed the mechanical being of Exodus, the demonic automaton that lay beneath was unleashed. It now uses its programming to travel from land to land, never staying in one place too long. It goes to tombs and dungeons, recording everything it sees from the monsters that lurk within. It learns to be a more effective opponent, as demonstrated by the instant slaying of my grizzly bear companion. It possesses a force field that can alternate between magic and melee protection. It also emits beams of energy that electrify the soul. Why would anyone attempt to face such a being? It is the dark core I seek. Within the chest of this metal demon, Exodus has a core that stores the multitude of information it learns. The information within the core is not what I seek, but destroying the core to unleash its power is. I have found the ancient gargoyle city of stone in the Savage Empire, but I have yet to find the Forge of Virtue within, where I must plunge the core. If what the gargoyles speak of is true, an item placed on the Shrine of Diligence will absorb the power of the core when it is destroyed. I must also speak the magical mantra 'Vas An Ort Ailem' and throw the core into the forge. The shrine is said to be next to the forge, but finding it means nothing if I cannot track down where Exodus will be next. The lands are so vast that finding him will prove to be quite difficult. Time is of the essence. Even if I manage to get the dark core, I have very limited time before Exodus rebuilds itself and the core slips from my grasp and returns to its chest. I wonder if anything can destroy this infernal machine.";
            }
            else if (book.BookTitle == "Death to Pirates")
            {
                book.BookText =
                    "Some believe that when a ship is swallowed by the sea, the souls of those on board are also claimed by it. I served on the Blue Wave for many years, transporting cargo from Lodoria City to distant ports such as Glacial Hills or Springvale. One cold day, we were tasked with delivering an unusual crate to a remote island in the southwestern part of Lodor. Unaware of any settlement there, the promise of good payment tempted us, so we set sail. However, upon arriving at the island, we were met with an overwhelming sense of pure evil. An elderly man aboard our ship shared tales of pirates who sought refuge on this cursed island, condemned to dwell among the restless spirits that haunt the woods. The mountains loomed ominously, the soil turned to mud, and the once thriving trees now stood blackened, as if consumed by fire. A pallid figure emerged from the dock, claiming possession of the crate. His hands were as white as bleached bone, and his voice sent shivers down our spines. Tossing us a bag of remaining gold, he bid us farewell and we hastened our departure. As we sailed away, we caught sight of a ship approaching from the east. Its ghostly white wood revealed shadowy figures shuffling upon the deck. The closer the ship came, the more our horror intensified. It was a vessel of the damned. They boarded our ship and mercilessly slaughtered nearly every man on board. They showed no interest in plunder, but instead sank the Blue Wave into the unforgiving depths. Only Vrendora, Selena, and I managed to survive, clinging to debris as we floated aimlessly at sea for days. Eventually, Captain Feldora of the Velvet Sky rescued us, offering salvation from our nightmarish ordeal. Upon our return to the Port of Dusk, Vrendora succumbed to madness and was killed by the local guards. Selena, driven by vengeance, acquired her own ship and crew, embarking on a relentless quest to find that cursed ship. As for me, I have chosen to spend my remaining years in Springvale, recounting tales of the treacherous sea and warning others that there are far greater dangers than krakens lurking beneath the waves.";
            }
            else if (book.BookTitle == "The Death Knights")
            {
                book.BookText =
                    "He was the first of the Death Knights, brought into service by the Devil Lord known as Brazuul. That was the day Kas the Kind was forever known as Kas the Bloody Handed. He spread terror throughout Sosaria and Lodoria, luring 14 others into his mad quest to rid the world of the order of law. A group of Paladins chased them from one end of the world to the other, defeating each one they found. Kas laid each disciple to rest at the bottom of various dungeons, hoping their tombs would be left undisturbed. Many believe these tales are used to scare children from leaving the safety of their villages, but I have found clues leading to the Tomb of Kas in Sosaria on an ancient map I found on Dracula's island in Lodoria. It is believed that the Death Knights often built their fortresses there, along with the land's necromancers. This map leads to the mountains, but I am curious about the back of the map. Written in blood are the words 'Mortem Mangone'. I am not sure what they mean, but soon I may obtain Kas' book and lantern and begin to learn the ways of the Death Knight. If my research is correct, a Death Knight may only have one book and lantern. Obtaining any other would cause the previous ones to vanish. I think this was how Kas kept his loyal subjects limited to the 14 he had chosen himself. Will I be worthy? Have my recent deeds been vile enough to draw his attention? Perhaps I should learn about knightship from a warrior guildmaster in Umbra or Ravendark, or Kas may not answer my call.";
            }
            else if (book.BookTitle == "The Darkness Within")
            {
                book.BookText =
                    "His name was Dracula, and he was once a baron in the land of Sosaria. Although he was a good man, rage and hatred had driven him into the arms of darkness. Dracula had once ruled the city of Montor, where they had enjoyed years of peace thanks to the walls he had built around the city, protecting it from lurking dangers. However, one fateful night, an assassin scaled the walls and murdered Dracula's wife while he was visiting Britain. When Dracula returned and learned of his wife's death, he locked himself in his bedroom for weeks, consumed by grief.<BR><BR>Eventually, witnesses spotted Dracula sneaking out of the city one night. A vigilant guard followed the baron, but lost sight of him when he entered Dungeon Doom. Months later, Dracula was sighted in the village of Grey, where he was found biting the neck of a local seamstress, seemingly drinking her blood. The guards arrived too late to apprehend him for his crimes. Similar sightings continued in various settlements as the nights passed. Some believed that Dracula was searching for the assassin who had murdered his wife, while others believed that he had been cursed by vampires and now sought blood from the living.<BR><BR>In response to the growing threat, Lord British assembled a group of his best guards to hunt down Dracula and put an end to his bloodthirst. They pursued him into the night, traveling south of Britain. Dracula sought sanctuary in the Tower of the Lich and was never seen again.";
            }
            else if (book.BookTitle == "The Destruction of Exodus")
            {
                book.BookText =
                    "After the defeat of Mondain and Minax, the people of Sosaria believed that the worst was behind them. Little did they know, a new threat was looming. Exodus, the offspring and creation of Mondain and Minax, was neither fully demon nor machine, but it sought revenge for the destruction of its creators. It embarked on a campaign of terror, summoning an island from the depths of the sea and amassing armies of evil to tear Sosaria apart with its powers.<BR><BR>For the third time, the Stranger returned to save the world from this new evil. This time, however, the hero had three companions to aid in their quest for survival against the hordes of Exodus. Together, they traversed the land, gathering crucial information, until they discovered the Four Cards hidden in the lost lands of Ambrosia. With the guidance of the Time Lord, the group learned how to defeat Exodus.<BR><BR>Equipped with exotic weapons and exotic armor, they were prepared to confront their foe. Their journey led them to the Isle of Fire, where they overcame the formidable Great Earth Serpent and battled their way through Exodus' minions. Finally, they reached the heart of Exodus, which was revealed to be a computer. By inserting the Four Cards, they triggered a powerful explosion that obliterated Exodus.<BR><BR>Now, the castle of Exodus lies in ruins. Those brave enough to explore it claim that demons and undead creatures now haunt the halls beneath the decaying fortress. Despite the dangers, some adventurers still venture inside, driven by the belief that untold riches await them within.";
            }
            else if (book.BookTitle == "The Knight Who Fell")
            {
                book.BookText =
                    "Knighthood is a noble quest one undertakes to bring light to a land shrouded in darkness. Unknown evils roam freely, and it is the duty of these knights to defend the peace that we enjoy in this world. A knight's power is often bestowed upon them due to their generosity. When they vanquish evil, they often come across great wealth that is too abundant to keep for themselves. Instead, they would take a portion of this fortune and leave it at shrines, altars, or statues of goddesses. The priests would then collect this gold and distribute it among the poor and less fortunate.<BR><BR>However, there was one knight who did not believe in this mantra. He coveted all the wealth he could accumulate, even going so far as to kill others for their possessions. His soul became so corrupted that his knightly powers gradually turned into something foul and evil. He adorned himself in black armor and terrorized villages and cities to seize whatever they had. In response, knights were dispatched to eliminate this traitor of the faith, but many lost their lives in the process.<BR><BR>Lord British assembled an army led by his greatest knight to track down this black knight. Their pursuit led them all the way to the enigmatic island of Umber Veil. Upon their arrival, they discovered the village of Renika engulfed in flames. The survivors had sought refuge in the nearby church, while others sought shelter in the mountain castle. They informed the army that the black knight had fled to Serpent Island. Satisfied that their lands would no longer be tormented, the army abandoned the chase. They aided Renika in rebuilding their lives before returning home.<BR><BR>That was many, many years ago. Since then, no one has sighted the black knight in Sosaria. However, rumors have persisted in the taverns over the years. Some claim to have sailed to Serpent Island and found an island where the black knight had constructed his fortress. A few even assert that he possesses the largest underground vault ever witnessed. Allegedly, everything the black knight has ever plundered resides within its depths, guarded by those assigned to protect it. Personally, I believe these tales to be exaggerated, as those who recount them have no tangible evidence of the vault's existence. We should simply be grateful that he is gone, forever banished from our realm.";
            }
            else if (book.BookTitle == "The Fall of Mondain")
            {
                book.BookText =
                    "Sosaria needed a stalwart hero, one who would brave horrific perils. A plague had befallen the realm, a scourge was upon the land! The villages lay wasted, ruinous mounds of ashes where once strong-hearted and clear-minded peasants trod, where fields of grain and fruit once lay, where livestock and birds grew fat from Sosaria's abundance. All manner of wicked and vile creatures preyed upon the people and ravaged the land. It was the work of an evil being, so feared that even the ground trembled at the mention of his name. Mondain the had executed his malice with precision. The nobles quarreled among themselves, each retreating to their keeps in hopes of witnessing the downfall of their rivals. Truly, the Evil One had added insult to injury by releasing upon the realm a horde of bloodthirsty and wicked creatures, causing the defenseless people to fall like grain before the reaper's scythe. These denizens of the underworld held dominion over all that could be seen, except for the fortresses of the ambitious nobles. Nowhere in the once peaceful country could a traveler find safe passage or lodging, except within the keeps of the self-proclaimed kings...and they demanded hard labor in exchange for their hospitality. Only the young Lord British remained steadfast in his vision of a peaceful and united Sosaria. In his castle and town, those with pure hearts found an ally and provisions for their fight to save the realm. He aided in ridding the land of the scourge that had befallen it. Without his help, Sosaria would surely have perished before the onslaught of the malevolent necromancer.";
            }
            else if (book.BookTitle == "Forging the Fire")
            {
                book.BookText =
                    "The bending of steel is something that has been done for centuries, but before the rise of man, it was believed to have been forged with the power of volcanic fire. Purslos was said to be a gargoyle and an excellent craftsman who could not only shape steel to his will but also enhance it with the power of the island's magma. Legend tells of Purslos making a sword for his master, Tuxluka. The sword was said to be made of solid fire and burned as well as it cut. Tuxluka used this dagger to win the Ophidian war and bring peace to the Serpent Island. One day, we may learn the secrets to this method of blacksmithing. Whether it is fable or fact, the stories are too abundant to ignore.";
            }
            else if (book.BookTitle == "Forgotten Dungeons")
            {
                book.BookText =
                    "I have spent my life searching for gold, gems, and jewels. Although I have found much, blood was always spilled along the way. Comrades fell, enemies vanquished. Now, I live the life of kings. I didn't need to build great castles or kingdoms in the land. I didn't need to rule a vast expanse to flaunt my wealth. I simply took what I earned and now live here in its well-lit hallways.<BR><BR>I have explored many dungeons. Some big, some small. The small ones were quickly searched to be sure. One day, an old wizard accompanied me as we searched a small dungeon for anything of value. It had none, but the creatures within were many and they were slain without effort. Tired from the day's travel, we decided to make camp for the night within the darkened rooms of this place. That is when the wizard mentioned that this would make a good home, if one had the gold to hire hands to clean it and fortify the walls. The wizard had no clue about the amount of gold I had in the many banks of the land, but I thought his idea was well thought out. We returned to Devil Guard the next day and parted ways. I traveled to Moon and hired many men to make this place my home. I have cleared many dungeons in my day, but this is my favorite.";
            }
            else if (book.BookTitle == "The Cruel Game")
            {
                book.BookText =
                    "The matters of gods can be dull to those who share the thrones of such power, so dull that a few of them decided to wager on the fall of worlds. The gods of flame, sickness, power, and ice made a deal that would determine which world would fall from the gifts granted to them. They decided to each build a mystical forge in different lands and simply sat by and watched the events unfold.<BR><BR>The god of flame built his forge on Serpent Island. The forge was so hot that it tore a hole in the land. The god of sickness built her forge in the most beautiful forest in the Savaged Empire, which was believed to be so poisonous that it turned the area around it into a sickly swamp. The god of power built his forge in the world of Ambrosia, where it was eventually worshipped by the inhabitants and shrines were built in his honor. The god of ice built his forge in the tropical region of Lodoria, which was believed to be so cold that it turned the entire jungle into a tundra winterland.<BR><BR>The gods each picked a champion from these lands and spoke to them in their dreams. They were each given the location of the forges, along with instructions on how to use them. They simply had to place weapons and armor on the stone tiles around the forge. Once they spoke their own names, the items would be infused with the power of the magical forges. They were to spread these arms and armor to their comrades for either protection or war. The gods then left these champions to their work. The first land to fall would grant that god the title of winner of the contest.<BR><BR>Years went on and lives were lost to the weapons forged. Some of these battles could be read in ancient texts. Some believe that Serpent Island fell, as tales about the surface are void of civilization. Others think Ambrosia was the one that made the god of sickness the victor of the contest, as it was believed to be totally destroyed by the power of the energy weapons forged. The legend has been told from generation to generation. No one has found these forges nor learned the names of the champions chosen for this cruel game. It will forever remain a mystery.";
            }
            else if (book.BookTitle == "The Ice Queen")
            {
                book.BookText =
                    "The tale I write is not a fable, and I would advise against treating it as such. Her name comes in many forms, and I will not speculate on what they might be. She was born into the Blood Cult, which inhabits the largest of the islands in the Isles of Dread. Her flesh was as white as snow, and her piercing eyes seemed to penetrate the very soul of those who gazed upon her. Those who saw her were reminded of stories of a girl destined for greatness. Nobody knew exactly what that entailed, but the cult consulted countless tomes in search of answers. Years were spent reading through scrolls, parchments, tablets, and books. When she was only 14, they finally discovered a scroll that prophesied her arrival. It stated that she would bring about the destruction of the cult and banish the Blood Goddess back to the realm of demons. It was decided that she must be killed before any of this could come to pass. They dragged her to the altar, bound her to it, and summoned a demon to claim her soul for the underworld. As the demon reached out to grab her, she screamed. Her scream was so loud that it sent the jungle animals fleeing in terror. The scream froze the demon in a block of ice, which then shattered into pieces around her. The bonds that held her were also frozen, crumbling away when she pulled on them. Standing tall on the altar, she screamed at the followers who had come to witness her demise. Many managed to escape, but many others suffered the same fate as the demon just moments before. She fled to a tropical island to the north with nothing but a small raft. There, she found a cave where she sought refuge for days, surviving on roots and berries she foraged. As she grew older, so did her power. The island slowly transformed into a frozen tundra, and the cave she called home developed ice walls and smooth hallways. Using her magic, she created golems made of pure ice to patrol her corridors. Now, she sits in her throne room, plotting the downfall of the cult she is destined to destroy.";
            }
            else if (book.BookTitle == "Luck of the Rogue")
            {
                book.BookText =
                    "Luck is a cosmic enigma that wizards and sages struggle to comprehend it. It transcends both magic and divine intervention, embodying the essence of chaos. Luck can manifest as either good or bad fortune. Some individuals are bestowed with a surplus of luck, often attributed to finding magical items with rabbit's feet that have been rubbed for good fortune. Personally, I seem to possess an abundance of luck, but what exactly does it mean to be lucky in this realm? Unlike my fellow adventurers, who fail to stumble upon the wondrous objects I come across during our journeys of slaying formidable beasts and discovering hidden treasure. This undoubtedly frustrates them. Additionally, I possess the uncanny ability to avoid traps that my comrades fall victim to. I won't deny that this advantageous separation of our spoils during misfortunes is a significant benefit. However, I am unable to ascertain the factors that determine why some are blessed with luck while others are not. Nonetheless, I have noticed that my own prosperity is enhanced as a result of my favorable fortune.";
            }
            else if (book.BookTitle == "A Tattered Journal")
            {
                book.BookText =
                    "- Autumn the 3rd - It was decades ago, but the burned memory makes it feel like yesterday. The entire city was burned to the ground by that mighty dragon, taking my family with it. I hunted it for years, never finding a trace. Five years ago, I overheard a drunken guard babbling in the tavern about a wizard who had captured the beast and sealed it within a crystal orb. They couldn't defeat the creature, so they chose this course of action to ensure Sosaria's safety. While I would commend the mage, vengeance was stolen from me.<BR><BR>- Autumn the 20th - I discovered the identity of the wizard and went to see him in Montor. However, I learned that he had met his end while searching for ancient texts in the depths of Dungeon Clues. Before his brother could arrive in Montor to claim the wizard's belongings, I sneaked into his home and stole the orb. I could see the dragon imprisoned within it, but I have much work to do if I am to break it open.<BR><BR>- Autumn the 42nd - I have acquired the few diamonds and mandrake roots that I need. Strangely, staring at the orb for long enough revealed to me the various items required to break the spell. I am now off to buy a fur cloak, as I need to find the horn of the frozen hells.<BR><BR>- Autumn the 70th - I barely survived, but I finally obtained the eye of plagues. I am currently resting at the inn in Springvale, planning to stay here for a few days to allow my wounds to heal. I am uncertain how I know this, but I am aware that I must take this crystal prison to the Dungeon Destard. I believe this is where the wizard trapped the dragon and thus, it must be released in the same location.<BR><BR>- Winter the 5th - I have failed. I unleashed the beast from its magical cage, but I was unable to slay it. The battle was arduous, and at one point, I had to run and hide. When I returned, the dragon was gone. I fear that I may never get my chance for revenge, but I will continue my search. I will search until either the dragon or I am no longer alive.";
            }
            else if (book.BookTitle == "The Curse of Mangar")
            {
                book.BookText =
                    "Long ago, when magic still prevailed, the evil wizard Mangar the Dark threatened a small but harmonious country town called Skara Brae. Evil creatures roamed beneath Skara Brae and joined his shadow domain. Mangar surrounded Skara Brae in a spell of Voided Death, totally isolating Skara Brae from any possible help. Then, one night, the town militiamen all disappeared. Only those who were within the town knew of this fate. Others in Sosaria saw nothing but an island full of ruins that was once Skara Brae. This was Mangar's plan all along. He wanted everyone to think Skara Brae was destroyed, so no one would come to oppose him. The future of Skara Brae hangs in the balance. No one is left to resist. Only a handful of unproven young warriors, junior wizards, a couple of bards barely old enough to drink, and some out-of-work thieves. I was there. I was the leader of a ragtag group of freedom fighters. We tried to defeat Mangar, but he was too powerful. Just when we thought we had him, he vanished in a puff of smoke, proclaiming his return. This was enough to weaken the magic of the Voided Death. A portal opened, but only for a short time. We entered the portal and returned to Sosaria, appearing on top of a tower that Mangar had built there. Be warned, this tower allows Mangar to travel between Skara Brae and Sosaria. If one is not careful, they might touch the crystal ball that would trap them in Skara Brae.";
            }
            else if (book.BookTitle == "The Times of Minax")
            {
                book.BookText =
                    "The Stranger did not have the task of saving Sosaria, but instead Earth from the Enchantress Minax. As the lover and apprentice of Mondain, she was quite angry over his death at the Stranger's hand and swore revenge. Manipulating the timeline to achieve this, she allowed Earth to perish in an atomic holocaust in 2111 - all life on Earth perished in the aftermath. The Stranger, having narrowly escaped the changes in the timeline, had to unravel the mystery of the Time Doors, which enabled time travel, in order to reach Minax and prevent these terrible events from ever occurring. Gathering the only weapon capable of killing Minax, the Quicksword Enilno, and wearing the protective Force Field Ring, the Stranger traveled back to the Time of Legends and confronted Minax in her castle to end her life. With her death, the timeline returned to normal, and no one remembered the terrible events that took place in the altered timeline... except for the Stranger and myself.";
            }
            else if (book.BookTitle == "Rangers of Lodoria")
            {
                book.BookText =
                    "Rangers from the land of men tracked dark elves back to their homeland, believing that the world of Lodoria was tainted with these evil fey. The rangers were determined to rid the world of them and embarked on a mission that had lasted many moons. During their hunt, they unexpectedly encountered the surface elves, who presented themselves as a more benign presence that the rangers were familiar with. This marked the beginning of an alliance between the rangers and the elves of Lodoria, with a shared goal of eradicating evil from the world. The rangers established their fort in the mountains near the City of Lodoria, which could only be reached through a cave as dark as the dark elves' skin. While this cave was treacherous and easy to become lost in, those from the ranger outpost knew the way down the mountain and back up again. Additionally, the druids used their magic to transport the rangers to the top of the mountain. However, legends spoke of a sealed and abandoned great hall called Undermountain, said to be located just below the ranger outpost. If one could find their way to Undermountain, they might discover themselves directly beneath the ranger outpost. With this in mind, the narrator planned to begin their search in the cave situated to the north of their camp—the one infested with lizards and snakes.";
            }
            else if (book.BookTitle == "Gem of Immortality")
            {
                book.BookText =
                    "When the Gem of Immortality was shattered by the Stranger, three shards went missing, each opposing one of the principles of truth, love, and courage. The shards ended up in Ambrosia and were lost until the ship 'The Ararat,' under Captain Johne, was sucked into a whirlpool. Johne found the shards, which drove him to temporary madness. He slew his three companions, and their blood fell on the shards, giving birth to the Shadowlords - Astaroth, the Shadowlord of Hatred; Faulinei, the Shadowlord of Falsehood; and Nosfentor, the Shadowlord of Cowardice. Wasting no time, they spread their evil throughout the lands and took over the elven castle of Stonegate as their lair. To rebuild the gem, I need to retrieve these shards from the Shadowlords. However, I have tried to slay them to no avail, as they possess the essence of immortality. In order to defeat them, I must find the Candle of Love to slay Astaroth, the Book of Truth to destroy Faulinei, and the Bell of Courage to defeat Nosfentor. The challenge lies in the fact that these items can be located in any dungeon in the realms. I have no choice but to embark on a quest to find them. However, I must be cautious, as these items are not of this world and can be claimed by others. If someone else obtains an item, it will vanish from my grasp. Once I acquire one, I must act swiftly and fulfill its purpose. As long as one member of my group possesses the item, we can defeat the respective Shadowlord and retrieve the shard. Once I have obtained all three shards, I will need to locate King Wolfgang's tomb and speak the same words he used to construct the gem, 'Cryst An Immortalis.' Rumor has it that he was buried in Britain, but his exact resting place remains unknown. Time is of the essence, as the first person to construct the gem will cause all shards to vanish. Losing the gem due to delays is simply not an option, given its immense importance.";
            }
            else if (book.BookTitle == "The Gods of Men")
            {
                book.BookText =
                    "Superstition and magic govern the lands, not kings. No matter where I travel, men follow the gods they know and the ones they grew up with tales of deeds and might. Although I did not believe in the concept of deities, the story I am about to tell is true. I heard rumors of magical scrolls contained within the halls of Doom. These tales were too hard to resist as many men have told the same tale. The chance they were telling the same lie is slim, so I made my way south to search for myself. I thought the chambers would be void of life, but I was later surrounded by the most hellish of creatures. My mystical words were not quick enough as a skeleton put its scimitar into my back. I found myself standing there, looking upon my impaled body. The creatures did not attack me, nor my lifeless body. I tried to escape through the door, but I could not touch it. My hands passed through it. I walked through the thick wood and made my way to the surface of the land. Everything looked gray to me. All manner of forest creatures did not react to my presence. I found myself drawn to something. I wasn't sure what it was, but I followed this feeling I had. It led me to an old shrine in the woods. It was a small stone pillar. In front of it were various items worshipers left for the god whose name was carved into the rock. I then heard a voice from above. I could not see who was speaking, but the voice echoed loudly in my ears. Whoever it was, they offered to bring me back to the land of the living. It was then that I realized I was dead. They asked for only a small sacrifice of my wealth to show that there is more to life than mere gold. I gladly accepted as I was not ready to leave this world. I found myself, whole, in front of the shrine. Colors returned to my vision, and I could feel the grass under my bare feet. I returned to Montor and grabbed some supplies from the bank. I went back to Doom and used an invisibility potion to creep around. I found where I fell in battle, my belongings still strewn about. I grabbed what I could carry and quickly left before the potion's effect was gone. I have found many of these shrines since. Some look like ankhs, while others are statues of beautiful women. I once found an altar in a cave as well. I sometimes see knights drop the gold they found at these shrines. No one takes it, as they would surely be cursed if they do. I now believe in the gods, as they spoke to me.";
            }
            else if (book.BookTitle == "Castles Above")
            {
                book.BookText =
                    "I thought it was a lone storm cloud in the distance. When no rain or thunder came, my attention was drawn further. It was a castle, high above the ground. I kept watching to see if it would fall, but it never wavered. I decided to approach it and try to get below it. That is when I found the blue magical lantern. Within its glow was a rope that ascended to the heavens. I used all of my strength and climbed the rope, finally reaching the castle above. I knocked on its thick wooden door, and an echo bellowed from within. On the third knock, the door pushed open just a bit. It was hard to open as it was old, warped, and probably hadn't been opened for many years. The interior of the castle was dusty and void of life. The halls and rooms were empty of any treasure. If I had the gold to furnish this place, I would make it my home. I wonder what manner of wizard abandoned such a home? Did they live here for years, only to return to the surface and live among others?";
            }
            else if (book.BookTitle == "Staff of Five Parts")
            {
                book.BookText =
                    "Legends suggest that the Staff of Ultimate Power possessed immense strength. It is believed that Merlin was killed by someone wielding it. The gods divided the staff into five fragments and entrusted them to guardians for safekeeping. If someone proves themselves worthy to the Time Lord, they may eventually gather all the pieces as overseer of the guardians. The fragments can only be combined near the molten core of the Moon. To accomplish this, one must sit on the stone throne and utter the specific words that will activate the power. The core is an inhospitable place where survival is impossible, but by following the rune marked path, one can avoid the scorching heat. The staff binds itself to the individual who assembles it, preventing others from wielding it. If anyone else attempts to hold it, the magic will be drained, rendering it a useless object. Only one piece of the staff can exist in this realm at a time. Therefore, when a guardian is killed, the slayer acquires the fragment they were protecting, causing all other identical fragments to vanish. The staff manifests itself as either a staff of wizardry, a staff of necromancy, or a staff of elementalism. The type of staff depends on which magic is more dominant in the person sitting on the throne and reciting the words. As of now, I have yet to discover the correct words to utter, and recent events have revealed that the fifth guardian has escaped from the Time Lord's domain. Xurtzar, a being of demonic energy, managed to flee by creating a moongate to the Serpent Island. Thus, only four guardians remain under the Time Lord's supervision. Xurtzar's whereabouts are unknown. I now ponder where Xurtzar might have fled and how I can find the words to speak. To defeat Mangar the Dark, I must become the most powerful wizard in Sosaria.";
            }
            else if (book.BookTitle == "The Story of Exodus")
            {
                book.BookText =
                    "No one in Sosaria, including the Stranger, could have anticipated that by killing Mondain and Minax, they would leave their only child, Exodus, as an orphan. Exodus, a unique being who was neither human nor machine, emerged from the depths of the Great Ocean with a vengeful agenda to destroy Sosaria. The power unleashed by Exodus was so immense that the Stranger, now known as the hero, sought the help of a mysterious entity called the Time Lord to stop him. The Stranger confronted Exodus in a manner similar to how they dealt with his parents. Some argue that Exodus had the potential to bring great good to the land if persuaded, but I formally disagree with those who believe the Stranger should have handled the situation differently.";
            }
            else if (book.BookTitle == "The Story of Minax")
            {
                book.BookText =
                    "The triumph of the Stranger did not last long, as slaying Mondain brought the wrath of Minax down upon the land. Minax was Mondain's young lover and a sorceress with even greater magical powers. She had the ability to command legions of foul creatures and, in her quest for vengeance over her lover's death, she inflicted much misery upon the people of Earth. Once again, the hero who would later be known as the Stranger took up the fight, as Earth was his home world. The Stranger defeated Minax's minions and ultimately destroyed her as well. While there have been speculations about the Stranger's motivations, there is insufficient evidence to suggest that the Stranger was driven to violence out of jealousy over Mondain's relationship with Minax. Therefore, such theories are hereby dismissed and should not be considered.";
            }
            else if (book.BookTitle == "The Story of Mondain")
            {
                book.BookText =
                    "The beginning of the First Age of Darkness was marked by the arrival of a sorcerer named Mondain. Mondain's father had withheld the secret of immortality from him, which caused constant disputes that eventually resulted in the father's death. Overwhelmed with grief and likely driven by the fear of persecution, Mondain unleashed his dark powers upon the kingdoms of Sosaria. In a desperate plea, Lord British called upon a champion to defend the realm. The hero who answered this call would later be known as the Stranger. It was through the Stranger's actions that Mondain's cursed gem of power was shattered, leading to Mondain's own tragic demise.";
            }
            else if (book.BookTitle == "The Bard's Tale")
            {
                book.BookText =
                    "The song I sing will tell the tale of a cold and wintry day, of castle walls and torchlit halls, and the price that men had to pay. When evil fled and brave men bled, the Dark One came to stay. For men of old, for blood and gold, they will rescue Skara Brae.<BR><BR>In Skara Brae, where heroes stray, a tale unfolds of yore.<BR>Of Mangar's might, in shadow's light, his evil we deplore.<BR>He cast a spell, a voiding hell, to seal the town's front door.<BR>With militiamen vanished then, our hopes were almost o'er.<BR><BR>But ragtag teams with youthful dreams arose to face the foe,<BR>With sword and spell, we fought like hell, our courage there to show.<BR>In Skara's name, we staked our claim, and battled toe to toe.<BR>Yet Mangar fled, he wasn't dead, his power still to grow.<BR><BR>A portal shone, not long to own, a path to Sosaria.<BR>We took our chance, a fleeting glance, escaping his dark aria.<BR>On tower's peak, of which we speak, we found a new dilemma,<BR>A crystal ball, that could enthrall and send us back to terror.<BR><BR>So heed my song, for it won't be long, till Mangar comes again.<BR>In Skara Brae, we'll find a way, to finally break his reign.<BR>With courage bold, both young and old, we'll free this land from bane.<BR>And when we're through, this much is true, Skara Brae will live again!<BR><BR>";
            }
            else if (book.BookTitle == "Death Dealing")
            {
                book.BookText =
                    "I pity those who fear death. There can be great power if you face the Reaper's stare, but most cannot perform such a feat. Many consider this course nothing but evil, wretched practices. Every creature has an essence that can be harnessed after death. We have learned how to gain power from it. We have also learned spells that require minor things like grave dust or pig iron. When people do not understand such things, they fear them. That is what drove most of us from the cities and villages. Nevertheless, it is easier to practice such magic in the absence of light. The elders foresaw this centuries ago and built their city of Umbra within the volcanic mountains of Sosaria. If one can find it, they can practice these arts without scrutiny from others. It would shock one to behold it for the first time, as those who have lived there their whole lives have flesh as pale as the bones of the dead. Some of us have even constructed our strongholds within the mountain halls of blackened rock. It is a glorious time to be dead, as long as it's not me.";
            }
            else if (book.BookTitle == "The Orb of the Abyss")
            {
                book.BookText =
                    "The Underworld is an unusual place for our order. Although I have heard tales of necromancy and holy magic working in the dark depths of that abyss, I find my spells failing too often for my travels to be relatively safe there. Even my scrolls and wands seem to fail too often. Those that dwell there seem to have no obstacles in unleashing spells. If I am to pursue the titans below, I will need to be at my full potential. Tales tell of the Codex of Ultimate Wisdom, which promises that anyone who holds the book will have no such difficulty with magic. That, however, is too far beyond my reach as it is said to be somewhere in the Ethereal Void. My next destination then is the dungeon of Hythloth where my research has shown that the Orb of the Abyss lies in wait. Wizards that possess this orb are able to cast their spells when in the Underworld. Legends even tell of wizards from centuries ago needing this orb when they used the ancient forgotten spells in these depths. The orb, however, is guarded by a great evil. Ancient scrolls tell of a hidden portal that leads to the domain of Satan himself. I have magic that will aid me in locating the room where the portal is hidden. The portal does not allow just anyone to pass, but only those who know the true name of the devil that lurks on the other side. If the sage in "
                    + RandomThings.GetRandomCity()
                    + " speaks true, then 'Lucifer' is the devil's name, and I will need to shout it when I find the hellish gate. First, I must go to "
                    + RandomThings.GetRandomCity()
                    + ", where I am hoping to find allies for the coming battle.";
            }
            else if (book.BookTitle == "The Underworld Gate")
            {
                book.BookText =
                    "There are legends of a place darker than the veil that covers the sky at night, a world that never sees the sun and where the vilest creatures dwell. The likes of men would never want to witness such things, but the brave and adventurous seek this realm for the rumors of riches that lie within. The Underworld is, in fact, real. Many years ago, monsters poured out from the cave that leads to this abyss and filled the lands with terror and destruction. Lord British and Baron Almric joined forces and led a campaign across the entire world, pushing most of them back into the Underworld. Once they were done, Almric used a long-lost spell to seal the entrance, locking them below the land of Sosaria forever. Now, a large stone blocks the way, covered in magical runes that only Baron Almric can open. But the Baron was slain by the Slasher of Veils in a futile attempt to rescue his daughter. Although he managed to reach the land before falling to his doom, it is undetermined where he was laid to rest. Some believe that he is buried near "
                    + RandomThings.GetRandomCity()
                    + ", but there are rumors that he was taken back to his wife's home of Skara Brae. If the death from the Slayer did not resurrect his corpse to wander the land, one must only find his tomb and retrieve his head. Presenting it at the great runic gate may break the spell and open the seal to the Underworld. One must beware the Underworld, however, as ancient texts state that wizardry is hindered when traveling the abyss.";
            }
            else if (book.BookTitle == "The Elemental Titans")
            {
                book.BookText =
                    "The early days of the Obsidian Fortress were dark times. The blood from the wars flowed freely as the Drow fought Zealan in the grand struggle of religious cleansing. Battles were planned and executed, and lives were lost all in the name of archaic beliefs. All the while, the Drow toiled daily to construct the Obsidian Fortress, as commanded by the benevolent being called the Guardian. The fear of the Destroyer was strong.<BR><BR>Years of sweat ultimately resulted in the completion of the Fortress. There, the Drow wizards met to focus their energies on worshiping the Elementals. Tremendous magical forces were used to collect a strange black mineral and shape it into a large, dark obelisk. From inside the Fortress, the followers channeled their thoughts through the obelisk to the four elements, giving them even greater power. Soon, they had amassed enough energy to become the great Titans of Earth, Water, Air, and Fire.<BR><BR>The war continued, but now the Drow had considerable assistance. Lithos moved the lands to trap the Zealans, while Hydros removed her waters from their reach. Pyros' fires raged and grew, fueled by the winds of Stratos. It was only a matter of attrition before the Zealans and their petty beliefs fell. Then came the Guardian's final words of warning: 'Take your people and depart from the Fortress. The Destroyer has come.' As the Drow left the Fortress, the Guardian attempted to destroy it. The Drow pleaded for the aid of the Titans. They were not disappointed. The four Titans appeared to challenge the Guardian. The land was all but destroyed as rock, rain, wind, and fire hailed down from above. The battle was long and fierce. Finally, however, the Titans returned victorious. The land, though scarred from the terrible fight, was still theirs. The Guardian had fled the Underworld, forever.<BR><BR>The great battle broke five pieces off the obelisk. The first fragment, called the Heart of Earth, was linked to Lithos, the Titan of Earth. Hydros, the Titan of Water, was linked to the Tear of the Seas. The third fragment, the Breath of Air, was linked to Stratos, the Titan of Air. A fourth fragment was linked to Pyros, the Titan of Fire. There is considerable speculation about a fifth fragment. Apparently, the tip of the great obelisk was seen hurling through the air almost entirely intact. However, no one ever saw the item land, so its location remains a mystery. Were all of the fragments to be gathered together and taken to the Obsidian Fortress, it might be possible to create a separate obelisk. Of course, it would still be necessary to fabricate a magical field of some sort to channel the energy from whatever source first gave the obelisk power. Some believe a great Zealan warrior, Khumash-Gor, found the Obelisk Tip. This is probably just a legend as Khumash-Gor died centuries ago during the great Underworld war.<BR><BR>Collecting the pieces of blackrock is something mere men will never achieve. Without the Obelisk Tip, a mortal man cannot even move the other pieces of blackrock. If one were to find this lost relic, they would possess the means to one day become a powerful Titan of Ether.";
            }
            else if (book.BookTitle == "The Dragon's Egg")
            {
                book.BookText =
                    "Dwarves are proud people, and we have made many mountains in Lodor our home. It saddens the heart to see my people falling to this blight that has spread from north to south. We are dying, and the oldest and wisest among us cannot determine the cause. It feels like a disease, but herbalists are unable to cure it. Those with magic tried, but died in the attempt. We are cursed, and soon we will be no more.<BR><BR>Many of our homes are becoming inhabited by demons or the dead. Those who made the armor and weapons of our clans are no longer here. Only a few of us remain, huddling together in the hopes of avoiding the fate of others. I was a dragon master, which was a special skill before the likes of elves became civilized. Before the elves were able to tame such beasts, we dragon masters would find the eggs of great dragons or wyrms. We would collect what was needed to ensure they could hatch. Being the first ones sighted by the beasts, the dragon masters would quickly follow our lead and treat us as if we were their mothers. They would fight with us against the legions of the dead and the foul dark elves that come to our domains.<BR><BR>I remember learning the mastery of dragons at a young age. My father would slay a mighty dragon and bring the egg home to my mother. In order to see if a dragon was alive within it, we had to take the egg to the lava pool in the Dwelling of Rhundar. Rhundar was home to the most notable crafters, and the light from the pool was so bright that we could see through the mighty shell of the eggs. Now Rhundar lies in ruins as the dead have destroyed all that lived there. Now we just call it Deceit, so as not to remind ourselves of the greatness it once was.<BR><BR>Once we saw a living dragon within, we had to brew the potions of the four elements and apply the liquids to the shell. Then it would soften so the baby dragon could break free. The combination of the potions caused the young ones to develop quickly upon birth, which made them favorable companions immediately. I had many dragon pets, but we could not keep them after the plague started. We set them free, where most of them sought shelter in Destard. I will miss those days, and it saddens me that I will not have children to teach this to.";
            }
            else if (book.BookTitle == "Magic in the Moon")
            {
                book.BookText =
                    "It started as an orb in the sky, and we named it Luna. We eagerly awaited the sunset so we could gaze upon it. The moon, known as Luna, appeared at specific times, and we diligently recorded its different phases. This helped us discover the various effects the moon has on Sosaria. The necromancers discovered that during the moon's fullest phase, the lycanthropes would be the most dominant and powerful. The sorcerers learned that the magical gates scattered throughout the land would lead to different places depending on these phases. This is when we began calling them moongates. The moon held such immense magical power that the wisest among us gathered daily to craft a spell capable of reaching this celestial object.<BR><BR>After nearly 20 years of research, we finally found a way to harness the power of a single moongate and store it within a small, mystical orb made of meteoric glass. This drained the magic from the moongates, allowing us to control our destination when stepping through one, as long as we knew where to go.<BR><BR>We placed this captured magic orb on the pedestal of the Shrine of Wizardry and took our first steps towards the moon. The planet was small, gray, and covered in craters, but offered ample space to build castles and villages. Thus, we constructed our city of Dawn.<BR><BR>With the help of conjuration, Luna was built in a mere ten revolutions around Sosaria. It provided a safe place for our magical research, eliminating the concerns of accidental unleashing of wizardry upon Sosaria.<BR><BR>We attempted to bring visitors to the moon, but the sheer power of the lunar surface rejected those who lacked sufficient energy. Therefore, the moon became the home for wizards, witches, warlocks, and sorcerers. The guildmasters discovered an asteroid nearby and built our main guild hall there. This arrangement proved beneficial for recruiting apprentices, as those weak in magic could travel to the asteroid to begin their studies. One day, they would gain enough power to join us on the moon and perhaps establish their own homes.";
            }
            else if (book.BookTitle == "The Maze of Wonder")
            {
                book.BookText =
                    "Before darkness consumed King Durmas IV, he was a wise and benevolent ruler. Many have forgotten the tales of his valiant battles against evil and the happiness he bestowed upon his subjects. He was also a patron of the arts, evident in the existence of his hedge maze to this day. Although the King's tomb lies within the maze, the evil that plagued him has not completely tainted its magnificence. The maze still stands as a testament to his glory, but one must be cautious as creatures now roam its passages. Cursed men transformed into minotaurs now serve as guardians of the tomb, while others seek solace within the maze, isolating themselves from the outside world. If you possess the bravery and strength, touring the maze is an experience worth undertaking. Originally designed by the King as a delightful path leading to a vibrant tavern at its heart, it now possesses a perilous allure that most individuals fear.";
            }
            else if (book.BookTitle == "The Pass of the Gods")
            {
                book.BookText =
                    "Where dwarves were once the master craftsmen of the world, their extinction was followed by the art of our elven lands. We elves create unique and unusual items, and decorate them in lavish colors. Our pursuit of fine craftsmanship led some of us on a journey to far-off valleys. They sought the highest peaks in the lands, thinking that being close to the gods would provide tutelage in the creation of items fit for deities. Their journey brought them to the orc lands of the Savaged Empire, where they found the very mountain they had only witnessed in their dreams. Our brothers and sisters never returned to the world of Lodoria, but many try to find the path of their pilgrimage. Perhaps they too can learn the art of celestial-guided crafting themselves.";
            }
            else if (book.BookTitle == "Valley of Corruption")
            {
                book.BookText =
                    "Our order is guided by the very principles of nature, neutrality, and the natural order of things. We see a balance in the world that must be protected and maintained. In recent years, this principle has slowly become corrupted by vile forces that we have yet to understand. Some of our brothers and sisters have left our order in pursuit of evil. What corrupts their minds is the desire for power over others. They no longer care for the balance of the world, but only for their own greed.<BR><BR>This corruption was most evident on the island of Kuldar. The baneful wizard Vordo convinced the local druids to join his legions of magic users, and then the demons and corrupt satyrs joined their ranks. We were tasked with dealing with this growing threat, but when we sailed to the island, it was gone. We sailed the seas for days, thinking we may have had the wrong coordinates, but it was as if it had never existed. We returned home with no further rumors of Vordo or his evil druids. Some say that Vordo unleashed a spell that doomed them all, sinking the island to the bottom of Neptune's depths. We will never know what truly happened, but the lands are safe from that dark wizard. Balance has been restored.";
            }
            else if (book.BookTitle == "The Demon Shard")
            {
                book.BookText =
                    "For centuries, warlocks often tempted daemons to visit this realm and trap them within pentagrams of immense power. Their goal was simple: to enslave a daemon to do the bidding of the warlock. To transport these daemons from the pentagrams, they had to trap them within shards of avarnium crystal. Ancient wizards sometimes possessed these shards, but they are either long dead or have joined the ranks of the dead. I have found such a crystal and hope to learn the secrets of the shard. I have learned that if I can free the daemon, it will owe me a life debt for its freedom and will accompany me without restraint. I know I have to find some shards of power to meld them with this crystal and shatter it.<br><br>I will consult the sages to learn the location of the other shards, but I need to know what type of daemon I am dealing with. I found an ancient scroll that gives me a clue. If I can make it to the Isles of Dread and commandeer a ship, I can sail to the island where the Blood Temple lies. There, I should be able to remove the first protective magics that cover this shard in secrecy. I should find the vortex of blood and stand within it. Then, using the shard, I can nullify that spell and see within it.";
            }
            else if (book.BookTitle == "The Syth Order")
            {
                book.BookText =
                    "It is said that the first of our order was the first of theirs. The castle fell from the sky centuries ago, and within those walls came the scourge of what we call the Syth. Most people believe them to be legends or fairy tales, as they make their presence known to few, and those few are often looking into the eyes of the Syth for their final moments. Their motives are the most vile in nature and perhaps could be compared with the goals of demons and devils. They wish to create chaos where there is peace and rule over those from the shadows. They slay those that stand in their way and take what they want to meet their ends. They are masters of both magic and the blade, but ancient teachings have described their power as originating from the mind rather than wizardry. To all who read this tome, beware. The Syth are real, and may you never have to face one in battle.";
            }
            else if (book.BookTitle == "The Rule of One")
            {
                book.BookText =
                    "Our mission was simple: acquire the plans for the new space station being constructed in order to identify a weakness and strike a devastating blow for the Syth. Two of us successfully infiltrated Captain Gadberry’s ship and downloaded the data, but his ship subsequently crashed on this planet. The prevailing theory circulating among the wreckage was that a stranger had taken an excessive amount of fuel from the main ship’s reserves, causing the orbit to decay. Shortly after, security discovered our presence and Master Malak and I escaped the wreckage, eluding pursuit in the forest. We established a beacon for evacuation, but we fear that we are too far in the galaxy for our signal to be picked up by our order.<BR><BR>We spent many years exploring the land and immersing ourselves in its cultures. We encountered a group of individuals often referred to as wizards who exhibited powers similar to our own. However, their methods of accessing these powers were more ritualistic, involving the use of herbs, trinkets, words, and gestures. Although different from our own approach, their abilities yielded similar effects and matched our strength of will. We searched for items of comparable power in the hopes that one could facilitate our escape from this planet, but our efforts proved fruitless.<BR><BR>We ultimately decided to expand our ranks on this planet by identifying individuals with the potential to become Syth. If we could not leave, then perhaps we should rule. Malak chose to establish a relationship with a group of death knights, as they exhibited methodologies akin to our own, making it relatively easy to impart the ways of the Syth. We became a formidable force, subduing even the mightiest opponents. However, this alliance brought forth new challenges.<BR><BR>The death knights grew hungry for power, to the extent that they began betraying one another as well as us. Their insatiable greed led to the demise of my master and fractured our collective unity. Subsequently, the holy knights of the land hunted down and defeated each death knight, leaving them dead and buried in various tombs scattered across the lands. I laid Malak to rest in the Fires of Hell, commemorating him with a statue. I entombed him alongside his datacron, which contains the knowledge of the Syth, albeit in a diminished state due to the death knights taking fragments of his knowledge with them. While I have no intention of restoring the datacron, it remains securely locked away. Should I ever change my mind, I need only utter the words 'Anakasu Arrii Venaal' to release its contents from their resting place.<BR><BR>And so, I remain as the last surviving Syth on this primitive planet. Gadberry never repaired his ship, which now lies abandoned at the crash site. According to the ship logs, the survivors ventured off to settle the land and live out their lives. I grow weary and old, but I have found an apprentice to whom I can pass on my knowledge. I will instill the principle of singularity, where only one Syth should exist. Any more than one could potentially lead to the destruction of the order as greed and power would drive us to eliminate each other in the pursuit of ultimate authority. Once my apprentice is deemed ready, they will assist me in bringing an end to my life and continue their own agenda until they too must pass on their knowledge to another. To claim Malak's datacron, they will need to grasp the concepts of psychology to a significant extent (at least 10 points) and possess negative karma. Additionally, they must master swordsmanship and tactics in order to wield the greatest power. Now, I shall rest and observe their progress.";
            }
            else if (book.BookTitle == "Antiquities")
            {
                book.BookText =
                    "Rare items are the goal for most land collectors, as I myself would pay plenty of gold for something unique. My days of exploring dreadful dungeons are long past, but other brave souls manage to bring back long-forgotten items from the dingy hallways of abandoned dwellings where evil commonly lurks. One may find an unusual weapon or armor that is no longer usable but can be decorative for a wealthy home or castle. Other rare items could be found in the forms of leather, furs, cloth, liquors, reagents, jewelry, or mystical items from long-dead wizards. Others may be decorative clocks, vases, statues, or paintings of people and scenery from long ago. These items are often kept by adventurers as trophies for their adventures. Others would rather barter for gold for such items. Someone skilled in item lore can usually determine the value of such items and who in the settlements may want them. Those who are not skilled in item lore or identification can usually guess who may want the item. These items cannot simply be sold to collectors, but instead, one would give them the item and the citizen will give them an appropriate amount of gold. Usually, those skilled in mercantile will get more gold than the average adventurer, but the value can still be great. So, if one wishes to carry such treasures, they can surely increase their wealth beyond the coins they may hoard.";
            }
            else if (book.BookTitle == "The Jedi Order")
            {
                book.BookText =
                    "This world is not my home. It lies far away in the Coruscant system, in the Zeta Quadrant, among the stars. When Gadberry's ship crashed, I became stranded here along with everyone else. However, we were not alone that ship. The Syth also came aboard at some point, attempting to steal critical data from the computer system. While they succeeded in their efforts, it proved to be futile as they, like me, could not escape. The Syth fled from the ship, and the crew sought my counsel on this vile cult of psychics. I gave chase into this strange world but never found a trace of them. I vowed to never return to the ship until I found the Syth and dealt with their treachery. Over the years, I learned more and more about this world, with its strange creatures and humanoid beings with powers similar to mine, which became a normal affair to me.<BR><BR>Despite this, I still believed the Syth were out there, plotting. I decided to teach the Jedi way to those who possessed the psychic potential of psychology in others. Some learned quickly, while others struggled to fully grasp the teachings. Those who joined the order battled the Syth, but they were the Syth of this world, not the ones who crashed here with me. This suggested that they were gathering followers. This secret war waged on, and we began to see fewer and fewer Syth in the land. Perhaps we had defeated them, at least on this planet.<BR><BR>As peace settled in, many members of our new order integrated into civilization. Although I knew them by their Jedi names, they embraced new names for themselves and became priests in the various villages of the realm. I approved of this life, as it provided a noble end for them. These ten Jedi were entrusted with a holocron of wisdom for a specific Jedi power. If time caught up with them, I only asked that they take their holocron to their final resting place. Anyone who presents themselves to learn the Jedi way will discover these secrets at my resting place. I chose a cave far to the east of Britain, where Jacen is instructed to lay me to rest. If someone is worthy of this path, they need only speak 'Oh Beh Wahn' at my tomb, and they will receive the wisdom I will pass on to start their journey. If the Syth return, then the Jedi must also return.";
            }
        }
    }
}
