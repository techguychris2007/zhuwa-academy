using System.Globalization;

namespace Calculator.App;

/// This is a calculator and a unit converter app
public class ConsoleApp
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private readonly TextReader _input;
    private readonly TextWriter _output;

    public ConsoleApp(TextReader input, TextWriter output)
    {
        _input = input;
        _output = output;
    }

    public void Run()
    {
        while (true)
        {
            PrintMenu();

            // A closed input stream (null) is treated the same as choosing Exit.
            string choice = _input.ReadLine()?.Trim() ?? "0";

            if (choice == "0")
            {
                _output.WriteLine("Goodbye!");
                return;
            }

            if (!HandleChoice(choice))
                return; // input ended part-way through an operation
        }
    }

    private void PrintMenu()
    {
        _output.WriteLine();
        _output.WriteLine("=== Calculator and Unit Converter ===");
        _output.WriteLine("1. Add");
        _output.WriteLine("2. Subtract");
        _output.WriteLine("3. Multiply");
        _output.WriteLine("4. Divide");
        _output.WriteLine("5. Kilometres to miles");
        _output.WriteLine("6. Miles to kilometres");
        _output.WriteLine("7. Celsius to Fahrenheit");
        _output.WriteLine("8. Fahrenheit to Celsius");
        _output.WriteLine("9. GHS to USD");
        _output.WriteLine("0. Exit");
        _output.Write("Choose an option: ");
    }

    
    private bool HandleChoice(string choice)
    {
        switch (choice)
        {
            case "1":
            case "2":
            case "3":
            case "4":
                return RunArithmetic(choice);

            case "5":
                return Convert("Enter kilometres: ", Operations.KilometresToMiles, "miles");

            case "6":
                return Convert("Enter miles: ", Operations.MilesToKilometres, "km");

            case "7":
                return Convert("Enter Celsius: ", Operations.CelsiusToFahrenheit, "°F");

            case "8":
                return Convert("Enter Fahrenheit: ", Operations.FahrenheitToCelsius, "°C");

            case "9":
                return RunCurrency();

            default:
                _output.WriteLine("Invalid option. Choose a number from 0 to 9.");
                return true;
        }
    }

    private bool RunArithmetic(string choice)
    {
        double? first = ReadDouble("Enter the first number: ");
        if (first is null) return false;

        double? second = ReadDouble("Enter the second number: ");
        if (second is null) return false;

        double a = first.Value, b = second.Value;

        double result;
        switch (choice)
        {
            case "1": result = Operations.Add(a, b); break;
            case "2": result = Operations.Subtract(a, b); break;
            case "3": result = Operations.Multiply(a, b); break;
            default:
                if (b == 0)
                {
                    _output.WriteLine("Cannot divide by zero.");
                    return true;
                }
                result = Operations.Divide(a, b);
                break;
        }

        ShowResult(result, "");
        return true;
    }

    private bool Convert(string prompt, Func<double, double> conversion, string unit)
    {
        double? value = ReadDouble(prompt);
        if (value is null) return false;

        ShowResult(conversion(value.Value), unit);
        return true;
    }

    private bool RunCurrency()
    {
        decimal? ghs = ReadDecimal("Enter the amount in GHS: ");
        if (ghs is null) return false;

        while (true)
        {
            decimal? rate = ReadDecimal("Enter the exchange rate (GHS for 1 USD): ");
            if (rate is null) return false;

            if (rate.Value <= 0)
            {
                _output.WriteLine("The rate must be greater than zero.");
                continue;
            }

            decimal usd = Operations.GhsToUsd(ghs.Value, rate.Value);
            _output.WriteLine($"Result: {usd.ToString("0.00", Culture)} USD");
            return true;
        }
    }

    /// <summary>Prompts until a finite number is entered. Returns null if input ends.</summary>
    private double? ReadDouble(string prompt)
    {
        while (true)
        {
            _output.Write(prompt);
            string? input = _input.ReadLine();
            if (input is null) return null;

            if (double.TryParse(input, NumberStyles.Float, Culture, out double number)
                && double.IsFinite(number))
            {
                return number;
            }

            _output.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    /// <summary>Prompts until a valid decimal is entered. Returns null if input ends.</summary>
    private decimal? ReadDecimal(string prompt)
    {
        while (true)
        {
            _output.Write(prompt);
            string? input = _input.ReadLine();
            if (input is null) return null;

            if (decimal.TryParse(input, NumberStyles.Number, Culture, out decimal number))
                return number;

            _output.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private void ShowResult(double result, string unit)
    {
        if (!double.IsFinite(result))
        {
            _output.WriteLine("The result is outside the supported number range.");
            return;
        }

        string text = result.ToString("0.####", Culture);
        _output.WriteLine(unit.Length == 0 ? $"Result: {text}" : $"Result: {text} {unit}");
    }
}
