using RandomUtils;
using System.Collections.Generic;

namespace Extensions
{
    public static class RandomExtension
    {
        public static int GetRandomLevel(this IEnumerable<ProbabilityForLevel> probabilities)
        {
            float randomValue = Random.RandomPercent();

            float cumulativeProbability = 0f;
            foreach (ProbabilityForLevel prob in probabilities)
            {
                cumulativeProbability += prob.Probability;

                if (randomValue <= cumulativeProbability)
                    return prob.Level;
            }

            // If no level found,
            return -1;
        }
    }
}