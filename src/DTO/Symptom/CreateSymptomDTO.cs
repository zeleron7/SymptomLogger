using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.DTO.Symptom
{
    public class CreateSymptomDTO
    {
        public string? Type { get; set; }

        [Required(ErrorMessage = "The Description is required.")]
        [StringLength(100, ErrorMessage = "The description must be less than 100 characters.")]
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int? Severity { get; set; }
    }
}
