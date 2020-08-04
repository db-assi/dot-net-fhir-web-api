using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_azure_fhir_web_api.IServices;
using Newtonsoft.Json.Linq;

namespace dotnet_azure_fhir_web_api.Services
{
    public class MedicationService : IMedicationService
    {
        private static readonly string[] requestOption = new string[] { "/MedicationRequest/", "/MedicationRequest?patient=" };
        private readonly IResourceFetchService _resource;
        private readonly ILoggerManager _logger;

        public MedicationService(IResourceFetchService resource, ILoggerManager logger)
        {
            _resource = resource;
            _logger = logger;
        }

        public async Task<List<JObject>> GetPatientMedication(string id)
        {
            _logger.LogInfo("Class: MedicationService, Method: GetPatientMedication");
            var json = await _resource.GetAllPages($"{requestOption[1]}{id}");

            return GetMedicationObject(json[0]["entry"]);
        }

        public async Task<List<JObject>> GetPatientMedicationPages(string id, int pages)
        {
            _logger.LogInfo("Class: MedicationService, Method: GetPatientMedicationPages");
            return await _resource.GetPages($"{requestOption[1]}{id}", pages);
        }

        public async Task<JObject> GetSingleMedication(string id)
        {
            _logger.LogInfo("Class: MedicationService, Method: GetSingleMedication");
            return await _resource.GetSingleResource($"{requestOption[0]}{id}");
        }

        private List<JObject> GetMedicationObject(JToken array)
        {
            List<JObject> result = new List<JObject>();
            foreach(var item in array)
            {
                result.Add(new JObject(
                    new JProperty("drug", item["resource"]["medicationCodeableConcept"]["text"]),
                    new JProperty("date", item["resource"]["authoredOn"]),
                    new JProperty("prescriber", item["resource"]["requester"]["display"])
                    ));
            }

            return result;
        }
    }
}
