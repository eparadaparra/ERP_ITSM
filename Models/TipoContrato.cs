using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class TipoContrato
    {
        public string RecId { get; set; }

        [JsonPropertyName("Descripcion")]
        public string EX_DescripcionTipoContrato { get; set; }
    }
}