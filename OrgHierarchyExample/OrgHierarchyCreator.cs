#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Commons;
using OrgHierarchyExample.OrgHierarchyResources;

namespace OrgHierarchyExample
{
  public class OrgHierarchyCreator
  {
    public static readonly Uri BaseUri = new Uri("http://localhost:8080/data/retailwebapi/api/v1-beta3/");

    public const string Username = "wfment";
    public const string Password = "123456";

    public const string LevelApi = "OrganizationalHierarchyLevels";
    public const string HierarchyApi = "OrganizationalHierarchies";
    public const string AlreadyExistsErrorCode = "DataAccessorErrorCodes.NameNotUnique";

    private readonly ApiClient _Client;

    private OrgHierarchyCreator()
    {
      _Client = new ApiClient(BaseUri, Username, Password);
    }

    public static void Go()
    {
      var creator = new OrgHierarchyCreator();
      creator.CreateBrandsAndDivisions();
      creator.CreateRegions();
      creator.CreateDistricts();
    }

    private void CreateBrandsAndDivisions()
    {
      Task createBrandsAndDivision = CreateOrgUnit("JDAStores", "JDA Stores", "Brand/Division", "RetailCo");
      WaitOnCreationTasks(createBrandsAndDivision);
    }

    private void CreateRegions()
    {
      Task createNorthMetroTask = CreateOrgUnit("NorthMetroAtlanta", "North Metro Atlanta", "Region", "JDAStores");
      Task createEastMetroTask = CreateOrgUnit("EastMetroAtlanta", "East Metro Atlanta", "Region", "JDAStores");
      WaitOnCreationTasks(createNorthMetroTask, createEastMetroTask);
    }

    private void CreateDistricts()
    {
      Task[] createDistrictsTasks =
      {
        CreateOrgUnit("Alpharetta", "Alpharetta", "District", "NorthMetroAtlanta"),
        CreateOrgUnit("Roswell", "Roswell", "District", "NorthMetroAtlanta"),
        CreateOrgUnit("Dunwoody", "Dunwoody", "District", "NorthMetroAtlanta"),
        CreateOrgUnit("Decatur", "Decatur", "District", "EastMetroAtlanta"),
        CreateOrgUnit("NorthDruidHills", "North Druid Hills", "District", "EastMetroAtlanta"),
        CreateOrgUnit("Tucker", "Tucker", "District", "EastMetroAtlanta")
      };

      WaitOnCreationTasks(createDistrictsTasks);
    }

    private void WaitOnCreationTasks(params Task[] tasks)
    {
      try
      {
        Task.WaitAll(tasks);
      }
      catch (AggregateException ae)
      {
        ae.Handle(HandleCreationException);
      }
    }

    private bool HandleCreationException(Exception e)
    {
      if (!(e is ApiCreateException<OrganizationalHierarchyResource>))
      {
        return false;
      }

      var apiException = e as ApiCreateException<OrganizationalHierarchyResource>;
      var error = apiException.ErrorBody;
      if (error.ErrorCode.Equals(AlreadyExistsErrorCode))
      {
        Console.WriteLine(apiException.PostedContent.Name + " already exists!");
        return true;
      }
      return false;
    }

    private async Task CreateOrgUnit(string name, string longName, string levelName, string parentName)
    {
      var getLevelTask = _Client.Get<OrganizationalHierarchyLevelResource>(UrlFormatter.AddNameToRoot(LevelApi, levelName));
      var getParentTask = _Client.Get<ParentOrganizationalHierarchyResource>(UrlFormatter.AddNameToRoot(HierarchyApi, parentName));

      var newOrgHierarchy = new OrganizationalHierarchyResource
      {
        Name = name,
        LongName = longName,
        OrganizationalHierarchyLevelAssignmentID = (await getLevelTask).ID,
        ParentOrganizationalHierarchyAssignmentID = (await getParentTask).ID
      };

      const string uri = "OrganizationalHierarchies";
      await _Client.Create(HttpMethod.Post, uri, newOrgHierarchy); // TODO save return value to get ID

      Console.WriteLine("Successfully created " + name);
    }
  }
}