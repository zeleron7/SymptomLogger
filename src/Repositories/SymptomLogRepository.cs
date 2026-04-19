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

        // Get all symptom logs
        public IQueryable<SymptomLog> GetSymptomLogs()
        {
            return _context.SymptomLog.AsNoTracking();
        }
        // Get symptom log by ID
        public async Task<SymptomLog> GetSymptomLogById(int id)
        {
            return await _context.SymptomLog.FindAsync(id);
        }
        // Add new symptom log
        public async Task<SymptomLog> AddSymptomLog(SymptomLog symptomLog, string userId)
        {
            symptomLog.UserId = userId;

            await _context.SymptomLog.AddAsync(symptomLog);
            await _context.SaveChangesAsync();

            return symptomLog;
        }
        // Update existing symptom log
        public async Task<SymptomLog> UpdateSymptomLog(SymptomLog symptomLog, string userId)
        {
            symptomLog.UserId = userId;

            _context.SymptomLog.Update(symptomLog);
            await _context.SaveChangesAsync();

            return symptomLog;
        }
        // Delete symptom log by ID
        public async Task<bool> DeleteSymptomLog(int id)
        {
            var symptomLog = await _context.SymptomLog.FindAsync(id);

            _context.SymptomLog.Remove(symptomLog);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
