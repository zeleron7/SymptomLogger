namespace SymptomLogger.src.Services.Interfaces
{
    public interface ISeedDatabaseService
    {
        // Seed sample symptom data for testing and development purposes
        Task SeedDatabaseAsync();
        Task ClearDatabaseAsync();
    }
}
