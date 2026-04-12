using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Condition;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Services.Interfaces
{
    public interface IConditionService
    {
        IQueryable<ResponseConditionDTO> GetConditions();
        Task<ResponseConditionDTO> GetConditionByIdAsync(int id);
        Task<ResponseConditionDTO> AddConditionAsync(CreateConditionDTO dto);
        Task<ResponseConditionDTO> UpdateConditionAsync(int id, JsonPatchDocument<UpdateConditionDTO> patchDoc);
        Task<bool> DeleteConditionAsync(int id);
    }
}
