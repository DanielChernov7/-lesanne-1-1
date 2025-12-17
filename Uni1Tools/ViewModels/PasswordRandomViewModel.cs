using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Uni1Tools.Models;
using Uni1Tools.Services;

namespace Uni1Tools.ViewModels;

public sealed class PasswordRandomViewModel : BaseViewModel
{
    private readonly PasswordGeneratorService _passwordGeneratorService;
    private readonly RandomNumberService _randomNumberService;
    private readonly StringResourceService _stringResources;

    private int _passwordLength = 16;
    private bool _includeUppercase = true;
    private bool _includeLowercase = true;
    private bool _includeDigits = true;
    private bool _includeSymbols = false;
    private bool _excludeSimilar = true;
    private string _generatedPassword = string.Empty;
    private string _strengthLabel = string.Empty;
    private string _randomMin = "1";
    private string _randomMax = "100";
    private string _randomCount = "5";
    private bool _uniqueNumbers = false;
    private string _randomNumbersOutput = string.Empty;
    private string _errorMessage = string.Empty;
    private string _statusMessage = string.Empty;

    public PasswordRandomViewModel(
        PasswordGeneratorService passwordGeneratorService,
        RandomNumberService randomNumberService,
        StringResourceService stringResources)
    {
        _passwordGeneratorService = passwordGeneratorService;
        _randomNumberService = randomNumberService;
        _stringResources = stringResources;

        GeneratePasswordCommand = new RelayCommand(_ => GeneratePassword());
        GenerateRandomCommand = new RelayCommand(_ => GenerateRandomNumbers());
        ResetCommand = new RelayCommand(_ => Reset());
        CopyPasswordCommand = new RelayCommand(_ => CopyPassword(), _ => !string.IsNullOrWhiteSpace(GeneratedPassword));

        StrengthLabel = GetStrengthLabel();
    }

    public int PasswordLength
    {
        get => _passwordLength;
        set
        {
            if (SetProperty(ref _passwordLength, value))
            {
                StrengthLabel = GetStrengthLabel();
            }
        }
    }

    public bool IncludeUppercase
    {
        get => _includeUppercase;
        set => SetProperty(ref _includeUppercase, value);
    }

    public bool IncludeLowercase
    {
        get => _includeLowercase;
        set => SetProperty(ref _includeLowercase, value);
    }

    public bool IncludeDigits
    {
        get => _includeDigits;
        set => SetProperty(ref _includeDigits, value);
    }

    public bool IncludeSymbols
    {
        get => _includeSymbols;
        set => SetProperty(ref _includeSymbols, value);
    }

    public bool ExcludeSimilar
    {
        get => _excludeSimilar;
        set => SetProperty(ref _excludeSimilar, value);
    }

    public string GeneratedPassword
    {
        get => _generatedPassword;
        set
        {
            if (SetProperty(ref _generatedPassword, value))
            {
                CopyPasswordCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string StrengthLabel
    {
        get => _strengthLabel;
        set => SetProperty(ref _strengthLabel, value);
    }

    public string RandomMin
    {
        get => _randomMin;
        set => SetProperty(ref _randomMin, value);
    }

    public string RandomMax
    {
        get => _randomMax;
        set => SetProperty(ref _randomMax, value);
    }

    public string RandomCount
    {
        get => _randomCount;
        set => SetProperty(ref _randomCount, value);
    }

    public bool UniqueNumbers
    {
        get => _uniqueNumbers;
        set => SetProperty(ref _uniqueNumbers, value);
    }

    public string RandomNumbersOutput
    {
        get => _randomNumbersOutput;
        set => SetProperty(ref _randomNumbersOutput, value);
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

    public RelayCommand GeneratePasswordCommand { get; }
    public RelayCommand GenerateRandomCommand { get; }
    public RelayCommand ResetCommand { get; }
    public RelayCommand CopyPasswordCommand { get; }

    private void GeneratePassword()
    {
        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;

        bool[] categories = { IncludeUppercase, IncludeLowercase, IncludeDigits, IncludeSymbols };
        int selectedCategories = categories.Count(selected => selected);

        if (selectedCategories == 0)
        {
            ErrorMessage = _stringResources.GetString("ErrorSelectCategory");
            return;
        }

        if (PasswordLength < selectedCategories)
        {
            ErrorMessage = _stringResources.GetString("ErrorLengthTooShort");
            return;
        }

        PasswordOptions options = new()
        {
            Length = PasswordLength,
            IncludeUppercase = IncludeUppercase,
            IncludeLowercase = IncludeLowercase,
            IncludeDigits = IncludeDigits,
            IncludeSymbols = IncludeSymbols,
            ExcludeSimilar = ExcludeSimilar
        };

        GeneratedPassword = _passwordGeneratorService.GeneratePassword(options);
        StrengthLabel = GetStrengthLabel();
    }

    private void GenerateRandomNumbers()
    {
        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;

        if (!ValidationHelper.TryParseInt(RandomMin, out int min) ||
            !ValidationHelper.TryParseInt(RandomMax, out int max) ||
            !ValidationHelper.TryParseInt(RandomCount, out int count))
        {
            ErrorMessage = _stringResources.GetString("ErrorRandomInputs");
            return;
        }

        if (count <= 0)
        {
            ErrorMessage = _stringResources.GetString("ErrorRandomCount");
            return;
        }

        int rangeSize = Math.Abs(max - min) + 1;
        if (UniqueNumbers && count > rangeSize)
        {
            ErrorMessage = _stringResources.GetString("ErrorRandomRange");
            return;
        }

        List<int> results = _randomNumberService.GenerateNumbers(min, max, count, UniqueNumbers);
        RandomNumbersOutput = string.Join(", ", results.OrderBy(value => value));
    }

    private void CopyPassword()
    {
        if (!string.IsNullOrWhiteSpace(GeneratedPassword))
        {
            Clipboard.SetText(GeneratedPassword);
            StatusMessage = _stringResources.GetString("StatusPasswordCopied");
        }
    }

    private void Reset()
    {
        PasswordLength = 16;
        IncludeUppercase = true;
        IncludeLowercase = true;
        IncludeDigits = true;
        IncludeSymbols = false;
        ExcludeSimilar = true;
        GeneratedPassword = string.Empty;
        StrengthLabel = string.Empty;

        RandomMin = "1";
        RandomMax = "100";
        RandomCount = "5";
        UniqueNumbers = false;
        RandomNumbersOutput = string.Empty;

        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;
    }

    private string GetStrengthLabel()
    {
        int categories = new[] { IncludeUppercase, IncludeLowercase, IncludeDigits, IncludeSymbols }.Count(selected => selected);
        int score = categories;

        if (PasswordLength >= 12)
        {
            score++;
        }
        if (PasswordLength >= 20)
        {
            score++;
        }
        if (PasswordLength >= 32)
        {
            score++;
        }

        return score switch
        {
            <= 2 => _stringResources.GetString("StrengthWeak"),
            <= 4 => _stringResources.GetString("StrengthMedium"),
            _ => _stringResources.GetString("StrengthStrong")
        };
    }
}
