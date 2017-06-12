#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.EmployeeResources
{
  public class JobResource
  {
    public int ID { get; set; }
    public string Name { get; set; }
    public string DisplayCode { get; set; }
    public string JobCode { get; set; }
    public int DepartmentID { get; set; }
  }
}