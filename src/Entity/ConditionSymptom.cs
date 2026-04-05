using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    // Join entity, Condition > ConditionSymptom < Symptom
    public class ConditionSymptom
    {
        [Key]
        public int ConditionSymptomId { get; set; } // Primary key
        public bool IsHallmark { get; set; } // True if this symptom is a defining/diagnostic symptom of the condition

        public Condition Condition { get; set; } // Navigation property to Condition
        public int ConditionId { get; set; } // Foreign key to Condition

        public Symptom Symptom { get; set; } // Navigation property to Symptom
        public int SymptomId { get; set; } // Foreign key to Symptom
    }
}
