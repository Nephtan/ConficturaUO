using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Items
{
    public class AssemblyAnalyzer : Item
    {
        [Constructable]
        public AssemblyAnalyzer()
            : base(0x1F13)
        {
            Weight = 1.0;
            Name = "assembly analyzer";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendMessage("What assembly should I take a look at?");
                from.Target = new InternalTarget();
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public AssemblyAnalyzer(Serial serial)
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

        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(10, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseAssembly)
                {
                    BaseAssembly creature = (BaseAssembly)targeted;

                    if (from.Skills[SkillName.Tinkering].Value < 100.0 && creature.Creator != from)
                        from.SendMessage(
                            "At your skill level, you can only examine assemblies that you have crafted"
                        ); // At your skill level, you can only lore tamed creatures
                    else
                    {
                        if (from.CheckSkill(SkillName.Tinkering, 0, 60))
                        {
                            from.CloseGump(typeof(DruidismGump)); // Line 69
                            from.SendGump(new DruidismGump(from, (BaseCreature)creature, 1));
                        }
                        else
                        {
                            creature.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                500334,
                                from.NetState
                            ); // You can't think of anything you know offhand.
                        }
                    }
                }
                else
                {
                    from.LocalOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        "That is not an assemby"
                    );
                }
            }
        }
    }
}
