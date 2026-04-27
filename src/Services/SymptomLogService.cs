using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using NuGet.ProjectModel;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.DTO.SymptomLogEntry;
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
                .Select(c => new ResponseSymptomLogDTO // manual mapping
                {
                    Notes = c.Notes,
                    LoggedAt = c.LoggedAt,
                    DurationMinutes = c.DurationMinutes,
                    StressLevel = c.StressLevel,
                    SleepHours = c.SleepHours,
                    isFlare = c.isFlare,
                    Symptoms = c.SymptomLogEntries.Select(e => new ResponseSymptomLogEntryDTO // map each symptom log entry to response DTO
                    {
                        SymptomName = e.SymptomName,
                        Severity = e.Severity
                    }).ToList()
                });                                      
        }
        #endregion

        // Get symptom log by ID
        #region GetSymptomLogByIdAsync
        public async Task<ResponseSymptomLogDTO> GetSymptomLogByIdAsync(int id, string userId)
        {
            var symptomLogById = await _symptomLogRepository.GetSymptomLogById(id);

            if (symptomLogById == null)
            {
                _logger.LogInformation("Symptom log with ID {SymptomLogId} not found.", id);
                throw new NotFoundException($"Symptom log with ID {id} not found.");
            }

            // validate user ID to ensure the symptom log being accessed belongs to the authenticated user
            if (symptomLogById.UserId != userId)
            {
                _logger.LogWarning("User tried to retrieve symptom log ID {SymptomLogId}", id);
                throw new UnauthorizedAccessException("You are not authorized to access this symptom log.");
            }

            // map symptom log entity to response DTO
            var responseDto = new ResponseSymptomLogDTO
            {
                Notes = symptomLogById.Notes,
                //Severity = symptomLogById.Severity,
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
        public async Task<ResponseSymptomLogDTO> AddSymptomLogAsync(CreateSymptomLogDTO dto, string userId)
        {
            if (dto == null)
            {
                _logger.LogInformation("CreateSymptomLogDTO is null.");
                throw new ArgumentNullException(nameof(dto), "Symptom log cannot be null.");
            }

            // Map DTO to entity
            #region MappingRequest
            var symptomLog = new SymptomLog
            {
                UserId = userId, // set the userId on the symptom log to ensure it is associated with the correct user
                ConditionId = dto.ConditionId, // set the condition ID on the symptom log to establish the relationship
                Notes = dto.Notes,
                DurationMinutes = dto.DurationMinutes ?? 0,
                StressLevel = dto.StressLevel ?? 0,
                SleepHours = dto.SleepHours ?? 0,
                isFlare = dto.isFlare ?? false, // false by default if not provided

                SymptomLogEntries = dto.Symptoms.Select(s => new SymptomLogEntry // map each symptom log entry DTO to entity
                {
                    SymptomId = dto.SymptomId,
                    SymptomName = s.SymptomName,
                    Severity = s.Severity
                }).ToList()
            };
            #endregion 

            var CreatedSymptomLog = await _symptomLogRepository.AddSymptomLog(symptomLog, userId);

            // Map the created symptom log entity back to response DTO
            #region MappingResponse
            var responseDto = new ResponseSymptomLogDTO
            {
                Notes = CreatedSymptomLog.Notes,
                DurationMinutes = CreatedSymptomLog.DurationMinutes,
                StressLevel = CreatedSymptomLog.StressLevel,
                SleepHours = CreatedSymptomLog.SleepHours,
                isFlare = CreatedSymptomLog.isFlare,

                Symptoms = CreatedSymptomLog.SymptomLogEntries.Select(e => new ResponseSymptomLogEntryDTO // map each symptom log entry entity to response DTO
                {
                    SymptomName = e.SymptomName,
                    Severity = e.Severity
                }).ToList()

            };
            #endregion 

            return responseDto;
        }
        #endregion

        // Update symptom
        #region UpdateSymptomAsync
        public async Task<ResponseSymptomLogDTO> UpdateSymptomLogAsync(int id, JsonPatchDocument<UpdateSymptomLogDTO> patchDoc, 
            string userId)
        {
            if (patchDoc == null)
            {
                _logger.LogInformation("Patch document is null for symptom log update.");
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null.");
            }
            var symptomLogToUpdate = await _symptomLogRepository.GetSymptomLogById(id);

            if (symptomLogToUpdate.UserId != userId)
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
                //Severity = symptomLogToUpdate.Severity,
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
            symptomLogToUpdate.DurationMinutes = updatedDto.DurationMinutes ?? 0;
            symptomLogToUpdate.StressLevel = updatedDto.StressLevel ?? 0;
            symptomLogToUpdate.SleepHours = updatedDto.SleepHours ?? 0;
            symptomLogToUpdate.isFlare = updatedDto.isFlare;
            #endregion 

            // ensure the userId is set on the entity to prevent unauthorized updates
            symptomLogToUpdate.UserId = userId;

            var updated = await _symptomLogRepository.UpdateSymptomLog(symptomLogToUpdate, userId);

            // map the updated entity back to response DTO
            #region responseDto
            var responseDto = new ResponseSymptomLogDTO
            {
                Notes = updated.Notes,
                //Severity = updated.Severity,
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
        public async Task<bool> DeleteSymptomLogAsync(int id, string userId)
        {
            var symptomLog = await _symptomLogRepository.GetSymptomLogById(id);
            if (symptomLog.UserId != userId)
            {
                _logger.LogWarning("Unknown user tried to delete symptom log with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            // null check to prevent null reference exception
            if (symptomLog == null)
            {
                return false;
            }
            symptomLog.UserId = userId; // ensure the userId is set on the entity to prevent unauthorized deletions

            return await _symptomLogRepository.DeleteSymptomLog(id);
        }
        #endregion 
    }
}
