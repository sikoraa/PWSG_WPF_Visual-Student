using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class ProjItem : Item
    {
        //private string fullName;
        public List<Item> l { get; set; }


        public ProjItem(string name, string fullName)
        {
            Name = name;
            Path = fullName;
        }

        public ProjItem(string name, string fullName, List<Item> ll)
        {
            Name = name;
            Path = fullName;
            l = ll;
        }
    }
}
