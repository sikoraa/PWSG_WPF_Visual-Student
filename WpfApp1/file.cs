using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class file
    {
        string name;
        string path;
        string content;
        bool changed;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged!= null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public file(string n="New File", string p="", string c="", bool ch=true)
        {
            Name = n; Path = p; Content = c; changed = ch;
        }

        public void LoadFile(string path)
        {
            content = "";
            Content = File.ReadAllText(path);
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            Changed = false;
        }
        

        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged(); }
        }

        public string Path
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged(); }
        }

        public string Content
        {
            get { return content; }
            set { content = value; NotifyPropertyChanged(); }
        }
        public bool Changed
        {
            get { return changed; }
            set { changed = value; NotifyPropertyChanged(); }
        }

        internal void Save()
        {
            if (Path =="") // to jest nowy plik i trzeba mu nadac sciezke/nazwe
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "C# Files (*cs)|*.cs";
                s.AddExtension = true;
                s.OverwritePrompt = true;
                if (s.ShowDialog() == true)
                {
                    Path = System.IO.Path.GetFullPath(s.FileName);
                    Name = System.IO.Path.GetFileName(s.FileName);
                }
                else return; // nie wybrano nazwy/sciezki
            }
            try
            {
                File.WriteAllText(path, content);
                Changed = false;
            }
            catch (UnauthorizedAccessException ex)
            {

            }
        }

        internal void SaveAs()
        {
            SaveFileDialog s = new SaveFileDialog();
            if (path != "")
                s.InitialDirectory = System.IO.Path.GetDirectoryName(path);
            s.Filter = "C# Files (*cs)|*.cs";
            s.AddExtension = true;
            s.OverwritePrompt = true;
            if (s.ShowDialog() == true)
            {
                this.path = System.IO.Path.GetFullPath(s.FileName);
                this.name = System.IO.Path.GetFileName(s.FileName);
            }
            else return; // nie wybrano nazwy/sciezki
            try
            {
                File.WriteAllText(path, content);
                Changed = false;
            }
            catch (UnauthorizedAccessException ex)
            {

            }
        }
    }
}
