using System;
using Server.Custom.Confictura.Mobiles;
using Server.Custom.Confictura.GardenGolems.Systems;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Confictura.GardenGolems.Gumps
{
    /// <summary>
    ///     Simplified caretaker interface that borrows layout cues from the gardening gumps while focusing on
    ///     the actions currently implemented for the garden golem prototype.
    /// </summary>
    public class GardenGolemCaretakerGump : Gump
    {
        private readonly GardenGolem m_Golem;

        public GardenGolemCaretakerGump(GardenGolem golem)
            : base(50, 50)
        {
            if (golem == null)
                throw new ArgumentNullException("golem");

            m_Golem = golem;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(0, 0, 320, 240, 0x13BE);
            AddLabel(20, 15, 0x44, "Garden Golem Caretaker");

            GardenGolemPlanterState state = golem.PlanterState;

            string cultivated = state.HasSeed ? state.DisplaySeedLabel : "No seed loaded";
            AddLabel(20, 50, 0x480, string.Format("Cultivating: {0}", cultivated));
            AddLabel(20, 80, 0x480, string.Format("Moisture: {0}/{1}", state.Moisture, GardenGolemPlanterState.MaxMoisture));
            AddLabel(20, 110, 0x480, string.Format("Infestation: {0}/{1}", state.Infestation, GardenGolemPlanterState.MaxInfestation));

            string harvestStatus = state.HasReadyYield ? string.Format("Harvest ready ({0}/3)", state.StoredYields) : string.Format("Growing ({0}/3 stored)", state.StoredYields);
            AddLabel(20, 140, 0x480, string.Format("Growth: {0}", harvestStatus));
            AddLabel(20, 170, 0x480, string.Format("Fertility bonus: +{0}", state.FertilityBonus));
            AddLabel(20, 200, 0x480, string.Format("Next check: {0}", state.NextGrowth.ToShortTimeString()));

            AddButton(220, 60, 0x837, 0x838, 1, GumpButtonType.Reply, 0);
            AddLabel(250, 60, 0x44, "Water");

            AddButton(220, 100, 0x837, 0x838, 2, GumpButtonType.Reply, 0);
            AddLabel(250, 100, 0x44, "Treat");

            AddButton(220, 140, 0x837, 0x838, 3, GumpButtonType.Reply, 0);
            AddLabel(250, 140, 0x44, "Harvest");

            AddButton(220, 180, 0x837, 0x838, 4, GumpButtonType.Reply, 0);
            AddLabel(250, 180, 0x44, "Eject seed");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Golem == null || m_Golem.Deleted)
                return;

            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1:
                    m_Golem.HandleWaterRequest(from);
                    break;
                case 2:
                    m_Golem.HandleTreatmentRequest(from);
                    break;
                case 3:
                    m_Golem.HandleHarvestRequest(from);
                    break;
                case 4:
                    m_Golem.HandleEjectSeed(from);
                    break;
            }

            if (from != null && !m_Golem.Deleted && info.ButtonID != 0)
                from.SendGump(new GardenGolemCaretakerGump(m_Golem));
        }
    }
}