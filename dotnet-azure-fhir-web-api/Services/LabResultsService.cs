using dotnet_azure_fhir_web_api.IServices;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.Services
{
    public class LabResultsService : ILabResultsService
    {
        private static readonly string[] requestOption = new string[] { "/DiagnosticReport/", "/DiagnosticReport?patient=" };
        private readonly IResourceFetchService _resource;
        private readonly ILoggerManager _logger;
        private readonly IObservationService _observation;

        public LabResultsService(IResourceFetchService resource, ILoggerManager logger, IObservationService observation)
        {
            _resource = resource;
            _logger = logger;
            _observation = observation;
        }

        public async Task<List<JObject>> GetPatientLabResults(string id)
        {
            _logger.LogInfo("Class: DiagnosticService, Method: GetPatientDiagnostic");
            List<JObject> observations = new List<JObject>();
            observations = await _observation.GetMultipleObservation(await GetObservationIds(id));
            List<JObject> result = new List<JObject>();
            foreach(var observation in observations)
            {
                result.Add(GetLabResultsFromObservation(observation));
            }

            return result;
        }

        private async Task<List<string>> GetObservationIds(string id)
        {
            _logger.LogInfo("Class: DiagnosticService, Method: GetObservationIds");
            var json = await _resource.GetAllPages($"{requestOption[1]}{id}");
            var array = (JArray)json[0]["entry"][0]["resource"]["result"];
            List<string> observationIds = new List<string>();

            foreach (var reference in array)
            {
                observationIds.Add(RemoveSubstring((string)reference["reference"], "Observation/"));
            }

            return observationIds;
        }

        private JObject GetLabResultsFromObservation(JObject json)
        {
            return new JObject(
                    new JProperty("type", json["code"]["text"]),
                    new JProperty("date", json["issued"]),
                    new JProperty("value", json["valueQuantity"]["value"]),
                    new JProperty("unit", json["valueQuantity"]["unit"])
                );
        }

        private string RemoveSubstring(string s, string sb)
        {
            char substringFirstChar = sb[0];
            char substringLastChar = sb[sb.Length - 1];

            int start = s.IndexOf(substringFirstChar);
            int end = s.IndexOf(substringLastChar) + 1;

            return s.Remove(start, end).Trim();
        }
    }
}
