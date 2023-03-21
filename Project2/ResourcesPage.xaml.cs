using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using static Project2.ResourcePage;
using static Project2.resourceTrait;
using static Project2.GalleryWindow;
using static Project2.galleryIcon;
using System.Diagnostics;

namespace Project2
{
	/// <summary>
	/// Interaction logic for Recources.xaml
	/// </summary>
	public partial class ResourcePage : Page
	{
		public ResourcePage(config currentConfig)
		{
			CurrentConfig = currentConfig;
			InitializeComponent();
			ResourceCollection = new ObservableCollection<resourceTrait>();
			foreach (resourceTrait Resource in CurrentConfig.ResList) //adds all Recources to ObservableCollection ResourceCollection
			{
				ResourceCollection.Add(Resource);
			}
			lstResources.ItemsSource = ResourceCollection;
			CurrentIndex = -1;  //skip the next use of CurrentIndex
			lstResources.SelectedIndex = 0;
		}
		config CurrentConfig { get; set; }
		int CurrentIndex { get; set; }  //keeps track of what index to use
		public ObservableCollection<resourceTrait> ResourceCollection;

		private void ResourceMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			MainWindow mainWindow = new MainWindow(CurrentConfig);
			Application.Current.MainWindow.Content = mainWindow;
		}

		private void btnResource_ClickAdd(object sender, RoutedEventArgs e)
		{
			resourceTrait newResource = new resourceTrait(CurrentConfig.newUID("ResList")) { Name = "new resource"};
			ResourceCollection.Add(newResource);
			CurrentConfig.saveToList(newResource);
			lstResources.SelectedIndex = ResourceCollection.Count - 1;
		}

		private void btnResource_ClickDelete(object sender, RoutedEventArgs e)
		{
			var index = lstResources.SelectedIndex;
			if (index >= 0)
			{
				ResourceCollection.Remove(CurrentConfig.GetTrait(ResourceCollection[index].UID, true)); //gets the Resource to be deleteted via GetTrait while it deletes it, and deletes its counterpart in ResourceCollection
            }
		}
        private void btnResource_ClickCopy(object sender, RoutedEventArgs e)
        {
            var index = lstResources.SelectedIndex;
			if (index >= 0)
			{
				resourceTrait newResource = new resourceTrait(CurrentConfig.newUID("ResList"))
				{
					Description = CurrentConfig.ResList[index].Description,
					Name = CurrentConfig.ResList[index].Name,
					Type = ResourceCollection[index].Type,
					TypeName = CurrentConfig.ResList[index].TypeName
                };

				ResourceCollection.Add(newResource);
				CurrentConfig.saveToList(newResource);
				lstResources.SelectedIndex = ResourceCollection.Count - 1;
			}
        }

        private void OnResourceChanged (object sender, RoutedEventArgs e)
		{
			int SelIndex = lstResources.SelectedIndex;  //saves selected index so it is not lost
            if (lstResources.SelectedIndex >= 0)    //lstResources.SelectedIndex returns -1 if nothing is selected
			{
				if (CurrentIndex >= 0)  //skips saving the previus selected resource if -1
				{
					SaveResource(CurrentIndex);

				}
				CurrentIndex = lstResources.SelectedIndex;
				resourceTrait currentRT = CurrentConfig.ResList[CurrentIndex]; //gets the trait to be loaded

                /*if (currentRT.Image == (CurrentConfig.PlaceholderImage.ToString()))
                {
                    System.Diagnostics.Debug.WriteLine("ONE: we have an image at: " + currentRT.Image);
                    System.Diagnostics.Debug.WriteLine("ONE: we have an image at: " + CurrentConfig.PlaceholderImage);
                    (this.FindName("ChosenImage") as Image).Source = new BitmapImage(new Uri(currentRT.Image, UriKind.Absolute));
                    //(this.FindName("ChosenImage") as Image).Source = new BitmapImage(new Uri(CurrentConfig.PlaceholderImage, UriKind.Relative));
                    System.Diagnostics.Debug.WriteLine("TWO: we have an image at: " + currentRT.Image);
                }
                else if(currentRT.Image == string.Empty)
                {
                    (this.FindName("ChosenImage") as Image).Source = new BitmapImage(new Uri(CurrentConfig.PlaceholderImage, UriKind.Relative));
                    //currentRT.Image = (this.FindName("ChosenImage") as Image).ToString();
                }*/
                if (currentRT.Image != string.Empty)
                {
                    (this.FindName("ChosenImage") as Image).Source = new BitmapImage(new Uri(currentRT.Image, UriKind.Absolute));
                }
                else
                {
                    (this.FindName("ChosenImage") as Image).Source = new BitmapImage(new Uri(CurrentConfig.PlaceholderImage, UriKind.Relative));
                }
                (this.FindName("nameBox") as TextBox).Text = currentRT.Name; //sets text to the name from the current MajorTrait object
				(this.FindName("descBox") as TextBox).Text = currentRT.Description;  //sets text to the description from the current MajorTrait object

                foreach (RadioButton rd in (this.FindName("GridRadioButtons") as Grid).Children.OfType<RadioButton>())
                {
                    if (int.Parse(rd.Tag.ToString()) == currentRT.Type)
                    {
                        rd.IsChecked = true;
                    }
                }
            }
			else
			{
				CurrentIndex = -1;
			}
			lstResources.SelectedIndex = SelIndex;  //applies saved index selection
        }


