using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilsModule;

public class UiBSGBN : Singleton<UiBSGBN>
{
   
    [SerializeField] private GameObject content;  // Parent pour les objets instanciés
    [SerializeField] private GameObject Horizontal;  // Préfab contenant un layout horizontal pour 2 StatContainer
    [SerializeField] private GameObject StatContainer;  // Préfab pour afficher les stats
    [SerializeField] private GameObject ObjectStatUI;
    [SerializeField] private GameObject ObjectSocleUI;

    [SerializeField] private GameObject NumberObject;
    [SerializeField] private GameObject NumberPlusObject;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject casePrefab;
    [SerializeField] private GameObject ContainerForNumberSocle;
    [SerializeField] private GameObject column;
    [SerializeField] private GameObject BoutonExtentDomain;




    public void CreateUiGBNUpgrade(List<UpgradeStats> ListStat)
    {
        // Nettoyer le contenu précédent
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // Compteur pour vérifier si deux StatContainer doivent être placés dans le même Horizontal
        GameObject currentHorizontalInstance = null;
        int counter = 0;

        // Parcourir chaque élément de la liste ListStat
        foreach (UpgradeStats upgradeStat in ListStat)
        {
            // Si le compteur est à 0 ou est un multiple de 2, on instancie un nouvel Horizontal
            if (counter % 2 == 0)
            {
                currentHorizontalInstance = Instantiate(Horizontal, content.transform);
            }

            // Instancier un nouveau StatContainer et l'ajouter à l'Horizontal
            GameObject statConteneurInstance = Instantiate(StatContainer, currentHorizontalInstance.transform);

            // Accéder au premier composant TextMeshPro dans les enfants de StatContainer
            TextMeshProUGUI textComponent = statConteneurInstance.GetComponentInChildren<TextMeshProUGUI>();

            // Construire la chaîne de caractères pour l'affichage
            string statsText = "";
            statsText += upgradeStat.name + "  Cost: " + upgradeStat.upgradeCost + "\n";
            for (int i = 0; i < upgradeStat.baseStats.Count; i++)
            {
                string baseStatText = upgradeStat.baseStats[i].name + " : " + upgradeStat.baseStats[i].quantity;
                string previewStatText = " -> " + upgradeStat.previewStats[i].quantity;

                // Ajout d'un saut de ligne après chaque stat, sauf pour la dernière ligne
                statsText += baseStatText + previewStatText + "\n";
            }

            // Supprimer le dernier saut de ligne
            statsText = statsText.TrimEnd('\n');

            // Affecter le texte complet au TextMeshPro du StatContainer
            textComponent.text = statsText;

            // Accéder au bouton dans le StatContainer et lui ajouter un listener pour appeler ShowSocleUpgradeForGBN
            Button upgradeButton = statConteneurInstance.GetComponentInChildren<Button>();
            if (upgradeButton != null)
            {
                // Utilisation de l'upgradeStat.name dans le listener
                string statName = upgradeStat.name;  // Capturer la variable locale
                upgradeButton.onClick.AddListener(() => ShowSocleUpgradeForGBN(statName));
            }

            // Incrémenter le compteur
            counter++;
        }
    }

    // Appeler la méthode du Singleton Blacksmith
    public void ShowSocleUpgradeForGBN(string name)
    {
        componentGBN GBNcomponent =  Blacksmith.Instance.ShowSocleUpgradeForGBN(name);
      
        ShowSocleUpgrade(GBNcomponent);
    }

    public void SwitchBetweenUi()
    {
        if (ObjectStatUI.activeSelf)
        {
            ObjectStatUI.SetActive(false);
            ObjectSocleUI.SetActive(true);
        }
        else if(ObjectSocleUI.activeSelf)
        {

            ObjectStatUI.SetActive(true);
            ObjectSocleUI.SetActive(false);
        }
    }


