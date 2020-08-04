using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface ILabResultsService
    {
        Task<List<JObject>> GetPatientLabResults(string id);
    }
}
