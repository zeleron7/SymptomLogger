using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.DTO.Symptom
{
    public class CreateSymptomDTO
    {
        public string Category { get; set; } = "";
        public string Description { get; set; } = ""; 
        public string Name { get; set; } = "";
    }
}
