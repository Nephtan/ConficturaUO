using Server.Custom.Confictura.Stabling;
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Regions;
using Server.Spells.Jedi;
using Server.Spells.Mystic;
using Server.Spells.Syth;
using Server.Targeting;

namespace Server.Mobiles
{
    public class AnimalTrainer : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.DruidsGuild; }
        }

        [Constructable]
        public AnimalTrainer()
            : base("the animal trainer")
        {
            SetSkill(SkillName.Druidism, 64.0, 100.0);
            SetSkill(SkillName.Taming, 90.0, 100.0);
            SetSkill(SkillName.Veterinary, 65.0, 88.0);
        }

        public override void InitSBInfo()
        {
            if (
                Server.Misc.Worlds.GetMyWorld(this.Map, this.Location, this.X, this.Y)
                == "the Serpent Island"
            )
            {
                m_SBInfos.Add(new SBGargoyleAnimalTrainer());
            }
            else if (
                Server.Misc.Worlds.GetMyWorld(this.Map, this.Location, this.X, this.Y)
                == "the Land of Lodoria"
            )
            {
                m_SBInfos.Add(new SBElfAnimalTrainer());
            }
            else if (
                Server.Misc.Worlds.GetMyWorld(this.Map, this.Location, this.X, this.Y)
                == "the Isles of Dread"
            )
            {
                m_SBInfos.Add(new SBBarbarianAnimalTrainer());
            }
            else if (
                Server.Misc.Worlds.GetMyWorld(this.Map, this.Location, this.X, this.Y)
                == "the Savaged Empire"
            )
            {
                m_SBInfos.Add(new SBOrkAnimalTrainer());
            }
            else if (Worlds.IsCrypt(this.Location, this.Map))
            {
                m_SBInfos.Add(new SBDeadAnimalTrainer());
            }
            else
            {
                m_SBInfos.Add(new SBHumanAnimalTrainer());
            }
            m_SBInfos.Add(new SBAnimalTrainer());
            m_SBInfos.Add(new SBBuyArtifacts());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
            AddItem(Utility.RandomBool() ? (Item)new QuarterStaff() : (Item)new ShepherdsCrook());
        }

        private class StableEntry : ContextMenuEntry
        {
            private AnimalTrainer m_Trainer;
            private Mobile m_From;

            public StableEntry(AnimalTrainer trainer, Mobile from)
                : base(6126, 12)
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.BeginStable(m_From);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new ClaimingGumpEntry(from, this));
            list.Add(new SpeechGumpEntry(from, this));
            list.Add(new RidingGumpEntry(from, this));
        }

        public class SpeechGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public SpeechGumpEntry(Mobile from, Mobile giver)
                : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(SpeechGump)))
                    {
                        Server.Misc.IntelligentAction.SayHey(m_Giver);
                        mobile.SendGump(
                            new SpeechGump(
                                mobile,
                                "Animal Companions",
                                SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Pets")
                            )
                        );
                    }
                }
            }
        }

        public class RidingGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public RidingGumpEntry(Mobile from, Mobile giver)
                : base(6098, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(Server.Mobiles.Veterinarian.RidingGump)))
                    {
                        mobile.SendGump(new Server.Mobiles.Veterinarian.RidingGump(mobile));
                    }
                }
            }
        }

        public class ClaimingGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private AnimalTrainer m_Giver;

            public ClaimingGumpEntry(Mobile from, AnimalTrainer giver)
                : base(6165, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    m_Giver.BeginClaimList(m_Mobile);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        private class ClaimListGump : Gump
        {
            private AnimalTrainer m_Trainer;
            private Mobile m_From;
            private List<BaseCreature> m_List;

            public ClaimListGump(AnimalTrainer trainer, Mobile from, List<BaseCreature> list)
                : base(25, 25)
            {
                from.SendSound(0x0EB);
                string color = "#bfad7d";

                m_Trainer = trainer;
                m_From = from;
                m_List = list;

                from.CloseGump(typeof(ClaimListGump));

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);
                AddImage(0, 0, 9590, Server.Misc.PlayerSettings.GetGumpHue(from));
                AddHtml(
                    12,
                    12,
                    376,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">PETS IN THE STABLE</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(410, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

                int y = 15;

                for (int i = 0; i < list.Count; ++i)
                {
                    BaseCreature pet = list[i] as BaseCreature;

                    if (pet == null || pet.Deleted)
                        continue;

                    y = y + 30;

                    AddButton(13, y, 4005, 4005, (i + 1), GumpButtonType.Reply, 0);
                    AddHtml(
                        50,
                        y,
                        349,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">"
                            + pet.Name
                            + " | "
                            + CheckBondingStatus(pet)
                            + "</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                }
            }

            public string CheckBondingStatus(BaseCreature pet)
            {
                string BondInfo;
                if (pet.IsBonded == true)
                {
                    BondInfo = "Bonded";
                }
                else if (pet.BondingBegin == DateTime.MinValue)
                {
                    BondInfo = "not bonding";
                }
                else
                {
                    DateTime today = DateTime.Now;
                    DateTime willbebonded = pet.BondingBegin + pet.BondingDelay;
                    TimeSpan daystobond = willbebonded - today;
                    if (daystobond.TotalDays > 0)
                    {
                        BondInfo = string.Format(
                            "{0} days, {1} hours and {2} minutes until it bonds",
                            daystobond.Days,
                            daystobond.Hours,
                            daystobond.Minutes
                        );
                    }
                    else
                    {
                        BondInfo = "Ready to bond!";
                    }
                }
                return BondInfo;
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                m_From.SendSound(0x0F2);
                int index = info.ButtonID - 1;

                if (index >= 0 && index < m_List.Count)
                    m_Trainer.EndClaimList(m_From, m_List[index]);
            }
        }

        private class ClaimAllEntry : ContextMenuEntry
        {
            private AnimalTrainer m_Trainer;
            private Mobile m_From;

            public ClaimAllEntry(AnimalTrainer trainer, Mobile from)
                : base(6127, 12)
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.Claim(m_From);
            }
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
            {
                list.Add(new StableEntry(this, from));

                if (from.Stabled.Count > 0)
                    list.Add(new ClaimAllEntry(this, from));
            }

            base.AddCustomContextEntries(from, list);
        }

        public static int GetMaxStabled(Mobile from)
        {
            double taming = from.Skills[SkillName.Taming].Value;
            double anlore = from.Skills[SkillName.Druidism].Value;
            double vetern = from.Skills[SkillName.Veterinary].Value;
            double herd = from.Skills[SkillName.Herding].Value;
            double sklsum = taming + anlore + vetern + herd;

            int max;

            if (sklsum >= 400.0)
                max = 7;
            else if (sklsum >= 300.0)
                max = 6;
            else if (sklsum >= 240.0)
                max = 5;
            else if (sklsum >= 200.0)
                max = 4;
            else if (sklsum >= 160.0)
                max = 3;
            else
                max = 2;

            if (taming >= 100.0)
                max += (int)((taming - 90.0) / 10);

            if (anlore >= 100.0)
                max += (int)((anlore - 90.0) / 10);

            if (vetern >= 100.0)
                max += (int)((vetern - 90.0) / 10);

            if (herd >= 100.0)
                max += (int)((herd - 90.0) / 10);

            max = max + Server.Misc.MyServerSettings.ExtraStableSlots();

            return max;
        }

        private class StableTarget : Target
        {
            private AnimalTrainer m_Trainer;

            public StableTarget(AnimalTrainer trainer)
                : base(12, false, TargetFlags.None)
            {
                m_Trainer = trainer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                    m_Trainer.EndStable(from, (BaseCreature)targeted);
                else if (targeted == from)
                    m_Trainer.SayTo(from, 502672); // HA HA HA! Sorry, I am not an inn.
                else
                    m_Trainer.SayTo(from, 1048053); // You can't stable that!
            }
        }

        private void CloseClaimList(Mobile from)
        {
            from.CloseGump(typeof(ClaimListGump));
        }

        public static void DismountPlayer(Mobile m)
        {
            CleanClaimList(m);

            if (m.Mount is EtherealMount)
            {
                IMount mount = m.Mount;
                EtherealMount ethy = (EtherealMount)mount;
                Server.Mobiles.EtherealMount.Dismount(m);
                ethy.Owner = m;
                if (!Server.Misc.Worlds.InBuilding(m))
                {
                    m.SendMessage("Your mount has moved to your pack.");
                }
            }
            else if (m.Mount is BaseMount)
            {
                BaseCreature pet = (BaseCreature)(m.Mount);
                Server.Mobiles.BaseMount.Dismount(m);

                if (!StableService.TryStable(m, pet, false, true, null))
                    return;
                pet.Language = "mount";

                if (!Server.Misc.Worlds.InBuilding(m))
                {
                    m.SendMessage("Your mount is safely waiting for you elsewhere.");
                }
            }
        }

        public static bool IsBeingFast(Mobile from)
        {
            if (from is PlayerMobile)
            {
                if (from.Mounted)
                    return true;

                Item shoes = from.FindItemOnLayer(Layer.Shoes);
                if (
                    (
                        shoes is BootsofHermes
                        || shoes is Artifact_BootsofHermes
                        || shoes is Artifact_SprintersSandals
                        || (shoes is HikingBoots && from.RaceID > 0)
                    )
                    && shoes.Weight < 5.0
                )
                    return true;

                if (Spells.Syth.SythSpeed.UnderEffect((PlayerMobile)from))
                    return true;

                if (Spells.Jedi.Celerity.UnderEffect((PlayerMobile)from))
                    return true;

                if (Spells.Mystic.WindRunner.UnderEffect((PlayerMobile)from))
                    return true;

                if (Spells.Shinobi.CheetahPaws.UnderEffect((PlayerMobile)from))
                    return true;
            }
            return false;
        }

        public static void CleanClaimList(Mobile from)
        {
            List<BaseCreature> list = new List<BaseCreature>();

            for (int i = 0; i < from.Stabled.Count; ++i)
            {
                BaseCreature pet = from.Stabled[i] as BaseCreature;

                if (pet == null || pet.Deleted)
                {
                    if (pet != null)
                        pet.IsStabled = false;
                    from.Stabled.RemoveAt(i);
                    --i;
                    continue;
                }
            }
        }

        public static void GetLastMounted(Mobile from)
        {
            bool hasMount = false;

            List<BaseCreature> list = new List<BaseCreature>();

            BaseCreature bc = null;
            int stabled = 0;

            for (int i = 0; i < from.Stabled.Count; ++i)
            {
                BaseCreature pet = from.Stabled[i] as BaseCreature;

                if (pet == null || pet.Deleted)
                {
                    if (pet != null)
                        pet.IsStabled = false;
                    from.Stabled.RemoveAt(i);
                    --i;
                    continue;
                }
                else if (pet.Language == "mount")
                {
                    bc = pet;
                }
            }

            for (int i = 0; i < from.Stabled.Count; ++i)
            {
                BaseCreature pet = from.Stabled[i] as BaseCreature;

                ++stabled;

                if (CanGetLastMounted(from, pet) && pet is BaseMount && pet == bc)
                {
                    if (StableService.TryReturn(from, pet, false))
                    {
                        Server.Mobiles.BaseMount.Ride((BaseMount)pet, from);
                        hasMount = true;
                        --i;
                    }
                }
            }

            Server.Mobiles.AnimalTrainer.CleanClaimList(from);

            if (!hasMount)
            {
                ArrayList ethy = new ArrayList();
                foreach (Item item in World.Items.Values)
                {
                    if (item is EtherealMount)
                    {
                        if (((EtherealMount)item).Owner == from)
                        {
                            ((EtherealMount)item).Rider = from;
                            ((EtherealMount)item).Owner = from;
                        }
                    }
                }
            }
        }

        public static bool CanGetLastMounted(Mobile from, BaseCreature pet)
        {
            return ((from.Followers + pet.ControlSlots) <= from.FollowersMax);
        }

        public static bool IsNoMountRegion(Mobile m, Region reg)
        {
            string world = Worlds.GetMyWorld(m.Map, m.Location, m.X, m.Y);

            if (reg.IsPartOf("the Hidden Valley"))
                return true;

            if (reg.IsPartOf("the Great Pyramid"))
                return true;

            if (reg.IsPartOf("the Corrupt Pass"))
                return true;

            if (reg.IsPartOf("the Hedge Maze"))
                return true;

            if (reg.IsPartOf("the Valley of Dark Druids"))
                return true;

            if (reg.IsPartOf("the Altar of the Dragon King"))
                return true;

            if (reg.IsPartOf("the Altar of the Blood God"))
                return true;

            if (
                !reg.IsPartOf("the Port")
                && (
                    reg is WantedRegion
                    || reg is ProtectedRegion
                    || reg is CaveRegion
                    || reg is BardDungeonRegion
                    || reg is MoonCore
                    || reg is UmbraRegion
                    || reg is DungeonRegion
                    || reg is PublicRegion
                )
            )
                return true;

            return false;
        }

        public static bool AllowMagicSpeed(Mobile m, Region reg)
        {
            if (
                reg.IsPartOf("the Port")
                || reg is ProtectedRegion
                || reg is PublicRegion
                || reg is UmbraRegion
                || reg is CaveRegion
            )
                return true;

            return false;
        }

        public void BeginClaimList(Mobile from)
        {
            if (Deleted || !from.CheckAlive())
                return;

            List<BaseCreature> list = new List<BaseCreature>();

            for (int i = 0; i < from.Stabled.Count; ++i)
            {
                BaseCreature pet = from.Stabled[i] as BaseCreature;

                if (pet == null || pet.Deleted)
                {
                    if (pet != null)
                        pet.IsStabled = false;
                    from.Stabled.RemoveAt(i);
                    --i;
                    continue;
                }

                StableService.ShowAccount(from, pet);
                list.Add(pet);
            }

            if (list.Count > 0)
                from.SendGump(new ClaimListGump(this, from, list));
            else
                SayTo(from, 502671); // But I have no animals stabled with me at the moment!
        }

        public void EndClaimList(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || pet == null || pet.Deleted || !from.CheckAlive()
                || from.Map != Map || !from.InRange(this, 14) || !from.Stabled.Contains(pet))
                return;
            StableService.TryReturn(from, pet, true);
        }

        public void BeginStable(Mobile from)
        {
            if (Deleted || from == null || !from.CheckAlive())
                return;
            from.SendMessage(StableService.Terms);
            from.SendMessage("You are using {0} of {1} normal stable slots.", from.Stabled.Count, GetMaxStabled(from));
            from.Target = new StableTarget(this);
        }

        public void EndStable(Mobile from, BaseCreature pet)
        {
            if (Deleted || from == null || !from.CheckAlive() || from.Map != Map || !from.InRange(this, 14))
                return;
            StableService.StableManually(from, pet, GetMaxStabled(from), null);
        }

        public void Claim(Mobile from)
        {
            Claim(from, null);
        }

        public void Claim(Mobile from, string petName)
        {
            if (Deleted || from == null || from.Map != Map || !from.InRange(this, 14))
                return;
            StableService.Claim(from, petName);
        }

        public bool CanClaim(Mobile from, BaseCreature pet)
        {
            return ((from.Followers + pet.ControlSlots) <= from.FollowersMax);
        }



        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.HasKeyword(0x0008)) // *stable*
            {
                e.Handled = true;

                CloseClaimList(e.Mobile);
                BeginStable(e.Mobile);
            }
            else if (!e.Handled && e.HasKeyword(0x0009)) // *claim*
            {
                e.Handled = true;

                CloseClaimList(e.Mobile);

                int index = e.Speech.IndexOf(' ');

                if (index != -1)
                    Claim(e.Mobile, e.Speech.Substring(index).Trim());
                else
                    Claim(e.Mobile);
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public AnimalTrainer(Serial serial)
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
    }
}
