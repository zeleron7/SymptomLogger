using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.Data;
using SymptomLogger.Models;

namespace SymptomLogger.Repositories.Interfaces
{
    public class SymptomRepository : ISymptomRepository
    {
        private readonly SymptomLoggerContext _context;

        public SymptomRepository(SymptomLoggerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Symptom>> GetSymptoms()
        {
            return await _context.Symptom.ToListAsync();
        }

        public async Task<Symptom> GetSymptomById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid symptom ID.", nameof(id));
            }
            return await _context.Symptom.FindAsync(id);
        }

        public async Task<Symptom> AddSymptom(Symptom symptom)
        {
            await _context.Symptom.AddAsync(symptom);
            await _context.SaveChangesAsync();

            return symptom;
        }

        public async Task<Symptom> UpdateSymptom(Symptom symptom)
        {
            _context.Symptom.Update(symptom);
            await _context.SaveChangesAsync();

            return symptom;
        }

        public async Task<bool> DeleteSymptom(int id)
        {
            var symptom = await _context.Symptom.FindAsync(id);
            if (symptom == null)
            {
                // validering
                return false;
            }

            _context.Symptom.Remove(symptom);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
