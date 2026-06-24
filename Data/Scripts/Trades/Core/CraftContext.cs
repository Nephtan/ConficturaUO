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
        private Container m_SourceContainer;
        private Container m_DestinationContainer;

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

        public void ResetCraftContainers()
        {
            m_SourceContainer = null;
            m_DestinationContainer = null;
        }
    }

    internal static class CraftContainerUtility
    {
        private const int MaxDisplayNameLength = 14;

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

            if (container != backpack && !container.IsChildOf(backpack))
            {
                message = "Select a container in your backpack.";
                return false;
            }

            return true;
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
