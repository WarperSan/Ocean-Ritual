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
            this.totalDistance += this.speed * elapsed;
            return this.totalDistance < this.range;
        }

        /// <inheritdoc/>
        public override void ResetCondition()
        {
            this.speed = 0;
            this.range = 0;
            this.totalDistance = 0;
        }
    }
}