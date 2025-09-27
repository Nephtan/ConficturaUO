using System;
using Server.Custom.Confictura.Mobiles;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Items
{
    /// <summary>
    ///     Alchemical core used to assemble a controllable Garden Golem. Mirrors the feel of Clockwork Assembly
    ///     while leaning on gardening reagents for its crafting cost.
    /// </summary>
    public class GardenGolemCore : Item
    {
        public override string DefaultName
        {
            get { return "garden golem core"; }
        }

        [Constructable]
        public GardenGolemCore()
            : base(0x1EA8)
        {
            Weight = 5.0;
            Hue = 0x48E;
        }

        public GardenGolemCore(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null)
                return;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (from.Skills[SkillName.Alchemy].Value < 90.0)
            {
                from.SendMessage("You must have at least 90.0 skill in Alchemy to animate a garden golem.");
                return;
            }

            if ((from.Followers + 3) > from.FollowersMax)
            {
                from.SendLocalizedMessage(1049607); // You have too many followers to control that creature.
                return;
            }

            Container pack = from.Backpack;
            if (pack == null)
                return;

            int res = pack.ConsumeTotal(
                new Type[] { typeof(FreshGardenSoil), typeof(FertileDirt), typeof(MandrakeRoot) },
                new int[] { 3, 20, 25 }
            );

            switch (res)
            {
                case 0:
                    from.SendMessage("You require fresh garden soil to stabilize the construct.");
                    return;
                case 1:
                    from.SendMessage("You require 20 fertile dirt to line the golem's chassis.");
                    return;
                case 2:
                    from.SendMessage("You require 25 mandrake roots as the animating reagent.");
                    return;
            }

            GardenGolem golem = new GardenGolem(true);

            if (golem.SetControlMaster(from))
            {
                Delete();
                golem.MoveToWorld(from.Location, from.Map);
                from.PlaySound(0x241);
                from.SendMessage("The garden golem awakens, awaiting your tending.");
            }
            else
            {
                golem.Delete();
                from.SendMessage("The garden golem fails to attune itself and crumbles.");
            }
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