using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Repositories.Interfaces
{
    public interface ISymptomRepository
    {
        // Symptoms
        IQueryable<Symptom> GetSymptoms();
        Task<Symptom> GetSymptomById(int id);
        Task<Symptom> AddSymptom(Symptom symptom, string patientId);
        Task<Symptom> UpdateSymptom(Symptom symptom, string patientId);
        Task<bool> DeleteSymptom(int id);
    }
}
