#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using EmployeeExample;
using EmployeeExample.UserResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeExampleContractTests
{
  [TestClass]
  public class UserApiContractTests
  {
    private static ApiClient _Client;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
      _Client = new ApiClient(Program.BaseUri, Program.Username, Program.Password);
    }

    [TestMethod]
    public void UserApiHasCorrectOptions()
    {
      IEnumerable<string> receivedOptions = _Client.Options(UserCreator.UserApi).Result;

      ISet<string> expectedOptions = new HashSet<string> { "GET", "POST" };
      string[] missingOptions = expectedOptions.Except(receivedOptions).ToArray();

      Assert.AreEqual(0, missingOptions.Count(), "Did not get " + String.Join(",", missingOptions));
    }

    [TestMethod]
    public void UserApiHasCorrectFields()
    {
      string url = UrlFormatter.AddParameter(UserCreator.UserApi, "loginName", "waveadmin");
      UserResource user = _Client.Get<UserResource>(url).Result;
      Assert.AreNotEqual(0, user.ID);
    }

    [TestMethod]
    public void OrgHierarchyApiHasCorrectOptions()
    {
      AssertApiHasGetOption(UserCreator.UserApi);
    }

    [TestMethod]
    public void OrgHierarchiesExist()
    {
      foreach (string org in UserCreator.OrgHierarchyNames)
      {
        string api = UrlFormatter.AddNameToRoot(UserCreator.OrgHierarchyApi, org);
        OrganizationalHierarchyResource orgHierachy = _Client.Get<OrganizationalHierarchyResource>(api).Result;
        Assert.AreNotEqual(0, orgHierachy.ID);
      }
    }

    [TestMethod]
    public void SecurityGroupsApiHasCorrectOptions()
    {
      AssertApiHasGetOption(UserCreator.SecurityGroupApi);
    }

    [TestMethod]
    public void SecurityGroupsExists()
    {
      foreach (string org in UserCreator.SecurityGroupNames)
      {
        string api = UrlFormatter.AddNameToRoot(UserCreator.SecurityGroupApi, org);
        Identifiable securityGroup = _Client.Get<Identifiable>(api).Result;
        Assert.AreNotEqual(0, securityGroup.ID);
      }
    }

    private void AssertApiHasGetOption(string api)
    {
      IEnumerable<string> receivedOptions = _Client.Options(api).Result;
      Assert.IsTrue(receivedOptions.Contains("GET"));
    }
  }
}
