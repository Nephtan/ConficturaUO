using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.PartySystem;
using Server.Engines.XmlSpawner2;
using Server.Factions;
using Server.Guilds;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;

namespace Server.Misc
{
    public class NotorietyHandlers
    {
        public static void Initialize()
        {
            Notoriety.Hues[Notoriety.Innocent] = 0x59;
            Notoriety.Hues[Notoriety.Ally] = 0x3F;
            Notoriety.Hues[Notoriety.CanBeAttacked] = 0x3B2;
            Notoriety.Hues[Notoriety.Criminal] = 0x3B2;
            Notoriety.Hues[Notoriety.Enemy] = 0x90;
            Notoriety.Hues[Notoriety.Murderer] = 0x22;
            Notoriety.Hues[Notoriety.Invulnerable] = 0x35;

            Notoriety.Handler = new NotorietyHandler(MobileNotoriety);

            Mobile.AllowBeneficialHandler = new AllowBeneficialHandler(Mobile_AllowBeneficial);
            Mobile.AllowHarmfulHandler = new AllowHarmfulHandler(Mobile_AllowHarmful);
        }

        private enum GuildStatus
        {
            None,
            Peaceful,
            Waring
        }

        private static GuildStatus GetGuildStatus(Mobile m)
        {
            if (m.Guild == null)
                return GuildStatus.None;
            else if (((Guild)m.Guild).Enemies.Count == 0 && m.Guild.Type == GuildType.Regular)
                return GuildStatus.Peaceful;

            return GuildStatus.Waring;
        }

        private static bool CheckBeneficialStatus(GuildStatus from, GuildStatus target)
        {
            if (from == GuildStatus.Waring || target == GuildStatus.Waring)
                return false;

            return true;
        }

        // This method determines if one mobile can perform a beneficial action towards another mobile.
        public static bool Mobile_AllowBeneficial(Mobile from, Mobile target) // Line 62
        {
            // Validate null references
            if (from == null || target == null)
                return false;

            // Validate AccessLevels
            if (from.AccessLevel > AccessLevel.Player)
                return true; // Staff can always perform beneficial actions

            if (target.AccessLevel > AccessLevel.Player)
                return false; // Beneficial actions cannot be performed on staff

            // Ensure both mobiles are on a valid map
            Map map = from.Map;
            if (map == null)
                return false;

            // Check for NPCs first as they have fewer restrictions
            if (!from.Player)
                return true; // NPCs can perform beneficial actions towards any target

            // Prevent beneficial actions towards uncontrolled creatures by players
            BaseCreature creature = target as BaseCreature;
            if (creature != null && !creature.Controlled)
                return false;

            // XmlPoints and NONPK checks follow, specific to player interactions
            PlayerMobile fromPlayer = from as PlayerMobile;
            PlayerMobile targetPlayer = target as PlayerMobile;
            if (fromPlayer != null && targetPlayer != null)
            {
                if (XmlPoints.AreInAnyGame(target))
                {
                    return XmlPoints.AreTeamMembers(from, target); // Directly return the team member status
                }

                if (fromPlayer.NONPK == NONPK.NONPK && targetPlayer.NONPK == NONPK.PK)
                    return false;
            }

            // Player Government System checks for social dynamics
            if (PlayerGovernmentSystem.CheckIfBanned(from, target) || PlayerGovernmentSystem.CheckAtWarWith(from, target))
                return false;
            if (PlayerGovernmentSystem.CheckCityAlly(from, target))
                return true;

            // Guild and allies check
            Guild fromGuild = from.Guild as Guild;
            Guild targetGuild = target.Guild as Guild;
            if (fromGuild != null && targetGuild != null && (fromGuild == targetGuild || fromGuild.IsAlly(targetGuild)))
                return true;

            // Use the GetGuildStatus and CheckBeneficialStatus methods to evaluate guild status
            GuildStatus fromGuildStatus = GetGuildStatus(from);
            GuildStatus targetGuildStatus = GetGuildStatus(target);
            if (!CheckBeneficialStatus(fromGuildStatus, targetGuildStatus))
                return false;

            // Map rules check to allow beneficial actions where no restrictions exist
            if ((map.Rules & MapRules.BeneficialRestrictions) == 0)
                return true;

            return true;
        }

