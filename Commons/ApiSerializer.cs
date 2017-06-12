#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Commons
{
  internal class ApiSerializer
  {
    private static readonly JsonSerializer Serializer = new JsonSerializer();

    public static void WriteToStream(Stream ms, object obj)
    {
      var jsonTextWriter = new JsonTextWriter(new StreamWriter(ms));
      Serializer.Serialize(jsonTextWriter, obj);
      jsonTextWriter.Flush();
      ms.Position = 0;
    }

    public static T ParseResponse<T>(HttpResponseMessage response)
    {
      var stream = response.Content.ReadAsStreamAsync().Result;
      JsonReader reader = new JsonTextReader(new StreamReader(stream));
      var ret = Serializer.Deserialize<T>(reader);
      return ret;
    }

    private static void PrintJson(object obj)
    {
      var builder = new StringBuilder();
      var jsonTextWriter = new JsonTextWriter(new StringWriter(builder));
      Serializer.Serialize(jsonTextWriter, obj);
      jsonTextWriter.Flush();
      Debug.WriteLine(builder.ToString());
    }
  }
}
