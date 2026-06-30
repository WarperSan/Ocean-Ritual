using InteractModule;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GemData TheGemme;

    [SerializeField]
    private int lvlOfGemme;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private bool DeletAfterGiveGem;

    [SerializeField]
    private AudioClip MusicPickGemme;

    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
        TheGemme = GeneratorGem.GenerateRandomGemme(lvlOfGemme);
        TheGemme.sprite = sprite;
        Inventory.Instance.AddItem(TheGemme);

        if (MusicPickGemme != null)
            SoundManager.Instance.PlaySound(MusicPickGemme, SoundType.UI, 1f);

        if (DeletAfterGiveGem)
            Destroy(gameObject);
    }
}