        public static bool Mobile_AllowHarmful(Mobile attacker, Mobile target)
        {
            // Validate null references
            if (attacker == null || target == null)
                return false;

            // Casts are moved up and done once to avoid redundancy.
            BaseCreature bcAttacker = attacker as BaseCreature;
            BaseCreature bcTarget = target as BaseCreature;
            PlayerMobile pmAttacker = attacker as PlayerMobile;
            PlayerMobile pmTarget = target as PlayerMobile;

            // Determine the attacker's controller for pets or summoned creatures early.
            Mobile attackerController = bcAttacker != null && (bcAttacker.Controlled || bcAttacker.Summoned)
                ? bcAttacker.ControlMaster ?? bcAttacker.SummonMaster
                : null;

            // Check for bard provoked creatures first.
            if (bcAttacker != null && bcAttacker.BardProvoked && bcAttacker.BardTarget == target)
                return true;

            // XmlPoints challenge mod
            if (XmlPoints.AreChallengers(attacker, target))
                return true;

            // Handle NONPK logic for attacking pets
            if (bcTarget != null)
            {
                PlayerMobile petOwner = bcTarget.ControlMaster as PlayerMobile;
                if (petOwner != null && pmAttacker != null)
                {
                    // Prevent NONPK.NONPK players from attacking pets of any player
                    if (pmAttacker.NONPK == NONPK.NONPK)
                    {
                        (attackerController ?? attacker).SendMessage(33, "You have chosen the path of [PvE] and cannot attack players or their pets."); // Line 157
                        return false;
                    }

                    // Prevent NONPK.PK players from attacking pets of NONPK.NONPK players
                    if (pmAttacker.NONPK == NONPK.PK && petOwner.NONPK == NONPK.NONPK)
                    {
                        attacker.SendMessage(33, "You cannot attack [PvE] players or their pets.");
                        return false;
                    }

                    // Prevent NONPK.Null players from attacking pets of NONPK.NONPK players
                    if (pmAttacker.NONPK == NONPK.Null && petOwner.NONPK == NONPK.NONPK)
                    {
                        attacker.SendMessage(33, "You cannot attack [PvE] players or their pets.");
                        return false;
                    }
                }
            }

            // Consolidate NONPK checks by using the previously cast PlayerMobile instances.
            bool attackerIsNonPk = (pmAttacker != null && pmAttacker.NONPK == NONPK.NONPK) || (attackerController != null && (attackerController as PlayerMobile) != null && (attackerController as PlayerMobile).NONPK == NONPK.NONPK);
            bool targetIsNonPk = pmTarget != null && pmTarget.NONPK == NONPK.NONPK;

            // Prevent NONPK.NONPK players from attacking any players or their pets
            if (attackerIsNonPk)
            {
                (attackerController ?? attacker).SendMessage(33, "You have chosen the path of [PvE] and cannot attack players or their pets.");
                return false;
            }

            // Prevent attacks on NONPK.NONPK players or their pets by any player
            if (targetIsNonPk)
            {
                attacker.SendMessage(33, "You cannot attack [PvE] players or their pets.");
                return false;
            }

            // Checks for controlled creatures trying to harm their master or others controlled by the same master.
            if (bcAttacker != null && (bcAttacker.ControlMaster == target || bcAttacker.SummonMaster == target))
                return false;

            if (bcAttacker != null && bcTarget != null && attackerController == (bcTarget.ControlMaster ?? bcTarget.SummonMaster))
                return false;

            // Player government system checks remain unchanged as their logic is specific and likely requires no optimization without more context.
            if (PlayerGovernmentSystem.CheckIfBanned(attacker, target) || PlayerGovernmentSystem.CheckAtWarWith(attacker, target) || PlayerGovernmentSystem.CheckCityAlly(attacker, target))
                return true;

            // Default to allow harm if none of the above conditions are met.
            return true;
        }

