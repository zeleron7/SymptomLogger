using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.Models
{
    public class Symptom
    {
        [Key]
        public int Id { get; set; }

        // Adding a Type property to categorize symptoms (e.g., "Physical", "Emotional", "Cognitive")

        [Required]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Severity { get; set; }
    }
}
