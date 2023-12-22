using System;

namespace Server.Items
{
    public class StandardAlchemyStudyBook : StudyBook
    {
        [Constructable]
        public StandardAlchemyStudyBook()
            : base(SkillName.Alchemy, 700) { }

        public StandardAlchemyStudyBook(Serial serial)
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

    public class StandardAnatomyStudyBook : StudyBook
    {
        [Constructable]
        public StandardAnatomyStudyBook()
            : base(SkillName.Anatomy, 700) { }

        public StandardAnatomyStudyBook(Serial serial)
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

    public class StandardDruidismStudyBook : StudyBook
    {
        [Constructable]
        public StandardDruidismStudyBook()
            : base(SkillName.Druidism, 700) { }

        public StandardDruidismStudyBook(Serial serial)
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

    public class StandardMercantileStudyBook : StudyBook
    {
        [Constructable]
        public StandardMercantileStudyBook()
            : base(SkillName.Mercantile, 700) { }

        public StandardMercantileStudyBook(Serial serial)
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

    public class StandardArmsLoreStudyBook : StudyBook
    {
        [Constructable]
        public StandardArmsLoreStudyBook()
            : base(SkillName.ArmsLore, 700) { }

        public StandardArmsLoreStudyBook(Serial serial)
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

    public class StandardParryStudyBook : StudyBook
    {
        [Constructable]
        public StandardParryStudyBook()
            : base(SkillName.Parry, 700) { }

        public StandardParryStudyBook(Serial serial)
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

    public class StandardBeggingStudyBook : StudyBook
    {
        [Constructable]
        public StandardBeggingStudyBook()
            : base(SkillName.Begging, 700) { }

        public StandardBeggingStudyBook(Serial serial)
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

    public class StandardBlacksmithStudyBook : StudyBook
    {
        [Constructable]
        public StandardBlacksmithStudyBook()
            : base(SkillName.Blacksmith, 700) { }

        public StandardBlacksmithStudyBook(Serial serial)
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

    public class StandardBowcraftStudyBook : StudyBook
    {
        [Constructable]
        public StandardBowcraftStudyBook()
            : base(SkillName.Bowcraft, 700) { }

        public StandardBowcraftStudyBook(Serial serial)
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

    public class StandardPeacemakingStudyBook : StudyBook
    {
        [Constructable]
        public StandardPeacemakingStudyBook()
            : base(SkillName.Peacemaking, 700) { }

        public StandardPeacemakingStudyBook(Serial serial)
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

    public class StandardCampingStudyBook : StudyBook
    {
        [Constructable]
        public StandardCampingStudyBook()
            : base(SkillName.Camping, 700) { }

        public StandardCampingStudyBook(Serial serial)
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

    public class StandardCarpentryStudyBook : StudyBook
    {
        [Constructable]
        public StandardCarpentryStudyBook()
            : base(SkillName.Carpentry, 700) { }

        public StandardCarpentryStudyBook(Serial serial)
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

    public class StandardCartographyStudyBook : StudyBook
    {
        [Constructable]
        public StandardCartographyStudyBook()
            : base(SkillName.Cartography, 700) { }

        public StandardCartographyStudyBook(Serial serial)
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

    public class StandardCookingStudyBook : StudyBook
    {
        [Constructable]
        public StandardCookingStudyBook()
            : base(SkillName.Cooking, 700) { }

        public StandardCookingStudyBook(Serial serial)
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

    public class StandardSearchingStudyBook : StudyBook
    {
        [Constructable]
        public StandardSearchingStudyBook()
            : base(SkillName.Searching, 700) { }

        public StandardSearchingStudyBook(Serial serial)
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

    public class StandardDiscordanceStudyBook : StudyBook
    {
        [Constructable]
        public StandardDiscordanceStudyBook()
            : base(SkillName.Discordance, 700) { }

        public StandardDiscordanceStudyBook(Serial serial)
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

    public class StandardPsychologyStudyBook : StudyBook
    {
        [Constructable]
        public StandardPsychologyStudyBook()
            : base(SkillName.Psychology, 700) { }

        public StandardPsychologyStudyBook(Serial serial)
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

    public class StandardHealingStudyBook : StudyBook
    {
        [Constructable]
        public StandardHealingStudyBook()
            : base(SkillName.Healing, 700) { }

        public StandardHealingStudyBook(Serial serial)
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

    public class StandardSeafaringStudyBook : StudyBook
    {
        [Constructable]
        public StandardSeafaringStudyBook()
            : base(SkillName.Seafaring, 700) { }

        public StandardSeafaringStudyBook(Serial serial)
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

    public class StandardForensicsStudyBook : StudyBook
    {
        [Constructable]
        public StandardForensicsStudyBook()
            : base(SkillName.Forensics, 700) { }

