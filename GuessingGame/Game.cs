namespace GuessingGame;

public enum GuessResult { Correct, TooLow, TooHigh }

public class Game
{
    public int SecretNumber { get; }
    public int MaxTries { get; }
    public int TriesUsed { get; private set; }
    public bool IsWon { get; private set; }
    public bool IsOver => IsWon || TriesUsed >= MaxTries;

    public Game(int secretNumber, int maxTries = 7)
    {
        if (maxTries < 1) throw new ArgumentOutOfRangeException(nameof(maxTries));
        SecretNumber = secretNumber;
        MaxTries = maxTries;
    }

    public GuessResult MakeGuess(int guess)
    {
        if (IsOver) throw new InvalidOperationException("Game is already over.");
        TriesUsed++;
        if (guess == SecretNumber) { IsWon = true; return GuessResult.Correct; }
        return guess < SecretNumber ? GuessResult.TooLow : GuessResult.TooHigh;
    }
}
