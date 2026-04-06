using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;

namespace SymptomLogger.src.Services
{
    public class SymptomLogService : ISymptomLogService
    {
        private readonly ISymptomLogRepository _symptomLogRepository;
        private readonly ILogger<SymptomLogService> _logger;
        private readonly IMapper _mapper;

        public SymptomLogService(ISymptomLogRepository symptomLogRepository, ILogger<SymptomLogService> logger,
            IMapper mapper)
        {
            _symptomLogRepository = symptomLogRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // Get all symptom logs
        #region GetSymptomsLogs
        public IQueryable<ResponseSymptomLogDTO> GetSymptomLogs()
        {
            return _symptomLogRepository.GetSymptomLogs()
                .ProjectTo<ResponseSymptomLogDTO>(_mapper.ConfigurationProvider);
        }
        #endregion

        // Get symptom log by ID
        #region GetSymptomLogByIdAsync
        public async Task<ResponseSymptomLogDTO> GetSymptomLogByIdAsync(int id, string patientId)
        {
            var symptomLogById = await _symptomLogRepository.GetSymptomLogById(id);
            // validate patient ID to ensure the symptom log being accessed belongs to the authenticated patient
            if (symptomLogById.PatientId != patientId)
            {
                _logger.LogWarning("User tried to retrieve symptom log ID {SymptomLogId}", id);
                throw new UnauthorizedAccessException();
            }
            return _mapper.Map<ResponseSymptomLogDTO>(symptomLogById);
        }
        #endregion 

        // Add new symptom log
        #region AddSymptomLogAsync
        public async Task<ResponseSymptomLogDTO> AddSymptomLogAsync(CreateSymptomLogDTO dto, string patientId)
        {
            try
            {
                // map DTO to entity
                var symptomLog = _mapper.Map<SymptomLog>(dto);

                // set the patientId passed from the controller to ensure the symptom is associated with the correct patient
                symptomLog.PatientId = patientId;
                symptomLog.SymptomId = dto.SymptomId;

                if (dto == null)
                {
                    _logger.LogInformation("CreateSymptomLogDTO is null.");
                    throw new ArgumentNullException(nameof(dto), "Symptom log cannot be null.");
                }
                var CreatedSymptomLog = await _symptomLogRepository.AddSymptomLog(symptomLog, patientId);
                return _mapper.Map<ResponseSymptomLogDTO>(CreatedSymptomLog);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new symptom log. DTO: {@CreateSymptomLogDTO}", dto);
                throw new Exception("An error occurred while adding the symptom log.", ex);
            }
        }
        #endregion

        // Update symptom
        #region UpdateSymptomAsync
        public async Task<ResponseSymptomLogDTO> UpdateSymptomLogAsync(int id, JsonPatchDocument<UpdateSymptomLogDTO> patchDoc, 
            string patientId)
        {
            var symptomLog = await _symptomLogRepository.GetSymptomLogById(id);

            if (symptomLog.PatientId != patientId)
            {
                _logger.LogWarning("Unknown user tried to update symptom log with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            if (symptomLog == null)
            {
                _logger.LogInformation("Symptom log with ID {SymptomId} not found for update.", id);
                throw new KeyNotFoundException($"Symptom log with ID {id} not found.");
            }

            // map updated symptom log to dto
            var dto = _mapper.Map<UpdateSymptomLogDTO>(symptomLog);

            /* apply the patch document to dto > map the patched dto back to the symptomLog entity >
               set the patientId to ensure the symptom log remains associated with the correct patient*/
            patchDoc.ApplyTo(dto);
            _mapper.Map(dto, symptomLog);
            symptomLog.PatientId = patientId;

            var UpdatedSymptomLog = await _symptomLogRepository.UpdateSymptomLog(symptomLog, patientId);
            return _mapper.Map<ResponseSymptomLogDTO>(UpdatedSymptomLog);
        }
        #endregion

        // Delete symptom by ID
        #region DeleteSymptomAsync
        public async Task<bool> DeleteSymptomLogAsync(int id, string patientId)
        {
            var symptomLog = await _symptomLogRepository.GetSymptomLogById(id);
            if (symptomLog.PatientId != patientId)
            {
                _logger.LogWarning("Unknown user tried to delete symptom log with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            // null check to prevent null reference exception
            if (symptomLog == null)
            {
                return false;
            }
            symptomLog.PatientId = patientId;

            return await _symptomLogRepository.DeleteSymptomLog(id);
        }
        #endregion 
    }
}
