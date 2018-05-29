using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int openedFiles = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<file> tab;
        List<Item> l = new List<Item>();
        file openedFile = null;
        string project = "";
        int openedTab = 0;

        public int OpenedTab { get => openedTab; set { openedTab = value; OnPropertyChanged(); } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tab = new ObservableCollection<file>();          
            listbox1.ItemsSource = tab;
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // About
        {
            System.Windows.MessageBox.Show("This is simple C# editor and compiler.", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) // New file
        {
            openedFiles++;
            file f = new file();
            f.Changed = true;
            openedFile = f;
            tab.Add(f);          
            OpenedTab = listbox1.SelectedIndex = openedFiles - 1;
        }

        //private void RichTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    if (openedFile != null)
        //        openedFile.Changed = true;
        //}

        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.Source is System.Windows.Controls.TabControl)
            {
                OpenedTab = listbox1.SelectedIndex;
                if (OpenedTab >= 0)
                    openedFile = tab[OpenedTab];
                else
                    openedFile = null;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e) // Open file
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "C# files (*.cs)|*.cs"
            };        
            openFileDialog.ValidateNames = false;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file tmp = new file();
                tab.Add(tmp);
                tmp.LoadFile(openFileDialog.FileName);
                openedFile = tmp;
                
                ++openedFiles;
                OpenedTab = listbox1.SelectedIndex = openedFiles - 1;

                

            }






        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e) // Open project
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.SelectedPath = "C:\\Users\\admin\\Desktop\\vs\\WindowsFormsApp1\\WindowsFormsApp1";
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<Item> tmp = ItemProvider.GetItems(f.SelectedPath, out project);
                if (tmp == null) return;
                l.Add(tmp[0]);
                projectTree.DataContext = l;
                projectTree.Items.Refresh();
            }

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e) // Exit button
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e) // save as
        {
            if (openedFiles > 0 && openedFile != null)
                openedFile.SaveAs();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e) // save
        {
            if (openedFiles > 0 && openedFile != null)
                openedFile.Save();
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            var tabItem = (sender as TabItem);

            if (null != tabItem.DataContext)
            {
                //  Hey, what about TabControl.ItemTemplate, eh? 
                var dataTemplate = (DataTemplate)tabItem.FindResource("RichTextBoxTemplate");

                tabItem.Content = dataTemplate.LoadContent();
                (tabItem.Content as FrameworkElement).DataContext = tabItem.DataContext;
            }
        }

        private void CloseFile(object sender, RoutedEventArgs e) // X button, close file
        { 
            file f = ((sender as System.Windows.Controls.Button).DataContext) as file;
            if (f.Changed)
            {
                if (System.Windows.MessageBox.Show("This file has been modified. Do you want to save before closing?", "Save file?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    f.Save();
            }
            tab.Remove(f);
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e) // highlight plugin1
        {

        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e) // highlight plugin2
        {

        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e) // highlight plugin2
        {

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void treeview_MouseDoubleClick(object sender, MouseButtonEventArgs e) // double click na plik w treeview
        {
            object tmp = projectTree.SelectedItem;
            if (tmp is FileItem)
            {
                FileItem f = tmp as FileItem;
                for (int i = 0; i < tab.Count; ++i) // jesli taki plik jest juz otwarty, to damy mu focus
                    if (f.Name == tab[i].Name && f.Path == tab[i].Path)
                        { OpenedTab = i; openedFile = tab[i]; return; }
                // otwieramy plik jesli nie byl otwarty
                tab.Add(new file(f.Name, f.Path, File.ReadAllText(f.Path), false));
                ++openedFiles;
                OpenedTab = openedFiles - 1;
            }
        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
