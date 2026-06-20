using System;
using System.Collections.Generic;
using Server;
using Server.Custom.Confictura.LoreBooks;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class MyLibrary : Gump
    {
        private const int RowsPerColumn = 23;
        private const int Columns = 4;
        private const int EntriesPerPage = RowsPerColumn * Columns;
        private const int EntryButtonBase = 1;
        private const int PreviousPageButton = 9001;
        private const int NextPageButton = 9002;

        public int m_Origin;
        private int m_Page;

        public MyLibrary(Mobile from, int source)
            : this(from, source, 0)
        {
        }

        public MyLibrary(Mobile from, int source, int page)
            : base(50, 50)
        {
            m_Origin = source;

            if (page < 0)
            {
                page = 0;
            }

            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                PlayerSettings.SetAllLibraryConfig(from);
            }

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            string color = "#ddbc4b";

            AddPage(0);
            AddImage(0, 0, 9546, PlayerSettings.GetGumpHue(from));
            AddHtml(12, 12, 200, 20, @"<BODY><BASEFONT Color=" + color + ">LIBRARY</BASEFONT></BODY>", false, false);
            AddButton(869, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

            List<LibraryDisplayEntry> entries = GetDisplayEntries(from);
            int totalPages = Math.Max(1, (entries.Count + EntriesPerPage - 1) / EntriesPerPage);

            if (page >= totalPages)
            {
                page = totalPages - 1;
            }

            m_Page = page;

            int start = m_Page * EntriesPerPage;
            int end = Math.Min(entries.Count, start + EntriesPerPage);

            for (int i = start; i < end; i++)
            {
                AddDisplayEntry(entries[i], i - start, color);
            }

            AddNavigation(totalPages, color);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null)
            {
                return;
            }

            from.CloseGump(typeof(MyLibrary));
            int button = info.ButtonID;

            if (button == PreviousPageButton)
            {
                from.SendGump(new MyLibrary(from, m_Origin, m_Page - 1));
                return;
            }

            if (button == NextPageButton)
            {
                from.SendGump(new MyLibrary(from, m_Origin, m_Page + 1));
                return;
            }

            if (button >= EntryButtonBase && button < EntryButtonBase + EntriesPerPage)
            {
                List<LibraryDisplayEntry> entries = GetDisplayEntries(from);
                int index = (m_Page * EntriesPerPage) + (button - EntryButtonBase);

                from.SendGump(new MyLibrary(from, m_Origin, m_Page));

                if (index >= 0 && index < entries.Count)
                {
                    OpenEntry(from, entries[index]);
                }
                else
                {
                    from.SendSound(0x4A);
                }

                return;
            }

            if (m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 1));
            }
            else
            {
                from.SendSound(0x4A);
            }
        }

        public static void readBook(Item book, Mobile m)
        {
            if (book == null || m == null)
            {
                return;
            }

            bool effect = false;
            PlayerLibraryEntry entry = LoreBookCatalog.GetLibraryEntryForItem(book);

            if (entry != null && !PlayerSettings.GetLibraryConfig(m, entry.Id))
            {
                PlayerSettings.SetLibraryConfig(m, entry.Id);
                effect = true;
            }

            if (effect)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                    0x376A,
                    9,
                    32,
                    0,
                    0,
                    5024,
                    0
                );
                m.SendSound(0x65C);
                m.SendMessage(book.Name + " has been added to your library.");
            }
        }

        private void AddDisplayEntry(LibraryDisplayEntry entry, int slot, string color)
        {
            int column = slot / RowsPerColumn;
            int row = slot % RowsPerColumn;
            int x = 16 + (column * 235);
            int y = 52 + (row * 30);

            AddButton(x, y, 4011, 4011, EntryButtonBase + slot, GumpButtonType.Reply, 0);
            AddHtml(
                x + 38,
                y,
                200,
                20,
                @"<BODY><BASEFONT Color=" + color + ">" + entry.Title + "</BASEFONT></BODY>",
                false,
                false
            );
        }

        private void AddNavigation(int totalPages, string color)
        {
            int y = 52 + (RowsPerColumn * 30);

            if (m_Page > 0)
            {
                AddButton(16, y, 4011, 4011, PreviousPageButton, GumpButtonType.Reply, 0);
                AddHtml(54, y, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Previous</BASEFONT></BODY>", false, false);
            }

            AddHtml(
                370,
                y,
                170,
                20,
                @"<BODY><CENTER><BASEFONT Color=" + color + ">Page " + (m_Page + 1) + " of " + totalPages + "</BASEFONT></CENTER></BODY>",
                false,
                false
            );

            if (m_Page + 1 < totalPages)
            {
                AddButton(720, y, 4011, 4011, NextPageButton, GumpButtonType.Reply, 0);
                AddHtml(758, y, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Next</BASEFONT></BODY>", false, false);
            }
        }

        private static List<LibraryDisplayEntry> GetDisplayEntries(Mobile from)
        {
            List<LibraryDisplayEntry> entries = new List<LibraryDisplayEntry>();

            entries.Add(new LibraryDisplayEntry(400, "Basics"));

            if (from.RaceID > 0)
            {
                entries.Add(new LibraryDisplayEntry(401, "Creature Help"));
            }

            entries.Add(new LibraryDisplayEntry(402, "Fame & Karma"));
            entries.Add(new LibraryDisplayEntry(403, "Item Properties"));
            entries.Add(new LibraryDisplayEntry(404, "Skills"));
            entries.Add(new LibraryDisplayEntry(405, "Weapon Abilities"));

            string[] unlockedIds = PlayerSettings.GetLibraryConfigIds(from);
            List<PlayerLibraryEntry> libraryEntries = LoreBookCatalog.GetLibraryEntries();

            for (int i = 0; i < libraryEntries.Count; i++)
            {
                PlayerLibraryEntry entry = libraryEntries[i];

                if (ContainsLibraryId(unlockedIds, entry.Id))
                {
                    entries.Add(new LibraryDisplayEntry(entry));
                }
            }

            return entries;
        }

        private static bool ContainsLibraryId(string[] ids, string id)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (String.Compare(ids[i], id, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static void OpenEntry(Mobile from, LibraryDisplayEntry entry)
        {
            if (entry.IsHelp)
            {
                OpenHelpEntry(from, entry.HelpButton);
                return;
            }

            OpenLibraryEntry(from, entry.LibraryEntry);
        }

        private static void OpenHelpEntry(Mobile from, int button)
        {
            if (button == 400)
            {
                from.CloseGump(typeof(BasicsGump));
                from.SendGump(new BasicsGump(from, 0));
            }
            else if (button == 401)
            {
                from.CloseGump(typeof(CreatureHelpGump));
                from.SendGump(new CreatureHelpGump(from, 0));
            }
            else if (button == 402)
            {
                from.CloseGump(typeof(FameKarma));
                from.SendGump(new FameKarma(from, 0));
            }
            else if (button == 403)
            {
                from.CloseGump(typeof(ItemPropsGump));
                from.SendGump(new ItemPropsGump(from, 0));
            }
            else if (button == 404)
            {
                from.CloseGump(typeof(NewSkillsGump));
                from.SendGump(new NewSkillsGump(from, 0));
            }
            else
            {
                from.CloseGump(typeof(WeaponAbilityBook.AbilityBookGump));
                from.SendGump(new WeaponAbilityBook.AbilityBookGump(from));
            }
        }

        private static void OpenLibraryEntry(Mobile from, PlayerLibraryEntry entry)
        {
            if (entry == null)
            {
                from.SendSound(0x4A);
                return;
            }

            Item item = null;

            try
            {
                Type itemType = ScriptCompiler.FindTypeByName(entry.ItemTypeName);

                if (itemType == null)
                {
                    from.SendMessage("That library entry could not be opened.");
                    Console.WriteLine("MyLibrary: Could not find item type {0}.", entry.ItemTypeName);
                    return;
                }

                item = Activator.CreateInstance(itemType) as Item;

                if (item == null)
                {
                    from.SendMessage("That library entry could not be opened.");
                    Console.WriteLine("MyLibrary: Type {0} did not create an item.", entry.ItemTypeName);
                    return;
                }

                item.Weight = -50.0;

                if (entry.IsLoreBook)
                {
                    LoreBook lore = item as LoreBook;

                    if (lore != null)
                    {
                        lore.writeBook(entry.LoreBookId);
                    }
                }

                item.OnDoubleClick(from);
            }
            catch (Exception e)
            {
                from.SendMessage("That library entry could not be opened.");
                Console.WriteLine("MyLibrary: Failed to open {0}: {1}", entry.Title, e.Message);
            }
            finally
            {
                if (item != null)
                {
                    item.Delete();
                }
            }
        }

        private sealed class LibraryDisplayEntry
        {
            public readonly int HelpButton;
            public readonly PlayerLibraryEntry LibraryEntry;
            public readonly string Title;

            public LibraryDisplayEntry(int helpButton, string title)
            {
                HelpButton = helpButton;
                Title = title;
            }

            public LibraryDisplayEntry(PlayerLibraryEntry entry)
            {
                LibraryEntry = entry;
                Title = entry.Title;
            }

            public bool IsHelp
            {
                get { return HelpButton > 0; }
            }
        }
    }
}
