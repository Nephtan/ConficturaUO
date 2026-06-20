using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Server;
using Server.Commands;
using Server.Items;
using Server.Misc;

namespace Server.Custom.Confictura.LoreBooks
{
    public sealed class LoreBookCatalogEntry
    {
        public readonly string Id;
        public readonly int LegacyId;
        public readonly string Title;
        public readonly string LibraryTitle;
        public readonly string Author;
        public readonly int Cover;
        public readonly int ItemID;
        public readonly int Hue;
        public readonly bool HasLight;
        public readonly LightType Light;
        public readonly bool Library;
        public readonly bool Loot;
        public readonly int LootWeight;
        public readonly int Sort;
        public readonly string Text;

        public LoreBookCatalogEntry(
            string id,
            int legacyId,
            string title,
            string libraryTitle,
            string author,
            int cover,
            int itemID,
            int hue,
            bool hasLight,
            LightType light,
            bool library,
            bool loot,
            int lootWeight,
            int sort,
            string text
        )
        {
            Id = id;
            LegacyId = legacyId;
            Title = title;
            LibraryTitle = libraryTitle;
            Author = author;
            Cover = cover;
            ItemID = itemID;
            Hue = hue;
            HasLight = hasLight;
            Light = light;
            Library = library;
            Loot = loot;
            LootWeight = lootWeight;
            Sort = sort;
            Text = text;
        }

        public string DisplayTitle
        {
            get
            {
                if (LibraryTitle != null && LibraryTitle.Length > 0)
                {
                    return LibraryTitle;
                }

                return Title;
            }
        }
    }

    public sealed class PlayerLibraryEntry
    {
        public readonly string Id;
        public readonly int LegacyRow;
        public readonly int Sort;
        public readonly string ItemTypeName;
        public readonly string Title;
        public readonly int Reference;
        public readonly string LoreBookId;

        public PlayerLibraryEntry(string id, int legacyRow, string itemTypeName, string title, int reference)
            : this(id, legacyRow, legacyRow, itemTypeName, title, reference, null)
        {
        }

        public PlayerLibraryEntry(LoreBookCatalogEntry entry, int legacyRow)
            : this(entry.Id, legacyRow, entry.Sort, "LoreBook", entry.DisplayTitle, entry.LegacyId, entry.Id)
        {
        }

        private PlayerLibraryEntry(
            string id,
            int legacyRow,
            int sort,
            string itemTypeName,
            string title,
            int reference,
            string loreBookId
        )
        {
            Id = id;
            LegacyRow = legacyRow;
            Sort = sort;
            ItemTypeName = itemTypeName;
            Title = title;
            Reference = reference;
            LoreBookId = loreBookId;
        }

        public bool IsLoreBook
        {
            get { return LoreBookId != null && LoreBookId.Length > 0; }
        }
    }

    public static class LoreBookCatalog
    {
        public const string CatalogFile = "Data/Scripts/Custom/LoreBooks/LoreBooks.xml";

