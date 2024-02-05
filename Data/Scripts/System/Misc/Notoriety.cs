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
        public static bool Mobile_AllowBeneficial(Mobile from, Mobile target)
        {
            // If either the from or target mobile is null or has higher access level than a player,
            // then they cannot perform beneficial actions towards each other.
            if (
                from == null
                || target == null
                || from.AccessLevel > AccessLevel.Player
                || target.AccessLevel > AccessLevel.Player
            )
                return true;

            // Get the map where the from mobile is located.
            Map map = from.Map;

            #region Factions
            // Check if the target mobile belongs to a faction.
            Faction targetFaction = Faction.Find(target, true);

            // If the server is not running on the Mondain's Legacy expansion, and the from mobile is in the same
            // facet as the target mobile's faction, then they can perform beneficial actions towards each other
            if ((!Core.ML || map == Faction.Facet) && targetFaction != null)
            {
                if (Faction.Find(from, true) != targetFaction)
                    return false;
            }
            #endregion

            // NONPK players should be able to perform beneficial actions towards all NONPK or neutral players.
            PlayerMobile fromPlayer = from as PlayerMobile;
            PlayerMobile targetPlayer = target as PlayerMobile;

            // XmlPoints challenge mod
            if (XmlPoints.AreInAnyGame(target))
                return XmlPoints.AreTeamMembers(from, target);

            if (fromPlayer != null && fromPlayer.NONPK == NONPK.NONPK)
            {
                if (targetPlayer != null && targetPlayer.NONPK == NONPK.PK)
                    return false;
            }

            // If the map is not null and there are no beneficial restrictions on the map,
            // then the from mobile can perform beneficial actions towards the target mobile.
            if (map != null && (map.Rules & MapRules.BeneficialRestrictions) == 0)
                return true; // In felucca, anything goes

            // If the from mobile is not a player, then it can perform beneficial actions towards the target mobile.
            if (!from.Player)
                return true; // NPCs have no restrictions

            // If the target mobile is an uncontrolled creature, then the from player cannot heal it.
            if (target is BaseCreature && !((BaseCreature)target).Controlled)
                return false; // Players cannot heal uncontrolled mobiles

            // If the from mobile is a young player and the target mobile is an older player,
            // then the from player cannot perform beneficial actions towards the target player.
            if (
                from is PlayerMobile
                && ((PlayerMobile)from).Young
                && (!(target is PlayerMobile) || !((PlayerMobile)target).Young)
            )
                return false; // Young players cannot perform beneficial actions towards older players

            // If the from mobile and the target mobile are guild members or allies, then they can perform beneficial actions towards each other.
            Guild fromGuild = from.Guild as Guild;
            Guild targetGuild = target.Guild as Guild;

            if (
                fromGuild != null
                && targetGuild != null
                && (targetGuild == fromGuild || fromGuild.IsAlly(targetGuild))
            )
                return true; // Guild members can be beneficial

            if (PlayerGovernmentSystem.CheckIfBanned(from, target))
                return false;

            if (PlayerGovernmentSystem.CheckAtWarWith(from, target))
                return false;

            if (PlayerGovernmentSystem.CheckCityAlly(from, target))
                return true;

            // Otherwise, check the beneficial status between the from mobile and the target mobile.
            return CheckBeneficialStatus(GetGuildStatus(from), GetGuildStatus(target));
        }

        public static bool Mobile_AllowHarmful(Mobile attacker, Mobile target)
        {
            // If either the attacker or the target is null, harm is not allowed.
            if (attacker == null || target == null)
                return false;

            BaseCreature bcAttacker = attacker as BaseCreature;
            if (bcAttacker != null && bcAttacker.BardProvoked && bcAttacker.BardTarget == target)
                return true;

            // XmlPoints challenge mod
            if (XmlPoints.AreChallengers(attacker, target))
                return true;

            // Check if the attacker is a controlled creature and if so, set the controller.
            BaseCreature attackerCreature = attacker as BaseCreature;
            Mobile attackerController =
                attackerCreature != null
                && (attackerCreature.Controlled || attackerCreature.Summoned)
                    ? (attackerCreature.ControlMaster ?? attackerCreature.SummonMaster)
                    : null;

            // Check if the attacker or target is NONPK player.
            bool attackerIsNonPk =
                (
                    attackerController is PlayerMobile
                    && ((PlayerMobile)attackerController).NONPK == NONPK.NONPK
                ) || (attacker is PlayerMobile && ((PlayerMobile)attacker).NONPK == NONPK.NONPK);
            bool targetIsNonPk =
                target is PlayerMobile && ((PlayerMobile)target).NONPK == NONPK.NONPK;
            bool targetIsNeutral =
                target is PlayerMobile && ((PlayerMobile)target).NONPK == NONPK.Null;
            bool targetIsPk = target is PlayerMobile && ((PlayerMobile)target).NONPK == NONPK.PK;

            // If a NONPK player is trying to attack another player, display a message and disallow harm.
            if (attackerIsNonPk && (target is PlayerMobile && (targetIsPk || targetIsNeutral)))
            {
                (attackerController ?? attacker).SendMessage(
                    33,
                    "You have chosen the path of [PvE] and cannot attack players."
                );
                return false;
            }

            // If a NONPK player is being attacked by a player, display a message and disallow harm.
            if (targetIsNonPk && attacker is PlayerMobile)
            {
                (attackerController ?? attacker).SendMessage(
                    33,
                    "You cannot attack [PvE] players."
                );
                return false;
            }

            // If a controlled creature is trying to harm its master or another creature controlled by the same master, disallow harm.
            if (attackerCreature != null)
            {
                if (
                    attackerCreature.ControlMaster == target
                    || attackerCreature.SummonMaster == target
                )
                    return false;

                BaseCreature targetCreature = target as BaseCreature;
                if (targetCreature != null)
                {
                    Mobile targetController = targetCreature.Controlled
                        ? targetCreature.ControlMaster
                        : targetCreature.SummonMaster;
                    if (attackerController == targetController)
                        return false;
                }
            }

            if (PlayerGovernmentSystem.CheckIfBanned(attacker, target))
                return true;

            if (PlayerGovernmentSystem.CheckAtWarWith(attacker, target))
                return true;

            if (PlayerGovernmentSystem.CheckCityAlly(attacker, target))
                return true;

            // If none of the above conditions are met, allow harm.
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
