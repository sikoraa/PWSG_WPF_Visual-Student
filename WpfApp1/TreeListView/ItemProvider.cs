using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    static public class ItemProvider
    {
        static public List<Item> GetItems_(string path)
        {
            List<Item> ret = new List<Item>();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach(DirectoryInfo d in di.GetDirectories()) // dodaje folder i rekurencyjnie wywoluje na nim ta funkcje
            {
                ret.Add(new DirectoryItem(d.Name, d.FullName, GetItems_(d.FullName)));
            }
            foreach(FileInfo f in di.GetFiles()) // dodaje plik z rozszerzeniem .cs
            {
                if (f.Extension == ".cs")
                    ret.Add(new FileItem(f.Name, f.FullName));
            }
            return ret;
        }

        static public List<Item> GetItems(string path, out string csProj)
        {
            csProj = "";
            string n = "";
            bool foundCsProj = false;
            List<Item> ret = new List<Item>();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo f in di.GetFiles()) // dodaje plik z rozszerzeniem .cs
            {
                if (f.Extension == ".cs") // jesli plik .cs to dodaj go do listy
                    ret.Add(new FileItem(f.Name, f.FullName));
                else if (f.Extension == ".csproj") // jak .csproj to zaznacz ze poprawny projekt
                {
                    csProj = f.FullName;
                    n = f.Name;
                    foundCsProj = true;
                }
            }
            if (!foundCsProj) // jesli nie znaleziono pliku .csproj to wyswietl komunikat
            {
                MessageBox.Show("This is not proper folder with c# project!", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            int i = 0;
            foreach (DirectoryInfo d in di.GetDirectories()) // dodaje folder i rekurencyjnie wywoluje na nim ta funkcje, wywolane na koncu zeby nie wywolywac niepotrzebnie jak nie ma .csproj
            {
                ret.Insert(i, new DirectoryItem(d.Name, d.FullName, GetItems_(d.FullName)));
                ++i;
            }
            // tworzy liste o 1 elemencie projektu (jako folder) i przypisuje mu liste plikow wewnatrz folderu z .csproj
            List<Item> p = new List<Item>();
            p.Add(new DirectoryItem("Project: " + Path.GetFileNameWithoutExtension(n), csProj, ret));
            return p;
        }
    }
}
