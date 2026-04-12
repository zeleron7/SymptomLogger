using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;
using SymptomLogger.src.Utils;

namespace SymptomLogger.src.Services
{
    public class SymptomLogService : ISymptomLogService
    {
        private readonly ISymptomLogRepository _symptomLogRepository;
        private readonly ILogger<SymptomLogService> _logger;

        public SymptomLogService(ISymptomLogRepository symptomLogRepository, ILogger<SymptomLogService> logger)
        {
            _symptomLogRepository = symptomLogRepository;
            _logger = logger;
        }

        // Get all symptom logs
        #region GetSymptomsLogs
        public IQueryable<ResponseSymptomLogDTO> GetSymptomLogs()
        {
            return _symptomLogRepository.GetSymptomLogs()
                .Select(c => new ResponseSymptomLogDTO // manul mapping 
                {
                    Notes = c.Notes,
                    Severity = c.Severity,
                    DurationMinutes = c.DurationMinutes,
                    StressLevel = c.StressLevel,
                    SleepHours = c.SleepHours,
                    isFlare = c.isFlare
                });                                      
        }
        #endregion

        // Get symptom log by ID
        #region GetSymptomLogByIdAsync
        public async Task<ResponseSymptomLogDTO> GetSymptomLogByIdAsync(int id, string patientId)
        {
            var symptomLogById = await _symptomLogRepository.GetSymptomLogById(id);

            if (symptomLogById == null)
            {
                _logger.LogInformation("Symptom log with ID {SymptomLogId} not found.", id);
                throw new NotFoundException($"Symptom log with ID {id} not found.");
            }

            // validate patient ID to ensure the symptom log being accessed belongs to the authenticated patient
            if (symptomLogById.PatientId != patientId)
            {
                _logger.LogWarning("User tried to retrieve symptom log ID {SymptomLogId}", id);
                throw new UnauthorizedAccessException("You are not authorized to access this symptom log.");
            }

            // map symptom log entity to response DTO
            var responseDto = new ResponseSymptomLogDTO
            {
                Notes = symptomLogById.Notes,
                Severity = symptomLogById.Severity,
                DurationMinutes = symptomLogById.DurationMinutes,
                StressLevel = symptomLogById.StressLevel,
                SleepHours = symptomLogById.SleepHours,
                isFlare = symptomLogById.isFlare
            };

            return responseDto;
        }
        #endregion 

        // Add new symptom log
        #region AddSymptomLogAsync
        public async Task<ResponseSymptomLogDTO> AddSymptomLogAsync(CreateSymptomLogDTO dto, string patientId)
        {
            // map DTO to entity
            var symptomLog = new SymptomLog
            {
                Notes = dto.Notes,
                Severity = dto.Severity,
                DurationMinutes = dto.DurationMinutes ?? 0,
                StressLevel = dto.StressLevel ?? 0,
                SleepHours = dto.SleepHours ?? 0,
                isFlare = dto.isFlare
            };

            // set the patientId and symptomId to ensure the new symptom log is associated with the correct patient and symptom
            symptomLog.PatientId = patientId;
            symptomLog.SymptomId = dto.SymptomId; 

            if (dto == null)
            {
                _logger.LogInformation("CreateSymptomLogDTO is null.");
                throw new ArgumentNullException(nameof(dto), "Symptom log cannot be null.");
            }
            var CreatedSymptomLog = await _symptomLogRepository.AddSymptomLog(symptomLog, patientId);

            // map the created symptom log entity back to response DTO
            var responseDto = new ResponseSymptomLogDTO
            {
                Notes = CreatedSymptomLog.Notes,
                Severity = CreatedSymptomLog.Severity,
                DurationMinutes = CreatedSymptomLog.DurationMinutes,
                StressLevel = CreatedSymptomLog.StressLevel,
                SleepHours = CreatedSymptomLog.SleepHours,
                isFlare = CreatedSymptomLog.isFlare
            };

            return responseDto;
        }
        #endregion

        // Update symptom
        #region UpdateSymptomAsync
        public async Task<ResponseSymptomLogDTO> UpdateSymptomLogAsync(int id, JsonPatchDocument<UpdateSymptomLogDTO> patchDoc, 
            string patientId)
        {
            if (patchDoc == null)
            {
                _logger.LogInformation("Patch document is null for symptom log update.");
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null.");
            }
            var symptomLogToUpdate = await _symptomLogRepository.GetSymptomLogById(id);

            if (symptomLogToUpdate.PatientId != patientId)
            {
                _logger.LogWarning("Unknown user tried to update symptom log with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            if (symptomLogToUpdate == null)
            {
                _logger.LogInformation("Symptom log with ID {SymptomId} not found for update.", id);
                throw new NotFoundException($"Symptom log with ID {id} not found.");
            }

            // create a dto to apply the patch document to
            #region ApplyPatch
            var updatedDto = new UpdateSymptomLogDTO
            {
                Notes = symptomLogToUpdate.Notes,
                Severity = symptomLogToUpdate.Severity,
                DurationMinutes = symptomLogToUpdate.DurationMinutes,
                StressLevel = symptomLogToUpdate.StressLevel,
                SleepHours = symptomLogToUpdate.SleepHours,
                isFlare = symptomLogToUpdate.isFlare
            };
            #endregion 

            patchDoc.ApplyTo(updatedDto);

            // copy DTO values back into the tracked entity (preserve keys)
            #region copyValues
            symptomLogToUpdate.Notes = updatedDto.Notes;
            symptomLogToUpdate.Severity = updatedDto.Severity;
            symptomLogToUpdate.DurationMinutes = updatedDto.DurationMinutes ?? 0;
            symptomLogToUpdate.StressLevel = updatedDto.StressLevel ?? 0;
            symptomLogToUpdate.SleepHours = updatedDto.SleepHours ?? 0;
            symptomLogToUpdate.isFlare = updatedDto.isFlare;
            #endregion 

            // ensure the patientId is set on the entity to prevent unauthorized updates
            symptomLogToUpdate.PatientId = patientId;

            var updated = await _symptomLogRepository.UpdateSymptomLog(symptomLogToUpdate, patientId);

            // map the updated entity back to response DTO
            #region responseDto
            var responseDto = new ResponseSymptomLogDTO
            {
                Notes = updated.Notes,
                Severity = updated.Severity,
                DurationMinutes = updated.DurationMinutes,
                StressLevel = updated.StressLevel,
                SleepHours = updated.SleepHours,
                isFlare = updated.isFlare
            };
            #endregion 

            return responseDto;
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
            symptomLog.PatientId = patientId; // ensure the patientId is set on the entity to prevent unauthorized deletions

            return await _symptomLogRepository.DeleteSymptomLog(id);
        }
        #endregion 
    }
}
