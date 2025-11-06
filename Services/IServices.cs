using ERP_ITSM.Models;
using Newtonsoft.Json.Linq;

namespace ERP_ITSM.Services
{
    public interface IServices
    {
        Task<JObject> FilterObj(string objeto, string filtro, string select, string logID = "");

        Task<Object> InsUpdObj(string objeto, Object obj, string metodo = "INS");

        Task<Object> LinkObj(string objParent, string recIdParent, string relationship, string recIdChild, string action = "");

        Task<List<object>> GetLinksObj(string objParent, string recIdParent, string relationship);
    }
}
