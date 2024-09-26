using InteractModule;
using UnityEngine;

public class PickableFish : MonoBehaviour, IInteractable
{
    [SerializeField] string FishName;
    [SerializeField] int MaxQuantity;
    [SerializeField] int Quantity;
    [SerializeField] Sprite sprite;

    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
       FishData poison =  new FishData();
        poison.name = FishName;
        poison.quantity = Quantity;
        poison.quantityMax = MaxQuantity;
        poison.sprite = sprite;
        Inventory.Instance.AddItem(poison);
    }
}
