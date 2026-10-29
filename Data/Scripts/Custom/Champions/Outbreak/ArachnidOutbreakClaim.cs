using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;

namespace Server.Custom.Confictura.Champions
{
    public class BroodVenomGland : Item
    {
        [Constructable]
        public BroodVenomGland() : this(1) { }
        [Constructable]
        public BroodVenomGland(int amount) : base(0x23A)
        {
            Name = "brood venom gland";
            Hue = 0x48D;
            Stackable = true;
            Weight = 0.1;
            Amount = Math.Max(1, amount);
        }
        public BroodVenomGland(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Independent world objects preserve earned items when an encounter is
    // stopped/deleted. Claim transfers the existing item; it never rolls or
    // recreates a reward. Lifecycle + sealed stage prevent paid-stage replay.
    public class ArachnidOutbreakClaim : Bag
    {
        private static readonly List<ArachnidOutbreakClaim> m_Claims = new List<ArachnidOutbreakClaim>();
        private Mobile m_Owner;
        private string m_AttemptId;
        private int m_Stage;
        public Mobile Owner { get { return m_Owner; } }
        public string AttemptId { get { return m_AttemptId; } }
        public int Stage { get { return m_Stage; } }

        public static void Initialize()
        {
            CommandSystem.Register("OutbreakClaim", AccessLevel.Player, OnCommand);
        }

        [Usage("OutbreakClaim")]
        [Description("Collect your earned outbreak rewards when alive with backpack space.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || from.Deleted || !from.Player)
                return;
            int collected = 0;
            int pending = 0;
            foreach (ArachnidOutbreakClaim claim in m_Claims.ToArray())
                if (!claim.Deleted && claim.Owner == from)
                {
                    if (claim.TryClaim(from))
                        ++collected;
                    else
                        ++pending;
                }
            from.SendMessage("Outbreak rewards: {0} stage claims collected; {1} pending.", collected, pending);
        }

        public static ArachnidOutbreakClaim QueueGlands(Mobile owner, string attemptId, int stage)
        {
            if (owner == null || owner.Deleted || !owner.Player || String.IsNullOrEmpty(attemptId) || stage < 0 || stage > 3)
                throw new ArgumentException("Invalid outbreak reward entitlement.");
            foreach (ArachnidOutbreakClaim claim in m_Claims)
                if (!claim.Deleted && claim.Owner == owner && claim.AttemptId == attemptId && claim.Stage == stage)
                    return claim;
            ArachnidOutbreakClaim created = new ArachnidOutbreakClaim(owner, attemptId, stage);
            if (owner.NetState != null)
                owner.SendMessage("You earned an outbreak stage reward. Use [OutbreakClaim to collect it when ready.");
            return created;
        }

        private ArachnidOutbreakClaim(Mobile owner, string attemptId, int stage)
        {
            m_Owner = owner;
            m_AttemptId = attemptId;
            m_Stage = stage;
            Name = "pending outbreak reward";
            Visible = false;
            Movable = false;
            AddItem(new BroodVenomGland(stage == 3 ? 3 : 1));
            m_Claims.Add(this);
        }

        public ArachnidOutbreakClaim(Serial serial) : base(serial)
        {
            m_Claims.Add(this);
        }

        public bool TryClaim(Mobile from)
        {
            if (Deleted || from == null || from.Deleted || from != m_Owner || !from.Player || !from.Alive || from.Backpack == null)
                return false;
            // Successful stacking may delete the source item. TryDropItem's
            // result is authoritative; a failed transfer keeps its original parent.
            foreach (Item item in Items.ToArray())
                if (!from.Backpack.TryDropItem(from, item, false))
                    return false;
            Delete();
            return true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            TryClaim(from);
        }

        public override void OnAfterDelete()
        {
            m_Claims.Remove(this);
            base.OnAfterDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Owner);
            writer.Write(m_AttemptId);
            writer.Write(m_Stage);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_AttemptId = reader.ReadString();
            m_Stage = reader.ReadInt();
        }
    }
}
