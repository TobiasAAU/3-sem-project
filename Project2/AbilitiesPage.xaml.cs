using ControlzEx.Standard;
using Project2.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Shell;
using static Project2.majorTrait;

namespace Project2
{
	/// <summary>
	/// Interaction logic for Abilities.xaml
	/// </summary>
	public partial class AbilitiesPage : Page
	{
		public AbilitiesPage(config currentConfig) //bility window constructor
		{
			CurrentConfig = currentConfig;
			InitializeComponent();
            RequireTypeBox.Items.Add("Race");
            RequireTypeBox.Items.Add("Ability");
            RequireTypeBox.Items.Add("Career");
            RequireTypeBox.Items.Add("Religion");
            RequireTypeBox.SelectedIndex = 0;
			MTCollection = new ObservableCollection<majorTrait>();
			foreach (majorTrait ability in CurrentConfig.AbiList) //adds all abilities to ObservableCollection MTCollection
			{
				MTCollection.Add(ability);
			}
			lstMTraits.ItemsSource = MTCollection;
			CurrentIndex = -1;  //skip the next use of CurrentIndex
			lstMTraits.SelectedIndex = 0;
        }
		public config CurrentConfig { get; set; }
		int CurrentIndex { get; set; }  //keeps track of what index to use
		private ObservableCollection<majorTrait> MTCollection;   //itemSource for lstMTraits ListVeiw


		private void MainMenu_MouseClick(object sender, MouseButtonEventArgs e)
		{
			MainWindow mainWindow = new MainWindow(CurrentConfig);
			Application.Current.MainWindow.Content = mainWindow;
		}

		private void btnAbility_ClickAdd(object sender, EventArgs e)
		{
			majorTrait tempAbility = new majorTrait(CurrentConfig.newUID("AbiList")) { Name = "new ability" };   //makes the new ability object
			CurrentConfig.saveToList(tempAbility);
			MTCollection.Add(tempAbility);
			lstMTraits.SelectedIndex = MTCollection.Count - 1;
		}

		private void btnAbility_ClickDelete(object sender, EventArgs e)
		{
			Functionality.DeleteRes(CurrentConfig, lstMTraits, MTCollection);
		}
		private void btnAbility_ClickCopy(object sender, EventArgs e)
		{
			var index = lstMTraits.SelectedIndex;
			if (index >= 0)
            {
				majorTrait tempAbility = new majorTrait(CurrentConfig.newUID("AbiList"))
				{
					AffectedResources = CurrentConfig.AbiList[index].AffectedResources,
					Dependencies = CurrentConfig.AbiList[index].Dependencies,
					Exclusions = CurrentConfig.AbiList[index].Exclusions,
					Discounts = CurrentConfig.AbiList[index].Discounts,
					Name = CurrentConfig.AbiList[index].Name,
					Description = CurrentConfig.AbiList[index].Description,
					Cost = CurrentConfig.AbiList[index].Cost,
                    FreeAbilities = CurrentConfig.AbiList[index].FreeAbilities,
                    CostTypes = CurrentConfig.AbiList[index].CostTypes

                };

                CurrentConfig.saveToList(tempAbility);
				MTCollection.Add(tempAbility);
				lstMTraits.SelectedIndex = MTCollection.Count - 1;
			}
        }
        private void OnClickAddFreeAbilities(object sender, EventArgs e)
        {
            int SelIndex = lstMTraits.SelectedIndex;  //saves selected index so it is not lost
            ComboBox comboBox = new ComboBox();
            comboBox.IsReadOnly = true;
            comboBox.IsDropDownOpen = false;
            comboBox.Margin = new Thickness(5, 5, 0, 0);
            comboBox.Height = 24;
            comboBox.Width = 185;
            comboBox.DisplayMemberPath = "Name";
            foreach (majorTrait abi in CurrentConfig.AbiList)   //adds all abilities from CurrentConfig to the combobox
            {
				if (abi != lstMTraits.SelectedItem)
                {
                    comboBox.Items.Add(abi);
                }
            }

            this.ListFreeAbilities.Items.Add(comboBox);
            lstMTraits.SelectedIndex = SelIndex;  //applies saved index selection
        }

