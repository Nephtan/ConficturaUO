using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Engines.Craft
{
    public enum CraftMarkOption
    {
        MarkItem,
        DoNotMark,
        PromptForMark
    }

    public class CraftContext
    {
        private List<CraftItem> m_Items;
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

        public List<CraftItem> Items
        {
            get { return m_Items; }
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

        public CraftContext()
        {
            m_Items = new List<CraftItem>();
            m_LastResourceIndex = -1;
            m_LastResourceIndex2 = -1;
            m_LastGroupIndex = -1;
            m_MakeAmount = 1;
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
    }
}
