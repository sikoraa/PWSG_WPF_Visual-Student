using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class FileItem : Item
    {
        public FileItem(string name, string path)
        {
            Name = name; Path = path;
        }
    }
}
