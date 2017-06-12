#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.UserResources
{
  public class UserSecurityGroupAssignmentResource
  {
    public int ID { get; set; }
    public int SecurityGroupID { get; set; }
    public string Name { get; set; }
  }
}