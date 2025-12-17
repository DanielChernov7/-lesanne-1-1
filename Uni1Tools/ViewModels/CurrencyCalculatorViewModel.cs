using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Uni1Tools.Models;
using Uni1Tools.Services;

namespace Uni1Tools.ViewModels;

public sealed class CurrencyCalculatorViewModel : BaseViewModel
{
    private readonly CurrencyRateService _currencyRateService;
    private readonly StringResourceService _stringResources;
    private NamedOption<string>? _selectedFromCurrency;
    private NamedOption<string>? _selectedToCurrency;
    private string _amount = string.Empty;
    private string _resultText = string.Empty;
    private string _errorMessage = string.Empty;
    private string _statusMessage = string.Empty;

    public CurrencyCalculatorViewModel(CurrencyRateService currencyRateService, StringResourceService stringResources)
    {
        _currencyRateService = currencyRateService;
        _stringResources = stringResources;

        Rates = new ObservableCollection<CurrencyRate>(_currencyRateService.LoadRates());
        CurrencyOptions = new ObservableCollection<NamedOption<string>>(
            Rates.Select(rate => new NamedOption<string>(rate.Code, rate.Code))
                 .OrderBy(option => option.Name));

        SelectedFromCurrency = CurrencyOptions.FirstOrDefault();
        SelectedToCurrency = CurrencyOptions.Skip(1).FirstOrDefault() ?? CurrencyOptions.FirstOrDefault();

        ConvertCommand = new RelayCommand(_ => Convert());
        ResetCommand = new RelayCommand(_ => Reset());
        SaveRatesCommand = new RelayCommand(_ => SaveRates());
    }

    public ObservableCollection<CurrencyRate> Rates { get; }
    public ObservableCollection<NamedOption<string>> CurrencyOptions { get; }

    public NamedOption<string>? SelectedFromCurrency
    {
        get => _selectedFromCurrency;
        set => SetProperty(ref _selectedFromCurrency, value);
    }

    public NamedOption<string>? SelectedToCurrency
    {
        get => _selectedToCurrency;
        set => SetProperty(ref _selectedToCurrency, value);
    }

    public string Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
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

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public RelayCommand ConvertCommand { get; }
    public RelayCommand ResetCommand { get; }
    public RelayCommand SaveRatesCommand { get; }

    private void Convert()
    {
        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;
        ResultText = string.Empty;

        if (!ValidationHelper.TryParseDouble(Amount, out double amountValue))
        {
            ErrorMessage = _stringResources.GetString("ErrorValidAmount");
            return;
        }

        if (amountValue < 0)
        {
            ErrorMessage = _stringResources.GetString("ErrorAmountPositive");
            return;
        }

        if (SelectedFromCurrency == null || SelectedToCurrency == null)
        {
            ErrorMessage = _stringResources.GetString("ErrorSelectCurrencies");
            return;
        }

        Dictionary<string, double> rateTable = _currencyRateService.BuildRateDictionary(Rates);
        if (!rateTable.TryGetValue(SelectedFromCurrency.Value, out double rateFrom) ||
            !rateTable.TryGetValue(SelectedToCurrency.Value, out double rateTo))
        {
            ErrorMessage = _stringResources.GetString("ErrorRateMissing");
            return;
        }

        double result = amountValue / rateFrom * rateTo;
        double usedRate = rateTo / rateFrom;
        string timestamp = DateTime.Now.ToString("g", CultureInfo.CurrentCulture);

        ResultText = $"{amountValue:0.##} {SelectedFromCurrency.Value} = {result:0.##} {SelectedToCurrency.Value}\n" +
                     $"{_stringResources.GetString("LabelRateUsed")}: {usedRate:0.####}\n" +
                     $"{_stringResources.GetString("LabelUpdated")}: {timestamp}";
    }

    private void SaveRates()
    {
        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;

        foreach (CurrencyRate rate in Rates)
        {
            if (string.IsNullOrWhiteSpace(rate.Code) || rate.Rate <= 0)
            {
                ErrorMessage = _stringResources.GetString("ErrorInvalidRate");
                return;
            }
        }

        _currencyRateService.SaveRates(Rates);
        StatusMessage = _stringResources.GetString("StatusRatesSaved");
    }

    private void Reset()
    {
        Amount = string.Empty;
        ResultText = string.Empty;
        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;
        SelectedFromCurrency = CurrencyOptions.FirstOrDefault();
        SelectedToCurrency = CurrencyOptions.Skip(1).FirstOrDefault() ?? CurrencyOptions.FirstOrDefault();
    }
}
