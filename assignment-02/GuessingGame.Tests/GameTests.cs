using Xunit;
using GuessingGame;

public class GameTests
{
    [Fact]
    public void Correct_guess_wins_the_game()
    {
        var game = new Game(42, maxTries: 5);
        var result = game.MakeGuess(42);
        Assert.Equal(GuessResult.Correct, result);
        Assert.True(game.IsWon);
        Assert.True(game.IsOver);
    }

    [Fact]
    public void Low_guess_returns_TooLow()
    {
        var game = new Game(50);
        Assert.Equal(GuessResult.TooLow, game.MakeGuess(30));
        Assert.False(game.IsWon);
    }

    [Fact]
    public void High_guess_returns_TooHigh()
    {
        var game = new Game(50);
        Assert.Equal(GuessResult.TooHigh, game.MakeGuess(70));
        Assert.False(game.IsWon);
    }

    [Fact]
    public void Game_is_over_after_max_tries_without_a_win()
    {
        var game = new Game(50, maxTries: 3);
        game.MakeGuess(1);
        game.MakeGuess(2);
        game.MakeGuess(3);
        Assert.True(game.IsOver);
        Assert.False(game.IsWon);
    }

    [Fact]
    public void Guessing_after_game_over_throws()
    {
        var game = new Game(50, maxTries: 1);
        game.MakeGuess(1);
        Assert.Throws<InvalidOperationException>(() => game.MakeGuess(2));
    }

    [Fact]
    public void TriesUsed_increments_on_each_guess()
    {
        var game = new Game(50, maxTries: 5);
        game.MakeGuess(10);
        game.MakeGuess(20);
        Assert.Equal(2, game.TriesUsed);
    }
}
