#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.EmployeeResources
{
  public class EmployeeSiteAssignmentResource
  {
    public int ID { get; set; }
    public int EmployeeID { get; set; }
    public int SiteId { get; set; }
    public string Start { get; set; } //date
    public string End { get; set; } //date
    public bool IsHomeSite { get; set; }
  }
}