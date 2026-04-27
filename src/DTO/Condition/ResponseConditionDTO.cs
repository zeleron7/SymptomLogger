namespace SymptomLogger.src.DTO.Condition
{
    public class ResponseConditionDTO
    {
        public int ConditionId { get; set; }
        public string Name { get; set; }
        public string System { get; set; } // e.g., "Musculoskeletal", "Digestive", "Neurological"
        public string IcdCode { get; set; } // Optional ICD code for standardized condition classification
    }
}