		public void ChangeIcon_click(object sender, RoutedEventArgs e)
		{
			GalleryWindow newWindow = new GalleryWindow(CurrentConfig);
		
			string imgSource = newWindow.uploadFile(sender, e);
			System.Diagnostics.Debug.WriteLine("we have an image at: " +imgSource);
            ChosenImage.Source = new BitmapImage(new Uri(imgSource, UriKind.Absolute));
			resourceTrait currentRT = CurrentConfig.ResList[CurrentIndex];
			currentRT.Image = ChosenImage.Source.ToString();
        }

        private void OnClickSaveResource(object sender, RoutedEventArgs e)
		{
			SaveResource();
		}

        /// <summary>
        /// Saves everything in the indexed Resource to the current config. if no index is given then it saves the currently selected Resource.
        /// </summary>
        private void SaveResource(int index = -1)
		{
			int SelIndex = lstResources.SelectedIndex;  //saves selected index so it is not lost

            string UID = "";
			if (index == -1)    //is true when funtion is called via a button
			{
				if (lstResources.SelectedIndex >= 0)
				{
					UID = CurrentConfig.ResList[lstResources.SelectedIndex].UID;    //uses the selected index to find the wanted UID
					index = CurrentConfig.ResList.FindIndex(i => string.Equals(i.UID, UID));
				}
			}
			else
			{
				UID = CurrentConfig.ResList[index].UID; //uses the given index to find the wanted UID
			}
			if (UID != "")
			{
				resourceTrait currentRT = CurrentConfig.GetTrait(UID);

				currentRT.Image = (this.FindName("ChosenImage") as Image).ToString();
				currentRT.Name = (this.FindName("nameBox") as TextBox).Text;
				currentRT.Description = (this.FindName("descBox") as TextBox).Text;

				foreach (RadioButton rd in (this.FindName("GridRadioButtons") as Grid).Children.OfType<RadioButton>())
				{
					if (rd.IsChecked == true)
					{
						currentRT.Type = int.Parse(rd.Tag.ToString());
					}
				}

                switch (currentRT.Type)
                {
					case 0:
						currentRT.TypeName = "Stat";
                        break;
                    case 1:
                        currentRT.TypeName = "XP";
                        break;
                    case 2:
                        currentRT.TypeName = "Currency";
                        break;
                    case 3:
                        currentRT.TypeName = "Misc";
                        break;

                }


				CurrentConfig.ResList[index] = currentRT;
				ResourceCollection.Clear();    // clears the list
				foreach (resourceTrait res in CurrentConfig.ResList)  //rewrites the list.
				{
					ResourceCollection.Add(res);
				}

				lstResources.SelectedIndex = SelIndex;  //applies saved index selection
            }
		}

        private void searchbar_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = (this.FindName("searchbar") as TextBox).Text;
            if (searchText != "")
            {
                ResourceCollection.Clear();
                foreach (resourceTrait resource in CurrentConfig.ResList)
                {
                    if (resource.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        ResourceCollection.Add(resource);
                    }
                }
                lstResources.SelectedIndex = 0;
            }
            else
            {
                ResourceCollection.Clear();
                foreach (resourceTrait resource in CurrentConfig.ResList)
                {
                    ResourceCollection.Add(resource);
                }
                lstResources.SelectedIndex = 0;
            }
        }
    }
}

