using System.Text;
using TMPro;
using UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace BlacksmithModule
{
    /// <summary>
    /// Item that displays the stats upgrade of a given equipment
    /// </summary>
    public class UpgradeStatsItem : UIComponent
    {
        #region Fields

        [Header("Fields")]
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Button button;

        #endregion

        public void SetItem(UpgradeStats stats)
        {
            // Construire la cha�ne de caract�res pour l'affichage
            StringBuilder builder = new();

            bool enought = Inventory.Instance.HaveEnoughtCash(stats.upgradeCost);
            string color = enought ? "green" : "red";

            builder.AppendFormat(" (<color={0}>${1}</color>)", color, stats.upgradeCost);
            builder.AppendLine();

            for (int i = 0; i < stats.baseStats.Count; i++)
            {
                UpgradeNameData baseStats = stats.baseStats[i];
                UpgradeNameData upgradedStats = stats.previewStats[i];

                builder.AppendFormat(
                    "{0}:\t<color=red>{1}</color> > <color=green>{2}</color>",
                    baseStats.name,
                    baseStats.quantity,
                    upgradedStats.quantity
                );

                // Ajout d'un saut de ligne apr�s chaque stat, sauf pour la derni�re ligne
                if (i != stats.baseStats.Count - 1)
                    builder.AppendLine();
            }

            // Affecter le texte complet au TextMeshPro du StatContainer
            text.text = builder.ToString();

            // Acc�der au bouton dans le StatContainer et lui ajouter un listener pour appeler ShowSocleUpgradeForGBN
            button.onClick.RemoveAllListeners();

            string statName = stats.name;  // Capturer la variable locale
            button.onClick.AddListener(() => UpgradeStatItem(statName, stats.upgradeCost));
            if(!enought)
            {
                button.gameObject.SetActive(false);
            }
        }
        public void UpgradeStatItem(string statName, int cost)
        {
            UiBSGBN.Instance.ShowSocleUpgradeForGBN(statName);
            Inventory.Instance.GetCashUpgradeSocleCost(cost);

        }
    }
   
}