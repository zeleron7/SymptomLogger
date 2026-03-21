//using SymptomLogger.DTO;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;

namespace SymptomLogger.src.Repositories.Interfaces
{
    public interface ISymptomRepository
    {
        // Symptoms
        Task<IEnumerable<Symptom>> GetSymptoms();
        Task<Symptom> GetSymptomById(int id);
        Task<Symptom> AddSymptom(Symptom symptom, string patientId);
        Task<Symptom> UpdateSymptom(Symptom symptom, string patientId);
        Task<bool> DeleteSymptom(int id);
    }
}
