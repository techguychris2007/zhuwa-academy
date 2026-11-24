using LibrarySystem.Models;

namespace LibrarySystem.Services;

public class Library
{
    private readonly List<Book>            _books   = new();
    private readonly List<Member>          _members = new();
    private readonly List<BorrowingRecord> _records = new();
    private int _nextBookId   = 1;
    private int _nextMemberId = 1;
    private int _nextRecordId = 1;

    // ── Books ────────────────────────────────────────────────────────────────

    public Book AddBook(string title, string author, string isbn)
    {
        if (_books.Exists(b => b.ISBN == isbn.Trim()))
            throw new ArgumentException($"A book with ISBN {isbn} already exists.");
        var book = new Book(_nextBookId++, title, author, isbn);
        _books.Add(book);
        return book;
    }

    public IReadOnlyList<Book> GetAllBooks() => _books.AsReadOnly();

    public IReadOnlyList<Book> SearchBooks(string query)
    {
        string q = query.ToLower();
        return _books
            .Where(b => b.Title.ToLower().Contains(q)
                     || b.Author.ToLower().Contains(q)
                     || b.ISBN.Contains(q))
            .ToList()
            .AsReadOnly();
    }

    public Book? FindBook(int id) => _books.Find(b => b.Id == id);

    // ── Members ──────────────────────────────────────────────────────────────

    public Member AddMember(string name, string email, bool isPremium)
    {
        if (_members.Exists(m => m.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"A member with email {email} already exists.");
        Member member = isPremium
            ? new PremiumMember(_nextMemberId++, name, email)
            : new RegularMember(_nextMemberId++, name, email);
        _members.Add(member);
        return member;
    }

    public IReadOnlyList<Member> GetAllMembers() => _members.AsReadOnly();

    public Member? FindMember(int id) => _members.Find(m => m.Id == id);

    // ── Borrowing & Returns ──────────────────────────────────────────────────

    public BorrowingRecord BorrowBook(int memberId, int bookId)
    {
        var member = FindMember(memberId)
            ?? throw new ArgumentException($"No member with ID {memberId}.");
        var book = FindBook(bookId)
            ?? throw new ArgumentException($"No book with ID {bookId}.");

        if (!book.IsAvailable)
            throw new InvalidOperationException($"\"{book.Title}\" is currently on loan.");

        member.IncrementLoans();   // throws if the member is at their limit
        book.CheckOut();

        var record = new BorrowingRecord(_nextRecordId++, book, member, DateTime.Today);
        _records.Add(record);
        return record;
    }

    public BorrowingRecord ReturnBook(int recordId)
    {
        var record = _records.Find(r => r.Id == recordId)
            ?? throw new ArgumentException($"No borrowing record with ID {recordId}.");

        if (record.IsReturned)
            throw new InvalidOperationException("This book has already been returned.");

        record.RecordReturn(DateTime.Today);
        record.Book.Return();
        record.Member.DecrementLoans();
        return record;
    }

    // ── Queries ──────────────────────────────────────────────────────────────

    public IReadOnlyList<BorrowingRecord> GetAllRecords()   => _records.AsReadOnly();
    public IReadOnlyList<BorrowingRecord> GetActiveLoans()  => _records.Where(r => !r.IsReturned).ToList().AsReadOnly();
    public IReadOnlyList<BorrowingRecord> GetOverdueLoans() => _records.Where(r => r.IsOverdue).ToList().AsReadOnly();

    public IReadOnlyList<BorrowingRecord> GetMemberHistory(int memberId) =>
        _records.Where(r => r.Member.Id == memberId).ToList().AsReadOnly();
}
