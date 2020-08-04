using dotnet_azure_fhir_web_api.IServices;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.Services
{
    public class VitalSignsService : IVitalSignsService
    {
        private static readonly string[] requestOption = new string[] { "/Observation/", "/Observation?patient=" };
        private readonly IResourceFetchService _resource;
        private readonly ILoggerManager _logger;

        public VitalSignsService(IResourceFetchService resource, ILoggerManager logger)
        {
            _resource = resource;
            _logger = logger;
        }

        public async Task<List<JObject>> GetPatientVitalSigns(string id)
        {
            _logger.LogInfo("Class: VitalSignsService, Method: GetPatientVitalSigns");
            var json = await _resource.GetAllPages($"{requestOption[1]}{id}");
            return CreateVitalSignsObject(json[0]["entry"]);
        }


        private List<JObject> CreateVitalSignsObject(JToken array)
        {
            List<JObject> result = new List<JObject>();

            foreach (var item in array)
            {
                if (((string)item["resource"]["category"][0]["coding"][0]["display"]).Contains("vital-signs") && !((string)item["resource"]["code"]["text"]).Contains("Blood Pressure"))
                {
                    result.Add(new JObject(
                        new JProperty("type", item["resource"]["code"]["text"]),
                        new JProperty("date", item["resource"]["issued"]),
                        new JProperty("value", item["resource"]["valueQuantity"]["value"]),
                        new JProperty("unit", item["resource"]["valueQuantity"]["unit"])
                    ));
                }

            }

            return result;
        }


    }
}
