using GradeTracker;

Console.WriteLine("=== Grade Tracker ===");

var students = new List<Student>();
bool running = true;

while (running)
{
    ShowMenu();
    running = HandleChoice(students);
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

static bool HandleChoice(List<Student> students) =>
    Console.ReadLine()?.Trim() switch
    {
        "1" => AddStudent(students),
        "2" => ViewScores(students),
        "3" => ViewStats(students),
        "4" => false,
        _   => InvalidOption()
    };

static bool AddStudent(List<Student> students)
{
    Console.Write("Student name: ");
    string name = Console.ReadLine()?.Trim() ?? "";
    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return true;
    }

    double score;
    while (true)
    {
        Console.Write("Score (0-100): ");
        if (double.TryParse(Console.ReadLine(), out score) && score is >= 0 and <= 100) break;
        Console.WriteLine("Enter a number between 0 and 100.");
    }

    students.Add(new Student(name, score));
    Console.WriteLine($"Added {name}: {score:F1}  Grade: {GradeCalculator.GetGrade(score)}");
    return true;
}

static bool ViewScores(List<Student> students)
{
    if (students.Count == 0) { Console.WriteLine("No students recorded yet."); return true; }

    Console.WriteLine($"\n{"Name",-20} {"Score",8} {"Grade",6}");
    Console.WriteLine(new string('-', 36));
    foreach (var s in students)
        Console.WriteLine($"{s.Name,-20} {s.Score,8:F1} {GradeCalculator.GetGrade(s.Score),6}");
    return true;
}

static bool ViewStats(List<Student> students)
{
    if (students.Count == 0) { Console.WriteLine("No students recorded yet."); return true; }

    Console.WriteLine($"\nCount:    {students.Count}");
    Console.WriteLine($"Average:  {GradeCalculator.Average(students):F2}");
    Console.WriteLine($"Highest:  {GradeCalculator.Highest(students):F1}");
    Console.WriteLine($"Lowest:   {GradeCalculator.Lowest(students):F1}");
    return true;
}

static bool InvalidOption()
{
    Console.WriteLine("Invalid option — enter 1, 2, 3, or 4.");
    return true;
}
