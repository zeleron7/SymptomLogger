using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Repositories.Interfaces
{
    public interface IConditionRepository
    {
        // Conditions 
        IQueryable<Condition> GetConditions();
        Task<Condition> GetConditionById(int id);
        Task<Condition> AddCondition(Condition condition);
        Task<Condition> UpdateCondition(Condition condition);
        Task<bool> DeleteCondition(int id);
    }
}
