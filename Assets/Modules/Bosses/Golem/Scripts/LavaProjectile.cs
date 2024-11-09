using EntityModule;

namespace BossesModule.Golem
{
    public class LavaProjectile : Projectile
    {
        /// <inheritdoc/>
        protected override void OnPostApply(Entity entity, Attack attack) => this.gameObject.SetActive(false);
    }
}
