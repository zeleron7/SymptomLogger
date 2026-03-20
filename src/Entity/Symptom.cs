using SymptomLogger.src.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SymptomLogger.src.Models
{
    public class Symptom
    {
        [Key]
        public int SymptomId { get; set; }

        // Type of symptom (e.g., headache, fatigue, etc.)
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Severity { get; set; }

        public Patient Patient { get; set; }

        // Foreign key to Patient
        [ForeignKey("Patient")]
        public string PatientId { get; set; }

    }
}