    public void ShowSocleUpgrade (componentGBN GBNcomponent)
    {
     
        SwitchBetweenUi();
        CreateUi(GBNcomponent.GBNScript.SocleListe[0].PowerGemObjectScript.GridGemme);
        CreateSocleChoice(GBNcomponent.GBNScript.SocleListe);
       
    }
    public void GiveRefDomainButton(GemmeGrid GemmeGrid)
    {
        BoutonExtentDomain.SetActive(true);

            Button upgradeButton = BoutonExtentDomain.GetComponentInChildren<Button>();
            if (upgradeButton != null)
            {

                upgradeButton.onClick.AddListener(() => UpgradeSocle(GemmeGrid));
            }
        
        
      
    }
    public void UpgradeSocle(GemmeGrid GemmeGrid)
    {
        BoutonExtentDomain.SetActive(false);
        GemmeGrid.UpGrade();

        SwitchBetweenUi();
    }
    public void SwitchCase()
    {

    }
    public void CreateSocleChoice(List<componentPowerGemObject> List)
    {
        // Nettoie les objets enfants précédents dans NumberObject (si nécessaire)
        foreach (Transform child in ContainerForNumberSocle.transform)
        {
            Destroy(child.gameObject);
        }

        // Parcours chaque élément de la liste des PowerGemObjects
        for (int i = 0; i < List.Count; i++)
        {
            // Instancie un nouvel objet pour chaque socle (un bouton, par exemple)
            GameObject buttonInstance = Instantiate(NumberObject, ContainerForNumberSocle.transform);

            // Assigne le numéro de l'index dans le texte du bouton
            TextMeshProUGUI textComponent = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
           
            if (textComponent != null)
            {
                textComponent.text = (i + 1).ToString(); // Affiche l'index comme numéro sur le bouton
            }

            // Ajoute un onClick listener au bouton instancié pour appeler CreateUi avec la bonne grille
            Button buttonComponent = buttonInstance.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i; // Capture l'index dans une variable locale pour le callback
                buttonComponent.onClick.AddListener(() =>
                {
                    CreateUi(List[index].PowerGemObjectScript.GridGemme);
                });
            }
        }
        if (List.Count < 8)
        {
            GameObject buttonInstance = Instantiate(NumberPlusObject, ContainerForNumberSocle.transform);
            Button buttonComponent = buttonInstance.GetComponent<Button>();
            if (buttonComponent != null)
            {
                
                buttonComponent.onClick.AddListener(() =>
                {
                    AddSocle();
                });
            }
        }
    }
    public void AddSocle()
    {
        Debug.Log("allo");
    }
    public void CreateUi(GemmeGrid Grid)
    {
        if (Grid.CanUpgrade())
        {
            GiveRefDomainButton(Grid);
        }
        else
        {
            BoutonExtentDomain.SetActive(false);
        }

            // Définir les dimensions maximales de la grille
            float maxColumnWidth = 250;
        float maxColumnHeight = 250;

        // Utiliser Grid.width et Grid.height
        int gridWidth = Grid.width;
        int gridHeight = Grid.height;

        // Calculer le facteur de mise à l'échelle en fonction de la taille de la grille
        float scalingFactorX = maxColumnWidth / gridWidth;
        float scalingFactorY = maxColumnHeight / gridHeight;
        float scalingFactor = Mathf.Min(scalingFactorX, scalingFactorY);

        // Supprimer les enfants existants dans la colonne
        foreach (Transform child in column.transform)
        {
            Destroy(child.gameObject);
        }

        // Ajuster la taille de la colonne pour correspondre à la grille
        RectTransform columnRect = column.GetComponent<RectTransform>();
        columnRect.sizeDelta = new Vector2(
            gridWidth * scalingFactor,
            gridHeight * scalingFactor
        );

        // Parcourir les lignes de la grille
        for (int i = 0; i < gridHeight; i++)
        {
            // Instancier une nouvelle ligne dans la colonne
            GameObject rowInstance = Instantiate(rowPrefab, column.transform);
            RectTransform rowRect = rowInstance.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(gridWidth * scalingFactor, scalingFactor);
            rowRect.anchoredPosition = new Vector2(0, i * scalingFactor);

            // Parcourir les colonnes de chaque ligne
            for (int j = 0; j < gridWidth; j++)
            {
                // Instancier une nouvelle case dans la ligne
                GameObject caseInstance = Instantiate(casePrefab, rowInstance.transform);
                RectTransform caseRect = caseInstance.GetComponent<RectTransform>();
                caseRect.sizeDelta = new Vector2(scalingFactor, scalingFactor);
                caseRect.anchoredPosition = new Vector2(j * scalingFactor, 0);

                // Récupérer le script Position et assigner X et Y
                Position posScript = caseInstance.GetComponent<Position>();
                if (posScript != null)
                {
                    posScript.SetPoition(j, i);  // On attribue les coordonnées de la case
                }
            }
        }

        // Positionner la colonne à zéro
        //columnRect.anchoredPosition = Vector2.zero;
    }



}