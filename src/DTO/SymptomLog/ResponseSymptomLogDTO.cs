using SymptomLogger.src.DTO.SymptomLogEntry;

namespace SymptomLogger.src.DTO.SymptomLog
{
    public class ResponseSymptomLogDTO
    {
        public string Notes { get; set; } = "";
        public DateTime LoggedAt { get; set; } = DateTime.Now;
        public int DurationMinutes { get; set; }
        public int StressLevel { get; set; }
        public int SleepHours { get; set; }
        public bool isFlare { get; set; }

        public ICollection<ResponseSymptomLogEntryDTO> Symptoms { get; set; } = new List<ResponseSymptomLogEntryDTO>();
    }
}
