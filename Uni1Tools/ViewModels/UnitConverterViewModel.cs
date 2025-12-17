using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Uni1Tools.Models;
using Uni1Tools.Services;

namespace Uni1Tools.ViewModels;

public sealed class UnitConverterViewModel : BaseViewModel
{
    private readonly UnitConversionService _unitConversionService;
    private readonly StringResourceService _stringResources;
    private string _inputValue = string.Empty;
    private string _resultText = string.Empty;
    private string _errorMessage = string.Empty;
    private NamedOption<UnitCategory>? _selectedCategory;
    private UnitOption? _selectedFromUnit;
    private UnitOption? _selectedToUnit;

    public UnitConverterViewModel(UnitConversionService unitConversionService, StringResourceService stringResources)
    {
        _unitConversionService = unitConversionService;
        _stringResources = stringResources;

        Categories = new ObservableCollection<NamedOption<UnitCategory>>
        {
            new NamedOption<UnitCategory>(_stringResources.GetString("UnitTemperature"), UnitCategory.Temperature),
            new NamedOption<UnitCategory>(_stringResources.GetString("UnitLength"), UnitCategory.Length),
            new NamedOption<UnitCategory>(_stringResources.GetString("UnitMass"), UnitCategory.Mass)
        };

        FromUnits = new ObservableCollection<UnitOption>();
        ToUnits = new ObservableCollection<UnitOption>();

        SelectedCategory = Categories.FirstOrDefault();

        ConvertCommand = new RelayCommand(_ => Convert());
        ResetCommand = new RelayCommand(_ => Reset());
        SwapCommand = new RelayCommand(_ => SwapUnits());
    }

    public ObservableCollection<NamedOption<UnitCategory>> Categories { get; }
    public ObservableCollection<UnitOption> FromUnits { get; }
    public ObservableCollection<UnitOption> ToUnits { get; }

    public NamedOption<UnitCategory>? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (SetProperty(ref _selectedCategory, value) && value != null)
            {
                LoadUnitsForCategory(value.Value);
            }
        }
    }

    public UnitOption? SelectedFromUnit
    {
        get => _selectedFromUnit;
        set => SetProperty(ref _selectedFromUnit, value);
    }

    public UnitOption? SelectedToUnit
    {
        get => _selectedToUnit;
        set => SetProperty(ref _selectedToUnit, value);
    }

    public string InputValue
    {
        get => _inputValue;
        set => SetProperty(ref _inputValue, value);
    }

    public string ResultText
    {
        get => _resultText;
        set => SetProperty(ref _resultText, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public RelayCommand ConvertCommand { get; }
    public RelayCommand ResetCommand { get; }
    public RelayCommand SwapCommand { get; }

    private void LoadUnitsForCategory(UnitCategory category)
    {
        FromUnits.Clear();
        ToUnits.Clear();

        IEnumerable<UnitOption> units = category switch
        {
            UnitCategory.Temperature => new List<UnitOption>
            {
                new UnitOption(_stringResources.GetString("UnitCelsius"), TemperatureUnit.Celsius),
                new UnitOption(_stringResources.GetString("UnitFahrenheit"), TemperatureUnit.Fahrenheit),
                new UnitOption(_stringResources.GetString("UnitKelvin"), TemperatureUnit.Kelvin)
            },
            UnitCategory.Length => new List<UnitOption>
            {
                new UnitOption(_stringResources.GetString("UnitMeter"), LengthUnit.Meter),
                new UnitOption(_stringResources.GetString("UnitKilometer"), LengthUnit.Kilometer),
                new UnitOption(_stringResources.GetString("UnitCentimeter"), LengthUnit.Centimeter),
                new UnitOption(_stringResources.GetString("UnitMillimeter"), LengthUnit.Millimeter)
            },
            UnitCategory.Mass => new List<UnitOption>
            {
                new UnitOption(_stringResources.GetString("UnitGram"), MassUnit.Gram),
                new UnitOption(_stringResources.GetString("UnitKilogram"), MassUnit.Kilogram),
                new UnitOption(_stringResources.GetString("UnitPound"), MassUnit.Pound)
            },
            _ => Enumerable.Empty<UnitOption>()
        };

        foreach (UnitOption unit in units)
        {
            FromUnits.Add(unit);
            ToUnits.Add(unit);
        }

        SelectedFromUnit = FromUnits.FirstOrDefault();
        SelectedToUnit = ToUnits.Skip(1).FirstOrDefault() ?? ToUnits.FirstOrDefault();
        ResultText = string.Empty;
        ErrorMessage = string.Empty;
    }

    private void Convert()
    {
        ErrorMessage = string.Empty;
        ResultText = string.Empty;

        if (!ValidationHelper.TryParseDouble(InputValue, out double value))
        {
            ErrorMessage = _stringResources.GetString("ErrorEnterValidNumber");
            return;
        }

        if (SelectedCategory == null || SelectedFromUnit == null || SelectedToUnit == null)
        {
            ErrorMessage = _stringResources.GetString("ErrorSelectUnits");
            return;
        }

        double result = _unitConversionService.Convert(SelectedCategory.Value, value, SelectedFromUnit.Unit, SelectedToUnit.Unit);
        ResultText = $"{value:0.###} {SelectedFromUnit.Name} = {result:0.###} {SelectedToUnit.Name}";
    }

    private void SwapUnits()
    {
        UnitOption? temp = SelectedFromUnit;
        SelectedFromUnit = SelectedToUnit;
        SelectedToUnit = temp;
    }

    private void Reset()
    {
        InputValue = string.Empty;
        ResultText = string.Empty;
        ErrorMessage = string.Empty;
        SelectedCategory = Categories.FirstOrDefault();
    }
}
