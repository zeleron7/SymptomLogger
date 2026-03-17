using Azure;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;

namespace SymptomLogger.src.Services
{
    public interface ISymptomService
    {
        // Symptoms
        Task<IEnumerable<Symptom>> GetSymptomsAsync();
        Task<Symptom> GetSymptomByIdAsync(int id);
        Task<Symptom> AddSymptomAsync(CreateSymptomDTO dto);
        Task<Symptom> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc);
        Task<bool> DeleteSymptomAsync(int id);

    }
}
