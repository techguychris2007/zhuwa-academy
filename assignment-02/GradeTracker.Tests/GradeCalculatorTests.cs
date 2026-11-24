using Xunit;
using GradeTracker;

public class GradeCalculatorTests
{
    private static (Student[] arr, int count) Sample()
    {
        var arr = new Student[10];
        arr[0] = new("Alice",   95);
        arr[1] = new("Bob",     72);
        arr[2] = new("Charlie", 55);
        return (arr, 3);
    }

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
        var (arr, count) = Sample();
        double expected = (95 + 72 + 55) / 3.0;
        Assert.Equal(expected, GradeCalculator.Average(arr, count), 5);
    }

    [Fact]
    public void Highest_returns_max_score()
    {
        var (arr, count) = Sample();
        Assert.Equal(95, GradeCalculator.Highest(arr, count));
    }

    [Fact]
    public void Lowest_returns_min_score()
    {
        var (arr, count) = Sample();
        Assert.Equal(55, GradeCalculator.Lowest(arr, count));
    }

    [Fact]
    public void Single_student_average_equals_their_score()
    {
        var arr = new Student[1] { new("Dana", 88) };
        Assert.Equal(88, GradeCalculator.Average(arr, 1));
    }
}
