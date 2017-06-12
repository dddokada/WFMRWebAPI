#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion

using Commons;

namespace OrgHierarchyCreatorAlternative.OrgHierarchyResources
{
  public class OrganizationalHierarchyResource : ParentOrganizationalHierarchyResource
  {
    //[Required]
    public string Name { get; set; }

    //[Required]
    public string LongName { get; set; }

    //[Required]
    public OrganizationalHierarchyLevelResource OrganizationalHierarchyLevelAssignment { get; set; }

    public ParentOrganizationalHierarchyResource ParentOrganizationalHierarchyAssignment { get; set; }

    public Link[] Actions { get; set; }
  }
}