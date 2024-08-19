namespace ProjectilesModule.Interfaces
{
    /// <summary>
    /// Defines the objects that want to be notify when a collision with a projectile happened
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// Called when a projectile collided with this object
        /// </summary>
        /// <param name="source">Projectile that caused the collision</param>
        public void OnCollision(Projectile source);
    }
}