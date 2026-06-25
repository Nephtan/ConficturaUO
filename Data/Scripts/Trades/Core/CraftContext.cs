using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;

namespace Server.Engines.Craft
{
    public enum CraftMarkOption
    {
        MarkItem,
        DoNotMark,
        PromptForMark
    }

    public enum CraftQueueRowState
    {
        Pending,
        Running,
        Disabled,
        Complete,
        Blocked,
        StopRequested
    }

    public class CraftQueueRow
    {
        private int m_RowId;
        private CraftItem m_CraftItem;
        private Type m_ItemType;
        private Type m_TypeRes;
        private string m_DisplayName;
        private int m_TargetAttempts;
        private int m_CompletedAttempts;
        private int m_Successes;
        private int m_Failures;
        private bool m_Enabled;
        private CraftQueueRowState m_State;
        private string m_LastMessage;

        public CraftQueueRow(int rowId, CraftItem craftItem, Type typeRes, int targetAttempts)
        {
            m_RowId = rowId;
            m_CraftItem = craftItem;
            m_ItemType = craftItem == null ? null : craftItem.ItemType;
            m_TypeRes = typeRes;
            m_DisplayName = GetDisplayName(craftItem);
            m_TargetAttempts = targetAttempts;
            m_Enabled = true;
            m_State = CraftQueueRowState.Pending;
        }

        public int RowId
        {
            get { return m_RowId; }
        }

        public CraftItem CraftItem
        {
            get { return m_CraftItem; }
        }

        public Type ItemType
        {
            get { return m_ItemType; }
        }

        public Type TypeRes
        {
            get { return m_TypeRes; }
        }

        public string DisplayName
        {
            get { return m_DisplayName; }
        }

        public int TargetAttempts
        {
            get { return m_TargetAttempts; }
            set { m_TargetAttempts = value; }
        }

        public int CompletedAttempts
        {
            get { return m_CompletedAttempts; }
        }

        public int Successes
        {
            get { return m_Successes; }
        }

        public int Failures
        {
            get { return m_Failures; }
        }

        public bool Enabled
        {
            get { return m_Enabled; }
            set
            {
                m_Enabled = value;

                if (!m_Enabled)
                    m_State = CraftQueueRowState.Disabled;
                else if (m_CompletedAttempts >= m_TargetAttempts)
                    m_State = CraftQueueRowState.Complete;
                else
                    m_State = CraftQueueRowState.Pending;
            }
        }

        public CraftQueueRowState State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public string LastMessage
        {
            get { return m_LastMessage == null ? String.Empty : m_LastMessage; }
            set { m_LastMessage = value; }
        }

        public bool IsComplete
        {
            get { return m_CompletedAttempts >= m_TargetAttempts; }
        }

        public bool CanRun
        {
            get { return m_Enabled && !IsComplete; }
        }

        public void RecordAttempt(bool success)
        {
            m_CompletedAttempts++;

            if (success)
            {
                m_Successes++;
                m_LastMessage = "Last attempt succeeded.";
            }
            else
            {
                m_Failures++;
                m_LastMessage = "Last attempt failed.";
            }

            if (m_CompletedAttempts >= m_TargetAttempts)
                m_State = CraftQueueRowState.Complete;
            else if (m_Enabled)
                m_State = CraftQueueRowState.Pending;
            else
                m_State = CraftQueueRowState.Disabled;
        }

        public void Reset()
        {
            m_CompletedAttempts = 0;
            m_Successes = 0;
            m_Failures = 0;
            m_LastMessage = null;
            m_State = m_Enabled ? CraftQueueRowState.Pending : CraftQueueRowState.Disabled;
        }

        private static string GetDisplayName(CraftItem craftItem)
        {
            if (craftItem == null)
                return "Unknown";

            if (!String.IsNullOrEmpty(craftItem.NameString))
                return craftItem.NameString;

            if (craftItem.ItemType != null)
                return craftItem.ItemType.Name;

            return "Unknown";
        }
    }

    public class CraftContext
    {
        public const int MaxCraftQueueRows = 25;
        public const int MaxCraftQueueAttemptsPerRow = 100;
        public const int MaxCraftQueuePendingAttempts = 500;

