using Uni1Tools.Models;
using Uni1Tools.Services;

namespace Uni1Tools.ViewModels;

public sealed class BmiCalculatorViewModel : BaseViewModel
{
    private readonly BmiService _bmiService;
    private readonly StringResourceService _stringResources;
    private string _weightKg = string.Empty;
    private string _heightCm = string.Empty;
    private string _resultText = string.Empty;
    private string _errorMessage = string.Empty;

    public BmiCalculatorViewModel(BmiService bmiService, StringResourceService stringResources)
    {
        _bmiService = bmiService;
        _stringResources = stringResources;
        CalculateCommand = new RelayCommand(_ => Calculate());
        ResetCommand = new RelayCommand(_ => Reset());
    }

    public string WeightKg
    {
        get => _weightKg;
        set => SetProperty(ref _weightKg, value);
    }

    public string HeightCm
    {
        get => _heightCm;
        set => SetProperty(ref _heightCm, value);
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

    public RelayCommand CalculateCommand { get; }
    public RelayCommand ResetCommand { get; }

    private void Calculate()
    {
        ErrorMessage = string.Empty;
        ResultText = string.Empty;

        if (!ValidationHelper.TryParseDouble(WeightKg, out double weight) ||
            !ValidationHelper.TryParseDouble(HeightCm, out double height))
        {
            ErrorMessage = _stringResources.GetString("ErrorValidWeightHeight");
            return;
        }

        if (weight <= 0 || height <= 0)
        {
            ErrorMessage = _stringResources.GetString("ErrorPositiveWeightHeight");
            return;
        }

        BmiResult result = _bmiService.Calculate(weight, height);
        ResultText = $"BMI: {result.Value:0.0}\n{_stringResources.GetString("LabelCategoryResult")}: {result.Category}\n" +
                     $"{_stringResources.GetString("LabelRecommendation")}: {result.Recommendation}";
    }

    private void Reset()
    {
        WeightKg = string.Empty;
        HeightCm = string.Empty;
        ResultText = string.Empty;
        ErrorMessage = string.Empty;
    }
}
