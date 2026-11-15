# Zhuwa Academy

Assignments for the Zhuwa Software Engineering Training Program.
Each project is in its own folder with a README covering setup, tests,
design, assumptions and limitations.

| Project | Language | Description |
|---|---|---|
| [calculator](calculator/) | C# / .NET 10 | Console calculator and unit converter (km/miles, °C/°F, GHS→USD) |
| [susu-tracker](susu-tracker/) | Python 3.10+ | Command-line tracker for a rotating savings (susu) group |
| [GuessingGame](GuessingGame/) | C# / .NET 8 | Random-number guessing game with limited tries |
| [GradeTracker](GradeTracker/) | C# / .NET 8 | Student grade tracker with repeating menu and statistics |

## Quick start

```bash
# Guessing Game
cd GuessingGame
dotnet run

# Grade Tracker
cd GradeTracker
dotnet run

# Run all tests
cd ..
dotnet test ConsoleApps.sln
```

---

## GuessingGame

- Guess a random number between 1–100 in **7 tries**
- Each wrong guess gives a "too high" / "too low" hint
- Play-again prompt after each round
- Methods: `RunGame`, `PromptGuess`, `DisplayResult`, `AskPlayAgain`
- Testable logic in `Game.cs` (no console dependency)

## GradeTracker

- Stores student names and scores in a `List<Student>` (in-memory)
- Menu loops until the user quits (option 4)
- Options: Add student | View all scores | View statistics | Quit
- Statistics: count, average, highest, lowest score
- Grade scale: A ≥ 90 · B ≥ 80 · C ≥ 70 · D ≥ 60 · F < 60
- Testable logic in `GradeCalculator.cs` (no console dependency)

### Architecture

```
Program.cs          — entry point, menu/game loop, console I/O
<Logic>.cs          — pure business logic, no console calls (testable)
<App>.Tests/        — xUnit tests targeting only the logic class
```

### Assumptions & Known Limitations

- Scores are stored in memory only; they are lost when the app exits.
- The guessing game range is fixed at 1–100 with 7 tries.
- Input parsing uses the current culture's decimal separator.
