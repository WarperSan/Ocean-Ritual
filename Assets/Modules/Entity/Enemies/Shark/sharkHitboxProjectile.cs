using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkHitboxProjectile : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        this.Attribute(new Attack() { Damage = 15, TargetType = ProjectileTarget.PLAYER, Type = AttackType.NORMAL});
    }

    protected override void OnPostApply(Entity entity, Attack attack)
    {
        Debug.Log(entity.Health);
    }
}
