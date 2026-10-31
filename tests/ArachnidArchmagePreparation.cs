using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Spells.Research;
using Server.Custom.Confictura.Champions;
using Server.Custom.Confictura.CloneOfflinePlayerCharacters;

namespace Server.TestOnly
{
    // Preparation or bounded stationary combat on the actual unsaved copied character.
    public static class ArachnidArchmagePreparation
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static PlayerMobile Actor;
        private static Timer Poll;
        private static DateTime Deadline = DateTime.UtcNow.AddMinutes(5);
        private static string State = "preparing", Failure = String.Empty;
        private static string Mode = "preparation";
        private static ArachnidOutbreakController Controller;
        private static ArachnidOutbreakSite Site;
        private static ArachnidOutbreakPlacement Placement;
        private static DateTime Started, Ended, LastOwnedDamage;
        private static int EnemyDeaths, PlayerDeaths, Incoming, Restored, Outgoing, MovementRequests;
        private static bool BossKilled;
        private static Point3D BossRewardCenter;
        private static bool BossRewardCenterKnown;
        private static bool InfestationCase;
        private static string ActorName="Zanthus";
        private static int SelectedSerial;
        private static bool WeakFrontier, MixedCase;
        private static DateTime FrontierStarted;
        private static string FrontierSurvey=String.Empty;
        private static ArachnidInfestationPatch TestedFrontier;
        private static readonly List<CharacterClone> TestClones=new List<CharacterClone>();
        private static AnimalTrainer Trainer;
        private static readonly List<BaseCreature> SelectedPets=new List<BaseCreature>();
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly Dictionary<int, bool> OriginalGroundGold = new Dictionary<int, bool>();

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady)
                throw new InvalidOperationException("Archmage preparation requires the disabled-production disposable root.");
            AutoSave.SavesEnabled = false;
            InfestationCase = File.Exists(Path.Combine(Evidence,"infestation-case.txt"));
            string selected=Path.Combine(Evidence,"active-build.txt");
            if(File.Exists(selected))ActorName=File.ReadAllText(selected).Trim();
            string serialFile=Path.Combine(Evidence,"active-build-serial.private");
            if(File.Exists(serialFile))SelectedSerial=Int32.Parse(File.ReadAllText(serialFile).Trim(),CultureInfo.InvariantCulture);
            else if(ActorName!="Zanthus" && ActorName!="Sargar")throw new Exception("Unsupported selected active build.");
            WeakFrontier=File.Exists(Path.Combine(Evidence,"weak-frontier-case.txt"));
            MixedCase=File.Exists(Path.Combine(Evidence,"mixed-clones.private"));
            if(WeakFrontier && !InfestationCase)throw new Exception("Frontier requires persistent infestation.");
            string mode = Path.Combine(Evidence, "archmage-mode.txt");
            if (File.Exists(mode)) Mode = File.ReadAllText(mode).Trim();
            if (Mode != "preparation" && Mode != "stationary" && Mode != "route") throw new Exception("Unsupported archmage case.");
            EventSink.HealthChanged += OnHealth;
            EventSink.MobileDeath += OnDeath;
            EventSink.Movement += OnMovement;
            Timer.DelayCall(TimeSpan.FromSeconds(1), Prepare);
        }

        private static void Prepare()
        {
            try
            {
                foreach (Mobile mobile in World.Mobiles.Values)
                    if (mobile is PlayerMobile && (SelectedSerial!=0 ? mobile.Serial.Value==SelectedSerial : String.Equals(mobile.Name, ActorName, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (Actor != null) throw new Exception("Ambiguous copied character.");
                        Actor = (PlayerMobile)mobile;
                    }
                if (Actor == null || !Actor.Alive || Actor.Blessed || Actor.AccessLevel != AccessLevel.Player)
                    throw new Exception("Requires the selected actual living, unblessed Player; no substitute or privilege normalization.");
                Manifest("archmage-original.private.xml");
                IAccount login = Accounts.GetAccount(File.ReadAllText(Path.Combine(Evidence, "account-name.private")));
                if (login == null) throw new Exception("Private login account unavailable.");
                IAccount original = Actor.Account;
                if (original != null)
                    for (int i = 0; i < original.Length; ++i)
                        if (original[i] == Actor) { original[i] = null; break; }
                login[0] = Actor;
                // Account routing and initial staging affect this unsaved process only.
                Actor.Combatant = null; Actor.Warmode = false;
                Point3D staging = new Point3D(2490, 2060, Map.Sosaria.GetAverageZ(2490, 2060));
                if (Mode != "preparation")
                {
                    if (InfestationCase)
                    {
                        Site = new ArachnidOutbreakSite("infestation-acceptance",2514,2088,true);
                        Placement = ArachnidInfestationTerrain.LootPlacement(new Point3D(2514,2088,0));
                    }
                    else
                    {
                    List<WildernessEncounterReserve> reserves = new List<WildernessEncounterReserve>();
                    foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
                        reserves.Add(new WildernessEncounterReserve(reserve.Id, reserve.Center.X, reserve.Center.Y, reserve.Id == "central-woodland"));
                    typeof(WildernessEncounterReserves).GetField("m_Proposals", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, reserves.AsReadOnly());
                    Site = new ArachnidOutbreakSite("central-woodland", 2514, 2088, true);
                    typeof(ArachnidOutbreakSites).GetField("m_Candidates", BindingFlags.Static | BindingFlags.NonPublic)
                        .SetValue(null, Array.AsReadOnly(new ArachnidOutbreakSite[] { Site }));
                    string reason;
                    if (!Site.Validate(new SosariaOutbreakTerrain(), out Placement, out reason)) throw new Exception(reason);
                    }
                    staging = WeakFrontier ? new Point3D(2490,2010,0) : Placement.Brood(0); if(!WeakFrontier)staging.Y -= 2;
                    staging.Z = Map.Sosaria.GetAverageZ(staging.X, staging.Y);
                }
                if (!Map.Sosaria.CanSpawnMobile(staging)) throw new Exception("Blocked initial staging.");
                Actor.MoveToWorld(staging, Map.Sosaria);
                // Spell.Cast explicitly rejects blessed players. Keep actual
                // Player vulnerability during native maintenance.
                State = "ready";
                CheckStatusSerialization();
            }
            catch (Exception error) { Failure = error.ToString(); State = "failed"; }
            Poll = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(250), Tick);
        }

        private static bool Running { get { return (State.StartsWith("running-") || State=="growing-frontier"); } }
        private static bool Owns(Mobile mobile)
        {
            IArachnidOutbreakOwned owned = mobile as IArachnidOutbreakOwned;
            return owned != null && owned.Controller == Controller && Controller != null;
        }
        private static void OnHealth(HealthChangedEventArgs e)
        {
            if (!Running) return;
            int change = e.NewHits - e.OldHits;
            if (e.Mobile == Actor) { if (change < 0) Incoming -= change; else Restored += change;
                File.AppendAllText(Path.Combine(Evidence,"player-health.private.tsv"),DateTime.UtcNow.ToString("o")+"\t"+State+"\t"+e.OldHits+"\t"+e.NewHits+"\t"+e.Kind+"\t"+e.Mobile.Map+"\t"+(e.Source==null?"none":e.Source.GetType().Name+":"+e.Source.Serial.Value)+"\n"); }
            else if (change < 0 && Owns(e.Mobile) && e.Kind==HealthChangeKind.Damage)
            {
                if(e.Mobile is ArachnidOutbreakChampion && e.Mobile.Map==Map.Sosaria){BossRewardCenter=e.Mobile.Location;BossRewardCenterKnown=true;}
                Outgoing -= change; LastOwnedDamage = DateTime.UtcNow;
                if(WeakFrontier)File.AppendAllText(Path.Combine(Evidence,"enemy-health.private.tsv"),DateTime.UtcNow.ToString("o")+"\t"+e.Mobile.Serial.Value+"\t"+e.OldHits+"\t"+e.NewHits+"\t"+e.Kind+"\t"+e.Mobile.Map+"\t"+(e.Source==null?"none":e.Source.GetType().Name+":"+e.Source.Serial.Value)+"\n");
                if(e.Source==Actor)OutgoingDirect-=change;
                else if(e.Source!=null && e.Source.GetDamageMaster(e.Mobile)==Actor)OutgoingPets-=change;
                else if(e.Source is CharacterClone)OutgoingClones-=change;
                else OutgoingOther-=change;
            }
            if(change<0 && Owns(e.Mobile) && e.Kind!=HealthChangeKind.Damage)UnattributedOwnedLoss-=change;
            if(SelectedPets.Contains(e.Mobile as BaseCreature)){if(change<0)PetIncoming-=change;else PetRestored+=change;}
        }
        private static int OutgoingDirect,OutgoingPets,OutgoingOther,OutgoingClones,UnattributedOwnedLoss,PetIncoming,PetRestored;
        private static readonly string FixtureInstance=Guid.NewGuid().ToString("N");
        private static void OnDeath(Mobile mobile)
        {
            if (!Running) return;
            if (mobile == Actor) ++PlayerDeaths;
            if (Owns(mobile)) { ++EnemyDeaths; if (mobile is ArachnidOutbreakChampion) BossKilled = true; }
        }
        private static void OnMovement(MovementEventArgs e)
        { if (Running && e.Mobile == Actor) ++MovementRequests; }
        private static void Begin()
        {
            if (Mode == "preparation" || State != "prepared" || Actor.NetState == null || Actor.Blessed || Actor.AccessLevel != AccessLevel.Player)
                throw new Exception("Combat requires the frozen prepared original Player.");
            if (!File.Exists(Path.Combine(Evidence, "archmage-combat-freeze.private.json"))) throw new Exception("Missing combat freeze.");
            foreach (Item item in World.Items.Values)
                if (item is Gold && item.Parent == null && item.Map == Map.Sosaria)
                    OriginalGroundGold[item.Serial.Value] = true;
            Controller = new ArachnidOutbreakController();
            ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(Controller);
            state.SetEnabled(true);
            Started = LastOwnedDamage = DateTime.UtcNow; Deadline = Started.AddMinutes(Mode == "stationary" ? 6 : 46); State = "running-" + Mode;
            if (InfestationCase)
            {
                bool begun=(bool)typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(Controller,new object[]{Placement.Boss,DateTime.UtcNow});
                if(!begun)throw new Exception("Infestation start refused.");
            }
            else typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(Controller, new object[] { Site, Placement });
            typeof(ArachnidOutbreakController).GetMethod("StartTimer", Private).Invoke(Controller, null);
            if(WeakFrontier){State="growing-frontier";FrontierStarted=DateTime.UtcNow;Deadline=DateTime.UtcNow.AddMinutes(15);}
        }

        private static void Tick()
        {
            try
            {
                if (File.Exists(Path.Combine(Evidence, "archmage-client-error.txt"))) State = "client-failed";
                if (State == "ready" && File.Exists(Path.Combine(Evidence, "archmage-macro-prepared.txt")))
                { Manifest("archmage-prepared.private.xml"); State = "prepared"; }
                string command = Path.Combine(Evidence, "archmage-command.txt");
                if (File.Exists(command))
                {
                    string text = File.ReadAllText(command).Trim(); File.Delete(command);
                    if (text == "start-" + Mode && Mode != "preparation") Begin();
                    else if(text=="claim-pets" && ActorName=="Sargar" && State=="ready")ClaimSelectedPets();
                    else if (text == "stop") State = "stopped";
                    else throw new Exception("Unsupported archmage command.");
                }
                if(State=="growing-frontier")StageFrontier();
                if (Running)
                {
                    if (!Actor.Alive) State = "dead";
                    else if (Controller.Phase == ArachnidOutbreakPhase.Cooldown)
                        State = BossKilled && (Controller.Status == "Completed; cooldown active." || (InfestationCase && Controller.Status == "Queen defeated; infestation ended.")) ? "completed" : "aborted";
                    else if(WeakFrontier && State=="running-route" && (DateTime.UtcNow-Started).TotalSeconds>=180)State="frontier-window-ended";
                    else if (Mode == "stationary" && (DateTime.UtcNow - Started).TotalSeconds >= 300) State = "stationary-window-ended";
                    else if (Mode == "stationary" && Controller.ClearedBroods != 0 && (DateTime.UtcNow - LastOwnedDamage).TotalSeconds >= 120) State = "stationary-idle";
                    else if ((DateTime.UtcNow - Started).TotalSeconds >= 2700) State = "timeout";
                }
                if (Started != DateTime.MinValue && !Running && Ended == DateTime.MinValue)
                { Ended = DateTime.UtcNow; Manifest("archmage-final.private.xml"); }
                if (DateTime.UtcNow > Deadline && State == "ready")
                { Failure = "Native preparation deadline."; State = "failed"; }
                Status();
                if (File.Exists(Path.Combine(Evidence, "native-stop.txt")) || DateTime.UtcNow > Deadline)
                {
                    if (Controller != null) Controller.Delete();
                    foreach(CharacterClone clone in TestClones)if(!clone.Deleted)clone.Delete();
                    if(Trainer!=null && !Trainer.Deleted)Trainer.Delete();
                    foreach(BaseCreature pet in SelectedPets)if(pet!=null && !pet.Deleted){pet.Combatant=null;pet.ControlOrder=OrderType.Stop;}
                    Poll.Stop(); AutoSave.SavesEnabled = false;
                    File.WriteAllText(Path.Combine(Evidence, "native-stopped.txt"), "Archmage fixture stopped without saving; only its owned encounter cleaned.");
                }
            }
            catch (Exception error)
            {
                Failure=error.ToString();State="failed";
                File.WriteAllText(Path.Combine(Evidence,"fixture-failure.txt"),Failure);
                try{Status();}catch(Exception statusError){File.AppendAllText(Path.Combine(Evidence,"fixture-failure.txt"),"\nStatus: "+statusError);}
                try
                {
                    if(Controller!=null && !Controller.Deleted)Controller.Delete();
                    foreach(CharacterClone clone in TestClones)if(!clone.Deleted)clone.Delete();
                    if(Poll!=null)Poll.Stop();AutoSave.SavesEnabled=false;
                    File.WriteAllText(Path.Combine(Evidence,"native-stopped.txt"),"Failed test-only fixture cleaned owned encounter without saving; failure retained.");
                }
                catch(Exception stopError){File.AppendAllText(Path.Combine(Evidence,"fixture-failure.txt"),"\nCleanup: "+stopError);}
            }
        }

        private static void StageFrontier()
        {
            ArachnidInfestationState colony=(ArachnidInfestationState)typeof(ArachnidOutbreakController).GetField("m_Infestation",Private).GetValue(Controller);
            if(colony==null)throw new Exception("Missing real colony.");
            int young=0,furthest=0;
            foreach(ArachnidInfestationPatch patch in colony.Patches)
            {
                if(patch.Id!=0 && patch.Established && !patch.Retired && colony.Tier(patch)==0){++young;furthest=Math.Max(furthest,Distance(patch.Center,colony.Origin));}
                if(patch.Id==0 || !patch.Established || patch.Retired || colony.Tier(patch)!=0 || Distance(patch.Center,colony.Origin)<36)continue;
                int dx=Math.Sign(patch.Center.X-colony.Origin.X),dy=Math.Sign(patch.Center.Y-colony.Origin.Y);
                for(int offset=12;offset<=15;++offset)
                {
                    Point3D point;
                    if(!ArachnidInfestationTerrain.TryPoint(patch.Center.X+dx*offset,patch.Center.Y+dy*offset,out point) || !Map.Sosaria.CanSpawnMobile(point))continue;
                    bool highThreat=false;int nearby=0;
                    foreach(Mobile enemy in (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(Controller))
                    {
                        if(enemy==null || enemy.Deleted || !enemy.Alive)continue;
                        int distance=Distance(enemy.Location,point);if(distance<=24)++nearby;
                        ArachnidInfestationCreature ranked=enemy as ArachnidInfestationCreature;
                        if(distance<=30 && (enemy is ArachnidOutbreakChampion || ranked!=null && ranked.Tier>=3))highThreat=true;
                    }
                    if(highThreat)continue;
                    FrontierSurvey="Natural tier0 patch="+patch.Id+" rootDistance="+Distance(point,colony.Origin)+" offset="+offset+" ownedWithin24="+nearby+"; no queen/tier3+ within30 at staging; no threat removals";
                    TestedFrontier=patch;Actor.MoveToWorld(point,Map.Sosaria);
                    if(MixedCase)
                    {
                        string[] selected=File.ReadAllLines(Path.Combine(Evidence,"mixed-clones.private"));
                        if(selected.Length<1 || selected.Length>2)throw new Exception("Only one or two selected proxy allies allowed.");
                        foreach(string line in selected)
                        {
                            PlayerMobile original=World.FindMobile((Serial)Int32.Parse(line.Trim(),CultureInfo.InvariantCulture)) as PlayerMobile;
                            if(original==null || original==Actor || !original.Alive || original.Blessed || original.AccessLevel!=AccessLevel.Player)throw new Exception("Invalid selected offline clone source.");
                            CharacterClone clone=new FrontierProxyAlly(original);CloneThings.CloneMobileProperties(original,clone);
                            CloneThings.CloneMobileItems(original,clone);CloneThings.CloneMobileBackpack(original,clone);CloneThings.CloneMobileMount(original,clone);
                            // Test-only allies filter hostility to this encounter; ordinary OmniAI/capabilities/resources remain. No hired status or follower-cap override.
                            clone.Team=190;clone.FightMode=FightMode.Closest;clone.Home=point;clone.RangeHome=24;
                            clone.MoveToWorld(new Point3D(point.X+TestClones.Count,point.Y,point.Z),Map.Sosaria);TestClones.Add(clone);
                        }
                    }
                    Started=LastOwnedDamage=DateTime.UtcNow;State="running-route";Manifest("frontier-start.private.xml");return;
                }
            }
            FrontierSurvey="Waiting natural growth; established tier0="+young+" furthest young center from root="+furthest+"; minimum center36, no queen/tier3+within30 of scout";
            if((DateTime.UtcNow-FrontierStarted).TotalSeconds>600)throw new Exception("No naturally established distant tier-zero frontier in bounded growth window; no substituted root test.");
        }

        private static int Distance(Point3D a,Point3D b){return Math.Max(Math.Abs(a.X-b.X),Math.Abs(a.Y-b.Y));}
        public static bool IsProxyTarget(Mobile mobile){return mobile is IArachnidOutbreakOwned && ((IArachnidOutbreakOwned)mobile).Controller==Controller;}
        private static void Attr(XmlWriter writer, string name, object value)
        { writer.WriteAttributeString(name, Convert.ToString(value, CultureInfo.InvariantCulture)); }

        private static void ClaimSelectedPets()
        {
            foreach(Mobile mobile in new List<Mobile>(Actor.AllFollowers))
            {
                BaseCreature pet=mobile as BaseCreature;
                if(pet is CragCat && !pet.Deleted && !pet.IsStabled && pet.ControlMaster==Actor && !SelectedPets.Contains(pet))SelectedPets.Add(pet);
            }
            if(Trainer==null)
            {
                Trainer=new AnimalTrainer();Trainer.MoveToWorld(Actor.Location,Actor.Map);
            }
            foreach(Mobile mobile in new List<Mobile>(Actor.Stabled))
            {
                BaseCreature pet=mobile as BaseCreature;
                if(!(pet is CragCat) || SelectedPets.Count>=Actor.FollowersMax || !Trainer.CanClaim(Actor,pet))continue;
                Trainer.EndClaimList(Actor,pet);
                if(!pet.IsStabled && pet.ControlMaster==Actor)SelectedPets.Add(pet);
            }
            Manifest("selected-tamer-prepared.private.xml");
            File.WriteAllText(Path.Combine(Evidence,"selected-pets-ready.txt"),SelectedPets.Count.ToString());
        }

        private static void Manifest(string name) { WriteCharacterManifest(name,Actor); }

        public static void WriteCharacterManifest(string name, PlayerMobile subject)
        {
            using (XmlWriter writer = XmlWriter.Create(Path.Combine(Evidence, name), new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("character"); Attr(writer, "serial", subject.Serial.Value);
                Attr(writer, "access", subject.AccessLevel); Attr(writer, "blessed", subject.Blessed);
                Attr(writer, "rawStr", subject.RawStr); Attr(writer, "rawDex", subject.RawDex); Attr(writer, "rawInt", subject.RawInt);
                Attr(writer, "str", subject.Str); Attr(writer, "dex", subject.Dex); Attr(writer, "int", subject.Int);
                Attr(writer, "hits", subject.Hits); Attr(writer, "hitsMax", subject.HitsMax);
                Attr(writer, "stam", subject.Stam); Attr(writer, "stamMax", subject.StamMax);
                Attr(writer, "mana", subject.Mana); Attr(writer, "manaMax", subject.ManaMax);
                Attr(writer,"karma",subject.Karma);Attr(writer,"tithing",subject.TithingPoints);Attr(writer,"statCap",subject.StatCap);
                Attr(writer, "body", subject.BodyValue); Attr(writer, "followers", subject.Followers);
                Attr(writer,"followersMax",subject.FollowersMax);
                BaseCreature mount=subject.Mount as BaseCreature;
                if(mount!=null){Attr(writer,"mountSerial",mount.Serial.Value);Attr(writer,"mountType",mount.GetType().FullName);Attr(writer,"mountHitsMax",mount.HitsMax);Attr(writer,"mountSlots",mount.ControlSlots);}
                Attr(writer,"backpack",subject.Backpack==null?0:subject.Backpack.Serial.Value);
                Attr(writer, "ancientDamage", ResearchSpell.DamagingSkill(subject)); Attr(writer, "ancientCasting", ResearchSpell.CastingSkill(subject));
                Attr(writer, "x", subject.X); Attr(writer, "y", subject.Y); Attr(writer, "z", subject.Z);
                for (int i = 0; i < subject.Skills.Length; ++i)
                {
                    Skill skill = subject.Skills[i];
                    if (skill.Base == 0 && skill.Value == 0) continue;
                    writer.WriteStartElement("skill"); Attr(writer, "name", skill.SkillName);
                    Attr(writer, "base", skill.Base); Attr(writer, "effective", skill.Value); Attr(writer, "cap", skill.Cap); writer.WriteEndElement();
                }
                foreach (Item item in subject.Items)
                {
                    if (item.Layer == Layer.Backpack || item.Layer == Layer.Bank) continue;
                    writer.WriteStartElement("equipment"); Attr(writer, "serial", item.Serial.Value);
                    Attr(writer, "type", item.GetType().FullName); Attr(writer, "layer", item.Layer);
                    Spellbook book=item as Spellbook;if(book!=null)Attr(writer,"spellContent",book.Content);
                    BaseWeapon weapon=item as BaseWeapon;if(weapon!=null){Attr(writer,"weaponSkill",weapon.Skill);Attr(writer,"primary",weapon.PrimaryAbility==null?"none":weapon.PrimaryAbility.GetType().Name);Attr(writer,"secondary",weapon.SecondaryAbility==null?"none":weapon.SecondaryAbility.GetType().Name);if(weapon.PrimaryAbility!=null){Attr(writer,"primaryRequired",weapon.PrimaryAbility.GetRequiredSkill(subject));Attr(writer,"primaryMana",weapon.PrimaryAbility.CalculateMana(subject));Attr(writer,"primarySkillEligible",weapon.PrimaryAbility.CheckSkills(subject));}}
                    writer.WriteEndElement();
                }
                if (subject.Backpack != null) Inventory(writer, subject.Backpack);
                foreach(Mobile mobile in subject.Stabled)
                {
                    BaseCreature pet=mobile as BaseCreature;if(pet==null)continue;
                    writer.WriteStartElement("savedCompanion");Attr(writer,"serial",pet.Serial.Value);Attr(writer,"type",pet.GetType().FullName);
                    Attr(writer,"autoStabled",subject.AutoStabled.Contains(pet));Attr(writer,"hitsMax",pet.HitsMax);Attr(writer,"slots",pet.ControlSlots);writer.WriteEndElement();
                }
                foreach(BaseCreature pet in SelectedPets)
                {
                    writer.WriteStartElement("pet");Attr(writer,"serial",pet.Serial.Value);Attr(writer,"type",pet.GetType().FullName);Attr(writer,"slots",pet.ControlSlots);
                    Attr(writer,"loyalty",pet.Loyalty);Attr(writer,"controlChance",pet.GetControlChance(Actor));Attr(writer,"hits",pet.Hits);Attr(writer,"hitsMax",pet.HitsMax);writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        private static void Inventory(XmlWriter writer, Container container)
        {
            foreach (Item item in container.Items)
            {
                writer.WriteStartElement("item"); Attr(writer, "serial", item.Serial.Value); Attr(writer, "type", item.GetType().FullName);
                Attr(writer, "amount", item.Amount); Spellbook book=item as Spellbook;if(book!=null)Attr(writer,"spellContent",book.Content); Attr(writer, "graphic", item.ItemID); Attr(writer, "hue", item.Hue);
                Container child = item as Container;
                if (child != null) Inventory(writer, child);
                if (item is ResearchBag || item is AncientSpellbook)
                    foreach (FieldInfo field in item.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                        if (field.FieldType == typeof(int))
                        { writer.WriteStartElement("research"); Attr(writer, "field", field.Name); Attr(writer, "value", field.GetValue(item)); writer.WriteEndElement(); }
                writer.WriteEndElement();
            }
        }

        private static void CheckStatusSerialization()
        {
            // Exercise the actual writer with a child entry before clients can connect. No health or other capability is changed.
            DamageEntry probe=new DamageEntry(Actor);
            ArachnidOutbreakController previous=Controller;DateTime previousEnd=Ended;
            ArachnidOutbreakController test=new ArachnidOutbreakController();
            Actor.DamageEntries.Add(probe);Controller=test;Ended=DateTime.UtcNow;
            try
            {
                Status("serializer-selfcheck.private.xml");
                XmlDocument check=new XmlDocument();check.XmlResolver=null;check.Load(Path.Combine(Evidence,"serializer-selfcheck.private.xml"));
                if(check.DocumentElement.SelectSingleNode("damageEntry")==null || !check.DocumentElement.HasAttribute("pendingGlands"))throw new Exception("Damage-entry/reward status serialization probe absent.");
                File.WriteAllText(Path.Combine(Evidence,"serializer-selfcheck-passed.txt"),"Actual writer parsed with damage-entry child and terminal reward attributes; transient zero-damage metadata and disabled empty controller removed before clients; no health/skill/gear/supply changes.");
            }
            finally{Actor.DamageEntries.Remove(probe);Controller=previous;Ended=previousEnd;test.Delete();}
        }

        private static void Status(){Status("archmage-state.xml");}
        private static void Status(string name)
        {
            string path = Path.Combine(Evidence, name);
            using (XmlWriter writer = XmlWriter.Create(path + ".tmp", new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("archmage"); Attr(writer, "state", State); Attr(writer, "failure", Failure);
                Attr(writer,"fixtureInstance",FixtureInstance);Attr(writer,"serverPid",System.Diagnostics.Process.GetCurrentProcess().Id);
                Attr(writer,"serverExe",Core.Assembly.Location);Attr(writer,"heartbeatUtc",DateTime.UtcNow.ToString("o"));
                Attr(writer, "online", Actor != null && Actor.NetState != null); Attr(writer, "encounter", Started != DateTime.MinValue);
                Attr(writer,"frontierSurvey",FrontierSurvey);Attr(writer,"frontierCase",WeakFrontier);Attr(writer,"clones",TestClones.Count);
                if(TestedFrontier!=null){Attr(writer,"frontierId",TestedFrontier.Id);Attr(writer,"frontierX",TestedFrontier.Center.X);Attr(writer,"frontierY",TestedFrontier.Center.Y);Attr(writer,"frontierBirth",TestedFrontier.Birth);}
                Attr(writer, "mode", Mode); Attr(writer, "blessed", Actor != null && Actor.Blessed);
                Attr(writer, "seconds", Started == DateTime.MinValue ? 0 : ((Ended == DateTime.MinValue ? DateTime.UtcNow : Ended) - Started).TotalSeconds);
                Attr(writer, "ownedDeaths", EnemyDeaths); Attr(writer, "playerDeaths", PlayerDeaths); Attr(writer, "bossKilled", BossKilled);
                Attr(writer, "incoming", Incoming); Attr(writer, "outgoing", Outgoing); Attr(writer, "restored", Restored); Attr(writer, "movementRequests", MovementRequests);
                Attr(writer,"outgoingDirect",OutgoingDirect);Attr(writer,"outgoingPets",OutgoingPets);Attr(writer,"outgoingOther",OutgoingOther);Attr(writer,"outgoingClones",OutgoingClones);Attr(writer,"unattributedOwnedLoss",UnattributedOwnedLoss);
                Attr(writer,"petIncoming",PetIncoming);Attr(writer,"petRestored",PetRestored);
                Attr(writer, "clearedBroods", Controller == null ? 0 : Controller.ClearedBroods);
                Attr(writer, "phase", Controller == null ? String.Empty : Controller.Phase.ToString());
                if(Actor!=null)
                {
                    Attr(writer,"actor",Actor.Serial.Value);Attr(writer,"hits",Actor.Hits);Attr(writer,"hitsMax",Actor.HitsMax);
                    Attr(writer,"mana",Actor.Mana);Attr(writer,"stam",Actor.Stam);Attr(writer,"poisoned",Actor.Poisoned);
                    Attr(writer,"poisonLevel",Actor.Poison==null?-1:Actor.Poison.Level);Attr(writer,"frozen",Actor.Frozen);Attr(writer,"paralyzed",Actor.Paralyzed);Attr(writer,"spell",Actor.Spell==null?"none":Actor.Spell.GetType().Name);
                    Attr(writer,"currentAbility",WeaponAbility.GetCurrentAbility(Actor)==null?"none":WeaponAbility.GetCurrentAbility(Actor).GetType().Name);
                    Attr(writer,"x",Actor.X);Attr(writer,"y",Actor.Y);Attr(writer,"z",Actor.Z);
                }
                if (Ended != DateTime.MinValue && Controller != null)
                {
                    int glands = 0, skulls = 0, artifacts = 0, gold = 0;
                    foreach (Item item in World.Items.Values)
                    {
                        ArachnidOutbreakClaim claim = item as ArachnidOutbreakClaim;
                        if (claim != null && claim.Owner == Actor && claim.AttemptId == Controller.AttemptId)
                        {
                            foreach (Item reward in claim.Items)
                                if (reward is BroodVenomGland) glands += reward.Amount;
                            if (claim.Kind == ArachnidOutbreakClaimKind.Skull) ++skulls;
                            if (claim.Kind == ArachnidOutbreakClaimKind.Artifact) ++artifacts;
                        }
                        if (item is Gold && item.Parent == null && item.Map == Map.Sosaria && BossRewardCenterKnown
                            && Utility.InRange(item.Location,BossRewardCenter,12)
                            && !OriginalGroundGold.ContainsKey(item.Serial.Value)) gold += item.Amount;
                    }
                    Attr(writer, "pendingGlands", glands); Attr(writer, "pendingSkulls", skulls); Attr(writer, "pendingArtifacts", artifacts);
                    Attr(writer, "newGroundGold", gold);
                    Attr(writer,"rewardCenterKnown",BossRewardCenterKnown);Attr(writer,"rewardX",BossRewardCenter.X);Attr(writer,"rewardY",BossRewardCenter.Y);
                }
                if(Controller!=null)
                {
                    ArachnidInfestationState colony=(ArachnidInfestationState)typeof(ArachnidOutbreakController).GetField("m_Infestation",Private).GetValue(Controller);
                    if(colony!=null)foreach(ArachnidInfestationPatch patch in colony.Patches)
                    {writer.WriteStartElement("patch");Attr(writer,"id",patch.Id);Attr(writer,"x",patch.Center.X);Attr(writer,"y",patch.Center.Y);Attr(writer,"tier",colony.Tier(patch));Attr(writer,"established",patch.Established);Attr(writer,"retired",patch.Retired);writer.WriteEndElement();}
                }
                if(Actor!=null)
                    foreach(DamageEntry entry in Actor.DamageEntries){writer.WriteStartElement("damageEntry");Attr(writer,"source",entry.Damager==null?0:entry.Damager.Serial.Value);Attr(writer,"type",entry.Damager==null?"none":entry.Damager.GetType().Name);Attr(writer,"damage",entry.DamageGiven);writer.WriteEndElement();}
                if (Placement != null)
                    for (int stage = 0; stage < 4; ++stage)
                    {
                        Point3D point = stage == 3 ? Placement.Boss : Placement.Brood(stage);
                        writer.WriteStartElement("anchor"); Attr(writer, "stage", stage);
                        Attr(writer, "x", point.X); Attr(writer, "y", point.Y); Attr(writer, "z", point.Z); writer.WriteEndElement();
                    }
                if (Controller != null)
                    foreach (Mobile mobile in (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(Controller))
                        if (mobile != null && !mobile.Deleted && mobile.Alive)
                        {
                            writer.WriteStartElement("enemy"); Attr(writer, "serial", mobile.Serial.Value); Attr(writer, "type", mobile.GetType().Name);
                            Attr(writer,"x",mobile.X);Attr(writer,"y",mobile.Y);Attr(writer,"z",mobile.Z);Attr(writer,"hits",mobile.Hits);Attr(writer,"hitsMax",mobile.HitsMax);Attr(writer,"distance",Distance(mobile.Location,Actor.Location));Attr(writer,"combatant",mobile.Combatant==null?0:mobile.Combatant.Serial.Value);
                            BaseCreature npc=mobile as BaseCreature;if(npc!=null){Attr(writer,"damageMin",npc.DamageMin);Attr(writer,"damageMax",npc.DamageMax);Attr(writer,"rangePerception",npc.RangePerception);Attr(writer,"aiRunning",npc.AIObject!=null && npc.AIObject.m_Timer.Running);}
                            ArachnidInfestationCreature ranked=mobile as ArachnidInfestationCreature;if(ranked!=null){Attr(writer,"tier",ranked.Tier);Attr(writer,"guardian",ranked.Guardian);Attr(writer,"founder",ranked.Founder);Attr(writer,"patch",ranked.PatchId);}
                            writer.WriteEndElement();
                        }
                foreach(CharacterClone clone in TestClones)
                {
                    writer.WriteStartElement("clone");Attr(writer,"serial",clone.Serial.Value);Attr(writer,"original",clone.Original.Serial.Value);Attr(writer,"alive",clone.Alive && !clone.Deleted);Attr(writer,"hits",clone.Hits);Attr(writer,"hitsMax",clone.HitsMax);Attr(writer,"combatant",clone.Combatant==null?0:clone.Combatant.Serial.Value);Attr(writer,"fightMode",clone.FightMode);Attr(writer,"team",clone.Team);writer.WriteEndElement();
                }
                foreach(BaseCreature pet in SelectedPets)
                    if(pet!=null && !pet.Deleted)
                    {
                        writer.WriteStartElement("pet");Attr(writer,"serial",pet.Serial.Value);Attr(writer,"alive",pet.Alive);Attr(writer,"deadBonded",pet.IsDeadBondedPet);
                        Attr(writer,"hits",pet.Hits);Attr(writer,"hitsMax",pet.HitsMax);Attr(writer,"poisoned",pet.Poisoned);
                        Attr(writer,"x",pet.X);Attr(writer,"y",pet.Y);Attr(writer,"z",pet.Z);Attr(writer,"order",pet.ControlOrder);
                        writer.WriteEndElement();
                    }
                writer.WriteEndElement();
            }
            if (File.Exists(path)) File.Replace(path + ".tmp", path, null); else File.Move(path + ".tmp", path);
        }
    }
    public class FrontierProxyAlly : CharacterClone
    {
        public FrontierProxyAlly(Mobile source):base(source){}
        public FrontierProxyAlly(Serial serial):base(serial){}
        public override bool IsEnemy(Mobile mobile){return ArachnidArchmagePreparation.IsProxyTarget(mobile) && base.IsEnemy(mobile);}
        public override void Serialize(GenericWriter writer){base.Serialize(writer);writer.Write(0);}
        public override void Deserialize(GenericReader reader){base.Deserialize(reader);reader.ReadInt();}
    }

}
