using System.Collections;
using System.Collections.Generic;
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
       

        if (!data.ContainsKey("Row") || !data.ContainsKey("CaseBool"))
        {
            Debug.LogError("Row ou CaseBool manquant dans le dictionnaire !");
            return;
        }

        GameObject rowPrefab = data["Row"];
        GameObject casePrefab = data["CaseHover"];

        float maxColumnWidth = 200f;
        float maxColumnHeight = 200;

        float scalingFactorX = maxColumnWidth / grid.GetLength(1);
        float scalingFactorY = maxColumnHeight / grid.GetLength(0);
        float scalingFactor = Mathf.Min(scalingFactorX, scalingFactorY);

        foreach (Transform child in column.transform)
        {
            Destroy(child.gameObject);
        }

        RectTransform columnRect = column.GetComponent<RectTransform>();
        columnRect.sizeDelta = new Vector2(
            grid.GetLength(1) * scalingFactor,
            grid.GetLength(0) * scalingFactor
        );

        for (int i = 0; i < grid.GetLength(0); i++) // Parcours normal des lignes
        {
            GameObject rowInstance = Instantiate(rowPrefab, column.transform);
            RectTransform rowRect = rowInstance.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(grid.GetLength(1) * scalingFactor, scalingFactor);
            rowRect.anchoredPosition = new Vector2(0, i * scalingFactor);

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
                    posScript.SetPoition(j, i);  // Ici, on attribue les coordonnées de la case
                }

                // Récupérer le script pour changer l'état de la case
                ChangeColorBasedOnBool CasScript = caseInstance.GetComponent<ChangeColorBasedOnBool>();

                if (grid[j, grid.GetLength(0) - 1 - i])
                {
                    CasScript.SwapState();
                }
            }
        }

        columnRect.anchoredPosition = Vector2.zero;
    }
}
