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
         List<int>  ListeIndexDisponible = new();
        List<int> ListeIndexItemIdentique = new();




        return (ListeIndexItemIdentique,ListeIndexDisponible);

    }
    public void AjoutEmplacement()
    {
        ItemList.Add(null);
    }
    public void NettoyerEmplacement()
    {
     
    }
    public void DropItem(int index)
    {

    }
    public ItemData GetItem(int index)
    {
     return  ItemList[index];

    }
    public void AddItem(ItemData item)
    {

    }

    public List<ItemData> GetInventaire()
    {
        return new List<ItemData>(ItemList); // Crée une nouvelle liste en copiant l'ancienne
    }


    public void UpdateItemListeUI()     
    {
      
    }
    public void TrierItemList(TypeOfSort SortType,bool gemmeFirst)
    {
        switch (SortType)
        {
            case TypeOfSort.Nom:
                TrierNom(gemmeFirst);
                Console.WriteLine("Tri par nom");
                break;

            case TypeOfSort.Type:
                TrierParType(gemmeFirst);
                Console.WriteLine("Tri par type");
                break;

            case TypeOfSort.Quantite:
                TrierQuantiter(gemmeFirst);
                Console.WriteLine("Tri par quantité");
                break;

            case TypeOfSort.Fusion:
                FusionAuto(gemmeFirst);
                Console.WriteLine("Tri par fusion");
                break;

            default:
               
                Console.WriteLine("Type de tri non reconnu");
                break;
        }
    }
    public void TrierNom(bool gemmeFirst)// trie  les poissonData de la liste par nom
    {

    }
    public void TrierParNiveau(bool gemmeFirst) // trie  les gemmeData de la liste par niveau
    {

    }
    public void TrierParType(bool gemmeFirst) // trie  la liste celon le type de chaque object dans la liste
    {

    }
    public void TrierQuantiter(bool gemmeFirst) // trie  la liste celon la quantité de chaque object dans la liste
    {

    }
    public void FusionAuto(bool gemmeFirst)//// a implémenter on touche pas
    {

    }
}
