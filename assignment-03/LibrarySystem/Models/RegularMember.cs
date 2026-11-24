namespace LibrarySystem.Models;

public class RegularMember : Member
{
    public override int    MaxBooks   => 3;
    public override int    LoanDays   => 14;
    public override string MemberType => "Regular";

    public RegularMember(int id, string name, string email) : base(id, name, email) { }
}