        public StandardForensicsStudyBook(Serial serial)
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

    public class StandardHerdingStudyBook : StudyBook
    {
        [Constructable]
        public StandardHerdingStudyBook()
            : base(SkillName.Herding, 700) { }

        public StandardHerdingStudyBook(Serial serial)
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

    public class StandardHidingStudyBook : StudyBook
    {
        [Constructable]
        public StandardHidingStudyBook()
            : base(SkillName.Hiding, 700) { }

        public StandardHidingStudyBook(Serial serial)
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

    public class StandardProvocationStudyBook : StudyBook
    {
        [Constructable]
        public StandardProvocationStudyBook()
            : base(SkillName.Provocation, 700) { }

        public StandardProvocationStudyBook(Serial serial)
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

    public class StandardInscribeStudyBook : StudyBook
    {
        [Constructable]
        public StandardInscribeStudyBook()
            : base(SkillName.Inscribe, 700) { }

        public StandardInscribeStudyBook(Serial serial)
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

    public class StandardLockpickingStudyBook : StudyBook
    {
        [Constructable]
        public StandardLockpickingStudyBook()
            : base(SkillName.Lockpicking, 700) { }

        public StandardLockpickingStudyBook(Serial serial)
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

    public class StandardMageryStudyBook : StudyBook
    {
        [Constructable]
        public StandardMageryStudyBook()
            : base(SkillName.Magery, 700) { }

        public StandardMageryStudyBook(Serial serial)
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

    public class StandardMagicResistStudyBook : StudyBook
    {
        [Constructable]
        public StandardMagicResistStudyBook()
            : base(SkillName.MagicResist, 700) { }

        public StandardMagicResistStudyBook(Serial serial)
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

    public class StandardTacticsStudyBook : StudyBook
    {
        [Constructable]
        public StandardTacticsStudyBook()
            : base(SkillName.Tactics, 700) { }

        public StandardTacticsStudyBook(Serial serial)
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

    public class StandardSnoopingStudyBook : StudyBook
    {
        [Constructable]
        public StandardSnoopingStudyBook()
            : base(SkillName.Snooping, 700) { }

        public StandardSnoopingStudyBook(Serial serial)
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

    public class StandardMusicianshipStudyBook : StudyBook
    {
        [Constructable]
        public StandardMusicianshipStudyBook()
            : base(SkillName.Musicianship, 700) { }

        public StandardMusicianshipStudyBook(Serial serial)
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

    public class StandardPoisoningStudyBook : StudyBook
    {
        [Constructable]
        public StandardPoisoningStudyBook()
            : base(SkillName.Poisoning, 700) { }

        public StandardPoisoningStudyBook(Serial serial)
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

    public class StandardMarksmanshipStudyBook : StudyBook
    {
        [Constructable]
        public StandardMarksmanshipStudyBook()
            : base(SkillName.Marksmanship, 700) { }

        public StandardMarksmanshipStudyBook(Serial serial)
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

    public class StandardSpiritualismStudyBook : StudyBook
    {
        [Constructable]
        public StandardSpiritualismStudyBook()
            : base(SkillName.Spiritualism, 700) { }

        public StandardSpiritualismStudyBook(Serial serial)
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

    public class StandardStealingStudyBook : StudyBook
    {
        [Constructable]
        public StandardStealingStudyBook()
            : base(SkillName.Stealing, 700) { }

        public StandardStealingStudyBook(Serial serial)
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

    public class StandardTailoringStudyBook : StudyBook
    {
        [Constructable]
        public StandardTailoringStudyBook()
            : base(SkillName.Tailoring, 700) { }

        public StandardTailoringStudyBook(Serial serial)
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

    public class StandardTamingStudyBook : StudyBook
    {
        [Constructable]
        public StandardTamingStudyBook()
            : base(SkillName.Taming, 700) { }

        public StandardTamingStudyBook(Serial serial)
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

    public class StandardTastingStudyBook : StudyBook
    {
        [Constructable]
        public StandardTastingStudyBook()
            : base(SkillName.Tasting, 700) { }

        public StandardTastingStudyBook(Serial serial)
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

    public class StandardTinkeringStudyBook : StudyBook
    {
        [Constructable]
        public StandardTinkeringStudyBook()
            : base(SkillName.Tinkering, 700) { }

        public StandardTinkeringStudyBook(Serial serial)
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

    public class StandardTrackingStudyBook : StudyBook
    {
        [Constructable]
        public StandardTrackingStudyBook()
            : base(SkillName.Tracking, 700) { }

        public StandardTrackingStudyBook(Serial serial)
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

