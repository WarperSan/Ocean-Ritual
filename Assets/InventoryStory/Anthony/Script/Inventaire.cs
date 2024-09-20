using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using UnityEngine;
using static EnumGeneral;
public class Inventaire : MonoBehaviour
{
    private int nombreDePlaceInventaire =10;
   private List<ItemData> ItemList;
    bool InventaireOuvert = false;
    // Start is called before the first frame update
    void Start()
    {
    
    }
    public void InitiateListe()
    {
        ItemList = new List<ItemData>();
    }
    public void UpgradeInventory(int AddingStockage)
    {
        if (AddingStockage < 0)
        {
            Debug.LogWarning($"AddingStockage is less than 0   :   {AddingStockage}     : upgrade fail");
        }
        else
        {
            nombreDePlaceInventaire += AddingStockage;
        }
       
    }


    public (List<int>, List<int>) ItemExistantDansListe(ItemData item)
    {
        List<int> ListeIndexDisponible = new(); // Index où l'objet peut être placé (vide)
        List<int> ListeIndexItemIdentique = new(); // Index d'objets identiques (même nom/type)

        // Parcourt l'inventaire pour trouver des emplacements disponibles ou identiques
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i] == null)
            {
                // Ajoute les emplacements vides
                ListeIndexDisponible.Add(i);
            }
            else
            {
                // Si l'objet est un PoissonData
                if (item is PoissonData poisson && ItemList[i] is PoissonData poissonInList)
                {
                    // Vérifie que le nom est identique et que la quantité max n'est pas atteinte
                    if (poissonInList.nom == poisson.nom && poissonInList.quantiter < poissonInList.quantiterMax)
                    {
                        ListeIndexItemIdentique.Add(i);
                    }
                }
                // Si l'objet est une GemmeData, on ignore les emplacements contenant d'autres objets
                else if (item is GemmeData)
                {
                    // Ne fais rien pour les Gemmes, on ne veut que les emplacements vides
                }
            }
        }

        return (ListeIndexItemIdentique, ListeIndexDisponible);
    }

    private void AjoutEmplacement()
    {
        ItemList.Add(null);
    }
    public void NettoyerEmplacement()
    {
        // Traverse la liste à l'envers
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            // Si on trouve un élément non null, on garde un seul null à la fin et on arrête
            if (ItemList[i] != null)
            {
                // S'il y a déjà un null à la fin, on le garde, sinon on en ajoute un
                if (i == ItemList.Count - 1 || ItemList[^1] != null)
                {
                    AjoutEmplacement();// On garde un emplacement vide (null) à la fin
                }
                break;
            }

            // Supprime les éléments null s'ils sont inutiles à la fin de la liste
            ItemList.RemoveAt(i);
        }
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
    }
    public void DropItem(int index)
    {
        // Vérifie si l'index est dans les limites de la liste
        if (index >= 0 && index < ItemList.Count)
        {
            ItemList.RemoveAt(index);
           
        }
        else
        {
            // Avertit que l'index est invalide
            Debug.LogWarning($"Index invalide : {index}. Aucune suppression effectuée.");
        }
    }
    public ItemData GetItem(int index)
    {
     return  ItemList[index];

    }
    public void AddItem(ItemData item)
    {
        // Récupère les emplacements d'objets similaires et disponibles
        var (ListeIndexItemIdentique, ListeIndexDisponible) = ItemExistantDansListe(item);

        int quantiteRestante = item.quantiter;

        // 1. Ajoute aux emplacements d'objets identiques si possible
        foreach (int index in ListeIndexItemIdentique)
        {
            var itemInList = ItemList[index];
            int placeDisponible = itemInList.quantiterMax - itemInList.quantiter;

            if (quantiteRestante <= placeDisponible)
            {
                itemInList.quantiter += quantiteRestante;
                quantiteRestante = 0;
                break;
            }
            else
            {
                itemInList.quantiter = itemInList.quantiterMax;
                quantiteRestante -= placeDisponible;
            }
        }

        // 2. Si la quantité restante > 0, ajoute dans les emplacements disponibles
        foreach (int index in ListeIndexDisponible)
        {
            if (quantiteRestante == 0)
            {
                break;
            }

            ItemList[index] = item;
            if (quantiteRestante <= item.quantiterMax)
            {
                ItemList[index].quantiter = quantiteRestante;
                quantiteRestante = 0;
            }
            else
            {
                ItemList[index].quantiter = item.quantiterMax;
                quantiteRestante -= item.quantiterMax;
            }
        }

        // 3. Si encore de la quantité à placer, crée un nouvel emplacement
        int iterationLimit = 100; // Limite maximale d'itérations pour éviter les boucles infinies
        int iterationCount = 0;   // Compteur d'itérations

        while (quantiteRestante > 0)
        {
            AjoutEmplacement();
            int dernierIndex = ItemList.Count - 1;
            ItemList[dernierIndex] = item;

            if (quantiteRestante <= item.quantiterMax)
            {
                ItemList[dernierIndex].quantiter = quantiteRestante;
                quantiteRestante = 0;
            }
            else
            {
                ItemList[dernierIndex].quantiter = item.quantiterMax;
                quantiteRestante -= item.quantiterMax;
            }

            // Incrémentation du compteur d'itérations
            iterationCount++;

            // Si la limite d'itérations est atteinte, on sort de la boucle
            if (iterationCount >= iterationLimit)
            {
                Debug.LogError("Boucle infinie détectée, la boucle a été interrompue après " + iterationLimit + " itérations.");
                break; // Sort de la boucle pour éviter de bloquer le programme
            }
        }
    }


    public List<ItemData> GetInventaire()
    {
        return new List<ItemData>(ItemList); // Crée une nouvelle liste en copiant l'ancienne
    }


    public void UpdateItemListeUI()     
    {
      
    }
    public void TrierItemList(TypeOfSort SortType)
    {
        switch (SortType)
        {
            case TypeOfSort.Nom:
                TrierNom();
                Console.WriteLine("Tri par nom de poisson les gemme apres");
                break;

            case TypeOfSort.Type:
                TrierParType();
                Console.WriteLine("Tri par type poisson ou gemme");
                break;

            case TypeOfSort.Quantite:
                TrierQuantiter();
                Console.WriteLine("Tri par quantité de poisson");
                break;

            case TypeOfSort.Fusion:
                FusionAuto();
                Console.WriteLine(" faire la fusion");
                break;

            case TypeOfSort.Niveau:
                TrierNiveau();
                Console.WriteLine(" trie par niveau de gemme poisson après");
                break;

            default:
               
                Console.WriteLine("Type de tri non reconnu");
                break;
        }
    }
    public void TrierNom( )// trie  les poissonData de la liste par nom
    {

    }
    public void TrierParNiveau( ) // trie  les gemmeData de la liste par niveau
    {

    }
    public void TrierParType( ) // trie  la liste celon le type de chaque object dans la liste
    {

    }
    public void TrierQuantiter( ) // trie  la liste celon la quantité de chaque object dans la liste
    {

    }
    public void TrierNiveau( ) // trie  la liste celon la quantité de chaque object dans la liste
    {

    }
    public void FusionAuto( )//// a implémenter on touche pas
    {

    }
}
