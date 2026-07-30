using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;

namespace Server.Custom.Confictura.Items
{
    public class MindOverMatterTalisman : MagicTalisman
    {
        [Constructable]
        public MindOverMatterTalisman()
        {
            MobileBalanceItemSupport.ClearRandomTalismanProperties(this);
            MobileBalanceItemSupport.ApplyMetadata(this, "Mindful Talisman", 0x2F5B, 0, Layer.Talisman, "Artefact");

            Attributes.BonusMana = 200;
            Attributes.LowerRegCost = 100;
            Attributes.CastRecovery = 4;
            Attributes.CastSpeed = 4;
            Attributes.SpellDamage = 200;
            Attributes.AttackChance = -100;
            Attributes.DefendChance = -100;
        }

        public MindOverMatterTalisman(Serial serial)
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

    public class GoldenDragonStatue : StatueAdventurer
    {
        [Constructable]
        public GoldenDragonStatue()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Golden Dragon Statue", 0x4C2D, 2843, Layer.Invalid, "Artefact");
        }

        public GoldenDragonStatue(Serial serial)
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

    public class MemorialofVirtue : StatueAdventurer
    {
        [Constructable]
        public MemorialofVirtue()
        {
            MobileBalanceItemSupport.ApplyMetadata(this, "Memorial of Virtue", 0x4FC7, 0, Layer.Invalid, "Artefact");
        }

        public MemorialofVirtue(Serial serial)
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

    public class HeartofFire : MagicTalisman
    {
        [Constructable]
        public HeartofFire()
        {
            MobileBalanceItemSupport.ClearRandomTalismanProperties(this);
            MobileBalanceItemSupport.ApplyMetadata(this, "Heart of Fire", 0x4D16, 2227, Layer.Talisman, "Legendary Artefact");

            Attributes.SpellDamage = 200;
            Attributes.LowerRegCost = 50;
            Attributes.RegenMana = 10;
            Attributes.RegenStam = 10;
            Attributes.WeaponDamage = 100;
        }

        public HeartofFire(Serial serial)
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

    public class LampofBadWishes : MagicTalisman
    {
        [Constructable]
        public LampofBadWishes()
        {
            MobileBalanceItemSupport.ClearRandomTalismanProperties(this);
            MobileBalanceItemSupport.ApplyMetadata(this, "Lamp of Bad Wishes", 0x2C82, 2843, Layer.Talisman, "Legendary Artefact");

            Attributes.BonusMana = 200;
            Attributes.LowerRegCost = 100;
            Attributes.DefendChance = -100;
            Attributes.Luck = 200;
            Attributes.RegenMana = 10;
            Attributes.WeaponDamage = 100;
        }

        public LampofBadWishes(Serial serial)
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
