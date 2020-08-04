using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IMedicationService
    {
        Task<List<JObject>> GetPatientMedication(string id);
        Task<List<JObject>> GetPatientMedicationPages(string id, int pages);
        Task<JObject> GetSingleMedication(string id);
    }
}
