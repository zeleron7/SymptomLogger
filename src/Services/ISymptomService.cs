using Azure;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using System.Security.Claims;

namespace SymptomLogger.src.Services
{
    public interface ISymptomService
    {
        // Symptoms
        Task<IEnumerable<ResponseSymptomDTO>> GetSymptomsAsync();
        Task<ResponseSymptomDTO> GetSymptomByIdAsync(int id, string patientId);
        Task<ResponseSymptomDTO> AddSymptomAsync(CreateSymptomDTO dto, string patientId);
        Task<ResponseSymptomDTO> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc, string patientId);
        Task<bool> DeleteSymptomAsync(int id, string patientId);
    }
}
