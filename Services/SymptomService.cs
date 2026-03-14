using SymptomLogger.Data;
using SymptomLogger.DTO;
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

        // Get all symptoms
        public async Task<IEnumerable<Symptom>> GetSymptomsAsync()
        {
            return await _symptomRepository.GetSymptoms();
        }

        // Get symptom by ID
        public async Task<Symptom> GetSymptomByIdAsync(int id)
        {
            // Validera / fixa med loggning?

            return await _symptomRepository.GetSymptomById(id);
        }

        // Add new symptom
        public async Task<Symptom> AddSymptomAsync(CreateSymptomDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto), "Symptom cannot be null.");
                }

                // Bind the DTO to the Symptom model
                var symptom = new Symptom
                {
                    Type = dto.Type,
                    Description = dto.Description,
                    Name = dto.Name,
                    Severity = dto.Severity
                };

                return await _symptomRepository.AddSymptom(symptom);
            }

            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while adding the symptom.", ex);
            }
        }

        // Update existing symptom
        public async Task<Symptom> UpdateSymptomAsync(Symptom symptom)
        {
            // Validera / fixa med loggning?

            return await _symptomRepository.UpdateSymptom(symptom);
        }

        // Delete symptom by ID
        public async Task<bool> DeleteSymptomAsync(int id)
        {
            return await _symptomRepository.DeleteSymptom(id);
        }

    }
}
