#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Commons;
using EmployeeExample.EmployeeResources;
using EmployeeExample.UserResources;

namespace EmployeeExample
{
  public class EmployeeCreator
  {
    public const string AlreadyExistsErrorCode = "EmployeeStatusErrorCode.StatusHistoryIsNotContinuousOrHasOverlappingStatuses";

    public const string JobName = "Delivery Driver";
    public const string PayPolicyName = "Standard Pay Rule";
    public const string PunchRuleName = "Standard Punch Rule";
    public const string SiteName = "ChristmasTimeStore";
    private const string StartDate = "2011-01-01T00:00:00";

    private static ApiClient _Client;

    public static readonly string SiteApi = "Sites";
    public static readonly string EmployeeApi = "Employees";
    public static readonly string ScheduleConstraintReasonApi = "ScheduleConstraintReasons";
    public static readonly string PunchRuleGroupApi = "PunchRuleGroups";
    public static readonly string PayPolicyApi = "PayPolicies";
    public static readonly string JobApi = "Jobs";

    public EmployeeCreator(ApiClient client)
    {
      _Client = client;
    }

    private int GetSiteID()
    {
      var url = UrlFormatter.AddNameToRoot(SiteApi, SiteName);
      return _Client.Get<Identifiable>(url).Result.ID;
    }

    public void CreateEmployee(UserResource createdUser)
    {
      var siteId = GetSiteID();

      var siteAssignments = CreateEmployeeSiteAssignmentCollection(createdUser, StartDate, siteId);
      var jobAssignments = CreateEmployeeJobAssignments(createdUser, StartDate, siteId);
      var employeeStatuses = CreateEmployeeStatusCollection(createdUser, siteId, StartDate);
      var scheduleConstraints = CreateEmployeeScheduleConstraintsList(createdUser, siteId);

      var employee = new EmployeeResource
      {
        SeniorityDate = StartDate,
        MinorStatus = "NotMinor",
        PunchValidationCode = "NoScheduleValidation",
        GenerateException = true,
        EmployeeSiteAssignmentCollection = siteAssignments,
        IsManagement = false,
        EmployeeJobAssignments = jobAssignments,
        ManagerInSchedule = false,
        ID = createdUser.ID,
        BadgeNumber = createdUser.ID.ToString(),
        SchoolDistrictID = null,
        SchedulingTypeCode = "Manual",
        IgnoreBiometricValidation = false,
        PayGroupID = null,
        CanWorkUnassignedJobs = false,
        PayrollSystemNumber = null,
        ManagerPassword = null,
        GenerateAlerts = false,
        BirthDate = "1992-09-02T00:00:00",
        EmployeeStatuses = employeeStatuses,
        EmployeeScheduleConstraints = scheduleConstraints,
        HireDate = StartDate
      };
      try
      {
        _Client.Create(HttpMethod.Post, EmployeeApi, employee).Wait();
        Console.WriteLine("Employee " + createdUser.Name.FirstName + " " + createdUser.Name.LastName + " was created");
      }
      catch (AggregateException aggregateException)
      {
        aggregateException.Handle(HandleCreationException);
      }
    }

    private bool HandleCreationException(Exception e)
    {
      if (!(e is ApiCreateException<EmployeeResource>))
      {
        return false;
      }

      var apiException = e as ApiCreateException<EmployeeResource>;
      var error = apiException.ErrorBody;
      if (error.ErrorCode.Equals(AlreadyExistsErrorCode))
      {
        Console.WriteLine("Employee " + apiException.PostedContent.ID + " already exists!");
        return true;
      }

      return false;
    }

