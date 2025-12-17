using System;
using System.Collections.Generic;

namespace Uni1Tools.Services;

public sealed class RandomNumberService
{
    /// <summary>
    /// Generates a list of random numbers within the given range.
    /// </summary>
    public List<int> GenerateNumbers(int min, int max, int count, bool unique)
    {
        if (min > max)
        {
            (min, max) = (max, min);
        }

        List<int> results = new();
        int rangeSize = max - min + 1;

        if (unique && count > rangeSize)
        {
            return results;
        }

        if (unique)
        {
            HashSet<int> set = new();
            while (set.Count < count)
            {
                int value = Random.Shared.Next(min, max + 1);
                set.Add(value);
            }
            results.AddRange(set);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                results.Add(Random.Shared.Next(min, max + 1));
            }
        }

        return results;
    }
}
