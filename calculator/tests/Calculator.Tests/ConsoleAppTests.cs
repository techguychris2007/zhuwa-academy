using Calculator.App;

namespace Calculator.Tests;

/// <summary>
/// End-to-end tests: feed the menu scripted input and check what it prints.
/// </summary>
public class ConsoleAppTests
{
    private static string Run(params string[] lines)
    {
        var input = new StringReader(string.Join("\n", lines) + "\n");
        var output = new StringWriter();
        new ConsoleApp(input, output).Run();
        return output.ToString();
    }

    [Fact]
    public void Exit_PrintsGoodbye() =>
        Assert.Contains("Goodbye!", Run("0"));

    [Fact]
    public void Add_PrintsResult() =>
        Assert.Contains("Result: 5", Run("1", "2", "3", "0"));

    [Fact]
    public void Divide_ByZero_ShowsMessageAndKeepsRunning()
    {
        string output = Run("4", "10", "0", "0");
        Assert.Contains("Cannot divide by zero.", output);
        Assert.Contains("Goodbye!", output);
    }

    [Fact]
    public void Celsius_PrintsFahrenheitWithUnit() =>
        Assert.Contains("Result: 212 °F", Run("7", "100", "0"));

    [Fact]
    public void Currency_RejectsNonPositiveRateThenConverts()
    {
        string output = Run("9", "100", "0", "-5", "12.5", "0");
        Assert.Contains("The rate must be greater than zero.", output);
        Assert.Contains("Result: 8.00 USD", output);
    }

    [Fact]
    public void InvalidNumber_IsRejectedAndReprompted()
    {
        string output = Run("1", "abc", "2", "3", "0");
        Assert.Contains("Invalid input. Please enter a valid number.", output);
        Assert.Contains("Result: 5", output);
    }

    [Theory]
    [InlineData("NaN")]
    [InlineData("Infinity")]
    public void NonFiniteNumbers_AreRejected(string value) =>
        Assert.Contains("Invalid input", Run("1", value, "1", "1", "0"));

    [Fact]
    public void UnknownOption_ShowsHelp() =>
        Assert.Contains("Invalid option. Choose a number from 0 to 9.", Run("42", "0"));

    [Fact]
    public void DecimalPoint_IsParsedRegardlessOfMachineCulture()
    {
        var original = Thread.CurrentThread.CurrentCulture;
        try
        {
            // German culture uses ',' as the decimal separator.
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            Assert.Contains("Result: 4", Run("1", "1.5", "2.5", "0"));
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = original;
        }
    }

    [Fact]
    public void ClosedInput_EndsWithoutCrashing()
    {
        // Input stops in the middle of an operation.
        var output = new StringWriter();
        new ConsoleApp(new StringReader("1\n2\n"), output).Run();
        Assert.Contains("Enter the second number:", output.ToString());
    }
}
