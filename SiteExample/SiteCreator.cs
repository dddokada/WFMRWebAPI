#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Commons;

namespace SiteExample
{
  public class SiteCreator
  {
    public static readonly Uri BaseUri = new Uri("http://localhost:8080/data/retailwebapi/api/v1-beta3/");

    public const string Username = "wfment";
    public const string Password = "123456";

    public const string AlreadyExistsErrorCode = "DataAccessorErrorCodes.NameNotUnique";

    public const string TimeZoneApiUrl = "TimeZones";
    public const string OperatingHoursApiUrl = "OperatingHours";
    public const string OrgHierarchyApiUrl = "OrganizationalHierarchies";

    public const string TimeZoneName = "Eastern Time (US & Canada) (GMT -5:00)";
    public const string OperatingHoursName = "Store Hours of Operation";
    public const string OrgHierarchyName = "Alpharetta";

    public const string SiteApiUrl = "Sites";

    private readonly ApiClient _Client;

    private SiteCreator()
    {
      _Client = new ApiClient(BaseUri, Username, Password);
    }

    public static void Go()
    {
      var creator = new SiteCreator();
      creator.CreateSite();
    }

    private void CreateSite()
    {
      Task<TimeZoneResource> timeZoneTask = GetTimeZone();
      Task<OperatingHoursSiteAssignmentResource[]> operatingHoursTask = GetOperatingHours();
      Task<ParentOrganizationalHierarchyResource> parentOrgTask = GetParentOrg();

      var site = new SiteResource
      {
        Name = "ChristmasTimeStore",
        LongName = "Christmas Store",
        Status = "Open",
        MailingAddress = new SiteMailingAddressResource
        {
          AddressLine1 = "3901 Brookside Parkway",
          State = "GA",
          ZipCode = "30024",
          City = "Alpharetta",
          County = "Fulton",
          CountryCode = "US",
          Email = "jda@jda.com",
          WorkPhone = "111-111-1111"
        },
        TimeZoneAssignmentID = timeZoneTask.Result.ID,
        OperatingHoursSiteAssignments = operatingHoursTask.Result,
        ParentOrganizationalHierarchyAssignmentID = parentOrgTask.Result.ID
      };

      try
      {
        _Client.Create(HttpMethod.Post, SiteApiUrl, site).Wait();
        Console.WriteLine("Created site " + site.Name);
      }
      catch (AggregateException aggregateException)
      {
        aggregateException.Handle(HandleCreationException);
      }
    }

    private bool HandleCreationException(Exception e)
    {
      if (!(e is ApiCreateException<SiteResource>))
      {
        return false;
      }

      var apiException = e as ApiCreateException<SiteResource>;
      var error = apiException.ErrorBody;
      if (error.ErrorCode.Equals(AlreadyExistsErrorCode))
      {
        Console.WriteLine(apiException.PostedContent.Name + " already exists!");
        return true;
      }

      return false;
    }

    private Task<TimeZoneResource> GetTimeZone()
    {
      var url = UrlFormatter.AddNameToRoot(TimeZoneApiUrl, TimeZoneName);
      return _Client.Get<TimeZoneResource>(url);
    }

    private async Task<OperatingHoursSiteAssignmentResource[]> GetOperatingHours()
    {
      var url = UrlFormatter.AddNameToRoot(OperatingHoursApiUrl, OperatingHoursName);
      Task<OperatingHoursResource> operatingHoursTask = _Client.Get<OperatingHoursResource>(url);
      var ret = new[]
      {
        new OperatingHoursSiteAssignmentResource {
          ID = 1, // this is required to be non-zero b/c of a bug
          Start = "2010-01-11T00:00:00",
          OperatingHoursID = (await operatingHoursTask).ID
        }
      };

      return ret;
    }

    private Task<ParentOrganizationalHierarchyResource> GetParentOrg()
    {
      var url = UrlFormatter.AddNameToRoot(OrgHierarchyApiUrl, OrgHierarchyName);
      return _Client.Get<ParentOrganizationalHierarchyResource>(url);
    }
  }
}
