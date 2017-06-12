#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;

namespace EmployeeExample.EmployeeResources
{
  public class EmployeeScheduleConstraintsCollectionResource
  {
    //[DataMember(IsRequired = true)]
    public int ID { get; set; }

    public List<EmployeeScheduleConstraintsResource> EmployeeScheduleConstraintsCollection { get; set; }

    public EmployeeScheduleConstraintsCollectionResource()
    {
      EmployeeScheduleConstraintsCollection = new List<EmployeeScheduleConstraintsResource>();
    }
  }

}