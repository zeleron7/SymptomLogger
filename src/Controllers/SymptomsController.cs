using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services;

namespace SymptomLogger.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize]
        [HttpGet("GetAll")] 
        public async Task<ActionResult<List<Symptom>>> GetAllSymptoms()
        {
            try
            {
                _logger.LogInformation("Retrieving all symptoms.");
                var symptoms = await _symptomService.GetSymptomsAsync();
                return Ok(symptoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving symptoms.");
                return StatusCode(500, "An error occurred while retrieving symptoms.");
            }
        }
        #endregion

        // GET: api/Symptoms/5
        #region Get symptom by ID
        [Authorize]
        [HttpGet("{id}")] 
        public async Task<ActionResult<Symptom>> GetSymptom(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving symptom with ID {SymptomId}.", id);
                var symptom = await _symptomService.GetSymptomByIdAsync(id);
                if (symptom == null)
                {
                    _logger.LogWarning("Symptom with ID {SymptomId} not found.", id);
                    return NotFound();
                }
                return Ok(symptom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving symptom with ID {SymptomId}.", id);
                return StatusCode(500, "An error occurred while retrieving the symptom.");
            }
        }
        #endregion

        // Patch: api/Symptoms/5 
        // Try this endpoint with postman or change JSON format [{"op": "replace", "path": "/description", "value": "Updated description"}]
        #region Update symptom

        [HttpPatch("{id}")] 
        public async Task<IActionResult> PatchSymptom(int id, [FromBody] JsonPatchDocument<UpdateSymptomDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    _logger.LogWarning("Patch document is null for symptom with ID {SymptomId}.", id);
                    return BadRequest("Patch document cannot be null.");
                }

                var updatedSymptom = await _symptomService.UpdateSymptomAsync(id, patchDoc);
                _logger.LogInformation("Symptom with ID {SymptomId} updated successfully.", id);

                return Ok(updatedSymptom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating symptom with ID {SymptomId}.", id);
                return StatusCode(500, "An error occurred while updating the symptom.");
            }
        }
        #endregion

        // POST: api/Symptoms
        #region Create new symptom
        [Authorize]
        [HttpPost] 
        public async Task<ActionResult<Symptom>> PostSymptom(CreateSymptomDTO dto)
        {
            // Check if Description is either empty or over 100 characters
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating symptom. Errors: {ModelErrors}.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            // validate default value 
            if (dto.Description == "string")
            {
                _logger.LogWarning("Default value for Description field detected. Please provide a valid description.");
                return BadRequest("Please describe your symptoms.");
            }

            try
            {
                var createdSymptom = await _symptomService.AddSymptomAsync(dto, User);
                return Ok(createdSymptom);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new symptom.");
                return StatusCode(500, "An error occurred while creating the symptom.");
            }
        }
        #endregion

        // DELETE: api/Symptoms/5
        #region Delete symptom

        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            try
            {
                var success = await _symptomService.DeleteSymptomAsync(id);
                _logger.LogInformation("Attempting to delete symptom with ID {SymptomId}. Success: {Success}.", id, success);

                if (!success)
                {
                    _logger.LogWarning("Symptom with ID {SymptomId} not found for deletion.", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting symptom with ID {SymptomId}.", id);
                return StatusCode(500, "An error occurred while deleting the symptom.");
            }
        }
        #endregion 
    }
}
