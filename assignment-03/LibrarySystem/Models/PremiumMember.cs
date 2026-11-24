namespace LibrarySystem.Models;

public class PremiumMember : Member
{
    public override int    MaxBooks   => 6;
    public override int    LoanDays   => 30;
    public override string MemberType => "Premium";

    public PremiumMember(int id, string name, string email) : base(id, name, email) { }
}
