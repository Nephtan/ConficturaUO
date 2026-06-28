/*************************************************************
 * File: EtherealMountClone.cs
 * Description: Copy of an ethereal mount ridden by an offline player.
 *************************************************************/
using Server.Items;
using Server.Mobiles;
using System.Reflection;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Duplicates an <see cref="EtherealMount"/> so the clone inherits the
    /// owner's mount visuals without granting permanent access to players.
    /// </summary>
    public class EtherealMountClone : EtherealMount
    {
        public EtherealMountClone(EtherealMount original)
            : base(original.RegularID, original.MountedID)
        {
            PropertyInfo[] properties = typeof(EtherealMount).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite && property.Name != "Rider")
                {
                    property.SetValue(this, property.GetValue(original, null), null);
                }
            }
        }

        public EtherealMountClone(Serial serial)
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