using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;

namespace Server.Custom.Confictura.Items
{
    public class RuneSword : RoyalSword
    {
        [Constructable]
        public RuneSword()
        {
            MobileBalanceItemSupport.Initialize(this, "Rune Sword", 0x26CE, 0, Layer.OneHanded, "Artefact");

            MinDamage = 45;
            MaxDamage = 70;
            WeaponAttributes.SelfRepair = 10;
            WeaponAttributes.HitLightning = 10;
        }

        public RuneSword(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class TridentofAtlantis : Spear
    {
        [Constructable]
        public TridentofAtlantis()
        {
            MobileBalanceItemSupport.Initialize(this, "Trident of Atlantis", 0xE87, 2843, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 75;
            MaxDamage = 90;
            WeaponAttributes.HitLeechMana = 100;
            WeaponAttributes.SelfRepair = 10;
            Attributes.SpellChanneling = 1;
        }

        public TridentofAtlantis(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class SwordofDarkness : RoyalSword
    {
        [Constructable]
        public SwordofDarkness()
        {
            MobileBalanceItemSupport.Initialize(this, "Sword of Darkness", 0x26CE, 1175, Layer.OneHanded, "Artefact");

            MinDamage = 35;
            MaxDamage = 50;
            WeaponAttributes.HitLeechStam = 100;
            WeaponAttributes.HitLeechHits = 25;
            Attributes.SpellChanneling = 1;
        }

        public SwordofDarkness(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class SwordofLight : RoyalSword
    {
        [Constructable]
        public SwordofLight()
        {
            MobileBalanceItemSupport.Initialize(this, "Sword of Light", 0x26CE, 1152, Layer.OneHanded, "Legendary Artefact");

            MinDamage = 35;
            MaxDamage = 50;
            WeaponAttributes.HitLightning = 50;
            WeaponAttributes.SelfRepair = 10;
            WeaponAttributes.HitLeechMana = 50;
            Attributes.SpellChanneling = 1;
            Attributes.AttackChance = 50;
        }

        public SwordofLight(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class WolfgangSword : NoDachi
    {
        [Constructable]
        public WolfgangSword()
        {
            MobileBalanceItemSupport.Initialize(this, "Sword of Kings", 0x27A2, 2843, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 55;
            MaxDamage = 80;
            WeaponAttributes.HitLeechStam = 100;
            Attributes.SpellChanneling = 1;
            Attributes.RegenHits = 10;
            SkillBonuses.SetValues(0, SkillName.Knightship, 50.0);
            SkillBonuses.SetValues(1, SkillName.Bushido, 50.0);
        }

        public WolfgangSword(Serial serial)
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

    public class Weightoftheworld : Tetsubo
    {
        [Constructable]
        public Weightoftheworld()
        {
            MobileBalanceItemSupport.Initialize(this, "Weight of the World", 0x27A6, 0, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 85;
            MaxDamage = 100;
            Weight = 500.0;
            SkillBonuses.SetValues(0, SkillName.Anatomy, 10.0);
        }

        public Weightoftheworld(Serial serial)
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

    public class DeathPenalty : Crossbow
    {
        [Constructable]
        public DeathPenalty()
        {
            MobileBalanceItemSupport.Initialize(this, "Death Penalty", 0xF50, 1175, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 35;
            MaxDamage = 60;
            Attributes.SpellChanneling = 1;
        }

        public DeathPenalty(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class Disintegrator : KilrathiHeavyGun
    {
        [Constructable]
        public Disintegrator()
        {
            MobileBalanceItemSupport.Initialize(this, "Disintegrator", 0x3F65, 0, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 75;
            MaxDamage = 90;
        }

        public Disintegrator(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class DreadMace : Mace
    {
        [Constructable]
        public DreadMace()
        {
            MobileBalanceItemSupport.Initialize(this, "The Dread Mace", 0x2681, 1175, Layer.OneHanded, "Legendary Artefact");

            MinDamage = 35;
            MaxDamage = 60;
            WeaponAttributes.HitPoisonArea = 100;
            Attributes.LowerRegCost = -100;
            Attributes.SpellChanneling = 1;
        }

        public DreadMace(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class ExodusMaul : Tetsubo
    {
        [Constructable]
        public ExodusMaul()
        {
            MobileBalanceItemSupport.Initialize(this, "Exodus Spine", 0x27A6, 1175, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 55;
            MaxDamage = 80;
            Attributes.SpellChanneling = 1;
        }

        public ExodusMaul(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }

    public class DoomBlade : NoDachi
    {
        [Constructable]
        public DoomBlade()
        {
            MobileBalanceItemSupport.Initialize(this, "Doom Blade", 0x27A2, 1175, Layer.TwoHanded, "Legendary Artefact");

            MinDamage = 55;
            MaxDamage = 80;
            Attributes.SpellDamage = -200;
            Attributes.SpellChanneling = 1;
            Weight = 100.0;
            WeaponAttributes.HitLeechMana = 100;
        }

        public DoomBlade(Serial serial)
            : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            SignedEquipSkillModController.Apply(this, parent);
        }

        public override void OnRemoved(object parent)
        {
            SignedEquipSkillModController.Remove(this);
            base.OnRemoved(parent);
        }

        public override void OnAfterDelete()
        {
            SignedEquipSkillModController.Remove(this);
            base.OnAfterDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            SignedEquipSkillModController.AddProperties(this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SignedEquipSkillModController.Rehydrate(this);
        }
    }
}
