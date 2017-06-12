#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.Net.Http;
using Commons;
using EmployeeExample.UserResources;

namespace EmployeeExample
{
  public class UserCreator
  {
    public const string AlreadyExistsErrorCode = "UserErrorCode.LoginNameMustBeUnique";

    public const string LoginName = "odai";
    public static readonly string UserApi = "Users";
    public static readonly string OrgHierarchyApi = "OrganizationalHierarchies";
    public static readonly string SecurityGroupApi = "SecurityGroups";

    public static readonly List<string> OrgHierarchyNames = new List<string>
    {
      "RetailCo",
      "JDAStores"
    };

    public static readonly List<string> SecurityGroupNames = new List<string>
    {
      "Client Admin",
      "Employee"
    };

    private readonly ApiClient _Client;

    public UserCreator(ApiClient client)
    {
      _Client = client;
    }

    public UserResource CreateUser()
    {
      UserResource user = null;
      try
      {
                Console.WriteLine("1");
                System.Threading.Thread.Sleep(1000);

                var securityGroupAssignment = SecurityGroupNames.ConvertAll(GetSecurityGroup);
                Console.WriteLine("2");
                System.Threading.Thread.Sleep(1000);

                var userHierarchies =
          new UserHierarchyCollectionResource
          {
            UserHierarchies = OrgHierarchyNames.ConvertAll(GetOrgHierarchy),
          };

        user = new UserResource
        {
          ContactInfo = new ContactInfoResource
          {
            AddressLine1 = "3905 Brookside Pkwy",
            CellPhone = "770 945 2211",
            City = "Alpharetta",
            CountryCode = "US",
            State = "GA",
            HomePhone = "7709454422",
            Email = "somename@somedomain.com",
          },
          IsLoginActive = true,
          Name = new Name("A-aron", "D", "Balaky", null, "JDA", LoginName),
          Password = "123456",
          UserHierarchyCollection = userHierarchies,
          UserSecurityGroupAssignment = securityGroupAssignment,
        };
                Console.WriteLine("!!?");
                System.Threading.Thread.Sleep(1000);
                var createdUser = _Client.Create(HttpMethod.Post, UserApi, user).Result;
        Console.WriteLine("User " + user.Name.LoginName + " was created.");
        return createdUser;
      }
      catch (AggregateException aggregateException)
      {
        aggregateException.Handle(HandleCreationException);
        return GetUserByLoginName(user.Name.LoginName);
      }
    }

    private bool HandleCreationException(Exception e)
    {
      if (!(e is ApiCreateException<UserResource>))
      {
        return false;
      }

      var apiException = e as ApiCreateException<UserResource>;
      var error = apiException.ErrorBody;
      if (error.ErrorCode.Equals(AlreadyExistsErrorCode))
      {
        Console.WriteLine(apiException.PostedContent.Name.LoginName + " already exists!");
        return true;
      }

      return false;
    }

    private UserResource GetUserByLoginName(string loginName)
    {
      var url = UrlFormatter.AddParameter(UserApi, "loginName", loginName);
      return _Client.Get<UserResource>(url).Result;
    }

    private UserHierarchyResource GetOrgHierarchy(string name)
    {
      var url = UrlFormatter.AddNameToRoot(OrgHierarchyApi, name);
      var orgHierarchy = _Client.Get<OrganizationalHierarchyResource>(url).Result;
      return new UserHierarchyResource { OrganizationalHierarchyID = orgHierarchy.ID };
    }

    private UserSecurityGroupAssignmentResource GetSecurityGroup(string name)
    {
            var url = UrlFormatter.AddNameToRoot(SecurityGroupApi, name);
            Console.WriteLine(name);
            Console.WriteLine(url);
            System.Threading.Thread.Sleep(10000);



            Identifiable securityGroup = _Client.Get<Identifiable>(url).Result;

            var assignment = new UserSecurityGroupAssignmentResource { SecurityGroupID = securityGroup.ID };
      return assignment;
    }
  }
}
