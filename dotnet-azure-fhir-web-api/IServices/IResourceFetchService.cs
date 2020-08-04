using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IResourceFetchService
    {
        Task<JObject> GetSingleResource(string requestOptions);
        Task<List<JObject>> GetMultipleResources(string requestOptions, List<string> ids);
        Task<List<JObject>> GetAllPages(string requestOptions);
        Task<List<JObject>> GetPages(string requestOptions, int pages);
    }
}
