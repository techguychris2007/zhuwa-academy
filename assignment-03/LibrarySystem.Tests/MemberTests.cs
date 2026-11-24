using Xunit;
using LibrarySystem.Models;

public class MemberTests
{
    [Fact]
    public void Regular_member_has_14_day_loan_and_3_book_limit()
    {
        var m = new RegularMember(1, "Alice", "a@a.com");
        Assert.Equal(14, m.LoanDays);
        Assert.Equal(3,  m.MaxBooks);
    }

    [Fact]
    public void Premium_member_has_30_day_loan_and_6_book_limit()
    {
        var m = new PremiumMember(1, "Bob", "b@b.com");
        Assert.Equal(30, m.LoanDays);
        Assert.Equal(6,  m.MaxBooks);
    }

    [Fact]
    public void IncrementLoans_increases_active_count()
    {
        var m = new RegularMember(1, "Alice", "a@a.com");
        m.IncrementLoans();
        Assert.Equal(1, m.ActiveLoans);
    }

    [Fact]
    public void IncrementLoans_at_limit_throws()
    {
        var m = new RegularMember(1, "Alice", "a@a.com");
        m.IncrementLoans();
        m.IncrementLoans();
        m.IncrementLoans();
        Assert.Throws<InvalidOperationException>(() => m.IncrementLoans());
    }

    [Fact]
    public void DecrementLoans_decreases_active_count()
    {
        var m = new RegularMember(1, "Alice", "a@a.com");
        m.IncrementLoans();
        m.DecrementLoans();
        Assert.Equal(0, m.ActiveLoans);
    }

    [Fact]
    public void DecrementLoans_below_zero_throws()
    {
        var m = new RegularMember(1, "Alice", "a@a.com");
        Assert.Throws<InvalidOperationException>(() => m.DecrementLoans());
    }
}
