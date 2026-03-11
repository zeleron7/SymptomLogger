using SymptomLogger.Models;
using SymptomLogger.Repositories.Interfaces;

namespace SymptomLogger.Services
{
    public class SymptomService : ISymptomService
    {
        public readonly ISymptomRepository _symptomRepository;

        public SymptomService(ISymptomRepository symptomRepository)
        {
            _symptomRepository = symptomRepository;
        }

        public async Task<IEnumerable<Symptom>> GetSymptomsAsync()
        {
            return await _symptomRepository.GetSymptoms();
        }

        public async Task<Symptom> GetSymptomByIdAsync(int id)
        {
            return await _symptomRepository.GetSymptomById(id);
        }

        public async Task<Symptom> AddSymptomAsync(Symptom symptom)
        {
            return await _symptomRepository.AddSymptom(symptom);
        }

        public async Task<Symptom> UpdateSymptomAsync(Symptom symptom)
        {
            return await _symptomRepository.UpdateSymptom(symptom);
        }

        public async Task<bool> DeleteSymptomAsync(int id)
        {
            return await _symptomRepository.DeleteSymptom(id);
        }

    }
}
