using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories.Interfaces;

namespace SymptomLogger.src.Repositories
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly SymptomLoggerContext _context;

        public ConditionRepository(SymptomLoggerContext context)
        {
            _context = context;
        }

        // Get all conditions
        public IQueryable<Condition> GetConditions()
        {
            return _context.Condition.AsNoTracking();
        }
        // Get condition by ID
        public async Task<Condition> GetConditionById(int id)
        {
            return await _context.Condition.FindAsync(id);
        }
        // Add new condition
        public async Task<Condition> AddCondition(Condition condition)
        {
            await _context.Condition.AddAsync(condition);
            await _context.SaveChangesAsync();

            return condition;
        }
        // Update existing condition
        public async Task<Condition> UpdateCondition(Condition condition)
        {
            _context.Condition.Update(condition);
            await _context.SaveChangesAsync();

            return condition;
        }
        // Delete condition by ID
        public async Task<bool> DeleteCondition(int id)
        {
            var condition = await _context.Condition.FindAsync(id);

            _context.Condition.Remove(condition);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
