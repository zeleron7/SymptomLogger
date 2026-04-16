using SymptomLogger.src.DTO.SymptomLogEntry;

namespace SymptomLogger.src.DTO.SymptomLog
{
    public class CreateSymptomLogDTO
    {
        public int SymptomId { get; set; }
        //public string SymptomName { get; set; } = "";
        public string Notes { get; set; } = "";
        //public int Severity { get; set; }

        public ICollection<CreateSymptomLogEntryDTO> Symptoms { get; set; } = new List<CreateSymptomLogEntryDTO>();

        // Analyze potential triggers and patterns
        public int? DurationMinutes { get; set; }
        public int? StressLevel { get; set; }
        public int? SleepHours { get; set; }
        public bool isFlare { get; set; }
    }
}
