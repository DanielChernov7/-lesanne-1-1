using System.Windows;

namespace Uni1Tools.Services;

public sealed class StringResourceService
{
    /// <summary>
    /// Gets a localized string from the application resources.
    /// </summary>
    public string GetString(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        return Application.Current.TryFindResource(key) as string ?? key;
    }
}
