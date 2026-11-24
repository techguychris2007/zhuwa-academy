using GuessingGame;

Console.WriteLine("=== Number Guessing Game ===");

bool playAgain = true;
while (playAgain)
{
    RunGame();
    playAgain = AskPlayAgain();
}
Console.WriteLine("Thanks for playing!");

static void RunGame()
{
    int secret = new Random().Next(1, 101);
    var game = new Game(secret, maxTries: 7);

    Console.WriteLine($"\nI'm thinking of a number between 1 and 100. You have {game.MaxTries} tries.");

    while (!game.IsOver)
    {
        int triesLeft = game.MaxTries - game.TriesUsed;
        int guess = PromptGuess(triesLeft);
        GuessResult result = game.MakeGuess(guess);
        DisplayResult(result, game);
    }

    if (!game.IsWon)
        Console.WriteLine($"Game over! The number was {game.SecretNumber}.");
}

static int PromptGuess(int triesLeft)
{
    while (true)
    {
        Console.Write($"Guess ({triesLeft} {(triesLeft == 1 ? "try" : "tries")} left): ");
        if (int.TryParse(Console.ReadLine(), out int guess) && guess is >= 1 and <= 100)
            return guess;
        Console.WriteLine("Enter a whole number between 1 and 100.");
    }
}

static void DisplayResult(GuessResult result, Game game)
{
    switch (result)
    {
        case GuessResult.Correct:
            Console.WriteLine($"Correct! You got it in {game.TriesUsed} {(game.TriesUsed == 1 ? "try" : "tries")}!");
            break;
        case GuessResult.TooLow:
            Console.WriteLine("Too low — try higher.");
            break;
        case GuessResult.TooHigh:
            Console.WriteLine("Too high — try lower.");
            break;
    }
}

static bool AskPlayAgain()
{
    Console.Write("\nPlay again? (y/n): ");
    return Console.ReadLine()?.Trim().ToLower() == "y";
}
