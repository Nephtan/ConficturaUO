using System;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    public class SpiderQueen : BaseChampion
    {
        public override ChampionSkullType SkullType
        {
            get { return ChampionSkullType.Venom; }
        }
        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(Artifact_ShroudOfDeciet) }; }
        }
        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ANecromancerShroud), typeof(CaptainJohnsHat) }; }
        }
        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(WallBlood), typeof(TatteredAncientMummyWrapping) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        [Constructable]
        public SpiderQueen()
            : base(AIType.AI_Mage)
        {
            Body = 0x48;
            Hue = 2412;
            Name = "SpiderQueen";

            BaseSoundID = 0x183;

            SetStr(755, 1200);
            SetDex(302, 400);
            SetInt(602, 800);

            SetHits(5000);
            SetStam(305, 700);

            SetDamage(25, 38);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Physical, 85, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 100.1, 120);
            SetSkill(SkillName.Psychology, 127.1, 160);
            SetSkill(SkillName.Meditation, 127.1, 150);
            SetSkill(SkillName.Magery, 127.7, 160);
            SetSkill(SkillName.MagicResist, 90.7, 160);
            SetSkill(SkillName.Tactics, 127.6, 140);
            SetSkill(SkillName.FistFighting, 127.6, 140);

            Fame = 28500;
            Karma = -28500;

            VirtualArmor = 90;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Lethal; }
        }
        public override Poison HitPoison
        {
            get { return Poison.Lethal; }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            // TODO: Web ability
        }

        public SpiderQueen(Serial serial)
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
