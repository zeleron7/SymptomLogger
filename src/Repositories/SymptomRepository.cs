using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;
using SymptomLogger.src.Repositories.Interfaces;

namespace SymptomLogger.src.Repositories
{
    public class SymptomRepository : ISymptomRepository
    {
        private readonly SymptomLoggerContext _context;

        public SymptomRepository(SymptomLoggerContext context)
        {
            _context = context;
        }

        // Get all symptoms
        public async Task<IEnumerable<Symptom>> GetSymptoms()
        {
            return await _context.Symptom.ToListAsync();
        }

        // Get symptom by ID
        public async Task<Symptom> GetSymptomById(int id)
        {
            return await _context.Symptom.FindAsync(id);
        }

        // Add new symptom
        public async Task<Symptom> AddSymptom(Symptom symptom)
        {
            await _context.Symptom.AddAsync(symptom);
            await _context.SaveChangesAsync();

            return symptom;
        }

        // Update existing symptom
        public async Task<Symptom> UpdateSymptom(Symptom symptom)
        {
            _context.Symptom.Update(symptom);
            await _context.SaveChangesAsync();

            return symptom;
        }

        // Delete symptom by ID
        public async Task<bool> DeleteSymptom(int id)
        {
            var symptom = await _context.Symptom.FindAsync(id);
            _context.Symptom.Remove(symptom);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
