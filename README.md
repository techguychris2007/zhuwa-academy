# Zhuwa Academy

Assignments for the Zhuwa Software Engineering Training Program.
Each project is in its own folder with a README covering setup, tests,
design, assumptions and limitations.

| Project | Language | Description |
|---|---|---|
| [calculator](calculator/) | C# / .NET 10 | Console calculator and unit converter (km/miles, °C/°F, GHS→USD) |
| [susu-tracker](susu-tracker/) | Python 3.10+ | Command-line tracker for a rotating savings (susu) group |

## Quick start

```powershell
# Calculator
cd calculator
dotnet run --project src/Calculator.App
dotnet test

# Susu tracker
cd susu-tracker
python cli.py
python -m unittest -v
```
