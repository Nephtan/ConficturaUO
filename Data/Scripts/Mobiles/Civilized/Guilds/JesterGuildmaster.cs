using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class JesterGuildmaster : BaseGuildmaster
    {
        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.JestersGuild; }
        }

        [Constructable]
        public JesterGuildmaster()
            : base("jester")
        {
            SetSkill(SkillName.Marksmanship, 75.0, 98.0);
            SetSkill(SkillName.Hiding, 65.0, 88.0);
            SetSkill(SkillName.Begging, 85.0, 100.0);
            SetSkill(SkillName.Lockpicking, 80.0, 100.0);
            SetSkill(SkillName.Psychology, 85.0, 100.0);
            SetSkill(SkillName.Stealth, 85.0, 100.0);
        }

        public override void InitSBInfo()
        {
            SBInfos.Add(new SBJester());
            SBInfos.Add(new SBThief());
            SBInfos.Add(new SBBuyArtifacts());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            if (this.FindItemOnLayer(Layer.OuterTorso) != null)
            {
                this.FindItemOnLayer(Layer.OuterTorso).Delete();
            }
            if (this.FindItemOnLayer(Layer.Shoes) != null)
            {
                this.FindItemOnLayer(Layer.Shoes).Delete();
            }
            if (this.FindItemOnLayer(Layer.InnerTorso) != null)
            {
                this.FindItemOnLayer(Layer.InnerTorso).Delete();
            }
            if (this.FindItemOnLayer(Layer.Shirt) != null)
            {
                this.FindItemOnLayer(Layer.Shirt).Delete();
            }

            if (Utility.RandomBool())
            {
                AddItem(new Server.Items.JesterHat(Utility.RandomBlueHue()));
            }
            else
            {
                AddItem(new Server.Items.JokerHat(Utility.RandomBlueHue()));
            }

            switch (Utility.RandomMinMax(0, 2))
            {
                case 0:
                    AddItem(new Server.Items.JokerRobe(Utility.RandomBlueHue()));
                    break;
                case 1:
                    AddItem(new Server.Items.JesterGarb(Utility.RandomBlueHue()));
                    break;
                case 2:
                    AddItem(new Server.Items.FoolsCoat(Utility.RandomBlueHue()));
                    break;
            }

            AddItem(new Server.Items.JesterShoes(Utility.RandomBlueHue()));
        }

        public override void SayWelcomeTo(Mobile m)
        {
            SayTo(m, "Welcome to the guild!"); // Welcome to the guild!
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.AddCustomContextEntries(from, list);
        }

        public JesterGuildmaster(Serial serial)
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
