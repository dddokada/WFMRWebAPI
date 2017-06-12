#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;

namespace EmployeeExample.EmployeeResources
{
  public class EmployeeJobAssignmentResource
  {
    public int ID { get; set; }

    //[DataMember(IsRequired = true)]
    //public int JobID { get; set; }

    public int JobID { get; set; }

    public int EmployeeID { get; set; }
    public string Start { get; set; } //date
    public string End { get; set; } //date
    public decimal? Skill { get; set; }
    public decimal? Rank { get; set; }

    //get
    public PrimaryJobSettingsResource PrimaryJobSettings { get; set; }

    //[Required]
    public IList<EmployeeJobAssignmentRateResource> Rates { get; set; }
  }
}
