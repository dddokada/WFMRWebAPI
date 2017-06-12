#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace OrgHierarchyExample.OrgHierarchyResources
{
  public class OrganizationalHierarchyResource : ParentOrganizationalHierarchyResource
  {
    //[Required]
    public string Name { get; set; }

    //[Required]
    public string LongName { get; set; }

    //[Required]
    public int OrganizationalHierarchyLevelAssignmentID { get; set; }

    public int? ParentOrganizationalHierarchyAssignmentID { get; set; }
  }
}