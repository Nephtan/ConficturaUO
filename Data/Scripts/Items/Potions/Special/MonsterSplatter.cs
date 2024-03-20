using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.Engines.XmlSpawner2;
using Server.Factions;
using Server.Guilds;

namespace Server.Items
{
    public class MonsterSplatter : Item
    {
        public Mobile owner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [Constructable]
        public MonsterSplatter(Mobile source)
            : base(0x122A)
        {
            Weight = 1.0;
            Movable = false;
            owner = source;
            Name = "splatter";
            ItemID = Utility.RandomList(
                0x122A,
                0x122A,
                0x122A,
                0x122B,
                0x122D,
                0x122E,
                0x263B,
                0x263C,
                0x263D,
                0x263E,
                0x263F,
                0x2640
            );
            ItemRemovalTimer thisTimer = new ItemRemovalTimer(this);
            thisTimer.Start();
        }

        public MonsterSplatter(Serial serial)
            : base(serial) { }

        public static int Hurt(Mobile m, int min, int max)
        {
            int v = 0;

            if (m is PlayerMobile)
            {
                int alchemySkill = (int)(Server.Items.BasePotion.EnhancePotions(m) / 5);
                v = Utility.RandomMinMax(min, max) + alchemySkill;
            }
            else
            {
                v = Utility.RandomMinMax(min, max);
            }

            return v;
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

        public bool CanDealDamageTo(Mobile from, Mobile target)
        {
            // Casts for player and creature checks
            BaseCreature bcAttacker = from as BaseCreature;
            BaseCreature bcTarget = target as BaseCreature;
            PlayerMobile pmAttacker = from as PlayerMobile;
            PlayerMobile pmTarget = target as PlayerMobile;
            Guild attackerGuild = from.Guild as Guild;
            Guild targetGuild = target.Guild as Guild;
            GuildStatus attackerGuildStatus = GetGuildStatus(from);
            GuildStatus targetGuildStatus = GetGuildStatus(target);

            // Pet ownership and control checks
            bool targetIsPet = bcTarget != null && (bcTarget.Controlled || bcTarget.Summoned);
            Mobile targetOwner = targetIsPet ? bcTarget.ControlMaster ?? bcTarget.SummonMaster : null;

            // Consensual PlayerMobile PvP System bools
            bool isAttackerNonPk = false, isTargetNonPk = false, isAttackerPk = false, isTargetPk = false, isAttackerNull = false, isTargetNull = false;

            if (pmAttacker != null)
            {
                isAttackerNonPk = pmAttacker.NONPK == NONPK.NONPK;
                isAttackerPk = pmAttacker.NONPK == NONPK.PK;
                isAttackerNull = pmAttacker.NONPK == NONPK.Null;
            }

            if (pmTarget != null)
            {
                isTargetNonPk = pmTarget.NONPK == NONPK.NONPK;
                isTargetPk = pmTarget.NONPK == NONPK.PK;
                isTargetNull = pmTarget.NONPK == NONPK.Null;
            }
            else if (targetOwner != null && targetOwner is PlayerMobile)
            {
                PlayerMobile ownerTarget = (PlayerMobile)targetOwner;
                isTargetNonPk = ownerTarget.NONPK == NONPK.NONPK;
                isTargetPk = ownerTarget.NONPK == NONPK.PK;
                isTargetNull = ownerTarget.NONPK == NONPK.Null;
            }

            if (pmAttacker == pmTarget || pmAttacker == targetOwner)
                return true;

            // Ensure that if from and target are opponents in an Xml Event they may harm each other
            if (XmlPoints.AreChallengers(from, target))
                return true;

            // Allow NONPK players to attack non-player mobiles (NPCs) or mobiles not owned by a PlayerMobile,
            // but not pets or summons owned by PlayerMobiles
            if (isAttackerNonPk && pmAttacker != null && bcTarget != null && !(targetOwner is PlayerMobile))
            {
                return true;
            }

            // Allow non-player mobiles (NPCs) or mobiles not owned by a PlayerMobile to attack all PlayerMobiles or their pets
            if (bcAttacker != null && (pmTarget != null || bcTarget != null))
            {
                return true;
            }

            // Prevent NONPK.NONPK players or their pets from initiating attacks on other players or their pets
            if (isAttackerNonPk && pmAttacker != null)
            {
                from.SendMessage(33, "You have chosen the path of [PvE] and cannot attack players or their pets.");
                return false;
            }

            // Prevent attacks on NONPK players or their pets from other players
            if (isTargetNonPk && pmAttacker != null)
            {
                from.SendMessage(33, "You cannot attack players or pets who have chosen the path of [PvE].");
                return false;
            }

            // Prevents pets of any status from attacking pets of NONPK.NONPK players
            if (targetIsPet && isTargetNonPk)
            {
                return false;
            }

            // Ensure city citizens and banned players can perform harmful actions on each other
            if (PlayerGovernmentSystem.CheckIfBanned(from, target))
                return true;

            // Ensure city citizens at war with other city citizens can perform harmful actions on each other
            if (PlayerGovernmentSystem.CheckAtWarWith(from, target))
                return true;

            // Ensure city citizens allied with other city citizens can perform harmful actions on each other
            if (PlayerGovernmentSystem.CheckCityAlly(from, target))
                return true;

            // Ensure guild members and allies of guild members can perform beneficial actions on each other
            if (attackerGuild != null && targetGuild != null && (attackerGuild == targetGuild || attackerGuild.IsAlly(targetGuild)))
                return true;

            // Ensure that if guild members are at war with each other that they cannot perform beneficial actions on each other
            if (!CheckBeneficialStatus(attackerGuildStatus, targetGuildStatus))
                return false;

            // Default to allow harm if none of the above conditions are met, including PK vs Null scenarios.
            return true;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (!CanDealDamageTo(this.owner, m))
            {
                return true; // Block damage and let the mobile move over.
            }

            if (
                m.Blessed
                || !m.Alive
                || (owner is BaseCreature && m is BaseCreature && !((BaseCreature)m).Controlled)
            )
            {
                return true;
            }

            SlayerEntry SilverSlayer = SlayerGroup.GetEntryByName(SlayerName.Silver);
            SlayerEntry ExorcismSlayer = SlayerGroup.GetEntryByName(SlayerName.Exorcism);

            switch (this.Name)
            {
                case "hot magma":
                    if (!(m is MagmaElemental))
                    {
                        HandleDamageEffect(m, 0x3709, 0x225, Hurt(owner, 24, 48), 0, 100);
                    }
                    break;

                case "quick silver":
                    HandleDamageEffect(
                        m,
                        soundId: 0x4D1,
                        damage: Hurt(owner, 24, 48),
                        phys: 50,
                        energy: 50
                    );
                    break;

                case "holy water":
                    if (SilverSlayer.Slays(m) || ExorcismSlayer.Slays(m))
                    {
                        HandleDamageEffect(
                            m,
                            0x3709,
                            0x225,
                            Hurt(owner, 40, 60),
                            20,
                            20,
                            20,
                            20,
                            20
                        );
                    }
                    break;

                case "glowing goo":
                    if (!(m is GlowBeetle) && !(m is GlowBeetleRiding))
                    {
                        int eSound =
                            (m is PlayerMobile && (m.Body == 0x190 || m.Body == 0x191))
                                ? (m.Body == 0x190 ? 0x43F : 0x32D)
                                : 0x229;

                        HandleDamageEffect(
                            m,
                            0x36B0,
                            eSound,
                            Hurt(owner, 24, 48),
                            energy: 50,
                            pois: 50
                        );
                    }
                    break;

                case "scorching ooze":
                    if (!(m is Lavapede))
                    {
                        int eSound =
                            (m.Body == 0x190 && m is PlayerMobile)
                                ? 0x43F
                                : (m.Body == 0x191 && m is PlayerMobile)
                                    ? 0x32D
                                    : 0x229;
                        HandleDamageEffect(m, 0x36B0, eSound, Hurt(owner, 24, 48), 0, 100);
                    }
                    break;

                case "blue slime":
                    if (!(m is SlimeDevil))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            cold: 100
                        );
                    }
                    break;

                case "swamp muck":
                    if (!(m is SwampThing))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            phys: 50,
                            energy: 50
                        );
                    }
                    break;

