#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.EmployeeResources
{
  public class PrimaryJobSettingsResource
  {
    public virtual int ID { get; set; }
    public virtual int EmployeeID { get; set; }

    public int PayPolicyID { get; set; }

    public int PunchRuleGroupID { get; set; }

    public int? ShiftStrategy { get; set; }

    public bool NonExempt { get; set; }
    public bool Hourly { get; set; }
    public bool PartTime { get; set; }
  }
}