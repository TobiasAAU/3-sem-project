using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;
using static Project2.majorTrait;
using static Project2.ItemPage;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using Image = System.Windows.Controls.Image;
using Project2.classes;

namespace Project2
{
    /// <summary>
    /// Interaction logic for Item.xaml
    /// </summary>
    public partial class ItemPage : Page
    {

        public ItemPage(config currentConfig) //Item window constructor
        {
            CurrentConfig = currentConfig;
            InitializeComponent();
            ItemCollection = new ObservableCollection<majorTrait>();
            foreach (majorTrait Item in CurrentConfig.IteList) //adds all Items to ObservableCollection ItemCollection
            {
                ItemCollection.Add(Item);
            }
            lstItems.ItemsSource = ItemCollection;
            CurrentIndex = -1;  //skip the next use of CurrentIndex
            lstItems.SelectedIndex = 0;
        }
        public config CurrentConfig { get; set; }
        int CurrentIndex { get; set; }  //keeps track of what index to use
        private ObservableCollection<majorTrait> ItemCollection;   //itemSource for lstItems ListVeiw

        /// <summary>
        /// sets page to MainWindow
        /// </summary>
        private void ItemMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(CurrentConfig);
            Application.Current.MainWindow.Content = mainWindow;
        }


        /// <summary>
        /// adds a new empty Item to Current config and newItem
        /// </summary>
        private void btnItems_ClickAdd(object sender, RoutedEventArgs e)
        {
            majorTrait tempItem = new majorTrait(CurrentConfig.newUID("IteList")) { Name = "new Item" };   //makes the new Item object
            CurrentConfig.saveToList(tempItem);
            ItemCollection.Add(tempItem);
            lstItems.SelectedIndex = ItemCollection.Count - 1;

        }
        /// <summary>
		/// deletes Item from both CurrentConfig and newItem
		/// </summary>
        private void btnItems_ClickDelete(object sender, RoutedEventArgs e)
        {
            Functionality.DeleteRes(CurrentConfig, lstItems, ItemCollection);
        }
        private void btnItems_ClickCopy(object sender, RoutedEventArgs e)
        {
            var index = lstItems.SelectedIndex;
            if (index >= 0)
            {
                majorTrait tempItem = new majorTrait(CurrentConfig.newUID("IteList"))
                {
                    Name = CurrentConfig.IteList[index].Name,
                    Description = CurrentConfig.IteList[index].Description,
                    AffectedResources = CurrentConfig.IteList[index].AffectedResources,
                    FreeAbilities = CurrentConfig.IteList[index].FreeAbilities
                };   //makes the new Item object
                CurrentConfig.saveToList(tempItem);
                ItemCollection.Add(tempItem);
                lstItems.SelectedIndex = ItemCollection.Count - 1;
            }

        }

        /// <summary>
        /// adds a combobox with all Abilities fron CurrentConfig as selectables
        /// </summary>
        private void OnClickAddStarterAbilities(object sender, RoutedEventArgs e)
        {
            int SelIndex = lstItems.SelectedIndex;  //saves selected index so it is not lost
            this.InitializeComponent();
            ComboBox comboBox = new ComboBox();
            comboBox.IsReadOnly = true;
            comboBox.IsDropDownOpen = false;
            comboBox.Margin = new Thickness(5, 5, 0, 0);
            comboBox.Height = 24;
            comboBox.Width = 185;
            comboBox.DisplayMemberPath = "Name";
            foreach (majorTrait abi in CurrentConfig.AbiList)  //adds all abilities from CurrentConfig to the combobox
            {
                comboBox.Items.Add(abi);
            }

            this.ListStarterAbilities.Items.Add(comboBox);
            lstItems.SelectedIndex = SelIndex;  //applies saved index selection
        }
        /// <summary>
        /// Deletes selected starterAbility
        /// </summary>
        private void OnClickDeleteStarterAbilities(object sender, RoutedEventArgs e)
        {
            var index = ListStarterAbilities.SelectedIndex;
            if (index >= 0)
            {
                ListStarterAbilities.Items.RemoveAt(index);
            }
        }

        /// <summary>
        /// add a combobox with all Resources fron CurrentConfig as selectables, together with a numer only textbox
        /// </summary>
        private void OnClickAddStarterResources(object sender, RoutedEventArgs e)
        {
            int SelIndex = lstItems.SelectedIndex;  //saves selected index so it is not lost
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


            stackPanel.Children.Add(comboBoxOne);   //makes the combobox a child of the stackpanel

            TextBox textBox = new TextBox();    //starts on the number only textbox
            textBox.Margin = new Thickness(15, 13, 0, 0);
            textBox.Width = 40;
            textBox.Height = 24;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.TextChanged += NumberValidationTextBox;

            stackPanel.Children.Add(textBox);   //makes the textbox a child of the stackpanel


            this.ListStarterResources.Items.Add(stackPanel);
            lstItems.SelectedIndex = SelIndex;  //applies saved index selection
        }

