using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilsModule;

namespace BlacksmithModule
{
    public class Blacksmith : Singleton<Blacksmith>
    {
        private List<IForgeable> forgeableItems = new();
        private readonly List<UpgradeStats> ListStat = new();

        private Dictionary<string, componentGBN> DictionaireComponentGBN = new();

        private void Start()
        {
            InterfaceUpgrade();
        }
        public void InterfaceUpgrade()
        {
            this.GetAllUpgradeableItem();
            this.GetAllUpgrade();
            UiBSGBN.Instance.CreateUiGBNUpgrade(ListStat);
        }
        private void GetAllUpgrade()
        {
            this.ListStat.Clear(); // Assurez-vous de vider la liste avant d'ajouter de nouveaux �l�ments
            DictionaireComponentGBN = new();
            // Parcours de chaque forgeable item
          Debug.Log(  this.forgeableItems.Count);
            foreach (IForgeable item in this.forgeableItems)
            {
                // R�cup�re les donn�es d'am�lioration
                UpgradeStats upgradeData = item.GetStatToUpgradeAndCost();

                // Ajoute les donn�es � ListStat
                this.ListStat.Add(upgradeData);
                DictionaireComponentGBN.Add(upgradeData.name, item.componentGBN);
            }
        }

        private void GetAllUpgradeableItem()
        {
            this.forgeableItems = FindObjectsOfType<MonoBehaviour>().OfType<IForgeable>().ToList();
        }

        public componentGBN ShowSocleUpgradeForGBN(string name)
        {
            return DictionaireComponentGBN[name];
        }
    }
}