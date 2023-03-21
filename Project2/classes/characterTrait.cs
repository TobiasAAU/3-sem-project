using System;
using System.Collections.Generic;


namespace Project2
{
	public class characterTrait
	{

		public characterTrait(string uid)
		{
			UID = uid; //r-**** for resource 0-**** for misc 1-**** for ability 2-**** for race
			Image = "";
			Name = "";
			Description = "";
		}

		public string Name { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public string UID { get; set; }

	}

	public class majorTrait : characterTrait
	{
		public majorTrait(string uid) : base(uid)
		{
			Cost = 0;
			CostTypes = new List<string>();
            Exclusions = new List<string>();
            Dependencies = new List<List<string>>();
			FreeAbilities = new List<string>();
			Discounts = new List<AmountUID>();
			AffectedResources = new List<AmountUID>();
			PlayerReq = new string("");
		}

		public class AmountUID
		{
			public AmountUID(string uid, int amount)
			{
				UID = uid;
				Amount = amount;
			}
			public string UID { get; set; }
			public int Amount { get; set; }
		}

		public int Cost { get; set; }
		public List<string> CostTypes { get; set; }
        public List<string> Exclusions { get; set; }
        public List<List<string>> Dependencies { get; set; }
		public List<string> FreeAbilities { get; set; }
		public List<AmountUID> Discounts { get; set; }
		public List<AmountUID> AffectedResources { get; set; }
		public string PlayerReq { get; set; }

		public void deleteContent()
		{
			this.Name = "";
			this.Image = "";
			this.PlayerReq = "";
			this.Description = "";
			this.Cost = 0;
			this.CostTypes = new List<string>();
            this.Exclusions = new List<string>();
            this.FreeAbilities = new List<string>();
			this.Discounts = new List<AmountUID>();
			this.AffectedResources = new List<AmountUID>();
			this.Dependencies = new List<List<string>>();
        }
	}

	public class resourceTrait : characterTrait
	{
		public resourceTrait(string uid) : base(uid)
		{
			Type = 3;
        }

		public int Type { get; set; }
        public string TypeName { get; set; }
    }
}