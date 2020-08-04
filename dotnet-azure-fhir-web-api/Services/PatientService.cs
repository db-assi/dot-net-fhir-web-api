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
            var json = await _resource.GetAllPages(requestOption);
            return GetPatientsObject(json);
        }

        public async Task<List<JObject>> GetPatientPages(int pages)
        {
            _logger.LogInfo("Class: PatientService, Method: GetPatientPages");
            var json = await _resource.GetPages(requestOption, pages);
            return GetPatientsObject(json);
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
                new JProperty("id", $"{json["id"]}"),
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

        private List<JObject> GetPatientsObject(List<JObject> list)
        {
            List<JObject> result = new List<JObject>();

            foreach (var bundle in list)
            {
                var array = (JArray)bundle["entry"];

                foreach (var json in array)
                {
                    string name = "";
                    foreach (var i in json["resource"]["name"][0]["given"])
                    {
                        name = name + " " + i;
                    }
                    result.Add(new JObject(
                        new JProperty("id", $"{json["resource"]["id"]}"),
                        new JProperty("name", $"{name}{" "}{json["resource"]["name"][0]["family"]}"),
                        new JProperty("line", $"{json["resource"]["address"][0]["line"][0]}"),
                        new JProperty("city", $"{json["resource"]["address"][0]["city"]}"),
                        new JProperty("state", $"{json["resource"]["address"][0]["state"]}"),
                        new JProperty("postcode", $"{json["resource"]["address"][0]["postalCode"]}"),
                        new JProperty("country", $"{json["resource"]["address"][0]["country"]}"),
                        new JProperty("ethnicity", json["resource"]["extension"][0]["extension"][0]["valueCoding"]["display"]),
                        new JProperty("gender", json["resource"]["gender"]),
                        new JProperty("birthDate", json["resource"]["birthDate"]),
                        new JProperty("maritalStatus", json["resource"]["maritalStatus"]["text"]),
                        new JProperty("language", json["resource"]["communication"][0]["language"]["text"])
                        ));
                }
            }
            return result;
        }


    }
}
