using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class RunicClockworkAssembly : Item
    {
        [Constructable]
        public RunicClockworkAssembly()
            : base(0x1eac)
        {
            Name = "Runic clockwork assembly";
            Weight = 50.0;
            Hue = 38;
        }

        public RunicClockworkAssembly(Serial serial)
            : base(serial) { }

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

        public override void OnDoubleClick(Mobile from)
        {
            if (Deleted)
                return;

            if (RootParent != from)
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            Container ourPack = from.Backpack;

            if (ourPack == null)
                return;

            Item powercore = null;

            powercore = ourPack.FindItemByType(typeof(RunicGolemInvaderPowerCore));
            if (powercore != null)
            {
                ConstructRunicGolemInvader(from);
                return;
            }

            powercore = ourPack.FindItemByType(typeof(GargoylePowerCore));
            if (powercore != null)
            {
                ConstructGargoyle(from);
                return;
            }

            powercore = ourPack.FindItemByType(typeof(OverseerPowerCore));
            if (powercore != null)
            {
                ConstructOverseer(from);
                return;
            }

            powercore = ourPack.FindItemByType(typeof(DaemonPowerCore));
            if (powercore != null)
            {
                ConstructDaemon(from);
                return;
            }

            powercore = ourPack.FindItemByType(typeof(DragonPowerCore));
            if (powercore != null)
            {
                ConstructDragon(from);
                return;
            }

            from.SendMessage(
                "You don't have any power cores in your backpack.  You cannot contruct anything."
            );
        }

        public void ConstructRunicGolemInvader(Mobile from)
        {
            Type[] types = new Type[5];
            int[] amounts = new int[5];

            if (from.Skills[SkillName.Tinkering].Value < 105.0)
            {
                from.SendMessage("You are not skilled enough to construct a runic golem invader");
                return;
            }

            Container ourPack = from.Backpack;

            if (ourPack == null)
                return;

            Item powercore = ourPack.FindItemByType(typeof(RunicGolemInvaderPowerCore));
            Item ironingot = ourPack.FindItemByType(typeof(IronIngot));
            Item bronzeingot = ourPack.FindItemByType(typeof(BronzeIngot));
            Item gears = ourPack.FindItemByType(typeof(Gears));

            if (ironingot == null)
            {
                from.SendMessage("You need 150 iron ingots to construct a runic golem invader");
                return;
            }
            else
            {
                if (ironingot.Amount < 150)
                {
                    from.SendMessage("You need 150 iron ingots to construct a runic golem invader");
                    return;
                }
                else
                {
                    types[0] = typeof(IronIngot);
                    amounts[0] = 150;
                }
            }

            if (bronzeingot == null)
            {
                from.SendMessage("You need 75 bronze ingots to construct a runic golem invader");
                return;
            }
            else
            {
                if (bronzeingot.Amount < 75)
                {
                    from.SendMessage(
                        "You need 75 bronze ingots to construct a runic golem invader"
                    );
                    return;
                }
                else
                {
                    types[1] = typeof(BronzeIngot);
                    amounts[1] = 75;
                }
            }

            if (gears == null)
            {
                from.SendMessage("You need 25 gears to construct a runic golem invader");
                return;
            }
            else
            {
                if (gears.Amount < 25)
                {
                    from.SendMessage("You need 5 gears to construct a runic golem invader");
                    return;
                }
                else
                {
                    types[2] = typeof(Gears);
                    amounts[2] = 25;
                }
            }

            if (powercore == null)
            {
                from.SendMessage(
                    "You need a runic golem invader power core to construct a runic golem invader"
                );
                return;
            }
            else
            {
                types[3] = typeof(RunicGolemInvaderPowerCore);
                amounts[3] = 1;
            }

            types[4] = typeof(RunicClockworkAssembly);
            amounts[4] = 1;

            if ((from.Followers + 4) > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to construct a  runic golem invader");
                return;
            }

            if (!from.CheckSkill(SkillName.Tinkering, 105.0, 120.0))
            {
                types = new Type[2];
                amounts = new int[2];

                types[0] = typeof(IronIngot);
                amounts[0] = Utility.Random(1, 10);
                types[1] = typeof(BronzeIngot);
                amounts[1] = Utility.Random(1, 10);

                ourPack.ConsumeTotal(types, amounts, true);
                from.SendMessage("You fail to create the runic golem invader and lose some ingots");
                return;
            }
            else
            {
                ourPack.ConsumeTotal(types, amounts, true);
            }

            double percentage = (from.Skills[SkillName.Tinkering].Base - 105) / 35;
            if (percentage < 0.5)
                percentage = 0.5;
            else if (percentage > 1.0)
                percentage = 1.0;

            object o = Activator.CreateInstance(typeof(RunicGolemInvader));

            RunicGolemInvader assembly = (RunicGolemInvader)o;

            assembly.Str = (int)(assembly.Str * percentage);
            assembly.Dex = (int)(assembly.Dex * percentage);
            assembly.Int = (int)(assembly.Int * percentage);

            assembly.Skills[SkillName.Anatomy].Base = Math.Round(
                assembly.Skills[SkillName.Anatomy].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Bludgeoning].Base = Math.Round(
                assembly.Skills[SkillName.Bludgeoning].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Tactics].Base = Math.Round(
                assembly.Skills[SkillName.Tactics].Base * percentage,
                1
            );
            assembly.Skills[SkillName.MagicResist].Base = Math.Round(
                assembly.Skills[SkillName.MagicResist].Base * percentage,
                1
            );

            assembly.Hits = assembly.HitsMax;
            assembly.ActiveSpeed = 0.1;

            assembly.Crafted = true;
            assembly.Controlled = true;
            assembly.ControlMaster = from;
            assembly.ControlOrder = OrderType.None;
            assembly.Tamable = false;
            assembly.ControlSlots = 4;
            assembly.Loyalty = 100;
            assembly.Creator = from;
            assembly.UpgradeLevel = 0;

            assembly.Map = from.Map;
            assembly.Location = from.Location;

            from.Followers += 4;
        }

        public void ConstructGargoyle(Mobile from)
        {
            Type[] types = new Type[5];
            int[] amounts = new int[5];

            if (from.Skills[SkillName.Tinkering].Value < 45.0)
            {
                from.SendMessage("You are not skilled enough to construct a gargoyle");
                return;
            }

            Container ourPack = from.Backpack;

            if (ourPack == null)
                return;

            Item powercore = ourPack.FindItemByType(typeof(GargoylePowerCore));
            Item ironingot = ourPack.FindItemByType(typeof(IronIngot));
            Item goldingot = ourPack.FindItemByType(typeof(GoldIngot));
            Item gears = ourPack.FindItemByType(typeof(Gears));

            if (ironingot == null)
            {
                from.SendMessage("You need 50 iron ingots to construct a gargoyle");
                return;
            }
            else
            {
                if (ironingot.Amount < 50)
                {
                    from.SendMessage("You need 50 iron ingots to construct a gargoyle");
                    return;
                }
                else
                {
                    types[0] = typeof(IronIngot);
                    amounts[0] = 50;
                }
            }

            if (goldingot == null)
            {
                from.SendMessage("You need 75 gold ingots to construct a gargoyle");
                return;
            }
            else
            {
                if (goldingot.Amount < 75)
                {
                    from.SendMessage("You need 75 gold ingots to construct a gargoyle");
                    return;
                }
                else
                {
                    types[1] = typeof(GoldIngot);
                    amounts[1] = 75;
                }
            }

            if (gears == null)
            {
                from.SendMessage("You need 10 gears to construct a gargoyle");
                return;
            }
            else
            {
                if (gears.Amount < 10)
                {
                    from.SendMessage("You need 10 gears to construct a gargoyle");
                    return;
                }
                else
                {
                    types[2] = typeof(Gears);
                    amounts[2] = 10;
                }
            }

            if (powercore == null)
            {
                from.SendMessage("You need a gargoyle power core to construct a gargoyle");
                return;
            }
            else
            {
                types[3] = typeof(GargoylePowerCore);
                amounts[3] = 1;
            }

            types[4] = typeof(RunicClockworkAssembly);
            amounts[4] = 1;

            if ((from.Followers + 3) > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to construct a gargoyle");
                return;
            }

            if (!from.CheckSkill(SkillName.Tinkering, 45.0, 80.0))
            {
                types = new Type[2];
                amounts = new int[2];

                types[0] = typeof(IronIngot);
                amounts[0] = Utility.Random(1, 10);
                types[1] = typeof(GoldIngot);
                amounts[1] = Utility.Random(1, 10);

                ourPack.ConsumeTotal(types, amounts, true);
                from.SendMessage("You fail to create the gargoyle an lose some ingots");
                return;
            }
            else
            {
                ourPack.ConsumeTotal(types, amounts, true);
            }

            double percentage = (from.Skills[SkillName.Tinkering].Base - 45) / 35;
            if (percentage < 0.5)
                percentage = 0.5;
            else if (percentage > 1.0)
                percentage = 1.0;

            object o = Activator.CreateInstance(typeof(MechGargoyle));

            MechGargoyle assembly = (MechGargoyle)o;

            assembly.Str = (int)(assembly.Str * percentage);
            assembly.Dex = (int)(assembly.Dex * percentage);
            assembly.Int = (int)(assembly.Int * percentage);

            assembly.Skills[SkillName.Anatomy].Base = Math.Round(
                assembly.Skills[SkillName.Anatomy].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Swords].Base = Math.Round(
                assembly.Skills[SkillName.Swords].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Tactics].Base = Math.Round(
                assembly.Skills[SkillName.Tactics].Base * percentage,
                1
            );
            assembly.Skills[SkillName.MagicResist].Base = Math.Round(
                assembly.Skills[SkillName.MagicResist].Base * percentage,
                1
            );

            assembly.Hits = assembly.HitsMax;
            assembly.ActiveSpeed = 0.1;

            assembly.Crafted = true;
            assembly.Controlled = true;
            assembly.ControlMaster = from;
            assembly.ControlOrder = OrderType.None;
            assembly.Tamable = false;
            assembly.ControlSlots = 3;
            assembly.Loyalty = 100;
            assembly.Creator = from;
            assembly.UpgradeLevel = 0;

            assembly.Map = from.Map;
            assembly.Location = from.Location;

            from.Followers += 3;
        }

        public void ConstructOverseer(Mobile from)
        {
            Type[] types = new Type[5];
            int[] amounts = new int[5];

            if (from.Skills[SkillName.Tinkering].Value < 55.0)
            {
                from.SendMessage("You are not skilled enough to construct an overseer");
                return;
            }

            Container ourPack = from.Backpack;

            if (ourPack == null)
                return;

            Item powercore = ourPack.FindItemByType(typeof(OverseerPowerCore));
            Item ironingot = ourPack.FindItemByType(typeof(IronIngot));
            Item agapiteingot = ourPack.FindItemByType(typeof(AgapiteIngot));
            Item gears = ourPack.FindItemByType(typeof(Gears));

            if (ironingot == null)
            {
                from.SendMessage("You need 50 iron ingots to construct an overseer");
                return;
            }
            else
            {
                if (ironingot.Amount < 50)
                {
                    from.SendMessage("You need 50 iron ingots to construct an overseer");
                    return;
                }
                else
                {
                    types[0] = typeof(IronIngot);
                    amounts[0] = 50;
                }
            }

            if (agapiteingot == null)
            {
                from.SendMessage("You need 75 agapite ingots to construct an overseer");
                return;
            }
            else
            {
                if (agapiteingot.Amount < 75)
                {
                    from.SendMessage("You need 75 agapite ingots to construct an overseer");
                    return;
                }
                else
                {
                    types[1] = typeof(AgapiteIngot);
                    amounts[1] = 75;
                }
            }

            if (gears == null)
            {
                from.SendMessage("You need 10 gears to construct an overseer");
                return;
            }
            else
            {
                if (gears.Amount < 10)
                {
                    from.SendMessage("You need 10 gears to construct an overseer");
                    return;
                }
                else
                {
                    types[2] = typeof(Gears);
                    amounts[2] = 10;
                }
            }

            if (powercore == null)
            {
                from.SendMessage("You need an overseer power core to construct an overseer");
                return;
            }
            else
            {
                types[3] = typeof(OverseerPowerCore);
                amounts[3] = 1;
            }

            types[4] = typeof(RunicClockworkAssembly);
            amounts[4] = 1;

            if ((from.Followers + 3) > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to construct an overseer");
                return;
            }

            if (!from.CheckSkill(SkillName.Tinkering, 55.0, 90.0))
            {
                types = new Type[2];
                amounts = new int[2];

                types[0] = typeof(IronIngot);
                amounts[0] = Utility.Random(1, 10);
                types[1] = typeof(AgapiteIngot);
                amounts[1] = Utility.Random(1, 10);

                ourPack.ConsumeTotal(types, amounts, true);
                from.SendMessage("You fail to create the overseer an lose some ingots");
                return;
            }
            else
            {
                ourPack.ConsumeTotal(types, amounts, true);
            }

            double percentage = (from.Skills[SkillName.Tinkering].Base - 55) / 35;
            if (percentage < 0.5)
                percentage = 0.5;
            else if (percentage > 1.0)
                percentage = 1.0;

            object o = Activator.CreateInstance(typeof(Overseer));

            Overseer assembly = (Overseer)o;

            assembly.Str = (int)(assembly.Str * percentage);
            assembly.Dex = (int)(assembly.Dex * percentage);
            assembly.Int = (int)(assembly.Int * percentage);

            assembly.Skills[SkillName.Anatomy].Base = Math.Round(
                assembly.Skills[SkillName.Anatomy].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Swords].Base = Math.Round(
                assembly.Skills[SkillName.Swords].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Tactics].Base = Math.Round(
                assembly.Skills[SkillName.Tactics].Base * percentage,
                1
            );
            assembly.Skills[SkillName.MagicResist].Base = Math.Round(
                assembly.Skills[SkillName.MagicResist].Base * percentage,
                1
            );

            assembly.Hits = assembly.HitsMax;
            assembly.ActiveSpeed = 0.1;

            assembly.Crafted = true;
            assembly.Controlled = true;
            assembly.ControlMaster = from;
            assembly.ControlOrder = OrderType.None;
            assembly.Tamable = false;
            assembly.ControlSlots = 3;
            assembly.Loyalty = 100;
            assembly.Creator = from;
            assembly.UpgradeLevel = 0;

            assembly.Map = from.Map;
            assembly.Location = from.Location;

            from.Followers += 3;
        }

        public void ConstructDaemon(Mobile from)
        {
            Type[] types = new Type[6];
            int[] amounts = new int[6];

            if (from.Skills[SkillName.Tinkering].Value < 75.0)
            {
                from.SendMessage("You are not skilled enough to construct a daemon");
                return;
            }

            Container ourPack = from.Backpack;

            if (ourPack == null)
                return;

            Item powercore = ourPack.FindItemByType(typeof(DaemonPowerCore));
            Item ironingot = ourPack.FindItemByType(typeof(IronIngot));
            Item veriteingot = ourPack.FindItemByType(typeof(VeriteIngot));
            Item gears = ourPack.FindItemByType(typeof(Gears));
            Item daemonbone = ourPack.FindItemByType(typeof(DaemonBone));

            if (ironingot == null)
            {
                from.SendMessage("You need 50 iron ingots to construct a daemon");
                return;
            }
            else
            {
                if (ironingot.Amount < 50)
                {
                    from.SendMessage("You need 50 iron ingots to construct a daemon");
                    return;
                }
                else
                {
                    types[0] = typeof(IronIngot);
                    amounts[0] = 50;
                }
            }

            if (veriteingot == null)
            {
                from.SendMessage("You need 100 verite ingots to construct a daemon");
                return;
            }
            else
            {
                if (veriteingot.Amount < 100)
                {
                    from.SendMessage("You need 100 verite ingots to construct a daemon");
                    return;
                }
                else
                {
                    types[1] = typeof(VeriteIngot);
                    amounts[1] = 100;
                }
            }

            if (gears == null)
            {
                from.SendMessage("You need 10 gears to construct a daemon");
                return;
            }
            else
            {
                if (gears.Amount < 10)
                {
                    from.SendMessage("You need 10 gears to construct a daemon");
                    return;
                }
                else
                {
                    types[2] = typeof(Gears);
                    amounts[2] = 10;
                }
            }

            if (daemonbone == null)
            {
                from.SendMessage("You need 50 daemon bone to construct a daemon");
                return;
            }
            else
            {
                if (daemonbone.Amount < 50)
                {
                    from.SendMessage("You need 50 daemon bone to construct a daemon");
                    return;
                }
                else
                {
                    types[3] = typeof(DaemonBone);
                    amounts[3] = 50;
                }
            }

            if (powercore == null)
            {
                from.SendMessage("You need a daemon power core to construct a daemon");
                return;
            }
            else
            {
                types[4] = typeof(DaemonPowerCore);
                amounts[4] = 1;
            }

            types[5] = typeof(RunicClockworkAssembly);
            amounts[5] = 1;

            if ((from.Followers + 3) > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to construct a daemon");
                return;
            }

            if (!from.CheckSkill(SkillName.Tinkering, 75.0, 110.0))
            {
                types = new Type[2];
                amounts = new int[2];

                types[0] = typeof(IronIngot);
                amounts[0] = Utility.Random(1, 10);
                types[1] = typeof(VeriteIngot);
                amounts[1] = Utility.Random(1, 10);

                ourPack.ConsumeTotal(types, amounts, true);
                from.SendMessage("You fail to create the daeom an lose some ingots");
                return;
            }
            else
            {
                ourPack.ConsumeTotal(types, amounts, true);
            }

            double percentage = (from.Skills[SkillName.Tinkering].Base - 75) / 35;
            if (percentage < 0.5)
                percentage = 0.5;
            else if (percentage > 1.0)
                percentage = 1.0;

            object o = Activator.CreateInstance(typeof(MetalDaemon));

            MetalDaemon assembly = (MetalDaemon)o;

            assembly.Str = (int)(assembly.Str * percentage);
            assembly.Dex = (int)(assembly.Dex * percentage);
            assembly.Int = (int)(assembly.Int * percentage);

            assembly.Skills[SkillName.Anatomy].Base = Math.Round(
                assembly.Skills[SkillName.Anatomy].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Bludgeoning].Base = Math.Round(
                assembly.Skills[SkillName.Bludgeoning].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Tactics].Base = Math.Round(
                assembly.Skills[SkillName.Tactics].Base * percentage,
                1
            );
            assembly.Skills[SkillName.MagicResist].Base = Math.Round(
                assembly.Skills[SkillName.MagicResist].Base * percentage,
                1
            );

            assembly.Hits = assembly.HitsMax;
            assembly.ActiveSpeed = 0.1;

            assembly.Crafted = true;
            assembly.Controlled = true;
            assembly.ControlMaster = from;
            assembly.ControlOrder = OrderType.None;
            assembly.Tamable = false;
            assembly.ControlSlots = 4;
            assembly.Loyalty = 100;
            assembly.Creator = from;
            assembly.UpgradeLevel = 0;

            assembly.Map = from.Map;
            assembly.Location = from.Location;

            from.Followers += 4;
        }

        public void ConstructDragon(Mobile from)
        {
            Type[] types = new Type[6];
            int[] amounts = new int[6];

            if (from.Skills[SkillName.Tinkering].Value < 85.0)
            {
                from.SendMessage("You are not skilled enough to construct a dragon");
                return;
            }

            Container ourPack = from.Backpack;

            if (ourPack == null)
                return;

            Item powercore = ourPack.FindItemByType(typeof(DragonPowerCore));
            Item ironingot = ourPack.FindItemByType(typeof(IronIngot));
            Item valoriteingot = ourPack.FindItemByType(typeof(ValoriteIngot));
            Item gears = ourPack.FindItemByType(typeof(Gears));
            Item dragonsblood = ourPack.FindItemByType(typeof(DragonsBlood));

            if (ironingot == null)
            {
                from.SendMessage("You need 50 iron ingots to construct a dragon");
                return;
            }
            else
            {
                if (ironingot.Amount < 50)
                {
                    from.SendMessage("You need 50 iron ingots to construct a dragon");
                    return;
                }
                else
                {
                    types[0] = typeof(IronIngot);
                    amounts[0] = 50;
                }
            }

            if (valoriteingot == null)
            {
                from.SendMessage("You need 100 valorite ingots to construct a dragon");
                return;
            }
            else
            {
                if (valoriteingot.Amount < 100)
                {
                    from.SendMessage("You need 100 valorite ingots to construct a dragon");
                    return;
                }
                else
                {
                    types[1] = typeof(ValoriteIngot);
                    amounts[1] = 100;
                }
            }

            if (gears == null)
            {
                from.SendMessage("You need 10 gears to construct a dragon");
                return;
            }
            else
            {
                if (gears.Amount < 10)
                {
                    from.SendMessage("You need 10 gears to construct a dragon");
                    return;
                }
                else
                {
                    types[2] = typeof(Gears);
                    amounts[2] = 10;
                }
            }

            if (dragonsblood == null)
            {
                from.SendMessage("You need 50 vials of dragon's blood to construct a daemon");
                return;
            }
            else
            {
                if (dragonsblood.Amount < 50)
                {
                    from.SendMessage("You need 50 vials of dragon's blood to construct a daemon");
                    return;
                }
                else
                {
                    types[3] = typeof(DragonsBlood);
                    amounts[3] = 50;
                }
            }

            if (powercore == null)
            {
                from.SendMessage("You need a dragon power core to construct a dragon");
                return;
            }
            else
            {
                types[4] = typeof(DragonPowerCore);
                amounts[4] = 1;
            }

            types[5] = typeof(RunicClockworkAssembly);
            amounts[5] = 1;

            if ((from.Followers + 3) > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to construct a dragon");
                return;
            }

            if (!from.CheckSkill(SkillName.Tinkering, 85.0, 120.0))
            {
                types = new Type[2];
                amounts = new int[2];

                types[0] = typeof(IronIngot);
                amounts[0] = Utility.Random(1, 10);
                types[1] = typeof(ValoriteIngot);
                amounts[1] = Utility.Random(1, 10);

                ourPack.ConsumeTotal(types, amounts, true);
                from.SendMessage("You fail to create the dragon an lose some ingots");
                return;
            }
            else
            {
                ourPack.ConsumeTotal(types, amounts, true);
            }

            double percentage = (from.Skills[SkillName.Tinkering].Base - 85) / 35;
            if (percentage < 0.5)
                percentage = 0.5;
            else if (percentage > 1.0)
                percentage = 1.0;

            object o = Activator.CreateInstance(typeof(IronDragon));

            IronDragon assembly = (IronDragon)o;

            assembly.Str = (int)(assembly.Str * percentage);
            assembly.Dex = (int)(assembly.Dex * percentage);
            assembly.Int = (int)(assembly.Int * percentage);

            assembly.Skills[SkillName.Anatomy].Base = Math.Round(
                assembly.Skills[SkillName.Anatomy].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Bludgeoning].Base = Math.Round(
                assembly.Skills[SkillName.Bludgeoning].Base * percentage,
                1
            );
            assembly.Skills[SkillName.Tactics].Base = Math.Round(
                assembly.Skills[SkillName.Tactics].Base * percentage,
                1
            );
            assembly.Skills[SkillName.MagicResist].Base = Math.Round(
                assembly.Skills[SkillName.MagicResist].Base * percentage,
                1
            );

            assembly.Hits = assembly.HitsMax;
            assembly.ActiveSpeed = 0.1;

            assembly.Crafted = true;
            assembly.Controlled = true;
            assembly.ControlMaster = from;
            assembly.ControlOrder = OrderType.None;
            assembly.Tamable = false;
            assembly.ControlSlots = 4;
            assembly.Loyalty = 100;
            assembly.Creator = from;
            assembly.UpgradeLevel = 0;

            assembly.Map = from.Map;
            assembly.Location = from.Location;

            from.Followers += 4;
        }
    }
}
