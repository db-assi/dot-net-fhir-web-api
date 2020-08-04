using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_azure_fhir_web_api.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace donet_azure_fhir_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabResultsController : ControllerBase
    {
        private readonly ILabResultsService _service;

        public LabResultsController(ILabResultsService service)
        {
            _service = service;
        }

        // GET: api/GetLabResults/<patient ID>
        [HttpGet("{id}", Name = "GetLabResults")]
        public async Task<List<JObject>> GetLabResults(string id)
        {
            return await _service.GetPatientLabResults(id);
        }
    }
}