using Azure;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;
using System.Security.Claims;

namespace SymptomLogger.src.Services
{
    public interface ISymptomService
    {
        // Symptoms
        Task<IEnumerable<ResponseSymptomDTO>> GetSymptomsAsync();
        Task<ResponseSymptomDTO> GetSymptomByIdAsync(int id);
        Task<ResponseSymptomDTO> AddSymptomAsync(CreateSymptomDTO dto, string patientId);
        Task<ResponseSymptomDTO> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc, string patientId);
        Task<bool> DeleteSymptomAsync(int id, string patientId);

    }
}