        private List<CraftItem> m_Items;
        private List<CraftQueueRow> m_CraftQueueRows;
        private int m_LastResourceIndex;
        private int m_LastResourceIndex2;
        private int m_LastGroupIndex;
        private bool m_DoNotColor;
        private CraftMarkOption m_MarkOption;
        private int m_MakeAmount;
        private CraftItem m_MakeAmountItem;
        private Type m_MakeAmountTypeRes;
        private BaseTool m_MakeAmountTool;
        private int m_MakeAmountRemaining;
        private int m_MakeAmountTotal;
        private int m_MakeAmountCurrent;
        private int m_MakeAmountSuccesses;
        private int m_MakeAmountFailures;
        private bool m_MakeAmountStopRequested;
        private string m_MakeAmountState;
        private Container m_SourceContainer;
        private Container m_DestinationContainer;
        private int m_NextCraftQueueRowId;
        private int m_SelectedCraftQueueRowId;
        private int m_ActiveCraftQueueRowId;
        private bool m_CraftQueueRunning;
        private bool m_CraftQueueStopRequested;
        private string m_CraftQueueState;
        private BaseTool m_CraftQueueTool;

        public List<CraftItem> Items
        {
            get { return m_Items; }
        }
        public List<CraftQueueRow> CraftQueueRows
        {
            get { return m_CraftQueueRows; }
        }
        public int LastResourceIndex
        {
            get { return m_LastResourceIndex; }
            set { m_LastResourceIndex = value; }
        }
        public int LastResourceIndex2
        {
            get { return m_LastResourceIndex2; }
            set { m_LastResourceIndex2 = value; }
        }
        public int LastGroupIndex
        {
            get { return m_LastGroupIndex; }
            set { m_LastGroupIndex = value; }
        }
        public bool DoNotColor
        {
            get { return m_DoNotColor; }
            set { m_DoNotColor = value; }
        }
        public CraftMarkOption MarkOption
        {
            get { return m_MarkOption; }
            set { m_MarkOption = value; }
        }
        public int MakeAmount
        {
            get { return m_MakeAmount; }
            set { m_MakeAmount = value; }
        }
        public bool HasMakeAmountBatch
        {
            get { return m_MakeAmountItem != null && m_MakeAmountTotal > 1; }
        }
        public CraftItem MakeAmountItem
        {
            get { return m_MakeAmountItem; }
        }
        public int MakeAmountTotal
        {
            get { return m_MakeAmountTotal; }
        }
        public int MakeAmountCurrent
        {
            get { return m_MakeAmountCurrent; }
        }
        public int MakeAmountSuccesses
        {
            get { return m_MakeAmountSuccesses; }
        }
        public int MakeAmountFailures
        {
            get { return m_MakeAmountFailures; }
        }
        public int MakeAmountAttemptsComplete
        {
            get { return m_MakeAmountSuccesses + m_MakeAmountFailures; }
        }
        public bool MakeAmountStopRequested
        {
            get { return m_MakeAmountStopRequested; }
        }
        public string MakeAmountState
        {
            get { return m_MakeAmountState == null ? String.Empty : m_MakeAmountState; }
        }
        public Container SourceContainer
        {
            get { return m_SourceContainer; }
            set { m_SourceContainer = value; }
        }
        public Container DestinationContainer
        {
            get { return m_DestinationContainer; }
            set { m_DestinationContainer = value; }
        }
        public int SelectedCraftQueueRowId
        {
            get { return m_SelectedCraftQueueRowId; }
            set { m_SelectedCraftQueueRowId = value; }
        }
        public bool HasCraftQueueRows
        {
            get { return m_CraftQueueRows.Count > 0; }
        }
        public bool CraftQueueRunning
        {
            get { return m_CraftQueueRunning; }
        }
        public bool CraftQueueStopRequested
        {
            get { return m_CraftQueueStopRequested; }
        }
        public string CraftQueueState
        {
            get { return m_CraftQueueState == null ? String.Empty : m_CraftQueueState; }
        }
        public BaseTool CraftQueueTool
        {
            get { return m_CraftQueueTool; }
        }

        public CraftContext()
        {
            m_Items = new List<CraftItem>();
            m_CraftQueueRows = new List<CraftQueueRow>();
            m_LastResourceIndex = -1;
            m_LastResourceIndex2 = -1;
            m_LastGroupIndex = -1;
            m_MakeAmount = 1;
            m_SelectedCraftQueueRowId = -1;
            m_ActiveCraftQueueRowId = -1;
            m_NextCraftQueueRowId = 1;
        }

