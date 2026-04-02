using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SymptomLogger.src.Entity
{
    public class Symptom
    {
        [Key]
        public int SymptomId { get; set; } // Primary key

        public string Category { get; set; } // Type
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        // add bool isActive for soft delete?

        public Patient Patient { get; set; } // Navigation property to Patient

        public string PatientId { get; set; } // Foreign key

    }
}
