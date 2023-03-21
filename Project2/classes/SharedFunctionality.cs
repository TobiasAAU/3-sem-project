using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Project2.classes
{
    internal class Functionality
    {
        public static void DeleteRes(config currentConfig, ListView uiList, ObservableCollection<majorTrait> collection)
        {
            var index = uiList.SelectedIndex;

            List<string> RequiredBy = GetDependencyReferences(currentConfig, collection[index].UID);
            if (index >= 0)
            {
                if (RequiredBy.Count() > 0)
                {
                    List<string> names = new List<string>();
                    foreach (string uid in RequiredBy)
                    {
                        names.Add(currentConfig.GetTrait(uid).Name);
                    }
                    MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
                    MessageBoxResult result = MessageBox.Show("Required by:\n" + string.Join("\n", names) + "\nDo you want to remove this as dependencies for everything? RECOMMENDED: YES(Pressing no will break everything atm)", "This resource is required by other abilities", buttons, MessageBoxImage.Error);

                    if (result.Equals(MessageBoxResult.Yes))
                    {
                        DeleteDependencies(currentConfig, collection[index].UID, RequiredBy);
                    }
                    else if (result.Equals(MessageBoxResult.Cancel))
                    {
                        return;
                    }
                }
                collection.Remove(currentConfig.GetTrait(collection[index].UID, true)); //gets the ability to be deleteted via GetTrait while it deletes it, and deletes its counterpart in AbilityCollection
                uiList.SelectedIndex = collection.Count - 1;
            }
        }

        private static void DeleteDependencies(config currentConfig, string toBeDeleted, List<string> dependers)
        {
            foreach (string depender in dependers)
            {
                majorTrait? temp = currentConfig.GetTrait(depender) as majorTrait;
                if (temp != null)
                {
                    foreach (List<string> orList in temp.Dependencies)
                    {
                        for (int i = 0; i < orList.Count(); i++)
                        {
                            if (orList[i] == toBeDeleted)
                            {
                                orList.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        private static List<string> GetDependencyReferences(config currentConfig, string RequirementUId)
        {
            List<string> requiredByList = new List<string>();
            requiredByList.AddRange(GetDependencyReferencesPartTwo(RequirementUId, currentConfig.AbiList));
            requiredByList.AddRange(GetDependencyReferencesPartTwo(RequirementUId, currentConfig.CarList));
            requiredByList.AddRange(GetDependencyReferencesPartTwo(RequirementUId, currentConfig.RacList));
            requiredByList.AddRange(GetDependencyReferencesPartTwo(RequirementUId, currentConfig.RelList));
            requiredByList = requiredByList.Distinct().ToList();
            return requiredByList;
        }
        static List<string> GetDependencyReferencesPartTwo(string RequirementUId, List<majorTrait> list)
        {
            List<string> requiredByList = new List<string>();
            foreach (majorTrait major in list)
            {
                foreach (List<string> orList in major.Dependencies)
                {
                    foreach (string dependency in orList)
                    {
                        if (dependency == RequirementUId)
                        {
                            requiredByList.Add(major.UID);
                        }
                    }
                }
            }
            return requiredByList;
        }
    }
}
