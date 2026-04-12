using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;
using SymptomLogger.src.Utils;

namespace SymptomLogger.src.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SymptomsController : ControllerBase
    {
        private readonly ISymptomService _symptomService;
        private readonly ILogger<SymptomsController> _logger;

        public SymptomsController(ISymptomService symptomService, ILogger<SymptomsController> logger)
        {
            _symptomService = symptomService;
            _logger = logger;
        }

        // GET: api/Symptoms
        #region Get all symptoms
        //[Authorize] // SET ADMIN ROLE LATER
        [HttpGet] 
        public async Task<ActionResult<IQueryable<ResponseSymptomDTO>>> GetAllSymptoms([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Retrieving all symptoms.");
            var symptoms = _symptomService.GetSymptoms();

            // Implement pagination using the PagedList utility class
            var PagedSymptoms = await PagedList<ResponseSymptomDTO>.CreateAsync(symptoms, pageParameters.PageNumber, pageParameters.PageSize);

            if (!symptoms.Any())
            {
                _logger.LogWarning("No symptoms found.");
                return NotFound();
            }
            _logger.LogInformation("Symptoms retrieved successfully. Count: {SymptomCount}.", symptoms.Count());
            return Ok(PagedSymptoms);
        }
        #endregion

        // GET: api/Symptoms/5
        #region Get symptom by ID
        [Authorize]
        [HttpGet("{id}")] 
        public async Task<ActionResult<ResponseSymptomDTO>> GetSymptom(int id)
        {
            _logger.LogInformation("Retrieving symptom with ID {SymptomId}.", id);
            var patientId = RetrievePatientId.GetPatientId(User);
            var symptom = await _symptomService.GetSymptomByIdAsync(id, patientId);
            
            _logger.LogInformation("Symptom with ID {SymptomId} retrieved successfully.", id);
            return Ok(symptom);
        }
        #endregion

        // POST: api/Symptoms
        #region Create new symptom
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseSymptomDTO>> AddSymptom(CreateSymptomDTO dto)
        {
            _logger.LogInformation("Creating new symptom");
            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            var patientId = RetrievePatientId.GetPatientId(User);
            var createdSymptom = await _symptomService.AddSymptomAsync(dto, patientId);

            _logger.LogInformation("Symptom created successfully with Description {Description}.", createdSymptom.Description);
            return Ok(createdSymptom);
        }
        #endregion

        // Patch: api/Symptoms/5 
        // JSON format [{"op": "replace", "path": "/description", "value": "Updated description"}]
        #region Update symptom
        [Authorize]
        [HttpPatch("{id}")] 
        public async Task<IActionResult> UpdateSymptom(int id, [FromBody] JsonPatchDocument<UpdateSymptomDTO> patchDoc)
        {
            _logger.LogInformation("Updating symptom with ID {SymptomId}.", id);
            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            var patientId = RetrievePatientId.GetPatientId(User);

            var updatedSymptom = await _symptomService.UpdateSymptomAsync(id, patchDoc, patientId);
            _logger.LogInformation("Symptom with ID {SymptomId} updated successfully.", id);

            return Ok(updatedSymptom);

        }
        #endregion

        // DELETE: api/Symptoms/5
        #region Delete symptom
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            _logger.LogInformation("Deleting symptom with ID {SymptomId}.", id);

            var patientId = RetrievePatientId.GetPatientId(User);
            var success = await _symptomService.DeleteSymptomAsync(id, patientId);

            if (!success)
            {
                _logger.LogWarning("Symptom with ID {SymptomId} not found for deletion.", id);
                return NotFound();
            }
            _logger.LogInformation("Symptom with ID {SymptomId} deleted successfully.", id);
            return Ok(new { message = "Symptom deleted successfully" });
        }
        #endregion 
    }
}