        /// <summary>
        /// Deletes selected starterAbility
        /// </summary>
        private void OnClickDeleteFreeAbilities(object sender, RoutedEventArgs e)
        {
            var index = ListFreeAbilities.SelectedIndex;
            if (index >= 0)
            {
                ListFreeAbilities.Items.RemoveAt(index);
            }
        }

        /// <summary>
        /// adds a row to hold a or requrement
        /// </summary>
        private void OnClickAddRequirmentsList(object sender, EventArgs e)
		{
			this.InitializeComponent();
			StackPanel ReqStackPanel = new StackPanel();
			ReqStackPanel.Height = 36;
            ReqStackPanel.Orientation = Orientation.Horizontal;
			ListRequirements.Items.Add(ReqStackPanel);
			ListRequirements.SelectedIndex = ListRequirements.Items.Count - 1;
		}

        /// <summary>
        /// Deletes the selected row
        /// </summary>
        private void OnClickDeleteRequirmentsList(object sender, RoutedEventArgs e)
		{
			var index = ListRequirements.SelectedIndex;
			if (index >= 0)
			{
				ListRequirements.Items.RemoveAt(index);
				ListRequirements.SelectedIndex = ListRequirements.Items.Count - 1;
			}
		}

        /// <summary>
        /// Adds a and requrement to the selected row
        /// </summary>
        private void OnClickAddRequirment(object sender, EventArgs e)
        {
            this.InitializeComponent();

            if (ListRequirements.SelectedValue != null)
            {
				int count = (ListRequirements.SelectedValue as StackPanel).Children.Count;
                ComboBox comboBoxSelection = new ComboBox();
                comboBoxSelection.IsReadOnly = true;
                comboBoxSelection.IsDropDownOpen = false;
                comboBoxSelection.Margin = new Thickness(5, 5, 0, 0);
                comboBoxSelection.Height = 26;
                comboBoxSelection.Width = 185;
                comboBoxSelection.VerticalAlignment = VerticalAlignment.Top;
                comboBoxSelection.DisplayMemberPath = "Name";


				switch (RequireTypeBox.SelectedItem.ToString())
				{
					case "Race":
						foreach (majorTrait MT in CurrentConfig.RacList)
                        {
                            if (MT != lstMTraits.SelectedItem)
                            {
                                comboBoxSelection.Items.Add(MT);
                            }
                        }
						break;

                    case "Ability":
                        foreach (majorTrait MT in CurrentConfig.AbiList)
                        {
                            if (MT != lstMTraits.SelectedItem)
                            {
                                comboBoxSelection.Items.Add(MT);
                            }
                        }
                        break;

                    case "Career":
                        foreach (majorTrait MT in CurrentConfig.CarList)
                        {
                            if (MT != lstMTraits.SelectedItem)
                            {
                                comboBoxSelection.Items.Add(MT);
                            }
                        }
                        break;

                    case "Religion":
                        foreach (majorTrait MT in CurrentConfig.RelList)
                        {
                            if (MT != lstMTraits.SelectedItem)
                            {
                                comboBoxSelection.Items.Add(MT);
                            }
                        }
                        break;
                }

                Label and = new Label();
                and.Height = 26;
                and.VerticalAlignment = VerticalAlignment.Top;

                if (count == 0)
                {
                    and.Width = 30;
                    and.Margin = new Thickness(0, 5, 0, 0);
                    and.Content = "OR";
				}
				else
                {
                    and.Width = 35;
                    and.Margin = new Thickness(5, 5, 0, 0);
                    and.Content = "AND";
				}

                (ListRequirements.SelectedValue as StackPanel).Children.Add(and);
                (ListRequirements.SelectedValue as StackPanel).Children.Add(comboBoxSelection);

            }
        }

