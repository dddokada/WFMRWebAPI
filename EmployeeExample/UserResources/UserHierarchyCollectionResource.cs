#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;

namespace EmployeeExample.UserResources
{
  public class UserHierarchyCollectionResource
  {
    public List<UserHierarchyResource> UserHierarchies { get; set; }

    public UserHierarchyCollectionResource()
    {
      UserHierarchies = new List<UserHierarchyResource>();
    }
  }
}