        private static readonly PlayerLibraryEntry[] m_StaticLibraryEntries = new PlayerLibraryEntry[]
        {
            new PlayerLibraryEntry("item.alchemical-elixirs", 2, "AlchemicalElixirs", "Alchemical Elixirs", 115),
            new PlayerLibraryEntry("item.alchemical-mixtures", 3, "AlchemicalMixtures", "Alchemical Mixtures", 116),
            new PlayerLibraryEntry("item.the-art-of-thievery", 5, "LearnStealingBook", "The Art of Thievery", 202),
            new PlayerLibraryEntry("item.barge-of-the-dead", 9, "BookofDeadClue", "Barge of the Dead", 104),
            new PlayerLibraryEntry("item.the-bottle-city", 12, "BookBottleCity", "The Bottle City", 103),
            new PlayerLibraryEntry("item.diary-on-lodoria", 26, "LodorBook", "Diary on Lodoria", 109),
            new PlayerLibraryEntry("item.elves-and-orks", 29, "CBookElvesandOrks", "Elves and Orks", 106),
            new PlayerLibraryEntry("item.gargoyle-secrets", 33, "LillyBook", "Gargoyle Secrets", 111),
            new PlayerLibraryEntry("item.the-golden-rangers", 36, "GoldenRangers", "The Golden Rangers", 114),
            new PlayerLibraryEntry("item.hidden-traps", 37, "LearnTraps", "Hidden Traps", 112),
            new PlayerLibraryEntry("item.journal-on-familiars", 40, "FamiliarClue", "Journal on Familiars", 108),
            new PlayerLibraryEntry("item.leather", 42, "LearnLeatherBook", "Leather", 207),
            new PlayerLibraryEntry("item.legend-of-the-sky-castle", 43, "GreyJournal", "Legend of the Sky Castle", 119),
            new PlayerLibraryEntry("item.lost-tribe-of-sosaria", 45, "CBookTheLostTribeofSosaria", "Lost Tribe of Sosaria", 110),
            new PlayerLibraryEntry("item.metals", 49, "LearnMetalBook", "Metals", 206),
            new PlayerLibraryEntry("item.reagents", 53, "LearnReagentsBook", "Reagents", 204),
            new PlayerLibraryEntry("item.reptile-scales", 54, "LearnScalesBook", "Reptile Scales", 203),
            new PlayerLibraryEntry("item.rune-magic", 56, "RuneJournal", "Rune Magic", 120),
            new PlayerLibraryEntry("item.sand-and-stone", 57, "LearnGraniteBook", "Sand and Stone", 208),
            new PlayerLibraryEntry("item.skinning-creatures", 58, "LearnMiscBook", "Skinning Creatures", 205),
            new PlayerLibraryEntry("item.skulls-and-shackles", 59, "SwordsAndShackles", "Skulls and Shackles", 300),
            new PlayerLibraryEntry("item.tailoring", 65, "LearnTailorBook", "Tailoring", 201),
            new PlayerLibraryEntry("item.tendrin-s-journal", 67, "TendrinsJournal", "Tendrin's Journal", 100),
            new PlayerLibraryEntry("item.titles-of-the-skilled", 69, "LearnTitles", "Titles of the Skilled", 113),
            new PlayerLibraryEntry("item.tomb-of-durmas", 70, "CBookTombofDurmas", "Tomb of Durmas", 105),
            new PlayerLibraryEntry("item.venom-and-poisons", 73, "BookOfPoisons", "Venom and Poisons", 117),
            new PlayerLibraryEntry("item.wizards-in-exile", 74, "MagestykcClueBook", "Wizards in Exile", 107),
            new PlayerLibraryEntry("item.wood", 75, "LearnWoodBook", "Wood", 200),
            new PlayerLibraryEntry("item.work-shoppes", 76, "WorkShoppes", "Work Shoppes", 118)
        };

        private static readonly LegacyLoreRow[] m_LegacyLoreRows = new LegacyLoreRow[]
        {
            new LegacyLoreRow(0, 1), new LegacyLoreRow(1, 44), new LegacyLoreRow(2, 6), new LegacyLoreRow(3, 7),
            new LegacyLoreRow(4, 10), new LegacyLoreRow(5, 11), new LegacyLoreRow(6, 15), new LegacyLoreRow(7, 17),
            new LegacyLoreRow(8, 18), new LegacyLoreRow(9, 19), new LegacyLoreRow(10, 23), new LegacyLoreRow(11, 22),
            new LegacyLoreRow(12, 20), new LegacyLoreRow(13, 25), new LegacyLoreRow(14, 41), new LegacyLoreRow(15, 30),
            new LegacyLoreRow(16, 31), new LegacyLoreRow(17, 32), new LegacyLoreRow(18, 14), new LegacyLoreRow(19, 38),
            new LegacyLoreRow(20, 46), new LegacyLoreRow(21, 66), new LegacyLoreRow(22, 16), new LegacyLoreRow(23, 68),
            new LegacyLoreRow(24, 52), new LegacyLoreRow(25, 34), new LegacyLoreRow(26, 35), new LegacyLoreRow(27, 13),
            new LegacyLoreRow(28, 60), new LegacyLoreRow(29, 61), new LegacyLoreRow(30, 62), new LegacyLoreRow(31, 63),
            new LegacyLoreRow(32, 8), new LegacyLoreRow(33, 21), new LegacyLoreRow(34, 50), new LegacyLoreRow(35, 71),
            new LegacyLoreRow(36, 28), new LegacyLoreRow(37, 27), new LegacyLoreRow(38, 47), new LegacyLoreRow(39, 48),
            new LegacyLoreRow(40, 51), new LegacyLoreRow(41, 72), new LegacyLoreRow(42, 24), new LegacyLoreRow(43, 64),
            new LegacyLoreRow(44, 55), new LegacyLoreRow(45, 4), new LegacyLoreRow(46, 39)
        };

