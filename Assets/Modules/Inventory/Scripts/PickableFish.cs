using InteractModule;
using UnityEngine;

public class PickableFish : MonoBehaviour, IInteractable
{
    [SerializeField] string FishName;
    [SerializeField] int MaxQuantity;
    [SerializeField] int Quantity;
    [SerializeField] Sprite sprite;
    [SerializeField] string Description;
    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
       FishData poison =  new FishData();
        poison.name = FishName;
        poison.quantity = Quantity;
        poison.quantityMax = MaxQuantity;
        poison.sprite = sprite;
        poison.description= Description;
        Inventory.Instance.AddItem(poison);
    }
}
