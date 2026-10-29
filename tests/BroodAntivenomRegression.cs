using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Engines.Craft;
using Server.Engines.PartySystem;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Custom.Confictura.Champions;

internal static class BroodAntivenomRegression
{
    private static int s_Checks;
    private static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    private static void Check(bool condition,string message){if(!condition)throw new Exception("FAILED: "+message);++s_Checks;}
    private static PlayerMobile Player(int x)
    {
        PlayerMobile player=new PlayerMobile();player.Player=true;player.RawStr=100;player.Body=0x190;
        player.Hits=player.HitsMax;player.AddItem(new Backpack());player.MoveToWorld(new Point3D(x,2088,0),Map.Sosaria);return player;
    }
    private static BroodAntivenom Dose(Mobile player,int amount)
    {
        BroodAntivenom item=new BroodAntivenom();item.Amount=amount;player.Backpack.AddItem(item);return item;
    }
    private static BroodAntivenomEffect Effect(Mobile player)
    {
        int count;BroodAntivenomEffect found=BroodAntivenomEffect.Find(player,out count);Check(count==1 && found!=null,"one effect per recipient");return found;
    }
    private static void Forget(Mobile player)
    {
        List<Item> cleanup=new List<Item>();foreach(Item item in World.Items.Values)
            if(item is BroodAntivenomEffect && ((BroodAntivenomEffect)item).Owner==player)cleanup.Add(item);
        foreach(Item item in cleanup)item.Delete();
    }
    private static void Protection()
    {
        PlayerMobile player=Player(2514);PlayerMobile hostile=Player(2515);DreadSpider wild=new DreadSpider();DreadSpider owned=new DreadSpider();
        try
        {
            string reason;BroodAntivenom item=Dose(player,10);int bottles=player.Backpack.GetAmount(typeof(Bottle));DateTime before=DateTime.UtcNow;
            Check(item.TryApply(player,player,out reason) && item.Amount==9 && player.Backpack.GetAmount(typeof(Bottle))==bottles+1,"self application consumes exactly one and returns one bottle");
            BroodAntivenomEffect effect=Effect(player);DateTime expiry=effect.Expires;
            Check(effect.Charges==3 && effect.Expires>=before.AddMinutes(5) && effect.NextUse>=before.AddMinutes(2),"three charges, five minute expiry and two minute recipient reuse");
            Check(!item.TryApply(player,player,out reason) && item.Amount==9 && effect.Expires==expiry,"active effect cannot stack or refresh");
            Check(player.ApplyPoison(wild,Poison.Greater)==ApplyPoisonResult.Poisoned && effect.Charges==3,"ordinary poison is not prevented");player.CurePoison(player);
            Check(player.ApplyPoison(wild,Poison.Deadly)==ApplyPoisonResult.Immune && !player.Poisoned && effect.Charges==2,"deadly wild poison uses one protection");
            Check(player.ApplyPoison(wild,Poison.Lethal)==ApplyPoisonResult.Immune && effect.Charges==1,"lethal wild poison uses one protection");
            player.ApplyPoison(wild,Poison.Deadly);
            Check(effect.Charges==0 && !player.Poisoned,"third prevention exhausts protection");
            Check(player.ApplyPoison(wild,Poison.Deadly)==ApplyPoisonResult.Poisoned,"fourth application is not prevented");
            Check(!item.TryApply(player,player,out reason) && item.Amount==9,"exhaustion does not bypass reuse delay");
            typeof(BroodAntivenomEffect).GetField("m_NextUse",Private).SetValue(effect,DateTime.UtcNow.AddSeconds(-1));
            Check(item.TryApply(player,player,out reason) && effect.Deleted && player.Poison==Poison.Deadly,"eligible retreatment does not cure existing poison");effect=Effect(player);
            player.ApplyPoison(wild,Poison.Deadly);Check(effect.Charges==3,"equal active poison consumes no protection");
            player.ApplyPoison(wild,Poison.Lethal);Check(effect.Charges==2 && player.Poison==Poison.Deadly,"preventing upgrade leaves current poison untouched");player.CurePoison(player);
            player.ApplyPoison(hostile,Poison.Deadly);Check(player.Poisoned && effect.Charges==2,"player poison excluded");player.CurePoison(player);
            owned.Controlled=true;owned.ControlMaster=hostile;
            player.ApplyPoison(owned,Poison.Deadly);Check(player.Poisoned && effect.Charges==2,"controlled source excluded");player.CurePoison(player);
            owned.Controlled=false;owned.ControlMaster=null;owned.Summoned=true;owned.SummonMaster=hostile;
            player.ApplyPoison(owned,Poison.Deadly);Check(player.Poisoned && effect.Charges==2,"summoned source excluded");player.CurePoison(player);
            owned.Summoned=false;owned.BardProvoked=true;owned.BardMaster=hostile;owned.BardTarget=player;
            player.ApplyPoison(owned,Poison.Deadly);Check(player.Poisoned && effect.Charges==2,"provoked source excluded");player.CurePoison(player);
            player.Young=true;player.ApplyPoison(wild,Poison.Lethal);Check(effect.Charges==2 && !player.Poisoned,"existing young immunity precedes antivenom");player.Young=false;
            typeof(BroodAntivenomEffect).GetField("m_Expires",Private).SetValue(effect,DateTime.UtcNow.AddSeconds(-1));
            Check(player.ApplyPoison(wild,Poison.Deadly)==ApplyPoisonResult.Poisoned && effect.Charges==2,"expired treatment does not consume or prevent");
        }
        finally{Forget(player);player.Delete();hostile.Delete();wild.Delete();owned.Delete();}
    }
    private static void Recipients()
    {
        PlayerMobile helper=Player(2514);PlayerMobile ally=Player(2515);PlayerMobile stranger=Player(2516);DreadSpider pet=new DreadSpider();
        Party party=new Party(helper);helper.Party=party;party.Add(ally);
        try
        {
            string reason;BroodAntivenom item=Dose(helper,6);
            Check(!item.TryApply(helper,stranger,out reason) && item.Amount==6,"nonparty player rejected without consumption");
            bool applied=item.TryApply(helper,ally,out reason);
            Console.WriteLine("Ally application: "+applied+"; "+reason+"; LOS="+helper.InLOS(ally)+"; beneficial="+helper.CanBeBeneficial(ally,false)+"; member="+party.Contains(ally)+"; actorparty="+(Party.Get(helper)==party));
            Check(applied && Effect(ally).AppliedBy==helper,"party ally allowed and applicator retained");
            pet.Controlled=true;pet.ControlMaster=ally;pet.MoveToWorld(new Point3D(2515,2088,0),Map.Sosaria);
            Check(item.TryApply(helper,pet,out reason) && Effect(pet).Owner==pet,"controlled pet belonging to party ally allowed");
            Forget(ally);ally.MoveToWorld(new Point3D(2518,2088,0),Map.Sosaria);
            Check(!item.TryApply(helper,ally,out reason),"range beyond two rejected");ally.MoveToWorld(new Point3D(2515,2088,0),Map.Lodor);
            Check(!item.TryApply(helper,ally,out reason),"different facet rejected");
            BroodAntivenom foreign=Dose(stranger,1);Check(!foreign.TryApply(helper,helper,out reason),"treatment in another character's backpack cannot be used");
            ally.MoveToWorld(new Point3D(2516,2088,0),Map.Sosaria);Static wall=new Static(0x80);wall.MoveToWorld(new Point3D(2515,2088,0),Map.Sosaria);
            try
            {
                Check(!helper.InLOS(ally),"real wall establishes blocked line of sight");int amount=item.Amount;
                Check(!item.TryApply(helper,ally,out reason) && item.Amount==amount,"blocked line of sight rejects without consumption");
            }
            finally{wall.Delete();}
            ally.MoveToWorld(new Point3D(2515,2088,0),Map.Sosaria);
            VampiricEmbraceSpell spell=new VampiricEmbraceSpell(ally,null);TransformTimer timer=new TransformTimer(ally,spell);
            TransformationSpellHelper.AddContext(ally,new TransformContext(timer,new List<ResistanceMod>(),typeof(VampiricEmbraceSpell),spell));
            try{Check(!item.TryApply(helper,ally,out reason) && reason.Contains("garlic"),"existing vampiric garlic restriction preserved");}
            finally{TransformationSpellHelper.RemoveContext(ally,false);}
            PoisonChangeRegression.Denier denier=new PoisonChangeRegression.Denier();denier.Denied=TurnMutationKind.Effect;
            TurnBasedCombatBridge.Register(denier);
            try{int amount=item.Amount;Check(!item.TryApply(helper,helper,out reason) && item.Amount==amount,"turn denied treatment creates no effect or consumption");}
            finally{TurnBasedCombatBridge.Unregister(denier);}
        }
        finally{Forget(helper);Forget(ally);Forget(pet);party.Remove(ally);party.Remove(helper);helper.Delete();ally.Delete();stranger.Delete();pet.Delete();}
    }
    private static void Crafting()
    {
        PlayerMobile player=Player(2514);MortarPestle tool=new MortarPestle();player.Backpack.AddItem(tool);
        try
        {
            CraftSystem system=DefAlchemy.CraftSystem;CraftItem recipe=system.CraftItems.SearchFor(typeof(BroodAntivenom));
            Check(recipe!=null && recipe.ForceNonExceptional && !recipe.UseAllRes,"known alchemy recipe is never exceptional and uses single-recipe resource policy");bool skills=false;
            player.Skills[SkillName.Alchemy].Base=80;
            Check(recipe.GetSuccessChance(player,null,system,false,ref skills)==0 && skills,"minimum skill starts at zero chance");
            player.Skills[SkillName.Alchemy].Base=100;
            Check(recipe.GetSuccessChance(player,null,system,false,ref skills)==1,"100 Alchemy is guaranteed success");
            player.Backpack.AddItem(new BroodVenomGland());player.Backpack.AddItem(new OrangePetals(2));player.Backpack.AddItem(new Garlic(6));player.Backpack.AddItem(new Bottle(2));
            recipe.CompleteCraft(1,false,player,system,null,tool,null);
            Check(player.Backpack.GetAmount(typeof(BroodAntivenom))==2,"actual successful craft produces exactly two treatments");
            Check(player.Backpack.GetAmount(typeof(BroodVenomGland))==0 && player.Backpack.GetAmount(typeof(OrangePetals))==0
                && player.Backpack.GetAmount(typeof(Garlic))==0 && player.Backpack.GetAmount(typeof(Bottle))==0,"successful recipe consumes one gland, two petals, six garlic and two bottles");
            player.Backpack.AddItem(new BroodVenomGland());player.Backpack.AddItem(new OrangePetals(2));player.Backpack.AddItem(new Garlic(6));player.Backpack.AddItem(new Bottle(2));
            int hue=0,max=0;object message=null;
            Check(recipe.ConsumeRes(player,null,system,ref hue,ref max,ConsumeType.All,ref message,true),"actual single-recipe failure resource path succeeds");
            Console.WriteLine("Failed recipe remaining: gland="+player.Backpack.GetAmount(typeof(BroodVenomGland))+"; petals="+player.Backpack.GetAmount(typeof(OrangePetals))+"; garlic="+player.Backpack.GetAmount(typeof(Garlic))+"; bottles="+player.Backpack.GetAmount(typeof(Bottle)));
            Check(player.Backpack.GetAmount(typeof(BroodVenomGland))==1 && player.Backpack.GetAmount(typeof(OrangePetals))==0
                && player.Backpack.GetAmount(typeof(Garlic))==0 && player.Backpack.GetAmount(typeof(Bottle))==0,"failed craft preserves gland and follows ordinary common-ingredient consumption");
            player.Backpack.AddItem(new OrangePetals(2));player.Backpack.AddItem(new Garlic(6));player.Backpack.AddItem(new Bottle(2));
            player.Skills[SkillName.Alchemy].Base=80;player.Skills[SkillName.Alchemy].SetLockNoRelay(SkillLock.Locked);
            recipe.CompleteCraft(1,false,player,system,null,tool,null);
            Check(player.Backpack.GetAmount(typeof(BroodVenomGland))==1 && player.Backpack.GetAmount(typeof(BroodAntivenom))==2
                && player.Backpack.GetAmount(typeof(OrangePetals))==0 && player.Backpack.GetAmount(typeof(Garlic))==0
                && player.Backpack.GetAmount(typeof(Bottle))==0,"actual failed craft at minimum skill preserves gland, yields nothing and consumes common ingredients");
        }
        finally{player.Delete();tool.Delete();}
    }
    private static void Contribution()
    {
        ArachnidOutbreakController.Initialize();ArachnidOutbreakController controller=new ArachnidOutbreakController();
        PlayerMobile helper=Player(2514);PlayerMobile patient=Player(2515);DreadSpider unrelated=new DreadSpider();
        Party party=new Party(helper);helper.Party=party;party.Add(patient);
        try
        {
            ArachnidOutbreakState state=(ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State",Private).GetValue(controller);state.SetEnabled(true);
            Point3D boss=new Point3D(2514,2088,0);
            ArachnidOutbreakPlacement placement=new ArachnidOutbreakPlacement(boss,new Point3D[]{boss,boss,boss},new Point3D[]{boss});
            typeof(ArachnidOutbreakController).GetMethod("BeginAt",Private).Invoke(controller,new object[]{new ArachnidOutbreakSite("antivenom-fixture",2514,2088,false),placement});
            List<Mobile> foes=(List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(controller);
            ArachnidOutbreakLedger ledger=(ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger",Private).GetValue(controller);
            PoisonChangedEventHandler observe=delegate(PoisonChangedEventArgs e){Console.WriteLine("Contribution poison: kind="+e.Kind+"; source="+(e.Source==null?"null":e.Source.GetType().Name)+"; old="+(e.OldPoison==null?-1:e.OldPoison.Level)+"; new="+(e.NewPoison==null?-1:e.NewPoison.Level));};
            EventSink.PoisonChanged+=observe;
            ApplyPoisonResult applied=patient.ApplyPoison(foes[0],Poison.Deadly);bool cured=patient.CurePoison(helper);EventSink.PoisonChanged-=observe;
            Console.WriteLine("Contribution condition: apply="+applied+"; cured="+cured+"; sourcePlayer="+foes[0].Player+"; points="+ledger.Points(0,helper)+"; attempt="+controller.AttemptId);
            Check(ledger.Points(0,helper)==2,"actual removal of encounter poison credits helper");patient.CurePoison(helper);
            patient.ApplyPoison(foes[0],Poison.Deadly);PoisonChangeRegression.Denier denier=new PoisonChangeRegression.Denier();TurnBasedCombatBridge.Register(denier);
            try{Check(!patient.CurePoison(helper) && patient.Poisoned && ledger.Points(0,helper)==2,"actual player override preserves denied cure and grants nothing");}
            finally{TurnBasedCombatBridge.Unregister(denier);}
            patient.CurePoison(patient);
            Check(ledger.Points(0,helper)==2,"repeated cure of resolved condition earns nothing");
            patient.ApplyPoison(foes[0],Poison.Deadly);patient.Poison=Poison.Deadly;patient.CurePoison(helper);
            Check(ledger.Points(0,helper)==2,"same-level direct reassignment invalidates old poison provenance");
            patient.ApplyPoison(unrelated,Poison.Deadly);patient.CurePoison(helper);
            Check(ledger.Points(0,helper)==2,"unrelated wild poison cure earns no encounter credit");
            patient.ApplyPoison(foes[0],Poison.Deadly);patient.CurePoison(patient);
            Check(ledger.Points(0,patient)==0,"self cure consumes condition without support credit");
            for(int i=0;i<4;++i){patient.ApplyPoison(foes[0],Poison.Deadly);patient.CurePoison(helper);}
            Check(ledger.Qualified(0).Contains(helper),"effective relevant cures qualify independently of damage");
            string reason;BroodAntivenom dose=Dose(helper,5);
            Check(dose.TryApply(helper,patient,out reason),"party treatment applied for prevention credit");
            patient.ApplyPoison(foes[1],Poison.Deadly);
            Check(ledger.Points(1,helper)==2 && !patient.Poisoned,"consumed protection against encounter enemy credits applicator");
            patient.ApplyPoison(foes[1],Poison.Lethal);patient.ApplyPoison(foes[1],Poison.Deadly);
            Check(ledger.Points(1,helper)==6,"three distinct protections credit exactly six points");
            BroodAntivenomEffect effect=Effect(patient);typeof(BroodAntivenomEffect).GetField("m_NextUse",Private).SetValue(effect,DateTime.UtcNow.AddSeconds(-1));
            dose.TryApply(helper,patient,out reason);patient.ApplyPoison(unrelated,Poison.Deadly);
            Check(ledger.Points(1,helper)==6,"unrelated wild prevention works without encounter credit");
            ledger.Seal(1);patient.ApplyPoison(foes[1],Poison.Lethal);
            Check(ledger.Points(1,helper)==6,"late protection cannot qualify for an already completed stage");
        }
        finally
        {
            Forget(patient);Forget(helper);controller.Delete();party.Remove(patient);party.Remove(helper);helper.Delete();patient.Delete();unrelated.Delete();
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();BroodAntivenomEffect.Initialize();
            Protection();Recipients();Crafting();Contribution();
            Console.WriteLine("PASS brood antivenom: "+s_Checks+" production-method assertions; no saves or listeners.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
