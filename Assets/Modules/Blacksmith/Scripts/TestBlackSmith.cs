using GemModule.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilsModule;

namespace BlacksmithModule
{
    public class TestBlackSmith : Singleton<TestBlackSmith>
    {
        [SerializeField] Gemcomponent Gem;
        [SerializeField] GemData TemporaryGemData;
        [SerializeField] bool show;
        [SerializeField] GemData GemData;
        [SerializeField] public bool canAddNewCase;
        [SerializeField] private ShowGemShape showGemShape;
        public bool CasseOnlytrue;
        private int Cost;
        public void ConvertGemme()
        {
            GemData = GemHelper.ConvertGemToGemData(Gem.GemScript);
            TemporaryGemData = GemHelper.ConvertGemToGemData(Gem.GemScript);
        }

        public void SetData(GemData gem)
        {
            // Clear UI
            if (gem == null)
            {
                GemData = null;
                TemporaryGemData = null;
                showGemShape.Clear();
                this.RestoreOriginalTexts();
                return;
            }

            GemData = gem;
            TemporaryGemData = new(gem);

            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (show)
            {
                show = !show;
                ConvertGemme();
                UpdateUI();
            }
        }

        public void ChangeValueGemme(int x, int y, bool boolean)
        {
            GemHelper.ModifiedList(ref TemporaryGemData.Shape.flatForme, TemporaryGemData.Shape.height - x - 1, y, boolean, TemporaryGemData.Shape.height);
            UpdateUI();
        }

        public void UpdateUI()
        {
            showGemShape.Show(TemporaryGemData, GemData);

            // Initialisation des textes originaux seulement si ce n'est pas encore fait
            if (originalTexts == null)
            {
                originalTexts = new string[3]; // Allocation du tableau
                                               // Sauvegarde des textes originaux avant modification
                originalTexts[0] = cost.text;            // Texte original du co�t
                originalTexts[1] = MissingCase.text;     // Texte original des cases manquantes
                originalTexts[2] = ModifiedCase.text;    // Texte original des cases modifi�es
            }

            // Obtenir les informations � partir des listes de formes
            (int totalCost, int modifiedSpot, int missingSpot, bool CasseOnlytrue) = GemHelper.GetInformationAboutForm(
                GemData.Shape.flatForme,
                TemporaryGemData.Shape.flatForme,
                GemData.LVL
            );
            this.CasseOnlytrue = CasseOnlytrue;

            // Si aucune case n'est manquante, on d�sactive la possibilit� d'ajouter de nouvelles cases
            canAddNewCase = missingSpot > 0;
            Cost = totalCost;
            this.SetTexts(totalCost, modifiedSpot, missingSpot);
        }

        #region Texts

        [Header("Texts")]
        [SerializeField] TextMeshProUGUI cost;
        [SerializeField] TextMeshProUGUI MissingCase;
        [SerializeField] TextMeshProUGUI ModifiedCase;

        // Tableaux pour stocker les textes originaux
        private string[] originalTexts;  // 0 = cost, 1 = MissingCase, 2 = ModifiedCase

        private void SetTexts(int totalCost, int missingCount, int modifiedCount)
        {
            // Mise � jour des textes avec les nouvelles valeurs, tout en sauvegardant les textes originaux
            cost.text = Inventory.Instance.HaveEnoughtCash(Cost)
                ? originalTexts[0] + " " + totalCost.ToString()
                : originalTexts[0] + " <color=#FF0000>" + totalCost.ToString() + "</color>";
            // Ajoute la valeur originale avec la nouvelle valeur du co�t
            MissingCase.text = originalTexts[1] + " " + missingCount.ToString();    // Ajoute la valeur originale avec le nombre de cases manquantes
            ModifiedCase.text = originalTexts[2] + " " + modifiedCount.ToString();  // Ajoute        nale avec le nombre de cases modifi�es

            // Rebuild layout
            LayoutRebuilder.ForceRebuildLayoutImmediate(cost.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(MissingCase.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(ModifiedCase.rectTransform);
        }
        
        private void RestoreOriginalTexts()
        {
            if (originalTexts == null)
                return;
                
            // Restaurer les textes originaux si n�cessaire
            cost.text = originalTexts[0];
            MissingCase.text = originalTexts[1];
            ModifiedCase.text = originalTexts[2];
        }

        public void ConfirmChoice()
        {
            if (Inventory.Instance.HaveEnoughtCash(Cost))// potentielement mettre que faut que le joueur a assé d'Argent
            {
                Inventory.Instance.RemoveCash(Cost);
                GemData.Shape = new(TemporaryGemData.Shape.flatForme, TemporaryGemData.Shape.width, TemporaryGemData.Shape.height);
                SetData(GemData);
            }
        }
        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}