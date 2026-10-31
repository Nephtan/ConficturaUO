using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Spells;
using Server.Spells.Third;
using Server.Custom.Confictura.CloneOfflinePlayerCharacters;

public static class OfflineCloneFidelityRegression
{
    private static int Checks;
    private static void Check(bool value,string message){++Checks;if(!value)throw new Exception(message);}
    private static CharacterClone Copy(PlayerMobile source)
    {
        CharacterClone clone=CloneThings.CreateClone(source);
        CloneThings.CloneMobileProperties(source,clone);CloneThings.CloneMobileItems(source,clone);
        CloneThings.CloneMobileBackpack(source,clone);CloneThings.CloneMobileMount(source,clone);return clone;
    }
    private static PlayerMobile Player()
    {
        PlayerMobile player=new PlayerMobile();player.RawStr=100;player.RawDex=80;player.RawInt=90;player.Body=0x190;
        player.AddItem(new Backpack());player.Skills[SkillName.Tactics].Base=100;player.Skills[SkillName.Anatomy].Base=100;
        player.Skills[SkillName.Healing].Base=100;player.Skills[SkillName.Magery].Base=100;
        Robe robe=new Robe();robe.Attributes.BonusStr=10;robe.Attributes.BonusInt=15;robe.Attributes.BonusHits=20;player.AddItem(robe);
        player.Hits=73;player.Stam=29;player.Mana=35;return player;
    }
    private static void StateAndDamage()
    {
        PlayerMobile original=Player();Longsword weapon=new Longsword();original.AddItem(weapon);CharacterClone clone=Copy(original);
        try
        {
            Check(clone.Str==original.Str && clone.Dex==original.Dex && clone.Int==original.Int,"equipment bonuses must apply once");
            Check(clone.HitsMax==original.HitsMax && clone.StamMax==original.StamMax && clone.ManaMax==original.ManaMax,"clone must use actual character vital maxima");
            Check(clone.Hits==73 && clone.Stam==29 && clone.Mana==35,"copying does not refill actual resources");
            int a,b,c,d;weapon.GetBaseDamageRange(original,out a,out b);((BaseWeapon)clone.Weapon).GetBaseDamageRange(clone,out c,out d);
            Check(a==c && b==d,"skill/stat damage bonuses must not be baked into base weapon damage and applied twice");
            clone.Hits=31;typeof(CharacterClone).GetMethod("SynchronizeSnapshot").Invoke(clone,new object[]{false});Check(clone.Hits==31,"normalization never refills an existing wounded clone");
            Check(original.Hits==73 && original.Mana==35,"source character is unchanged");
            Check(clone.GetMaxResistance(ResistanceType.Physical)==original.GetMaxResistance(ResistanceType.Physical),"resistance cap follows actual character rules");
        }
        finally{clone.Delete();original.Delete();}
    }
    private static void Consumables()
    {
        PlayerMobile original=Player();CharacterClone clone=Copy(original);LargeSpider npc=new LargeSpider();
        try
        {
            clone.MoveToWorld(new Point3D(2514,2088,Map.Sosaria.GetAverageZ(2514,2088)),Map.Sosaria);
            FireballSpell spell=new FireballSpell(clone,null);SpellRegistry.Register(17,typeof(FireballSpell));
            Check(!clone.CheckSpellCast(spell),"skills alone do not grant an unlearned spell");
            Spellbook book=new Spellbook(1UL<<17);clone.AddToBackpack(book);
            Check(clone.CheckSpellCast(spell),"an actual known spell remains eligible");
            Check(!spell.ConsumeReagents(),"clone cannot cast with missing reagents");
            for(int i=0;i<spell.Info.Reagents.Length;++i){Item reagent=(Item)Activator.CreateInstance(spell.Info.Reagents[i]);reagent.Amount=spell.Info.Amounts[i];clone.AddToBackpack(reagent);}
            Check(spell.ConsumeReagents(),"eligible finite reagents can be consumed");Check(!spell.ConsumeReagents(),"consumed reagents cannot be reused");
            Bow bow=new Bow();clone.AddItem(bow);Check(!bow.OnFired(clone,npc),"clone cannot fire without ammo");
            clone.AddToBackpack(new Arrow(2));Check(bow.OnFired(clone,npc) && clone.Backpack.GetAmount(typeof(Arrow))==1,"clone consumes actual ammunition");
            Check(bow.OnFired(npc,clone),"ordinary NPC ammunition policy remains unchanged");
            clone.Hits=10;Check(!BandageSelf(clone),"healing skill does not create bandages");
            Bandage bandage=new Bandage(2);clone.AddToBackpack(bandage);
            Check(BandageSelf(clone) && bandage.Amount==1,"clone uses the real finite bandage action; map="+clone.Map+"; self LOS="+clone.InLOS(clone)+"; beneficial="+clone.CanBeBeneficial(clone,false));
            Check(!BandageSelf(clone) && bandage.Amount==1,"pending bandage cannot be duplicated or consume another charge");
        }
        finally{clone.Delete();original.Delete();npc.Delete();}
    }
    private static bool BandageSelf(CharacterClone clone){return (bool)typeof(CharacterClone).GetMethod("TryBandage").Invoke(clone,new object[]{clone});}
    private static void MountOwnership()
    {
        PlayerMobile original=Player();Horse mount=new Horse();mount.SetControlMaster(original);mount.Rider=original;int followers=original.Followers;
        CharacterClone clone=Copy(original);MountClone copy=clone.Mount as MountClone;
        Check(copy!=null && copy.ControlMaster==null && copy.Spawner==null,"clone mount does not inherit original owner/spawner links");
        Check(original.Followers==followers && mount.ControlMaster==original && mount.Rider==original,"mount cloning does not change original ownership/followers");
        clone.Delete();Check(copy.Deleted && !mount.Deleted && mount.Rider==original,"clone cleanup deletes its copy and preserves the original mount");mount.Delete();original.Delete();
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();
            StateAndDamage();Consumables();MountOwnership();Console.WriteLine("PASS clone fidelity: "+Checks+" source-method checks; no existing saves or listener.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
