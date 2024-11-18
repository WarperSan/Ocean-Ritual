using InteractModule;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField] GemData TheGemme;
    [SerializeField] int lvlOfGemme;
    [SerializeField] Sprite sprite;
    [SerializeField] bool DeletAfterGiveGem = false;
    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
        TheGemme= GeneratorGem.GenerateRandomGemme(lvlOfGemme);
        TheGemme.sprite= sprite;
        Inventory.Instance.AddItem(TheGemme);
        if (DeletAfterGiveGem)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