        private static List<LoreBookCatalogEntry> m_Entries = new List<LoreBookCatalogEntry>();
        private static List<LoreBookCatalogEntry> m_LootEntries = new List<LoreBookCatalogEntry>();
        private static Dictionary<string, LoreBookCatalogEntry> m_ById =
            new Dictionary<string, LoreBookCatalogEntry>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<int, LoreBookCatalogEntry> m_ByLegacyId = new Dictionary<int, LoreBookCatalogEntry>();
        private static Dictionary<string, LoreBookCatalogEntry> m_ByTitle =
            new Dictionary<string, LoreBookCatalogEntry>(StringComparer.OrdinalIgnoreCase);

        private static bool m_Loaded;
        private static DateTime m_LastWriteTime;
        private static LoreBookReloadTimer m_ReloadTimer;

        public static void Initialize()
        {
            Reload();

            CommandSystem.Register(
                "ReloadLoreBooks",
                AccessLevel.GameMaster,
                new CommandEventHandler(ReloadLoreBooks_OnCommand)
            );

            if (m_ReloadTimer == null)
            {
                m_ReloadTimer = new LoreBookReloadTimer();
                m_ReloadTimer.Start();
            }
        }

        public static bool Reload()
        {
            return LoadXml(true);
        }

        public static LoreBookCatalogEntry GetEntry(string id)
        {
            EnsureLoaded();

            if (id == null || id.Length == 0)
            {
                return null;
            }

            LoreBookCatalogEntry entry;
            if (m_ById.TryGetValue(id, out entry))
            {
                return entry;
            }

            return null;
        }

        public static LoreBookCatalogEntry GetEntryByLegacyId(int legacyId)
        {
            EnsureLoaded();

            LoreBookCatalogEntry entry;
            if (m_ByLegacyId.TryGetValue(legacyId, out entry))
            {
                return entry;
            }

            return null;
        }

        public static LoreBookCatalogEntry GetEntryByTitle(string title)
        {
            EnsureLoaded();

            if (title == null || title.Length == 0)
            {
                return null;
            }

            LoreBookCatalogEntry entry;
            if (m_ByTitle.TryGetValue(title, out entry))
            {
                return entry;
            }

            return null;
        }

        public static LoreBookCatalogEntry GetRandomLootEntry()
        {
            EnsureLoaded();

            int totalWeight = 0;

            for (int i = 0; i < m_LootEntries.Count; i++)
            {
                totalWeight += Math.Max(0, m_LootEntries[i].LootWeight);
            }

            if (totalWeight <= 0)
            {
                return null;
            }

            int roll = Utility.Random(totalWeight);

            for (int i = 0; i < m_LootEntries.Count; i++)
            {
                LoreBookCatalogEntry entry = m_LootEntries[i];
                int weight = Math.Max(0, entry.LootWeight);

                if (roll < weight)
                {
                    return entry;
                }

                roll -= weight;
            }

            return null;
        }

        public static void ApplyToBook(LoreBook book, LoreBookCatalogEntry entry)
        {
            if (book == null || entry == null)
            {
                return;
            }

            book.BookRegion = null;
            book.BookMap = null;
            book.BookWorld = null;
            book.BookItem = entry.Id;
            book.BookTrue = 1;
            book.BookPower = 0;

            book.ItemID = entry.ItemID > 0 ? entry.ItemID : RandomThings.GetRandomBookItemID();
            book.Hue = entry.Hue >= 0 ? entry.Hue : Utility.RandomColor(0);
            book.Light = entry.HasLight ? entry.Light : LightType.Empty;
            DynamicBook.SetBookCover(entry.Cover, book);

            ApplyText(book, entry);
        }

