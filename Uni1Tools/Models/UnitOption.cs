using System;

namespace Uni1Tools.Models;

public sealed class UnitOption
{
    public UnitOption(string name, Enum unit)
    {
        Name = name;
        Unit = unit;
    }

    public string Name { get; }
    public Enum Unit { get; }
}
