#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
namespace SiteExample
{
  public class SiteMailingAddressResource
  {
    public int ID { get; set; }
    //[Required]
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    //[Required]
    public string City { get; set; }
    public string State { get; set; }
    //[Required]
    public string CountryCode { get; set; }
    //[Required]
    public string ZipCode { get; set; }
    public string County { get; set; }
    public string WorkPhone { get; set; }
    public string FaxNumber { get; set; }
    //[Required]
    public string Email { get; set; }
  }
}