        public static bool ApplyText(LoreBook book)
        {
            if (book == null)
            {
                return false;
            }

            LoreBookCatalogEntry entry = GetEntry(book.BookItem);

            if (entry == null)
            {
                entry = GetEntryByTitle(book.BookTitle);
            }

            if (entry == null)
            {
                entry = GetEntryByTitle(book.Name);
            }

            if (entry == null)
            {
                return false;
            }

            ApplyText(book, entry);
            return true;
        }

        public static List<PlayerLibraryEntry> GetLibraryEntries()
        {
            EnsureLoaded();

            List<PlayerLibraryEntry> entries = new List<PlayerLibraryEntry>();

            for (int i = 0; i < m_StaticLibraryEntries.Length; i++)
            {
                entries.Add(m_StaticLibraryEntries[i]);
            }

            for (int i = 0; i < m_Entries.Count; i++)
            {
                LoreBookCatalogEntry loreEntry = m_Entries[i];

                if (loreEntry.Library)
                {
                    entries.Add(new PlayerLibraryEntry(loreEntry, GetLegacyLibraryRow(loreEntry.LegacyId)));
                }
            }

            entries.Sort(CompareLibraryEntries);

            return entries;
        }

        public static string[] GetAllLibraryIds()
        {
            List<PlayerLibraryEntry> entries = GetLibraryEntries();
            string[] ids = new string[entries.Count];

            for (int i = 0; i < entries.Count; i++)
            {
                ids[i] = entries[i].Id;
            }

            return ids;
        }

        public static PlayerLibraryEntry GetLibraryEntry(string id)
        {
            if (id == null || id.Length == 0)
            {
                return null;
            }

            List<PlayerLibraryEntry> entries = GetLibraryEntries();

            for (int i = 0; i < entries.Count; i++)
            {
                if (String.Compare(entries[i].Id, id, true) == 0)
                {
                    return entries[i];
                }
            }

            return null;
        }

