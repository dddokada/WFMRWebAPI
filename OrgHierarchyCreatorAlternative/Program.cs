#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons;

namespace OrgHierarchyCreatorAlternative
{
  public class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        OrgHierarchyCreator.Go();
      }
      catch (AggregateException aggregateException)
      {
        var innerException = aggregateException.InnerException;
        if (innerException is ApiException)
        {
          var apiException = innerException as ApiException;
          var responseBody = apiException.Response.Content.ReadAsStringAsync().Result;
          Console.Error.WriteLine(responseBody);
        }
        else
        {
          Console.Error.WriteLine(aggregateException);
        }
      }
      catch (Exception e)
      {
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
