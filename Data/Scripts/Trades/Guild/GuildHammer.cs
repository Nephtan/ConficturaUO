using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class GuildHammer : Item
    {
        [Constructable]
        public GuildHammer()
            : base(0xFB5)
        {
            Name = "Extraordinary Smithing Hammer";
            Weight = 5.0;
            Hue = 0x430;
        }

        public GuildHammer(Serial serial)
            : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
            {
                int canDo = 0;
                foreach (Mobile m in this.GetMobilesInRange(20))
                {
                    if (m is BlacksmithGuildmaster)
                        ++canDo;
                }
                foreach (Item i in this.GetItemsInRange(20))
                {
                    if (i is BlacksmithShoppe && !i.Movable)
                    {
                        BlacksmithShoppe b = (BlacksmithShoppe)i;

                        if (b.ShoppeOwner == from)
                            ++canDo;
                    }
                }
                if (
                    from.Map == Map.SavagedEmpire
                    && from.X > 1054
                    && from.X < 1126
                    && from.Y > 1907
                    && from.Y < 1983
                )
                {
                    ++canDo;
                }

                PlayerMobile pc = (PlayerMobile)from;
                if (pc.NpcGuild != NpcGuild.BlacksmithsGuild)
                {
                    from.SendMessage("Only those of the Blacksmiths Guild may use this!");
                }
                else if (from.Skills[SkillName.Blacksmith].Value < 90)
                {
                    from.SendMessage("Only a master blacksmith can use this!");
                }
                else if (canDo == 0)
                {
                    from.SendMessage(
                        "You need to be near a smithing guildmaster, or a smithing shoppe you own, to use this!"
                    );
                }
                else
                {
                    from.SendMessage("Select the metal equipment you would like to enhance...");
                    from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnTarget));
                }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new SpeechGumpEntry(from));
        }

        public class SpeechGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;

            public SpeechGumpEntry(Mobile from)
                : base(6121, 3)
            {
                m_Mobile = from;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(SpeechGump)))
                    {
                        mobile.SendGump(
                            new SpeechGump(
                                mobile,
                                "Enhancing Items",
                                SpeechFunctions.SpeechText(m_Mobile, m_Mobile, "Enhance")
                            )
                        );
                    }
                }
            }
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (obj is Item)
            {
                Item item = (Item)obj;

                if (((Item)obj).RootParent != from)
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
                else if (item is ILevelable)
                {
                    from.SendMessage("You cannot enhance legendary artefacts!");
                }
                else if (item is BaseWeapon)
                {
                    BaseWeapon weapon = (BaseWeapon)item;

                    if (Server.Misc.MaterialInfo.IsAnyKindOfMetalItem(item))
                    {
                        GuildCraftingProcess process = new GuildCraftingProcess(from, (Item)obj);
                        process.BeginProcess();
                    }
                    else
                    {
                        from.SendMessage("You cannot enhance this item!");
                    }
                }
                else if (item is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)item;

                    if (Server.Misc.MaterialInfo.IsAnyKindOfMetalItem(item))
                    {
                        GuildCraftingProcess process = new GuildCraftingProcess(from, (Item)obj);
                        process.BeginProcess();
                    }
                    else
                    {
                        from.SendMessage("You cannot enhance this item!");
                    }
                }
                else
                {
                    from.SendMessage("You cannot enhance this item!");
                }
            }
        }

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
