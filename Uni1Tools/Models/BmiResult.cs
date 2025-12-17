namespace Uni1Tools.Models;

public sealed class BmiResult
{
    public double Value { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}
