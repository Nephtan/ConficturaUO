using System;
using Server.Gumps;
using Server.Network;

namespace Server.Custom.Confictura
{
    public sealed class TurnBasedCombatGump : Gump
    {
        private const int PageSize = 8;
        private readonly Mobile m_From;
        private readonly TurnCombatGroup m_Group;
        private readonly int m_ListPage;

        public TurnBasedCombatGump(Mobile from, TurnCombatGroup group)
            : this(from, group, 0)
        {
        }

        public TurnBasedCombatGump(Mobile from, TurnCombatGroup group, int listPage)
            : base(20, 40)
        {
            m_From = from;
            m_Group = group;
            m_ListPage = listPage;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 380, 370, 9270);
            AddAlphaRegion(12, 12, 356, 346);
            AddLabel(20, 18, 1153, "Turn-Based Combat");

            TurnParticipant self = TurnBasedCombatManager.Instance.GetParticipant(from);
            TurnParticipant current = group.Current;
            string currentName = current == null ? "Waiting" : SafeName(current.Mobile);
            int ap = self == null ? 0 : self.ActionPoints;

            AddLabel(20, 45, 0x59, String.Format("Group {0}  Round {1}", group.ID, group.Round));
            AddLabel(20, 65, 0x481, "Current: " + currentName);
            AddLabel(20, 85, 0x481, "Your AP: " + ap);

            AddLabel(
                20,
                105,
                0x481,
                String.Format(
                    "Hits {0}/{1}  Stam {2}/{3}  Mana {4}/{5}",
                    from.Hits,
                    from.HitsMax,
                    from.Stam,
                    from.StamMax,
                    from.Mana,
                    from.ManaMax
                )
            );

            string nextMana = self == null
                ? "n/a"
                : Math.Max(0.0, self.Effects.ManaRegenRemaining.TotalSeconds).ToString("0.0") + "s";
            AddLabel(20, 125, 0x481, "Next mana tick: " + nextMana);

            string pending = self != null && self.Pending != null
                ? self.Pending.Request.Kind.ToString()
                : "None";
            AddLabel(20, 145, 0x481, "Pending: " + pending);

            string effects = self == null
                ? "None"
                : BuildEffects(self);
            AddHtml(
                20,
                167,
                335,
                35,
                "<BASEFONT COLOR=#DDDDDD>Effects: " + Utility.FixHtml(effects) + "</BASEFONT>",
                false,
                false
            );

            AddLabel(20, 206, 1153, "Initiative");
            DrawInitiative(group);

            AddButton(20, 332, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(52, 334, 0x481, "End Turn");
            AddButton(140, 332, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddLabel(172, 334, 0x481, "Escape");
            AddButton(258, 332, 4011, 4013, 3, GumpButtonType.Reply, 0);
            AddLabel(290, 334, 0x481, "Refresh");
        }

        private void DrawInitiative(TurnCombatGroup group)
        {
            int pageCount = Math.Max(1, (group.Participants.Count + PageSize - 1) / PageSize);
            int page = m_ListPage;

            if (page < 0)
                page = 0;
            else if (page >= pageCount)
                page = pageCount - 1;

            int start = page * PageSize;
            int end = Math.Min(start + PageSize, group.Participants.Count);

            for (int i = start; i < end; ++i)
            {
                TurnParticipant participant = group.Participants[i];
                int y = 227 + ((i - start) * 12);
                string marker = group.Current == participant ? "> " : "  ";
                string wait = participant.EligibleRound > group.Round ? " (next round)" : "";
                AddLabel(
                    24,
                    y,
                    group.Current == participant ? 0x59 : 0x481,
                    String.Format(
                        "{0}{1}  {2}{3}",
                        marker,
                        participant.InitiativeTotal,
                        SafeName(participant.Mobile),
                        wait
                    )
                );
            }

            if (pageCount > 1)
            {
                if (page > 0)
                    AddButton(290, 206, 4014, 4016, 1000 + page - 1, GumpButtonType.Reply, 0);

                if (page + 1 < pageCount)
                    AddButton(335, 206, 4005, 4007, 1000 + page + 1, GumpButtonType.Reply, 0);
            }
        }

        private static string BuildEffects(TurnParticipant participant)
        {
            string effects = "";

            if (participant.Mobile.Poisoned)
            {
                PoisonImpl.PoisonTimer poison = participant.Effects.PoisonTimer;
                effects = poison == null
                    ? "Poison"
                    : "Poison (" + poison.RemainingTicks + " ticks)";
            }

            if (participant.Mobile.Paralyzed)
                effects += effects.Length == 0 ? "Paralyzed" : ", Paralyzed";

            if (participant.Mobile.Frozen)
                effects += effects.Length == 0 ? "Frozen" : ", Frozen";

            if (participant.Effects.BandageContext != null)
            {
                string bandage = "Bandaging ("
                    + Math.Max(0.0, participant.Effects.BandageRemaining.TotalSeconds).ToString("0.0")
                    + "s)";
                effects += effects.Length == 0 ? bandage : ", " + bandage;
            }

            return effects.Length == 0 ? "None" : effects;
        }

        private static string SafeName(Mobile mobile)
        {
            if (mobile == null)
                return "Unknown";

            return Utility.FixHtml(String.IsNullOrEmpty(mobile.Name) ? mobile.GetType().Name : mobile.Name);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;

            if (from == null || manager == null || manager.GetGroup(from) != m_Group)
                return;

            switch (info.ButtonID)
            {
                case 1:
                {
                    if (!manager.EndTurn(from, true))
                        from.SendMessage("It is not your turn.");

                    break;
                }
                case 2:
                {
                    string reason;

                    if (!manager.TryEscape(from, out reason))
                        from.SendMessage(reason);

                    break;
                }
                case 3:
                {
                    from.SendGump(new TurnBasedCombatGump(from, m_Group, m_ListPage));
                    break;
                }
                default:
                {
                    if (info.ButtonID >= 1000)
                        from.SendGump(new TurnBasedCombatGump(from, m_Group, info.ButtonID - 1000));

                    break;
                }
            }
        }
    }
}
