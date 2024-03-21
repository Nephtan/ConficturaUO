using System;
using System.Collections;
using System.Text;
using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
    public class SpeechGump : Gump
    {
        public SpeechGump(Mobile from, string sTitle, string sText)
            : base(50, 50)
        {
            string color = "#d5a496";

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddImage(0, 2, 9543, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddHtml(
                12,
                15,
                341,
                20,
                @"<BODY><BASEFONT Color=" + color + ">" + sTitle + "</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            AddHtml(
                12,
                50,
                380,
                253,
                @"<BODY><BASEFONT Color=" + color + ">" + sText + "</BASEFONT></BODY>",
                (bool)false,
                (bool)true
            );
            AddButton(367, 12, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendSound(0x4A);
        }
    }
}

namespace Server.Misc
{
    class SpeechFunctions
    {
        public static string SpeechText(Mobile m, Mobile from, string sConversation)
        {
            string sYourName = from.Name;
            string sMyName = m.Name;

            Server.Gumps.MyChat.speechText(sConversation, from);

            string sText = "";

            if (sConversation == "Ranger")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I can teach you about surviving in the land. Surviving in the wilderness is often easier for those who can rely on their wits rather than magic. We rangers have no need for magical scrolls or powerful spells. We know when and where to rest. We strategically place our camps for complete safety. A well-rested adventurer is one who can return and share their tales. If you want to live off the land, you must practice the skill of camping. With a bedroll and a simple campfire, you can rest safely for the night. You can also use small tents, which are useful in immediate danger. If you can assemble your tent quickly enough, you can remain safe for a few minutes while you prepare for the dangers outside. These tents can only be used outside, so don't try to use them in dungeons or tombs.<br><br>Camping tents are what keep the best rangers going for long periods of time. It could be weeks before a ranger returns to a settlement, as camping tents can provide almost everything a ranger needs. For skilled rangers, camping tents can provide food, water, and unlimited safety for rest. Those who are experts in camping can build their camping tents and have even more comforts, enough to attract other adventurers who may provide you with food, drink, and survival gear. These camping tents also allow you to organize your items from your bank box. The best part about camping tents is that you can use them underground. This allows for greater dungeon exploration as you can safely store your found treasure without overburdening yourself. I sometimes sell small tents and camping tents, but you may find it easier to get them from provisioners.<br><br>You can use traps to help hunt creatures. Sometimes you can buy plain trapping tools, but tinkers can usually make the best ones since they can use metals that cause more damage than plain iron. Simply use the tools and a trap will be placed at your feet. Although you can avoid your own trap, others may not be able to. The higher your trap removing skill, the more effective your trap will be. If you have trapping tools made from more precious metals, they will be even more effective.<br><br>Safe travels.";
            }
            else if (sConversation == "Shipwright")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I can teach you about sailing and navigating the high seas. When you are able to purchase your own ship, you will be given a deed from which you can have your ship built. Ships may only be placed in water, and there must be a nearby dock. When you dry dock your ship, you must also be near a dock. When dry docked, you will get a small representation of that ship to carry with you for safekeeping. Take this to any outdoor water source and double-click it, where you will then be asked to place it in the water. A key will be placed in your backpack and bank box to lock and unlock the vessel. If you have recall magic, you can use the key to magically travel to your boat no matter where it is. Now, if you own your own home, you may get a 'docking lantern' from me to place in your home. This also assumes that your home is built near a body of water. The 'docking lantern' must be secured in your home. This allows you to launch or dry dock a ship near that home. You can even set the security on the lantern so that only certain people you trust can make use of its light.<br><br>You may paint your ship any color you want, but keep in mind that the entire ship will have the color you paint it. Use any method you would to dye items on your ship that is in your pack. When you place the ship in the water, it will have that color. I have seen some horrific looking ships because of this, but it does allow you to have a ghostly looking ship or even evil dark ship on the high. No matter if on the water or in your possession, the ship will maintain the color which it is painted.<br><br>As for sailing the ship, you must be on the deck of your boat to pilot her. Double click the tillerman, and a window will appear that will allow you to navigate the ship. You may place this window over your mini-map for better navigation. You will notice red diamonds on the different directions. Select these to move the boat in that direction. Don't forget to raise the anchor first. This can be done on the wooden placard. There you can drop or raise the anchor. You can rename your ship to whatever you want. Lastly, you can set the speed of your ship to either full speed or move alone a step at a time. On the placard, there are three triangle arrows. There is one on the left, one on the right, and one on the bottom. Selecting the left or right ones will turn your ship in that direction. Selecting the bottom one will make your vessel come about. There is a red flag attached to the placard. Selecting this will stop your ship. There is a gray tab on the right of the placard. Selecting this will switch this window from one map size to another. If you wish to dry dock your ship, make sure the hold is empty as well as the deck. While on shore, double click the tillerman, and he will ask you if you wish to dry dock the ship.<br><br>If you have a sea chart from a cartographer, you can use it to plot a course. After you plot the course on the map, give it to the tillerman. Once he exclaims 'a map!', he will give it back to you, and you can simply say 'start'. The tillerman will follow the course you set on the sea chart.<br><br>There are different sizes and styles of ships. The type you acquire matters little. The larger ships allow you to have more passengers, and the hold size greatly increases the bigger the ship. Some like to sail the world and flaunt their wealth. Others merely seek adventure on the high seas. A few seek only to relax and fish. Abandoned boats will only last about 30 sunrises before the hull rots and sinks into the sea, so don't leave her alone that long without putting her in dry dock. Be careful out there no matter the reason. Pirates usually sail about and they take no prisoners. May the wind be at your back.";
            }
            else if (sConversation == "Banker")
            {
                sText =
                    "Welcome to our bank, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and if you need currency conversion assistance, I can help. Typically, merchants only accept gold coins. However, adventurers often come across coins from the past, such as copper and silver. These coins are relatively common to find. To make use of them, we melt down the copper and silver and sell it to local blacksmiths. We offer a trade of 1 gold coin for every 5 silver coins you bring to us, and the same trade ratio applies for every 10 copper coins you bring. Though it may seem unfair, this is the only way to exchange these coins. Simply give us a stack of coins, or you can double click on the stack in your bank box, and we will handle the conversion for you. If you happen to stumble upon piles of jewels, we can also convert those for you. The local jeweler frequently visits to barter for jewels. You can identify jewels by their size, which is similar to most coins, but they vary in color. Please do not mistake jewels for gems like diamonds and sapphires. Jewels are found in piles, just like coins. For every jewel you bring, we will exchange it for 2 gold coins.<br><br>In addition to coins and jewels, we deal in certain types of stones and crystals that you may come across. We can convert gold nuggets into coins for you, but please note that we are not referring to gold ore. Gold nuggets have a distinct shimmering glow. If you discover gemstones, which differ from jewels and common stones like diamonds, we can offer 2 gold coins for each one, as we use this currency with other distant kingdoms. Crystals also hold value for us, particularly when it comes to dealings with beings from other planes of existence. Since crystals are incredibly rare to find, we have a great interest in them. Some claim to have mined for crystals in the Mines of Morinia, but no one has returned to verify these tales. Crystals are shimmering red in color, and we are willing to offer 5 gold coins for each crystal you find.<br><br>Now, this might sound peculiar, but we have noticed the existence of bright green coins. Adventurers claim to discover these coins in an ancient castle that fell from the sky centuries ago. If you upon these coins, we are than willing to exchange them for 3 gold coins each. However, please be aware that these coins are quite, so the chances of finding them are unlikely. Lastly, if you require a safe for your, we sell safes that enable access to your bank account from the comfort of your own residence. While they do cost a significant amount of gold, we offer them for sale nonetheless.";
            }
            else if (sConversation == "Weaponsmith")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and if you require repairs for your weapon, you can hire me to do the job. However, I must apologize as I do not fix ranged weapons. It would be best to consult a bowyer for any repairs needed on ranged weapons.";
            }
            else if (sConversation == "Variety")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and if you come across anything unusual in your adventures, I may be interested in taking a look. I am willing to pay well for rare items such as vases, paintings, statues, banners, or carpets. There are also other rare items you may discover, such as unusual reagents that an alchemist might desire, strange armor or shields that armorers would seek, unusual decorative weapons that a weaponsmith would find interesting, or rare books that scribes would love to read. You might even come across ancient scrolls containing strange spells that you could never decipher, but a mage would be willing to pay a great sum for. Sages might be interested in stone tablets with long-forgotten text, while tailors would trade for special cloth and leatherworkers for incredible leather. Fur traders would desire piles of strange furs, while bards often enjoy adding ancient instruments to their collections. Even a finely aged keg of ale would be greatly appreciated by the local tavern. When you find these items, you can either keep them for yourself or give them to one of us local merchants who would be interested in acquiring them. You will be given gold based on the value and rarity of the item. Skilled merchants or arms dealers would often receive more gold than the average adventurer.";
                if (Server.Items.MuseumBook.IsEnabled())
                {
                    sText =
                        sText
                        + "<br><br>I am seeking an experienced adventurer to catalogue a specific number of antiques for my museum. These antiques have not yet been proven to exist, but there are a total of 60 of them. If you are interested in assisting me and have already explored the nine known lands, kindly provide me with 500 gold coins so that I can create a copy of the book you will need for this task. Once you have found all the antiques, please return the book to me, and I will reward you with "
                        + MuseumBook.QuestValue()
                        + " gold. Additionally, I am willing to trade gold for any antiques you discover, as each item holds a specific value. Should you choose to trade, simply hand me the item, and I will provide you with the corresponding amount of gold.";
                }
            }
            else if (sConversation == "Thief")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", but that is known only to a few. If you come across a chest with a stubborn lock, you can hire me to remove it. If you're interested in my skills, 'The Art of Tomb Raiding' book can help you. Those skilled in stealing can take unique items from dungeons. It may be worth your while.<br><br>Teaching yourself how to stealth around can be a skillful test. You need to achieve a 30.0 in hiding. To improve, try stealth in heavier armor as you get better. Here are examples of items to wear for different skill ranges:<br><br>30.0-57.4:<br>   - Leather Gorget<br>   - Leather Gloves<br><br>57.4-65.0:<br>   - Ringmail Gloves<br>   - Leather Sleeves<br>   - Leather Gorget<br><br>65.0-84.5:<br>   - Ringmail Gloves<br>   - Ringmail Sleeves<br>   - Leather Gorget<br><br>84.5-95.0:<br>   - Ringmail Gloves<br>   - Ringmail Leggings<br>   - Leather Tunic<br>   - Studded Sleeves<br>   - Studded Gorget<br><br>95.0-100:<br>   - Leather Leggings<br>   - Chainmail Tunic<br>   - Studded Gorget<br>   - Leather Cap<br><br>100-120:<br>   - Chainmail Tunic<br>   - Ringmail Sleeves<br>   - Platemail Gorget<br><br>You can use traps to catch unsuspecting travelers off guard. You can buy plain trapping tools, but tinkers can make better tools using metals that cause more damage. Use the tools to place a trap at your feet. While you can avoid your own trap, others may not. The higher your trap-removing skill, the more effective your trap will be. Trapping tools made from more precious metals are even more effective.<br><br>Now, let me tell you about the towns and cities. We, as a thieves' guild, lighten the purses of local citizens. It's a tricky venture, but merchants carry gold in their pockets and coffers. You need to snoop the merchant or coffer to see if there is gold to steal. If the merchant has gold, you can try to steal it. If the coffer has gold, use your stealing skill on the coffer itself. Avoid getting noticed to prevent guards from being called. You can steal coins and other items from creatures by standing next to them and attacking.<br><br>Lastly, if you want to do shady jobs for our prominent clientele, whisper to my guildmaster and they will give you a job. These jobs aren't limited to guild members, any able thief can do them. You will receive a message about what you need to steal and where to find it. These tasks are unusual because they involve discreet exchanges, so you will always receive instructions on which city to take the item to. After dropping it off, your reward will be waiting along with another job if you're interested.<br><br>Be careful out there. Remember that you can perform a sneak attack on enemies if you attack them while hidden. Performing such attacks will test your hiding and stealth skills, increasing the damage of the attack. Powerful foes are best dealt with using a sneak attack.";
            }
            else if (sConversation == "Tailor")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and if you don't like the look of your cloak or robe, you can hire me to alter it for you. Some of these garments may appear unusual compared to what I usually sell, but I can make them look more for you. If you happen to acquire a hood or cowl, their color can be changed. Just double-click it while it's on your head. You can choose something you're already equipped with, such as a robe, and it will change its color accordingly. This allows you to effortlessly coordinate your outfit with a hooded robe or cloak.";
            }
            else if (sConversation == "Scribe")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I can decipher almost anything. Many spellcasters seek mystical power, but the only way to obtain such things is by exploring the forgotten dungeons of the land. If you find any unusual scrolls, you can hire me to identify them. It may make your spellbook more complete. Also, keep in mind that you can hold certain spellbooks in your trinket spot. This is beneficial if your spellbook has properties that go beyond the normal bounds.<br><br>Furthermore, if you come across any strange parchments, you can hire me to decipher them for you. I charge 100 gold for this service, as it is more difficult to unravel these oddly written riddles or letters. These parchments often contain clues or riddles that lead to many strange and valuable things. However, half of the time they are simply false or misleading. I am not the only one capable of deciphering these parchments. A very intelligent adventurer could also solve them. Parchments may be coded in various levels of difficulty, ranging from plainly coded (20 int) to diabolically coded (80 int). Whether the information they contain is true or false, I cannot say. Perhaps a gypsy could shed some light on it for you.<br><br>If you are interested in performing librarian duties and are practicing inscription, there are many books and bookshelves in the dangerous areas of the land that you may want to investigate. You will need a monocle, which you can purchase or have a glass blower create for you. Some have simply come across them. Monocles will help you search the books and bookshelves in dungeons, for example. Simply use a monocle on a shelf or book pile to investigate it further. Monocles wear out, so be sure to bring enough with you if you embark on a lengthy research journey. Searching these books and bookshelves may yield blank scrolls as well as magical scrolls. You may also come across rare books.<br><br>If you are a wizard or necromancer and wish to perform research to increase your knowledge of ancient forms of magic, I can sell you a research pack to hold all of your research materials. Not only can you research ancient forms of magic, but you can also study widely known spells that you may have difficulty finding the original source for. Research requires searching the world for ancient wizard tomes of information to construct the spells. You must also become highly proficient in inscription to research and scribe the new magic you learn about. If you want to pursue this research, give me 500 gold coins and I will provide you with a pack specifically for this purpose.";
            }
            else if (sConversation == "Mapmaker")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and maybe you are searching for lost treasure? If you find a treasure map that you cannot decipher, you could hire me to assist you. Of course, the more valuable the rumored treasure, the higher the cost for me to interpret the map. Alternatively, you could practice cartography and draw your own maps. With enough skill, you could decode your own treasure maps. These maps provide a specific area where the treasure can be found, along with coordinates for its location, so remember to bring a sextant. Once you reach the designated spot, use a regular shovel to dig up the treasure indicated on the map. However, be cautious, as opening the chest may attract attention from creatures who will undoubtedly try to prevent you from claiming your find. Lastly, I can create a world map for you to aid in your travels. If you provide me with 1,000 gold, I will begin working on it.";
            }
            else if (sConversation == "Mage")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and you can hire me to charge any magic wands you may have. I usually require 100 gold per spell circle of the wand, which will increase the charges by 5.<br><br>Also, if you are curious about the wizardly arts, I have some advice for you. The 'create food' spell not only creates food but also conjures a mystical bottle of water. Make sure to eat and drink during your journey. The 'remove trap' spell should not be overlooked. It does more than simply remove traps from containers. It also creates a magical orb that goes in your backpack. This wand will passively check for traps on the floors and walls, along with those placed on containers. The 'telekinesis' spell allows you to pick up an object that is out of your reach and place it in your backpack. However, it does not work on stacked items or items in another container. Some dungeons have hidden traps and chests within them. Although the 'reveal' spell was meant to only find creatures that hide, it will also find traps and hidden treasure containers.<br><br>Casting spells can be a tedious process, but it doesn't have to be. There is a secret that few wizards know. You can have up to 4 custom spell bars that you can cast with. These spell bars keep your favorite spells at your fingertips and allow you to cast them quicker. To learn the commands to access these secrets, you can find them by using the 'Help' button on the paperdoll.<br><br>A word of warning in practicing other forms of magic: elementalism and magery cannot be bestowed upon the same spellcaster. Knowing about both forms will cause any of the spells to fail when attempting to cast. This includes having items that enhance these types of skills. An aspiring spellcaster must choose a path and move toward that goal. You can either pursue elementalism or learn about the more diverse and powerful magery. If you find that you are losing interest in one of these schools of magic and want to quickly pursue the other, then search for the Sorcerer Cave in Sosaria or the Conjurer's Cave in Lodoria. They have crystals discovered centuries ago that can help you forget what you learned in one area of magic so you can learn another.";
            }
            else if (sConversation == "Monk")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I am an experienced practitioner of the exotic arts of bushido, ninjitsu, shinobi, and the way of the monk. I can teach you many of these skills and I also sell tomes and scrolls that will enable you to practice these abilities on your own. An exotic lifestyle may not be suitable for everyone, but if you choose to follow this path, you may be interested in exploring the land in search of Oriental treasures (in your Help options, you can set your Play Style to Oriental). In doing so, you will come across a wide range of exotic items, including armor, weapons, and other relics.<br><br>Both bushido and ninjitsu are relatively easier to master, but those who embark on the path of mystics or monks must seek out the appropriate shrines and meditate there in order to learn the various skills that can be. If you obtain a Monk's Tome, it will provide a comprehensive explanation of these practices Another rare pursuit is the way of the shinobi, which enhances a ninja's abilities by providing additional skills such as disguises, the ability to run as fast a horse, or the skill to unlock treasure chests. To undertake this goal, one simply needs to be dedicated to the path of the ninja and possess a shinobi scroll.<br><br>If you are simply interested in purchasing some exotic items, I offer a variety of options suitable for both ninjas and samurai. Additionally, you are welcome to utilize our dojo for training or to take rest.";
            }
            else if (sConversation == "Necromancer")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and casting spells can be a tedious process, but it doesn't have to be. There is a secret that few necromancers know: you can have up to 2 custom spell bars that allow you to cast your favorite spells more quickly. To learn the commands to access these secrets, you can find them by using the 'Help' button on the paperdoll.<BR><BR>Also, if you have the stomach for it, you can get yourself a grave digging shovel and head for the cemetery. People are buried with all sorts of valuables, and there is no need for a dead man to have them. These actions will work your forensic skills, which may make you a good undertaker one day.<BR><BR>Before I forget, the ancient text you will find is incorrect. The 'exorcism' spell does not actually do what the book states. It is really used against demons and the dead. If you are powerful enough, you can send them back to where they came from.<BR><BR>A word of warning: practicing other forms of magic, such as elementalism and necromancy, cannot be bestowed upon the same spellcaster. Knowing about both forms will cause any of the spells to fail when attempting to cast. This includes having items that enhance these types of skills. An aspiring spellcaster must choose a path and move toward that goal. You can either pursue elementalism or learn about the deathly magic of necromancy. If you find that you are losing interest in one of these schools of magic and want to quickly pursue the other, then search for the Sorcerer Cave in Sosaria or the Conjurerer's Cave in Lodoria. They have crystals discovered centuries ago that can help you forget what you learned in one area of magic so you can learn another.";
            }
            else if (sConversation == "Elementalism")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I practice elemental magic, which differs from other forms of magic in that it relies on both the intellect and physical condition of the spellcaster. Reagents are not required, but casting these spells demands an equal use of mana and stamina. If you come across magical items with lower reagent quality, the stamina required for spells will be reduced proportionally by that value. Elementalists do not require supplemental skills like mages with psychology or necromancers with spiritualism. We have a guild at our Lyceum, hidden on the Devil Guard mountains with an illusionary spell. To reach there, you only need to cast the Sanctuary spell. This spell cannot be used everywhere, but as you learn more about elementalism, you will be able to cast it in places like dungeons to quickly find safety.<BR><BR>When you visit the Lyceum, you will find four shrines dedicated to the elements of Earth, Air, Water, and Fire. Each elementalist can only focus on one element at a time, but they can change their focus whenever they want by stepping up onto the shrine of their choice. If you want to change your clothing to match your focused element, simply say the word 'culara'. Changing focus does not affect your spell library. For example, if you have 10 spells in your Earth Elemental Spellbook and you change your focus to Water Elemental Magic, your Water Elemental Spellbook will also have the same 10 types of spells.<BR><BR>The only way to scribe elemental spells onto scrolls is by writing the magic words with daemon blood. Casting spells can be a tedious process, but it doesn't have to be. There is a secret that few elementalists know - you can have up to 2 custom spell bars that allow you to cast your favorite spells quicker. To learn the commands to access these secrets, you can find them by using the 'Help' button on the paperdoll.<BR><BR>Our magic is said to have been forged by the Titans of the Elements, and many elementalists try to venture into the Underworld to seek them out. What they desire are the spellbooks from the Titans, as they are extremely powerful. However, most mages and elementalists often fail in their spellcasting attempts in the Underworld. Elementalists believe that equipping one of the Titan's spellbooks would allow them to explore the dark realm without this hindrance. Whether this is true or false, one can only try.<BR><BR>A word of warning: practicing other forms of magic such as magery and necromancy cannot be bestowed upon the same elementalist. Having knowledge of these other forms of magic will cause any spell to fail when attempting to cast it. This includes having items that enhance these types of skills. Aspiring spellcasters must choose a path and pursue it. You can either focus on elementalism or learn about magery and necromancy. Knowledge of elementalism even affects the forces of magic wands or runic magic. The pursuit of magic research is also not achievable for elementalists. If you find yourself losing interest in one of these schools of magic and want to quickly pursue the other, you can search for the Sorcerer Cave in Sosaria or the Conjurerer's Cave in Lodoria. These caves have crystals that were discovered centuries ago, which can help you forget what you learned in one area of magic so you can learn another.<BR><BR>Lastly, elemental magic is highly sensitive to the forces surrounding the caster. The more armor you wear, the more likely your spells will fail. You can either avoid wearing armor or find armor suitable for spellcasters, preferably with mage armor qualities. Shields with spell channeling forces can also be helpful. Metal armor is the most obstructive in this regard, while wooden armor is less detrimental. Leather armor has the least impact, but finding quality clothing is the desired course.";
            }
            else if (sConversation == "LeatherWorker")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I can assist you in repairing your leather gear if you're interested in hiring my services. My expertise includes fixing leather armor, bear masks, deer masks, as well as pugilist and throwing gloves.";
            }
            else if (sConversation == "Herbalist")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and have you ever come across peculiar reagents during your journeys? If so, you could enlist my services to identify them. It's essential to determine the nature of these substances before incorporating them into potions or casting magical spells.";
            }
            else if (sConversation == "Furtrader")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and if you have a bear or deer skin mask, you can hire me to repair any rips or tears it may have. You surely want to keep it in good shape, don't you?";
            }
            else if (sConversation == "Bowyer")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I specialize in repairing worn bows and crossbows. If you need my services in that regard, feel free to hire me. I can also change the style of a quiver for you if you simply hand it over.";
            }
            else if (sConversation == "Alchemist")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and maybe you've come across some peculiar liquids? If that's the case, you could consider hiring me to identify them for you. It takes a unique individual to accurately determine the nature of various liquids, and you might be pleasantly surprised by the results.";
            }
            else if (sConversation == "Blacksmith")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and maybe you're here because your armor or sword broke? You've come to the right place. I can fix almost any weapon, except for bows and crossbows. I can also repair nearly any metal armor you have.";
            }
            else if (sConversation == "Armorer")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I am highly skilled in the art of repairing metal armor. If you possess any armor in need of my services, do not hesitate to employ me. I will restore it to a state nearly indistinguishable from its original condition. After all, who wants to confront a dragon while donning damaged armor?";
            }
            else if (sConversation == "Knight")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + " the Knight, and perhaps you are interested in seeking the path to knighthood? It is a difficult path, but if you remain true, you will be mightier than the mightiest of barbarians.<br><br>Knight magic can be a tedious process to call forth, but it doesn't have to be. There is a secret that few knights know. You can have up to two custom spell bars that you can cast with. These spell bars keep your favorite spells at your fingertips and allow you to cast them quicker. To learn the commands to access these secrets, you can find them by using the 'Help' button on the paperdoll.<br><br>Also, keep in mind that we are able to remove curses. This includes curses on the body as well as on items. If you end up having one of your items cursed or your tomes of knowledge locked in a cursed box, you can remove the curse on your own if you have the skill to summon the magic of 'remove curse'.";
            }
            else if (sConversation == "Provisioner")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and perhaps you need some equipment before heading out? There are many things I carry that can help you. Lanterns and torches can light your way in the darkest of dungeons. Be warned, these items will eventually burn out, and you will have to light another. Many adventurers fall from traps set in these dark places. If you want to be more cautious, I do sell 10-foot poles. These allow you to tap containers, floors, and walls to search for traps. This works passively, so you only need one in your pack, and you will automatically check for such dangers. Almost all traps are invisible to the naked eye, so beware. Some are more obvious, such as a stone face on a wall that shoots fire or mushrooms that may explode with harmful spores. With a bedroll and a simple campfire, you can rest safely for the night. You may also make use of small tents. These tents are useful when you find yourself in immediate danger. If you can build your tent fast enough, you can remain safe for a couple of minutes while you prepare yourself for the dangers outside. These types of tents can only be assembled outside, so don't try to use them within the dungeons and tombs.<br><br>Camping tents are what keep the best adventurers going for long periods of time. It could be weeks before one returns to a settlement, as camping tents can provide almost anything an adventurer needs. For those skilled in camping, a camping tent can provide food and water, along with unlimited safety for rest. Those experts in camping can build their camping tents and have even more comforts. Enough to draw in other adventurers that may provide you with some of their wares like food, drink, and survival gear. These camping tents can also allow you to organize your items from your bank box. The nicest part of camping tents is that you can use them while underground. This allows for greater dungeon exploration as you can safely store your found treasure and not overburden yourself.<br><br>We sometimes sell merchant crates for your home if you are a craftsman. These are crates where the Merchants Guild will come to your home once a day and take what is in the crate in exchange for gold. It can really help those that want to set up a shop and make a few gold from the items they craft.<br><br>If you have some unusual item, I can sometimes tell you what you can get for it, and from whom. These are strange items that are usually decorative in nature and not something you would be able to use. It could be armor, artwork, banners, books, cloth, carpets, coins, furs, gems, gravestones, instruments, jewels, leather, orbs, paintings, reagents, rugs, scrolls, statues, tablets, vases, or weapons. If I can't tell you the value of it, then you will have to find what to do with it on your own. If I determine that it does have value, give the item to the merchant I mention, and they will give you some gold for it. Just hand it to them or keep it to decorate your home.<br><br>Safe travels.";
            }
            else if (sConversation == "Architect")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and perhaps you are interested in owning land and building a home? I can only provide you with a construction contract, but it is up to you to find the area in which you wish to build. You must find a semi-flat area devoid of large obstructions and clear enough for the size of the home you wish to build. ";

                if (Server.Misc.MyServerSettings.AllowCustomHomes())
                {
                    sText =
                        sText
                        + "You can either build a standard home, or a plot you can customize yourself. ";
                }

                if (Server.Misc.MyServerSettings.AllowHouseDyes())
                {
                    sText =
                        sText
                        + "What makes standard homes unique is the ability to paint them before construction. You can easily dye the contract in a color of your choosing, ensuring that the entire home will be built in that particular shade. However, it is worth noting that there have been instances where poorly chosen colors have resulted in unsightly homes. Nevertheless, this feature allows for the creation of various structures, such as a dark tower or a blood-red castle. It even provides the opportunity to construct a house that appears to be carved from ice. It is important to mention that while the structure itself will reflect the chosen color, the doors and house sign will not. ";
                }

                sText =
                    sText
                    + "<br><br>You may find some abandoned dungeons throughout the lands. We cleared them out of small vermin and put them up for sale. If you happen to find one for sale, and you have the gold, you may purchase it.<br><br>Over the years, some wizards have fallen from either accidents or old age. They once owned great castles in the sky that we put up for sale as well. They are hard to find as you must find the mystical rope that leads to these castles in the sky. They are not as expensive as dungeons, and they also do not provide all of the space and benefits that dungeons have...but they are generally larger than castles you could build on the ground.<br><br>Decorating your home can be quite the task when you have found many interesting relics you wish to display. If you get a set of 'homeowner tools', it will make this job much easier. These tools allow you to raise or lower locked down items in your home. You can also move them north, south, east, or west. You can quickly secure or unsecure things with this tool, lock down items, or place a trash barrel. Some items can be turned in two different directions, and these tools can do that for you. If you get an item/deed that has a direction in its name (like east or south), place that item/deed on the floor of your home and use the 'flip deed' option to get the item/deed in a different direction. Keep in mind that most of these particular items are 'deeds', but there are some things like carpets or trophies that look like the item that will be placed in the home. If you do place a deeded-type item in your home, you can remove it (re-deed it) by chopping the item with an axe. You can get rid of the trash barrel in a similar fashion.";

                if (Server.Items.BasementDoor.IsEnabled())
                {
                    sText =
                        sText
                        + "<br><br>If you want access to the public basement from your home, you just need to purchase a basement trapdoor and secure it on your floor. This public area is accessible from various locations in the land, including people's homes and certain shops such as tinkers, blacksmiths, or tailors. Basements have designated areas for crafting, but only if accessed through a merchant's shop. They provide a shared space for people to gather, for crafters to sell their goods, and possibly for fixing your items. Basements are secure areas where you can unwind and socialize with others.";
                }
                if (Server.Items.MovingBox.IsEnabled())
                {
                    sText =
                        sText
                        + "<br><br>If you ever need to move from your current home to another, the work can be quite involved. I sell housing crates that can help you with this. They can hold a large number of items and are easy to carry, even when completely full. However, these crates can only be opened by the person who purchased them and they can only be opened when inside a home. If you try to lock them down or secure them in the home, the items will be counted towards your home's storage. It is also best to fill the crate while it is on the floor in your house, rather than in your pack. Otherwise, you may attempt to put containers with more items than your pack can hold into the crate while it is in your pack, which won't work well. Once the crate is filled, you can place it in your pack without any issues. Additionally, you can open the crate at the bank if you need to move your possessions to a newly placed home. However, you should be cautious as the crate can be stolen by others or misplaced, resulting in the permanent loss of your belongings.";
                }
                if (Server.Misc.MyServerSettings.LawnsAllowed())
                {
                    sText =
                        sText
                        + "<br><br>For homeowners looking to enhance the aesthetic appeal of their property, we offer a range of lawn tools. These tools enable you to add elements such as trees, rocks, water features, yard decorations, and diverse ground terrains to the outskirts of your home. Once you make a purchase, simply bring the tools back home and utilize them while standing on your land. You will have the freedom to select from various building options, each requiring a specific amount of gold, which will be deducted from your bank account. Further information about these tools can be obtained by using them within your home.";
                }
                if (Server.Misc.MyServerSettings.ShantysAllowed())
                {
                    sText =
                        sText
                        + "<br><br>If you are a homeowner who wants to remodel their home, you can purchase remodeling tools from me. These tools enable you to place items such as walls, doors, stairs, or other decorative elements. Once purchased, simply take them home and use them while standing in your house. You will be provided with choices on what you want to build, with each option having a specific cost in gold. The required funds will be deducted from your bank box. When you use these tools in your home, you can acquire additional information about them.";
                }
            }
            else if (sConversation == "Guard")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I am a guard on patrol here. There aren't many laws of the land, but the few there are... are something to be followed. Don't try to steal from anyone. If caught, you will be cast as a criminal for 2 minutes. During this time, you can be killed by others without repercussions to them. They may also freely take things that belong to you. Most importantly, we will kill you on sight. Another thing that will make you a criminal is to attack another innocent person.<br><br>Murder is the most foul of crimes. The town crier will know of all those in the land wanted for murder. Killing another innocent person will cast you as a murderer. Each murder violation will only be forgotten by all if you are within the realm for 40 real-world hours. Whether you are a murderer or a criminal, the townsfolk will have no dealings with such filth. It is best to stay on the right side of law and order.<br><br>We also have set bounties on criminals that plague our lands. If you bring me the head of a pirate or one of the many types of thieves, you will be given gold for your efforts. There are also assassins, hunters, or ninjas about. We have a price on that murderous bunch as well. It is normally an offensive act to cut the head from a corpse, but doing such a thing on this scum will bring no ill will toward you.";
            }
            else if (sConversation == "Healer")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ". There are a few things you should know about our group. We offer free healing, but only at certain intervals. We will assist you if you are in need, but summoning spirits from the dead is a draining task, as any experienced wizard will confirm. If your spirit does find its way here, we will require a donation to our order in exchange for our help. It is a small contribution considering the opportunity to return to the land of the living. The gods can resurrect you on their own, but it will take a toll on your body if you allow them to do so.<br><br>There are also shrines scattered throughout the lands. These places not only allow you to donate gold, but also provide the means for resurrection. The shrines require the same amount of gold or donation as we do, but they may be closer to your corpse than we are.<br><br>We have heard rumors of resurrection potions and witchery brewing mixtures that can bring souls back to life. However, we have not encountered such things ourselves, so we cannot be certain. Perhaps you will have better luck with these matters.<br><br>If you are skeptical about using magic for healing, carrying bandages can be a valuable option. Some individuals have become skilled in healing with bandages and are known to cure poison or revive the dead. To make bandages easier to use, you have a couple of options. You can utilize the built-in macros for bandaging yourself or others. Another option is to use the '[bandself' or '[bandother' commands. Both options work effectively and could potentially save your life.<br><br>Regardless of how one meets their end, their spirit will be guided to the nearest healer or shrine if they seek assistance in resurrecting their soul.<br><br>Once someone is revived, they often seek the location where they perished. This is relatively straightforward if the death occurs in a hallway or cave, but it becomes more challenging in the wilderness where the landscape appears similar in various places. If you need assistance in locating your remains, simply use the '[corpse' command and it will guide you to it, as long as you are in close proximity.";
            }
            else if (sConversation == "Druid")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + " the Druid. I am the protector of all things in the forest. All plants and animals serve a purpose in the world, and the balance of that must be maintained. If your animal companion were to suffer a horrible fate, you may be able to bring their soul to me, and I would resurrect them for you. I do not charge for such services, as it is merely my duty to maintain this balance.<br><br>You may also be interested in druidic herbalism to create unique potions. There is a book called 'Druidic Herbalism' that will explain how you can explore this hidden magic of nature.";
            }
            else if (sConversation == "Cook")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + " the culinary around here. To become a chef, simply cook! For many preparations, you need to be near a heat source, which is usually an oven. However, for some dishes like barbeque, a campfire or forge will suffice. Here are a few examples of cooking heat sources:<br><br>- Campfire<br>- Fireplace<br>- Forge<br>- Heating stand<br>- Oven<br><br>Some common cooking tools that you'll need include a rolling pin, skillet, and flour sifter.";
            }
            else if (sConversation == "Sage")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + " the Sage. If you have an unknown magical wand or a strange artifact that you cannot identify, perhaps I could assist you. I charge 500 gold to identify wands and 5,000 gold to identify artifacts. Alternatively, you can develop this skill yourself and no longer require my services.<br><br>If you are interested in performing librarian duties and practicing inscription, there are many books and bookshelves in dangerous areas of the land that you may want to explore. To investigate these areas, you will need a monocle. You can either purchase one or have a glass blower create it for you. Some adventurers have come across them by chance. Monocles are useful for searching books and bookshelves in dungeons. Simply use a monocle on a shelf or book pile to further investigate it. Keep in mind thatles do wear out, so bring enough with you if you plan on embarking on a lengthy research journey. Searching these books and bookshelves may result in finding blank scrolls or magical scrolls You also stumble upon rare books.<br><br>Before you depart, I offer a word of advice. Knowledge widespread and should be pursued. In dungeons, you find books containing legends and clues. with people in cities or villages may provide valuable information. Leave no stone unturned. If you come across a book, see if its pages hold any valuable information. I personally sell many tomes that can aid you in overcoming the challenges of this world.<br><br>Additionally, if you come across any strange parchments, I can decipher them for you for a fee of 100 gold. Deciphering these peculiar riddles or letters can be challenging. These parchments often contain clues or riddles that lead to mysterious and valuable items. However, half of the time, they are false or misleading. I am not the only one capable of deciphering these parchments. A highly intelligent adventurer could also solve them. If you need further assistance, a gypsy may be able to shed some light on the matter.<br><br>If you are a wizard or necromancer seeking to increase your knowledge in ancient forms of magic, I can sell you a research pack to hold all your research materials. With this pack, you can study not only ancient forms of magic but also widely known spells that you may have trouble finding the original source for. Research requires you to search the world for ancient wizard tomes containing information to construct the spells. Additionally, you must become proficient in inscription to research and scribe the new magic you discover. If you wish to pursue this research, I offer a research pack for the price of 500 gold coins.<br><br>For those seeking a grand quest to uncover an artifact, I can provide guidance on your journey. However, my advice does not come cheap. The best advice I can offer requires a payment of 10,000 gold. To begin your quest, provide me with the necessary gold for my advice. I will provide you with an artifact encyclopedia, which will serve as your starting point for uncovering the whereabouts of your desired artifact. The accuracy of these encyclopedias varies depending on the amount of gold you are willing to spend.<br><br>Legend Lore:<br><br>Level 1 - 5,000 Gold<br>Level 2 - 6,000 Gold<br>Level 3 - 7,000 Gold<br>Level 4 - 8,000 Gold<br>Level 5 - 9,000 Gold<br>Level 6 - 10,000 Gold<br><br>Please note that we cannot provide absolute accurate information on the location of an artifact. However, the higher the lore level of the encyclopedia, the better your chances of finding it. Once you receive the encyclopedia, open it and choose an artifact from its pages. If you are unsure of the artifact you seek, you can browse through my inventory. At the end of my inventory, you will find research replicas of these artifacts priced at zero gold. Hover over these artifacts to see what they offer, although you cannot purchase them. Artifacts such as books, quivers, and instruments will be displayed with some common and random qualities. The actual artifact will possess slightly different properties. The remaining items have set qualities and a number of Enchantment Points that you can spend to customize the artifact for yourself. Once you find these artifacts, single click on them and select the Status option to allocate points to additional attributes. After selecting an artifact from the book, tear out the appropriate page and discard the rest. This page will provide you with your first clue on where to search. The artifact may be located in various lands or worlds, some of which you may have never encountered before. The page will provide you with the coordinates of the place you seek, so ensure that you have a sextant with you.<br><br>Throughout history, many people stored these artifacts on crafted stone blocks. These blocks are often adorned with a symbol on the surface, with a metal chest resting on top, potentially containing the artifact. Some treasure hunters find the chests empty, realizing that the legends were false. The likelihood of finding the artifact increases with the lore level of the book from which you tore the page. At the very least, you may discover a substantial sum of gold to cover some of your expenses on this journey. Certain discoveries may provide new clues about the artifact, which you can update in your notes. The most disappointing outcome is stumbling upon a fake artifact. These items resemble the artifact you were searching for but are ultimately useless.<br><br>These quests are extensive, and you can participate in one quest at a time. If you haven't completed a quest and attempt to seek advice for another, you will find that the page from your previous quest has gone missing. It would have been lost somewhere along the way. Once you finish a quest, whether successful or not, I will not have any new advice for you for quite some time. You will have to wait until then to embark on a new quest. Good luck on your treasure hunting endeavors, and may the gods assist you on your journey.";
            }
            else if (sConversation == "Bard")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + " and I am quite the musician. Bards not only entertain others in the local tavern; we also have mystical abilities with our music. We can cause others to become aggressive with each other, or we can play soothing music to calm them down. We can also play music so irritating that our opponents will often fumble in battle. One of the rarest forms of bardic performances is usually found in a bard's songbook. Although I might have an empty book for sale, the songs are something of a lost art. A few bards have found the notes and lyrics deep within dungeons, but those stories may simply be rumors. I do sometimes have a song or two for sale, but that is about it. I believe there were sixteen songs written over centuries, at least those with magical effects.<br><br>Bardic songs can be used just like wizards cast spells. You can call forth the magic from your book or use the song bars as explained by using the 'Help' button on the paperdoll. Unlike other types of magic, bardic songs do have a command you can type for each song. This command is something similar to '[KnightsMinne', and you can see which command goes with which song in your bardic songbook. Many of these songs' effectiveness is dependent on the bard's knowledge of music. The best results of these songs are performed by those who are skilled in musicianship, provocation, discordance, and peacemaking. You also need an instrument to play these songs. The bardic songbook will have you set your instrument of choice. If you lose the instrument, simply open the book and assign a new instrument. If you have an instrument with slayer properties, it will have greater effects on your enemies. Each time you play one of these songs, your instrument's uses will decrease by one, so be wary of your instrument's condition.<br><br>If you raise bees, you could probably make some wax that you can use on instruments. It helps them last longer. Sometimes I sell this type of wax as well.<br><br>Some instruments have mystical powers. If you are a skilled enough musician, you can equip these instruments or songbooks where you would normally place trinkets. This allows you to take advantage of the magical effects the items may have. If you decide to make an instrument with your carpentry skill, keep in mind that the better the wood and skill, the more uses you will get from the instrument.";
            }
            else if (sConversation == "Tanner")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and I can fix your leather gear for you if you wish to hire me. I can repair leather armor, bear masks, deer masks, pugilist gloves, and throwing gloves. Additionally, I occasionally sell taxidermy kits. If you are skilled in tailoring, you can purchase one of these kits, which will enable you to mount certain creatures as trophies. Once you obtain the taxidermy kit, examine the various trophies you can create. Remember to carry the taxidermy kit with you when hunting these creatures, as it is essential for obtaining the item required to craft the trophy. Wishing you the best of luck in your hunting endeavors!";
            }
            else if (sConversation == "Tavern")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and you have come to the right place if you don't want to travel alone. Henchmen are followers that can join you on adventures so you do not have to traverse the dangerous dungeons alone. You can purchase the services of a henchman from barkeeps, tavern keepers, or innkeepers. There are three different types of henchmen you can have: a fighter, archer, or wizard. These henchmen use a similar system for tamed animals, with a few exceptions. First, you can heal your henchmen with your healing skill. Second, you cannot transfer an active henchman to another player. Third, you cannot stable your henchmen. Lastly, you cannot be bonded to your henchmen. Each 'henchman item' represents the type of henchman you have (helms for fighters, crossbows for archers, or crystal balls for wizards). Although you cannot transfer your henchman, you can give the 'henchman item' to another person where they will then have possession of the henchman. Along those lines, if someone else manages to get your 'henchman item' from you, the henchman is then theirs.<br><br>You must be in an area such as an inn, tavern, or home to call your henchman. Once you call them, they will take possession of the 'henchman item' and keep it until one of the following occurs: they are killed, you release them, or they get annoyed with the lack of treasure being found. For every 5 gold you give them, they will travel with you for 1 minute. This equals to 300 gold per hour, where the maximum they will take from you is enough for 6 hours of adventuring. You can pay your henchman in a few different ways. You can give them many types of treasure like coins, gems, or rare items for payment. Rare items are those unique items you may find that you can give to merchants in towns for a high price. Each time you pay them, you will get a message indicating how many minutes they will be traveling with you. When they have about 5 minutes left, they will begin to express their annoyance for the lack of treasure. This is a warning to find some treasure quickly, or your henchman will leave. If your henchman does depart, the 'henchman item' will appear in your backpack. The next time you call upon your henchman, make sure you have something to give them so they will travel with you. A henchman always remembers how much treasure you have given them. This means if a henchman has about 4 hours left of travel, and you 'release' them, they will remember that they have 4 hours of travel when you call upon them again. Keep in mind that this 'adventuring time' does not count down when you are in an area like a tavern, home, inn, bank, or camping tent.<br><br>Each henchman will have a unique name, title, and clothing. If you do not care for your henchman's appearance, simply give the 'henchman item' to an innkeeper, tavern keeper, or barkeeper and they will exchange the henchman for another one. When you do this, the new henchman will retain any bandages and time remaining for adventuring the previous henchman had, as payments and bandages are transferred to the new henchman. As mentioned earlier, you do not stable henchmen. You instead 'release' them and their 'henchman item' will appear in your backpack and you can call the henchman later. You can release henchmen anywhere you are. If a henchman is slain, the 'henchman item' will appear in your backpack. The name of the 'henchman item' will indicate that the henchman is dead. You will have to seek out a healer and 'hire' them to resurrect your henchman. When you 'hire' a healer to do this, it will cost an amount of gold indicated on the item, and you must select the 'henchman item' when the targeting cursor comes up. The 'dead' indicator will vanish and you can then return to an area like an inn, tavern, bank, or home and call your henchman again.<br><br>If you ever mount a creature or magically enhance your travel speed, your henchman will mount a steed shortly after so they can keep up with you. Henchmen are only as able of an adventurer as you are. Their skill level is an average value of your total skills. Their stats are a distribution of your total non-magically-enhanced stats. So basically, the better you are, the better your henchmen will be. These henchmen only help you in your battles. They do not pick locks or remove traps. That is up to you to manage. You can give them bandages and they will use them as they need them to cure their poison or heal their wounds. If you ever want to know how many bandages they have left in their pack, simply say 'report' and they will tell you. You can give them potions though and they will drink them, giving you an empty bottle back. The potions they can make use of are heal, cure, rejuvenate, refresh, and mana potions. You are only able to take two henchmen with you at any one time.";
            }
            else if (sConversation == "Gypsy")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + ", and we have a reputation for having visions from the past or future. If you have a deciphered parchment and are wondering if the words written speak the truth, I can examine it and reveal whether it is true or false. I would ask for payment in gold based on the complexity of the coding. <br><br>These are the different levels of coding: <br>- plainly coded (100 gold)<br>- expertly coded (200 gold)<br>- adeptly coded (300 gold)<br>- cleverly coded (400 gold)<br>- deviously coded (500 gold)<br>- ingeniously coded (600 gold)<br>- diabolically coded (700 gold)<br><br>If you have found a simple note and are unsure if the words are true, I can use the tarot to determine if the message was written truthfully or falsely (100 gold).<br><br>If you need a legend verified, it requires great concentration and effort. I will review it and the cost will depend on the likelihood of the story.<br><br> You can choose from:<br>- highly unlikely (4000 gold)<br>- unlikely (3500 gold)<br>- somewhat unlikely (3000 gold)<br>- somewhat reliable (2500 gold)<br>- reliable (2000 gold)<br>- highly reliable (1500 gold)";
            }
            else if (sConversation == "MadGodPriest")
            {
                sText =
                    "So, "
                    + sYourName
                    + "...you have learned the true name of the Mad God. His disciples are growing every day. Soon, we will become a powerful voice to invoke him, and he will release us from this prison that Mangar has entangled us in. Those who truly worship Tarjan do so deep within the Catacombs. Take this key and seek enlightenment in the darkness below.";
            }
            else if (sConversation == "NecroGreeter")
            {
                sText =
                    "Hail, "
                    + sYourName
                    + "...this is the island of Dracula, the Master Vampire. He has turned this place into a sanctuary for his kind but does have patience for those who study the necrotic arts. Any necromancers who have achieved an apprentice level of skill, undertakers who have reached an adept level of skill, vile death knights of equal skill, or those strange Syth cultists can roam the island without fear of attack from Dracula's minions. They can also build sanctuaries for themselves here. However, be warned, trespassing into Dracula's castle is strictly prohibited.<br><br>For necromancers, undertakers, death knights, or Syth cultists wishing to build a stronghold on the island, there are many suitable locations. The nearby village of Ravendark is an option, but most residents only trust those who understand their craft and refuse entry to outsiders.<br><br>The Master does not concern himself with the slaying of his minions, and many necromancers, undertakers, death knights, or Syth cultists often do so to further their quest for power.<br><br>For other travelers, be cautious. The island is a dangerous place to navigate.";
            }
            else if (sConversation == "Kylearan")
            {
                sText =
                    "Mangar has trapped me in my own tower by unleashing his minions in my hallways. We were once friends, Mangar and I. Now, power has corrupted him to the point where companions serve no purpose.<br><br>Take this ebony key. It will not work on the door to Mangar's tower, but he did say that it would work on his secret door beneath the streets of Skara Brae. I also want you to have this small chest that has a relic inside. Choose one so it may help you on your quest. Go now, and may you vanquish Mangar and his evil void.";
            }
            else if (sConversation == "Assassin")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". You've stumbled upon our elusive yet exclusive guild of assassins. If you possess the spirit to join our ranks, I am the one you should approach. While I oversee the guild's operations, Xardok holds the esteemed position of high ruler. He resides on a secluded island in Sosaria, and if you wish to undertake any assignments for the guild, he is the one to approach. Additionally, I offer various items for sale that may aid assassins in their pursuit of eliminating targets. Should you find yourself entangled with the authorities due to a murder, you can wait for a period of 8 hours in real-world game time to see if the guards forget about the crime. However, any murder committed after that time frame will necessitate a waiting period of 40 hours in real-world game time to ensure the guards disregard a single instance of homicide.";
                if (Server.Misc.MyServerSettings.AllowBribes() >= 1000)
                {
                    sText =
                        sText
                        + " You could also hire me for "
                        + Server.Misc.MyServerSettings.AllowBribes()
                        + " gold to bribe the guards into forgetting about a murder you may have committed. Guild members only need to pay half of that amount, and I cannot persuade the guards to overlook fugitives. If you don't have enough gold in your pack, I can simply deduct it from your bank box.";
                }
            }
            else if (sConversation == "Xardok")
            {
                sText =
                    "Welcome to my island, "
                    + sYourName
                    + ". Some call this place a haven for the villainous, but I consider it merely a place to relax without the worries of the law. From the noblest of knights to the most murderous scoundrels, my castle welcomes anyone within its halls. There is even a thief in my southern tower, and they have a metal chest that can access one's bank box. Come here to practice, sell goods, or buy supplies. You can even rest here for the night.<br><br>Now you may have heard rumors that this is the home of the Assassins Guild. That may or may not be true. The Assassins Guild is a group of people who do things that others wish not to dirty their hands with. People go to them to have someone in particular put to death. Anyone may officially join the guild; they need only to seek an assassin guildmaster. Some believe they settle among the thieves in their guild, secretly looking for new members. Only the unsavory guild members are welcome into the fold when assassin work is sought, so those with good karma are usually kept at bay from such tasks. Members are given targets they must seek and slay. Each task must be completed to get another. If you fail at one task, the guild leader will not grant another unless atonement is given. The more famous an assassin, the better chance to get a high-priced victim. Of course, the more gold for a target usually means the target may be difficult to handle. Those who complete tasks for the guild often lose karma since society thinks what they do is despicable, but that is nothing a good assassin worries about.<br><br>To get a target assigned to you, one must simply ask the guild leader if they wish to 'hire' them. They would not send you to a land you have never been, but they may send you to any town or dungeon in lands you have traveled. If you do not know the location of a particular place, that is of no concern to the guild. You had better begin your exploration of such areas. Any other details of the victim can be read in the quest log (typing '[quests'). When such a task is completed, one would usually tell the guild leader that they are 'done'.<br><br>If one were tasked to kill a village citizen, the guild has been known to pay the right people to help others forget about the assassin's murder. Of course, such jobs are difficult as the assassin must escape the village to collect their reward and be absolved of the crime they were 'wrongly' accused of. When performing these tasks, it is of utmost importance that only the intended victim be assassinated. Anyone witnessing the deed should be quickly fled from as the guild cannot make a village forget about too many murders. The easiest way to dispatch of a troublesome individual is to catch them alone, without the eyes of the guards on you. If such a thing is not possible, some say that the most effective method is to poison some food or drink. Greater poison works best for these situations. Once you have some tainted food or drink, give it to your victim to ingest. Although many can tell the food and drink is tainted, the ones that are normally marked seem foolish at times. Make sure you can hide right away before those nearby know what has occurred. Make sure you can then sneak out of the area before you are caught. Don't wait for your mark to suffer the poison's effects. Just get out of there. If you used greater or deadly poison, they will more than likely perish in due time. If you are unable to move with great stealth or lack the magic to escape, then being an assassin may not be for you.<br><br>But in the end, all of this is just legend and hearsay.";
            }
            else if (sConversation == "XardokFail")
            {
                sText =
                    "What are you waiting for? You have been given your task, now carry it out with diligence! We did not choose a victim for you that is too difficult... we made sure of that. If you feel this victim is beyond your mettle, I will remind you that the guild cares little about what you think. If you wish to rid yourself of this contract, then you must pay the bounty offered as your atonement for failure. So whatever the reward is, you must put that total in the box of atonement... if you wish to slither away from what we ask of you, that is.";
            }
            else if (sConversation == "ScrapMetal")
            {
                sText =
                    "This barrel is for rusty scrap metal. If you come across any rusty metal items, put them in this barrel, and you will get 5 gold per stone's weight of rusty metal. The metalworkers remove the impurities and smelt it back into bars of iron. Any members of the Blacksmiths Guild will get 10 gold per stone's weight of rusty metal.";
            }
            else if (sConversation == "Aquarium")
            {
                sText =
                    "In this tub, you can observe a wide variety of fish. Typically, skilled fishermen capture exceptionally rare fish and place them in these tubs. The proprietor of this establishment generously compensates for such specimens. Although they are challenging to capture, the more proficient the fisherman, the greater the likelihood of securing a fish that commands a high price. Members of the Mariners Guild are entitled to double the amount of gold for the fish they successfully catch.";
            }
            else if (sConversation == "TrailMap")
            {
                sText =
                    "This is a trail map that reveals a hidden route leading to the depicted location. To utilize these maps, a minimum skill level of 80 in either tracking or cartography is required. They cannot be duplicated and eventually deteriorate from frequent use. By double-clicking the map while it is in your possession, you will embark on the path it outlines, embarking on a solo journey. No one else can accompany you on this swift expedition. However, if you place the map down and double-click it, others can join you on the journey by double-clicking the map left behind. The original map will return to your pack, while the abandoned map will only linger for approximately 30 seconds, necessitating swift action from your companions. If a map leads to an uncharted realm, one you have yet to explore independently, you will discard the map as the route to this location eludes you.";
            }
            else if (sConversation == "CampTent")
            {
                sText =
                    "This is a camping tent that can provide you with a safe refuge from the dangers of the land and offer a place to rest. To use these tents, you need to have a camping skill of at least 40, and please note that they will eventually wear out with use. <BR><BR>To set up the tent for yourself, simply double click on it while it is in your pack. Once inside the tent, no one will be able to follow you unless they have their own tent and possess the necessary skill. However, if you place the tent on the ground and double click it, others will be able to join you inside. <BR><BR>The original rolled-up tent will return to your pack, while the set-up tent will remain on the ground for approximately 30 seconds. It's important for your comrades to act quickly and follow you inside during this time. <BR><BR>If anyone wishes to leave the tent, they can simply double click on the tent flap they entered through. While anyone can stay in the tent for as long as they desire, they will return to the exact spot where they initially used the tent when they decide to leave.";
            }
            else if (sConversation == "DeathKnight")
            {
                sText =
                    "If you are following a path of chaos set forth by the Bloody Handed, it is crucial to instill fear in those who dare to oppose the darkness. Once you have earned a reputation for being despicable and have attained the rank of journeyman death knight, step into the pentagram and utter the name of the Bloody Handed. This ritual will transform you into the embodiment of nightmares, causing men to flee and children to perish at the mere thought of you.<br><br>Should you ascend to the esteemed rank of grandmaster death knight and prove yourself through heinous deeds, I have the ability to summon a dread horse for you. All I ask in return is a tribute of 10,000 gold.<br><br>Lastly, if you desire the gift of demonic wings, I possess the power to transform any cloak you possess into such a form. However, it is important to note that these wings do not grant the ability to fly. The cost for this transformation is 35,000 gold, although your karma will influence the final price. For those of the most wicked nature, the price may be reduced to 20,000 gold, while those of pure heart may be required to pay 50,000 gold. Should you find yourself dissatisfied with the wings, you may return them to me and I will transform them back into a cloak at no additional cost.";
            }
            else if (sConversation == "MerchantCrate")
            {
                sText =
                    "Merchant crates are items that craftsmen can use to store and sell the goods they make in their homes. Every day, a representative from the Merchants Guild will collect any items left in the crate and leave gold as payment for the owner's hard work. When you place an item in the crate, you will be aware of its gold value. However, craftsmen can only receive gold for armor, weapons, or clothing that they have crafted themselves. Non-crafted armor, weapons, or clothing will not have any value. The value of crafted armor and weapons will vary depending on the resources used and factors such as durability and quality. Most crafted items will have some value to the Merchants Guild. Other items like potions, scrolls, tools, furniture, or food can also be sold for a price. However, tools must have at least 50 uses to be considered valuable. If an item cannot be crafted, it is unlikely to fetch any gold. There is an exception for ingots and logs, as they are highly sought after in the land and have varying values based on their type.<BR><BR>To withdraw the gold from the crate, simply click on it and select the 'Transfer' option. Although the crate indicates the gold value it holds, the actual amount received may be higher if the person withdrawing the gold is a member of the Merchants Guild or has a high Mercantile skill. It is important to lock down these crates in a home for them to be of any use.";
            }
            else if (sConversation == "Patch")
            {
                sText =
                    "As new additions are made to the world, it may be necessary to update your client to accommodate these changes. This bulletin board will display the most recent date on which a patch was prepared. This information will help you determine whether or not you have already installed the patch. If you downloaded the client on or after the displayed date, the patch is not necessary. If you do need to download it, simply click on the bulletin board and select 'Open'. Your web browser will open, allowing you to obtain the patch. Once downloaded, unzip the contents into your UO directory, replacing any existing files. It is important to ensure that your client is closed before proceeding with this update.";
            }
            else if (sConversation == "GodOfLegends")
            {
                sText =
                    "You stand within the Hall of Legends, "
                    + sYourName
                    + ". This is where the most notable champions are laid to rest. Some were the most lawful of heroes, while others may have been the greatest villains of the land. They all share one common trait: bravery in the face of danger and the slaying of powerful creatures that roam the lands. They have killed great dragons, mighty demons, and powerful undead. Their names are known throughout the land, and their history is forever remembered.<BR><BR>Legends were born from their bravery, and items were forged and wielded by them. From Elric's sword of Stormbringer to Merlin's mighty staff these legendary artifacts were sometimes lost stolen, hidden, or found by others. Although these items were once mundane, these champions used them in many battles. Those victories made these items into legendary artifacts.<BR><BR>It is not your time to lay among the mighty, but you could begin your journey with an item of power forged in your name. I have placed a book on the pedestal. If you have 15,000 points of fame, 15,000 points of karma (either toward good or evil), and 10,000 gold for tribute, then you may select an item from the book. Although I will create the item for you, it will appear as something simply sold by merchants. As long as you are equipped with the item, it will grow in power as you achieve victory over the many fearsome foes of the lands.<BR><BR>Upon selecting an item, your fame and karma will reduce accordingly. It is up to you to rebuild them. You can seek as many legendary artifacts as you are able to achieve. They are very special compared to regular items of common appearance. Legendary artifacts will never need to be repaired. If you meet an untimely end, you will still have it in your possession when you return to the living. Certain traps that affect equipped items will have no adverse effects on legendary artifacts. Creatures that attempt to ruin legendary artifacts will fail in their attempt. If you are careless with your artifact and leave it lying about, then fate will decide what may happen to it. Your item will gain levels as you equip it and gain victory over your adversaries. When the item gains a level, simply click on it and select 'Status' to give the item more power. Be careful when adding power to items, as you cannot change any attributes once you select them. You can use regular dye tubs on legendary items, making them any color you choose. If you want to rename your artifact, show me that you have been gifted with one by handing it to me. I will return your artifact, along with a branding iron you may use to mark your artifact with any name you choose.";
            }
            else if (sConversation == "Farmer")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". Welcome to my market. I have grown everything here, and you might find some food that could help you on a long journey. Many farms grow things like corn, wheat, and turnips. We simply take them from the garden when they have fully grown. There are also fruits available. These can be found throughout the land, and you can use a bladed item to pluck the fruit from apple, peach, or pear trees. If you happen to find yourself on a rare tropical island, you might also be able to obtain bananas, dates, or coconuts in the same fashion. Farming may require more work, but it is safer than attempting to punch a bear for meat.";
            }
            else if (sConversation == "Courier")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". I occasionally deliver mail on behalf of prominent individuals who are seeking a brave adventurer to undertake a specific task. We journey from village to city, city to village, searching for the intended recipients. These messages typically instruct the recipient to retrieve a rare item from a place that others are afraid to visit. If you receive such a message, ensure that you are capable of fulfilling the requested task. Failure to do so may result in a lack of future messages being delivered to you.<br><br>You will be provided with the coordinates of the desired location, so it is essential to have a sextant and the mailed message in your possession. Without the mailed message, it is highly unlikely that you will ever locate the item.<br><br>The sought-after items will be stored on blocks of intricately crafted stone. These stones are often adorned with symbols on their surfaces, where a metal chest rests, potentially containing the desired item. These quests are conducted in a 'virtual' manner. This means that any achievements you accomplish are genuine, but any references to the items you find are not. If your quest message states that you have obtained the item, it will not physically appear in your backpack. However, you will possess it in a virtual sense. The mailed message will keep track of this fact for you. Consequently, you will never lose the item, and it will remain unique to your specific quest message. Once you have retrieved the item, present the mailed message to the sender to claim your reward.";
            }
            else if (sConversation == "Painter")
            {
                sText =
                    "Hail, "
                    + sYourName
                    + ". As you can see, I am a painter with extensive experience in creating portraits. Although I am currently out of canvases, I am more than willing to paint a portrait of you if you happen to acquire one. Rest assured, I have a variety of painting styles to choose from. My fee for this service is 5,000 gold, but I guarantee your satisfaction with the final result. Occasionally, tailors may have canvases available, although they rarely sell them. If you wish to make any changes to the portrait, simply return it to me and I will gladly make the necessary adjustments free of charge. Furthermore, even if you acquire a new title, I can incorporate it into a previously painted portrait if you provide it to me. Ultimately, my aim is to ensure the happiness of my customers. However, if you possess the skill to work with wax, my services may not be necessary. Now, in the event that you come across any rare paintings during your travels, I am prepared to pay double what an art collector would offer. This is due to the fact that these collectors tend to hoard rare art, thus requiring me to provide a greater incentive in order to obtain such pieces.";
            }
            else if (sConversation == "Trophy")
            {
                sText =
                    "This is a wooden baseboard for mounting fish or animal heads. These mounts can be used as decorative items for your home. To use, simply click once on the board and select 'Use'. Then, target the corpse of a creature or fish. If it can be mounted, you will create a trophy to take with you. Any creatures you mount will be engraved with information about who killed the creature and where it was slain. Please note that not every creature's head can be mounted. <br><br>Below is a list of creatures whose heads can be mounted:<br><br>Balrons<br>Bears<br>Bugbears<br>Cerberus<br>Chimeras<br>Cyclops<br>Daemons<br>Deer<br>Demons<br>Devils<br>Dragon Turtles<br>Dragons<br>Drakes<br>Ettins<br>Fiends<br>Flesh Golems<br>Gargoyles<br>Giants<br>Goblins<br>Gorillas<br>Griffons<br>Hell Beasts<br>Hippogriffs<br>Hydras<br>Lions<br>Lizardmen<br>Manticores<br>Meglasaurs<br>Minotaurs<br>Nighmares<br>Ogres<br>Orcs<br>Owlbears<br>Pixies<br>Ratmen<br>Stegosaurus<br>Styguanas<br>Teradactyls<br>Tigers<br>Titans<br>Trollbears<br>Trolls<br>Tyranasaurs<br>Unicorns<br>Walrus<br>Watchers<br>Wyverns<br>Yetis<br>";
            }
            else if (sConversation == "Stonecrafter")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". My name is "
                    + sMyName
                    + " and I am a stone crafter who sells tomes that teach mining and stone manipulation. However, to purchase these tomes, you must have achieved the grandmaster title in mining or carpentry, respectively. The stone you find comes in various colors, similar to the types of ore you would discover. Any creations made from a specific stone will reflect its color. You have the option to use or sell what you create, or place it in a merchant crate for trading by merchant guilds. Altering the color of crafted items can be done using a furniture dye tub or other methods you may discover. Homeowner tools allow you to change the direction that most items face. If you have a larger block of stone for a statue or piece of furniture, you can set it on the floor of your home and use the option to flip a deed, which will change the direction. Afterwards, you can construct the statue or furniture within your home. Most items, excluding home-built blocks, can have their names changed by double-clicking and providing a new name. This feature is particularly useful for naming carved statues or tombstones.";
            }
            else if (sConversation == "Stake")
            {
                sText =
                    "You have discovered a wooden mallet and stake. If you successfully slay a vampire, you can simply click on this item and utilize it to drive the stake into the heart of the creature. This action will grant you credit for such kills. Once you have accumulated a value of at least 1,000, take the mallet and stake to either the priest in Britain or the City of Lodoria, where they will reward you with the desired gold. If your aspiration is to become a priest yourself, you will need proficiency in healing and spiritualism, as well as a minimum of 2,500 karma. After earning 1,000 gold using this mallet and stake, present it to the priest, and they will guide you towards the path of divinity.";
            }
            else if (sConversation == "Devon")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". I am "
                    + sMyName
                    + " and I have been fishing these waters for many years. There isn't much competition in these waters, as there are only a few of us who have dared to venture the Underworld. I have seen some odd creatures, though, and have heard even stranger stories from others. I have built a few ships for adventurers, even though it is mostly petrified wood this deep under the surface. I may have one that interests you. Unlike the surface seas, launching a ship here is much easier, and thus we never really need to rely on a dockmaster for aid. This huge cavern seems to summon a large enough breeze to push the sails, so you shouldn't have troubles navigating the waters. Be leery of the lake, as legends tell of the Titan of Water lurking within the murky depths.<br><br>If you get lost within this cavern, you can use a magic sextant to find your way. It may seem silly as there is no sun here, but I do have magic sextants that can see through the cavern ceiling when gazed through. Keep that in mind if you are searching for something as well.";
            }
            else if (sConversation == "Pets")
            {
                sText =
                    "Greetings, "
                    + sYourName
                    + ". I am "
                    + sMyName
                    + " and I have worked with animals for many years. Taming is quite a feat that only a few can accomplish. If you have pets, we can provide stable services for them if you wish. If you possess expert camping skills, you can acquire a hitching post and use it in your home to stable your own pets.<br><br>The number of animal companions you can have with you is determined by your skill with animals. Having a good herding skill can help you manage more pets.<br><br>If you are in need of a pack animal, I may be able to assist you. Many adventurers accumulate a great deal of treasure on their journeys and often lament the amount they must leave behind due to the inability to carry it. A pack horse can be of great help as they are inexpensive. However, many of these animals have been slain in dangerous areas, so it is important to be cautious and protect them. On occasion, we are able to raise a sturdy pack mule, which is more resilient than other pack animals. They require more control to bring with you, but they can carry a significant amount and monsters tend to leave them alone for some reason. Due to their popularity among adventurers, they cost more gold, but the investment may be well worth it. I have also heard tales of tamers training elephants and dinosaurs to transport their goods, although I have yet to witness it.";

                if (Server.Misc.MyServerSettings.NoMountsInCertainRegions())
                {
                    sText =
                        sText
                        + "<br><br>When exploring the land, be aware that there are certain areas where riding mounts is prohibited. These areas include dungeons, caves, and certain indoor locations. If you are riding an ethereal mount, it will automatically be moved to your backpack when you enter these areas. You can then use the ethereal mount again when you leave the restricted area. If you are riding a regular animal, it will run off to a safe place upon entering these areas and will reappear when you exit, allowing you to continue riding.<br><br>If you find that you cannot locate your riding animal for any reason, be sure to check the stables as it may have wandered there. If you have an animal that is not allowed in these restricted areas but still want it to accompany you, simply leave the area and dismount the animal before proceeding. <br><br>It is important to note that if you are without your mount for an extended period of time, it will eventually return to the stables. In this case, the stablemaster will withdraw gold from your bank account on a weekly basis. If you have no gold in your account, your mount will be released into the wild.";
                }
            }
            else if (sConversation == "Powerscroll")
            {
                sText =
                    "Hail, "
                    + sYourName
                    + ". I am "
                    + sMyName
                    + " and I am one of the teachers of higher knowledge here in Lodoria. Since we elves can live for hundreds of years, we have learned much more about certain skills than you humans. Although we teach many elves here within these castle walls, you would need to embark on a journey to the shrines of enlightenment if you wish to pursue such things. If you are willing to part with the gold required for our acquired knowledge, then you must take the scroll to the lost land of Ambrosia. Seek the appropriate shrine of enlightenment and read the scroll there.<br><br>Wondrous: Up to 105 Skill<br>Exalted: Up to 110 Skill<br>Mythical: Up to 115 Skill<br>Legendary: Up to 120 Skill<br>Power: Up to 125 Skill<br><br>Ambrosia could once only be reached by ship, but since the defeat of Exodus, a portal is said to have opened on his island that is supposed to lead there. Of course, these are only rumors, but it may be a good place for you to start. Good luck, "
                    + sYourName
                    + ".";
            }
            else if (sConversation == "Frankenstein")
            {
                sText =
                    "Hail, "
                    + sYourName
                    + ". I am "
                    + sMyName
                    + " and I am the undertaker in our guild, specializing in the study of the revival of life rather than the aspects of death like my fellow necromancers. Throughout my life, I have dedicated myself to unraveling the secrets of Victor Frankenstein. Regrettably, I have yet to come across any of his journals that could further my research. I once had an acquaintance who was assisting me in my endeavors, and he did manage to find some texts on the subject. Unfortunately, his ship was lost at sea, and he was never seen again. Perhaps it was for the best, as his findings seemed to only scratch the surface of reanimated corpses.<br><br>If, by chance, you come across any of Victor Frankenstein's journals, I would be greatly interested in acquiring them from you. Naturally, you are welcome to attempt following his instructions yourself. With your forensic skills, you may have better luck than I. It is believed that reanimated corpses can either become formidable combatants or powerful slaves, depending on the strength of the brain used. For instance, a brain from an ogre would not be as effective as one from a storm giant. To acquire the necessary body parts, you must bring one of Frankenstein's journals with you and use a blade on the corpses of giants. Your forensic skills will greatly determine the quality of the body parts you gather to construct a creature. Look for severed limbs, heads, and torsos, as those are the ones you need. Additionally, you must find a fresh brain, as the others are usually decayed and devoid of life. Once you have gathered all the necessary components, you will require a power coil, similar to the one I possess in my lab. Following the instructions in the journal, you can use the power coil to animate the corpse. Feel free to utilize my power coil, but I also obtain them from a local tinker, so you may purchase one from me if you wish to have it in your own home.";
            }
            else if (sConversation == "Jester")
            {
                sText = Server.Items.BagOfTricks.JesterSpeech();
            }
            else if (sConversation == "Enhance")
            {
                sText =
                    "This tool is exclusively available to members of a specific crafting guild and can only be used by them. By using this tool on an item, its capabilities can be enhanced. These items can only be used by master craftsmen and require a certain amount of gold to add enhancements. If used on an item that you have crafted and bears your mark, the cost of adding enhancements will be significantly reduced. The tool can only be used when near your guildmaster or a shop in your home that is relevant to the skill you are utilizing. There are also rumors suggesting that these items can be used in the Enchanted Pass, where elves are known for crafting extraordinary items. However, an item can only be enhanced up to a certain level of power, providing an opportunity to create truly mystical items.";
            }
            else if (sConversation == "EnhanceJewels")
            {
                sText =
                    "This is a special tool that is exclusively available to members of a particular crafting guild. It can only be utilized by these members. Simply apply this tool to an item in order to enhance its capabilities. These items can only be utilized by master craftsmen, and adding enhancements requires a specific amount of gold. You can only use this item if you are in proximity to your guildmaster or a shoppe in your home that is relevant to the skill you are employing. There are also rumors suggesting that these items can be used in the Enchanted Pass, a place where elves are renowned for crafting extraordinary items. An item can only be enhanced up to a certain level of power, but it should provide you with the means to create truly mystical items.";
            }
            else if (sConversation == "Jedi")
            {
                sText =
                    "Greetings, child. It has been quite a long time since I last spoke to someone. In case you're wondering, I am a descendant of the Jedi, a group that was stranded on this world long ago. Despite lacking a means to leave, we had a mission to eliminate the Sith who arrived around the same time. As the Sith grew in numbers, so did the Jedi. While we gradually addressed the Sith threat, many of our members integrated into society and became known as priests. They offered healing to the sick and imparted teachings on virtuous behavior to those who would listen. If you possess good character (positive karma) and possess a psychic ability (psychology of 25 or more), you are welcome to pursue the path of the Jedi. All I ask is for a demonstration of commitment. I am aware of a family in dire need, and I wish to provide them with 5,000 gold to start afresh. If you are prepared for enlightenment and are willing to part with the gold, please hand it over to me. In return, I will bestow upon you the wisdom needed to embark on your journey along the path of the Jedi.";
            }
            return sText;
        }
    }
}

/* TO ADD CONVERSATIONS TO NPCS, INCLUDE THE LINES OF CODE BELOW...

using Server.Misc;
using Server.ContextMenus;
using Server.Gumps;

        ///////////////////////////////////////////////////////////////////////////
        public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
        {
            base.GetContextMenuEntries( from, list );
            list.Add( new SpeechGumpEntry( from, this ) );
        }

        public class SpeechGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;
            
            public SpeechGumpEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
              if( !( m_Mobile is PlayerMobile ) )
                return;
                
                PlayerMobile mobile = (PlayerMobile) m_Mobile;
                {
                    if ( ! mobile.HasGump( typeof( SpeechGump ) ) )
                    {
                        mobile.SendGump(new SpeechGump( mobile, "Camping Safely", SpeechFunctions.SpeechText( m_Giver, m_Mobile, "Ranger" ) ));
                    }
                }
      }
    }
        ///////////////////////////////////////////////////////////////////////////

*/
