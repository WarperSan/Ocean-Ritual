namespace EntityModule.Conditions
{
    public class DistanceCondition : ProjectileCondition
    {
        private float speed;
        private float range;
        private float totalDistance;

        public void Set(float speed, float range)
        {
            this.speed = speed;
            this.range = range;
        }

        /// <inheritdoc/>
        public override bool UpdateCondition(float elapsed)
        {
            totalDistance += speed * elapsed;
            return totalDistance < range;
        }

        /// <inheritdoc/>
        public override void ResetCondition()
        {
            speed = 0;
            range = 0;
            totalDistance = 0;
        }
    }
}