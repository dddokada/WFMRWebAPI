#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteExample
{
  public class OperatingHoursSiteAssignmentResource
  {
    public int ID { get; set; }
    public string Start { get; set; }
    public int OperatingHoursID { get; set; }
    public int SiteID { get; set; }
  }
}
