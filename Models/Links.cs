using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class LinkReleaseRequest
    {
        public required string tipo { get; set; }
        public required string recIdRelease { get; set; }
        public required string recIdRelationship { get; set; }
    }

    public class Links
    {
        [JsonPropertyName("code")]
        public string code { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("message")]
        public string[] message { get; set; }

        [JsonPropertyName("help")]
        public string help { get; set; }
    }

    public class CloneLinkReleaseRequest
    {
        public required string recId { get; set; }
        public required string parentRecId { get; set; }
    }
}
