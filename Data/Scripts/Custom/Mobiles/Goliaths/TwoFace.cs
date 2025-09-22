using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the AncientEttin configured as "Two Face" in the Dio XML spawn set.
    ///     Applies the XML-defined statistics and attachments so the spawn behaves as intended without XMLSpawner data.
    /// </summary>
    public class TwoFace : AncientEttin
    {
        [Constructable]
        public TwoFace()
            : base()
        {
            // Identity derived from the XML entry
            Name = "Two Face";
            Title = null; // No explicit title provided in the XML definition

            // Health settings supplied via hitsmaxseed/hits
            HitsMaxSeed = 3000;
            SetHits(3000, 3000);

            // Damage range from damagemin/damagemax
            SetDamage(100, 170);

            // Currency payload indicated by cointype/coins
            CoinType = "jewels";
            Coins = 9000;
            PackItem(new DDJewels(9000));

            // Attach stamina drain effect from ATTACH/xmlstamdrain,900
            XmlAttach.AttachTo(this, new XmlStamDrain(900));
        }

        public TwoFace(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }
}