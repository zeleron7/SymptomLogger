using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.Data;
using SymptomLogger.Models;
using SymptomLogger.Services;

namespace SymptomLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomsController : ControllerBase
    {
        private readonly SymptomLoggerContext _context;
        private readonly ISymptomService _symptomService;

        public SymptomsController(SymptomLoggerContext context, ISymptomService service)
        {
            _context = context;
            _symptomService = service;
        }

        // GET: api/Symptoms
        [HttpGet] // ALL SYMPTOMS
        public async Task<ActionResult<List<Symptom>>> GetAllSymptoms()
        {
            try
            {
                var symptoms = await _symptomService.GetSymptomsAsync();
                return Ok(symptoms);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "An error occurred while retrieving symptoms.");
            }
        }

        // GET: api/Symptoms/5
        [HttpGet("{id}")] // SYMPTOM BY ID
        public async Task<ActionResult<Symptom>> GetSymptom(int id)
        {
            try
            {
                var symptom = await _symptomService.GetSymptomByIdAsync(id);
                if (symptom == null)
                {
                    return NotFound();
                }
                return Ok(symptom);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
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
                    return BadRequest();
                }

                var updatedSymptom = await _symptomService.UpdateSymptomAsync(symptom);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "An error occurred while updating the symptom.");
            }

            return Ok(symptom);
        }

        // POST: api/Symptoms
        [HttpPost] // CREATE SYMPTOM
        public async Task<ActionResult<Symptom>> PostSymptom(Symptom symptom)
        {
            try
             {
                var createdSymptom = await _symptomService.AddSymptomAsync(symptom);
                return Ok(createdSymptom);
            }

            catch (Exception ex)
            {
                // Log the exception (not implemented here)
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
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "An error occurred while deleting the symptom.");
            }
        }
    }
}
