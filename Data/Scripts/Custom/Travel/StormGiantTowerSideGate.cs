using Server;
using Server.Items;

namespace Server.Custom.Confictura
{
    // Decoration links adjacent doors of the same type. The tower's side gates open independently.
    public class StormGiantTowerSideGate : IronGate
    {
        [Constructable]
        public StormGiantTowerSideGate(DoorFacing facing)
            : base(facing)
        {
        }

        public StormGiantTowerSideGate(Serial serial)
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
