using Azure;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;
using SymptomLogger.src.Repositories.Interfaces;
using System.Net;

namespace SymptomLogger.src.Services
{
    public class SymptomService : ISymptomService
    {
        public readonly ISymptomRepository _symptomRepository;
        public readonly SymptomLoggerContext _context;
        public readonly ILogger<SymptomService> _logger;

        public SymptomService(ISymptomRepository symptomRepository, SymptomLoggerContext context, ILogger<SymptomService> logger)
        {
            _symptomRepository = symptomRepository;
            _context = context;
            _logger = logger;
        }

        // Get all symptoms
        public async Task<IEnumerable<Symptom>> GetSymptomsAsync()
        {
            return await _symptomRepository.GetSymptoms();
        }

        // Get symptom by ID
        public async Task<Symptom> GetSymptomByIdAsync(int id)
        {
            return await _symptomRepository.GetSymptomById(id);
        }

        // Add new symptom
        #region AddSymptomAsync
        public async Task<Symptom> AddSymptomAsync(CreateSymptomDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogInformation("CreateSymptomDTO is null.");
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
                _logger.LogError(ex, "An error occurred while adding a new symptom. DTO: {@CreateSymptomDTO}", dto);
                throw new Exception("An error occurred while adding the symptom.", ex);
            }
        }
        #endregion

        // Update symptom
        #region UpdateSymptomAsync
        public async Task<Symptom> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc)
        {
            var updatedSymptom = await _context.Symptom.FindAsync(id);

            if (updatedSymptom == null)
            {
                _logger.LogInformation("Symptom with ID {SymptomId} not found for update.", id);
                throw new KeyNotFoundException($"Symptom with ID {id} not found.");
            }

            var dto = new UpdateSymptomDTO
            {
                Type = updatedSymptom.Type,
                Description = updatedSymptom.Description,
                Name = updatedSymptom.Name,
                Severity = updatedSymptom.Severity
            };

            patchDoc.ApplyTo(dto);

            // Only update properties that are not null in the DTO
            updatedSymptom.Type = dto.Type ?? updatedSymptom.Type;
            updatedSymptom.Description = dto.Description ?? updatedSymptom.Description;
            updatedSymptom.Name = dto.Name ?? updatedSymptom.Name;
            updatedSymptom.Severity = dto.Severity ?? updatedSymptom.Severity;

            return await _symptomRepository.UpdateSymptom(updatedSymptom);
        }
        #endregion 

        // Delete symptom by ID
        public async Task<bool> DeleteSymptomAsync(int id)
        {
            return await _symptomRepository.DeleteSymptom(id);
        }

    }
}
