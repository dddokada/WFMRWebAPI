#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample
{
  public sealed class Name
  {
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string FirstName { get; set; }
    public string NickName { get; set; }
    private string _Suffix;
    public string LoginName { get; set; }

    public string Suffix
    {
      get { return string.IsNullOrWhiteSpace(_Suffix) ? string.Empty : _Suffix.Trim(); }
      set { _Suffix = value; }
    }

    public Name()
    {
    }

    public Name(string firstName, string middleName, string lastName, string suffix, string nickName, string loginName)
    {
      LastName = lastName;
      FirstName = firstName;
      MiddleName = middleName;
      NickName = nickName;
      Suffix = suffix;
      LoginName = loginName;
    }

    public string GetFormattedName()
    {
      return string.Format("{0}, {1}", LastName, string.IsNullOrWhiteSpace(NickName) ? FirstName : NickName);
    }

    public string GetDisplayName()
    {
      return string.Format("{0} {1}", string.IsNullOrWhiteSpace(NickName) ? FirstName : NickName, LastName);
    }

    public override string ToString()
    {
      return string.Format("{0}, {1} {2} ({3})", LastName, FirstName, MiddleName, LoginName);
    }
  }
}
