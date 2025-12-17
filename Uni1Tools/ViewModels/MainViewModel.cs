using Uni1Tools.Services;

namespace Uni1Tools.ViewModels;

public sealed class MainViewModel : BaseViewModel
{
    private readonly UnitConverterViewModel _unitConverterViewModel;
    private readonly CurrencyCalculatorViewModel _currencyCalculatorViewModel;
    private readonly PasswordRandomViewModel _passwordRandomViewModel;
    private readonly BmiCalculatorViewModel _bmiCalculatorViewModel;
    private BaseViewModel _currentViewModel;

    public MainViewModel()
    {
        StringResourceService stringResources = new();
        UnitConversionService unitConversionService = new();
        CurrencyRateService currencyRateService = new();
        PasswordGeneratorService passwordGeneratorService = new();
        RandomNumberService randomNumberService = new();
        BmiService bmiService = new(stringResources);

        _unitConverterViewModel = new UnitConverterViewModel(unitConversionService, stringResources);
        _currencyCalculatorViewModel = new CurrencyCalculatorViewModel(currencyRateService, stringResources);
        _passwordRandomViewModel = new PasswordRandomViewModel(passwordGeneratorService, randomNumberService, stringResources);
        _bmiCalculatorViewModel = new BmiCalculatorViewModel(bmiService, stringResources);

        _currentViewModel = _unitConverterViewModel;

        ShowUnitConverterCommand = new RelayCommand(_ => CurrentViewModel = _unitConverterViewModel);
        ShowCurrencyCalculatorCommand = new RelayCommand(_ => CurrentViewModel = _currencyCalculatorViewModel);
        ShowPasswordRandomCommand = new RelayCommand(_ => CurrentViewModel = _passwordRandomViewModel);
        ShowBmiCalculatorCommand = new RelayCommand(_ => CurrentViewModel = _bmiCalculatorViewModel);
    }

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public RelayCommand ShowUnitConverterCommand { get; }
    public RelayCommand ShowCurrencyCalculatorCommand { get; }
    public RelayCommand ShowPasswordRandomCommand { get; }
    public RelayCommand ShowBmiCalculatorCommand { get; }
}
