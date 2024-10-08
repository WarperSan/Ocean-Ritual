using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShowForm : MonoBehaviour
{
    string casePath = "BlacksmithUi/Ui";
    [SerializeField] GameObject column;  // Le parent qui contient les lignes (row)
    Dictionary<string, GameObject> data;  // Dictionnaire contenant les prefabs chargés
    [SerializeField] GemData Gem;
    // Start is called before the first frame update
   
    public void GetResource()
    {
        // Récupérer les objets GameObject à partir du dictionnaire
        data = DictionaryGenerator.DictionaryGameObjectGenerator(casePath);
    }

    public void CreateUi(bool[,] grid)
    {
        if (data == null)
        {
            GetResource();
        }

       // ShowTab(grid);  // Affichage dans la console pour débogage, comme dans ShowTab

        if (!data.ContainsKey("Row") || !data.ContainsKey("CaseHover"))
        {
            Debug.LogError("Row ou CaseHover manquant dans le dictionnaire !");
            return;
        }

        GameObject rowPrefab = data["Row"];
        GameObject casePrefab = data["CaseHover"]; // Utilisation de CaseHover pour correspondre à ShowTab

        // Calcul du facteur d'échelle pour ajuster les dimensions des cases
        float maxColumnWidth = 200f;
        float maxColumnHeight = 200;

        float scalingFactorX = maxColumnWidth / grid.GetLength(1);
        float scalingFactorY = maxColumnHeight / grid.GetLength(0);
        float scalingFactor = Mathf.Min(scalingFactorX, scalingFactorY);

        // Suppression des éléments précédents
        foreach (Transform child in column.transform)
        {
            Destroy(child.gameObject);
        }

        RectTransform columnRect = column.GetComponent<RectTransform>();
        columnRect.sizeDelta = new Vector2(
            grid.GetLength(1) * scalingFactor,
            grid.GetLength(0) * scalingFactor
        );

        // Inversion de l'ordre des lignes
        for (int i = grid.GetLength(0) - 1; i >= 0; i--) // Inverse les lignes comme dans ShowTab
        {
            GameObject rowInstance = Instantiate(rowPrefab, column.transform);
            RectTransform rowRect = rowInstance.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(grid.GetLength(1) * scalingFactor, scalingFactor);
            rowRect.anchoredPosition = new Vector2(0, (grid.GetLength(0) - 1 - i) * scalingFactor); // Positionnement correct des lignes

            for (int j = 0; j < grid.GetLength(1); j++) // Parcours des colonnes
            {
                GameObject caseInstance = Instantiate(casePrefab, rowInstance.transform);
                RectTransform caseRect = caseInstance.GetComponent<RectTransform>();
                caseRect.sizeDelta = new Vector2(scalingFactor, scalingFactor);
                caseRect.anchoredPosition = new Vector2(j * scalingFactor, 0);

                // Récupérer le script Position et assigner X et Y
                Position posScript = caseInstance.GetComponent<Position>();
                if (posScript != null)
                {
                    posScript.SetPoition(j, i);  // Assigner les coordonnées de la case
                }

                // Récupérer le script pour changer l'état de la case
                ChangeColorBasedOnBool CasScript = caseInstance.GetComponent<ChangeColorBasedOnBool>();

                // Inverser l'indexation des lignes et colonnes dans grid
                if (grid[i, j])  // Utilisation correcte de i et j dans le tableau
                {
                    CasScript.SwapState(); // Si la case est true, on applique l'état (par exemple, [X])
                }
            }
        }

        columnRect.anchoredPosition = Vector2.zero;
    }

    public void ShowTab(bool[,] grid)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = grid.GetLength(0) - 1; i >= 0; i--) // Inverser l'ordre des lignes pour correspondre à l'affichage
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                sb.Append(grid[j, i] ? "[X]" : "[ ]"); // Utiliser [X] pour true et [ ] pour false
            }
            sb.AppendLine(); // Passer à la ligne suivante après chaque ligne du tableau
        }

        Debug.Log(sb.ToString()); // Afficher le résultat dans la console
    }
}
