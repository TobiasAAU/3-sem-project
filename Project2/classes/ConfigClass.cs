using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Project2
{
    /// <summary>
    /// Interaction logic for the configuration
    /// </summary>
    public class config
	{
		public config()
		{
			RacList = new List<majorTrait>();
			AbiList = new List<majorTrait>();
			CarList = new List<majorTrait>();
			RelList = new List<majorTrait>();
            IteList = new List<majorTrait> ();
            ResList = new List<resourceTrait>();
			IcoList = new List<galleryIcon>();
			SaveDestination = new string(System.Reflection.Assembly.GetExecutingAssembly().Location);
			Temppath = new string(string.Empty);
			PlaceholderImage = new string("/Images/Gallery.png");
		}


		public string SaveDestination { get; set; }
        public List<majorTrait> RacList { get; set; }
		public List<majorTrait> AbiList { get; set; }
		public List<majorTrait> CarList { get; set; }
		public List<majorTrait> RelList { get; set; }
		public List<majorTrait> IteList { get; set; }
        public List<resourceTrait> ResList { get; set; }
		public List<galleryIcon> IcoList { get; set; }
		public string Temppath { get; set; }
		public string PlaceholderImage { get;  }

        /// <summary>
        /// Serialize and Write the entire current configuration to a place the user chooses or to the chosen save destination with a given "name"
        /// </summary>
        public void WriteToJson(string name = "")
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            if (name == "")
			{
                SaveFileDialog theFileDialog = new SaveFileDialog();
				theFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
				theFileDialog.InitialDirectory = this.SaveDestination;

				if (theFileDialog.ShowDialog() == true)
				{
					string fullFileName = theFileDialog.FileName;
					string jsonString = JsonSerializer.Serialize(this, options);
					File.WriteAllText(fullFileName, jsonString);
				}
			}
			else
			{
                string jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(this.SaveDestination + name, jsonString);
            }
        }

        /// <summary>
        /// creates and returns a UID given a List type
        /// </summary>
        public string newUID(string type)
		{
			return type  + "-/" + Guid.NewGuid().ToString();
		}
		public dynamic getIcon(string imgName, bool isDelete=false)
		{
			int index;
			index = IcoList.FindIndex(i => string.Equals(i.imgName, imgName));
			galleryIcon SelectedIcon = this.IcoList[index];
            if (isDelete)
            {
                this.IcoList.RemoveAt(index);
            }
            return SelectedIcon;
        }

        public bool saveIcontoList(dynamic galleryIcon)
        {
            IcoList.Add(galleryIcon);
            return true;
        }

        /// <summary>
        /// Finds and returns the trait corosponding to the UID given. optionaly it can delete the trait befor it returns it
        /// </summary>
        public dynamic GetTrait(string uid, bool isDelete = false)
        {
			string[] id = uid.Split("-/"); // "race", "religion", "career", "ability", "Resource"
			int index;
			switch (id[0])
			{
				case "RacList":
					index = RacList.FindIndex(i => string.Equals(i.UID, uid));
					majorTrait SelRac = this.RacList[index];
					if (isDelete)
                    {
						this.RacList.RemoveAt(index);
                    }
					return SelRac;

				case "RelList":
					index = RelList.FindIndex(i => string.Equals(i.UID, uid));
					majorTrait SelRel = this.RelList[index];
					if (isDelete)
					{
						this.RelList.RemoveAt(index);
					}
					return SelRel;

				case "CarList":
					index = CarList.FindIndex(i => string.Equals(i.UID, uid));
					majorTrait SelCar = this.CarList[index];
					if (isDelete)
					{
						this.CarList.RemoveAt(index);
					}
					return SelCar;

				case "AbiList":
					index = AbiList.FindIndex(i => string.Equals(i.UID, uid));
					majorTrait SelAbi = this.AbiList[index];
					if (isDelete)
					{
						this.AbiList.RemoveAt(index);
					}

					return SelAbi;

				case "IteList":
                    index = IteList.FindIndex(i => string.Equals(i.UID, uid));
                    majorTrait SelIte = this.IteList[index];
                    if (isDelete)
                    {
                        this.IteList.RemoveAt(index);
                    }
                    return SelIte;

				case "ResList":
					index = ResList.FindIndex(i => string.Equals(i.UID, uid));
					resourceTrait SelRes = this.ResList[index];
					if (isDelete)
					{
						this.ResList.RemoveAt(index);
					}
					return SelRes;

                default: return false; 

			}
		}

        /// <summary>
        /// reads the traits UID and puts it in its type list
        /// </summary>
        public bool saveToList(dynamic trait)
		{
			string[] id = trait.UID.Split("-/"); // "race", "religion", "career", "ability"

			switch (id[0])
			{
				case "RacList":
					RacList.Add(trait);
					break;

				case "RelList":
					RelList.Add(trait);
					break;

				case "CarList":
					CarList.Add(trait);
                    break;

				case "AbiList":
					AbiList.Add(trait);
                    break;

                case "IteList":
                    IteList.Add(trait);
                    break;

                case "ResList":
					ResList.Add(trait);
					break ;

				default: return false;

			}
			return true;

		}

	}
}
