using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class DirectoryItem : Item
    {
        public List<Item> Items { get; set; }
        public DirectoryItem()
        {
            Items = new List<Item>();
        }
        public DirectoryItem(string n, string p, List<Item> l)
        {
            Items = l;
            Name = n;
            Path = p;
        }
    }
}
