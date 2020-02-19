using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Models
{
    class ItemList : List<Item>
    {
        public string Heading { get; set; }
        public List<Item> Items => this;
    }
}
