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
            return await _context.Symptom.FindAsync(id);
        }

        public async Task<Symptom> AddSymptom(Symptom symptom)
        {
            _context.Symptom.Add(symptom);
            await _context.SaveChangesAsync();
            return symptom;
        }

        public async Task<Symptom> UpdateSymptom(Symptom symptom)
        {
            _context.Entry(symptom).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return symptom;
        }

        public async Task<bool> DeleteSymptom(int id)
        {
            var symptom = await _context.Symptom.FindAsync(id);
            if (symptom == null)
                return false;

            _context.Symptom.Remove(symptom);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
