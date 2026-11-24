using LibrarySystem.Services;

var library = new Library();
SeedData(library);

bool running = true;
while (running)
{
    ShowMenu();
    running = HandleChoice(library);
}
Console.WriteLine("Goodbye!");

// ── Menu ─────────────────────────────────────────────────────────────────────

static void ShowMenu()
{
    Console.WriteLine("\n=== Library System ===");
    Console.WriteLine(" 1. Add book");
    Console.WriteLine(" 2. Register member");
    Console.WriteLine(" 3. Borrow book");
    Console.WriteLine(" 4. Return book");
    Console.WriteLine(" 5. List all books");
    Console.WriteLine(" 6. Search books");
    Console.WriteLine(" 7. List all members");
    Console.WriteLine(" 8. Active loans");
    Console.WriteLine(" 9. Overdue loans");
    Console.WriteLine("10. Member history");
    Console.WriteLine(" 0. Exit");
    Console.Write("Choice: ");
}

static bool HandleChoice(Library library)
{
    try
    {
        return Console.ReadLine()?.Trim() switch
        {
            "1"  => AddBook(library),
            "2"  => RegisterMember(library),
            "3"  => BorrowBook(library),
            "4"  => ReturnBook(library),
            "5"  => ListBooks(library),
            "6"  => SearchBooks(library),
            "7"  => ListMembers(library),
            "8"  => ActiveLoans(library),
            "9"  => OverdueLoans(library),
            "10" => MemberHistory(library),
            "0"  => false,
            _    => InvalidOption()
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return true;
    }
}

// ── Operations ────────────────────────────────────────────────────────────────

static bool AddBook(Library library)
{
    string title  = Prompt("Title");
    string author = Prompt("Author");
    string isbn   = Prompt("ISBN");
    var book = library.AddBook(title, author, isbn);
    Console.WriteLine($"Added: {book}");
    return true;
}

static bool RegisterMember(Library library)
{
    string name  = Prompt("Name");
    string email = Prompt("Email");
    Console.Write("Type (1=Regular, 2=Premium): ");
    bool isPremium = Console.ReadLine()?.Trim() == "2";
    var member = library.AddMember(name, email, isPremium);
    Console.WriteLine($"Registered: {member}");
    return true;
}

static bool BorrowBook(Library library)
{
    int memberId = PromptInt("Member ID");
    int bookId   = PromptInt("Book ID");
    var record   = library.BorrowBook(memberId, bookId);
    Console.WriteLine($"Borrowed: {record}");
    return true;
}

static bool ReturnBook(Library library)
{
    int recordId = PromptInt("Borrowing record ID");
    var record   = library.ReturnBook(recordId);
    Console.WriteLine(record.IsOverdue ? $"Returned late: {record}" : $"Returned: {record}");
    return true;
}

static bool ListBooks(Library library)
{
    var books = library.GetAllBooks();
    if (books.Count == 0) { Console.WriteLine("No books."); return true; }
    foreach (var b in books) Console.WriteLine(b);
    return true;
}

static bool SearchBooks(Library library)
{
    string query = Prompt("Search (title / author / ISBN)");
    var results = library.SearchBooks(query);
    if (results.Count == 0) { Console.WriteLine("No matches."); return true; }
    foreach (var b in results) Console.WriteLine(b);
    return true;
}

static bool ListMembers(Library library)
{
    var members = library.GetAllMembers();
    if (members.Count == 0) { Console.WriteLine("No members."); return true; }
    foreach (var m in members) Console.WriteLine(m);
    return true;
}

static bool ActiveLoans(Library library)
{
    var loans = library.GetActiveLoans();
    if (loans.Count == 0) { Console.WriteLine("No active loans."); return true; }
    foreach (var r in loans) Console.WriteLine(r);
    return true;
}

static bool OverdueLoans(Library library)
{
    var overdue = library.GetOverdueLoans();
    if (overdue.Count == 0) { Console.WriteLine("No overdue loans."); return true; }
    foreach (var r in overdue) Console.WriteLine(r);
    return true;
}

static bool MemberHistory(Library library)
{
    int memberId = PromptInt("Member ID");
    var history  = library.GetMemberHistory(memberId);
    if (history.Count == 0) { Console.WriteLine("No records for that member."); return true; }
    foreach (var r in history) Console.WriteLine(r);
    return true;
}

static bool InvalidOption()
{
    Console.WriteLine("Enter a number from 0 to 10.");
    return true;
}

// ── Helpers ───────────────────────────────────────────────────────────────────

static string Prompt(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        string value = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(value)) return value;
        Console.WriteLine($"{label} cannot be blank.");
    }
}

static int PromptInt(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        if (int.TryParse(Console.ReadLine(), out int value) && value > 0) return value;
        Console.WriteLine("Enter a positive whole number.");
    }
}

// ── Seed data ─────────────────────────────────────────────────────────────────

static void SeedData(Library library)
{
    library.AddBook("The Great Gatsby",       "F. Scott Fitzgerald", "978-0-7432-7356-5");
    library.AddBook("To Kill a Mockingbird",  "Harper Lee",          "978-0-06-112008-4");
    library.AddBook("1984",                   "George Orwell",       "978-0-452-28423-4");
    library.AddBook("Things Fall Apart",      "Chinua Achebe",       "978-0-385-47454-2");
    library.AddBook("Brave New World",        "Aldous Huxley",       "978-0-06-085052-4");

    library.AddMember("Alice Johnson", "alice@example.com", false);
    library.AddMember("Bob Smith",     "bob@example.com",   true);
}
