using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class file
    {
        string name;
        string path;
        string content;
        bool changed;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public file(string p="New File", string n ="", string c="", bool ch=true)
        {
            Name = n; Path = p; Content = c; changed = ch;
        }

        public void LoadFile(string path)
        {
            StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open));
            File.WriteAllText(path, this.content);
            this.path = path;
            name = System.IO.Path.GetFileName(path);
            changed = false;
        }
        

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        public string Path
        {
            get { return path; }
            set { path = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }
        public bool Changed
        {
            get { return changed; }
            set { changed = value; OnPropertyChanged(); }
        }

        internal void Save()
        {
            throw new NotImplementedException();
        }
    }
}
