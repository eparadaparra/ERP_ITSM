using ERP_ITSM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Web;
using static ERP_ITSM.Conn.CustomException;

namespace ERP_ITSM.Services
{
    public class Services : IServices
    {
        #region Declaración de Variables

        private static string? _url;

        private static string? _ambiente;

        private static List<string> _lstLogEvent = [];

        #endregion

        public Services()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _url = builder.GetSection("HttpClient:url").Value;
            _ambiente = Boolean.Parse(builder.GetSection("Settings:EnableDev").Value!)
                    ? builder.GetSection("HttpClient:dev").Value
                    : builder.GetSection("HttpClient:pro").Value;
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient _client = new HttpClient();

            _client.DefaultRequestHeaders.AcceptCharset.ParseAdd("utf-8");

            _client.BaseAddress = new Uri(_url!);

            _client.DefaultRequestHeaders.Clear();

            return _client;
        }

        private async Task<string> GetNewGuid()
        {
            return await Task.Run(() => Guid.NewGuid().ToString());
        }

        private async Task CreateLogEvents(string displayName = "", string recId = "", JObject? item = null, string program = "", string dateTime = "", string user = "", string logType = "", string extra = "", string fnc = "")
        {
            string logID = await GetNewGuid();
            _lstLogEvent ??= [];

            var loglist = new List<string> {
                        $"ID LOG: {logID}",
                        $"PROCESO DE {logType} DE OBJETO {displayName} EN IVANTI \"**{fnc}**\"\n",
                        $"INICIA {logType} {extra} --->\n",
                    };

            if (new[] { "ACTUALIZACIÓN", "CREACIÓN", "IMPORTACIÓN" }.Contains(logType))
            {
                loglist.Add($"RecId {displayName}: {recId}");
                loglist.Add($"{item}");
            }
            else
            {
                loglist.Add($"{item}");
            }

            if (program != "")
            {
                loglist.Add($"\n\t*** DATOS ERP {logType} ***");
                loglist.Add($"\tPROGRAMA: {program}");
                loglist.Add($"\tFECHA: {dateTime}");
                loglist.Add($"\tUSUARIO: {user} \n");
            }

            _lstLogEvent.AddRange(loglist);
        }

        private static void ClearLogEvents()
        {
            _lstLogEvent.Clear();
        }
        
        public async Task<JObject> FilterObj(string objeto, string filtro, string select, string logID = "")
        {
            try
            {
                HttpClient client = CreateHttpClient();

                var queryParams = HttpUtility.ParseQueryString(string.Empty);
                queryParams["objeto"] = objeto;
                queryParams["filter"] = filtro;
                queryParams["select"] = select;

                var response = await client.GetAsync($"{_ambiente}/api/Obj/Filter?{queryParams}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    new FileService().CreaJsonFile(errorContent); // Assume FileService exists
                    return null;
                }

                string content = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject<JObject>(content); // Convertimos a JObject para un mejor manejo

                if (logID == "lst")
                {
                    return respActivity;
                }

                var values = respActivity["value"] as JArray;

                if (values != null && values.Count > 0)
                {
                    //new FileService().CreaJsonFile(content); // Assume logging on success too
                    string firstValue = values[0].ToString();
                    return JsonConvert.DeserializeObject<JObject>(firstValue);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                new Logs().WriteExc(ex, logID: logID); // Assume Logs exists
                return null;
            }
        }

        public async Task<Object> InsUpdObj(string objeto, object obj, string metodo = "INS")
        {
            metodo = metodo.ToUpper() switch
            {
                "INS" => "/api/Obj/CreateObjLst",
                "UPD" => "/api/Obj/UpdateObjLst",
                _ => throw new ArgumentException("Método no válido. Use 'INS' o 'UPD'.")
            };

            // Crear la estructura completa
            var transformedObject = new JObject
            {
                ["data"] = new JArray {
                    new JObject
                    {
                        [objeto.ToUpper()] = new JArray
                        {
                            JObject.FromObject(obj)
                        }
                    }
                }
            };

            try
            {
                // Enviar via POST
                var respActivity = await SendToApi(transformedObject, metodo);
              
                //var values = respActivity["recId"] as JArray;
                return respActivity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating object: {ex.Message}", ex);
            }
        }

        private async Task<Object> SendToApi(JObject data, string uri)
        {
            HttpClient client = CreateHttpClient();
            var stringCcontent = new StringContent(
                data.ToString(),
                Encoding.UTF8,
                "application/json"
            );
            var response = await client.PostAsync( String.Concat(_ambiente, uri), stringCcontent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var respActivity = JsonConvert.DeserializeObject(content); // Convertimos a JObject para un mejor manejo
            
            //var values = respActivity["recId"] as JArray;
            return respActivity;
        }

        public async Task<object> LinkObj(string objParent, string recIdParent, string relationship, string recIdChild)
        {
            HttpClient client = CreateHttpClient();

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["objeto"] = objParent;
            queryParams["recIdObj"] = recIdParent;
            queryParams["relation"] = relationship;
            queryParams["recIdRelation"] = recIdChild;

            var response = await client.PatchAsync($"{_ambiente}/api/Obj/Relationships?{queryParams}", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var respActivity = JsonConvert.DeserializeObject(content); // Convertimos a JObject para un mejor manejo

            //var values = respActivity["recId"] as JArray;
            return respActivity;
        }

        public async Task<List<string>> GetLinksObj(string objParent, string recIdParent, string relationship)
        {
            HttpClient client = CreateHttpClient();

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["objeto"] = objParent;
            queryParams["recIdObj"] = recIdParent;
            queryParams["relation"] = relationship;

            var response = await client.GetAsync($"{_ambiente}/api/Obj/Relationships?{queryParams}");
            var content = await response.Content.ReadAsStringAsync();
            var jResponse = JsonConvert.DeserializeObject<GetLinks>(content);
            var recIdLst = new List<string>();

            if( jResponse.Value is JArray jArray )
            {
                foreach (var item in jArray)
                {
                    string recId = item["RecId"]?.ToString();
                    if (!string.IsNullOrEmpty(recId))
                    {
                        recIdLst.Add(recId);
                    }
                }
                return recIdLst;
            }
            else 
            {
                // Caso sin instancias - lista vacía
                Console.WriteLine("No se encontraron instancias.");
                return [];
            }
        }
    }
}
