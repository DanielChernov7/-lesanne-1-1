namespace Uni1Tools.Models;

public sealed class PasswordOptions
{
    public int Length { get; set; }
    public bool IncludeUppercase { get; set; }
    public bool IncludeLowercase { get; set; }
    public bool IncludeDigits { get; set; }
    public bool IncludeSymbols { get; set; }
    public bool ExcludeSimilar { get; set; }
}
