using System;
using Server.Engines.Craft;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.SkillHandlers;
using Server.Targeting;
using Server.Targets;

namespace Server.Items
{
    public class FarmLabelMaker : Item
    {
        private string m_FarmName;

        [CommandProperty(AccessLevel.GameMaster)]
        public string FarmName
        {
            get { return m_FarmName; }
            set
            {
                m_FarmName = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public FarmLabelMaker()
            : base(0xFC0)
        {
            Weight = 1.0;
            Name = "Farm Label Maker";
            Hue = 0x2E;
        }

        public FarmLabelMaker(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((string)m_FarmName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_FarmName = reader.ReadString();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.

                return;
            }
            else
            {
                from.SendMessage(
                    "Click on this label tool to name your Farm or a juice keg/bottle you crafted to add a new label."
                );
                from.Target = new RenameItemTarget(this); // Call our target
            }
        }

        private class RenameItemTarget : Target
        {
            private FarmLabelMaker m_LabelMaker;

            public RenameItemTarget(FarmLabelMaker labelmaker)
                : base(-1, false, TargetFlags.None)
            {
                m_LabelMaker = labelmaker;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    from.SendMessage(
                        "Invalid Target.  Only crafted juice kegs, bottles, or this label maker can be labeled."
                    );
                }
                else if (target is Item)
                {
                    Item item = (Item)target;

                    if (target == m_LabelMaker || target is BaseCraftJuice || target is JuiceKeg)
                    {
                        if (item.RootParent != from) // Make sure its in their pack or they are wearing it
                        {
                            from.SendMessage("The item must be in your pack to label it.");
                        }
                        else
                        {
                            if (item is BaseCraftJuice)
                            {
                                BaseCraftJuice juice = (BaseCraftJuice)item;
                                if (juice.Crafter != from)
                                {
                                    from.SendMessage(
                                        "That bottle is either not worth labeling or was not crafted at your farm!"
                                    );
                                }
                                else
                                {
                                    from.SendMessage("Enter label name now...");
                                    from.Prompt = new LabelPrompt(item);
                                }
                            }
                            else if (item is JuiceKeg)
                            {
                                JuiceKeg keg = (JuiceKeg)item;
                                if (keg.Crafter != from)
                                {
                                    from.SendMessage(
                                        "That keg is either not worth labeling or was not crafted at your farm!"
                                    );
                                }
                                else
                                {
                                    from.SendMessage("Enter label name now...");
                                    from.Prompt = new LabelPrompt(item);
                                }
                            }
                            else
                            {
                                from.SendMessage("Enter your farm name now...");
                                from.Prompt = new LabelPrompt2(m_LabelMaker);
                            }
                        }
                    }
                    else
                    {
                        from.SendMessage(
                            "Invalid Target. Only crafted juice kegs, bottles, or this label maker can be labeled."
                        );
                    }
                }
                else
                {
                    from.SendMessage(
                        "Invalid Target. Only crafted juice kegs, bottles, or this label maker can be labeled."
                    );
                }
            }
        }

        private class LabelPrompt : Prompt
        {
            private Item m_Item;

            public LabelPrompt(Item item)
            {
                m_Item = item;
            }

            public override void OnResponse(Mobile from, string text)
            {
                m_Item.Name = text;
                from.SendMessage("The juice has been labeled");
            }
        }

        private class LabelPrompt2 : Prompt
        {
            private FarmLabelMaker m_Item;

            public LabelPrompt2(FarmLabelMaker item)
            {
                m_Item = item;
            }

            public override void OnResponse(Mobile from, string text)
            {
                m_Item.m_FarmName = text;
                from.SendMessage(
                    "As long as you keep this tool in your pack, all future juice you create will be labeled with this farm name."
                );
            }
        }
    }
}
