using System.Collections.Generic;
using System.Linq;

public struct ProbabilityForLevel
{
    public int Level;
    public float Probability;

    public ProbabilityForLevel(int level, float probability)
    {
        this.Level = level;
        this.Probability = probability;
    }
}

static class GestionProbabiliter
{
    private const float PERCENT_REDUCTION = 0.75f;
    private const float MIN_PERCENTAGE = 10f;

    public static ProbabilityForLevel[] ProbabilitiesForLevels(IEnumerable<int> levels)
    {
        int amount = levels.Count();

        // If no level
        if (amount == 0)
            return System.Array.Empty<ProbabilityForLevel>();

        // If only one level
        if (amount == 1)
        {
            return new ProbabilityForLevel[]
            {
                new(levels.ElementAt(0), 100)
            };
        }

        // Calculate for each level
        var probabilities = new ProbabilityForLevel[amount];

        float percentRemaining = 100f;

        // Set level for each level
        for (int i = 0; i < amount; i++)
        {
            float probability = percentRemaining;

            // If there is a percent remaining
            if (percentRemaining >= MIN_PERCENTAGE)
                probability = percentRemaining * PERCENT_REDUCTION;

            // Reduce remaining percent
            percentRemaining -= probability;

            // Add probability to level
            probabilities[i] = new ProbabilityForLevel(levels.ElementAt(i), probability);

            // If second remains less than MIN, set to 0%
            if (i == 0 && percentRemaining < MIN_PERCENTAGE)
                percentRemaining = 0;
        }

        // Add remaining to last one
        if (percentRemaining > 0)
            probabilities[^1].Probability += percentRemaining;

        return probabilities;
    }
}