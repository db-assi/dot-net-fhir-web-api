using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IObservationService
    {
        Task<List<JObject>> GetPatientObservations(string id);
        Task<List<JObject>> GetPatientObservationPages(string id, int pages);
        Task<JObject> GetSingleObservation(string id);
        Task<List<JObject>> GetMultipleObservation(List<string> ids);
    }
}
