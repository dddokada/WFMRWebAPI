#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.EmployeeResources
{
  public class EmployeeJobAssignmentRateResource
  {
    public int EmployeeJobAssignmentID { get;  set; }
    public int EmployeeID { get;  set; }
    public string Start { get;  set; } //date
    public string End { get;  set; } //date
    public decimal Rate { get;  set; }
  }
}