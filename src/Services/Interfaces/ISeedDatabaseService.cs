namespace SymptomLogger.src.Services.Interfaces
{
    public interface ISeedDatabaseService
    {
        Task SeedDatabaseAsync();
        Task ClearDatabaseAsync();
    }
}
