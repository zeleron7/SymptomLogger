using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    // Represents an individual symptom entry within a symptom log
    public class SymptomLogEntry
    {
        [Key]
        public int SymptomLogEntryId { get; set; } // Primary key
        public int Severity { get; set; } // Scale of 1-10 for symptom severity
        public string SymptomName { get; set; } 

        public SymptomLog SymptomLog { get; set; } // Navigation property to SymptomLog
        public int SymptomLogId { get; set; } // Foreign key

        public Symptom Symptom { get; set; } // Navigation property to Symptom
        public int SymptomId { get; set; } // Foreign key
    }
}
