using EntityModule;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TurtleProjectile : Projectile
{
    [SerializeField] int DPS = 30;
    public void CreateAttack()
    {

        ResetSelf();
        Attribute(new Attack()
        {
            Damage = DPS,
            Type = AttackType.NORMAL,
            TargetType = ProjectileTarget.PLAYER
        });
        

    }
    protected override void OnPostApply(Entity entity, Attack attack) { this.gameObject.SetActive(false); } 
    protected override void OnStart() { CreateAttack(); }
}
