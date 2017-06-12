#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.EmployeeResources
{
  public class EmployeeStatusResource
  {
    public int ID { get; set; }
    public int EmployeeID { get; set; }

    public int HomeSiteID { get; set; }
    public bool IsActive { get; set; }

    public string EmployeeStatusCode { get; set; }

    public string Start { get; set; }
    public string End { get; set; }

  }

}