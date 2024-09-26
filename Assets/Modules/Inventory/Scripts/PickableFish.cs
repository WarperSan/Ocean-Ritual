using InteractModule;
using UnityEngine;

public class PickableFish : MonoBehaviour, IInteractable
{
    [SerializeField] string FishName;
    [SerializeField] int MaxQUantiter;
    [SerializeField] int Quantiter;
    [SerializeField] Sprite sprite;

    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
       PoissonData poison =  new PoissonData();
        poison.nom = FishName;
        poison.quantiter = Quantiter;
        poison.quantiterMax = MaxQUantiter;
        poison.sprite = sprite;
        Inventaire.Instance.AddItem(poison);
    }
}
