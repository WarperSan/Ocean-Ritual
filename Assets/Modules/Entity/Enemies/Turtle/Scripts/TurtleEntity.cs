using EntityModule.Entities;
using UnityEngine;

public class TurtleEntity : EntityBehaviour
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float chance = 20;

    [SerializeField]
    private int min = 1;

    [SerializeField]
    private int max = 4;

    #region EntityBehaviour

    protected override void OnStart() => base.OnStart();

    /// <inheritdoc/>
    private void Update() => UpdateTree();

    protected override void OnDeath(float overDamage)
    {
        (bool, int) value = GeneratorGem.randomlvl(min, max, chance);

        if (value.Item1)
        {
            GemData TheGemme = GeneratorGem.GenerateRandomGemme(value.Item2);
            TheGemme.sprite = sprite;
            Inventory.Instance.AddItem(TheGemme);
        }

        gameObject.SetActive(false);
    }

    #endregion
}