        /// <summary>
        /// deletes selected Starter Resources 
        /// </summary>
        private void OnClickDeleteStarterResources(object sender, RoutedEventArgs e)
        {
            var index = ListStarterResources.SelectedIndex;
            if (index >= 0)
            {
                ListStarterResources.Items.RemoveAt(index);
            }
        }
        /// <summary>
        ///	Saves the current Item (if it exists) via SaveItem() and loads up the new one that was clicked.
        /// </summary>
        private void OnItemChanged(object sender, RoutedEventArgs e)
        {
            int SelIndex = lstItems.SelectedIndex;  //saves selected index so it is not lost
            if (lstItems.SelectedIndex >= 0)    //lstItems.SelectedIndex returns -1 if nothing is selected
            {
                if (CurrentIndex >= 0)  //skips saving the previus selected Item if -1
                {
                    SaveItem(CurrentIndex);
                    ListStarterAbilities.Items.Clear();
                    ListStarterResources.Items.Clear();
                }
                CurrentIndex = lstItems.SelectedIndex;
                majorTrait currentMT = CurrentConfig.IteList[CurrentIndex]; //gets the trait to be loaded

                (this.FindName("nameBox") as TextBox).Text = currentMT.Name; //sets text to the name from the current MajorTrait object
                (this.FindName("descBox") as TextBox).Text = currentMT.Description;  //sets text to the description from the current MajorTrait object

                foreach (string FreeAbil in currentMT.FreeAbilities)    //makes the needed comboboxes to hold the free abilities
                {
                    OnClickAddStarterAbilities(sender, e);
                }
                int ind = 0;
                string TempUID;
                foreach (ComboBox BOX in (this.FindName("ListStarterAbilities") as ListView).Items)
                {
                    TempUID = currentMT.FreeAbilities[ind];
                    BOX.SelectedIndex = CurrentConfig.AbiList.FindIndex(i => string.Equals(i.UID, TempUID));   //selects the free abilities in the comboboxes
                    ind++;
                }
                foreach (AmountUID affRes in currentMT.AffectedResources)	//makes the needed comboboxes to hold the starter resources
                {
                    OnClickAddStarterResources(sender, e);
                }
                ind = 0;
                foreach (StackPanel PANEL in (this.FindName("ListStarterResources") as ListView).Items)
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
                ListStarterAbilities.Items.Clear();
                ListStarterResources.Items.Clear();
            }
            lstItems.SelectedIndex = SelIndex;  //applies saved index selection
        }
        /// <summary>
        /// works as a middle man between butons and SaveItem 
        /// </summary>
        private void OnClickSaveItem(object sender, RoutedEventArgs e)
        {
            SaveItem();
        }

        /// <summary>
        /// Saves everything in the indexed Item to the current config. if no index is given then it saves the currently selected Item.
        /// </summary>
        private void SaveItem(int index = -1)
        {
            int SelIndex = lstItems.SelectedIndex;  //saves selected index so it is not lost

            string UID = "";
            if (index == -1)    //is true when funtion is called via a button
            {
                if (lstItems.SelectedIndex >= 0)
                {
                    UID = CurrentConfig.IteList[lstItems.SelectedIndex].UID;    //uses the selected index to find the wanted UID
                    index = CurrentConfig.IteList.FindIndex(i => string.Equals(i.UID, UID));
                }
            }
            else
            {
                UID = CurrentConfig.IteList[index].UID; //uses the given index to find the wanted UID
            }
            if (UID != "")
            {
                majorTrait currentMT = CurrentConfig.GetTrait(UID);
                currentMT.deleteContent();

                currentMT.Name = (this.FindName("nameBox") as TextBox).Text;
                currentMT.Description = (this.FindName("descBox") as TextBox).Text;

                foreach (ComboBox BOX in (this.FindName("ListStarterAbilities") as ListView).Items)
                {
                    if (BOX.SelectedIndex >= 0)
                    {
                        string TempUID = CurrentConfig.AbiList[BOX.SelectedIndex].UID;
                        currentMT.FreeAbilities.Add(TempUID); // saves the free abilities
                    }
                }

                foreach (StackPanel PANEL in (this.FindName("ListStarterResources") as ListView).Items)
                {
                    foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
                    {
                        foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
                        {
                            if (box.SelectedIndex >= 0 & textBox.Text != "")
                            {
                                string TempUID = CurrentConfig.ResList[box.SelectedIndex].UID;  //gets the affected rescource
                                int TempVal = int.Parse(textBox.Text);  //gets the value
                                currentMT.AffectedResources.Add(new AmountUID(TempUID, TempVal));  //saves the affected rescources and their values
                            }
                        }
                    }
                }
                CurrentConfig.IteList[index] = currentMT;
                ItemCollection.Clear(); // clears the list
                foreach (majorTrait Item in CurrentConfig.IteList)  //rewrites the list.
                {
                    ItemCollection.Add(Item);
                }
                lstItems.SelectedIndex = SelIndex;  //applies saved index selection
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
                ItemCollection.Clear();
                foreach (majorTrait item in CurrentConfig.IteList) //adds all races to ObservableCollection RaceCollection
                {
                    if (item.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        ItemCollection.Add(item);
                    }
                }
                lstItems.SelectedIndex = 0;
            }
            else
            {
                ItemCollection.Clear();
                foreach (majorTrait item in CurrentConfig.IteList) //adds all races to ObservableCollection RaceCollection
                {
                    ItemCollection.Add(item);
                }
                lstItems.SelectedIndex = 0;
            }
        }

        private void ChangeIcon_click(object sender, RoutedEventArgs e)
        {

        }

    }
}
