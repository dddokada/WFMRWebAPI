#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;

namespace EmployeeExample.EmployeeResources
{
  public class EmployeeResource
  {
    //[DataMember(IsRequired = true)]
    public int ID { get; set; }

    //[Required]
    public string BadgeNumber { get; set; }

    public string BirthDate { get; set; }

    public string HireDate { get; set; }

    public string SeniorityDate { get; set; }

    //[NullableEnumValidation(typeof(GenderCode))]
    public string Gender { get; set; }

    public string MinorStatus { get; set; }

    public bool IsManagement { get; set; }

    public int? ManagerPassword { get; set; }

    public bool ManagerInSchedule { get; set; }

    public bool GenerateException { get; set; }

    public bool GenerateAlerts { get; set; }

    public bool CanWorkUnassignedJobs { get; set; }

    public string SchedulingTypeCode { get; set; }

    public bool IgnoreBiometricValidation { get; set; }

    public string PunchValidationCode { get; set; }

    // get
    public int? SchoolDistrictID { get; set; }

    //[Required] get
    public List<EmployeeJobAssignmentResource> EmployeeJobAssignments { get; set; }

    //get
    public EmployeeSiteAssignmentCollectionResource EmployeeSiteAssignmentCollection { get; set; }

    //[Required] get
    public List<EmployeeStatusResource> EmployeeStatuses { get; set; }

    // get
    public List<EmployeeScheduleConstraintsResource> EmployeeScheduleConstraints { get; set; }

    public string PayrollSystemNumber { get; set; }

    public string PayGroupID { get; set; }
  }

}