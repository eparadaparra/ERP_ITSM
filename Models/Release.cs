using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class ReleaseRequest
    {
        [JsonPropertyName("CustID")]
        public string EX_CustID_Link_RecID { get; set; }

    }
    public class Release
    {
        [JsonPropertyName("RecId")]
        public string RecId { get; set; }

        [JsonPropertyName("EX_TipoRelease")]
        public string EX_TipoRelease { get; set; } = "Nueva implementación";

        [JsonPropertyName("EX_CustID_Link_Category")]
        public string EX_CustID_Link_Category { get; set; } = "Account";

        [JsonPropertyName("EX_CustID_Link_RecID")]
        public string EX_CustID_Link_RecID { get; set; } = String.Empty;

        [JsonPropertyName("TypeOfRelease")]
        public string TypeOfRelease { get; set; } = "Minor";

        [JsonPropertyName("ReleaseClassification")]
        public string ReleaseClassification { get; set; } = "Standalone";

        [JsonPropertyName("ReleaseBaseType")]
        public string ReleaseBaseType { get; set; } = "Location Based";

        [JsonPropertyName("OwnerTeam")]
        public string OwnerTeam { get; set; } = "Entrega de servicios";

        [JsonPropertyName("Status")]
        public string Status { get; set; } = "Draft";

        [JsonPropertyName("Subject")]
        public string Subject { get; set; } = "Implementación de servicio";

        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = "Implementación de servicio";

        [JsonPropertyName("Impact")]
        public string Impact { get; set; } = "Low";

        [JsonPropertyName("Urgency")]
        public string Urgency { get; set; } = "Low";

        [JsonPropertyName("EX_LocationID_Link_Category")]
        public string EX_LocationID_Link_Category { get; set; } = "Location";

    }
    public class ODataClosedRelease<T>
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<ClosedRelease> Value { get; set; }
    }
    public class ClosedRelease
    {
        [JsonPropertyName("RecId")]
        public string RecId { get; set; }

        [JsonPropertyName("Ticket_Heat")]
        public long ReleaseNumber { get; set; }

        // Propiedades calculadas para fecha y hora
        [JsonProperty("FechaCierre")]
        public string FechaCierre => GetFechaPart();

        [JsonProperty("HoraCierre")]
        public string HoraCierre => GetHoraPart();

        [JsonPropertyName("Estatus")]
        public string Status { get; set; }

        [JsonPropertyName("ClosedDateTime")]
        public string ClosedDateTime { get; set; }

        private string GetFechaPart()
        {
            if (DateTimeOffset.TryParse(ClosedDateTime, out var dateTime))
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        private string GetHoraPart()
        {
            if (DateTimeOffset.TryParse(ClosedDateTime, out var dateTime))
            {
                return dateTime.ToString("HH:mm:ss:fff");
            }
            return string.Empty;
        }
    }
}
