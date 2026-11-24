using Xunit;
using LibrarySystem.Models;
using LibrarySystem.Services;

public class LibraryTests
{
    private static Library MakeLibrary()
    {
        var lib = new Library();
        lib.AddBook("Book A", "Author A", "ISBN-001");
        lib.AddBook("Book B", "Author B", "ISBN-002");
        lib.AddMember("Alice", "alice@test.com", false);  // Regular: 3 books, 14 days
        lib.AddMember("Bob",   "bob@test.com",   true);   // Premium: 6 books, 30 days
        return lib;
    }

    // ── AddBook ───────────────────────────────────────────────────────────────

    [Fact]
    public void AddBook_stores_book()
    {
        var lib = new Library();
        var book = lib.AddBook("Test", "Author", "ISBN-X");
        Assert.Single(lib.GetAllBooks());
        Assert.Equal("Test", book.Title);
    }

    [Fact]
    public void AddBook_rejects_duplicate_ISBN()
    {
        var lib = new Library();
        lib.AddBook("A", "Author", "ISBN-1");
        Assert.Throws<ArgumentException>(() => lib.AddBook("B", "Author", "ISBN-1"));
    }

    // ── AddMember ─────────────────────────────────────────────────────────────

    [Fact]
    public void AddMember_rejects_duplicate_email()
    {
        var lib = new Library();
        lib.AddMember("Alice", "a@a.com", false);
        Assert.Throws<ArgumentException>(() => lib.AddMember("Alice2", "a@a.com", false));
    }

    // ── BorrowBook ────────────────────────────────────────────────────────────

    [Fact]
    public void BorrowBook_marks_book_unavailable()
    {
        var lib = MakeLibrary();
        lib.BorrowBook(memberId: 1, bookId: 1);
        Assert.False(lib.FindBook(1)!.IsAvailable);
    }

    [Fact]
    public void BorrowBook_on_unavailable_book_throws()
    {
        var lib = MakeLibrary();
        lib.BorrowBook(1, 1);
        Assert.Throws<InvalidOperationException>(() => lib.BorrowBook(2, 1));
    }

    [Fact]
    public void BorrowBook_beyond_regular_limit_throws()
    {
        var lib = new Library();
        lib.AddBook("B1", "A", "i1");
        lib.AddBook("B2", "A", "i2");
        lib.AddBook("B3", "A", "i3");
        lib.AddBook("B4", "A", "i4");
        lib.AddMember("Alice", "a@a.com", false);  // limit 3
        lib.BorrowBook(1, 1);
        lib.BorrowBook(1, 2);
        lib.BorrowBook(1, 3);
        Assert.Throws<InvalidOperationException>(() => lib.BorrowBook(1, 4));
    }

    [Fact]
    public void Regular_member_loan_is_14_days()
    {
        var lib    = MakeLibrary();
        var record = lib.BorrowBook(memberId: 1, bookId: 1);
        Assert.Equal(DateTime.Today.AddDays(14), record.DueDate);
    }

    [Fact]
    public void Premium_member_loan_is_30_days()
    {
        var lib    = MakeLibrary();
        var record = lib.BorrowBook(memberId: 2, bookId: 1);
        Assert.Equal(DateTime.Today.AddDays(30), record.DueDate);
    }

    [Fact]
    public void BorrowBook_with_invalid_member_throws()
    {
        var lib = MakeLibrary();
        Assert.Throws<ArgumentException>(() => lib.BorrowBook(999, 1));
    }

    [Fact]
    public void BorrowBook_with_invalid_book_throws()
    {
        var lib = MakeLibrary();
        Assert.Throws<ArgumentException>(() => lib.BorrowBook(1, 999));
    }

    // ── ReturnBook ────────────────────────────────────────────────────────────

    [Fact]
    public void ReturnBook_makes_book_available_again()
    {
        var lib    = MakeLibrary();
        var record = lib.BorrowBook(1, 1);
        lib.ReturnBook(record.Id);
        Assert.True(lib.FindBook(1)!.IsAvailable);
    }

    [Fact]
    public void ReturnBook_decrements_member_active_loans()
    {
        var lib    = MakeLibrary();
        var record = lib.BorrowBook(1, 1);
        lib.ReturnBook(record.Id);
        Assert.Equal(0, lib.FindMember(1)!.ActiveLoans);
    }

    [Fact]
    public void ReturnBook_twice_throws()
    {
        var lib    = MakeLibrary();
        var record = lib.BorrowBook(1, 1);
        lib.ReturnBook(record.Id);
        Assert.Throws<InvalidOperationException>(() => lib.ReturnBook(record.Id));
    }

    // ── Queries ───────────────────────────────────────────────────────────────

    [Fact]
    public void SearchBooks_finds_by_title()
    {
        var lib     = MakeLibrary();
        var results = lib.SearchBooks("Book A");
        Assert.Single(results);
        Assert.Equal("Book A", results[0].Title);
    }

    [Fact]
    public void GetActiveLoans_excludes_returned()
    {
        var lib    = MakeLibrary();
        var record = lib.BorrowBook(1, 1);
        lib.BorrowBook(1, 2);
        lib.ReturnBook(record.Id);
        Assert.Single(lib.GetActiveLoans());
    }

    [Fact]
    public void GetMemberHistory_returns_all_records_for_member()
    {
        var lib = MakeLibrary();
        lib.BorrowBook(1, 1);
        lib.BorrowBook(2, 2);
        Assert.Single(lib.GetMemberHistory(1));
    }
}