        public static Guild GetGuildFor(Guild def, Mobile m)
        {
            Guild g = def;

            BaseCreature c = m as BaseCreature;

            if (c != null && c.Controlled && c.ControlMaster != null)
            {
                c.DisplayGuildTitle = false;

                if (
                    c.Map != Map.Internal
                    && (
                        Core.AOS
                        || Guild.NewGuildSystem
                        || c.ControlOrder == OrderType.Attack
                        || c.ControlOrder == OrderType.Guard
                    )
                )
                    g = (Guild)(c.Guild = c.ControlMaster.Guild);
                else if (c.Map == Map.Internal || c.ControlMaster.Guild == null)
                    g = (Guild)(c.Guild = null);
            }

            return g;
        }

        public static int CorpseNotoriety(Mobile source, Corpse target)
        {
            if (target.AccessLevel > AccessLevel.Player)
                return Notoriety.CanBeAttacked;

            Body body = (Body)target.Amount;

            BaseCreature cretOwner = target.Owner as BaseCreature;

            if (cretOwner != null)
            {
                Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
                Guild targetGuild = GetGuildFor(target.Guild as Guild, target.Owner);

                if (sourceGuild != null && targetGuild != null)
                {
                    if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                        return Notoriety.Ally;
                    else if (sourceGuild.IsEnemy(targetGuild))
                        return Notoriety.Enemy;
                }

                Faction srcFaction = Faction.Find(source, true, true);
                Faction trgFaction = Faction.Find(target.Owner, true, true);

                if (
                    srcFaction != null
                    && trgFaction != null
                    && srcFaction != trgFaction
                    && source.Map == Faction.Facet
                )
                    return Notoriety.Enemy;

                if (CheckHouseFlag(source, target.Owner, target.Location, target.Map))
                    return Notoriety.CanBeAttacked;

                int actual = Notoriety.CanBeAttacked;

                if (
                    target.Kills >= 1
                    || (body.IsMonster && IsSummoned(target.Owner as BaseCreature))
                    || (
                        target.Owner is BaseCreature
                        && (
                            ((BaseCreature)target.Owner).AlwaysMurderer
                            || ((BaseCreature)target.Owner).IsAnimatedDead
                        )
                    )
                )
                    actual = Notoriety.Murderer;

                if (DateTime.Now >= (target.TimeOfDeath + Corpse.MonsterLootRightSacrifice))
                    return actual;

                Party sourceParty = Party.Get(source);

                List<Mobile> list = target.Aggressors;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (
                        list[i] == source
                        || (sourceParty != null && Party.Get(list[i]) == sourceParty)
                    )
                        return actual;
                }

                if (PlayerGovernmentSystem.CheckBanLootable(source, target.Owner))
                    return Notoriety.Enemy;

                if (PlayerGovernmentSystem.CheckAtWarWith(source, target.Owner))
                    return Notoriety.Enemy;

                if (PlayerGovernmentSystem.CheckCityAlly(source, target.Owner))
                    return Notoriety.Ally;

                return Notoriety.Innocent;
            }
            else
            {
                if (
                    target.Kills >= 1
                    || (body.IsMonster && IsSummoned(target.Owner as BaseCreature))
                    || (
                        target.Owner is BaseCreature
                        && (
                            ((BaseCreature)target.Owner).AlwaysMurderer
                            || ((BaseCreature)target.Owner).IsAnimatedDead
                        )
                    )
                )
                    return Notoriety.Murderer;

                if (
                    target.Criminal
                    && target.Map != null
                    && ((target.Map.Rules & MapRules.HarmfulRestrictions) == 0)
                )
                    return Notoriety.Criminal;

                Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
                Guild targetGuild = GetGuildFor(target.Guild as Guild, target.Owner);

                if (sourceGuild != null && targetGuild != null)
                {
                    if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                        return Notoriety.Ally;
                    else if (sourceGuild.IsEnemy(targetGuild))
                        return Notoriety.Enemy;
                }

                Faction srcFaction = Faction.Find(source, true, true);
                Faction trgFaction = Faction.Find(target.Owner, true, true);

                if (
                    srcFaction != null
                    && trgFaction != null
                    && srcFaction != trgFaction
                    && source.Map == Faction.Facet
                )
                {
                    List<Mobile> secondList = target.Aggressors;

                    for (int i = 0; i < secondList.Count; ++i)
                    {
                        if (secondList[i] == source || secondList[i] is BaseFactionGuard)
                            return Notoriety.Enemy;
                    }
                }

                if (
                    target.Owner != null
                    && target.Owner is BaseCreature
                    && ((BaseCreature)target.Owner).AlwaysAttackable
                )
                    return Notoriety.CanBeAttacked;

                if (CheckHouseFlag(source, target.Owner, target.Location, target.Map))
                    return Notoriety.CanBeAttacked;

                if (!(target.Owner is PlayerMobile) && !IsPet(target.Owner as BaseCreature))
                    return Notoriety.CanBeAttacked;

                List<Mobile> list = target.Aggressors;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i] == source)
                        return Notoriety.CanBeAttacked;
                }

                if (PlayerGovernmentSystem.CheckBanLootable(source, target.Owner))
                    return Notoriety.Enemy;

                if (PlayerGovernmentSystem.CheckAtWarWith(source, target.Owner))
                    return Notoriety.Enemy;

                if (PlayerGovernmentSystem.CheckCityAlly(source, target.Owner))
                    return Notoriety.Ally;

                return Notoriety.Innocent;
            }
        }

        public static int MobileNotoriety(Mobile source, Mobile target)
        {
            if (
                Core.AOS
                && (
                    target.Blessed
                    || (target is BaseVendor && ((BaseVendor)target).IsInvulnerable)
                    || target is PlayerVendor
                    || target is PlayerBarkeeper
                )
            )
                return Notoriety.Invulnerable;

            if (
                target is BasePerson
                || target is BaseNPC
                || (target is BaseVendor && target.RaceID > 0)
            )
                return Notoriety.Innocent;

            if (target.AccessLevel > AccessLevel.Player)
                return Notoriety.CanBeAttacked;

            if (source.Player && !target.Player && source is PlayerMobile && target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;

                Mobile master = bc.GetMaster();

                if (master != null && master.AccessLevel > AccessLevel.Player)
                    return Notoriety.CanBeAttacked;

                master = bc.ControlMaster;

                if (Core.ML && master != null)
                {
                    if (
                        (source == master && CheckAggressor(target.Aggressors, source))
                        || (CheckAggressor(source.Aggressors, bc))
                    )
                        return Notoriety.CanBeAttacked;
                    else
                        return MobileNotoriety(source, master);
                }

                if (
                    !bc.Summoned
                    && !bc.Controlled
                    && ((PlayerMobile)source).EnemyOfOneType == target.GetType()
                )
                    return Notoriety.Enemy;
            }

            //###############ADDED FOR NONPK SO NONPK CANT ATTACK CRIMINALS OR REDS##################
            if (source is PlayerMobile && target is PlayerMobile)
            {
                var sourcePlayer = (PlayerMobile)source;
                var targetPlayer = (PlayerMobile)target;

                if (sourcePlayer.NONPK == NONPK.NONPK || targetPlayer.NONPK == NONPK.NONPK)
                {
                    return Notoriety.Invulnerable;
                }
            }
            //#################ADDED FOR NONPK SO NONPK CANT ATTACK CRIMINALS OR REDS###############

            if (
                target.Kills >= 1
                || (
                    target.Body.IsMonster
                    && IsSummoned(target as BaseCreature)
                    && !(target is BaseFamiliar)
                    && !(target is Golem)
                )
                || (
                    target is BaseCreature
                    && (
                        ((BaseCreature)target).AlwaysMurderer
                        || ((BaseCreature)target).IsAnimatedDead
                    )
                )
            )
                return Notoriety.Murderer;

            if (target.Criminal)
                return Notoriety.Criminal;

            // XmlPoints challenge mod
            if (XmlPoints.AreTeamMembers(source, target))
                return Notoriety.Ally;
            else if (XmlPoints.AreChallengers(source, target))
                return Notoriety.Enemy;

            Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
            Guild targetGuild = GetGuildFor(target.Guild as Guild, target);

            if (sourceGuild != null && targetGuild != null)
            {
                if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                    return Notoriety.Ally;
                else if (sourceGuild.IsEnemy(targetGuild))
                    return Notoriety.Enemy;
            }

            Faction srcFaction = Faction.Find(source, true, true);
            Faction trgFaction = Faction.Find(target, true, true);

            if (
                srcFaction != null
                && trgFaction != null
                && srcFaction != trgFaction
                && source.Map == Faction.Facet
            )
                return Notoriety.Enemy;

            if (
                SkillHandlers.Stealing.ClassicMode
                && target is PlayerMobile
                && ((PlayerMobile)target).PermaFlags.Contains(source)
            )
                return Notoriety.CanBeAttacked;

            if (target is BaseCreature && ((BaseCreature)target).AlwaysAttackable)
                return Notoriety.CanBeAttacked;

            if (CheckHouseFlag(source, target, target.Location, target.Map))
                return Notoriety.CanBeAttacked;

            if (!(target is BaseCreature && ((BaseCreature)target).InitialInnocent)) //If Target is NOT A baseCreature, OR it's a BC and the BC is initial innocent...
            {
                if (
                    !target.Body.IsHuman
                        && !target.Body.IsGhost
                        && !IsPet(target as BaseCreature)
                        && !(target is PlayerMobile)
                    || !Core.ML
                        && !target.CanBeginAction(typeof(Server.Spells.Seventh.PolymorphSpell))
                )
                    return Notoriety.CanBeAttacked;
            }

            if (CheckAggressor(source.Aggressors, target))
                return Notoriety.CanBeAttacked;

            if (CheckAggressed(source.Aggressed, target))
                return Notoriety.CanBeAttacked;

            if (target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;

                if (
                    bc.Controlled
                    && bc.ControlOrder == OrderType.Guard
                    && bc.ControlTarget == source
                )
                    return Notoriety.CanBeAttacked;
            }

            if (source is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)source;

                Mobile master = bc.GetMaster();
                if (master != null)
                    if (
                        CheckAggressor(master.Aggressors, target)
                        || MobileNotoriety(master, target) == Notoriety.CanBeAttacked
                    )
                        return Notoriety.CanBeAttacked;
            }

            if (PlayerGovernmentSystem.CheckIfBanned(source, target))
                return Notoriety.Enemy;

            if (PlayerGovernmentSystem.CheckAtWarWith(source, target))
                return Notoriety.Enemy;

            if (PlayerGovernmentSystem.CheckCityAlly(source, target))
                return Notoriety.Ally;

            return Notoriety.Innocent;
        }

        public static bool CheckHouseFlag(Mobile from, Mobile m, Point3D p, Map map)
        {
            BaseHouse house = BaseHouse.FindHouseAt(p, map, 16);

            if (house == null || house.Public || !house.IsFriend(from))
                return false;

            if (m != null && house.IsFriend(m))
                return false;

            BaseCreature c = m as BaseCreature;

            if (c != null && !c.Deleted && c.Controlled && c.ControlMaster != null)
                return !house.IsFriend(c.ControlMaster);

            return true;
        }

        public static bool IsPet(BaseCreature c)
        {
            return (c != null && c.Controlled);
        }

        public static bool IsSummoned(BaseCreature c)
        {
            return (
                c != null
                && /*c.Controlled &&*/
                c.Summoned
            );
        }

        public static bool CheckAggressor(List<AggressorInfo> list, Mobile target)
        {
            for (int i = 0; i < list.Count; ++i)
                if (list[i].Attacker == target)
                    return true;

            return false;
        }

        public static bool CheckAggressed(List<AggressorInfo> list, Mobile target)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                AggressorInfo info = list[i];

                if (!info.CriminalAggression && info.Defender == target)
                    return true;
            }

            return false;
        }
    }
}
