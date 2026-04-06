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
using SymptomLogger.src.Utils;
using SymptomLogger.src.DTO.SymptomLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.Services.Interfaces;

namespace SymptomLogger.src.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [HttpGet]
        public async Task<ActionResult<IQueryable<ResponseSymptomLogDTO>>> GetAllSymptomLogs([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Retrieving all symptoms logs.");
            var symptomLogs = _symptomLogService.GetSymptomLogs();

            // Implement pagination using the PagedList utility class
            var pagedSymptomLogs = await PagedList<ResponseSymptomLogDTO>.CreateAsync(symptomLogs, pageParameters.PageNumber,
                pageParameters.PageSize);

            if (!symptomLogs.Any())
            {
                _logger.LogWarning("No symptom logs found.");
                return NotFound();
            }
            _logger.LogInformation("Symptom logs retrieved successfully. Count: {SymptomLogCount}.", symptomLogs.Count());
            return Ok(pagedSymptomLogs);
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
        // JSON format [{"op": "replace", "path": "/notes", "value": "Updated notes"}]
        #region Update symptom
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSymptomLog(int id, [FromBody] JsonPatchDocument<UpdateSymptomLogDTO> patchDoc)
        {
            _logger.LogInformation("Updating symptom log with ID {SymptomId}.", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Patch document is null for symptom log with ID {SymptomId}.", id);
                return BadRequest("Patch document cannot be null.");
            }
            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            var patientId = RetrievePatientId.GetPatientId(User);

            var updatedSymptomLog = await _symptomLogService.UpdateSymptomLogAsync(id, patchDoc, patientId);
            _logger.LogInformation("Symptom log with ID {SymptomId} updated successfully.", id);

            return Ok(updatedSymptomLog);
        }
        #endregion

        // POST: api/Symptoms
        #region Create new symptom
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseSymptomLogDTO>> AddSymptomLog(CreateSymptomLogDTO dto)
        {
            _logger.LogInformation("Creating new symptom log");

            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            var patientId = RetrievePatientId.GetPatientId(User);
            var createdSymptom = await _symptomLogService.AddSymptomLogAsync(dto, patientId);
            _logger.LogInformation("Symptom log created successfully with Note {Notes}.", createdSymptom.Notes);

            return Ok(createdSymptom);
        }
        #endregion

        // DELETE: api/Symptoms/5
        #region Delete symptom
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSymptomLog(int id)
        {
            _logger.LogInformation("Deleting symptom log with ID {SymptomId}.", id);

            var patientId = RetrievePatientId.GetPatientId(User);
            var success = await _symptomLogService.DeleteSymptomLogAsync(id, patientId);

            if (!success)
            {
                _logger.LogWarning("Symptom log with ID {SymptomId} not found for deletion.", id);
                return NotFound();
            }
            _logger.LogInformation("Symptom log with ID {SymptomId} deleted successfully.", id);
            return Ok(new { message = "Symptom log deleted successfully" });
        }
        #endregion 
    }
}
