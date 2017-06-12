#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System.Net.Http;

namespace Commons
{
  public class ApiCreateException<T> : ApiException
  {
    public T PostedContent { get; private set; }

    public ApiCreateException(HttpResponseMessage response, T postedContent)
      : base(response)
    {
      PostedContent = postedContent;
    }
  }
}
