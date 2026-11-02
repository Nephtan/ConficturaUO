using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public partial class ArachnidOutbreakController
    {
        // A collected claim is deleted, so its existence cannot serve as a paid-reward receipt.
        private List<Mobile> m_FrontierRewarded = new List<Mobile>();
        public int FrontierRewardRecipients { get { return m_FrontierRewarded.Count; } }

        private void AwardFrontierReclamation(ArachnidInfestationCreature creature)
        {
            if (!FrontierSuppliesEnabled) return;
            foreach (DamageStore rights in BaseCreature.GetLootingRights(creature.DamageEntries,creature.HitsMax))
            {
                Mobile player=rights.m_Mobile;
                if (!rights.m_HasRight || player==null || player.Deleted || !player.Player || player.AccessLevel!=AccessLevel.Player
                    || player.Map!=Map.Sosaria || !player.InRange(creature.Location,32) || m_FrontierRewarded.Contains(player)) continue;
                ArachnidOutbreakClaim reward=ArachnidOutbreakClaim.QueueReward(player,AttemptId,0,ArachnidOutbreakClaimKind.FrontierSupplies,CreateFrontierSupplies);
                if (reward!=null) m_FrontierRewarded.Add(player);
            }
        }

        private static Item CreateFrontierSupplies()
        {
            Bag bag=new Bag(); bag.Name="frontier reclamation supplies";
            bag.DropItem(new Bandage(20)); bag.DropItem(new LesserHealPotion()); bag.DropItem(new LesserCurePotion());
            return bag;
        }
    }
}