    public class StandardVeterinaryStudyBook : StudyBook
    {
        [Constructable]
        public StandardVeterinaryStudyBook()
            : base(SkillName.Veterinary, 700) { }

        public StandardVeterinaryStudyBook(Serial serial)
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

    public class StandardSwordsStudyBook : StudyBook
    {
        [Constructable]
        public StandardSwordsStudyBook()
            : base(SkillName.Swords, 700) { }

        public StandardSwordsStudyBook(Serial serial)
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

    public class StandardBludgeoningStudyBook : StudyBook
    {
        [Constructable]
        public StandardBludgeoningStudyBook()
            : base(SkillName.Bludgeoning, 700) { }

        public StandardBludgeoningStudyBook(Serial serial)
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

    public class StandardFencingStudyBook : StudyBook
    {
        [Constructable]
        public StandardFencingStudyBook()
            : base(SkillName.Fencing, 700) { }

        public StandardFencingStudyBook(Serial serial)
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

    public class StandardFistFightingStudyBook : StudyBook
    {
        [Constructable]
        public StandardFistFightingStudyBook()
            : base(SkillName.FistFighting, 700) { }

        public StandardFistFightingStudyBook(Serial serial)
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

    public class StandardLumberjackingStudyBook : StudyBook
    {
        [Constructable]
        public StandardLumberjackingStudyBook()
            : base(SkillName.Lumberjacking, 700) { }

        public StandardLumberjackingStudyBook(Serial serial)
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

    public class StandardMiningStudyBook : StudyBook
    {
        [Constructable]
        public StandardMiningStudyBook()
            : base(SkillName.Mining, 700) { }

        public StandardMiningStudyBook(Serial serial)
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

    public class StandardMeditationStudyBook : StudyBook
    {
        [Constructable]
        public StandardMeditationStudyBook()
            : base(SkillName.Meditation, 700) { }

        public StandardMeditationStudyBook(Serial serial)
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

    public class StandardStealthStudyBook : StudyBook
    {
        [Constructable]
        public StandardStealthStudyBook()
            : base(SkillName.Stealth, 700) { }

        public StandardStealthStudyBook(Serial serial)
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

    public class StandardRemoveTrapStudyBook : StudyBook
    {
        [Constructable]
        public StandardRemoveTrapStudyBook()
            : base(SkillName.RemoveTrap, 700) { }

        public StandardRemoveTrapStudyBook(Serial serial)
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

    public class StandardNecromancyStudyBook : StudyBook
    {
        [Constructable]
        public StandardNecromancyStudyBook()
            : base(SkillName.Necromancy, 700) { }

        public StandardNecromancyStudyBook(Serial serial)
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

    public class StandardFocusStudyBook : StudyBook
    {
        [Constructable]
        public StandardFocusStudyBook()
            : base(SkillName.Focus, 700) { }

        public StandardFocusStudyBook(Serial serial)
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

    public class StandardKnightshipStudyBook : StudyBook
    {
        [Constructable]
        public StandardKnightshipStudyBook()
            : base(SkillName.Knightship, 700) { }

        public StandardKnightshipStudyBook(Serial serial)
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

    public class StandardBushidoStudyBook : StudyBook
    {
        [Constructable]
        public StandardBushidoStudyBook()
            : base(SkillName.Bushido, 700) { }

        public StandardBushidoStudyBook(Serial serial)
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

    public class StandardNinjitsuStudyBook : StudyBook
    {
        [Constructable]
        public StandardNinjitsuStudyBook()
            : base(SkillName.Ninjitsu, 700) { }

        public StandardNinjitsuStudyBook(Serial serial)
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

    public class StandardElementalismStudyBook : StudyBook
    {
        [Constructable]
        public StandardElementalismStudyBook()
            : base(SkillName.Elementalism, 700) { }

        public StandardElementalismStudyBook(Serial serial)
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

    //public class StandardMysticismStudyBook : StudyBook
    //   {
    //       [Constructable]
    //       public StandardMysticismStudyBook()
    //           : base(SkillName.Mysticism, 700)
    //       {
    //       }

    //       public StandardMysticismStudyBook(Serial serial)
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

    //public class StandardImbuingStudyBook : StudyBook
    //   {
    //       [Constructable]
    //       public StandardImbuingStudyBook()
    //           : base(SkillName.Imbuing, 700)
    //       {
    //       }

    //       public StandardImbuingStudyBook(Serial serial)
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

    //public class StandardThrowingStudyBook : StudyBook
    //   {
    //       [Constructable]
    //       public StandardThrowingStudyBook()
    //           : base(SkillName.Throwing, 700)
    //       {
    //       }

    //       public StandardThrowingStudyBook(Serial serial)
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
