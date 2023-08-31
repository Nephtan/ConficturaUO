/* ****************************************************
SkillStone.cs
Created By: Poolmanjim
Last Updated: 07/11/2021
VersioN: 0.9.2 Beta
GitHub Link: https://github.com/poolmanjim/SkillStone

DESCRIPTION
For ServUO - Skill Stone can be used to add to a player's current skills.

INSTALLATION INSTRUCTIONS
1. Copy SkillStone.cs into your ServUO scripts folder.
2. In game use [add skillstone to add the skill stone.

CUSTOMIZATION INSTRUCTIONS
1. Locate "public SkillStone() : base(0x1870)" in the script.
2. Modify the lines for SkillPoints and SkillMaxLevel in the code block to be the values you want.
3. Save the script.

LICENSE
MIT License (see below). However, here are my desires beyond what the license states.
1. Leave the credit section above in tact.
2. Redistribute at will.
3. By license, you are allowed to sell this. I ask kindly, that you avoid doing that. I want this to build the community, not one person.
4. Do not intentionally include this in a world-ending AI.

Copyright (c) 2021 Poolmanjim

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*********
CHANGE LOG
    v0.9.2
        Fixed bug causing crash if the stone was used, server rebooted, and it was used again.

**************************************************** */
using System;
using System.Collections.Generic;

