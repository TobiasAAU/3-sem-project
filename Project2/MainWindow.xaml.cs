using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow(config currentConfig)
        {

            CurrentConfig = currentConfig;
            InitializeComponent();
        }
        public config CurrentConfig { get; set; }

        private void RaceMainMenu_MouseClick(object sender, MouseButtonEventArgs e)
        {
            RacePage race = new RacePage(CurrentConfig); //we need to talk about naming stuff!!
            Application.Current.MainWindow.Content = race;
           
        }

        private void GalleryMainMenu_MouseClick(object sender, MouseButtonEventArgs e)
        {
            GalleryWindow gallery = new GalleryWindow(CurrentConfig);
            Application.Current.MainWindow.Content = gallery;

        }

        private void ItemMainMenu_MouseClick(object sender, MouseButtonEventArgs e)
        {
            ItemPage item = new ItemPage(CurrentConfig);
            Application.Current.MainWindow.Content = item;

        }

        private void CareerMainMenu_MouseClick(object sender, MouseButtonEventArgs e)
        {
            CareerPage item = new CareerPage(CurrentConfig);
            Application.Current.MainWindow.Content = item;

        }
        private void selectFolder_Click(object sender, System.EventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = CurrentConfig.SaveDestination; // Use current value for initial dir
            dialog.Title = "Select a Directory"; // instead of default "Save As"
            dialog.Filter = "Directory|*.this.directory"; // Prevents displaying files
            dialog.FileName = "select"; // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");
                path = path + "\\";
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                // Our final value is in path
                MessageBox.Show("the path is " + path);

                CurrentConfig.SaveDestination = path;
            }

        }
        private void ReligionMainMenu_MouseClick(object sender, MouseButtonEventArgs e)
        {

            ReligionPage religion = new ReligionPage(CurrentConfig);
            Application.Current.MainWindow.Content = religion;
        }

        private void AbilityMainMenu_MouseClick(object sender, MouseButtonEventArgs e)
        {
            AbilitiesPage ability = new AbilitiesPage(CurrentConfig);
            Application.Current.MainWindow.Content = ability;
        }

        private void ResourcesMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ResourcePage resources = new ResourcePage(CurrentConfig);
            Application.Current.MainWindow.Content = resources;
            string input = "Empty";
            //input = Interaction.InputBox("Name:", "Name: (REQUIRED?)", "Default", x_coordinate, y_coordinate);
            //TextBlock myTextBlock = "Empty";
        }

        private void onclickSave(object sender, MouseButtonEventArgs e)
        {
            CurrentConfig.WriteToJson();
            MessageBox.Show("Config JSON file saved to destination");
        }

        private void onclickLoad(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog theFileDialog = new OpenFileDialog();
            theFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            theFileDialog.InitialDirectory = CurrentConfig.SaveDestination;

            if (theFileDialog.ShowDialog() == true)
            {
                string fullFileName = theFileDialog.FileName;
                string jsonString = File.ReadAllText(fullFileName);
                CurrentConfig = JsonSerializer.Deserialize<config>(jsonString);
                if (File.Exists(CurrentConfig.SaveDestination) == false)
                {
                    CurrentConfig.SaveDestination = theFileDialog.InitialDirectory;
                }
                MessageBox.Show("Config JSON file read and loaded");
            }
        }

        private void onclickZip(object sender, MouseButtonEventArgs e)
        {
            if (File.Exists(CurrentConfig.SaveDestination + "Configuration.zip"))
            {
                File.Delete(CurrentConfig.SaveDestination + "Configuration.zip");
            }
            using (ZipArchive zip = ZipFile.Open(CurrentConfig.SaveDestination + "Configuration.zip", ZipArchiveMode.Create))
            {
                CurrentConfig.WriteToJson("Rules.json");
                zip.CreateEntryFromFile((CurrentConfig.SaveDestination + "Rules.json"), "Configuration.json");
                foreach (galleryIcon icon in CurrentConfig.IcoList)
                {
                    zip.CreateEntryFromFile(icon.imgPath, icon.imgName);
                }
            }
            MessageBox.Show("current loaded configuration and asotiated icons have been compressed to zip");

        }

        /* private void AddMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
         {
             string input = "Empty";
             input = Interaction.InputBox("Name:", "Name: (REQUIRED?)", "Default", x_coordinate, y_coordinate);

             string NewLabel = LabelGenerator(input);
             GridTextBlock.Children.Add(NewLabel);

             TextBlock myTextBlock = new TextBlock();
             myTextBlock.FontSize = "14";
             myTextBlock.FontWeight = Fontweights.Bold;
             myTextBlock.Text = input;

             input.Children.Add(myTextBlock);
         }

         private void LabelGenerator(string input)
         {
             Label newlabel = new Label();
             newlabel.HorizontalAlignment = "Left";
             newlabel.VerticalAlignment = "Top";
             newlabel.FontSize = "14";
             newlabel.FontWeight = Fontweights.Bold;
             int i = 0;
             int f = 0 % 2;
             while(true);
                 if(!string.IsNullOrEmpty(GridTextBlock.Grid.Column.i))
                 {
                     i++;
                 }
                 if(!string.IsNullOrEmpty(GridTextBlock.Grid.Column.f){
                     f++;
                 }
             if (string.IsNullOrEmpty(GridTextBlock.Grid.Column.f && string.IsNullOrEmpty(GridTextBlock.Grid.Column.i)))
                 {
                     break;
                 }
             newlabel.Column = i;
             newlabel.Row = f;
             newlabel.x:name = input;
             return newlabel;

         }*/

    }

}
