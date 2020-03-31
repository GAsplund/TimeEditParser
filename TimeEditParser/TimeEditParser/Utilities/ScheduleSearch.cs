using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
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
