/*************************************************************
 * File: MountClone.cs
 * Description: Runtime clone of a mount ridden by an offline player.
 *************************************************************/
using System.Reflection;
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
            PropertyInfo[] properties = typeof(BaseMount).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite && property.Name != "Rider")
                {
                    property.SetValue(this, property.GetValue(original, null), null);
                }
            }
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