using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using TimeEditParser.Objects;
using Xamarin.Forms;

namespace TimeEditParser.Utilities
{
    class ScheduleSearch
    {
        public static dynamic GetGroups(string link, bool useCache = true)
        {
            // Initiate variables
            dynamic doc;
            var web = new HtmlWeb();
            try
            {
                if(!useCache || !Application.Current.Properties.ContainsKey("groupsCache"))
                {
                    Dictionary<string, string> groups = new Dictionary<string, string>();
                    doc = web.Load(link + "objects.html?fr=t&partajax=t&im=f&sid=3&l=sv_SE&types=183");
                    doc = doc.DocumentNode.SelectNodes(".//div[contains(@class, 'clickable2 searchObject')]");
                    foreach (HtmlNode groupNode in doc)
                    {
                        groups[groupNode.Attributes["data-name"].Value.ToString()] = groupNode.Attributes["data-id"].Value.ToString();
                    }
                    return groups;

                } else
                {
                    return Application.Current.Properties["groupsCache"];
                }
            }
            catch // Return false if the setting does not exist, or if it fails to find the document nodes
            {
                return false;
            }
        }

        public static List<FilterCategory> GetFilters(string link)
        {
            HtmlDocument doc;
            var web = new HtmlWeb();
            doc = web.Load(link + "ri1Q7.html");
            List<FilterCategory> filters = new List<FilterCategory>();
            HtmlNode filtersList = doc.DocumentNode.SelectSingleNode(".//select[@id='fancytypeselector']");
            foreach(HtmlNode filterOption in filtersList.SelectNodes(".//option"))
            {
                FilterCategoryDropDown currentCategory = new FilterCategoryDropDown();
                // <option value="183" selected="">Klass</option>
                currentCategory.Name = filterOption.InnerText;
                currentCategory.Value = filterOption.Attributes["value"].Value;
                

                // <form id="fancyformfieldsearch" name="fancyformfieldsearch" data-loadselected="f">
                HtmlNode filtersLists = doc.DocumentNode.SelectSingleNode("//form[@id=\"fancyformfieldsearch\"]");
                // <fieldset id="ffset183" class="fancyfieldset  fancyNoBorder">
                HtmlNode filterCollectionNode = filtersLists.SelectSingleNode("//fieldset[@id='ffset" + currentCategory.Value + "']");
                // <select class="fancyformfieldsearchselect objectFieldsParam " multiple="" size="14" data-param="fe" data-tefieldkind="CATEGORY" data-name="Period" name="183_22" data-prefix="22" id="ff183_22">
                HtmlNodeCollection filterSelectionNodes = filterCollectionNode.SelectNodes(".//select");
                if (filterSelectionNodes == null) continue;
                foreach (HtmlNode filterSelectionNode in filterSelectionNodes)
                {
                    List<FilterCategory> subFilters = new List<FilterCategory>();
                    // <option value="10/11">10/11</option>
                    foreach (HtmlNode filterCheckNode in filterSelectionNode.SelectNodes(".//option"))
                    {
                        subFilters.Add(new FilterCategory { Name = filterCheckNode.InnerText, Value = filterCheckNode.Attributes["value"].Value });
                    }
                    currentCategory.Filters.Add(filterSelectionNode.Attributes["data-name"].Value, subFilters);
                    string dataParam = filterSelectionNode.Attributes["data-param"].Value;
                    string dataPrefix = filterSelectionNode.Attributes["data-prefix"].Value;
                }
                filters.Add(currentCategory);
            }
            return filters;
        }

        // Function for getting the link for schedule using the group id and the link base
        public static string ScheduleLink(string groupID, string linkbase)
        {
            if (!linkbase.EndsWith("/"))
            {
                linkbase += "/";
            }
            return linkbase + "ri.html?sid=3&bl=b&ds=f&objects=" + groupID;
        }
    }
}
