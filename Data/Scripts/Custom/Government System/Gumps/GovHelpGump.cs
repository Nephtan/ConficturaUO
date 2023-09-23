using System;
using Server;
using Server.Gumps;

namespace Server.Gumps
{
    public class GovHelpGump : Gump
    {
        public GovHelpGump()
            : base(50, 50)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(17, 23, 489, 323, 5120);
            AddImageTiled(22, 58, 483, 10, 5121);
            AddHtml(23, 29, 479, 22, @"<BASEFONT COLOR=WHITE><CENTER>Player Government System Help System</CENTER></BASEFONT>", (bool)false, (bool)false);
            AddImageTiled(158, 60, 6, 283, 5123);
            AddButton(25, 65, 4005, 4006, 1, GumpButtonType.Page, 1);
            AddButton(25, 90, 4005, 4006, 2, GumpButtonType.Page, 2);
            AddButton(25, 115, 4005, 4006, 3, GumpButtonType.Page, 3);
            AddButton(25, 140, 4005, 4006, 4, GumpButtonType.Page, 4);
            AddButton(25, 165, 4005, 4006, 5, GumpButtonType.Page, 5);
            AddButton(25, 190, 4005, 4006, 6, GumpButtonType.Page, 6);
            AddButton(25, 215, 4005, 4006, 7, GumpButtonType.Page, 7);
            AddButton(25, 240, 4005, 4006, 8, GumpButtonType.Page, 8);
            AddButton(25, 265, 4005, 4006, 9, GumpButtonType.Page, 9);
            AddButton(25, 290, 4005, 4006, 10, GumpButtonType.Page, 10);
            AddButton(25, 315, 4005, 4006, 11, GumpButtonType.Page, 11);
            AddLabel(60, 65, 1149, @"Starting Off");
            AddLabel(60, 90, 1149, @"Elections");
            AddLabel(60, 115, 1149, @"Taxes");
            AddLabel(60, 140, 1149, @"Treasury");
            AddLabel(60, 165, 1149, @"City Growth");
            AddLabel(60, 190, 1149, @"Maintenance");
            AddLabel(60, 215, 1149, @"Waring");
            AddLabel(60, 240, 1149, @"Allegiances");
            AddLabel(60, 265, 1149, @"City Commands");
            AddLabel(60, 290, 1149, @"Misc.");
            AddLabel(60, 315, 1149, @"Credits");
            AddPage(1);
            AddHtml(166, 63, 331, 275, String.Format("<CENTER><U>Starting Off</U></CENTER><BR>So, you want to become a mayor? There are several things you need to understand before you start. First, obtain a city hall deed. You can purchase one from the city manager in town. This building will serve as the hub for city management and voting. Next, select an ideal location for your city. Choose an expansive area so others can position their homes nearby, allowing your city to flourish. The chosen location must also be at least {0} yards (tiles) away from any other city or guarded area. Make sure you're content with your location because once the hall is set up, you cannot reclaim its deed.<BR>With the city hall in place, it's time to gather citizens. You must recruit {1} citizens within 24 hours; otherwise, your city will be deleted. Since all the characters from your account can join the city, consider inviting a few close friends. If they join with all their characters, you should meet the requirement. Additionally, contribute to your city's treasury. Each time your city updates, it incurs a maintenance fee. If your city lacks the funds, it will be deleted. Access the maintenance report through the city management stone.<BR>The more citizens you have, the more your city thrives. Every {2} days, your city updates. With a sufficient number of citizens, your city will advance to the next level.", (PlayerGovernmentSystem.CityRangeOffset * 2).ToString(), PlayerGovernmentSystem.Level1.ToString(), PlayerGovernmentSystem.CityUpdate.Days.ToString()), true, true);
            AddPage(2);
            AddHtml(166, 63, 331, 275, String.Format("<CENTER><U>Elections</U></CENTER><BR>Every {0} days, your city will hold an election. There can only be two running mates per election, and citizens vote for the mayor they wish to see in office. Each citizen can vote only once per election.<BR>If there is just one running mate, the city election is considered null and void, and the current mayor will remain in office for that term. It's always beneficial to run for mayor and earn the citizens' votes. If the votes are tied, the election is null and void, and the current mayor remains in office for that term.<BR>Remember, as mayor, you can be voted out, and the succeeding mayor will inherit the powers you once had. Therefore, refrain from decorating the city with your favorite weapons and rare items because, if you're voted out, the next mayor assumes control over the city's decorations.<BR>By keeping the people happy and treating them well, you should be a successful mayor and stay in office for a long time. It's wise to regularly check the voting stone to see who is running against you.", PlayerGovernmentSystem.VoteUpdate.Days.ToString()), true, true);
            AddPage(3);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>Taxes</U><BR>Taxes are a crucial element of any city. Although they might be unpopular, they're essential for the city to function and sustain itself. Just as in real life, taxes are used to enhance the town – or at least that's the hope. A significant amount of money flows into a city. There are maintenance charges, and civic buildings require money or resources. As your city grows, its upkeep costs increase. If there's insufficient money in the treasury, the city could vanish. Once your city reaches the point of levying taxes, it's advisable to start with a minimal amount. Citizens might be reluctant to pay taxes, but as the mayor, it's your responsibility to explain the utilization of these funds. It's also your duty to spend the money judiciously - or perhaps on that new katana you've been eyeing. As a mayor, you can withdraw funds from the treasury at will. However, be cautious: every online citizen is notified of such actions. If they're vigilant citizens, they'll want accountability for their tax money.<BR>During the City Update (tax time), funds are debited from both your and your citizens' bank accounts. Yes, even the mayor pays. If a citizen doesn't have sufficient gold in their bank, a check occurs every time they log in. This check deducts any available funds towards their overdue taxes or takes the entire amount if they can cover it. There's no opting out; it's part of city life.<BR>Overcharging can deter citizens from residing in your city. However, as mayor, you have the discretion to set tax rates, ranging from 0 (no taxes) to 10,000 weekly. There are three tax types: Income, Property, and Travel. Property taxes are deducted from citizens' bank accounts weekly, whereas income comes from their City Player Vendors. Travel taxes apply only when someone uses a civic moongate, either entering or leaving your city. This structure lets you impose fees on non-city members using your public moongates. If someone uses a public moongate outside your city to enter, they're also charged.<BR>Undercharging can deplete your funds in maintenance, potentially resulting in the loss of your city. The onus is on you, as mayor, to determine the appropriate tax rate for your city.", true, true);
            AddPage(4);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>Treasury</U><BR>The treasury is the lifeline of your city, functioning as its bank account for gold. All taxes are channeled to the treasury weekly. Players can deposit gold into the treasury whenever they choose. The mayor has the privilege to deposit and withdraw at will. Primarily, the funds in the treasury are used for city maintenance. However, the mayor has the discretion to use it for any purpose, be it for city needs or personal desires. Insufficient funds can lead to the city's deletion. Each week, during maintenance calculations, the city draws the required amount. If the treasury is lacking, the city, along with its civic structures, will be deleted. Any locked-down items (excluding those in player homes) become available for public acquisition, and the mayor loses the right to reclaim deeds for civic buildings.<BR>It's crucial to monitor both the treasury balance and the maintenance report, gauging the required funds against the treasury's holdings.", true, true);
            AddPage(5);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>City Growth</U></CENTER><BR>During the update period, your city will check and update itself if needed. If you have the required players for the next city level, your city will advance, unlocking the associated features. This also increases city lockdowns, and the city's boundaries will expand. A city can span up to 250x250 from the city hall's location, offering a vast open area. Each level has a distinct city rank. For example, if your city has many citizens, newcomers might receive a message like 'You have entered the empire of Arwen's city'. However, attracting residents is crucial. Simply owning a house within the city doesn't grant citizenship; one must join via the city stone. Otherwise, these residents occupy space without contributing, living tax-free. Additionally, you can't ban house owners in the city. It's advisable to manage city housing permissions carefully, allowing house placements only when desired.<BR>As the city develops, you can introduce features like hiring guards and city registration. Registered cities with a moongate appear on the public moongate city list, attracting visitors to explore the city's vendors and amenities. A well-established shopping center can be a magnet, drawing people in. And if your citizens prosper, the city's treasury grows.", true, true);
            AddPage(6);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>Maintenance</U><BR>Maintenance is crucial for your city. It ensures the city's operations and is charged based on the civic buildings and features enabled. Review the maintenance report for a detailed cost breakdown. Taxes are instrumental in sustaining the city. Being a mayor is challenging but can be rewarding and costly. However, with proper management, it becomes simpler.<BR><BR>To minimize costs, it's vital to strategize. If your city is near a popular spot, like a dungeon or hunting site, consider installing a moongate and imposing a travel tax to increase revenue. Well-stocked vendors also justify a moongate. Proximity to a dungeon? Consider adding a healer so players can resurrect in your city for a fee. This can be a significant revenue source, especially near perilous sites. Although these amenities increase maintenance, they can be profitable. For instance, a bank might not generate income but could attract traffic for other ventures. Stables can also be a revenue source, with players paying 30 gold per pet.", (bool)true, (bool)true);
            AddPage(7);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>Warring</U><BR>As with anything in life, you will encounter enemies. You can engage in war with other cities via the 'War Dept.' menu. Here, you can declare wars, view and retract your declarations, see and accept war invitations to your city, cancel wars with other cities, and check the list of cities your city is at war with.<BR>Once a city declares war, it's akin to two guilds being at war. The citizens of the city you're at war with will appear orange to your city's citizens on all facets.", (bool)true, (bool)true);
            AddPage(8);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>Allegiances</U><BR>You can also use the 'War Dept.' menu to become allies with other cities. This works the same way as if you were to war with another city. Instead of orange, your allied city's citizens will appear green. Just like in guilds, all parties can attack each other, but it's considered friendly fire.", (bool)true, (bool)true);
            AddPage(9);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>City Commands</U></CENTER><BR><B>I wish to lock this down</B><BR>This command will secure an item inside the town or any civic building. You can secure any item you wish. However, unlike in a house, items inside containers won't be secured, and you cannot secure them inside the bag. City decor is for display only, not for storage.<BR><BR><B>I wish to release this</B><BR>This command will release any item you have secured in the city. It works anywhere in the city, except inside player houses.<BR><BR><B>I ban thee</B><BR>This command will ban a person from your city. However, you cannot ban a member of the city or someone who owns a house within city limits. Once a player is banned, they appear orange to all city citizens but only while inside the city.", true, true);
            AddPage(10);
            AddHtml(166, 63, 331, 275, @"<CENTER><U>Misc.</U><BR>Location is key for any city. You're going to want an area that's a high-traffic spot for your city to grow. No one wants to live in the middle of nowhere, and no one will visit a city located there. It's beneficial to be near a dungeon or a popular fighting area. Including vendors in your city is also advantageous. Everyone loves to shop. If you boast the best vendor malls, it's sure to attract visitors to your city.<BR>If you have any other questions, please ask your staff.", true, true);
            AddPage(11);
            AddLabel(170, 66, 1149, @"Concept by. RoninGT A.K.A. Paige");
            AddLabel(170, 93, 1149, @"Coded by Avelyn");
            AddLabel(170, 230, 1149, @"Built On: RunUO v2.2");
            AddLabel(170, 250, 1149, @"Verison: 2.23");


        }
    }
}
