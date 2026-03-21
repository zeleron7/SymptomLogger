using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;
using SymptomLogger.src.Repositories.Interfaces;
using System.Net;
using System.Security.Claims;

namespace SymptomLogger.src.Services
{
    public class SymptomService : ISymptomService
    {
        private readonly ISymptomRepository _symptomRepository;
        private readonly SymptomLoggerContext _context;
        private readonly ILogger<SymptomService> _logger;
        private readonly IMapper _mapper;

        public SymptomService(ISymptomRepository symptomRepository, SymptomLoggerContext context, ILogger<SymptomService> logger,
            IMapper mapper)
        {
            _symptomRepository = symptomRepository;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // Get all symptoms
        #region GetSymptomsAsync
        public async Task<IEnumerable<ResponseSymptomDTO>> GetSymptomsAsync()
        {
            var Symptoms = await _symptomRepository.GetSymptoms();
            return _mapper.Map<IEnumerable<ResponseSymptomDTO>>(Symptoms);
        }
        #endregion

        // Get symptom by ID
        #region GetSymptomByIdAsync
        public async Task<ResponseSymptomDTO> GetSymptomByIdAsync(int id)
        {
            var SymptomById = await _symptomRepository.GetSymptomById(id);
            return _mapper.Map<ResponseSymptomDTO>(SymptomById);
        }
        #endregion 

        // Add new symptom
        #region AddSymptomAsync
        public async Task<ResponseSymptomDTO> AddSymptomAsync(CreateSymptomDTO dto, string patientId)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogInformation("CreateSymptomDTO is null.");
                    throw new ArgumentNullException(nameof(dto), "Symptom cannot be null.");
                }

                // map DTO to entity
                var symptom = _mapper.Map<Symptom>(dto);

                // set the patientId passed from the controller to ensure the symptom is associated with the correct patient
                symptom.PatientId = patientId;

                var CreatedSymptom = await _symptomRepository.AddSymptom(symptom, patientId);
                return _mapper.Map<ResponseSymptomDTO>(CreatedSymptom);
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
        public async Task<ResponseSymptomDTO> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc, string patientId)
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

            // set the patientId passed from the controller to ensure the symptom is associated with the correct patient
            updatedSymptom.PatientId = patientId;

            var UpdatedSymptom = await _symptomRepository.UpdateSymptom(updatedSymptom, patientId);
            return _mapper.Map<ResponseSymptomDTO>(UpdatedSymptom);
        }
        #endregion

        // Delete symptom by ID
        #region DeleteSymptomAsync
        public async Task<bool> DeleteSymptomAsync(int id, string patientId)
        {
            var symptom = await _symptomRepository.GetSymptomById(id);

            // null check to prevent null reference exception and log if symptom not found
            if (symptom == null) 
            {
                _logger.LogInformation("Symptom with ID {SymptomId} not found for deletion.", id);
                return false;
            }
            symptom.PatientId = patientId;
            _context.Symptom.Remove(symptom);

            return await _symptomRepository.DeleteSymptom(id);
        }
        #endregion 

    }
}
