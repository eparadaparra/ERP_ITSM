using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class MilestoneRequest
    {
        public string ReleaseLink_RecID { get; set; }
        public string EX_CustID_Link_RecID { get; set; }

    }
    public class Milestone
    {
        [JsonPropertyName("ReleaseLink_RecID")]
        public string ReleaseLink_RecID { get; set; } = string.Empty;

        [JsonPropertyName("ReleaseEnvironment")]
        public string ReleaseEnvironment { get; set; } = "Production";

        [JsonPropertyName("Status")]
        public string Status { get; set; } = "Pending";

        [JsonPropertyName("Subject")]
        public string Subject { get; set; } = "Planning";

        [JsonPropertyName("Description")]
        public string Description { get; set; } = "Validar viabilidad y planear entrega.";

        [JsonPropertyName("StartDate")]
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;

        [JsonPropertyName("SequenceOrder")]
        public string SequenceOrder { get; set; } = "1";

        [JsonPropertyName("EX_LocationID_Link_Category")]
        public string EX_LocationID_Link_Category { get; set; } = "Location";

        [JsonPropertyName("EX_LocationID_Link_RecID")]
        public string EX_LocationID_Link_RecID { get; set; } = string.Empty;

        [JsonPropertyName("EX_CustID_Link_Category")]
        public string EX_CustID_Link_Category { get; set; } = "Account";

        [JsonPropertyName("EX_CustID_Link_RecID")]
        public string EX_CustID_Link_RecID { get; set; } = string.Empty;

        [JsonPropertyName("OwnerTeam")]
        public string OwnerTeam { get; set; } = "Entrega de servicios";
    }
}
