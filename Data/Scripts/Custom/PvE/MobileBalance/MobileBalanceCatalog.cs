using System;
using Server;
using Server.Custom.Confictura.Items;
using Server.Custom.Confictura.Mobiles;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.PvE.MobileBalance
{
    internal sealed class MobileBalanceSkill
    {
        private readonly SkillName m_Skill;
        private readonly double m_Value;

        public SkillName Skill
        {
            get { return m_Skill; }
        }

        public double Value
        {
            get { return m_Value; }
        }

        public MobileBalanceSkill(SkillName skill, double value)
        {
            m_Skill = skill;
            m_Value = value;
        }
    }

    internal static class MobileBalanceCatalog
    {
        public static void ApplyProfile(BaseCreature creature)
        {
            if (creature == null)
            {
                return;
            }

            Type type = creature.GetType();

            if (type == typeof(Forgotten))
            {
                Apply(creature, 25, 50, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(Atlantian))
            {
                Apply(creature, 25, 50, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(RedDeath))
            {
                Apply(creature, 25, 80, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(Honorguard))
            {
                Apply(creature, 60, 80, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(Scientist))
            {
                Apply(creature, 90, 150, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(DarkCultist))
            {
                Apply(creature, 25, 50, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(RuneKnight))
            {
                Apply(creature, 50, 100, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(EternalGuardian))
            {
                Apply(creature, 90, 130, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(AtlantianParagon))
            {
                Apply(creature, 50, 100, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(DiosMonster))
            {
                Apply(creature, 150, 170, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(WolfgangsDragon))
            {
                Apply(creature, 110, 160, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(TrueVirtue))
            {
                Apply(creature, 150, 200, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(Triton))
            {
                Apply(creature, 140, 190, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(FallenAngel))
            {
                Apply(creature, 120, 160, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(Olympus))
            {
                Apply(creature, 80, 140, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(OldKingWolfgang))
            {
                Apply(creature, 150, 200, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(Atlas))
            {
                Apply(creature, 90, 200, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(BlackbartRoberts))
            {
                Apply(creature, 100, 150, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Poisoning, 125),
                });
            }
            else if (type == typeof(ArmageddonEngine))
            {
                Apply(creature, 150, 200, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(VoidWraith))
            {
                Apply(creature, 90, 150, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(PlagueGod))
            {
                Apply(creature, 100, 160, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(DragonIllusion))
            {
                Apply(creature, 90, 160, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(FalseFireGod))
            {
                Apply(creature, 110, 200, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(FinalExodus))
            {
                Apply(creature, 190, 230, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(FireKing))
            {
                Apply(creature, 120, 180, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(ImmortalGenie))
            {
                Apply(creature, 90, 150, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(OldManWillow))
            {
                Apply(creature, 90, 125, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
            else if (type == typeof(Garuda))
            {
                Apply(creature, 100, 140, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(ForgottenCreation))
            {
                Apply(creature, 75, 110, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(DragonHydra))
            {
                Apply(creature, 150, 200, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(VampireIllusion))
            {
                Apply(creature, 25, 50, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                });
            }
            else if (type == typeof(TreasureGoblin))
            {
                Apply(creature, 50, 90, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Hiding, 125),
                    new MobileBalanceSkill(SkillName.Stealth, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                    new MobileBalanceSkill(SkillName.Parry, 125),
                });
            }
            else if (type == typeof(Skinwalker))
            {
                Apply(creature, 75, 100, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                });
            }
            else if (type == typeof(FireIllusion))
            {
                Apply(creature, 75, 120, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                    new MobileBalanceSkill(SkillName.Tactics, 125),
                    new MobileBalanceSkill(SkillName.MagicResist, 125),
                    new MobileBalanceSkill(SkillName.Magery, 125),
                    new MobileBalanceSkill(SkillName.Psychology, 125),
                });
            }
            else if (type == typeof(Lovecraftian))
            {
                Apply(creature, 100, 160, new MobileBalanceSkill[]
                {
                    new MobileBalanceSkill(SkillName.Searching, 125),
                });
            }
        }

        public static void DropLoot(BaseCreature creature, Container corpse)
        {
            if (creature == null || corpse == null)
            {
                return;
            }

            Type type = creature.GetType();

            if (type == typeof(DiosMonster))
            {
                TryDrop(corpse, "ITEM-001", 60, 1, 1);
            }
            else if (type == typeof(WolfgangsDragon))
            {
                TryDrop(corpse, "ITEM-002", 70, 1, 1);
            }
            else if (type == typeof(TrueVirtue))
            {
                TryDrop(corpse, "ITEM-003", 80, 1, 1);
            }
            else if (type == typeof(RuneKnight))
            {
                TryDrop(corpse, "ITEM-004", 20, 1, 1);
            }
            else if (type == typeof(Triton))
            {
                TryDrop(corpse, "ITEM-005", 70, 1, 1);
            }
            else if (type == typeof(FallenAngel))
            {
                TryDrop(corpse, "ITEM-006", 50, 1, 1);
            }
            else if (type == typeof(Olympus))
            {
                TryDrop(corpse, "ITEM-007", 20, 1, 1);
            }
            else if (type == typeof(OldKingWolfgang))
            {
                TryDrop(corpse, "ITEM-008", 60, 1, 1);
            }
            else if (type == typeof(EternalGuardian))
            {
                TryDrop(corpse, "ITEM-009", 30, 1, 1);
            }
            else if (type == typeof(Atlas))
            {
                TryDrop(corpse, "ITEM-010", 60, 1, 1);
            }
            else if (type == typeof(BlackbartRoberts))
            {
                TryDrop(corpse, "ITEM-011", 30, 1, 1);
            }
            else if (type == typeof(ArmageddonEngine))
            {
                TryDrop(corpse, "ITEM-012", 80, 1, 1);
            }
            else if (type == typeof(VoidWraith))
            {
                TryDrop(corpse, "ITEM-013", 5, 1, 1);
            }
            else if (type == typeof(PlagueGod))
            {
                TryDrop(corpse, "ITEM-014", 60, 1, 1);
            }
            else if (type == typeof(DragonIllusion))
            {
                TryDrop(corpse, "ITEM-015", 30, 1, 1);
            }
            else if (type == typeof(FalseFireGod))
            {
                TryDrop(corpse, "ITEM-016", 50, 1, 1);
            }
            else if (type == typeof(FinalExodus))
            {
                TryDrop(corpse, "ITEM-017", 30, 1, 1);
                TryDrop(corpse, "ITEM-018", 30, 1, 1);
                TryDrop(corpse, "ITEM-019", 30, 1, 1);
                TryDrop(corpse, "ITEM-020", 30, 1, 1);
            }
            else if (type == typeof(FireKing))
            {
                TryDrop(corpse, "ITEM-021", 70, 1, 1);
            }
            else if (type == typeof(ImmortalGenie))
            {
                TryDrop(corpse, "ITEM-022", 20, 1, 1);
            }
            else if (type == typeof(AtlantianParagon))
            {
                TryDrop(corpse, "ITEM-023", 20, 1, 1);
            }
            else if (type == typeof(Forgotten))
            {
                TryDrop(corpse, "ITEM-024", 20, 1, 1);
            }
            else if (type == typeof(Atlantian))
            {
                TryDrop(corpse, "ITEM-025", 30, 1, 1);
            }
            else if (type == typeof(RedDeath))
            {
                TryDrop(corpse, "ITEM-026", 90, 1, 1);
            }
            else if (type == typeof(Honorguard))
            {
                TryDrop(corpse, "ITEM-027", 10, 1, 1);
            }
            else if (type == typeof(Scientist))
            {
                TryDrop(corpse, "ITEM-028", 10, 1, 1);
            }
            else if (type == typeof(DarkCultist))
            {
                TryDrop(corpse, "ITEM-029", 5, 1, 1);
            }
            else if (type == typeof(OldManWillow))
            {
                TryDrop(corpse, "ITEM-030", 10, 1, 1);
                TryDrop(corpse, "ITEM-031", 5, 1, 1);
            }
            else if (type == typeof(Garuda))
            {
                TryDrop(corpse, "ITEM-032", 50, 1, 3);
                TryDrop(corpse, "ITEM-033", 20, 1, 1);
                TryDrop(corpse, "ITEM-034", 60, 1, 1);
            }
            else if (type == typeof(ForgottenCreation))
            {
                TryDrop(corpse, "ITEM-035", 5, 1, 1);
            }
            else if (type == typeof(DragonHydra))
            {
                TryDrop(corpse, "ITEM-036", 40, 1, 1);
            }
            else if (type == typeof(VampireIllusion))
            {
                TryDrop(corpse, "ITEM-037", 10, 1, 1);
            }
            else if (type == typeof(TreasureGoblin))
            {
                TryDrop(corpse, "ITEM-038", 5, 1, 1);
            }
            else if (type == typeof(Skinwalker))
            {
                TryDrop(corpse, "ITEM-039", 20, 1, 1);
            }
        }

        private static void Apply(
            BaseCreature creature,
            int damageMin,
            int damageMax,
            MobileBalanceSkill[] skills
        )
        {
            creature.SetDamage(damageMin, damageMax);

            for (int i = 0; i < skills.Length; ++i)
            {
                creature.SetSkill(skills[i].Skill, skills[i].Value);
            }
        }

        private static void TryDrop(
            Container corpse,
            string itemId,
            double chancePercent,
            int quantityMin,
            int quantityMax
        )
        {
            if (Utility.RandomDouble() * 100.0 >= chancePercent)
            {
                return;
            }

            int amount = Utility.RandomMinMax(quantityMin, quantityMax);
            Item item = MobileBalanceItemFactory.Create(itemId, amount);

            if (item != null)
            {
                corpse.DropItem(item);
            }
        }
    }

    internal static class MobileBalanceItemFactory
    {
        public static Item Create(string itemId, int amount)
        {
            Item item;

            switch (itemId)
            {
                case "ITEM-001":
                    item = new MindOverMatterTalisman();
                    Configure(item, "Mindful Talisman", 0x2F5B, 0, Layer.Talisman, "Artefact", amount);
                    break;
                case "ITEM-002":
                    item = new GoldenDragonStatue();
                    Configure(item, "Golden Dragon Statue", 0x4C2D, 2843, Layer.Invalid, "Artefact", amount);
                    break;
                case "ITEM-003":
                    item = new MemorialofVirtue();
                    Configure(item, "Memorial of Virtue", 0x4FC7, 0, Layer.Invalid, "Artefact", amount);
                    break;
                case "ITEM-004":
                    item = new RuneSword();
                    Configure(item, "Rune Sword", 0x26CE, 0, Layer.OneHanded, "Artefact", amount);
                    break;
                case "ITEM-005":
                    item = new TridentofAtlantis();
                    Configure(item, "Trident of Atlantis", 0xE87, 2843, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-006":
                    item = new SwordofDarkness();
                    Configure(item, "Sword of Darkness", 0x26CE, 1175, Layer.OneHanded, "Artefact", amount);
                    break;
                case "ITEM-007":
                    item = new SwordofLight();
                    Configure(item, "Sword of Light", 0x26CE, 1152, Layer.OneHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-008":
                    item = new WolfgangSword();
                    Configure(item, "Sword of Kings", 0x27A2, 2843, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-009":
                    item = new GuardianShield();
                    Configure(item, "Shield of the Guardian", 0x1B74, 1152, Layer.TwoHanded, "Artefact", amount);
                    break;
                case "ITEM-010":
                    item = new Weightoftheworld();
                    Configure(item, "Weight of the World", 0x27A6, 0, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-011":
                    item = new DeathPenalty();
                    Configure(item, "Death Penalty", 0xF50, 1175, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-012":
                    item = new Disintegrator();
                    Configure(item, "Disintegrator", 0x3F65, 0, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-013":
                    item = new RobeoftheVoid();
                    Configure(item, "Robe of the Void", 0x1F03, 1175, Layer.OuterTorso, "Legendary Artefact", amount);
                    break;
                case "ITEM-014":
                    item = new DreadMace();
                    Configure(item, "The Dread Mace", 0x2681, 1175, Layer.OneHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-015":
                    item = new ArmoroftheTitans();
                    Configure(item, "Titan Slayer Armor", 0x2B08, 2843, Layer.InnerTorso, "Artefact", amount);
                    break;
                case "ITEM-016":
                    item = new ArmorofthefalseGods();
                    Configure(item, "God Slayer Armor", 0x2B08, 1152, Layer.InnerTorso, "Legendary Artefact", amount);
                    break;
                case "ITEM-017":
                    item = new ExodusArmor();
                    Configure(item, "Exodus Ribcage", 0x144F, 1175, Layer.InnerTorso, "Legendary Artefact", amount);
                    break;
                case "ITEM-018":
                    item = new ExodusMaul();
                    Configure(item, "Exodus Spine", 0x27A6, 1175, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-019":
                    item = new ExodusHead();
                    Configure(item, "Exodus Skull", 0x1451, 1175, Layer.Helm, "Legendary Artefact", amount);
                    break;
                case "ITEM-020":
                    item = new ExodusHands();
                    Configure(item, "Exodus Claws", 0x2643, 1175, Layer.Gloves, "Legendary Artefact", amount);
                    break;
                case "ITEM-021":
                    item = new HeartofFire();
                    Configure(item, "Heart of Fire", 0x4D16, 2227, Layer.Talisman, "Legendary Artefact", amount);
                    break;
                case "ITEM-022":
                    item = new LampofBadWishes();
                    Configure(item, "Lamp of Bad Wishes", 0x2C82, 2843, Layer.Talisman, "Legendary Artefact", amount);
                    break;
                case "ITEM-023":
                    item = new CrystalToken();
                    Configure(item, "Crystal Token", 0x1E85, 1152, Layer.Invalid, null, amount);
                    break;
                case "ITEM-024":
                    item = new LoreBook();
                    Configure(item, "Lost Tome", 0x5689, 0, Layer.Invalid, null, amount);
                    break;
                case "ITEM-025":
                    item = new TrulyRareFish();
                    Configure(item, "Truly Rare Fish", 0xDD6, 0, Layer.Invalid, null, amount);
                    break;
                case "ITEM-026":
                    item = new EvilSkull();
                    Configure(item, "Evil Skull", 0x1AE1, 2227, Layer.Invalid, null, amount);
                    break;
                case "ITEM-027":
                    item = new PowderOfTemperament();
                    Configure(item, "Powder of Fortifying", 0x1006, 0, Layer.Invalid, null, amount);
                    break;
                case "ITEM-028":
                    item = new SuperPotion();
                    Configure(item, "Superior Potion", 0x180F, 1152, Layer.Invalid, null, amount);
                    break;
                case "ITEM-029":
                    item = new ShadowToken();
                    Configure(item, "Shadow Token", 0xF19, 1175, Layer.Invalid, null, amount);
                    break;
                case "ITEM-030":
                    item = new OrangePetals();
                    Configure(item, "Orange Petals", 0x1E85, 2227, Layer.Invalid, null, amount);
                    break;
                case "ITEM-031":
                    item = new RoseOfMoonPetal();
                    Configure(item, "Petal of the Rose of Trinsic", 0x1E85, 2227, Layer.Invalid, null, amount);
                    break;
                case "ITEM-032":
                    item = new PhoenixFeather(amount);
                    Configure(item, "Pheonix Feather", 0x1E85, 2227, Layer.Invalid, null, amount);
                    break;
                case "ITEM-033":
                    item = new JewelImmortality();
                    Configure(item, "Jewel of Immortality", 0xF19, 2227, Layer.Invalid, null, amount);
                    break;
                case "ITEM-034":
                    item = new RedLeaves();
                    Configure(item, "Red Leaves", 0x1E85, 2227, Layer.Invalid, null, amount);
                    break;
                case "ITEM-035":
                    item = new EnchantedSextant();
                    Configure(item, "Magic Telescope", 0x5308, 2843, Layer.Invalid, "Artefact", amount);
                    break;
                case "ITEM-036":
                    item = new DoomBlade();
                    Configure(item, "Doom Blade", 0x27A2, 1175, Layer.TwoHanded, "Legendary Artefact", amount);
                    break;
                case "ITEM-037":
                    item = new MetalPigmentsOfIslesDread();
                    Configure(item, "Pigments of Tokuno", 0x180F, 0, Layer.Invalid, null, amount);
                    break;
                case "ITEM-038":
                    item = new CensusRecords();
                    Configure(item, "Legendary Registry of Heroes", 0x5356, 0, Layer.Invalid, "Artefact", amount);
                    break;
                case "ITEM-039":
                    item = new ClothingBlessDeed();
                    Configure(item, "A Clothing Bless Deed", 0x14F0, 0, Layer.Invalid, null, amount);
                    break;
                default:
                    return null;
            }

            return item;
        }

        private static void Configure(
            Item item,
            string name,
            int itemID,
            int hue,
            Layer layer,
            string propertyLabel,
            int amount
        )
        {
            MobileBalanceItemSupport.Initialize(item, name, itemID, hue, layer, propertyLabel);
            item.Amount = amount;
        }
    }
}
