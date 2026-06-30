using EntityModule;
using UnityEngine;

public class TurtleProjectile : Projectile
{
    [SerializeField]
    private int DPS = 30;

    public void CreateAttack()
    {
        ResetSelf();

        Attribute(new Attack
        {
            Damage = DPS,
            Type = AttackType.NORMAL,
            TargetType = ProjectileTarget.PLAYER,
        });
    }

    protected override void OnPostApply(Entity entity, Attack attack) => gameObject.SetActive(false);
    protected override void OnStart() => CreateAttack();
}