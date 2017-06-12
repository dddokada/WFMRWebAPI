#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace EmployeeExample.EmployeeResources
{
  public class EmployeeScheduleConstraintsResource
  {
    //[DataMember(IsRequired = true)]
    public int ID { get; set; }

    public int EmployeeID { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public bool CanWorkSplit { get; set; }
    public decimal? MinHoursBetweenShifts { get; set; }
    public int? MaxDaysPerWeek { get; set; }
    public int? MaxConsecutiveDays { get; set; }
    public int? MaxConsecutiveDaysAcrossWeeks { get; set; }
    public decimal? MaxHoursPerWeek { get; set; }
    public decimal? MinHoursPerWeek { get; set; }
    public bool StrictlyEnforceMinHoursPerWeek { get; set; }
    public bool StrictlyEnforceMaxHoursPerWeek { get; set; }
    public bool StrictlyEnforceMinHoursBetweenShifts { get; set; }
    public bool StrictlyEnforceMaxDaysPerWeek { get; set; }
    public bool StrictlyEnforceMaxConsecutiveDays { get; set; }
    public bool StrictlyEnforceMaxConsecutiveDaysAcrossWeeks { get; set; }
    public int ReasonID { get; set; }
  }
}