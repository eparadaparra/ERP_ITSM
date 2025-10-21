using ERP_ITSM.Models;
using ERP_ITSM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERP_ITSM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ERPController : ControllerBase
    {
        private readonly IServices _services;

        private Release _release            = new Release();
        private Milestone _milestone        = new Milestone();
        private Location _location          = new Location();
        private Contrato _contrato          = new Contrato();
        private CI _ci                      = new CI();
        private TipoMaterial _tipoMarerial  = new TipoMaterial();
        private Service _service            = new Service();

        public ERPController(IServices servicesAPI)
        {
            _services = servicesAPI;
        }

        #region Account
        [HttpPost]
        [Route("Account")]
        public async Task<IActionResult> GetAccount([FromBody] GetAccountRequest body)
        {
            string custID = body?.custID?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(custID))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "custID is required", data = "" });
            }
            string filter = $"CustID eq '{custID}'";
            string seleccion = "RecID, CustID, Name";

            var resp = await _services.FilterObj("Account", filter, seleccion /*, idLog*/);

            if (resp.ContainsKey("logID"))//|| string.IsNullOrEmpty(customer.RecID))
            {
                return Ok(new { status = StatusCodes.Status200OK, message = "No se encontró Customer", data = "" });
            }
            else
            {
                Account customer = resp.ToObject<Account>();
                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = customer });
            }
        }
        #endregion

        #region Location
        [HttpPost]
        [Route("Location")]
        public async Task<IActionResult> GetLocationt([FromBody] GetLocationRequest body)
        {
            string custID = body?.custID?.Trim() ?? string.Empty;
            string idSitio = body?.idSitio?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(custID) || string.IsNullOrEmpty(idSitio))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "custID is required", data = "" });
            }
            string filter = $"CustID eq '{custID}' AND EX_IdSitio eq '{idSitio}'";
            string seleccion = "CustID, RecId, EX_IdSitio, Name";

            var resp = await _services.FilterObj("Location", filter, seleccion /*, idLog*/);
            //Console.WriteLine(resp.GetType());

            if (resp.ContainsKey("logID"))
            {
                return Ok(new { status = StatusCodes.Status200OK, message = "No se encontró el Sitio", data = "" });
            }
            else
            {
                GetLocation location = resp.ToObject<GetLocation>();
                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = location });
            }
        }
        #endregion

        #region Find Release
        [HttpPost]
        [Route("Release")]
        public async Task<IActionResult> GetRelease([FromBody] GetTicketRequest body)
        {
            string releaseNum = body?.no_Ticket?.Trim() ?? string.Empty;
            string idSitio = body?.no_Sitio?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(releaseNum) || string.IsNullOrEmpty(idSitio))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "No Ticket and No Sitio is required", data = "" });
            }

            string filter = $"ReleaseNumber eq {releaseNum} AND EX_TipoRelease eq 'Nuevo Sitio' ";  //  AND EX_LocationID_Link_RecID eq '{idSitio}' 

            string seleccion = "ReleaseNumber, EX_LocationID_Link_RecID, EX_CustID_Link_RecID";

            var respRelease = await _services.FilterObj( "Release", filter, seleccion );
            //Console.WriteLine(resp.GetType());

            if  ( respRelease.ContainsKey("logID") )
            {
                return Ok(new { status = StatusCodes.Status200OK, message = "No se encontró el Release", data = "" });
            }
            else
            {
                var recIdSitio = respRelease["EX_LocationID_Link_RecID"].ToString();
                var recCustId = respRelease["EX_CustID_Link_RecID"].ToString();

                var respLocation = await _services.FilterObj( "Location", $"recId eq '{recIdSitio}' AND EX_CustID_Link_RecID eq '{recCustId}'", "EX_IdSitio");

                if (respLocation["EX_IdSitio"].ToString() != idSitio)
                {
                    return Ok(new { status = StatusCodes.Status200OK, message = $"No coincíde el sitio {idSitio} para el Release {releaseNum}", data = "" });
                }
                else {

                    respRelease.Remove("EX_CustID_Link_RecID");
                    respRelease["ReleaseNumber"] = releaseNum;
                    respRelease["EX_LocationID_Link_RecID"] = idSitio;
                    Console.WriteLine(respRelease);

                    findRelease release = respRelease.ToObject<findRelease>();

                    return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = release });
                }
                    
            }
        }
        #endregion

        #region External Contact
        [HttpPost]
        [Route("ExternalContact")]
        public async Task<IActionResult> GetContact([FromBody] GetExternalContactRequest body)
        {
            string email = body?.email?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "email is required", data = "" });
            }
            string filter = $"PrimaryEmail eq '{email}'";
            string seleccion = "PrimaryEmail, RecId, DisplayName";

            var resp = await _services.FilterObj("ExternalContact", filter, seleccion /*, idLog*/);
            //Console.WriteLine(resp.GetType());

            if (resp.ContainsKey("logID"))
            {
                return Ok(new { status = StatusCodes.Status200OK, message = "No se encontró Contacto", data = "" });
            }
            else
            {
                ExternalContact contacto = resp.ToObject<ExternalContact>();
                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = contacto });
            }
        }
        #endregion

        #region Tipo Material
        [HttpPost]
        [Route("TipoMaterial")]
        public async Task<IActionResult> GetTipoMaterial([FromBody] TipoMaterialRequest body)
        {
            string codigoMaterial = body?.EX_CodigoTipoMaterial?.Trim() ?? string.Empty;
            _tipoMarerial.EX_CodigoTipoMaterial = codigoMaterial;
            string filter = String.Empty;
            string seleccion = String.Empty;

            if (string.IsNullOrEmpty(codigoMaterial))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Tipo Material is required", data = "" });
            }

            filter = $"EX_CodigoTipoMaterial eq '{codigoMaterial}'";
            seleccion = "ivnt_ParentAssetType, ivnt_SubType, EX_CodigoTipoMaterial, ivnt_ParentAssetType_Valid, RecId";

            var resp = await _services.FilterObj("ivnt_AssetSubType", filter, seleccion );

            if (!resp.ContainsKey("RecId"))
            {
                return Ok(new { status = StatusCodes.Status200OK, message = "No se encontró Código de Material", data = "" });
            }

            SetValuesModels( _tipoMarerial, resp!);

            var recIdAssetType = resp["ivnt_ParentAssetType_Valid"].ToString();

            filter = $"RecId eq '{recIdAssetType}'";
            seleccion = "ivnt_ParentAssetTypeDisplay";

            var respAsstType = await _services.FilterObj("ivnt_ParentAssetType", filter, seleccion);

            SetValuesModels(_tipoMarerial, respAsstType!);
                
            return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = _tipoMarerial });
            
        }
        #endregion

        #region Tipo Servicio
        [HttpPost]
        [Route("Service")]
        public async Task<IActionResult> GetService([FromBody] GetServiceRequest body)
        {
            string codigoServicio = body?.Summary?.Trim() ?? string.Empty;

            string filter = String.Empty;
            string seleccion = String.Empty;

            if (string.IsNullOrEmpty(codigoServicio))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Csódigo Servicio is required", data = "" });
            }

            filter = $"Summary eq '{codigoServicio}' AND CIType eq 'Service'";
            seleccion = "RecId, Name, Summary";

            var resp = await _services.FilterObj("Service", filter, seleccion);

            if (!resp.ContainsKey("RecId"))
            {
                return Ok(new { status = StatusCodes.Status200OK, message = "No se encontró el Servicio", data = "" });
            }

            SetValuesModels(_service, resp!);

            return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = _service });

        }
        #endregion

        #region Crea Release
        [HttpPost]
        [Route("NuevoRelease")]
        public async Task<IActionResult> SetRelease([FromBody] ReleaseRequest body)
        {
            try
            {
                #region Primero valida si contiene los datos necesarios
                string custID = body?.EX_CustID_Link_RecID?.Trim() ?? string.Empty;
                MilestoneRequest milestoneRequest = new MilestoneRequest();

                if (string.IsNullOrEmpty(custID))
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "CustID is required", data = "{}" });
                }
                #endregion

                #region Valida si Existe el Customer
                    string filter = $"CustID eq '{custID}'";
                    string seleccion = "RecId";

                    var respAcc = await _services.FilterObj("Account", filter, seleccion);

                    if (!respAcc.ContainsKey("RecId"))
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Customer No Existe", data = new { custID = custID } });
                    }
                #endregion

                #region Crea Release
                    body!.EX_CustID_Link_RecID = respAcc["RecId"]!.ToString();
                    milestoneRequest.EX_CustID_Link_RecID = respAcc["RecId"]!.ToString();

                    SetValuesModels(_release, body);

                    var resultRelease = await _services.InsUpdObj("Release", _release);
                    //Console.WriteLine(resultRelease.GetType());
                    var jRelease = JsonConvert.DeserializeObject<JArray>(resultRelease.ToString());

                    if (jRelease == null || jRelease.Count == 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Error al crear Release", data = "{}" });
                    }
                #endregion

                #region Crea Milestone
                    var recIdRelease = jRelease[0]["RELEASEPROJECT"]["RecId"].ToString();
                    _release.RecId = recIdRelease;
                    milestoneRequest.ReleaseLink_RecID = recIdRelease;

                    SetValuesModels(_milestone, milestoneRequest);

                    var resultMilestone = await _services.InsUpdObj("Milestone", _milestone);

                    var jMilestone = JsonConvert.DeserializeObject<JArray>(resultMilestone.ToString());

                    var recIdMilestone = jMilestone[0]["RELEASEMILESTONE"]["RecId"].ToString();

                    if (jMilestone == null || jMilestone.Count == 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Error al crear Milestone", data = "{}" });
                    }
                #endregion
                
                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = new { RecId = _release.RecId } });
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                return Ok(new { status = StatusCodes.Status200OK, message = $"Error: {ex.Message}", data = "{}" });
            }

        }
        #endregion

        #region Crea Sitio
        [HttpPost]
        [Route("NuevoSitio")]
        public async Task<IActionResult> SetLocation([FromBody] LocationRequest body)
        {
            try
            {
                #region Primero valida si contiene los datos necesarios
                string custID = body?.CustID?.Trim() ?? string.Empty;
                string idSitio = body?.EX_IdSitio?.Trim()?? string.Empty;

                if (string.IsNullOrEmpty(custID) || string.IsNullOrEmpty(idSitio))
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "custID y IdSitio is required", data = "{}" });
                }
                #endregion

                #region Valida si Existe el Customer
                string filter = $"CustID eq '{custID}'";
                string seleccion = "RecId";

                var respAcc = await _services.FilterObj("ACCOUNT", filter, seleccion);

                if (!respAcc.ContainsKey("RecId"))
                {
                    return Ok(new { status = StatusCodes.Status200OK, message = "Customer No Existe", data = new { custID = custID } });
                }
                #endregion

                #region Valida si Existe el Sitio
                filter = $"CustID eq '{custID}' AND EX_IdSitio eq '{idSitio}'";
                seleccion = "RecId";

                var resp = await _services.FilterObj("Location", filter, seleccion);

                if (resp.ContainsKey("RecId"))
                {
                    _location.RecId = resp["RecId"].ToString();
                    SendUpdate(_location, body!);

                    return Ok(new { status = StatusCodes.Status200OK, message = "Sitio Existente", data = new { RecId = resp["RecId"].ToString() } });
                }
                #endregion

                SetValuesModels(_location, body!);
                _location.EX_CustID_Link = custID;
                _location.EX_Unick = $"{custID}_{idSitio}";

                var result = await _services.InsUpdObj("Location", _location);  //Console.WriteLine(result.GetType());

                var json = JsonConvert.DeserializeObject<JArray>(result.ToString());

                string recId = json[0]["LOCATION"]["RecId"].ToString();

                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = new { RecId = recId } });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Ok(new { status = StatusCodes.Status200OK, message = $"Error: {ex.Message}", data = "{}" });
            }
        }
        #endregion

        #region Crea Contrato
        [HttpPost]
        [Route("NuevoContrato")]
        public async Task<IActionResult> SetContrato([FromBody] ContratoRequest body)
        {
            try
            {
                #region Primero valida si contiene los datos necesarios
                    string custID       = body?.CustID?.Trim() ?? string.Empty;
                    string idSitio      = body?.ivnt_AssetLocation?.Trim() ?? string.Empty;
                    string tipoContrato = body?.EX_TipoContrato_Link?.Trim() ?? string.Empty;
                    string contrato     = body?.ivnt_InternalID?.Trim() ?? string.Empty;
                    string recIdAcc     = String.Empty;
                    string recIdLoc     = String.Empty;
                    string recIdTipoCnt = String.Empty;
                    string filter       = String.Empty;
                    string seleccion    = String.Empty;

                    if (string.IsNullOrEmpty(custID) || string.IsNullOrEmpty(idSitio) || string.IsNullOrEmpty(tipoContrato) || string.IsNullOrEmpty(contrato))
                    {
                        return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "custID, IdSitio TipoContrato and contrato is required", data = "{}" });
                    }
                #endregion

                #region Valida si Existe el Customer
                filter = $"CustID eq '{custID}'";
                    seleccion = "RecId";

                    var respAcc = await _services.FilterObj("ACCOUNT", filter, seleccion);

                    if (!respAcc.ContainsKey("RecId"))
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Customer No Existe", data = new { CustID = custID } });
                    } else {
                        recIdAcc = respAcc["RecId"].ToString();
                }
                    #endregion

                #region Valida si Existe el Sitio
                    filter = $"CustID eq '{custID}' AND EX_IdSitio eq '{idSitio}'";
                    seleccion = "RecId";

                    var respLocation = await _services.FilterObj("Location", filter, seleccion);

                    if (!respLocation.ContainsKey("RecId"))
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = $"Sitio No Existe para cliente {custID}", data = new { ShiptoID = idSitio } });
                    } else
                        {
                            recIdLoc = respLocation["RecId"].ToString();
                        }
                #endregion

                #region Valida si Existe el TipoContrato
                filter = $"EX_TipoContrato eq '{tipoContrato}'";
                    seleccion = "RecId, EX_DescripcionTipoContrato";

                    var respTipoCnt = await _services.FilterObj("EX_ObjTiposContratos", filter, seleccion);

                    if (!respTipoCnt.ContainsKey("RecId"))
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Tipo de Contrato No Existe", data = new { AgreementTypeID = tipoContrato } });
                    } else {
                        recIdTipoCnt = respTipoCnt["RecId"].ToString();
                    }
                #endregion

                #region Valida si Existe el Contrato
                filter = $"ivnt_InternalID eq '{contrato}'";
                seleccion = "RecId";

                var resp = await _services.FilterObj("CONTRACT", filter, seleccion /*, idLog*/);

                if (resp.ContainsKey("RecId"))
                {
                    _contrato.RecId = resp["RecId"].ToString();
                    _contrato.ivnt_AssetLocation = recIdLoc;
                    _contrato.EX_CustID_Link_RecID = recIdAcc;
                    _contrato.EX_TipoContrato_Link_RecID = recIdTipoCnt;

                    SendUpdate(_contrato, body!);

                    return Ok(new { status = StatusCodes.Status200OK, message = "Contrato Existente", data = new { RecId = resp["RecId"].ToString() } });
                }
                #endregion

                SetValuesModels(_contrato, body);
                _contrato.EX_CustID_Link = custID;
                _contrato.Name = contrato;

                var result = await _services.InsUpdObj("Contract", _contrato);

                var json = JsonConvert.DeserializeObject<JArray>(result.ToString());

                string recId = json[0]["CI__CONTRACT"]["RecId"].ToString();     //JArray locationsArray = result[""] as JArray;

                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = new { RecId = recId } });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Ok(new { status = StatusCodes.Status200OK, message = $"Error: {ex.Message}", data = "{}" });
            }
        }
        #endregion

        #region Crea CI's
        [HttpPost]
        [Route("NuevoCI")]
        public async Task<IActionResult> SetCI([FromBody] CIRequest body)
        {
            try
            {
                #region Primero valida si contiene los datos necesarios
                    string custID       = body?.CustID?.Trim() ?? string.Empty;
                    string sitioId      = body?.ivnt_Location?.Trim() ?? string.Empty;
                    string contrato     = body?.EX_Contrato?.Trim() ?? string.Empty;
                    string tipoMaterial = body?.CIType?.Trim() ?? string.Empty;
                    string serialNumber = body?.SerialNumber?.Trim() ?? string.Empty;
                    string tipoCNT = String.Empty;

                if (string.IsNullOrEmpty(custID) || string.IsNullOrEmpty(contrato) || string.IsNullOrEmpty(sitioId) || string.IsNullOrEmpty(tipoMaterial) || string.IsNullOrEmpty(serialNumber))
                    {
                        return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "custID, Contrato, SitioId and Tipo CI is required", data = "{}" });
                    }
                #endregion

                #region Valida si Existe el Customer
                    string filter = $"CustID eq '{custID}'";
                    string seleccion = "RecId";
                    var respAcc = await _services.FilterObj("Account", filter, seleccion);
                    if (!respAcc.ContainsKey("RecId")) {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Customer No Existe", data = new { custID = custID } }); 
                    } else {
                        _ci.EX_CustID_Link = respAcc["RecId"].ToString();
                }
                #endregion
                
                #region Valida si Existe el Sitio
                    filter = $"CustID eq '{custID}' AND EX_IdSitio eq '{sitioId}'";
                    seleccion = "RecId";
                    var respLocation = await _services.FilterObj("Location", filter, seleccion);
                    if (!respLocation.ContainsKey("RecId"))
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Sitio No Existe", data = new { ShiptoID = sitioId } });
                    } else
                    {
                        _ci.ivnt_AssetLocation = respLocation["RecId"]!.ToString();
                }
                #endregion
                
                #region Valida si Existe el Contrato
                    filter = $"ivnt_InternalID eq '{contrato}'";
                    seleccion = "RecId";
                    var respContract = await _services.FilterObj("Contract", filter, seleccion);
                    if (!respContract.ContainsKey("RecId"))
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Contrato No Existe", data = new { contrato = contrato } });
                    } else
                    {
                        _ci.EX_ParentLink = respContract["RecId"]!.ToString();
                        tipoCNT = RelationshipName("CI_CONTRATO");
                }
                #endregion

                #region Valida si Existe el Ci
                    filter = $"EX_Contrato eq '{contrato}' AND SerialNumber eq '{serialNumber}'";
                    seleccion = "RecId";

                    var respCI = await _services.FilterObj("CI", filter, seleccion );

                    if (respCI.ContainsKey("RecId"))
                    {
                        _ci.RecId               = respCI["RecId"].ToString();

                        await _services.LinkObj("CI", _ci.RecId, tipoCNT, respContract["RecId"]!.ToString());
                        await _services.LinkObj("CI", _ci.RecId, "EX_CIAssocCIContract2", respContract["RecId"]!.ToString());

                        SendUpdate(_ci, body!);

                        return Ok(new { status = StatusCodes.Status200OK, message = "CI Existente", data = new { RecId = respCI["RecId"].ToString() } });
                    }
                #endregion

                #region Crea CI

                SetValuesModels(_ci, body);
                JObject newCI = JObject.FromObject(_ci);

                newCI["ivnt_AssetFullType"] = body?.ivnt_ParentAssetTypeDisplay?.Trim() + " - " + newCI["ivnt_AssetSubtype"]!.ToString().Trim();
                newCI["EX_Linea"]           = newCI["ivnt_AssetSubtype"].ToString() == "Linea" ? newCI["EX_Linea"].ToString() : "";
                newCI["EX_PlanDatos"]       = newCI["ivnt_AssetSubtype"].ToString() == "Linea" ? newCI["EX_Imei"].ToString() : "";
                newCI["Name"]               = String.Concat(contrato, " - ", (newCI["ivnt_AssetSubtype"]!.ToString() == "Linea") ? newCI["EX_Linea"].ToString() : serialNumber);
                newCI["EX_CustID_Link"]     = newCI["CustID"]!.ToString();
                newCI["ivnt_AssetLocation"] = newCI["ivnt_Location"]!.ToString();
                newCI["EX_ParentLink"]      = newCI["EX_Contrato"]!.ToString();

                var result = await _services.InsUpdObj("CI", newCI); // Console.WriteLine(result.GetType());

                var json = JsonConvert.DeserializeObject<JArray>(result.ToString());

                string recId = json[0]["CI"]["RecId"].ToString();     //JArray locationsArray = result[""] as JArray;

                await _services.LinkObj("CI", recId, tipoCNT, respContract["RecId"]!.ToString());
                await _services.LinkObj("CI", recId, "EX_CIAssocCIContract2", respContract["RecId"]!.ToString());

                return Ok(new { status = StatusCodes.Status200OK, message = "Successed", data = new { recId = recId } });
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Ok(new { status = StatusCodes.Status200OK, message = $"Error: {ex.Message}", data = "{}" });
            }
        }
        #endregion

        #region Obtiene Links de Objetos
        [HttpPost]
        [Route("GetsLinks")]
        public async Task<IActionResult> GetLink([FromBody] ObjParentLink body)
        {
            string recIdRelease = body?.recId?.Trim() ?? string.Empty;
            string recIdName = body?.objParent?.Trim() ?? string.Empty;
            string tipo = String.Empty;
            List<string> links = new List<string>();

            if (string.IsNullOrEmpty(recIdRelease) || string.IsNullOrEmpty(recIdName))
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "recId and objName is required", data = "" });
            } //ReleaseProjectAssocCI

            tipo = RelationshipName("RELEASE_CI");

            links = await _services.GetLinksObj(recIdName, recIdRelease, tipo);

            return Ok(new { status = StatusCodes.Status200OK, message = (links.Count != 0) ? $"{links.Count} CI's asignados" : "No hay CI's asignados", data = links });

        }
        #endregion

        #region Link Object to Release
        [HttpPost]
        [Route("LinkToRelease")]
        public async Task<IActionResult> SetLink([FromBody] LinkReleaseRequest body)
        {
            try
            {
                string tipo = body?.tipo?.Trim().ToUpper() ?? string.Empty;
                string recIdRelease = body?.recIdRelease?.Trim() ?? string.Empty;
                string recIdRelationship = body?.recIdRelationship?.Trim() ?? string.Empty;

                tipo = RelationshipName(tipo);

                if (string.IsNullOrEmpty(tipo) || string.IsNullOrEmpty(tipo) || tipo == "SIN_TIPO")
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = $"\"Tipo\": {tipo} is incorrect", data = "" });
                }

                if (string.IsNullOrEmpty(recIdRelease) || string.IsNullOrEmpty(recIdRelationship))
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "recIdRelease and recIdRelationship is required", data = "" });
                }

                var resp = await _services.LinkObj("Release", recIdRelease, tipo, recIdRelationship);

                if (tipo == "RELEASE_CI")
                { 
                }
                //Console.WriteLine(resp.GetType());
                var jResp = JsonConvert.DeserializeObject<Links>(resp.ToString());

                if (jResp.code == "ISM_4000")
                {
                    return Ok(new { status = StatusCodes.Status400BadRequest, message = jResp.message[0].ToString() });
                }
                else
                {
                    if (tipo == "RELEASE_SITIO")
                    {
                        try
                        {
                            var respMilestone = await _services.FilterObj("Milestone", $"ParentLink_RecId eq '{recIdRelease}'", "RecId");
                            if (respMilestone.ContainsKey("RecId"))
                            {
                                var tipoMilestone = RelationshipName("RELEASE_MILESTONE");
                                var recIdMilestone = respMilestone["RecId"].ToString();
                                await _services.LinkObj("Milestone", recIdMilestone, tipoMilestone, recIdRelationship);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al enlazar Milestone con Sitio: {ex.Message}");

                        }
                    }
                }
                return Ok(new { status = StatusCodes.Status200OK, message = jResp.message[0].ToString() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status200OK, message = $"Error: Falló Fnc SetLink - {ex.Message}", data = "{}" });
            }
        }
        #endregion
    
        private void SetValuesModels(object modelo, object body)
        {
            var jObject = JObject.FromObject(body);

            foreach (var property in jObject.Properties())
            {
                //var propInfo = typeof(Location).GetProperty(property.Name);
                var propInfo = modelo.GetType().GetProperty(property.Name);
                if (propInfo != null)
                {
                    propInfo.SetValue(modelo, property.Value.ToString()); 
                }
            }
        }

        private async void SendUpdate(object modelo, object body)
        {
            string objName = modelo.GetType().Name;
            string recIdLOC = String.Empty;
            JObject newObject = JObject.FromObject(modelo);

            switch (objName)
            {
                case "Contrato":
                case "CI":
                    recIdLOC = newObject["ivnt_AssetLocation"]?.ToString() ?? string.Empty;
                    break;
                default:
                    break;
            };

            SetValuesModels(modelo, body);
            newObject = JObject.FromObject(modelo);

            switch (objName) {
                case "Contrato":
                    newObject.Remove("CIType");
                    newObject.Remove("CITypeList");
                    newObject.Remove("EX_TipoContrato_Link");
                    newObject.Remove("EX_TipoContrato_Link_Category");
                    newObject.Remove("ivnt_AssetFullType");
                    newObject.Remove("ivnt_ContractStatus");
                    newObject.Remove("ivnt_InternalID");
                    newObject.Remove("Name");
                    newObject.Remove("EX_CustID_Link");
                    newObject["ivnt_AssetLocation"] = recIdLOC;

                    objName = "Contract";
                    break;
                case "Location":
                    newObject.Remove("EX_CustID_Link");
                    newObject.Remove("CustID");
                    newObject.Remove("EX_IdSitio");
                    newObject.Remove("EX_Unick");
                    objName = "Location";
                    break;
                case "CI":
                    newObject.Remove("Name");
                    newObject.Remove("ivnt_AssetFullType");

                    newObject["EX_Linea"]     = newObject["ivnt_AssetSubtype"].ToString() == "Linea" ? newObject["EX_Linea"].ToString() : "";
                    newObject["EX_PlanDatos"] = newObject["ivnt_AssetSubtype"].ToString() == "Linea" ? newObject["EX_Imei"].ToString() : "";

                    objName = "Ci";
                    break;
            };

            await _services.InsUpdObj(objName, newObject, "UPD");
        }
    
        private string RelationshipName(string tipo)
        {
            return tipo.ToUpper() switch
            {
                "CI_CONTRATO"       => "EX_CIAssocCIContract".ToUpper(),
                "RELEASE_SITIO"     => "EX_ReleaseProjectAssocLocation".ToUpper(),
                "RELEASE_CI"        => "ReleaseProjectAssocCI".ToUpper(),
                "RELEASE_SERVICE"   => "ReleaseProjectAssocCIService".ToUpper(),
                "RELEASE_MILESTONE" => "EX_ReleaseMilestoneAssocLocation".ToUpper(),
                _ => "SIN_TIPO"
            };

        }
    }

}
