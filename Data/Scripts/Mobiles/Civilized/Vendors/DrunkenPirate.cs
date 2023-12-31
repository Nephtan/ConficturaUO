using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;

namespace Server.Mobiles
{
    public class DrunkenPirate : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.FishermensGuild; }
        }

        [Constructable]
        public DrunkenPirate()
            : base("the drunken pirate")
        {
            SetSkill(SkillName.Carpentry, 60.0, 83.0);
            SetSkill(SkillName.Seafaring, 75.0, 98.0);
            SetSkill(SkillName.Cartography, 90.0, 100.0);

            Direction = Direction.West;
            CantWalk = true;
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright());
            m_SBInfos.Add(new SBFisherman());
            m_SBInfos.Add(new SBSailor());
            m_SBInfos.Add(new SBHighSeas());
            m_SBInfos.Add(new RSScrolls());
            m_SBInfos.Add(new SBMapmaker());
            m_SBInfos.Add(new SBBuyArtifacts());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new Server.Items.PirateHat(Utility.RandomNeutralHue()));
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
                                "The High Seas",
                                SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Shipwright")
                            )
                        );
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        public DrunkenPirate(Serial serial)
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
