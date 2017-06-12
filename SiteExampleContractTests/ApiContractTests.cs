#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;
using System.Linq;
using Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteExample;

namespace SiteExampleContractTests
{
  [TestClass]
  public class ApiContractTests
  {
    private static ApiClient _Client;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
      _Client = new ApiClient(SiteCreator.BaseUri, SiteCreator.Username, SiteCreator.Password);
    }

    [TestMethod]
    public void SiteApiHasCorrectOptions()
    {
      IEnumerable<string> options = _Client.Options(SiteCreator.SiteApiUrl).Result;
      Assert.IsTrue(options.Contains("POST"), "Could not find POST in Allow header.");
    }

    [TestMethod]
    public void TimeZoneApiHasCorrectOptions()
    {
      AssertApiHasGetOption(SiteCreator.TimeZoneApiUrl);
    }

    [TestMethod]
    public void OperatingHoursApiHasCorrectOptions()
    {
      AssertApiHasGetOption(SiteCreator.OperatingHoursApiUrl);
    }

    [TestMethod]
    public void OrgHierarchyApiHasCorrectOptions()
    {
      AssertApiHasGetOption(SiteCreator.OrgHierarchyApiUrl);
    }

    [TestMethod]
    public void TimeZoneResourceHasCorrectFields()
    {
      string api = UrlFormatter.AddNameToRoot(SiteCreator.TimeZoneApiUrl, SiteCreator.TimeZoneName);
      var timeZone = _Client.Get<TimeZoneResource>(api).Result;
      Assert.AreNotEqual(0, timeZone.ID);
    }

    [TestMethod]
    public void OperatingHoursResourceHasCorrectFields()
    {
      string api = UrlFormatter.AddNameToRoot(SiteCreator.OperatingHoursApiUrl, SiteCreator.OperatingHoursName);
      var operatingHours = _Client.Get<OperatingHoursResource>(api).Result;
      Assert.AreNotEqual(0, operatingHours.ID);
    }

    [TestMethod]
    public void OrgHierarchyResourceHasCorrectFields()
    {
      var api = UrlFormatter.AddNameToRoot(SiteCreator.OrgHierarchyApiUrl, SiteCreator.OrgHierarchyName);
      var parentOrgHierarchy = _Client.Get<ParentOrganizationalHierarchyResource>(api).Result;
      Assert.AreNotEqual(0, parentOrgHierarchy.ID);
    }

    private void AssertApiHasGetOption(string api)
    {
      IEnumerable<string> options = _Client.Options(api).Result;
      Assert.IsTrue(options.Contains("GET"), "Could not find Get in Allow header.");
    }
  }
}
