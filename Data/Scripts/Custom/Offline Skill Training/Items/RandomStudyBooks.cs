using System;
using System.Collections.Generic;

namespace Server.Items
{
    public class AdvancedRandomStudyBook : StudyBook
    {
        private static readonly SkillName[] m_Skills = new SkillName[]
        {
            SkillName.Blacksmith,
            SkillName.Tailoring,
            SkillName.Swords,
            SkillName.Fencing,
            SkillName.Bludgeoning,
            SkillName.Marksmanship,
            SkillName.FistFighting,
            SkillName.Parry,
            SkillName.Tactics,
            SkillName.Anatomy,
            SkillName.Healing,
            SkillName.Magery,
            SkillName.Meditation,
            SkillName.Psychology,
            SkillName.MagicResist,
            SkillName.Taming,
            SkillName.Druidism,
            SkillName.Veterinary,
            SkillName.Musicianship,
            SkillName.Provocation,
            SkillName.Discordance,
            SkillName.Peacemaking,
            SkillName.Knightship,
            SkillName.Focus,
            SkillName.Necromancy,
            SkillName.Stealing,
            SkillName.Stealth,
            SkillName.Spiritualism,
            SkillName.Ninjitsu,
            SkillName.Bushido,
            SkillName.Elementalism,
            SkillName.Alchemy,
            SkillName.Cooking,
            SkillName.Seafaring,
            SkillName.Lumberjacking,
            SkillName.Mining,
            SkillName.Tinkering,
            SkillName.Begging,
            SkillName.Forensics,
            SkillName.Mercantile,
            SkillName.Tasting,
            SkillName.Camping,
            SkillName.Hiding,
            SkillName.Inscribe,
            SkillName.Searching,
            SkillName.RemoveTrap,
            SkillName.Lockpicking,
            SkillName.Poisoning,
            SkillName.Snooping,
            SkillName.Cartography,
            SkillName.Herding,
            SkillName.Tracking
        };
        private static readonly List<SkillName> _Skills = new List<SkillName>();
        public static List<SkillName> Skills
        {
            get
            {
                if (_Skills.Count == 0)
                {
                    _Skills.AddRange(m_Skills);
                }
                return _Skills;
            }
        }

        [Constructable]
        public AdvancedRandomStudyBook()
            : base(Skills[Utility.Random(Skills.Count)], 1000) { }

        public AdvancedRandomStudyBook(Serial serial)
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

    public class LegendaryRandomStudyBook : StudyBook
    {
        private static readonly SkillName[] m_Skills = new SkillName[]
        {
            SkillName.Blacksmith,
            SkillName.Tailoring,
            SkillName.Swords,
            SkillName.Fencing,
            SkillName.Bludgeoning,
            SkillName.Marksmanship,
            SkillName.FistFighting,
            SkillName.Parry,
            SkillName.Tactics,
            SkillName.Anatomy,
            SkillName.Healing,
            SkillName.Magery,
            SkillName.Meditation,
            SkillName.Psychology,
            SkillName.MagicResist,
            SkillName.Taming,
            SkillName.Druidism,
            SkillName.Veterinary,
            SkillName.Musicianship,
            SkillName.Provocation,
            SkillName.Discordance,
            SkillName.Peacemaking,
            SkillName.Knightship,
            SkillName.Focus,
            SkillName.Necromancy,
            SkillName.Stealing,
            SkillName.Stealth,
            SkillName.Spiritualism,
            SkillName.Ninjitsu,
            SkillName.Bushido,
            SkillName.Elementalism,
            SkillName.Alchemy,
            SkillName.Cooking,
            SkillName.Seafaring,
            SkillName.Lumberjacking,
            SkillName.Mining,
            SkillName.Tinkering,
            SkillName.Begging,
            SkillName.Forensics,
            SkillName.Mercantile,
            SkillName.Tasting,
            SkillName.Camping,
            SkillName.Hiding,
            SkillName.Inscribe,
            SkillName.Searching,
            SkillName.RemoveTrap,
            SkillName.Lockpicking,
            SkillName.Poisoning,
            SkillName.Snooping,
            SkillName.Cartography,
            SkillName.Herding,
            SkillName.Tracking
        };
        private static readonly List<SkillName> _Skills = new List<SkillName>();
        public static List<SkillName> Skills
        {
            get
            {
                if (_Skills.Count == 0)
                {
                    _Skills.AddRange(m_Skills);
                }
                return _Skills;
            }
        }

        [Constructable]
        public LegendaryRandomStudyBook()
            : base(Skills[Utility.Random(Skills.Count)], 1200) { }

        public LegendaryRandomStudyBook(Serial serial)
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

    public class StandardRandomStudyBook : StudyBook
    {
        private static readonly SkillName[] m_Skills = new SkillName[]
        {
            SkillName.Blacksmith,
            SkillName.Tailoring,
            SkillName.Swords,
            SkillName.Fencing,
            SkillName.Bludgeoning,
            SkillName.Marksmanship,
            SkillName.FistFighting,
            SkillName.Parry,
            SkillName.Tactics,
            SkillName.Anatomy,
            SkillName.Healing,
            SkillName.Magery,
            SkillName.Meditation,
            SkillName.Psychology,
            SkillName.MagicResist,
            SkillName.Taming,
            SkillName.Druidism,
            SkillName.Veterinary,
            SkillName.Musicianship,
            SkillName.Provocation,
            SkillName.Discordance,
            SkillName.Peacemaking,
            SkillName.Knightship,
            SkillName.Focus,
            SkillName.Necromancy,
            SkillName.Stealing,
            SkillName.Stealth,
            SkillName.Spiritualism,
            SkillName.Ninjitsu,
            SkillName.Bushido,
            SkillName.Elementalism,
            SkillName.Alchemy,
            SkillName.Cooking,
            SkillName.Seafaring,
            SkillName.Lumberjacking,
            SkillName.Mining,
            SkillName.Tinkering,
            SkillName.Begging,
            SkillName.Forensics,
            SkillName.Mercantile,
            SkillName.Tasting,
            SkillName.Camping,
            SkillName.Hiding,
            SkillName.Inscribe,
            SkillName.Searching,
            SkillName.RemoveTrap,
            SkillName.Lockpicking,
            SkillName.Poisoning,
            SkillName.Snooping,
            SkillName.Cartography,
            SkillName.Herding,
            SkillName.Tracking
        };
        private static readonly List<SkillName> _Skills = new List<SkillName>();
        public static List<SkillName> Skills
        {
            get
            {
                if (_Skills.Count == 0)
                {
                    _Skills.AddRange(m_Skills);
                }
                return _Skills;
            }
        }

        [Constructable]
        public StandardRandomStudyBook()
            : base(Skills[Utility.Random(Skills.Count)], 700) { }

        public StandardRandomStudyBook(Serial serial)
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
