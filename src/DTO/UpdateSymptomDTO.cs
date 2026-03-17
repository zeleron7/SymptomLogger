using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.DTO
{
    public class UpdateSymptomDTO
    {
        public string? Type { get; set; }

        [StringLength(100, ErrorMessage = "The description must be less than 100 characters.")]
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int? Severity { get; set; }
    }
}
