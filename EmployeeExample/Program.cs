#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Net;
using System.Diagnostics;
using Commons;
using EmployeeExample.UserResources;

namespace EmployeeExample
{
  public class Program
  {
//    public static readonly Uri BaseUri = new Uri("http://walmart-jp-wfmr-c.jdadelivers.com/data/retailwebapi/api/v1-beta3/");
        public static readonly Uri BaseUri = new Uri("https://walmart-jp-wfmr-c.jdadelivers.com/Retail/data/retailwebapi/api/v1-beta3/");
                                                      

        //ServicePointManager.SecurityProtocol = BaseUri.Tls11 | SecurityProtocolType.Tls12;
        // ServicePoint mySP = ServicePointManager.FindServicePoint(BaseUri);
        //mySP.SecurityProtocol = SecurityProtocolType.Tls12;

        public const string Username = "odai";
       public const string Password = "e123456";

    public static void Main()
    {
            Console.WriteLine("AA?");
            var client = new ApiClient(BaseUri, Username, Password);
            Console.WriteLine(BaseUri);
            System.Threading.Thread.Sleep(1000);
            try
            {
        var userCreator = new UserCreator(client);

        Console.WriteLine("createdUser");
        var createdUser = userCreator.CreateUser();
        Console.WriteLine("createdUser End");
           System.Threading.Thread.Sleep(1000);

         var employeeCreator = new EmployeeCreator(client);
        employeeCreator.CreateEmployee(createdUser);
      }
      catch (AggregateException aggregateException)
      {
        var innerException = aggregateException.InnerException;
        if (innerException is ApiException)
        {
          var apiException = innerException as ApiException;
          var responseBody = apiException.Response.Content.ReadAsStringAsync().Result;
          Console.Error.WriteLine(responseBody);
          Debug.WriteLine(responseBody);
        }
        else
        {
          Console.Error.WriteLine(aggregateException);
        }
      }
      catch (Exception e)
      {
        Debug.WriteLine(e);
        Console.Error.WriteLine(e);
      }
      finally
      {
        Console.WriteLine("Finished! Press Enter");
        Console.ReadLine();
      }
    }
  }
}
