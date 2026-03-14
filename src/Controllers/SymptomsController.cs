using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;
using SymptomLogger.src.Services;

namespace SymptomLogger.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomsController : ControllerBase
    {
        private readonly SymptomLoggerContext _context;
        private readonly ISymptomService _symptomService;
        private readonly ILogger<SymptomsController> _logger;

        public SymptomsController(SymptomLoggerContext context, ISymptomService service, ILogger<SymptomsController> logger)
        {
            _context = context;
            _symptomService = service;
            _logger = logger;
        }

        // GET: api/Symptoms
        [HttpGet] // ALL SYMPTOMS
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

        // GET: api/Symptoms/5
        [HttpGet("{id}")] // SYMPTOM BY ID
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

        // PUT: api/Symptoms/5
        [HttpPut("{id}")] // UPDATE SYMPTOM
        public async Task<IActionResult> PutSymptom(int id, Symptom symptom)
        {
            try
            {
                if (id != symptom.Id)
                {
                    _logger.LogWarning("Symptom ID in the URL does not match the ID in the body. URL ID: {UrlId}, Body ID: {BodyId}.", id, symptom.Id);
                    return BadRequest();
                }

                var updatedSymptom = await _symptomService.UpdateSymptomAsync(symptom);
                _logger.LogInformation("Symptom with ID {SymptomId} updated successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating symptom with ID {SymptomId}.", id);
                return StatusCode(500, "An error occurred while updating the symptom.");
            }

            return Ok(symptom);
        }

        // POST: api/Symptoms
        [HttpPost] // CREATE SYMPTOM
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
                var createdSymptom = await _symptomService.AddSymptomAsync(dto);
                return Ok(createdSymptom);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new symptom.");
                return StatusCode(500, "An error occurred while creating the symptom.");
            }

        }

        // DELETE: api/Symptoms/5
        [HttpDelete("{id}")] // DELETE SYMPTOM
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
    }
}
