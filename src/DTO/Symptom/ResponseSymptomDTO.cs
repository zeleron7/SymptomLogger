using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.DTO.Symptom
{
    public class ResponseSymptomDTO
    {
        public string Category { get; set; } 
        public string Description { get; set; } 
        public string Name { get; set; } 
        public string BodySystem { get; set; }
    }
}
