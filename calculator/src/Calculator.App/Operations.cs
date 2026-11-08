namespace Calculator.App;

/// <summary>
/// Pure calculation and conversion rules. No console input or output
/// happens here, so every rule can be unit tested directly.
/// </summary>
public static class Operations
{
    public const double KilometresPerMile = 1.609344;

    public static double Add(double first, double second) => first + second;

    public static double Subtract(double first, double second) => first - second;

    public static double Multiply(double first, double second) => first * second;

    public static double Divide(double first, double second)
    {
        if (second == 0)
            throw new DivideByZeroException("Cannot divide by zero.");

        return first / second;
    }

    public static double KilometresToMiles(double kilometres) =>
        kilometres / KilometresPerMile;

    public static double MilesToKilometres(double miles) =>
        miles * KilometresPerMile;

    public static double CelsiusToFahrenheit(double celsius) =>
        celsius * 9.0 / 5.0 + 32;

    public static double FahrenheitToCelsius(double fahrenheit) =>
        (fahrenheit - 32) * 5.0 / 9.0;

    /// <summary>
    /// Converts cedis to dollars. <paramref name="ghsPerUsd"/> is how many
    /// cedis buy one dollar. Uses decimal so money is not affected by
    /// binary floating-point rounding. The result is rounded to cents.
    /// </summary>
    public static decimal GhsToUsd(decimal ghs, decimal ghsPerUsd)
    {
        if (ghsPerUsd <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(ghsPerUsd), "The rate must be greater than zero.");

        return Math.Round(ghs / ghsPerUsd, 2, MidpointRounding.AwayFromZero);
    }
}
