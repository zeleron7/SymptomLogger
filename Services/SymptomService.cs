using SymptomLogger.Data;
using SymptomLogger.Models;
using SymptomLogger.Repositories.Interfaces;
using System.Net;

namespace SymptomLogger.Services
{
    public class SymptomService : ISymptomService
    {
        public readonly ISymptomRepository _symptomRepository;
        public readonly SymptomLoggerContext _context;

        public SymptomService(ISymptomRepository symptomRepository, SymptomLoggerContext Context)
        {
            _symptomRepository = symptomRepository;
            _context = Context;
        }

        public async Task<IEnumerable<Symptom>> GetSymptomsAsync()
        {
            return await _symptomRepository.GetSymptoms();
        }

        public async Task<Symptom> GetSymptomByIdAsync(int id)
        {
            // Validera / fixa med loggning?

            return await _symptomRepository.GetSymptomById(id);
        }

        public async Task<Symptom> AddSymptomAsync(Symptom symptom)
        {

            try
            {
                if (symptom == null)
                {
                    throw new ArgumentNullException(nameof(symptom), "Symptom cannot be null.");
                }

                return await _symptomRepository.AddSymptom(symptom);
            }

            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while adding the symptom.", ex);
            }
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
