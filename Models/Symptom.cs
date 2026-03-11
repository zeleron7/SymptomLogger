namespace SymptomLogger.Models
{
    public class Symptom
    {
        public int Id { get; set; }

        // Adding a Type property to categorize symptoms (e.g., "Physical", "Emotional", "Cognitive")
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Severity { get; set; }
    }
}
