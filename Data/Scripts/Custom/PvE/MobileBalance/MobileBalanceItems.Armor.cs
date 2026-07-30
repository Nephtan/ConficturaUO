using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;

namespace Server.Custom.Confictura.Items
{
    public class GuardianShield : MetalKiteShield
    {
        [Constructable]
        public GuardianShield()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Shield of the Guardian", 0x1B74, 1152, Layer.TwoHanded, "Artefact");

            EnergyBonus = 75;
            Attributes.LowerRegCost = 50;
            Attributes.DefendChance = 30;
            Attributes.BonusHits = 100;
            Attributes.BonusMana = 100;
            Attributes.BonusStam = 100;
            ArmorAttributes.MageArmor = 1;
            ArmorAttributes.SelfRepair = 10;
        }

        public GuardianShield(Serial serial)
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

    public class RobeoftheVoid : Robe
    {
        [Constructable]
        public RobeoftheVoid()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Robe of the Void", 0x1F03, 1175, Layer.OuterTorso, "Legendary Artefact");

            Attributes.BonusMana = 100;
            Attributes.LowerRegCost = 100;
            Attributes.DefendChance = -10;
        }

        public RobeoftheVoid(Serial serial)
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

    public class ArmoroftheTitans : RoyalChest
    {
        [Constructable]
        public ArmoroftheTitans()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Titan Slayer Armor", 0x2B08, 2843, Layer.InnerTorso, "Artefact");

            PhysicalBonus = 50;
            ArmorAttributes.MageArmor = 1;
            Attributes.DefendChance = 25;
        }

        public ArmoroftheTitans(Serial serial)
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

    public class ArmorofthefalseGods : RoyalChest
    {
        [Constructable]
        public ArmorofthefalseGods()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "God Slayer Armor", 0x2B08, 1152, Layer.InnerTorso, "Legendary Artefact");

            PhysicalBonus = 50;
            EnergyBonus = 50;
            FireBonus = 50;
            ColdBonus = 50;
            ArmorAttributes.MageArmor = 1;
            Attributes.DefendChance = 45;
            Attributes.WeaponDamage = 50;
            Attributes.RegenHits = 10;
        }

        public ArmorofthefalseGods(Serial serial)
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

    public class ExodusArmor : BoneChest
    {
        [Constructable]
        public ExodusArmor()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Exodus Ribcage", 0x144F, 1175, Layer.InnerTorso, "Legendary Artefact");

            PhysicalBonus = 75;
            FireBonus = 75;
            ArmorAttributes.MageArmor = 1;
            Attributes.AttackChance = 20;
            Attributes.BonusHits = 100;
            Attributes.ReflectPhysical = 50;
            Attributes.WeaponDamage = 100;
            Attributes.SpellDamage = 100;
        }

        public ExodusArmor(Serial serial)
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

    public class ExodusHead : BoneHelm
    {
        [Constructable]
        public ExodusHead()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Exodus Skull", 0x1451, 1175, Layer.Helm, "Legendary Artefact");

            EnergyBonus = 75;
            ColdBonus = 75;
            ArmorAttributes.MageArmor = 1;
            Attributes.ReflectPhysical = 50;
            Attributes.BonusMana = 200;
            Attributes.WeaponDamage = 50;
            Attributes.SpellDamage = 100;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 5;
        }

        public ExodusHead(Serial serial)
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

    public class ExodusHands : DragonGloves
    {
        [Constructable]
        public ExodusHands()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Exodus Claws", 0x2643, 1175, Layer.Gloves, "Legendary Artefact");

            FireBonus = 75;
            ArmorAttributes.MageArmor = 1;
            Attributes.AttackChance = 50;
            Attributes.ReflectPhysical = 20;
            Attributes.WeaponDamage = 100;
        }

        public ExodusHands(Serial serial)
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