        private void OnClick_AddCostType(object sender, EventArgs e)
		{
			int SelIndex = lstMTraits.SelectedIndex;  //saves selected index so it is not lost
            this.InitializeComponent();

			ComboBox comboBoxOne = new ComboBox();  //starts on the currency combobox
			comboBoxOne.Text = "Select currency";
			comboBoxOne.IsReadOnly = true;
			comboBoxOne.IsDropDownOpen = false;
			comboBoxOne.Margin = new Thickness(5, 5, 0, 0);
			comboBoxOne.Height = 24;
			comboBoxOne.Width = 185;

			comboBoxOne.DisplayMemberPath = "Name";
			foreach (resourceTrait res in CurrentConfig.ResList)
            {
                if (res != lstMTraits.SelectedItem)
                {
                    comboBoxOne.Items.Add(res);
                }
			}

			this.ListCosts.Items.Add(comboBoxOne);   //makes the combobox a child of the stackpanel
			ListCosts.SelectedIndex = ListCosts.Items.Count - 1;
			lstMTraits.SelectedIndex = SelIndex;    //applies saved index selection
        }

		private void OnClick_DeleteCostType(object sender, EventArgs e)
		{
			var index = ListCosts.SelectedIndex;
			if (index >= 0)
			{
				ListCosts.Items.RemoveAt(index);
				ListCosts.SelectedIndex = ListCosts.Items.Count - 1;
			}
		}
		private void OnClick_AddAffectedResources(object sender, EventArgs e)
		{
			int SelIndex = lstMTraits.SelectedIndex;  //saves selected index so it is not lost
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
                if (res != lstMTraits.SelectedItem)
                {
                    comboBoxOne.Items.Add(res);
                }
			}


			stackPanel.Children.Add(comboBoxOne);   //makes the combobox a child of the stackpanel

			TextBox textBox = new TextBox();    //starts on the number only textbox
			textBox.Margin = new Thickness(15, 13, 0, 0);
			textBox.Width = 40;
			textBox.Height = 24;
			textBox.VerticalAlignment = VerticalAlignment.Top;
			textBox.TextChanged += NumberValidationTextBox;

			stackPanel.Children.Add(textBox);   //makes the textbox a child of the stackpanel


			this.ListAffectedResources.Items.Add(stackPanel);
			ListAffectedResources.SelectedIndex = ListAffectedResources.Items.Count - 1;
			lstMTraits.SelectedIndex = SelIndex;  //applies saved index selection
        }
		/// <summary>
		/// deletes selected Starter Resources 
		/// </summary>
		private void OnClick_DeleteAffectedResources(object sender, EventArgs e)
		{
			var index = ListAffectedResources.SelectedIndex;
			if (index >= 0)
			{
				ListAffectedResources.Items.RemoveAt(index);
				ListAffectedResources.SelectedIndex = ListAffectedResources.Items.Count - 1;
			}
		}

		private void OnClick_AddExclusion(object sender, EventArgs e)
		{
			int SelIndex = lstMTraits.SelectedIndex;  //saves selected index so it is not lost
            this.InitializeComponent();

			ComboBox comboBoxOne = new ComboBox();  //starts on the recouse combobox
			comboBoxOne.Text = "Select Stat";
			comboBoxOne.IsReadOnly = true;
			comboBoxOne.IsDropDownOpen = false;
			comboBoxOne.Margin = new Thickness(5, 5, 0, 0);
			comboBoxOne.Height = 24;
			comboBoxOne.Width = 185;
			comboBoxOne.DisplayMemberPath = "Name";
			string[] name = ((sender as Button).Content.ToString()).Split(' ');
			if (name[1] == "CAREER")
			{
				foreach (majorTrait MT in CurrentConfig.CarList)
                {
                    if (MT != lstMTraits.SelectedItem)
                    {
                        comboBoxOne.Items.Add(MT);
                    }
                }
			}
			else if (name[1] == "ABILITY")
			{
				foreach (majorTrait MT in CurrentConfig.AbiList)
                {
                    if (MT != lstMTraits.SelectedItem)
                    {
                        comboBoxOne.Items.Add(MT);
                    }
                }
			}
			else if (name[1] == "RACE")
			{
				foreach (majorTrait MT in CurrentConfig.RacList)
                {
                    if (MT != lstMTraits.SelectedItem)
                    {
                        comboBoxOne.Items.Add(MT);
                    }
                }
			}

			this.ListExclusion.Items.Add(comboBoxOne);
			ListExclusion.SelectedIndex = ListExclusion.Items.Count - 1;
			lstMTraits.SelectedIndex = SelIndex;  //applies saved index selection
        }

