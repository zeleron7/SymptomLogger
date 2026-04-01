namespace SymptomLogger.src.Entity
{
    public class SymptomLog
    {
        public int SymptomLogId { get; set; } // Primary key
        public string Notes { get; set; }
        public int Severity { get; set; }
        public DateTime LoggedAt { get; set; } = DateTime.Now;

        public  Patient Patient{ get; set; } // Navigation property to Patient
        public string PatientId { get; set; } // Foreign key

        public Symptom Symptom { get; set; } // Navigation property to Symptom
        public string SymptomId { get; set; } // Foreign key
    }
}
    
