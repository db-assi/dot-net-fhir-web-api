using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IProtectedWebApiCallerService
    {
        Task<JObject> ProtectedWebApiCaller(string webApiUrl);
    }
}
