# Assignment 02 — Console Apps (C# / .NET 8)

Two small C# console applications practising arrays, loops, methods, and basic OOP.

## Projects

| Project | Description |
|---|---|
| `GuessingGame` | Random-number guessing game with 7 tries |
| `GradeTracker` | Student grade tracker with a repeating menu |
| `GuessingGame.Tests` | xUnit unit tests for game logic |
| `GradeTracker.Tests` | xUnit unit tests for grade calculation |

## Setup

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
cd assignment-02
dotnet build
```

## Running the apps

```bash
dotnet run --project GuessingGame
dotnet run --project GradeTracker
```

## Running the tests

```bash
dotnet test ConsoleApps.sln
```

All 20 tests pass.

## Architecture

Both apps separate console I/O from business logic:

```
Program.cs           — entry point, menu/game loop, all Console calls
Game.cs              — pure game state and rules (no console dependency)
GradeCalculator.cs   — pure grade/stats functions (no console dependency)
<App>.Tests/         — xUnit tests that target only the logic layer
```

Keeping I/O out of the logic layer means tests run without any console mocking.

### GuessingGame

- `Game` class holds secret number, tries used, win/loss state
- `MakeGuess(int)` returns a `GuessResult` enum: `Correct`, `TooLow`, or `TooHigh`
- `Program.cs` owns all prompting and display

### GradeTracker

- Scores are stored in a fixed-size `Student[]` array (capacity 100); a separate `int studentCount` tracks how many slots are filled — this is intentional, to practise working with arrays directly rather than using a resizing collection
- `GradeCalculator` is a static class with pure functions: `GetGrade`, `Average`, `Highest`, `Lowest`; each takes the array and the filled count as arguments
- Menu loops until the user chooses Quit (option 4)

## Assumptions & Known Limitations

- Scores are stored in memory only; they are lost when GradeTracker exits.
- The roster is capped at 100 students (fixed array size).
- The guessing-game range (1–100) and try limit (7) are hardcoded.
- Both apps target **.NET 8**, matching the rest of the repo (see root README).
