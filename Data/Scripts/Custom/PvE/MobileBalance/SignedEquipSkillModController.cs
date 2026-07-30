using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Custom.Confictura.PvE.MobileBalance
{
    internal sealed class SignedSkillModDefinition
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

        public SignedSkillModDefinition(SkillName skill, double value)
        {
            m_Skill = skill;
            m_Value = value;
        }
    }

    internal static class SignedEquipSkillModController
    {
        private static readonly Dictionary<Item, List<SkillMod>> m_ActiveMods =
            new Dictionary<Item, List<SkillMod>>();

        public static void Apply(Item item, object parent)
        {
            Remove(item);

            Mobile mobile = parent as Mobile;
            SignedSkillModDefinition[] definitions = GetDefinitions(item);

            if (mobile == null || definitions == null || item.Deleted)
            {
                return;
            }

            List<SkillMod> mods = new List<SkillMod>();

            for (int i = 0; i < definitions.Length; ++i)
            {
                SignedSkillModDefinition definition = definitions[i];
                EquipedSkillMod mod = new EquipedSkillMod(
                    definition.Skill,
                    true,
                    definition.Value,
                    item,
                    mobile
                );

                mod.ObeyCap = false;
                mobile.AddSkillMod(mod);
                mods.Add(mod);
            }

            m_ActiveMods[item] = mods;
        }

        public static void Remove(Item item)
        {
            List<SkillMod> mods;

            if (item == null || !m_ActiveMods.TryGetValue(item, out mods))
            {
                return;
            }

            m_ActiveMods.Remove(item);

            for (int i = 0; i < mods.Count; ++i)
            {
                mods[i].Remove();
            }
        }

        public static void Rehydrate(Item item)
        {
            new RehydrateTimer(item).Start();
        }

        public static void AddProperties(Item item, ObjectPropertyList list)
        {
            SignedSkillModDefinition[] definitions = GetDefinitions(item);

            if (definitions == null)
            {
                return;
            }

            for (int i = 0; i < definitions.Length; ++i)
            {
                SignedSkillModDefinition definition = definitions[i];
                string value = definition.Value >= 0
                    ? "+" + definition.Value.ToString("0.##")
                    : definition.Value.ToString("0.##");

                list.Add(definition.Skill + ": " + value);
            }
        }

        private static SignedSkillModDefinition[] GetDefinitions(Item item)
        {
            if (item is Custom.Confictura.Items.MindOverMatterTalisman)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Magery, 100),
                    new SignedSkillModDefinition(SkillName.Parry, -100),
                    new SignedSkillModDefinition(SkillName.Psychology, 100),
                };
            }

            if (item is Custom.Confictura.Items.RuneSword)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Elementalism, -100),
                    new SignedSkillModDefinition(SkillName.Magery, -100),
                    new SignedSkillModDefinition(SkillName.Necromancy, -100),
                };
            }

            if (item is Custom.Confictura.Items.TridentofAtlantis)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Fencing, 50),
                    new SignedSkillModDefinition(SkillName.Seafaring, 100),
                    new SignedSkillModDefinition(SkillName.Hiding, -100),
                };
            }

            if (item is Custom.Confictura.Items.SwordofDarkness)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Knightship, -100),
                    new SignedSkillModDefinition(SkillName.Necromancy, 100),
                    new SignedSkillModDefinition(SkillName.Herding, -100),
                };
            }

            if (item is Custom.Confictura.Items.SwordofLight)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Knightship, 100),
                    new SignedSkillModDefinition(SkillName.Snooping, -100),
                    new SignedSkillModDefinition(SkillName.Stealth, -100),
                };
            }

            if (item is Custom.Confictura.Items.GuardianShield)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Tactics, 50),
                    new SignedSkillModDefinition(SkillName.Anatomy, -100),
                    new SignedSkillModDefinition(SkillName.MagicResist, 50),
                };
            }

            if (item is Custom.Confictura.Items.DeathPenalty)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Anatomy, 30),
                    new SignedSkillModDefinition(SkillName.Parry, -10),
                    new SignedSkillModDefinition(SkillName.Taming, -100),
                    new SignedSkillModDefinition(SkillName.Herding, -100),
                };
            }

            if (item is Custom.Confictura.Items.Disintegrator)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Anatomy, 50),
                    new SignedSkillModDefinition(SkillName.Parry, -10),
                };
            }

            if (item is Custom.Confictura.Items.RobeoftheVoid)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Taming, -100),
                    new SignedSkillModDefinition(SkillName.Magery, 20),
                    new SignedSkillModDefinition(SkillName.Necromancy, 20),
                    new SignedSkillModDefinition(SkillName.Parry, -10),
                    new SignedSkillModDefinition(SkillName.Herding, -100),
                    new SignedSkillModDefinition(SkillName.MagicResist, 50),
                };
            }

            if (item is Custom.Confictura.Items.DreadMace)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Necromancy, 10),
                    new SignedSkillModDefinition(SkillName.Poisoning, 10),
                    new SignedSkillModDefinition(SkillName.Knightship, -10),
                };
            }

            if (item is Custom.Confictura.Items.ArmoroftheTitans)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Knightship, 10),
                    new SignedSkillModDefinition(SkillName.Hiding, -100),
                    new SignedSkillModDefinition(SkillName.Blacksmith, 50),
                };
            }

            if (item is Custom.Confictura.Items.ArmorofthefalseGods)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Tactics, 50),
                    new SignedSkillModDefinition(SkillName.Hiding, -10),
                    new SignedSkillModDefinition(SkillName.Anatomy, 50),
                    new SignedSkillModDefinition(SkillName.Alchemy, 50),
                    new SignedSkillModDefinition(SkillName.Parry, 100),
                };
            }

            if (item is Custom.Confictura.Items.ExodusArmor)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Parry, -20),
                    new SignedSkillModDefinition(SkillName.Necromancy, 20),
                    new SignedSkillModDefinition(SkillName.Anatomy, 50),
                };
            }

            if (item is Custom.Confictura.Items.ExodusMaul)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Parry, -20),
                    new SignedSkillModDefinition(SkillName.Anatomy, 20),
                };
            }

            if (item is Custom.Confictura.Items.ExodusHead)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Parry, -20),
                    new SignedSkillModDefinition(SkillName.Psychology, 30),
                    new SignedSkillModDefinition(SkillName.Taming, -100),
                };
            }

            if (item is Custom.Confictura.Items.ExodusHands)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Parry, -20),
                    new SignedSkillModDefinition(SkillName.Anatomy, 30),
                };
            }

            if (item is Custom.Confictura.Items.HeartofFire)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.MagicResist, -20),
                    new SignedSkillModDefinition(SkillName.Anatomy, 20),
                    new SignedSkillModDefinition(SkillName.Psychology, 100),
                    new SignedSkillModDefinition(SkillName.Taming, -100),
                    new SignedSkillModDefinition(SkillName.Stealth, -100),
                };
            }

            if (item is Custom.Confictura.Items.LampofBadWishes)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.MagicResist, -50),
                    new SignedSkillModDefinition(SkillName.Parry, -10),
                    new SignedSkillModDefinition(SkillName.Psychology, 100),
                    new SignedSkillModDefinition(SkillName.Stealth, -100),
                    new SignedSkillModDefinition(SkillName.Hiding, -100),
                    new SignedSkillModDefinition(SkillName.Herding, -100),
                    new SignedSkillModDefinition(SkillName.Anatomy, 50),
                    new SignedSkillModDefinition(SkillName.Tactics, 100),
                    new SignedSkillModDefinition(SkillName.Mercantile, 50),
                };
            }

            if (item is Custom.Confictura.Items.DoomBlade)
            {
                return new SignedSkillModDefinition[]
                {
                    new SignedSkillModDefinition(SkillName.Anatomy, 100),
                    new SignedSkillModDefinition(SkillName.MagicResist, -20),
                    new SignedSkillModDefinition(SkillName.Parry, -50),
                    new SignedSkillModDefinition(SkillName.Hiding, -100),
                    new SignedSkillModDefinition(SkillName.Bushido, 50),
                };
            }

            return null;
        }

        private sealed class RehydrateTimer : Timer
        {
            private readonly Item m_Item;

            public RehydrateTimer(Item item)
                : base(TimeSpan.Zero)
            {
                m_Item = item;
            }

            protected override void OnTick()
            {
                if (m_Item != null && !m_Item.Deleted)
                {
                    Apply(m_Item, m_Item.Parent);
                }
            }
        }
    }

    internal static class MobileBalanceItemSupport
    {
        public static void ApplyMetadata(
            Item item,
            string name,
            int itemID,
            int hue,
            Layer layer,
            string propertyLabel
        )
        {
            item.Name = name;
            item.ItemID = itemID;
            item.Hue = hue;
            item.Layer = layer;
            item.LootType = LootType.Regular;
            item.Enchantment = propertyLabel;
        }

        public static void ClearRandomTalismanProperties(MagicTalisman talisman)
        {
            for (int i = 0; i < 5; ++i)
            {
                talisman.SkillBonuses.SetValues(i, SkillName.Alchemy, 0.0);
            }

            talisman.Attributes.NightSight = 0;
        }
    }
}
