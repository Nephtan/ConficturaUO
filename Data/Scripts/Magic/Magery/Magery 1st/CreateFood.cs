using System;
using Server.Items;

namespace Server.Spells.First
{
    public class CreateFoodSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Create Food",
            "In Mani Ylem",
            224,
            9011,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public CreateFoodSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        private static FoodInfo[] m_Food = new FoodInfo[]
        {
            new FoodInfo(typeof(Grapes), "a grape bunch"),
            new FoodInfo(typeof(Ham), "a ham"),
            new FoodInfo(typeof(CheeseWedge), "a wedge of cheese"),
            new FoodInfo(typeof(Muffins), "muffins"),
            new FoodInfo(typeof(FishSteak), "a fish steak"),
            new FoodInfo(typeof(Ribs), "cut of ribs"),
            new FoodInfo(typeof(CookedBird), "a cooked bird"),
            new FoodInfo(typeof(Sausage), "sausage"),
            new FoodInfo(typeof(Apple), "an apple"),
            new FoodInfo(typeof(Peach), "a peach")
        };

        private bool PlaceConjuredItem(Item item)
        {
            if (item == null)
                return false;

            bool delivered = false;

            try
            {
                delivered = Caster.PlaceInBackpack(item);
                return delivered;
            }
            finally
            {
                // Rejected conjurations must not become ground items or orphaned world objects.
                if (!delivered && !item.Deleted)
                    item.Delete();
            }
        }

        public override void OnCast()
        {
            try
            {
                if (!CheckSequence())
                    return;

                bool delivered;

                if (Server.Items.BaseRace.BloodDrinker(Caster.RaceID))
                {
                    delivered = PlaceConjuredItem(new BloodyDrink());
                    Caster.SendMessage(delivered
                        ? "Some fresh blood magically appears in your backpack."
                        : "Your backpack cannot hold the conjured blood.");
                }
                else if (Server.Items.BaseRace.BrainEater(Caster.RaceID))
                {
                    delivered = PlaceConjuredItem(new FreshBrain());
                    Caster.SendMessage(delivered
                        ? "Some fresh brains magically appears in your backpack."
                        : "Your backpack cannot hold the conjured brains.");
                }
                else
                {
                    FoodInfo foodInfo = m_Food[Utility.Random(m_Food.Length)];
                    bool foodDelivered = PlaceConjuredItem(foodInfo.Create());
                    bool waterDelivered = PlaceConjuredItem(new WaterFlask());
                    delivered = foodDelivered || waterDelivered;

                    if (foodDelivered && waterDelivered)
                        Caster.SendMessage("Some food and drink magically appear in your backpack.");
                    else if (foodDelivered)
                        Caster.SendMessage("Some food magically appears in your backpack, but there is no room for the drink.");
                    else if (waterDelivered)
                        Caster.SendMessage("Some drink magically appears in your backpack, but the food could not be delivered.");
                    else
                        Caster.SendMessage("Your backpack cannot hold the conjured food or drink.");
                }

                if (delivered)
                {
                    Caster.FixedParticles(
                        0,
                        10,
                        5,
                        2003,
                        Server.Misc.PlayerSettings.GetMySpellHue(true, Caster, 0),
                        0,
                        EffectLayer.RightHand
                    );
                    Caster.PlaySound(0x1E2);
                }
            }
            finally
            {
                FinishSequence();
            }
        }
    }

    public class FoodInfo
    {
        private Type m_Type;
        private string m_Name;

        public Type Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public FoodInfo(Type type, string name)
        {
            m_Type = type;
            m_Name = name;
        }

        public Item Create()
        {
            Item item;

            try
            {
                item = (Item)Activator.CreateInstance(m_Type);
            }
            catch
            {
                item = null;
            }

            return item;
        }
    }
}
