using dotnet_azure_fhir_web_api.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace donet_azure_fhir_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitalSignsController : ControllerBase
    {
        private readonly IVitalSignsService _service;

        public VitalSignsController(IVitalSignsService service)
        {
            _service = service;
        }

        // GET: api/VitalSigns/<patient ID>
        [HttpGet("{id}", Name = "GetVitalSigns")]
        public async Task<List<JObject>> GetVitalSigns(string id)
        {
            return await _service.GetPatientVitalSigns(id);
        }

    }
}