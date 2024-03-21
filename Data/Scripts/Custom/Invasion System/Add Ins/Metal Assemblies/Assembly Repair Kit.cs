using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class AssemblyRepairKit : Item
    {
        [Constructable]
        public AssemblyRepairKit()
            : base(0x1eba)
        {
            Name = "assembly repair kit";
            Weight = 1.0;
        }

        public AssemblyRepairKit(Serial serial)
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
            if (IsChildOf(from.Backpack))
            {
                Container ourPack = from.Backpack;

                if (ourPack == null)
                    return;

                Item ironingot = ourPack.FindItemByType(typeof(IronIngot));

                if (ironingot == null)
                {
                    from.SendMessage("You need iron ingots to repair an assembly");
                    return;
                }

                from.SendLocalizedMessage(1044276); // Target an item to repair

                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public void Target(Mobile tinker, Mobile assembly)
        {
            if (Deleted || !tinker.CanSee(this))
                return;

            int number = -1;

            if (!tinker.CanSee(assembly))
            {
                number = 500237; // Target can not be seen.
            }
            else if (assembly.Hits == assembly.HitsMax)
            {
                number = 1044281; // That being is not damaged!
            }
            else
            {
                if (!tinker.BeginAction(typeof(AssemblyRepairKit)))
                {
                    number = 500310; // You are busy with something else
                }
                else
                {
                    TimeSpan duration = TimeSpan.FromSeconds(8);

                    InternalTimer t = new InternalTimer(this, tinker, assembly, duration);
                    t.Start();

                    tinker.SendMessage("You begin to repair the assembly");
                    return;
                }
            }

            tinker.SendLocalizedMessage(number);
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Tinker;
            private Mobile m_Assembly;
            private AssemblyRepairKit m_Kit;

            public InternalTimer(
                AssemblyRepairKit kit,
                Mobile tinker,
                Mobile assembly,
                TimeSpan duration
            )
                : base(duration)
            {
                m_Kit = kit;
                m_Tinker = tinker;
                m_Assembly = assembly;
            }

            protected override void OnTick()
            {
                Type[] types = new Type[1];
                int[] amounts = new int[1];

                m_Tinker.EndAction(typeof(AssemblyRepairKit));

                Container ourPack = m_Tinker.Backpack;

                if (ourPack == null)
                    return;

                Item ironingot = ourPack.FindItemByType(typeof(IronIngot));

                if (
                    m_Kit.Deleted
                    || !m_Tinker.CanSee(m_Kit)
                    || !m_Tinker.CanSee(m_Assembly)
                    || ironingot == null
                )
                    return;

                int amounttouse = (int)(m_Tinker.Skills[SkillName.Tinkering].Base / 20);

                if (amounttouse > ironingot.Amount)
                    amounttouse = ironingot.Amount;

                if (amounttouse < 1)
                    amounttouse = 1;

                types[0] = typeof(IronIngot);
                amounts[0] = amounttouse;

                ourPack.ConsumeTotal(types, amounts, true);

                int tinkerNumber = -1;
                bool checkSkills = false;

                if (!m_Tinker.Alive)
                {
                    tinkerNumber = 500962; //You were unable to finish your work before you died.
                }
                else if (!m_Tinker.InRange(m_Assembly, 1))
                {
                    tinkerNumber = 500963; // You did not stay close enough to heal your target.
                }
                else if (m_Assembly.Hits == m_Assembly.HitsMax)
                {
                    tinkerNumber = 500423; // That item is already in full repair
                }
                else
                {
                    checkSkills = true;

                    double tinkering = m_Tinker.Skills[SkillName.Tinkering].Value;

                    if (tinkering < Utility.Random(85))
                    {
                        tinkerNumber = 1044280;
                    }
                    else
                    {
                        tinkerNumber = 500425;

                        double toHeal = amounttouse * 20 * Utility.RandomDouble();

                        m_Assembly.Heal((int)toHeal);

                        BaseCreature creature = (BaseCreature)m_Assembly;
                        creature.Loyalty = 100;
                    }
                }

                if (tinkerNumber != -1)
                    m_Tinker.SendLocalizedMessage(tinkerNumber);

                if (checkSkills)
                    m_Tinker.CheckSkill(SkillName.Tinkering, 0, 120);
            }
        }

        private class InternalTarget : Target
        {
            private AssemblyRepairKit m_Kit;

            public InternalTarget(AssemblyRepairKit kit)
                : base(1, false, TargetFlags.Beneficial)
            {
                m_Kit = kit;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseAssembly)
                {
                    if (!((BaseCreature)targeted).Controlled)
                    {
                        from.SendLocalizedMessage(500426);
                    }
                    else
                    {
                        m_Kit.Target(from, (Mobile)targeted);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500426);
                }
            }
        }
    }
}
