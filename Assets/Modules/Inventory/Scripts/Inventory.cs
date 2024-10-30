using System;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;
using System.Linq;
using FishingModule;
using UtilsModule;
using TMPro;
using BlacksmithModule;

[System.Serializable]
public class Inventory : Singleton<Inventory>
{
    public int NbSlotInventory = 12 * 2; // 12 items per page for 2 pages

    [SerializeField] public List<ItemData> ItemList = new();
    [SerializeField] private int Cash = 0;
    [SerializeField] List<FishData> poissons;
    [SerializeField] List<GemData> gemmes;

    [SerializeField] int CashToUpGradeSocle;
    [SerializeField] TextMeshProUGUI textCostInventory;
    [SerializeField] TextMeshProUGUI textCostInventoryBlacksmith;
    //bool InventaireOuvert = false;

    // Start is called before the first frame update
    void Start()
    {
        //AddFishTest();
        //AddTestGemme();
        //UpdateListeComplementary();
        InitiateList();
    }

    void AddTestGemme()
    {
        string[] gemColors = { "gems Rouge", "gems Bleue", "gems Verte" };

        for (int i = 0; i < 6; i++)
        {
            GemData gem = new GemData
            {
                GemColorsName = gemColors[i % gemColors.Length], // Alternance des couleurs Rouge, Bleue, Verte
                LVL = i + 1 // Niveau croissant
            };
            ItemList.Add(gem);
        }
    }

    public void UpdateListeComplementary()
    {
        poissons = ItemList.OfType<FishData>().ToList();
        gemmes = ItemList.OfType<GemData>().ToList();
    }

    public void InitiateList()
    {
        ItemList = new List<ItemData>();

        //// Tant que l'inventaire n'est pas plein, ajoute des null
        while (ItemList.Count < NbSlotInventory)
        {
            AddSlot();
        }
        UpdateCashCost();
    }

    public void UpgradeInventory(int AddingStockage)
    {
        if (AddingStockage < 0)
        {
            Debug.LogWarning($"AddingStockage is less than 0   :   {AddingStockage}     : upgrade fail");
        }
        else
        {
            NbSlotInventory += AddingStockage;
        }
    }

