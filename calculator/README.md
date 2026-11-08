# Calculator and Unit Converter

A C# console app that does basic arithmetic and converts between common units.

## Features

| Option | What it does |
|---|---|
| 1–4 | Add, subtract, multiply, divide |
| 5 / 6 | Kilometres ⇄ miles |
| 7 / 8 | Celsius ⇄ Fahrenheit |
| 9 | GHS → USD at an exchange rate you enter |
| 0 | Exit |

## Requirements

[.NET SDK 10.0](https://dotnet.microsoft.com/download) or newer. Check with:

```powershell
dotnet --version
```

## Run

From the `calculator` folder:

```powershell
dotnet run --project src/Calculator.App
```

## Run tests

```powershell
dotnet test
```

There are 41 xUnit tests. They cover every operation, divide-by-zero,
rate validation, rounding, invalid and non-finite input, culture
handling, and the full menu flow.

## Example session

```text
Choose an option: 9
Enter the amount in GHS: 100
Enter the exchange rate (GHS for 1 USD): 0
The rate must be greater than zero.
Enter the exchange rate (GHS for 1 USD): 12.5
Result: 8.00 USD
```

## Project structure

```text
calculator/
├── Calculator.slnx
├── src/Calculator.App/
│   ├── Program.cs        Entry point: wires the console to ConsoleApp
│   ├── ConsoleApp.cs     Menu loop, input prompts, validation, output
│   └── Operations.cs     Pure calculation and conversion rules
└── tests/Calculator.Tests/
    ├── OperationsTests.cs   Unit tests for each rule
    └── ConsoleAppTests.cs   End-to-end tests with scripted input
```

## Design

The app is split into two layers:

- **`Operations`** is a static class of pure functions. It does no I/O, so
  each rule is tested directly. Invalid arguments (divide by zero, rate ≤ 0)
  throw exceptions, so a caller can't quietly get a wrong answer.
- **`ConsoleApp`** handles user interaction. It takes a `TextReader` and a
  `TextWriter` instead of using `Console` directly. Tests drive it with a
  `StringReader` and check what it writes to a `StringWriter`, so the menu
  loop is tested without a real terminal.

`Program.cs` only connects `Console.In`/`Console.Out` to `ConsoleApp`.

Other decisions:

- **Money uses `decimal`, measurements use `double`.** `decimal` avoids
  binary rounding errors for currency. The USD result is rounded to cents,
  with midpoint values rounded away from zero.
- **Input is parsed with `CultureInfo.InvariantCulture`.** `12.5` means
  twelve and a half on every machine, including ones set to a locale that
  uses a comma as the decimal separator.
- **Invalid input never crashes the app.** Non-numbers, `NaN` and
  `Infinity` are rejected, and the user is asked again. Results that
  overflow are reported instead of printed as `∞`.
- **Closed input exits cleanly.** If input ends (for example Ctrl+Z or
  piped input running out), the app returns normally instead of calling
  `Environment.Exit`. This keeps the code testable.

## Assumptions

- The user enters the exchange rate as **cedis per one US dollar**
  (e.g. `12.5`), because rates are usually quoted that way in Ghana.
- Numbers use `.` as the decimal separator. Thousands separators are
  accepted for the GHS amount but not for other numbers.
- Leading and trailing spaces in a menu choice are ignored.
- Results other than currency are shown to at most 4 decimal places.

## Known limitations

- The exchange rate is not fetched live. The user must supply it.
- Only GHS → USD is supported, not the reverse or other currencies.
- Each operation takes exactly two operands. There is no expression parsing
  (e.g. `2 + 3 * 4`) and no memory or history.
- Very large or very small values follow `double` precision limits
  (about 15–17 significant digits).
- The `°` symbol needs a UTF-8-capable terminal. `Program.cs` sets UTF-8
  output, but very old Windows consoles may still show it incorrectly.
