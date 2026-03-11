using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.Data;
using SymptomLogger.Models;

namespace SymptomLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomsController : ControllerBase
    {
        private readonly SymptomLoggerContext _context;

        public SymptomsController(SymptomLoggerContext context)
        {
            _context = context;
        }

        // GET: api/Symptoms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Symptom>>> GetSymptom()
        {
            return await _context.Symptom.ToListAsync();
        }

        // GET: api/Symptoms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Symptom>> GetSymptom(int id)
        {
            var symptom = await _context.Symptom.FindAsync(id);

            if (symptom == null)
            {
                return NotFound();
            }

            return symptom;
        }

        // PUT: api/Symptoms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSymptom(int id, Symptom symptom)
        {
            if (id != symptom.Id)
            {
                return BadRequest();
            }

            _context.Entry(symptom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SymptomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Symptoms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Symptom>> PostSymptom(Symptom symptom)
        {
            _context.Symptom.Add(symptom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSymptom", new { id = symptom.Id }, symptom);
        }

        // DELETE: api/Symptoms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            var symptom = await _context.Symptom.FindAsync(id);
            if (symptom == null)
            {
                return NotFound();
            }

            _context.Symptom.Remove(symptom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SymptomExists(int id)
        {
            return _context.Symptom.Any(e => e.Id == id);
        }
    }
}
