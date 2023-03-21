using Project2.classes;
using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;

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
using static Project2.majorTrait;

namespace Project2
{
    /// <summary>
    /// Interaction logic for Religion.xaml
    /// </summary>
    public partial class ReligionPage : Page
    {

        public ReligionPage(config currentConfig)
        {
            CurrentConfig = currentConfig;
            InitializeComponent();
            ReligionCollection = new ObservableCollection<majorTrait>();
            foreach (majorTrait religion in CurrentConfig.RelList) //adds all religion to ObservableCollection ReligionCollection
            {
                ReligionCollection.Add(religion);
            }
            lstReligion.ItemsSource = ReligionCollection;
            CurrentIndex = -1;  //skip the next use of CurrentIndex
            lstReligion.SelectedIndex = 0;
        }
        public config CurrentConfig { get; set; }
        int CurrentIndex { get; set; }
        private ObservableCollection<majorTrait> ReligionCollection;

        private void ReligionMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            MainWindow mainWindow = new MainWindow(CurrentConfig);
            Application.Current.MainWindow.Content = mainWindow;
        }

        private void btnReligion_ClickAdd(object sender, RoutedEventArgs e)
        {
            majorTrait tempReligion = new majorTrait(CurrentConfig.newUID("RelList")) { Name = "new religion" };   //makes the new religion object
            CurrentConfig.saveToList(tempReligion);
            ReligionCollection.Add(tempReligion);
            lstReligion.SelectedIndex = ReligionCollection.Count - 1;
        }

        private void btnReligion_ClickDelete(object sender, RoutedEventArgs e)
        {
            Functionality.DeleteRes(CurrentConfig, lstReligion, ReligionCollection);
        }
        private void btnReligion_ClickCopy(object sender, RoutedEventArgs e)
        {
            var index = lstReligion.SelectedIndex;
            if (index >= 0)
            {
                majorTrait tempReligion = new majorTrait(CurrentConfig.newUID("RelList"))
                {
                    AffectedResources = CurrentConfig.RelList[index].AffectedResources,
                    Name = CurrentConfig.RelList[index].Name,
                    Description = CurrentConfig.RelList[index].Description
                };   //makes the new religion object
                CurrentConfig.saveToList(tempReligion);
                ReligionCollection.Add(tempReligion);
                lstReligion.SelectedIndex = ReligionCollection.Count - 1;
            }
        }

        private void OnClickAddAffectedResources(object sender, RoutedEventArgs e)
        {
            this.InitializeComponent();
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            ComboBox comboBoxOne = new ComboBox();  //starts on the recouse combobox
            comboBoxOne.Text = "Select Stat";
            comboBoxOne.IsReadOnly = true;
            comboBoxOne.IsDropDownOpen = false;
            comboBoxOne.Margin = new Thickness(5, 5, 0, 0);
            comboBoxOne.Height = 24;
            comboBoxOne.Width = 185;

            comboBoxOne.DisplayMemberPath = "Name";
            foreach (resourceTrait res in CurrentConfig.ResList)
            {
                comboBoxOne.Items.Add(res);
            }

            stackPanel.Children.Add(comboBoxOne);

            TextBox textBox = new TextBox();
            textBox.Margin = new Thickness(15, 5, 0, 0);
            textBox.Width = 40;
            textBox.Height = 24;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.TextChanged += NumberValidationTextBox;

            stackPanel.Children.Add(textBox);

            StackPanel childStackPanel = new StackPanel();
            childStackPanel.Orientation = Orientation.Vertical;

            stackPanel.Children.Add(childStackPanel);

            this.ListAffectedResources.Items.Add(stackPanel);

        }

        private void OnClickDeleteAffectedResources(object sender, RoutedEventArgs e)
        {
            var index = ListAffectedResources.SelectedIndex;

            if (index >= 0)
            {
                ListAffectedResources.Items.RemoveAt(index);
            }

        }

