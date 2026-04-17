using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    // Seed entity (autoimmune diseases)
    public class Condition
    {
        [Key]
        public int ConditionId { get; set; } // Primary key
        public string Name { get; set; } // e.g., "Rheumatoid Arthritis", "Crohn's Disease", "Multiple Sclerosis"
        public string System { get; set; } // e.g., "Musculoskeletal", "Digestive", "Neurological"
        public string IcdCode { get; set; } // ICD code for standardized condition classification
    }
}
