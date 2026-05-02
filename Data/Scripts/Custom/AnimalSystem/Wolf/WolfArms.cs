using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13cd, 0x13c5 )]
	public class WolfArms : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 1000; } }
		public override int InitMaxHits{ get{ return 1000; } }

		public override int AosStrReq{ get{ return 0; } }
		public override int OldStrReq{ get{ return 0; } }

		public override int ArmorBase{ get{ return 13; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		[Constructable]
		public WolfArms() : base( 0x13CD )
		{
            Name = "Wolf Arms";
			Weight = 0;
            Movable = false;
            LootType = LootType.Blessed;

            ArmorAttributes.SelfRepair = 100;
		}

		public WolfArms( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 1.0 )
				Weight = 2.0;
		}
	}
}