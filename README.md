# Console Apps — C#

Two small C# 8 console applications built for a coursework submission.

---

## Projects

| Project | Description |
|---|---|
| `GuessingGame` | Random-number guessing game with limited tries |
| `GradeTracker` | Student grade tracker with a repeating menu |
| `GuessingGame.Tests` | xUnit tests for game logic |
| `GradeTracker.Tests` | xUnit tests for grade calculation |

---

## Setup

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
git clone https://github.com/techguychris2007/zhuwa-academy.git
cd zhuwa-academy
dotnet build
```

---

## Running the Apps

```bash
# Guessing Game
dotnet run --project GuessingGame

# Grade Tracker
dotnet run --project GradeTracker
```

---

## Running the Tests

```bash
dotnet test
```

Expected output: **15 tests passing** (6 for GuessingGame, 9 for GradeTracker — 10 grade-boundary cases + 4 stat tests + 1 edge case).

---

## App Details

### Guessing Game

- Picks a random number between 1 and 100
- Player has **7 tries** to guess it
- Each wrong guess gives a "too high" or "too low" hint
- Play-again prompt after each round
- Methods: `RunGame`, `PromptGuess`, `DisplayResult`, `AskPlayAgain`
- Logic lives in `Game.cs` (testable, no console dependency)

### Grade Tracker

- Stores student names and scores in a `List<Student>` (in-memory array-backed collection)
- Menu loops until the user chooses Quit (option 4)
- Options: Add student | View all scores | View statistics | Quit
- Statistics shown: count, average, highest, lowest
- Grade scale: A ≥ 90 · B ≥ 80 · C ≥ 70 · D ≥ 60 · F < 60
- Pure logic lives in `GradeCalculator.cs` (testable, no console dependency)

---

## Architecture

Both apps follow the same pattern:

```
Program.cs          — entry point, menu/game loop, console I/O
<Logic>.cs          — pure business logic with no console calls (testable)
<App>.Tests/        — xUnit tests that target only the logic class
```

Keeping I/O out of the logic classes lets the tests run without mocking the console.

---

## Assumptions & Known Limitations

- Scores are stored in memory only; they are lost when the app exits.
- The guessing game's secret number range is fixed at 1–100 with 7 tries.
- Input parsing uses the current culture's decimal separator (e.g. `.` on en-US).
- No persistence layer — a future improvement could serialize to JSON on exit.