		private void OnClick_DeleteExclusion(object sender, EventArgs e)
		{
			var index = ListExclusion.SelectedIndex;
			if (index >= 0)
			{
				ListExclusion.Items.RemoveAt(index);
				ListExclusion.SelectedIndex = ListExclusion.Items.Count - 1;
			}
		}

		private void OnClick_AddDiscount(object sender, EventArgs e)
		{
			int SelIndex = lstMTraits.SelectedIndex;  //saves selected index so it is not lost
            this.InitializeComponent();
			StackPanel stackPanelDiscounts = new StackPanel();
			stackPanelDiscounts.Orientation = Orientation.Horizontal;

			ComboBox comboBoxOne = new ComboBox();  //starts on the recouse combobox
			comboBoxOne.Text = "Select Stat";
			comboBoxOne.IsReadOnly = true;
			comboBoxOne.IsDropDownOpen = false;
			comboBoxOne.Margin = new Thickness(5, 5, 0, 0);
			comboBoxOne.Height = 24;
			comboBoxOne.Width = 185;
			comboBoxOne.DisplayMemberPath = "Name";
			string[] name = ((sender as Button).Content.ToString()).Split(' ');
			if (name[1] == "CAREER")
			{
				foreach (majorTrait MT in CurrentConfig.CarList)
                {
                    if (MT != lstMTraits.SelectedItem)
                    {
                        comboBoxOne.Items.Add(MT);
                    }
                }
			}
			else if (name[1] == "ABILITY")
			{
				foreach (majorTrait MT in CurrentConfig.AbiList)
                {
                    if (MT != lstMTraits.SelectedItem)
                    {
                        comboBoxOne.Items.Add(MT);
                    }
                }
			}
			else if (name[1] == "RACE")
			{
				foreach (majorTrait MT in CurrentConfig.RacList)
                {
                    if (MT != lstMTraits.SelectedItem)
                    {
                        comboBoxOne.Items.Add(MT);
                    }
                }
			}

			stackPanelDiscounts.Children.Add(comboBoxOne);   //makes the combobox a child of the stackpanel

			TextBox textBox = new TextBox();    //starts on the number only textbox
			textBox.Margin = new Thickness(15, 13, 0, 0);
			textBox.Width = 40;
			textBox.Height = 24;
			textBox.VerticalAlignment = VerticalAlignment.Top;
			textBox.TextChanged += NumberValidationTextBox;

			stackPanelDiscounts.Children.Add(textBox);   //makes the textbox a child of the stackpanel


			this.ListDiscounts.Items.Add(stackPanelDiscounts);
			ListDiscounts.SelectedIndex = ListDiscounts.Items.Count - 1;
			lstMTraits.SelectedIndex = SelIndex;  //applies saved index selection
        }

		/// <summary>
		/// deletes selected Starter Resources 
		/// </summary>
		private void OnClick_DeleteDiscount(object sender, RoutedEventArgs e)
		{
			var index = ListDiscounts.SelectedIndex;
			if (index >= 0)
			{
				ListDiscounts.Items.RemoveAt(index);
				ListDiscounts.SelectedIndex = ListDiscounts.Items.Count - 1;
			}
		}

