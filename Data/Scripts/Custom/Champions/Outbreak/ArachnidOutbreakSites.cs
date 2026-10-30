using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Server.Custom.Confictura.Champions
{
    public static class ArachnidOutbreakSites
    {
        // Keep approved encounter centers tied to their permanent housing policy.
        private static readonly ReadOnlyCollection<ArachnidOutbreakSite> m_Candidates =
            CreateReviewedCandidates();

        private static ReadOnlyCollection<ArachnidOutbreakSite> CreateReviewedCandidates()
        {
            List<ArachnidOutbreakSite> candidates = new List<ArachnidOutbreakSite>();
            foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
            {
                if (reserve.Reviewed)
                    candidates.Add(new ArachnidOutbreakSite(reserve.Id, reserve.Center.X, reserve.Center.Y, true));
            }
            return candidates.AsReadOnly();
        }

        public static ReadOnlyCollection<ArachnidOutbreakSite> Candidates
        {
            get { return m_Candidates; }
        }

        public static ArachnidOutbreakSite Find(string id)
        {
            foreach (ArachnidOutbreakSite site in m_Candidates)
                if (String.Equals(site.Id, id, StringComparison.Ordinal))
                    return site;
            return null;
        }
    }
}
