using ExtensionsModule;
using System.Collections.Generic;
using System.Text;
using UIModule;
using UnityEngine;

namespace GemModule.UI
{
    public class ShowGemShape : UIComponent
    {
        private string casePath = "BlacksmithUi/Ui";
        private Dictionary<string, GameObject> data; // Dictionnaire contenant les prefabs charg�s

        [SerializeField]
        private GemData Gem;

        #region Colors

        [Header("Colors")]
        [SerializeField]
        [Tooltip("Color used when a case is filled")]
        private Color activeColor = Color.green;

        [SerializeField]
        [Tooltip("Color used when a case is empty, but was previously filled")]
        private Color wasActiveColor = Color.black;

        [SerializeField]
        [Tooltip("Color used when a case is empty")]
        private Color inactiveColor = Color.gray;

        #endregion

        public void GetResource() =>
            // R�cup�rer les objets GameObject � partir du dictionnaire
            data = DictionaryGenerator.DictionaryGameObjectGenerator(casePath);

        public void Show(GemData gem, GemData original)
        {
            bool[,] grid = gem.Shape.GetForme();
            bool[,] originalGrid = original.Shape.GetForme();

            if (data == null)
                GetResource();

            // ShowTab(grid);  // Affichage dans la console pour d�bogage, comme dans ShowTab

            if (!data.ContainsKey("Row") || !data.ContainsKey("CaseBool"))
            {
                Debug.LogError("Row ou CaseHover manquant dans le dictionnaire !");
                return;
            }

            GameObject rowPrefab = data["Row"];
            GameObject casePrefab = data["CaseBool"]; // Utilisation de CaseHover pour correspondre � ShowTab

            // Calcul du facteur d'�chelle pour ajuster les dimensions des cases
            float scalingFactor = Mathf.Min(
                Rect.sizeDelta.x / grid.GetLength(1),
                Rect.sizeDelta.y / grid.GetLength(0)
            );

            // Suppression des �l�ments pr�c�dents
            Clear();

            // Inversion de l'ordre des lignes
            for (int i = grid.GetLength(0) - 1; i >= 0; i--) // Inverse les lignes comme dans ShowTab
            {
                GameObject rowInstance = Instantiate(rowPrefab, transform);
                RectTransform rowRect = rowInstance.GetComponent<RectTransform>();
                rowRect.sizeDelta = new Vector2(grid.GetLength(1) * scalingFactor, scalingFactor);
                rowRect.anchoredPosition = new Vector2(0, (grid.GetLength(0) - 1 - i) * scalingFactor); // Positionnement correct des lignes

                for (int j = 0; j < grid.GetLength(1); j++) // Parcours des colonnes
                {
                    GameObject caseInstance = Instantiate(casePrefab, rowInstance.transform);
                    RectTransform caseRect = caseInstance.GetComponent<RectTransform>();
                    caseRect.sizeDelta = new Vector2(scalingFactor, scalingFactor);
                    caseRect.anchoredPosition = new Vector2(j * scalingFactor, 0);

                    // R�cup�rer le script Position et assigner X et Y
                    if (caseInstance.TryGetComponent(out Position posScript))
                        posScript.SetPoition(j, i); // Assigner les coordonn�es de la case

                    // R�cup�rer le script pour changer l'�tat de la case
                    ChangeColorBasedOnBool CasScript = caseInstance.GetComponent<ChangeColorBasedOnBool>();

                    // Inverser l'indexation des lignes et colonnes dans grid
                    bool isActive = grid[i, j];
                    CasScript.SetState(isActive, activeColor, originalGrid[i, j] ? wasActiveColor : inactiveColor);
                }
            }
        }

        public void Clear() =>
            // Suppression des �l�ments pr�c�dents
            transform.RemoveAll();

        #region Editor

        private void ShowTab(bool[,] grid)
        {
            #if UNITY_EDITOR
            var sb = new StringBuilder();

            for (int i = grid.GetLength(0) - 1; i >= 0; i--) // Inverser l'ordre des lignes pour correspondre � l'affichage
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                    sb.Append(grid[j, i] ? "[X]" : "[   ]"); // Utiliser [X] pour true et [ ] pour false

                sb.AppendLine(); // Passer � la ligne suivante apr�s chaque ligne du tableau
            }

            Debug.Log(sb.ToString()); // Afficher le r�sultat dans la console
            #endif
        }

        #endregion
    }
}