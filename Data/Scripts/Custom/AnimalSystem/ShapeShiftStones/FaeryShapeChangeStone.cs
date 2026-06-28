using System; 
using System.Collections; 
using Server.Items; 
using Server.Misc; 
using Server.Network; 
using Server.Multis; 

namespace Server.Items 
{ 
   public class FaerieShapeShiftStone : Item 
   { 
      [Constructable] 
      public FaerieShapeShiftStone() : base( 0x1870 ) 
      { 
         Name = "a Faerie ShapeShift Stone"; 
         Movable = false;
 LootType=LootType.Blessed;  
		 Hue = 1952;
      } 
		
	public FaerieShapeShiftStone( Serial serial ) : base( serial ) 
    { 
    } 

	public int m_Transformed;
	public int m_SkinHue;
	public int m_BodyValue;

	[CommandProperty( AccessLevel.GameMaster )] 
	 public int Transformed 
	 { 
		get { return m_Transformed; } 
	 } 

	[CommandProperty( AccessLevel.GameMaster )] 
	 public int SkinHue 
	 { 
		get { return m_SkinHue; } 
	 } 

	[CommandProperty( AccessLevel.GameMaster )] 
	 public int BodyValue
	 { 
		get { return m_BodyValue; } 
	 } 

	public override void OnDoubleClick( Mobile from ) 
    { 
		if(m_Transformed==1)
		{
			UnTransform(from);
		}
		else
		{
			Transform(from);
		}
	} 

	public override bool HandlesOnSpeech{ get{ return true; } } 

	public override void OnSpeech( SpeechEventArgs e ) 
	{ 
		if ( !e.Handled && this.IsChildOf( e.Mobile.Backpack )) 
        { 
			string keyword = e.Speech; 
			switch ( keyword ) 
			{ 
                 case "transform":
				{ 
					 if (m_Transformed==0)
					 {
						Transform(e.Mobile);
						e.Handled = true;
					 }
					break;
				} 
				case "untransform":
				{
					 if (m_Transformed==1)
					 {
						UnTransform(e.Mobile);
						e.Handled = true;
					 }
					break;
				}
			} 
         } 

         base.OnSpeech( e ); 
      } 

		public void Transform(Mobile from)
		{
			m_Transformed = 1;
			if(from.Female)
			{
				m_BodyValue = 401;
				from.BodyValue = 176;
			}
			else
			{
				m_BodyValue = 400;
				from.BodyValue = 58;
			}
			m_SkinHue = from.Hue;
			from.Hue = 0;
		}

		public void UnTransform(Mobile from)
		{
			m_Transformed = 0;
			from.BodyValue=m_BodyValue ;
			from.Hue=m_SkinHue;
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version 
			writer.Write( (int) m_Transformed );
			writer.Write( (int) m_SkinHue );
			writer.Write( (int) m_BodyValue );

      } 

      public override void Deserialize( GenericReader reader ) 
      { 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			m_Transformed = reader.ReadInt(); 
			m_SkinHue = reader.ReadInt(); 
			m_BodyValue = reader.ReadInt(); 
      } 
   } 
} 
