using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories.Interfaces;
using System.Net;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;

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
        public IQueryable<ResponseSymptomDTO> GetSymptoms()
        {
            return _symptomRepository.GetSymptoms()
                .ProjectTo<ResponseSymptomDTO>(_mapper.ConfigurationProvider);
        }
        #endregion

        // Get symptom by ID
        #region GetSymptomByIdAsync
        public async Task<ResponseSymptomDTO> GetSymptomByIdAsync(int id, string patientId)
        {
            var SymptomById = await _symptomRepository.GetSymptomById(id);
            // validate patient ID to ensure the symptom being accessed belongs to the authenticated patient
            if (SymptomById.PatientId != patientId) 
            {
                _logger.LogWarning("User tried to retrieve symptom ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }
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
            var updatedSymptom = await _symptomRepository.GetSymptomById(id);

            if (updatedSymptom.PatientId != patientId)
            {
                _logger.LogWarning("Unknown user tried to update symptom with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            if (updatedSymptom == null)
            {
                _logger.LogInformation("Symptom with ID {SymptomId} not found for update.", id);
                throw new KeyNotFoundException($"Symptom with ID {id} not found.");
            }

            // map updated symptom to dto
            var dto = _mapper.Map<UpdateSymptomDTO>(updatedSymptom);

            /* apply the patch document to dto > map the patched dto back to the symptom entity >
               set the patientId to ensure the symptom remains associated with the correct patient*/
            patchDoc.ApplyTo(dto);
            _mapper.Map(dto, updatedSymptom);
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
            if (symptom.PatientId != patientId)
            {
                _logger.LogWarning("Unknown user tried to delete symptom with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            // null check to prevent null reference exception
            if (symptom == null) 
            {
                return false;
            }
            symptom.PatientId = patientId;
            _context.Symptom.Remove(symptom);

            return await _symptomRepository.DeleteSymptom(id);
        }
        #endregion 

    }
}
