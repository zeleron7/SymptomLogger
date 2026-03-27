namespace SymptomLogger.src.Services
{
    public interface ISeedDatabaseService
    {
        Task SeedDatabase();
        Task ClearDatabase();
    }
}
