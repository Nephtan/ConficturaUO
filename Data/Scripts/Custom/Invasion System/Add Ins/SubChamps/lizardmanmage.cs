using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a lizardman corpse")]
    public class LizardmanMage : BaseCreature
    {
        [Constructable]
        public LizardmanMage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("lizardman");
            Body = 35;
            BaseSoundID = 4176;

            SetStr(1464, 180);
            SetDex(101, 130);
            SetInt(186, 210);

            SetHits(889, 108);

            SetDamage(75, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 258, 30);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 108, 20);

            SetSkill(SkillName.Psychology, 60.1, 70.0);
            SetSkill(SkillName.Magery, 60.1, 70.0);
            SetSkill(SkillName.MagicResist, 55.1, 80.0);
            SetSkill(SkillName.Tactics, 50.1, 75.0);
            SetSkill(SkillName.FistFighting, 50.1, 75.0);

            Fame = 7500;
            Karma = -7500;

            VirtualArmor = 40;

            PackGold(175, 225);
            // PackItem( new Token( 65, 110 ) );
            PackReg(6);
            PackScroll(1, 7);
        }

        /*public override void OnDamage( int amount, Mobile attacker, bool willKill )
                {
                    if( attacker is dovPlayerMobile )
                    {
                        dovPlayerMobile mobile = attacker as dovPlayerMobile;
        
                        if( mobile.LizRatFriendship == LizRat.Lizardman )
                        {
                            mobile.LizRatFriendship = LizRat.None;
                            mobile.SendLocalizedMessage( 1054103 );
                        }
                    }
                    base.OnDamage( amount, attacker, willKill );
                }*/
        public override int Meat
        {
            get { return 1; }
        }
        public override int Hides
        {
            get { return 12; }
        }
        public override HideType HideType
        {
            get { return HideType.Spined; }
        }

        public LizardmanMage(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
