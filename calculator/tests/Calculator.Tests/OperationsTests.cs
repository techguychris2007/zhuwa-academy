using Calculator.App;

namespace Calculator.Tests;

public class OperationsTests
{
    private const int Precision = 6;

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-4, 1.5, -2.5)]
    [InlineData(0, 0, 0)]
    public void Add_ReturnsSum(double a, double b, double expected) =>
        Assert.Equal(expected, Operations.Add(a, b), Precision);

    [Theory]
    [InlineData(10, 4, 6)]
    [InlineData(4, 10, -6)]
    public void Subtract_ReturnsDifference(double a, double b, double expected) =>
        Assert.Equal(expected, Operations.Subtract(a, b), Precision);

    [Theory]
    [InlineData(3, 4, 12)]
    [InlineData(-2, 2.5, -5)]
    [InlineData(7, 0, 0)]
    public void Multiply_ReturnsProduct(double a, double b, double expected) =>
        Assert.Equal(expected, Operations.Multiply(a, b), Precision);

    [Theory]
    [InlineData(10, 4, 2.5)]
    [InlineData(-9, 3, -3)]
    public void Divide_ReturnsQuotient(double a, double b, double expected) =>
        Assert.Equal(expected, Operations.Divide(a, b), Precision);

    [Fact]
    public void Divide_ByZero_Throws() =>
        Assert.Throws<DivideByZeroException>(() => Operations.Divide(5, 0));

    [Theory]
    [InlineData(1.609344, 1)]
    [InlineData(100, 62.137119)]
    [InlineData(0, 0)]
    public void KilometresToMiles_Converts(double km, double expectedMiles) =>
        Assert.Equal(expectedMiles, Operations.KilometresToMiles(km), Precision);

    [Theory]
    [InlineData(1, 1.609344)]
    [InlineData(26.2, 42.164813)]
    public void MilesToKilometres_Converts(double miles, double expectedKm) =>
        Assert.Equal(expectedKm, Operations.MilesToKilometres(miles), Precision);

    [Theory]
    [InlineData(0, 32)]
    [InlineData(100, 212)]
    [InlineData(-40, -40)]
    [InlineData(37, 98.6)]
    public void CelsiusToFahrenheit_Converts(double c, double expectedF) =>
        Assert.Equal(expectedF, Operations.CelsiusToFahrenheit(c), Precision);

    [Theory]
    [InlineData(32, 0)]
    [InlineData(212, 100)]
    [InlineData(-40, -40)]
    public void FahrenheitToCelsius_Converts(double f, double expectedC) =>
        Assert.Equal(expectedC, Operations.FahrenheitToCelsius(f), Precision);

    [Theory]
    [InlineData(5.5)]
    [InlineData(-273.15)]
    public void TemperatureConversions_RoundTrip(double celsius) =>
        Assert.Equal(celsius,
            Operations.FahrenheitToCelsius(Operations.CelsiusToFahrenheit(celsius)),
            Precision);

    [Fact]
    public void GhsToUsd_DividesByRate() =>
        Assert.Equal(8.00m, Operations.GhsToUsd(100m, 12.5m));

    [Fact]
    public void GhsToUsd_RoundsToCents() =>
        Assert.Equal(3.33m, Operations.GhsToUsd(10m, 3m));

    [Fact]
    public void GhsToUsd_RoundsMidpointAwayFromZero() =>
        Assert.Equal(0.13m, Operations.GhsToUsd(0.25m, 2m)); // 0.125 -> 0.13

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GhsToUsd_NonPositiveRate_Throws(decimal rate) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Operations.GhsToUsd(100m, rate));
}
