using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class ObjParentLink
    {
        [JsonPropertyName("ObjectId")]
        public string recId { get; set; }
    }
    public class GetLinks
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        public object Value { get; set; } // Usamos JsonElement para manejar ambos tipos
    }
}
