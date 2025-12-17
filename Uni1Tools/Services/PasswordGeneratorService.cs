using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Uni1Tools.Models;

namespace Uni1Tools.Services;

public sealed class PasswordGeneratorService
{
    private const string UppercaseChars = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string LowercaseChars = "abcdefghijkmnopqrstuvwxyz";
    private const string DigitChars = "23456789";
    private const string SymbolChars = "!@#$%^&*_-+";
    private const string SimilarChars = "O0oIl1";

    /// <summary>
    /// Generates a password using the provided options.
    /// </summary>
    public string GeneratePassword(PasswordOptions options)
    {
        if (options.Length <= 0)
        {
            return string.Empty;
        }

        List<string> pools = new();
        if (options.IncludeUppercase)
        {
            pools.Add(FilterSimilar(UppercaseChars, options.ExcludeSimilar));
        }
        if (options.IncludeLowercase)
        {
            pools.Add(FilterSimilar(LowercaseChars, options.ExcludeSimilar));
        }
        if (options.IncludeDigits)
        {
            pools.Add(FilterSimilar(DigitChars, options.ExcludeSimilar));
        }
        if (options.IncludeSymbols)
        {
            pools.Add(FilterSimilar(SymbolChars, options.ExcludeSimilar));
        }

        if (pools.Count == 0)
        {
            return string.Empty;
        }

        List<char> characters = new();
        foreach (string pool in pools)
        {
            characters.Add(GetRandomChar(pool));
        }

        string combined = string.Concat(pools);
        while (characters.Count < options.Length)
        {
            characters.Add(GetRandomChar(combined));
        }

        return new string(Shuffle(characters).ToArray());
    }

    private static string FilterSimilar(string input, bool excludeSimilar)
    {
        if (!excludeSimilar)
        {
            return input;
        }

        return new string(input.Where(ch => !SimilarChars.Contains(ch)).ToArray());
    }

    private static char GetRandomChar(string pool)
    {
        int index = RandomNumberGenerator.GetInt32(pool.Length);
        return pool[index];
    }

    private static IEnumerable<char> Shuffle(IList<char> characters)
    {
        for (int i = characters.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (characters[i], characters[j]) = (characters[j], characters[i]);
        }

        return characters;
    }
}