    private List<EmployeeScheduleConstraintsResource> CreateEmployeeScheduleConstraintsList(
      UserResource createdUser, int siteId)
    {
      var scheduleConstraintReasonId = GetScheduleConstraintReason(siteId, "General Constraint");

      return new List<EmployeeScheduleConstraintsResource>
        {
          new EmployeeScheduleConstraintsResource
          {
            StrictlyEnforceMaxDaysPerWeek = false,
            End = null,
            Start = "2000-01-01T00:00:00",
            StrictlyEnforceMaxHoursPerWeek = false,
            StrictlyEnforceMinHoursPerWeek = false,
            EmployeeID = createdUser.ID,
            MaxHoursPerWeek = null,
            MinHoursPerWeek = null,
            StrictlyEnforceMaxConsecutiveDays = false,
            MaxConsecutiveDays = null,
            MaxConsecutiveDaysAcrossWeeks = null,
            CanWorkSplit = false,
            MaxDaysPerWeek = null,
            MinHoursBetweenShifts = null,
            StrictlyEnforceMinHoursBetweenShifts = false,
            ReasonID = scheduleConstraintReasonId,
            StrictlyEnforceMaxConsecutiveDaysAcrossWeeks = false
          }
        };
    }

    private int GetScheduleConstraintReason(int siteId, string reasonName)
    {
      var url = UrlFormatter.AddSiteToRoot(ScheduleConstraintReasonApi, siteId);
      var reasonsForSite = _Client.Get<ScheduleConstraintReasonCollection>(url).Result;
      var reason = reasonsForSite.ScheduleConstraintReasons.First(r => r.Name.Equals(reasonName));
      return reason.ID;
    }

    private List<EmployeeStatusResource> CreateEmployeeStatusCollection(UserResource createdUser, int siteId,
      string startDate)
    {
      return new List<EmployeeStatusResource>
      {
          new EmployeeStatusResource
          {
            EmployeeID = createdUser.ID,
            EmployeeStatusCode = "Hire",
            HomeSiteID = siteId,
            IsActive = true,
            Start = startDate,
            End = null
          }
      };
    }

    private List<EmployeeJobAssignmentResource> CreateEmployeeJobAssignments(UserResource createdUser,
     string startDate, int siteId)
    {
      var punchRuleId = GetPunchRuleGroup(siteId);
      var payPolicyId = GetPayPolicy(siteId);
      var jobId = GetJob();

      return new List<EmployeeJobAssignmentResource>
      {
        new EmployeeJobAssignmentResource
        {
          PrimaryJobSettings = new PrimaryJobSettingsResource
          {
            NonExempt = false,
            EmployeeID = createdUser.ID,
            Hourly = true,
            PartTime = false,
            PunchRuleGroupID = punchRuleId,
            ShiftStrategy = null,
            PayPolicyID = payPolicyId
          },
          EmployeeID = createdUser.ID,
          JobID = jobId,
          Start = startDate,
          End = null,
          Rank = null,
          Rates = new List<EmployeeJobAssignmentRateResource>
          {
            new EmployeeJobAssignmentRateResource
            {
              EmployeeID = createdUser.ID,
              Start = startDate,
              End = null,
              Rate = 14.75M
            }
          },
          Skill = null,
        }
      };
    }

    private int GetPunchRuleGroup(int siteId)
    {
      var url = UrlFormatter.AddSiteToRoot(PunchRuleGroupApi, siteId);
      var punchRulesForSite =
        _Client.Get<PunchRuleGroupResource[]>(url).Result;
      var punchRuleId = punchRulesForSite.First(rule => rule.Name.Equals(PunchRuleName)).ID;
      return punchRuleId;
    }

    private int GetPayPolicy(int siteId)
    {
      var url = UrlFormatter.AddSiteToRoot(PayPolicyApi, siteId);
      var payPolicies = _Client.Get<PayPolicyResource[]>(url).Result;
      var payPolicy = payPolicies.First(policy => policy.Name.Equals(PayPolicyName));
      return payPolicy.ID;
    }

    private int GetJob()
    {
      var url = UrlFormatter.AddNameToRoot(JobApi, JobName);
      return _Client.Get<JobResource>(url).Result.ID;
    }

    private EmployeeSiteAssignmentCollectionResource CreateEmployeeSiteAssignmentCollection(
      UserResource createdUser, string startDate, int siteId)
    {
      return new EmployeeSiteAssignmentCollectionResource
      {
        EmployeeSiteAssignments = new List<EmployeeSiteAssignmentResource>
        {
          new EmployeeSiteAssignmentResource
          {
            EmployeeID = createdUser.ID,
            End = null,
            Start = startDate,
            SiteId = siteId,
          }
        }
      };
    }
  }
}
