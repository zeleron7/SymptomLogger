using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.DTO.Symptom
{
    public class UpdateSymptomDTO
    {
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
