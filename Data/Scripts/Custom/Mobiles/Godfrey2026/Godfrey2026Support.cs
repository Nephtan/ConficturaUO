using Server;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    internal static class Godfrey2026Support
    {
        public static void ApplyProfile(BaseCreature creature, string name, string title, int hits, int minDamage, int maxDamage)
        {
            ApplyIdentity(creature, name, title);
            creature.HitsMaxSeed = hits;
            creature.SetHits(hits, hits);
            creature.SetDamage(minDamage, maxDamage);
        }

        public static void ApplyIdentity(BaseCreature creature, string name, string title)
        {
            creature.Name = name;
            creature.Title = title;
        }

        public static void SetResistance(BaseCreature creature, ResistanceType type)
        {
            creature.SetResistance(type, 100, 100);
        }

        public static void AddMortalStrike(BaseCreature creature)
        {
            XmlAttach.AttachTo(creature, new XmlWeaponAbility("MortalStrike"));
        }

        public static void AddGodfreyLoot(BaseCreature creature, int amount)
        {
            if (amount > 0)
            {
                creature.AddLoot(LootPack.SuperBoss, amount);
            }
        }
    }
}
