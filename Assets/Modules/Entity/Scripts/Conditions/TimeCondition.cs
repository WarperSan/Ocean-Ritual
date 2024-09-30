namespace EntityModule.Conditions
{
    public class TimeCondition : ProjectileCondition
    {
        public float time;
        private float remainingTime;

        /// <inheritdoc/>
        public override bool UpdateCondition(float elapsed)
        {
            this.remainingTime -= elapsed;
            return this.remainingTime > 0;
        }

        /// <inheritdoc/>
        public override void ResetCondition()
        {
            this.remainingTime = time;
        }
    }
}