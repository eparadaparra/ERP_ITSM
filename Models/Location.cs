using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class LocationRequest 
    {
        [JsonPropertyName("CustID")]
        public required string CustID { get; set; }

        [JsonPropertyName("ShiptoID")]
        public required string EX_IdSitio { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Addr1")]
        public string Address { get; set; }

        [JsonPropertyName("Addr2")]
        public string EX_Colonia { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Zip")]
        public string Zip { get; set; }

        [JsonPropertyName("Country")]
        public string Country { get; set; }

        [JsonPropertyName("securityentrycode")]
        public string EX_PlazaCliente { get; set; }

        [JsonPropertyName("Crtd_Prog")]
        public string Program { get; set; }

        [JsonPropertyName("Crtd_DateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("Crtd_User")]
        public string User { get; set; }
    }
    public class Location
    {
        public string RecId { get; set; }

        [JsonPropertyName("EX_CustID_Link")]
        public string EX_CustID_Link { get; set; }

        [JsonPropertyName("CustID")]
        public string CustID { get; set; }

        [JsonPropertyName("ShiptoID")]
        public string EX_IdSitio { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Addr1")]
        public string Address { get; set; }

        [JsonPropertyName("Addr2")]
        public string EX_Colonia { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Zip")]
        public string Zip { get; set; }

        [JsonPropertyName("Country")]
        public string Country { get; set; }

        [JsonPropertyName("securityentrycode")]
        public string EX_PlazaCliente { get; set; }

        [JsonPropertyName("CustID_ShiptoID")]
        public string EX_Unick { get; set; }

        [JsonPropertyName("Crtd_Prog")]
        public string Program { get; set; }

        [JsonPropertyName("Crtd_DateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("Crtd_User")]
        public string User { get; set; }
    }

}
