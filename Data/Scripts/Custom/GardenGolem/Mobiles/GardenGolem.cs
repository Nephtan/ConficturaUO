using System;
using Server;
using Server.Custom.Confictura.GardenGolems.Gumps;
using Server.Custom.Confictura.GardenGolems.Systems;
using Server.Custom.Confictura.Items;
using Server.Engines.Plants;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Experimental hybrid construct that carries a miniature planter bed. The creature borrows combat
    ///     behavior from heavy golems while hosting a <see cref="GardenGolemPlanterState" /> used to duplicate
    ///     gardening produce for caretakers.
    /// </summary>
    [CorpseName("a garden golem corpse")]
    public class GardenGolem : BaseCreature
    {
        private readonly GardenGolemPlanterState m_PlanterState;
        private bool m_IsCrafted;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsCrafted
        {
            get { return m_IsCrafted; }
            set { m_IsCrafted = value; }
        }

        /// <summary>
        ///     Exposes the embedded planter state so that gumps and staff commands can inspect values.
        /// </summary>
        public GardenGolemPlanterState PlanterState
        {
            get { return m_PlanterState; }
        }

        [Constructable]
        public GardenGolem()
            : this(false)
        {
        }

        [Constructable]
        public GardenGolem(bool crafted)
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            m_PlanterState = new GardenGolemPlanterState();
            m_IsCrafted = crafted;

            Name = "a garden golem";
            Body = 446;
            Hue = 1782;
            BaseSoundID = 541;

            SetStr(450, 500);
            SetDex(90, 110);
            SetInt(50, 80);

            SetHits(400, 450);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 60.0, 75.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.FistFighting, 80.0, 95.0);

            Fame = 5500;
            Karma = -5500;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 95.0;

            VirtualArmor = 40;
        }

        public GardenGolem(Serial serial)
            : base(serial)
        {
            m_PlanterState = new GardenGolemPlanterState();
        }

        public override void OnThink()
        {
            base.OnThink();

            if (m_PlanterState.Tick(DateTime.Now) && ControlMaster != null)
            {
                PublicOverheadMessage(
                    MessageType.Emote,
                    0x44,
                    false,
                    "Fresh growth rustles within the golem's planter."
                );
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!from.InRange(this, 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            if (ControlMaster != null && from != ControlMaster)
            {
                from.SendMessage("Only the golem's caretaker can tend to its planter.");
                return;
            }

            if (!Controlled || ControlMaster == null)
            {
                from.SendMessage("Wild garden golems cannot be tended while untamed.");
                return;
            }

            from.CloseGump(typeof(GardenGolemCaretakerGump));
            from.SendGump(new GardenGolemCaretakerGump(this));
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped == null)
                return false;

            if (ControlMaster != null && from != ControlMaster)
            {
                SayTo(from, "You are not attuned to tend this golem.");
                return false;
            }

            if (!Controlled || ControlMaster == null)
            {
                SayTo(from, "The wild golem rejects your attempt to meddle with its planter.");
                return false;
            }

            if (!from.InRange(this, 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return false;
            }

            Seed seed = dropped as Seed;
            if (seed != null)
            {
                if (m_PlanterState.HasSeed)
                {
                    SayTo(from, "The planter already cultivates a specimen. Eject it first.");
                    return false;
                }

                if (!CanCaretakerProvideSeed(from, seed))
                {
                    from.SendLocalizedMessage(1042664); // You must have the object in your backpack to use it.
                    return false;
                }

                m_PlanterState.LoadSeed(seed);
                seed.Delete();

                Say("The golem absorbs the seed into the rich soil lining its chassis.");
                from.CloseGump(typeof(GardenGolemCaretakerGump));
                from.SendGump(new GardenGolemCaretakerGump(this));
                return true;
            }

            return base.OnDragDrop(from, dropped);
        }

        public void HandleWaterRequest(Mobile from)
        {
            if (!CheckCaretaker(from))
                return;

            m_PlanterState.Water();
            from.SendMessage("You rehydrate the soil that fills the golem's planter.");
        }

        public void HandleHarvestRequest(Mobile from)
        {
            if (!CheckCaretaker(from))
                return;

            if (!m_PlanterState.HasReadyYield)
            {
                from.SendMessage("The planter has nothing ready to collect yet.");
                return;
            }

            Item harvest = m_PlanterState.Harvest();

            if (harvest == null)
            {
                from.SendMessage("Infesting pests scatter the budding produce. Treat the planter first.");
                return;
            }

            if (!from.Backpack.TryDropItem(from, harvest, false))
            {
                from.SendMessage("You need free space in your backpack to store the harvest.");
                harvest.Delete();
                return;
            }

            from.SendMessage("You collect the harvest carefully stored inside the golem's planter.");
        }

        public void HandleTreatmentRequest(Mobile from)
        {
            if (!CheckCaretaker(from))
                return;

            if (!m_PlanterState.HasSeed)
            {
                from.SendMessage("The planter is empty and does not require treatment.");
                return;
            }

            if (m_PlanterState.Infestation == 0)
            {
                from.SendMessage("The planter is currently free of pests.");
            }
            else
            {
                m_PlanterState.TreatInfestation();
                m_PlanterState.ApplyFertilityBoost(1);
                from.SendMessage("You apply an herbal tonic that purges pests and invigorates the soil.");
            }
        }

        public void HandleEjectSeed(Mobile from)
        {
            if (!CheckCaretaker(from))
                return;

            if (!m_PlanterState.HasSeed)
            {
                from.SendMessage("The planter is empty.");
                return;
            }

            Item sample = new Seed(m_PlanterState.SeedPlantType, m_PlanterState.SeedPlantHue, true);

            if (!from.Backpack.TryDropItem(from, sample, false))
            {
                from.SendMessage("You need space in your backpack before removing the specimen.");
                sample.Delete();
                return;
            }

            m_PlanterState.ClearSeed();
            from.SendMessage("You coax the seedling back out of the golem's chassis.");
        }

        private bool CheckCaretaker(Mobile from)
        {
            if (from == null)
                return false;

            if (!from.InRange(this, 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return false;
            }

            if (ControlMaster != null && from != ControlMaster)
            {
                from.SendMessage("Only the bonded caretaker can perform that action.");
                return false;
            }

            return true;
        }

        public override void GenerateLoot()
        {
            if (!m_IsCrafted)
            {
                AddLoot(LootPack.Rich);
                AddLoot(LootPack.Average);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (c == null)
                return;

            int soilAmount = m_IsCrafted ? Utility.RandomMinMax(1, 3) : Utility.RandomMinMax(3, 5);
            c.DropItem(new FreshGardenSoil(soilAmount));

            if (!m_IsCrafted && Utility.RandomDouble() < 0.60)
            {
                c.DropItem(new Seed(PlantTypeInfo.RandomFirstGeneration(), PlantHueInfo.RandomFirstGeneration(), false));
            }

            if (m_PlanterState.HasSeed)
            {
                Item sample = new Seed(m_PlanterState.SeedPlantType, m_PlanterState.SeedPlantHue, true);
                c.DropItem(sample);
            }
        }

        private bool CanCaretakerProvideSeed(Mobile from, Seed seed)
        {
            if (from == null || seed == null)
                return false;

            if (from.Backpack != null && seed.IsChildOf(from.Backpack))
                return true;

            if (seed.RootParent == from)
                return true;

            BounceInfo bounce = seed.GetBounce();

            if (bounce != null)
            {
                if (bounce.m_Parent == from.Backpack)
                    return true;

                Item bounceItem = bounce.m_Parent as Item;

                if (bounceItem != null && bounceItem.RootParent == from)
                    return true;

                Mobile bounceMobile = bounce.m_Parent as Mobile;

                if (bounceMobile == from)
                    return true;
            }

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
            writer.Write(m_IsCrafted);

            m_PlanterState.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_IsCrafted = reader.ReadBool();

            m_PlanterState.Deserialize(reader);

            if (version < 1)
                m_IsCrafted = false;
        }
    }
}