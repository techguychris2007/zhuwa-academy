namespace GradeTracker;

public record Student(string Name, double Score);

public static class GradeCalculator
{
    public static string GetGrade(double score) => score switch
    {
        >= 90 => "A",
        >= 80 => "B",
        >= 70 => "C",
        >= 60 => "D",
        _     => "F"
    };

    public static double Average(IEnumerable<Student> students) =>
        students.Average(s => s.Score);

    public static double Highest(IEnumerable<Student> students) =>
        students.Max(s => s.Score);

    public static double Lowest(IEnumerable<Student> students) =>
        students.Min(s => s.Score);
}
