using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEntity : Entity
{
    
    protected override void OnDeath(float overDamage)
    {
        this.gameObject.SetActive(false);
    }
}
