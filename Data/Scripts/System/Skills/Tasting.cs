using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class Tasting
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Tasting].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();

            m.SendLocalizedMessage(502807); // What would you like to taste?

            return TimeSpan.FromSeconds(1.0);
        }

        [PlayerVendorTarget]
        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(2, false, TargetFlags.None)
            {
                AllowNonlocal = true;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    from.SendLocalizedMessage(502816); // You feel that such an action would be inappropriate.
                }
                else if (targeted is Food)
                {
                    Food food = (Food)targeted;

                    if (from.CheckTargetSkill(SkillName.Tasting, food, 0, 125))
                    {
                        if (food.Poison != null)
                        {
                            food.SendLocalizedMessageTo(from, 1038284); // It appears to have poison smeared on it.
                        }
                        else
                        {
                            // No poison on the food
                            food.SendLocalizedMessageTo(from, 1010600); // You detect nothing unusual about this substance.
                        }
                    }
                    else
                    {
                        // Skill check failed
                        food.SendLocalizedMessageTo(from, 502823); // You cannot discern anything about this substance.
                    }
                }
                else if (targeted is BasePotion)
                {
                    BasePotion potion = (BasePotion)targeted;

                    potion.SendLocalizedMessageTo(from, 502813); // You already know what kind of potion that is.
                    potion.SendLocalizedMessageTo(from, potion.LabelNumber);
                }
                else if (targeted is PotionKeg)
                {
                    PotionKeg keg = (PotionKeg)targeted;

                    if (keg.Held <= 0)
                    {
                        keg.SendLocalizedMessageTo(from, 502228); // There is nothing in the keg to taste!
                    }
                    else
                    {
                        keg.SendLocalizedMessageTo(from, 502229); // You are already familiar with this keg's contents.
                        keg.SendLocalizedMessageTo(from, keg.LabelNumber);
                    }
                }
                else if (targeted is StaticTarget)
                {
                    StaticTarget targ = (StaticTarget)targeted;

                    if (targ.ItemID >= 3340 && targ.ItemID <= 3348)
                    {
                        if (from.Skills[SkillName.Camping].Value >= 50)
                        {
                            if (from.CheckTargetSkill(SkillName.Tasting, targeted, 0, 100))
                            {
                                from.AddToBackpack(new Mushrooms(targ.ItemID));
                                from.SendMessage("You find some edible mushrooms!");
                            }
                            else
                            {
                                from.SendMessage("You fail to find any edible mushrooms.");
                            }
                        }
                        else
                        {
                            from.SendMessage("Your Camping skill is too low to find mushrooms.");
                        }
                    }
                }
                else if (targeted is AddonComponent)
                {
                    AddonComponent targ = (AddonComponent)targeted;

                    if (targ.ItemID >= 3340 && targ.ItemID <= 3348)
                    {
                        if (from.Skills[SkillName.Camping].Value >= 50)
                        {
                            if (from.CheckTargetSkill(SkillName.Tasting, targeted, 0, 100))
                            {
                                from.AddToBackpack(new Mushrooms(targ.ItemID));
                                from.SendMessage("You find some edible mushrooms!");
                            }
                            else
                            {
                                from.SendMessage("You fail to find any edible mushrooms.");
                            }
                        }
                        else
                        {
                            from.SendMessage("Your Camping skill is too low to find mushrooms.");
                        }
                    }
                    // End TasteID mushroom hunting
                    else
                    {
                        // The target is not food or potion or potion keg.
                        from.SendLocalizedMessage(502820); // That's not something you can taste.
                    }
                }
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendLocalizedMessage(502815); // You are too far away to taste that.
            }
        }
    }
}
