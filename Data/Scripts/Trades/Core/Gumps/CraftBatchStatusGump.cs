using System;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Engines.Craft
{
    public class CraftBatchStatusGump : Gump
    {
        private const int StopButtonID = 1;
        private const int LabelColor = 0x7FFF;
        private const int FontColor = 0xFFFFFF;
        private const int WarningColor = 0xFF8080;

        private Mobile m_From;
        private CraftSystem m_CraftSystem;

        public CraftBatchStatusGump(Mobile from, CraftSystem craftSystem, CraftContext context)
            : this(from, craftSystem, context, false, null)
        {
        }

        public CraftBatchStatusGump(
            Mobile from,
            CraftSystem craftSystem,
            CraftContext context,
            string finalReason
        )
            : this(from, craftSystem, context, true, finalReason)
        {
        }

        private CraftBatchStatusGump(
            Mobile from,
            CraftSystem craftSystem,
            CraftContext context,
            bool final,
            string finalReason
        )
            : base(580, 40)
        {
            m_From = from;
            m_CraftSystem = craftSystem;

            if (from != null)
                from.CloseGump(typeof(CraftBatchStatusGump));

            AddPage(0);

            AddBackground(0, 0, 270, 174, 5054);
            AddImageTiled(10, 10, 250, 22, 2624);
            AddImageTiled(10, 37, 250, 94, 2624);
            AddImageTiled(10, 136, 250, 28, 2624);
            AddAlphaRegion(10, 10, 250, 154);

            AddHtml(
                10,
                12,
                250,
                18,
                String.Format(
                    "<CENTER><BASEFONT COLOR=#{0:X6}>{1}</BASEFONT></CENTER>",
                    FontColor,
                    final ? "BATCH SUMMARY" : "BATCH CRAFTING"
                ),
                false,
                false
            );

            AddLabel(20, 42, 0x480, "Recipe");
            AddRecipeName(context);

            AddStatusLine(20, 66, "Attempt", GetProgressText(context));
            AddStatusLine(
                20,
                86,
                "Results",
                String.Format(
                    "{0} success, {1} failed",
                    context == null ? 0 : context.MakeAmountSuccesses,
                    context == null ? 0 : context.MakeAmountFailures
                )
            );
            AddStatusLine(20, 106, "State", GetStateText(context, final, finalReason));

            if (!final && context != null && context.HasMakeAmountBatch)
            {
                if (context.MakeAmountStopRequested)
                {
                    AddHtml(
                        20,
                        142,
                        230,
                        18,
                        String.Format(
                            "<BASEFONT COLOR=#{0:X6}>Stop requested. Current attempt will finish.</BASEFONT>",
                            WarningColor
                        ),
                        false,
                        false
                    );
                }
                else
                {
                    AddButton(20, 141, 4017, 4019, StopButtonID, GumpButtonType.Reply, 0);
                    AddHtml(
                        55,
                        144,
                        180,
                        18,
                        String.Format("<BASEFONT COLOR=#{0:X6}>STOP AFTER THIS ATTEMPT</BASEFONT>", FontColor),
                        false,
                        false
                    );
                }
            }
            else
            {
                AddHtml(
                    20,
                    142,
                    230,
                    18,
                    String.Format("<BASEFONT COLOR=#{0:X6}>Right-click to close.</BASEFONT>", FontColor),
                    false,
                    false
                );
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (sender == null || m_From == null || m_From.Deleted || sender.Mobile != m_From)
                return;

            if (info.ButtonID != StopButtonID || m_CraftSystem == null)
                return;

            CraftContext context = m_CraftSystem.GetContext(m_From);

            if (context == null || !context.HasMakeAmountBatch)
                return;

            context.RequestMakeAmountStop();
            m_From.SendMessage("Batch crafting will stop after the current attempt completes.");
            m_From.SendGump(new CraftBatchStatusGump(m_From, m_CraftSystem, context));
        }

        private void AddRecipeName(CraftContext context)
        {
            CraftItem item = context == null ? null : context.MakeAmountItem;

            if (item != null && item.NameNumber > 0)
                AddHtmlLocalized(83, 42, 165, 18, item.NameNumber, LabelColor, false, false);
            else
                AddHtml(83, 42, 165, 18, FormatValue(GetRecipeName(item)), false, false);
        }

        private void AddStatusLine(int x, int y, string label, string value)
        {
            AddHtml(
                x,
                y,
                70,
                18,
                String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", FontColor, label),
                false,
                false
            );
            AddHtml(x + 75, y, 165, 18, FormatValue(value), false, false);
        }

        private static string GetRecipeName(CraftItem item)
        {
            if (item == null)
                return "Unknown";

            if (item.NameString != null && item.NameString.Length > 0)
                return item.NameString;

            if (item.ItemType != null)
                return item.ItemType.Name;

            return "Unknown";
        }

        private static string GetProgressText(CraftContext context)
        {
            if (context == null || !context.HasMakeAmountBatch)
                return "0 / 0";

            return String.Format("{0} / {1}", context.MakeAmountCurrent, context.MakeAmountTotal);
        }

        private static string GetStateText(CraftContext context, bool final, string finalReason)
        {
            if (final && finalReason != null && finalReason.Length > 0)
                return finalReason;

            if (context == null || !context.HasMakeAmountBatch)
                return "Not active";

            return context.MakeAmountState;
        }

        private static string FormatValue(string value)
        {
            return String.Format(
                "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>",
                FontColor,
                EscapeHtml(value)
            );
        }

        private static string EscapeHtml(string value)
        {
            if (value == null)
                return String.Empty;

            return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