        public static PlayerLibraryEntry GetLibraryEntryByLegacyRow(int row)
        {
            List<PlayerLibraryEntry> entries = GetLibraryEntries();

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].LegacyRow == row)
                {
                    return entries[i];
                }
            }

            return null;
        }

        public static PlayerLibraryEntry GetLibraryEntryForItem(Item item)
        {
            if (item == null)
            {
                return null;
            }

            LoreBook loreBook = item as LoreBook;

            if (loreBook != null)
            {
                LoreBookCatalogEntry loreEntry = GetEntry(loreBook.BookItem);

                if (loreEntry == null)
                {
                    loreEntry = GetEntryByTitle(loreBook.BookTitle);
                }

                if (loreEntry == null)
                {
                    loreEntry = GetEntryByTitle(loreBook.Name);
                }

                if (loreEntry != null)
                {
                    return GetLibraryEntry(loreEntry.Id);
                }
            }

            string typeName = item.GetType().Name;
            List<PlayerLibraryEntry> entries = GetLibraryEntries();

            for (int i = 0; i < entries.Count; i++)
            {
                PlayerLibraryEntry entry = entries[i];

                if (!entry.IsLoreBook && String.Compare(entry.ItemTypeName, typeName, true) == 0)
                {
                    return entry;
                }
            }

            for (int i = 0; i < entries.Count; i++)
            {
                PlayerLibraryEntry entry = entries[i];

                if (String.Compare(entry.Title, item.Name, true) == 0)
                {
                    return entry;
                }
            }

            return null;
        }

        private static void ApplyText(LoreBook book, LoreBookCatalogEntry entry)
        {
            book.BookItem = entry.Id;
            book.BookTitle = entry.Title;
            book.Name = entry.Title;
            book.BookAuthor = entry.Author;
            book.BookText = entry.Text;
            book.InvalidateProperties();
        }

        private static void EnsureLoaded()
        {
            if (!m_Loaded)
            {
                LoadXml(false);
            }
        }

        private static bool LoadXml(bool verbose)
        {
            if (!File.Exists(CatalogFile))
            {
                if (verbose)
                {
                    Console.WriteLine("LoreBooks: Missing catalog file: {0}", CatalogFile);
                }

                return false;
            }

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(CatalogFile);

                XmlElement root = document["LoreBooks"];

                if (root == null)
                {
                    throw new InvalidOperationException("Missing LoreBooks root element.");
                }

                List<LoreBookCatalogEntry> entries = new List<LoreBookCatalogEntry>();
                List<LoreBookCatalogEntry> lootEntries = new List<LoreBookCatalogEntry>();
                Dictionary<string, LoreBookCatalogEntry> byId =
                    new Dictionary<string, LoreBookCatalogEntry>(StringComparer.OrdinalIgnoreCase);
                Dictionary<int, LoreBookCatalogEntry> byLegacyId = new Dictionary<int, LoreBookCatalogEntry>();
                Dictionary<string, LoreBookCatalogEntry> byTitle =
                    new Dictionary<string, LoreBookCatalogEntry>(StringComparer.OrdinalIgnoreCase);

                foreach (XmlNode child in root.ChildNodes)
                {
                    XmlElement element = child as XmlElement;

                    if (element == null || element.Name != "Book")
                    {
                        continue;
                    }

                    LoreBookCatalogEntry entry = ParseEntry(element);

                    if (entry == null)
                    {
                        continue;
                    }

                    if (byId.ContainsKey(entry.Id))
                    {
                        Console.WriteLine("LoreBooks: Duplicate id \"{0}\" ignored.", entry.Id);
                        continue;
                    }

                    entries.Add(entry);
                    byId[entry.Id] = entry;

                    if (entry.LegacyId >= 0 && !byLegacyId.ContainsKey(entry.LegacyId))
                    {
                        byLegacyId[entry.LegacyId] = entry;
                    }

                    if (entry.Title != null && entry.Title.Length > 0 && !byTitle.ContainsKey(entry.Title))
                    {
                        byTitle[entry.Title] = entry;
                    }

                    if (entry.LibraryTitle != null && entry.LibraryTitle.Length > 0 && !byTitle.ContainsKey(entry.LibraryTitle))
                    {
                        byTitle[entry.LibraryTitle] = entry;
                    }

                    if (entry.Loot && entry.LootWeight > 0)
                    {
                        lootEntries.Add(entry);
                    }
                }

                if (entries.Count == 0)
                {
                    throw new InvalidOperationException("Catalog contained no valid books.");
                }

                entries.Sort(CompareLoreEntries);

                m_Entries = entries;
                m_LootEntries = lootEntries;
                m_ById = byId;
                m_ByLegacyId = byLegacyId;
                m_ByTitle = byTitle;
                m_Loaded = true;
                m_LastWriteTime = File.GetLastWriteTime(CatalogFile);

                if (verbose)
                {
                    Console.WriteLine("LoreBooks: Loaded {0} books from {1}.", entries.Count, CatalogFile);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("LoreBooks: Failed to load {0}: {1}", CatalogFile, e.Message);
                return false;
            }
        }

        private static LoreBookCatalogEntry ParseEntry(XmlElement element)
        {
            string id = GetString(element, "id", "");
            string title = GetString(element, "title", "");
            string author = GetString(element, "author", "");
            string text = GetText(element);

            if (id.Length == 0 || title.Length == 0 || text.Length == 0)
            {
                Console.WriteLine("LoreBooks: Entry with missing id, title, or text ignored.");
                return null;
            }

            int legacyId = GetInt(element, "legacyId", -1);
            string libraryTitle = GetString(element, "libraryTitle", "");
            int cover = GetInt(element, "cover", 0);
            int itemID = GetInt(element, "itemID", 0);
            int hue = GetInt(element, "hue", -1);
            bool library = GetBool(element, "library", true);
            bool loot = GetBool(element, "loot", true);
            int lootWeight = GetInt(element, "lootWeight", 1);
            int sort = GetInt(element, "sort", legacyId >= 0 ? legacyId : 100000);
            LightType light = LightType.Empty;
            bool hasLight = false;
            string lightValue = GetString(element, "light", "");

            if (lightValue.Length > 0)
            {
                try
                {
                    light = (LightType)Enum.Parse(typeof(LightType), lightValue, true);
                    hasLight = true;
                }
                catch
                {
                    Console.WriteLine("LoreBooks: Invalid light \"{0}\" on \"{1}\" ignored.", lightValue, id);
                }
            }

            return new LoreBookCatalogEntry(
                id,
                legacyId,
                title,
                libraryTitle,
                author,
                cover,
                itemID,
                hue,
                hasLight,
                light,
                library,
                loot,
                lootWeight,
                sort,
                text
            );
        }

        private static string GetText(XmlElement element)
        {
            XmlElement textElement = element["Text"];

            if (textElement == null)
            {
                return "";
            }

            return textElement.InnerText.Trim();
        }

        private static string GetString(XmlElement element, string name, string defaultValue)
        {
            XmlAttribute attribute = element.Attributes[name];

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.Value.Trim();
        }

        private static int GetInt(XmlElement element, string name, int defaultValue)
        {
            string value = GetString(element, name, "");

            if (value.Length == 0)
            {
                return defaultValue;
            }

            try
            {
                if (value.StartsWith("0x") || value.StartsWith("0X"))
                {
                    return Int32.Parse(value.Substring(2), NumberStyles.HexNumber);
                }

                return Int32.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                Console.WriteLine("LoreBooks: Invalid integer \"{0}\" for {1}.", value, name);
                return defaultValue;
            }
        }

        private static bool GetBool(XmlElement element, string name, bool defaultValue)
        {
            string value = GetString(element, name, "");

            if (value.Length == 0)
            {
                return defaultValue;
            }

            if (String.Compare(value, "true", true) == 0 || value == "1" || String.Compare(value, "yes", true) == 0)
            {
                return true;
            }

            if (String.Compare(value, "false", true) == 0 || value == "0" || String.Compare(value, "no", true) == 0)
            {
                return false;
            }

            return defaultValue;
        }

        private static int CompareLoreEntries(LoreBookCatalogEntry left, LoreBookCatalogEntry right)
        {
            int result = left.Sort.CompareTo(right.Sort);

            if (result != 0)
            {
                return result;
            }

            return String.Compare(left.Title, right.Title, true);
        }

        private static int CompareLibraryEntries(PlayerLibraryEntry left, PlayerLibraryEntry right)
        {
            int result = left.Sort.CompareTo(right.Sort);

            if (result != 0)
            {
                return result;
            }

            return String.Compare(left.Title, right.Title, true);
        }

        private static int GetLegacyLibraryRow(int legacyId)
        {
            for (int i = 0; i < m_LegacyLoreRows.Length; i++)
            {
                if (m_LegacyLoreRows[i].LegacyId == legacyId)
                {
                    return m_LegacyLoreRows[i].LibraryRow;
                }
            }

            return 0;
        }

        private static void ReloadLoreBooks_OnCommand(CommandEventArgs e)
        {
            bool loaded = Reload();

            if (e.Mobile != null)
            {
                e.Mobile.SendMessage(loaded ? "Lore book catalog reloaded." : "Lore book catalog reload failed; keeping the previous catalog.");
            }
        }

        private struct LegacyLoreRow
        {
            public readonly int LegacyId;
            public readonly int LibraryRow;

            public LegacyLoreRow(int legacyId, int libraryRow)
            {
                LegacyId = legacyId;
                LibraryRow = libraryRow;
            }
        }

        private sealed class LoreBookReloadTimer : Timer
        {
            public LoreBookReloadTimer()
                : base(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15))
            {
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                if (!File.Exists(CatalogFile))
                {
                    return;
                }

                DateTime checkTime = File.GetLastWriteTime(CatalogFile);

                if (checkTime > m_LastWriteTime)
                {
                    Console.WriteLine("LoreBooks: rereading catalog file");
                    Reload();
                }
            }
        }
    }
}
