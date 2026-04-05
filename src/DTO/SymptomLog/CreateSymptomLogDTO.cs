namespace SymptomLogger.src.DTO.SymptomLog
{
    public class CreateSymptomLogDTO
    {
        public int SymptomId { get; set; }
        public string Notes { get; set; } = "";
        public int Severity { get; set; }

        // Analyze potential triggers and patterns
        public int? DurationMinutes { get; set; }
        public int? StressLevel { get; set; }
        public int? SleepHours { get; set; }
        public bool isFlare { get; set; }
    }
}
