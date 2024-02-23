using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SawMillSouthAddon : BaseAddon
    {
        public override string AddonName
        {
            get { return "saw mill"; }
        }

        public override BaseAddonDeed Deed
        {
            get { return new SawMillSouthAddonDeed(); }
        }

        [Constructable]
        public SawMillSouthAddon()
        {
            AddNamedComponent(new AddonComponent(1928), 0, 0, 0);
            AddNamedComponent(new AddonComponent(1928), 1, 0, 0);
            AddNamedComponent(new AddonComponent(4525), 1, 0, 5);
            AddNamedComponent(new AddonComponent(7130), 0, 0, 5);
        }

        private void AddNamedComponent(AddonComponent component, int x, int y, int z)
        {
            component.Name = "saw mill";
            AddComponent(component, x, y, z);
        }

        public SawMillSouthAddon(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SawMillSouthAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get { return new SawMillSouthAddon(); }
        }

        [Constructable]
        public SawMillSouthAddonDeed()
        {
            Name = "saw mill deed (south)";
        }

        public SawMillSouthAddonDeed(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
