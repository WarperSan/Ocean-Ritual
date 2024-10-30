using EntityModule;
using EntityModule.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TurtleEntity :  EntityBehaviour
{
    [SerializeField] UnityEngine.Sprite sprite;
    [SerializeField] float chance = 20;
    [SerializeField] int min = 1;
    [SerializeField] int max = 4;
    [SerializeField] bool dead = false;
    #region EntityBehaviour
    protected override void OnStart()
    {
        base.OnStart();
    }

    /// <inheritdoc/>
    private void Update()
    {
        this.UpdateTree();
    }
    protected override void OnDeath(float overDamage)
    {
        (bool, int) value = GeneratorGem.randomlvl(min, max, chance);
        if (value.Item1)
        {
            GemData TheGemme = GeneratorGem.GenerateRandomGemme(value.Item2);
            TheGemme.sprite = sprite;
            Inventory.Instance.AddItem(TheGemme);

        }

        this.gameObject.SetActive(false);
    }

    #endregion
}

