using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Repositories.Interfaces
{
    public interface ISymptomLogRepository
    {
        // Symptom logs
        IQueryable<SymptomLog> GetSymptomLogs(); 
        Task<SymptomLog> GetSymptomLogById(int id);
        Task<SymptomLog> AddSymptomLog(SymptomLog symptomLog, string patientId);
        Task<SymptomLog> UpdateSymptomLog(SymptomLog symptomLog, string patientId);
        Task<bool> DeleteSymptomLog(int id);
    }
}
