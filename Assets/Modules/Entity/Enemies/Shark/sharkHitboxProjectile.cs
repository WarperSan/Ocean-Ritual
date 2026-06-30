using EntityModule;
using UnityEngine;

public class sharkHitboxProjectile : Projectile
{
    [SerializeField]
    protected Rigidbody rbShark;

    public bool hitPlayer { get; private set; }

    // Start is called before the first frame update
    private void Start() => OnReset();

    protected override void OnReset()
    {
        hitPlayer = false;

        Attribute(new Attack
        {
            Damage = 15,
            TargetType = ProjectileTarget.PLAYER_CONTROLLED,
            Type = AttackType.NORMAL,
        });
    }

    protected override void OnPostApply(Entity entity, Attack attack)
    {
        // Debug.Log(entity.Health);
        hitPlayer = true;

        Vector3 direction = rbShark.transform.position - entity.transform.position;
        direction.y = 0;
        direction.Normalize();
        direction *= 10;
        direction.y = 5;

        //knockback?
        rbShark.AddForce(direction, ForceMode.Impulse);
    }
}