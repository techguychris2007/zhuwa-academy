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

    public static double Average(Student[] students, int count)
    {
        double sum = 0;
        for (int i = 0; i < count; i++) sum += students[i].Score;
        return sum / count;
    }

    public static double Highest(Student[] students, int count)
    {
        double max = students[0].Score;
        for (int i = 1; i < count; i++)
            if (students[i].Score > max) max = students[i].Score;
        return max;
    }

    public static double Lowest(Student[] students, int count)
    {
        double min = students[0].Score;
        for (int i = 1; i < count; i++)
            if (students[i].Score < min) min = students[i].Score;
        return min;
    }
}
