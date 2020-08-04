using dotnet_azure_fhir_web_api.IServices;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.Services
{
    public class PatientService : IPatientService
    {
        private static readonly string requestOption = "/Patient/";
        private readonly IResourceFetchService _resource;
        private readonly ILoggerManager _logger;

        public PatientService(IResourceFetchService resource, ILoggerManager logger)
        {
            _resource = resource;
            _logger = logger;
        }

        public async Task<List<JObject>> GetPatients()
        {
            _logger.LogInfo("Class: PatientService, Method: GetAllPages");
            return await _resource.GetAllPages(requestOption);
        }

        public async Task<List<JObject>> GetPatientPages(int pages)
        {
            _logger.LogInfo("Class: PatientService, Method: GetPatientPages");
            return await _resource.GetPages(requestOption, pages);
        }

        public async Task<JObject> GetPatient(string id)
        {
            _logger.LogInfo("Class: PatientService, Method: GetPatient");
            var json = await _resource.GetSingleResource($"{requestOption}{id}");
            return GetPatientObject(json);

        }

        private JObject GetPatientObject(JObject json)
        {
            string name = "";
            foreach (var item in json["name"][0]["given"])
            {
                name = name + " " + item;
            }
            return new JObject(
                new JProperty("title", $"{json["name"][0]["prefix"][0]}"),
                new JProperty("name", $"{name}{" "}{json["name"][0]["family"]}"),
                new JProperty("line", $"{json["address"][0]["line"][0]}"),
                new JProperty("city", $"{json["address"][0]["city"]}"),
                new JProperty("state", $"{json["address"][0]["state"]}"),
                new JProperty("postcode", $"{json["address"][0]["postalCode"]}"),
                new JProperty("country", $"{json["address"][0]["country"]}"),
                new JProperty("ethnicity", json["extension"][0]["extension"][0]["valueCoding"]["display"]),
                new JProperty("gender", json["gender"]),
                new JProperty("birthDate", json["birthDate"]),
                new JProperty("maritalStatus", json["maritalStatus"]["text"]),
                new JProperty("language", json["communication"][0]["language"]["text"])
                );
        }
    }
}
