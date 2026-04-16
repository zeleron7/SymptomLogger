using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    // Seed entity (autoimmune diseases)
    public class Condition
    {
        [Key]
        public int ConditionId { get; set; } // Primary key
        public string Name { get; set; }
        public string System { get; set; } // e.g., "Musculoskeletal", "Digestive", "Neurological"
        public string IcdCode { get; set; } // ICD code for standardized condition classification
    }
}
