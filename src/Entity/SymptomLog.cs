using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    public class SymptomLog
    {
        [Key]
        public int SymptomLogId { get; set; } // Primary key
        public string Notes { get; set; }
        //public int Severity { get; set; }
        public DateTime LoggedAt { get; set; } = DateTime.Now;

        // Analyze potential triggers and patterns
        public int DurationMinutes { get; set; } 
        public int StressLevel { get; set; }
        public int SleepHours { get; set; }
        public bool isFlare { get; set; } // Indicates if this log entry represents a flare-up of symptoms

        public  Patient Patient { get; set; } // Navigation property to Patient
        public string PatientId { get; set; } // Foreign key

        // SymptomLog can have multiple SymptomLogEntries, representing different symptoms logged at the same time
        public ICollection<SymptomLogEntry> SymptomLogEntries { get; set; } 
        //public Symptom Symptom { get; set; } // Navigation property to Symptom
        //public int SymptomId { get; set; } // Foreign key
    }
}
    
