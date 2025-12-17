using System.Globalization;

namespace Uni1Tools.Services;

public static class ValidationHelper
{
    /// <summary>
    /// Attempts to parse a double using the current culture.
    /// </summary>
    public static bool TryParseDouble(string? input, out double value)
    {
        return double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
    }

    /// <summary>
    /// Attempts to parse an integer using the current culture.
    /// </summary>
    public static bool TryParseInt(string? input, out int value)
    {
        return int.TryParse(input, NumberStyles.Integer, CultureInfo.CurrentCulture, out value);
    }
}
