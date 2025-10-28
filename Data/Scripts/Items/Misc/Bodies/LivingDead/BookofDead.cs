using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class BookofDead : Item
    {
        public override string DefaultName
        {
            get { return "Book of the Dead"; }
        }

        [Constructable]
        public BookofDead()
            : base(0x1C11)
        {
            Weight = 10.0;
            Hue = 2500;
            LootType = LootType.Blessed;
        }

        public BookofDead(Serial serial)
            : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            double NecroSkill = from.Skills[SkillName.Necromancy].Value;
            double SpiritSkill = from.Skills[SkillName.SpiritSpeak].Value;

            if (NecroSkill < 80.0)
            {
                from.SendMessage(
                    "You must have at least 80.0 skill in Necromancy to resurect the dead."
                );
                return;
            }
            else if ((from.Followers + 2) > from.FollowersMax)
            {
                from.SendLocalizedMessage(1049607); // You have too many followers to control that creature.
                return;
            }

            double scalar;

            if (NecroSkill >= 120.0)
                scalar = 1.45;
            else if (NecroSkill >= 115.0)
                scalar = 1.35;
            else if (NecroSkill >= 110.0)
                scalar = 1.25;
            else if (NecroSkill >= 105.0)
                scalar = 1.15;
            else if (NecroSkill >= 100.0)
                scalar = 1.1;
            else if (NecroSkill >= 95.0)
                scalar = 1.02;
            else if (NecroSkill >= 90.0)
                scalar = 1.0;
            else
                scalar = 0.9;

            // Spirit Speak grants additional control over the construct. Small bonuses are
            // applied to the overall stat scalar and to the new sustain mechanics.
            double spiritBonus = Math.Max(0.0, (SpiritSkill - 80.0) * 0.0025);
            double siphonRatio = 0.75 - Math.Min(0.35, Math.Max(0.0, (SpiritSkill - 80.0) * 0.007));
            double empowermentBonus = Math.Max(0.0, (SpiritSkill - 80.0) * 0.004);

            scalar += spiritBonus;

            // Ensure the siphon ratio stays within reasonable limits (35% - 75%).
            if (siphonRatio < 0.35)
                siphonRatio = 0.35;
            else if (siphonRatio > 0.75)
                siphonRatio = 0.75;

            Container pack = from.Backpack;

            if (pack == null)
                return;

            int res = pack.ConsumeTotal(
                new Type[]
                {
                    typeof(DarkHeart),
                    typeof(Head),
                    typeof(Torso),
                    typeof(RightArm),
                    typeof(LeftArm),
                    typeof(RightLeg),
                    typeof(LeftLeg)
                },
                new int[] { 1, 1, 1, 1, 1, 1, 1, }
            );

            switch (res)
            {
                case 0:
                {
                    from.SendMessage("You need a Dark Heart to resurrect the dead.");
                    break;
                }
                case 1:
                {
                    from.SendMessage("You need a severed Head to resurrect the dead.");
                    break;
                }
                case 2:
                {
                    from.SendMessage("You need a Torso to resurrect the dead.");
                    break;
                }
                case 3:
                {
                    from.SendMessage("You need a Right Arm to resurrect the dead.");
                    break;
                }
                case 4:
                {
                    from.SendMessage("You need a Left Arm to resurrect the dead.");
                    break;
                }
                case 5:
                {
                    from.SendMessage("You need a Right Leg to resurrect the dead.");
                    break;
                }
                case 6:
                {
                    from.SendMessage("You need a Left Leg to resurrect the dead.");
                    break;
                }
                default:
                {
                    corpse z = new corpse(true, scalar, siphonRatio, empowermentBonus);

                    if (z.SetControlMaster(from))
                    {
                        z.MoveToWorld(from.Location, from.Map);
                        from.PlaySound(0x754);
                        from.FixedParticles(0x376A, 10, 30, 5052, EffectLayer.LeftFoot);
                        from.Say("Um Zex Fal Lum");
                        from.SendMessage(
                            0x59,
                            "Your mastery over spirit and sinew strengthens the corpse you summon."
                        );
                    }

                    break;
                }
            }
        }

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
