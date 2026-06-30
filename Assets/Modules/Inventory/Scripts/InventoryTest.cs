using UnityEngine;
using static EnumGeneral;

public class InventoryTest : MonoBehaviour
{
    [SerializeField]
    private Inventory InventoryPlayer;

    [SerializeField]
    private bool AddFish;

    [SerializeField]
    private bool AddGemme;

    [SerializeField]
    private bool clearInventaire;

    [SerializeField]
    private bool Sort;

    [SerializeField]
    private bool SwapPlace;

    [SerializeField]
    private bool drop;

    [SerializeField]
    private int index1;

    [SerializeField]
    private int index2;

    [SerializeField]
    private int indexDrop;

    [SerializeField]
    private FishData Fish;

    [SerializeField]
    private GemData gem;

    [SerializeField]
    private TypeOfSort TypeSort;

    // Start is called before the first frame update
    private void Start() => Inventory.Instance.InitiateList();

    // Update is called once per frame
    private void Update()
    {
        if (AddFish)
        {
            AddFish = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de FishData
            var NewFish = new FishData(Fish.fish, Fish.quantity);

            Inventory.Instance.AddItem(NewFish); // Utilise la nouvelle instance
        }

        if (AddGemme)
        {
            AddGemme = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de GemData
            var NewGemme = new GemData
            {
                GemColorsName = gem.GemColorsName,
                LVL = gem.LVL,
                quantity = 1,
                quantityMax = 1,
                sprite = gem.sprite,
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