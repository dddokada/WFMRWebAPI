#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;

namespace EmployeeExample.UserResources
{
  public class UserResource
  {
    public int ID { get; set; }

    //[Required]
    public Name Name { get; set; }

    //[Required]
    public string Password { get; set; }

    public bool IsLoginActive { get; set; }
    public bool ForcePasswordChange { get; set; }
    public int? DefaultOrgHierarchyID { get; set; }
    public int? DefaultSecurityGroupID { get; set; }
    public List<UserSecurityGroupAssignmentResource> UserSecurityGroupAssignment { get; set; }
    public UserHierarchyCollectionResource UserHierarchyCollection { get; set; }
    public ContactInfoResource ContactInfo { get; set; }
    public int? LanguageID { get; set; }
  }
}