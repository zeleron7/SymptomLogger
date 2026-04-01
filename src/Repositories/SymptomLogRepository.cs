using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Data;
using SymptomLogger.src.Entity;
using Microsoft.EntityFrameworkCore;

namespace SymptomLogger.src.Repositories
{
    public class SymptomLogRepository : ISymptomLogRepository
    {
        private readonly SymptomLoggerContext _context;
        public SymptomLogRepository(SymptomLoggerContext context) 
        {
            _context = context;
        }

        public IQueryable<SymptomLog> GetSymptomLogs()
        {
            //return _context.SymptomLog.AsNoTracking();
            return _context.SymptomLog.Include(s => s.Symptom).AsNoTracking();
        }
        public async Task<SymptomLog> GetSymptomLogById(int id)
        {
            return await _context.SymptomLog.FindAsync(id);
        }
        public async Task<SymptomLog> AddSymptomLog(SymptomLog symptomLog)
        {
            await _context.SymptomLog.AddAsync(symptomLog);
            await _context.SaveChangesAsync();

            return symptomLog;
        }
        public async Task<SymptomLog> UpdateSymptomLog(SymptomLog symptomLog)
        {
            _context.SymptomLog.Update(symptomLog);
            await _context.SaveChangesAsync();
            return symptomLog;
        }

        public async Task<bool> DeleteSymptomLog(int id)
        {
            var symptomLog = await _context.SymptomLog.FindAsync(id);
            if (symptomLog == null)
            {
                return false;
            }
            _context.SymptomLog.Remove(symptomLog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
