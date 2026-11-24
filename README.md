# Zhuwa Academy

Assignments for the Zhuwa Software Engineering Training Program.
Each assignment lives in its own folder with a README covering setup, tests,
design decisions, assumptions and limitations.

All projects target **.NET 8** (the installed SDK on this machine).

| Assignment | Folder | Description |
|---|---|---|
| Calculator | [calculator/](calculator/) | Console calculator and unit converter (km/miles, °C/°F, GHS→USD) |
| Susu tracker | [susu-tracker/](susu-tracker/) | Command-line tracker for a rotating savings (susu) group |
| Assignment 02 | [assignment-02/](assignment-02/) | Guessing game and grade tracker — arrays, loops, methods |
| Assignment 03 | [assignment-03/](assignment-03/) | Library management system — OOP with inheritance and polymorphism |

## Quick start

```bash
# Calculator
dotnet run --project calculator/src/Calculator.App

# Guessing Game
dotnet run --project assignment-02/GuessingGame

# Grade Tracker
dotnet run --project assignment-02/GradeTracker

# Library System
dotnet run --project assignment-03/LibrarySystem
```

## Running tests

```bash
# Assignment 02
dotnet test assignment-02/ConsoleApps.sln

# Assignment 03
dotnet test assignment-03/LibrarySystem.sln

# Calculator
dotnet test calculator/tests/Calculator.Tests
```
