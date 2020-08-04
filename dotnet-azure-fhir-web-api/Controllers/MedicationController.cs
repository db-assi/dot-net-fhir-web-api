using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_azure_fhir_web_api.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace dotnet_azure_fhir_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationController : ControllerBase
    {

        private readonly IMedicationService _service;

        public MedicationController(IMedicationService service)
        {
            _service = service;
        }

        // GET: api/Medication/<patient ID>
        [HttpGet("{id}", Name = "GetMedication")]
        public async Task<List<JObject>> GetObservations(string id)
        {
            return await _service.GetPatientMedication(id);
        }

        // GET: api/Medication/single/<medication ID>
        [HttpGet("single/{id}", Name = "GetSingleMedication")]
        public async Task<JObject> GetSingleObservation(string id)
        {
            return await _service.GetSingleMedication(id);
        }

        // GET: api/Medication/pages/<number of pages>/<patient ID>
        [HttpGet("pages/{pages}/{id}", Name = "GetPatientMedicationPages")]
        public async Task<List<JObject>> GetPatientMedicationPages(string id, int pages)
        {
            return await _service.GetPatientMedicationPages(id, pages);
        }



    }
}