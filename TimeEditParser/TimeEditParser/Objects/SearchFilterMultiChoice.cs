using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Objects
{
    class SearchFilterMultiChoice : FilterCategory
    {
        // Key: data-name Value: List of possible filters
        public Dictionary<string, List<FilterCategory>> Filters { get; set; } = new Dictionary<string, List<FilterCategory>>();
    }
}
