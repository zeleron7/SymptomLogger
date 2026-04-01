using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Repositories.Interfaces
{
    public interface ISymptomLogRepository
    {
        // CHANGE TO RESPONSE DTO
        IQueryable<SymptomLog> GetSymptomLogs(); 
        Task<SymptomLog> GetSymptomLogById(int id);
        Task <SymptomLog> AddSymptomLog(SymptomLog symptomLog);
        Task <SymptomLog> UpdateSymptomLog(SymptomLog symptomLog);
        Task<bool> DeleteSymptomLog(int id);
    }
}
