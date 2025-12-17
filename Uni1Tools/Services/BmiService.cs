using Uni1Tools.Models;

namespace Uni1Tools.Services;

public sealed class BmiService
{
    private readonly StringResourceService _stringResources;

    public BmiService(StringResourceService stringResources)
    {
        _stringResources = stringResources;
    }

    /// <summary>
    /// Calculates BMI, category, and recommendation text.
    /// </summary>
    public BmiResult Calculate(double weightKg, double heightCm)
    {
        double heightMeters = heightCm / 100;
        double bmi = weightKg / (heightMeters * heightMeters);
        string categoryKey;
        string recommendationKey;

        if (bmi < 18.5)
        {
            categoryKey = "BmiUnderweight";
            recommendationKey = "BmiUnderweightRec";
        }
        else if (bmi < 25)
        {
            categoryKey = "BmiNormal";
            recommendationKey = "BmiNormalRec";
        }
        else if (bmi < 30)
        {
            categoryKey = "BmiOverweight";
            recommendationKey = "BmiOverweightRec";
        }
        else
        {
            categoryKey = "BmiObese";
            recommendationKey = "BmiObeseRec";
        }

        return new BmiResult
        {
            Value = bmi,
            Category = _stringResources.GetString(categoryKey),
            Recommendation = _stringResources.GetString(recommendationKey)
        };
    }
}
