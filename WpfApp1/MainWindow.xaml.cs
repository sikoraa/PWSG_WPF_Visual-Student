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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //bool oneFile = true;
        int openedFiles = 0;
        //List<(string,int)> tab = new List<(string,int)>();
        //List<string> tab = new List<string>();
        ObservableCollection<file> tab = new ObservableCollection<file>();
        file openedFile = null;
        int openedTab = 0;
        public MainWindow()
        {
            InitializeComponent();
            //txtbox.IsEnabled = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // About
        {
            System.Windows.MessageBox.Show("This is simple C# editor and compiler.", "About");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) // New file
        {
            openedFiles++;
            tab.Add(new file());          
            openedTab = listbox1.SelectedIndex = openedFiles - 1;

        }

        private void RichTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (openedFile != null)
                openedFile.Changed = true;
        }

        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.Source is System.Windows.Controls.TabControl)
            {
                //tab[openedTab] = new TextRange(txtbox.Document.ContentStart, txtbox.Document.ContentEnd).Text;
                openedTab = listbox1.SelectedIndex;
                //txtbox.Document.Blocks.Clear();
                //txtbox.Document.Blocks.Add(new Paragraph(new Run(tab[openedTab])));
                //txtbox.Focus();
                //do work when tab is changed
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e) // Open file
        {
            string file;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "C# files (*.cs)|*.cs";
            openFileDialog.ShowDialog();

            if (openFileDialog == null || openFileDialog.FileName == "") return;
            file = File.ReadAllText(openFileDialog.FileName);
            if (openedFiles == 0)
            {
                //listrow.Height = new GridLength(20, GridUnitType.Pixel);
                //txtbox.IsEnabled = true;
            }
            //tab.Add("");
            listbox1.Items.Add(openFileDialog.FileName);
            file c = new file();
            c.LoadFile(openFileDialog.FileName);
            tab.Add(c);
            //tab.Add(file);// = file;
            ++openedFiles;
            openedTab = openedFiles - 1;
            listbox1.SelectedIndex = openedTab;
            //txtbox.Document.Blocks.Clear();
            //txtbox.Document.Blocks.Add(new Paragraph(new Run(file)));
            //listbox1.Items.Add(System.IO.Path(openFileDialog.FileName));
            //tab[openedTab] = new TextRange(txtbox.Document.ContentStart, txtbox.Document.ContentEnd).Text;

            //var result = openFileDialog.ShowDialog();
            //if (result == false)
            //{
            //    try
            //    {
            //        if ((myStream = openFileDialog1.OpenFile()) != null)
            //        {
            //            using (myStream)
            //            {
            //                // Insert code to read the stream here.
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            //    }
            //}
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e) // Open project
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e) // Exit button
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e) // save as
        {

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e) // save
        {

        }

        private void CloseFile(object sender, RoutedEventArgs e)
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
    }
}
