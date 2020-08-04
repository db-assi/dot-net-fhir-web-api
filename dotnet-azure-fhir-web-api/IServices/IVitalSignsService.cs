using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IVitalSignsService
    {
        Task<List<JObject>> GetPatientVitalSigns(string id);
    }
}
