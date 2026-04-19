using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SymptomLogger.src.Entity
{
    // Represents a symptom (definition of a symptom, not an instance of a symptom occurrence)
    public class Symptom
    {
        [Key]
        public int SymptomId { get; set; } // Primary key
        public string Name { get; set; }
        public string Category { get; set; } // e.g., "Pain", "Fatigue", "Nausea"
        public string BodySystem { get; set; } // e.g., "Musculoskeletal", "Digestive", "Neurological"
        public string? Description { get; set; }
        // add bool isActive for soft delete?

        public User User { get; set; } // Navigation property to User
        public string UserId { get; set; } // Foreign key
    }
}
