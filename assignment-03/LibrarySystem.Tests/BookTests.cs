using Xunit;
using LibrarySystem.Models;

public class BookTests
{
    private static Book MakeBook() => new(1, "Test Book", "Test Author", "ISBN-001");

    [Fact]
    public void New_book_is_available()
    {
        Assert.True(MakeBook().IsAvailable);
    }

    [Fact]
    public void CheckOut_marks_book_unavailable()
    {
        var book = MakeBook();
        book.CheckOut();
        Assert.False(book.IsAvailable);
    }

    [Fact]
    public void CheckOut_twice_throws()
    {
        var book = MakeBook();
        book.CheckOut();
        Assert.Throws<InvalidOperationException>(() => book.CheckOut());
    }

    [Fact]
    public void Return_makes_book_available_again()
    {
        var book = MakeBook();
        book.CheckOut();
        book.Return();
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void Return_without_checkout_throws()
    {
        Assert.Throws<InvalidOperationException>(() => MakeBook().Return());
    }

    [Theory]
    [InlineData("", "Author", "ISBN")]
    [InlineData("Title", "", "ISBN")]
    [InlineData("Title", "Author", "")]
    public void Blank_fields_throw(string title, string author, string isbn)
    {
        Assert.Throws<ArgumentException>(() => new Book(1, title, author, isbn));
    }
}
