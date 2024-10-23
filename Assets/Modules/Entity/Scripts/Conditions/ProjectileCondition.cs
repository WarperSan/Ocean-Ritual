using UnityEngine;

namespace EntityModule.Conditions
{
    [RequireComponent(typeof(Projectile))]
    public abstract class ProjectileCondition : MonoBehaviour
    {
        /// <summary>
        /// Updates this condition
        /// </summary>
        /// <param name="elapsed">Time passed since last call</param>
        /// <returns>Should the projectile stay alive?</returns>
        public abstract bool UpdateCondition(float elapsed);

        /// <summary>
        /// Resets this condition's variables
        /// </summary>
        public virtual void ResetCondition() { }
    }
}