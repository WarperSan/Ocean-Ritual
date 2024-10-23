using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class sharkHitboxProjectile : Projectile
{
    [SerializeField]
    protected Rigidbody rbShark;
    public bool hitPlayer {  get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        OnReset();
        
    }

    protected override void OnReset()
    {
        hitPlayer = false;
        this.Attribute(new Attack() { Damage = 15, TargetType = ProjectileTarget.PLAYER_CONTROLLED, Type = AttackType.NORMAL });
    }
    

    protected override void OnPostApply(Entity entity, Attack attack)
    {
       // Debug.Log(entity.Health);
        hitPlayer = true;

        Vector3 direction = this.rbShark.transform.position - entity.transform.position;
        direction.y = 0;
        direction.Normalize();
        direction *= 10;
        direction.y = 5;

        //knockback?
        this.rbShark.AddForce(direction,ForceMode.Impulse);
    }
}
