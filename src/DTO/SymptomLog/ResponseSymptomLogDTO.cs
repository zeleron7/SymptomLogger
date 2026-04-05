namespace SymptomLogger.src.DTO.SymptomLog
{
    public class ResponseSymptomLogDTO
    {
        public string Notes { get; set; } = "";
        public int Severity { get; set; }
        public int DurationMinutes { get; set; }
        public int StressLevel { get; set; }
        public int SleepHours { get; set; }
        public bool isFlare { get; set; }
    }
}
