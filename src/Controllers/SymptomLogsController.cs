using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Services;
using SymptomLogger.src.Utils;
using SymptomLogger.src.DTO.SymptomLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace SymptomLogger.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomLogsController : ControllerBase
    {
        private readonly ISymptomLogService _symptomLogService;
        private readonly ILogger<SymptomLogsController> _logger;

        public SymptomLogsController(ISymptomLogService symptomLogService, ILogger<SymptomLogsController> logger)
        {
            _symptomLogService = symptomLogService;
            _logger = logger;
        }

        // GET: api/SymptomLogs
        #region Get all symptom logs
        [HttpGet("Getall")]
        public async Task<ActionResult<IQueryable<ResponseSymptomLogDTO>>> GetSymptomLogs([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Retrieving all symptoms logs.");
            var symptomLogs = _symptomLogService.GetSymptomLogs();

            // Implement pagination using the PagedList utility class
            var PagedSymptomLogs = await PagedList<ResponseSymptomLogDTO>.CreateAsync(symptomLogs, pageParameters.PageNumber,
                pageParameters.PageSize);

            if (!symptomLogs.Any())
            {
                _logger.LogWarning("No symptom logs found.");
                return NotFound();
            }
            _logger.LogInformation("Symptom logs retrieved successfully. Count: {SymptomLogCount}.", symptomLogs.Count());

            return Ok(PagedSymptomLogs);
        }
        #endregion 

        // GET: api/Symptoms/5
        #region Get symptom log by ID
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseSymptomLogDTO>> GetSymptomLog(int id)
        {
            _logger.LogInformation("Retrieving symptom with ID {SymptomId}.", id);
            var patientId = RetrievePatientId.GetPatientId(User);

            var symptom = await _symptomLogService.GetSymptomLogByIdAsync(id, patientId);
            if (symptom == null)
            {
                _logger.LogWarning("Symptom with ID {SymptomId} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("Symptom with ID {SymptomId} retrieved successfully.", id);
            return Ok(symptom);
        }
        #endregion

        // Patch: api/Symptoms/5 
        // Try this endpoint with postman or change JSON format [{"op": "replace", "path": "/description", "value": "Updated description"}]
        #region Update symptom
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSymptom(int id, [FromBody] JsonPatchDocument<UpdateSymptomLogDTO> patchDoc)
        {
            _logger.LogInformation("Updating symptom with ID {SymptomId}.", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Patch document is null for symptom with ID {SymptomId}.", id);
                return BadRequest("Patch document cannot be null.");
            }

            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            var patientId = RetrievePatientId.GetPatientId(User);

            var updatedSymptom = await _symptomLogService.UpdateSymptomLogAsync(id, patchDoc, patientId);
            _logger.LogInformation("Symptom with ID {SymptomId} updated successfully.", id);

            return Ok(updatedSymptom);

        }
        #endregion

        // POST: api/Symptoms
        #region Create new symptom
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseSymptomLogDTO>> PostSymptomLog(CreateSymptomLogDTO dto)
        {
            _logger.LogInformation("Creating new symptom");

            // Check if Description is either empty or over 100 characters
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating symptom. Errors: {ModelErrors}.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            // validate default value 
            if (dto.Notes == "string")
            {
                _logger.LogWarning("Default value for Description field detected. Please provide a valid description.");
                return BadRequest("Please describe your symptoms.");
            }

            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            var patientId = RetrievePatientId.GetPatientId(User);
            var createdSymptom = await _symptomLogService.AddSymptomLogAsync(dto, patientId);
            _logger.LogInformation("Symptom created successfully with Notes {Notes}.", createdSymptom.Notes);

            return Ok(createdSymptom);
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
            var success = await _symptomLogService.DeleteSymptomLogAsync(id, patientId);

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
