using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class GetAccountRequest
    {
        public string custID { get; set; }
    }
    public class GetLocationRequest
    {
        public string custID { get; set; }
        public string idSitio { get; set; }
    }
    public class GetTicketRequest
    {
        public string no_Ticket { get; set; }
        public string no_Sitio { get; set; }
    }
    public class GetExternalContactRequest
    {
        public string email { get; set; }
    }
    public class GetServiceRequest
    {
        [JsonPropertyName("ServicseCode")]
        public string Summary { get; set; }
    }

    public class Account
    {
        [JsonPropertyName("CustIDLink_RecID")]
        public required string RecID { get; set; }
        [JsonPropertyName("CustID")]
        public required string CustID { get; set; }
        [JsonPropertyName("Cliente")]
        public required string Name { get; set; }
    }
    public class GetLocation
    {
        [JsonPropertyName("RecID")]
        public required string RecID { get; set; }
        [JsonPropertyName("CustID")]
        public required string CustID { get; set; }
        [JsonPropertyName("sSitio")]
        public required string Name { get; set; }
        [JsonPropertyName("No_Sitio")]
        public required string EX_IdSitio { get; set; }
    }
    public class findRelease
    {
        [JsonPropertyName("no_Ticket")]
        public required string ReleaseNumber { get; set; }
        [JsonPropertyName("no_Sitio")]
        public required string EX_LocationID_Link_RecID { get; set; }
    }
    public class ExternalContact
    {
        [JsonPropertyName("Email")]
        public required string PrimaryEmail { get; set; }
        [JsonPropertyName("Profile_Link_RecID")]
        public required string RecId { get; set; }
        [JsonPropertyName("ProfileFullName")]
        public required string DisplayName { get; set; }
    }
    public class Service
    {
        [JsonPropertyName("RecId")]
        public string RecId { get; set; }

        [JsonPropertyName("Service")]
        public string Name { get; set; }

        [JsonPropertyName("ServicseCode")]
        public string Summary { get; set; }
    }
}
