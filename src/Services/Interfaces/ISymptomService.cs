using Azure;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using System.Security.Claims;

namespace SymptomLogger.src.Services.Interfaces
{
    public interface ISymptomService
    {
        // Symptoms
        IQueryable<ResponseSymptomDTO> GetSymptoms();
        Task<ResponseSymptomDTO> GetSymptomByIdAsync(int id, string userId);
        Task<ResponseSymptomDTO> AddSymptomAsync(CreateSymptomDTO dto, string userId);
        Task<ResponseSymptomDTO> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc, string userId);
        Task<bool> DeleteSymptomAsync(int id, string userId);
    }
}