        private void OnReligionChanged(object sender, RoutedEventArgs e)
        {
            int SelIndex = lstReligion.SelectedIndex;  //saves selected index so it is not lost
            if (lstReligion.SelectedIndex >= 0)    //lstReligion.SelectedIndex returns -1 if nothing is selected
            {
                if (CurrentIndex >= 0)  //skips saving the previus selected Religion if -1
                {
                    SaveReligion(CurrentIndex);
                    ListAffectedResources.Items.Clear();
                }
                CurrentIndex = lstReligion.SelectedIndex;
                majorTrait currentMT = CurrentConfig.RelList[CurrentIndex]; //gets the trait to be loaded

                (this.FindName("nameBox") as TextBox).Text = currentMT.Name; //sets text to the name from the current MajorTrait object
                (this.FindName("descBox") as TextBox).Text = currentMT.Description;  //sets text to the description from the current MajorTrait object

                foreach (AmountUID affRes in currentMT.AffectedResources)	//makes the needed comboboxes to hold the starter resources
                {
                    OnClickAddAffectedResources(sender, e);
                }
                int ind = 0;
                foreach (StackPanel PANEL in (this.FindName("ListAffectedResources") as ListView).Items)
                {
                    foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
                    {
                        foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
                        {
                            AmountUID tempAffRes = currentMT.AffectedResources[ind];
                            box.SelectedIndex = CurrentConfig.ResList.FindIndex(i => string.Equals(i.UID, tempAffRes.UID)); //selects the starter resources in the comboboxes
                            textBox.Text = tempAffRes.Amount.ToString(); //sets the right amounts in the textboxes
                        }
                    }
                    ind++;
                }
            }
            else
            {
                CurrentIndex = -1;
                ListAffectedResources.Items.Clear();
            }
            lstReligion.SelectedIndex = SelIndex;  //applies saved index selection
        }

        private void OnClickSaveReligion(object sender, RoutedEventArgs e)
        {
            SaveReligion();
        }

        private void SaveReligion(int index = -1)
        {
            int SelIndex = lstReligion.SelectedIndex;  //saves selected index so it is not lost

            string UID = "";
            if (index == -1)    //is true when funtion is called via a button
            {
                if (lstReligion.SelectedIndex >= 0)
                {
                    UID = CurrentConfig.RelList[lstReligion.SelectedIndex].UID;    //uses the selected index to find the wanted UID
                    index = CurrentConfig.RelList.FindIndex(i => string.Equals(i.UID, UID));
                }
            }
            else
            {
                UID = CurrentConfig.RelList[index].UID; //uses the given index to find the wanted UID
            }
            if (UID != "")
            {
                majorTrait currentMT = CurrentConfig.GetTrait(UID);
                currentMT.deleteContent();
                currentMT.Name = (this.FindName("nameBox") as TextBox).Text;
                currentMT.Description = (this.FindName("descBox") as TextBox).Text;


                foreach (StackPanel PANEL in (this.FindName("ListAffectedResources") as ListView).Items)
                {
                    foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
                    {
                        foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
                        {
                            if (box.SelectedIndex >= 0 & textBox.Text != "")
                            {
                                string TempUID = CurrentConfig.ResList[box.SelectedIndex].UID;  //gets the affected rescource
                                int TempVal = int.Parse(textBox.Text);  //gets the value
                                currentMT.AffectedResources.Add(new AmountUID(TempUID, TempVal));   //saves the affected rescources and their values
                            }
                        }
                    }
                }

                CurrentConfig.RelList[index] = currentMT;
                ReligionCollection.Clear(); // clears the list
                foreach (majorTrait religion in CurrentConfig.RelList)  //rewrites the list.
                {
                    ReligionCollection.Add(religion);
                }

                lstReligion.SelectedIndex = SelIndex;  //applies saved index selection
            }
        }
        /// <summary>
		/// Validates input in "amount" textbox to only allow integers.
		/// </summary>
		private void NumberValidationTextBox(object sender, EventArgs e)
        {
            try
            {
                int.Parse((sender as TextBox).Text); //if Parse is unsuccessful, text is something other than integer
            }
            catch
            {
                (sender as TextBox).Text = "";
            }
        }

        private void searchbar_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = (this.FindName("searchbar") as TextBox).Text;
            if (searchText != "")
            {
                ReligionCollection.Clear();
                foreach (majorTrait religion in CurrentConfig.RelList) //adds all races to ObservableCollection RaceCollection
                {
                    if (religion.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        ReligionCollection.Add(religion);
                    }
                }
                lstReligion.SelectedIndex = 0;
            }
            else
            {
                ReligionCollection.Clear();
                foreach (majorTrait race in CurrentConfig.RacList) //adds all races to ObservableCollection RaceCollection
                {
                    ReligionCollection.Add(race);
                }
                lstReligion.SelectedIndex = 0;
            }
        }

        private void ChangeIcon_click(object sender, RoutedEventArgs e)
        {

        }
    }
}


