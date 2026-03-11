using SymptomLogger.Models;

namespace SymptomLogger.Services
{
    public interface ISymptomService
    {
        // Symptoms
        Task<IEnumerable<Symptom>> GetSymptomsAsync();
        Task<Symptom> GetSymptomByIdAsync(int id);
        Task<Symptom> AddSymptomAsync(Symptom symptom);
        Task<Symptom> UpdateSymptomAsync(Symptom symptom);
        Task<bool> DeleteSymptomAsync(int id);

    }
}
