using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;

namespace SymptomLogger.src.Services.Interfaces
{
    public interface ISymptomLogService
    {
        // Symptom logs
        IQueryable<ResponseSymptomLogDTO> GetSymptomLogs();
        Task<ResponseSymptomLogDTO> GetSymptomLogByIdAsync(int id, string userId);
        Task<ResponseSymptomLogDTO> AddSymptomLogAsync(CreateSymptomLogDTO dto, string userId);
        Task<ResponseSymptomLogDTO> UpdateSymptomLogAsync(int id, JsonPatchDocument<UpdateSymptomLogDTO> patchDoc, string userId);
        Task<bool> DeleteSymptomLogAsync(int id, string userId);
    }
}
