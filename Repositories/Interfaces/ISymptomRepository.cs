using SymptomLogger.DTO;
using SymptomLogger.Models;

namespace SymptomLogger.Repositories.Interfaces
{
    public interface ISymptomRepository
    {
        // Symptoms
        Task<IEnumerable<Symptom>> GetSymptoms();
        Task<Symptom> GetSymptomById(int id);
        Task<Symptom> AddSymptom(Symptom symptom);
        Task<Symptom> UpdateSymptom(Symptom symptom);
        Task<bool> DeleteSymptom(int id);

    }
}
