using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class WolfGuise : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 18; } }
        public override int BaseFireResistance { get { return 13; } }
        public override int BaseColdResistance { get { return 23; } }
        public override int BasePoisonResistance { get { return 13; } }
        public override int BaseEnergyResistance { get { return 13; } }

        public override int InitMinHits { get { return 1000; } }
        public override int InitMaxHits { get { return 1000; } }

        public override int AosStrReq { get { return 0; } }
        public override int OldStrReq { get { return 0; } }

        public override int ArmorBase { get { return 13; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public WolfGuise()
            : base(0x2683)
        {
            Weight = 0;
            Name = "Wolf Guise";
            Layer = Layer.OuterTorso;
            Movable = false;
            LootType = LootType.Blessed;

            ArmorAttributes.SelfRepair = 100;
        }

        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(World_Login);
        }

        private static void World_Login(LoginEventArgs args)
        {
            if (args.Mobile != null)
            {
                Item item = args.Mobile.FindItemOnLayer(Layer.OuterTorso);
                if (item != null)
                {
                    if (item is WolfGuise)
                        args.Mobile.BodyMod = 25;
                }
            }
        }

        public override bool OnEquip(Mobile from)
        {
            from.BodyMod = 25;
            return base.OnEquip(from);
        }

        public WolfGuise(Serial serial)
            : base(serial)
        {
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
        }

    }
}
