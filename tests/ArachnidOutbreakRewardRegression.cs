using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

internal static class ArachnidOutbreakRewardRegression
{
    private static int s_Checks;
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        ++s_Checks;
    }
    private static Mobile Player()
    {
        Mobile player = new Mobile();
        player.Player = true;
        player.RawStr = 100;
        player.Body = 0x190;
        player.Hits = player.HitsMax;
        player.AddItem(new Backpack());
        return player;
    }
    private static ArachnidOutbreakLedger Ledger(ArachnidOutbreakController controller)
    {
        return (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger", Private).GetValue(controller);
    }
    private static ArachnidOutbreakController Begin(bool singleLootTile = false)
    {
        ArachnidOutbreakController controller = new ArachnidOutbreakController();
        ArachnidOutbreakState state = (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(controller);
        state.SetEnabled(true);
        Point3D boss = new Point3D(2514,2088,0);
        List<Point3D> tiles = new List<Point3D>();
        tiles.Add(boss);
        if (!singleLootTile)
        {
            SosariaOutbreakTerrain terrain = new SosariaOutbreakTerrain();
            for (int x = boss.X - 56; x <= boss.X + 56; ++x)
                for (int y = boss.Y - 56; y <= boss.Y + 56; ++y)
                {
                    Point3D point;
                    if (terrain.TryGetPoint(x,y,out point)) tiles.Add(point);
                }
        }
        ArachnidOutbreakPlacement placement = new ArachnidOutbreakPlacement(boss,
            new Point3D[] { new Point3D(2486,2068,0), new Point3D(2542,2068,0), new Point3D(2514,2118,0) }, tiles.ToArray());
        typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(controller,
            new object[] { new ArachnidOutbreakSite("rewards-fixture",2514,2088,false), placement });
        return controller;
    }
    private static List<ArachnidOutbreakClaim> Claims(Mobile player)
    {
        List<ArachnidOutbreakClaim> result = new List<ArachnidOutbreakClaim>();
        foreach (Item item in World.Items.Values)
        {
            ArachnidOutbreakClaim claim = item as ArachnidOutbreakClaim;
            if (claim != null && !claim.Deleted && claim.Owner == player) result.Add(claim);
        }
        return result;
    }
    private static void CrowdQualification()
    {
        List<Mobile> players = new List<Mobile>();
        try
        {
            for (int i=0;i<100;++i) players.Add(Player());
            ArachnidOutbreakLedger crowd = new ArachnidOutbreakLedger("crowd",10000,10000);
            for (int i=0;i<20;++i) crowd.CreditDamage(0,players[i],500);
            Check(crowd.ActiveContributors(0)==20 && crowd.QualificationThreshold(0)==2.5
                && crowd.Qualified(0).Count==20,"twenty equal damage contributors qualify instead of all missing ten points");
            Check(crowd.Qualified(1).Count==0 && crowd.ActiveContributors(-1)==0,"crowd effort stays stage-local and bounds guarded");
            players[0].Body=0x192;
            Check(crowd.Qualified(0).Contains(players[0]) && players[0].NetState==null,"earned dead/offline effort counts toward the active stage group");
            Check(crowd.Seal(0) && !crowd.CreditDamage(0,players[20],500),"sealed crowd stage cannot be diluted by later arrivals");
            MemoryStream bytes=new MemoryStream();BinaryFileWriter writer=new BinaryFileWriter(bytes,true);
            crowd.Serialize(writer);writer.Write(78413);writer.Close();
            BinaryFileReader reader=new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
            ArachnidOutbreakLedger restored=new ArachnidOutbreakLedger();restored.Deserialize(reader);
            Check(reader.ReadInt()==78413 && restored.IsSealed(0) && restored.Qualified(0).Count==20
                && restored.QualificationThreshold(0)==2.5,"unchanged version-zero ledger format preserves sealed crowd qualification and read boundary");reader.Close();

            ArachnidOutbreakLedger floor=new ArachnidOutbreakLedger("floor",10000,10000);
            for(int i=0;i<50;++i) floor.CreditDamage(0,players[i],200);
            Check(floor.QualificationThreshold(0)==2.0 && floor.Qualified(0).Count==50,"large group reaches the meaningful-effort floor");
            ArachnidOutbreakLedger tiny=new ArachnidOutbreakLedger("tiny",10000,10000);
            for(int i=0;i<100;++i) tiny.CreditDamage(0,players[i],100);
            Check(tiny.ActiveContributors(0)==0 && tiny.Qualified(0).Count==0,"sub-floor tags cannot manufacture an active group or qualify");
            ArachnidOutbreakLedger tags=new ArachnidOutbreakLedger("tags",10000,10000);
            for(int i=0;i<5;++i) tags.CreditDamage(0,players[i],500);
            for(int i=5;i<100;++i) tags.CreditDamage(0,players[i],100);
            Check(tags.ActiveContributors(0)==5 && tags.QualificationThreshold(0)==10.0 && tags.Qualified(0).Count==0,
                "nearby low-effort contributors cannot lower the small-group target");
            ArachnidOutbreakLedger support=new ArachnidOutbreakLedger("support",10000,10000);
            for(int i=0;i<25;++i) support.CreditValidatedSupport(0,players[i],2);
            Check(support.ActiveContributors(0)==25 && support.Qualified(0).Count==25,"validated meaningful support uses the same crowd policy without damage");
        }
        finally { foreach(Mobile player in players) player.Delete(); }
    }
    private static void Scoring()
    {
        Mobile damage = Player();
        Mobile support = Player();
        Mobile patient = Player();
        Mobile npc = new Mobile();
        try
        {
            ArachnidOutbreakLedger ledger = new ArachnidOutbreakLedger("fixture",4320,9792);
            Check(!ledger.CreditDamage(-1,damage,1000) && !ledger.CreditDamage(4,damage,1000)
                && !ledger.CreditDamage(0,damage,0) && !ledger.CreditDamage(0,npc,1000), "invalid events cannot contribute");
            ledger.CreditDamage(0,damage,431);
            Check(ledger.Qualified(0).Count == 0, "substantial but insufficient damage does not qualify");
            ledger.CreditDamage(0,damage,1);
            Check(ledger.Qualified(0).Contains(damage) && ledger.Qualified(1).Count == 0, "ten percent budget qualifies only its own stage");
            Check(!ledger.CreditValidatedSupport(0,support,Double.NaN)
                && !ledger.CreditValidatedSupport(0,support,Double.PositiveInfinity)
                && !ledger.CreditValidatedSupport(0,support,-2), "nonfinite or negative support rejected");
            for (int i=0;i<5;++i) ledger.CreditValidatedSupport(0,support,2);
            Check(ledger.Qualified(0).Contains(support), "independent support qualification without damage");
            Check(!ledger.CreditValidatedHealing(1,support,support,100), "self healing excluded");
            ledger.CreditValidatedHealing(1,support,patient,99);
            Check(ledger.Qualified(1).Count == 0, "healing threshold uses patient maximum health");
            ledger.CreditValidatedHealing(1,support,patient,1);
            Check(ledger.Qualified(1).Contains(support), "effective encounter healing accumulates");
            Check(ledger.Seal(0) && !ledger.Seal(0) && !ledger.CreditDamage(0,patient,4320), "completed stage seals once and rejects later contribution");
            damage.Body=0x192;
            Check(ledger.Qualified(0).Contains(damage) && damage.NetState == null, "dead offline characters retain earned qualification");
            MemoryStream bytes = new MemoryStream();
            BinaryFileWriter writer = new BinaryFileWriter(bytes,true);
            ledger.Serialize(writer); writer.Write(87321); writer.Close();
            BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
            ArachnidOutbreakLedger restored = new ArachnidOutbreakLedger(); restored.Deserialize(reader);
            Check(reader.ReadInt()==87321 && restored.AttemptId==ledger.AttemptId, "ledger positional serializer and identity");
            Check(restored.IsSealed(0) && restored.Qualified(0).Count==2 && restored.Qualified(1).Contains(support), "stage scores and seal persist");
            reader.Close();
            ledger.CreditDamage(3,patient,Int32.MaxValue);
            Check(ledger.Points(3,patient)==10 && ledger.Qualified(3).Contains(patient), "large single hit qualifies without arithmetic overflow");
        }
        finally { damage.Delete();support.Delete();patient.Delete();npc.Delete(); }
    }
    private static void Delivery()
    {
        Mobile player=Player();
        Mobile stranger=Player();
        ArachnidOutbreakController controller=Begin();
        ArachnidOutbreakClaim claim=null;
        try
        {
            List<Mobile> creatures=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            ArachnidOutbreakSpider guardian=(ArachnidOutbreakSpider)creatures[0];
            guardian.Damage(432,player);
            Check(Ledger(controller).Qualified(0).Contains(player), "actual damage hook qualifies participant");
            guardian.Kill();
            Check(Claims(player).Count==1 && Ledger(controller).IsSealed(0), "actual guardian death queues and seals stage");
            claim=Claims(player)[0];
            Item gland=claim.Items[0];
            guardian.OnDeath(null);
            Check(Claims(player).Count==1, "duplicate death callback cannot queue again");
            Check(!claim.TryClaim(stranger) && gland.Parent==claim, "another character cannot collect");
            player.Body=0x192;
            Check(!claim.TryClaim(player) && gland.Parent==claim, "dead claim stays intact");
            player.Body=0x190;
            player.Backpack.MaxItems=1;
            player.Backpack.AddItem(new Item(1));
            Check(!claim.TryClaim(player) && gland.Parent==claim && !gland.Deleted, "full backpack retains exact original item");
            controller.Stop(); controller.Delete();
            Check(!claim.Deleted && gland.Parent==claim, "stopping and deleting encounter preserve independent earned item");
            player.Backpack.Items[0].Delete();
            Check(claim.TryClaim(player) && claim.Deleted && gland.Parent==player.Backpack, "available space transfers existing item");
            Check(!claim.TryClaim(player) && player.Backpack.GetAmount(typeof(BroodVenomGland))==1, "repeated collection cannot mint another gland");
            guardian.OnDeath(null);
            Check(Claims(player).Count==0, "paid stage cannot recreate a consumed claim");
            player.Backpack.MaxItems=125;
            ArachnidOutbreakClaim boss=ArachnidOutbreakClaim.QueueGlands(player,"another-attempt",3);
            Check(Object.ReferenceEquals(boss,ArachnidOutbreakClaim.QueueGlands(player,"another-attempt",3)), "pending entitlement enqueue is idempotent");
            Check(boss.TryClaim(player) && player.Backpack.GetAmount(typeof(BroodVenomGland))==4, "boss awards three and merges stack without loss or duplication");
        }
        finally
        {
            foreach(ArachnidOutbreakClaim pending in Claims(player))pending.Delete();
            controller.Delete(); player.Delete(); stranger.Delete();
        }
    }
    private static void Attribution()
    {
        ArachnidOutbreakController controller=Begin();
        Mobile owner=Player();Mobile bard=Player();Mobile replacement=Player();
        DreadSpider pet=new DreadSpider();DreadSpider provoked=new DreadSpider();DreadSpider wild=new DreadSpider();
        try
        {
            List<Mobile> creatures=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            pet.Controlled=true;pet.ControlMaster=owner;
            creatures[0].Damage(432,pet);
            Check(Ledger(controller).Qualified(0).Contains(owner), "actual controlled damage credits current owner");
            pet.ControlMaster=replacement;
            creatures[1].Damage(432,pet);
            Check(Ledger(controller).Qualified(1).Contains(replacement) && !Ledger(controller).Qualified(1).Contains(owner), "pet transfer does not credit former owner");
            provoked.BardProvoked=true;provoked.BardMaster=bard;provoked.BardTarget=creatures[1];
            creatures[1].Damage(432,provoked);
            Check(Ledger(controller).Qualified(1).Contains(bard), "actual provocation damage credits bard against designated target");
            creatures[2].Damage(432,provoked);
            creatures[2].Damage(432,wild);
            Check(Ledger(controller).Qualified(2).Count==0, "wrong provocation target and unattributed wild combat earn nothing");
            creatures[2].Delete();
            Check(Claims(owner).Count==0 && Claims(bard).Count==0 && Claims(replacement).Count==0, "administrative deletion produces no stage payout");
        }
        finally {controller.Delete();pet.Delete();provoked.Delete();wild.Delete();owner.Delete();bard.Delete();replacement.Delete();}
    }
    private static void HealingProvenance()
    {
        ArachnidOutbreakController controller=Begin();
        Mobile healer=Player();Mobile patient=Player();Mobile outsider=Player();
        DreadSpider pet=new DreadSpider();pet.SetHits(100);pet.Controlled=true;pet.ControlMaster=healer;
        try
        {
            List<Mobile> foes=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            ArachnidOutbreakLedger ledger=Ledger(controller);
            patient.Damage(20,outsider);patient.Damage(20,foes[0]);
            ArachnidOutbreakState state=(ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State",Private).GetValue(controller);
            Check(!state.Engaged,"being attacked without acting cannot keep an encounter engaged");
            patient.Heal(10,healer,false);
            Check(ledger.Points(0,healer)==0,"preexisting unrelated injury is restored without contribution");
            patient.Heal(15,healer,false);
            Check(ledger.Points(0,healer)==0.5 && state.Engaged,"only five actual encounter health restored counts and refreshes activity");
            patient.Heal(15,patient,false);
            Check(ledger.Points(0,healer)==0.5 && ledger.Points(0,patient)==0,"self healing consumes injury without credit");
            patient.Damage(20,outsider);patient.Heal(20,healer,false);
            Check(ledger.Points(0,healer)==0.5,"resolved encounter injury cannot credit later unrelated healing");
            patient.Damage(40,foes[0]);patient.Damage(10,patient);patient.Heal(50,healer,false);
            Check(ledger.Points(0,healer)==4.5,"self inflicted damage does not inflate legitimate enemy restoration");
            patient.Damage(20,foes[0]);patient.Hits+=20;patient.Damage(20,outsider);patient.Heal(100,healer,false);
            Check(ledger.Points(0,healer)==4.5,"direct restoration consumes injury; overheal cannot replay it");
            patient.Damage(20,foes[1]);patient.Damage(20,foes[2]);patient.Heal(40,healer,false);
            Check(ledger.Points(1,healer)==2 && ledger.Points(2,healer)==2,"mixed encounter wounds credit their exact separate stages");
            BaseCreature source=(BaseCreature)foes[0];source.Controlled=true;source.ControlMaster=outsider;
            patient.Damage(20,source);patient.Heal(20,healer,false);
            Check(ledger.Points(0,healer)==4.5,"player controlled event creature cannot manufacture support wounds");
            source.Controlled=false;source.ControlMaster=null;source.BardProvoked=true;source.BardMaster=outsider;source.BardTarget=patient;
            patient.Damage(20,source);patient.Heal(20,healer,false);
            Check(ledger.Points(0,healer)==4.5,"player provoked event creature cannot manufacture support wounds");source.BardProvoked=false;
            patient.Damage(60,foes[0]);patient.Heal(60,healer,false);
            Check(ledger.Qualified(0).Contains(healer),"actual effective healing can independently qualify without damage");
            pet.Damage(20,foes[2]);pet.Heal(20,healer,false);
            Check(ledger.Points(2,healer)==4,"helping a participating controlled pet counts actual restoration");
            healer.Damage(20,foes[2]);healer.Heal(20,healer,false);
            Check(ledger.Points(2,healer)==4,"healing own character earns no support credit");
            patient.Damage(10,foes[2]);
            ArachnidOutbreakWounds wounds=(ArachnidOutbreakWounds)typeof(ArachnidOutbreakController).GetField("m_Wounds",Private).GetValue(controller);
            MemoryStream bytes=new MemoryStream();BinaryFileWriter writer=new BinaryFileWriter(bytes,true);
            wounds.Serialize(writer);writer.Write(83721);writer.Close();
            BinaryFileReader reader=new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
            ArachnidOutbreakWounds restored=new ArachnidOutbreakWounds();restored.Deserialize(reader);
            Check(reader.ReadInt()==83721,"wound serializer consumes exact positional data");reader.Close();
            typeof(ArachnidOutbreakController).GetField("m_Wounds",Private).SetValue(controller,restored);
            patient.Heal(10,healer,false);
            Check(ledger.Points(2,healer)==5,"saved injury balance can be restored and consumed once");
            patient.Damage(100,foes[2]);patient.Heal(100,healer,false);
            Check(ledger.Points(2,healer)==10,"exactly zero health survivor retains genuine encounter healing eligibility");
            patient.Damage(10,foes[2]);patient.Damage(1000,foes[2]);
            Check(!patient.Alive,"overkill causes actual player death");patient.Resurrect();patient.Heal(100,outsider,false);
            Check(ledger.Points(2,outsider)==0,"resurrection cannot reuse pre-death wounds");
            patient.Damage(20,foes[2]);bool rescued=false;
            PlayerDeathEventHandler immediate=delegate(PlayerDeathEventArgs e)
            {
                if(e.Mobile==patient){patient.Resurrect();patient.Heal(100,outsider,false);rescued=true;}
            };
            EventSink.PlayerDeath+=immediate;
            try{patient.Kill();}finally{EventSink.PlayerDeath-=immediate;}
            Check(rescued && patient.Alive && ledger.Points(2,outsider)==0,"injuries clear before a death callback resurrects and heals");
            pet.IsBonded=true;pet.Damage(1000,foes[2]);
            Check(pet.IsDeadBondedPet,"actual bonded pet death recognized");pet.ResurrectPet();pet.Heal(100,outsider,false);
            Check(ledger.Points(2,outsider)==0,"bonded pet resurrection cannot reuse old wounds");
            pet.Damage(20,foes[2]);pet.Controlled=false;pet.ControlMaster=null;pet.Heal(20,outsider,false);
            pet.Damage(20,outsider);pet.Controlled=true;pet.ControlMaster=healer;pet.Heal(20,outsider,false);
            Check(ledger.Points(2,outsider)==0,"released pet cannot exchange healed encounter wounds for later unrelated injury");
            ledger.Seal(1);patient.Damage(20,foes[1]);patient.Heal(20,healer,false);
            Check(ledger.Points(1,healer)==2,"completed stage cannot gain late support eligibility");
        }
        finally {controller.Delete();pet.Delete();healer.Delete();patient.Delete();outsider.Delete();}
    }
    private sealed class CollectibleBoss : ArachnidOutbreakChampion
    {
        public int Rolls;
        public bool Empty;
        public CollectibleBoss(ArachnidOutbreakController controller):base(controller){}
        public override Item RollVictoryArtifact(){++Rolls;return Empty?null:new Calm();}
    }
    private static int GroundGold()
    {
        int total=0;
        foreach(Item item in World.Items.Values)
            if(item is Gold && item.Parent==null && item.Map==Map.Sosaria)total+=item.Amount;
        return total;
    }
    private static int Scrolls()
    {
        int total=0;foreach(Item item in World.Items.Values)if(item is PowerScroll)++total;return total;
    }
    private static void BossRewards(bool damageParticipant,bool artifact,bool distantLoot = false)
    {
        ArachnidOutbreakController controller=Begin();
        Mobile support=Player();Mobile damage=Player();Mobile patient=Player();
        CollectibleBoss boss=new CollectibleBoss(controller);boss.Empty=!artifact;
        try
        {
            List<Mobile> foes=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            foreach(Mobile guardian in foes.ToArray())guardian.Kill();
            Check(controller.Phase==ArachnidOutbreakPhase.Boss,"three actual guardian deaths unlock boss rewards");
            foes.Add(boss);boss.Home=new Point3D(2514,2088,0);boss.MoveToWorld(boss.Home,Map.Sosaria);
            typeof(ArachnidOutbreakController).GetField("m_BossCreated",Private).SetValue(controller,true);
            if(damageParticipant)boss.Damage(1000,damage);
            for(int i=0;i<2;++i){patient.Damage(60,boss);patient.Heal(60,support,false);}
            Check(Ledger(controller).Qualified(3).Contains(support),"actual boss-stage support qualifies independently");
            support.Body=0x192;
            Point3D death = new Point3D(2483,2121,0);
            boss.MoveToWorld(death,Map.Sosaria);
            if(distantLoot)
            {
                List<Point3D> tiles=new List<Point3D>();
                SosariaOutbreakTerrain terrain=new SosariaOutbreakTerrain();
                for(int x=boss.Home.X-8;x<=boss.Home.X+8 && tiles.Count<25;++x)
                    for(int y=boss.Home.Y-8;y<=boss.Home.Y+8 && tiles.Count<25;++y)
                    {
                        Point3D point;
                        if(terrain.TryGetPoint(x,y,out point))tiles.Add(point);
                    }
                Check(tiles.Count==25,"fallback fixture has 25 safe tiles beyond the death's local radius");
                typeof(ArachnidOutbreakController).GetField("m_Placement",Private).SetValue(controller,
                    new ArachnidOutbreakPlacement(boss.Home,new Point3D[]{boss.Home,boss.Home,boss.Home},tiles.ToArray()));
            }
            int gold=GroundGold();int scrolls=Scrolls();boss.Kill();
            Check(controller.Phase==ArachnidOutbreakPhase.Cooldown && Ledger(controller).IsSealed(3),"real boss death settles and seals stage");
            Check(GroundGold()-gold==15000,"boss shower contains exactly 15000 shared ground gold");
            HashSet<Point2D> positions = new HashSet<Point2D>();
            int piles = 0;
            foreach (Item item in World.Items.Values)
                if (item is Gold && item.Parent == null && item.Map == Map.Sosaria)
                {
                    ++piles;
                    positions.Add(new Point2D(item.X,item.Y));
                    Check(item.Amount == 600,"each shared gold pile contains 600");
                }
            Check(piles == 25 && positions.Count == 25,"all 25 gold piles occupy different ground tiles");
            foreach (Point2D point in positions)
                Check(Math.Abs(point.X-death.X)<=(distantLoot?56:12) && Math.Abs(point.Y-death.Y)<=(distantLoot?56:12),
                    "gold follows the actual boss death rather than the original spawn anchor");
            if(distantLoot)
                foreach(Point2D point in positions)
                    Check(Math.Abs(point.X-death.X)>12 || Math.Abs(point.Y-death.Y)>12,"wider safe-ground fallback actually supplies the payout");
            Check(boss.Rolls==1,"one existing artifact opportunity per clear");
            List<ArachnidOutbreakClaim> all=Claims(support);all.AddRange(Claims(damage));
            int glands=0,skulls=0,artifacts=0;
            foreach(ArachnidOutbreakClaim claim in all)
            {
                if(claim.Kind==ArachnidOutbreakClaimKind.Glands)
                {
                    ++glands;Check(claim.Items[0] is BroodVenomGland && claim.Items[0].Amount==3,"each qualified boss participant earns three glands");
                }
                else if(claim.Kind==ArachnidOutbreakClaimKind.Skull){++skulls;Check(claim.Items[0] is ChampionSkull,"skull item retained in claim");}
                else if(claim.Kind==ArachnidOutbreakClaimKind.Artifact){++artifacts;Check(claim.Items[0] is Calm,"selected named artifact retained in claim");}
            }
            Check(glands==(damageParticipant?2:1) && skulls==1 && artifacts==(artifact?1:0),"one skull, optional artifact, no duplicate individual rewards");
            Check(Scrolls()==scrolls,"complete new encounter creates no power scrolls");
            typeof(ArachnidOutbreakController).GetMethod("CreatureDied",Private).Invoke(controller,new object[]{boss,null});
            Check(GroundGold()-gold==15000 && boss.Rolls==1 && Claims(support).Count+Claims(damage).Count==all.Count,"replayed boss callback cannot repeat gold, collectible roll or claims");
            if(!damageParticipant)
                Check(Claims(damage).Count==0 && Claims(support).Find(c=>c.Kind==ArachnidOutbreakClaimKind.Skull)!=null,"dead offline support-only player receives collectible rights");
        }
        finally
        {
            foreach(ArachnidOutbreakClaim c in Claims(support))c.Delete();foreach(ArachnidOutbreakClaim c in Claims(damage))c.Delete();
            controller.Delete();boss.Delete();support.Delete();damage.Delete();patient.Delete();
            List<Item> cleanup=new List<Item>();foreach(Item item in World.Items.Values)if(item is Gold && item.Parent==null && item.Map==Map.Sosaria)cleanup.Add(item);
            foreach(Item item in cleanup)item.Delete();
        }
    }
    private static void UnsafeBossLoot(bool blockWithPortal)
    {
        ArachnidOutbreakController controller=Begin(true);Mobile player=Player();CollectibleBoss boss=new CollectibleBoss(controller);
        List<Moongate> portals=new List<Moongate>();
        try
        {
            List<Mobile> foes=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            foreach(Mobile guardian in foes.ToArray())guardian.Kill();
            foes.Add(boss);boss.MoveToWorld(new Point3D(2514,2088,0),Map.Sosaria);
            List<Point3D> tiles=new List<Point3D>();
            if(blockWithPortal)
            {
                SosariaOutbreakTerrain terrain=new SosariaOutbreakTerrain();
                for(int x=boss.X-12;x<=boss.X+12 && tiles.Count<25;++x)
                    for(int y=boss.Y-12;y<=boss.Y+12 && tiles.Count<25;++y)
                    {
                        Point3D point;
                        if(terrain.TryGetPoint(x,y,out point))tiles.Add(point);
                    }
                Check(tiles.Count==25,"portal fixture starts with 25 valid distinct terrain tiles");
                foreach(Point3D point in tiles)
                {
                    Moongate portal=new Moongate();portal.MoveToWorld(point,Map.Sosaria);portals.Add(portal);
                }
            }
            else
                for(int i=0;i<25;++i)tiles.Add(boss.Location);
            typeof(ArachnidOutbreakController).GetField("m_Placement",Private).SetValue(controller,
                new ArachnidOutbreakPlacement(boss.Location,new Point3D[]{boss.Location,boss.Location,boss.Location},tiles.ToArray()));
            boss.Damage(1000,player);int gold=GroundGold();boss.Kill();
            Check(controller.Phase==ArachnidOutbreakPhase.Cooldown && controller.Status.Contains("cancelled"),"fewer than 25 distinct safe loot tiles cancels outcome without stacking fallback");
            Check(Claims(player).Count==0 && GroundGold()==gold && boss.Rolls==0,"unsafe outcome creates no new boss rewards or collectible roll");
        }
        finally {foreach(Moongate portal in portals)portal.Delete();controller.Delete();boss.Delete();player.Delete();}
    }
    private static void GoldShowerSafety()
    {
        MethodInfo shower=typeof(ArachnidOutbreakController).GetMethod("PlayGoldShower",BindingFlags.Static|BindingFlags.NonPublic);
        Mobile player=Player();Gold gold=new Gold(600);
        try
        {
            gold.MoveToWorld(new Point3D(2514,2088,0),Map.Sosaria);
            int total=GroundGold();int items=World.Items.Count;
            shower.Invoke(null,new object[]{gold});shower.Invoke(null,new object[]{gold});
            Check(GroundGold()==total && World.Items.Count==items && gold.Amount==600,"repeated cosmetic shower cannot create or change reward items");
            player.Backpack.AddItem(gold);
            shower.Invoke(null,new object[]{gold});
            Check(gold.Parent==player.Backpack && gold.Amount==600 && World.Items.Count==items,"collected gold stays collected during delayed effects");
            gold.MoveToWorld(Point3D.Zero,Map.Internal);
            shower.Invoke(null,new object[]{gold});
            Check(gold.Map==Map.Internal && gold.Amount==600,"internal-map gold is not relocated by a pending shower");
            gold.Delete();items=World.Items.Count;
            shower.Invoke(null,new object[]{gold});shower.Invoke(null,new object[]{null});shower.Invoke(null,new object[]{player});
            Check(gold.Deleted && World.Items.Count==items,"deleted and invalid cosmetic targets cannot recreate gold");
        }
        finally {gold.Delete();player.Delete();}
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();Corpse.Initialize();ArachnidOutbreakController.Initialize();
            Scoring(); CrowdQualification(); Delivery(); Attribution(); HealingProvenance();BossRewards(true,true);BossRewards(false,true);BossRewards(false,false);BossRewards(true,false,true);UnsafeBossLoot(true);UnsafeBossLoot(false);GoldShowerSafety();
            Console.WriteLine("PASS outbreak rewards: "+s_Checks+" component assertions; no world saves or listeners.");return 0;
        }
        catch(Exception exception){Console.Error.WriteLine(exception);return 1;}
    }
}