        public CraftItem LastMade
        {
            get
            {
                if (m_Items.Count > 0)
                    return m_Items[0];

                return null;
            }
        }

        public void OnMade(CraftItem item)
        {
            m_Items.Remove(item);

            if (m_Items.Count == 10)
                m_Items.RemoveAt(9);

            m_Items.Insert(0, item);
        }

        public void StartMakeAmountBatch(CraftItem item, Type typeRes, BaseTool tool, int amount)
        {
            if (item == null || tool == null || tool.Deleted || amount <= 1)
            {
                ClearMakeAmountBatch();
                return;
            }

            m_MakeAmountItem = item;
            m_MakeAmountTypeRes = typeRes;
            m_MakeAmountTool = tool;
            m_MakeAmountRemaining = amount - 1;
            m_MakeAmountTotal = amount;
            m_MakeAmountCurrent = 1;
            m_MakeAmountSuccesses = 0;
            m_MakeAmountFailures = 0;
            m_MakeAmountStopRequested = false;
            m_MakeAmountState = "Crafting";
        }

        public void ClearMakeAmountBatch()
        {
            m_MakeAmountItem = null;
            m_MakeAmountTypeRes = null;
            m_MakeAmountTool = null;
            m_MakeAmountRemaining = 0;
            m_MakeAmountTotal = 0;
            m_MakeAmountCurrent = 0;
            m_MakeAmountSuccesses = 0;
            m_MakeAmountFailures = 0;
            m_MakeAmountStopRequested = false;
            m_MakeAmountState = null;
        }

        public bool MatchesMakeAmountBatch(CraftItem item, Type typeRes, BaseTool tool)
        {
            return HasMakeAmountBatch
                && m_MakeAmountItem == item
                && m_MakeAmountTypeRes == typeRes
                && m_MakeAmountTool == tool
                && tool != null
                && !tool.Deleted
                && tool.UsesRemaining > 0;
        }

        public bool HasMakeAmountRemaining
        {
            get { return m_MakeAmountRemaining > 0; }
        }

        public void RequestMakeAmountStop()
        {
            if (HasMakeAmountBatch)
            {
                m_MakeAmountStopRequested = true;
                m_MakeAmountState = "Stopping after current attempt";
            }
        }

        public void SetMakeAmountState(string state)
        {
            if (HasMakeAmountBatch)
                m_MakeAmountState = state;
        }

        public void RecordMakeAmountAttempt(bool success)
        {
            if (!HasMakeAmountBatch)
                return;

            if (success)
            {
                m_MakeAmountSuccesses++;
                m_MakeAmountState = "Last attempt succeeded";
            }
            else
            {
                m_MakeAmountFailures++;
                m_MakeAmountState = "Last attempt failed";
            }
        }

        public bool ConsumeMakeAmountBatch(CraftItem item, Type typeRes, BaseTool tool)
        {
            if (!MatchesMakeAmountBatch(item, typeRes, tool))
                return false;

            if (m_MakeAmountStopRequested || m_MakeAmountRemaining <= 0)
                return false;

            m_MakeAmountRemaining--;
            m_MakeAmountCurrent++;
            m_MakeAmountState = "Crafting";
            return true;
        }

        public void ResetCraftContainers()
        {
            m_SourceContainer = null;
            m_DestinationContainer = null;
        }

        public CraftQueueRow AddCraftQueueRow(CraftItem item, Type typeRes, int targetAttempts, out string message)
        {
            message = null;

            if (item == null)
            {
                message = "Select a valid recipe before adding it to the queue.";
                return null;
            }

            if (m_CraftQueueRunning)
            {
                message = "Stop the active queue before adding more recipes.";
                return null;
            }

            if (m_CraftQueueRows.Count >= MaxCraftQueueRows)
            {
                message = String.Format("The crafting queue is limited to {0} rows.", MaxCraftQueueRows);
                return null;
            }

            if (targetAttempts < 1)
                targetAttempts = 1;
            else if (targetAttempts > MaxCraftQueueAttemptsPerRow)
                targetAttempts = MaxCraftQueueAttemptsPerRow;

            if (GetCraftQueuePendingAttempts() + targetAttempts > MaxCraftQueuePendingAttempts)
            {
                message = String.Format(
                    "The crafting queue is limited to {0} pending attempts.",
                    MaxCraftQueuePendingAttempts
                );
                return null;
            }

            CraftQueueRow row = new CraftQueueRow(m_NextCraftQueueRowId++, item, typeRes, targetAttempts);
            m_CraftQueueRows.Add(row);
            m_SelectedCraftQueueRowId = row.RowId;
            m_CraftQueueState = String.Format("Added {0}.", row.DisplayName);
            message = m_CraftQueueState;
            return row;
        }

