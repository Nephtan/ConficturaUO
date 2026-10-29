using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Engines.Craft;
using Server.Engines.PartySystem;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Custom.Confictura.Champions
{
    public class BroodAntivenom : Item, ICraftable
    {
        [Constructable]
        public BroodAntivenom() : base(0xF0D)
        {
            Name = "brood antivenom";
            Hue = 0x48D;
            Weight = 1.0;
            Stackable = true;
        }
        public BroodAntivenom(Serial serial) : base(serial) { }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            if (from == null || from.Deleted || Deleted)
                return false;
            if (item == this && (from.Backpack == null || !IsChildOf(from.Backpack)))
            {
                from.SendLocalizedMessage(1042038);
                return false;
            }
            return base.CheckItemUse(from, item);
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Deleted || !from.Alive || Deleted || from.Backpack == null || !IsChildOf(from.Backpack))
                return;
            from.SendMessage("Apply the antivenom to yourself, a party ally, or a controlled pet belonging to either.");
            from.Target = new ApplyTarget(this);
        }
        private sealed class ApplyTarget : Target
        {
            private readonly BroodAntivenom m_Treatment;
            public ApplyTarget(BroodAntivenom treatment) : base(2, false, TargetFlags.Beneficial) { m_Treatment = treatment; }
            protected override void OnTarget(Mobile from, object targeted)
            {
                string reason;
                m_Treatment.TryApply(from, targeted as Mobile, out reason);
                if (from != null && !from.Deleted)
                    from.SendMessage(reason);
            }
        }

        public bool TryApply(Mobile from, Mobile recipient, out string reason)
        {
            reason = "You cannot apply that treatment.";
            if (Deleted || from == null || from.Deleted || !from.Alive || from.Backpack == null || !IsChildOf(from.Backpack)
                || recipient == null || recipient.Deleted || !recipient.Alive || recipient.IsDeadBondedPet
                || from.Map == null || from.Map == Map.Internal || from.Map != recipient.Map
                || !from.InRange(recipient.Location, 2) || !from.InLOS(recipient) || !from.CanBeBeneficial(recipient, false))
                return false;
            Mobile principal = recipient;
            BaseCreature pet = recipient as BaseCreature;
            if (!recipient.Player)
            {
                if (pet == null || !pet.Controlled || pet.ControlMaster == null)
                    return false;
                principal = pet.ControlMaster;
            }
            Party party = Party.Get(from);
            if (principal != from && (party == null || !party.Contains(principal)))
            {
                reason = "That recipient must belong to your party.";
                return false;
            }
            if (TransformationSpellHelper.UnderTransformation(recipient, typeof(Server.Spells.Necromancy.VampiricEmbraceSpell)))
            {
                reason = "The garlic in the treatment would surely kill the recipient.";
                return false;
            }
            int count;
            BroodAntivenomEffect effect = BroodAntivenomEffect.Find(recipient, out count);
            DateTime now = DateTime.UtcNow;
            if (count > 1)
            {
                reason = "The recipient's treatment state needs staff review.";
                return false;
            }
            if (effect != null && effect.Active(now))
            {
                reason = "That recipient already has active antivenom protection.";
                return false;
            }
            if (effect != null && effect.NextUse > now)
            {
                reason = "That recipient must wait before another treatment.";
                return false;
            }
            IDisposable scope = TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(from, recipient, TurnMutationKind.Effect, 0, 3));
            if (scope == null)
            {
                reason = "You cannot apply that treatment at this time.";
                return false;
            }
            try
            {
                if (effect != null)
                    effect.Delete();
                new BroodAntivenomEffect(recipient, from);
                from.DoBeneficial(recipient);
                Consume(1);
                from.AddToBackpack(new Bottle());
                recipient.PlaySound(0x2D6);
                recipient.SendMessage("Brood antivenom: three deadly/lethal wild-poison protections for up to five minutes. Use [BroodAntivenom for status.");
                if (pet != null && pet.ControlMaster != null && pet.ControlMaster != from)
                    pet.ControlMaster.SendMessage("Brood antivenom on {0}: three protections for up to five minutes. Use [BroodAntivenom for status.", pet.Name);
                reason = "Applied brood antivenom. It does not cure existing poison.";
                return true;
            }
            finally { scope.Dispose(); }
        }

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            Amount = 2;
            return 1;
        }
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

    public class BroodAntivenomEffect : Item
    {
        private static readonly List<BroodAntivenomEffect> m_Effects = new List<BroodAntivenomEffect>();
        private static Timer m_Cleanup;
        private Mobile m_Owner, m_AppliedBy;
        private int m_Charges;
        private DateTime m_Expires, m_NextUse;
        public Mobile Owner { get { return m_Owner; } }
        public Mobile AppliedBy { get { return m_AppliedBy; } }
        public int Charges { get { return m_Charges; } }
        public DateTime Expires { get { return m_Expires; } }
        public DateTime NextUse { get { return m_NextUse; } }
        public bool Active(DateTime now) { return !Deleted && m_Charges > 0 && now < m_Expires; }

        public static void Initialize()
        {
            EventSink.PoisonPrevention -= OnPrevention;
            EventSink.PoisonPrevention += OnPrevention;
            CommandSystem.Register("BroodAntivenom", AccessLevel.Player, OnStatus);
            if (m_Cleanup == null)
                m_Cleanup = Timer.DelayCall(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), Cleanup);
            Cleanup();
        }
        [Usage("BroodAntivenom")]
        [Description("Show antivenom charges, expiry and reuse delay for yourself and your controlled pets.")]
        private static void OnStatus(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || from.Deleted || !from.Player)
                return;
            from.CloseGump(typeof(BroodAntivenomStatus));
            from.SendGump(new BroodAntivenomStatus(from));
        }
        public static BroodAntivenomEffect Find(Mobile owner, out int count)
        {
            count = 0;
            BroodAntivenomEffect found = null;
            foreach (BroodAntivenomEffect effect in m_Effects)
                if (!effect.Deleted && effect.Owner == owner)
                {
                    found = effect;
                    ++count;
                }
            return count == 1 ? found : null;
        }
        private static void Cleanup()
        {
            DateTime now = DateTime.UtcNow;
            foreach (BroodAntivenomEffect effect in m_Effects.ToArray())
                if (effect.Owner == null || effect.Owner.Deleted || (!effect.Active(now) && now >= effect.NextUse))
                    effect.Delete();
        }
        private static void OnPrevention(PoisonPreventionEventArgs e)
        {
            BaseCreature source = e.Source as BaseCreature;
            if (e.Prevented || e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive || e.Mobile.IsDeadBondedPet
                || e.Poison == null || (e.Poison.Level != 3 && e.Poison.Level != 4)
                || source == null || source.Player || source.Controlled || source.Summoned || source.BardProvoked)
                return;
            int count;
            BroodAntivenomEffect effect = Find(e.Mobile, out count);
            if (effect == null || !effect.Active(DateTime.UtcNow))
                return;
            --effect.m_Charges;
            e.Prevent();
            e.Mobile.SendMessage("Brood antivenom prevented poison: {0} protection(s) remain. Use [BroodAntivenom for time and reuse status.", effect.Charges);
            BaseCreature pet = e.Mobile as BaseCreature;
            if (pet != null && pet.ControlMaster != null)
                pet.ControlMaster.SendMessage("Antivenom on {0}: {1} protection(s) remain.", pet.Name, effect.Charges);
            ArachnidOutbreakController.RecordPoisonPrevention(e.Mobile, e.Source, effect.AppliedBy);
        }

        public BroodAntivenomEffect(Mobile owner, Mobile appliedBy) : base(0x1F14)
        {
            m_Owner = owner; m_AppliedBy = appliedBy;
            m_Charges = 3;
            m_Expires = DateTime.UtcNow.AddMinutes(5);
            m_NextUse = DateTime.UtcNow.AddMinutes(2);
            Visible = false; Movable = false; Name = "brood antivenom effect";
            m_Effects.Add(this);
        }
        public BroodAntivenomEffect(Serial serial) : base(serial) { m_Effects.Add(this); }
        public override void OnAfterDelete()
        {
            m_Effects.Remove(this);
            base.OnAfterDelete();
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Owner); writer.Write(m_AppliedBy);
            writer.Write(m_Charges); writer.Write(m_Expires); writer.Write(m_NextUse);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile(); m_AppliedBy = reader.ReadMobile();
            m_Charges = reader.ReadInt(); m_Expires = reader.ReadDateTime(); m_NextUse = reader.ReadDateTime();
            if (m_Charges < 0 || m_Charges > 3)
                m_Charges = 0;
        }

        private sealed class BroodAntivenomStatus : Gump
        {
            public BroodAntivenomStatus(Mobile viewer) : base(50, 50)
            {
                List<BroodAntivenomEffect> visible = new List<BroodAntivenomEffect>();
                foreach (BroodAntivenomEffect effect in m_Effects)
                {
                    BaseCreature pet = effect.Owner as BaseCreature;
                    if (!effect.Deleted && (effect.Owner == viewer || (pet != null && pet.Controlled && pet.ControlMaster == viewer)))
                        visible.Add(effect);
                }
                AddBackground(0, 0, 470, 100 + 75 * Math.Max(1, visible.Count), 9270);
                AddLabel(20, 15, 1152, "Brood Antivenom");
                int y = 45;
                if (visible.Count == 0)
                    AddLabel(20, y, 1152, "No active protection or reuse delay.");
                foreach (BroodAntivenomEffect effect in visible)
                {
                    DateTime now = DateTime.UtcNow;
                    AddLabel(20, y, 1152, effect.Owner == viewer ? "You" : effect.Owner.Name);
                    AddLabel(20, y + 20, 1152, String.Format("Protections: {0}; time left: {1}s", effect.Active(now) ? effect.Charges : 0,
                        effect.Active(now) ? Math.Max(0, (int)Math.Ceiling((effect.Expires - now).TotalSeconds)) : 0));
                    AddLabel(20, y + 40, 1152, String.Format("Retreatment delay: {0}s", Math.Max(0, (int)Math.Ceiling((effect.NextUse - now).TotalSeconds))));
                    y += 75;
                }
                AddButton(420, 15, 4005, 4007, 1, GumpButtonType.Reply, 0);
            }
            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (sender == null || info == null)
                    return;
                Mobile from = sender.Mobile;
                if (from == null || from.Deleted || !from.Player || info.ButtonID != 1)
                    return;
                from.SendGump(new BroodAntivenomStatus(from));
            }
        }
    }
}
