#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Globalization;
using System.Web;

namespace Commons
{
  public class UrlFormatter
  {
    public static string AddNameToRoot(string apiRoot, string name)
    {
      string encodedName = HttpUtility.UrlEncode(name);
      return AddParameter(apiRoot, "name", encodedName);
    }

    public static string AddSiteToRoot(string apiRoot, int siteId)
    {
      return AddParameter(apiRoot, "siteIDFilter", siteId.ToString(CultureInfo.InvariantCulture));
    }

    public static string AddParameter(string apiRoot, string parameter, string value)
    {
      return apiRoot + "?" + parameter + "=" + value;
    }
  }
}
