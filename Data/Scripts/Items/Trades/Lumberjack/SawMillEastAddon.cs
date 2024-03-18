using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SawMillEastAddon : BaseAddon
    {
        public override string AddonName
        {
            get { return "saw mill"; }
        }

        public override BaseAddonDeed Deed
        {
            get { return new SawMillEastAddonDeed(); }
        }

        [Constructable]
        public SawMillEastAddon()
        {
            AddNamedComponent(new AddonComponent(1928), 0, 0, 0);
            AddNamedComponent(new AddonComponent(1928), 0, 1, 0);
            AddNamedComponent(new AddonComponent(4530), 0, 1, 5);
            AddNamedComponent(new AddonComponent(7127), 0, 0, 5);
        }

        private void AddNamedComponent(AddonComponent component, int x, int y, int z)
        {
            component.Name = "saw mill";
            AddComponent(component, x, y, z);
        }

        private static void AddComplexComponent(
            BaseAddon addon,
            int item,
            int xoffset,
            int yoffset,
            int zoffset,
            int hue,
            int lightsource,
            string name,
            int amount
        )
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType)lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
            if (name != null && name.Length > 0)
                ac.Name = name;
        }

        public SawMillEastAddon(Serial serial)
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

    public class SawMillEastAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get { return new SawMillEastAddon(); }
        }

        [Constructable]
        public SawMillEastAddonDeed()
        {
            Name = "saw mill deed (east)";
        }

        public SawMillEastAddonDeed(Serial serial)
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
