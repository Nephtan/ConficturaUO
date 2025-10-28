using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Craft
{
    public class CraftGumpItem : Gump
    {
        private Mobile m_From;
        private CraftSystem m_CraftSystem;
        private CraftItem m_CraftItem;
        private BaseTool m_Tool;

        private const int LabelHue = 0x480; // 0x384
        private const int RedLabelHue = 0x20;

        private const int LabelColor = 0x7FFF;
        private const int RedLabelColor = 0x6400;

        private const int GreyLabelColor = 0x3DEF;

        // Number of material entries that can be displayed on a single page without
        // overlapping other sections of the gump.
        private const int ResourcesPerPage = 4;

        private int m_OtherCount;

        public CraftGumpItem(
            Mobile from,
            CraftSystem craftSystem,
            CraftItem craftItem,
            BaseTool tool
        )
            : base(40, 40)
        {
            m_From = from;
            m_CraftSystem = craftSystem;
            m_CraftItem = craftItem;
            m_Tool = tool;

            from.CloseGump(typeof(CraftGump));
            from.CloseGump(typeof(CraftGumpItem));

            bool needsRecipe = (
                craftItem.Recipe != null
                && from is PlayerMobile
                && !((PlayerMobile)from).HasRecipe(craftItem.Recipe)
            );

            CraftContext context = m_CraftSystem.GetContext(m_From);

            CraftSubResCol res = (
                m_CraftItem.UseSubRes2 ? m_CraftSystem.CraftSubRes2 : m_CraftSystem.CraftSubRes
            );
            int resIndex = -1;

            if (context != null)
                resIndex = (
                    m_CraftItem.UseSubRes2 ? context.LastResourceIndex2 : context.LastResourceIndex
                );

            bool cropScroll =
                (m_CraftItem.Resources.Count > 1)
                && m_CraftItem.Resources.GetAt(m_CraftItem.Resources.Count - 1).ItemType
                    == typeofBlankScroll
                && typeofSpellScroll.IsAssignableFrom(m_CraftItem.ItemType);

            int resourcesPerPage = ResourcesPerPage;
            int totalResources = m_CraftItem.Resources.Count - (cropScroll ? 1 : 0);

            if (totalResources < 0)
                totalResources = 0;

            int totalPages = Math.Max(1, (int)Math.Ceiling(totalResources / (double)resourcesPerPage));
            bool retainedColorShown = false;

            // Build the gump one page at a time so long resource lists can be paged
            // through instead of being truncated after four entries.
            for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
            {
                if (pageIndex == 0)
                    AddPage(0);
                else
                    AddPage(pageIndex);

                BuildPage(
                    pageIndex,
                    totalPages,
                    needsRecipe,
                    res,
                    resIndex,
                    cropScroll,
                    resourcesPerPage,
                    totalResources,
                    ref retainedColorShown
                );
            }
        }

        // Builds a single page of the crafting gump, including navigation controls,
        // the per-page resource list, and the "Other" section messaging.
        private void BuildPage(
            int pageIndex,
            int totalPages,
            bool needsRecipe,
            CraftSubResCol res,
            int resIndex,
            bool cropScroll,
            int resourcesPerPage,
            int totalResources,
            ref bool retainedColorShown
        )
        {
            AddBackground(0, 0, 530, 417, 5054);
            AddImageTiled(10, 10, 510, 22, 2624);
            AddImageTiled(10, 37, 150, 148, 2624);
            AddImageTiled(165, 37, 355, 90, 2624);
            AddImageTiled(10, 190, 155, 22, 2624);
            AddImageTiled(10, 217, 150, 53, 2624);
            AddImageTiled(165, 132, 355, 80, 2624);
            AddImageTiled(10, 275, 155, 22, 2624);
            AddImageTiled(10, 302, 150, 53, 2624);
            AddImageTiled(165, 217, 355, 80, 2624);
            AddImageTiled(10, 360, 155, 22, 2624);
            AddImageTiled(165, 302, 355, 80, 2624);
            AddImageTiled(10, 387, 510, 22, 2624);
            AddAlphaRegion(10, 10, 510, 399);

            AddHtmlLocalized(170, 40, 150, 20, 1044053, LabelColor, false, false); // ITEM
            AddHtmlLocalized(10, 192, 150, 22, 1044054, LabelColor, false, false); // <CENTER>SKILLS</CENTER>
            AddHtmlLocalized(10, 277, 150, 22, 1044055, LabelColor, false, false); // <CENTER>MATERIALS</CENTER>
            AddHtmlLocalized(10, 362, 150, 22, 1044056, LabelColor, false, false); // <CENTER>OTHER</CENTER>

            if (totalPages > 1)
            {
                if (pageIndex > 0)
                    AddButton(170, 277, 4014, 4016, 0, GumpButtonType.Page, pageIndex - 1);

                if (pageIndex < totalPages - 1)
                    AddButton(480, 277, 4005, 4007, 0, GumpButtonType.Page, pageIndex + 1);

                // Let crafters know which materials page they are viewing.
                AddLabel(
                    340,
                    277,
                    LabelHue,
                    String.Format("Page {0}/{1}", pageIndex + 1, totalPages)
                );
            }

            if (m_CraftSystem.GumpTitleNumber > 0)
                AddHtmlLocalized(
                    10,
                    12,
                    510,
                    20,
                    m_CraftSystem.GumpTitleNumber,
                    LabelColor,
                    false,
                    false
                );
            else
                AddHtml(10, 12, 510, 20, m_CraftSystem.GumpTitleString, false, false);

            AddButton(15, 387, 4014, 4016, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(50, 390, 150, 18, 1044150, LabelColor, false, false); // BACK

            if (needsRecipe)
            {
                AddButton(270, 387, 4005, 4007, 0, GumpButtonType.Page, pageIndex);
                AddHtmlLocalized(305, 390, 150, 18, 1044151, GreyLabelColor, false, false); // MAKE NOW
            }
            else
            {
                AddButton(270, 387, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(305, 390, 150, 18, 1044151, LabelColor, false, false); // MAKE NOW
            }

            if (m_CraftItem.NameNumber > 0)
                AddHtmlLocalized(330, 40, 180, 18, m_CraftItem.NameNumber, LabelColor, false, false);
            else
                AddLabel(330, 40, LabelHue, m_CraftItem.NameString);

            m_OtherCount = 0;

            if (m_CraftItem.UseAllRes)
                AddHtmlLocalized(
                    170,
                    302 + (m_OtherCount++ * 20),
                    310,
                    18,
                    1048176,
                    LabelColor,
                    false,
                    false
                ); // Makes as many as possible at once

            DrawItem();
            DrawSkill();

            retainedColorShown = DrawResource(
                pageIndex,
                resourcesPerPage,
                totalResources,
                res,
                resIndex,
                retainedColorShown
            );

            if (m_CraftItem.RequiredExpansion != Expansion.None)
            {
                bool supportsEx = (
                    m_From.NetState != null
                    && m_From.NetState.SupportsExpansion(m_CraftItem.RequiredExpansion)
                );
                TextDefinition.AddHtmlText(
                    this,
                    170,
                    302 + (m_OtherCount++ * 20),
                    310,
                    18,
                    RequiredExpansionMessage(m_CraftItem.RequiredExpansion),
                    false,
                    false,
                    supportsEx ? LabelColor : RedLabelColor,
                    supportsEx ? LabelHue : RedLabelHue
                );
            }

            if (needsRecipe)
                AddHtmlLocalized(
                    170,
                    302 + (m_OtherCount++ * 20),
                    310,
                    18,
                    1073620,
                    RedLabelColor,
                    false,
                    false
                ); // You have not learned this recipe.

            if (cropScroll)
                AddHtmlLocalized(
                    170,
                    302 + (m_OtherCount++ * 20),
                    360,
                    18,
                    1044379,
                    LabelColor,
                    false,
                    false
                ); // Inscribing scrolls also requires a blank scroll and mana.
        }

        private TextDefinition RequiredExpansionMessage(Expansion expansion)
        {
            switch (expansion)
            {
                case Expansion.SE:
                    return 1063363; // * Requires the "Samurai Empire" expansion
                case Expansion.ML:
                    return 1072651; // * Requires the "Mondain's Legacy" expansion
                default:
                    return String.Format(
                        "* Requires the \"{0}\" expansion",
                        ExpansionInfo.GetInfo(expansion).Name
                    );
            }
        }

        private bool m_ShowExceptionalChance;

        public void DrawItem()
        {
            Type type = m_CraftItem.ItemType;

            AddItem(20, 50, CraftItem.ItemIDOf(type));

            if (type != typeof(BaseMagicStaff))
            {
                m_ShowExceptionalChance = false;
            }
            else if (m_CraftItem.IsMarkable(type))
            {
                AddHtmlLocalized(
                    170,
                    302 + (m_OtherCount++ * 20),
                    310,
                    18,
                    1044059,
                    LabelColor,
                    false,
                    false
                ); // This item may hold its maker's mark
                m_ShowExceptionalChance = true;
            }
        }

        public void DrawSkill()
        {
            for (int i = 0; i < m_CraftItem.Skills.Count; i++)
            {
                CraftSkill skill = m_CraftItem.Skills.GetAt(i);
                double minSkill = skill.MinSkill,
                    maxSkill = skill.MaxSkill;

                if (minSkill < 0)
                    minSkill = 0;

                AddHtmlLocalized(
                    170,
                    132 + (i * 20),
                    200,
                    18,
                    1044060 + (int)skill.SkillToMake,
                    LabelColor,
                    false,
                    false
                );
                AddLabel(430, 132 + (i * 20), LabelHue, String.Format("{0:F1}", minSkill));
            }

            CraftSubResCol res = (
                m_CraftItem.UseSubRes2 ? m_CraftSystem.CraftSubRes2 : m_CraftSystem.CraftSubRes
            );
            int resIndex = -1;

            CraftContext context = m_CraftSystem.GetContext(m_From);

            if (context != null)
                resIndex = (
                    m_CraftItem.UseSubRes2 ? context.LastResourceIndex2 : context.LastResourceIndex
                );

            bool allRequiredSkills = true;
            double chance = m_CraftItem.GetSuccessChance(
                m_From,
                resIndex > -1 ? res.GetAt(resIndex).ItemType : null,
                m_CraftSystem,
                false,
                ref allRequiredSkills
            );
            double excepChance = m_CraftItem.GetExceptionalChance(m_CraftSystem, chance, m_From);

            if (chance < 0.0)
                chance = 0.0;
            else if (chance > 1.0)
                chance = 1.0;

            AddHtmlLocalized(170, 80, 250, 18, 1044057, LabelColor, false, false); // Success Chance:
            AddLabel(430, 80, LabelHue, String.Format("{0:F1}%", chance * 100));

            if (m_ShowExceptionalChance)
            {
                if (excepChance < 0.0)
                    excepChance = 0.0;
                else if (excepChance > 1.0)
                    excepChance = 1.0;

                AddHtmlLocalized(170, 100, 250, 18, 1044058, 32767, false, false); // Exceptional Chance:
                AddLabel(430, 100, LabelHue, String.Format("{0:F1}%", excepChance * 100));
            }
        }

        private static Type typeofBlankScroll = typeof(BlankScroll);
        private static Type typeofSpellScroll = typeof(SpellScroll);

        // Draws the resources that belong on the requested page. Returns whether the
        // color-retention note has been shown so subsequent pages avoid duplicating it.
        private bool DrawResource(
            int pageIndex,
            int resourcesPerPage,
            int totalResources,
            CraftSubResCol res,
            int resIndex,
            bool retainedColor
        )
        {
            int startIndex = pageIndex * resourcesPerPage;

            for (int i = 0; i < resourcesPerPage; i++)
            {
                int resourceIndex = startIndex + i;

                if (resourceIndex >= totalResources)
                    break;

                Type type;
                string nameString;
                int nameNumber;

                CraftRes craftResource = m_CraftItem.Resources.GetAt(resourceIndex);

                type = craftResource.ItemType;
                nameString = craftResource.NameString;
                nameNumber = craftResource.NameNumber;

                // Resource Mutation
                if (type == res.ResType && resIndex > -1)
                {
                    CraftSubRes subResource = res.GetAt(resIndex);

                    type = subResource.ItemType;

                    nameString = subResource.NameString;
                    nameNumber = subResource.GenericNameNumber;

                    if (nameNumber <= 0)
                        nameNumber = subResource.NameNumber;
                }
                // ******************

                if (!retainedColor && m_CraftItem.RetainsColorFrom(m_CraftSystem, type))
                {
                    retainedColor = true;
                    AddHtmlLocalized(
                        170,
                        302 + (m_OtherCount++ * 20),
                        310,
                        18,
                        1044152,
                        LabelColor,
                        false,
                        false
                    ); // * The item retains the color of this material
                    AddLabel(500, 219 + (i * 20), LabelHue, "*");
                }

                int rowY = 219 + (i * 20);

                if (nameNumber > 0)
                    AddHtmlLocalized(170, rowY, 310, 18, nameNumber, LabelColor, false, false);
                else
                    AddLabel(170, rowY, LabelHue, nameString);

                AddLabel(430, rowY, LabelHue, craftResource.Amount.ToString());
            }

            return retainedColor;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            // Back Button
            if (info.ButtonID == 0)
            {
                CraftGump craftGump = new CraftGump(m_From, m_CraftSystem, m_Tool, null);
                m_From.SendGump(craftGump);
            }
            else // Make Button
            {
                int num = m_CraftSystem.CanCraft(m_From, m_Tool, m_CraftItem.ItemType);

                if (num > 0)
                {
                    m_From.SendGump(new CraftGump(m_From, m_CraftSystem, m_Tool, num));
                }
                else
                {
                    Type type = null;

                    CraftContext context = m_CraftSystem.GetContext(m_From);

                    if (context != null)
                    {
                        CraftSubResCol res = (
                            m_CraftItem.UseSubRes2
                                ? m_CraftSystem.CraftSubRes2
                                : m_CraftSystem.CraftSubRes
                        );
                        int resIndex = (
                            m_CraftItem.UseSubRes2
                                ? context.LastResourceIndex2
                                : context.LastResourceIndex
                        );

                        if (resIndex > -1)
                            type = res.GetAt(resIndex).ItemType;
                    }

                    m_CraftSystem.CreateItem(
                        m_From,
                        m_CraftItem.ItemType,
                        type,
                        m_Tool,
                        m_CraftItem
                    );
                }
            }
        }
    }
}
