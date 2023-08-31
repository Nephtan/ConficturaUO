// LOTR - Ring & Ring Wraith Package
// X-SirSly-X


using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a ring wrath")]
    public class RingWraith : BaseCreature
    {
        [Constructable]
        public RingWraith()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "A Ring Wraith";
            Body = 400;
            Hue = 1;

            SetStr(250, 300);
            SetDex(250, 300);
            SetInt(250, 300);

            SetHits(200, 300);

            SetDamage(8, 18);

            Direction = (Direction)Utility.Random(8);

            HoodedShroudOfShadows hood = new HoodedShroudOfShadows();
            hood.Movable = false;
            AddItem(hood);

            Longsword sword = new Longsword();
            sword.Movable = false;
            AddItem(sword);

            SetDamageType(ResistanceType.Physical, 40, 80);
            SetDamageType(ResistanceType.Fire, 40, 80);
            SetDamageType(ResistanceType.Poison, 40, 80);

            SetResistance(ResistanceType.Physical, 40, 80);
            SetResistance(ResistanceType.Cold, 40, 80);
            SetResistance(ResistanceType.Fire, 40, 80);
            SetResistance(ResistanceType.Energy, 40, 80);
            SetResistance(ResistanceType.Poison, 40, 80);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Swords, 100.0, 120.0);
        }

        public override bool AlwaysMurderer
        {
            get { return true; }
        }
        public override bool AutoDispel
        {
            get { return true; }
        }
        public override bool BardImmune
        {
            get { return true; }
        }
        public override bool Uncalmable
        {
            get { return true; }
        }
        public override bool Unprovokable
        {
            get { return true; }
        }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(3))
            {
                default:
                case 0:
                    return WeaponAbility.ArmorIgnore;
                case 1:
                    return WeaponAbility.Disarm;
                case 2:
                    return WeaponAbility.Dismount;
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            defender.Damage(Utility.Random(5, 10), this);
            defender.Stam -= Utility.Random(5, 10);
            defender.Mana -= Utility.Random(5, 10);
        }

        public RingWraith(Serial serial)
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
//Sly
