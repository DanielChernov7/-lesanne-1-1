namespace Uni1Tools.Models;

public sealed class NamedOption<T>
{
    public NamedOption(string name, T value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public T Value { get; }
}
