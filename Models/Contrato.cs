using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class ContratoRequest
    {
        [JsonPropertyName("custID")]
        public required string CustID { get; set; }

        [JsonPropertyName("SiteId")]
        public required string ivnt_AssetLocation { get; set; }

        [JsonPropertyName("AgreementTypeID")]
        public required string EX_TipoContrato_Link { get; set; }

        [JsonPropertyName("ContractId")]
        public required string ivnt_InternalID { get; set; }

        [JsonPropertyName("User21")]
        public string EX_MesesRestantes { get; set; }

        [JsonPropertyName("expiredate")]
        public string ivnt_ExpiryDate { get; set; }

        [JsonPropertyName("Crtd_Prog")]
        public string Program { get; set; }

        [JsonPropertyName("Crtd_DateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("Crtd_User")]
        public string User { get; set; }
    }
    public class Contrato
    {
        public string RecId { get; set; }

        [JsonPropertyName("CIType")]
        public string CIType { get; set; } = "Contract";

        [JsonPropertyName("CITypeList")]
        public string CITypeList { get; set; } = "Contrato";

        [JsonPropertyName("CustID")]
        public string CustID { get; set; }

        [JsonPropertyName("EX_CustID_Link")]
        public string EX_CustID_Link { get; set; }

        [JsonPropertyName("EX_CustID_Link_RecID")]
        public string EX_CustID_Link_RecID { get; set; }

        [JsonPropertyName("EX_MesesRestantes")]
        public string EX_MesesRestantes { get; set; }

        [JsonPropertyName("EX_TipoContrato_Link")]
        public string EX_TipoContrato_Link { get; set; }

        [JsonPropertyName("EX_TipoContrato_Link_Category")]
        public string EX_TipoContrato_Link_Category { get; set; } = "EX_ObjTiposContratos";

        [JsonPropertyName("EX_TipoContrato_Link_RecID")]
        public string EX_TipoContrato_Link_RecID { get; set; }

        [JsonPropertyName("ivnt_AssetFullType")]
        public string ivnt_AssetFullType { get; set; } = "Contract";

        [JsonPropertyName("ivnt_AssetLocation")]
        public string ivnt_AssetLocation { get; set; }

        [JsonPropertyName("ivnt_ContractStatus")]
        public string ivnt_ContractStatus { get; set; } = "Active";

        [JsonPropertyName("ivnt_ExpiryDate")]
        public string ivnt_ExpiryDate { get; set; }

        [JsonPropertyName("ivnt_InternalID")]
        public string ivnt_InternalID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Program")]
        public string Program { get; set; }

        [JsonPropertyName("DateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("User")]
        public string User { get; set; }
    }
}
