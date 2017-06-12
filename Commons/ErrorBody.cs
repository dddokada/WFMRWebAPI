#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;

namespace Commons
{
  public class ErrorBody
  {
    private string _UserMessage;
    private string _ErrorCode;

    public String UserMessage
    {
      get { return _UserMessage ?? ""; }
      set { _UserMessage = value; }
    }

    public String ErrorCode
    {
      get { return _ErrorCode ?? ""; }
      set { _ErrorCode = value; }
    }
  }
}
