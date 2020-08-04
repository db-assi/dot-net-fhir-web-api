using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IPatientService
    {
        Task<List<JObject>> GetPatients();
        Task<List<JObject>> GetPatientPages(int pages);
        Task<JObject> GetPatient(string id);
    }
}
