#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace SiteExample
{
  public class SiteResource
  {
    public int ID { get; set; }
    // [Required]
    public string Name { get; set; }
    // [Required]
    public string LongName { get; set; }
    // [EnumValidation(typeof(SiteStatus))]
    public string Status { get; set; }
    // [Required]
    public int TimeZoneAssignmentID { get; set; }
    // [Required]
    public OperatingHoursSiteAssignmentResource[] OperatingHoursSiteAssignments { get; set; }
    // [Required]
    public SiteMailingAddressResource MailingAddress { get; set; }
    public int ParentOrganizationalHierarchyAssignmentID { get; set; }
  }
}