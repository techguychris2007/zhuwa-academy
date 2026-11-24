using GradeTracker;

Console.WriteLine("=== Grade Tracker ===");

const int MaxStudents = 100;
var students = new Student[MaxStudents];
int studentCount = 0;
bool running = true;

while (running)
{
    ShowMenu();
    running = HandleChoice(students, ref studentCount);
}

Console.WriteLine("Goodbye!");

static void ShowMenu()
{
    Console.WriteLine("\n--- Menu ---");
    Console.WriteLine("1. Add student score");
    Console.WriteLine("2. View all scores");
    Console.WriteLine("3. View statistics");
    Console.WriteLine("4. Quit");
    Console.Write("Choice: ");
}

static bool HandleChoice(Student[] students, ref int count) =>
    Console.ReadLine()?.Trim() switch
    {
        "1" => AddStudent(students, ref count),
        "2" => ViewScores(students, count),
        "3" => ViewStats(students, count),
        "4" => false,
        _   => InvalidOption()
    };

static bool AddStudent(Student[] students, ref int count)
{
    if (count >= students.Length) { Console.WriteLine("Roster is full (100 students)."); return true; }

    string name;
    while (true)
    {
        Console.Write("Student name: ");
        name = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(name)) break;
        Console.WriteLine("Name cannot be blank — try again.");
    }

    double score;
    while (true)
    {
        Console.Write("Score (0-100): ");
        if (double.TryParse(Console.ReadLine(), out score) && score is >= 0 and <= 100) break;
        Console.WriteLine("Enter a number between 0 and 100.");
    }

    students[count++] = new Student(name, score);
    Console.WriteLine($"Added {name}: {score:F1}  Grade: {GradeCalculator.GetGrade(score)}");
    return true;
}

static bool ViewScores(Student[] students, int count)
{
    if (count == 0) { Console.WriteLine("No students recorded yet."); return true; }

    Console.WriteLine($"\n{"Name",-20} {"Score",8} {"Grade",6}");
    Console.WriteLine(new string('-', 36));
    for (int i = 0; i < count; i++)
        Console.WriteLine($"{students[i].Name,-20} {students[i].Score,8:F1} {GradeCalculator.GetGrade(students[i].Score),6}");
    return true;
}

static bool ViewStats(Student[] students, int count)
{
    if (count == 0) { Console.WriteLine("No students recorded yet."); return true; }

    Console.WriteLine($"\nCount:    {count}");
    Console.WriteLine($"Average:  {GradeCalculator.Average(students, count):F2}");
    Console.WriteLine($"Highest:  {GradeCalculator.Highest(students, count):F1}");
    Console.WriteLine($"Lowest:   {GradeCalculator.Lowest(students, count):F1}");
    return true;
}

static bool InvalidOption()
{
    Console.WriteLine("Invalid option — enter 1, 2, 3, or 4.");
    return true;
}