                case "poisonous slime":
                    if (!(m is AbyssCrawler))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            pois: 100
                        );
                    }
                    break;

                case "poison spit":
                    if (!(m is Neptar) && !(m is NeptarWizard))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            pois: 100
                        );
                    }
                    break;

                case "poison spittle":
                    if (!(m is Lurker))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            pois: 100
                        );
                    }
                    break;

                case "fungal slime":
                    if (!(m is Fungal) && !(m is FungalMage) && !(m is CreepingFungus))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            phys: 50,
                            energy: 50
                        );
                    }
                    break;

                case "spider ooze":
                    if (!(m is ZombieSpider))
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 24, 48),
                            phys: 50,
                            energy: 50
                        );
                    }
                    break;

                case "acidic slime":
                    if (!(m is ToxicElemental))
                    {
                        HandleDamageEffect(m, 0x231, Hurt(owner, 24, 48), 50);
                    }
                    break;

                case "acidic ichor":
                    if (
                        !(m is AntaurKing)
                        && !(m is AntaurProgenitor)
                        && !(m is AntaurSoldier)
                        && !(m is AntaurWorker)
                    )
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x231,
                            damage: Hurt(owner, 24, 48),
                            phys: 50,
                            energy: 50
                        );
                    }
                    break;

                case "thick blood":
                    if (!(m is BloodElemental) && !(m is BloodDemon))
                    {
                        int eSound = 0x229;
                        if (m.Body == 0x190 && m is PlayerMobile)
                        {
                            eSound = 0x43F;
                        }
                        else if (m.Body == 0x191 && m is PlayerMobile)
                        {
                            eSound = 0x32D;
                        }

                        HandleDamageEffect(
                            m,
                            particleId: 0x36B0,
                            soundId: eSound,
                            damage: Hurt(owner, 24, 48),
                            pois: 100
                        );
                    }
                    break;

                case "infected blood":
                    if (!(m is Infected))
                    {
                        int eSound = 0x229;
                        if (m.Body == 0x190 && m is PlayerMobile)
                        {
                            eSound = 0x43F;
                        }
                        else if (m.Body == 0x191 && m is PlayerMobile)
                        {
                            eSound = 0x32D;
                        }

                        HandleDamageEffect(m, 0x36B0, eSound, Hurt(owner, 24, 48), pois: 100);
                    }
                    break;

                case "alien blood":
                    if (!(m is Xenomorph) && !(m is Xenomutant))
                    {
                        int eSound = 0x229;
                        if (m.Body == 0x190 && m is PlayerMobile)
                        {
                            eSound = 0x43F;
                        }
                        else if (m.Body == 0x191 && m is PlayerMobile)
                        {
                            eSound = 0x32D;
                        }

                        HandleDamageEffect(
                            m,
                            0x36B0,
                            eSound,
                            Hurt(owner, 24, 48),
                            20,
                            20,
                            20,
                            20,
                            20
                        );
                    }
                    break;

                case "green blood":
                    if (!(m is ZombieGiant))
                    {
                        int eSound = 0x229;
                        if (m.Body == 0x190 && m is PlayerMobile)
                        {
                            eSound = 0x43F;
                        }
                        else if (m.Body == 0x191 && m is PlayerMobile)
                        {
                            eSound = 0x32D;
                        }

                        HandleDamageEffect(
                            m,
                            0x36B0,
                            eSound,
                            Hurt(owner, 24, 48),
                            phys: 20,
                            energy: 80
                        );
                    }
                    break;

                case "toxic blood":
                    if (!(m is Mutant))
                    {
                        int eSound = 0x229;
                        if (m.Body == 0x190 && m is PlayerMobile)
                        {
                            eSound = 0x43F;
                        }
                        else if (m.Body == 0x191 && m is PlayerMobile)
                        {
                            eSound = 0x32D;
                        }

                        HandleDamageEffect(m, 0x36B0, eSound, Hurt(owner, 24, 48), pois: 100);
                    }
                    break;

                case "freezing water":
                    if (
                        !(
                            m is WaterElemental
                            || m is WaterWeird
                            || m is DeepWaterElemental
                            || m is Dagon
                        )
                    )
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 20, 40),
                            cold: 100
                        );
                    }
                    break;

                case "deep water":
                    if (
                        !(
                            m is WaterElemental
                            || m is WaterWeird
                            || m is DeepWaterElemental
                            || m is Dagon
                        )
                    )
                    {
                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, 40, 60),
                            cold: 100
                        );
                    }
                    break;

                case "lesser poison potion":
                case "poison potion":
                case "greater poison potion":
                case "deadly poison potion":
                case "lethal poison potion":
                    {
                        int pSkill = (int)(owner.Skills[SkillName.Poisoning].Value / 50);
                        int tSkill = (int)(owner.Skills[SkillName.Tasting].Value / 33);
                        int aSkill = (int)(owner.Skills[SkillName.Alchemy].Value / 33);
                        int pMin = pSkill + tSkill + aSkill;
                        int pMax = pMin * 2;
                        Poison pois = Poison.Lesser;

                        switch (this.Name)
                        {
                            case "poison potion":
                                pMin += 2;
                                pMax += 2;
                                pois = Poison.Regular;
                                break;
                            case "greater poison potion":
                                pMin += 3;
                                pMax += 3;
                                pois = Poison.Greater;
                                break;
                            case "deadly poison potion":
                                pMin += 4;
                                pMax += 4;
                                pois = Poison.Deadly;
                                break;
                            case "lethal poison potion":
                                pMin += 5;
                                pMax += 5;
                                pois = Poison.Lethal;
                                break;
                        }

                        if (pMin >= Utility.RandomMinMax(1, 16))
                        {
                            m.ApplyPoison(owner, pois);
                        }

                        HandleDamageEffect(
                            m,
                            soundId: 0x4D1,
                            damage: Hurt(owner, pMin, pMax),
                            pois: 100
                        );
                        break;
                    }

                case "liquid fire":
                    int liqMinFire = Server.Items.BaseLiquid.GetLiquidBonus(owner);
                    int liqMaxFire = liqMinFire * 2;

                    HandleDamageEffect(
                        m,
                        0x3709,
                        0x208,
                        Hurt(owner, liqMinFire, liqMaxFire),
                        phys: 20,
                        fire: 80
                    );
                    break;

                case "liquid goo":
                    int liqMinGoo = Server.Items.BaseLiquid.GetLiquidBonus(owner);
                    int liqMaxGoo = liqMinGoo * 2;

                    HandleDamageEffect(
                        m,
                        Utility.RandomList(0x3967, 0x3979),
                        0x5C3,
                        Hurt(owner, liqMinGoo, liqMaxGoo),
                        energy: 80
                    );
                    break;

                case "liquid ice":
                    int liqMinIce = Server.Items.BaseLiquid.GetLiquidBonus(owner);
                    int liqMaxIce = liqMinIce * 2;

                    HandleDamageEffect(
                        m,
                        0x1A84,
                        0x10B,
                        Hurt(owner, liqMinIce, liqMaxIce),
                        cold: 80
                    );
                    break;

                case "liquid rot":
                    int liqMinRot = Server.Items.BaseLiquid.GetLiquidBonus(owner);
                    int liqMaxRot = liqMinRot * 2;

                    HandleDamageEffect(
                        m,
                        0x3400,
                        0x108,
                        Hurt(owner, liqMinRot, liqMaxRot),
                        20,
                        0,
                        0,
                        80,
                        0
                    );
                    break;

                case "liquid pain":
                    int liqMinPain = Server.Items.BaseLiquid.GetLiquidBonus(owner);
                    int liqMaxPain = liqMinPain * 2;

                    m.FixedParticles(0x37C4, 1, 8, 9916, 39, 3, EffectLayer.Head);
                    m.FixedParticles(0x37C4, 1, 8, 9502, 39, 4, EffectLayer.Head);
                    m.PlaySound(0x210);

                    HandleDamageEffect(
                        m,
                        damage: Hurt(owner, liqMinPain, liqMaxPain),
                        phys: 80,
                        fire: 5,
                        cold: 5,
                        pois: 5,
                        energy: 5
                    );
                    break;
            }

            return true;
        }

        private void HandleDamageEffect(
            Mobile m,
            int particleId = 0,
            int soundId = 0,
            int damage = 0,
            int phys = 0,
            int fire = 0,
            int cold = 0,
            int pois = 0,
            int energy = 0
        )
        {
            if (!CanDealDamageTo(this.owner, m))
            {
                return; // Block damage and exit the method.
            }

            owner.DoHarmful(m);

            if (particleId != 0)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                    particleId,
                    10,
                    30,
                    5052
                );
            }

            if (soundId != 0)
            {
                Effects.PlaySound(m.Location, m.Map, soundId);
            }

            if (damage != 0)
            {
                AOS.Damage(m, owner, damage, phys, fire, cold, pois, energy);
            }
        }

        public static void AddSplatter(
            int iX,
            int iY,
            int iZ,
            Map iMap,
            Point3D iLoc,
            Mobile source,
            string description,
            int color,
            int glow
        )
        {
            Effects.PlaySound(iLoc, iMap, 0x026);

            double weight = 1.0;
            if (glow > 0)
            {
                weight = 2.0;
            }

            MonsterSplatter Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX - 2), (iY - 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX - 1), (iY - 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX - 1), iY, iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX - 1), (iY + 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D(iX, (iY + 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX + 1), (iY + 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX + 1), iY, iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX + 1), (iY - 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D(iX, (iY - 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX + 1), (iY - 2), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX + 2), (iY - 2), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX - 2), (iY + 1), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX - 2), (iY + 2), iZ), iMap);
            Spill = new MonsterSplatter(source);
            Spill.Name = description;
            Spill.Hue = color;
            Spill.Weight = weight;
            Spill.MoveToWorld(new Point3D((iX + 1), (iY + 2), iZ), iMap);

            if (glow > 0)
            {
                StrangeGlow Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX - 2), (iY - 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX - 1), (iY - 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX - 1), iY, iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX - 1), (iY + 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D(iX, (iY + 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX + 1), (iY + 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX + 1), iY, iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX + 1), (iY - 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D(iX, (iY - 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX + 1), (iY - 2), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX + 2), (iY - 2), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX - 2), (iY + 1), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX - 2), (iY + 2), iZ), iMap);
                Glow = new StrangeGlow();
                Glow.Name = description;
                Glow.MoveToWorld(new Point3D((iX + 1), (iY + 2), iZ), iMap);
            }
        }

        public static bool TooMuchSplatter(Mobile from)
        {
            int splatter = 0;

            foreach (Item i in from.GetItemsInRange(10))
            {
                if (i is MonsterSplatter)
                {
                    MonsterSplatter splat = (MonsterSplatter)i;
                    if (splat.owner != from)
                        splatter++;
                }
            }

            if (splatter > 16)
            {
                return true;
            }

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Mobile)owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            owner = reader.ReadMobile();
            this.Delete(); // none when the world starts
        }

        public class ItemRemovalTimer : Timer
        {
            private Item i_item;

            public ItemRemovalTimer(Item item)
                : base(TimeSpan.FromSeconds(30.0))
            {
                Priority = TimerPriority.OneSecond;
                i_item = item;
            }

            protected override void OnTick()
            {
                if ((i_item != null) && (!i_item.Deleted))
                {
                    i_item.Delete();
                }
            }
        }
    }
}
