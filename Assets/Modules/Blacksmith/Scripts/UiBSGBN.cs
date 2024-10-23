using BlacksmithModule;
using ExtensionsModule;
using GemModule.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilsModule;

public class UiBSGBN : Singleton<UiBSGBN>
{
    [SerializeField] private GameObject ObjectStatUI;
    [SerializeField] private GameObject ObjectSocleUI;

    [SerializeField] private GameObject NumberObject;
    [SerializeField] private GameObject NumberPlusObject;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject casePrefab;
    [SerializeField] private GameObject ContainerForNumberSocle;
    [SerializeField] private GameObject column;
    [SerializeField] private GameObject BoutonExtentDomain;
    [SerializeField] private GameObject SoclePrefab;
    // Appeler la m�thode du Singleton Blacksmith
    public void ShowSocleUpgradeForGBN(string name)
    {
        componentGBN GBNcomponent = Blacksmith.Instance.ShowSocleUpgradeForGBN(name);

        ShowSocleUpgrade(GBNcomponent, name);
    }

    public void SwitchBetweenUi()
    {
        if (ObjectStatUI.activeSelf)
        {
            ObjectStatUI.SetActive(false);
            ObjectSocleUI.SetActive(true);
        }
        else if (ObjectSocleUI.activeSelf)
        {

            ObjectStatUI.SetActive(true);
            ObjectSocleUI.SetActive(false);
        }
    }

    public void ShowSocleUpgrade(componentGBN GBNcomponent, string name)
    {

        SwitchBetweenUi();
        CreateUi(GBNcomponent.GBNScript.SocleListe[0].PowerGemObjectScript.GridGemme, name);
        CreateSocleChoice(GBNcomponent, name);

    }
    public void GiveRefDomainButton(GemmeGrid GemmeGrid, string name)
    {
        BoutonExtentDomain.SetActive(true);

        Button upgradeButton = BoutonExtentDomain.GetComponentInChildren<Button>();
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => UpgradeStateBase(name));
            upgradeButton.onClick.AddListener(() => UpgradeSocle(GemmeGrid));
        }

    }
    public void UpgradeSocle(GemmeGrid GemmeGrid)
    {
        BoutonExtentDomain.SetActive(false);
        GemmeGrid.UpGrade();

        SwitchBetweenUi();
    }
    public void UpgradeStateBase(string name)
    {
        componentGBN GBNcomponent = Blacksmith.Instance.ShowSocleUpgradeForGBN(name);
        Equipment equipmentScript = GBNcomponent.GetComponentInParent<Equipment>();
        if (equipmentScript != null)
        {
            equipmentScript.UpgradeEquipment();
            Blacksmith.Instance.InterfaceUpgrade();
        }
    }
    public void CreateSocleChoice(componentGBN component, string name)
    {
        List<componentPowerGemObject> List = component.GBNScript.SocleListe;
        // Nettoie les objets enfants pr�c�dents dans NumberObject (si n�cessaire)
        foreach (Transform child in ContainerForNumberSocle.transform)
        {
            Destroy(child.gameObject);
        }

        // Parcours chaque �l�ment de la liste des PowerGemObjects
        for (int i = 0; i < List.Count; i++)
        {
            // Instancie un nouvel objet pour chaque socle (un bouton, par exemple)
            GameObject buttonInstance = Instantiate(NumberObject, ContainerForNumberSocle.transform);

            // Assigne le num�ro de l'index dans le texte du bouton
            TextMeshProUGUI textComponent = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                textComponent.text = (i + 1).ToString(); // Affiche l'index comme num�ro sur le bouton
            }

            // Ajoute un onClick listener au bouton instanci� pour appeler CreateUi avec la bonne grille
            Button buttonComponent = buttonInstance.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i; // Capture l'index dans une variable locale pour le callback
                buttonComponent.onClick.AddListener(() =>
                {
                    CreateUi(List[index].PowerGemObjectScript.GridGemme, name);
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
                    UpgradeStateBase(name);
                    AddSocle(component);
                    SwitchBetweenUi();
                });
            }
        }
    }
    public void AddSocle(componentGBN component)
    {
    //
        component.AddNewSocle(SoclePrefab);
    }
    public void CreateUi(GemmeGrid Grid, string name)
    {
        bool MoreGrid = false;
        if (Grid.CanUpgrade())
        {
            MoreGrid = true;
            GiveRefDomainButton(Grid, name);
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

        // Si MoreGrid est vrai, on ajoute une ligne et une colonne
        if (MoreGrid)
        {
            gridWidth += 1;
            gridHeight += 1;
        }

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

                // Appliquer une couleur noire si on est sur la dernière ligne ou la dernière colonne
                if (MoreGrid && (i == 0 || j == gridWidth - 1))
                {
                    // Appliquer une couleur noire à la case
                    RawImage caseImage = caseInstance.GetComponent<RawImage>();
                    if (caseImage != null)
                    {
                        caseImage.color = Color.black; // Appliquer la couleur noire
                    }
                }
            }
        }

        // Positionner la colonne à zéro (si nécessaire)
        // columnRect.anchoredPosition = Vector2.zero;
    }


    #region Upgrade Stats

    [Header("Upgrade Stats")]
    [SerializeField] private Transform upgradeStatsContent;  // Parent pour les objets instanci�s
    [SerializeField] private GameObject Horizontal;  // Pr�fab contenant un layout horizontal pour 2 StatContainer
    [SerializeField] private GameObject StatContainer;  // Pr�fab pour afficher les stats

    public void CreateUiGBNUpgrade(List<UpgradeStats> ListStat)
    {
        // Nettoyer le contenu pr�c�dent
        upgradeStatsContent.RemoveAll();

        // Compteur pour v�rifier si deux StatContainer doivent �tre plac�s dans le m�me Horizontal
        GameObject currentHorizontalInstance = null;
        int counter = 0;

        // Parcourir chaque �l�ment de la liste ListStat
        foreach (UpgradeStats upgradeStat in ListStat)
        {
            // Si le compteur est � 0 ou est un multiple de 2, on instancie un nouvel Horizontal
            if (counter % 2 == 0)
            {
                currentHorizontalInstance = Instantiate(Horizontal, upgradeStatsContent);
            }

            // Instancier un nouveau StatContainer et l'ajouter � l'Horizontal
            GameObject statConteneurInstance = Instantiate(StatContainer, currentHorizontalInstance.transform);

            if (statConteneurInstance.TryGetComponent(out UpgradeStatsItem item))
                item.SetItem(upgradeStat);

            // Incr�menter le compteur
            counter++;
        }
    }

    #endregion

    #region Singleton

    /// <inheritdoc/>
    protected override bool DestroyOnLoad => true;

    #endregion
}