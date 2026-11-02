using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    public sealed class CharacterCapabilityProfile
    {
        public int NominalLevel;
        public string TrainingBand, WeaponSkill;
        public double HighestBaseSkill, WeaponSkillValue;
        public int NearbyLivingCompanions, NearbyCompanionHealth, Bandages, HealingPotions, CurePotions, DeathPreventionItems;
        public readonly List<string> Roles = new List<string>();
        public readonly List<string> Uncertainty = new List<string>();
    }

    // Read-only readiness evidence. It deliberately does not replace character level, gate
    // rewards, change encounter strength, or infer spell/gear effects from an item name.
    public static class CharacterCapabilityService
    {
        private static double Base(Mobile mobile, SkillName skill) { return mobile.Skills[skill].Base; }
        private static double Highest(Mobile mobile, params SkillName[] skills)
        { double highest=0; foreach (SkillName skill in skills) highest=Math.Max(highest,Base(mobile,skill)); return highest; }

        public static CharacterCapabilityProfile Inspect(Mobile mobile)
        {
            CharacterCapabilityProfile result=new CharacterCapabilityProfile();
            if (mobile==null || mobile.Deleted || mobile.Skills==null) { result.Uncertainty.Add("No current mobile/skill evidence."); return result; }
            result.NominalLevel=CharacterLevelService.GetOverallLevel(mobile);
            for (int i=0;i<mobile.Skills.Length;++i) result.HighestBaseSkill=Math.Max(result.HighestBaseSkill,mobile.Skills[i].Base);
            result.TrainingBand=result.HighestBaseSkill<25 ? "Newcomer" : result.HighestBaseSkill<50 ? "Novice" : result.HighestBaseSkill<80 ? "Journeyman" : result.HighestBaseSkill<100 ? "Master" : "Veteran";
            if (Highest(mobile,SkillName.Swords,SkillName.Fencing,SkillName.Bludgeoning,SkillName.FistFighting)>=30) result.Roles.Add("Melee");
            if (Base(mobile,SkillName.Marksmanship)>=30) result.Roles.Add("Archer");
            if (Highest(mobile,SkillName.Magery,SkillName.Elementalism,SkillName.Necromancy,SkillName.Druidism)>=30) result.Roles.Add("Caster");
            if (Highest(mobile,SkillName.Taming,SkillName.Herding)>=50) result.Roles.Add("Tamer");
            if (Base(mobile,SkillName.Musicianship)>=50 && Highest(mobile,SkillName.Provocation,SkillName.Discordance,SkillName.Peacemaking)>=30) result.Roles.Add("Bard");
            if (Highest(mobile,SkillName.Healing,SkillName.Veterinary,SkillName.Spiritualism)>=30) result.Roles.Add("Support");
            if (Highest(mobile,SkillName.Blacksmith,SkillName.Carpentry,SkillName.Tailoring,SkillName.Tinkering,SkillName.Bowcraft,SkillName.Inscribe,SkillName.Alchemy,SkillName.Cooking)>=50) result.Roles.Add("Crafting");

            BaseWeapon weapon=mobile.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;
            if (weapon==null) weapon=mobile.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;
            SkillName used=weapon==null ? SkillName.FistFighting : weapon.GetUsedSkill(mobile,true);
            result.WeaponSkill=used.ToString(); result.WeaponSkillValue=mobile.Skills[used].Value;
            if (result.Roles.Contains("Melee") && result.WeaponSkillValue<30) result.Uncertainty.Add("Equipped attack skill is below30; trained weapon skills may not match the loadout.");
            if (result.Roles.Contains("Archer") && (weapon==null || used!=SkillName.Marksmanship)) result.Uncertainty.Add("Archer training is present without a currently used ranged attack skill.");
            PlayerMobile player=mobile as PlayerMobile;
            if (player!=null)
            {
                // The follower cache can be empty before global initialization after World.Load.
                // Nearby creatures' actual master references work in both saved and running worlds.
                if (player.Map!=null && player.Map!=Map.Internal)
                {
                    IPooledEnumerable nearby=player.GetMobilesInRange(24);
                    try { foreach (Mobile companion in nearby)
                    {
                        BaseCreature creature=companion as BaseCreature;
                        if (creature==null || creature.Deleted || !creature.Alive || creature.IsDeadBondedPet || creature.ControlMaster!=player && creature.SummonMaster!=player) continue;
                        ++result.NearbyLivingCompanions; result.NearbyCompanionHealth+=creature.Hits;
                    }} finally { nearby.Free(); }
                }
                if (Base(mobile,SkillName.Knightship)>=30 && player.TithingPoints==0) result.Uncertainty.Add("Knightship training has zero current tithing.");
            }
            if (mobile.Backpack!=null) foreach (Item item in mobile.Backpack.FindItemsByType(typeof(Item),true))
            {
                string name=item.GetType().Name;
                if (item is Bandage) result.Bandages+=item.Amount;
                if (name=="LesserHealPotion" || name=="HealPotion" || name=="GreaterHealPotion" || name=="HerbalHealingPotion") result.HealingPotions+=item.Amount;
                if (name=="LesserCurePotion" || name=="CurePotion" || name=="GreaterCurePotion") result.CurePotions+=item.Amount;
                if (name=="JewelImmortality") result.DeathPreventionItems+=item.Amount;
            }
            int fightingRoles=0; foreach (string role in result.Roles) if (role=="Melee" || role=="Archer" || role=="Caster" || role=="Tamer" || role=="Bard") ++fightingRoles;
            if (fightingRoles>1) result.Uncertainty.Add("Hybrid training: available actions, equipment and resource costs determine the effective role.");
            if (result.Roles.Contains("Tamer") && result.NearbyLivingCompanions==0) result.Uncertainty.Add("Taming training has no observed living companion within24 tiles; stabled/off-map pets are not assessed.");
            if (result.Roles.Contains("Bard") || result.Roles.Contains("Support")) result.Uncertainty.Add("Support/control usefulness is encounter-dependent; a solo damage test is incomplete.");
            if (result.Roles.Contains("Caster")) result.Uncertainty.Add("Caster training does not establish usable spellbooks, reagents, charges, transformations or macro decisions.");
            if (result.Bandages+result.HealingPotions+result.CurePotions==0) result.Uncertainty.Add("No counted carried recovery supplies; innate, spell and equipment recovery require separate audit.");
            result.Uncertainty.Add("Training band uses highest base skill, including noncombat skills; nominal level and role tags are not a measured combat rating.");
            return result;
        }
    }
}
