#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;

namespace EmployeeExample.EmployeeResources
{
  public class EmployeeSiteAssignmentCollectionResource
  {
    public int ID { get; set; }
    public List<EmployeeSiteAssignmentResource> EmployeeSiteAssignments { get; set; }

    public EmployeeSiteAssignmentCollectionResource()
    {
      EmployeeSiteAssignments = new List<EmployeeSiteAssignmentResource>();
    }
  }
}