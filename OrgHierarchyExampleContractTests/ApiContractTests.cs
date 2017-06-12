#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Collections.Generic;
using Commons;
using OrgHierarchyExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using OrgHierarchyExample.OrgHierarchyResources;

namespace OrgHierarchyExampleContractTests
{
  [TestClass]
  public class ApiContractTests
  {
    private static ApiClient _Client;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
      _Client = new ApiClient(OrgHierarchyCreator.BaseUri, OrgHierarchyCreator.Username, OrgHierarchyCreator.Password);
    }

    [TestMethod]
    public void OrgHierarchyLevelApiExists()
    {
      IEnumerable<string> options = _Client.Options(OrgHierarchyCreator.LevelApi).Result;
      Assert.IsTrue(options.Contains("GET"), "Could not find Get in Allow header.");
    }

    [TestMethod]
    public void OrgHierarchyApiExists()
    {
      IEnumerable<string> receivedOptions = _Client.Options(OrgHierarchyCreator.HierarchyApi).Result;

      ISet<string> expectedOptions = new HashSet<string> { "GET", "POST" };
      string[] missingOptions = expectedOptions.Except(receivedOptions).ToArray();

      Assert.AreEqual(0, missingOptions.Count(), "Did not get " + String.Join(",", missingOptions));
    }

    [TestMethod]
    public void OrgHierarchyHasRequiredFields()
    {
      OrganizationalHierarchyResource org = _Client.Get<OrganizationalHierarchyResource>(OrgHierarchyCreator.HierarchyApi + "?name=RetailCo").Result;
      Assert.AreNotEqual(0, org.ID);
      Assert.IsNotNull(org.Name);
    }

    [TestMethod]
    public void OrgHierarchyLevelHasRequiredFields()
    {
      OrganizationalHierarchyLevelResource level = _Client.Get<OrganizationalHierarchyLevelResource>(OrgHierarchyCreator.LevelApi + "?name=Enterprise").Result;
      Assert.AreNotEqual(0, level.ID);
    }
  }
}
