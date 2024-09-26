using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class InventoryTest : MonoBehaviour
{
    [SerializeField] Inventory InventoryPlayer;

    [SerializeField] bool AddFish = false;
    [SerializeField] bool AddGemme = false;
    [SerializeField] bool clearInventaire = false;
    [SerializeField] bool Sort = false;
    [SerializeField] bool SwapPlace = false;
    [SerializeField] bool drop = false;
    [SerializeField] int index1;
    [SerializeField] int index2;
    [SerializeField] int indexDrop;
    [SerializeField] FishData Fish;
    [SerializeField] GemmeData gemme;
    [SerializeField] TypeOfSort TypeSort;

    // Start is called before the first frame update
    void Start()
    {
        InventoryPlayer.InitiateList();
    }

    // Update is called once per frame
    void Update()
    {
        if (AddFish)
        {
            AddFish = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de FishData
            FishData NewFish = new FishData()
            {
                name = Fish.name,
                quantity = Fish.quantity,
                quantityMax = Fish.quantityMax,
                sprite = Fish.sprite,
            };

            Inventory.Instance.AddItem(NewFish); // Utilise la nouvelle instance
        }

        if (AddGemme)
        {
            AddGemme = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de GemmeData
            GemmeData NewGemme = new GemmeData()
            {
                GemmeColorsName = gemme.GemmeColorsName,
                LVL = gemme.LVL,
                quantity = 1,
                quantityMax = 1,
                sprite = gemme.sprite,
            };

            Inventory.Instance.AddItem(NewGemme); // Utilise la nouvelle instance
        }
        if (clearInventaire)
        {
            clearInventaire = false;
            InventoryPlayer.CleanSlot();
        }
        if (Sort)
        {
            Sort = false;
            InventoryPlayer.SortItem(TypeSort);
        }
        if (SwapPlace)
        {
            SwapPlace = false;
            InventoryPlayer.SwapPlace(index1, index2);
        }
        if (drop)
        {
            drop = false;
            InventoryPlayer.DropItem(indexDrop);
        }
    }
}