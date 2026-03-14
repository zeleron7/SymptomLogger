using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.Models
{
    public class Symptom
    {
        [Key]
        public int Id { get; set; }

        // Type of symptom (e.g., headache, fatigue, etc.)
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Severity { get; set; }
    }
}
