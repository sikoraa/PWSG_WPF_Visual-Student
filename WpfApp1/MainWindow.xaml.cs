using Microsoft.Build.BuildEngine;
using Microsoft.Build.Execution;
using Microsoft.Build.Evaluation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using Microsoft.Build.Framework;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Windows.Markup;
using PluginContracts;
using System.Reflection;
using System.Windows.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int openedFiles = 0;
        string projectPath = "";
        private string consoleMsg;
        public string ConsoleMessages { get { return consoleMsg; } set { consoleMsg = value; OnPropertyChanged(); } }
        public List<ErrorMessage> ErrorMessages { get { return errorMsg; } set { errorMsg = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<file> tab;
        ObservableCollection<System.Windows.Controls.RichTextBox> textBoxes = new ObservableCollection<System.Windows.Controls.RichTextBox>();
        ObservableCollection<Item> l = new ObservableCollection < Item >();


        //List<Item> l = new List<Item>();
        file openedFile = null;
        string project = "";
        int openedTab = 0;
        private List<ErrorMessage> errorMsg = new List<ErrorMessage>();
       // public List<ErrorMessage> ErrorMessages { get { return errorMsg; } set { errorMsg = value; OnPropertyChanged(); } }
        public int OpenedTab { get => openedTab; set { openedTab = value; OnPropertyChanged(); } }
        public ObservableCollection<file> Tab { get => tab; set { tab = value; OnPropertyChanged(); } }
        public object ViewModel { get; private set; }
        public string BuildPath { get; private set; }
        public string ProjPath { get; private set; }
        //public object ErrorMessages { get; private set; }

        //public ObservableCollection<file> Tab1 { get => tab; set => tab = value; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Tab = new ObservableCollection<file>();          
            listbox1.ItemsSource = Tab;
            openedTab = 0;
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // About
        {
            System.Windows.MessageBox.Show("This is simple C# editor and compiler.", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) // New file
        {
            openedFiles++;
            file f = new file();
            //f.Changed = true;
            openedFile = f;
            Tab.Add(f);
            //OpenedTab = listbox1.SelectedIndex = openedFiles - 1;
            //listbox1.SelectedIndex = OpenedTab = Tab.Count - 1;
            OpenedTab = Tab.Count - 1;
            listbox1.Items.Refresh();
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
                Tab.Add(tmp);
                tmp.LoadFile(openFileDialog.FileName);
                openedFile = tmp;
                
                ++openedFiles;
                //OpenedTab = listbox1.SelectedIndex = openedFiles - 1;
                //listbox1.SelectedIndex = OpenedTab = Tab.Count - 1;
                OpenedTab = Tab.Count - 1;
            }
            //listbox1.Items.Refresh();






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
            //listbox1.Items.Refresh();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e) // Exit button
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e) // save as
        {
            if (Tab.Count > 0)
                Tab[OpenedTab].SaveAs();
            //listbox1.Items.Refresh();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e) // save
        {
            if (Tab.Count > 0)
                Tab[OpenedTab].Save();
                
                    //openedFile.Save();
            //listbox1.Items.Refresh();
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
            //listbox1.Items.Refresh();
        }

        private void CloseFile(object sender, RoutedEventArgs e) // X button, close file
        { 
            file f = ((sender as System.Windows.Controls.Button).DataContext) as file;
            if (f.Changed)
            {
                if (System.Windows.MessageBox.Show("Do you want to close unsaved document?", "Close Document", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                else
                {
                    Tab.Remove(f);
                    --openedFiles;
                    OpenedTab = tab.Count - 1;
                }
            }
            else
            {
                Tab.Remove(f);
                --openedFiles;
                OpenedTab = tab.Count - 1;
            }
           //listbox1.Items.Refresh();

        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e) // highlight plugin1
        {
            //listbox1.Items.Refresh();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e) // highlight plugin2
        {
            //listbox1.Items.Refresh();
        }

        private bool buildProj(int run)
        {
            if (l.Count <= 0) return false;
            if (ProjPath == null || ProjPath == "") // ((DirectoryItem)projectTree.SelectedItem).Path;
                ProjPath = l[0].Path;
            ConsoleMessages = "";
            BuildPath = System.IO.Path.GetDirectoryName(ProjPath) + "\\build";

            if (Directory.Exists(BuildPath))
                Directory.Delete(BuildPath, true);

            ErrorMessages.Clear();
            var props = new Dictionary<string, string>
            {
                {"OutputPath", BuildPath}
            };
            ProjectInstance pc = new ProjectInstance(ProjPath, props, "14.0");

            StringBuilder sb = new StringBuilder();
            WriteHandler handler = (x) =>
            {
                sb.AppendLine(x);

                var divided = x.Split(new char[] { ' ', ':' }, 4, StringSplitOptions.RemoveEmptyEntries);
                var couldBeError = divided.Length > 1 ? true : false;
                if (couldBeError && divided[1] == "error")
                    ErrorMessages.Add(new ErrorMessage(divided[0], "error " + divided[2], divided[3]));

            };
            var logger = new ConsoleLogger(LoggerVerbosity.Normal, handler, null, null);

            var buildParams = new BuildParameters()
            {
                DetailedSummary = false,
                Loggers = new List<ILogger> { logger },
                DefaultToolsVersion = "14.0"
            };
            var targets = new List<string> { "Build" };
            var reqData = new BuildRequestData(pc, targets.ToArray());

            var res = BuildManager.DefaultBuildManager.Build(buildParams, reqData);
            ConsoleMessages = sb.ToString();
            errorListBox.Items.Refresh();
            if (res.OverallResult == BuildResultCode.Failure)
                return false;          
            if (run == 1)
                runProj(BuildPath);
            return true;
        }

        private void runProj(string buildPath)
        {
            Process p = new Process();
            string fName = System.IO.Path.GetFileNameWithoutExtension(ProjPath) + ".exe";
            p.StartInfo.FileName = buildPath + "\\" + fName;
            //p.StartInfo.FileName = Path;
            p.Start();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e) // execute
        {
            if (l.Count <= 0)         
                System.Windows.Forms.MessageBox.Show("You can not build project if it is not loaded!", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                buildProj(chooseComboBox.SelectedIndex);
            }
            //listbox1.Items.Refresh();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Tab[OpenedTab].Changed = true;

            System.Windows.Controls.RichTextBox tmp = (System.Windows.Controls.RichTextBox)sender;
            TextRange range = new TextRange(tmp.Document.ContentStart, tmp.Document.ContentEnd);
            string prev = range.Text;
            string c = Tab[OpenedTab].Content;
            Tab[OpenedTab].Content = range.Text;

            if (c.Length > prev.Length)
            {
                Tab[OpenedTab].Changed = true;
                return;
            }

            if (prev.Substring(0, c.Length) != c)
                Tab[OpenedTab].Changed = true;

            //listbox1.Items.Refresh();
        }

        private void treeview_MouseDoubleClick(object sender, MouseButtonEventArgs e) // double click na plik w treeview
        {
            object tmp = projectTree.SelectedItem;
            if (tmp is FileItem)
            {
                FileItem f = tmp as FileItem;
                for (int i = 0; i < Tab.Count; ++i) // jesli taki plik jest juz otwarty, to damy mu focus
                    if (f.Name == Tab[i].Name && f.Path == Tab[i].Path)
                        { OpenedTab = i; openedFile = Tab[i]; return; }
                // otwieramy plik jesli nie byl otwarty
                Tab.Add(new file(f.Name, f.Path, File.ReadAllText(f.Path), false));
                ++openedFiles;
                OpenedTab = openedFiles - 1;
                //listbox1.Items.Refresh();
            }
        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RichTextBox tmp = (System.Windows.Controls.RichTextBox)sender;
            Paragraph paragraph = new Paragraph();
            Run run = new Run();
            run.Text = Tab[OpenedTab].Content;

            paragraph.Margin = new Thickness(0);
            paragraph.FontFamily = new FontFamily("Monaco");
            paragraph.FontSize = 12;
            paragraph.Inlines.Add(run);
            FlowDocument flowDocument = new FlowDocument(paragraph);
            tmp.Document = flowDocument;

            if (!textBoxes.Contains(tmp))
                textBoxes.Add(tmp);
            //listbox1.Items.Refresh();
        }

        private void projectTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(projectTree.SelectedItem is DirectoryItem)
            {
                string[] tmp = ((DirectoryItem)projectTree.SelectedItem).Name.Split(':'); // Project: *.csproj
                if (tmp.Length > 1 && tmp[0] == "Project") // znaleziono Project: ...
                {
                    ProjPath = ((DirectoryItem)projectTree.SelectedItem).Path;
                }
            }
        }
    }
}