using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class SkillStone : Item
    {
        // Total number of available skill points.
        private int m_SkillPoints;

        // Max any given skill can be set to using the stone.
        private double m_SkillMaxLevel;

        // Player who is assigned to this stone.
        private PlayerMobile m_AssignedPlayer;

        // Property for accessing and setting the m_SkillPoints field.
        [CommandProperty(AccessLevel.GameMaster)]
        public int SkillPoints
        {
            get { return m_SkillPoints; }
            set { m_SkillPoints = value; }
        }

        // Property for accessing and setting the m_SkillMaxLevel field.
        [CommandProperty(AccessLevel.GameMaster)]
        public double SkillMaxLevel
        {
            get { return m_SkillMaxLevel; }
            set { m_SkillMaxLevel = value; }
        }

        // Property for accessing and setting the m_AssignedPlayer field.
        [CommandProperty(AccessLevel.GameMaster)]
        public PlayerMobile AssignedPlayer
        {
            get { return m_AssignedPlayer; }
            set { m_AssignedPlayer = value; }
        }

        [Constructable]
        public SkillStone()
            : base(0x1870)
        {
            // Initialize fields and set default values for inherited properties.
            Name = "Skill Stone";
            Hue = 2704;
            LootType = LootType.Blessed;
            Movable = false;
            // Set default number of available skill points (1 point = 0.1 skill).
            SkillPoints = 1500;
            // Set default maximum value for skills set using the stone.
            SkillMaxLevel = 50;
        }

        public SkillStone(int skillPoints, double skillMaxLevel)
            : base(0x1870)
        {
            // Initialize fields and set default values for inherited properties.
            Name = "Skill Stone";
            Hue = 2704;
            LootType = LootType.Blessed;
            Movable = false;
            // Set number of available skill points to the value of the skillPoints argument (1 point = 0.1 skill).
            SkillPoints = skillPoints;
            // Set maximum value for skills set using the stone to the value of the skillMaxLevel argument.
            SkillMaxLevel = skillMaxLevel;
        }

        public override void OnDoubleClick(Mobile from)
        {
            // Close any existing SetSkillsGump gumps for the player.
            from.CloseGump(typeof(SetSkillsGump));

            // Check if the player has a backpack.
            if (from.Backpack == null)
            {
                // If the player does not have a backpack, send a message and return.
                from.SendMessage("This must be in your backpack to function.");
                return;
            }

            // Check if the AssignedPlayer field is null.
            if (this.AssignedPlayer == null)
            {
                // If the AssignedPlayer field is null, assign the player to the field, update the Name property, and send a message.
                this.AssignedPlayer = (PlayerMobile)from;
                this.Name = String.Format("{0}'s {1}", from.Name, this.Name);
                from.SendMessage("The skill stone has been assigned to you!");
            }
            // If the AssignedPlayer field is not null, but the player is not the same as the player assigned to the stone...
            else if (this.AssignedPlayer != (PlayerMobile)from)
            {
                // ...send a message indicating that the stone does not belong to the player.
                from.SendMessage("This skill stone does not belong to you!");
            }
            else
            {
                // If the player has a sufficient AccessLevel, send a debug message indicating that the stone is working.
                if (from.AccessLevel >= AccessLevel.GameMaster)
                    from.SendMessage("Debug: Stone is working.");
            }

            // If the player is the same as the player assigned to the stone, open a SetSkillsGump gump for the player.
            if (this.AssignedPlayer == (PlayerMobile)from)
                from.SendGump(new SetSkillsGump(from, this, SkillInfo.Table, null));
        }

        public SkillStone(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            // Serialize the base class.
            base.Serialize(writer);

            // Write version number.
            writer.Write(0);
            // Write the value of the m_SkillPoints field.
            writer.Write((int)m_SkillPoints);
            // Write the value of the m_SkillMaxLevel field.
            writer.Write((int)m_SkillMaxLevel);
            // Write the value of the m_AssignedPlayer field.
            writer.Write((PlayerMobile)m_AssignedPlayer);
        }

        public override void Deserialize(GenericReader reader)
        {
            // Deserialize the base class.
            base.Deserialize(reader);

            // Read the version number.
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    // Read the value of the m_SkillPoints field.
                    m_SkillPoints = reader.ReadInt();
                    // Read the value of the m_SkillMaxLevel field.
                    m_SkillMaxLevel = reader.ReadInt();
                    // Read the value of the m_AssignedPlayer field.
                    m_AssignedPlayer = (PlayerMobile)reader.ReadMobile();
                    break;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            // Add the base class's name properties to the list.
            base.AddNameProperties(list);
            // Add the number of remaining skill points to the display.
            list.Add(String.Format("Points Remaining: {0}", this.SkillPoints));
        }
    }

    public class SetSkillsGump : Gump
    {
        // Mobile player.
        private readonly Mobile m_Player;

        // Skill stone.
        private readonly SkillStone m_SkillStone;

        // Color of labels.
        private const int LabelColor = 0x7FFF;

        public SetSkillsGump(Mobile player, SkillStone stone, SkillInfo[] skills, object notice)
            : base(0, 0)
        {
            // Assign values to fields.
            m_Player = player;
            m_SkillStone = stone;

            // HTML string for the number of remaining skill points and the stone's maximum skill level.
            string skillsRemainingHTML = String.Format(
                "<CENTER><BASEFONT COLOR=#FFFF00>Remaining Skill Points: </BASEFONT><BASEFONT COLOR=#CCCCCC>{0}</BASEFONT> <BASEFONT COLOR=#FFFF00>Stone Max Skill Level: </BASEFONT><BASEFONT COLOR=#CCCCCC>{1}</BASEFONT></CENTER>",
                stone.SkillPoints,
                stone.SkillMaxLevel
            );

            AddPage(0);
            // Add a background image to the page.
            AddBackground(0, 0, 550, 550, 2620); // Stone BG: x,y,w,h,imageId
            // Add an HTML label to the page.
            AddHtml(
                0,
                15,
                550,
                25,
                "<CENTER><BASEFONT COLOR=#FFFF00>Skill Stone</BASEFONT></CENTER>",
                false,
                false
            );
            // Add an HTML label to the page.
            AddHtml(
                0,
                30,
                550,
                25,
                "<CENTER><BASEFONT COLOR=#FFFF00>Created By: Poolmanjim</BASEFONT></CENTER>",
                false,
                false
            );
            // Add an HTML label to the page.
            AddHtml(
                0,
                45,
                550,
                25,
                "<CENTER><BASEFONT COLOR=#FFFF00>Version 0.9.2 Beta</BASEFONT></CENTER>",
                false,
                false
            );
            // Add an HTML label to the page.
            AddHtml(0, 60, 550, 25, skillsRemainingHTML, false, false);
            // Add an HTML label to the page.
            AddHtml(
                50,
                90,
                400,
                60,
                "<BASEFONT COLOR=#CCCCCC>1 point = 0.1 skill. Click the number area and enter a value up to the maximum skill level supported. Click the button next to the field to set your skill. </BASEFONT>",
                false,
                false
            );

            // AddAlphaRegion(10, 70, 125, 460);

            for (int skillCount = 0; skillCount < skills.Length; ++skillCount)
            {
                // Calculate page and Y offset for each skill
                int skillsPerPage = 15;
                int index = (skillCount % skillsPerPage);

                int stdOriginY = 165;
                int standardOffY = 25;
                int offsetY = stdOriginY + (index * standardOffY);

                // Format strings for skill label and player's current skill value
                String skillLabel = String.Format(
                    "{0}. {1}",
                    skillCount.ToString(),
                    skills[skillCount].Name
                );
                String playerSkillBase = player.Skills[skillCount].Base.ToString();

                // If this is the first skill on a new page, add a "Next Page" button
                if (index == 0)
                {
                    if (skillCount > 0)
                    {
                        AddButton(
                            425,
                            470,
                            4005,
                            4007,
                            0,
                            GumpButtonType.Page,
                            (skillCount / skillsPerPage) + 1
                        );
                        AddHtmlLocalized(460, 470, 100, 18, 1044045, LabelColor, false, false); // NEXT PAGE
                    }

                    // Add a new page for the current set of skills
                    AddPage((skillCount / skillsPerPage) + 1);

                    // If this is not the first page, add a "Previous Page" button
                    if (skillCount > 0)
                    {
                        AddButton(
                            425,
                            495,
                            4014,
                            4015,
                            0,
                            GumpButtonType.Page,
                            skillCount / skillsPerPage
                        );
                        AddHtmlLocalized(460, 495, 100, 18, 1044044, LabelColor, false, false); // PREV PAGE
                    }
                }

                // Skill List
                AddLabel(150, stdOriginY + (index * 20), 1153, skillLabel);
                AddTextEntry(
                    300,
                    stdOriginY + (index * 20),
                    50,
                    25,
                    1153,
                    skillCount + 1,
                    playerSkillBase
                );
                //AddAlphaRegion( 295, 70 + (index * 20), 50, 15);
                AddButton(
                    355,
                    stdOriginY + (index * 20),
                    4005,
                    4007,
                    skillCount + 1,
                    GumpButtonType.Reply,
                    0
                ); // x, y, off click , on click, button type
            }

            // Close Button at Bottom
            // Add the skill label, a text entry for the skill value, and a "Set Skill" button
            AddButton(425, 520, 4005, 4007, 0x2, GumpButtonType.Reply, 0); // x, y, off click , on click, button type
            AddHtmlLocalized(460, 520, 140, 25, 1011012, false, false); // CANCEL
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            // Switch statement to handle the button press
            switch (info.ButtonID)
            {
                case 0: // Cancel
                    return;
                default:
                    // Foreach loop to iterate through the text entries in the relay info
                    foreach (TextRelay textEntry in info.TextEntries)
                    {
                        // Declare variables for the new skill value, difference in skill value, and the target skill
                        double newSkillValue;
                        double skillDiff;
                        Skill targetSkill;

                        // Set the target skill to the skill of the player at the index of the current text entry's ID minus 1 (to account for the 0 index)
                        targetSkill = m_Player.Skills[textEntry.EntryID - 1];

                        // Try to convert the text entry to a double
                        try
                        {
                            newSkillValue = Convert.ToDouble(textEntry.Text);
                        }
                        // If the conversion fails, set newSkillValue to 0 and send a message to the player
                        catch (System.Exception)
                        {
                            newSkillValue = 0;
                            m_Player.SendMessage(
                                2117,
                                String.Format(
                                    "You cannot set your skill {0} to a value that is not a number.",
                                    targetSkill.Name
                                )
                            );
                        }

                        // Calculate the difference between the new skill value and the old skill value
                        skillDiff = newSkillValue - targetSkill.Base;

                        // Rules out some weird conditions with adding.
                        // If the difference between the new skill value and the target skill's base value is negative, set it to 0.
                        if (skillDiff < 0)
                        {
                            skillDiff = 0;
                        }

                        // If the skill is equal to the existing or is just 0, ignore it.
                        if ((targetSkill.Base == newSkillValue) || newSkillValue == 0) // Skill is equal to target
                        {
                            // Literally do nothing, we don't particularly care in this case so we ignore it. This is the default action.
                        }
                        // If the skill is NOT equal to the existing or is just 0...
                        else
                        {
                            // Check if new skill value is greater than the maximum allowed by the stone
                            if (newSkillValue > m_SkillStone.SkillMaxLevel)
                            {
                                // Inform player that the stone is locked to a max skill level
                                m_Player.SendMessage(
                                    2117,
                                    String.Format(
                                        "You cannot set your skill that high. Your stone is locked to a max skill of {0}",
                                        m_SkillStone.SkillMaxLevel
                                    )
                                );
                            }
                            // Check if new skill value is less than or equal to the player's current skill
                            else if (targetSkill.Base >= newSkillValue) // Skill is currently higher
                            {
                                // Inform player that their current skill is higher than the new value
                                m_Player.SendMessage(
                                    2117,
                                    String.Format(
                                        "Your ability in the skill in {0} skill is higher than that. Current: {1}.",
                                        targetSkill.Name,
                                        targetSkill.Base
                                    )
                                );
                            }
                            // Check if new skill value is greater than the player's skill cap
                            else if (targetSkill.Cap < newSkillValue) // Skill is at Skill Cap
                            {
                                // Inform player that they cannot set their skill higher than their skill cap
                                m_Player.SendMessage(
                                    2117,
                                    String.Format(
                                        "You cannot set your skill in {0} any higher. Current: {1}/{2}. Use a PowerScroll to go higher.",
                                        targetSkill.Name,
                                        targetSkill.Base,
                                        targetSkill.Cap
                                    )
                                );
                                m_Player.SendMessage(newSkillValue.ToString());
                            }
                            // Check if setting the skill to the new value would cause the player to go over their total skill cap
                            else if (
                                m_Player.Skills.Cap < ((m_Player.SkillsTotal) + (skillDiff * 10))
                            ) // Skill max
                            {
                                // Inform player that setting the skill to the new value would cause them to go over their total skill cap
                                m_Player.SendMessage(
                                    2117,
                                    "You cannot set your skill any higher. You are at the skill cap of the server."
                                );
                            }
                            // Check if the stone has enough points to raise the skill to the new value
                            else if ((newSkillValue * 10) > m_SkillStone.SkillPoints)
                            {
                                // Inform player that the stone lacks the points to raise the skill to the new value
                                m_Player.SendMessage(
                                    2117,
                                    String.Format(
                                        "Your skill stone lacks the points to raise your skill that high. Points remaining: {0}.",
                                        m_SkillStone.SkillPoints
                                    )
                                );
                            }
                            else
                            {
                                // Set the skill to the new value
                                targetSkill.Base = newSkillValue;
                                // Deduct the points used to set the skill from the skill stone's total points
                                m_SkillStone.SkillPoints -= (int)(skillDiff * 10);
                                // Send a message to the player indicating that the skill has been set and how many points remain on the stone
                                m_Player.SendMessage(
                                    String.Format(
                                        "Set your {0} skill to {1}. The stone's power drains. You have {2} points remaining.",
                                        targetSkill.Name,
                                        newSkillValue,
                                        m_SkillStone.SkillPoints.ToString()
                                    )
                                );
                            }
                        }
                    }
                    break;
            }

            // Check if the SkillStone still has any points remaining
            if (m_SkillStone.SkillPoints <= 0)
            {
                // If not, delete the SkillStone
                m_SkillStone.Delete();
            }
            else
            {
                // Otherwise, re-open the SetSkillsGump for the player
                m_Player.SendGump(new SetSkillsGump(m_Player, m_SkillStone, SkillInfo.Table, null));
            }
        }
    }
}