		bool amworkingonchange = false;
		private void OnAbilityChanged(object sender, RoutedEventArgs e)
		{
			if (amworkingonchange == false)
			{

				amworkingonchange = true;
				abilityChange(sender, e);

				//Task.Delay(2);
				amworkingonchange = false;
			}

		}


		private void abilityChange(object sender, EventArgs e)
		{
			if (lstMTraits.SelectedIndex >= 0)    //lstMTraits.SelectedIndex returns -1 if nothing is selected
			{
				if (CurrentIndex >= 0)
				{
					SaveAbility(CurrentIndex);
					ListRequirements.Items.Clear();
					ListAffectedResources.Items.Clear();
					ListDiscounts.Items.Clear();
					ListCosts.Items.Clear();
                    ListFreeAbilities.Items.Clear();
                    ListExclusion.Items.Clear();
                    (this.FindName("nameBox") as TextBox).Text = "";
                    (this.FindName("descBox") as TextBox).Text = "";
                    (this.FindName("costBox") as TextBox).Text = "";
                }
				CurrentIndex = lstMTraits.SelectedIndex;
				majorTrait currentMT = CurrentConfig.AbiList[CurrentIndex]; //gets the trait to be loaded
		 
				(this.FindName("nameBox") as TextBox).Text = currentMT.Name; //sets text to the name from the current MajorTrait object
				(this.FindName("descBox") as TextBox).Text = currentMT.Description;  //sets text to the description from the current MajorTrait object

				foreach (AmountUID AffRes in currentMT.AffectedResources)
				{
					OnClick_AddAffectedResources(sender, e);
				}
				int ind = 0;
				string TempUID;
				foreach (StackPanel PANEL in (this.FindName("ListAffectedResources") as ListView).Items)
				{
					foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
					{
						foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
						{
							AmountUID tempAffRes = currentMT.AffectedResources[ind];
							box.SelectedItem = CurrentConfig.GetTrait(tempAffRes.UID); //selects the starter resources in the comboboxes
							textBox.Text = tempAffRes.Amount.ToString(); //sets the right amounts in the textboxes
						}
					}
					ind++;
				}

                (this.FindName("costBox") as TextBox).Text = currentMT.Cost.ToString();
                foreach (string CostType in currentMT.CostTypes)
				{
					OnClick_AddCostType(sender, e);
				}
				ind = 0;
				foreach (ComboBox BOX in (this.FindName("ListCosts") as ListView).Items)
				{
					TempUID = currentMT.CostTypes[ind];
					BOX.SelectedItem = CurrentConfig.GetTrait(TempUID);
					ind++;
				}

                foreach (string FreeAbil in currentMT.FreeAbilities)    //makes the needed comboboxes to hold the free abilities
                {
                    OnClickAddFreeAbilities(sender, e);
                }
                ind = 0;
                foreach (ComboBox BOX in (this.FindName("ListFreeAbilities") as ListView).Items)
                {
                    TempUID = currentMT.FreeAbilities[ind];
                    BOX.SelectedItem = CurrentConfig.GetTrait(TempUID);    //selects the free abilities in the comboboxes
                    ind++;
                }

                foreach (string str in currentMT.Exclusions)
				{
					string[] typeid = str.Split("-/");
					if (typeid[0] == "RacList")
					{
						Button nsender = new Button() { Content = "ADD RACE" };
						OnClick_AddExclusion(nsender, e);
					}
					else if (typeid[0] == "AbiList")

					{
						Button nsender = new Button() { Content = "ADD ABILITY" };
						OnClick_AddExclusion(nsender, e);
					}
					else if (typeid[0] == "CarList")
					{
						Button nsender = new Button() { Content = "ADD CAREER" };
						OnClick_AddExclusion(nsender, e);
					}
				}

				ind = 0;
				ListView ltr = (this.FindName("ListExclusion") as ListView);
				foreach (ComboBox box in ltr.Items)
				{
                    box.SelectedItem = CurrentConfig.GetTrait(currentMT.Exclusions[ind]);
                    ind++;
				}


				foreach (AmountUID Disc in currentMT.Discounts)
				{
					string[] Condui = Disc.UID.Split("-/");
					if (Condui[0] == "RacList")
					{
						Button nsender = new Button() { Content = "ADD RACE" };
						OnClick_AddDiscount(nsender, e);
					}
					else if (Condui[0] == "AbiList")

					{
						Button nsender = new Button() { Content = "ADD ABILITY" };
						OnClick_AddDiscount(nsender, e);
					}
					else if (Condui[0] == "CarList")
					{
						Button nsender = new Button() { Content = "ADD CAREER" };
						OnClick_AddDiscount(nsender, e);
					}

				}
				ind = 0;
				foreach (StackPanel PANEL in (this.FindName("ListDiscounts") as ListView).Items)
				{
					foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
					{
						foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
						{
							AmountUID tempDisc = currentMT.Discounts[ind];
                            box.SelectedItem = CurrentConfig.GetTrait(tempDisc.UID);
                            textBox.Text = tempDisc.Amount.ToString();	//sets the right amounts in the textboxes
						}
					}
					ind++;
				}

                string[] id;
				ind = 0;
				foreach (List<string> list in currentMT.Dependencies)
				{
					OnClickAddRequirmentsList(sender , e);
					for (int i = 0; i < list.Count; i++)
					{
                        id = currentMT.Dependencies[ind][i].Split("-/");
                        switch (id[0])
                        {
                            case "RacList":
                                RequireTypeBox.SelectedIndex = 0;
                                break;

                            case "RelList":
                                RequireTypeBox.SelectedIndex = 3;
                                break;

                            case "CarList":
                                RequireTypeBox.SelectedIndex = 2;
                                break;

                            case "AbiList":
                                RequireTypeBox.SelectedIndex = 1;
                                break;
                        }
                        OnClickAddRequirment(sender , e);
					}
					ind++;
				}

                ind = 0;
                int ind2 = 0;
                foreach (StackPanel PANEL in (this.FindName("ListRequirements") as ListView).Items)
                {
                    foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
                    {
                        try
                        {
                            box.SelectedItem = CurrentConfig.GetTrait(currentMT.Dependencies[ind][ind2]);
                        }
                        catch { }
                        ind2++;
                    }
                    ind2 = 0;
                    ind++;
                }
            }
			else
			{
				CurrentIndex = -1;
				ListRequirements.Items.Clear();
				ListAffectedResources.Items.Clear();
				ListDiscounts.Items.Clear();
				ListCosts.Items.Clear();
                ListFreeAbilities.Items.Clear();
                ListExclusion.Items.Clear();
                (this.FindName("nameBox") as TextBox).Text = "";
                (this.FindName("descBox") as TextBox).Text = "";
                (this.FindName("costBox") as TextBox).Text = "";
            }
		}

