using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class AssemblyUpgradeKit : Item
    {
        [Constructable]
        public AssemblyUpgradeKit()
            : base(0x1505)
        {
            Name = "assembly upgrade kit";
            Weight = 1.0;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add(1060847, "{0}\t{1}", "  Upgrades assemblies crafted by tinkers", " "); // ~1_val~ ~2_val~
        }

        public AssemblyUpgradeKit(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("What item assembly do would want to upgrade?");

            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AssemblyUpgradeKit m_Kit;

            public InternalTarget(AssemblyUpgradeKit kit)
                : base(1, false, TargetFlags.Beneficial)
            {
                m_Kit = kit;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseAssembly)
                {
                    if (((BaseAssembly)targeted).ControlMaster != from)
                        from.SendMessage("You can not upgrade that");
                    else if (!((BaseAssembly)targeted).UpgradeAssembly())
                        from.SendMessage(
                            "That assembly has already been upgraded as much as possible"
                        );
                    else
                    {
                        from.SendMessage(
                            "You upgrade the assembly.  It appears to be stronger now."
                        );
                        m_Kit.Delete();
                    }
                }
                else
                {
                    from.SendMessage("You cannot upgrade that");
                }
            }
        }
    }
}
