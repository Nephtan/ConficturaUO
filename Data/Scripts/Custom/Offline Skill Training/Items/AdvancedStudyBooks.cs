using System;

namespace Server.Items
{
    public class AdvancedAlchemyStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedAlchemyStudyBook()
            : base(SkillName.Alchemy, 1000) { }

        public AdvancedAlchemyStudyBook(Serial serial)
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

    public class AdvancedAnatomyStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedAnatomyStudyBook()
            : base(SkillName.Anatomy, 1000) { }

        public AdvancedAnatomyStudyBook(Serial serial)
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

    public class AdvancedDruidismStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedDruidismStudyBook()
            : base(SkillName.Druidism, 1000) { }

        public AdvancedDruidismStudyBook(Serial serial)
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

    public class AdvancedMercantileStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMercantileStudyBook()
            : base(SkillName.Mercantile, 1000) { }

        public AdvancedMercantileStudyBook(Serial serial)
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

    public class AdvancedArmsLoreStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedArmsLoreStudyBook()
            : base(SkillName.ArmsLore, 1000) { }

        public AdvancedArmsLoreStudyBook(Serial serial)
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

    public class AdvancedParryStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedParryStudyBook()
            : base(SkillName.Parry, 1000) { }

        public AdvancedParryStudyBook(Serial serial)
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

    public class AdvancedBeggingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedBeggingStudyBook()
            : base(SkillName.Begging, 1000) { }

        public AdvancedBeggingStudyBook(Serial serial)
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

    public class AdvancedBlacksmithStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedBlacksmithStudyBook()
            : base(SkillName.Blacksmith, 1000) { }

        public AdvancedBlacksmithStudyBook(Serial serial)
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

    public class AdvancedBowcraftStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedBowcraftStudyBook()
            : base(SkillName.Bowcraft, 1000) { }

        public AdvancedBowcraftStudyBook(Serial serial)
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

    public class AdvancedPeacemakingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedPeacemakingStudyBook()
            : base(SkillName.Peacemaking, 1000) { }

        public AdvancedPeacemakingStudyBook(Serial serial)
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

    public class AdvancedCampingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedCampingStudyBook()
            : base(SkillName.Camping, 1000) { }

        public AdvancedCampingStudyBook(Serial serial)
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

    public class AdvancedCarpentryStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedCarpentryStudyBook()
            : base(SkillName.Carpentry, 1000) { }

        public AdvancedCarpentryStudyBook(Serial serial)
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

    public class AdvancedCartographyStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedCartographyStudyBook()
            : base(SkillName.Cartography, 1000) { }

        public AdvancedCartographyStudyBook(Serial serial)
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

    public class AdvancedCookingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedCookingStudyBook()
            : base(SkillName.Cooking, 1000) { }

        public AdvancedCookingStudyBook(Serial serial)
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

    public class AdvancedSearchingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedSearchingStudyBook()
            : base(SkillName.Searching, 1000) { }

        public AdvancedSearchingStudyBook(Serial serial)
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

    public class AdvancedDiscordanceStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedDiscordanceStudyBook()
            : base(SkillName.Discordance, 1000) { }

        public AdvancedDiscordanceStudyBook(Serial serial)
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

    public class AdvancedPsychologyStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedPsychologyStudyBook()
            : base(SkillName.Psychology, 1000) { }

        public AdvancedPsychologyStudyBook(Serial serial)
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

    public class AdvancedHealingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedHealingStudyBook()
            : base(SkillName.Healing, 1000) { }

        public AdvancedHealingStudyBook(Serial serial)
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

    public class AdvancedSeafaringStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedSeafaringStudyBook()
            : base(SkillName.Seafaring, 1000) { }

        public AdvancedSeafaringStudyBook(Serial serial)
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

    public class AdvancedForensicsStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedForensicsStudyBook()
            : base(SkillName.Forensics, 1000) { }

        public AdvancedForensicsStudyBook(Serial serial)
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

    public class AdvancedHerdingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedHerdingStudyBook()
            : base(SkillName.Herding, 1000) { }

        public AdvancedHerdingStudyBook(Serial serial)
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

    public class AdvancedHidingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedHidingStudyBook()
            : base(SkillName.Hiding, 1000) { }

