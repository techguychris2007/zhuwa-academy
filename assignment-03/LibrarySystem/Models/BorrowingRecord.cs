namespace LibrarySystem.Models;

public class BorrowingRecord
{
    public int      Id         { get; }
    public Book     Book       { get; }
    public Member   Member     { get; }
    public DateTime BorrowDate { get; }
    public DateTime DueDate    { get; }
    public DateTime? ReturnDate { get; private set; }

    public bool IsReturned => ReturnDate.HasValue;
    public bool IsOverdue  => !IsReturned && DateTime.Today > DueDate;

    public BorrowingRecord(int id, Book book, Member member, DateTime borrowDate)
    {
        Id         = id;
        Book       = book;
        Member     = member;
        BorrowDate = borrowDate;
        DueDate    = borrowDate.AddDays(member.LoanDays);
    }

    public void RecordReturn(DateTime returnDate)
    {
        if (IsReturned) throw new InvalidOperationException("This book has already been returned.");
        ReturnDate = returnDate;
    }

    public override string ToString()
    {
        string status = IsReturned
            ? $"Returned {ReturnDate:yyyy-MM-dd}"
            : IsOverdue
                ? $"OVERDUE (due {DueDate:yyyy-MM-dd})"
                : $"Due {DueDate:yyyy-MM-dd}";
        return $"[{Id}] \"{Book.Title}\" → {Member.Name} | Borrowed {BorrowDate:yyyy-MM-dd} | {status}";
    }
}
