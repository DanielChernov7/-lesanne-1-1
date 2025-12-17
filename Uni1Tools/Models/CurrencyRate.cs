using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Uni1Tools.Models;

public sealed class CurrencyRate : INotifyPropertyChanged
{
    private string _code = string.Empty;
    private double _rate;

    public string Code
    {
        get => _code;
        set
        {
            if (_code != value)
            {
                _code = value;
                OnPropertyChanged();
            }
        }
    }

    public double Rate
    {
        get => _rate;
        set
        {
            if (Math.Abs(_rate - value) > 0.0000001)
            {
                _rate = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
