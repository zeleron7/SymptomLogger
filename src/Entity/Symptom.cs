using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SymptomLogger.src.Entity
{
    public class Symptom
    {
        [Key]
        public int SymptomId { get; set; } // Primary key

        // Type of symptom (e.g., headache, fatigue, etc.)
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Severity { get; set; }

        public Patient Patient { get; set; } // Navigation property to Patient

        [ForeignKey("Patient")]
        public string PatientId { get; set; } // Foreign key

    }
}
