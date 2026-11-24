namespace LibrarySystem.Models;

public abstract class Member
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public int ActiveLoans { get; private set; }

    public abstract int MaxBooks  { get; }
    public abstract int LoanDays  { get; }
    public abstract string MemberType { get; }

    protected Member(int id, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))  throw new ArgumentException("Name is required.",  nameof(name));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
        Id    = id;
        Name  = name.Trim();
        Email = email.Trim();
    }

    public void IncrementLoans()
    {
        if (ActiveLoans >= MaxBooks)
            throw new InvalidOperationException(
                $"{Name} has reached the {MemberType} limit of {MaxBooks} book(s).");
        ActiveLoans++;
    }

    public void DecrementLoans()
    {
        if (ActiveLoans <= 0) throw new InvalidOperationException("No active loans to decrement.");
        ActiveLoans--;
    }

    public override string ToString() =>
        $"[{Id}] {Name} ({MemberType}) — {Email} | {ActiveLoans}/{MaxBooks} active, {LoanDays}-day loan";
}