		private void OnClickSaveAbility(object sender, EventArgs e)
		{
			SaveAbility();
		}

		private void SaveAbility(int index = -1)
		{
			int SelIndex = lstMTraits.SelectedIndex;  //saves selected index so it is not lost

            string UID = "";
			if (index == -1)    //is true when funtion is called via a button
			{
				if (lstMTraits.SelectedIndex >= 0)
				{
					UID = CurrentConfig.AbiList[lstMTraits.SelectedIndex].UID;    //uses the selected index to find the wanted UID
					index = CurrentConfig.AbiList.FindIndex(i => string.Equals(i.UID, UID));
				}
			}
			else
			{
				UID = CurrentConfig.AbiList[index].UID; //uses the given index to find the wanted UID
			}
			if (UID != "")
			{
				majorTrait currentMT = CurrentConfig.GetTrait(UID);
				currentMT.deleteContent();

				currentMT.Name = (this.FindName("nameBox") as TextBox).Text;
				currentMT.Description = (this.FindName("descBox") as TextBox).Text;
				string textcost = (this.FindName("costBox") as TextBox).Text;
				if (textcost != "")
				{
					currentMT.Cost = int.Parse(textcost);
				}
				else
                {
                    currentMT.Cost = 0;
                }

				foreach (StackPanel PANEL in (this.FindName("ListAffectedResources") as ListView).Items)
				{
					foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
					{
						foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
						{
							if (box.SelectedIndex >= 0 & textBox.Text != "")
							{
								string TempUID = (box.SelectedItem as resourceTrait).UID;  //gets the affected rescource
								int TempVal = int.Parse(textBox.Text);  //gets the value
								currentMT.AffectedResources.Add(new AmountUID(TempUID, TempVal));   //saves the affected rescources and their values
							}
						}
					}
				}

                foreach (ComboBox BOX in (this.FindName("ListFreeAbilities") as ListView).Items)
                {
                    if (BOX.SelectedIndex >= 0)
                    {
                        string TempUID = (BOX.SelectedItem as majorTrait).UID;
                        currentMT.FreeAbilities.Add(TempUID); // saves the free abilities
                    }
                }

                foreach (ComboBox BOX in (this.FindName("ListCosts") as ListView).Items)
				{
					if (BOX.SelectedIndex >= 0)
					{
						string TempUID = (BOX.SelectedItem as resourceTrait).UID;
						currentMT.CostTypes.Add(TempUID);
					}
				}

				foreach (StackPanel PANEL in (this.FindName("ListDiscounts") as ListView).Items)
				{
					foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
					{
						foreach (TextBox textBox in PANEL.Children.OfType<TextBox>())
						{
							if (box.SelectedItem != null & textBox.Text != "")
							{
								currentMT.Discounts.Add(new AmountUID((box.SelectedItem as majorTrait).UID, int.Parse(textBox.Text)));
							}
						}
					}
				}

				foreach (ComboBox box in (this.FindName("ListExclusion") as ListView).Items.OfType<ComboBox>())
				{
					if (box.SelectedItem != null)
					{
						currentMT.Exclusions.Add((box.SelectedItem as majorTrait).UID);
					}
				}

				int ind = 0;
				foreach (StackPanel PANEL in (this.FindName("ListRequirements") as ListView).Items)
				{
					currentMT.Dependencies.Add(new List<string>());
					foreach (ComboBox box in PANEL.Children.OfType<ComboBox>())
					{
						if (box.SelectedItem != null)
						{
							currentMT.Dependencies[ind].Add((box.SelectedItem as majorTrait).UID);
						}
					}
					if (currentMT.Dependencies[ind].Count == 0)
					{
						currentMT.Dependencies.RemoveAt(ind);
						ind--;
					}
					ind++;
				}



				CurrentConfig.AbiList[index] = currentMT;
				MTCollection.Clear(); // clears the list
				foreach (majorTrait ability in CurrentConfig.AbiList)  //rewrites the list.
				{
					MTCollection.Add(ability);
				}

				lstMTraits.SelectedIndex = SelIndex;  //applies saved index selection
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
                MTCollection.Clear();
                foreach (majorTrait ability in CurrentConfig.AbiList) //adds all races to ObservableCollection RaceCollection
                {
                    if (ability.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        MTCollection.Add(ability);
                    }
                }
                lstMTraits.SelectedIndex = 0;
            }
            else
            {
                MTCollection.Clear();
                foreach (majorTrait ability in CurrentConfig.AbiList) //adds all races to ObservableCollection RaceCollection
				{
                    MTCollection.Add(ability);
                }
                lstMTraits.SelectedIndex = 0;
            }
        }
    }
}