        public CraftQueueRow GetCraftQueueRow(int rowId)
        {
            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                if (m_CraftQueueRows[i].RowId == rowId)
                    return m_CraftQueueRows[i];
            }

            return null;
        }

        public CraftQueueRow GetSelectedCraftQueueRow()
        {
            CraftQueueRow row = GetCraftQueueRow(m_SelectedCraftQueueRowId);

            if (row != null)
                return row;

            if (m_CraftQueueRows.Count > 0)
            {
                row = m_CraftQueueRows[0];
                m_SelectedCraftQueueRowId = row.RowId;
            }
            else
            {
                m_SelectedCraftQueueRowId = -1;
            }

            return row;
        }

        public CraftQueueRow GetActiveCraftQueueRow()
        {
            return GetCraftQueueRow(m_ActiveCraftQueueRowId);
        }

        public bool RemoveCraftQueueRow(int rowId, out string message)
        {
            message = null;

            if (m_CraftQueueRunning)
            {
                message = "Stop the active queue before removing rows.";
                return false;
            }

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                CraftQueueRow row = m_CraftQueueRows[i];

                if (row.RowId == rowId)
                {
                    m_CraftQueueRows.RemoveAt(i);

                    if (m_SelectedCraftQueueRowId == rowId)
                        m_SelectedCraftQueueRowId = m_CraftQueueRows.Count > 0 ? m_CraftQueueRows[Math.Min(i, m_CraftQueueRows.Count - 1)].RowId : -1;

                    message = "Queue row removed.";
                    m_CraftQueueState = message;
                    return true;
                }
            }

