using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Objects
{
    class FilterCategoryDropDown : FilterCategory
    {
        public Dictionary<string, List<FilterCategory>> Filters = new Dictionary<string, List<FilterCategory>>();
    }
}
