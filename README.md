# Uni1 Tool Suite (WPF .NET 8)

A small WPF desktop app that includes:
- Unit converter (temperature, length, mass)
- Currency exchange calculator with editable rates
- Password generator and random number generator
- BMI (KMI) calculator

## Requirements
- Windows
- .NET 8 SDK

## Build and Run
```powershell
# From the repo root
 dotnet build
 dotnet run --project Uni1Tools\Uni1Tools.csproj
```

## Calculator Overview
- Unit Converter: Convert between temperature, length, and mass units with swap support.
- Currency Calculator: Convert currencies using built-in rates, edit and save custom rates to AppData.
- Password + Random: Generate secure passwords with selectable rules and copy to clipboard; generate random numbers with unique option.
- BMI (KMI): Calculate BMI from weight and height with category and recommendation.

## Currency Rates Storage
Rates are saved to:
`%AppData%\Uni1Tools\currency_rates.json`

## Screenshots
Add screenshots here:
- `docs/screenshots/unit-converter.png`
- `docs/screenshots/currency-calculator.png`
- `docs/screenshots/password-random.png`
- `docs/screenshots/bmi-calculator.png`

## Push to GitHub
1) Initialize and commit:
```powershell
 git init
 git add .
 git commit -m "Initial WPF calculator suite"
```

2) Create a GitHub repo and add the remote:
```powershell
 git branch -M main
 git remote add origin https://github.com/<your-username>/<your-repo>.git
 git push -u origin main
```

3) Paste your GitHub link here:
- https://github.com/<your-username>/<your-repo>