            message = "That queue row is no longer available.";
            return false;
        }

        public bool MoveCraftQueueRow(int rowId, int offset, out string message)
        {
            message = null;

            if (m_CraftQueueRunning)
            {
                message = "Stop the active queue before reordering rows.";
                return false;
            }

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                if (m_CraftQueueRows[i].RowId == rowId)
                {
                    int newIndex = i + offset;

                    if (newIndex < 0 || newIndex >= m_CraftQueueRows.Count)
                    {
                        message = "That queue row cannot move farther.";
                        return false;
                    }

                    CraftQueueRow row = m_CraftQueueRows[i];
                    m_CraftQueueRows.RemoveAt(i);
                    m_CraftQueueRows.Insert(newIndex, row);
                    message = "Queue row moved.";
                    m_CraftQueueState = message;
                    return true;
                }
            }

            message = "That queue row is no longer available.";
            return false;
        }

        public bool ToggleCraftQueueRow(int rowId, out string message)
        {
            message = null;
            CraftQueueRow row = GetCraftQueueRow(rowId);

            if (row == null)
            {
                message = "That queue row is no longer available.";
                return false;
            }

            if (m_CraftQueueRunning)
            {
                message = "Stop the active queue before changing enabled rows.";
                return false;
            }

            row.Enabled = !row.Enabled;
            message = row.Enabled ? "Queue row enabled." : "Queue row disabled.";
            m_CraftQueueState = message;
            return true;
        }

        public bool SetCraftQueueRowTarget(int rowId, int targetAttempts, out string message)
        {
            message = null;
            CraftQueueRow row = GetCraftQueueRow(rowId);

            if (row == null)
            {
                message = "Select a valid queue row first.";
                return false;
            }

            if (m_CraftQueueRunning)
            {
                message = "Stop the active queue before changing amounts.";
                return false;
            }

            if (targetAttempts < 1 || targetAttempts > MaxCraftQueueAttemptsPerRow)
            {
                message = String.Format("Enter an amount from 1 to {0}.", MaxCraftQueueAttemptsPerRow);
                return false;
            }

            if (targetAttempts < row.CompletedAttempts)
            {
                message = "Reset or remove that row before lowering it below completed attempts.";
                return false;
            }

            int currentPending = GetCraftQueuePendingAttempts() - Math.Max(0, row.TargetAttempts - row.CompletedAttempts);
            int newPending = Math.Max(0, targetAttempts - row.CompletedAttempts);

            if (currentPending + newPending > MaxCraftQueuePendingAttempts)
            {
                message = String.Format(
                    "The crafting queue is limited to {0} pending attempts.",
                    MaxCraftQueuePendingAttempts
                );
                return false;
            }

            row.TargetAttempts = targetAttempts;

            if (row.IsComplete)
                row.State = CraftQueueRowState.Complete;
            else if (row.Enabled)
                row.State = CraftQueueRowState.Pending;

            message = "Queue amount updated.";
            m_CraftQueueState = message;
            return true;
        }

        public void ClearCraftQueue()
        {
            if (m_CraftQueueRunning)
                return;

            m_CraftQueueRows.Clear();
            m_SelectedCraftQueueRowId = -1;
            m_ActiveCraftQueueRowId = -1;
            m_CraftQueueState = "Queue cleared.";
        }

        public void ClearCompletedCraftQueueRows()
        {
            if (m_CraftQueueRunning)
                return;

            for (int i = m_CraftQueueRows.Count - 1; i >= 0; i--)
            {
                if (m_CraftQueueRows[i].IsComplete)
                    m_CraftQueueRows.RemoveAt(i);
            }

            if (GetSelectedCraftQueueRow() == null)
                m_SelectedCraftQueueRowId = -1;

            m_CraftQueueState = "Completed queue rows cleared.";
        }

        public void ResetCraftQueueRow(int rowId, out string message)
        {
            message = null;

            if (m_CraftQueueRunning)
            {
                message = "Stop the active queue before resetting rows.";
                return;
            }

            CraftQueueRow row = GetCraftQueueRow(rowId);

            if (row == null)
            {
                message = "That queue row is no longer available.";
                return;
            }

            row.Reset();
            message = "Queue row reset.";
            m_CraftQueueState = message;
        }

        public bool StartCraftQueue(BaseTool tool, out string message)
        {
            message = null;

            if (m_CraftQueueRunning)
            {
                message = "The crafting queue is already running.";
                return false;
            }

            if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
            {
                message = "You need a usable tool to start the crafting queue.";
                return false;
            }

            if (GetNextCraftQueueRow() == null)
            {
                message = "The crafting queue has no enabled incomplete rows.";
                return false;
            }

            m_CraftQueueTool = tool;
            m_CraftQueueRunning = true;
            m_CraftQueueStopRequested = false;
            m_ActiveCraftQueueRowId = -1;
            m_CraftQueueState = "Queue started.";
            ClearMakeAmountBatch();
            return true;
        }

        public void RequestCraftQueueStop()
        {
            if (m_CraftQueueRunning)
            {
                m_CraftQueueStopRequested = true;
                m_CraftQueueState = "Stopping after current attempt.";

                CraftQueueRow row = GetActiveCraftQueueRow();

                if (row != null)
                    row.State = CraftQueueRowState.StopRequested;
            }
        }

        public bool PrepareNextCraftQueueAttempt(BaseTool tool, out CraftQueueRow row, out string reason)
        {
            row = null;
            reason = null;

            if (!m_CraftQueueRunning)
                return false;

            if (m_CraftQueueStopRequested)
            {
                reason = "Stopped by player.";
                return false;
            }

            if (m_CraftQueueTool != null && tool != m_CraftQueueTool)
            {
                reason = "Stopped because the tool changed.";
                return false;
            }

            if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
            {
                reason = "Stopped because the tool has no uses remaining.";
                return false;
            }

            row = GetNextCraftQueueRow();

            if (row == null)
            {
                reason = "Completed.";
                return false;
            }

            m_ActiveCraftQueueRowId = row.RowId;
            m_SelectedCraftQueueRowId = row.RowId;
            row.State = CraftQueueRowState.Running;
            row.LastMessage = "Crafting.";
            m_CraftQueueState = String.Format("Crafting {0}.", row.DisplayName);
            return true;
        }

        public void RecordCraftQueueAttempt(CraftItem item, Type typeRes, BaseTool tool, bool success)
        {
            if (!m_CraftQueueRunning)
                return;

            CraftQueueRow row = GetActiveCraftQueueRow();

            if (row == null || row.CraftItem != item || row.TypeRes != typeRes || (m_CraftQueueTool != null && tool != m_CraftQueueTool))
                row = FindMatchingCraftQueueRow(item, typeRes);

            if (row == null)
                return;

            row.RecordAttempt(success);
            m_SelectedCraftQueueRowId = row.RowId;
            m_CraftQueueState = row.LastMessage;
        }

        public void EndCraftQueue(string reason)
        {
            if (!m_CraftQueueRunning && !m_CraftQueueStopRequested)
                return;

            CraftQueueRow row = GetActiveCraftQueueRow();

            if (row != null && row.State == CraftQueueRowState.Running)
                row.State = row.IsComplete ? CraftQueueRowState.Complete : CraftQueueRowState.Pending;

            m_CraftQueueRunning = false;
            m_CraftQueueStopRequested = false;
            m_ActiveCraftQueueRowId = -1;
            m_CraftQueueTool = null;
            m_CraftQueueState = reason;
        }

        public int GetCraftQueuePendingAttempts()
        {
            int total = 0;

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                CraftQueueRow row = m_CraftQueueRows[i];

                if (row.Enabled)
                    total += Math.Max(0, row.TargetAttempts - row.CompletedAttempts);
            }

            return total;
        }

        public int GetCraftQueueCompletedAttempts()
        {
            int total = 0;

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
                total += m_CraftQueueRows[i].CompletedAttempts;

            return total;
        }

        public int GetCraftQueueTargetAttempts()
        {
            int total = 0;

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                if (m_CraftQueueRows[i].Enabled)
                    total += m_CraftQueueRows[i].TargetAttempts;
            }

            return total;
        }

        public int GetCraftQueueSuccesses()
        {
            int total = 0;

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
                total += m_CraftQueueRows[i].Successes;

            return total;
        }

        public int GetCraftQueueFailures()
        {
            int total = 0;

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
                total += m_CraftQueueRows[i].Failures;

            return total;
        }

        public int GetCraftQueueDisabledRows()
        {
            int total = 0;

            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                if (!m_CraftQueueRows[i].Enabled)
                    total++;
            }

            return total;
        }

        private CraftQueueRow GetNextCraftQueueRow()
        {
            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                CraftQueueRow row = m_CraftQueueRows[i];

                if (row.CanRun)
                    return row;
            }

            return null;
        }

        private CraftQueueRow FindMatchingCraftQueueRow(CraftItem item, Type typeRes)
        {
            for (int i = 0; i < m_CraftQueueRows.Count; i++)
            {
                CraftQueueRow row = m_CraftQueueRows[i];

                if (row.CraftItem == item && row.TypeRes == typeRes && row.CanRun)
                    return row;
            }

            return null;
        }
    }

    internal static class CraftContainerUtility
    {
        private const int MaxDisplayNameLength = 14;
        private const int WorldContainerRange = 2;

        public static Container ResolveSourceContainer(Mobile from, CraftContext context, bool sendMessage)
        {
            return ResolveContainer(from, context, true, sendMessage);
        }

        public static Container ResolveDestinationContainer(Mobile from, CraftContext context, bool sendMessage)
        {
            return ResolveContainer(from, context, false, sendMessage);
        }

        public static bool TrySetSourceContainer(
            Mobile from,
            CraftContext context,
            Container container,
            out string message
        )
        {
            return TrySetContainer(from, context, container, true, out message);
        }

        public static bool TrySetDestinationContainer(
            Mobile from,
            CraftContext context,
            Container container,
            out string message
        )
        {
            return TrySetContainer(from, context, container, false, out message);
        }

        public static string GetSourceDisplayName(Mobile from, CraftContext context)
        {
            return GetDisplayName(from, context, true);
        }

        public static string GetDestinationDisplayName(Mobile from, CraftContext context)
        {
            return GetDisplayName(from, context, false);
        }

        public static bool PlaceCraftResult(Mobile from, CraftContext context, Item item)
        {
            if (from == null || item == null)
                return false;

            Container backpack = from.Backpack;
            Container destination = ResolveDestinationContainer(from, context, true);

            if (destination != null && destination != backpack)
            {
                if (destination.TryDropItem(from, item, false))
                    return true;

                from.SendMessage("The selected output bag cannot hold that item; using your backpack.");
            }

            return from.AddToBackpack(item);
        }

        private static Container ResolveContainer(
            Mobile from,
            CraftContext context,
            bool source,
            bool sendMessage
        )
        {
            Container backpack = GetBackpack(from);

            if (context == null)
                return backpack;

            Container selected = source ? context.SourceContainer : context.DestinationContainer;

            if (selected == null || selected == backpack)
                return backpack;

            string message;

            if (IsValidContainer(from, selected, out message))
                return selected;

            if (source)
                context.SourceContainer = null;
            else
                context.DestinationContainer = null;

            if (sendMessage && from != null)
            {
                if (source)
                    from.SendMessage("The selected source container is no longer available; using your backpack.");
                else
                    from.SendMessage("The selected output bag is no longer available; using your backpack.");
            }

            return backpack;
        }

        private static bool TrySetContainer(
            Mobile from,
            CraftContext context,
            Container container,
            bool source,
            out string message
        )
        {
            message = null;

            if (context == null)
            {
                message = "Crafting context is not available.";
                return false;
            }

            string validationMessage;

            if (!IsValidContainer(from, container, out validationMessage))
            {
                message = validationMessage;
                return false;
            }

            Container backpack = from.Backpack;

            if (container == backpack)
            {
                if (source)
                    context.SourceContainer = null;
                else
                    context.DestinationContainer = null;

                message = source
                    ? "Crafting source reset to your backpack."
                    : "Crafting output reset to your backpack.";
                return true;
            }

            if (source)
                context.SourceContainer = container;
            else
                context.DestinationContainer = container;

            message = String.Format(
                "{0} set to {1}.",
                source ? "Crafting source" : "Crafting output",
                GetContainerDisplayName(container)
            );
            return true;
        }

        private static bool IsValidContainer(Mobile from, Container container, out string message)
        {
            message = null;

            if (from == null || from.Deleted)
            {
                message = "You cannot select crafting containers right now.";
                return false;
            }

            Container backpack = from.Backpack;

            if (backpack == null)
            {
                message = "You must have a backpack to use crafting containers.";
                return false;
            }

            if (container == null || container.Deleted)
            {
                message = "That is not an available container.";
                return false;
            }

            if (container == backpack || container.IsChildOf(backpack))
                return true;

            if (container.RootParent is Mobile)
            {
                message = "Select your backpack, a bag in your backpack, or a nearby container you can access.";
                return false;
            }

            if (container.Map == null || container.Map != from.Map)
            {
                message = "That container is not available from here.";
                return false;
            }

            if (!from.InRange(container.GetWorldLocation(), WorldContainerRange))
            {
                message = "That container is too far away.";
                return false;
            }

            if (!HasContainerAccess(from, container))
            {
                message = "You cannot access that container.";
                return false;
            }

            return true;
        }

        private static bool HasContainerAccess(Mobile from, Container container)
        {
            if (container.IsAccessibleTo(from))
                return true;

            BaseHouse house = BaseHouse.FindHouseAt(container);

            if (house == null)
                return false;

            if (house.IsBanned(from))
                return false;

            SecureAccessResult secureAccess = house.CheckSecureAccess(from, container);

            switch (secureAccess)
            {
                case SecureAccessResult.Accessible:
                    return true;
                case SecureAccessResult.Inaccessible:
                    return false;
            }

            if (house.IsLockedDown(container))
                return house.IsCoOwner(from);

            return house.Public || house.HasAccess(from);
        }

        private static Container GetBackpack(Mobile from)
        {
            if (from == null || from.Deleted)
                return null;

            return from.Backpack;
        }

        private static string GetDisplayName(Mobile from, CraftContext context, bool source)
        {
            Container container = ResolveContainer(from, context, source, false);

            if (container == null)
                return "No backpack";

            if (from != null && container == from.Backpack)
                return "Backpack";

            return GetContainerDisplayName(container);
        }

        private static string GetContainerDisplayName(Container container)
        {
            if (container == null)
                return "Backpack";

            string name = container.Name;

            if (String.IsNullOrEmpty(name))
                name = container.GetType().Name;

            return CropText(name, MaxDisplayNameLength);
        }

        private static string CropText(string text, int maxLength)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            if (text.Length <= maxLength)
                return text;

            if (maxLength <= 3)
                return text.Substring(0, maxLength);

            return text.Substring(0, maxLength - 3) + "...";
        }
    }
}