    public (List<int>, List<int>) ItemInListExistingOrNull(ItemData item)
    {
        List<int> ListIndexAvailable = new(); // Index où l'objet peut être placé (vide)
        List<int> ListIndexItemSame = new(); // Index d'objets identiques (même name/type)

        // Parcourt l'inventaire pour trouver des emplacements disponibles ou identiques
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i] == null)
            {
                // Ajoute les emplacements vides
                ListIndexAvailable.Add(i);
            }
            else
            {
                // Si l'objet est un FishData
                if (item is FishData poisson && ItemList[i] is FishData poissonInList)
                {
                    // Vérifie que le name est identique et que la quantité max n'est pas atteinte
                    if (poissonInList.fish == poisson.fish && poissonInList.quantity < poissonInList.quantityMax)
                    {
                        ListIndexItemSame.Add(i);
                    }
                }
                // Si l'objet est une GemData, on ignore les emplacements contenant d'autres objets
                else if (item is GemData)
                {
                    // Ne fais rien pour les Gemmes, on ne veut que les emplacements vides
                }
            }
        }

        return (ListIndexItemSame, ListIndexAvailable);
    }

    private void AddSlot()
    {
        ItemList.Add(null);
    }

    public void CleanSlot()
    {
        // Supprimer les éléments null au début
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i] == null)
            {
                ItemList.RemoveAt(i);
                i--; // Ajuster l'index après suppression
            }
            else
            {
                break; // Arrêter la suppression dès qu'on trouve un élément non null
            }
        }

        // Supprimer les éléments null à la fin
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            if (ItemList[i] == null)
            {
                ItemList.RemoveAt(i);
            }
            else
            {
                break; // Arrêter la suppression dès qu'on trouve un élément non null
            }
        }

        // Ajouter un emplacement vide à la fin si nécessaire
        if (ItemList.Count == 0 || ItemList[^1] != null)
        {
            AddSlot(); // Ajoute un emplacement vide
        }

        UpdateListeComplementary();
    }



    public void SwapPlace(int index1, int index2)
    {
        // Vérifie que les index sont valides
        if (index1 < 0 || index1 >= ItemList.Count || index2 < 0 || index2 >= ItemList.Count)
        {
            Debug.LogError("Les index fournis ne sont pas valides.");
            return;
        }

        // Effectue l'échange des éléments
        ItemData temp = ItemList[index1];
        ItemList[index1] = ItemList[index2];
        ItemList[index2] = temp;
        UpdateListeComplementary();
    }

    public void DropItem(int index)
    {
        // Vérifie si l'index est dans les limites de la liste
        if (index >= 0 && index < ItemList.Count)
        {
            ItemList[index] = null;
        }
        else
        {
            // Avertit que l'index est invalide
            Debug.LogWarning($"Index invalide : {index}. Aucune suppression effectuée.");
        }
        UpdateListeComplementary();

    }

    public ItemData GetItem(int index)
    {
        return ItemList[index];
    }
    public void AddItem(ItemData item, int slot)
    {
        if (ItemList[slot] == null)
        {
            ItemList[slot] = item;

        }
        else
        {
            AddItem(item);
        }

    }
    public void AddItem(ItemData item)
    {
        // Récupère les emplacements d'objets similaires et disponibles
        var (ListIndexAvailable, ListIndexVoid) = ItemInListExistingOrNull(item);

        //// Calcul du nombre d'éléments déjà présents dans l'inventaire
        //int nombreItemsTotal = ItemList.Sum(item => item?.quantity ?? 0);

        //// Vérifier si l'inventaire est plein
        //if (nombreItemsTotal >= NbSlotInventory)
        //{
        //    // Inventory déjà plein, on ne fait rien
        //    return;
        //}

        int quantityremaining = item.quantity;

        // 1. Ajoute aux emplacements d'objets identiques si possible
        foreach (int index in ListIndexAvailable)
        {
            var itemInList = ItemList[index];
            int placeAvailable = itemInList.quantityMax - itemInList.quantity;

            if (quantityremaining <= placeAvailable)
            {
                itemInList.quantity += quantityremaining;
                quantityremaining = 0;
                break;
            }
            else
            {
                itemInList.quantity = itemInList.quantityMax;
                quantityremaining -= placeAvailable;
            }
        }

        // 2. Si la quantité restante > 0, ajoute dans les emplacements disponibles
        foreach (int index in ListIndexVoid)
        {
            if (quantityremaining == 0)
            {
                break;
            }

            ItemList[index] = item;
            if (quantityremaining <= item.quantityMax)
            {
                ItemList[index].quantity = quantityremaining;
                quantityremaining = 0;
            }
            else
            {
                ItemList[index].quantity = item.quantityMax;
                quantityremaining -= item.quantityMax;
            }
        }

        //// 3. Si encore de la quantité à placer, crée un nouvel emplacement
        //int iterationLimit = 100; // Limite maximale d'itérations pour éviter les boucles infinies
        //int iterationCount = 0;   // Compteur d'itérations

        //while (quantityremaining > 0)
        //{
        //    Debug.Log("passe dans le while");
        //    AddSlot();
        //    int dernierIndex = ItemList.Count - 1;
        //    ItemList[dernierIndex] = item;

        //    if (quantityremaining <= item.quantityMax)
        //    {
        //        ItemList[dernierIndex].quantity = quantityremaining;
        //        quantityremaining = 0;
        //    }
        //    else
        //    {
        //        ItemList[dernierIndex].quantity = item.quantityMax;
        //        quantityremaining -= item.quantityMax;
        //    }

        //    // Incrémentation du compteur d'itérations
        //    iterationCount++;

        //    // Si la limite d'itérations est atteinte, on sort de la boucle
        //    if (iterationCount >= iterationLimit)
        //    {
        //        Debug.LogError("Boucle infinie détectée, la boucle a été interrompue après " + iterationLimit + " itérations.");
        //        break; // Sort de la boucle pour éviter de bloquer le programme
        //    }
        //}

        UpdateListeComplementary();
    }

    public List<ItemData> GetInventaire()
    {
        Debug.Log($"get inventaire {ItemList.Count}");
        return new List<ItemData>(ItemList); // Crée une nouvelle liste en copiant l'ancienne
    }

    public void UpdateItemListeUI(InventoryUI ui) => ui.UpdateUI(ItemList);

    public void SortItem(TypeOfSort SortType)
    {
        switch (SortType)
        {
            case TypeOfSort.Nom:
                SortName();
                Console.WriteLine("Tri par name de fish les gems apres");
                break;

            case TypeOfSort.Type:
                this.SortType();
                Console.WriteLine("Tri par type fish ou gems");
                break;

            case TypeOfSort.Quantite:
                SortQuantity();
                Console.WriteLine("Tri par quantité de fish");
                break;

            case TypeOfSort.Fusion:
                AutoMerge();
                Console.WriteLine(" faire la fusion");
                break;

            case TypeOfSort.Niveau:
                SortLVL();
                Console.WriteLine(" trie par niveau de gems fish après");
                break;

            default:

                Console.WriteLine("Type de tri non reconnu");
                break;
        }
        UpdateListeComplementary();
    }

    public void SortName()
    {
        // Séparer les fish et les gems
        List<FishData> fish = ItemList.OfType<FishData>().ToList();
        List<GemData> gem = ItemList.OfType<GemData>().ToList();

        // Trier les fish par name (supposant que FishData a un champ 'name')
        fish = fish.OrderBy(p => p.fish.DisplayName).ToList();

        // Réorganiser l'inventaire avec fish d'abord, puis les gems
        ItemList = new List<ItemData>();
        ItemList.AddRange(fish);
        ItemList.AddRange(gem);

        Debug.Log("Liste triée par name de fish.");
    }



    public void SortType()
    {
        // Séparer les fish et les gems
        List<FishData> fish = ItemList.OfType<FishData>().ToList();
        List<GemData> gem = ItemList.OfType<GemData>().ToList();

        // Réorganiser l'inventaire avec les fish d'abord, puis les gems
        ItemList = new List<ItemData>();
        ItemList.AddRange(fish);
        ItemList.AddRange(gem);

        Debug.Log("Liste triée par type (fish puis gems).");
    }

    public void SortQuantity()
    {
        // Trier les objets par quantité (qu'ils soient des fish ou des gems)
        ItemList = ItemList.OrderByDescending(item => item.quantity).ToList();

        Debug.Log("Liste triée par quantité.");
    }

    public void SortLVL()
    {
        // Séparer les fish et les gems
        List<GemData> gem = ItemList.OfType<GemData>().ToList();
        List<FishData> fish = ItemList.OfType<FishData>().ToList();

        // Trier uniquement les gems par niveau
        gem = gem.OrderByDescending(g => g.LVL).ToList();

        // Réorganiser l'inventaire avec les gems d'abord, puis les fish
        ItemList = new List<ItemData>();
        ItemList.AddRange(gem);
        ItemList.AddRange(fish);

        Debug.Log("Liste triée par niveau de gems.");
    }

    public void AutoMerge()
    {
        // On garde les gems intactes
        List<ItemData> gem = ItemList.Where(item => item is GemData).ToList();

        // On filtre les fish avec quantité > 0
        List<FishData> fish = ItemList.OfType<FishData>()
                                             .Where(poisson => poisson.quantity > 0)
                                             .ToList();

        // On crée un dictionnaire pour compter et fusionner les fish par name
        Dictionary<FishSO, (int quantiteTotale, int quantiterMax)> fusionPoissons = new Dictionary<FishSO, (int, int)>();

        foreach (var poisson in fish)
        {
            if (!fusionPoissons.ContainsKey(poisson.fish))
            {
                // On stocke la quantité totale et le quantityMax
                fusionPoissons[poisson.fish] = (poisson.quantity, poisson.quantityMax);
            }
            else
            {
                // On ajoute la quantité au total déjà enregistré
                fusionPoissons[poisson.fish] = (fusionPoissons[poisson.fish].quantiteTotale + poisson.quantity, poisson.quantityMax);
            }
        }

        // Nouvelle liste des fish fusionnés
        List<FishData> poissonsFusionnes = new List<FishData>();

        foreach (var entry in fusionPoissons)
        {
            int quantiteTotale = entry.Value.quantiteTotale;
            int quantiterMax = entry.Value.quantiterMax;

            // On répartit les fish en respectant la quantité maximale propre à chaque fish
            while (quantiteTotale > 0)
            {
                var nouveauPoisson = new FishData(
                    entry.Key,
                    Math.Min(quantiterMax, quantiteTotale) // Utilisation de la valeur quantityMax propre à ce fish
                );
                poissonsFusionnes.Add(nouveauPoisson);
                quantiteTotale -= nouveauPoisson.quantity;
            }
        }

        // Maintenant on replace tout dans ItemList
        // En gardant d'abord les fish fusionnés, puis les gems
        ItemList = poissonsFusionnes.Cast<ItemData>()
                                    .Concat(gem)
                                    .ToList();
    }

    #region Singleton

    /// <inheritdoc/>
    protected override bool DestroyOnLoad => true;

    #endregion

    public void UpdateCashCost()
    {
        if (textCostInventory)
            textCostInventory.text = Cash.ToString();

        if (textCostInventoryBlacksmith)
            textCostInventoryBlacksmith.text = Cash.ToString();
        Blacksmith.Instance.InterfaceUpgrade();
    }
    public void AddCash(int AddingCash = 0)
    {


        Cash += AddingCash;
        UpdateCashCost();
    }

    public void RemoveCash(int RemovingCash = 0)
    {
        if (Cash - RemovingCash >= 0)
        {
            Cash -= RemovingCash;
            UpdateCashCost();
        }
    }

    public void GetCashUpgradeSocleCost(int cash = 0)
    {
        CashToUpGradeSocle = cash;
    }
    public void RemoveCashSocleCost()
    {
        RemoveCash(CashToUpGradeSocle);
    }
    public bool HaveEnoughtCash(int CashNeed = 0)
    {

        return Cash >= CashNeed;
    }

    public int NumberOfCashFromSellingFish()
    {
        int CashFromSelling = 0;
        List<FishData> fish = ItemList.OfType<FishData>()
                                           .Where(poisson => poisson.quantity > 0)
                                           .ToList();
        foreach (FishData fishData in fish)
        {

            CashFromSelling += fishData.fish.GetPrice() * fishData.quantity;

        }


        return CashFromSelling;
    }

    public void sellingAllFish()
    {
        int totalCash = NumberOfCashFromSellingFish();


        ItemList.RemoveAll(item => item is FishData fishData);
        // Ajout du total obtenu à la variable Cash
        AddCash(totalCash);
    }
}
