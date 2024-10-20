using InteractModule;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField] GemData TheGemme;
    [SerializeField] int lvlOfGemme;
    [SerializeField] Sprite sprite;

    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
        TheGemme= GeneratorGem.GenerateRandomGemme(lvlOfGemme);
        TheGemme.sprite= sprite;
        Inventory.Instance.AddItem(TheGemme);
    }
}
