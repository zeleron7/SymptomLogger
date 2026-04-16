namespace SymptomLogger.src.DTO.SymptomLogEntry
{
    public class ResponseSymptomLogEntryDTO
    {
        public string SymptomName { get; set; }
        public int Severity { get; set; } // Scale of 1-10 for symptom severity
    }
}
