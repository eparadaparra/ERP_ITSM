using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class TipoMaterialRequest
    {
        [JsonPropertyName("CodigoMaterial")]
        public string EX_CodigoTipoMaterial { get; set; }
    }
    public class TipoMaterial
    {
        [JsonPropertyName("CIType")]
        public string ivnt_ParentAssetType { get; set; }
        [JsonPropertyName("ivnt_AssetSubtype")]
        public string ivnt_SubType { get; set; }
        [JsonPropertyName("CodigoMaterial")]
        public string EX_CodigoTipoMaterial { get; set; }
        [JsonPropertyName("ParentAssetType")]
        public string ivnt_ParentAssetTypeDisplay { get; set; }


    }
}
