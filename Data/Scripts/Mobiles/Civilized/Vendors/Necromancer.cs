using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;

namespace Server.Mobiles
{
    public class Necromancer : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.NecromancersGuild; }
        }

        [Constructable]
        public Necromancer()
            : base("the necromancer")
        {
            SetSkill(SkillName.Spiritualism, 65.0, 88.0);
            SetSkill(SkillName.Inscribe, 60.0, 83.0);
            SetSkill(SkillName.Meditation, 60.0, 83.0);
            SetSkill(SkillName.MagicResist, 65.0, 88.0);
            SetSkill(SkillName.Necromancy, 64.0, 100.0);
            SetSkill(SkillName.Forensics, 82.0, 100.0);

            Hue = 1150;
            HairHue = 932;
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer());
            m_SBInfos.Add(new RSJars());
            m_SBInfos.Add(new SBBuyArtifacts());
            m_SBInfos.Add(new SBScaryDeco());
            m_SBInfos.Add(new SBPaganReagents());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            if (Utility.RandomBool())
            {
                AddItem(new Server.Items.BlackStaff());
            }
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
                                "Dealing With Deathly Things",
                                SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Necromancer")
                            )
                        );
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        public Necromancer(Serial serial)
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
