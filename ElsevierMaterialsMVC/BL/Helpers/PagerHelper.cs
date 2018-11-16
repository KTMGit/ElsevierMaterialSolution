using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IniCore.Web.Mvc.Html;
using System.Text;
using ElsevierMaterials.Models.Domain.AdvancedSearch;


namespace ElsevierMaterialsMVC.BL.Helpers
{
    public static class PagerHelper
    {
        /// <summary>
        /// Generates html for given pager descriptor taking into account style rules used for SKF's sites.
        /// </summary>
        /// <param name="helper">The helper</param>
        /// <param name="descriptor">The descriptor for the pager</param>
        /// <returns>Generated IHtmlString for SKF pager</returns>
        public static IHtmlString KTMPager(this HtmlHelper helper, PagerDescriptor descriptor)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"pager\">");
            //sb.Append("<div class=\"page-size\">");
            //sb.Append("<span class=\"text\">").Append(descriptor.PageSizeDescription).Append("</span>");
            //for (int i = 0; i < descriptor.AllowedPageSizes.Count; i++) {
            //    if (descriptor.AllowedPageSizes[i] == descriptor.SelectedPageSize) {
            //        sb.Append("<span class=\"link-active\">").Append(descriptor.AllowedPageSizes[i]).Append("</span>");
            //    } else {
            //        sb.Append("<a class=\"link\">").Append(descriptor.AllowedPageSizes[i]).Append("</a>");
            //    }
            //}
            //sb.Append("</div>");
            sb.Append("<div class=\"pages\">");

            int startPage = 0;
            int endPage = 0;

            bool generatePreviousPageSet = true;
            bool generateNextPageSet = true;

            if (descriptor.TotalRecordCount > 0)
            {
                // generate 'Previous page' link
                // ...
                if (descriptor.SelectedPage > 1)
                {
                    sb.Append("<a class=\"previous\" page=\"" + (descriptor.SelectedPage - 1) + "\"></a>");
                }
                // calculate page links
                if ((int)Math.Ceiling((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) == 1)
                {
                    // the selected page is in the first set of pages
                    generatePreviousPageSet = false;
                    startPage = 1;
                    if (descriptor.TotalPageCount < descriptor.VisiblePageCount)
                    {
                        endPage = descriptor.TotalPageCount;
                    }
                    else
                    {
                        endPage = descriptor.VisiblePageCount;
                    }
                    if (descriptor.VisiblePageCount >= descriptor.TotalPageCount)
                    {
                        generateNextPageSet = false;
                    }
                }
                else if ((int)Math.Ceiling((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) == (int)(Math.Ceiling((descriptor.TotalPageCount + 0.0) / descriptor.VisiblePageCount)))
                {
                    // the selected page is in the last set of pages
                    startPage = (int)Math.Floor((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) * descriptor.VisiblePageCount + 1;
                    endPage = descriptor.TotalPageCount;
                    if (startPage > endPage)
                    {
                        startPage -= descriptor.VisiblePageCount;
                    }
                    generateNextPageSet = false;
                }
                else
                {
                    // the selected page is in middle set of pages
                    startPage = (int)Math.Floor((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) * descriptor.VisiblePageCount + 1;
                    endPage = (int)Math.Ceiling((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) * descriptor.VisiblePageCount;
                    if (startPage > endPage)
                    {
                        startPage -= descriptor.VisiblePageCount;
                    }
                }
                // generate first page link, generate previous page set link:  ( < | 1 | ... | 20 | 21 | 22)
                if (generatePreviousPageSet)
                {
                    if (descriptor.SelectedPage % descriptor.VisiblePageCount != 0)
                    {
                        sb.Append("<a class=\"link\">1</a><a class=\"link pageset\" page=\"" + (int)(Math.Floor((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) * descriptor.VisiblePageCount) + "\">...</a>");
                    }
                    else
                    {
                        sb.Append("<a class=\"link\">1</a><a class=\"link pageset\" page=\"" + (int)(Math.Floor((descriptor.SelectedPage - 1.0) / descriptor.VisiblePageCount) * descriptor.VisiblePageCount) + "\">...</a>");
                    }
                }

                // generate page links
                GeneratePageLinks(startPage, endPage, sb, descriptor);

                // generate next page set link, generate last page link: ( 27 | 28 | 29 | ... | 183 | > )
                if (generateNextPageSet)
                {
                    sb.Append("<a class=\"link pageset\" page=\"" + (int)(Math.Ceiling((descriptor.SelectedPage + 0.0) / descriptor.VisiblePageCount) * descriptor.VisiblePageCount + 1) + "\">...</a><a class=\"link-last\">" + descriptor.TotalPageCount + "</a>");
                }

                // generate 'Next page' link
                if (descriptor.SelectedPage < descriptor.TotalPageCount)
                {
                    sb.Append("<a class=\"next\" page=\"" + (descriptor.SelectedPage + 1) + "\"></a>");
                }
            }
            sb.Append("</div>");
            sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        private static void GeneratePageLinks(int startPage, int endPage, StringBuilder sb, PagerDescriptor descriptor)
        {
            if (startPage <= 0)
                return;
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == descriptor.SelectedPage)
                {
                    if (i == 1)
                    {
                        sb.Append("<span class=\"link-active-first\">").Append(i).Append("</span>");
                    }
                    else
                    {
                        sb.Append("<span class=\"link-active\">").Append(i).Append("</span>");
                    }
                }
                else
                {
                    sb.Append("<a class=\"link\">").Append(i).Append("</a>");
                }
            }
        }
    }

    public static class SortLinkHelper
    {
        public static IHtmlString SortLink(this HtmlHelper helper, string text, string property, SortOrder defaultSortOrder, SortDescriptor currentDescriptor)
        {
            string cssClass = "";
            SortOrder ord = defaultSortOrder;
            if (property.ToUpper() == currentDescriptor.PropertyName.ToUpper())
            {
                switch (currentDescriptor.Order)
                {
                    case SortOrder.Ascending:
                        cssClass = "sort-desc";
                        ord = SortOrder.Descending;
                        break;
                    case SortOrder.Descending:
                        cssClass = "sort-asc";
                        ord = SortOrder.Ascending;
                        break;
                    default:
                        break;
                }
            }
            return MvcHtmlString.Create("<div class=\"sort " + cssClass + "\"><a prop=\"" + property + "\" ord=\"" + ((int)ord).ToString() + "\" href=\"#\">" + text + "</a></div>");
        }
    }

    public static class GridExtensions
    {
        public static IHtmlString DescriptorToJson(this HtmlHelper helper, GridDescriptor descriptor)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(descriptor);
            return MvcHtmlString.Create(json);
        }
    }
    
}