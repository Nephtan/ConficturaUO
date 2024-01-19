using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    public class BaseAssembly : BaseSpecialCreature
    {
        private BaseAI m_AI; // THE AI
        private bool m_bCrafted = false; // Creature was crafted.  For support of golems.

        //todo 	private bool		m_AlwaysObey = true;		// Creature always obeys commands. Horses and pack horses for example

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Crafted
        {
            get { return m_bCrafted; }
            set { m_bCrafted = value; }
        }

        private int m_UpgradeLevel = 0;

        private Mobile m_Creator = null;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UpgradeLevel
        {
            get { return m_UpgradeLevel; }
            set { m_UpgradeLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Creator
        {
            get { return m_Creator; }
            set { m_Creator = value; }
        }

        [Constructable]
        public BaseAssembly()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.7, 1.0)
        {
            ControlSlots = 3;
        }

        public bool UpgradeAssembly()
        {
            if (m_UpgradeLevel >= 4)
                return false;

            m_UpgradeLevel++;
            Str += (int)(Str * .15);
            Dex += (int)(Dex * .15);
            Int += (int)(Int * .15);
            VirtualArmor += (int)(VirtualArmor * .15);
            SetAssemblyTitle();

            Skills[SkillName.Anatomy].Cap += 5;
            Skills[SkillName.Bludgeoning].Cap += 5;
            Skills[SkillName.Swords].Cap += 5;
            Skills[SkillName.Fencing].Cap += 5;
            Skills[SkillName.FistFighting].Cap += 5;
            Skills[SkillName.Marksmanship].Cap += 5;
            Skills[SkillName.Tactics].Cap += 5;

            return true;
        }

        public void SetAssemblyTitle()
        {
            switch (m_UpgradeLevel)
            {
                case 0:
                    Title = "";
                    break;
                case 1:
                    Title = "[Enhanced]";
                    break;
                case 2:
                    Title = "[Upgraded]";
                    break;
                case 3:
                    Title = "[Tuned Up]";
                    break;
                case 4:
                    Title = "[Well Oiled]";
                    break;
            }
        }

        public BaseAssembly(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_UpgradeLevel);
            writer.Write((Mobile)m_Creator);
            writer.Write((bool)m_bCrafted);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            //int size = reader.ReadInt();
            //for ( int i = 0; i < size; ++i )
            switch (version)
            {
                case 0:
                    m_UpgradeLevel = reader.ReadInt();
                    m_Creator = reader.ReadMobile();
                    m_bCrafted = reader.ReadBool();
                    break;
            }
        }

        public override void OnDeath(Container c)
        {
            if (!Summoned && !Crafted)
            {
                int totalFame = Fame / 100;
                int totalKarma = -Karma / 100;

                ArrayList toGive = new ArrayList();

                //List<Aggressors> list = Aggressors;
                //List list = List<Aggressors>;//Aggressors;
                for (
                    int i = 0;
                    i
                        < /*Attacker.*/
                        Aggressors.Count;
                    ++i
                )
                //for ( int i = 0; i < list.Count; ++i )
                {
                    //List<AggressorInfo> info = (AggressorInfo)list[i];
                    AggressorInfo info = (AggressorInfo) /*Attacker.*/
                        Aggressors[i];
                    //List<AggressorInfo> info = (AggressorInfo)list[i];//controlMaster.Aggressors;

                    if (
                        (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0)
                        && !toGive.Contains(info.Attacker)
                    )
                        toGive.Add(info.Attacker);
                }

                //list = Aggressed;
                //for ( int i = 0; i < list.Count; ++i )
                for (
                    int i = 0;
                    i
                        < /*owner.*/
                        Aggressors.Count;
                    ++i
                )
                {
                    //AggressorInfo info = (AggressorInfo)list[i];
                    AggressorInfo info = (AggressorInfo) /*Defender.*/
                        Aggressors[i];

                    if (
                        (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0)
                        && !toGive.Contains(info.Defender)
                    )
                        toGive.Add(info.Defender);
                }

                for (int i = 0; i < toGive.Count; ++i)
                {
                    Mobile m = (Mobile)toGive[i];

                    int karmaToGive = totalKarma;
                    int fameToGive = totalFame;

                    if (m.Karma < 0)
                    {
                        if (Karma > 0 && m.Karma <= -Karma)
                            karmaToGive = 0;
                    }
                    else if (m.Karma > 0)
                    {
                        if (Karma < 0 && m.Karma >= -Karma)
                            karmaToGive = 0;
                    }

                    if (m.Fame > Fame)
                        fameToGive = 0;

                    if ((m.Karma + karmaToGive) > 10000)
                        karmaToGive = 10000 - m.Karma;
                    else if ((m.Karma + karmaToGive) < -10000)
                        karmaToGive = -10000 - m.Karma;

                    if ((m.Fame + fameToGive) > 10000)
                        fameToGive = 10000 - m.Fame;
                    else if ((m.Fame + fameToGive) < 0)
                        fameToGive = -m.Fame;

                    m.Karma += karmaToGive;
                    m.Fame += fameToGive;

                    if (fameToGive >= 40)
                        m.SendLocalizedMessage(1019054); // You have gained a lot of fame.
                    else if (fameToGive >= 25)
                        m.SendLocalizedMessage(1019053); // You have gained a good amount of fame.
                    else if (fameToGive >= 10)
                        m.SendLocalizedMessage(1019052); // You have gained some fame.
                    else if (fameToGive >= 1)
                        m.SendLocalizedMessage(1019051); // You have gained a little fame.
                    else if (fameToGive <= -40)
                        m.SendLocalizedMessage(1019058); // You have lost a lot of fame.
                    else if (fameToGive <= -25)
                        m.SendLocalizedMessage(1019057); // You have lost a good amount of fame.
                    else if (fameToGive <= -10)
                        m.SendLocalizedMessage(1019056); // You have lost some fame.
                    else if (fameToGive <= -1)
                        m.SendLocalizedMessage(1019055); // You have lost a little fame.

                    if (karmaToGive >= 40)
                        m.SendLocalizedMessage(1019062); // You have gained a lot of karma.
                    else if (karmaToGive >= 25)
                        m.SendLocalizedMessage(1019060); // You have gained some karma.
                    else if (karmaToGive >= 10)
                        m.SendLocalizedMessage(1019060); // You have gained some karma.
                    else if (karmaToGive >= 1)
                        m.SendLocalizedMessage(1019059); // You have gained a little karma.
                    else if (karmaToGive <= -40)
                        m.SendLocalizedMessage(1019066); // You have lost a lot of karma.
                    else if (karmaToGive <= -25)
                        m.SendLocalizedMessage(1019065); // You have lost a good amount of karma.
                    else if (karmaToGive <= -10)
                        m.SendLocalizedMessage(1019064); // You have lost some karma.
                    else if (karmaToGive <= -1)
                        m.SendLocalizedMessage(1019063); // You have lost a little karma.
                }
            }

            base.OnDeath(c);

            if (Crafted)
                c.Delete();
        }
    }
}