        public AdvancedHidingStudyBook(Serial serial)
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

    public class AdvancedProvocationStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedProvocationStudyBook()
            : base(SkillName.Provocation, 1000) { }

        public AdvancedProvocationStudyBook(Serial serial)
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

    public class AdvancedInscribeStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedInscribeStudyBook()
            : base(SkillName.Inscribe, 1000) { }

        public AdvancedInscribeStudyBook(Serial serial)
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

    public class AdvancedLockpickingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedLockpickingStudyBook()
            : base(SkillName.Lockpicking, 1000) { }

        public AdvancedLockpickingStudyBook(Serial serial)
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

    public class AdvancedMageryStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMageryStudyBook()
            : base(SkillName.Magery, 1000) { }

        public AdvancedMageryStudyBook(Serial serial)
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

    public class AdvancedMagicResistStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMagicResistStudyBook()
            : base(SkillName.MagicResist, 1000) { }

        public AdvancedMagicResistStudyBook(Serial serial)
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

    public class AdvancedTacticsStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedTacticsStudyBook()
            : base(SkillName.Tactics, 1000) { }

        public AdvancedTacticsStudyBook(Serial serial)
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

    public class AdvancedSnoopingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedSnoopingStudyBook()
            : base(SkillName.Snooping, 1000) { }

        public AdvancedSnoopingStudyBook(Serial serial)
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

    public class AdvancedMusicianshipStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMusicianshipStudyBook()
            : base(SkillName.Musicianship, 1000) { }

        public AdvancedMusicianshipStudyBook(Serial serial)
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

    public class AdvancedPoisoningStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedPoisoningStudyBook()
            : base(SkillName.Poisoning, 1000) { }

        public AdvancedPoisoningStudyBook(Serial serial)
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

    public class AdvancedMarksmanshipStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMarksmanshipStudyBook()
            : base(SkillName.Marksmanship, 1000) { }

        public AdvancedMarksmanshipStudyBook(Serial serial)
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

    public class AdvancedSpiritualismStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedSpiritualismStudyBook()
            : base(SkillName.Spiritualism, 1000) { }

        public AdvancedSpiritualismStudyBook(Serial serial)
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

    public class AdvancedStealingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedStealingStudyBook()
            : base(SkillName.Stealing, 1000) { }

        public AdvancedStealingStudyBook(Serial serial)
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

    public class AdvancedTailoringStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedTailoringStudyBook()
            : base(SkillName.Tailoring, 1000) { }

        public AdvancedTailoringStudyBook(Serial serial)
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

    public class AdvancedTamingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedTamingStudyBook()
            : base(SkillName.Taming, 1000) { }

        public AdvancedTamingStudyBook(Serial serial)
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

    public class AdvancedTastingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedTastingStudyBook()
            : base(SkillName.Tasting, 1000) { }

        public AdvancedTastingStudyBook(Serial serial)
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

    public class AdvancedTinkeringStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedTinkeringStudyBook()
            : base(SkillName.Tinkering, 1000) { }

        public AdvancedTinkeringStudyBook(Serial serial)
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

    public class AdvancedTrackingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedTrackingStudyBook()
            : base(SkillName.Tracking, 1000) { }

        public AdvancedTrackingStudyBook(Serial serial)
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

    public class AdvancedVeterinaryStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedVeterinaryStudyBook()
            : base(SkillName.Veterinary, 1000) { }

        public AdvancedVeterinaryStudyBook(Serial serial)
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

    public class AdvancedSwordsStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedSwordsStudyBook()
            : base(SkillName.Swords, 1000) { }

        public AdvancedSwordsStudyBook(Serial serial)
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

    public class AdvancedBludgeoningStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedBludgeoningStudyBook()
            : base(SkillName.Bludgeoning, 1000) { }

        public AdvancedBludgeoningStudyBook(Serial serial)
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

    public class AdvancedFencingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedFencingStudyBook()
            : base(SkillName.Fencing, 1000) { }

        public AdvancedFencingStudyBook(Serial serial)
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

    public class AdvancedFistFightingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedFistFightingStudyBook()
            : base(SkillName.FistFighting, 1000) { }

        public AdvancedFistFightingStudyBook(Serial serial)
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

