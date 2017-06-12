#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Commons;
using OrgHierarchyCreatorAlternative.OrgHierarchyResources;
using System.Linq;

namespace OrgHierarchyCreatorAlternative
{
  public class OrgHierarchyCreator
  {
    public static readonly Uri BaseUri = new Uri("http://ilrwesapp1.jdadelivers.com/retail/data/retailwebapi/api/v1alpha1/");

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
      OrgHierarchyCreator creator = new OrgHierarchyCreator();
      var parent = creator.GetEnterpriseOrgUnit();
      creator.CreateBrandsAndDivisions(parent);
    }

    private OrganizationalHierarchyResource GetEnterpriseOrgUnit()
    {
      return _Client.Get<OrganizationalHierarchyResource>(HierarchyApi + "?name=RetailCo").Result;
    }

    private void CreateBrandsAndDivisions(OrganizationalHierarchyResource parent)
    {
      OrganizationalHierarchyResource jdaStores = CreateOrgUnit("JDAStores", "JDA Stores", "Brand/Division", parent).Result;
      Task northRegionTask = CreateNorthRegion(jdaStores);
      Task eastRegionTask = CreateEastRegion(jdaStores);
      Task.WaitAll(northRegionTask, eastRegionTask);
    }

    private async Task CreateNorthRegion(OrganizationalHierarchyResource parent)
    {
      OrganizationalHierarchyResource northMetroAtlanta = await CreateOrgUnit("NorthMetroAtlanta", "North Metro Atlanta", "Region", parent);
      CreateNorthDistricts(northMetroAtlanta);
    }

    private void CreateNorthDistricts(OrganizationalHierarchyResource northRegion)
    {
      Task alpharetta = CreateOrgUnit("Alpharetta", "Alpharetta", "District", northRegion);
      Task roswell = CreateOrgUnit("Roswell", "Roswell", "District", northRegion);
      Task dunwoody = CreateOrgUnit("Dunwoody", "Dunwoody", "District", northRegion);
      Task.WaitAll(alpharetta, roswell, dunwoody);
    }

    private async Task CreateEastRegion(OrganizationalHierarchyResource parent)
    {
      OrganizationalHierarchyResource eastMetroAtlanta = await CreateOrgUnit("EastMetroAtlanta", "East Metro Atlanta", "Region", parent);
      CreateEastDistricts(eastMetroAtlanta);
    }

    private void CreateEastDistricts(OrganizationalHierarchyResource eastRegion)
    {
      Task decatur = CreateOrgUnit("Decatur", "Decatur", "District", eastRegion);
      Task northDruidHills = CreateOrgUnit("NorthDruidHills", "North Druid Hills", "District", eastRegion);
      Task tucker = CreateOrgUnit("Tucker", "Tucker", "District", eastRegion);

      Task.WaitAll(decatur, northDruidHills, tucker);
    }

    private void HandleCreationException(ApiCreateException<OrganizationalHierarchyResource> apiException)
    {
      ErrorBody error = apiException.ErrorBody;
      if (error.ErrorCode.Equals(AlreadyExistsErrorCode))
      {
        Console.WriteLine(apiException.PostedContent.Name + " already exists!");
      }
      else
      {
        throw apiException;
      }
    }

    private async Task<OrganizationalHierarchyResource> CreateOrgUnit(string name, string longName, string levelName, OrganizationalHierarchyResource parent)
    {
      Link addLink = parent.Actions.First(link => link.Rel.Equals("add"));
      Task<OrganizationalHierarchyLevelResource> getLevelTask =
        _Client.Get<OrganizationalHierarchyLevelResource>(LevelApi + "?name=" + levelName);

      var newOrgHierarchy = new OrganizationalHierarchyResource
      {
        Name = name,
        LongName = longName,
        OrganizationalHierarchyLevelAssignment = await getLevelTask,
        ParentOrganizationalHierarchyAssignment = parent
      };

      OrganizationalHierarchyResource orgUnit = null;
      bool orgUnitCreated;
      try
      {
        orgUnit = await _Client.Create(addLink, newOrgHierarchy);
        Console.WriteLine("Successfully created " + name);
        orgUnitCreated = true;
      }
      catch (ApiCreateException<OrganizationalHierarchyResource> apiException)
      {
        HandleCreationException(apiException);
        orgUnitCreated = false;
      }

      if (!orgUnitCreated)
      {
        orgUnit = await _Client.Get<OrganizationalHierarchyResource>(HierarchyApi + "?name=" + name);
      }

      return orgUnit;
    }
  }
}