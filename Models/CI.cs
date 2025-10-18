using System.Text.Json.Serialization;

namespace ERP_ITSM.Models
{
    public class CIRequest
    {
        [JsonPropertyName("CIType")]
        public string CIType { get; set; }

        [JsonPropertyName("ivnt_AssetSubtype")]
        public string ivnt_AssetSubtype { get; set; }

        [JsonPropertyName("ivnt_ParentAssetTypeDisplay")]
        public string ivnt_ParentAssetTypeDisplay { get; set; }

        [JsonPropertyName("CustID")]
        public string CustID { get; set; }

        [JsonPropertyName("ContractID")]
        public string EX_Contrato { get; set; }

        [JsonPropertyName("Servicio")]
        public string EX_CiService { get; set; }

        [JsonPropertyName("Caracteristica")]
        public string EX_Imei { get; set; }

        [JsonPropertyName("SitioId")]
        public string ivnt_Location { get; set; }

        [JsonPropertyName("Manufid")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("Modelid")]
        public string Model { get; set; }

        [JsonPropertyName("SerialNbr")]
        public string SerialNumber { get; set; }

        [JsonPropertyName("User1")]             // smSvcEquipment
        public string EX_Linea { get; set; }

        [JsonPropertyName("Crtd_Prog")]
        public string Program { get; set; }

        [JsonPropertyName("Crtd_DateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("Crtd_User")]
        public string User { get; set; }
    }
    public class CI
    {
        public string RecId { get; set; }

        [JsonPropertyName("CIType")]
        public string CIType { get; set; }

        [JsonPropertyName("ivnt_AssetSubtype")]
        public string ivnt_AssetSubtype { get; set; }

        [JsonPropertyName("EX_CustID_Link")]
        public string EX_CustID_Link { get; set; }
        //public string EX_CustID_Link_RecId { get; set; }

        [JsonPropertyName("CustID")]
        public string CustID { get; set; }

        [JsonPropertyName("EX_Contrato")]
        public string EX_Contrato { get; set; }

        [JsonPropertyName("EX_ParentLink")]
        public string EX_ParentLink { get; set; }
        //public string EX_ParentLink_RecId { get; set; }

        [JsonPropertyName("EX_CiService")]
        public string EX_CiService { get; set; }

        [JsonPropertyName("EX_Imei")]
        public string EX_Imei { get; set; }

        [JsonPropertyName("ivnt_AssetFullType")]
        public string ivnt_AssetFullType { get; set; }

        [JsonPropertyName("ivnt_AssetLocation")]
        public string ivnt_AssetLocation { get; set; }
        //public string ivnt_AssetLocation_RecId { get; set; }

        [JsonPropertyName("ivnt_Location")]
        public string ivnt_Location { get; set; }

        [JsonPropertyName("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("Model")]
        public string Model { get; set; }

        [JsonPropertyName("SerialNumber")]
        public string SerialNumber { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; } = "Deployment in Progress";

        [JsonPropertyName("EX_Linea")]
        public string EX_Linea { get; set; }

        [JsonPropertyName("EX_PlanDatos")]
        public string EX_PlanDatos { get; set; }

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
