#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Commons;
using EmployeeExample;
using EmployeeExample.EmployeeResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeExampleContractTests
{
  [TestClass]
  public class EmployeeApiContractTests
  {
    private static ApiClient _Client;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
      _Client = new ApiClient(Program.BaseUri, Program.Username, Program.Password);
    }

    [TestMethod]
    public void EmployeeApiHasCorrectOptions()
    {
      IEnumerable<string> receivedOptions = _Client.Options(EmployeeCreator.EmployeeApi).Result;

      ISet<string> expectedOptions = new HashSet<string> { "GET", "POST" };
      string[] missingOptions = expectedOptions.Except(receivedOptions).ToArray();

      Assert.AreEqual(0, missingOptions.Count(), "Did not get " + String.Join(",", missingOptions));

    }

    [TestMethod]
    public void EmployeeApiExists()
    {
      IEnumerable<string> options = _Client.Options(EmployeeCreator.EmployeeApi).Result;
      Assert.IsTrue(options.Contains("POST"), "Could not find POST in Allow header.");
    }

    [TestMethod]
    public void ScheduleConstraintReasonApiHasCorrectOptions()
    {
      AssertApiHasGetOption(EmployeeCreator.ScheduleConstraintReasonApi);
    }

    [TestMethod]
    public void ScheduleConstraintReasonHasCorrectFields()
    {
      string api = UrlFormatter.AddSiteToRoot(EmployeeCreator.ScheduleConstraintReasonApi, GetSiteID());
      ScheduleConstraintReasonCollection reasonCollection = _Client.Get<ScheduleConstraintReasonCollection>(api).Result;
      Assert.IsNotNull(reasonCollection.ScheduleConstraintReasons);

      ScheduleConstraintReasonResource sampleConstraintReason = reasonCollection.ScheduleConstraintReasons[0];
      Assert.AreNotEqual(0, sampleConstraintReason.ID);
      Assert.IsNotNull(sampleConstraintReason.Name);
    }

    [TestMethod]
    public void PunchRuleGroupApiHasCorrectOptions()
    {
      AssertApiHasGetOption(EmployeeCreator.PunchRuleGroupApi);
    }

    [TestMethod]
    public void PunchRuleGroupExists()
    {
      string api = UrlFormatter.AddNameToRoot(EmployeeCreator.PunchRuleGroupApi, EmployeeCreator.PunchRuleName);
      PunchRuleGroupResource[] reasonCollection = _Client.Get<PunchRuleGroupResource[]>(api).Result;
      PunchRuleGroupResource punchRuleGroup =
        reasonCollection.FirstOrDefault(group => group.Name.Equals(EmployeeCreator.PunchRuleName));
      Assert.IsNotNull(punchRuleGroup);
      Assert.AreNotEqual(0, punchRuleGroup.ID);
    }

    [TestMethod]
    public void PayPolicyApiHasCorrectOptions()
    {
      AssertApiHasGetOption(EmployeeCreator.PayPolicyApi);
    }

    [TestMethod]
    public void PayPolicyExists()
    {
      string api = UrlFormatter.AddNameToRoot(EmployeeCreator.PayPolicyApi, EmployeeCreator.PayPolicyName);
      PayPolicyResource[] payPolicies = _Client.Get<PayPolicyResource[]>(api).Result;
      PayPolicyResource payPolicy = payPolicies.FirstOrDefault(group => group.Name.Equals(EmployeeCreator.PayPolicyName));
      Assert.IsNotNull(payPolicy);
      Assert.AreNotEqual(0, payPolicy.ID);
    }

    [TestMethod]
    public void JobApiHasCorrectOptions()
    {
      AssertApiHasGetOption(EmployeeCreator.JobApi);
    }

    [TestMethod]
    public void JobExists()
    {
      string api = UrlFormatter.AddNameToRoot(EmployeeCreator.JobApi, EmployeeCreator.JobName);
      JobResource payPolicies = _Client.Get<JobResource>(api).Result;
      Assert.AreEqual(EmployeeCreator.JobName, payPolicies.Name);
      Assert.AreNotEqual(0, payPolicies.ID);
    }

    [TestMethod]
    public void SiteApiHasCorrectOptions()
    {
      AssertApiHasGetOption(EmployeeCreator.SiteApi);
    }

    [TestMethod]
    public void SiteApiExists()
    {
      Assert.AreNotEqual(0, GetSiteID());
    }

    private static int GetSiteID()
    {
      var url = UrlFormatter.AddNameToRoot(EmployeeCreator.SiteApi, EmployeeCreator.SiteName);
      Identifiable site = _Client.Get<Identifiable>(url).Result;
      return site.ID;
    }

    private void AssertApiHasGetOption(string api)
    {
      IEnumerable<string> receivedOptions = _Client.Options(api).Result;
      Assert.IsTrue(receivedOptions.Contains("GET"));
    }
  }
}
