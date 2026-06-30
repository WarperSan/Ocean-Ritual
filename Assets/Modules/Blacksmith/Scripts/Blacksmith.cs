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

        public void InterfaceUpgrade()
        {
            GetAllUpgradeableItem();
            GetAllUpgrade();
            UiBSGBN.Instance?.CreateUiGBNUpgrade(ListStat);
        }

        private void GetAllUpgrade()
        {
            ListStat.Clear(); // Assurez-vous de vider la liste avant d'ajouter de nouveaux �l�ments
            DictionaireComponentGBN = new Dictionary<string, componentGBN>();
            // Parcours de chaque forgeable item

            foreach (IForgeable item in forgeableItems)
            {
                // R�cup�re les donn�es d'am�lioration
                UpgradeStats upgradeData = item.GetStatToUpgradeAndCost();

                // Ajoute les donn�es � ListStat
                ListStat.Add(upgradeData);
                DictionaireComponentGBN.Add(upgradeData.name, item.componentGBN);
            }
        }

        private void GetAllUpgradeableItem()
        {
            //Debug.Log("Nombre d'objets forgeables trouvés : " + forgeableItems.Count);
            forgeableItems = FindObjectsOfType<MonoBehaviour>().OfType<IForgeable>().ToList();
            //Debug.Log("Nombre d'objets forgeables trouvés après recherche : " + forgeableItems.Count);

            foreach (IForgeable item in forgeableItems)
                Debug.Log("Nom de l'objet : " + ((MonoBehaviour)item).gameObject.name);
        }

        public componentGBN ShowSocleUpgradeForGBN(string name) => DictionaireComponentGBN[name];

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}