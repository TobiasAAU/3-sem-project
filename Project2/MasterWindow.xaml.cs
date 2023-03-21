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
using System.Windows.Shapes;
using static Project2.majorTrait;

namespace Project2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

        }
        public config CurrentConfig { get; set; }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            config CurrentConfig = new();


            majorTrait smite = new majorTrait(CurrentConfig.newUID("AbiList"));
            majorTrait EldrichBLAST = new majorTrait(CurrentConfig.newUID("AbiList"));
            majorTrait Warrior = new majorTrait(CurrentConfig.newUID("CarList"));
            majorTrait Warlock = new majorTrait(CurrentConfig.newUID("CarList"));
            majorTrait Dwarf = new majorTrait(CurrentConfig.newUID("RacList"));
            majorTrait Elf = new majorTrait(CurrentConfig.newUID("RacList"));
            majorTrait CultOfSigmar = new majorTrait(CurrentConfig.newUID("RelList"));
            resourceTrait health = new resourceTrait(CurrentConfig.newUID("ResList"));
            majorTrait hammer = new majorTrait(CurrentConfig.newUID("IteList"));
            resourceTrait mana = new resourceTrait(CurrentConfig.newUID("ResList"));
            resourceTrait xp = new resourceTrait(CurrentConfig.newUID("ResList"));

            smite.Name = "SMITE";
            smite.Description = "slaa gud modstander med hellig styrke";
            smite.Cost = 5;
            smite.CostTypes = new List<string>() { xp.UID };
            smite.Dependencies = new List<List<string>>()
            {
                new List<string>(){ Warrior.UID}
            };
            smite.Discounts.Add(new AmountUID(Warrior.UID, 3));
            smite.AffectedResources.Add(new AmountUID(health.UID, 2));
            CurrentConfig.AbiList.Add(smite);
            
            EldrichBLAST.Name = "Eldrich BLAST";
            EldrichBLAST.Description = "it's a BLAST to use";
            EldrichBLAST.Cost = 4;
            EldrichBLAST.CostTypes = new List<string>() { xp.UID };
            EldrichBLAST.Dependencies = new List<List<string>>()
            {
                new List<string>(){ Warlock.UID}
            };
            EldrichBLAST.Discounts.Add(new AmountUID(Warlock.UID, 2));
            EldrichBLAST.AffectedResources.Add(new AmountUID(mana.UID, 2));
            CurrentConfig.AbiList.Add(EldrichBLAST);
            
            Warrior.Name = "Warrior";
            Warrior.Description = "am gonna swing a sword";
            Warrior.AffectedResources.Add(new AmountUID(health.UID, 2));
            CurrentConfig.CarList.Add(Warrior);

            Warlock.Name = "Warlock";
            Warlock.Description = "-_-";
            Warlock.Dependencies = new List<List<string>>()
            {
                new List<string>(){ Warrior.UID}
            };
            Warlock.AffectedResources.Add(new AmountUID(mana.UID, 2));
            CurrentConfig.CarList.Add(Warlock);

            Elf.Name = "Elf";
            Elf.Description = "the most pompius pricks of all races";
            Elf.FreeAbilities = new List<string>() {EldrichBLAST.UID};
            Elf.AffectedResources.Add(new AmountUID(health.UID, 2));
            Elf.AffectedResources.Add(new AmountUID(mana.UID, 3));
            CurrentConfig.RacList.Add(Elf);

            Dwarf.Name = "Dwarf";
            Dwarf.Description = "this be armor";
            Dwarf.FreeAbilities = new List<string>() {smite.UID};
            Dwarf.AffectedResources.Add(new AmountUID(health.UID, 4));
            CurrentConfig.RacList.Add(Dwarf);

            CultOfSigmar.Name = "Cult Of Sigmar";
            CultOfSigmar.Description = "The Cult of Sigmar, also sometimes called the Church of Sigmar, the Holy Temple of Sigmar, the Clergy of Sigmar or simply the Sigmarite Cult, is the state church of the Empire that administrates the worship of that realm's patron god, Sigmar Heldenhammer.";
            CultOfSigmar.AffectedResources.Add(new AmountUID(health.UID, 2));
            CurrentConfig.saveToList(CultOfSigmar);

            health.Name = "health";
            health.Description = "ye dead if this be zero";
            health.Type = 0;
            health.TypeName = "Stat";
            CurrentConfig.ResList.Add(health);

            mana.Name = "Mana";
            mana.Description = "le-Magic juice";
            mana.Type = 0;
            mana.TypeName = "Stat";
            CurrentConfig.ResList.Add(mana);

            xp.Name = "xp";
            xp.Description = "GAINS!!";
            xp.Type = 1;
            xp.TypeName = "XP";
            CurrentConfig.ResList.Add(xp);

            hammer.Name = "hammer";
            hammer.Description = "Le-Bonk";
            CurrentConfig.IteList.Add(hammer);

            MainWindow mainwindow = new MainWindow(CurrentConfig);
            Application.Current.MainWindow.Content = mainwindow;
        }
    }
}
