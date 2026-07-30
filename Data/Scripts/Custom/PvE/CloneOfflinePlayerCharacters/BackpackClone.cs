/*************************************************************
 * File: BackpackClone.cs
 * Description: Restricted backpack used by offline player clones.
 *************************************************************/
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Backpack that mirrors a player's pack but blocks interaction from
    /// other players to prevent item duplication exploits.
    /// </summary>
    public class BackpackClone : Backpack
    {
        public BackpackClone()
        {
        }

        public BackpackClone(Serial serial)
            : base(serial)
        {
        }

        public override bool IsAccessibleTo(Mobile mobile)
        {
            if (mobile is PlayerMobile && mobile.AccessLevel < AccessLevel.Owner)
            {
                mobile.SendMessage("You cannot access this clone's backpack.");
                return false;
            }

            return base.IsAccessibleTo(mobile);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.AccessLevel < AccessLevel.Owner)
            {
                from.SendMessage("You cannot access this clone's backpack.");
            }

            base.OnDoubleClick(from);
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