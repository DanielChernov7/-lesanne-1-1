using System;
using Uni1Tools.Models;

namespace Uni1Tools.Services;

public sealed class UnitConversionService
{
    /// <summary>
    /// Converts a value between units within the specified category.
    /// </summary>
    public double Convert(UnitCategory category, double value, Enum fromUnit, Enum toUnit)
    {
        return category switch
        {
            UnitCategory.Temperature => ConvertTemperature(value, (TemperatureUnit)fromUnit, (TemperatureUnit)toUnit),
            UnitCategory.Length => ConvertLength(value, (LengthUnit)fromUnit, (LengthUnit)toUnit),
            UnitCategory.Mass => ConvertMass(value, (MassUnit)fromUnit, (MassUnit)toUnit),
            _ => value
        };
    }

    private static double ConvertTemperature(double value, TemperatureUnit from, TemperatureUnit to)
    {
        double kelvin = from switch
        {
            TemperatureUnit.Celsius => value + 273.15,
            TemperatureUnit.Fahrenheit => (value - 32) * 5 / 9 + 273.15,
            TemperatureUnit.Kelvin => value,
            _ => value
        };

        return to switch
        {
            TemperatureUnit.Celsius => kelvin - 273.15,
            TemperatureUnit.Fahrenheit => (kelvin - 273.15) * 9 / 5 + 32,
            TemperatureUnit.Kelvin => kelvin,
            _ => kelvin
        };
    }

    private static double ConvertLength(double value, LengthUnit from, LengthUnit to)
    {
        double meters = from switch
        {
            LengthUnit.Meter => value,
            LengthUnit.Kilometer => value * 1000,
            LengthUnit.Centimeter => value / 100,
            LengthUnit.Millimeter => value / 1000,
            _ => value
        };

        return to switch
        {
            LengthUnit.Meter => meters,
            LengthUnit.Kilometer => meters / 1000,
            LengthUnit.Centimeter => meters * 100,
            LengthUnit.Millimeter => meters * 1000,
            _ => meters
        };
    }

    private static double ConvertMass(double value, MassUnit from, MassUnit to)
    {
        double grams = from switch
        {
            MassUnit.Gram => value,
            MassUnit.Kilogram => value * 1000,
            MassUnit.Pound => value * 453.59237,
            _ => value
        };

        return to switch
        {
            MassUnit.Gram => grams,
            MassUnit.Kilogram => grams / 1000,
            MassUnit.Pound => grams / 453.59237,
            _ => grams
        };
    }
}
