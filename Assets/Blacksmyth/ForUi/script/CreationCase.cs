using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class CreationCase : MonoBehaviour
{
    string casePath = "BlacksmithUi/Ui";
    [SerializeField] GameObject column;  // Le parent qui contient les lignes (row)
    Dictionary<string, GameObject> data;  // Dictionnaire contenant les prefabs chargés
    [SerializeField] Gemcomponent Gem;
    private void Start()
    {
        CreateUi(Gem.GemScript.form.GetForme());
    }

    // Méthode pour récupérer les ressources via le dictionnaire
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
        ShowTab(grid);

        // Vérifier que le dictionnaire contient bien les prefabs requis
        if (!data.ContainsKey("Row") || !data.ContainsKey("CaseBool"))
        {
            Debug.LogError("Row ou CaseBool manquant dans le dictionnaire !");
            return;
        }

        // Récupérer les prefabs depuis le dictionnaire
        GameObject rowPrefab = data["Row"];
        GameObject casePrefab = data["CaseBool"];

        // Calculer l'échelle dynamique en fonction de la taille du tableau
        float scalingFactor = 50f; // Taille fixe pour chaque case, ajustez si nécessaire

        // Effacer les lignes précédentes
        foreach (Transform child in column.transform)
        {
            Destroy(child.gameObject);
        }

        // Ajuster la taille de la colonne
        RectTransform columnRect = column.GetComponent<RectTransform>();
        columnRect.sizeDelta = new Vector2(
            grid.GetLength(1) * scalingFactor,
            grid.GetLength(0) * scalingFactor
        );

        // Parcourir les lignes en bas en haut
        for (int i = 0; i < grid.GetLength(0); i++) // Parcours normal des lignes
        {
            // Instancier une ligne (row) et ajuster sa taille
            GameObject rowInstance = Instantiate(rowPrefab, column.transform);
            RectTransform rowRect = rowInstance.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(grid.GetLength(1) * scalingFactor, scalingFactor);
            rowRect.anchoredPosition = new Vector2(0, i * scalingFactor); // Positionner la ligne

            // Parcourir les colonnes
            for (int j = 0; j < grid.GetLength(1); j++) // Parcours normal des colonnes
            {
                // Instancier une case et ajuster sa taille
                GameObject caseInstance = Instantiate(casePrefab, rowInstance.transform);
                RectTransform caseRect = caseInstance.GetComponent<RectTransform>();
                caseRect.sizeDelta = new Vector2(scalingFactor, scalingFactor);
                caseRect.anchoredPosition = new Vector2(j * scalingFactor, 0); // Positionner la case

                // Récupérer le script pour changer l'état de la case
                ChangeColorBasedOnBool CasScript = caseInstance.GetComponent<ChangeColorBasedOnBool>();

                // Changer la couleur ou l'état de la case selon la valeur dans le tableau
                if (grid[j, grid.GetLength(0) - 1 - i])  // Utiliser j pour la colonne et (height - 1 - i) pour l'affichage
                {
                    CasScript.SwapState();  // Appeler la méthode pour modifier l'état ou la couleur
                }
            }
        }

        // Centrer le tableau dans le parent (optionnel si tout est déjà positionné correctement)
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
