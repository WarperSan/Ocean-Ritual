using FishingModule;
using InteractModule;
using UnityEngine;

public class PickableFish : MonoBehaviour, IInteractable
{
    public FishSO fishToAdd;
    [SerializeField] int MaxQuantity;
    [SerializeField] int Quantity;
    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
        var poisson = new FishData(fishToAdd, Quantity)
        {
            quantity = Quantity,
            quantityMax = MaxQuantity,
            fish = fishToAdd
        };
        Inventory.Instance.AddItem(poisson);
    }
}
