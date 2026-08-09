using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Server.Commands;
using Server.Factions;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Misc
{
    public static class TestCenter
    {
        private const string SuppliesName = "Test Center Supplies";
        private const int SuppliesItemID = 0xE7D;
        private const int SuppliesHue = 0x489;
        private const int CorpseSearchRadius = 12;

        public static bool Enabled
        {
            get { return MyServerSettings.TestCenterEnabled(); }
        }

        public static void Initialize()
        {
            if (!Enabled)
                return;

            Console.WriteLine("*******************************************************");
            Console.WriteLine("*** TEST CENTER IS ENABLED FOR ALL PLAYER ACCOUNTS. ***");
            Console.WriteLine("*******************************************************");

            CommandSystem.Register(
                "TCSupplies",
                AccessLevel.Player,
                new CommandEventHandler(Supplies_OnCommand)
            );
            CommandSystem.Register(
                "TCResurrect",
                AccessLevel.Player,
                new CommandEventHandler(Resurrect_OnCommand)
            );
            CommandSystem.Register(
                "TCCorpses",
                AccessLevel.Player,
                new CommandEventHandler(Corpses_OnCommand)
            );

            EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
        }

        private static void EventSink_Speech(SpeechEventArgs args)
        {
            if (args == null || args.Handled || args.Mobile == null || !args.Mobile.Player)
                return;

            string speech = args.Speech == null ? String.Empty : args.Speech.Trim();

            if (Insensitive.Equals(speech, "help"))
            {
                args.Mobile.CloseGump(typeof(TCHelpGump));
                args.Mobile.SendGump(new TCHelpGump());
                args.Handled = true;
                return;
            }

            if (!Insensitive.StartsWith(speech, "set"))
                return;

            args.Handled = true;

            string[] split = speech.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 3)
            {
                args.Mobile.SendMessage("Usage: set str|dex|int <value> or set <skill name> <value>");
                return;
            }

            string name = split[1];

            if (
                Insensitive.Equals(name, "str")
                || Insensitive.Equals(name, "dex")
                || Insensitive.Equals(name, "int")
            )
            {
                int statValue;

                if (!Int32.TryParse(split[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out statValue))
                {
                    args.Mobile.SendMessage("Stats must be set to a whole number between 10 and 125.");
                    return;
                }

                if (Insensitive.Equals(name, "str"))
                    ChangeStrength(args.Mobile, statValue);
                else if (Insensitive.Equals(name, "dex"))
                    ChangeDexterity(args.Mobile, statValue);
                else
                    ChangeIntelligence(args.Mobile, statValue);

                return;
            }

            double skillValue;

            if (
                !Double.TryParse(
                    split[2],
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out skillValue
                )
            )
            {
                args.Mobile.SendMessage("Skills must be set to a numeric value.");
                return;
            }

            ChangeSkill(args.Mobile, name, skillValue);
        }

        private static void ChangeStrength(Mobile from, int value)
        {
            if (!CheckStatValue(from, value))
                return;

            if ((value + from.RawDex + from.RawInt) > from.StatCap)
                from.SendLocalizedMessage(1005629);
            else
            {
                from.RawStr = value;
                from.SendLocalizedMessage(1005630);
            }
        }

        private static void ChangeDexterity(Mobile from, int value)
        {
            if (!CheckStatValue(from, value))
                return;

            if ((from.RawStr + value + from.RawInt) > from.StatCap)
                from.SendLocalizedMessage(1005629);
            else
            {
                from.RawDex = value;
                from.SendLocalizedMessage(1005630);
            }
        }

        private static void ChangeIntelligence(Mobile from, int value)
        {
            if (!CheckStatValue(from, value))
                return;

            if ((from.RawStr + from.RawDex + value) > from.StatCap)
                from.SendLocalizedMessage(1005629);
            else
            {
                from.RawInt = value;
                from.SendLocalizedMessage(1005630);
            }
        }

        private static bool CheckStatValue(Mobile from, int value)
        {
            if (value >= 10 && value <= 125)
                return true;

            from.SendLocalizedMessage(1005628);
            return false;
        }

        private static void ChangeSkill(Mobile from, string name, double value)
        {
            SkillName skillName;

            if (
                !Enum.TryParse<SkillName>(name, true, out skillName)
                || !Enum.IsDefined(typeof(SkillName), skillName)
            )
            {
                from.SendLocalizedMessage(1005631);
                return;
            }

            Skill skill = from.Skills[skillName];

            if (skill == null)
            {
                from.SendLocalizedMessage(1005631);
                return;
            }

            if (value < 0.0 || value > skill.Cap)
            {
                from.SendMessage("Your skill in {0} is capped at {1:F1}.", skill.Info.Name, skill.Cap);
                return;
            }

            int newFixedPoint = (int)(value * 10.0);
            int oldFixedPoint = skill.BaseFixedPoint;

            if (((skill.Owner.Total - oldFixedPoint) + newFixedPoint) > skill.Owner.Cap)
            {
                from.SendMessage("You can not exceed the skill cap. Try setting another skill lower first.");
                return;
            }

            skill.BaseFixedPoint = newFixedPoint;
            from.SendMessage("Your skill in {0} is now {1:F1}.", skill.Info.Name, skill.Base);
        }

        [Usage("TCSupplies")]
        [Description("Claims the one-time Test Center supply package.")]
        private static void Supplies_OnCommand(CommandEventArgs e)
        {
            GiveSupplies(e.Mobile, true);
        }

        [Usage("TCResurrect")]
        [Description("Resurrects your character without a fee or death penalty on the Test Center.")]
        private static void Resurrect_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || from.Deleted)
                return;

            if (from.Alive)
            {
                from.SendMessage("You are already alive.");
                return;
            }

            if (
                from.Map == null
                || from.Map == Map.Internal
                || !from.Map.CanFit(from.Location, 16, false, false)
            )
            {
                from.SendLocalizedMessage(502391);
                return;
            }

            from.Resurrect();

            if (from.Alive)
                from.SendMessage("You have resurrected yourself through the Test Center.");
            else
                from.SendMessage("You cannot be resurrected at this location.");
        }

        [Usage("TCCorpses")]
        [Description("Moves your active corpses to separate valid tiles near your location.")]
        private static void Corpses_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (
                from == null
                || from.Deleted
                || from.Map == null
                || from.Map == Map.Internal
            )
            {
                return;
            }

            List<Corpse> corpses = new List<Corpse>();

            foreach (Item item in World.Items.Values)
            {
                Corpse corpse = item as Corpse;

                if (
                    corpse != null
                    && !corpse.Deleted
                    && corpse.Owner == from
                    && corpse.Parent == null
                    && corpse.Map != null
                    && corpse.Map != Map.Internal
                )
                {
                    corpses.Add(corpse);
                }
            }

            if (corpses.Count == 0)
            {
                from.SendMessage("No active corpses belonging to you could be found.");
                return;
            }

            corpses.Sort(
                delegate(Corpse left, Corpse right)
                {
                    return right.TimeOfDeath.CompareTo(left.TimeOfDeath);
                }
            );

            List<Point3D> destinations = FindCorpseDestinations(from, corpses.Count);
            int moved = Math.Min(corpses.Count, destinations.Count);

            for (int i = 0; i < moved; ++i)
                corpses[i].MoveToWorld(destinations[i], from.Map);

            int skipped = corpses.Count - moved;

            from.SendMessage(
                "Test Center corpse retrieval moved {0} corpse{1} and skipped {2}.",
                moved,
                moved == 1 ? String.Empty : "s",
                skipped
            );
        }

        private static List<Point3D> FindCorpseDestinations(Mobile from, int count)
        {
            List<Point3D> destinations = new List<Point3D>();
            Map map = from.Map;

            for (int radius = 0; radius <= CorpseSearchRadius && destinations.Count < count; ++radius)
            {
                for (int xOffset = -radius; xOffset <= radius && destinations.Count < count; ++xOffset)
                {
                    for (
                        int yOffset = -radius;
                        yOffset <= radius && destinations.Count < count;
                        ++yOffset
                    )
                    {
                        if (
                            radius > 0
                            && Math.Abs(xOffset) != radius
                            && Math.Abs(yOffset) != radius
                        )
                        {
                            continue;
                        }

                        Point3D destination;

                        if (
                            TryGetCorpseDestination(
                                map,
                                from.X + xOffset,
                                from.Y + yOffset,
                                from.Z,
                                out destination
                            )
                        )
                        {
                            destinations.Add(destination);
                        }
                    }
                }
            }

            return destinations;
        }

        private static bool TryGetCorpseDestination(
            Map map,
            int x,
            int y,
            int preferredZ,
            out Point3D destination
        )
        {
            destination = new Point3D(x, y, preferredZ);

            if (map.CanFit(destination, 16, false, false))
                return true;

            int averageZ = map.GetAverageZ(x, y);
            destination = new Point3D(x, y, averageZ);

            return map.CanFit(destination, 16, false, false);
        }

        public static bool GiveSupplies(Mobile from, bool sendMessage)
        {
            if (!Enabled || from == null || from.Deleted || !from.Player)
                return false;

            BankBox bank = from.BankBox;

            if (FindSupplies(bank) != null)
            {
                if (sendMessage)
                    from.SendMessage("You have already received your Test Center supplies.");

                return false;
            }

            WoodenBox supplies = new WoodenBox();
            supplies.ItemID = SuppliesItemID;
            supplies.Hue = SuppliesHue;
            supplies.Name = SuppliesName;
            supplies.Movable = false;

            try
            {
                FillSupplies(from, supplies);
                bank.DropItem(supplies);
                RaiseCaps(from);
            }
            catch (Exception exception)
            {
                supplies.Delete();

                Console.WriteLine(
                    "Test Center: Failed to create supplies for {0}: {1}",
                    from,
                    exception
                );

                if (sendMessage)
                    from.SendMessage("Your Test Center supplies could not be created. Please contact staff.");

                return false;
            }

            if (sendMessage)
                from.SendMessage("Your Test Center supplies have been placed in your bank box.");

            return true;
        }

        private static Container FindSupplies(BankBox bank)
        {
            if (bank == null)
                return null;

            for (int i = 0; i < bank.Items.Count; ++i)
            {
                Container container = bank.Items[i] as Container;

                if (
                    container != null
                    && container.ItemID == SuppliesItemID
                    && container.Hue == SuppliesHue
                    && !container.Movable
                    && Insensitive.Equals(container.Name, SuppliesName)
                )
                {
                    return container;
                }
            }

            return null;
        }

        private static void RaiseCaps(Mobile from)
        {
            if (from.StatCap < 250)
                from.StatCap = 250;

            for (int i = 0; i < from.Skills.Length; ++i)
            {
                Skill skill = from.Skills[i];

                if (skill != null && skill.Cap < 120.0)
                    skill.Cap = 120.0;
            }
        }

        private static void FillSupplies(Mobile from, Container supplies)
        {
            AddCurrency(supplies);
            AddPotionKegs(supplies);
            AddTools(supplies);
            AddAmmunition(supplies);
            AddTreasureMaps(from, supplies);
            AddRawMaterials(supplies);
            AddSpellCastingSupplies(supplies);
            AddEthereals(supplies);
            AddArtifacts(supplies);
            AddMinorArtifacts(from, supplies);
            AddTokunoArtifacts(supplies);
            AddBows(supplies);
        }

        private static void AddCurrency(Container supplies)
        {
            WoodenBox box = new WoodenBox();
            box.Name = "Currency";

            box.DropItem(new BankCheck(500000));
            box.DropItem(new BankCheck(250000));
            box.DropItem(new BankCheck(100000));
            box.DropItem(new BankCheck(100000));
            box.DropItem(new BankCheck(50000));
            box.DropItem(new Silver(9000));
            box.DropItem(new Gold(60000));

            supplies.DropItem(box);
        }

        private static void AddPotionKegs(Container supplies)
        {
            Backpack bag = new Backpack();
            bag.Name = "Various Potion Kegs";

            bag.DropItem(MakePotionKeg(PotionEffect.CureGreater, 0x2D));
            bag.DropItem(MakePotionKeg(PotionEffect.HealGreater, 0x499));
            bag.DropItem(MakePotionKeg(PotionEffect.PoisonDeadly, 0x46));
            bag.DropItem(MakePotionKeg(PotionEffect.RefreshTotal, 0x21));
            bag.DropItem(MakePotionKeg(PotionEffect.ExplosionGreater, 0x74));
            bag.DropItem(new Bottle(1000));

            supplies.DropItem(bag);
        }

        private static Item MakePotionKeg(PotionEffect type, int hue)
        {
            PotionKeg keg = new PotionKeg();
            keg.Held = 100;
            keg.Type = type;
            keg.Hue = hue;
            return keg;
        }

        private static void AddTools(Container supplies)
        {
            Bag bag = new Bag();
            bag.Name = "Tool Bag";

            bag.DropItem(new TinkerTools(1000));
            bag.DropItem(new HousePlacementTool());
            bag.DropItem(new DovetailSaw(1000));
            bag.DropItem(new Scissors());
            bag.DropItem(new MortarPestle(1000));
            bag.DropItem(new ScribesPen(1000));
            bag.DropItem(new SmithHammer(1000));
            bag.DropItem(new TwoHandedAxe());
            bag.DropItem(new FletcherTools(1000));
            bag.DropItem(new SewingKit(1000));

            bag.DropItem(new RunicHammer(CraftResource.DullCopper, 1000));
            bag.DropItem(new RunicHammer(CraftResource.ShadowIron, 1000));
            bag.DropItem(new RunicHammer(CraftResource.Copper, 1000));
            bag.DropItem(new RunicHammer(CraftResource.Bronze, 1000));
            bag.DropItem(new RunicHammer(CraftResource.Gold, 1000));
            bag.DropItem(new RunicHammer(CraftResource.Agapite, 1000));
            bag.DropItem(new RunicHammer(CraftResource.Verite, 1000));
            bag.DropItem(new RunicHammer(CraftResource.Valorite, 1000));

            bag.DropItem(new RunicSewingKit(CraftResource.SpinedLeather, 1000));
            bag.DropItem(new RunicSewingKit(CraftResource.HornedLeather, 1000));
            bag.DropItem(new RunicSewingKit(CraftResource.BarbedLeather, 1000));

            supplies.DropItem(bag);
        }

        private static void AddAmmunition(Container supplies)
        {
            Bag bag = new Bag();
            bag.Name = "Bag Of Archery Ammo";
            bag.DropItem(new Arrow(5000));
            bag.DropItem(new Bolt(5000));
            supplies.DropItem(bag);
        }

        private static void AddTreasureMaps(Mobile from, Container supplies)
        {
            Bag bag = new Bag();
            bag.Name = "Bag Of Treasure Maps";

            for (int level = 1; level <= 6; ++level)
            {
                bag.DropItem(CreateTreasureMap(from, level));
                bag.DropItem(CreateTreasureMap(from, level));
            }

            bag.DropItem(new Lockpick(30));
            bag.DropItem(new Pickaxe());
            supplies.DropItem(bag);
        }

        private static TreasureMap CreateTreasureMap(Mobile from, int level)
        {
            Map map = from.Map;
            Point3D location = from.Location;

            if (map == null || map == Map.Internal)
            {
                map = Map.Sosaria;
                location = new Point3D(3579, 3423, 0);
            }

            return new TreasureMap(level, map, location, location.X, location.Y);
        }

        private static void AddRawMaterials(Container supplies)
        {
            Bag bag = new Bag();
            bag.Hue = 0x835;
            bag.Name = "Raw Materials Bag";

            bag.DropItem(new BarbedLeather(5000));
            bag.DropItem(new HornedLeather(5000));
            bag.DropItem(new SpinedLeather(5000));
            bag.DropItem(new Leather(5000));
            bag.DropItem(new Cloth(5000));
            bag.DropItem(new Board(5000));
            bag.DropItem(new BlankScroll(500));

            bag.DropItem(new DullCopperIngot(5000));
            bag.DropItem(new ShadowIronIngot(5000));
            bag.DropItem(new CopperIngot(5000));
            bag.DropItem(new BronzeIngot(5000));
            bag.DropItem(new GoldIngot(5000));
            bag.DropItem(new AgapiteIngot(5000));
            bag.DropItem(new VeriteIngot(5000));
            bag.DropItem(new ValoriteIngot(5000));
            bag.DropItem(new IronIngot(5000));

            bag.DropItem(new RedScales(5000));
            bag.DropItem(new YellowScales(5000));
            bag.DropItem(new BlackScales(5000));
            bag.DropItem(new GreenScales(5000));
            bag.DropItem(new WhiteScales(5000));
            bag.DropItem(new BlueScales(5000));

            supplies.DropItem(bag);
        }

        private static void AddSpellCastingSupplies(Container supplies)
        {
            Backpack bag = new Backpack();
            bag.Hue = 0x480;
            bag.Name = "Spell Casting Supplies";

            bag.DropItem(new Spellbook(UInt64.MaxValue));
            bag.DropItem(new NecromancerSpellbook((UInt64)0xFFFF));
            bag.DropItem(new BookOfChivalry((UInt64)0x3FF));
            bag.DropItem(new BookOfBushido());
            bag.DropItem(new BookOfNinjitsu());

            Runebook runebook = new Runebook(10);
            runebook.CurCharges = runebook.MaxCharges;
            bag.DropItem(runebook);

            BagOfAllReagents reagents = new BagOfAllReagents(150);
            reagents.Hue = 0x2D;
            bag.DropItem(reagents);

            BagOfNecroReagents necroReagents = new BagOfNecroReagents(150);
            necroReagents.Hue = 0x488;
            bag.DropItem(necroReagents);
            bag.DropItem(new BagOfAllReagents(500));

            for (int i = 0; i < 9; ++i)
                bag.DropItem(new RecallRune());

            bag.DropItem(new FireHorn());
            supplies.DropItem(bag);
        }

        private static void AddEthereals(Container supplies)
        {
            Backpack bag = new Backpack();
            bag.Hue = 0x490;
            bag.Name = "Bag Of Ethereals";

            bag.DropItem(new EtherealHorse());
            bag.DropItem(new EtherealOstard());
            bag.DropItem(new EtherealLlama());
            bag.DropItem(new EtherealKirin());
            bag.DropItem(new EtherealUnicorn());
            bag.DropItem(new EtherealRidgeback());
            bag.DropItem(new EtherealSwampDragon());
            bag.DropItem(new EtherealBeetle());

            supplies.DropItem(bag);
        }

        private static void AddArtifacts(Container supplies)
        {
            Backpack firstBag = new Backpack();
            firstBag.Hue = 0x48F;
            firstBag.Name = "Bag of Artifacts";
            firstBag.DropItem(new TitansHammer());
            firstBag.DropItem(new InquisitorsResolution());
            firstBag.DropItem(new BladeOfTheRighteous());
            firstBag.DropItem(new ZyronicClaw());
            supplies.DropItem(firstBag);

            Backpack secondBag = new Backpack();
            secondBag.Hue = 0x48F;
            secondBag.Name = "Bag of Artifacts";

            secondBag.DropItem(new GauntletsOfNobility());
            secondBag.DropItem(new MidnightBracers());
            secondBag.DropItem(new VoiceOfTheFallenKing());
            secondBag.DropItem(new OrnateCrownOfTheHarrower());
            secondBag.DropItem(new HelmOfInsight());
            secondBag.DropItem(new HolyKnightsBreastplate());
            secondBag.DropItem(new ArmorOfFortune());
            secondBag.DropItem(new TunicOfFire());
            secondBag.DropItem(new LeggingsOfBane());
            secondBag.DropItem(new ArcaneShield());
            secondBag.DropItem(new Aegis());
            secondBag.DropItem(new RingOfTheVile());
            secondBag.DropItem(new BraceletOfHealth());
            secondBag.DropItem(new RingOfTheElements());
            secondBag.DropItem(new OrnamentOfTheMagician());
            secondBag.DropItem(new DivineCountenance());
            secondBag.DropItem(new JackalsCollar());
            secondBag.DropItem(new HuntersHeaddress());
            secondBag.DropItem(new HatOfTheMagi());
            secondBag.DropItem(new ShadowDancerLeggings());
            secondBag.DropItem(new SpiritOfTheTotem());
            secondBag.DropItem(new BladeOfInsanity());
            secondBag.DropItem(new AxeOfTheHeavens());
            secondBag.DropItem(new TheBeserkersMaul());
            secondBag.DropItem(new Frostbringer());
            secondBag.DropItem(new BreathOfTheDead());
            secondBag.DropItem(new TheDragonSlayer());
            secondBag.DropItem(new BoneCrusher());
            secondBag.DropItem(new StaffOfTheMagi());
            secondBag.DropItem(new SerpentsFang());
            secondBag.DropItem(new LegacyOfTheDreadLord());
            secondBag.DropItem(new TheTaskmaster());
            secondBag.DropItem(new TheDryadBow());

            supplies.DropItem(secondBag);
        }

        private static void AddMinorArtifacts(Mobile from, Container supplies)
        {
            Backpack bag = new Backpack();
            bag.Hue = 0x48F;
            bag.Name = "Bag of Minor Artifacts";

            bag.DropItem(new LunaLance());
            bag.DropItem(new VioletCourage());
            bag.DropItem(new CavortingClub());
            bag.DropItem(new CaptainQuacklebushsCutlass());
            bag.DropItem(new NightsKiss());
            bag.DropItem(new ShipModelOfTheHMSCape());
            bag.DropItem(new AdmiralsHeartyRum());
            bag.DropItem(new CandelabraOfSouls());
            bag.DropItem(new IolosLute());
            bag.DropItem(new GwennosHarp());
            bag.DropItem(new ArcticDeathDealer());
            bag.DropItem(new EnchantedTitanLegBone());
            bag.DropItem(new NoxRangersHeavyCrossbow());
            bag.DropItem(new BlazeOfDeath());
            bag.DropItem(new DreadPirateHat());
            bag.DropItem(new BurglarsBandana());
            bag.DropItem(new GoldBricks());
            bag.DropItem(new AlchemistsBauble());
            bag.DropItem(new PhillipsWoodenSteed());
            bag.DropItem(new PolarBearMask());
            bag.DropItem(new BowOfTheJukaKing());
            bag.DropItem(new GlovesOfThePugilist());
            bag.DropItem(new OrcishVisage());
            bag.DropItem(new StaffOfPower());
            bag.DropItem(new ShieldOfInvulnerability());
            bag.DropItem(new HeartOfTheLion());
            bag.DropItem(new ColdBlood());
            bag.DropItem(new GhostShipAnchor());
            bag.DropItem(new SeahorseStatuette());
            bag.DropItem(new WrathOfTheDryad());
            bag.DropItem(new PixieSwatter());

            for (int i = 0; i < 10; ++i)
                bag.DropItem(CreateMessageInABottle(from));

            supplies.DropItem(bag);
        }

        private static MessageInABottle CreateMessageInABottle(Mobile from)
        {
            Map map = from.Map;
            Point3D location = from.Location;

            if (map == null || map == Map.Internal)
            {
                map = Map.Sosaria;
                location = new Point3D(3579, 3423, 0);
            }

            return new MessageInABottle(map, 4, location, location.X, location.Y);
        }

        private static void AddTokunoArtifacts(Container supplies)
        {
            Bag bag = new Bag();
            bag.Hue = 0x501;
            bag.Name = "Tokuno Minor Artifacts";

            bag.DropItem(new Exiler());
            bag.DropItem(new HanzosBow());
            bag.DropItem(new TheDestroyer());
            bag.DropItem(new DragonNunchaku());
            bag.DropItem(new PeasantsBokuto());
            bag.DropItem(new TomeOfEnlightenment());
            bag.DropItem(new ChestOfHeirlooms());
            bag.DropItem(new HonorableSwords());
            bag.DropItem(new AncientUrn());
            bag.DropItem(new FluteOfRenewal());
            bag.DropItem(new MagicPigment());
            bag.DropItem(new AncientSamuraiDo());
            bag.DropItem(new LegsOfStability());
            bag.DropItem(new GlovesOfTheSun());
            bag.DropItem(new AncientFarmersKasa());
            bag.DropItem(new ArmsOfTacticalExcellence());
            bag.DropItem(new DaimyosHelm());
            bag.DropItem(new BlackLotusHood());
            bag.DropItem(new DemonForks());
            bag.DropItem(new PilferedDancerFans());

            supplies.DropItem(bag);
        }

        private static void AddBows(Container supplies)
        {
            Bag bag = new Bag();
            bag.Name = "Bag of Bows";

            bag.DropItem(new Bow());
            bag.DropItem(new CompositeBow());
            bag.DropItem(new Crossbow());
            bag.DropItem(new HeavyCrossbow());
            bag.DropItem(new RepeatingCrossbow());
            bag.DropItem(new Yumi());

            for (int i = 0; i < bag.Items.Count; ++i)
            {
                BaseRanged bow = bag.Items[i] as BaseRanged;

                if (bow != null)
                {
                    bow.Attributes.WeaponSpeed = 35;
                    bow.Attributes.WeaponDamage = 35;
                }
            }

            supplies.DropItem(bag);
        }

        public class TCHelpGump : Gump
        {
            public TCHelpGump()
                : base(40, 40)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 430, 330, 5054);
                AddLabel(20, 15, 0x34, "Confictura Test Center");
                AddHtml(
                    20,
                    45,
                    390,
                    205,
                    "Say <B>set str 125</B>, <B>set dex 125</B>, or <B>set int 125</B> to adjust stats.<BR>"
                        + "Say <B>set &lt;skill&gt; &lt;value&gt;</B> to adjust a skill within your caps.<BR><BR>"
                        + "[TCSupplies - Claim the one-time bank supply package.<BR>"
                        + "[TCResurrect - Resurrect yourself without a fee or penalty.<BR>"
                        + "[TCCorpses - Bring your active corpses to separate nearby tiles.<BR><BR>"
                        + "The Help menu's stuck option has no daily use quota while Test Center is enabled; its normal safety checks and delay still apply.",
                    true,
                    true
                );

                AddButton(20, 265, 0xFB7, 0xFB9, 1, GumpButtonType.Reply, 0);
                AddLabel(55, 265, 0x34, "List skill names");
                AddButton(20, 295, 0xFB1, 0xFB3, 0, GumpButtonType.Reply, 0);
                AddLabel(55, 295, 0x34, "Close");
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (sender == null || sender.Mobile == null || info.ButtonID != 1)
                    return;

                string[] names = Enum.GetNames(typeof(SkillName));
                Array.Sort(names);

                StringBuilder text = new StringBuilder();

                for (int i = 0; i < names.Length; ++i)
                {
                    string name = names[i];

                    if (text.Length > 0 && (text.Length + name.Length + 1) > 220)
                    {
                        sender.Mobile.SendMessage(0x35, text.ToString());
                        text = new StringBuilder();
                    }

                    if (text.Length > 0)
                        text.Append(' ');

                    text.Append(name);
                }

                if (text.Length > 0)
                    sender.Mobile.SendMessage(0x35, text.ToString());
            }
        }
    }
}
