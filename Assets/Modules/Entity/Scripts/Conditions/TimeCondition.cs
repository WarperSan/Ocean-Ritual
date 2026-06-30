namespace EntityModule.Conditions
{
    public class TimeCondition : ProjectileCondition
    {
        public float time;
        private float remainingTime;

        /// <inheritdoc/>
        public override bool UpdateCondition(float elapsed)
        {
            remainingTime -= elapsed;
            return remainingTime > 0;
        }

        /// <inheritdoc/>
        public override void ResetCondition() => remainingTime = time;
    }
}