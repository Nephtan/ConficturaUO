using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;

namespace Server.Mobiles
{
    public class Artist : BaseCreature
    {
        public override bool CanTeach
        {
            get { return true; }
        }

        [Constructable]
        public Artist()
            : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            InitStats(31, 41, 51);

            SetSkill(SkillName.Anatomy, 36, 68);

            SpeechHue = Utility.RandomTalkHue();
            Title = "the painter";
            Hue = Utility.RandomSkinColor();

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName("female");
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName("male");
            }

            AddItem(new Doublet(Utility.RandomDyedHue()));
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            AddItem(new HalfApron(Utility.RandomDyedHue()));

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();

            pack.DropItem(new Gold(250, 300));

            pack.Movable = false;

            AddItem(pack);
        }

        public override bool ClickTitle
        {
            get { return false; }
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new SpeechGumpEntry(from, this));
        }

        public class SpeechGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public SpeechGumpEntry(Mobile from, Mobile giver)
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
                {
                    if (!mobile.HasGump(typeof(SpeechGump)))
                    {
                        Server.Misc.IntelligentAction.SayHey(m_Giver);
                        mobile.SendGump(
                            new SpeechGump(
                                mobile,
                                "Portraits of Adventurers",
                                SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Painter")
                            )
                        );
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        public static int BeggingPose(Mobile from) // LET US SEE IF THEY ARE BEGGING
        {
            int beggar = 0;
            if (from is PlayerMobile)
            {
                if (((PlayerMobile)from).CharacterBegging > 0)
                {
                    beggar = (int)from.Skills[SkillName.Begging].Value;
                }
            }
            return beggar;
        }

        public static int BeggingKarma(Mobile from) // LET US SEE IF THEY ARE BEGGING
        {
            int charisma = 0;
            if (from.Karma > -2459)
            {
                charisma = 40;
            }
            from.CheckSkill(SkillName.Begging, 0, 125);
            return charisma;
        }

        public static string BeggingWords() // LET US SEE IF THEY ARE BEGGING
        {
            string sSpeak = "Please give me a good price as I am so poor.";
            switch (Utility.RandomMinMax(0, 5))
            {
                case 0:
                    sSpeak = "Please give me a good price as I am so poor.";
                    break;
                case 1:
                    sSpeak = "I have very little gold so whatever you can give...";
                    break;
                case 2:
                    sSpeak = "I have not eaten in days so your gold will surely help.";
                    break;
                case 3:
                    sSpeak = "Will thou give a poor soul more for these?";
                    break;
                case 4:
                    sSpeak = "I have fallen on hard times, will thou be kind?";
                    break;
                case 5:
                    sSpeak = "Whatever you can give for these will surely help.";
                    break;
            }
            return sSpeak;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is PaintCanvas)
            {
                Container pack = from.Backpack;
                int paintPrice = 5000;

                if (Server.Mobiles.BaseVendor.BeggingPose(from) > 0) // LET US SEE IF THEY ARE BEGGING
                {
                    paintPrice =
                        paintPrice
                        - (int)((from.Skills[SkillName.Begging].Value * 0.005) * paintPrice);
                    if (paintPrice < 1)
                    {
                        paintPrice = 1;
                    }
                }

                if (pack.ConsumeTotal(typeof(Gold), paintPrice))
                {
                    if (Server.Mobiles.BaseVendor.BeggingPose(from) > 0)
                    {
                        Titles.AwardKarma(
                            from,
                            -Server.Mobiles.BaseVendor.BeggingKarma(from),
                            true
                        );
                    } // DO ANY KARMA LOSS
                    this.SayTo(from, "Here is a nice painting of you.");
                    from.SendMessage(String.Format("You pay {0} gold.", paintPrice));

                    WaxPaintingA portrait = new WaxPaintingA();

                    string sTitle = "the " + Server.Misc.GetPlayerInfo.GetSkillTitle(from);
                    if (from.Title != null)
                    {
                        sTitle = from.Title;
                    }
                    sTitle = sTitle.Replace("  ", String.Empty);
                    portrait.Name = "painting of " + from.Name + " " + sTitle;
                    portrait.PaintingFlipID1 = 0xEA3;
                    portrait.PaintingFlipID2 = 0xEA4;
                    portrait.ItemID = 0xEA3;

                    from.AddToBackpack(portrait);
                }
                else
                {
                    this.SayTo(
                        from,
                        "It would cost you {0} gold to have a portrait done.",
                        paintPrice
                    );
                    from.SendMessage("You do not have enough gold.");
                    from.AddToBackpack(new PaintCanvas());
                }
                dropped.Delete();
            }
            else if (dropped is WaxPaintingA && dropped.Weight == 15.0)
            {
                this.SayTo(from, "How about this?");

                WaxPaintingA portrait = (WaxPaintingA)dropped;

                string sTitle = "the " + Server.Misc.GetPlayerInfo.GetSkillTitle(from);
                if (from.Title != null)
                {
                    sTitle = from.Title;
                }
                sTitle = sTitle.Replace("  ", String.Empty);
                portrait.Name = "painting of " + from.Name + " " + sTitle;

                if (dropped.ItemID == 0xEA3 || dropped.ItemID == 0xEA4)
                {
                    portrait.PaintingFlipID1 = 0xEE7;
                    portrait.PaintingFlipID2 = 0xEC9;
                    portrait.ItemID = 0xEE7;
                }
                else if (dropped.ItemID == 0xEE7 || dropped.ItemID == 0xEC9)
                {
                    portrait.PaintingFlipID1 = 0xE9F;
                    portrait.PaintingFlipID2 = 0xEC8;
                    portrait.ItemID = 0xE9F;
                }
                else if (dropped.ItemID == 0xE9F || dropped.ItemID == 0xEC8)
                {
                    portrait.PaintingFlipID1 = 0xEA6;
                    portrait.PaintingFlipID2 = 0xEA8;
                    portrait.ItemID = 0xEA6;
                }
                else if (dropped.ItemID == 0xEA6 || dropped.ItemID == 0xEA8)
                {
                    portrait.PaintingFlipID1 = 0x2A5D;
                    portrait.PaintingFlipID2 = 0x2A61;
                    portrait.ItemID = 0x2A5D;
                }
                else if (dropped.ItemID == 0x2A5D || dropped.ItemID == 0x2A61)
                {
                    portrait.PaintingFlipID1 = 0x2A65;
                    portrait.PaintingFlipID2 = 0x2A67;
                    portrait.ItemID = 0x2A65;
                }
                else if (dropped.ItemID == 0x2A65 || dropped.ItemID == 0x2A67)
                {
                    portrait.PaintingFlipID1 = 0x2A69;
                    portrait.PaintingFlipID2 = 0x2A6D;
                    portrait.ItemID = 0x2A69;
                }
                else
                {
                    portrait.PaintingFlipID1 = 0xEA3;
                    portrait.PaintingFlipID2 = 0xEA4;
                    portrait.ItemID = 0xEA3;
                }

                from.AddToBackpack(dropped);
            }
            else
            {
                PlayerMobile pm = (PlayerMobile)from;

                int RelicValue = 0;

                if (Server.Misc.RelicItems.RelicValue(dropped, this) > 0)
                    RelicValue = Server.Misc.RelicItems.RelicValue(dropped, this);

                if (RelicValue > 0)
                {
                    int gBonus = 0;

                    if (BeggingPose(from) > 0) // LET US SEE IF THEY ARE BEGGING
                    {
                        Titles.AwardKarma(from, -BeggingKarma(from), true);
                        from.Say(BeggingWords());
                        gBonus = (int)
                            Math.Round(
                                ((from.Skills[SkillName.Begging].Value * RelicValue) / 100),
                                0
                            );
                    }
                    gBonus = gBonus + RelicValue;
                    from.SendSound(0x3D);
                    from.AddToBackpack(new Gold(gBonus));
                    string sMessage = "";
                    switch (Utility.RandomMinMax(0, 9))
                    {
                        case 0:
                            sMessage =
                                "I have been looking for something like this. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 1:
                            sMessage =
                                "I have heard of this item before. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 2:
                            sMessage =
                                "I never thought I would see one of these. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 3:
                            sMessage =
                                "I have never seen one of these. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 4:
                            sMessage =
                                "What a rare item. Here is " + gBonus.ToString() + " gold for you.";
                            break;
                        case 5:
                            sMessage =
                                "This is quite rare. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 6:
                            sMessage =
                                "This will go nicely in my collection. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 7:
                            sMessage =
                                "I have only heard tales about such items. Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 8:
                            sMessage =
                                "How did you come across this? Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                        case 9:
                            sMessage =
                                "Where did you find this? Here is "
                                + gBonus.ToString()
                                + " gold for you.";
                            break;
                    }
                    this.PrivateOverheadMessage(
                        MessageType.Regular,
                        1153,
                        false,
                        sMessage,
                        from.NetState
                    );
                    dropped.Delete();
                    return true;
                }
            }
            return base.OnDragDrop(from, dropped);
        }

        public override void OnAfterSpawn()
        {
            Server.Misc.MorphingTime.CheckMorph(this);
        }

        protected override void OnMapChange(Map oldMap)
        {
            base.OnMapChange(oldMap);
            Server.Misc.MorphingTime.CheckMorph(this);
        }

        public Artist(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
