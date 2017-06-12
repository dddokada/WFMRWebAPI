#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Net.Http;

namespace Commons
{
  public class ApiException : Exception
  {
    public HttpResponseMessage Response { get; private set; }

    private ErrorBody _ErrorBody;
    public ErrorBody ErrorBody
    {
      get { return _ErrorBody; }
      private set { _ErrorBody = value ?? new ErrorBody(); }
    }

    public ApiException(HttpResponseMessage response)
      : base("Received a " + response.StatusCode + " from server.")
    {
      Response = response;
      ErrorBody = ApiSerializer.ParseResponse<ErrorBody>(response);
    }
  }
}
