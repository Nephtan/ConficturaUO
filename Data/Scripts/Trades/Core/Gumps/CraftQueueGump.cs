using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Engines.Craft
{
    public class CraftQueueGump : Gump
    {
        private const int LabelHue = 0x480;
        private const int LabelColor = 0x7FFF;
        private const int FontColor = 0xFFFFFF;
        private const int WarningColor = 0xFF8080;
        private const int TextEntryHue = 0;
        private const int TextEntryFrame = 0x52;
        private const int TextEntryBackground = 0xBBC;

        private const int RowsPerPage = 8;
        private const int TargetTextEntryID = 9100;

        private const int BackButtonID = 1;
        private const int StartButtonID = 2;
        private const int StopButtonID = 3;
        private const int ClearButtonID = 4;
        private const int PreviousPageButtonID = 5;
        private const int NextPageButtonID = 6;
        private const int ApplyTargetButtonID = 7;
        private const int ResetSelectedButtonID = 8;
        private const int ClearCompleteButtonID = 9;

        private const int SelectRowButtonBase = 1000;
        private const int ToggleRowButtonBase = 2000;
        private const int MoveUpRowButtonBase = 3000;
        private const int MoveDownRowButtonBase = 4000;
        private const int RemoveRowButtonBase = 5000;

        private Mobile m_From;
        private CraftSystem m_CraftSystem;
        private BaseTool m_Tool;
        private int m_Page;
        private string m_Notice;

        public CraftQueueGump(Mobile from, CraftSystem craftSystem, BaseTool tool)
            : this(from, craftSystem, tool, 0, null)
        {
        }

        public CraftQueueGump(Mobile from, CraftSystem craftSystem, BaseTool tool, string notice)
            : this(from, craftSystem, tool, 0, notice)
        {
        }

        public CraftQueueGump(Mobile from, CraftSystem craftSystem, BaseTool tool, int page, string notice)
            : base(40, 40)
        {
            m_From = from;
            m_CraftSystem = craftSystem;
            m_Tool = tool;
            m_Page = page;
            m_Notice = notice;

            CraftContext context = craftSystem == null ? null : craftSystem.GetContext(from);

            if (from != null)
            {
                from.CloseGump(typeof(CraftGump));
                from.CloseGump(typeof(CraftGumpItem));
                from.CloseGump(typeof(CraftQueueGump));
            }

            int totalPages = GetTotalPages(context);

            if (m_Page < 0)
                m_Page = 0;
            else if (m_Page >= totalPages)
                m_Page = totalPages - 1;

            AddPage(0);
            DrawFrame(context, totalPages);
            DrawRows(context);
            DrawSelectedEditor(context);
            DrawFooter(context);
        }

        private void DrawFrame(CraftContext context, int totalPages)
        {
            AddBackground(0, 0, 530, 437, 5054);
            AddImageTiled(10, 10, 510, 22, 2624);
            AddImageTiled(10, 37, 510, 42, 2624);
            AddImageTiled(10, 84, 510, 236, 2624);
            AddImageTiled(10, 325, 510, 55, 2624);
            AddImageTiled(10, 385, 510, 42, 2624);
            AddAlphaRegion(10, 10, 510, 417);

            AddHtml(10, 12, 510, 18, FormatCenter("CRAFTING QUEUE"), false, false);

            string state = context == null ? "No crafting context" : context.CraftQueueState;

            if (String.IsNullOrEmpty(state))
                state = context != null && context.CraftQueueRunning ? "Running" : "Idle";

            AddHtml(20, 42, 160, 18, FormatLabel("State", state), false, false);
            AddHtml(
                185,
                42,
                150,
                18,
                FormatLabel("SRC", CraftContainerUtility.GetSourceDisplayName(m_From, context)),
                false,
                false
            );
            AddHtml(
                340,
                42,
                165,
                18,
                FormatLabel("OUT", CraftContainerUtility.GetDestinationDisplayName(m_From, context)),
                false,
                false
            );

            AddHtml(
                20,
                62,
                160,
                18,
                FormatLabel("Page", String.Format("{0}/{1}", m_Page + 1, totalPages)),
                false,
                false
            );
            AddHtml(
                185,
                62,
                150,
                18,
                FormatLabel("Attempts", GetProgressText(context)),
                false,
                false
            );
            AddHtml(
                340,
                62,
                165,
                18,
                FormatLabel("Results", GetResultText(context)),
                false,
                false
            );

            AddLabel(55, 88, LabelHue, "Recipe");
            AddLabel(220, 88, LabelHue, "Attempts");
            AddLabel(300, 88, LabelHue, "Result");
            AddLabel(375, 88, LabelHue, "On");
            AddLabel(423, 88, LabelHue, "Order");
            AddLabel(480, 88, LabelHue, "Del");

            if (!String.IsNullOrEmpty(m_Notice))
                AddHtml(20, 305, 485, 18, FormatValue(m_Notice, WarningColor), false, false);
        }

        private void DrawRows(CraftContext context)
        {
            List<CraftQueueRow> rows = context == null ? null : context.CraftQueueRows;

            if (rows == null || rows.Count == 0)
            {
                AddHtml(20, 140, 480, 18, FormatValue("Add recipes from item details to build a queue."), false, false);
                return;
            }

            int start = m_Page * RowsPerPage;
            int end = Math.Min(start + RowsPerPage, rows.Count);

            for (int i = start; i < end; i++)
            {
                CraftQueueRow row = rows[i];
                int visibleIndex = i - start;
                int y = 112 + (visibleIndex * 24);
                bool selected = context.SelectedCraftQueueRowId == row.RowId;

                AddButton(20, y, selected ? 4006 : 4005, 4007, SelectRowButtonBase + row.RowId, GumpButtonType.Reply, 0);
                AddHtml(55, y + 2, 155, 18, FormatValue(Crop(row.DisplayName, 24)), false, false);
                AddHtml(
                    220,
                    y + 2,
                    70,
                    18,
                    FormatValue(String.Format("{0}/{1}", row.CompletedAttempts, row.TargetAttempts)),
                    false,
                    false
                );
                AddHtml(
                    300,
                    y + 2,
                    70,
                    18,
                    FormatValue(String.Format("{0}S {1}F", row.Successes, row.Failures)),
                    false,
                    false
                );
                AddButton(
                    375,
                    y,
                    row.Enabled ? 4005 : 4017,
                    row.Enabled ? 4007 : 4019,
                    ToggleRowButtonBase + row.RowId,
                    GumpButtonType.Reply,
                    0
                );
                AddButton(420, y, 4014, 4016, MoveUpRowButtonBase + row.RowId, GumpButtonType.Reply, 0);
                AddButton(445, y, 4005, 4007, MoveDownRowButtonBase + row.RowId, GumpButtonType.Reply, 0);
                AddButton(480, y, 4017, 4019, RemoveRowButtonBase + row.RowId, GumpButtonType.Reply, 0);

                AddHtml(55, y + 14, 315, 12, FormatSmall(Crop(GetRowState(row), 52)), false, false);
            }
        }

        private void DrawSelectedEditor(CraftContext context)
        {
            CraftQueueRow row = context == null ? null : context.GetSelectedCraftQueueRow();

            if (row == null)
            {
                AddHtml(20, 334, 300, 18, FormatValue("Select a queue row to edit its amount."), false, false);
                return;
            }

            AddHtml(20, 334, 230, 18, FormatLabel("Selected", Crop(row.DisplayName, 26)), false, false);
            AddHtml(
                255,
                334,
                125,
                18,
                FormatLabel("State", GetRowState(row)),
                false,
                false
            );

            AddHtml(20, 357, 70, 18, FormatValue("AMOUNT"), false, false);
            AddImageTiled(90, 354, 58, 22, TextEntryFrame);
            AddImageTiled(92, 356, 54, 18, TextEntryBackground);
            AddTextEntry(98, 356, 44, 18, TextEntryHue, TargetTextEntryID, row.TargetAttempts.ToString());
            AddButton(155, 354, 4005, 4007, ApplyTargetButtonID, GumpButtonType.Reply, 0);
            AddHtml(190, 357, 65, 18, FormatValue("APPLY"), false, false);
            AddButton(270, 354, 4017, 4019, ResetSelectedButtonID, GumpButtonType.Reply, 0);
            AddHtml(305, 357, 80, 18, FormatValue("RESET"), false, false);
        }

        private void DrawFooter(CraftContext context)
        {
            bool running = context != null && context.CraftQueueRunning;
            bool stopRequested = context != null && context.CraftQueueStopRequested;

            AddButton(20, 394, 4014, 4016, BackButtonID, GumpButtonType.Reply, 0);
            AddHtml(55, 397, 60, 18, FormatValue("BACK"), false, false);

            if (running)
            {
                if (stopRequested)
                {
                    AddHtml(130, 397, 180, 18, FormatValue("STOP REQUESTED", WarningColor), false, false);
                }
                else
                {
                    AddButton(130, 394, 4017, 4019, StopButtonID, GumpButtonType.Reply, 0);
                    AddHtml(165, 397, 120, 18, FormatValue("STOP"), false, false);
                }
            }
            else
            {
                AddButton(130, 394, 4005, 4007, StartButtonID, GumpButtonType.Reply, 0);
                AddHtml(165, 397, 90, 18, FormatValue("START"), false, false);
            }

            AddButton(250, 394, 4017, 4019, ClearButtonID, GumpButtonType.Reply, 0);
            AddHtml(285, 397, 70, 18, FormatValue("CLEAR"), false, false);

            AddButton(365, 394, 4017, 4019, ClearCompleteButtonID, GumpButtonType.Reply, 0);
            AddHtml(400, 397, 105, 18, FormatValue("CLEAR DONE"), false, false);

            if (m_Page > 0)
                AddButton(365, 302, 4014, 4016, PreviousPageButtonID, GumpButtonType.Reply, 0);

            if (m_Page < GetTotalPages(context) - 1)
                AddButton(480, 302, 4005, 4007, NextPageButtonID, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (sender == null || m_From == null || m_From.Deleted || sender.Mobile != m_From)
                return;

            if (m_CraftSystem == null)
                return;

            CraftContext context = m_CraftSystem.GetContext(m_From);

            if (context == null)
                return;

            int buttonID = info.ButtonID;
            string message = null;

            if (buttonID <= 0)
                return;

            if (buttonID >= RemoveRowButtonBase)
            {
                context.RemoveCraftQueueRow(buttonID - RemoveRowButtonBase, out message);
                SendRefresh(message);
                return;
            }

            if (buttonID >= MoveDownRowButtonBase)
            {
                context.MoveCraftQueueRow(buttonID - MoveDownRowButtonBase, 1, out message);
                SendRefresh(message);
                return;
            }

            if (buttonID >= MoveUpRowButtonBase)
            {
                context.MoveCraftQueueRow(buttonID - MoveUpRowButtonBase, -1, out message);
                SendRefresh(message);
                return;
            }

            if (buttonID >= ToggleRowButtonBase)
            {
                context.ToggleCraftQueueRow(buttonID - ToggleRowButtonBase, out message);
                SendRefresh(message);
                return;
            }

            if (buttonID >= SelectRowButtonBase)
            {
                CraftQueueRow row = context.GetCraftQueueRow(buttonID - SelectRowButtonBase);

                if (row == null)
                    message = "That queue row is no longer available.";
                else
                    context.SelectedCraftQueueRowId = row.RowId;

                SendRefresh(message);
                return;
            }

            switch (buttonID)
            {
                case BackButtonID:
                {
                    if (context.CraftQueueRunning)
                    {
                        SendRefresh("Stop the queue before returning to the crafting menu.");
                    }
                    else
                    {
                        m_From.SendGump(new CraftGump(m_From, m_CraftSystem, m_Tool, null));
                    }

                    break;
                }
                case StartButtonID:
                {
                    CraftQueueController.StartQueue(m_From, m_CraftSystem, m_Tool);
                    break;
                }
                case StopButtonID:
                {
                    if (context.CraftQueueRunning)
                    {
                        context.RequestCraftQueueStop();
                        m_From.SendMessage("Crafting queue will stop after the current attempt completes.");
                        SendRefresh(null);
                    }
                    else
                    {
                        SendRefresh("The crafting queue is not running.");
                    }

                    break;
                }
                case ClearButtonID:
                {
                    if (context.CraftQueueRunning)
                        SendRefresh("Stop the queue before clearing it.");
                    else
                    {
                        context.ClearCraftQueue();
                        SendRefresh("Queue cleared.");
                    }

                    break;
                }
                case PreviousPageButtonID:
                {
                    SendPage(m_Page - 1, null);
                    break;
                }
                case NextPageButtonID:
                {
                    SendPage(m_Page + 1, null);
                    break;
                }
                case ApplyTargetButtonID:
                {
                    ApplyTargetAmount(context, info);
                    break;
                }
                case ResetSelectedButtonID:
                {
                    CraftQueueRow row = context.GetSelectedCraftQueueRow();

                    if (row == null)
                        SendRefresh("Select a queue row first.");
                    else
                    {
                        context.ResetCraftQueueRow(row.RowId, out message);
                        SendRefresh(message);
                    }

                    break;
                }
                case ClearCompleteButtonID:
                {
                    if (context.CraftQueueRunning)
                        SendRefresh("Stop the queue before clearing completed rows.");
                    else
                    {
                        context.ClearCompletedCraftQueueRows();
                        SendRefresh("Completed rows cleared.");
                    }

                    break;
                }
                default:
                {
                    SendRefresh("That queue action is no longer available.");
                    break;
                }
            }
        }

        private void ApplyTargetAmount(CraftContext context, RelayInfo info)
        {
            CraftQueueRow row = context.GetSelectedCraftQueueRow();

            if (row == null)
            {
                SendRefresh("Select a queue row first.");
                return;
            }

            TextRelay relay = info.GetTextEntry(TargetTextEntryID);

            if (relay == null || relay.Text == null)
            {
                SendRefresh("Enter an amount from 1 to 100.");
                return;
            }

            string text = relay.Text.Trim();
            int amount;

            if (!Int32.TryParse(text, out amount))
            {
                SendRefresh("Enter an amount from 1 to 100.");
                return;
            }

            string message;

            context.SetCraftQueueRowTarget(row.RowId, amount, out message);
            SendRefresh(message);
        }

        private void SendRefresh(string notice)
        {
            m_From.SendGump(new CraftQueueGump(m_From, m_CraftSystem, m_Tool, m_Page, notice));
        }

        private void SendPage(int page, string notice)
        {
            m_From.SendGump(new CraftQueueGump(m_From, m_CraftSystem, m_Tool, page, notice));
        }

        private static int GetTotalPages(CraftContext context)
        {
            int rowCount = context == null ? 0 : context.CraftQueueRows.Count;

            if (rowCount <= 0)
                return 1;

            return (int)Math.Ceiling(rowCount / (double)RowsPerPage);
        }

        private static string GetProgressText(CraftContext context)
        {
            if (context == null)
                return "0/0";

            return String.Format("{0}/{1}", context.GetCraftQueueCompletedAttempts(), context.GetCraftQueueTargetAttempts());
        }

        private static string GetResultText(CraftContext context)
        {
            if (context == null)
                return "0S 0F";

            return String.Format("{0}S {1}F", context.GetCraftQueueSuccesses(), context.GetCraftQueueFailures());
        }

        private static string GetRowState(CraftQueueRow row)
        {
            if (row == null)
                return String.Empty;

            if (!String.IsNullOrEmpty(row.LastMessage))
                return row.LastMessage;

            return row.State.ToString();
        }

        private static string FormatCenter(string value)
        {
            return String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>{1}</BASEFONT></CENTER>", FontColor, EscapeHtml(value));
        }

        private static string FormatLabel(string label, string value)
        {
            return String.Format(
                "<BASEFONT COLOR=#{0:X6}>{1}: {2}</BASEFONT>",
                FontColor,
                EscapeHtml(label),
                EscapeHtml(value)
            );
        }

        private static string FormatValue(string value)
        {
            return FormatValue(value, FontColor);
        }

        private static string FormatValue(string value, int color)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, EscapeHtml(value));
        }

        private static string FormatSmall(string value)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}><SMALL>{1}</SMALL></BASEFONT>", FontColor, EscapeHtml(value));
        }

        private static string EscapeHtml(string value)
        {
            if (value == null)
                return String.Empty;

            return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private static string Crop(string value, int maxLength)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            if (value.Length <= maxLength)
                return value;

            if (maxLength <= 3)
                return value.Substring(0, maxLength);

            return value.Substring(0, maxLength - 3) + "...";
        }
    }

    internal static class CraftQueueController
    {
        private class CraftQueueContinueState
        {
            private Mobile m_From;
            private CraftSystem m_CraftSystem;
            private BaseTool m_Tool;

            public CraftQueueContinueState(Mobile from, CraftSystem craftSystem, BaseTool tool)
            {
                m_From = from;
                m_CraftSystem = craftSystem;
                m_Tool = tool;
            }

            public Mobile From
            {
                get { return m_From; }
            }

            public CraftSystem CraftSystem
            {
                get { return m_CraftSystem; }
            }

            public BaseTool Tool
            {
                get { return m_Tool; }
            }
        }

        public static void StartQueue(Mobile from, CraftSystem craftSystem, BaseTool tool)
        {
            if (from == null || from.Deleted || craftSystem == null)
                return;

            CraftContext context = craftSystem.GetContext(from);

            if (context == null)
                return;

            string message;

            if (!context.StartCraftQueue(tool, out message))
            {
                from.SendGump(new CraftQueueGump(from, craftSystem, tool, message));
                return;
            }

            from.SendGump(new CraftQueueGump(from, craftSystem, tool, "Queue started."));
            ContinueQueue(from, craftSystem, tool);
        }

        public static void ContinueQueueAfterAttempt(Mobile from, CraftSystem craftSystem, BaseTool tool)
        {
            if (from == null || from.Deleted || craftSystem == null)
                return;

            CraftContext context = craftSystem.GetContext(from);

            if (context == null || !context.CraftQueueRunning)
                return;

            ContinueQueue(from, craftSystem, tool);
        }

        public static void EndQueue(Mobile from, CraftSystem craftSystem, CraftContext context, string reason)
        {
            if (context == null || !context.CraftQueueRunning)
                return;

            BaseTool tool = context.CraftQueueTool;

            context.EndCraftQueue(reason);

            if (from != null && !from.Deleted && craftSystem != null)
            {
                from.SendMessage(
                    String.Format(
                        "Crafting queue ended: {0} ({1}/{2} attempts, {3} successes, {4} failures).",
                        reason,
                        context.GetCraftQueueCompletedAttempts(),
                        context.GetCraftQueueTargetAttempts(),
                        context.GetCraftQueueSuccesses(),
                        context.GetCraftQueueFailures()
                    )
                );
                from.SendGump(new CraftQueueGump(from, craftSystem, tool, reason));
            }
        }

        private static void ContinueQueue(Mobile from, CraftSystem craftSystem, BaseTool tool)
        {
            Timer.DelayCall(
                TimeSpan.Zero,
                new TimerStateCallback(ContinueQueue_Callback),
                new CraftQueueContinueState(from, craftSystem, tool)
            );
        }

        private static void ContinueQueue_Callback(object state)
        {
            CraftQueueContinueState queueState = state as CraftQueueContinueState;

            if (queueState == null)
                return;

            Mobile from = queueState.From;
            CraftSystem craftSystem = queueState.CraftSystem;
            BaseTool tool = queueState.Tool;

            if (from == null || from.Deleted || craftSystem == null)
                return;

            CraftContext context = craftSystem.GetContext(from);

            if (context == null || !context.CraftQueueRunning)
                return;

            CraftQueueRow row;
            string reason;

            if (!context.PrepareNextCraftQueueAttempt(tool, out row, out reason))
            {
                EndQueue(from, craftSystem, context, reason == null ? "Stopped." : reason);
                return;
            }

            if (row == null || row.CraftItem == null || row.ItemType == null)
            {
                EndQueue(from, craftSystem, context, "Stopped because a queue row is no longer valid.");
                return;
            }

            CraftContainerUtility.ResolveSourceContainer(from, context, true);
            CraftContainerUtility.ResolveDestinationContainer(from, context, true);

            craftSystem.CreateItem(from, row.ItemType, row.TypeRes, tool, row.CraftItem, CraftGump.MinMakeAmount);
        }
    }
}
