using Xunit;
using GradeTracker;

public class GradeCalculatorTests
{
    private static List<Student> Sample() =>
    [
        new("Alice",   95),
        new("Bob",     72),
        new("Charlie", 55),
    ];

    [Theory]
    [InlineData(100, "A")]
    [InlineData(90,  "A")]
    [InlineData(89,  "B")]
    [InlineData(80,  "B")]
    [InlineData(79,  "C")]
    [InlineData(70,  "C")]
    [InlineData(69,  "D")]
    [InlineData(60,  "D")]
    [InlineData(59,  "F")]
    [InlineData(0,   "F")]
    public void GetGrade_returns_correct_letter(double score, string expected) =>
        Assert.Equal(expected, GradeCalculator.GetGrade(score));

    [Fact]
    public void Average_calculates_correctly()
    {
        double expected = (95 + 72 + 55) / 3.0;
        Assert.Equal(expected, GradeCalculator.Average(Sample()), 5);
    }

    [Fact]
    public void Highest_returns_max_score()
    {
        Assert.Equal(95, GradeCalculator.Highest(Sample()));
    }

    [Fact]
    public void Lowest_returns_min_score()
    {
        Assert.Equal(55, GradeCalculator.Lowest(Sample()));
    }

    [Fact]
    public void Single_student_average_equals_their_score()
    {
        var one = new List<Student> { new("Dana", 88) };
        Assert.Equal(88, GradeCalculator.Average(one));
    }
}
