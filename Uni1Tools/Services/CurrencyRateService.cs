using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Uni1Tools.Models;

namespace Uni1Tools.Services;

public sealed class CurrencyRateService
{
    private const string RatesFileName = "currency_rates.json";

    /// <summary>
    /// Loads the currency rates from local storage or returns defaults.
    /// </summary>
    public List<CurrencyRate> LoadRates()
    {
        string path = GetRatesFilePath();
        if (!File.Exists(path))
        {
            return GetDefaultRates();
        }

        try
        {
            string json = File.ReadAllText(path);
            List<CurrencyRate>? rates = JsonSerializer.Deserialize<List<CurrencyRate>>(json);
            return rates == null || rates.Count == 0 ? GetDefaultRates() : rates;
        }
        catch
        {
            return GetDefaultRates();
        }
    }

    /// <summary>
    /// Saves the provided rates to local storage.
    /// </summary>
    public void SaveRates(IEnumerable<CurrencyRate> rates)
    {
        string path = GetRatesFilePath();
        string? directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(rates, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Builds a dictionary of rates keyed by currency code.
    /// </summary>
    public Dictionary<string, double> BuildRateDictionary(IEnumerable<CurrencyRate> rates)
    {
        return rates
            .GroupBy(rate => rate.Code, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key.ToUpperInvariant(), group => group.First().Rate);
    }

    private static List<CurrencyRate> GetDefaultRates()
    {
        return new List<CurrencyRate>
        {
            new CurrencyRate { Code = "EUR", Rate = 1.0 },
            new CurrencyRate { Code = "USD", Rate = 1.08 },
            new CurrencyRate { Code = "GBP", Rate = 0.86 },
            new CurrencyRate { Code = "SEK", Rate = 11.2 },
            new CurrencyRate { Code = "NOK", Rate = 11.4 }
        };
    }

    private static string GetRatesFilePath()
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Uni1Tools");
        return Path.Combine(folder, RatesFileName);
    }
}
