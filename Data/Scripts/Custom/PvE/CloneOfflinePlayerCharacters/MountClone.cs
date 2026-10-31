/*************************************************************
 * File: MountClone.cs
 * Description: Runtime clone of a mount ridden by an offline player.
 *************************************************************/
using Server.Mobiles;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Copy of a <see cref="BaseMount"/> that keeps the clone's equipment
    /// layout consistent with the original player.
    /// </summary>
    public class MountClone : BaseMount
    {
        public MountClone(BaseMount original)
            : base(
                original.Name,
                original.BodyValue,
                original.ItemID,
                original.AI,
                original.FightMode,
                original.RangePerception,
                original.RangeFight,
                original.ActiveSpeed,
                original.PassiveSpeed
            )
        {
            // Do not copy Spawner, control owners, summon owners, stabling or rider links.
            CloneThings.CloneMobileProperties(original,this);
            SetHits(original.HitsMax-GetStatOffset(StatType.Str));
            SetStam(original.StamMax-GetStatOffset(StatType.Dex));
            SetMana(original.ManaMax-GetStatOffset(StatType.Int));
            Hits=original.Hits; Stam=original.Stam; Mana=original.Mana;
            SetDamage(original.DamageMin,original.DamageMax);
            VirtualArmor=original.VirtualArmor; Tamable=false;
        }

        public MountClone(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
