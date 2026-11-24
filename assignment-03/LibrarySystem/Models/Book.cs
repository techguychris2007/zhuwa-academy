namespace LibrarySystem.Models;

public class Book
{
    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public string ISBN { get; }
    public bool IsAvailable { get; private set; } = true;

    public Book(int id, string title, string author, string isbn)
    {
        if (string.IsNullOrWhiteSpace(title))  throw new ArgumentException("Title is required.",  nameof(title));
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author is required.", nameof(author));
        if (string.IsNullOrWhiteSpace(isbn))   throw new ArgumentException("ISBN is required.",   nameof(isbn));
        Id     = id;
        Title  = title.Trim();
        Author = author.Trim();
        ISBN   = isbn.Trim();
    }

    public void CheckOut()
    {
        if (!IsAvailable) throw new InvalidOperationException($"\"{Title}\" is already on loan.");
        IsAvailable = false;
    }

    public void Return()
    {
        if (IsAvailable) throw new InvalidOperationException($"\"{Title}\" is not currently on loan.");
        IsAvailable = true;
    }

    public override string ToString() =>
        $"[{Id}] \"{Title}\" by {Author} (ISBN: {ISBN}) — {(IsAvailable ? "Available" : "On loan")}";
}
