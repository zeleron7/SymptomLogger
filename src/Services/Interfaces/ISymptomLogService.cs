using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;

namespace SymptomLogger.src.Services.Interfaces
{
    public interface ISymptomLogService
    {
        // Symptom logs
        IQueryable<ResponseSymptomLogDTO> GetSymptomLogs();
        Task<ResponseSymptomLogDTO> GetSymptomLogByIdAsync(int id, string patientId);
        Task<ResponseSymptomLogDTO> AddSymptomLogAsync(CreateSymptomLogDTO dto, string patientId);
        Task<ResponseSymptomLogDTO> UpdateSymptomLogAsync(int id, JsonPatchDocument<UpdateSymptomLogDTO> patchDoc, string patientId);
        Task<bool> DeleteSymptomLogAsync(int id, string patientId);
    }
}
