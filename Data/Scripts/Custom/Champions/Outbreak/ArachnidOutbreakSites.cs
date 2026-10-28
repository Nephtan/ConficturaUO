using System;
using System.Collections.ObjectModel;

namespace Server.Custom.Confictura.Champions
{
    public static class ArachnidOutbreakSites
    {
        // The original six terrain candidates were retired: all allowed housing.
        // Add only sites with a permanent housing prohibition across the entire
        // footprint/buffer, then copied-world occupancy and native-client review.
        private static readonly ReadOnlyCollection<ArachnidOutbreakSite> m_Candidates =
            Array.AsReadOnly(new ArachnidOutbreakSite[]
            {
            });

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
