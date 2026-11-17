using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class StaffEventQuestNpc : XmlQuestNPC
    {
        public enum Profile
        {
            MadameEva,
            Katara,
            Silas
        }

        private const string MadameEvaSettings =
            "XmlQuestNPC/Name/Madame Eva/TalkText/Free Reading/Female/true/ATTACH/XmlDialog,EvaDialog";

        private const string KataraSettings =
            "XmlQuestNPC/Name/Katara/Female/true/Fame/10000/Karma/10000/ATTACH/XmlDialog,KataraDialog/" +
            "TalkText/hail/ColdResistSeed/60/EnergyResistSeed/60/FireResistSeed/60/PhysicalResistanceSeed/" +
            "60/HitsMaxSeed/325/Hits/325/Hue/1050/RawDex/13/RawInt/149/RawStr/88/SpeechHue/52/" +
            "UNEQUIP,OneHanded,delete/UNEQUIP,TwoHanded,delete/UNEQUIP,Shoes,delete/UNEQUIP,Pants,delete/" +
            "UNEQUIP,Shirt,delete/UNEQUIP,Helm,delete/UNEQUIP,Gloves,delete/UNEQUIP,Ring,delete/" +
            "UNEQUIP,Neck,delete/UNEQUIP,Waist,delete/UNEQUIP,InnerTorso,delete/UNEQUIP,Bracelet,delete/" +
            "UNEQUIP,FacialHair,delete/UNEQUIP,MiddleTorso,delete/UNEQUIP,Earrings,delete/" +
            "UNEQUIP,Arms,delete/UNEQUIP,Cloak,delete/UNEQUIP,Backpack,delete/UNEQUIP,OuterTorso,delete/" +
            "UNEQUIP,OuterLegs,delete/UNEQUIP,InnerLegs,delete/UNEQUIP,Hair,delete/EQUIP/<FancyRobe/Hue/" +
            "2498/ItemID/11114>/EQUIP/<Artifact_BootsofHermes/Hue/2907/ItemID/1030>/EQUIP/<WizardStaff/Hue/" +
            "1167/GraphicID/3721>/EQUIP/<Artifact_HelmOfInsight/Hue/2803/ItemID/11119>/HairItemID/8253/" +
            "HairHue/1319/BodyValue/401";

        private const string SilasSettings =
            "xmlquestnpc,0/name/Gutter-Tooth Silas/title/the slumlord/EQUIP/shirt/EQUIP/shortpants/EQUIP/" +
            "boots/ATTACH/XmlDialog,Silas";

        private Profile m_Profile;
        private bool m_ProfileApplied;

        [Constructable]
        public StaffEventQuestNpc(Profile profile)
            : base(profile == Profile.Silas ? 0 : -1)
        {
            m_Profile = profile;
        }

        public StaffEventQuestNpc(Serial serial)
            : base(serial)
        {
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            if (m_ProfileApplied)
                return;

            string settings;
            switch (m_Profile)
            {
                case Profile.MadameEva:
                    settings = MadameEvaSettings;
                    break;
                case Profile.Katara:
                    settings = KataraSettings;
                    break;
                case Profile.Silas:
                    settings = SilasSettings;
                    break;
                default:
                    return;
            }

            // Apply the fixed outfit and dialogue settings after regional spawn initialization.
            string status;
            BaseXmlSpawner.ApplyObjectStringProperties(null, settings, this, null, this, out status);
            m_ProfileApplied = true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
            writer.Write((int)m_Profile);
            writer.Write(m_ProfileApplied);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Profile = (Profile)reader.ReadInt();
            m_ProfileApplied = reader.ReadBool();
        }
    }

    public class StaffUndeadInformant : Lich
    {
        [Constructable]
        public StaffUndeadInformant()
        {
        }

        public StaffUndeadInformant(Serial serial)
            : base(serial)
        {
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            Name = "Undead Informant";
            Frozen = true;
            BodyValue = 168;
            Title = null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            // Mobile.Frozen is transient in the base save format.
            Frozen = true;
        }
    }
}
