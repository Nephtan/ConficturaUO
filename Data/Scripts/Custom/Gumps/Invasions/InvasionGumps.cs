using System;
using System.Collections.Generic;
using System.Text;
using Server.Custom.Confictura.Invasions;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Confictura.Gumps.Invasions
{
    public sealed class InvasionAdminGump : Gump
    {
        private enum ButtonId
        {
            Close = 0,
            ToggleEngine = 1,
            ValidateAll = 2,
            ShowLegacyScan = 3,
            PromptLegacyCleanup = 4,
            ConfirmLegacyCleanup = 5
        }

        private const int CityButtonBase = 100;
        private const int CityButtonStride = 10;
        private const int ToggleCityOffset = 1;
        private const int AddCorruptionOffset = 2;
        private const int AdvanceOffset = 3;
        private const int PauseOffset = 4;
        private const int AbortOffset = 5;
        private const int LiberateOffset = 6;

        private readonly int[] m_Sessions;
        private readonly int[] m_LegacySerials;
        private readonly bool m_ShowLegacy;
        private readonly bool m_ConfirmCleanup;

        public InvasionAdminGump(Mobile from, bool showLegacy, bool confirmCleanup)
            : base(40, 40)
        {
            from.CloseGump(typeof(InvasionAdminGump));
            m_ShowLegacy = showLegacy;
            m_ConfirmCleanup = confirmCleanup;
            m_Sessions = new int[3];
            List<Item> legacy = showLegacy ? InvasionService.GetLegacyInvasionObjects() : new List<Item>();
            m_LegacySerials = new int[legacy.Count];

            for (int i = 0; i < legacy.Count; ++i)
                m_LegacySerials[i] = legacy[i].Serial.Value;

            AddPage(0);
            AddBackground(0, 0, 780, showLegacy ? 650 : 455, 9270);
            AddHtml(25, 20, 730, 25, "<BASEFONT COLOR=#E8D49A><CENTER>Organic City Invasions</CENTER></BASEFONT>", false, false);

            InvasionWorldState worldState = InvasionService.WorldState;
            string engineStatus = worldState != null && worldState.EngineEnabled ? "Enabled" : "Disabled";
            AddLabel(30, 55, 1152, "Engine: " + engineStatus + (InvasionService.DuplicateStateDetected ? " (DUPLICATE STATE BLOCK)" : String.Empty));
            AddButton(210, 54, 4005, 4007, (int)ButtonId.ToggleEngine, GumpButtonType.Reply, 0);
            AddLabel(245, 55, 1152, worldState != null && worldState.EngineEnabled ? "Disable" : "Enable");
            AddButton(340, 54, 4005, 4007, (int)ButtonId.ValidateAll, GumpButtonType.Reply, 0);
            AddLabel(375, 55, 1152, "Validate all");
            AddButton(485, 54, 4005, 4007, (int)ButtonId.ShowLegacyScan, GumpButtonType.Reply, 0);
            AddLabel(520, 55, 1152, "Legacy scan");

            int y = 95;

            for (int i = 0; i < 3; ++i)
            {
                InvasionCityId cityId = (InvasionCityId)i;
                InvasionCityState city = InvasionService.GetCityState(cityId);
                m_Sessions[i] = city == null ? 0 : city.Session;
                AddCityRow(cityId, city, y);
                y += 82;
            }

            if (m_ShowLegacy)
            {
                AddLabel(30, 350, 1152, "Legacy invasion objects found: " + legacy.Count);
                AddHtml(30, 375, 710, 145, BuildLegacyList(legacy), true, true);
                AddButton(30, 530, 4005, 4007, (int)ButtonId.PromptLegacyCleanup, GumpButtonType.Reply, 0);
                AddLabel(65, 531, 33, "Review cleanup confirmation");
            }

            if (m_ConfirmCleanup)
            {
                AddHtml(30, 560, 710, 35, "<BASEFONT COLOR=#FF7777>This deletes only the objects displayed in this confirmation. Their tracked spawns follow each legacy spawner's normal deletion behavior.</BASEFONT>", false, false);
                AddButton(30, 610, 4023, 4025, (int)ButtonId.ConfirmLegacyCleanup, GumpButtonType.Reply, 0);
                AddLabel(65, 611, 33, "Confirm displayed legacy cleanup");
            }

            AddButton(730, showLegacy ? 610 : 417, 4017, 4019, (int)ButtonId.Close, GumpButtonType.Reply, 0);
        }

        private static string BuildLegacyList(List<Item> legacy)
        {
            if (legacy.Count == 0)
                return "<BASEFONT COLOR=#CCCCCC>No legacy invasion objects are present.</BASEFONT>";

            StringBuilder builder = new StringBuilder();
            builder.Append("<BASEFONT COLOR=#CCCCCC>");

            for (int i = 0; i < legacy.Count; ++i)
            {
                Item item = legacy[i];
                builder.AppendFormat("0x{0:X8} {1} at {2} on {3}<BR>", item.Serial.Value, item.GetType().Name, item.Location, item.Map);
            }

            builder.Append("</BASEFONT>");
            return builder.ToString();
        }

        private void AddCityRow(InvasionCityId cityId, InvasionCityState city, int y)
        {
            int cityIndex = (int)cityId;
            int buttonBase = CityButtonBase + (cityIndex * CityButtonStride);
            string name = InvasionCities.Get(cityId).Name;

            if (city == null)
            {
                AddLabel(30, y, 33, name + ": unavailable");
                return;
            }

            AddImageTiled(25, y - 5, 730, 72, 2624);
            AddAlphaRegion(25, y - 5, 730, 72);
            AddLabel(35, y, 1152, String.Format("{0}: {1} | corruption {2}/100 | faction {3}", name, city.State, city.Corruption, InvasionService.GetFactionName(city.Faction)));
            AddLabel(35, y + 22, city.Enabled ? 68 : 33, String.Format("City {0}; validation {1}; session {2}; {3}", city.Enabled ? "enabled" : "disabled", city.ValidationPassed ? "passed" : "required", city.Session, city.StaffPaused ? "staff paused" : (city.IdlePaused ? "idle paused" : "running")));

            AddButton(35, y + 45, 4005, 4007, buttonBase + ToggleCityOffset, GumpButtonType.Reply, 0);
            AddLabel(65, y + 46, 1152, city.Enabled ? "Disable" : "Enable");
            AddButton(145, y + 45, 4005, 4007, buttonBase + AddCorruptionOffset, GumpButtonType.Reply, 0);
            AddLabel(175, y + 46, 1152, "+25 corruption");
            AddButton(290, y + 45, 4005, 4007, buttonBase + AdvanceOffset, GumpButtonType.Reply, 0);
            AddLabel(320, y + 46, 1152, "Advance");
            AddButton(390, y + 45, 4005, 4007, buttonBase + PauseOffset, GumpButtonType.Reply, 0);
            AddLabel(420, y + 46, 1152, city.StaffPaused ? "Resume" : "Pause");
            AddButton(500, y + 45, 4005, 4007, buttonBase + AbortOffset, GumpButtonType.Reply, 0);
            AddLabel(530, y + 46, 33, "Abort");
            AddButton(600, y + 45, 4005, 4007, buttonBase + LiberateOffset, GumpButtonType.Reply, 0);
            AddLabel(630, y + 46, 1152, "Liberate");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender == null ? null : sender.Mobile;

            if (from == null || from.Deleted || from.AccessLevel < AccessLevel.Administrator || info == null || info.ButtonID <= 0)
                return;

            if (info.ButtonID == (int)ButtonId.ToggleEngine)
            {
                bool desired = InvasionService.WorldState != null && !InvasionService.WorldState.EngineEnabled;
                string reason;

                if (!InvasionService.TrySetEngineEnabled(desired, out reason))
                    from.SendMessage(33, "Engine change refused: {0}", reason);
                else
                    InvasionService.RecordStaffAction(from, "set engine " + (desired ? "enabled" : "disabled"));
            }
            else if (info.ButtonID == (int)ButtonId.ValidateAll)
            {
                for (int i = 0; i < 3; ++i)
                {
                    string reason;

                    if (InvasionService.ValidateCity((InvasionCityId)i, out reason))
                    {
                        from.SendMessage("{0}: validation passed.", InvasionCities.Get((InvasionCityId)i).Name);
                        List<string> warnings = InvasionService.GetSpawnerValidationWarnings((InvasionCityId)i);

                        for (int j = 0; j < warnings.Count; ++j)
                            from.SendMessage(53, warnings[j]);
                    }
                    else
                        from.SendMessage(33, "{0}: {1}", InvasionCities.Get((InvasionCityId)i).Name, reason);
                }

                InvasionService.RecordStaffAction(from, "validated all cities");
            }
            else if (info.ButtonID == (int)ButtonId.ShowLegacyScan)
            {
                InvasionService.MarkLegacyScanReviewed();
                InvasionService.RecordStaffAction(from, "reviewed the legacy invasion scan");
                from.SendGump(new InvasionAdminGump(from, true, false));
                return;
            }
            else if (info.ButtonID == (int)ButtonId.PromptLegacyCleanup && m_ShowLegacy)
            {
                from.SendGump(new InvasionAdminGump(from, true, true));
                return;
            }
            else if (info.ButtonID == (int)ButtonId.ConfirmLegacyCleanup && m_ConfirmCleanup)
            {
                int removed = InvasionService.CleanupLegacyInvasionObjects(m_LegacySerials);
                from.SendMessage("Removed {0} confirmed legacy invasion objects.", removed);
                InvasionService.RecordStaffAction(from, "removed " + removed + " displayed legacy invasion objects");
            }
            else if (info.ButtonID >= CityButtonBase)
            {
                int encoded = info.ButtonID - CityButtonBase;
                int cityIndex = encoded / CityButtonStride;
                int action = encoded % CityButtonStride;

                if (cityIndex < 0 || cityIndex >= 3)
                    return;

                InvasionCityId cityId = (InvasionCityId)cityIndex;
                InvasionCityState city = InvasionService.GetCityState(cityId);

                if (city == null || (city.State != InvasionState.Dormant && city.Session != m_Sessions[cityIndex]))
                {
                    from.SendMessage(33, "That city state changed. Reopen the console.");
                    return;
                }

                HandleCityAction(from, cityId, city, action, m_Sessions[cityIndex]);
            }

            from.SendGump(new InvasionAdminGump(from, m_ShowLegacy, false));
        }

        private static void HandleCityAction(Mobile from, InvasionCityId cityId, InvasionCityState city, int action, int session)
        {
            string reason;

            switch (action)
            {
                case ToggleCityOffset:
                    bool cityDesired = !city.Enabled;

                    if (!InvasionService.TrySetCityEnabled(cityId, cityDesired, out reason))
                        from.SendMessage(33, "City change refused: {0}", reason);
                    else
                        InvasionService.RecordStaffAction(from, "set " + InvasionCities.Get(cityId).Name + " " + (cityDesired ? "enabled" : "disabled"));
                    break;
                case AddCorruptionOffset:
                    if (!InvasionService.AddTestCorruption(cityId, 25))
                        from.SendMessage(33, "Test corruption requires an enabled engine, enabled city, and Dormant state.");
                    else
                        InvasionService.RecordStaffAction(from, "added test corruption to " + InvasionCities.Get(cityId).Name);
                    break;
                case AdvanceOffset:
                    if (!InvasionService.ForceAdvance(cityId, session, out reason))
                        from.SendMessage(33, "Advance refused: {0}", reason);
                    else
                        InvasionService.RecordStaffAction(from, "advanced " + InvasionCities.Get(cityId).Name);
                    break;
                case PauseOffset:
                    if (!InvasionService.SetPaused(cityId, session, !city.StaffPaused))
                        from.SendMessage(33, "Pause change refused.");
                    else
                        InvasionService.RecordStaffAction(from, (city.StaffPaused ? "paused " : "resumed ") + InvasionCities.Get(cityId).Name);
                    break;
                case AbortOffset:
                    if (!InvasionService.Abort(cityId, session))
                        from.SendMessage(33, "Abort refused.");
                    else
                        InvasionService.RecordStaffAction(from, "aborted " + InvasionCities.Get(cityId).Name);
                    break;
                case LiberateOffset:
                    if (!InvasionService.ForceLiberate(cityId, session))
                        from.SendMessage(33, "Liberation is available only during the matching occupation session.");
                    else
                        InvasionService.RecordStaffAction(from, "forced liberation of " + InvasionCities.Get(cityId).Name);
                    break;
            }
        }
    }

    public sealed class InvasionInfoGump : Gump
    {
        public InvasionInfoGump(Mobile from)
            : base(100, 80)
        {
            from.CloseGump(typeof(InvasionInfoGump));
            AddPage(0);
            AddBackground(0, 0, 500, 350, 9270);
            AddHtml(20, 20, 460, 25, "<BASEFONT COLOR=#E8D49A><CENTER>City Invasions</CENTER></BASEFONT>", false, false);

            InvasionCityId currentCity;
            bool hasCurrent = InvasionService.TryGetConflictCity(from.Map, from.Location, out currentCity);
            int y = 60;

            for (int i = 0; i < 3; ++i)
            {
                InvasionCityId cityId = (InvasionCityId)i;
                InvasionCityState city = InvasionService.GetCityState(cityId);
                string stateText;

                if (city == null || !city.Enabled)
                    stateText = "quiet";
                else if (city.State == InvasionState.Dormant)
                    stateText = InvasionService.GetQualitativeCorruption(city.Corruption);
                else
                    stateText = city.State + (city.Faction == InvasionFaction.None ? String.Empty : " - " + InvasionService.GetFactionName(city.Faction));

                AddLabel(30, y, hasCurrent && currentCity == cityId ? 68 : 1152, InvasionCities.Get(cityId).Name + ": " + stateText);
                y += 30;
            }

            PlayerMobile player = from as PlayerMobile;

            if (hasCurrent && player != null)
            {
                AddLabel(30, 165, 1152, "Your side: " + InvasionService.GetSide(player, currentCity));
                AddLabel(30, 190, 1152, InvasionService.GetPlayerConflictStatus(player));
                AddHtml(30, 215, 430, 45, "<BASEFONT COLOR=#E8D49A>Objectives: " + InvasionService.GetObjectiveSummary(InvasionService.GetCityState(currentCity)) + "</BASEFONT>", false, false);
            }
            else
            {
                AddLabel(30, 165, 1152, "You are not inside an active conflict zone.");
            }

            AddHtml(30, 275, 430, 42, "<BASEFONT COLOR=#CCCCCC>During an active invasion, marked boundaries carry automatic conflict. Entry grace ends when you move more than three tiles or take an action.</BASEFONT>", false, false);
            AddButton(445, 315, 4017, 4019, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender == null ? null : sender.Mobile;

            if (from == null || from.Deleted || info == null || info.ButtonID <= 0)
                return;
        }
    }

    public sealed class InvasionFactionGump : Gump
    {
        private readonly InvasionCityId m_City;
        private readonly int m_Session;

        public InvasionFactionGump(Mobile from, InvasionCityId city, int session)
            : base(160, 120)
        {
            from.CloseGump(typeof(InvasionFactionGump));
            m_City = city;
            m_Session = session;
            AddPage(0);
            AddBackground(0, 0, 430, 245, 9270);
            AddHtml(20, 20, 390, 40, "<BASEFONT COLOR=#E8D49A><CENTER>Choose the force that will invade " + InvasionCities.Get(city).Name + "</CENTER></BASEFONT>", false, false);
            AddFactionButton(35, 80, 1, "Clockwork Dominion", "Durable constructs, machinery, and support engineers.");
            AddFactionButton(35, 125, 2, "Blood Court", "Lifesteal fighters, skirmishers, and curse casters.");
            AddFactionButton(35, 170, 3, "Abyssal Legion", "Demons, fire and poison pressure, and summoners.");
            AddButton(380, 210, 4017, 4019, 0, GumpButtonType.Reply, 0);
        }

        private void AddFactionButton(int x, int y, int buttonId, string name, string description)
        {
            AddButton(x, y, 4005, 4007, buttonId, GumpButtonType.Reply, 0);
            AddLabel(x + 35, y, 1152, name);
            AddLabel(x + 35, y + 20, 0x7A1, description);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender == null ? null : sender.Mobile;

            if (from == null || from.Deleted || info == null || info.ButtonID <= 0 || info.ButtonID > 3)
                return;

            InvasionFaction faction = (InvasionFaction)info.ButtonID;
            string reason;

            if (!InvasionService.TryStartRitual(from, m_City, faction, m_Session, out reason))
                from.SendMessage(33, "Ritual refused: {0}", reason);
        }
    }
}