    public class AdvancedLumberjackingStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedLumberjackingStudyBook()
            : base(SkillName.Lumberjacking, 1000) { }

        public AdvancedLumberjackingStudyBook(Serial serial)
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

    public class AdvancedMiningStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMiningStudyBook()
            : base(SkillName.Mining, 1000) { }

        public AdvancedMiningStudyBook(Serial serial)
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

    public class AdvancedMeditationStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedMeditationStudyBook()
            : base(SkillName.Meditation, 1000) { }

        public AdvancedMeditationStudyBook(Serial serial)
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

    public class AdvancedStealthStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedStealthStudyBook()
            : base(SkillName.Stealth, 1000) { }

        public AdvancedStealthStudyBook(Serial serial)
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

    public class AdvancedRemoveTrapStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedRemoveTrapStudyBook()
            : base(SkillName.RemoveTrap, 1000) { }

        public AdvancedRemoveTrapStudyBook(Serial serial)
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

    public class AdvancedNecromancyStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedNecromancyStudyBook()
            : base(SkillName.Necromancy, 1000) { }

        public AdvancedNecromancyStudyBook(Serial serial)
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

    public class AdvancedFocusStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedFocusStudyBook()
            : base(SkillName.Focus, 1000) { }

        public AdvancedFocusStudyBook(Serial serial)
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

    public class AdvancedKnightshipStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedKnightshipStudyBook()
            : base(SkillName.Knightship, 1000) { }

        public AdvancedKnightshipStudyBook(Serial serial)
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

    public class AdvancedBushidoStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedBushidoStudyBook()
            : base(SkillName.Bushido, 1000) { }

        public AdvancedBushidoStudyBook(Serial serial)
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

    public class AdvancedNinjitsuStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedNinjitsuStudyBook()
            : base(SkillName.Ninjitsu, 1000) { }

        public AdvancedNinjitsuStudyBook(Serial serial)
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

    public class AdvancedElementalismStudyBook : StudyBook
    {
        [Constructable]
        public AdvancedElementalismStudyBook()
            : base(SkillName.Elementalism, 1000) { }

        public AdvancedElementalismStudyBook(Serial serial)
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

    //public class AdvancedMysticismStudyBook : StudyBook
    //   {
    //       [Constructable]
    //       public AdvancedMysticismStudyBook()
    //           : base(SkillName.Mysticism, 1000)
    //       {
    //       }

    //       public AdvancedMysticismStudyBook(Serial serial)
    //           : base(serial)
    //       {
    //       }

    //       public override void Serialize(GenericWriter writer)
    //       {
    //           base.Serialize(writer);

    //           writer.Write((int)0); // version
    //       }

    //       public override void Deserialize(GenericReader reader)
    //       {
    //           base.Deserialize(reader);

    //           int version = reader.ReadInt();
    //       }
    //   }

    //public class AdvancedImbuingStudyBook : StudyBook
    //   {
    //       [Constructable]
    //       public AdvancedImbuingStudyBook()
    //           : base(SkillName.Imbuing, 1000)
    //       {
    //       }

    //       public AdvancedImbuingStudyBook(Serial serial)
    //           : base(serial)
    //       {
    //       }

    //       public override void Serialize(GenericWriter writer)
    //       {
    //           base.Serialize(writer);

    //           writer.Write((int)0); // version
    //       }

    //       public override void Deserialize(GenericReader reader)
    //       {
    //           base.Deserialize(reader);

    //           int version = reader.ReadInt();
    //       }
    //   }

    //public class AdvancedThrowingStudyBook : StudyBook
    //   {
    //       [Constructable]
    //       public AdvancedThrowingStudyBook()
    //           : base(SkillName.Throwing, 1000)
    //       {
    //       }

    //       public AdvancedThrowingStudyBook(Serial serial)
    //           : base(serial)
    //       {
    //       }

    //       public override void Serialize(GenericWriter writer)
    //       {
    //           base.Serialize(writer);

    //           writer.Write((int)0); // version
    //       }

    //       public override void Deserialize(GenericReader reader)
    //       {
    //           base.Deserialize(reader);

    //           int version = reader.ReadInt();
    //       }
    //   }
}
